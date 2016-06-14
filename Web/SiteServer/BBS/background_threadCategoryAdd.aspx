<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundThreadCategoryAdd" %>

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

        <script>
            function setOptionColor(obj) {
                for (var i=0;i<obj.options.length;i++) {
                    if (obj.options[i].value=="") 
                        obj.options[i].style.color="gray";
                    else
                        obj.options[i].style.color="black";
                }
            }
        </script>

        <div class="popover popover-static">
            <h3 class="popover-title">
                <asp:Literal ID="ltlPageTitle" runat="server" /></h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="170">
                            <bairong:Help HelpText="分类所属版块" Text="分类所属版块：" runat="server"></bairong:Help>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlForumIDList" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator ControlToValidate="ddlForumIDList" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                            <script type="text/javascript" language="javascript">
                                setOptionColor(document.getElementById('<%=ddlForumIDList.ClientID%>'));
                            </script>
                        </td>
                    </tr>
                    <tr>
                        <td width="170">
                            <bairong:Help HelpText="分类名称" Text="分类名称：" runat="server"></bairong:Help>
                        </td>
                        <td>
                            <asp:TextBox Columns="40" MaxLength="200" ID="txtCategoryName" runat="server" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCategoryName"
                                ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td width="170">
                            <bairong:Help HelpText="简 介" Text="简 介：" runat="server"></bairong:Help>
                        </td>
                        <td>
                            <asp:TextBox TextMode="MultiLine" Width="90%" Rows="5" MaxLength="1000" ID="txtSummary" runat="server" />
                        </td>
                    </tr>
                </table>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                            <input class="btn" type="button" onclick="location.href='background_threadCategory.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>    ';return false;" value="返 回" />
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
