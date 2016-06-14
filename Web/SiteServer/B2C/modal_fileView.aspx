<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.Modal.FileView" Trace="false" %>

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
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>

    <table class="table table-striped">
        <tr>
            <td width="33%">
                <bairong:Help HelpText="文件名称" Text="文件名称：" runat="server">
                </bairong:Help>
            </td>
            <td>
                <asp:Literal ID="ltlFileName" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td width="33%">
                <bairong:Help HelpText="文件类型" Text="文件类型：" runat="server">
                </bairong:Help>
            </td>
            <td>
                <asp:Literal ID="ltlFileType" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td width="33%">
                <bairong:Help HelpText="文件位置" Text="位置：" runat="server">
                </bairong:Help>
            </td>
            <td>
                <asp:Literal ID="ltlFilePath" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td width="33%">
                <bairong:Help HelpText="文件大小" Text="大小：" runat="server">
                </bairong:Help>
            </td>
            <td>
                <asp:Literal ID="ltlFileSize" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td width="33%">
                <bairong:Help HelpText="文件创建时间" Text="创建时间：" runat="server">
                </bairong:Help>
            </td>
            <td>
                <asp:Literal ID="ltlCreationTime" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td width="33%">
                <bairong:Help HelpText="文件最后修改时间" Text="最后修改时间：" runat="server">
                </bairong:Help>
            </td>
            <td>
                <asp:Literal ID="ltlLastWriteTime" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td width="33%">
                <bairong:Help HelpText="文件最后访问时间" Text="最后访问时间：" runat="server">
                </bairong:Help>
            </td>
            <td>
                <asp:Literal ID="ltlLastAccessTime" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>

    <table width="100%">
        <tr>
            <td class="center">
                <ul class="breadcrumb">
                    <asp:Literal ID="Open" runat="server"></asp:Literal>
                    &nbsp;&nbsp;
                    <asp:PlaceHolder ID="PlaceHolder_Edit" runat="server">
                        <asp:LinkButton ID="Edit" OnClick="Edit_OnClick" runat="server" Text="修 改" />
                        &nbsp;&nbsp;</asp:PlaceHolder>
                    <asp:Literal ID="ChangeName" runat="server"></asp:Literal>
                </ul>
            </td>
        </tr>
    </table>

</form>
</body>
</html>
<!-- check for 3.6.4 html permissions -->