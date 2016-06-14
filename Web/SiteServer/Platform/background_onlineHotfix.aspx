<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundOnlineHotfix" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
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
  <bairong:alerts text="如果是内网，请到官方网站下载升级包后点击“导入升级包”" runat="server"></bairong:alerts>

  <asp:Literal id="ltlHotfix" runat="server" />

  <div class="popover popover-static">
  <h3 class="popover-title">在线产品升级</h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td width="120">当前版本号：
        </td>
        <td>
          <asp:Literal ID="ltlVersion" runat="server"></asp:Literal>
        </td>
      </tr>
      <tr>
        <td>
        最近升级时间：</td>
        <td>
          <asp:Literal ID="ltlUpdateDate" runat="server"></asp:Literal>
        </td>
      </tr>
    </table>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <asp:Button ID="btnImport" class="btn btn-success" text="导入升级包" runat="server" />
        </td>
      </tr>
    </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->