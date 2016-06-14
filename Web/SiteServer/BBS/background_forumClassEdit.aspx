<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.BBS.BackgroundPages.BackgroundForumClassEdit" %>

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
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts text="以下设置没有继承性，即仅对当前分区有效，不会对下级子版块产生影响。" runat="server"></bairong:alerts>

  <script language="javascript" type="text/javascript">
	function _checkCol(column, className, bcheck)
	{
		var elements = $('#' + column + '.' + className);
		for(var i=0; i<elements.length; i++){
			_checkAll(elements[i], bcheck);
		}
	}
  </script>

  <div class="popover popover-static">
    <h3 class="popover-title">编辑分区</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
        	<td width="150">分区名称：</td>
		  	<td colspan="2" >
		  		<asp:TextBox  Columns="45" MaxLength="255" id="txtForumName" runat="server"/>
				<asp:RequiredFieldValidator id="RequiredFieldValidator"
				ControlToValidate="txtForumName"
				errorMessage=" *" foreColor="red" 
				Display="Dynamic"
				runat="server"/>
		  </td> 
		</tr>
		<tr>
			<td width="150">分区索引：</td>
		  	<td colspan="2" >
		  		<asp:TextBox  Columns="45" MaxLength="255" id="txtIndexName" runat="server"/>
				<asp:RegularExpressionValidator
				runat="server"
				ControlToValidate="txtIndexName"
				ValidationExpression="[^']+"
				errorMessage=" *" foreColor="red" 
				Display="Dynamic" />
			</td>
		</tr>
		<tr>
			<td width="150">分区名称颜色：</td>
		  	<td>
		  	  	<asp:TextBox  Columns="45" MaxLength="200" id="txtColor" runat="server"/>
				<asp:RegularExpressionValidator
				runat="server"
				ControlToValidate="txtColor"
				ValidationExpression="[^']+"
				errorMessage=" *" foreColor="red" 
				Display="Dynamic" />
			</td>
		</tr> 
		<tr id="LinkTypeRow" runat="server">
			<td>下级子分区横排：</td>
		  	<td colspan="2">
			  <asp:DropDownList id="ddlColumns" runat="server">		 
	          	<asp:ListItem Text="0" Value="0" Selected="true"></asp:ListItem>
	          	<asp:ListItem Text="1" Value="1" ></asp:ListItem>
	          	<asp:ListItem Text="2" Value="2"></asp:ListItem>
	          	<asp:ListItem Text="3" Value="3"></asp:ListItem>
	          	<asp:ListItem Text="4" Value="4"></asp:ListItem>
	          	<asp:ListItem Text="5" Value="5"></asp:ListItem>
	          </asp:DropDownList>
	        <br />
	        <span>设置下级子分区横排时每行分区数量，如果设置为 0，则按正常方式排列</span>
	      </td>
		</tr> 
		<tr>
			<td>分区状态：</td>
		  	<td colspan="2">
		  	  <asp:RadioButtonList ID="rblThreadState" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" runat="server"> 
	          </asp:RadioButtonList>
	          <span>选择“否”将暂时将分区隐藏不显示，但分区内容仍将保留，且用户仍可通过 URL 访问到此分区及其版块</span>
	      </td>
		</tr> 
      </table>
  
      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button class="btn btn-primary" id="Submit" text="修 改" onclick="Submit_OnClick" runat="server"/>
		    <input class="btn" type="button" onClick="location.href='background_forum.aspx';return false;" value="返 回" />
          </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->