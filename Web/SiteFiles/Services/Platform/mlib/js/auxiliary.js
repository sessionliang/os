function ShowImg(obj, imageID, rootUrl){
	if(obj.value==""){
		_get(imageID).src = '../sitefiles/bairong/Icons/empty.gif';
		return false;
	}
	if(obj.value.search(/\.swf$/i) != -1) {
		_get(imageID).src = '../sitefiles/bairong/Icons/flash.jpg';
		return false;
	}
	if(obj.value.search(/\.flv|\.avi|\.mpg|\.mpeg|\.smi|\.mp3|\.mid|\.midi|\.rm|\.wmv|\.wma|\.asf|\.mov$/i) != -1) {
		_get(imageID).src = '../sitefiles/bairong/Icons/player.gif';
		return false;
	}
	if(obj.value.search(/\.bmp|\.jpg|\.jpeg|\.gif|\.png$/i) != -1) {
		var imageUrl = obj.value;
		if (imageUrl){
			if (imageUrl.charAt(0) == '~'){
				imageUrl = imageUrl.replace('~', rootUrl);
			}
		}
		if(imageUrl && imageUrl.substr(0,2)=='//'){
			imageUrl = imageUrl.replace('//', '/');
		}
		_get(imageID).src=imageUrl;
	}
}

function DeleteAttachment(attributeName)
{
	_get("add_" + attributeName).style.display='';
	_get("del_" + attributeName).style.display='none';
	_get(attributeName).value = '';
}

function SelectAttachment(attributeName, attachmentUrl, fileViewUrl)
{
	_get("add_" + attributeName).style.display = 'none';
	_get("del_" + attributeName).style.display = '';
	_get("del_" + attributeName).innerHTML = "<a href='javascript:;' onclick=\"" + fileViewUrl + "\">查看</a> <a href='javascript:;' onclick=\"DeleteAttachment('" + attributeName + "');\">移除</a>";
	_get(attributeName).value = attachmentUrl;
}