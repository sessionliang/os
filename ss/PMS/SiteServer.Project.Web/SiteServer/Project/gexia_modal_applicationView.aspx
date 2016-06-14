<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.GeXia.BackgroundPages.Modal.ApplicationView" Trace="false"%>

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
		<td align="left" width="120">序号：</td>
	  <td align="left">
      	<asp:Literal ID="ltlID" runat="server"></asp:Literal>
      </td>
	  </tr>
      <tr><td align="left">申请资源：</td>
	  <td align="left">
      	<asp:Literal id="ltlApplicationType" runat="server"/>
      </td>
	  </tr>
      <tr><td align="left">申请类型：</td>
	  <td align="left">
      	<asp:Literal id="ltlApplyResource" runat="server"/>
      </td>
	  </tr>
      <tr><td align="left">联系人：</td>
	  <td align="left">
      	<asp:Literal ID="ltlContactPerson" runat="server"></asp:Literal>
      </td>
	  </tr>
      <tr><td align="left">联系邮箱：</td>
	  <td align="left">
      	<asp:Literal ID="ltlEmail" runat="server"></asp:Literal>
      </td>
	  </tr>
      <tr><td align="left">移动电话：</td>
	  <td align="left">
      	<asp:Literal ID="ltlMobile" runat="server"></asp:Literal>
      </td>
	  </tr>
    <tr><td align="left">QQ：</td>
    <td align="left">
        <asp:Literal ID="ltlQQ" runat="server"></asp:Literal>
      </td>
    </tr>
    <tr>
    	<td align="left">固定电话：</td>
	  	<td align="left">
      	<asp:Literal ID="ltlTelephone" runat="server"></asp:Literal>
      </td>
	  </tr>
	  <tr>
    	<td align="left">地区：</td>
	  	<td align="left">
      	<asp:Literal ID="ltlLocation" runat="server"></asp:Literal>
      </td>
	  </tr>
	  <tr>
    	<td align="left">详细地址：</td>
	  	<td align="left">
      	<asp:Literal ID="ltlAddress" runat="server"></asp:Literal>
      </td>
	  </tr>
	  <tr>
    	<td align="left">单位类型：</td>
	  	<td align="left">
      	<asp:Literal ID="ltlOrgType" runat="server"></asp:Literal>
      </td>
	  </tr>
	  <tr>
    	<td align="left">单位名称：</td>
	  	<td align="left">
      	<asp:Literal ID="ltlOrgName" runat="server"></asp:Literal>
      </td>
	  </tr>
	  <tr>
    	<td align="left">属于IT部门：</td>
	  	<td align="left">
      	<asp:Literal ID="ltlIsITDepartment" runat="server"></asp:Literal>
      </td>
	  </tr>
	  <tr>
    	<td align="left">附言：</td>
	  	<td align="left">
      	<asp:Literal ID="ltlComment" runat="server"></asp:Literal>
      </td>
	  </tr>
	  <tr>
    	<td align="left">IP地址：</td>
	  	<td align="left">
      	<asp:Literal ID="ltlIPAddress" runat="server"></asp:Literal>
      </td>
	  </tr>
	  <tr>
    	<td align="left">申请时间：</td>
	  	<td align="left">
      	<asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
      </td>
	  </tr>
	  
	</table>

</form>
</body>
</html>