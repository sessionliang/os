<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.BBS.BackgroundPages.Modal.FaceAdd" %>

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
<bairong:alerts text="文件夹名称是在论坛smile文件夹下的表情文件夹名称" runat="server"></bairong:alerts>

  <table cellpadding="2" cellspacing="2" width="95%" class="center">
    <tr>
      <td width="120">文件夹名称：</td>
      <td>
        <asp:TextBox  Width="180" id="tbFaceName" runat="server"/>
        <asp:RequiredFieldValidator
                        ControlToValidate="tbFaceName"
                        errorMessage=" *" foreColor="red" 
                        Display="Dynamic"
                        runat="server"/></td>
    </tr>
    <tr>
      <td>表情显示名称：</td>
      <td>
        <asp:TextBox  Width="180" id="tbTitle" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="tbTitle"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    <tr>
      <td>状态：</td>
      <td>
        <asp:RadioButtonList ID="rblIsEnabled" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList>
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->