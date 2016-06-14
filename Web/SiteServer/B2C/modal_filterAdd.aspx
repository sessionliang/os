<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.Modal.FilterAdd" Trace="false"%>

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
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>

  <table class="table table-noborder table-hover">
    <tr>
      <td width="120">筛选属性：</td>
      <td><asp:DropDownList ID="ddlAttributeName" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
      <td>自定义显示名称：</td>
      <td><asp:TextBox ID="tbFilterName"  Width="180" runat="server"></asp:TextBox></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->