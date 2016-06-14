<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.CMS.BackgroundPages.ConsoleOrganizationAreaAdd" Trace="false" %>

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
        <asp:Button ID="btnSubmit" UseSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" Style="display: none" />
        <bairong:Alerts runat="server"></bairong:Alerts>

        <div class="popover popover-static">
            <h3 class="popover-title">添加区域</h3>
            <div class="popover-content">


                <table class="table table-noborder table-hover">
                    <tr>
                        <td>上级区域：</td>
                        <td>
                            <asp:DropDownList ID="ParentItemID" runat="server"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td width="80">区域名称：</td>
                        <td>
                            <asp:TextBox Columns="30" ID="tbItemName" runat="server" />
                            <asp:RequiredFieldValidator ControlToValidate="tbItemName" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" /></td>
                    </tr>
                </table>
                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                            <input class="btn" type="button" onclick="location.href='<%=ReturnUrl%>    ';return false;" value="返 回" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
