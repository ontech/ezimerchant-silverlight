using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

namespace ezimerchant.Server
{
    [
        ToolboxData("<{0}:SecureFormSigner runat=server></{0}:SecureFormSigner>"),
        ControlBuilderAttribute(typeof(ListItemControlBuilder)),
        ParseChildren(true, "FormFields")
    ]
    public class SecureFormSigner : WebControl
    {
        private ListItemCollection m_FormFields;
        private string m_SigningKey = "";

        public SecureFormSigner() : base()
        {
            m_FormFields = new ListItemCollection();
        }

        public string Signature
        {
            get
            {
                var keys = new ArrayList();

                foreach (ListItem l in m_FormFields)
                {
                    keys.Add(l.Text);
                }

                keys.Sort();

                string formdata = "";
                foreach (string k in keys)
                {
					var Key = k.ToUpperInvariant();
					
					if(Key == "PRODUCTCODE" ||
						Key == "PRODUCTNAME" ||
						Key == "PRODUCTPRICE" ||
						Key.StartsWith("PRODUCTPRICE(") ||
						Key == "PRODUCTLISTPRICE" ||
						Key == "PRODUCTWEIGHT" ||
						Key == "PRODUCTWIDTH" ||
						Key == "PRODUCTHEIGHT" ||
						Key == "PRODUCTLENGTH" ||
						Key == "PRODUCTTAX" ||
						Key == "PRODUCTIMAGEURL" ||
						Key.StartsWith("OPTIONTYPE(") ||
						Key.StartsWith("OPTIONNAME(") ||
						Key.StartsWith("OPTIONVALUES("))
					{
						formdata += UpperCaseUrlEncode(Key) + "=" + UpperCaseUrlEncode((string)m_FormFields.FindByText(k).Value) + "&";
					}
                }

                if(formdata.Length > 0)
                    formdata = formdata.Substring(0, formdata.Length - 1);

                var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(m_SigningKey));

                byte[] formdatabytes = Encoding.UTF8.GetBytes(formdata);

                return Convert.ToBase64String(hmac.ComputeHash(formdatabytes));
            }
        }

        public string FormData
        {
            get
            {
                var keys = new ArrayList();

                foreach (ListItem l in m_FormFields)
                {
                    keys.Add(l.Text);
                }

                keys.Sort();

                string formdata = "";
                foreach (string k in keys)
                {
                    formdata += UpperCaseUrlEncode(k.ToUpperInvariant()) + "=" + UpperCaseUrlEncode((string)m_FormFields.FindByText(k).Value) + "&";
                }

                if (formdata.Length > 0)
                    formdata = formdata.Substring(0, formdata.Length - 1);

                return formdata + "&SIGNATURE=" + HttpUtility.UrlEncode(Signature);
            }
        }



        [Category("Parameters")]
        public string SigningKey
        {
            get
            {
                return m_SigningKey;
            }

            set
            {
                m_SigningKey = value;
            }
        }

        [
            Category("Form Fields"),
            PersistenceMode(PersistenceMode.InnerDefaultProperty)
        ]
        public virtual ListItemCollection FormFields
        {
            get
            {
                if (this.m_FormFields == null)
                    m_FormFields = new ListItemCollection();

                return m_FormFields;
            }
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
        }

        private static string UpperCaseUrlEncode(string s)
        {
            char[] temp = HttpUtility.UrlEncode(s, Encoding.UTF8).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }
            return new string(temp).Replace("(", "%28").Replace(")", "%29");
        }
    }
}
