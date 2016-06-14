<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundPublishRemote" %>

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
          <asp:ImageButton ID="ibPublish" runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/toLeft.gif"></asp:ImageButton>

          <img src="../../sitefiles/bairong/icons/filesystem/management/seperator.gif" />

          <asp:HyperLink id="hlUp" runat="server">
            <img src="../../sitefiles/bairong/icons/filesystem/management/up.gif" />
          </asp:HyperLink>
          <asp:HyperLink id="hlReload" runat="server">
            <img src="../../sitefiles/bairong/icons/filesystem/management/reload.gif" />
          </asp:HyperLink>

          <img src="../../sitefiles/bairong/icons/filesystem/management/seperator.gif" />

          <asp:HyperLink ID="hlUpload" NavigateUrl="javascript:;" runat="server">
            <img src="../../sitefiles/bairong/icons/add.gif" />
            上传文件
          </asp:HyperLink>
          <asp:ImageButton ID="ibCreate" runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/create.gif"></asp:ImageButton>
          <asp:HyperLink id="hlDelete" runat="server">
            <img src="../../sitefiles/bairong/icons/filesystem/management/delete.gif" />
          </asp:HyperLink>
        </td>
      </tr>
      <tr>
        <td>
          <label class="checkbox inline">
            <input type="checkbox" onClick="_checkFormAll(this.checked);"> 全选
          </label>
          <asp:DropDownList ID="ddlListType" class="input-medium" runat="server" OnSelectedIndexChanged="ddlListType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
          &nbsp;&nbsp;当前目录：
          <asp:Literal id="ltlCurrentDirectory" runat="server" />
        </td>
      </tr>
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