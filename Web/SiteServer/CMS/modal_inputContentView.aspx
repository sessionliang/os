<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.InputContentView" Trace="false"%>

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
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td width="118" height="25"><asp:Literal id="ltlDataKey" runat="server" />：</td>
          <td><asp:Literal id="ltlDataValue" runat="server" /></td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
    <tr>
      <td>添加者：</td>
      <td><asp:Literal id="ltlAddUserName" runat="server" /></td>
    </tr>
    <tr>
      <td>添加者IP：</td>
      <td><asp:Literal id="ltlIPAddress" runat="server" /></td>
    </tr>
    <tr>
      <td>添加者位置：</td>
      <td><asp:Literal id="ltlLocation" runat="server" /></td>
    </tr>
    <tr>
      <td>添加时间：</td>
      <td><asp:Literal id="ltlAddDate" runat="server" /></td>
    </tr>
    <tr>
      <td>回复内容：</td>
      <td><asp:Literal id="ltlReply" runat="server" /></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->