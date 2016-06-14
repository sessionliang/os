<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.Project.BackgroundPages.Modal.LicenseView" Trace="false"%>

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

	<table class="table noborder table-hover">
	  <tr>
		<td align="left">许可产品：</td>
	  <td align="left">
      	<asp:Literal ID="ltlProductID" runat="server"></asp:Literal>
      </td>
	  </tr>
      <tr><td align="left">域名：</td>
	  <td align="left">
      	<asp:Literal id="ltlDomain" runat="server"/>
      </td>
	  </tr>
      <tr><td align="left">Mac地址：</td>
	  <td align="left">
      	<asp:Literal id="ltlMacAddress" runat="server"/>
      </td>
	  </tr>
      <tr><td align="left">许可版本：</td>
	  <td align="left">
      	<asp:Literal ID="ltlProductUser" runat="server"></asp:Literal>
      </td>
	  </tr>
      <tr><td align="left">可管理网站数：</td>
	  <td align="left">
      	<asp:Literal ID="ltlMaxSiteNumber" runat="server"></asp:Literal>
      </td>
	  </tr>
      <tr><td align="left">添加时间：</td>
	  <td align="left">
      	<asp:Literal ID="ltlLicenseDate" runat="server"></asp:Literal>
      </td>
	  </tr>
      <tr>
      	<td align="left">站点名称：</td>
			  <td align="left">
		      	<asp:Literal ID="ltlSiteName" runat="server"></asp:Literal>
		      </td>
			 </tr>
      <tr>
      	<td align="left">客户名称：</td>
	  <td align="left">
      	<asp:Literal ID="ltlClientName" runat="server"></asp:Literal>
      </td>
	  </tr>
	  <asp:PlaceHolder id="phExpireDate" runat="server">
	  	<tr>
	      <td align="left">非正版过期时间：</td>
		  	<td align="left">
	      	<asp:Literal ID="ltlExpireDate" runat="server"></asp:Literal>
	      </td>
		  </tr>
		</asp:PlaceHolder>
	</table>

	<hr />
	<table class="table noborder">
	  <tr>
	    <td class="center">
	      <asp:Button class="btn btn-success" id="btnDownload" text="下载许可证" OnClick="btnDownload_OnClick" runat="server" />
	    </td>
	  </tr>
	</table>

</form>
</body>
</html>