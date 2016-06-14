<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.BBS.BackgroundPages.Modal.LinkAdd" %>

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

  <table class="table table-noborder table-hover">
    <tr>
      <td width="100">链接名称：</td>
      <td>
        <asp:TextBox  Width="220" id="txtLinkName" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="txtLinkName"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    <tr>
      <td>链接地址：</td>
      <td>
        <asp:TextBox  Width="220" id="txtLinkUrl" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="txtLinkUrl"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    <tr>
      <td>LOGO地址：</td>
      <td>
        <asp:TextBox  Width="220" id="txtIconUrl" runat="server"/>
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->