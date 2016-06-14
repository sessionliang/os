<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.Modal.SpecAdd" Trace="false"%>

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
      <td width="120">规格名称：</td>
      <td>
        <asp:TextBox  Columns="25" MaxLength="50" id="SpecName" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="SpecName" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="SpecName" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
    <tr>
      <td>显示方式：</td>
      <td><asp:RadioButtonList ID="IsIcon" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList></td>
    </tr>
    <tr>
      <td>是否多选项：</td>
      <td><asp:RadioButtonList ID="IsMultiple" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList></td>
    </tr>
    <tr>
      <td>是否必选项：</td>
      <td><asp:RadioButtonList ID="IsRequired" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList></td>
    </tr>
    <tr>
      <td>规格备注：</td>
      <td>
        <asp:TextBox TextMode="MultiLine" Rows="4"  Columns="50" id="Description" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="Description" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
  </table>
  
</form>
</body>
</html>
<!-- check for 3.6.4 html permissions -->