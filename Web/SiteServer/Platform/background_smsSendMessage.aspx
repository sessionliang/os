<%@ Page Language="C#" AutoEventWireup="true" Inherits="BaiRong.BackgroundPages.BackgroundSMSSendMessage" %>

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
  <h3 class="popover-title">发送测试短信</h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td width="140">接收手机号码：</td>
        <td>
            <asp:TextBox width="400" Rows="4" TextMode="MultiLine" ID="tbMobile" runat="server" />
            <asp:RequiredFieldValidator ControlToValidate="tbMobile" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
            <br />
            <span>（发送多个手机号码请用逗号“,”分开，如：13700000000,13900000000）</span>
        </td>
      </tr>
      <tr>
        <td>短信内容：</td>
        <td>
          <asp:TextBox width="400" Rows="4" TextMode="MultiLine" ID="tbMessage" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="tbMessage" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        </td>
      </tr>
    </table>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <asp:Button class="btn btn-primary" ID="Submit" Text="发 送" OnClick="Submit_OnClick" runat="server" />
        </td>
      </tr>
    </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->