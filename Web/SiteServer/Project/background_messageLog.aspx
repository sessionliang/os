<%@ Page Language="C#" Inherits="BRS.BackgroundPages.BackgroundMessageLog" %>
<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.Controls" Assembly="SiteServer.Controls" %>

<html>
<head runat="server">
<link rel="stylesheet" href="../../../../../SiteServer/style.css" type="text/css" />
<script src="../../../../../sitefiles/bairong/Scripts/global.v1.3.5.js"></script>
<bairong:custom type="style" runat="server"></bairong:custom>
<bairong:Code type="Prototype" runat="server" />
<bairong:Code type="Scriptaculous" runat="server" />

<meta http-equiv="content-type" content="text/html;charset=utf-8">
</head>
<body>
<bairong:custom type="showpopwin" runat="server" />
<form id="myForm" runat="server">
<bairong:Message runat="server"></bairong:Message>

<DIV class="column">
<div class="columntitle">短信发送日志</div>
<ASP:DataGrid id="MyDataGrid" runat="server"
			Width="100%"
			Align="center"
			ShowHeader="true"
			ShowFooter="false"
			CellPadding="2"
			BorderWidth="0"
AutoGenerateColumns="false"
			DataKeyField="ID"
			HeaderStyle-CssClass="summary-title"
			ItemStyle-CssClass="tdbg"
			CellSpacing="1" >
		<HeaderStyle HorizontalAlign="center" Height="25" />
		<ItemStyle Height="23"/>
		
		<Columns>
			<asp:TemplateColumn
				HeaderText="用户名">
				<ItemTemplate>
					<%#DataBinder.Eval(Container.DataItem,"UserName")%>
				</ItemTemplate>
				<ItemStyle HorizontalAlign="center" />
			</asp:TemplateColumn>
            <asp:BoundColumn
				HeaderText="短信手机号码"
				DataField="Mobiles"
				ReadOnly="true">
				<ItemStyle Width="100" HorizontalAlign="center" />
			</asp:BoundColumn>
            <asp:BoundColumn
				HeaderText="短信内容"
				DataField="Message"
				ReadOnly="true">
				<ItemStyle Width="100" HorizontalAlign="center" />
			</asp:BoundColumn>
            <asp:BoundColumn
				HeaderText="发送条数"
				DataField="TotalNum"
				ReadOnly="true">
				<ItemStyle Width="60" HorizontalAlign="center" />
			</asp:BoundColumn>
			<asp:BoundColumn
				HeaderText="发送时间"
				DataField="SendDate"
				DataFormatString="{0:yyyy-MM-dd}"
				ReadOnly="true">
				<ItemStyle Width="100" HorizontalAlign="center" />
			</asp:BoundColumn>
			<asp:TemplateColumn >
				<ItemTemplate>
					<a href="messageLog.aspx?Delete=True&ID=<%# DataBinder.Eval(Container.DataItem,"ID")%>" onClick="javascript:return confirm('此操作将删除充值记录，确认吗？');">删除</a>
				</ItemTemplate>
				<ItemStyle Width="50" HorizontalAlign="center" />
			</asp:TemplateColumn>
			<asp:TemplateColumn
				HeaderText="<input type=&quot;checkbox&quot; onclick=&quot;_checkFormAll(this.checked)&quot;>">
				<ItemTemplate>
				<input type="checkbox" name="IDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' />
				</ItemTemplate>
				<ItemStyle Width="20" HorizontalAlign="center"/>
			</asp:TemplateColumn>
		</Columns>
	</ASP:DataGrid>
</DIV>
</form>

</body>
</html>