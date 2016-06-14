<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.Modal.UserConfigMenuAdd" Trace="false"%>

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
      <td width="80">菜单标识：</td>
      <td><asp:TextBox id="tbID" MaxLength="50" Size="30" runat="server"/></td>
    </tr>
    <tr>
      <td>菜单名称：</td>
      <td>
        <asp:TextBox id="tbText" MaxLength="50" Size="30" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="tbText"
          ErrorMessage=" *" foreColor="red"
          Display="Dynamic"
          runat="server" />
      </td>
    </tr>
    <asp:PlaceHolder ID="phLink" runat="server">
    <tr>
      <td>链接：</td>
      <td><asp:TextBox id="tbHref" Size="30" runat="server"/></td>
    </tr>
    <tr>
      <td>目标：</td>
      <td><asp:TextBox id="tbTarget" MaxLength="50" Size="30" runat="server"/></td>
    </tr>
    </asp:PlaceHolder>
    <tr>
      <td>权限：</td>
      <td><asp:CheckBoxList ID="cblPermissions" RepeatDirection="Horizontal" RepeatColumns="3" runat="server"></asp:CheckBoxList></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->