<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundSubscribeSet" %>

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

        <div class="popover popover-static">
            <h3 class="popover-title">其他设置</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td style="width: 200px;">邮件内容模板：</td>
                        <td>
                            <asp:DropDownList ID="ddlEmailContentAddress" class="input-medium" Style="width: 300px;" runat="server"></asp:DropDownList>
                            <asp:Button class="btn" ID="tmAddE" Text="新建内容模板" OnClick="Add_OnClick" runat="server" />
                            <br />
                            <span class="gray">系统已经创建[信息订阅邮件内容模板],如需修改，请在【显示管理】的【模板管理】中操作单页模板</span>
                        </td>
                    </tr>
                    <tr>
                        <td>手机内容模板：</td>
                        <td>
                            <asp:DropDownList ID="ddlMobileContentAddress" class="input-medium" Style="width: 300px;" runat="server"></asp:DropDownList>
                            <asp:Button class="btn" ID="tmAddM" Text="新建内容模板" OnClick="Add_OnClick" runat="server" />
                            <br />
                            <span class="gray">系统已经创建[信息订阅手机内容模板],如需修改，请在【显示管理】的【模板管理】中操作单页模板</span>
                        </td>
                    </tr>
                    <tr>
                        <td>推送周期：</td>
                        <td>
                            <asp:RadioButton ID="PushTypeWeek" GroupName="PushType" runat="server" Text="每周" Checked="true" />
                            <asp:TextBox class="input-mini" Columns="50" MaxLength="1" ID="PushWeekDate" runat="server" />
                            <asp:RegularExpressionValidator
                                runat="server"
                                ControlToValidate="PushWeekDate"
                                ValidationExpression="[1-7]"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:RadioButton ID="PushTypeMonth" GroupName="PushType" runat="server" Text="每月" />
                            <asp:TextBox class="input-mini" Columns="50" MaxLength="2" ID="PushMonthDate" runat="server" />
                            <asp:RegularExpressionValidator
                                runat="server"
                                ControlToValidate="PushMonthDate"
                                ValidationExpression="\d+"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic" />
                        </td>
                    </tr>
                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
