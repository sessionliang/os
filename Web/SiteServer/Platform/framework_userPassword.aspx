<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.FrameworkUserPassword" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <!--#include file="../inc/header.aspx"-->
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->
    <form class="form-inline" runat="server" autocomplete="off">
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />

        <ul class="nav nav-pills">
            <li><a href="framework_userTheme.aspx">
                <lan>主题设置</lan>
            </a></li>
            <li><a href="framework_userLanguage.aspx">
                <lan>语言设置</lan>
            </a></li>
            <li><a href="framework_userProfile.aspx">
                <lan>修改资料</lan>
            </a></li>
            <li class="active"><a href="framework_userPassword.aspx">
                <lan>更改密码</lan>
            </a></li>
        </ul>

        <div class="popover popover-static">
            <h3 class="popover-title">
                <lan>更改密码</lan>
            </h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="150">管理员登录名：</td>
                        <td>
                            <asp:Literal ID="UserName" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td>当前密码：</td>
                        <td>
                            <!--防止表单的自动填充功能-->
                            <input type="password" style="display: none" />
                            <!--防止表单的自动填充功能-->
                            <asp:TextBox ID="CurrentPassword" runat="server" MaxLength="50" Size="20" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="CurrentPassword" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>新密码：</td>
                        <td>
                            <asp:TextBox ID="NewPassword" runat="server" MaxLength="50" Size="20" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="NewPassword" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                runat="server"
                                ControlToValidate="NewPassword"
                                ValidationExpression="[^']+"
                                ErrorMessage="不能输入单引号"
                                Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td>重复输入新密码：</td>
                        <td>
                            <asp:TextBox ID="ConfirmNewPassword" runat="server" TextMode="Password" MaxLength="50" Size="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ControlToValidate="ConfirmNewPassword" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                            <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword" Display="Dynamic" ForeColor="red" ErrorMessage=" 两次输入的新密码不一致！请再输入一遍您上面填写的新密码。"></asp:CompareValidator>
                        </td>
                    </tr>
                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" OnClick="Submit_Click" runat="server" Text="修 改" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
