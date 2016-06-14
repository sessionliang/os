<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.ProjectDocumentAdd" Trace="false"%>

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
      <td width="100">上传文件：</td>
      <td>
          <input type=file  id=myFile size="35" runat="server"/>
          <asp:RequiredFieldValidator ControlToValidate="myFile" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
      </td>
    </tr>
    <tr>
      <td>文件说明：</td>
      <td><asp:TextBox id="tbDescription" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="40" style="height:50px" TextMode="MultiLine" MaxLength="50" runat="server" /></td>
    </tr>
  </table>
  
</form>
</body>
</html>
