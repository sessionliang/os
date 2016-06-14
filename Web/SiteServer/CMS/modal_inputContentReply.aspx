<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.CMS.BackgroundPages.Modal.InputContentReply" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
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
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td width="80">
            <asp:Literal id="ltlDataKey" runat="server" />：
          </td>
          <td><asp:Literal id="ltlDataValue" runat="server" /></td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
    <tr>
      <td colspan="2"><bairong:BREditor id="breReply" runat="server"></bairong:BREditor></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->