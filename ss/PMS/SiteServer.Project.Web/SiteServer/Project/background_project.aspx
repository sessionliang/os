<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundProject" %>

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

  <asp:dataGrid id="dgContents" DataKeyField="ProjectID" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" runat="server">
  	<Columns>
            <asp:TemplateColumn HeaderText="项目名称">
				<ItemTemplate>
					<asp:Literal ID="ltlProjectName" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="项目类型">
				<ItemTemplate>
					<asp:Literal ID="ltlProjectType" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" Width="80" />
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="开始时间">
                <ItemTemplate>
                    <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
                </ItemTemplate>
                <ItemStyle cssClass="center" Width="70" />
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="合同金额">
				<ItemTemplate>
					<asp:Literal ID="ltlAmountTotal" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" Width="100" />
		 </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="待回款">
                <ItemTemplate>
                  <asp:Literal ID="ltlAmountRemain" runat="server"></asp:Literal>
                </ItemTemplate>
                <ItemStyle cssClass="center" Width="100" />
             </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="合同号">
				<ItemTemplate>
					<asp:Literal ID="ltlContractNO" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" Width="110" />
			</asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="组成人员">
                <ItemTemplate>
                    <asp:Literal ID="ltlUserNameCollection" runat="server"></asp:Literal>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="项目备注">
				<ItemTemplate>
					<asp:Literal ID="ltlDescription" runat="server"></asp:Literal>
				</ItemTemplate>
			</asp:TemplateColumn>
            <asp:TemplateColumn>
				<ItemTemplate>
					<asp:Literal ID="ltlDocumentUrl" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle cssClass="center" />
			</asp:TemplateColumn>     
            <asp:TemplateColumn>
                <ItemTemplate>
                    <asp:Literal ID="ltlApplyUrl" runat="server"></asp:Literal>
                </ItemTemplate>
                <ItemStyle cssClass="center" />
            </asp:TemplateColumn> 
            <asp:TemplateColumn>
				<ItemTemplate>
					<asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle Width="30" cssClass="center" />
			</asp:TemplateColumn>
			<asp:TemplateColumn>
				<ItemTemplate>
					<asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
				</ItemTemplate>
				<ItemStyle Width="30" cssClass="center" />
			</asp:TemplateColumn>
		</Columns>
  </asp:dataGrid>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="AddButton" Text="新 增" runat="server" />
  </ul>

</form>
</body>
</html>