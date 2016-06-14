<%@ Page Language="C#" Trace="false" EnableViewState="false" Inherits="SiteServer.CMS.BackgroundPages.BackgroundBasePage" %>

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
<form runat="server">
	<table class="table noborder table-condensed table-hover">
		<tr class="info thead">
		  <td onclick="location.reload();">
		  	<lan>栏目列表</lan>
		  </td>
		</tr>
	  <site:NodeTree runat="server"></site:NodeTree>
	</table>
</form>
</body>
</html>
<!-- check for 3.6 html permissions -->