<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundAdSelect" %>

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
  <bairong:alerts text="在此选择进入对应的广告类型后可添加、修改及删除对应广告" runat="server"></bairong:alerts>

  <asp:DataList ID="MyDataList" RepeatColumns="4" ItemStyle-Width="25%" runat="server">
    <ItemTemplate>
      <table class="table table-noborder table-hover">
        <tr>
          <td class="center" style="line-height:30px;">
              <asp:Literal ID="ltlImage" runat="server"></asp:Literal>
              <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
          </td>
        </tr>
      </table>
    </ItemTemplate>
  </asp:DataList>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->