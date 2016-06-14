<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundInput" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
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

  <asp:dataGrid id="dgContents" showHeader="true" AutoGenerateColumns="false" DataKeyField="InputID" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" gridlines="none" runat="server">
    <Columns>
      <asp:TemplateColumn
        HeaderText="提交表单名称">
        <ItemTemplate> &nbsp;<%#DataBinder.Eval(Container.DataItem,"InputName")%> </ItemTemplate>
        <ItemStyle HorizontalAlign="left" />
      </asp:TemplateColumn>
      <asp:TemplateColumn
        HeaderText="需要审核">
        <ItemTemplate> &nbsp;<%#GetIsCheckedHtml((string)DataBinder.Eval(Container.DataItem,"IsChecked"))%> </ItemTemplate>
        <ItemStyle Width="60" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn
        HeaderText="需要回复">
        <ItemTemplate> &nbsp;<%#GetIsCodeValidateHtml((string)DataBinder.Eval(Container.DataItem,"IsReply"))%> </ItemTemplate>
        <ItemStyle Width="60" cssClass="center" />
      </asp:TemplateColumn>
      <asp:BoundColumn
        HeaderText="添加日期"
        DataField="AddDate"
        DataFormatString="{0:yyyy-MM-dd}"
        ReadOnly="true">
        <ItemStyle Width="70" cssClass="center" />
      </asp:BoundColumn>
      <asp:TemplateColumn HeaderText="上升">
        <ItemTemplate>
          <asp:Literal ID="UpLink" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="50" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn HeaderText="下降">
        <ItemTemplate>
          <asp:Literal ID="DownLink" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="50" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn >
        <ItemTemplate>
          <asp:Literal ID="StyleUrl" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="70" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate>
          <asp:Literal ID="TemplateUrl" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="70" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate>
          <asp:Literal ID="MailSMSUrl" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="80" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate>
          <asp:Literal ID="PreviewUrl" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="50" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate>
          <asp:Literal ID="EditUrl" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="50" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate>
          <asp:Literal ID="ExportUrl" runat="server"></asp:Literal>
        </ItemTemplate>
        <ItemStyle Width="50" cssClass="center" />
      </asp:TemplateColumn>
      <asp:TemplateColumn>
        <ItemTemplate> <a href="background_input.aspx?Delete=True&PublishmentSystemID=<%=base.PublishmentSystemID%>&InputID=<%# DataBinder.Eval(Container.DataItem,"InputID")%>" onClick="javascript:return confirm('此操作将删除提交表单“<%# DataBinder.Eval(Container.DataItem,"InputName")%>”及相关数据，确认吗？');">删除</a> </ItemTemplate>
        <ItemStyle Width="50" cssClass="center" />
      </asp:TemplateColumn>
    </Columns>
  </asp:dataGrid>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="AddInput" Text="添加提交表单" runat="server" />
    <asp:Button class="btn" id="Import" Text="导入提交表单" runat="server" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->