<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.Modal.UploadOrUrlImage" Trace="false" %>

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

  <script type="text/javascript" language="javascript">
  <asp:Literal id="ltlScript" runat="server"></asp:Literal>
  </script>

  <table class="table table-noborder table-hover">
    <tr>
      <td width="120" class="center">选择上传图片的方式：</td>
      <td><asp:RadioButtonList ID="rblIsUpload" AutoPostBack="true" OnSelectedIndexChanged="rblIsUpload_SelectedIndexChanged" RepeatDirection="Horizontal" class="radiobuttonlist noborder" runat="server"></asp:RadioButtonList></td>
    </tr>
    <asp:PlaceHolder ID="phUpload" runat="server">
    <tr>
      <td class="center">选择上传的图片：</td>
      <td><input type=file  id=myFile size="45" runat="server"/>
        <asp:RequiredFieldValidator ControlToValidate="myFile" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" /></td>
    </tr>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phUrl" runat="server">
    <tr>
      <td class="center">输入图片地址：</td>
      <td>
        <asp:TextBox class="input_text" id="tbImageUrl" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbImageUrl" ValidationExpression="[^']+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
    </asp:PlaceHolder>
    
  </table>

</form>
</body>
</html>
<!-- check for 3.6.4 html permissions -->