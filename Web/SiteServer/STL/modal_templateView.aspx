<%@ Page Language="C#" validateRequest="false" Inherits="SiteServer.STL.BackgroundPages.Modal.TemplateView" Trace="false"%>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server">
<bairong:alerts runat="server"></bairong:alerts>

  <style type="text/css">
  .CodeMirror-line-numbers { width: 22px; color: #aaa; background-color: #eee; text-align: right; padding-right: .3em; font-size: 10pt; font-family: monospace; padding-top: .4em; line-height: 16px; }
  </style>
  <script src="../../SiteFiles/bairong/Scripts/CodeMirror/js/codemirror.js" type="text/javascript"></script>

  <div style="border: 1px solid #CCC; width:100%">
    <asp:TextBox width="100%" TextMode="MultiLine" id="tbContent" runat="server" Wrap="false" />
  </div>
  <script type="text/javascript">
    $(document).ready(function(){
      var isTextArea = false;
      var editor = CodeMirror.fromTextArea('tbContent', {
          height: $(window.parent.document).find('.modal-body').height() - 20 + 'px',
          parserfile: ["parsexml.js"],
          stylesheet: ["../../SiteFiles/bairong/Scripts/CodeMirror/css/xmlcolors.css"],
          path: "../../SiteFiles/bairong/Scripts/CodeMirror/js/",
          continuousScanning: 500,
          lineNumbers: true
      });
      $('#reindent').show().click(function(){
        if (!isTextArea) editor.reindent();
      });
    });
  </script>

  <hr />
  <table class="table noborder">
    <tr>
      <td class="center">
        <asp:Button class="btn btn-primary" id="btnSubmit" text="确 定"  runat="server" onClick="Submit_OnClick" />
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->