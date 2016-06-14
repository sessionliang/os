<%@ Page Language="C#" Inherits="SiteServer.WeiXin.BackgroundPages.ConsoleAccountSync" enableViewState = "false" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server">
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <asp:Button id="btnSync" class="btn btn-success" text="同步公众账号" runat="server" OnClick="btnSync_Click" />

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->