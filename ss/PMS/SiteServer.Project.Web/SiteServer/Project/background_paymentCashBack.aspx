<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundPaymentCashBack" %>

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
  <bairong:alerts text="" runat="server" />

  <ul class="nav nav-pills">
    <li class="<%=GetActive(false)%>"><a href="background_paymentCashBack.aspx?isPayment=False">全部待返款项</a></li>
    <li class="<%=GetActive(true)%>"><a href="background_paymentCashBack.aspx?isPayment=True">全部已返款项</a></li>
  </ul>

    <asp:dataGrid id="MyDataGrid" DataKeyField="PaymentID" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" runat="server">
      <Columns>
      <asp:TemplateColumn HeaderText="项目名称">
        <ItemTemplate>
          <asp:Literal ID="ltlProjectName" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="合同号">
        <ItemTemplate>
          <asp:Literal ID="ltlContractNO" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="100" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="次序">
        <ItemTemplate>
          <asp:Literal ID="ltlPaymentOrder" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="40" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="返款标准">
        <ItemTemplate>
          <asp:Literal ID="ltlPremise" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle HorizontalAlign="left" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="预计返款日期">
        <ItemTemplate>
          <asp:Literal ID="ltlExpectDate" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="100" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="预计返款">
        <ItemTemplate>
          <asp:Literal ID="ltlAmountExpect" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="100" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="实际返款日期">
        <ItemTemplate>
          <asp:Literal ID="ltlPaymentDate" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="100" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="实际返款">
        <ItemTemplate>
          <asp:Literal ID="ltlAmountPaid" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="100" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate>
          <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="40" cssClass="center" />
      </asp:TemplateColumn>
      </Columns>
    </ASP:DataGrid>

    <br>

    <table class="table table-bordered table-hover">
      <tr class="info thead">
        <td>合计项目总数</td>
        <td>预计返款总数</td>
        <td>预计返款总额</td>
        <td>实际返款总数</td>
        <td>实际返款总额</td>
      </tr>
      <tr>
        <td class="center"><asp:Literal ID="ltlProjectCount" runat="server"></asp:Literal>
          个 </td>
        <td class="center"><asp:Literal ID="ltlExpectCount" runat="server"></asp:Literal>
          笔 </td>
        <td class="center"><asp:Literal ID="ltlExpectAmountCount" runat="server"></asp:Literal></td>
        <td class="center"><asp:Literal ID="ltlPaymentCount" runat="server"></asp:Literal>
          笔 </td>
        <td class="center"><asp:Literal ID="ltlPaymentAmountCount" runat="server"></asp:Literal></td>
      </tr>
    </table>

    <br>

</form>
</body>
</html>