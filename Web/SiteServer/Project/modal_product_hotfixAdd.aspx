<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.HotfixAdd" Trace="false"%>

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
    <tr>
      <td width="120">版本号：</td>
      <td>
        <asp:TextBox id="tbVersion" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbVersion" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbVersion" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
    <tr>
      <td width="120">补丁号：</td>
      <td>
        <asp:TextBox id="tbHotfix" runat="server" />
        <span>只允许数字</span>
      </td>
    </tr>
    <tr>
      <td>下载地址：</td>
      <td>
        <asp:TextBox id="tbFileUrl" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbFileUrl" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbFileUrl" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
    <tr>
      <td>升级说明地址：</td>
      <td>
        <asp:TextBox id="tbPageUrl" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbPageUrl" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
    <tr>
      <td>发布日期：</td>
      <td>
        <bairong:DateTimeTextBox id="tbPubDate" now="true" runat="server" />
      </td>
    </tr>
    <tr>
      <td>文字说明：</td>
      <td>
        <asp:TextBox id="tbMessage" runat="server" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="tbMessage" ValidationExpression="[^',]+" errorMessage=" *" foreColor="red" display="Dynamic" />
      </td>
    </tr>
    <tr>
      <td>是否开启：</td>
      <td><asp:RadioButtonList ID="rblIsEnabled" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList></td>
    </tr>
    <tr>
      <td>升级限制：</td>
      <td><asp:RadioButtonList ID="rblIsRestrict" AutoPostBack="true" OnSelectedIndexChanged="rblIsRestrict_SelectedIndexChanged" RepeatDirection="Horizontal" runat="server"></asp:RadioButtonList></td>
    </tr>
    <asp:PlaceHolder ID="phRestrict" runat="server" Visible="true">
      <tr>
        <td>允许升级域名：</td>
        <td><asp:TextBox id="tbRestrictDomain" runat="server" /></td>
      </tr>
      <tr>
        <td>允许升级产品：</td>
        <td>
          <asp:TextBox id="tbRestrictProductIDCollection" runat="server" />
          <span>多个产品用“,”分隔</span>
        </td>
      </tr>
      <tr>
        <td>允许升级数据库类型：</td>
        <td><asp:TextBox id="tbRestrictDatabase" runat="server" /></td>
      </tr>
      <tr>
        <td>允许升级版本：</td>
        <td><asp:TextBox id="tbRestrictVersion" runat="server" /></td>
      </tr>
      <tr>
        <td>允许升级补丁：</td>
        <td><asp:TextBox id="tbRestrictHotfix" runat="server" /></td>
      </tr>
    </asp:PlaceHolder>
  </table>
  
</form>
</body>
</html>
