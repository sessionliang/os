<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.Modal.UserConsigneeAdd" %>

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
        <bairong:Alerts runat="server"></bairong:Alerts>
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
        <table class="table noborder table-hover">
            <tr>
                <td width="120">收货人：</td>
                <td>
                    <asp:TextBox ID="tbConsignee" runat="server" Columns="45" MaxLength="50"></asp:TextBox></td>
            </tr>
            <tr>
                <td>所在地区：</td>
                <td>
                    <span id="province"></span>
                    <input type="hidden" id="provinceValue" runat="server" />
                    <%--                    <asp:DropDownList ID="ddlProvice" class="input-medium" runat="server">
                        <asp:ListItem Value="" Text="--请选择--"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlCity" class="input-medium" runat="server">
                        <asp:ListItem Value="" Text="--请选择--"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlArea" class="input-medium" runat="server">
                        <asp:ListItem Value="" Text="--请选择--"></asp:ListItem>
                    </asp:DropDownList>--%>
                </td>
            </tr>
            <tr>
                <td>详细地址：</td>
                <td>
                    <asp:TextBox ID="tbAddress" runat="server" class="input-xlarge"></asp:TextBox></td>
            </tr>
            <tr>
                <td>邮编：</td>
                <td>
                    <asp:TextBox ID="tbZipcode" runat="server" Columns="45" MaxLength="50"></asp:TextBox></td>
            </tr>
            <tr>
                <td>手机：</td>
                <td>
                    <asp:TextBox ID="tbMobile" runat="server" Columns="45" MaxLength="50"></asp:TextBox></td>
            </tr>
            <tr>
                <td>固定电话：</td>
                <td>
                    <asp:TextBox ID="tbTel" runat="server" Columns="45" MaxLength="50"></asp:TextBox></td>
            </tr>
            <tr>
                <td>邮箱：</td>
                <td>
                    <asp:TextBox ID="tbEmail" runat="server" Columns="45" MaxLength="50"></asp:TextBox></td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
