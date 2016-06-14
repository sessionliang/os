<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundUserNewGroupAdd" %>

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

        <div class="popover popover-static">
            <h3 class="popover-title">用户组设置</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td style="width: 200px;">用户组名称：</td>

                        <td>
                            <asp:TextBox Columns="25" MaxLength="50" ID="tbUserGroupName" runat="server" />
                            <asp:RequiredFieldValidator
                                ControlToValidate="tbUserGroupName"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic"
                                runat="server" />
                            <asp:RegularExpressionValidator
                                runat="server"
                                ControlToValidate="tbUserGroupName"
                                ValidationExpression="[^',]+"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic" />
                            <span>唯一标识此用户组的字符串</span>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 200px;">用户组描述：</td>
                        <td>
                            <asp:TextBox Columns="50" TextMode="MultiLine" ID="tbDescription" runat="server" />
                            <asp:RegularExpressionValidator
                                runat="server"
                                ControlToValidate="tbDescription"
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
                           
                    <input type="button" class="btn" value="返 回" onclick="javascript:location.href='background_userNewGroup.aspx?PublishmentSystemID=<%= base.PublishmentSystemID%>';" />
                     
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
