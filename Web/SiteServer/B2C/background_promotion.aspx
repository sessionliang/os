<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundPromotion" %>

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
      <td>促销名称</td>
      <td>开始时间</td>
      <td>结束时间</td>
      <td>促销标签</td>
      <td>促销目标</td>
      <td>条件</td>
      <td>优惠</td>
      <td>添加时间</td>
      <td>是否有效</td>
      <td width="50"></td>
      <td width="50"></td>
      <td width="20"><input type="checkbox" onClick="selectRows(document.getElementById('contents'), this.checked);" /></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center">
            <asp:Literal ID="ltlItemIndex" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlPromotionName" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlStartDate" runat="server"></asp:Literal>
          </td>
          <td>
            <asp:Literal ID="ltlEndDate" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlTags" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlTarget" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlIf" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlPromotion" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlIsValid" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlEdit" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlDelete" runat="server"></asp:Literal>
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
    <asp:Button class="btn" id="btnEnable" Text="启 用" runat="server" />
    <asp:Button class="btn" id="btnDisable" Text="禁 用" runat="server" />
  </ul>

</form>
</body>
</html>