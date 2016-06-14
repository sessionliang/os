<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundPayment" %>

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

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          项目名称：
          <code><asp:Literal ID="ltlProjectName" runat="server"></asp:Literal></code>
          合同号：
          <code><asp:Literal ID="ltlContractNO" runat="server"></asp:Literal></code>
          创建日期：
          <code><asp:Literal ID="ltlAddDate" runat="server"></asp:Literal></code>
        </td>
      </tr>
    </table>
  </div>

  		<h4>回款清单</h4>

		<asp:dataGrid id="dgContents1" DataKeyField="PaymentID" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" runat="server">
		<Columns>
			<asp:TemplateColumn HeaderText="次序">
				<ItemTemplate>
					<asp:Literal ID="ltlPaymentOrder" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle width="30" cssClass="center" />
			</asp:TemplateColumn>
           	 <asp:TemplateColumn HeaderText="发票号">
				<ItemTemplate>
					<asp:Literal ID="ltlInvoiceNO" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
            	<asp:TemplateColumn HeaderText="回款标准">
				<ItemTemplate>
					<asp:Literal ID="ltlPremise" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle HorizontalAlign="left" />
			</asp:TemplateColumn>
            	<asp:TemplateColumn HeaderText="预计回款日期">
				<ItemTemplate>
					<asp:Literal ID="ltlExpectDate" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
            	<asp:TemplateColumn HeaderText="预计回款">
				<ItemTemplate>
					<asp:Literal ID="ltlAmountExpect" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
            	<asp:TemplateColumn HeaderText="发票">
				<ItemTemplate>
					<asp:Literal ID="ltlIsInvoice" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
            	<asp:TemplateColumn HeaderText="是否回款">
				<ItemTemplate>
					<asp:Literal ID="ltlIsPayment" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="实际回款日期">
				<ItemTemplate>
					<asp:Literal ID="ltlPaymentDate" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
            	<asp:TemplateColumn HeaderText="实际回款">
				<ItemTemplate>
					<asp:Literal ID="ltlAmountPaid" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
            	<asp:TemplateColumn>
				<ItemTemplate>
					<asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle Width="40" cssClass="center" />
			</asp:TemplateColumn>
			<asp:TemplateColumn>
				<ItemTemplate>
					<asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle Width="40" cssClass="center" />
			</asp:TemplateColumn>
		</Columns>
	</ASP:DataGrid>

	<hr />

	<asp:PlaceHolder id="phCashBack" runat="server" visible="false">

	<h4>返款清单</h4>

	<asp:dataGrid id="dgContents2" DataKeyField="PaymentID" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" runat="server">
		<Columns>
			<asp:TemplateColumn HeaderText="次序">
				<ItemTemplate>
					<asp:Literal ID="ltlPaymentOrder" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle width="30" cssClass="center" />
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
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
            	<asp:TemplateColumn HeaderText="预计返款">
				<ItemTemplate>
					<asp:Literal ID="ltlAmountExpect" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
            	<asp:TemplateColumn HeaderText="发票">
				<ItemTemplate>
					<asp:Literal ID="ltlIsInvoice" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
            	<asp:TemplateColumn HeaderText="是否返款">
				<ItemTemplate>
					<asp:Literal ID="ltlIsPayment" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="实际返款日期">
				<ItemTemplate>
					<asp:Literal ID="ltlPaymentDate" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
            	<asp:TemplateColumn HeaderText="实际返款">
				<ItemTemplate>
					<asp:Literal ID="ltlAmountPaid" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
            	<asp:TemplateColumn>
				<ItemTemplate>
					<asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle Width="40" cssClass="center" />
			</asp:TemplateColumn>
			<asp:TemplateColumn>
				<ItemTemplate>
					<asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle Width="40" cssClass="center" />
			</asp:TemplateColumn>
		</Columns>
	</ASP:DataGrid>

	<hr />

	</asp:PlaceHolder>

	<table class="table table-bordered table-hover">
		<tr class="info thead">
			<td>合同金额</td>
			<td>预计总回款</td>
			<td>已回款</td>
			<td>待回款</td>
			<td>预计总返款</td>
			<td>已返款</td>
			<td>待返款</td>
		</tr>
		<tr>
			<td class="center"><asp:Literal id="ltlAmountTotal" runat="server" /></td>
			<td class="center"><asp:Literal id="ltlAmountExpectAll" runat="server" /></td>
			<td class="center"><asp:Literal id="ltlAmountPaidAll" runat="server" /></td>
			<td class="center"><asp:Literal id="ltlAmountNotPaidAll" runat="server" /></td>
			<td class="center"><asp:Literal id="ltlAmountExpectAllCashBack" runat="server" /></td>
			<td class="center"><asp:Literal id="ltlAmountPaidAllCashBack" runat="server" /></td>
			<td class="center"><asp:Literal id="ltlAmountNotPaidAllCashBack" runat="server" /></td>
		</tr>
	</table>

 	<ul class="breadcrumb breadcrumb-button">
	    <asp:Button class="btn btn-success" id="btnAdd" Text="新 增" runat="server" />
	    <asp:Button class="btn" id="btnReturn" Text="返 回" runat="server" />
	</ul>

</form>
</body>
</html>