<%@ Page Language="C#" Inherits="SiteServer.GeXia.BackgroundPages.Modal.OEMPassword" %>

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
      <td width="130">用户名：</td>
      <td><asp:Literal id="ltlUserName" runat="server"/></td>
    </tr>
    <tr>
      <td>新密码：</td>
      <td>
        <asp:TextBox TextMode="Password" id="tbPassword" MaxLength="50" Size="20" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="tbPassword"
          ErrorMessage=" *" foreColor="red"
          Display="Dynamic"
          runat="server"
          />
      </td>
    </tr>
    <tr>
      <td>再次输入新密码：</td>
      <td>
        <asp:TextBox TextMode="Password" id="tbConfirmPassword" MaxLength="50" Size="20" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="tbConfirmPassword"
          ErrorMessage=" *" foreColor="red"
          Display="Dynamic"
          runat="server"
          />
        <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="tbPassword" ControlToValidate="tbConfirmPassword" Display="Dynamic" ErrorMessage=" 两次输入的密码不一致！请再输入一遍您上面填写的密码。" foreColor="red"></asp:CompareValidator>
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->