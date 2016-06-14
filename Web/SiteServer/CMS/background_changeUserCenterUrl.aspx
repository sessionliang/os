<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.ChangeUserCenterUrl" Trace="false" %>

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
        <bairong:Alerts Text="此操作将修改用户中心的访问地址，修改前请先确认此地址能够被访问。" runat="server"></bairong:Alerts>

        <div class="popover popover-static">
            <h3 class="popover-title">用户中心访问地址设置</h3>
            <div class="popover-content">
                <div>
                    外网访问地址：<asp:Literal ID="ltOuterUrl" runat="server"></asp:Literal>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                            内网访问地址：<asp:Literal ID="ltInnerUrl" runat="server"></asp:Literal>
                </div>
                <hr />
                <table class="table noborder table-hover">
                    <tr>
                        <td width="160">生成页面URL前缀</td>
                        <td>
                            <asp:TextBox ID="tbPublishmentSystemUrl" Columns="40" MaxLength="200" Style="ime-mode: disabled;" runat="server" />
                            <asp:RequiredFieldValidator ControlToValidate="tbPublishmentSystemUrl" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbPublishmentSystemUrl" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                            <br />
                            <span class="gray">页面所有地址将保留此前缀，可以设置绝对路径（域名）或者相对路径（如：“/”）</span>
                        </td>
                    </tr>
                    <tr>
                        <td>网站部署方式：</td>
                        <td>
                            <asp:DropDownList ID="ddlIsMultiDeployment" AutoPostBack="true" OnSelectedIndexChanged="ddlIsMultiDeployment_SelectedIndexChanged" runat="server"></asp:DropDownList>
                            <br />
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
                                <br />
                                <span class="gray">外部访问的地址，通常填写网站域名</span>
                            </td>
                        </tr>
                        <tr>
                            <td>网站内部访问地址：</td>
                            <td>
                                <asp:TextBox ID="tbInnerUrl" Columns="40" MaxLength="200" Style="ime-mode: disabled;" runat="server" />
                                <asp:RequiredFieldValidator ControlToValidate="tbInnerUrl" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="tbInnerUrl" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                                <br />
                                <span class="gray">内部访问的地址，后台访问将访问此地址</span>
                            </td>
                        </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <td>功能页面访问方式：</td>
                        <td>
                            <asp:DropDownList ID="ddlFuncFilesType" AutoPostBack="true" OnSelectedIndexChanged="ddlFuncFilesType_SelectedIndexChanged" runat="server" />
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
