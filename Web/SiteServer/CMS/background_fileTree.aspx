<%@ Page Language="C#" Trace="false" EnableViewState="false" Inherits="SiteServer.CMS.BackgroundPages.BackgroundFileTree" %>

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
	  <tr class="info">
	    <td style="padding-left:60px;">
	      <a href="jabascript:;" onclick="location.reload();" title="点击刷新文件夹"><lan>文件夹</lan></a></td>
	  </tr>
	  <site:DirectoryTree runat="server"></site:DirectoryTree>
	</table>
</form>
</body>
</html>
<!-- check for 3.6 html permissions -->