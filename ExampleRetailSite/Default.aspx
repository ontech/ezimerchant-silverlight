<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ExampleRetailSite._Default" %>
<%@ Register assembly="SecureFormSigner" namespace="ezimerchant" tagprefix="ezi" %>
<%
Response.ContentType = "text/plain";

var Signer = new SecureFormSigner();
Signer.FormFields.Add(new ListItem("PRODUCTCODE", "b"));

Response.Write(Signer.FormData);    
%>