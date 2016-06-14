<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.Modal.InvoiceAdd" %>

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
      <td width="100">发票类型：</td>
      <td>
        <asp:RadioButtonList id="rblIsCompany" OnSelectedIndexChanged="rblIsCompany_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" class="radiobuttonlist" runat="server"></asp:RadioButtonList>
      </td>
    </tr>
    <asp:PlaceHolder id="phCompany" runat="server">
    <tr>
      <td>发票抬头：</td>
      <td>
        <asp:TextBox ID="tbInvoiceTitle" runat="server"></asp:TextBox>
      </td>
    </tr>
    </asp:PlaceHolder>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->