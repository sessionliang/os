﻿<%@ Page Language="C#" Inherits="SiteServer.WCM.BackgroundPages.Modal.GovPublicDepartmentAdd" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.WCM.Controls" Assembly="SiteServer.WCM" %>
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
	  <td width="120">部门名称：</td>
	  <td><asp:TextBox Columns="25" MaxLength="50" id="DepartmentName" runat="server" />
		<asp:RequiredFieldValidator ControlToValidate="DepartmentName" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
		<asp:RegularExpressionValidator runat="server" ControlToValidate="DepartmentName" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
	</tr>
    <tr>
	  <td width="120">部门编号：</td>
	  <td><asp:TextBox Columns="25" MaxLength="50" id="Code" runat="server" />
		<asp:RequiredFieldValidator ControlToValidate="Code" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
		<asp:RegularExpressionValidator runat="server" ControlToValidate="Code" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
	</tr>
    <tr>
	  <td width="120">上级部门：</td>
	  <td><asp:DropDownList ID="ParentID" runat="server"> </asp:DropDownList></td>
	</tr>
	<tr>
	  <td width="120">部门简介：</td>
	  <td><asp:TextBox Columns="60" Rows="4" TextMode="MultiLine" id="Summary" runat="server" /></td>
	</tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->