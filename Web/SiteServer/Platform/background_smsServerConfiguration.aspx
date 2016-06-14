<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundSMSServerConfiguration" %>

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
            <h3 class="popover-title">配置短信服务商</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="180">名称：</td>
                        <td>
                            <asp:TextBox ID="tbSMSServerName" runat="server" />
                            <asp:RequiredFieldValidator ControlToValidate="tbSMSServerName" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbSMSServerName" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td>类型：</td>
                        <td>
                            <asp:Literal ID="ltlSMSServerType" runat="server" />
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phYunPian" Visible="false" runat="server">
                        <tr>
                            <td>服务商认证ID（APIKEY）：</td>
                            <td>
                                <asp:TextBox ID="tbYunPianKey" runat="server" />
                                <asp:RequiredFieldValidator ControlToValidate="tbYunPianKey" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbYunPianKey" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                                &nbsp;<a href="http://www.yunpian.com/" target="_blank">（云片网）</a>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phWeiMi" Visible="false" runat="server">
                        <tr>
                            <td>UID代码：</td>
                            <td>
                                <asp:TextBox ID="tbUID" runat="server" />
                                <asp:RequiredFieldValidator ControlToValidate="tbUID" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbUID" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                                &nbsp;<a href="http://www.weimi.cc/" target="_blank">（微米）</a>
                            </td>
                        </tr>
                        <tr>
                            <td>UID密码：</td>
                            <td>
                                <asp:TextBox ID="tbUPASS" runat="server" />
                                <asp:RequiredFieldValidator ControlToValidate="tbUPASS" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbUPASS" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                            </td>
                        </tr>
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
