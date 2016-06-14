<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.StlTemplate.StlElementContent" validateRequest="false" %>

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
          <td width="120"><span rel="tooltip" data-original-title="type 属性">内容字段：</span></td>
          <td colspan="3">
            <asp:DropDownList id="ddlType" class="input-xlarge" runat="server" />
          </td>
        </tr>
        <tr>
          <td>显示字数：</td>
          <td colspan="3">
            <asp:TextBox id="tbWordNum" class="input-mini" runat="server" rel="tooltip" data-original-title="width属性" />
            <asp:RegularExpressionValidator
                  ControlToValidate="tbWordNum"
                  ValidationExpression="\d+"
                  Display="Dynamic"
                  ErrorMessage=" *" foreColor="red"
                  runat="server"/>
          </td>
        </tr>
        <tr>
          <td width="120">其他控制：</td>
          <td colspan="3">
            <label class="checkbox inline">
              <asp:CheckBox id="cbIsClearTags" runat="server" /><label for="cbIsClearTags">是否清除HTML标签</label>
            </label>
            <label class="checkbox inline">
              <asp:CheckBox id="cbIsReturnToBR" runat="server" /><label for="cbIsReturnToBR">是否将回车替换为HTML换行标签</label>
            </label>
            <br />
            <label class="checkbox inline">
              <asp:CheckBox id="cbIsUpper" runat="server" /><label for="cbIsUpper">是否转换为大写</label>
            </label>
            <label class="checkbox inline">
              <asp:CheckBox id="cbIsLower" runat="server" /><label for="cbIsLower">是否转换为小写</label>
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