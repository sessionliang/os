<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.ChannelSelect" Trace="false"%>

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

  <table class="table table-bordered table-hover">
    <tr class="info thead">
      <td>
        栏目名称
      </td>
    </tr>
    <tr treeItemLevel="2">
      <td>
        <img align="absmiddle" style="cursor:pointer" onClick="displayChildren(this);" isAjax="false" isOpen="true" src="../../sitefiles/bairong/icons/tree/minus.gif" /><img align="absmiddle" border="0" src="../../sitefiles/bairong/icons/tree/folder.gif" />&nbsp;<asp:Literal ID="ltlPublishmentSystem" runat="server"></asp:Literal>
      </td>
    </tr>
    <asp:Repeater ID="rptChannel" runat="server">
      <itemtemplate>
        <asp:Literal id="ltlHtml" runat="server" />
      </itemtemplate>
    </asp:Repeater>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->