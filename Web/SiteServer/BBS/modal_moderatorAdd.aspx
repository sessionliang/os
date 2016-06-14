<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.BBS.BackgroundPages.Modal.ModeratorAdd" %>

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
<bairong:alerts text="多个版主用“,”隔开" runat="server"></bairong:alerts>

  <table class="table table-noborder table-hover">
    <tr>
      <td width="180">版主：</td>
      <td><asp:TextBox  Width="220" id="tbUserName" runat="server"/></td>
    </tr>
    <tr>
      <td>将版主的权力继承到下级版块：</td>
      <td>
        <asp:RadioButtonList ID="rblIsInherit" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList>
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->