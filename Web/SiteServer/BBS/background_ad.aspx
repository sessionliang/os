<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundAd" %>

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

  <asp:dataGrid id="MyDataGrid" runat="server" showHeader="true"
      ShowFooter="false"
      AutoGenerateColumns="false"
      DataKeyField="AdName"
      HeaderStyle-CssClass="info thead"
      CssClass="table table-bordered table-hover"
      gridlines="none">
    <Columns>
		<asp:TemplateColumn
			HeaderText="广告名称">
			<ItemTemplate>
				<%#DataBinder.Eval(Container.DataItem,"AdName")%>
			</ItemTemplate>
			<ItemStyle cssClass="center" />
		</asp:TemplateColumn>
		<asp:TemplateColumn
			HeaderText="广告类型">
			<ItemTemplate>
				<%#GetAdType((string)DataBinder.Eval(Container.DataItem,"AdType"))%>
			</ItemTemplate>
			<ItemStyle Width="140" cssClass="center" />
		</asp:TemplateColumn>
		<asp:BoundColumn
			HeaderText="生效时间"
			DataField="StartDate"
			DataFormatString="{0:yyyy-MM-dd}"
			ReadOnly="true">
			<ItemStyle Width="70" cssClass="center" />
		</asp:BoundColumn>
		<asp:TemplateColumn HeaderText="是否有效">
			<ItemTemplate>
				<div class="center"><%#GetIsEnabled((string)DataBinder.Eval(Container.DataItem,"IsEnabled"))%></div>
			</ItemTemplate>
			<ItemStyle Width="70" cssClass="center" />
		</asp:TemplateColumn>
    <asp:TemplateColumn >
			<ItemTemplate>
				<asp:Literal id="ltlEditUrl" runat="server" />
			</ItemTemplate>
			<ItemStyle Width="50" cssClass="center" />
		</asp:TemplateColumn>
		<asp:TemplateColumn >
			<ItemTemplate>
				<asp:Literal id="ltlDeleteUrl" runat="server" />
			</ItemTemplate>
			<ItemStyle Width="50" cssClass="center" />
		</asp:TemplateColumn>
		<asp:TemplateColumn
			HeaderText="<input type=&quot;checkbox&quot; onclick=&quot;_checkFormAll(this.checked)&quot;>">
			<ItemTemplate>
			<input type="checkbox" name="AdNameCollection" value='<%#DataBinder.Eval(Container.DataItem, "AdName")%>' />
			</ItemTemplate>
			<ItemStyle Width="20" cssClass="center"/>
		</asp:TemplateColumn>
	</Columns>
  </asp:dataGrid>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="AddAd" OnClick="AddAd_OnClick" Text="添加广告" runat="server" />  
		<input class="btn" type="button" onClick="location.href='background_adSelect.aspx?publishmentSystemID=<%=PublishmentSystemID%>';return false;" value="返 回" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->