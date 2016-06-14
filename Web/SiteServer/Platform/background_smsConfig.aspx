<%@ Page Language="C#" AutoEventWireup="true" Inherits="BaiRong.BackgroundPages.BackgroundSMSConfig" %>

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

  <div class="popover popover-static">
  <h3 class="popover-title">手机短信设置</h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td width="200">短信通账号：</td>
        <td>
            <asp:TextBox Columns="35" MaxLength="200" ID="tbSMSAccount" runat="server" />
            <asp:RequiredFieldValidator ControlToValidate="tbSMSAccount" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbSMSAccount" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
            <a href="http://www.siteserver.cn/sms" target="_blank">注册短信通</a>
        </td>
      </tr>
      <tr>
        <td width="200">密码：</td>
        <td>
            <asp:TextBox Columns="35" MaxLength="50" ID="tbSMSPassword" runat="server" />
            <asp:RequiredFieldValidator ControlToValidate="tbSMSPassword" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbSMSPassword" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
        </td>
      </tr>
    </table>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <asp:Button class="btn btn-primary" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
        </td>
      </tr>
    </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->