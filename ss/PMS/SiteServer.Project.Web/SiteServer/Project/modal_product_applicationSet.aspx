<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.Modal.ApplicationSet" Trace="false"%>

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
      <td width="120">处理结果：</td>
      <td>
        <asp:TextBox id="tbHandleSummary" style="height:140px" TextMode="MultiLine" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbHandleSummary" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbHandleSummary" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
  </table>
  
</form>
</body>
</html>
