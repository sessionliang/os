<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundPreview" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
<link rel="icon" type="image/png" href="../images/gexia_icon.png">
<title>预览</title>
</head>

<body>

<link rel="stylesheet" type="text/css" href="/SiteFiles/bairong/dad/styles/template.css" />
<script type="text/javascript" src="js/jquery.qrcode.min.js"></script> 

<div class="spec_Box" ng-app>
    <div class="spec_topBg stl-top" ng-controller="stlTopCtrl"> 
      
      <a href="javascript:;" class="spec_top_nbtn spec_webModel" device-width="568px"><span class="spec_imgBg1"></span></a>
      <a href="javascript:;" class="spec_top_nbtn spec_webModel" device-width="320px"><span class="spec_imgBg2"></span></a>
      <a href="javascript:;" class="spec_top_nbtn spec_webModel" device-width="768px"><span class="spec_imgBg3"></span></a>
      <a href="javascript:;" class="spec_top_nbtn spec_webModel spec_top_cutnbtn" device-width="100%"><span class="spec_imgBg4"></span></a>
    </div>

    <div class="spec_mid" style="left:0">
      <div class="spec_pageCon">
        <asp:Literal id="ltlContent" runat="server" />
      </div>
    </div>

</div>

<div id="qrCode" style="position:absolute; top: 50px; left: 10px;width:200px;height:200px"></div>
<div style="position:absolute; top: 260px; left: 10px; width:200px;">
  <a class="btn btn-large btn-block btn-success" style="font-size:16px;" id="url-copy">复制链接</a>
</div>

</body>
</html>

<script type="text/javascript" src="../weixin/js/jquery.zclip.min.js"></script>
<script type="text/javascript">
$('a#url-copy').zclip({
  path:'../weixin/js/ZeroClipboard.swf',
  copy:$('iframe').attr('src'),
  afterCopy:function(){
    toastr.success('URL已复制到粘贴板', '操作成功');
  }
});

function specCenter(fullSize){
    if (fullSize){
        $(".spec_pageCon").css("left",0);
    }else{
        $(".spec_pageCon").css("left",($(".spec_mid").width()-$(".spec_pageCon").width())/2);
    }
}

$(".spec_webModel").click(function(){
    $(".spec_webModel").removeClass("spec_top_cutnbtn");
    $(this).addClass("spec_top_cutnbtn");    
    $(".spec_pageCon").css('width', $(this).attr('device-width'));
    var isFull = $(this).attr('device-width') == "100%";
    specCenter(isFull);
    if (isFull){
      $('#qrCode').hide();
    }else{
      $('#qrCode').show();
    }
});
</script>