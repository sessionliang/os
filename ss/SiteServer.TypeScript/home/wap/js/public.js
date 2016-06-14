jQuery(function(){
//选项卡滑动切换通用
jQuery(function(){jQuery(".hoverTag .chgBtn").hover(function(){jQuery(this).parent().find(".chgBtn").removeClass("chgCutBtn");jQuery(this).addClass("chgCutBtn");var cutNum=jQuery(this).parent().find(".chgBtn").index(this);jQuery(this).parents(".hoverTag").find(".chgCon").hide();jQuery(this).parents(".hoverTag").find(".chgCon").eq(cutNum).show();})})

//选项卡点击切换通用
jQuery(function(){jQuery(".clickTag .chgBtn").click(function(){jQuery(this).parent().find(".chgBtn").removeClass("chgCutBtn");jQuery(this).addClass("chgCutBtn");var cutNum=jQuery(this).parent().find(".chgBtn").index(this);jQuery(this).parents(".clickTag").find(".chgCon").hide();jQuery(this).parents(".clickTag").find(".chgCon").eq(cutNum).show();})})


$(".mbom_backTop").click(function(){
	$("body,html").animate({"scrollTop":0},300);
	});


$(window).scroll(function(){
	if($(document).scrollTop()>95){
		$(".mfixMenu").fadeIn(500);
		}else{
		$(".mfixMenu").fadeOut(500);	
			}
	})

})

//屏蔽页面错误
jQuery(window).error(function(){
  return true;
});
jQuery("img").error(function(){
  $(this).hide();
});