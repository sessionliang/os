<%@ Page Language="C#" Inherits="SiteServer.WCM.BackgroundPages.Modal.GovPublicCategoryChannelSelect" Trace="false"%>

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

  <table class="table noborder table-hover table-condensed">
    <tr treeItemLevel="2">
      <td nowrap><img align="absmiddle" style="cursor:pointer" onClick="displayChildren(this);" isAjax="false" isOpen="true" src="../../sitefiles/bairong/icons/tree/minus.gif" /><img align="absmiddle" border="0" src="../../sitefiles/bairong/icons/tree/category.gif" />&nbsp;主题分类</td>
    </tr>
    <asp:Repeater ID="rptCategory" runat="server">
      <itemtemplate>
        <asp:Literal id="ltlHtml" runat="server" />
      </itemtemplate>
    </asp:Repeater>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->