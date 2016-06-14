<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.WeiXin.BackgroundPages.Modal.CouponAdd" Trace="false" %>

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
        <asp:Button ID="btnSubmit" UseSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" Style="display: none" />
        <bairong:Alerts Text="添加成功后,点击优惠券明细. 可点击上传SN码生成（点击下载示例按要求上传）.也可点击添加,系统生成SN码." runat="server"></bairong:Alerts>

        <link href="css/emotion.css" rel="stylesheet">

        <table class="table table-noborder">
            <tr>
                <td width="120">优惠劵名称：</td>
                <td>
                    <asp:TextBox ID="tbTitle" runat="server" />
                    <asp:RequiredFieldValidator ControlToValidate="tbTitle" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                </td>
            </tr>
            <tr style="display: none;">
                <td>优惠劵总数：</td>
                <td>
                    <asp:TextBox ID="tbTotalNum" class="input-mini" Text="0" runat="server" />
                    <asp:RegularExpressionValidator
                        ControlToValidate="tbTotalNum"
                        ValidationExpression="\d+"
                        Display="Dynamic"
                        ForeColor="red"
                        ErrorMessage="必须为数字"
                        runat="server" />
                    <asp:RequiredFieldValidator ControlToValidate="tbTotalNum" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                </td>
            </tr>
            <tr style="display: none;">
                <td>自定义SN码：</td>
                <td class="checkbox">
                    <asp:CheckBox ID="cbIsEnabled" runat="server" Checked="true" Text="勾选后将不会自动生成SN码。" />
                </td>
            </tr>
            <tr style="display: none;">
                <td></td>
                <td class="checkbox">
                    <a href="../../SiteFiles/Services/WeiXin/coupon/sncode-example.xls">下载自动上传SN模板</a>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
