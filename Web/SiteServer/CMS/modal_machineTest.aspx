<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.MachineTest" Trace="false"%>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server">
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>

  <table class="table table-bordered table-striped">
    <tr>
      <td width="120"> 服务器名称：</td>
      <td><asp:Literal id="MachineName" runat="server" /></td>
    </tr>
    <tr>
      <td> 服务器类型：</td>
      <td><asp:Literal id="ServiceType" runat="server" /></td>
    </tr>
    <tr>
      <td> 连接方式：</td>
      <td><asp:Literal id="ConnectionType" runat="server" /></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->