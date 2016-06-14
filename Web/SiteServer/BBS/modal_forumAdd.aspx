<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.BBS.BackgroundPages.Modal.ForumAdd" Trace="false"%>

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
<bairong:alerts text="版块或分区之间用换行分割，下级版块在版块前添加“－”字符，索引可以放到括号中，如：<br />版块一<br />－下级版块" runat="server"></bairong:alerts>

	<table class="table table-noborder table-hover">
		<tr>
			<td>
				父版块或分区：
			</td>
			<td>
				<asp:DropDownList ID="ParentForumID" runat="server"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td colspan="2" class="center">
				<asp:TextBox height="230" width="95%" TextMode="MultiLine" id="ForumNames" runat="server"/>
				<asp:RequiredFieldValidator id="RequiredFieldValidator"
					ControlToValidate="ForumNames"
					errorMessage=" *" foreColor="red" 
					Display="Dynamic"
					runat="server"/>
			</td>
		</tr>
	</table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->