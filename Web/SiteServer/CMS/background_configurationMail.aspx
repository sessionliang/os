<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundConfigurationMail" %>

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
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <div class="popover popover-static">
    <h3 class="popover-title">邮件发送设置</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="200"><bairong:help HelpText="发送邮件的SMTP服务器" Text="SMTP服务器：" runat="server" ></bairong:help></td>
          <td><asp:TextBox Columns="35" MaxLength="200" id="MailDomain" runat="server" />
            <asp:RequiredFieldValidator ControlToValidate="MailDomain" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="MailDomain"
                                      ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
        </tr>
        <tr>
          <td width="200"><bairong:help HelpText="发送邮件的SMTP服务器端口" Text="SMTP端口：" runat="server" ></bairong:help></td>
          <td><asp:TextBox Columns="10" MaxLength="50" Text="24" id="MailDomainPort" runat="server" />
            <asp:RequiredFieldValidator ControlToValidate="MailDomainPort" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
            <asp:RegularExpressionValidator
              ControlToValidate="MailDomainPort"
              ValidationExpression="\d+"
              Display="Dynamic"
              ErrorMessage="SMTP端口必须为大于零的整数"
              runat="server"/>
            <asp:CompareValidator 
              ControlToValidate="MailDomainPort" 
              Operator="GreaterThan" 
              ValueToCompare="0" 
              Display="Dynamic"
              ErrorMessage="SMTP端口必须为大于零的整数"
              runat="server"/></td>
        </tr>
        <tr>
          <td width="200"><bairong:help HelpText="发送邮件中显示的发件人" Text="显示发件人：" runat="server" ></bairong:help></td>
          <td><asp:TextBox Columns="35" MaxLength="50" id="MailFromName" runat="server" /></td>
        </tr>
        <tr>
          <td width="200"><bairong:help HelpText="发送邮件的系统邮箱" Text="系统邮箱：" runat="server" ></bairong:help></td>
          <td><asp:TextBox Columns="35" MaxLength="50" id="MailServerUserName" runat="server" />
            <asp:RequiredFieldValidator ControlToValidate="MailServerUserName" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="MailServerUserName"
                                      ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
        </tr>
        <tr>
          <td width="200"><bairong:help HelpText="发送邮件的系统邮箱密码" Text="系统邮箱密码：" runat="server" ></bairong:help></td>
          <td><asp:TextBox Columns="35" MaxLength="50" TextMode="Password" id="MailServerPassword" runat="server" />
            <asp:RequiredFieldValidator ControlToValidate="MailServerPassword" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="MailServerPassword"
                                      ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
        </tr>
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

  <div class="popover popover-static">
    <h3 class="popover-title">邮件发送测试</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="200"><bairong:help HelpText="发送测试到此邮箱" Text="邮箱地址：" runat="server" ></bairong:help></td>
          <td>
            <asp:TextBox Columns="35" MaxLength="200" id="TestMail" runat="server" />
            <span>多个邮箱以“;”分割</span>
          </td>
        </tr>
      </table>
  
      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button class="btn btn-primary" text="发 送" onclick="Send_OnClick" runat="server" />
          </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->