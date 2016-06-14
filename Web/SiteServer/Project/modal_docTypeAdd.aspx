<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.DocTypeAdd" Trace="false"%>

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

  <table width="95%" class="center" cellpadding="2" cellspacing="2">
    <tr>
      <td>父类别：</td>
      <td><asp:DropDownList id="ddlParentID" runat="server" /></td>
    </tr>
    <tr>
      <td width="120">文档类别名称：</td>
      <td><asp:TextBox id="tbTypeName" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="25" MaxLength="50" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbTypeName" errorMessage=" *" foreColor="red" display="Dynamic"
						runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbTypeName"
						ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" /></td>
    </tr>
    <tr>
      <td>文档类别备注：</td>
      <td><asp:TextBox id="tbDescription" onfocus="this.className='colorfocus';" onblur="this.className='colorblur';" Columns="40" style="height:50px" TextMode="MultiLine" MaxLength="50" runat="server" /></td>
    </tr>
    </asp:PlaceHolder>
  </table>
  
</form>
</body>
</html>
