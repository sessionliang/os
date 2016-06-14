<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.RequestForm" %>
<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" ng-app>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta http-equiv = "X-UA-Compatible" content = "IE=edge,chrome=1" />
<title>阿里云成品网站开通资料提交</title>
<link href="css/style.css" rel="stylesheet" type="text/css" />
<bairong:Code type="jQuery" runat="server" />
<bairong:Code type="calendar" runat="server" />
<bairong:Code type="angularjs" runat="server" />
  <script>
    $(document).ready(function(){
        $.each( $('.f-input'), function( i, val ) {
          $(this).html($(this).attr('inputHtml'));
        });
        $.each( $('.f-des'), function( i, val ) {
          if ($(this).attr('imageUrl') != ''){
            $(this).html('<img src="' + $(this).attr('imageUrl') + '" />');
          }
        });
        $('.loading').hide();
        $('.loaded').show();
    });

    function SettingsController($scope) {
      $scope.order = <asp:Literal id="ltlJsonOrder" runat="server" />;
      $scope.formPages = <asp:Literal id="ltlJsonFormPages" runat="server" />;
      $scope.formGroups = <asp:Literal id="ltlJsonFormGroups" runat="server" />;
      if ($scope.order.state == 'working'){
        $('.input-form').hide();
        $('.working').show();
      }else if ($scope.order.state == 'done'){
        $('.input-form').hide();
        $('.done').show();
      }else{
        $('.input-form').show();
      }
    }
  </script>
</head>
<body class="aliBody">
<div class="aliTop">
 <div class="aliTopCon"><img src="img/aliLogo.jpg" width="328" height="40" /></div>
</div>
<bairong:alerts runat="server" />
<div class="screen content loading">
  <p style="height:240px; text-align:center; padding-top:150px;"><img src="../pic/animated_loading.gif" align="center" /> 页面载入中...</p>
</div>
<div class="aliMain loaded" style="display:none" ng-controller="SettingsController">
<div class="ali_con1">
    亲爱的 <a href="javascript:;" class="ali_blue" ng-bind="order.loginname"></a> ，您购买的网站模板为： <a href="{{order.mobanurl}}" class="ali_blue" target="_blank" ng-bind="order.mobanurl"></a>。
      <br />
      网站开通需要完成本次初始资料提交，资料提交后我们将在两个工作日内对资料进行整理并更新到网站中，请您务必仔细填写。
      <br />
      填写过程中有疑问请点击页面右侧在线QQ咨询，或者添加售后服务QQ号码：售后服务QQ：<span class="ali_org">  4008770550  </span>
</div>
<div class="ali_pos ali_pos{{order.stage}}">
<ul>
  <li ng-repeat="formPage in formPages" class="{{formPage.state}}">
    {{$index + 1 + '.' + formPage.title}}
  </li>
</ul>
</div>
<div class="ali_box input-form">
    <form class="auth-info white-bg" id="form" enctype="multipart/form-data" runat="server">
    <input id="currentPageID" name="currentPageID" type="hidden" value="{{order.currentpageid}}" />
    <input id="orderFormID" name="orderFormID" type="hidden" value="{{order.orderformid}}" />
    <input id="orderID" name="orderID" type="hidden" value="{{order.orderid}}" />
    <input id="mobanID" name="mobanID" type="hidden" value="{{order.mobanid}}" />

    <div ng-repeat="formGroup in formGroups">
        <div class="ali_set1">{{formGroup.title}}</div>
        <ul>

         <li ng-repeat="element in formGroup.elements">
         <span class="ali_span"><span class="ali_squer"> {{element.require}} </span>{{element.displayName}}:</span>
         <div class="ali_info">
            <div class="f-input" inputHtml='{{element.inputHtml}}'></div>
            <div class="aliImgBox f-des" imageUrl="{{element.imageUrl}}"></div>
         </div>
         </li>

        </ul>
        <div class="ali_lineBox"></div>
    </div>

    <div class="ali_btnBox">
        <a href="#"></a>
        <asp:ImageButton ImageUrl="img/ali_btn1.jpg" class="fui-btn btn-action fui-btn-l" ng-disabled="form.$invalid" text="下一步" OnClick="Submit_OnClick" runat="server" />
    </div>
    <div class="clear"></div>
    </form>
</div>

<div class="ali_susBox working" style="display:none">
    <div class="ali_susTbox"><div class="ali_susT">您于{{order.submitdate}}提交的资料，网站正在制作中。预计将于在两个工作日内开通成功！</div></div>
    <div class="ali_svr">
    欢迎您通过各种联系方式联系我们的售后技术支持，感谢您的配合！<br />
    售后服务QQ：<span class="ali_org">4008770550</span>，售后服务电话：<span class="ali_org">4008-770-550</span>
    </div>
    <div class="clear"></div>
</div>

<div class="ali_susBox done" style="display:none">
    <div class="ali_susTbox"><div class="ali_susT">您于{{order.submitdate}}提交的资料，网站已初始化并开通成功，请与售后服务人员联系！</div></div>
    <div class="ali_svr">
    欢迎您通过各种联系方式联系我们的售后技术支持，感谢您的配合！<br />
    售后服务QQ：<span class="ali_org">4008770550</span>，售后服务电话：<span class="ali_org">4008-770-550</span>
    </div>
    <div class="clear"></div>
</div>

<div class="clear"></div>
</div>
<div class="alifooter">版权声明
          <span>Copyright </span>
           北京百容千域软件技术开发有限责任公司. All Rights Reserved
</div>
</body>
</html>
<!-- WPA Button Begin -->
<!-- <script charset="utf-8" type="text/javascript" src="http://wpa.b.qq.com/cgi/wpa.php?key=XzkzODAxMTMwMF84ODA2MV80MDA4NzcwNTUwXw"></script> -->
<!-- WPA Button End -->