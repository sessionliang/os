<%@ Page Language="C#" ValidateRequest="false" Inherits="SiteServer.CMS.BackgroundPages.Modal.InputClassifyAdd" Trace="false" %>

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
        <asp:Button ID="btnSubmit" UseSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" Style="display: none" />
        <bairong:Alerts runat="server"></bairong:Alerts>

        <script language="javascript">
            function selectChannel(nodeNames, nodeID) {
                $('#nodeNames').html(nodeNames);
                $('#nodeID').val(nodeID);
            }
        </script>

        <table class="table noborder">
            <tr>
                <td width="80">父级分类： </td>
                <td>
                    <asp:DropDownList ID="ParentItemID" AutoPostBack="true" OnSelectedIndexChanged="ParentItemID_SelectedIndexChanged" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <td colspan="2">
                <div class="alert alert-info">
                    分类之间用换行分割，下级分类在分类前添加“－”字符，索引可以放到括号中，如：
                        <br>
                    分类一(分类索引)<br>
                    －下级分类(下级索引)<br>
                    －－下下级分类
                </div>
            </td>
            </tr>
            <tr>
                <td colspan="2" class="center">
                    <asp:TextBox Style="width: 98%; height: 240px" TextMode="MultiLine" ID="ItemNames" runat="server" /></td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
