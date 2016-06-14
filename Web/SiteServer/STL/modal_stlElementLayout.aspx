<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.StlTemplate.StlElementLayout" validateRequest="false" %>

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
    var cols = [<asp:Literal id="ltlCols" runat="server" />];

    $(document).ready(function(){

      $('#myTab a').click(function (e) {
        e.preventDefault();
        $(this).tab('show');
      });

      window.prettyPrint && prettyPrint();

      <asp:Literal id="ltlSetting" runat="server" />

    });

    function SettingsController($scope) {
      $scope.colCount = cols.length;
      $scope.cols = cols;

      $('#colCount').change(function() {
        var count = parseInt($(this).val());
        var length = cols.length;

        if (count > length){
          for (var i = 0; i < count - length; i++) {
            $scope.cols.push({'num':'','unit':'%'});
          };
        }else if(count < length){

          for (var i = 0; i < length - count; i++) {
            $scope.cols.splice(cols.length - 1, 1);
          };
        }
      });
      
      $scope.addCol = function(){
        $scope.cols.push({});
      };
    }
  </script>

  <ul class="nav nav-pills" id="myTab">
    <li class="active"><a href="#datasource">布局设置</a></li>
    <li><a href="#itemlist">内部标签</a></li>
    <li><a href="#stlelement">STL 标签</a></li>
  </ul>

  <div class="tab-content">
    <div class="tab-pane active" id="datasource">

      <table class="table table-noborder" ng-controller="SettingsController">
        <tr>
          <td width="120">上边距：</td>
          <td width="240">
            <asp:TextBox id="tbMarginTop" class="input-mini" runat="server" rel="tooltip" data-original-title="marginTop属性" /> px
          </td>
          <td width="120">下边距：</td>
          <td width="240">
            <asp:TextBox id="tbMarginBottom" class="input-mini" runat="server" rel="tooltip" data-original-title="marginBottom属性" /> px
          </td>
          <td></td>
        </tr>
        
        <tr>
          <td>设置：</td>
          <td colspan="4">
            <table class="table">
              <tr>
                <td>
                  栏数：
                  <select class="input-mini" id="colCount" name="colCount" ng-model="colCount">
                    <option value="1">1</option>
                    <option value="2">2</option>
                    <option value="3">3</option>
                    <option value="4">4</option>
                  </select>
                </td>
              </tr>
              <tr ng-repeat="col in cols">
                <td>
                  第{{$index + 1}}栏：
                  <input class="input-mini" name="colNum" type="text" ng-model="col.num">
                  <select class="input-mini" name="colUnit" ng-model="col.unit">
                    <option value="%">%</option>
                    <option value="px">px</option>
                  </select>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>

    </div>
    <div class="tab-pane" id="itemlist">

      <table class="table table-noborder" ng-controller="SettingsController">
        <tr>
          <td width="120">内部标签模板：</td>
          <td>
            <asp:TextBox id="tbItemTemplate" textMode="MultiLine" width="90%" rows="5" runat="server" />
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