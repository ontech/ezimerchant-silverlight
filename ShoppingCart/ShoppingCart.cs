using System;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Threading;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Json;


namespace ezimerchant.Client
{
    public class ShoppingCart
    {
        private JsonObject m_CartData;
        private string m_SignedFormData;
        private AsyncCallback m_Callback;
        private object m_CallbackState;
        private IAsyncResult m_AsyncResult = null;
        private Dispatcher m_Dispatcher;

        public string SecureDomain;
        public string FormSignWebService;

        public IAsyncResult BeginAddToCart(Dictionary<string, string> QueryParameters, AsyncCallback callback, object state)
        {
            if (m_AsyncResult != null)
                throw new InvalidOperationException();

            m_Dispatcher = Application.Current.RootVisual.Dispatcher;
            m_Callback = callback;
            m_CallbackState = state;

            var query = ToQueryString(QueryParameters);
            if(query != "")
                query = "?" + query;

            var uri = new Uri(HtmlPage.Document.DocumentUri, FormSignWebService + query);
            var request = (HttpWebRequest)WebRequest.Create(uri);

            m_AsyncResult = request.BeginGetResponse(FormReady, request);

            return m_AsyncResult;
        }

        public void EndAddToCart(IAsyncResult asyncResult)
        {
            if (m_AsyncResult != null)
                m_AsyncResult.AsyncWaitHandle.WaitOne();
        }

        public IAsyncResult BeginGetCart(AsyncCallback callback, object state)
        {
            if (m_AsyncResult != null)
                throw new InvalidOperationException();

            m_Dispatcher = Application.Current.RootVisual.Dispatcher; 
            m_Callback = callback;
            m_CallbackState = state;

            var request = (HttpWebRequest)WebRequest.Create("https://" + SecureDomain + "/cart/view?json");
            request.Method = "GET";

            m_AsyncResult = request.BeginGetResponse(CartReady, request);

            return m_AsyncResult;
        }

        public JsonObject EndGetCart(IAsyncResult asyncResult)
        {
            if(m_AsyncResult != null)
                m_AsyncResult.AsyncWaitHandle.WaitOne();

            return m_CartData;
        }

        public IAsyncResult BeginEditCartLine(AsyncCallback callback, object state, int CartLineID, int Quantity)
        {
            if (m_AsyncResult != null)
                throw new InvalidOperationException();

            m_Dispatcher = Application.Current.RootVisual.Dispatcher;
            m_Callback = callback;
            m_CallbackState = state;

            m_SignedFormData = "ACTION=UpdateCart&CARTLINE=" + CartLineID.ToString() + "&QUANTITY=" + Quantity.ToString();

            var request = (HttpWebRequest)WebRequest.Create("https://" + SecureDomain + "/cart/?nocontent");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            m_AsyncResult = request.BeginGetRequestStream(RequestReady, request); 

            return m_AsyncResult;
        }

        public void EndEditCartLine(IAsyncResult asyncResult0
        {
            if(m_AsyncResult != null)
                m_AsyncResult.AsyncWaitHandle.WaitOne();
        }

        private void FormReady(IAsyncResult asyncResult)
        {
            HttpWebRequest request = asyncResult.AsyncState as HttpWebRequest;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult);
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            m_SignedFormData = reader.ReadToEnd();

            m_Dispatcher.BeginInvoke(delegate()
            {
                HttpWebRequest postrequest = (HttpWebRequest)WebRequest.Create(new Uri("https://" + SecureDomain + "/cart/?nocontent"));
                postrequest.Method = "POST";
                postrequest.ContentType = "application/x-www-form-urlencoded";
                postrequest.BeginGetRequestStream(RequestReady, postrequest);
            });
        }

        private void RequestReady(IAsyncResult asyncResult)
        {
            HttpWebRequest request = asyncResult.AsyncState as HttpWebRequest;
            Stream stream = request.EndGetRequestStream(asyncResult);

            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(m_SignedFormData);
            writer.Flush();
            writer.Close();


            m_Dispatcher.BeginInvoke(delegate()
            {
                request.BeginGetResponse(ResponseReady, request);
            });
        }

        private void ResponseReady(IAsyncResult asyncResult)
        {
            var currentAsyncResult = m_AsyncResult;

            var request = asyncResult.AsyncState as HttpWebRequest;

            request.EndGetResponse(asyncResult);

            m_Dispatcher.BeginInvoke(delegate()
            {
                m_AsyncResult = null;

                if (m_Callback != null)
                    m_Callback.Invoke(currentAsyncResult);
            });
        }

        private void CartReady(IAsyncResult asyncResult)
        {
            var currentAsyncResult = m_AsyncResult;

            var request = asyncResult.AsyncState as WebRequest;

            var response = request.EndGetResponse(asyncResult);
            var responseStream = response.GetResponseStream();

            m_CartData = (JsonObject)JsonObject.Load(responseStream);

            m_Dispatcher.BeginInvoke(delegate()
            {
                m_AsyncResult = null;

                if (m_Callback != null)
                    m_Callback.Invoke(currentAsyncResult);
            });
        }

        private static string ToQueryString(Dictionary<string, string> qs)
        {
            if (qs == null)
                return "";

            var result = "";
            foreach (var entry in qs)
            {
                result += HttpUtility.UrlEncode(entry.Key) + "=" + HttpUtility.UrlEncode(entry.Value) + "&";
            }

            if (result == "")
                result += "&";

            return result.Substring(0, result.Length - 1);
        }
    }
}
