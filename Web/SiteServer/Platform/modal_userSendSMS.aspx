﻿<%@ Page Language="C#" validateRequest="false" Inherits="BaiRong.BackgroundPages.Modal.UserSendSMS" Trace="false"%>

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
      <td width="120">接收人号码：</td>
      <td><asp:TextBox width="300" Rows="3" TextMode="MultiLine" id="MailUserNames" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="MailUserNames" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <span class="gray">多个手机号以“,”分割</span> </td>
    </tr>
    <tr>
      <td colspan="3"><bairong:BREditor id="Body" width="500" height="250" runat="server"></bairong:BREditor></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->