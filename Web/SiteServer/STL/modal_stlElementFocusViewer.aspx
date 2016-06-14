<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.StlTemplate.StlElementFocusViewer" validateRequest="false" %>

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
          <td width="120"><span rel="tooltip" data-original-title="channelIndex属性">栏目索引：</span></td>
          <td width="240">
            <select name="channelIndex" id="channelIndex">
              <option value=""></option>
              <option ng-repeat="channel in channels" value="{{channel.channelIndex}}" style="{{channel.style}}" nodeID="{{channel.nodeID}}" parentsPath="{{channel.parentsPath}}">{{channel.title}}{{channel.channelIndex}}</option>
            </select>
            <span id="ctlChannelIndex" style="color:Red;display:none;"><br />请选择索引栏目</span>
          </td>
          <td width="120"><span rel="tooltip" data-original-title="channelName属性，请先选择栏目索引">栏目名称：</span></td>
          <td width="240">
            <select name="channelName" id="channelName">
              <option value=""></option>
              <option ng-repeat="channel in channelNames" value="{{channel.channelName}}">{{channel.title}}</option>
            </select>
            <span id="ctl02" style="color:Red;display:none;"> *</span>
          </td>
          <td></td>
        </tr>
        <tr>
          <td><span rel="tooltip" data-original-title="scope属性，默认为Self">内容范围：</span></td>
          <td>
            <asp:DropDownList id="ddlScope" runat="server" />
          </td>
          <td><span rel="tooltip" data-original-title="order属性，默认为Default">排序：</span></td>
          <td>
            <asp:DropDownList id="ddlOrder" runat="server" />
          </td>
          <td></td>
        </tr>
        <tr>
          <td>显示条数：</td>
          <td colspan="4">
            从第 <asp:TextBox id="tbStartNum" class="input-mini" runat="server" rel="tooltip" data-original-title="startNum属性" /> 条开始，共显示
            <asp:TextBox id="tbTotalNum" class="input-mini" runat="server" rel="tooltip" data-original-title="totalNum属性" /> 条
          </td>
        </tr>
        <tr>
          <td><span rel="tooltip" data-original-title="theme属性">显示样式：</span></td>
          <td>
            <asp:DropDownList id="ddlTheme" runat="server" />
          </td>
          <td>图片尺寸：</td>
          <td>
            宽度 <asp:TextBox id="tbWidth" class="input-mini" runat="server" rel="tooltip" data-original-title="width属性" /> px
            高度<asp:TextBox id="tbHeight" class="input-mini" runat="server" rel="tooltip" data-original-title="height属性" /> px
          </td>
        </tr>
        <tr>
          <td>筛选条件：</td>
          <td colspan="4">
            <table class="table">
            <tr>
              <td>
                仅显示内容组：
                <asp:DropDownList id="ddlGroup" runat="server" />
                不显示内容组：
                <asp:DropDownList id="ddlGroupNot" runat="server" />
              </td>
            </tr>
            <tr>
              <td>
                <asp:CheckBoxList id="cblFilterAttributes" repeatDirection="Horizontal" repeatColumns="5" class="checkboxlist" runat="server" />
              </td>
            </tr>
            </table>

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