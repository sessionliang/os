<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.StlTemplate.StlTemplateEdit" validateRequest="false" %>

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

  <table class="table table-noborder">
    <tr>
      <td class="align-right">
        <asp:Button class="btn" id="btnEditorType" text="采用代码编辑模式" onclick="EditorType_OnClick" runat="server" />
        <input id="reindent" style="display:none" type="button" class="btn" onClick="javascript:;" value="对代码应用格式" />
      </td>
    </tr>
    <tr>
      <td>
        <div style="border: 1px solid #CCC; width:100%">
          <asp:TextBox width="99%" TextMode="MultiLine" id="tbContent" runat="server" Height="450" Wrap="false" />
        </div>
        <asp:PlaceHolder id="phCodeMirror" runat="server">
        <script type="text/javascript">
          $(document).ready(function(){
            var isTextArea = false;
            var editor = CodeMirror.fromTextArea('tbContent', {
                height: "450px",
                parserfile: ["parsexml.js"],
                stylesheet: ["../../SiteFiles/bairong/Scripts/CodeMirror/css/xmlcolors.css"],
                path: "../../SiteFiles/bairong/Scripts/CodeMirror/js/",
                continuousScanning: 500,
                lineNumbers: true
            });
            $('#reindent').show().click(function(){
              if (!isTextArea) editor.reindent();
            });
            setInterval(function() {
              $('#tbContent').val(editor.getCode());
            }, 10);
          });
        </script>
        </asp:PlaceHolder>
      </td>
    </tr>
  </table>

  <hr />
  <table class="table noborder table-condensed">
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