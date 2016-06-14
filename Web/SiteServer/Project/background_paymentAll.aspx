<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundPaymentAll" %>

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
      <asp:TemplateColumn HeaderText="发票号">
        <ItemTemplate>
          <asp:Literal ID="ltlInvoiceNO" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="100" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="次序">
        <ItemTemplate>
          <asp:Literal ID="ltlPaymentOrder" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="40" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="回款标准">
        <ItemTemplate>
          <asp:Literal ID="ltlPremise" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle HorizontalAlign="left" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="回款日期">
        <ItemTemplate>
          <asp:Literal ID="ltlPaymentDate" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="80" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="回款金额">
        <ItemTemplate>
          <asp:Literal ID="ltlAmountPaid" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="100" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="发票">
        <ItemTemplate>
          <asp:Literal ID="ltlIsInvoice" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="40" cssClass="center" />
      </asp:TemplateColumn>
      </Columns>
    </ASP:DataGrid>
  
  <br>

    <table class="table table-bordered table-hover">
      <tr class="info thead">
        <td>项目总数</td>
        <td>回款总数</td>
        <td>回款总额</td>
        <td>已开发票总数</td>
        <td>已开发票总额</td>
      </tr>
      <tr style="height:25px;">
        <td class="center"><asp:Literal ID="ltlProjectCount" runat="server"></asp:Literal>
          个 </td>
        <td class="center"><asp:Literal ID="ltlPaymentCount" runat="server"></asp:Literal>
          笔 </td>
        <td class="center"><asp:Literal ID="ltlPaymentAmountCount" runat="server"></asp:Literal></td>
        <td class="center"><asp:Literal ID="ltlInvoiceCount" runat="server"></asp:Literal>
          笔 </td>
        <td class="center"><asp:Literal ID="ltlInvoiceAmountCount" runat="server"></asp:Literal></td>
      </tr>
    </table>
  
  <br>
</form>
</body>
</html>