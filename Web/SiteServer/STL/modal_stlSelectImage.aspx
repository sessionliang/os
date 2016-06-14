<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.StlTemplate.StlSelectImage" Trace="false"%>

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
<bairong:alerts runat="server"></bairong:alerts>

  <table width="100%" cellspacing=0 cellpadding=0 border=0 style="line-height:28px;">
    <tr>
      <td width="50"><asp:ImageButton runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/back.gif" CommandName="NavigationBar" CommandArgument="Back" OnCommand="LinkButton_Command"></asp:ImageButton></TD>
      <TD width="50"><asp:ImageButton runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/up.gif" CommandName="NavigationBar" CommandArgument="Up" OnCommand="LinkButton_Command"></asp:ImageButton></TD>
      <TD width="50"><asp:ImageButton runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/reload.gif" CommandName="NavigationBar" CommandArgument="Reload" OnCommand="LinkButton_Command"></asp:ImageButton></TD>
      <TD width="5"><asp:ImageButton runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/seperator.gif"></asp:ImageButton></TD>
      <TD width="80"><nobr>
        <asp:HyperLink ID="hlUploadLink" runat="server">
          <asp:ImageButton  runat="server" ImageUrl="~/sitefiles/bairong/icons/add.gif" ImageAlign="AbsBottom"></asp:ImageButton>
          上传图片</asp:HyperLink>
        </nobr></TD>
      <TD align="right"><span>当前目录：
        <asp:Literal id="ltlCurrentDirectory" runat="server" />
        &nbsp;</span></td>
    </tr>
  </table>

  <hr />
  
  <asp:Literal id="ltlFileSystems" runat="server" enableViewState="false" />

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->