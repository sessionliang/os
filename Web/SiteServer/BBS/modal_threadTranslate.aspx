<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.BBS.BackgroundPages.Modal.ThreadTranslate" Trace="false"%>

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
			<td width="150">转移到版块或分区：</td>
			<td>
				<asp:DropDownList ID="ddlTargetForumID" runat="server"></asp:DropDownList>
			</td>
		</tr>
    </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->