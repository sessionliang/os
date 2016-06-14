<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundInputClassify" enableViewState = "false" %>

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

  <table class="table table-bordered table-hover">
    <tr class="info thead">
      <td>分类名</td>
      <td width="100">分类索引</td>
      <td width="30">上升</td>
      <td width="30">下降</td>
      <td width="50">&nbsp;</td>
      <td width="20"></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
          <asp:Literal id="ltlHtml" runat="server" />
      </itemtemplate>
    </asp:Repeater>
  </table>

  <ul class="breadcrumb breadcrumb-button">
    <asp:PlaceHolder id="PlaceHolder_AddChannel" runat="server">
      <asp:Button class="btn btn-success" id="AddChannel1" Text="快速添加" runat="server" />
      <asp:Button class="btn" id="AddChannel2" Text="添加分类" runat="server" />
    </asp:PlaceHolder>
    <asp:PlaceHolder id="PlaceHolder_Delete" runat="server">
      <asp:Button class="btn" id="Delete" Text="删 除" runat="server" />
    </asp:PlaceHolder>
    <asp:PlaceHolder id="PlaceHolder_Permission" runat="server">
      <asp:Button class="btn" id="btnPermission" Text="权 限" runat="server" />
    </asp:PlaceHolder>
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->