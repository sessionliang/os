<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.StlTemplate.StlElementImage" validateRequest="false" %>

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
           $('#tbSrc').val(response.imageUrl);
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
    var channels = [<asp:Literal id="ltlChannels" runat="server" />];

    $(document).ready(function(){

      $('#myTab a').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
      });

      window.prettyPrint && prettyPrint();

      $('#channelIndex').change(function() {
        $('#ctlChannelIndex').hide();

        var channelIndex = $(this).val();
        var nodeID = $('option:selected', this).attr('nodeID');
        if (channelIndex.length == 0){
          $('#ctlChannelIndex').show();
        }
        else{
          var html = '<option value=""></option>';
          for (var i = 0; i < channels.length; i++) {
           var channel = channels[i];
           var arr = channel.parentsPath.split(",");
           if ($.inArray(nodeID, arr) != -1){
            html += '<option value="' + channel.channelName + '">' + channel.title + '</option>';
           }
          }
          $('#channelName').html(html);
        }
      });

      $('#ddlImageSource').change(function() {
        var source = $('option:selected', this).attr('value');
        $('.source').hide();
        $('.' + source).show();
      });

      $('#ddlImageSource').change();

      <asp:Literal id="ltlSetting" runat="server" />

    });

    function SettingsController($scope) {
      $scope.channels = channels;
      $scope.channelNames = channels;
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
          <td  width="120">图片来源：</td>
          <td colspan="3">
            <asp:DropDownList id="ddlImageSource" runat="server" />
          </td>
        </tr>
        <tr class="source channel" style="display:none">
          <td width="120"><span rel="tooltip" data-original-title="channelIndex属性">栏目索引：</span></td>
          <td width="240">
            <select name="channelIndex" id="channelIndex">
              <option value=""></option>
              <option ng-repeat="channel in channels" value="{{channel.channelIndex}}" style="{{channel.style}}" nodeID="{{channel.nodeID}}" parentsPath="{{channel.parentsPath}}">{{channel.title}}{{channel.channelIndex}}</option>
            </select>
            <span id="ctlChannelIndex" style="color:Red;display:none;"><br />请选择索引栏目</span>
          </td>
          <td width="120"><span rel="tooltip" data-original-title="channelName属性，请先选择栏目索引">栏目名称：</span></td>
          <td>
            <select name="channelName" id="channelName">
              <option value=""></option>
              <option ng-repeat="channel in channelNames" value="{{channel.channelName}}">{{channel.title}}</option>
            </select>
            <span id="ctl02" style="color:Red;display:none;"> *</span>
          </td>
        </tr>
        <tr class="source src" style="display:none">
          <td width="120"><span rel="tooltip" data-original-title="src属性">图片地址：</span></td>
          <td colspan="3">
            <asp:TextBox id="tbSrc" class="input-xlarge" runat="server" />
            <div id="uploadFile" class="btn btn-success">上 传</div>
            <span id="img_upload_txt" style="clear:both; font-size:12px; color:#FF3737;"></span>
          </td>
        </tr>
        <tr>
          <td>图片尺寸：</td>
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