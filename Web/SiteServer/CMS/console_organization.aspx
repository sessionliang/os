<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.ConsoleOrganization" %>

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
            <asp:PlaceHolder ID="PlaceHolder_AddChannel" runat="server">
                <asp:Button class="btn btn-success" ID="btnAdd" Text="添加" runat="server" />
                <asp:Button class="btn" ID="btnTaxis" Text="排 序" runat="server" />
            </asp:PlaceHolder>
            <asp:Button class="btn" ID="btnTranslate" Text="转 移" runat="server" />
            <asp:Button class="btn" ID="btnDelete" OnClick="btnDelete_OnClick" Text="删 除" runat="server" />
            <div id="contentSearch" style="margin-top: 10px;">
                区域：
      <asp:DropDownList ID="ddlAreaID" class="input-medium" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
                关键字：
      <asp:TextBox class="input-medium" ID="Keyword" runat="server" />
                <asp:Button class="btn" ID="Search" OnClick="Search_OnClick" Text="搜 索" runat="server" />
            </div>
        </div>
        <table class="table table-bordered table-hover">
            <tr class="info thead">
                <td class="center" style="width: 180px;">名称</td>
                <td class="center" style="width: 80px;">地址</td>
                <td class="center" style="width: 80px;">电话</td>
                <td class="center" style="width: 80px;">区域</td>
                <td class="center" style="width: 80px;">分类</td>
                <td class="center" style="width: 40px;">&nbsp;</td>
                <td class="center" style="width: 40px;">&nbsp;</td>
                <td width="20" class="center">
                    <input onclick="_checkFormAll(this.checked)" type="checkbox" />
                </td>
            </tr>
            <asp:Repeater ID="rptContents" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Literal ID="ItemName" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ItemAddress" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ItemPhone" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ItemArea" runat="server"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="ItemClassify" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ItemEidtRow" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <asp:Literal ID="ItemDelRow" runat="server"></asp:Literal>
                        </td>
                        <td class="center">
                            <input type="checkbox" name="ContentIDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

        <bairong:SqlPager ID="spContents" runat="server" class="table table-pager" />


    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
