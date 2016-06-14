var topController = {};

topController.top = $pageInfo.top;

utilService.render('topController', topController);

function specCenter(fullSize){
    if (fullSize){
        $(".spec_pageCon").css("left",0);
    }else{
        $(".spec_pageCon").css("left",($(".spec_mid").width()-$(".spec_pageCon").width())/2);
    }
    $('.spec_pageCon').perfectScrollbar('update');
}

$(function(){
//选项卡滑动切换通用
    $(".spec_hoverTag .spec_chgBtn").hover(function(){$(this).parent().find(".spec_chgBtn").removeClass("spec_chgCutBtn");$(this).addClass("spec_chgCutBtn");var cutNum=$(this).parent().find(".spec_chgBtn").index(this);$(this).parents(".spec_hoverTag").find(".spec_chgCon").hide();$(this).parents(".spec_hoverTag").find(".spec_chgCon").eq(cutNum).show();});

    //选项卡点击切换通用
    $(".spec_clickTag .spec_chgBtn").click(function(){$(this).parent().find(".spec_chgBtn").removeClass("spec_chgCutBtn");$(this).addClass("spec_chgCutBtn");var cutNum=$(this).parent().find(".spec_chgBtn").index(this);$(this).parents(".spec_clickTag").find(".spec_chgCon").hide();$(this).parents(".spec_clickTag").find(".spec_chgCon").eq(cutNum).show();});

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

    $(".spec_top_link a").click(function(){
        $('.spec_top_link a').removeClass();
        $(this).addClass('spec_active');
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

    $("#stl-redo").click(function() {
        if ($(this).hasClass('disabled') == false){
            $.post($pageInfo.stlTemplateUrl, 
                {
                    operation: 'UndoRedo',
                    isRedo: 'true'
                }, 
                function(data, textStatus) {
              var obj = $.parseJSON(data);
              if(obj.success == 'false'){
                alert(obj.errorMessage);
              }else{
                location.reload(true);
              }
                }
            );
        }
    });

    $("#stl-undo").click(function() {
        if ($(this).hasClass('disabled') == false){
            $.post($pageInfo.stlTemplateUrl, 
                {
                    operation: 'UndoRedo',
                    isUndo: 'true'
                }, 
                function(data, textStatus) {
              var obj = $.parseJSON(data);
              if(obj.success == 'false'){
                alert(obj.errorMessage);
              }else{
                location.reload(true);
              }
                }
            );
        }
    });

    $("#stl-save").click(function() {
        if ($(this).hasClass('disabled') == false){
            $.post($pageInfo.stlTemplateUrl, 
                {
                    operation: 'Save'
                },
                function(data, textStatus) {
              var obj = $.parseJSON(data);
              if(obj.success == 'false'){
                alert(obj.errorMessage);
              }else{
                location.reload(true);
              }
                }
            );
        }
    });
});