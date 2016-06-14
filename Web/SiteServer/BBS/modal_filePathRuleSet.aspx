<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.Modal.FilePathRuleSet" Trace="false"%>

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

  <table class="table noborder table-hover">
    <tr id="FilePathRow" runat="server">
      <td>生成页面路径：</td>
      <td>
        <asp:TextBox  Columns="45" MaxLength="200" id="FilePath" runat="server"/>
        <asp:RegularExpressionValidator
					runat="server"
					ControlToValidate="FilePath"
					ValidationExpression="[^']+"
					ErrorMessage=" *"
          foreColor="red"
					Display="Dynamic" />
      </td>
    </tr>
    <tr>
      <td>下级板块页面命名规则：</td>
      <td>
        <asp:TextBox  Columns="45" MaxLength="200" id="FilePathRule" runat="server"/>
        <asp:Button ID="btnCreateForumRule" text="构造" class="btn" runat="server"></asp:Button>
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->