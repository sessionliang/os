<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundConfigurationCreate" %>

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
        <bairong:Alerts Text="在此对生成页面进行详细设置" runat="server"></bairong:Alerts>

        <div class="popover popover-static">
            <h3 class="popover-title">页面生成设置</h3>
            <div class="popover-content">

                <table class="table noborder table-hover">
                    <tr>
                        <td width="260">是否仅浏览时生成页面：</td>
                        <td>
                            <asp:RadioButtonList ID="IsCreateRedirectPage" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
                    </tr>
                    <tr>
                        <td>当内容变动时是否生成本页：</td>
                        <td>
                            <asp:RadioButtonList ID="IsCreateContentIfContentChanged" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
                    </tr>
                    <tr>
                        <td>当栏目变动时是否生成本页：</td>
                        <td>
                            <asp:RadioButtonList ID="IsCreateChannelIfChannelChanged" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
                    </tr>
                    <tr>
                        <td>当内容变动时是否递归生成父栏目页：</td>
                        <td>
                            <asp:RadioButtonList ID="IsCreateChannelsByChildNodeID" RepeatDirection="Horizontal" class="noborder" runat="server" OnSelectedIndexChanged="IsCreateChannelsByChildNodeID_OnSelectedIndexChanged" AutoPostBack="true"></asp:RadioButtonList>
                            <span class="gray">设置内容定时审核或者下架任务时，该选项起作用</span>
                        </td>
                    </tr>
                    <asp:PlaceHolder ID="phScoap" runat="server" Visible="false">
                    <tr>
                        <td>递归生成父栏目页的时间间隔：</td>
                        <td>
                            <asp:TextBox ID="tbScope" class="noborder" runat="server" Text="0"></asp:TextBox>（单位：分钟）
                        </td>
                    </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <td>生成页面中是否显示相关信息：</td>
                        <td>
                            <asp:RadioButtonList ID="IsCreateShowPageInfo" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
                    </tr>
                    <tr>
                        <td>是否设置meta标签强制IE8兼容：</td>
                        <td>
                            <asp:RadioButtonList ID="IsCreateIE8Compatible" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
                    </tr>
                    <tr>
                        <td>是否设置meta标签强制浏览器清除缓存：</td>
                        <td>
                            <asp:RadioButtonList ID="IsCreateBrowserNoCache" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
                    </tr>
                    <tr>
                        <td>是否设置包含JS容错代码：</td>
                        <td>
                            <asp:RadioButtonList ID="IsCreateJsIgnoreError" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
                    </tr>
                    <tr>
                        <td>内容列表及搜索是否可包含重复标题：</td>
                        <td>
                            <asp:RadioButtonList ID="IsCreateSearchDuplicate" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
                    </tr>
                    <tr>
                        <td>是否将stl:include转换为SSI动态包含：</td>
                        <td>
                            <asp:RadioButtonList ID="IsCreateIncludeToSSI" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList>
                            <span class="gray">需要IIS启用服务器端包含功能，同时需要将生成页面后缀设置为“.shtml”</span>
                        </td>
                    </tr>
                    <tr>
                        <td>是否生成页面中包含JQuery脚本引用：</td>
                        <td>
                            <asp:RadioButtonList ID="IsCreateWithJQuery" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList></td>
                    </tr>
                    <tr>
                        <td>IIS默认页：</td>
                        <td>
                            <asp:TextBox ID="tbIISDefaultPage" Columns="40" MaxLength="200" runat="server" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbIISDefaultPage" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" /><br />
                            <span class="gray">需要在IIS中将此值设置为默认页</span>
                        </td>
                    </tr>
                    <tr>
                        <td>是否启用双击生成页面：</td>
                        <td>
                            <asp:RadioButtonList ID="IsCreateDoubleClick" RepeatDirection="Horizontal" class="noborder" runat="server"></asp:RadioButtonList>
                            <span class="gray">此功能通常用于制作调试期间，网站开发期间建议启用</span>
                        </td>
                    </tr>
                    <tr>
                        <td>实际生成翻页内容列表最大数：</td>
                        <td>
                            <asp:TextBox ID="tbCreateStaticMaxPage" class="input-mini" runat="server" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="tbCreateStaticMaxPage" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />页<br />
                            <span class="gray">在此设置内容翻页中生成页面数量的最大值，从而提高生成速度；设置为0代表将页面全部生成</span>
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
