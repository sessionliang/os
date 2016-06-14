<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundApplyAnalysis" %>

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
  <bairong:alerts text="不设置开始时间将统计自建站以来所有的统计数据。" runat="server" />

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
          项目名称：
          <asp:DropDownList ID="ddlInteractName" AutoPostBack="true" OnSelectedIndexChanged="Analysis_OnClick" runat="server"></asp:DropDownList>
          &nbsp;&nbsp;
          <asp:Button class="btn" id="Analysis" style="margin-bottom:0px;" OnClick="Analysis_OnClick" Text="统 计" runat="server" />
        </td>
      </tr>
    </table>
  </div>

  <table class="table table-bordered table-hover">
    <tr class="info thead">
      <td width="200">统计对象</td>
      <td width="60">办件总数</td>
      <td width="60">已回复</td>
      <td width="60">未回复</td>
      <td width="60">回复率</td>
      <td>回复率</td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <asp:Literal id="ltlTrHtml" runat="server"></asp:Literal>
            <td><asp:Literal id="ltlTarget" runat="server"></asp:Literal></td>
            <td class="center" style="font-weight:bold"><asp:Literal id="ltlTotalCount" runat="server"></asp:Literal></td>
            <td class="center" style="font-weight:bold"><asp:Literal id="ltlDoCount" runat="server"></asp:Literal></td>
            <td class="center" style="font-weight:bold"><asp:Literal id="ltlUndoCount" runat="server"></asp:Literal></td>
            <td class="center" style="font-weight:bold"><asp:Literal id="ltlPercentage" runat="server"></asp:Literal></td>
            <td>
              <div class="progress">
                <asp:Literal id="ltlBar" runat="server"></asp:Literal>
              </div>
            </td>
          </tr>
       </itemtemplate>
    </asp:Repeater>
  </table>

  <br>

</form>
</body>
</html>