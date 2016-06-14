<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.WeiXin.BackgroundPages.Modal.KeywordAddNews" Trace="false"%>

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
      <td width="120">关键词：</td>
      <td colspan="3">
        <asp:TextBox id="tbKeywords" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbKeywords" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
        <span class="gray">多个关键词请用空格格开：例如: 微信 腾讯</span>
      </td>
    </tr>
    <tr>
      <td>匹配规则：</td>
      <td width="240">
        <asp:DropDownList id="ddlMatchType" runat="server" />
      </td>
      <td width="80">是否启用：</td>
      <td class="checkbox">
        <asp:CheckBox id="cbIsEnabled" text="启用关键字"  runat="server" />
      </td>
    </tr>
    <asp:PlaceHolder id="phSelect" visible="false" runat="server">
    <tr>
      <td>从微官网选择：</td>
      <td class="checkbox" colspan="3">
        <asp:CheckBox id="cbIsSelect" checked="true" text="选择微官网内容" runat="server" />
      </td>
    </tr>
    </asp:PlaceHolder>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->