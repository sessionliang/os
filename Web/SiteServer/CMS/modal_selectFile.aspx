﻿<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.SelectFile" Trace="false" %>

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

  <table class="table table-condensed noborder">
    <tr>
      <td>
        <asp:ImageButton runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/back.gif" CommandName="NavigationBar" CommandArgument="Back" OnCommand="LinkButton_Command"></asp:ImageButton>
        <asp:ImageButton runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/up.gif" CommandName="NavigationBar" CommandArgument="Up" OnCommand="LinkButton_Command"></asp:ImageButton>
        <asp:ImageButton runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/reload.gif" CommandName="NavigationBar" CommandArgument="Reload" OnCommand="LinkButton_Command"></asp:ImageButton>
        <asp:HyperLink ID="hlUploadLink" runat="server">
          <asp:ImageButton  runat="server" ImageUrl="~/sitefiles/bairong/icons/add.gif" ImageAlign="AbsBottom"></asp:ImageButton>
          上传附件</asp:HyperLink>
          <asp:DropDownList ID="ddlListType" class="input-medium" runat="server" OnSelectedIndexChanged="ddlListType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
      </td>
    </tr>
    <tr>
      <td>当前目录：<asp:Literal id="ltlCurrentDirectory" runat="server" /></td>
    </tr>
  </table>
  
  <hr />

  <asp:Literal id="ltlFileSystems" runat="server" enableViewState="false" />

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->