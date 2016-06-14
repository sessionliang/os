<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.CMS.BackgroundPages.BackgroundWebsiteMessageSetting" %>

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
                <table class="table table-noborder table-hover">
                    <tr>
                        <td width="200">是否需要审核：</td>
                        <td>
                            <asp:RadioButtonList ID="IsChecked" RepeatDirection="Horizontal" class="noborder" runat="server">
                                <asp:ListItem Text="需要审核" Value="False"></asp:ListItem>
                                <asp:ListItem Text="不需要审核" Value="True" Selected="true"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>是否需要回复：</td>
                        <td>
                            <asp:RadioButtonList ID="IsReply" RepeatDirection="Horizontal" class="noborder" runat="server">
                                <asp:ListItem Text="需要回复" Value="True"></asp:ListItem>
                                <asp:ListItem Text="不需要回复" Value="False" Selected="true"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>提交成功提示信息：</td>
                        <td>
                            <asp:TextBox Columns="45" Rows="3" TextMode="MultiLine" MaxLength="50" ID="MessageSuccess" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>提交失败提示信息：</td>
                        <td>
                            <asp:TextBox Columns="45" Rows="3" TextMode="MultiLine" MaxLength="50" ID="MessageFailure" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>允许匿名提交：</td>
                        <td>
                            <asp:RadioButtonList ID="IsAnomynous" RepeatDirection="Horizontal" class="noborder" runat="server" OnSelectedIndexChanged="IsAnomynous_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="允许" Value="True" Selected="true"></asp:ListItem>
                                <asp:ListItem Text="不允许" Value="False"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phLoginUrl" runat="server">
                        <tr>
                            <td>登录地址：</td>
                            <td>
                                <asp:TextBox ID="tbLoginUrl" runat="server"></asp:TextBox>
                                <span class="gray">当设置不允许匿名提交的时候，用户在未登录的情况下提交信息，系统会提示，请先登录。这里设置的就是提示信息中的登录地址。该地址已@开头表示在本站点文件夹中。</span>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <td>显示验证码：</td>
                        <td>
                            <asp:RadioButtonList ID="IsValidateCode" RepeatDirection="Horizontal" class="noborder" runat="server">
                                <asp:ListItem Text="显示" Value="True" Selected="true"></asp:ListItem>
                                <asp:ListItem Text="不显示" Value="False"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>表单提交成功后是否隐藏：</td>
                        <td>
                            <asp:RadioButtonList ID="IsSuccessHide" RepeatDirection="Horizontal" class="noborder" runat="server">
                                <asp:ListItem Text="隐藏" Value="True" Selected="true"></asp:ListItem>
                                <asp:ListItem Text="不隐藏" Value="False"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>表单提交成功后是否刷新页面：</td>
                        <td>
                            <asp:RadioButtonList ID="IsSuccessReload" RepeatDirection="Horizontal" class="noborder" runat="server">
                                <asp:ListItem Text="刷新" Value="True"></asp:ListItem>
                                <asp:ListItem Text="不刷新" Value="False" Selected="true"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>启用Ctrl+Enter快速提交：</td>
                        <td>
                            <asp:RadioButtonList ID="IsCtrlEnter" RepeatDirection="Horizontal" class="noborder" runat="server">
                                <asp:ListItem Text="启用" Value="True" Selected="true"></asp:ListItem>
                                <asp:ListItem Text="不启用" Value="False"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <div class="popover popover-static">
            <h3 class="popover-title">邮件/短信发送设置</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr style="display: none;">
                        <td width="155">回复留言名称：</td>
                        <td width="260">
                            <asp:Literal ID="ltlWebsiteMessageName" runat="server"></asp:Literal></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td width="155">是否发送邮件： </td>
                        <td>
                            <asp:RadioButtonList ID="rblIsMail" AutoPostBack="true" OnSelectedIndexChanged="rblIsMail_SelectedIndexChanged" RepeatDirection="Horizontal" class="noborder" runat="server">
                                <asp:ListItem Text="发送邮件" Value="True"></asp:ListItem>
                                <asp:ListItem Text="不发送邮件" Value="False" Selected="true"></asp:ListItem>
                            </asp:RadioButtonList></td>
                        <td class="gray">设置回复内容后是否需要发送邮件提醒</td>
                    </tr>
                    <asp:PlaceHolder ID="phMail" Visible="false" runat="server">
                        <tr>
                            <td width="155">邮件接收人： </td>
                            <td>
                                <asp:RadioButtonList ID="rblMailReceiver" AutoPostBack="true" OnSelectedIndexChanged="rblMailReceiver_SelectedIndexChanged" runat="server">
                                    <asp:ListItem Text="指定邮箱" Value="True" Selected="true"></asp:ListItem>
                                    <asp:ListItem Text="留言回复者" Value="False"></asp:ListItem>
                                    <asp:ListItem Text="留言回复者及指定邮箱" Value="All"></asp:ListItem>
                                </asp:RadioButtonList></td>
                            <td class="gray">设置邮件提醒的收信人</td>
                        </tr>
                        <asp:PlaceHolder ID="phMailTo" runat="server">
                            <tr>
                                <td>指定邮箱地址： </td>
                                <td>
                                    <asp:TextBox Columns="35" MaxLength="50" ID="tbMailTo" runat="server" /></td>
                                <td class="gray">多个邮箱用";"分隔</td>
                            </tr>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phMailFiledName" runat="server">
                            <tr>
                                <td>回复留言邮箱字段： </td>
                                <td>
                                    <asp:DropDownList ID="ddlMailFiledName" runat="server"></asp:DropDownList></td>
                                <td class="gray">设置回复留言的邮箱字段，系统将向此字段的邮箱发送邮件</td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td>邮件标题： </td>
                            <td>
                                <asp:TextBox Columns="35" MaxLength="50" ID="tbMailTitle" Text="邮件提醒" runat="server" />
                                <asp:RequiredFieldValidator ControlToValidate="tbMailTitle" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" /></td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td width="155">自定义邮件发送内容：</td>
                            <td>
                                <asp:RadioButtonList ID="rblIsMailTemplate" AutoPostBack="true" OnSelectedIndexChanged="rblIsMailTemplate_SelectedIndexChanged" RepeatDirection="Horizontal" class="noborder" runat="server">
                                    <asp:ListItem Text="自定义内容" Value="True"></asp:ListItem>
                                    <asp:ListItem Text="使用系统默认内容" Value="False" Selected="true"></asp:ListItem>
                                </asp:RadioButtonList></td>
                            <td class="gray">设置是否自定义邮件发送内容</td>
                        </tr>
                        <asp:PlaceHolder ID="phMailTemplate" Visible="false" runat="server">
                            <tr>
                                <td width="155">邮件发送内容：</td>
                                <td colspan="2">
                                    <asp:TextBox Width="90%" TextMode="MultiLine" ID="tbMailContent" runat="server" Rows="10" Wrap="false" Text="" />
                                    <br>
                                    <span class="gray">（
                <asp:Literal ID="ltlTips1" runat="server"></asp:Literal>
                                        ）</span></td>
                            </tr>
                        </asp:PlaceHolder>
                    </asp:PlaceHolder>
                    <tr>
                        <td colspan="3">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td width="155">是否发送短信： </td>
                        <td>
                            <asp:RadioButtonList ID="rblIsSMS" AutoPostBack="true" OnSelectedIndexChanged="rblIsSMS_SelectedIndexChanged" RepeatDirection="Horizontal" class="noborder" runat="server">
                                <asp:ListItem Text="发送短信" Value="True"></asp:ListItem>
                                <asp:ListItem Text="不发送短信" Value="False" Selected="true"></asp:ListItem>
                            </asp:RadioButtonList></td>
                        <td class="gray">设置回复内容后是否需要发送短信提醒</td>
                    </tr>
                    <asp:PlaceHolder ID="phSMS" Visible="false" runat="server">
                        <tr>
                            <td width="155">短信接收人： </td>
                            <td>
                                <asp:RadioButtonList ID="rblSMSReceiver" AutoPostBack="true" OnSelectedIndexChanged="rblSMSReceiver_SelectedIndexChanged" runat="server">
                                    <asp:ListItem Text="指定手机" Value="True" Selected="true"></asp:ListItem>
                                    <asp:ListItem Text="留言回复者" Value="False"></asp:ListItem>
                                    <asp:ListItem Text="留言回复者及指定手机" Value="All"></asp:ListItem>
                                </asp:RadioButtonList></td>
                            <td class="gray">设置短信提醒的收信人</td>
                        </tr>
                        <asp:PlaceHolder ID="phSMSTo" runat="server">
                            <tr>
                                <td>指定手机号码： </td>
                                <td>
                                    <asp:TextBox Columns="35" MaxLength="50" ID="tbSMSTo" runat="server" /></td>
                                <td class="gray">多个手机号码用";"分隔</td>
                            </tr>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phSMSFiledName" runat="server">
                            <tr>
                                <td>回复留言手机字段： </td>
                                <td>
                                    <asp:DropDownList ID="ddlSMSFiledName" runat="server"></asp:DropDownList></td>
                                <td class="gray">设置回复留言的手机字段，系统将向此字段的手机号码发送短信</td>
                            </tr>
                        </asp:PlaceHolder>
                        <tr>
                            <td width="155">自定义短信发送内容：</td>
                            <td>
                                <asp:RadioButtonList ID="rblIsSMSTemplate" AutoPostBack="true" OnSelectedIndexChanged="rblIsSMSTemplate_SelectedIndexChanged" RepeatDirection="Horizontal" class="noborder" runat="server">
                                    <asp:ListItem Text="自定义内容" Value="True"></asp:ListItem>
                                    <asp:ListItem Text="使用系统默认内容" Value="False" Selected="true"></asp:ListItem>
                                </asp:RadioButtonList></td>
                            <td class="gray">设置是否自定义短信发送内容</td>
                        </tr>
                        <asp:PlaceHolder ID="phSMSTemplate" Visible="false" runat="server">
                            <tr>
                                <td width="155">短信发送内容：</td>
                                <td colspan="2">
                                    <asp:TextBox Width="90%" TextMode="MultiLine" ID="tbSMSContent" runat="server" Rows="10" Wrap="false" Text="" />
                                    <br>
                                    <span class="gray">（
                <asp:Literal ID="ltlTips2" runat="server"></asp:Literal>
                                        ）</span></td>
                            </tr>
                        </asp:PlaceHolder>
                    </asp:PlaceHolder>
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
