<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundBasePage" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>


<!doctype html>
<html lang="en">
<head> 
  <meta charset="utf-8">
  <title>设置管理功能导航</title> 
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
      <h1>设置管理功能导航 <small>移动鼠标滑轮显示更多</small>    </h1>
    </div>
    <div class="content clearfix">
      <div class="items">
        <a class="box" href="../cms/background_configurationSite.aspx?publishmentSystemID=<%=PublishmentSystemID%>" style="background: #91D100;">
          <span>站点设置</span>
          <i class="icon-cog"></i>
        </a>
        <a class="box" href="../cms/background_configurationContent.aspx?publishmentSystemID=<%=PublishmentSystemID%>" style="background: #00AEEF;">
          <span>内容管理设置  </span>
          <i class="icon-file-text"></i>
        </a>
        <a class="box" href="../cms/background_tableStyleContent.aspx?publishmentSystemID=<%=PublishmentSystemID%>" style="background: #F3B200;">
          <span>内容字段管理</span>
          <i class="icon-edit"></i>
        </a>
        <a class="box" href="../cms/background_nodeGroup.aspx?publishmentSystemID=<%=PublishmentSystemID%>" style="background: #2572EB;">
          <span>栏目组管理</span>
          <i class="icon-align-justify"></i>
        </a>
        <a class="box" href="../cms/background_contentGroup.aspx?publishmentSystemID=<%=PublishmentSystemID%>" style="background: #199900;">
          <span>内容组管理</span>
          <i class="icon-th"></i>
        </a>
        <a class="box" href="../cms/background_contentTags.aspx?publishmentSystemID=<%=PublishmentSystemID%>" style="background: #00A3A3;">
          <span>内容标签管理</span>
          <i class="icon-bookmark"></i>
        </a>
        <a class="box" href="../stl/background_templateFilePathRule.aspx?publishmentSystemID=<%=PublishmentSystemID%>" style="background: #FF76BC;">
          <span>页面命名规则</span>
          <i class="icon-ticket"></i>
        </a>
      </div>
    </div>
  </div>
</body>
</html>