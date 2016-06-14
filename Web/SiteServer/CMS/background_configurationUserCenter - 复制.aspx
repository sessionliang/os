<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundConfigurationUserCenter" %>

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
        <script>
            $(document).ready(function () {
                $('#myTab a').click(function (e) {
                    e.preventDefault();
                    changeTab($(this).attr('index'));
                });
      <%=GetChangeTabFunction()%>
            });
            function changeTab(index) {
                $('#index').val(index);
                $($('#myTab a').get(index)).tab('show');
            }
        </script>

        <input type="hidden" id="index" name="index" value="0" />

        <ul class="nav nav-pills" id="myTab">
            <li class="active"><a href="#basic" index="0">基本设置</a></li>
            <li><a href="#advance" index="1">访问地址设置</a></li>
        </ul>

        <div class="tab-content">
            <div class="tab-pane active" id="basic">
                <table class="table table-bordered table-hover">
                    <tr>
                        <td width="200">用户中心名称：</td>
                        <td>
                            <asp:TextBox Columns="25" MaxLength="50" ID="PublishmentSystemName" runat="server" class="input-xlarge" />
                            <asp:RequiredFieldValidator ControlToValidate="PublishmentSystemName" ErrorMessage=" *" ForeColor="red" Display="Dynamic" runat="server" />
                            <asp:RegularExpressionValidator runat="server" ControlToValidate="PublishmentSystemName" ValidationExpression="[^']+" ErrorMessage=" *" ForeColor="red" Display="Dynamic" />
                        </td>
                    </tr>
                    <site:AuxiliaryControl ID="acAttributes" runat="server" />
                </table>
            </div>
            <div class="tab-pane" id="advance">
                <table class="table table-bordered table-hover">
                    <tr>
                        <td colspan="2">
                            <div>
                                外网访问地址：<asp:Literal ID="ltOuterUrl" runat="server"></asp:Literal>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                            内网访问地址：<asp:Literal ID="ltInnerUrl" runat="server"></asp:Literal>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>生成页面URL前缀</td>
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
            </div>
        </div>

        <hr />
        <table class="table noborder">
            <tr>
                <td class="center">
                    <asp:Button class="btn btn-primary" ID="Submit" Text="确 定" OnClick="Submit_OnClick" runat="server" />
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
