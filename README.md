
ezimerchant integration with Microsoft Silverlight

Allows you to run a shopping cart from within the silverlight environment.
Integrates with PayPal and securely handles credit card processing

Requirements
============
A silverlight control to host the shopping cart
An ezimerchant account

copy and edit the clientaccesspolicy.xml and crossdomain.xml files and place them in the template directory inside the corresponding ezimerchant account

Having Issues?
==============

You can grab us at support@ezimerchant.com please also see [http://www.ezimerchant.com/](http://www.ezimerchant.com/) sign up for a free [Shopping Cart](http://www.ezimerchant.com/)

Example Explained
=================

ExampleRetailSite - is a demonstration ASP.NET application that makes use of the SecureFormSigner and ExampleSilverlightClient
ExampleSilverlightClient - makes use of the ShoppingCart silverlight class library to implement a shopping cart experience in silverlight
SecureFormSigner - an ASP.NET server control that implements form signing to avoid end users tampering with product information during transit through a browser to the secure cart/checkout
ShoppingCart - a silverlight class library that implements an integration with the ezimerchant secure cart and checkout environment

Walkthrough
===========

After setting up and running the ExampleRetailSite when you request ExampleSilverlightClientTestPage.aspx the browser will load the ExampleSilverlightClient silverlight component.

When you click Add To Cart the ExampleSilverlightClient will interact with the silverlight ShoppingCart class library. The class library makes a request to /GetSignedForm.aspx with a 
query string that represents the product to be added to the cart. The /GetSignedForm.aspx file interacts with the SecureFormSigner to generate a signed form string. This signed form string is
sent back to the silverlight client which then forwards it to the secure shopping domain.

You will notice the GetSignedForm.aspx file only uses PRODUCTCODE, PRODUCTNAME and PRODUCTPRICE. The available form fields that the cart can use are:

* PRODUCTCODE 
* PRODUCTNAME
* PRODUCTPRICE
* PRODUCTPRICE(CUR) - per currency pricing - CUR is the three letter ISO currency code http://en.wikipedia.org/wiki/ISO_4217
* PRODUCTLISTPRICE
* PRODUCTWEIGHT
* PRODUCTWIDTH
* PRODUCTHEIGHT
* PRODUCTLENGTH
* PRODUCTTAX
* PRODUCTIMAGEURL
* OPTIONNAME(n) - the name of option n
* OPTIONTYPE(n) - the type of option n (select, textarea, text, checkbox, file)
* OPTIONVALUE(n) - the current value for option n
* OPTIONVALUES(n)(x) - this represents the "x'th" possible drop down values for a select option numbered n
* QUANTITY

TODO:
=====

Customer login and order history available through the ShoppingCart class library so this functionality can be consumed by silverlight clients



