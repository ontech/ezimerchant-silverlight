using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Json;
using ezimerchant.Client;

namespace ExampleSilverlightClient
{
    public partial class MainPage : UserControl
    {
        private ShoppingCart m_Cart;

        public MainPage()
        {
            InitializeComponent();

            m_Cart = new ShoppingCart();
            m_Cart.SecureDomain = "kfnzkm.ezimerchant.com";
            m_Cart.FormSignWebService = "/GetSignedForm.aspx";
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            m_Cart.BeginAddToCart(null, new AsyncCallback(CartPosted), null);

            MyButton.IsEnabled = false;
        }

        private void MyButton2_Click(object sender, RoutedEventArgs e)
        {
            m_Cart.BeginGetCart(new AsyncCallback(CartView), null);
        }

        private void CartPosted(IAsyncResult asyncResult)
        {
            MyButton.IsEnabled = true;

            //HtmlPage.Window.Navigate(new Uri("https://" + m_Cart.SecureDomain + "/cart/"));

            //m_Cart.BeginGetCart(new AsyncCallback(CartView), null);
        }

        private void CartView(IAsyncResult asyncResult)
        {
            HtmlPage.Window.Alert(m_Cart.EndGetCart(asyncResult).ToString());
        }
    }
}
