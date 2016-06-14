<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.CMS.BackgroundPages.ConsoleOrganizationClassifyAdd" Trace="false" %>

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
            <h3 class="popover-title">添加分类</h3>
            <div class="popover-content">

                <table class="table noborder">
                    <tr>
                        <td width="80">父级分类： </td>
                        <td>
                            <asp:DropDownList ID="ParentItemID" AutoPostBack="true" OnSelectedIndexChanged="ParentItemID_SelectedIndexChanged" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>分类名称：</td>
                        <td>
                            <asp:TextBox ID="ItemName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator"
                                ControlToValidate="ItemName"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>分类编码：</td>
                        <td>
                            <asp:TextBox ID="ItemIndexName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RegularExpressionValidator1"
                                runat="server"
                                ControlToValidate="ItemIndexName"
                                ValidationExpression="[^']+"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic" />
                        </td>
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
