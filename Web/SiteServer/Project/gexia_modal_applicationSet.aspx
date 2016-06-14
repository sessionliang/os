<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.GeXia.BackgroundPages.Modal.ApplicationSet" Trace="false"%>

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

  <table class="table noborder table-hover">
    <tr>
      <td width="120">备注：</td>
      <td>
        <asp:TextBox id="tbHandleSummary" style="height:140px" TextMode="MultiLine" runat="server" />
      </td>
    </tr>
  </table>
  
</form>
</body>
</html>
