<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundBasePage" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>


<!doctype html>
<html lang="en">
<head> 
  <meta charset="utf-8">
  <title>生成管理功能导航</title> 
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
      <h1>生成管理功能导航 <small>移动鼠标滑轮显示更多</small>    </h1>
    </div>
    <div class="content clearfix">
      <div class="items">
        <a class="box" href="../stl/background_progressBar.aspx?CreateIndex=True&publishmentSystemID=<%=PublishmentSystemID%>" style="background: #640f6c;">
          <span>生成首页</span>
          <i class="icon-globe"></i>
        </a>
        <a class="box" href="../stl/background_createChannel.aspx?publishmentSystemID=<%=PublishmentSystemID%>" style="background: #00aeef;">
          <span>生成栏目页  </span>
          <i class="icon-globe"></i>
        </a>
        <a class="box" href="../stl/background_createContent.aspx?publishmentSystemID=<%=PublishmentSystemID%>" style="background: #f58d00;">
          <span>生成内容页</span>
          <i class="icon-globe"></i>
        </a>
        <a class="box" href="../stl/background_createFile.aspx?publishmentSystemID=<%=PublishmentSystemID%>" style="background: #1e7145;">
          <span>生成文件页</span>
          <i class="icon-globe"></i>
        </a>
      </div>
    </div>
  </div>
</body>
</html>