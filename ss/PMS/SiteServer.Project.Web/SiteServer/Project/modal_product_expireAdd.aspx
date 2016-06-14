<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.ExpireAdd" Trace="false"%>

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
      <td width="120">是否限制：</td>
      <td><asp:RadioButtonList ID="rblIsExpire" AutoPostBack="true" OnSelectedIndexChanged="rblIsExpire_SelectedIndexChanged" class="radiobuttonlist" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList></td>
    </tr>
    <asp:PlaceHolder ID="phIsExpire" runat="server" Visible="true">
      <tr>
        <td>试用到期时间：</td>
        <td><bairong:DateTimeTextBox id="tbExpireDate" runat="server" /></td>
      </tr>
      <tr>
        <td>限制原因：</td>
        <td>
          <asp:TextBox id="tbExpireReason" style="height:50px;width:90%" TextMode="MultiLine" runat="server" />
        </td>
      </tr>
    </asp:PlaceHolder>
  </table>
  
</form>
</body>
</html>
