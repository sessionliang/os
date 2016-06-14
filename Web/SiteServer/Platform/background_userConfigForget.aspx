<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundUserConfigForget" %>

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
            <h3 class="popover-title">忘记密码设置</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="200">密码找回方式：</td>
                        <td>
                            <asp:CheckBoxList ID="cblPasswordFind" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="cblPasswordFind_SelectedIndexChanged" AutoPostBack="true"></asp:CheckBoxList>
                            <span class="gray">选择多项，控制密码找回方式</span>
                        </td>
                    </tr>
                    <asp:PlaceHolder runat="server" ID="phPhone">
                        <tr>
                            <td>短信验证通知：</td>
                            <td>
                                <asp:TextBox ID="phoneNotice" runat="server" TextMode="MultiLine" Height="200" Width="95%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="phoneNotice" runat="server" ErrorMessage=" *" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="phoneNotice" ErrorMessage=" *" ForeColor="red" Display="Dynamic" ValidationExpression="[^']*\[VerifyCode\][^']*"></asp:RegularExpressionValidator>
                                <br />
                                <span class="gray">当密码找回方式选择手机号，那么系统会根据此格式发送信息。<br />
                                    [UserName]代表账号，[DisplayName]代表姓名，[AddDate]代表当前时间，[VerifyCode]代表验证码，发送内容必须包含[VerifyCode]</span>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phEmail">
                        <tr>
                            <td>邮箱验证标题：</td>
                            <td>
                                <asp:TextBox ID="emailNoticeTitle" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="emailNoticeTitle" ErrorMessage=" *" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>邮箱验证通知：</td>
                            <td>
                                <asp:TextBox ID="emailNotice" runat="server" TextMode="MultiLine" Height="200" Width="95%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="emailNotice" runat="server" ErrorMessage=" *" ForeColor="Red"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="emailNotice" ErrorMessage=" *" ForeColor="red" Display="Dynamic" ValidationExpression="[^']*\[VerifyUrl\][^']*"></asp:RegularExpressionValidator>
                                <br />
                                <span class="gray">当密码找回方式选择邮箱，那么系统会根据此格式发送信息。<br />
                                    [UserName]代表账号，[DisplayName]代表姓名，[AddDate]代表当前时间，[VerifyUrl]代表验证地址，发送内容必须包含[VerifyUrl]</span>
                            </td>
                        </tr>
                   </asp:PlaceHolder>
                   <%--  <tr>
                        <td>是否发送消息：</td>
                        <td>
                            <asp:DropDownList ID="ddlIsSendMsg" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="ddlIsSendMsg_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </td>
                    </tr>
                    <asp:PlaceHolder runat="server" ID="phMessage">
                        <tr>
                            <td>通知消息标题：</td>
                            <td>
                                <asp:TextBox ID="messageTitle" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="messageTitle" ErrorMessage=" *" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>通知消息内容：</td>
                            <td>
                                <asp:TextBox ID="messageContent" runat="server" TextMode="MultiLine" Height="200" Width="95%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="messageContent" runat="server" ErrorMessage=" *" ForeColor="Red"></asp:RequiredFieldValidator>
                                <br />
                                <span class="gray">当密码找回之后，系统会根据此格式发送信息。<br />
                                    [UserName]代表账号，[DisplayName]代表姓名，[AddDate]代表当前时间</span>
                            </td>
                        </tr>
                    </asp:PlaceHolder>--%>
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
