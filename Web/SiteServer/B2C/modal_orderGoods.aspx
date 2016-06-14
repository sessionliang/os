<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.Modal.OrderGoods" %>

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
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>

  <script type="text/javascript">
  $(document).ready(function()
  {
    loopRows(document.getElementById('contents'), function(cur){ cur.onclick = chkSelect; });
    $(".popover-hover").popover({trigger:'hover',html:true});
  });
  </script>

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          栏目：
          <asp:DropDownList ID="NodeIDDropDownList" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
          <asp:CheckBox ID="IsDuplicate" class="checkbox inline" Text="包含重复标题" runat="server"></asp:CheckBox>
        </td>
      </tr>
      <tr>
        <td>
          时间：从
          <bairong:DateTimeTextBox id="DateFrom" class="input-small" Columns="12" runat="server" />
          &nbsp;到&nbsp;
          <bairong:DateTimeTextBox id="DateTo" class="input-small" Columns="12" runat="server" />
          目标：
          <asp:DropDownList ID="SearchType" class="input-small" runat="server"> </asp:DropDownList>
          关键字：
          <asp:TextBox id="Keyword"
            MaxLength="500"
            Size="37"
            runat="server"/>
          <asp:Button class="btn" OnClick="Search_OnClick" id="Search" text="搜 索"  runat="server"/>
        </td>
      </tr>
    </table>
  </div>

  <table id="contents" class="table table-bordered table-hover">
    <tr class="info thead">
      <td width="120">栏目</td>
      <td>商品标题(点击查看) </td>
      <td width="140">货号 </td>
      <td>货品规格 </td>
      <td width="40">价格 </td>
      <td width="20"></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td>
            <asp:Literal ID="ltlChannel" runat="server"></asp:Literal>
          </td>
          <td>
            <asp:Literal ID="ltlTitle" runat="server"></asp:Literal>
          </td>
          <td></td>
          <td></td>
          <td></td>
          <td class="center">
            
          </td>
        </tr>
        
        <asp:Repeater ID="rptGoods" runat="server">
          <itemtemplate>
            <tr>
              <td></td>
              <td></td>
              <td>
                <asp:Literal ID="ltlGoodsSN" runat="server"></asp:Literal>
              </td>
              <td>
                <div class="specItem">
                  <asp:Literal ID="ltlGoodsSpec" runat="server"></asp:Literal>
                </div>
              </td>
              <td class="center">
                <asp:Literal ID="ltlPrice" runat="server"></asp:Literal>
              </td>
              <td class="center">
                <asp:Literal ID="ltlSelect" runat="server"></asp:Literal>
              </td>
            </tr>
          </itemtemplate>
        </asp:Repeater>

      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->