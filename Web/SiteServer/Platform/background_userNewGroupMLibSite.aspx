<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundUserNewGroupMLibSite" %>

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
            <h3 class="popover-title">用户组投稿设置</h3>
            <div class="popover-content">
                <table class="table noborder">
                    <tr>
                        <td style="width: 200px;">当前用户组：</td>
                        <td>
                            <asp:Label ID="lbGroupName" runat="server"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr class="info">
                        <td colspan="2">用户中心已启用投稿系统，请设置当前用户组有关投稿的设置，以投稿基本设置为默认值（规则与投稿基本设置一致，优先级为用户组设置 > 投稿基本设置）</td>
                    </tr>
                    <asp:PlaceHolder ID="phMLibAddUser" runat="server" Visible="false">
                        <tr>
                            <td style="width: 200px;">稿件发布者：</td>
                            <td>
                                <asp:TextBox MaxLength="50" class="input-small" ID="tbMLibAddUser" Text="0" runat="server" />
                                <asp:RequiredFieldValidator
                                    ControlToValidate="tbMLibAddUser"
                                    ErrorMessage=" *" ForeColor="red"
                                    Display="Dynamic"
                                    runat="server" />
                                <br />
                                <span>因稿件发布者所有拥有的站点栏目权限为用户投稿范围，请输入拥有站点及栏目权限的管理员账号，所有用户的稿件以同一管理员身份发布投递的稿件</span><br />
                                <span>同时此用户组下所有用户的投稿范围为稿件发布者所属角色下有权限的站点及栏目</span><br />
                            </td>
                        </tr>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phMLibValidityDate" runat="server" Visible="false">
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
                                    Display="Dynamic" />月
                                <span>设置此用户组下所有用户的投稿有效期,0表示不限时间</span><br />
                                <span>另在用户管理中可单独给每个用户设置投稿有效期</span>
                            </td>
                        </tr>
                    </asp:PlaceHolder>

                    <asp:PlaceHolder ID="phMlibNum" runat="server" Visible="false">
                        <tr>
                            <td style="width: 200px;">稿件可投递数量：</td>
                            <td>
                                <asp:TextBox class="input-small" MaxLength="8" ID="MlibNum" Text="0" runat="server" />
                                <asp:RequiredFieldValidator
                                    ControlToValidate="MlibNum"
                                    ErrorMessage=" *" ForeColor="red"
                                    Display="Dynamic"
                                    runat="server" />
                                <asp:RegularExpressionValidator
                                    runat="server"
                                    ControlToValidate="MlibNum"
                                    ValidationExpression="[0-9]+"
                                    ErrorMessage=" *" ForeColor="red"
                                    Display="Dynamic" />
                                <span>设置此用户组下所有用户的可投稿数量，0表示不限数量，且已删除稿件不记数</span>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="false">
                        <asp:PlaceHolder ID="phIsMLibCheck" runat="server" Visible="false">
                            <tr>
                                <td style="width: 200px;">是否需要审核：</td>
                                <td>
                                    <asp:RadioButtonList ID="IsMLibCheck" RepeatDirection="Horizontal" class="radiobuttonlist" runat="server"></asp:RadioButtonList>
                                    <span>当前用户组下的用户投递的稿件是否需要审核</span>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td style="width: 200px;">使用投稿设置的投稿范围：</td>
                            <td>
                                <asp:RadioButtonList ID="rblIsUseMLibScope" OnSelectedIndexChanged="rblIsUseMLibScope_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" class="radiobuttonlist" runat="server"></asp:RadioButtonList>
                                <span>使用则无需再设置用户的可投稿范围，否则需要设置用户组下用户的可投稿范围，如果未设置，则用户组下的用户不可投稿，但是可保存到草稿箱</span>
                            </td>
                        </tr>
                        <asp:PlaceHolder ID="phMLibScope" runat="server" Visible="false">
                            <tr class="info">
                                <td colspan="2">请设置稿件可投递的站点和栏目：点击下列链接，进入对应站点的栏目范围设置界面</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Literal ID="ltlPublishmentSystems" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <span>注：点击网站选择稿件允许投递的栏目&nbsp;&nbsp;<img src="../pic/canedit.gif" align="absmiddle" />稿件可投递的网站&nbsp;
                                            <img src="../pic/cantedit.gif" align="absmiddle" />不在投递范围的网站</span>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                    </asp:PlaceHolder>
                </table>
                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />

                            <input type="button" class="btn" value="返 回" onclick="javascript: location.href = 'background_userNewGroup.aspx?PublishmentSystemID=<%= base.PublishmentSystemID%>';" />

                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
