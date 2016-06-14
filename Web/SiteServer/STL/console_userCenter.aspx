<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.ConsoleUserCenter" EnableViewState="false" %>

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

        <asp:DataGrid ID="dgContents" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" GridLines="none" runat="server">
            <Columns>
                <asp:TemplateColumn HeaderText="用户中心名称">
                    <ItemTemplate>
                        <asp:Literal ID="ltlPublishmentSystemName" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="left" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="应用类型">
                    <ItemTemplate>
                        <asp:Literal ID="ltlPublishmentSystemType" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="110" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="创建日期">
                    <ItemTemplate>
                        <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="70" HorizontalAlign="left" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="整站删除">
                    <ItemTemplate>
                        <asp:Literal ID="ltlDelete" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="60" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="设置默认">
                    <ItemTemplate>
                        <asp:Literal ID="ltlChangeDefault" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="90" CssClass="center" />
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
