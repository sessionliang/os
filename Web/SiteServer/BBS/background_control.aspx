<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundControl" %>

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
  <h3 class="popover-title">防灌水设置</h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td width="170">两次发表时间间隔(秒)：</td>
        <td>
          <asp:TextBox  Columns="10" id="tbPostInterval" runat="server" />
          <asp:RegularExpressionValidator runat="server" ControlToValidate="tbPostInterval"
                ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
          <br />
          <span>(两次发帖间隔小于此时间，0 为不限制) </span></td>
      </tr>
      <tr>
        <td>发表主题启用验证码：</td>
        <td><asp:RadioButtonList ID="rblIsVerifyCodeThread" AutoPostBack="true" OnSelectedIndexChanged="rblIsVerifyCode_SelectedIndexChanged" RepeatDirection="Horizontal" runat="server">
            <asp:ListItem Text="启用" Value="True" Selected="true"></asp:ListItem>
            <asp:ListItem Text="不启用" Value="False"></asp:ListItem>
          </asp:RadioButtonList></td>
      </tr>
      <tr>
        <td>回复主题启用验证码：</td>
        <td><asp:RadioButtonList ID="rblIsVerifyCodePost" AutoPostBack="true" OnSelectedIndexChanged="rblIsVerifyCode_SelectedIndexChanged" RepeatDirection="Horizontal" runat="server">
            <asp:ListItem Text="启用" Value="True" Selected="true"></asp:ListItem>
            <asp:ListItem Text="不启用" Value="False"></asp:ListItem>
          </asp:RadioButtonList></td>
      </tr>
      <asp:PlaceHolder ID="phPostVerifyCode" runat="server">
        <tr>
          <td>验证码发帖限制：</td>
          <td><asp:TextBox  Columns="10" id="tbPostVerifyCodeCount" runat="server" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbPostVerifyCodeCount"
                ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
            <br />
            <span>(发帖数超过此设置的会员将不受验证码功能限制，0 表示所有会员均受限制)</span> </td>
        </tr>
      </asp:PlaceHolder>
    </table>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <asp:Button class="btn btn-primary" id="Submit" text="修 改" onclick="Submit_OnClick" runat="server" />
        </td>
      </tr>
    </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->