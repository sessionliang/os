<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundPublishLocal" %>

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
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          <asp:ImageButton runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/up.gif" CommandName="NavigationBar" CommandArgument="Up" OnCommand="LinkButton_Command"></asp:ImageButton>
          <asp:ImageButton runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/reload.gif" CommandName="NavigationBar" CommandArgument="Reload" OnCommand="LinkButton_Command"></asp:ImageButton>
          <asp:ImageButton runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/seperator.gif"></asp:ImageButton>
          <asp:HyperLink ID="UploadLink" NavigateUrl="javascript:;" runat="server">
            <asp:ImageButton  runat="server" ImageUrl="~/sitefiles/bairong/icons/add.gif" ImageAlign="AbsBottom"></asp:ImageButton>
            上传文件
          </asp:HyperLink>
          <asp:ImageButton ID="CreateButton" runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/create.gif"></asp:ImageButton>
          <asp:ImageButton ID="DeleteButton" runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/delete.gif" CommandName="NavigationBar" CommandArgument="Delete" OnCommand="LinkButton_Command"></asp:ImageButton>
          <asp:ImageButton runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/seperator.gif"></asp:ImageButton>
          <asp:ImageButton ID="SendButton" runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/toRight.gif"></asp:ImageButton>
        </td>
      </tr>
      <TR>
        <TD>
          <label class="checkbox inline">
            <input type="checkbox" onClick="_checkFormAll(this.checked);"> 全选
          </label>
          <asp:DropDownList ID="ddlListType" class="input-medium" runat="server" OnSelectedIndexChanged="ddlListType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
          &nbsp;&nbsp;当前目录：
          <asp:Literal id="ltlCurrentDirectory" runat="server" />
        </TD>
      </TR>
    </table>
  </div>

  <table class="table table-bordered">
    <tr>
      <td>
        <asp:Literal id="ltlFileSystems" runat="server" enableViewState="false" />
      </td>
    </tr>
  </table>

  <br>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->