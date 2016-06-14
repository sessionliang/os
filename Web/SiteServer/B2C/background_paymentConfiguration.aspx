<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundPaymentConfiguration" %>

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
            <h3 class="popover-title">配置支付方式</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="180">支付方式名称：</td>
                        <td>
                            <asp:TextBox ID="tbPaymentName" runat="server" />
                            <asp:RequiredFieldValidator ControlToValidate="tbPaymentName" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbPaymentName" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td>类型：</td>
                        <td>
                            <asp:Literal ID="ltlPaymentType" runat="server" />
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phAlipay" Visible="false" runat="server">
                        <tr>
                            <td>支付宝账户：</td>
                            <td>
                                <asp:TextBox ID="tbAlipaySellerEmail" runat="server" />
                                <asp:RequiredFieldValidator ControlToValidate="tbAlipaySellerEmail" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbAlipaySellerEmail" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                            </td>
                        </tr>
                        <tr>
                            <td>合作者身份（PID）：</td>
                            <td>
                                <asp:TextBox ID="tbAlipayPartner" runat="server" />
                                <asp:RequiredFieldValidator ControlToValidate="tbAlipayPartner" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbAlipayPartner" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                            </td>
                        </tr>
                        <tr>
                            <td>安全校验码（Key）：</td>
                            <td>
                                <asp:TextBox ID="tbAlipayKey" runat="server" />
                                <asp:RequiredFieldValidator ControlToValidate="tbAlipayKey" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbAlipayKey" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                            </td>
                        </tr>
                        <tr>
                            <td>选择接口类型：</td>
                            <td>
                                <asp:DropDownList ID="ddlAlipayType" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phUnionpay" Visible="false" runat="server">
                        <tr>
                            <td>商户号：</td>
                            <td>
                                <asp:TextBox ID="tbMerID" runat="server" />
                                <asp:RequiredFieldValidator ControlToValidate="tbMerID" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbMerID" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                            </td>
                        </tr>
                        <tr>
                            <td>商户私钥证书：</td>
                            <td>
                                <asp:FileUpload ID="fuSignCert" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>商户私钥证书密码：</td>
                            <td>
                                <asp:TextBox ID="tbSignCertPwd" TextMode="Password" runat="server" />
<%--                                <asp:RequiredFieldValidator ControlToValidate="tbSignCertPwd" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbSignCertPwd" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td>银联公钥证书：</td>
                            <td>
                                <asp:FileUpload ID="fuEncryptCert" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>是否是正式环境：</td>
                            <td>
                                <asp:DropDownList ID="ddlIsTest" runat="server" OnSelectedIndexChanged="ddlIsTest_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <td>支付方式描述：</td>
                        <td>
                            <bairong:BREditor ID="breDescription" runat="server"></bairong:BREditor>
                        </td>
                    </tr>
                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                            <input type="button" value="返 回" onclick="javascript:location.href = 'background_payment.aspx?PublishmentSystemID=<%=PublishmentSystemID%>    ';" class="btn">
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
