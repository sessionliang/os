<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.TagStyleResumeAdd" Trace="false"%>

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
      <td width="160"><bairong:help HelpText="样式名称" Text="样式名称：" runat="server" ></bairong:help></td>
      <td><asp:TextBox Columns="25" MaxLength="50" id="StyleName" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="StyleName" errorMessage=" *" foreColor="red" display="Dynamic"
						runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="StyleName"
						ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td><bairong:help HelpText="设置提交内容后是否需要发送邮件给管理员" Text="是否发送邮件提醒：" runat="server" ></bairong:help></td>
      <td><asp:RadioButtonList ID="IsMail" AutoPostBack="true" OnSelectedIndexChanged="IsMail_SelectedIndexChanged" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="发送邮件" Value="True"></asp:ListItem>
          <asp:ListItem Text="不发送邮件" Value="False" Selected="true"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <asp:PlaceHolder ID="phMail" Visible="false" runat="server">
      <tr>
        <td><bairong:help HelpText="发送到管理员邮箱" Text="发送到管理员邮箱：" runat="server" ></bairong:help></td>
        <td><asp:TextBox Columns="35" MaxLength="50" id="MailTo" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="MailTo" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator ControlToValidate="MailTo" ValidationExpression="(\w[0-9a-zA-Z_-]*@(\w[0-9a-zA-Z-]*\.)+\w{2,})" Display="Dynamic" runat="server"><br>
            * 电子邮件格式不正确!</asp:RegularExpressionValidator></td>
      </tr>
      <tr>
        <td><bairong:help HelpText="邮件标题" Text="邮件标题：" runat="server" ></bairong:help></td>
        <td><asp:TextBox Columns="35" MaxLength="50" id="MailTitleFormat" Text="提醒（来自提交表单）" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="MailTitleFormat" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" /></td>
      </tr>
    </asp:PlaceHolder>
    <tr>
      <td><bairong:help HelpText="设置提交内容后是否需要发送短信给管理员" Text="是否发送短信提醒：" runat="server" ></bairong:help></td>
      <td><asp:RadioButtonList ID="IsSMS" AutoPostBack="true" OnSelectedIndexChanged="IsSMS_SelectedIndexChanged" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="发送短信" Value="True"></asp:ListItem>
          <asp:ListItem Text="不发送短信" Value="False" Selected="true"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <asp:PlaceHolder ID="phSMS" Visible="false" runat="server">
      <tr>
        <td><bairong:help HelpText="发送到管理员手机" Text="发送到管理员手机：" runat="server" ></bairong:help></td>
        <td><asp:TextBox Columns="35" MaxLength="50" id="SMSTo" runat="server" />
          <asp:RequiredFieldValidator ControlToValidate="SMSTo" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
          <asp:RegularExpressionValidator ControlToValidate="SMSTo" ValidationExpression="^1[358]\d{9}$" Display="Dynamic" runat="server"><br>
            * 手机号码格式不正确!</asp:RegularExpressionValidator></td>
      </tr>
      <tr>
        <td><bairong:help HelpText="短信标题" Text="短信标题：" runat="server" ></bairong:help></td>
        <td><asp:TextBox Columns="35" MaxLength="50" id="SMSTitle" Text="提醒：" runat="server" /></td>
      </tr>
    </asp:PlaceHolder>
    <tr>
      <td><bairong:help HelpText="提交成功提示信息" Text="提交成功提示信息：" runat="server" ></bairong:help></td>
      <td><asp:TextBox Columns="45" Rows="3" TextMode="MultiLine" MaxLength="50" id="MessageSuccess" runat="server" /></td>
    </tr>
    <tr>
      <td><bairong:help HelpText="提交失败提示信息" Text="提交失败提示信息：" runat="server" ></bairong:help></td>
      <td><asp:TextBox Columns="45" Rows="3" TextMode="MultiLine" MaxLength="50" id="MessageFailure" runat="server" /></td>
    </tr>
    <tr>
      <td><bairong:help HelpText="设置是否允许匿名提交" Text="允许匿名提交：" runat="server" ></bairong:help></td>
      <td><asp:RadioButtonList ID="IsAnomynous" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="允许" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="不允许" Value="False"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->