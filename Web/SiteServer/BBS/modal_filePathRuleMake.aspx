<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.Modal.FilePathRuleMake" Trace="false"%>

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

  <script language="JavaScript" type="text/JavaScript">
	function AddOnPos(obj, charvalue)
	{
		//obj代表要插入字符的输入框
		//value代表要插入的字符
		
		obj.focus();
		var r = document.selection.createRange();
		var ctr = obj.createTextRange();
		var i;
		var s = obj.value;
		
		//注释掉的这种方法只能用在单行的输入框input内
		//对多行输入框textarea无效
		//r.setEndPoint("StartToStart", ctr);
		//i = r.text.length;
		//取到光标位置----Start----
		var ivalue = "&^asdjfls2FFFF325%$^&"; 
		r.text = ivalue;
		i = obj.value.indexOf(ivalue);
		r.moveStart("character", -ivalue.length);
		r.text = "";
		//取到光标位置----End----
		//插入字符
		obj.value = s.substr(0,i) + charvalue + s.substr(i,s.length);
		ctr.collapse(true);
		ctr.moveStart("character", i + charvalue.length);
		ctr.select();
	}
  </script>

  	<table class="table table-noborder table-hover">
		<tr>
			<td colspan="2">
			  <div class="columnsubtitle">替换内容</div>
			  <table cellspacing="2" cellpadding="2" class="center" border="0" style="width:100%;">
				  <tr class="summary-title" class="center" style="height:25px;">
					<td width="0">规则</td>
					<td width="0">含义</td>
					<td width="0">规则</td>
					<td width="0">含义</td>
				  </tr>
				  <asp:Literal ID="Rules" runat="server"></asp:Literal>
			  </table>
			</td>
		</tr>
		<tr>
			<td width="150" class="center">
				页面命名规则：
			</td>
			<td>
				<asp:TextBox  Columns="50" id="TheRule" runat="server" />
			</td>
		</tr>
    </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->