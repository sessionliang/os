﻿<%@ Page Language="C#" Inherits="SiteServer.WCM.BackgroundPages.BackgroundGovPublicCategory" %>

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
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <table class="table table-bordered table-hover">
    <tr class="info thead">
      <td>名称</td>
      <td style="width:200px;">代码</td>
      <td class="center" style="width:30px;">上升</td>
      <td class="center" style="width:30px;">下降</td>
      <td class="center" style="width:50px;">&nbsp;</td>
      <td width="20" class="center">
        <input onclick="_checkFormAll(this.checked)" type="checkbox" />
      </td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <asp:Literal id="ltlHtml" runat="server" />
      </itemtemplate>
    </asp:Repeater>
  </table>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="AddChannel" Text="添 加" runat="server" />
    <asp:Button class="btn" id="Delete" Text="删 除" runat="server" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->