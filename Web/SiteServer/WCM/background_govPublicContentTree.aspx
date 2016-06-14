<%@ Page Language="C#" Trace="false" EnableViewState="false" Inherits="SiteServer.WCM.BackgroundPages.BackgroundGovPublicContentTree" %>

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
<form runat="server">
  <table class="table noborder table-condensed table-hover">
    <tr class="info">
      <td style="padding-left:60px;">
        <a href="javascript:;" onclick="location.reload();" title="点击刷新栏目树"><lan>信息管理</lan></a></td>
    </tr>
  </table>

  <table class="table noborder table-condensed table-hover">
    <tr treeItemLevel="2">
      <td nowrap><img align="absmiddle" style="cursor:pointer" onClick="displayChildren(this);" isAjax="false" isOpen="true" src="../../sitefiles/bairong/icons/tree/minus.gif" /><img align="absmiddle" border="0" src="../../sitefiles/bairong/icons/tree/category.gif" />&nbsp;主题分类</td>
    </tr>
    <asp:Repeater ID="rptCategoryChannel" runat="server">
      <itemtemplate>
        <asp:Literal id="ltlHtml" runat="server" />
      </itemtemplate>
    </asp:Repeater>
  </table>

  <table class="table noborder table-condensed table-hover">
    <tr treeItemLevel="0">
      <td nowrap><img align="absmiddle" style="cursor:pointer" onClick="displayChildren_Department(this);" isAjax="true" isOpen="false" id="0" src="../../sitefiles/bairong/icons/tree/plus.gif" /><img align="absmiddle" src="../../sitefiles/bairong/icons/tree/category.gif" />&nbsp;机构分类</td>
    </tr>
  </table>

  <asp:Repeater ID="rptCategoryClass" runat="server">
    <itemtemplate>
      <table class="table noborder table-condensed table-hover">
        <tr treeItemLevel="0">
          <td nowrap><asp:Literal ID="ltlPlusIcon" runat="server"></asp:Literal><img align="absmiddle" src="../../sitefiles/bairong/icons/tree/category.gif" />&nbsp;<asp:Literal id="ltlClassName" runat="server" />分类</td>
        </tr>
      </table>
    </itemtemplate>
  </asp:Repeater>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->