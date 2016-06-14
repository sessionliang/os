<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.Modal.AccountView" Trace="false"%>

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
      <td align="right">客户名称：</td>
      <td colspan="3"><%=GetValue("AccountName")%></td>
    </tr>
    <tr>
      <td width="80" align="right">添加人：</td>
      <td><%=GetValue("AddUserName")%></td>
      <td width="80" align="right">负责人：</td>
      <td><%=GetValue("ChargeUserName")%></td>
    </tr>
    <tr>
      <td align="right">状态：</td>
      <td><%=GetValue("Status")%></td>
      <td align="right">客户等级：</td>
      <td><%=GetValue("Priority")%></td>
    </tr>
    <tr>
      <td align="right">行业：</td>
      <td><%=GetValue("BusinessType")%></td>
      <td align="right">分类：</td>
      <td colspan="3"><%=GetValue("Classification")%></td>
    </tr>
    <tr>
      <td align="right">网址：</td>
      <td><%=GetValue("Website")%></td>
      <td align="right">电话：</td>
      <td><%=GetValue("Telephone")%></td>
    </tr>
    <tr>
      <td align="right">所处地区：</td>
      <td colspan="3"><%=GetValue("Province")%> <%=GetValue("City")%> <%=GetValue("Area")%></div>
      </td>
    </tr>
    <tr>
      <td align="right">详细地址：</td>
      <td colspan="3"><%=GetValue("Address")%></td>
    </tr>
    <tr>
      <td align="right">客户简介：</td>
      <td colspan="3"><%=GetValue("Description")%></td>
    </tr>
    <tr>
      <td align="right">聊天记录<br />或者<br />活动记录：</td>
      <td colspan="3"><%=GetValue("ChatOrNote")%></td>
    </tr>
  </table>

</form>
</body>
</html>