<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.FrameworkLeft" Trace="False" enableViewState = "false"%>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<style type="text/css">
body { padding:0; margin:0; }
.container,.dropdown,.dropdown-menu {margin-left: 6px; float: none;}
.navbar, .navbar-inner, .nav{margin-bottom: 5px; padding:0; }
.dropdown,.dropdown-toggle{width:100%}
.navbar-inner {height: 35px; min-height: 35px;}
.table-condensed td {padding:2px 5px;}
</style>
<!--[if IE]>
<style type="text/css">
.navbar-inner {height: 40px; min-height: 40px;}
.dropdown {margin-left: 0;}
</style>
<![endif]--> 
<div class="container" style="height:50px;width:153px;">
<div class="navbar navbar-fixed-top">
  <div class="navbar-inner">
  	<ul class="nav">
	  <li><a href="#" style="font-size:14px; padding-left:20px;"><asp:Literal ID="ltlTitle" runat="server"/></a></li>
	</ul>
  </div>
</div>
</div>

<asp:Literal ID="ltlScript" runat="server"></asp:Literal>
<form runat="server">
  <table class="table table-condensed noborder table-hover">
    <bairong:NavigationTree ID="navigationTree" runat="server" />
  </table>
</form>

</body>
</html>
<!-- check for 3.6 html permissions -->