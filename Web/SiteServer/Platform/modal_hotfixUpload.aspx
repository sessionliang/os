<%@ Page Language="C#" Trace="false" Inherits="BaiRong.BackgroundPages.Modal.HotfixUpload" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" enctype="multipart/form-data" method="post" runat="server">
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts text="在导入升级包之前，<font style='color:red'>建议手动备份您的数据库和程序文件</font>" runat="server"></bairong:alerts>

  <table class="table noborder table-hover">
    <tr>
      <td>补丁包：</td>
      <td>
        <input type="file" id="hifUpload" size="35" runat="server"/>
        <asp:RequiredFieldValidator ControlToValidate="hifUpload" ErrorMessage=" *" foreColor="red" Display="Dynamic" runat="server" />
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->