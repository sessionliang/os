<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.ApplyTranslate" Trace="false"%>

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
<bairong:alerts runat="server" text="转移后办件将不再属于此项目"></bairong:alerts>

  <table class="table table-noborder table-hover">
      <tr>
        <td class="center" width="120">转移到：</td>
        <td><asp:DropDownList ID="ddlProjectID" runat="server"></asp:DropDownList></td>
      </tr>
    </table>

</form>
</body>
</html>
