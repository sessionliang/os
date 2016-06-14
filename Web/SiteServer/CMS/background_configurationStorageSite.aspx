<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundConfigurationStorageSite" %>

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
        <bairong:Alerts Text="采用独立空间存储站点文件需要在服务管理菜单中设置好对应的存储空间并在服务器中安装SiteServer Service服务组件" runat="server" />

        <div class="popover popover-static">
            <h3 class="popover-title">Web服务器部署</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="200">网站部署方式：</td>
                        <td>
                            <asp:RadioButtonList ID="rblIsSiteStorage" AutoPostBack="true" OnSelectedIndexChanged="rblIsSiteStorage_SelectedIndexChanged" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList>
                            <span>设置网站的部署方式，分离部署即前后台分离。</span>
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phSiteStorage" runat="server">
                        <tr>
                            <td>Web访问地址：</td>
                            <td>
                                <asp:DropDownList ID="ddlSiteStorageID" runat="server"></asp:DropDownList>
                                <br>
                                <span>Web访问地址为前台访问地址</span>
                            </td>
                        </tr>
                        <tr>
                            <td>生成页面URL前缀：</td>
                            <td>
                                <asp:TextBox Columns="25" MaxLength="50" ID="tbSiteStoragePath" runat="server" />
                                <asp:RequiredFieldValidator ControlToValidate="tbSiteStoragePath" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbSiteStoragePath" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                                <br>
                                <span>前缀设置之后，页面会保留前缀。同时，页面在web服务器中的存放位置，也应该在前缀对应的文件夹中。</span>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <td width="200">主子站部署方式：</td>
                        <td>
                            <asp:RadioButtonList ID="rblIsSonSiteAlone" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList>
                            <span>设置主子站的部署方式，子站单独部署即子站可以设置单独的域名。</span>
                        </td>
                    </tr>
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
