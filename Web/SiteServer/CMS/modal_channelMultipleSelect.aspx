<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.ChannelMultipleSelect" Trace="false"%>

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
<bairong:alerts runat="server" text="点击栏目名称进行选择"></bairong:alerts>

  <asp:PlaceHolder id="phPublishmentSystemID" runat="server">
    <div class="well well-small">
      选择站点：
      <asp:DropDownList ID="ddlPublishmentSystemID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PublishmentSystemID_OnSelectedIndexChanged"> </asp:DropDownList>
    </div>
  </asp:PlaceHolder>

  <table class="table table-bordered table-hover">
    <tr treeItemLevel="0">
      <td>
        <img align="absmiddle" src="../../sitefiles/bairong/icons/tree/minus.gif" />
        <img align="absmiddle" border="0" src="../../sitefiles/bairong/icons/tree/folder.gif" />
        <asp:Literal ID="ltlChannelName" runat="server"></asp:Literal>
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