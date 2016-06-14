<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundTemplateList" %>

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
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />

        <asp:DataGrid ID="dgContents" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" runat="server">
            <Columns>
                <asp:TemplateColumn
                    HeaderText="模板文件">
                    <ItemTemplate>
                        &nbsp;<asp:Literal ID="ltlName" runat="server"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
                    </ItemTemplate>
                    <ItemStyle CssClass="center" Width="80" />
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>

        <ul class="breadcrumb breadcrumb-button">
            <input type="button" class="btn" onclick="location.href='background_template.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>    ';" value="返 回" />
        </ul>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
