/**
 * @class jQuery
 * 本类是Xwb JavaScript API对jQuery方法的扩展。
 */
(function(X, $){
	var FALSE = false,
		TRUE = true,
		NULL = null,
		toInt = parseInt,
		isIE6 = !!($.browser.msie && $.browser.version == '6.0');;
	// 图片控制组件
	var imgCtrler = function() {
	    this.cid = 'imageCtrler';
	    this.canvas = NULL;
	    this.maxWidth = 440;
	    this.width = 0;
	    this.height = 0;
	    this.curAngle = 0;
	};

	imgCtrler.prototype = {
	    // 初始化，支持canvas的创建canvas，IE使用矩阵滤镜
	    init: function(data) {
	        var _el = data.el;
	        
	        this.width = _el.offsetWidth;
	        this.height = _el.offsetHeight;
            
            var ver = this.browserVer = parseInt($.browser.version);

	        if($.browser.msie && (document.documentMode || ver) < 9 ) {
	            var _matrix = 'DXImageTransform.Microsoft.Matrix';

	            _el.style.filter = 'progid:DXImageTransform.Microsoft.Matrix()';
	            _el.filters.item(_matrix).SizingMethod = "auto expand";

                $(_el).addClass('narrow-move');
	            _matrix = NULL;
	        } else {
	            this.canvas = $('<canvas></canvas>')
	                .attr({
	                    'height': this.height,
	                    'width': this.width
	                })
					.addClass('narrow-move')
					.attr('rel', 'e:zo')
	                .insertBefore(_el)[0];
	            
	            if(this.canvas.getContext) {
	                $(_el).hide();
	                
	                var ctx = this.canvas.getContext('2d');
	                
	                //ctx.drawImage(_el,0,0);
	
	            }
	        }
            var container = _el.parentNode;
            this._clientWidth = container.clientWidth;//变换区域宽度
            this._clientHeight = container.clientHeight;//变换区域高度
	        
	        this.element = _el;
	    },
	    
	    //旋转图片
	    //旋转方式，'left'或者'right'
	    rotate: function(dir) {
	        if(!this.element) {return;}
	        
	        //相对原始图片的旋转角度
	        var _angle;
	        if(dir === 'right') {
	            _angle = this.curAngle + 90;
	            this.curAngle = _angle>=360 ? 0 : _angle;
	        }else if(dir === 'left') {
	            _angle = this.curAngle - 90;
	            this.curAngle = _angle<0 ? 360+_angle : _angle;
	        }
	        _angle = NULL;
	        
	        //调整图片旋转后的大小
	        var drawW,drawH, h=this.width,w=this.height;
	            
	        this.width = w;
	        this.height = h;
	
	        if(w > this.maxWidth) {
	            h = toInt(this.maxWidth * h/w);
	            w = this.maxWidth;
	        }
	        
	        if(this.canvas) {
	            var 
	                ctx = this.canvas.getContext('2d'), el = this.element, 
	                cpx=0, cpy=0;
	            //设置画布大小，重置了内容
	            $(this.canvas).attr({
	                'width': w,
	                'height': h
	            });
	            
	            ctx.clearRect(0,0,w,h);
	            
	            switch(this.curAngle) {
	                case 0:
	                    cpx = 0;
	                    cpy = 0;
	                    drawW = w;
	                    drawH = h;
	                    break;
	                case 90:
	                    cpx = w;
	                    cpy = 0;
	                    drawW = h;
	                    drawH = w;
	                    break;
	                case 180:
	                    cpx = w;
	                    cpy = h;
	                    drawW = w;
	                    drawH = h;
	                    break;
	                case 270:
	                    cpx = 0;
	                    cpy = h;
	                    drawW = h;
	                    drawH = w;
	                    break;
	            }
	            
	            ctx.save();
	            ctx.translate(cpx,cpy);
	            ctx.rotate(this.curAngle * Math.PI/180);
	            ctx.drawImage(el, 0, 0, drawW,drawH);
	            ctx.restore();
	            
	        }else {
	            var 
	                _rad = this.curAngle * Math.PI/180,
	                _cos = Math.cos(_rad),
	                _sin = Math.sin(_rad),
	                _el = this.element,
	                _matrix = 'DXImageTransform.Microsoft.Matrix';
	                
	            _el.filters.item(_matrix).M11 = _cos;
	            _el.filters.item(_matrix).M12 = -_sin;
	            _el.filters.item(_matrix).M21 = _sin;
	            _el.filters.item(_matrix).M22 = _cos;
	            
	            // this.width = _el.offsetWidth;
	            // this.height = _el.offsetHeight;
	            
	            switch(this.curAngle) {
	                case 0:
	                case 180:
	                    drawW = w;
	                    drawH = h;
	                    break;
	                case 90:
	                case 270:
	                    drawW = h;
	                    drawH = w;
	                    break;
	            }
	            
	            _el.width = drawW;
	            _el.height = drawH;
                
                //_el.style.top = ( this._clientHeight - _el.offsetHeight ) / 2 + "px";
                //_el.style.left = ( this._clientWidth - _el.offsetWidth ) / 2 + "px";
                
	            //修正IE8下图片占位的问题
	            //18是操作菜单的高度
	            if((document.documentMode || this.browserVer) == 8) {
	                var $_el = $(_el);
                    $(_el).css({ 'display': 'block', 'margin-left': (this._clientWidth - _el.offsetWidth) / 2 })
                    .parent().height($_el.height());
	            }
	        }
	    }
	};

	$.extend($.fn, {
		   /**
			* 文字放大渐隐（微博数增加效果）
			*@param {Number} num 增加数值
			*@param {Number} times 放大倍数
			*@return {jQuery}
			*/
			zoomText: function(num, times) {
				this.each(function() {
					var
						$clone,
						$el = $(this),
						offset = $el.offset(),
						text = $el.text();
						
					times = isNaN(times) ? 2 : times;
					
					if(!isNaN(+text)) {
						text = +text + (num || 1);
					}
					
					$el.text(text);
					
					$clone = $el.clone()
						.attr('id', '')
						.css({
							'position': 'absolute',
							'top': offset.top,
							'left': offset.left,
							'font': $el.css('font'),
							'color': $el.css('color')
						})
						.appendTo($(document.body));
					
					var fontsize = times * parseInt($el.css('font-size'));
					
					$clone.animate({
						'font-size': fontsize,
						'top': offset.top - ($el.height()/4),
						'left': offset.left - ($el.width()/2),
						'opacity': 0.1
					}, {
						'duration': 300,
						'complete': function() {
							$clone.remove();
						}
					});
					
				});
				
				return this;
			},

			/**
			* 图片左右旋转
			* @param {String} dir  旋转方式，'left'或者'right'
			* @return {jQuery} this
			*/
			imgRotate: function(dir) {
			   
				this.each(function() {
					if(this.tagName !== 'IMG') {return FALSE};
					var img = $(this).data('img');

					if (!img)
					{
						var img = new imgCtrler();
						img.init({el: this});
						
						img.maxWidth = $(this).parent().width();

						$(this).data('img', img);
					}
					
					img.rotate(dir);
				});
				
				return this;
			},
			/**
			 * 文本输入框加上聚焦清空，失焦停留提示功能。<br/>
			 如果利用{@link Xwb.ax.ValidationMgr}类作表单验证，制作类似功能时不必直接采用该方法，
			 Validator类提供一系列验证器无需写代码即可轻松实现，详见该类的各种验证器。<br/>
			 如果当前文本框已经过{@link Xwb.ax.SelectionHolder}实例处理，
			 则focusText方法会利用当前{@link Xwb.ax.SelectionHolder}实例输出文本。
			 * @param {String} hoverText 停留提示文字
			 * @param {String} [focusStyle] 修饰样式类
			 * @param {DomSelector} [cssNode]
			 * @param {Boolean} [removeOnFocus] 如果为false，当聚焦后添加css类，否则移除css类
			  <pre><code>
                $('#id').focusText('这里输入用户名', 'focusStyle');
              </code></pre>
			 */
			focusText : function(text, css, cssNode, removeOnFocus){
			    this.each(function(){
			        $(this).focus(function(){
			            if(this.value === text){
			                var selHolder = $(this).data('xwb_selholder');
			                if(selHolder)
			                    selHolder.setText('');
			                else this.value = '';
			            }
			            if(css){
			                if(removeOnFocus)
			                    $(cssNode||this).removeClass(css);
			                else $(cssNode||this).addClass(css);
			            }
			        })
			        .blur(function(){
			            if($.trim(this.value) === ''){
			                var selHolder = $(this).data('xwb_selholder');
			                if(selHolder)
			                    selHolder.setText(text);
			                else this.value = text;
			            }
			            if(css){
			                if(removeOnFocus)
			                    $(cssNode||this).addClass(css);
			                else $(cssNode||this).removeClass(css);
			            }
			        });
			    });
			},
			
			/**
			 * 方法使用'hidden'样式控制元素的显示或隐藏状态。
			 * 如果无参数，返回当前元素hidden样式的状态，否则利用'hidden'CSS类进行隐藏或显示元素。
			  <pre><code>
			    // 获得显示状态
			    if ($('#id').cssDisplay()) {}
			    // 显示
			    $('#id').cssDisplay(true);
             </code></pre>
             *@return {Boolean|jQuery}
			 */
			cssDisplay : function(b){
			    var len = this.length, jq;
			    if(len){
			        if(len === 1){
			            if(b === undefined){
			                var v = !this.hasClass('hidden');
			                return v;
			            }else {
			                if(b) this.removeClass('hidden');
			                else this.addClass('hidden');
			            }
			        }
			        
			        else {
			            this.each(function(){
    			            if(b) $(this).removeClass('hidden');
			                else $(this).addClass('hidden');
			            });
			        }
			    }
			    return this;
			},
			
            /**
             * 检查是否含有某个样式,如果有,添加或删除该样式.
             * @param {String} css 样式名称
             * @param {Boolean} addOrRemove true 时添加样式，false时移除该样式
             * @return {jQuery} this
             */
			checkClass : function(cs, b){
			    if(cs){
    			    this.each(function(){
    			        var jq = $(this);
            			var hc = jq.hasClass(cs);
            			if(b){
            				if(!hc)
            				jq.addClass(cs);
            			}else if(hc){
            				jq.removeClass(cs);
            			}			        
    			    });
			    }
        		return this;
			},
			
            /**
            * 替换view元素样式类.<br/>
            * <code>comp.switchClass('mouseoverCss', 'mouseoutCss');</code><br/>
            * @param {String} oldSty 已存在的CSS类名
            * @param {String} newSty 新的CSS类名
            * @return {Object} this
            */
              switchClass: function(oldSty, newSty) {
                    this.each(function(){
                        var jq = $(this);
                        jq.removeClass(oldSty);
                        jq.addClass(newSty);
                    });
                    return this;
              },
              
              /**
               * 获得相对于viewport的位置，只适用于单个元素
               * @return {left, top}
               */
              absolutePos : function(){
                    var off = this.offset(), doc = $(document);
                    var st  = doc.scrollTop(), sl = doc.scrollLeft();
                    off.left -= sl;
                    off.top -= st;
                    return off;
              },
              
              slideMenu : function(slideLayer, hoverCs){
                this.each(function(){
                    (function(jq){
                        var layer = jq.find(slideLayer);
                        var setTimer, clsTimer;
                        function slidedown(){
                            layer.show().cssDisplay(true);
                            if(hoverCs)
                                jq.addClass(hoverCs);
                        }
                        
                        function slideup(){
                            if(hoverCs)
                                jq.removeClass(hoverCs);
                            layer.cssDisplay(false);
                        }
                        
                        function clear() { 
                            if(setTimer){
                                clearTimeout(setTimer);
                                setTimer = false;
                            }
                            clsTimer = setTimeout(slideup, 80);
                        }
                        
                        function set(){
                            if(clsTimer){
                                clearTimeout(clsTimer);
                                clsTimer = false;
                            }
                            setTimer = setTimeout(slidedown, 100);
                        }
                        
                        jq.hover(set, clear);
                    })($(this));
                });
              },

		/**
	    * 截取微博内容
	    *@param {Number} num  位置，默认10
        *@param {Boolean} hasFace  是否显示表情图片，否为文字代替
        *@param {String} postfix  后缀
        *@return jQuery
	    */
        substrText: function(num, hasFace, postfix) {
            var re = new RegExp('(?:<a.*?>.*?<\\/a>)|(?:<img.*?>)|.','gi');
            
	        this.each(function() {
                var 
                    cache = [],
                    postfix = postfix || '...',
                    text = this.innerHTML,
                    match = text.match(re);
                    
                num = num||10;
                    
                if(match && match.length > num) {
                    
                    match = match.slice(0, num).join('');

                    text = hasFace ? match : match.replace(/<img.*?title=\"(.*?)\".*?>/gi, '[$1]');
                    
                    $(this).html(text+postfix);
                }
	        });
            
            return this;
        },

		/**
	    * IE6修复PNG图片
	    *@return jQuery
	    */
        fixPng: function() {			

            if(isIE6) {
            	var fixFn = function() {
                    if(this.tagName == 'IMG') {
                        var $img = $('<span></span>').css({
                            width: this.offsetWidth,
                            height: this.offsetHeight,
                            display: 'inline-block'
                        });
                        $img[0].style.filter = 'progid:DXImageTransform.Microsoft.AlphaImageLoader(src="'+this.src+'", sizingMethod="crop")';                        
                        $(this).replaceWith($img);
                    }
                }
                this.each(function(){
                	if(this.complete){
                		fixFn.call(this);
                	} else {
                		this.onload = fixFn ;
                	}
                });
            }
            
            return this;
        },

        makeScroll : function(opt){
            var scrollor = this[0];
            var tid,cid;
            var dir = 1;
            var nxtBtn = $(opt.nextBtn);
            var prevBtn = $(opt.prevBtn);
            
            function updateStatus(){
                if(opt.ldisabledCs){
                    var show = scrollor.scrollWidth > scrollor.offsetWidth;
                    prevBtn.cssDisplay(show);
                    nxtBtn.cssDisplay(show);
                    prevBtn.checkClass(opt.ldisabledCs, scrollor.scrollLeft==0);
                    nxtBtn.checkClass(opt.rdisabledCs, scrollor.scrollLeft + scrollor.offsetWidth === scrollor.scrollWidth);
                }
            }
            
            opt.pace = opt.pace||25;
            
            function timer(){
                var nxt = scrollor.scrollLeft + opt.pace * dir;
                if(nxt<0)
                    nxt = 0;
                if(nxt>scrollor.scrollWidth)
                    nxt = scrollor.scrollWidth;

                scrollor.scrollLeft = nxt;
                updateStatus();
                tid = setTimeout(arguments.callee, opt.interval||20);
            }
            
            var clicked;
            function startRoll(){
                clicked = false;
                timer();
            }
            
            function leftMousedown(){
                clicked = true;
                dir = -1;
                mid = setTimeout(startRoll, 200);
                return false;
            }
            
            function rightMousedown(){
                clicked = true;
                dir = 1;
                mid = setTimeout(startRoll, 200);
                return false;             
            }
            
            function mouseup(){
                if(clicked){
                    var nxt = scrollor.scrollLeft + opt.pace * dir * 4;
                    if(nxt<0)
                        nxt = 0;
                    if(nxt>scrollor.scrollWidth)
                        nxt = scrollor.scrollWidth;
                    
                    $(scrollor).animate(
                        {scrollLeft : nxt}, 'fast', 
                        function(){ updateStatus();}
                    );
                }
                
                clearTimeout(tid);
                clearTimeout(mid);
            }
            
            if(opt.nextBtn)
                $(opt.nextBtn).mousedown(rightMousedown).mouseup(mouseup).click(function(){return false;});
            
            if(opt.prevBtn)
                $(opt.prevBtn).mousedown(leftMousedown).mouseup(mouseup).click(function(){return false;});
							
			if(opt.moveScorll) {
				nxtBtn.hover(rightMousedown,mouseup);
				prevBtn.hover(leftMousedown,mouseup);
			}
            var offw = 0;
            if(opt.items){
                $(scrollor).find(opt.items).each(function(){
                    offw += $(this).outerWidth(true);
                });
                $(scrollor).find(opt.ct||':first-child').css('width', offw);
            }
            
            updateStatus();
            
            return opt;
        },
		scrollView: function() {
		   var parent = this,
		   
				childrens = parent.children(),
			   
			   //时间触发计时器
			   timer,
			   
			   //滚动触发周期, 真实等待时间应该为 delay - 1000 - 500
			   //1000是滚动效果的时长
			   //500是淡入效果时长
			   delay = 4000;
		   
		   if (!childrens.length) {
			   return;
		   }
		   
		   //滚动到下一条
		   function scrollToNext(callback) {
			   var $last = $(parent.children(':last'));
			   
			   var height = $last.height();
			   
			   $last
			   .height(0)
			   .css({opacity: 0, 'font-size':0, overflow: 'hidden'})
			   .remove()
			   .prependTo(parent)
			   .animate({height: height}, 1000, 'cubicOut', function(){$last.css({'font-size': '12px'});})
			   .animate({opacity: 1}, 500, function() {
					!timer && $.isFunction(callback) && callback();   
			   });
	  
		   }
		   
		   //设置计时器,开始滚动
		   function startScroll () {
			   if (!timer) {
				timer = window.setInterval(scrollToNext, delay);
			   }
		   }
		   
		   //计算内容高度，如果内容高度小于等于高度，则不启动滚动效果
		   var contentHeight = 0;
		   
		   $.each(childrens, function(i, node) {
			   contentHeight += $(node).height();
		   });
		   
		   if (contentHeight > parent.height()) {
			   //鼠标飘过的控制，
			   //mouseover时停止计时器，但当前的滚动会继续，直至单次完成。
			   parent.hover(function(){
				   if (timer) {
					   timer = window.clearInterval(timer);
				   }
				   
			   }, function() {
				   startScroll();
			   });
			   
			   startScroll();
		   }
	   }
	   //整块的循环移动
	   //Count 一次移动子元素数量
	   //delay 移动间隔时间
	   //animateTime 动画执行时间
	   ,scrollMass : function(Count,delay,animateTime){
			var parent =this,
				length = parent.children().length,
				timer,
				delay = delay,
				tmp = parent.children()[0].offsetWidth * Count,
				p = false;
		   if (length <= Count) {
			   return this; 
		   }
		   function scrollNext(){
				parent.animate({marginLeft:-tmp},animateTime?animateTime:1000,function(){
					if( timer !=0 ){
						parent.css('marginLeft',0);
						parent.append( parent.children(':lt(' +  Count + ')') );
					}
					if( p ) return;
					start();
				});
		   };
		   
		   function start(){ timer = window.setTimeout(scrollNext,delay); }
		   this.hover(function(){ 
				p = true ;clearTimeout(timer); 
		   },function(){ 
				p = false;start();
		   });
		   start();
		   return this;
	   }
	});
	
/**
 * jQuery 高亮查找插件，完全基于文本结点的DOM操作，对原有非文本结点不造成任何影响。
 * @param {Array} keywords 高亮文本数组
 * @param {String} [style] 应用于高亮文本的样式类
 * 用法:
 <pre><code>
   高亮:
   $('#ss').highlight(['keyword', ... ]);
   清除:
   $('#ss').clearHighlight();
   
   或者传递自定义的样式类
   $('#ss').highlight(['keyword', ... ], 'cssClass');
   清除:
   $('#ss').clearhighlight('cssClass');   
 </code></pre>
 请确保每次高亮前清除先前已高亮内容。
 可选配置信息：$.fn.highlight.cls
 * @method highlight
*/
 
(function(){

// private
var IGNORE,S,ESC,LT,GT, inited;

// private
function init(){
    IGNORE = /INPUT|IMG|SCRIPT|STYLE|HEAD|MAP|AREA|TEXTAREA|SELECT|META|OBJECT|IFRAME/i;
    S      = /^\s+$/;
    ESC    = /(\.|\\|\/|\*|\?|\+|\[|\(|\)|\]|\{|\}|\^|\$|\|)/g;
    LT     = /</g;
    GT     = />/g;
    inited = TRUE;
}

// entry
function entry(keys, cls){
    if (!keys) {
        return;
    }
    
    if(!inited)
      init();
  
    if(typeof keys === 'string')
      keys = $.trim(keys).split(S);

    // normalize
    var arr = [];
    for(var i=0,len=keys.length;i<len;i++){
       if(keys[i] && !S.test(keys[i])) {
          arr[arr.length] = keys[i].replace(LT, '&lt;')
                                   .replace(GT, '&gt;')
                                   .replace(ESC, '\\$1');
       }
    }
    var reg     = new RegExp('(' + arr.join('|') + ')', 'gi'),
        placing = '<span class="'+(cls||entry.cls)+'">$1</span>',
        div     = document.createElement('DIV');
    this.each(function(){
      highlightEl(this, reg, placing, div);
    });

	return this;
}

// public
$.fn.highlight = entry;
/**
 * 清除高亮
 * @see {@link #highlight}
 * @method clearHighlight
 */
$.fn.clearHighlight = function(cls) {
  if(!inited)
    init();
  var cls = cls||entry.cls;
  this.each(function(){
    clearElhighlight(this, cls);
  });
};


// private
function replaceTextNode(textNode, reg, placing, div) {
  var data = textNode.data;
  if(!S.test(data)){
     data = data.replace(LT, '&lt;').replace(GT, '&gt;');
     if(reg.test(data)){
        if(!div)
          div = document.createElement('DIV');
        // html escape
        div.innerHTML = data.replace(reg, placing);
        // copy nodes
        var chs = div.childNodes,
            arr = [],
            fr = document.createDocumentFragment();
        
        // copy to array
        for(var i=0,len=chs.length;i<len;i++)
          arr[i] = chs[i];
        
        for(i=0;i<len;i++)
          fr.appendChild(arr[i]);
        
        textNode.parentNode.replaceChild(fr, textNode);
     }
  }
}

// private
function highlightEl(el, reg, placing, div){
    var chs = el.childNodes, 
        arr = [], i, len, nd;
      
      // copy to array
      for(i=0,len=chs.length;i<len;i++){
        if(!IGNORE.test( chs[i].tagName ))
          arr.push(chs[i]);
      }
      
      for(i=0,len=arr.length;i<len;i++){
        nd = arr[i];
        // textnode
        if(nd.nodeType === 3){
          try { 
            replaceTextNode(nd, reg, placing, div);
          }catch(e){}
        }else arguments.callee(nd, reg, placing, div);
      }
}


// private
function clearElhighlight(el, cls) {
  var chs = el.childNodes, 
      arr = [], i, len, nd, t;
      
  // copy to array
  for(i=0,len=chs.length;i<len;i++){
    if(!IGNORE.test( chs[i].tagName ))
    arr.push(chs[i]);
  }

  for(i=0,len=arr.length;i<len;i++){
    nd = arr[i];
    t = nd.nodeType;
    // textnode
    if(t === 3)
      continue;
    // span
    if(t === 1 && nd.tagName === 'SPAN' && nd.className === cls){
      // merge text nodes
      var textNode = nd.childNodes[0], 
          p        = nd.parentNode,
          pre      = nd.previousSibling,
          nxt      = nd.nextSibling;
      
      if(pre && pre.nodeType === 3){
        p.removeChild(pre);
        textNode.data = pre.data + textNode.data;
      }
      
      if(nxt && nxt.nodeType === 3){
        p.removeChild(nxt);
        textNode.data = textNode.data + nxt.data;
      }

      p.replaceChild(textNode, nd);
    }else arguments.callee(nd, cls);
  }
}

entry.cls = 'search-txt';

})();

/**
 * 获得或设置cookie
 * @param {String} name
 * @param {String} value
 * @param {Object} options
 * @method
 */
$.cookie = function(name, value, options) {
    if (typeof value != 'undefined') { // name and value given, set cookie
        options = options || {};
        if (value === NULL) {
            value = '';
            options.expires = -1;
        }
        var expires = '';
        if (options.expires && (typeof options.expires == 'number' || options.expires.toUTCString)) {
            var date;
            if (typeof options.expires == 'number') {
                date = new Date();
                date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000));
            } else {
                date = options.expires;
            }
            expires = '; expires=' + date.toUTCString(); // use expires attribute, max-age is not supported by IE
        }
        // CAUTION: Needed to parenthesize options.path and options.domain
        // in the following expressions, otherwise they evaluate to undefined
        // in the packed version for some reason...
        var path = options.path ? '; path=' + (options.path) : '';
        var domain = options.domain ? '; domain=' + (options.domain) : '';
        var secure = options.secure ? '; secure' : '';
        document.cookie = [name, '=', encodeURIComponent(value), expires, path, domain, secure].join('');
    } else { // only name given, get cookie
        var cookieValue = NULL;
        if (document.cookie && document.cookie != '') {
            var cookies = document.cookie.split(';');
            for (var i = 0; i < cookies.length; i++) {
                var cookie = jQuery.trim(cookies[i]);
                // Does this cookie string begin with the name we want?
                if (cookie.substring(0, name.length + 1) == (name + '=')) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break;
                }
            }
        }
        return cookieValue;
    }
};

$.easing.cubicOut = function (x, t, b, c, d) {
return c*((t=t/d-1)*t*t + 1) + b;
} 

})(Xwb, jQuery);