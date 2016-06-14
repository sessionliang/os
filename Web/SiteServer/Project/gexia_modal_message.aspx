<%@ Page Language="C#" Inherits="SiteServer.GeXia.BackgroundPages.Modal.Message" Trace="false"%>

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

  <table id="contents" class="table table-bordered table-hover">
    <tr class="info thead">
      <td>次序 </td>
      <td>标题 </td>
      <td>发送短信</td>
      <td>发送邮件</td>
      <td>短信模板</td>
      <td></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center"><asp:Literal ID="ltlIndex" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlTitle" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlIsSMS" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlIsEmail" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlTemplateSMS" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlStatus" runat="server"></asp:Literal></td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

</form>
</body>
</html>