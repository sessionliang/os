// JavaScript Document
(function($) {
$.fn.scrollFx = function(o) {
return this.each(function() {
var self = this;

self.$ul = $("ul",this),self.$li = $("li", this),self.$l = self.$li.length,self.$w = parseInt (self.$li.outerWidth(true)),self.$max_x = self.$l * self.$w,$autoId = self;
self.$ul.append(self.$li.slice(0,5).clone());
auto();

function auto(){  
self.$autoId = setInterval(showImg,20);
};  

function showImg (){
self.$ul = $("ul",self);
self.$le = parseInt(self.$ul.css("left"));

self.$le > -self.$max_x ? self.$le-- : self.$le = 0;
self.$ul.css({left:self.$le+"px"});
};

self.$ul.mouseover(function(){
clearInterval(self.$autoId);                 
});

self.$ul.mouseleave(function(){
auto();  
});

});
};
})(jQuery)