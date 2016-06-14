<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundForumTranslate" %>

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
  <bairong:alerts runat="server" />

  <script>
	function setOptionColor(obj) {
		for (var i=0;i<obj.options.length;i++) {
			if (obj.options[i].value=="") 
				obj.options[i].style.color="gray";
			else
				obj.options[i].style.color="black";
		}
	}
  </script>

  <div class="popover popover-static">
    <h3 class="popover-title">版块合并</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
			<td width="140">从版块：</td>
	  		<td>
	  			<asp:ListBox ID="ForumIDFrom" Height="300" style="width:auto" SelectionMode="Multiple" runat="server"></asp:ListBox>
	  			<asp:RequiredFieldValidator
					ControlToValidate="ForumIDFrom"
					errorMessage=" *" foreColor="red" 
					Display="Dynamic"
					runat="server"/>
				<script type="text/javascript" language="javascript">
				setOptionColor(document.getElementById('<%=ForumIDFrom.ClientID%>'));
				</script>
			</td>
	 	</tr>
		<tr>
	  		<td>合并到：</td>
	  		<td> 
	  			<asp:DropDownList ID="ForumIDTo" runat="server"></asp:DropDownList>
  				<asp:RequiredFieldValidator
					ControlToValidate="ForumIDTo"
					errorMessage=" *" foreColor="red" 
					Display="Dynamic"
					runat="server"/>
				<script type="text/javascript" language="javascript"> 
				setOptionColor(document.getElementById('<%=ForumIDTo.ClientID%>'));
				</script>
			</td>
		</tr>
      </table>
  
      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
          	<asp:Button class="btn btn-primary" id="Submit" text="合 并" onclick="Submit_OnClick" runat="server"/>
	        <asp:PlaceHolder ID="phReturn" runat="server">
	      	<input class="btn" type="button" onClick="location.href='<%=ReturnUrl%>';return false;" value="返 回" />
	        </asp:PlaceHolder>
          </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->