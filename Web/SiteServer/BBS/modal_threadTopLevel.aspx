
<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.BBS.BackgroundPages.Modal.ThreadTopLevel" Trace="false"%>

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
			<td width="100">批理置顶：</td>
			<td>
				<asp:RadioButtonList ID="rblTopLevel" AutoPostBack="true"  RepeatDirection="Vertical" runat="server" OnSelectedIndexChanged="TopLevel_SelectedIndexChanged">
					<asp:ListItem Text="解除" Value="0" Selected="true"></asp:ListItem>
				    <asp:ListItem Text="置顶I" Value="1"></asp:ListItem>
				    <asp:ListItem Text="置顶II" Value="2"></asp:ListItem>
				    <asp:ListItem Text="置顶III" Value="3"></asp:ListItem>
				</asp:RadioButtonList>
			</td>
		</tr>  
		<asp:PlaceHolder ID="phCheck" runat="server" Visible="false">
		<tr>
			<td>置顶时长：</td>
			<td>
				<asp:TextBox class="input-small" id="txtTopLevelDate" runat="server" /> 天 
			</td>
		</tr>
		</asp:PlaceHolder> 
    </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->