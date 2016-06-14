<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.TagStyleGovInteractApplyAdd" Trace="false"%>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
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
      <td width="120">允许匿名提交：</td>
      <td><asp:RadioButtonList ID="IsAnomynous" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="允许" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="不允许" Value="False"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <tr>
      <td>显示验证码：</td>
      <td><asp:RadioButtonList ID="IsValidateCode" RepeatDirection="Horizontal" class="noborder" runat="server">
          <asp:ListItem Text="显示" Value="True" Selected="true"></asp:ListItem>
          <asp:ListItem Text="不显示" Value="False"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->