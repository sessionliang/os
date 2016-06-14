<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.CMS.BackgroundPages.ConsoleOrganizationAdd" Trace="false" %>

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
        <asp:Button ID="btnSubmit" UseSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" Style="display: none" />
        <bairong:Alerts runat="server"></bairong:Alerts>

        <div class="popover popover-static">
            <h3 class="popover-title">添加分支机构</h3>
            <div class="popover-content">

                <table class="table noborder">
                    <tr>
                        <td width="80">所属分类： </td>
                        <td>
                            <asp:DropDownList ID="ddlClassifyID" OnSelectedIndexChanged="ddlClassifyID_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>名称：</td>
                        <td>
                            <asp:TextBox ID="tbName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator"
                                ControlToValidate="tbName"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic"
                                runat="server" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3"
                                runat="server"
                                ControlToValidate="tbName"
                                ValidationExpression="[^']+"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td width="80">所在区域： </td>
                        <td>
                            <asp:DropDownList ID="ddlAreaID" AutoPostBack="true" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>地址：</td>
                        <td>
                            <asp:TextBox ID="tbAddress" runat="server"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                runat="server"
                                ControlToValidate="tbAddress"
                                ValidationExpression="[^']+"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td>推荐路线：</td>
                        <td>
                            <asp:TextBox ID="tbExplain" runat="server"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator"
                                runat="server"
                                ControlToValidate="tbExplain"
                                ValidationExpression="[^']+"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td>电话：</td>
                        <td>
                            <asp:TextBox ID="tbPhone" runat="server"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                                runat="server"
                                ControlToValidate="tbPhone"
                                ValidationExpression="[^']+"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td>经度：</td>
                        <td>
                            <asp:TextBox ID="tbLongitude" runat="server"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                runat="server"
                                ControlToValidate="tbLongitude"
                                ValidationExpression="^[-]?(\d|([1-9]\d)|(1[0-7]\d)|(180))(\.\d*)?$"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td>纬度：</td>
                        <td>
                            <asp:TextBox ID="tbLatitude" runat="server"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                runat="server"
                                ControlToValidate="tbLatitude"
                                ValidationExpression="^[-]?(\d|([1-8]\d)|(90))(\.\d*)?$"
                                ErrorMessage=" *" ForeColor="red"
                                Display="Dynamic" />
                        </td>
                    </tr>
                    <tr>
                        <td>图片：</td>
                        <td>
                            <asp:TextBox ID="tbLogoUrl" runat="server"></asp:TextBox>   
                            <asp:Button class="btn" ID="ImgUpload" Text="选择图片"   runat="server" />
                            <!--<asp:Button class="btn" ID="ImgSelect" Text="  "   runat="server" />
                            <asp:Button class="btn" ID="ImgCut" Text="  "   runat="server" />
                            <asp:Button class="btn" ID="ImgView" Text="  "   runat="server" />-->
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
