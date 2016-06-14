<%@ Page Language="C#" ValidateRequest="false" Inherits="BaiRong.BackgroundPages.BackgroundUserSettings" %>

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

        <ul class="nav nav-pills">
            <li id="tab1" class="active"><a href="javascript:;" onclick="_toggleTab(1,3);setCurrentTab(1);">注册设置</a></li>
            <li id="tab2"><a href="javascript:;" onclick="_toggleTab(2,3);setCurrentTab(2);">登录设置</a></li>
            <li id="tab3"><a href="javascript:;" onclick="_toggleTab(3,3);setCurrentTab(3);">忘记密码设置</a></li>
            <input type="hidden" id="hidCurrentTab" name="hidCurrentTab" />
        </ul>
        <script type="text/javascript">
            function setCurrentTab(num) {
                $("#hidCurrentTab").val(num);
            }
        </script>

        <div id="column1" style="display: none" class="popover popover-static">
            <h3 class="popover-title">注册设置</h3>
            <div class="popover-content">
                <table class="table noborder table-hover">
                    <tr>
                        <td width="200">允许新用户注册：</td>
                        <td>
                            <asp:RadioButtonList ID="IsRegisterAllowed" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
                            <span class="gray">选择否将禁止新用户注册, 但不影响过去已注册的会员的使用</span>
                        </td>
                    </tr>
                    <tr>
                        <td>注册用户名最小长度：</td>
                        <td>
                            <asp:TextBox ID="tbRegisterUserNameMinLength" class="input-mini" runat="server"></asp:TextBox>
                            <span class="gray">0代表不限制</span>
                        </td>
                    </tr>
                    <tr>
                        <td>注册密码限制：</td>
                        <td>
                            <asp:DropDownList ID="ddlRegisterPasswordRestriction" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <!--<tr>
                        <td>允许同一Email注册不同用户：</td>
                        <td>
                            <asp:RadioButtonList ID="IsEmailDuplicated" runat="server" RepeatDirection="Horizontal"></asp:RadioButtonList>
                            <span class="gray">如果选择否，一个 Email 地址只能注册一个用户名</span>
                        </td>
                    </tr>-->
                    <tr>
                        <td>用户名称保留关键字：</td>
                        <td>
                            <asp:TextBox ID="ReservedUserNames" TextMode="MultiLine" Width="360" Height="60" runat="server"></asp:TextBox>
                            <br />
                            <span class="gray">使用&ldquo;,&rdquo;分隔多个用户名</span>
                        </td>
                    </tr>
                    <tr>
                        <td>用户注册人工审核：</td>
                        <td>
                            <asp:DropDownList ID="RegisterAuditType" runat="server"></asp:DropDownList>
                            <br />
                            <span class="gray">选择&quot;开启&quot;用户注册需要管理员进行审核;<br />
                                选择&quot;关闭&quot;用户注册不需要管理员审核，但是可以在菜单[消息管理--消息提醒设置]中，选择发送注册验证信息。</span>
                        </td>
                    </tr>
                    <%-- <tr>
                        <td>新用户注册验证：</td>
                        <td>
                            <asp:DropDownList ID="RegisterVerifyType" AutoPostBack="true" OnSelectedIndexChanged="RegisterType_SelectedIndexChanged" runat="server"></asp:DropDownList>
                            <br />
                            <span class="gray">选择&quot;无验证&quot;用户可直接注册成功;选择&quot;Email 验证&quot;将向用户注册 Email 发送一封验证邮件以确认邮箱的有效性;选择&quot;人工审核&quot;将由管理员人工逐个确定是否允许新用户注册</span>
                        </td>
                    </tr>
                    <tr>
                        <td>注册成功欢迎消息内容：</td>
                        <td>
                            <asp:TextBox ID="RegisterWelcome" TextMode="MultiLine" Width="360" Height="60" runat="server"></asp:TextBox></td>
                    </tr>
                    <asp:PlaceHolder ID="phVerifyMailContent" runat="server">
                        <tr>
                            <td>Email验证邮件内容：</td>
                            <td>
                                <asp:TextBox ID="RegisterVerifyMailContent" TextMode="MultiLine" Width="95%" Height="200" runat="server"></asp:TextBox>
                                <br />
                                <span class="gray">[UserName]代表账号，[DisplayName]代表姓名，[AddDate]代表当前时间，[VerifyUrl]代表用户注册验证地址，邮件内容必须包含[VerifyUrl]</span>
                            </td>
                        </tr>
                    </asp:PlaceHolder>--%>
                    <tr>
                        <td>同一IP注册间隔限制：</td>
                        <td>
                            <asp:TextBox class="input-mini" MaxLength="10" ID="tbRegisterMinHoursOfIPAddress" runat="server" />
                            小时
          <asp:RegularExpressionValidator runat="server" ControlToValidate="tbRegisterMinHoursOfIPAddress" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                            <br>
                            <span>同一IP在本时间间隔内将只能注册一个帐号，0 为不限制</span>
                        </td>
                    </tr>
                    <%--<tr>
                        <td>发送欢迎信息：</td>
                        <td>
                            <asp:DropDownList ID="ddlRegisterWelcomeType" AutoPostBack="true" OnSelectedIndexChanged="RegisterType_SelectedIndexChanged" runat="server"></asp:DropDownList>
                            <br>
                            <span>可选择是否自动向新注册用户发送一条欢迎信息</span>
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phRegisterWelcome" runat="server">
                        <tr>
                            <td>欢迎信息标题：</td>
                            <td>
                                <asp:TextBox Columns="60" ID="tbRegisterWelcomeTitle" runat="server" Text="" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbRegisterWelcomeTitle" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                                <br>
                                <span>系统发送的欢迎信息的标题</span>
                            </td>
                        </tr>
                        <tr>
                            <td>欢迎信息内容：</td>
                            <td>
                                <asp:TextBox TextMode="MultiLine" Width="90%" Style="height: 160px;" MaxLength="500" ID="tbRegisterWelcomeContent" runat="server" Text="" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbRegisterWelcomeContent" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                                <br>
                                <span>系统发送的欢迎信息的内容，[UserName]代表账号，[DisplayName]代表姓名，[AddDate]代表当前时间</span>
                            </td>
                        </tr>
                    </asp:PlaceHolder>--%>
                </table>

            </div>
        </div>

        <div id="column2" style="display: none" class="popover popover-static">
            <h3 class="popover-title">登录设置</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="200">用户登录方式：</td>
                        <td>
                            <asp:CheckBoxList ID="cblLoginMethod" runat="server" RepeatDirection="Horizontal"></asp:CheckBoxList>
                            <span class="gray">选择多项，控制登录验证方式</span>
                        </td>
                    </tr>
                    <tr>
                        <td>是否记录登录IP：</td>
                        <td>
                            <asp:RadioButtonList ID="rblIsRecordIP" runat="server" RepeatDirection="Horizontal">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>是否记录登录来源：</td>
                        <td>
                            <asp:RadioButtonList ID="rblIsRecordSource" runat="server" RepeatDirection="Horizontal">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>是否开启失败锁定：</td>
                        <td>
                            <asp:RadioButtonList ID="rblIsFailToLock" OnSelectedIndexChanged="rblIsFailToLock_SelectedIndexChanged" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phFailToLock" runat="server">
                        <tr>
                            <td>失败次数锁定：</td>
                            <td>
                                <asp:TextBox ID="loginFailCount" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="loginFailCount" runat="server" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                <br />
                                <span class="gray">一旦登录失败达到指定次数之后用户就会被锁定</span>
                            </td>
                        </tr>
                        <tr>
                            <td>用户锁定类型：</td>
                            <td>
                                <asp:DropDownList ID="ddlLockType" OnSelectedIndexChanged="ddlLockType_SelectedIndexChanged" runat="server" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <asp:PlaceHolder ID="phLockingTime" runat="server">
                            <tr>
                                <td>锁定时间：</td>
                                <td>
                                    <asp:TextBox ID="lockingTime" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="lockingTime" runat="server" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </asp:PlaceHolder>
                    </asp:PlaceHolder>
                </table>

            </div>
        </div>

        <div id="column3" style="display: none" class="popover popover-static">
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
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="phoneNotice" runat="server" ErrorMessage=" *" ForeColor="Red"></asp:RequiredFieldValidator>
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
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="emailNoticeTitle" ErrorMessage=" *" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>邮箱验证通知：</td>
                            <td>
                                <asp:TextBox ID="emailNotice" runat="server" TextMode="MultiLine" Height="200" Width="95%"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="emailNotice" runat="server" ErrorMessage=" *" ForeColor="Red"></asp:RequiredFieldValidator>
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

            </div>
        </div>

        <hr />
        <table class="table noborder">
            <tr>
                <td class="center">
                    <asp:Button class="btn btn-primary" ID="Submit" Text="修 改" OnClick="Submit_OnClick" runat="server" />
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
