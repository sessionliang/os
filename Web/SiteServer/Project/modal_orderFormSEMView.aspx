<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.Modal.OrderFormSEMView" Trace="false"%>

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
      <td width="140" align="right">优化网址：</td>
      <td>
        <%=GetValue("Domain")%>
      </td>
    </tr>
    <tr>
      <td align="right">标题（Title）：</td>
      <td>
        <%=GetValue("Title")%>
      </td>
    </tr>
    <tr>
      <td align="right">关键词（KeyWords）：</td>
      <td>
        <%=GetValue("Keywords")%>
      </td>
    </tr>
    <tr>
      <td align="right">描述（Description）：</td>
      <td>
        <%=GetValue("Description")%>
      </td>
    </tr>
    <tr>
      <td align="right">提交时间：</td>
      <td>
        <%=GetValue("AddDate")%>
      </td>
    </tr>
  </table>

</form>
</body>
</html>