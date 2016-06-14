jQuery(function(){
//选项卡滑动切换通用
jQuery(function(){jQuery(".hoverTag .chgBtn").hover(function(){jQuery(this).parent().find(".chgBtn").removeClass("chgCutBtn");jQuery(this).addClass("chgCutBtn");var cutNum=jQuery(this).parent().find(".chgBtn").index(this);jQuery(this).parents(".hoverTag").find(".chgCon").hide();jQuery(this).parents(".hoverTag").find(".chgCon").eq(cutNum).show();})})

//选项卡点击切换通用
jQuery(function(){jQuery(".clickTag .chgBtn").click(function(){jQuery(this).parent().find(".chgBtn").removeClass("chgCutBtn");jQuery(this).addClass("chgCutBtn");var cutNum=jQuery(this).parent().find(".chgBtn").index(this);jQuery(this).parents(".clickTag").find(".chgCon").hide();jQuery(this).parents(".clickTag").find(".chgCon").eq(cutNum).show();})})

$(".msc_lay2").hover(function(){$(".msc_lay2hover,.msc_lay2box").show();},function(){$(".msc_lay2hover,.msc_lay2box").hide();});

$(".bmr_dl dt").click(function(){
	$(".bmr_dl").removeClass("bmr_cutdl");
	$(this).parents(".bmr_dl").addClass("bmr_cutdl");
	});
	
var m2h1=$(".bmr_m2L").height();
var m2h2=$(".bmr_m2R").height();
if(m2h1>m2h2){
	$(".bmr_m2R").height(m2h1);
	}else{
	$(".bmr_m2L").height(m2h2);	
		}
		
//评论
$(".bmrp_tkBtn").click(function(){
	$(this).parents(".bmrp_d2").find(".bmrp_dTk").slideToggle(200);
	});
		
jQuery(".bmr_imgEs").slide({titCell:"",mainCell:".bmr_imgBox ul",autoPage:true,effect:"left",vis:7});
jQuery(".bmr_res1").slide({titCell:"",mainCell:".bmr_resimgBox ul",autoPage:true,effect:"top",vis:2});



})
//屏蔽页面错误
jQuery(window).error(function(){
  return true;
});
jQuery("img").error(function(){
  $(this).hide();
});