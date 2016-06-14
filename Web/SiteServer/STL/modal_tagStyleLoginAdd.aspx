<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.TagStyleLoginAdd" Trace="false"%>

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
      <td width="120"><bairong:help HelpText="样式名称" Text="样式名称：" runat="server" ></bairong:help></td>
      <td><asp:TextBox Columns="25" MaxLength="50" id="StyleName" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="StyleName" errorMessage=" *" foreColor="red" display="Dynamic"
						runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="StyleName"
						ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td><bairong:help HelpText="设置是否显示验证码" Text="显示验证码：" runat="server" ></bairong:help></td>
      <td><asp:RadioButtonList ID="IsValidateCode" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="显示" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="不显示" Value="False"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <tr>
      <td><bairong:help HelpText="设置表单显示方式" Text="表单显示方式：" runat="server" ></bairong:help></td>
      <td><asp:RadioButtonList ID="IsHorizontal" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="竖向显示" Value="False"></asp:ListItem>
          <asp:ListItem Text="横向显示" Value="True" Selected="true"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <tr>
      <td><bairong:help HelpText="设置是否显示记录状态复选框" Text="显示记录状态复选框：" runat="server" ></bairong:help></td>
      <td><asp:RadioButtonList ID="IsRemember" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="显示" Value="True"></asp:ListItem>
          <asp:ListItem Text="不显示" Value="False" Selected="true"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <tr>
      <td><bairong:help HelpText="设置是否显示忘记密码链接" Text="显示忘记密码链接：" runat="server" ></bairong:help></td>
      <td><asp:RadioButtonList ID="IsForget" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="显示" Value="True"></asp:ListItem>
          <asp:ListItem Text="不显示" Value="False" Selected="true"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <tr>
      <td><bairong:help HelpText="设置是否显示新用户注册链接：" Text="显示新用户注册链接：" runat="server" ></bairong:help></td>
      <td><asp:RadioButtonList ID="IsRegister" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="显示" Value="True"></asp:ListItem>
          <asp:ListItem Text="不显示" Value="False" Selected="true"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->