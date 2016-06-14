jQuery(function(){
//选项卡滑动切换通用
jQuery(function(){jQuery(".spec_hoverTag .spec_chgBtn").hover(function(){jQuery(this).parent().find(".spec_chgBtn").removeClass("spec_chgCutBtn");jQuery(this).addClass("spec_chgCutBtn");var cutNum=jQuery(this).parent().find(".spec_chgBtn").index(this);jQuery(this).parents(".spec_hoverTag").find(".spec_chgCon").hide();jQuery(this).parents(".spec_hoverTag").find(".spec_chgCon").eq(cutNum).show();})})

//选项卡点击切换通用
jQuery(function(){jQuery(".spec_clickTag .spec_chgBtn").click(function(){jQuery(this).parent().find(".spec_chgBtn").removeClass("spec_chgCutBtn");jQuery(this).addClass("spec_chgCutBtn");var cutNum=jQuery(this).parent().find(".spec_chgBtn").index(this);jQuery(this).parents(".spec_clickTag").find(".spec_chgCon").hide();jQuery(this).parents(".spec_clickTag").find(".spec_chgCon").eq(cutNum).show();})})


function specCenter(fullSize){
    if (fullSize){
        $(".spec_pageCon").css("left",0);
    }else{
        $(".spec_pageCon").css("left",($(".spec_mid").width()-$(".spec_pageCon").width())/2);
    }
    $('.spec_pageCon').perfectScrollbar('update');
}

// $(".spec_webModel").click(function(){$(".spec_webModel").removeClass("spec_top_cutnbtn");$(this).addClass("spec_top_cutnbtn");$(".spec_pageCon").hide();$(".spec_pageCon").eq($(".spec_webModel").index(this)).show();});

$(".spec_webModel").click(function(){
    $(".spec_webModel").removeClass("spec_top_cutnbtn");
    $(this).addClass("spec_top_cutnbtn");    
    $(".spec_pageCon").css('width', $(this).attr('device-width'));
    specCenter($(this).attr('device-width') == "100%");
});

$(".spec_webPage").click(function(){$(".spec_midCon").hide();$(".spec_web_midCon").show();});
$(".spec_padPage").click(function(){$(".spec_midCon").hide();$(".spec_pad_midCon").show();});
$(".spec_mobPage").click(function(){$(".spec_midCon").hide();$(".spec_mob_midCon").show();});
$(".spec_webPage,.spec_padPage,.spec_mobPage").click(function(){$(".spec_top_rmenu").removeClass("spec_top_rmenucut");$(this).addClass("spec_top_rmenucut");});

$(".spec_front").click(function(){$(".spec_mrImgCon li").removeClass("spec_mrckImg");$(this).parents("li").addClass("spec_mrckImg");});

$(".spec_mr_backBtn").click(function(){$(this).parents("li").removeClass("spec_mrckImg")});
$(".spec_top_subMenu").click(function(){$(".spec_top_slideMneu").slideToggle(200)});
$(".spec_top_subMenu").hover(function(){return false;},function(){$(".spec_top_slideMneu").slideUp(200)});

$(".spec_mimt").click(function(){$(this).toggleClass("spec_mimt2");$(this).next(".spec_mim_tools").slideToggle(300)});

$(".spec_mlBomBtn").click(function(){
    $(".spec_top_link .btn-group .btn").removeClass("active");
    $(".spec_mleft").animate({"left":"-234px"},300);
    $(".spec_mid").animate({"left":0},300);
});

$(".spec_top_link .btn-group .btn").click(function(){
    $('.spec_mlCon').hide();
    $('#'+$(this).attr("left")).show();
	$(".spec_mleft").animate({"left":0},300);
    $(".spec_mid").animate({"left":"234px"},300);
});

$('.spec_mlCon').perfectScrollbar({wheelSpeed: 30});
$('.spec_pageCon').perfectScrollbar({wheelSpeed: 30});

$('.spec_imgBg5').parent().click(function(){
    $(this).toggleClass('spec_top_cutnbtn');
    if ($('.spec_mright').is(":visible")){
        $('.spec_mpage').css('right', 0);
        $('.spec_mright').hide();
    }else{
        $('.spec_mpage').css('right', 235);
        $('.spec_mright').show();
    }
    
});


})
//屏蔽页面错误
jQuery(window).error(function(){
  return true;
});
jQuery("img").error(function(){
  $(this).hide();
});