<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundInputMainContent" %>

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
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />

        <div class="well well-small">
            <table class="table table-noborder">
                <tr>
                    <td>表单名称：
          <asp:TextBox ID="InputName"
              MaxLength="200"
              Size="37"
              runat="server" />添加时间：从
          <bairong:DateTimeTextBox ID="DateFrom" class="input-small" Columns="12" runat="server" />
                        &nbsp;到&nbsp;
          <bairong:DateTimeTextBox ID="DateTo" class="input-small" Columns="12" runat="server" />
                        <asp:Button class="btn" OnClick="Search_OnClick" ID="Search" Text="搜 索" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:DataGrid ID="dgContents" ShowHeader="true" AutoGenerateColumns="false" DataKeyField="InputID" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" GridLines="none" runat="server">
            <Columns>
                <asp:TemplateColumn
                    HeaderText="提交表单名称">
                    <ItemTemplate>&nbsp;<%#DataBinder.Eval(Container.DataItem,"InputName")%> </ItemTemplate>
                    <ItemStyle HorizontalAlign="left" />
                </asp:TemplateColumn>
                <asp:TemplateColumn
                    HeaderText="需要审核">
                    <ItemTemplate>&nbsp;<%#GetIsCheckedHtml((string)DataBinder.Eval(Container.DataItem,"IsChecked"))%> </ItemTemplate>
                    <ItemStyle Width="60" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn
                    HeaderText="需要回复">
                    <ItemTemplate>&nbsp;<%#GetIsCodeValidateHtml((string)DataBinder.Eval(Container.DataItem,"IsReply"))%> </ItemTemplate>
                    <ItemStyle Width="60" CssClass="center" />
                </asp:TemplateColumn>
                <asp:BoundColumn
                    HeaderText="添加日期"
                    DataField="AddDate"
                    DataFormatString="{0:yyyy-MM-dd}"
                    ReadOnly="true">
                    <ItemStyle Width="70" CssClass="center" />
                </asp:BoundColumn>
                <asp:TemplateColumn HeaderText="上升">
                    <ItemTemplate>
                        <asp:Literal ID="UpLink" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="30" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="下降">
                    <ItemTemplate>
                        <asp:Literal ID="DownLink" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="30" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Literal ID="StyleUrl" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="50" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Literal ID="TemplateUrl" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="60" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Literal ID="MailSMSUrl" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="80" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Literal ID="PreviewUrl" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="25" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Literal ID="EditUrl" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="25" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate><a href="background_inputContent.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>&ItemID=<%# DataBinder.Eval(Container.DataItem,"ClassifyID")%>&InputID=<%# DataBinder.Eval(Container.DataItem,"InputID")%>&InputName=<%# DataBinder.Eval(Container.DataItem,"InputName")%>">数据</a> </ItemTemplate>
                    <ItemStyle Width="25" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Literal ID="ExportUrl" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="25" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Literal ID="DeleteUrl" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="25" CssClass="center" />
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />
        <asp:PlaceHolder ID="PhButton" runat="server">
            <ul class="breadcrumb breadcrumb-button">
                <asp:Button class="btn btn-success" ID="AddInput" Text="添加提交表单" runat="server" />
                <asp:Button class="btn" ID="Import" Text="导入提交表单" runat="server" />
            </ul>
        </asp:PlaceHolder>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
