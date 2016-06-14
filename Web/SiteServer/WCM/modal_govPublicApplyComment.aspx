<%@ Page Language="C#" Inherits="SiteServer.WCM.BackgroundPages.Modal.GovPublicApplyComment" Trace="false"%>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.WCM.Controls" Assembly="SiteServer.WCM" %>
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
      <td width="100">批示意见：</td>
      <td><asp:TextBox ID="tbCommentRemark" runat="server" TextMode="MultiLine" Columns="60" rows="4"></asp:TextBox></td>
    </tr>
    <tr>
      <td>批示部门：</td>
      <td><asp:Literal ID="ltlDepartmentName" runat="server"></asp:Literal></td>
    </tr>
    <tr>
      <td>批示人：</td>
      <td><asp:Literal ID="ltlUserName" runat="server"></asp:Literal></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->