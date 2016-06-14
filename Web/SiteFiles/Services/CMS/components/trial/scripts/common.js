$(function(){
    function bannerScorll(){
        var imgEls = $('.nav-imgs-con');
        var navEls = $('.nav-imgs-nav');
        var imgElLis = imgEls.find('li');
        var navElLis = navEls.find('li');
        swipe(navEls[0].parentNode, {
              auto: 3000,
              continuous: true,
          startSlide: 0,
              callback: function(pos) {

                var i = navElLis.length;
                while (i--) {
                  navElLis[i].className = ' ';
                }
                navElLis[pos].className = 'selected';

              }
        });
    }
  	bannerScorll();

    $(".nav-tabs li").click(function(){
        var index = $(this).index();
        var oDiv = $(".info_box");
        $(this).addClass("active").siblings().removeClass();
        oDiv.hide().eq(index).show();
    })
})