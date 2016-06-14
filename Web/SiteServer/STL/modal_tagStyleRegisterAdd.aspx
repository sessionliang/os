<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.TagStyleRegisterAdd" Trace="false"%>

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
      <td width="120"><bairong:help HelpText="注册用户的用户类型" Text="用户类型：" runat="server" ></bairong:help></td>
      <td><asp:DropDownList ID="TypeID" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
      <td width="120"><bairong:help HelpText="注册用户的默认用户组" Text="默认用户组：" runat="server" ></bairong:help></td>
      <td><asp:DropDownList ID="GroupID" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
      <td width="120"><bairong:help HelpText="注册用户的默认用户积分" Text="默认用户积分：" runat="server" ></bairong:help></td>
      <td><asp:TextBox class="input-mini" MaxLength="50" id="Credits" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="Credits" errorMessage=" *" foreColor="red" display="Dynamic"
						runat="server" />
        <asp:RegularExpressionValidator
					runat="server"
					ControlToValidate="Credits"
					ValidationExpression="^([0-9]|[1-9][0-9]{1,})$"
					errorMessage=" *" foreColor="red" 
					Display="Dynamic" /></td>
    </tr>
    <tr>
      <td width="120"><bairong:help HelpText="取消注册时的返回地址" Text="返回地址：" runat="server" ></bairong:help></td>
      <td><asp:TextBox Columns="45" id="ReturnUrl" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="ReturnUrl" errorMessage=" *" foreColor="red" display="Dynamic"
						runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="ReturnUrl"
						ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->