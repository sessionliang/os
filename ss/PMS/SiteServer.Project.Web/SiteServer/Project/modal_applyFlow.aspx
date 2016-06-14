<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.ApplyFlow" Trace="false"%>

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

<style>
.flowItem{
width: 200px; color: #15418a; line-height: 20px; padding-top: 5px; padding-right: 5px; padding-bottom: 5px; padding-left: 5px; border-top-color: #7fa5fa; border-right-color: #7fa5fa; border-bottom-color: #7fa5fa; border-left-color: #7fa5fa; border-top-width: 1px; border-right-width: 1px; border-bottom-width: 1px; border-left-width: 1px; border-top-style: solid; border-right-style: solid; border-bottom-style: solid; border-left-style: solid; background-color: rgb(230, 238, 249);  
}
</style>

  <table width="60" border="0" class="center" cellPadding="6" cellSpacing="6">
    <tbody>
      <tr>
        <td class="center">
        <asp:Literal ID="ltlFlows" runat="server"></asp:Literal>
    </td>
      </tr>
    </tbody>
  </table>

</form>
</body>
</html>
