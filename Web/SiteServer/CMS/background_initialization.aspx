<%@ Page Language="c#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundInitialization" Trace="False"%>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<title>系统初始化...</title>
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server">
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <script language="javascript">
	if (window.top != self){
		window.top.location = self.location;
	}
  </script>

  <div class="well" style="margin-top:20px;">
    <table class="table table-noborder">
      <tr>
        <td class="center">
          <asp:Literal ID="ltlContent" runat="server"></asp:Literal>
        </td>
      </tr>
    </table>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->