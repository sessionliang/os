<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.Modal.StorageTest" Trace="false"%>

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

  <table class="table table-bordered table-striped">
    <tr>
      <td width="120"> 空间名称：</td>
      <td><asp:Literal id="ltlStorageName" runat="server" /></td>
    </tr>
    <tr>
      <td> 空间域名：</td>
      <td><asp:Literal id="ltlStorageURL" runat="server" /></td>
    </tr>
    <tr>
      <td> 空间类型：</td>
      <td><asp:Literal id="ltlStorageType" runat="server" /></td>
    </tr>
  </table>
  <br>
  <asp:Literal id="ltlTest" runat="server" />

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->