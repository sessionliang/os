﻿<%@ Page Language="C#" Inherits="SiteServer.STL.BackgroundPages.BackgroundTemplateReference" enableviewstate="false"%>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server">
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          <blockquote style="margin-left:15px;margin-top:10px;">
            <p>STL语言为SiteServer模板语言(SiteServer Template Language)的缩写，是一种和HTML语言类似的服务器端语言。</p>
            <small><a href="http://cms.siteserver.cn/download.html" target="_blank"><img src="../../sitefiles/bairong/icons/download.gif" border="0"> 下载SiteServer模板制作器</a></small>
          </blockquote>
        </td>
      </tr>
    </table>
  </div>

  <table cellspacing="2" cellpadding="2" class="table table-bordered table-striped" class="center" border="0">
    <tr class="info">
      <td colspan="3">STL 元素</td>
    </tr>
    <tr class="center">
      <td width="130">元素</td>
      <td width="100">含义</td>
      <td>属性</td>
    </tr>
    <asp:Literal ID="ltlTemplateElements" runat="server"></asp:Literal>
  </table>
  <br>
  <table cellspacing="2" cellpadding="2" class="table table-bordered table-striped" class="center" border="0">
    <tr class="info">
      <td colspan="3">STL 实体</td>
    </tr>
    <tr class="center" style="height:25px;">
      <td width="130">实体</td>
      <td width="100">含义</td>
      <td>属性</td>
    </tr>
    <asp:Literal ID="ltlTemplateEntities" runat="server"></asp:Literal>
  </table>
  <br>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->