var leftController = {};

leftController.left = $pageInfo.left;

utilService.render('leftController', leftController);

$(function(){
    
    if ($pageInfo.stlPublishmentSystemType == 'Weixin'){
      $("a[device-width='320px']").click();
      $('#qrCode').qrcode({width: 200, height:200, text: $pageInfo.stlAbsoluteUrl});
      $('.spec_qrCode').show();
    }

    $(".spec_mimt").click(function(){$(this).toggleClass("spec_mimt2");$(this).next(".spec_mim_tools").slideToggle(300)});

    $(".spec_mlBomBtn").click(function(){
        $(".spec_top_link a").removeClass("spec_active");
        $(".spec_mleft").animate({"left":"-234px"},300);
        $(".spec_mid").animate({"left":0},300);
    });
});