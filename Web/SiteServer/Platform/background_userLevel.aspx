<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundUserLevel" %>

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

        <bairong:Alerts Text="总积分是衡量用户级别的唯一标准，你可以在此设定用户的总积分计算公式。" runat="server"></bairong:Alerts>

        <div class="popover popover-static">
            <h3 class="popover-title">总积分计算公式</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td><strong>总积分计算公式:</strong>&nbsp;&nbsp;
        <asp:Literal ID="ltlCreditCalculate" runat="server"></asp:Literal></td>
                    </tr>
                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button ID="SetButton" class="btn btn-success" Text="设置总积分计算公式" runat="server" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

        <div class="popover popover-static">
            <h3 class="popover-title">用户等级</h3>
            <div class="popover-content">

                <div class="tips">用户等级以积分或币计算公式确定等级和权限，系统将根据积分或币计算公式判断所在级别并赋予相应的阅读及操作权限。 </div>

                <asp:DataGrid ID="UserLevelDataGrid" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" runat="server">
                    <Columns>
                        <asp:TemplateColumn HeaderText="等级名称">
                            <ItemTemplate>
                                <asp:Literal ID="ltlLevelName" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle CssClass="center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="最小值">
                            <ItemTemplate>
                                <asp:Literal ID="ltlMinNum" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle Width="70" HorizontalAlign="left" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="最大值">
                            <ItemTemplate>
                                <asp:Literal ID="ltlMaxNum" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle Width="70" HorizontalAlign="left" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="等级图标">
                            <ItemTemplate>
                                <asp:Literal ID="ltlStars" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle Width="70" HorizontalAlign="left" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle Width="70" CssClass="center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:Literal ID="ltlPermissionUrl" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle Width="100" CssClass="center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle Width="70" CssClass="center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button ID="UserLevelAddButton" class="btn btn-success" Text="添加等级" runat="server" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

        <div class="popover popover-static">
            <h3 class="popover-title">系统用户等级</h3>
            <div class="popover-content">

                <div class="tips">系统用户等级是系统预定义的用户等级，预留了从管理员到游客的 8 个系统等级。 </div>

                <asp:DataGrid ID="SysLevelDataGrid" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" runat="server">
                    <Columns>
                        <asp:TemplateColumn HeaderText="等级名称">
                            <ItemTemplate>
                                <asp:Literal ID="ltlLevelName" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle CssClass="center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn HeaderText="等级图标">
                            <ItemTemplate>
                                <asp:Literal ID="ltlStars" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle Width="70" HorizontalAlign="left" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle Width="70" CssClass="center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:Literal ID="ltlPermissionUrl" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle Width="100" CssClass="center" />
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <ItemTemplate>
                                <asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
                            </ItemTemplate>
                            <ItemStyle Width="70" CssClass="center" />
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
            </div>
        </div>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
