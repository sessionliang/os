<%@ Page Language="C#" Trace="false" EnableViewState="false" Inherits="BaiRong.BackgroundPages.BackgroundDepartmentTree" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form runat="server">
  <table class="table noborder table-condensed table-hover">
    <tr class="info">
      <td style="padding-left:60px;">
        <a href="background_administrator.aspx" target="department"><lan>管理员管理</lan></a></td>
    </tr>
    <tr>
      <td><img align="absmiddle" style="cursor:pointer" onClick="displayChildren(this);" isAjax="false" isOpen="true" src="../../sitefiles/bairong/icons/tree/minus.gif" /><img align="absmiddle" border="0" src="../../sitefiles/bairong/icons/tree/department.gif" />&nbsp;<a id="a_all" href="background_administrator.aspx" onClick="fontWeightLink(this)" target="department">所有部门</a></td>
    </tr>
    <asp:Repeater ID="rptDepartment" runat="server">
      <itemtemplate>
        <asp:Literal id="ltlHtml" runat="server" />
      </itemtemplate>
    </asp:Repeater>
  </table>
</form>
</body>
</html>
<!-- check for 3.6 html permissions -->