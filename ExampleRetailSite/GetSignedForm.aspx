<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ExampleRetailSite._Default" %>
<%@ Register assembly="SecureFormSigner" namespace="ezimerchant.Server" tagprefix="ezi" %>
<%
Response.ContentType = "text/plain";

var Signer = new SecureFormSigner();
Signer.SigningKey = "3I67kHTw1V2kGSvNN1hKhw==";
Signer.FormFields.Add(new ListItem("PRODUCTCODE", "DIMAOND"));
Signer.FormFields.Add(new ListItem("PRODUCTNAME", "Diamond Ring"));
Signer.FormFields.Add(new ListItem("PRODUCTPRICE", "100000.34"));

Response.Write(Signer.FormData);    
%>