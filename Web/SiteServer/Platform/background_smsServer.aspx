<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundSMSServer" %>

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

        <asp:DataGrid ID="dgContents" runat="server" ShowHeader="true"
            ShowFooter="false"
            AutoGenerateColumns="false"
            AllowPaging="true"
            DataKeyField="SMSServerEName"
            OnPageIndexChanged="dgContents_Page"
            HeaderStyle-CssClass="info thead"
            CssClass="table table-bordered table-hover"
            GridLines="none">
            <PagerStyle Mode="NumericPages" PageButtonCount="10" />
            <Columns>
                <asp:BoundColumn
                    HeaderText="标记"
                    DataField="SMSServerEName">
                    <ItemStyle Width="130" HorizontalAlign="left" />
                </asp:BoundColumn>
                <asp:BoundColumn
                    HeaderText="名称"
                    DataField="SMSServerName"></asp:BoundColumn>
                <asp:TemplateColumn>
                    <ItemTemplate><a href='background_smsServerAdd.aspx?SMSServerEName=<%# DataBinder.Eval(Container.DataItem,"SMSServerEName")%>'>修改</a> </ItemTemplate>
                    <ItemStyle Width="80" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate><a href='background_smsServer.aspx?Open=True&SMSServerEName=<%# DataBinder.Eval(Container.DataItem,"SMSServerEName")%>'><asp:Literal ID="ltlStatue" runat="server"></asp:Literal></a> </ItemTemplate>
                    <ItemStyle Width="80" CssClass="center" />
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate><a href='background_smsServer.aspx?Delete=True&SMSServerEName=<%# DataBinder.Eval(Container.DataItem,"SMSServerEName")%>' onclick="javascript:return confirm('此操作将会删除角色“<%# DataBinder.Eval(Container.DataItem,"SMSServerEName")%>”，确认吗？');">删除</a> </ItemTemplate>
                    <ItemStyle Width="50" CssClass="center" />
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>

        <table class="table table-noborder">
            <tr>
                <td>
                    <asp:LinkButton ID="pageFirst" OnClick="NavigationButtonClick" CommandName="FIRST" runat="server">首页</asp:LinkButton>
                    |
        <asp:LinkButton ID="pagePrevious" OnClick="NavigationButtonClick" CommandName="PREVIOUS" runat="server">前页</asp:LinkButton>
                    |
        <asp:LinkButton ID="pageNext" OnClick="NavigationButtonClick" CommandName="NEXT" runat="server">后页</asp:LinkButton>
                    |
        <asp:LinkButton ID="pageLast" OnClick="NavigationButtonClick" CommandName="LAST" runat="server">尾页</asp:LinkButton></td>
                <td class="align-right">分页
        <asp:Label Enabled="False" runat="server" ID="currentPage" /></td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
