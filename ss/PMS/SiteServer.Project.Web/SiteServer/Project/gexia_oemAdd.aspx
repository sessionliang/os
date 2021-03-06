﻿<%@ Page Language="C#" Inherits="SiteServer.GeXia.BackgroundPages.BackgroundOEMAdd" %>

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
    <h3 class="popover-title"><asp:Literal id="ltlPageTitle" runat="server" /></h3>
    <div class="popover-content">
      
      <table class="table noborder table-hover">
        <tr>
          <td width="120">账号：</td>
          <td>
            <asp:TextBox ID="tbUserName" MaxLength="50" Size="35" runat="server" />
            <asp:RequiredFieldValidator ControlToValidate="tbUserName" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbUserName" ValidationExpression="[^']+" ErrorMessage="不能用单引号" Display="Dynamic" />
            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbUserName" ValidationExpression="[^<]+" ErrorMessage="不能用&lt;注册" Display="Dynamic" />
            <span>（帐号用于登录系统，由字母、数字组成）</span>
          </td>
        </tr>
        <tr>
          <td>OEM级别：</td>
          <td>
            <asp:DropDownList ID="ddlOEMType" runat="server"></asp:DropDownList>
          </td>
        </tr>
        <asp:PlaceHolder ID="phPassword" runat="server">
        <tr>
          <td>密码：</td>
          <td>
            <asp:TextBox TextMode="Password" ID="tbPassword" MaxLength="50" Size="35" runat="server" />
            <asp:RequiredFieldValidator ControlToValidate="tbPassword" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
          </td>
        </tr>
        <tr>
          <td>确认密码：</td>
          <td>
            <asp:TextBox TextMode="Password" ID="tbConfirmPassword" MaxLength="50" Size="35" runat="server" />
            <asp:RequiredFieldValidator ControlToValidate="tbConfirmPassword" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
            <asp:CompareValidator ID="tbNewPasswordCompare" runat="server" ControlToCompare="tbPassword" ControlToValidate="tbConfirmPassword" Display="Dynamic" foreColor="red" ErrorMessage=" 两次输入的密码不一致！请再输入一遍您上面填写的密码。"></asp:CompareValidator>
          </td>
        </tr>
        </asp:PlaceHolder>
        <tr>
          <td>手机号码：</td>
          <td>
            <asp:TextBox ID="tbMobile" runat="server" Size="35"></asp:TextBox>
            <asp:RegularExpressionValidator ControlToValidate="tbMobile" ValidationExpression="^0?\d{11}$" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
          </td>
        </tr>
        <tr>
          <td>电子邮箱：</td>
          <td>
            <asp:TextBox ID="tbEmail" runat="server" Size="35"></asp:TextBox>
            <asp:RegularExpressionValidator ControlToValidate="tbEmail" ValidationExpression="(\w[0-9a-zA-Z_-]*@(\w[0-9a-zA-Z-]*\.)+\w{2,})" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
          </td>
        </tr>
        <tr>
          <td>公司名称：</td>
          <td>
            <asp:TextBox ID="tbCompanyName" runat="server" Size="35"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td>OEM平台名称：</td>
          <td>
            <asp:TextBox ID="tbPlatformName" runat="server" Size="35"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td>平台域名：</td>
          <td>
            <asp:TextBox ID="tbDomain" runat="server" Size="35"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td>允许新用户注册：</td>
          <td>
            <asp:CheckBox ID="cbIsRegisterAllowed" runat="server" text="允许"></asp:CheckBox>
          </td>
        </tr>
        <tr>
          <td>Logo地址：</td>
          <td>
            <asp:TextBox ID="tbLogoUrl" runat="server" Size="35"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td>首页大图地址：</td>
          <td>
            <asp:TextBox ID="tbIndexImageUrl" runat="server" Size="35"></asp:TextBox>
          </td>
        </tr>
        <tr>
          <td>版权：</td>
          <td>
            <asp:TextBox ID="tbCopyright" runat="server" Size="35"></asp:TextBox>
          </td>
        </tr>
      </table>

      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button class="btn btn-primary" OnClick="Submit_OnClick" Text="确 定" runat="server" />
            <asp:Button class="btn" ID="btnReturn" Text="返 回" runat="server" />
          </td>
        </tr>
      </table>

    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->