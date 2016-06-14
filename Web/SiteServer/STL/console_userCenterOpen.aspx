<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.ConsoleUserCenterOpen" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <!--#include file="../inc/header.aspx"-->
    <script type="text/javascript">
        function displaySiteTemplateDiv(obj) {
            if (obj.checked) {
                document.getElementById('RowSiteTemplateName').style.display = '';
            } else {
                document.getElementById('RowSiteTemplateName').style.display = 'none';
            }
        }
    </script>
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->
    <form class="form-inline" runat="server">
        <asp:Literal ID="ltlBreadCrumb" runat="server" />
        <bairong:Alerts runat="server" />
        <div class="popover popover-static">
            <h3 class="popover-title">
                <asp:Literal ID="ltlPageTitle" runat="server" /></h3>
            <div class="popover-content">
                <asp:PlaceHolder ID="CreateSiteParameters" runat="server" Visible="true">
                    <blockquote>
                        <p>开启用户中心</p>
                        <small>在此设置用户中心名称，前台访问地址等信息。</small>
                    </blockquote>

                    <table class="table table-hover table-noborder">
                        <tr>
                            <td>是否使用模板</td>
                            <td>
                                <asp:CheckBox runat="server" ID="UseSiteTemplate" Checked="true" Text="使用"/>
                            </td>
                        </tr>
                        <tr id="RowSiteTemplateName">
                            <td>用户中心模板名称：</td>
                            <td>
                                <div id="SiteTemplateDiv">
                                    <asp:DropDownList runat="server" ID="ddlSiteTemplate"></asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td width="160">用户中心名称：</td>
                            <td>
                                <asp:TextBox Columns="35" MaxLength="50" ID="UserCenterName" Text="用户中心" runat="server" />
                                <asp:RegularExpressionValidator
                                    runat="server"
                                    ControlToValidate="UserCenterName"
                                    ValidationExpression="[^']+"
                                    ErrorMessage=" *" ForeColor="red"
                                    Display="Dynamic" />
                            </td>
                        </tr>
                        <tr>
                            <td>前台访问地址：</td>
                            <td>
                                <asp:TextBox Columns="25" MaxLength="50" ID="UserCenterDir" Text="Home" runat="server" />
                                <asp:RequiredFieldValidator
                                    ControlToValidate="UserCenterDir"
                                    ErrorMessage=" *" ForeColor="red"
                                    Display="Dynamic"
                                    runat="server" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="UserCenterDir" ValidationExpression="[a-zA-Z0-9_]+" ErrorMessage=" 只允许包含字母、数字以及下划线" Display="Dynamic" />
                                <br>
                                <span>实际在服务器中保存此网站的文件夹名称，此路径必须以英文或拼音命名。</span>
                            </td>
                        </tr>
                    </table>

                </asp:PlaceHolder>

                <asp:PlaceHolder ID="OperatingError" runat="server" Visible="false">

                    <blockquote style="margin-top: 20px;">
                        <p>发生错误</p>
                        <small>执行向导过程中出错</small>
                    </blockquote>

                    <div class="alert alert-error">
                        <h4>
                            <asp:Literal ID="ltlErrorMessage" runat="server"></asp:Literal></h4>
                    </div>

                </asp:PlaceHolder>

                <hr />
                <table class="table noborder">
                    <tr>
                        <td class="center">
                            <asp:Button class="btn btn-primary" ID="Next" OnClick="NextPlaceHolder" runat="server" Text="开  启"></asp:Button>
                        </td>
                    </tr>
                </table>

            </div>
        </div>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
