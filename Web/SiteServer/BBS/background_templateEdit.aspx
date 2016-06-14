<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.BBS.BackgroundPages.BackgroundTemplateEdit" %>

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
	function changeExtension(sel,tb,holder){
	    tb.value = sel.options[sel.options.selectedIndex].value;
	    if (sel.options[sel.options.selectedIndex].value==""){
	        holder.style.display = '';
	    }else{
	        holder.style.display = 'none';
	    }
	}
  </script>
  <style type="text/css">
	.CodeMirror-line-numbers {
	width: 22px;
	color: #aaa;
	background-color: #eee;
	text-align: right;
	padding-right: .3em;
	font-size: 10pt;
	font-family: monospace;
	padding-top: .4em;
	line-height: 16px;
	}
  </style>
  <script src="../../SiteFiles/bairong/Scripts/CodeMirror/js/codemirror.js" type="text/javascript"></script>

  <div class="popover popover-static">
    <h3 class="popover-title">修改模板文件</h3>
    <div class="popover-content">
    
      <table class="table noborder">
		<tr>
			<td>
			模板文件：<asp:Literal ID="ltlFilePath" runat="server"></asp:Literal>
		  </td>
		</tr>
		<tr>
			<td class="center">
				<div style="border: 1px solid #CCC; width:100%">
                <asp:TextBox  width="100%" TextMode="MultiLine" id="Content" runat="server" Height="460" Wrap="false" />
                </div>
                <script type="text/javascript">
$(document).ready(function(){
var editor = CodeMirror.fromTextArea('Content', {
height: "460px",
parserfile: ["parsexml.js", "parsecss.js", "tokenizejavascript.js", "parsejavascript.js", "parsehtmlmixed.js"],
stylesheet: ["../../SiteFiles/bairong/Scripts/CodeMirror/css/xmlcolors.css", "../../SiteFiles/bairong/Scripts/CodeMirror/css/jscolors.css", "../../SiteFiles/bairong/Scripts/CodeMirror/css/csscolors.css"],
path: "../../SiteFiles/bairong/Scripts/CodeMirror/js/",
continuousScanning: 500,
lineNumbers: true
});

});
</script>                                   

			  </td>
			</tr>
	    </table>
  
      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button class="btn btn-primary" text="确 定" onclick="Submit_OnClick" runat="server" />
			<input type=button class="btn" onClick="location.href='background_templateList.aspx?publishmentSystemID=<%=PublishmentSystemID%>&templateDir=<%=Request.QueryString["templateDir"]%>&directoryName=<%=Request.QueryString["directoryName"]%>';" value="返 回" />
          </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->