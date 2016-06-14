<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundMLibManageSite" %>

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
        <bairong:Alerts runat="server" Text="用户的投稿范围为稿件发布者所属角色下有权限的站点及栏目；站点栏目的审核规则为稿件的审核规则，通过设置站点栏目的审核规则来控制稿件的审核" />

        <div class="popover popover-static">
            <h3 class="popover-title">投稿启用设置</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td style="width: 200px;">是否启用用户中心投稿：</td>
                        <td>
                            <asp:RadioButtonList ID="IsUseMLib" RepeatDirection="Horizontal" OnSelectedIndexChanged="IsUseMLib_SelectedIndexChanged" class="radiobuttonlist" AutoPostBack="true" runat="server"></asp:RadioButtonList>
                            <span class="gray">启用用户投稿，用户中心才会显示投稿菜单</span>
                        </td>
                    </tr>
                </table>

                <asp:PlaceHolder ID="phPublishmentSystem" runat="server">
                    <hr />
                    <table class="table noborder">
                        <tr class="info">
                            <td colspan="2">投稿基本设置</td>
                        </tr>
                        <tr>
                            <td style="width: 200px;">稿件发布者：</td>
                            <td>
                                <asp:TextBox MaxLength="50" class="input-small" ID="tbUnifiedMLibAddUser" Text="0" runat="server" />
                                <asp:RequiredFieldValidator
                                    ControlToValidate="tbUnifiedMLibAddUser"
                                    ErrorMessage=" *" ForeColor="red"
                                    Display="Dynamic"
                                    runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="cbUnifiedMLibAddUser" runat="server" />允许用户组单独设置 
                                <br />
                                <span>因稿件发布者所有拥有的站点栏目权限为用户投稿范围，请输入拥有站点及栏目权限的管理员账号，所有用户的稿件以同一管理员身份发布投递的稿件</span><br />
                                <span>同时用户的投稿范围为稿件发布者所属角色下有权限的站点及栏目</span><br />
                                <span>如果勾选了[允许用户组单独设置]，则可在用户组管理菜单中给每个用户组设置不同的稿件发布者</span>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px;">用户投稿有效期：</td>
                            <td>
                                <asp:TextBox MaxLength="4" class="input-small" ID="tbMLibValidityDate" Text="0" runat="server" />
                                <asp:RequiredFieldValidator
                                    ControlToValidate="tbMLibValidityDate"
                                    ErrorMessage=" *" ForeColor="red"
                                    Display="Dynamic"
                                    runat="server" />
                                <asp:RegularExpressionValidator
                                    runat="server"
                                    ControlToValidate="tbMLibValidityDate"
                                    ValidationExpression="[0-9]+"
                                    ErrorMessage=" *" ForeColor="red"
                                    Display="Dynamic" />
                                月 &nbsp;
                                        <asp:CheckBox ID="cbUnifiedMLibValidityDate" runat="server" />允许用户组单独设置 
                                <br />
                                <span>设置所有用户的投稿有效期,0表示不限时间</span>
                                <br />
                                <span>如果勾选了[允许用户组单独设置]，则可在用户组管理菜单中给每个用户组设置不同的投稿有效期</span>
                                <br />
                                <span>另在用户管理中可单独给每个用户设置投稿有效期</span>
                            </td>
                        </tr>
                        <asp:PlaceHolder ID="phUnifiedMLibNum" runat="server">
                            <tr>
                                <td style="width: 200px;">用户可投稿数量：</td>
                                <td>
                                    <asp:TextBox class="input-small" MaxLength="8" ID="tbUnifiedMlibNum" Text="0" runat="server" />
                                    <asp:RequiredFieldValidator
                                        ControlToValidate="tbUnifiedMlibNum"
                                        ErrorMessage=" *" ForeColor="red"
                                        Display="Dynamic"
                                        runat="server" />
                                <asp:RegularExpressionValidator
                                    runat="server"
                                    ControlToValidate="tbUnifiedMlibNum"
                                    ValidationExpression="[0-9]+"
                                    ErrorMessage=" *" ForeColor="red"
                                    Display="Dynamic" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:CheckBox ID="cbUnifiedMLibNum" Style="margin-bottom: 20px;" runat="server" />允许用户组单独设置 
                                    <br />
                                    <span>设置所有用户可投稿数量，0表示不限数量，且已删除稿件不记数</span>
                                    <br />
                                    <span>如果勾选了[允许用户组单独设置]，则可在用户组管理菜单中给每个用户组设置不同的投稿数量</span>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="false">
                            <tr>
                                <td style="width: 200px;">稿件审核方式：</td>
                                <td>
                                    <asp:RadioButtonList ID="rblMLibCheckType" RepeatDirection="Horizontal" class="radiobuttonlist" runat="server"></asp:RadioButtonList>
                                    <span class="gray">以栏目为纬度：可以在设置投稿范围时给栏目单独设置；以用户组为纬度：可以在用户组管理中单独设置</span>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                    </table>
                </asp:PlaceHolder>
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
