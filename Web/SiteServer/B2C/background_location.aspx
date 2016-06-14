<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundLocation" %>

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
      <td>地区名称</td>
      <td width="30">上升</td>
      <td width="30">下降</td>
      <td width="50">&nbsp;</td>
      <td width="20"><input onclick="_checkFormAll(this.checked)" type="checkbox" /></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
          <tr>
            <asp:Literal id="ltlHtml" runat="server" />
          </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="btnAdd" Text="新 增" runat="server" />
    <asp:Button class="btn" id="btnDelete" Text="删 除" runat="server" />
    <asp:Button class="btn" id="btnRestore" Visible="false" Text="删除并导入初始数据" runat="server" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->