<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundParameter" %>

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
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <table class="table table-bordered table-hover">
    <tr class="info thead">
      <td width="140">参数名称</td>
      <td>值</td>
    </tr>
    <%=PrintParameter()%>
</form>
</body>
</html>
<!-- check for 3.6 html permissions -->