<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.GeXia.BackgroundPages.Modal.AuthYearAdd" Trace="false"%>

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

  <table class="table table-noborder">
    <tr>
      <td width="120">扩增年数：</td>
      <td>
        <asp:TextBox id="tbYear" class="input-mini" runat="server" />
        <asp:RegularExpressionValidator
                ControlToValidate="tbYear"
                ValidationExpression="\d+"
                Display="Dynamic"
                foreColor="red"
                ErrorMessage="必须为数字"
                runat="server"/>
        <asp:RequiredFieldValidator ControlToValidate="tbYear" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        年
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->