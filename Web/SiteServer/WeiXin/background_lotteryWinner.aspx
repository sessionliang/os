<%@ Page Language="C#" Inherits="SiteServer.WeiXin.BackgroundPages.BackgroundLotteryWinner" %>

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
  <bairong:alerts runat="server" />

  <script type="text/javascript">
  $(document).ready(function()
  {
    loopRows(document.getElementById('contents'), function(cur){ cur.onclick = chkSelect; });
    $(".popover-hover").popover({trigger:'hover',html:true});
  });
  </script>

  <table id="contents" class="table table-bordered table-hover">
    <tr class="info thead">
      <td width="20"></td>
      <td>奖项</td>
      <td>姓名</td>
      <td>手机</td>
      <td>邮箱</td>
      <td>地址</td>
      <td>状态</td>
      <td>中奖时间</td>
      <td>兑奖码</td>
      <td>兑奖时间</td>
      <td width="20"><input type="checkbox" onClick="selectRows(document.getElementById('contents'), this.checked);" /></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center">
            <asp:Literal ID="ltlItemIndex" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlAward" runat="server"></asp:Literal>
          </td>
          <td>
            <asp:Literal ID="ltlRealName" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlMobile" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlEmail" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlAddress" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlStatus" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlCashSN" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlCashDate" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <input type="checkbox" name="IDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' />
          </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn" id="btnDelete" Text="删 除" runat="server" />
    <asp:Button class="btn" id="btnSetting" Text="设置状态" runat="server" />
    <asp:Button class="btn" id="btnExport" Text="导出CSV" runat="server" />
    <asp:Button class="btn" id="btnReturn" Text="返 回" runat="server" />
  </ul>

</form>
</body>
</html>