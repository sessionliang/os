<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.MailSubscribeAdd" Trace="false"%>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body onLoad="document.getElementById('<%=Receiver.ClientID%>').focus();">
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server">
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>

  <table class="table table-noborder table-hover">
    <tr>
      <td width="100"><bairong:help HelpText="订阅人的姓名" Text="订阅人：" runat="server" ></bairong:help></td>
      <td><asp:TextBox Columns="40" id="Receiver" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="Receiver" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="Receiver"
						ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td width="100"><bairong:help HelpText="订阅邮箱" Text="订阅邮箱：" runat="server" ></bairong:help></td>
      <td><asp:TextBox Columns="40" id="Mail" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="Mail" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="Mail"
						ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->