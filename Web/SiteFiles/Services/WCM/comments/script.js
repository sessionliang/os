function commentReference(userName, dateTime, comment)
{
	var obj = document.getElementById("Content");
	if (obj){
		obj.value = "[quote] 原帖由 " + userName;
		obj.value += " 于 " + dateTime + " 发表\r\n";
		obj.value += comment + "[/quote]\r\n";
		obj.focus();
	}
}

function commentDiggCheck(publishmentSystemID, commentID)
{
	var allcookies = document.cookie;
	var cookieName = "stlCommentDigg_" + publishmentSystemID + "_" + commentID;
	var pos = allcookies.indexOf(cookieName + "=");
	if (pos != -1) {
		alert(decodeURIComponent("对不起，不能重复操作!"));
		return false;
	}else{
		var str = cookieName + "=true";
		var date = new Date();
		var ms = 24*3600*1000;
		date.setTime(date.getTime() + ms);
		str += "; expires=" + date.toGMTString();
		document.cookie = str;
		return true;
	}
}

function commentDigg(ajaxUrl, publishmentSystemID, commentID, isGood)
{
    if (commentDiggCheck(publishmentSystemID, commentID)){
        try
        {
            var e = document.getElementById('commentsDigg_' + commentID + '_' + isGood);
            e.innerHTML = (parseInt(e.innerHTML) + 1) + '';

            var url = ajaxUrl + '&publishmentSystemID=' + publishmentSystemID + '&commentID=' + commentID + '&isGood=' + isGood;

            if(window.XMLHttpRequest)
            {
	            req=new XMLHttpRequest();
            }
            else if(window.ActiveXObject)
            {
	            req=new ActiveXObject("Microsoft.XMLHttp");
            }

            if(req)
            {
	            req.open("GET",url,true);
	            req.send(null);
            }

        }catch(e){}
    }
}