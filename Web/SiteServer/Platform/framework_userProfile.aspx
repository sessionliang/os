<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.FrameworkUserProfile" %>

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

  <ul class="nav nav-pills">
    <li><a href="framework_userTheme.aspx"><lan>主题设置</lan></a></li>
    <li><a href="framework_userLanguage.aspx"><lan>语言设置</lan></a></li>
    <li class="active"><a href="framework_userProfile.aspx"><lan>修改资料</lan></a></li>
    <li><a href="framework_userPassword.aspx"><lan>更改密码</lan></a></li>
  </ul>

  <div class="popover popover-static">
  <h3 class="popover-title"><lan>修改资料</lan></h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td width="120">账号：</td>
        <td><asp:Literal ID="UserName" runat="server"></asp:Literal></td>
      </tr>
      <tr>
        <td>姓名：</td>
        <td><asp:TextBox ID="DisplayName" runat="server" Width="180px"></asp:TextBox>
          <asp:RequiredFieldValidator runat="server" ControlToValidate="DisplayName"
            ErrorMessage="姓名为必填项。" ToolTip="姓名为必填项。"  Display="Dynamic"></asp:RequiredFieldValidator></td>
      </tr>
      <tr>
        <td>电子邮件：</td>
        <td><asp:TextBox ID="Email" runat="server" Width="180px"></asp:TextBox>
          <asp:RegularExpressionValidator ControlToValidate="Email" 
            ValidationExpression="(\w[0-9a-zA-Z_-]*@(\w[0-9a-zA-Z-]*\.)+\w{2,})" 
            ErrorMessage="邮件格式不正确。" Display="Dynamic" runat="server"/></td>
      </tr>
      <tr>
        <td>手机号码：</td>
        <td>
          <asp:TextBox ID="Mobile" runat="server" Width="180px"></asp:TextBox>
          <asp:RegularExpressionValidator ControlToValidate="Mobile" ValidationExpression="^0?\d{11}$" ErrorMessage="手机号码格式不正确" Display="Dynamic" runat="server" />
        </td>
      </tr>
      <tr>
        <td>找回密码问题：</td>
        <td><asp:TextBox ID="Question" runat="server" Width="180px"></asp:TextBox>
          </td>
      </tr>
      <tr>
        <td>找回密码答案：</td>
        <td>
          <asp:TextBox ID="Answer" runat="server" Width="180px"></asp:TextBox>
        </td>
      </tr>
    </table>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <asp:Button class="btn btn-primary" OnClick="Submit_Click" runat="server" Text="修 改"  />
        </td>
      </tr>
    </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->