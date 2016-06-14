<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.TagStyleDiggAdd" Trace="false"%>

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
      <td width="150"><bairong:help HelpText="样式名称" Text="样式名称：" runat="server" ></bairong:help></td>
      <td><asp:TextBox Columns="25" MaxLength="50" id="StyleName" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="StyleName" errorMessage=" *" foreColor="red" display="Dynamic"
						runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="StyleName"
						ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td><bairong:help HelpText="设置显示类型" Text="显示类型：" runat="server" ></bairong:help></td>
      <td><asp:RadioButtonList ID="DiggType" runat="server"></asp:RadioButtonList></td>
    </tr>
    <tr>
      <td><bairong:help HelpText="设置赞同显示文字" Text="赞同显示文字：" runat="server" ></bairong:help></td>
      <td><asp:TextBox Columns="25" MaxLength="50" id="GoodText" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="GoodText" errorMessage=" *" foreColor="red" display="Dynamic"
						runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="GoodText"
						ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td><bairong:help HelpText="设置不赞同显示文字" Text="不赞同显示文字：" runat="server" ></bairong:help></td>
      <td><asp:TextBox Columns="25" MaxLength="50" id="BadText" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="BadText" errorMessage=" *" foreColor="red" display="Dynamic"
						runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="BadText"
						ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td><bairong:help HelpText="设置显示样式" Text="显示样式：" runat="server" ></bairong:help></td>
      <td><asp:DropDownList ID="ddlTheme" runat="server"></asp:DropDownList></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->