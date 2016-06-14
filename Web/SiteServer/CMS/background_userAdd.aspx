<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundUserAdd" %>


<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <!--#include file="../inc/header.aspx"-->
    <style type="text/css">
        .city {
            width: 75px;
        }
    </style>
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->

    <form class="form-inline" runat="server">
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />
        <script src="../../SiteFiles/Services/WeiXin/card/scripts/lib/jquery-1.11.0.min.js"></script>
        <script src="../../SiteFiles/Services/WeiXin/card/scripts/lib/provincesdata.js"></script>
        <script src="../../SiteFiles/Services/WeiXin/card/scripts/lib/jquery.provincesCity.js"></script>
        <script type="text/javascript">
            $(function () {
                $("#province").ProvinceCity();
                $("#province select").attr("class", "city")
                if ($("#provinceValue").val().length > 0) {
                    $("#province select").eq(0).val($("#provinceValue").val().split(',')[0]);
                    $("#province select").eq(1).append('<option value="' + $("#provinceValue").val().split(',')[1] + '" selected="selected">' + $("#provinceValue").val().split(',')[1] + '</option>');
                    $("#province select").eq(2).append('<option value="' + $("#provinceValue").val().split(',')[2] + '" selected="selected">' + $("#provinceValue").val().split(',')[2] + '</option>');
                }
                $("#province select").change(function () {
                    $("#provinceValue").val($("#province select").eq(0).val() + "," + $("#province select").eq(1).val() + "," + $("#province select").eq(2).val());
                });
            });
        </script>
        <div class="popover popover-static operation-area">
            <h3 class="popover-title">
                <asp:Literal ID="ltlPageTitle" runat="server" />
            </h3>
            <div class="popover-content">
                <div class="container-fluid" id="weixinactivate">
                    <div class="row-fluid">
                        <table class="table noborder table-hover">
                            <tr>
                                <td>账号：</td>
                                <td>
                                    <asp:TextBox ID="tbUserName" MaxLength="50" runat="server" />
                                    <asp:RequiredFieldValidator
                                        ControlToValidate="tbUserName"
                                        ErrorMessage=" *" ForeColor="red"
                                        Display="Dynamic"
                                        runat="server" /><span class="gray">（帐号用于登录系统，由字母、数字组成）</span>
                                </td>
                            </tr>
                            <tr>
                                <td>姓名：</td>
                                <td>
                                    <asp:TextBox ID="tbDisplayName" MaxLength="50" runat="server" />
                                    <asp:RequiredFieldValidator
                                        ControlToValidate="tbDisplayName"
                                        ErrorMessage=" *" ForeColor="red"
                                        Display="Dynamic"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>性别：</td>
                                <td>
                                    <asp:DropDownList ID="ddlGender" runat="server"></asp:DropDownList>
                                </td>
                            </tr>

                            <asp:PlaceHolder ID="phPassword" runat="server">
                                <tr>
                                    <td>密 码：</td>
                                    <td>
                                        <asp:TextBox ID="tbPassword" TextMode="Password" MaxLength="50" runat="server" />
                                        <asp:RequiredFieldValidator
                                            ControlToValidate="tbPassword"
                                            ErrorMessage=" *" ForeColor="red"
                                            Display="Dynamic"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>确认密码：</td>
                                    <td>
                                        <asp:TextBox ID="tbConfirmPassword" TextMode="Password" MaxLength="50" runat="server" />
                                        <asp:RequiredFieldValidator
                                            ControlToValidate="tbConfirmPassword"
                                            ErrorMessage=" *" ForeColor="red"
                                            Display="Dynamic"
                                            runat="server" />
                                        <asp:CompareValidator ID="tbNewPasswordCompare" runat="server" ControlToCompare="tbPassword" ControlToValidate="tbConfirmPassword" Display="Dynamic" ErrorMessage=" 两次输入的密码不一致！请再输入一遍您上面填写的密码。" ForeColor="red"></asp:CompareValidator>
                                    </td>
                                </tr>
                            </asp:PlaceHolder>
                            <tr>
                                <td>所在区域：</td>
                                <td>
                                    <span id="province"></span>
                                    <input type="hidden" id="provinceValue" runat="server" />
                                 </td>
                              </tr>
                            <tr>
                                <td>详细地址：</td>
                                <td>
                                    <asp:TextBox ID="tbAddress" MaxLength="50" runat="server" />
                                    <asp:RequiredFieldValidator
                                        ControlToValidate="tbAddress"
                                        ErrorMessage=" *" ForeColor="red"
                                        Display="Dynamic"
                                        runat="server" />
                               </td>
                            </tr>
                            <tr>
                                <td>电子邮箱：</td>
                                <td>
                                    <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox>
                                    <asp:RegularExpressionValidator ControlToValidate="tbEmail"
                                        ValidationExpression="(\w[0-9a-zA-Z_-]*@(\w[0-9a-zA-Z-]*\.)+\w{2,})"
                                        ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>手机号码：</td>
                                <td>
                                    <asp:TextBox ID="tbMobile" runat="server"></asp:TextBox>
                                    <asp:RegularExpressionValidator ControlToValidate="tbMobile"
                                        ValidationExpression="^(13|15|18)\d{9}$"
                                        ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                </td>
                            </tr>
                             <bairong:UserAuxiliaryControl ID="uacAttributes" runat="server"/>
                        </table>
                    </div>

                    <hr />
                    <table class="table table-noborder">
                        <tr>
                            <td class="center">
                                <asp:Button class="btn btn-primary" ID="btnSubmit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                                <asp:Button class="btn" ID="btnReturn" Text="返 回" runat="server" />
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
        </div>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
