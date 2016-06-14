<%@ Page Language="C#" AutoEventWireup="true" Inherits="SiteServer.BBS.BackgroundPages.Modal.IdentifyAdd" %>

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
      <td>图章名称：</td>
      <td>
        <asp:TextBox Width="220" ID="tbTitle" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbTitle" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
      </td>
    </tr>
    <tr>
      <td>图章：</td>
      <td>
        <asp:TextBox Width="220" ID="tbStampUrl" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbStampUrl" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
      </td>
    </tr>
    <tr>
      <td>图标：</td>
      <td>
        <asp:TextBox Width="220" ID="tbIconUrl" runat="server" />
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->