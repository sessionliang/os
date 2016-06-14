<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.RestrictionOptions" %>

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

  <table class="table table-noborder">
    <tr>
      <td>
        <div class="alert alert-info">
          栏目页面访问限制选项：
        </div>
      </td>
    </tr>
    <tr>
      <td><asp:RadioButtonList ID="RestrictionTypeOfChannel" RepeatDirection="Vertical" runat="server"></asp:RadioButtonList></td>
    </tr>
    <tr>
      <td>
        <div class="alert alert-info">
          内容页面访问限制选项：
        </div>
      </td>
    </tr>
    <tr>
      <td><asp:RadioButtonList ID="RestrictionTypeOfContent" RepeatDirection="Vertical" runat="server"></asp:RadioButtonList></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->