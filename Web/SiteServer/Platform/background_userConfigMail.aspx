<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundUserConfigMail" %>

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

        <div class="popover popover-static">
            <h3 class="popover-title">邮件设置</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="200">
                            <bairong:Help HelpText="发送邮件的SMTP服务器" Text="SMTP服务器：" runat="server"></bairong:Help>
                        </td>
                        <td>
                            <asp:TextBox Columns="35" MaxLength="200" ID="MailDomain" runat="server" />
                            <asp:RequiredFieldValidator ControlToValidate="MailDomain" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="MailDomain" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" /></td>
                    </tr>
                    <tr>
                        <td width="200">
                            <bairong:Help HelpText="发送邮件的SMTP服务器端口" Text="SMTP端口：" runat="server"></bairong:Help>
                        </td>
                        <td>
                            <asp:TextBox Columns="10" MaxLength="50" Text="24" ID="MailDomainPort" runat="server" />
                            <asp:RequiredFieldValidator ControlToValidate="MailDomainPort" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                            <asp:RegularExpressionValidator
                                ControlToValidate="MailDomainPort"
                                ValidationExpression="\d+"
                                Display="Dynamic"
                                ErrorMessage="SMTP端口必须为大于零的整数"
                                runat="server" />
                            <asp:CompareValidator
                                ControlToValidate="MailDomainPort"
                                Operator="GreaterThan"
                                ValueToCompare="0"
                                Display="Dynamic"
                                ErrorMessage="SMTP端口必须为大于零的整数"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td width="200">
                            <bairong:Help ID="Help1" HelpText="发送邮件中显示的发件人" Text="显示发件人：" runat="server"></bairong:Help>
                        </td>
                        <td>
                            <asp:TextBox Columns="35" MaxLength="50" ID="MailFromName" runat="server" /></td>
                    </tr>
                    <tr>
                        <td width="200">
                            <bairong:Help HelpText="发送邮件的系统邮箱" Text="系统邮箱：" runat="server"></bairong:Help>
                        </td>
                        <td>
                            <asp:TextBox Columns="35" MaxLength="50" ID="MailServerUserName" runat="server" />
                            <%--<asp:RequiredFieldValidator ControlToValidate="MailServerUserName" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />--%>
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="MailServerUserName" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                            <span class="gray">例如：example@domain.com</span>
                        </td>
                    </tr>
                    <tr>
                        <td width="200">
                            <bairong:Help HelpText="发送邮件的系统邮箱密码" Text="系统邮箱密码：" runat="server"></bairong:Help>
                        </td>
                        <td>
                            <asp:TextBox Columns="35" MaxLength="50" TextMode="Password" ID="MailServerPassword" runat="server" />
                            <%--<asp:RequiredFieldValidator ControlToValidate="MailServerPassword" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />--%>
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="MailServerPassword" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" /></td>
                    </tr>
                    <tr>
                        <td width="200">
                            <bairong:Help ID="Help2" HelpText="发送邮件是否SSL" Text="是否SSL：" runat="server"></bairong:Help>
                        </td>
                        <td>
                            <asp:DropDownList Columns="35" MaxLength="50" ID="EnableSsl" runat="server" /></td>
                    </tr>
                    <tr>
                        <td width="200">
                            <bairong:Help HelpText="是否启用" Text="是否启用：" runat="server"></bairong:Help>
                        </td>
                        <td>
                            <asp:RadioButtonList Columns="35" MaxLength="50" ID="MailIsEnabled" runat="server" RepeatDirection="Horizontal" />
                            <span class="gray">设置为是时，在点击修改按钮之后系统会自动检测邮件服务器是否设置正确</span>
                        </td>
                    </tr>
                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" ID="Submit" Text="修 改" OnClick="Submit_OnClick" runat="server" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

        <div class="popover popover-static">
            <h3 class="popover-title">邮件发送测试</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="200">
                            <bairong:Help HelpText="发送测试到此邮箱" Text="邮箱地址：" runat="server"></bairong:Help>
                        </td>
                        <td>
                            <asp:TextBox Columns="35" MaxLength="200" ID="TestMail" runat="server" />
                            <span class="gray">（多个邮箱以“;”分割）</span>
                        </td>
                    </tr>
                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" Text="发 送" OnClick="Send_OnClick" runat="server" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
