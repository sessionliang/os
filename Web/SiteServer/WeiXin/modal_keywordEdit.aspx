<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.WeiXin.BackgroundPages.Modal.KeywordEdit" Trace="false"%>

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
      <td width="120">关键词：</td>
      <td><asp:TextBox id="tbKeyword" runat="server" />
        <asp:RequiredFieldValidator ControlToValidate="tbKeyword" errorMessage=" *" foreColor="red" display="Dynamic" runat="server" />
      </td>
    </tr>
    <tr>
      <td>匹配规则：</td>
      <td>
        <asp:DropDownList id="ddlMatchType" runat="server" />
      </td>
    </tr>
    <tr>
      <td>是否启用：</td>
      <td class="checkbox">
        <asp:CheckBox id="cbIsEnabled" text="启用关键字"  runat="server" />
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->