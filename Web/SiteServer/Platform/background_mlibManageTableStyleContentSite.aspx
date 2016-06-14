<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundMLibManageTableStyleContentSite" %>

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
                    <td width="60px">站点：</td>
                    <td width="200px">
                        <asp:DropDownList ID="ddlPublishmentSystem" OnSelectedIndexChanged="ddlPublishmentSystem_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                    </td>
                    <td width="60px">栏目：</td>
                    <td>
                        <asp:DropDownList ID="NodeIDDropDownList" OnSelectedIndexChanged="NodeIDDropDownList_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                    </td> 
                </tr>
            </table>
        </div>

        <asp:DataGrid ID="dgContents" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" GridLines="none" runat="server">
            <Columns>
                <asp:TemplateColumn
                    HeaderText="字段名">
                    <ItemTemplate>
                        <asp:Literal ID="ltlAttributeName" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="140" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="显示名称">
                    <ItemTemplate>
                        <asp:Literal ID="ltlDisplayName" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="140" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="表单提交类型">
                    <ItemTemplate>
                        <asp:Literal ID="ltlInputType" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="120" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="字段类型">
                    <ItemTemplate>
                        <asp:Literal ID="ltlFieldType" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="120" HorizontalAlign="left" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="后台启用">
                    <ItemTemplate>
                        <asp:Literal ID="ltlIsVisible" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="50" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="验证规则">
                    <ItemTemplate>
                        <asp:Literal ID="ltlValidate" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="100" HorizontalAlign="left" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="上升">
                    <ItemTemplate>
                        <asp:HyperLink ID="UpLinkButton" runat="server"><img src="../Pic/icon/up.gif" border="0" alt="上升" /></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle Width="40" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="下降">
                    <ItemTemplate>
                        <asp:HyperLink ID="DownLinkButton" runat="server"><img src="../Pic/icon/down.gif" border="0" alt="下降" /></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle Width="40" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="显示样式" Visible="false">
                    <ItemTemplate>
                        <asp:Literal ID="ltlEditStyle" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="120" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="表单验证" Visible="false">
                    <ItemTemplate>
                        <asp:Literal ID="ltlEditValidate" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="80" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="是否启用">
                    <ItemTemplate>
                        <asp:Literal ID="ltlAttributeNameCollection" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle Width="50" CssClass="center" />
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>

        <ul class="breadcrumb breadcrumb-button">
            <asp:Button class="btn btn-success" ID="SaveStyle" Text="保存所选字段" OnClick="Submit_OnClick" runat="server" />
            <asp:Button class="btn" ID="AddStyle" Text="新增虚拟字段" runat="server" />
        </ul>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
