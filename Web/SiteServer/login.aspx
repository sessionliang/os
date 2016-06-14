<%@ Page Language="c#" Inherits="BaiRong.BackgroundPages.FrameworkLogin" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>管理员登录</title>
    <bairong:Code Type="JQuery" runat="server" />
    <bairong:Code Type="bootstrap" runat="server" />
    <bairong:Code Type="html5shiv" runat="server" />
    <script language="JavaScript">
        if (window.top != self) {
            window.top.location = self.location;
        }
        $(document).ready(function () { $('#UserName').focus(); });
    </script>
    <link href="css/login.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="js/public.js"></script>
    <!--[if IE 6]>
<script src="js/ie6PNG.js" type="text/javascript"></script>
<script type="text/javascript">DD_belatedPNG.fix('*');</script>
<![endif]-->
</head>
<body class="yunBg">
    <form class="form-inline" runat="server" autocomplete="off">
        <div class="yunMain">
            <div class="yunTop">
                <asp:Literal ID="ltlLogo" runat="server" />
            </div>
            <asp:Literal ID="ltlMessage" runat="server" />
            <div class="yunItmName">
                <img src="pic/login/yun_ico1.jpg" width="31" height="32" /><span class="yunItmS">管理员登录</span>
            </div>
            <div class="yunBox">
                <div class="yun_u1">
                    <ul>
                        <li><span class="yun_s1">用户名：</span>

                            <asp:TextBox class="yun_int1" ID="UserName" runat="server" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator"
                                ControlToValidate="UserName"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic"
                                runat="server" />
                            <asp:RegularExpressionValidator
                                runat="server"
                                ControlToValidate="UserName"
                                ValidationExpression="[^']+"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic" />

                        </li>
                        <li><span class="yun_s1">密码：</span>
                            <!--防止表单的自动填充功能-->
                            <input type="password" style="display:none" />
                            <!--防止表单的自动填充功能-->
                            <asp:TextBox class="yun_int1" ID="Password" TextMode="Password" runat="server" />
                            <asp:RequiredFieldValidator
                                ControlToValidate="Password" ID="RequiredFieldValidator1"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic"
                                runat="server" />
                            <asp:RegularExpressionValidator
                                runat="server"
                                ControlToValidate="Password"
                                ValidationExpression="[^']+"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic" />

                        </li>
                        <asp:PlaceHolder ID="phValidateCode" runat="server">
                            <li><span class="yun_s1">验证码：</span>

                                <asp:TextBox class="yun_int1 yun_int2" ID="ValidateCode" runat="server" />
                                <asp:Literal ID="ValidateCodeImage" runat="server"></asp:Literal>
                                <asp:RequiredFieldValidator
                                    ControlToValidate="ValidateCode"
                                    ErrorMessage=" *" ForeColor="red"
                                    Display="Dynamic"
                                    runat="server" />
                            </li>
                        </asp:PlaceHolder>
                        <li><span class="yun_s1">&nbsp;</span>

                            <label class="checkbox">
                                <asp:CheckBox ID="RememberMe" Checked="true" runat="server"></asp:CheckBox>
                                记住用户名
                            </label>

                            &nbsp;&nbsp;&nbsp;&nbsp; <a class="btn" href="forgetPassword.aspx">忘记密码？</a></li>
                        <li><span class="yun_s1">&nbsp;</span>

                            <asp:Button class="yun_submit" ID="LoginSubmit" Style="width: 101px" OnClick="Submit_OnClick" runat="server" />

                            <!-- 您还不是SITEYUN用户，立即 <a class="yun_org" href="#">免费注册</a> -->
                        </li>
                        <%-- <li class="yun_hzbox">使用合作网站账号登录： 
                            <a href="javascript:;" onclick="sdkController.login(1);">
                                <img src="pic/login/ynu_sico3.jpg" width="16" height="16" alt="腾讯QQ" /></a> &nbsp;
                            <a href="javascript:;" onclick="sdkController.login(2);">
                                <img src="pic/login/ynu_sico4.jpg" width="16" height="16" alt="新浪Weibo" /></a></li>--%>
                    </ul>
                </div>
            </div>
            <div class="yunFooter">北京百容千域软件技术开发有限公司 版权所有 Copyright © 2003-<script>document.write(new Date().getFullYear());</script></div>
        </div>
    </form>
</body>
</html>
<asp:literal id="ltlScript" runat="server"></asp:literal>

