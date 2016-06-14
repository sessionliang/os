<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.RequestNew" %>
<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html ng-app>
<head>
  <meta charset="utf-8"/>
  <meta name="renderer" content="webkit" />
  <meta http-equiv = "X-UA-Compatible" content = "IE=edge,chrome=1" />
  <title>阿里云成品网站</title>
  <!--[if IE]>
   <script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
  <![endif]-->
  <bairong:Code type="jQuery" runat="server" />
  <bairong:Code type="calendar" runat="server" />
  <bairong:Code type="bootstrap" runat="server" />
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
    }
  </script>
<link rel="stylesheet" type="text/css" href="css/request.css" />
</head>
<body>
  <div id="doc" class="whole-bg" ng-controller="SettingsController">
    <div id="head" class="head head-status-new">
      <div class="mod-logo">
        <h1 class="" ></h1>
      </div>
    </div>
    <bairong:alerts runat="server" />
    <div class="screen content loading">
      <p style="height:240px; text-align:center; padding-top:150px;"><img src="../pic/animated_loading.gif" align="center" /> 页面载入中...</p>
    </div>
    <div class="screen content loaded" style="display:none">

      <div class="layout">
        <div class="mod-status-tips">
            <div class="status-content fd-clr">
                
                <div class="status-detail">
                    <h3 class="detail-title"><i class="status-icon status-success"></i>亲爱的 <span ng-bind="order.loginname"></span> ，您的订单已提交成功，我们将尽快与您联系为您开通系统。</h3>
                    <ul>
                      <li style="text-align:center">欢迎您通过各种方式联系我们的售后技术支持，感谢您的配合！
                      </li>
                      <li style="text-align:center">售后服务QQ：<em class="orange">  4008770550  </em>，售后服务电话：<em class="orange">  4008-770-550  </em>
                      </li>
                    </ul>
                </div>

            </div>
        </div>
    </div>
    </div>
    <div class="footer">
      <div>
        <p>
          版权声明
          <span class="copyright" >Copyright &copy;2013</span>
           北京百容千域软件技术开发有限责任公司. All Rights Reserved
        </p>
      </div>
    </div>
  </div>

</body>
</html>
<!-- WPA Button Begin -->
<!-- <script charset="utf-8" type="text/javascript" src="http://wpa.b.qq.com/cgi/wpa.php?key=XzkzODAxMTMwMF84ODA2MV80MDA4NzcwNTUwXw"></script> -->
<!-- WPA Button End -->