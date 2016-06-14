<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.StlTemplate.StlElementFile" validateRequest="false" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html ng-app>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server">
<bairong:alerts runat="server"></bairong:alerts>

  <bairong:code type="ajaxUpload" runat="server" />
  <script type="text/javascript" language="javascript">
  $(document).ready(function(){
    new AjaxUpload('uploadFile', {
     action: '<%=UploadUrl%>',
     name: "filedata",
     data: {},
     onSubmit: function(file, ext) {
       var reg = /^(<%=TypeCollectionForbidden%>)$/i;
       if (ext && !reg.test(ext)) {
         $('#img_upload_txt').text('上传中... ');
       } else {
         $('#img_upload_txt').text('系统不允许上传指定的格式');
         return false;
       }
     },
     onComplete: function(file, response) {
      $('#img_upload_txt').text('');
       if (response) {
         response = eval("(" + response + ")");
         if (response.success == 'true') {
           $('#tbSrc').val(response.fileUrl);
         } else {
           $('#img_upload_txt').text(response.message);
         }
       }
     }
    });
  });
  </script>

  <link href="../../sitefiles/bairong/jquery/templateDesign/js/prettify/style.css" rel="stylesheet">
  <script src="../../sitefiles/bairong/jquery/templateDesign/js/prettify/script.js"></script>
  <script>
    $(document).ready(function(){

      $('#myTab a').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
      });

      window.prettyPrint && prettyPrint();

      $('#ddlFileSource').change(function() {
        var source = $('option:selected', this).attr('value');
        $('.source').hide();
        $('.' + source).show();
      });

      $('#ddlFileSource').change();

      <asp:Literal id="ltlSetting" runat="server" />

    });

    function SettingsController($scope) {

    }
  </script>

  <ul class="nav nav-pills" id="myTab">
    <li class="active"><a href="#datasource">数据源</a></li>
    <li><a href="#stlelement">STL 标签</a></li>
  </ul>

  <div class="tab-content">
    <div class="tab-pane active" id="datasource">

      <table class="table table-noborder" ng-controller="SettingsController">
        <tr>
          <td  width="120">附件来源：</td>
          <td colspan="3">
            <asp:DropDownList id="ddlFileSource" runat="server" />
          </td>
        </tr>
        <tr class="source attribute" style="display:none">
          <td width="120"><span rel="tooltip" data-original-title="type 属性">内容字段：</span></td>
          <td colspan="3">
            <asp:DropDownList id="ddlType" runat="server" />
          </td>
        </tr>
        <tr class="source src" style="display:none">
          <td width="120"><span rel="tooltip" data-original-title="src 属性">附件地址：</span></td>
          <td colspan="3">
            <asp:TextBox id="tbSrc" class="input-xlarge" runat="server" />
            <div id="uploadFile" class="btn btn-success">上 传</div>
            <span id="img_upload_txt" style="clear:both; font-size:12px; color:#FF3737;"></span>
          </td>
        </tr>
        <tr>
          <td width="120"><span rel="tooltip" data-original-title="isCount 属性">选项：</span></td>
          <td>
            <label class="checkbox inline">
              <asp:CheckBox id="cbIsCount" runat="server" /><label for="cbIsCount">统计下载次数</label>
            </label>
            <label class="checkbox inline">
              <asp:CheckBox id="cbIsFilesize" runat="server" /><label for="cbIsFilesize">显示文件大小</label>
            </label>
          </td>
        </tr>
      </table>

    </div>

    <div class="tab-pane" id="stlelement">

      <div class="pull-right">
        <asp:Button id="btnApply" onclick="btnApply_OnClick" class="btn btn-success" runat="server" text="显示设置后的标签" />
      </div>

      <br>

      <hr />

      <pre class="prettyprint linenums"><asp:Literal id="ltlStlElement" runat="server" /></pre>

    </div>
  </div>

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