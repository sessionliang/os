<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundConfigurationCreateUrl" %>

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
            <h3 class="popover-title">网站访问设置</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="200">生成页面URL前缀</td>
                        <td>
                            <asp:TextBox ID="tbPublishmentSystemUrl" Columns="40" MaxLength="200" Style="ime-mode: disabled;" runat="server" />
                            <asp:RequiredFieldValidator ControlToValidate="tbPublishmentSystemUrl" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbPublishmentSystemUrl" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                            <br>
                            <span class="gray">页面所有地址将保留此前缀，可以设置绝对路径（域名）或者相对路径（如：“/”）</span>
                        </td>
                    </tr>
                    <tr>
                        <td width="200">用户中心访问地址</td>
                        <td>
                            <asp:TextBox ID="tbHomeUrl" Columns="40" MaxLength="200" Style="ime-mode: disabled;" runat="server" />
                            <asp:RequiredFieldValidator ControlToValidate="tbHomeUrl" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbHomeUrl" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td>网站部署方式：</td>
                        <td>
                            <asp:DropDownList ID="ddlIsMultiDeployment" AutoPostBack="true" OnSelectedIndexChanged="ddlIsMultiDeployment_SelectedIndexChanged" runat="server"></asp:DropDownList>
                            <br>
                            <span class="gray">如果是多服务器部署，请选择“内外网分离部署”</span>
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phIsMultiDeployment" runat="server">
                        <tr>
                            <td>网站外部访问地址：</td>
                            <td>
                                <asp:TextBox ID="tbOuterUrl" Columns="40" MaxLength="200" Style="ime-mode: disabled;" runat="server" />
                                <asp:RequiredFieldValidator ControlToValidate="tbOuterUrl" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbOuterUrl" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                                <br>
                                <span class="gray">外部访问的地址，通常填写网站域名</span>
                            </td>
                        </tr>
                        <tr>
                            <td>网站内部访问地址：</td>
                            <td>
                                <asp:TextBox ID="tbInnerUrl" Columns="40" MaxLength="200" Style="ime-mode: disabled;" runat="server" />
                                <asp:RequiredFieldValidator ControlToValidate="tbInnerUrl" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbInnerUrl" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                                <br>
                                <span class="gray">内部访问的地址，后台访问将访问此地址</span>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <td>功能页面访问方式：</td>
                        <td>
                            <asp:DropDownList ID="ddlFuncFilesType" AutoPostBack="true" OnSelectedIndexChanged="ddlFuncFilesType_SelectedIndexChanged" runat="server" />
                            <br>
                            <span class="gray">在此设置功能页面（文件夹SiteFiles）的访问方式，如果站点与后台访问域名不同（跨域），建议采用“复制到站内访问”方式</span>
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phCrossDomainFilesCopy" runat="server">
                        <tr>
                            <td>将跨域代理页复制到站点中：</td>
                            <td>
                                <asp:Button class="btn btn-success" ID="btnCopyCrossDomainFiles" Text="复 制" OnClick="btnCopyCrossDomainFiles_OnClick" runat="server" />
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phFuncFilesCopy" runat="server">
                        <tr>
                            <td>将功能页复制到站点中：</td>
                            <td>
                                <asp:Button class="btn btn-success" ID="btnCopyFuncFiles" Text="复 制" OnClick="btnCopyFuncFiles_OnClick" runat="server" />
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="phAPIUrl" runat="server">
                        <tr>
                            <td>API地址：</td>
                            <td>
                                <asp:TextBox ID="tbAPIUrl" Columns="40" MaxLength="200" Style="ime-mode: disabled;" runat="server" />
                                <asp:RequiredFieldValidator ControlToValidate="tbAPIUrl" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbAPIUrl" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <td>子站是否单独部署：</td>
                        <td>
                            <asp:DropDownList ID="ddlIsSonSiteAlone" AutoPostBack="true" runat="server" />
                            <br />
                            <span class="gray">如果子站需要单独设置域名，那么选择“是”。</span>
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
