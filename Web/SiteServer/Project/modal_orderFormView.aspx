<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.Modal.OrderFormView" Trace="false"%>

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

  <table class="table table-bordered table-hover">
    <tr>
      <td width="80" align="right">模板地址：</td>
      <td><%=GetValue("MobanID")%></td>
      <td width="80" align="right">提交时间：</td>
      <td><%=GetValue("AddDate")%></td>
    </tr>
    <%=GetValue("List")%>
  </table>

</form>
</body>
</html>