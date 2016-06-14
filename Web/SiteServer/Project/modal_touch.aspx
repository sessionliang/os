<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.Touch" Trace="false"%>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html ng-app="pmsApp">
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server" ng-controller="touchController">
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>

  <div ng-show="loading"><img src="../pic/loading.gif" /></div>
  <div ng-show="!loading" class="all" style="display:none">
    
    <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          搜索：
          <div class="input-append">
            <input class="input-medium" type="text" ng-model="filterText">
            <button class="btn" type="button" ng-disabled="filterText.length == 0" ng-click="filterText = ''"><i class="icon-remove"></i></button>
          </div>
        </td>
        <td class="pull-right">合计：<code ng-bind="touches.length"></code></td>
      </tr>
    </table>
  </div>

  <table id="contents" class="table table-bordered table-hover">
    <tr class="info thead">
      <td>联系人（我方） </td>
      <td>联系方式 </td>
      <td>联系人（对方） </td>
      <td>备注</td>
      <td>联系时间 </td>
      <td width="80">
        <a class="btn" href="javascript:;" ng-click="toggleAddMode()"><i class="icon-plus" ng-class="{'icon-minus': addMode, 'icon-plus': !addMode}"></i></a>
      </td>
    </tr>
    <tr ng-show="addMode">
        <td>
            
        </td>
        <td>
            <select ng-model="touch['touchBy']" class="input-small">
              <option value="Action">内部动作</option>
              <option value="Phone">电话</option>
              <option value="QQ">QQ</option>
              <option value="AliWangWang">阿里旺旺</option>
              <option value="SMS">短信</option>
              <option value="Email">邮件</option>
              <option value="Meeting">见面</option>
            </select>
        </td>
        <td>
            <input type="text" ng-model="touch['contactName']" class="input-medium">
        </td>
        <td>
            <textarea ng-model="touch['summary']" rows="3"></textarea>
        </td>
        <td>
            
        </td>
        <td>
            <div class="btn-toolbar">
                <div class="btn-group">
                    <a class="btn" href="javascript:;" ng-click="addTouch()"><i class="icon-ok"></i></a>
                    <a class="btn" href="javascript:;" ng-click="toggleAddMode()"><i class="icon-remove"></i></a>
                </div>
            </div>
        </td>
    </tr>
    <tr ng-repeat="touch in touches | filter:filterText">
      <td class="center" ng-switch on="touch.editMode">
        <div ng-switch-default>
            <span ng-bind="touch.addUserName"></span>
        </div>
        <div ng-switch-when="true">
            
        </div>
      </td>
      <td class="center" ng-switch on="touch.editMode">
        <div ng-switch-default>
            <span ng-bind="getTouchBy(touch.touchBy)"></span>
        </div>
        <div ng-switch-when="true">
            <select ng-model="touch['touchBy']" class="input-small">
              <option value="Action">内部动作</option>
              <option value="Phone">电话</option>
              <option value="QQ">QQ</option>
              <option value="AliWangWang">阿里旺旺</option>
              <option value="SMS">短信</option>
              <option value="Email">邮件</option>
              <option value="Meeting">见面</option>
            </select>
        </div>
      </td>
      <td class="center" ng-switch on="touch.editMode">
        <div ng-switch-default>
            <span ng-bind="touch.contactName"></span>
        </div>
        <div ng-switch-when="true">
            <input type="text" ng-model="touch['contactName']" class="input-medium">
        </div>
      </td>
      <td class="center" ng-switch on="touch.editMode">
        <div ng-switch-default>
            <span ng-bind="touch.summary"></span>
        </div>
        <div ng-switch-when="true">
            <textarea ng-model="touch['summary']" rows="3"></textarea>
        </div>
      </td>
      <td class="center" ng-switch on="touch.editMode">
        <div ng-switch-default>
            <span ng-bind="touch.addDate.replace('T', ' ')"></span>
        </div>
        <div ng-switch-when="true">
            
        </div>
      </td>
      <td class="center" ng-switch on="touch.editMode">
        <div ng-switch-default>
            <div class="btn-toolbar">
                <div class="btn-group">
                    <a class="btn" href="javascript:;" ng-click="toggleEditMode(touch)"><i class="icon-edit"></i></a>
                    <a class="btn" href="javascript:;" ng-click="deleteTouch($index)"><i class="icon-trash"></i></a>
                </div>
            </div>
        </div>
        <div ng-switch-when="true">
            <div class="btn-toolbar">
                <div class="btn-group">
                    <a class="btn" href="javascript:;" ng-click="updateTouch(touch);toggleEditMode(touch);"><i class="icon-ok"></i></a>
                    <a class="btn" href="javascript:;" ng-click="toggleEditMode(touch)"><i class="icon-remove"></i></a>
                </div>
            </div>
        </div>
      </td>
    </tr>
  </table>

  </div>
  
</form>

<asp:Literal id="ltlPageInfoScript" runat="server" />

<script src="lib/jquery/jquery-2.0.3.min.js"></script>
<script src="lib/angular/angular.min.js" language="javascript" type="text/javascript"></script>
<script src="lib/angular/angular-resource.min.js" language="javascript" type="text/javascript"></script>
<link href="lib/toastr/toastr.min.css" rel="stylesheet" type="text/css" />
<script src="lib/toastr/toastr.min.js" language="javascript" type="text/javascript"></script>

<script language="javascript" src="js/pms_app/app.js"></script>
<script language="javascript" src="js/pms_app/services/notifyService.js"></script>
<script language="javascript" src="js/pms_app/services/touchService.js"></script>
<script language="javascript" src="js/pms_app/controllers/touchController.js"></script>

</body>
</html>
