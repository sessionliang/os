/*!
 * 定义ui类的HTML模块。
 * 只有动态创建的HTML才有必要放到这里。
 * 模板用法可参考Xwb.util.Tpl类
 */

// htmls可任意改，但ID模板名称和rel属性不能更改
Xwb.ax.Tpl.reg({
    
    // Xwb.ui.Box
    Box : [
        '<div class="win-pop {.cs}">',
            '<div class="win-t"><div></div></div>',
            '<div class="win-con">',
                '<div class="win-con-in">',
                    '[?.title?{BoxTitlebar}?]',
                    '<div class="win-box" id="xwb_dlg_ct">',
                        '{.contentHtml}',
                    '</div>',
                '</div>',
                '<div class="win-con-bg"></div>',
            '</div>',
            '<div class="win-b"><div></div></div>',
			'[?.closeable?<a href="#" class="ico-close-btn" id="xwb_cls" title="關閉"></a>?]',
            '{.boxOutterHtml}',
        '</div>'
     ].join(''),
     
     // Xwb.ui.Dialog
     DialogContent : '{.dlgContentHtml}<div class="btn-area" id="xwb_dlg_btns">{.buttonHtml}</div>',
     
     BoxTitlebar : '<h4 class="win-tit x-bg"><span id="xwb_title">{.title}</span></h4>',
     
     Mask : '<div class="shade-div"></div>',
     
     // Xwb.mod.ForwardBox
     ForwardDlgContentHtml : [
            '<p id="xwb_forward_text"></p>',
            '<div class="forward-tool"><span id="xwb_warn_tip">還可以輸入140字</span><a href="#" class="icon-face-choose" id="xwb_face_trig"></a></div>',
            '<textarea id="xwb_fw_input" class="style-normal"></textarea>',
            '<div id="xwb_fw_lbl"></div>'
     ].join(''),
     
     ForwardDlgLabel : '<label><input type="checkbox" value="{.uid}">同時作為{.nick}的評論發布</label>',
     
     MsgDlgContent : '<div class="tips-c"><div class="icon-alert" id="xwb_msgdlg_icon"></div><p id="xwb_msgdlg_ct"></p></div>',
     
     Button : '<a href="#" class="btn-s1 {.cs}" id="xwb_btn_{.id}"><span>{.title}</span></a>',

	EmotionHotList: '<div class="hot-e-list" id="hotEm">{.hotList}</div>',
     
     EmotionBoxContent : [
        '<div class="win-tab bottom-line" id="cate">',
		 '	{.category} ',
        '</div>',
        '<div class="emotion-box" id="box">',
			'{EmotionHotList}',
            '<div class="e-list" id="flist0">',
				'{.faces}',
            '</div>',
        '</div>'
     ].join(''),
     ArrowBoxBottom  : '<div class="arrow"></div>',
     EmotionIcon       : '<a href="javascript:;" title="{.title}" rel="e:em"><img src="{.src}"></a>',
     Loading : '<div id="xweibo_loading" class="loading"></div>',
     CommentArea : [
        '<div class="feed-comment box-style hidden">',
          '<div class="box-t skin-bg"><span class="skin-bg"></span></div>',
          '<div class="box-content">{.cmtBoxHtml}<ul id="cmtCt" class="hidden"></ul><div class="more-comment" id="more">還有<span id="lefCnt"></span>條評論，<a href="{.cmtUrl}">點擊查看</a></div>',
          '</div>',
          '<div class="box-b skin-bg"><span class="skin-bg"></span></div>',
          '<span class="box-arrow skin-bg"></span>',
        '</div>'
    ].join(''),
    
    CmtBox : [
              '<div class="post-comment" id="cmtBox">',
                  '<div class="post-comment-main">',
                      '<a href="javascript:;" class="icon-face-choose" rel="e:ic"></a>',
                      '<a class="btn-s1" href="javascript:;" rel="e:sd"><span>評論</span></a>',
                      '<div class="comment-keyin style-normal"><div><textarea class="comment-textarea" id="inputor"></textarea></div></div>',
                  '</div>',
                  '<div>',
                      '<span class="keyin-tips" id="warn">您還可以輸入70字</span>',
                      '<label><input type="checkbox" id="sync"/>同時發一條微博</label>',
                  '</div>',
              '</div>'
   ].join(''),
    atwho:['<div class="atwho">',
            '    [?!.noAt?<h4 id="title">想用@提到誰？</h4>?]',
			'	<ul id="mainUL">',
			'	</ul>',
			'</div>'].join(''),
    MBCmtBox : [
    '<div class="feed-comment box-style" id="cmtBox">',
        '<div class="box-t skin-bg"><span class="skin-bg"></span></div>',
        '<div class="box-content">',
            '<div class="post-comment">',
                '<div class="post-comment-main">',
                    '<a class="icon-face-choose" href="#" rel="e:ic"></a>',
                    '<a href="javascript:;" class="btn-s1" rel="e:sd"><span>評論</span></a>',
                    '<div class="comment-keyin style-normal"><div><textarea id="inputor" class="comment-textarea style-normal"></textarea></div></div>',
                '</div>',
                '<div>',
                	'<span class="keyin-tips" id="warn">您還可以輸入70字</span>',
                    '<label><input type="checkbox" id="sync">同時發一條微博</label>',
                '</div>',
            '</div>',
                '</div>',
        '<div class="box-b skin-bg"><span class="skin-bg"></span></div>',
        '<span class="box-arrow skin-bg"></span>',
    '</div>'
    ].join(''),

    Comment : [
        '<li rel="c:{.id},n:{.nick}">',
            '<div class="user-pic">',
                '<a href="{.usrUrl}"><img src="{.profileImg}" title="{.nick}" alt="{.nick}的頭像" width="{.picSz}" height="{.picSz}" /></a>',
            '</div>',
            '<div class="comment-content">',
                '<p><a href="{.usrUrl}">{.nick}{.verifiedHtml}</a>：{.text}<span>({.time})</span></p>',
                '<div>[?.canDel?<a class="ico-del" href="javascript:;" rel="e:dl">刪除</a>?]<a class="ico-reply" href="javascript:;" rel="e:rp">回覆</a></div>',
            '</div>',
        '</li>'
    ].join(''),
    
    MBlogCmt : [
        '<li rel="c:{.id},n:{.nick}">',
            '<div class="user-pic">',
                '<a href="{.usrUrl}"><img alt="" src="{.profileImg}" /></a>',
            '</div>',
            '<div class="comment-c">',
                '<p class="c-info"><a href="{.usrUrl}">{.nick}{.verifiedHtml}</a> {.text}({.time}) </p>',
                '<div class="c-for" id="trigs">',
                    '<a href="javascript:;" class="ico-reply" rel="e:rp">回覆</a>',
					'[?.canDel?<a href="javascript:;" class="ico-del" rel="e:dl">刪除</a>?]',
                '</div>',
                '{.cmtBoxHtml}',
            '</div>',
        '</li>'
    ].join(''),
    
    Followed : '<span class="followed-icon {.cs}">已關注</span>',

    MediaBoxContentHtml : [
			'<div class="insert-box">',
				'<p>請輸入<a href="http://video.sina.com.cn" target="_blank">新浪播客</a>、<a href="http://www.youku.com" target="_blank">優酷網</a>、<a href="http://www.tudou.com" target="_blank">土豆網</a>、<a href="http://www.ku6.com/" target="_blank">酷6網</a>等視頻網站的視頻播放頁鏈接</p>',
				'<input type="text" id="xwb_inputor" class="style-normal"><a href="#" class="btn-s1" rel="e:ok"><span>確定</span></a>',
				'<p class="error-tips hidden" id="xwb_err_tip">你輸入的鏈接地址無法識別<span><a href="#" rel="e:cc">取消操作</a>或者<a href="#" rel="e:nm">作為普通的鏈接</a>發布。</span></p>',
			'</div>'
    ].join(''),

	PictureBox: [
		'<div class="box-style" style="display:none">',
		'	<div class="box-t skin-bg"><span class="skin-bg"></span></div>',
		'	<div class="box-content">',
		'   <div class="show-img"><p><a href="#" rel="e:zo" class="ico-piup">收起</a>',
		'   <a href="{.org}" target="_blank" class="ico-src ">查看原圖</a>',
		'   <a href="#" class="ico-turnleft" rel="e:tl">向左轉</a>',
		'   <a href="#" class="ico-turnright" rel="e:tr">向右轉</a></p>',
		'   <div name="img"></div></div>',
		'   </div>',
		'	<div class="box-b skin-bg"><span class="skin-bg"></span></div>',
		'	<span class="box-arrow skin-bg"></span>',
		'</div>'
	].join(''),
	
	//转发中的图片显示
	PictureBoxForward: [
		'<div class="show-img cutline" style="display:none">',
		'<p><a class="ico-piup" rel="e:zo" href="#">收起</a><a class="ico-src" target="_blank" href="{.org}">查看原圖</a><a rel="e:tl" class="ico-turnleft" href="#">向左轉</a><a rel="e:tr" class="ico-turnright" href="#">向右轉</a></p>',
		'<div name="img"></div></div>'
	].join(''),

	PreviewBox: [
		'<div class="preview-img"></div>'
	].join(''),
	
	VideoThumbHtml: [
		'<div class="feed-img"><img width="120px" height="90px" src="{.img}" alt="">',
		'<div class="video-view" rel="e:pv,i:{.id}"></div></div>'
	].join(''),
	
	VideoBox: [
		'<div class="box-style showbox" style="display:none">',
		'	<div class="box-t skin-bg"><span class="skin-bg"></span></div>',
		'	<div class="show-video box-content">',
		'   <p><a rel="e:cv" href="#" class="ico-piup">收起</a><span>|</span><a title="{.title}" target="_blank" href="{.href}">{.title}</a></p>',
		'   <div>{.flash}</div></div>',
		'	<div class="box-b skin-bg"><span class="skin-bg"></span></div>',
		'	<span class="box-arrow skin-bg"></span>',
		'</div>'
	].join(''),

	VideoBoxForward: [
		'<div class="cutline show-video">',
		'<p><a rel="e:cv" href="#" class="ico-piup">收起</a><span>|</span><a title="{.title}" target="_blank" href="{.href}">{.title}</a></p>',
		'<div>{.flash}</div></div>'
	].join(''),
    
    MusicBoxContentHtml : [
        '<div class="insert-box music-box">',
            '<p>请输入<a href="http://music.sina.com.cn/yueku/" target="_blank">新浪樂庫</a>等音樂站點的音樂播放鏈接</p>',
            '<input type="text" id="xwb_inputor" class="style-normal"><a href="#" class="btn-s1" rel="e:ok"><span>確定</span></a>',
            '<p class="error-tips hidden" id="xwb_err_tip">你的輸入的鏈接地址無法識別<span><a href="#" rel="e:cc">取消操作</a>或者<a href="#" rel="e:nm">作為普通的鏈接</a>發布。</span></p>',
        '</div>'
    ].join(''),
    
    UploadImgBtn : '<a href="#" rel="e:dlp" class="ico-close-btn" title="刪除"></a>',
    
    SearchDropdownList : '<ul class="head-searchlist"><li class="mouseover">含<span></span>的微博</li><li>名<span></span>的人</li></ul>',
    
    PrivateMsgContent : [
        '<div class="field"><label>發私信給：</label><input class="style-normal" type="text" id="sender" /><span class="tips-wrong hidden" id="warnPos">她還沒有關注你,暫時不能發私信</span></div>',
        '<div class="field"><label>私信内容：</label><textarea class="style-normal" id="content"></textarea></div>',
        '<input type="hidden" id="uid" value="">',
        '<div class="field-area">',
            '<div class="icon-face-choose" rel="e:ic"></div>',
			'<a class="btn-s1 btn-s1-light fl" href="javascript:;" rel="e:sd"><span>发送</span></a>',
			'<span class="tips-txt" id="word">您還可以輸入140字</span>',
        '</div>'
    ].join(''),
    
    NoticeLayer : [
        '<div class="new-tips-fixed hidden">',
            '<h4 id="xwb_title">提示</h4>',
                '<p id="wbs" class="hidden"><span id="c">0</span>條新微博，<a href="{.wbsUrl}">點擊查看</a></p>',
                '<p id="fans" class="hidden"><span id="c">0</span>個新粉絲，<a href="{.fansUrl}">點擊查看</a></p>',
                '<p id="cmts" class="hidden"><span id="c">0</span>條新評論，<a href="{.cmtsUrl}">點擊查看</a></p>',
                '<p id="msgs" class="hidden"><span id="c">0</span>封新私信，<a href="{.msgsUrl}">點擊查看</a></p>',
                '<p id="refer" class="hidden"><span id="c">0</span>條微博提到了，<a href="{.referUrl}">點擊查看</a></p>',
                '<p id="notify" class="hidden"><span id="c">0</span>條通知，<a href="{.notifyUrl}">點擊查看</a></p>',
            '<a href="#" class="ico-close-btn" id="xwb_cls" ></a>',
        '</div>'
    ].join(''),
    
    NoticeLayer2 : [
        '<div class="new-tips">',
            '<a id="wbs" class="hidden" href="{.wbsUrl}"><span id="c">0</span>條新微博</a>',
            '<a id="fans" class="hidden" href="{.fansUrl}"><span id="c">0</span>個新粉絲</a>',
            '<a id="cmts" class="hidden" href="{.cmtsUrl}"><span id="c">0</span>條新評論</a>',
            '<a id="msgs" class="hidden" href="{.msgsUrl}"><span id="c">0</span>封新私信</a>',
            '<a id="refer" class="hidden" href="{.referUrl}"><span id="c">0</span>條微博提到你</a>',
            '<a id="notify" class="hidden" href="{.notifyUrl}"><span id="c">0</span>條通知</a>',
            '<a href="javascript:;" class="ico-close-btn" id="xwb_cls"></a>',
        '</div>'
    ].join(''),
    
	//登录
	Login: [
		'<div class="login-area">',
		'	<span><a href="{.regUrl}">註冊帳戶</a></span><a class="btn-web-account bind-btn-bg" href="{.siteLoginUrl}">{.siteName}登入</a>',
		'	<span><a href="{.sinaRegUrl}" target="_blank">開通微博</a></span><a class="btn-sina-account bind-btn-bg" href="#" id="oauth">新浪微博帳戶登入</a>',
		'</div>',
		'<div class="bind-tips">提示：您可以使用{.siteName}帳戶或新浪微博帳戶登入本網站</div>'
	].join(''),
	
    PostBoxContent : [
        '<div class="post-box">',
            '<div class="post-title" id="postTitle">有什麼新鮮事告訴大家？</div>',
            '<div class="key-tips" id="xwb_word_cal">您還可以輸入<span>140</span>字</div>',
            '<div class="post-textarea" id="focusEl"><div class="inner"><textarea id="xwb_inputor"></textarea></div></div>',
            '<div class="add-area">',
                '<a class="ico-face" href="#" rel="e:ic">表情</a>',
                '<span class="pic_uploading hidden" id="xwb_upload_tip">正在上載..</span>',
                '<span class="pic-name hidden" id="xwb_photo_name"><a class="ico-close-btn" href="#" ></a></span>',
				'<div class="share-upload-pic" id="uploadBtn">',
					'<form class="upload-pic"  method="post"  enctype="multipart/form-data" id="xwb_post_form" action="" target="">',
						'<input type="file" name="pic" id="xwb_img_file" value="" />',
					'</form>',
					'<a class="ico-pic" href="#" id="xwb_btn_img">圖片</a>',
				'</div>',
                '<a class="ico-video" href="#" id="xwb_btn_vd" rel="e:vd">視頻</a>',
                '<a class="ico-music" href="#" id="xwb_btn_ms" rel="e:ms">音樂</a>',
                '<a class="ico-topic" href="#" id="xwb_btn_tp" rel="e:tp">話題</a>',
            '</div>',
            '<div class="share-btn" rel="e:sd"></div>',
            '<div class="post-success hidden" id="xwb_succ_mask"><span clsss="icon-success"></span>發布成功！</div>',
			'<div class="post-verify ico-verify hidden" id="xwb_veri_mask">信息已進入審核中</div>',
        '</div>'
    ].join(''),
    
    AnchorTipContent : '<div class="tips-c"><div class="tips-ok"></div><p id="xwb_title"></p></div>',
    AnchorDlgContent : '<div class="tips-c"><div class="ico-ask"></div><p id="xwb_title"></p></div>',
    
    SpanBoxContent : [
        '<div class="win-box-inner">',
            '<p class="desc">不良信息是指含有色情、暴力、廣告或其他難捱你正常微博生活的內容。</p>',
            '<p>你要舉報的是“{.nick}”發的微博：</p>',
            '<div class="report-box">',
                '<div>{.text}</div>',
                '<img src="{.img}" class="user" width="30px" height="30px" >',
            '</div>',
            '<p>你可以填寫更多舉報說明：（選填）</p>',
            '<p><textarea rows="" cols="" id="content"></textarea></p>',
            '<div class="foot-con">',
                '<p>請放心，您的隱私將會得到保護。<br>舉報電話：400 690 0000</p>',
                '<div class="btn-area">',
                    '<a href="#" class="btn-s1 btn-s1-light" rel="e:ok"><span>確認舉報</span></a>',
                    '<a href="#" class="btn-s1" rel="e:cancel"><span>取消</span></a>',
                '</div>',
            '</div>',
        '</div>'
    ].join(''),
    addFollowContent:['<input type="text" id="Content"/><a class="btn-s1" href="javascript:;" rel="e:submit" ><span>儲存</span></a>',
                    '<p>請添加想關注的話題</p>',
                    '<p class="warn hidden">添加關注話題失敗</p>'].join(''),
    evevtApplicants:['<div class="field"><label>聯絡方式：</label><input type="text" name="contact" id="contact"/></div>',
        '        <div class="field"><label>備註：</label><textarea name="note" id="note"></textarea></div>',
        '          <div class="field-area">',
        '            <a href="javascript:;" class="btn-s1 btn-s1-light fl" rel="e:sd"><span>發送</span></a>',
        '            <span class="tips-txt" id="tips">說明：長度不能超過100個字</span>',
        '            <span class="tips-wrong hidden" id="wrong">這裡是提示信息</span>',
        '        </div>'].join(''),
        
     colorBox : ['<ul>',
                 '<li class="cur" rel="e:getCls,c:#000,w:bg1"><span style="background:#000"></span></li>',
					'<li rel="e:getCls,c:#808080,w:bg2"><span style="background:#808080"></span></li>',
					'<li rel="e:getCls,c:#f66,w:bg3"><span style="background:#f66"></span></li>',
					'<li rel="e:getCls,c:#90c,w:bg4"><span style="background:#90c"></span></li>',
					'<li rel="e:getCls,c:#e7f2fb,w:bg5"><span style="background:#e7f2fb"></span></li>',
					'<li rel="e:getCls,c:#fff,w:bg6"><span style="background:#fff"></span></li>',
					'<li rel="e:getCls,c:#f00,w:bg7"><span style="background:#f00"></span></li>',
					'<li rel="e:getCls,c:#f60,w:bg8"><span style="background:#f60"></span></li>',
					'<li rel="e:getCls,c:#fc0,w:bg9"><span style="background:#fc0"></span></li>',
					'<li rel="e:getCls,c:#090,w:bg10"><span style="background:#090"></span></li>',
				'</ul>',
				'<div class="btn-area">',
					'<a href="#" class="btn-s1" rel="e:comf"><span>確定</span></a>',
				'</div>'].join(''),
				
				
	  colorpicker : ['<div class="colorpicker"><div class="colorpicker_color"><div><div></div></div></div><div class="colorpicker_hue"><div></div></div><div class="colorpicker_new_color"></div><div class="colorpicker_current_color"></div><div class="colorpicker_hex"><input type="text" maxlength="6" size="6" /></div><div class="colorpicker_rgb_r colorpicker_field"><input type="text" maxlength="3" size="3" /><span></span></div><div class="colorpicker_rgb_g colorpicker_field"><input type="text" maxlength="3" size="3" /><span></span></div><div class="colorpicker_rgb_b colorpicker_field"><input type="text" maxlength="3" size="3" /><span></span></div><div class="colorpicker_hsb_h colorpicker_field"><input type="text" maxlength="3" size="3" /><span></span></div><div class="colorpicker_hsb_s colorpicker_field"><input type="text" maxlength="3" size="3" /><span></span></div><div class="colorpicker_hsb_b colorpicker_field"><input type="text" maxlength="3" size="3" /><span></span></div><div class="colorpicker_submit"></div>',
	  '<div class="cp-oper"><span class="txt" id="cltitle">主鏈接色</span><span class="c-view" id="cRealColor"><span style="background:#0082cb;"></span></span><label>#<input value="" class="input-txt" id="colorshow" /></label></div>',
		  '<a href="#" class="btn-close" rel="e:closeCls"></a></div>'].join(''),
		  
	verify :	'<div class="verify-c tips ico-verify">該信息在審核中，請稍後再試！</div>'
});