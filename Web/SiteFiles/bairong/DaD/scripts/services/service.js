function htmlAttributesDecode(str)  
{  
   if (!str) return "";
   var s = "";
   s = str.replace(/_lt_/g, "<");  
   s = s.replace(/_gt_/g, ">");
   s = s.replace(/_amp_/g, "&");
   s = s.replace(/_dq_/g, '"');
   s = s.replace(/_sq_/g, "'");
   s = s.replace(/_eq_/g, "=");
   s = s.replace(/_n_/g, '\n');
   s = s.replace(/_r_/g, '\r');
   return s;  
}   

$(document).ready(function(){

    $('.stl-element-tips').hover(function(){
      layer.tips('<div class="spec_tips">' + htmlAttributesDecode($(this).attr('tipHtml')) + '</div>', this, {
          style: ['background-color:#47A447; color:#fff; height: 28px; top: -15px;', '#47A447'],
          guide: 2,
          time: 5
      });
    });

    $('.stl-tips').hover(function(){
        layer.tips($(this).attr('tip-content'), this, {
            style: ['background-color:#3595CC; color:#fff', '#3595CC'],
            guide: parseInt($(this).attr('tip-guide')),
            time: 3
        });
    });

});