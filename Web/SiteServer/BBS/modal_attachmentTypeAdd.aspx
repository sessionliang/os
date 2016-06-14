<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.BBS.BackgroundPages.Modal.AttachmentTypeAdd" %>

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
      <td>附件后缀： </td>
      <td>
        <asp:TextBox  Width="180" id="tbFileExtName" runat="server"/>
        <asp:RequiredFieldValidator
          ControlToValidate="tbFileExtName"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
      </td>
    </tr>
    <tr>
      <td>附件最大大小： </td>
      <td>
        <asp:TextBox  Width="60" id="tbMaxSize" runat="server"/> K
        <asp:RequiredFieldValidator
          ControlToValidate="tbMaxSize"
          errorMessage=" *" foreColor="red" 
          Display="Dynamic"
          runat="server"/>
        <span class="gray">(设置为0可禁止用户上传此类型附件)</span>
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->