<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.ApplyComment" Trace="false"%>

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
        <td class="center" width="120">批注意见：</td>
        <td><asp:TextBox ID="tbCommentRemark" runat="server" TextMode="MultiLine" Columns="60" rows="4"></asp:TextBox></td>
      </tr>
      <tr>
        <td class="center">批注部门：</td>
        <td><asp:Literal ID="ltlDepartmentName" runat="server"></asp:Literal></td>
      </tr>
      <tr>
        <td class="center">批注人：</td>
        <td><asp:Literal ID="ltlUserName" runat="server"></asp:Literal></td>
      </tr>
    </table>

</form>
</body>
</html>
