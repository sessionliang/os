<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.UrlSummaryAdd" Trace="false"%>

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
        <td>
          <bairong:BREditor id="brSummary" runat="server" />
        </td>
      </tr>
    </asp:PlaceHolder>
  </table>
  
</form>
</body>
</html>
