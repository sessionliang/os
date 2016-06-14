<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.CMS.BackgroundPages.Modal.CommentAdd" Trace="false"%>

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
      <asp:TextBox ID="tbContent" runat="server" TextMode="MultiLine" Width="98%" Height="380"></asp:TextBox>
      <asp:RequiredFieldValidator ControlToValidate="tbContent" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->