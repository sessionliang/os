<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.Modal.StlTemplate.StlElementInclude" validateRequest="false" %>

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
    <li class="active"><a href="#datasource">布局设置</a></li>
    <li><a href="#stlelement">STL 标签</a></li>
  </ul>

  <div class="tab-content">
    <div class="tab-pane active" id="datasource">

      <table class="table table-noborder">
        <tr>
          <td width="160"><span rel="tooltip" data-original-title="file属性">包含文件：</span></td>
          <td>
            <asp:DropDownList id="ddlFile" runat="server" />
          </td>
        </tr>
        <tr>
          <td><span rel="tooltip" data-original-title="isContext属性">STL解析与当前页面相关：</span></td>
          <td>
            <asp:RadioButtonList id="rblIsContext" repeatDirection="Horizontal" class="radiobuttonlist" runat="server" />
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