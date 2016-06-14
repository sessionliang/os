<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.OrderSelect" Trace="false"%>

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

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          订单类型：
          <asp:DropDownList ID="ddlType" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
          客户ID：
          <asp:TextBox ID="tbLoginName" style="height:20px; line-height:20px;" onFocus="this.className='colorfocus';" onBlur="this.className='colorblur';" Size="20" runat="server"></asp:TextBox>
          关键字：
          <asp:TextBox ID="tbKeyword" style="height:20px; line-height:20px;" onFocus="this.className='colorfocus';" onBlur="this.className='colorblur';" Size="20" runat="server"></asp:TextBox>
          <asp:Button OnClick="Search_OnClick" Text="搜 索" class="btn" style="margin-bottom: 0px" runat="server"></asp:Button>
        </td>
        <td class="pull-right">合计：<code><asp:Literal ID="ltlTotalCount" runat="server"></asp:Literal></code></td>
      </tr>
    </table>
  </div>

  <table id="contents" class="table table-bordered table-hover">
    <tr class="info thead">
      <td>订单ID </td>
      <td>客户ID </td>
      <td>订单金额 </td>
      <td>临时域名 </td>
      <td>模板ID </td>
      <td>邮箱</td>
      <td>QQ</td>
      <td>下单日期</td>
      <td>状态</td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center"><asp:Literal ID="ltlSN" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlLoginName" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlAmount" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlDomainTemp" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlMobanID" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlEmail" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlQQ" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlAddDate" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlStatus" runat="server"></asp:Literal></td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />
  
</form>
</body>
</html>
