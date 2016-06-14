<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.WeiXin.BackgroundPages.Modal.AppointmentHandle" Trace="false" %>

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

        <table class="table table-noborder table-hover">
            <tr>
                <td width="80">预约状态</td>
                <td>
                    <asp:DropDownList ID="ddlStatus" autoPostBack="true" onSelectedIndexChanged="ddlStatus_SelectedIndexChanged" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <asp:PlaceHolder id="phMessage" runat="server">
            <tr>
                <td>留言</td>
                <td>
                    <asp:TextBox ID="tbMessage" TextMode="Multiline" class="textarea" Rows="3" Style="width: 95%; padding: 5px;" runat="server" />
                </td>
            </tr>
            </asp:PlaceHolder>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
