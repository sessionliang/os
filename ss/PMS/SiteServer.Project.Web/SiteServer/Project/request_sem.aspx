<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.RequestSEM" %>
<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html ng-app>
<head>
  <meta charset="utf-8"/>
  <meta name="renderer" content="webkit" />
  <meta http-equiv = "X-UA-Compatible" content = "IE=edge,chrome=1" />
  <title>阿里云成品网站搜索引擎优化表单</title>
  <!--[if IE]>
   <script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script>
  <![endif]-->
  <bairong:Code type="jQuery" runat="server" />
  <bairong:Code type="calendar" runat="server" />
  <bairong:Code type="bootstrap" runat="server" />
  <bairong:Code type="angularjs" runat="server" />
  <script>
    $(document).ready(function(){
        $('.loading').hide();
        $('.loaded').show();
    });

    function SettingsController($scope) {
      $scope.order = <asp:Literal id="ltlJsonOrder" runat="server" />;
      if ($scope.order.state == 'done'){
        $('.input-form').hide();
        $('.done').show();
      }else{
        $('.input-form').show();
        $('.done').hide();
      }
    }
  </script>
  <style type="text/css">
  .css-form input.ng-invalid.ng-dirty {
    background-color: #FA787E;
  }

  .css-form input.ng-valid.ng-dirty {
    background-color: #78FA89;
  }
</style>
<link rel="stylesheet" type="text/css" href="css/request.css" />
</head>
<body>
  <div id="doc" class="whole-bg" ng-controller="SettingsController">
    <div id="head" class="head head-sem">
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
        <div class="fui-top-tips">
          亲爱的 <span ng-bind="order.loginname"></span> ，您好。
          <br />
          SEO优化需要完成本次资料提交，资料提交后我们将在两个工作日内将完成搜索引擎优化工作，请您务必仔细填写。
          <br />
          填写过程中有疑问请点击页面右侧在线QQ咨询，或者添加售后服务QQ号码：售后服务QQ：<em class="orange">  4008770550  </em>
        </div>
      </div>
      <div class="layout input-form">
        <form class="auth-info white-bg" id="form" runat="server">
          <input id="orderID" name="orderID" type="hidden" value="{{order.orderid}}" />

          <fieldset>

            <div class="bor-bottom pad">
              <div class="mod-title">
                <h3 style="background: url(img/1625170_1084294223.png) no-repeat;" onclick="$('#groupContent').toggle();">SEO准备资料</h3>
              </div>
              <div class="mod-content" id="groupContent">
                
                <div class="form-line">
                  <label class="f-label" >
                    <span class="f-require" >*</span>
                    优化网址：
                  </label>
                  <div class="f-content">
                    <p class="f-input">
                      <input id="Domain" name="Domain" type="text" class="input_text" value="" style="width:380px;" required>
                      <br />
                      <span style="color:#999">需要进行SEO优化的网站首页地址</span>
                    </p>
                  </div>
                </div>

                <div class="form-line">
                  <label class="f-label" >
                    <span class="f-require" >*</span>
                    标题（Title）：
                  </label>
                  <div class="f-content">
                    <p class="f-input">
                      <input id="Title" name="Title" type="text" class="input_text" value="" style="width:380px;" required>
                      <br />
                      <span style="color:#999">搜索结果出现的链接的文字，一般不超过80个字符</span>
                    </p>
                    <p class="f-des">
                      <img src="http://moban.download.siteserver.cn/img/form_sem/title.png">
                    </p>
                  </div>
                </div>

                <div class="form-line">
                  <label class="f-label" >
                    <span class="f-require" >*</span>
                    关键词（KeyWords）：
                  </label>
                  <div class="f-content">
                    <p class="f-input">
                      <textarea id="Keywords" name="Keywords" style="width:380px;height:60px;" required></textarea>
                      <br />
                      <span style="color:#999">便于用户通过搜索引擎能搜到网站的词汇，关键词代表了网站的定位，一般不超过100个字符</span>
                      <br />
                      <span style="color:#999">范例：CMS,SiteServer,政府CMS,WCM,内容管理系统,网站管理,.net cms,网站内容管理,静态生成,网站群,门户系统,网站建设,建站系统</span>
                    </p>
                  </div>
                </div>

                <div class="form-line">
                  <label class="f-label" >
                    <span class="f-require" >*</span>
                    描述（Description）：
                  </label>
                  <div class="f-content">
                    <p class="f-input">
                      <textarea id="Description" name="Description" style="width:380px;height:60px;" required></textarea>
                      <br />
                      <span style="color:#999">搜索结果出现的描述文字，一般不超过200个字符</span>
                    </p>
                    <p class="f-des">
                      <img src="http://moban.download.siteserver.cn/img/form_sem/description.png">
                    </p>
                  </div>
                </div>

              </div>
            </div>

            <div class="mod-btn">
              <asp:Button class="fui-btn btn-action fui-btn-l" ng-disabled="form.$invalid" text="提 交" OnClick="Submit_OnClick" runat="server" />
            </div>
          </fieldset>

        </form>
      </div>
      <div class="layout done" style="display:none">
        <div class="mod-status-tips">
            <div class="status-content fd-clr">
                
                <div class="status-detail">
                    <h3 class="detail-title"><i class="status-icon status-success"></i>您于{{order.submitdate}}提交的资料，预计将于{{order.donedate}}之前完成搜索引擎优化工作</h3>
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