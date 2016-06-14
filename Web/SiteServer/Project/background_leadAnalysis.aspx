<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundLeadAnalysis" %>

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
  <bairong:alerts text="不设置开始时间将统计所有数据。" runat="server" />

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          开始时间：
          <bairong:DateTimeTextBox id="StartDate" Columns="30" runat="server" />
          &nbsp;&nbsp;
          结束时间：
          <bairong:DateTimeTextBox id="EndDate" Columns="30" runat="server" />
          &nbsp;&nbsp;
          <asp:Button class="btn" id="Analysis" style="margin-bottom:0px;" OnClick="Analysis_OnClick" Text="统 计" runat="server" />
        </td>
      </tr>
    </table>
  </div>

  <table class="table table-bordered table-hover">
    <tr class="info thead">
      <td>统计对象</td>
      <td width="60">线索总数</td>
      <td width="60">工单总数</td>
      <td width="60">无效线索</td>
      <td>线索来源</td>
      <td width="60">丢单</td>
      <td width="60">成单</td>
      <td width="60">成单率</td>
      <td>成单率：<br />成单/(线索总数-无效线索+工单总数)</td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <asp:Literal id="ltlTrHtml" runat="server"></asp:Literal>
            <td><asp:Literal id="ltlTarget" runat="server"></asp:Literal></td>
            <td class="center" >
              <code><asp:Literal id="ltlLeadTotalCount" runat="server"></asp:Literal></code>
            </td>
            <td class="center">
              <code><asp:Literal id="ltlRequestTotalCount" runat="server"></asp:Literal></code>
            </td>
            <td class="center">
              <code><asp:Literal id="ltlInvalidCount" runat="server"></asp:Literal></code>
            </td>
            <td>
              <asp:Literal id="ltlSource" runat="server"></asp:Literal>
            </td>
            <td class="center" >
              <code><asp:Literal id="ltlFailureCount" runat="server"></asp:Literal></code>
            </td>
            <td class="center">
              <code><asp:Literal id="ltlSuccessCount" runat="server"></asp:Literal></code>
            </td>
            <td class="center">
              <code><asp:Literal id="ltlPercentage" runat="server"></asp:Literal></code>
            </td>
            <td>
                <asp:Literal id="ltlBar" runat="server"></asp:Literal>
            </td>
          </tr>
       </itemtemplate>
    </asp:Repeater>
  </table>

  <br>

</form>
</body>
</html>