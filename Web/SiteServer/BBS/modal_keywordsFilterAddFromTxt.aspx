<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.Modal.KeywordsFilterAddFromTxt" %>

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

  <table class="table table-noborder table-hover">
    <tr>
        <td width="80">上传文件：</td>
        <td>
            <asp:FileUpload ID="fileTxt" runat="server" />
        </td>
    </tr>
    <tr>
        <td>选择分类：</td>
        <td>
            <asp:DropDownList ID="ddlCategory" runat="server">
            </asp:DropDownList>
        </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->