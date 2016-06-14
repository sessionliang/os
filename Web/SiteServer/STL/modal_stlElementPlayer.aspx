<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.StlTemplate.StlElementPlayer" validateRequest="false" %>

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
       var reg = /^(<%=TypeCollection%>)$/i;
       if (ext && reg.test(ext)) {
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
           $('#tbPlayUrl').val(response.playUrl);
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

      <asp:Literal id="ltlSetting" runat="server" />

    });
  </script>

  <ul class="nav nav-pills" id="myTab">
    <li class="active"><a href="#datasource">数据源</a></li>
    <li><a href="#stlelement">STL 标签</a></li>
  </ul>

  <div class="tab-content">
    <div class="tab-pane active" id="datasource">

      <table class="table table-noborder" ng-controller="SettingsController">
        <tr>
          <td width="120"><span rel="tooltip" data-original-title="playUrl">视频地址：</span></td>
          <td colspan="3">
            <asp:TextBox id="tbPlayUrl" class="input-xlarge" runat="server" />
            <div id="uploadFile" class="btn btn-success">上 传</div>
            <span id="img_upload_txt" style="clear:both; font-size:12px; color:#FF3737;"></span>
          </td>
        </tr>
        <tr>
          <td width="120"><span rel="tooltip" data-original-title="playBy">选择播放器：</span></td>
          <td colspan="3">
            <asp:DropDownList id="ddlPlayBy" class="input-xlarge" runat="server" />
          </td>
        </tr>
        <tr>
          <td>视频尺寸：</td>
          <td colspan="3">
            宽度 <asp:TextBox id="tbWidth" class="input-mini" runat="server" rel="tooltip" data-original-title="width属性" />
            <asp:RegularExpressionValidator
                  ControlToValidate="tbWidth"
                  ValidationExpression="\d+"
                  Display="Dynamic"
                  ErrorMessage=" *" foreColor="red"
                  runat="server"/>
            px&nbsp;&nbsp;&nbsp;&nbsp;
            高度 <asp:TextBox id="tbHeight" class="input-mini" runat="server" rel="tooltip" data-original-title="height属性" />
            <asp:RegularExpressionValidator
                  ControlToValidate="tbHeight"
                  ValidationExpression="\d+"
                  Display="Dynamic"
                  ErrorMessage=" *" foreColor="red"
                  runat="server"/>
            px
          </td>
        </tr>
        <tr>
          <td width="120"><span rel="tooltip" data-original-title="isAutoPlay 属性">是否自动播放：</span></td>
          <td colspan="3">
            <label class="checkbox inline">
              <asp:CheckBox id="cbIsAutoPlay" runat="server" /><label for="cbIsAutoPlay">自动播放</label>
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