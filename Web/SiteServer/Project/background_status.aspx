<%@ Page Language="C#" Inherits="BRS.BackgroundPages.BackgroundStatus" %>
<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>


<html>
<head>
<link rel="stylesheet" href="../../../../../SiteServer/style.css" type="text/css" />
<bairong:custom type="style" runat="server"></bairong:custom>

<bairong:Code type="Prototype" runat="server" />
<bairong:Code type="Scriptaculous" runat="server" />

<meta http-equiv="content-type" content="text/html;charset=utf-8">
</head>
<body>
		<form id="myForm" runat="server">
<bairong:Message runat="server"></bairong:Message>

<DIV class="column">
<div class="columntitle">服务状态</div>

		<table cellspacing="3" cellpadding="3" Align="center" border="0" style="width:100%;">
	<tr class="summary-title" align="Center" style="height:25px;">
		<td>短信余额</td><td><asp:Literal ID="ltlRemainding" runat="server"></asp:Literal>
		条</td>
	</tr>
</table>

	
</div>
</form>
</body>
</html>