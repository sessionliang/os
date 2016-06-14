<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundBasePage" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>


<!doctype html>
<html lang="en">
<head> 
  <meta charset="utf-8">
  <title>微官网显示管理功能导航</title> 
  <meta name="description" content="" />
  <meta name="keywords" content="" />
  <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
  <script language="javascript" src="../../sitefiles/bairong/jquery/jquery-1.9.1.min.js"></script>
  <link rel="stylesheet" type="text/css" media="all" href="../scripts/lib/metro/metro.css" />
  <script src="../scripts/lib/metro/jquery.plugins.min.js"></script>
  <script src="../scripts/lib/metro/metro.js"></script>
  <!--[if lt IE 9]>
    <script src="../scripts/lib/metro/respond.min.js"></script>
  <![endif]-->
  <link rel="stylesheet" type="text/css" href="../scripts/lib/bootstrap/css/bootstrap.min.css" media="all" />
  <link href="../scripts/lib/font-awesome/css/font-awesome.css" rel="stylesheet">
</head> 
<body>
  <div class="metro-layout horizontal">
    <div class="header">
      <h1>显示管理功能导航 <small>移动鼠标滑轮显示更多</small>    </h1>
    </div>
    <div class="content clearfix">
      <div class="items">
        
        <a class="box" href="../stl/background_templateMatch.aspx?publishmentSystemID=<%=PublishmentSystemID%>" style="background: #3c5b9b;">
          <span>匹配模板</span>
          <i class="icon-refresh"></i>
        </a>
        <a class="box" href="../stl/background_templateInclude.aspx?publishmentSystemID=<%=PublishmentSystemID%>" style="background: #1e7145;">
          <span>包含文件管理</span>
          <i class="icon-file"></i>
        </a>
        <a class="box height2" href="../stl/background_templateMain.aspx?publishmentSystemID=<%=PublishmentSystemID%>" style="background: #d32c2c;">
          <span>模板管理</span>
          <i class="icon-dashboard big"></i>
        </a>
        <a class="box" href="../stl/background_templateImport.aspx?publishmentSystemID=<%=PublishmentSystemID%>&templateType=IndexPageTemplate" style="background: #00aeef;">
          <span>更换首页模板</span>
          <i class="icon-exchange"></i>
        </a>
        <a class="box" href="../stl/background_templateImport.aspx?publishmentSystemID=<%=PublishmentSystemID%>&templateType=ChannelTemplate" style="background: #f58d00;">
          <span>更换栏目模板</span>
          <i class="icon-exchange"></i>
        </a>
        <a class="box" href="../stl/background_templateImport.aspx?publishmentSystemID=<%=PublishmentSystemID%>&templateType=ContentTemplate" style="background: #00aba9;">
          <span>更换内容模板</span>
          <i class="icon-exchange"></i>
        </a>
      </div>
    </div>
  </div>
</body>
</html>