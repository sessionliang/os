<%@ Page Language="C#" Inherits="SiteServer.WeiXin.BackgroundPages.BackgroundCoupon" %>

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
      <td>优惠劵名称</td>
      <td>关联活动</td>
      <td>优惠劵总数</td>
      <td>已领取总数</td>
      <td>已使用总数</td>
      <td>添加日期</td>
      <td></td>
      <td></td>
      <td width="20"><input type="checkbox" onClick="selectRows(document.getElementById('contents'), this.checked);" /></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center">
            <asp:Literal ID="ltlItemIndex" runat="server"></asp:Literal>
          </td>
          <td>
            <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlActTitle" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlTotalNum" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlHoldNum" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlCashNum" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
          </td>
           <td class="center">
            <asp:Literal ID="ltlSN" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
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
    <asp:Button class="btn btn-success" id="btnAdd" Text="添 加" runat="server" />
    <asp:Button class="btn" id="btnDelete" Text="删 除" runat="server" />
    <asp:Button class="btn" id="btnReturn" text="返 回" runat="server"/>
  </ul>

</form>
</body>
</html>