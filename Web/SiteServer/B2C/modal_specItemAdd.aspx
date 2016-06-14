<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.Modal.SpecItemAdd" Trace="false"%>

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
      <td width="120">规格值名称：</td>
      <td>
        <asp:TextBox  Columns="25" MaxLength="50" id="tbTitle" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbTitle" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbTitle" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
    <asp:PlaceHolder ID="phIcon" runat="server">
    <tr>
      <td>图标：</td>
      <td>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="300"><input id="IconUrlUploader" style="display:none;" type="file" runat="server" />
              <asp:TextBox  Columns="40" id="IconUrl" runat="server" style="display:" /></td>
            <td rowspan="2" style="padding-left:0px;"><IMG src='<%=GetPreviewImageSrc()%>' alt=图标预览 align="absmiddle" name="preview_IconUrl" id="preview_IconUrl"></td>
          </tr>
          <tr>
            <td valign="top"><a id="iconUrlLink1" style="" href="javascript:;" onClick="document.getElementById('iconUrlLink2').style.fontWeight = '';this.style.fontWeight = 'bold';document.getElementById('IconUrlUploader').style.display = '';document.getElementById('IconUrl').style.display = 'none'">上传图标</a>&nbsp; <a id="iconUrlLink2" style="font-weight:bold" href="javascript:;" onClick="document.getElementById('iconUrlLink1').style.fontWeight = '';this.style.fontWeight = 'bold';document.getElementById('IconUrlUploader').style.display = 'none';document.getElementById('IconUrl').style.display = ''">输入 URL</a></td>
          </tr>
        </table>
      </td>
    </tr>
    </asp:PlaceHolder>
  </table>
  
</form>
</body>
</html>
<!-- check for 3.6.4 html permissions -->