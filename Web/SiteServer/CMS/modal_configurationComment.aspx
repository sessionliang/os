<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.ConfigurationComment" Trace="false"%>

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

  <table class="table table-noborder table-hover">
    <tr>
      <td width="160"><bairong:help HelpText="设置栏目是否可评论" Text="栏目是否可评论：" runat="server" ></bairong:help></td>
      <td><asp:RadioButtonList ID="IsChannelCommentable" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
    </tr>
    <tr>
      <td width="160"><bairong:help HelpText="设置栏目下内容是否可评论" Text="内容是否可评论：" runat="server" ></bairong:help></td>
      <td><asp:RadioButtonList ID="IsContentCommentable" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->