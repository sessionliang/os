//siteserver sync

function isNull(obj){
	if (typeof(obj) == "undefined")
	  return true;
	  
	if (obj == undefined)
	  return true;
	  
	if (obj == null)
	  return true;
	 
	return false;
}

function chkSelect(e){
 var e = (e || event);
 var el = this;
 if (el.className == 'info') return;
 if (el.getElementsByTagName('input') && el.getElementsByTagName('input').length > 0){
	 el.className = (el.className == 'success' ? '' : 'success');
	 el.getElementsByTagName('input')[0].checked = (el.className == 'success');
 }
}

function loopRows(oTable, callBack){
 if (!oTable) return;
 callBack = callBack || function(){};
 var trs = oTable.rows;
 var i = 0 , l = trs.length;
 var flag = i < l;

 while(flag ? i < l : i > l ){
  var cur = trs[i];
  try{
   callBack(cur, i);
  }catch(e){ if(e == 'break'){ break; }}
  flag ? i++ : i--;
 }
}

function selectRows(layer, bcheck)
{
	for(var i=0; i<layer.childNodes.length; i++)
	{
		if (layer.childNodes[i].childNodes.length>0)
		{
			selectRows(layer.childNodes[i],bcheck);
		}
		else
		{
			if (layer.childNodes[i].type=="checkbox")
			{
				layer.childNodes[i].checked = bcheck;
				var cb = $(layer.childNodes[i]);
				var tr = cb.closest('tr');
				if (tr.attr('class') != 'info'){
					cb.attr('checked') ? tr.addClass('success') : tr.removeClass('success');
				}
			}
		}
	}
}

function showImage(obj, imageID, rootUrl, publishmentSystemUrl){
    if(obj.value==""){
		document.getElementById(imageID).setAttribute('href', '../../sitefiles/bairong/Icons/empty.gif');
		return false;
	}
	if(obj.value.search(/\.bmp|\.jpg|\.jpeg|\.gif|\.png$/i) != -1) {
		var imageUrl = obj.value;
		if (imageUrl){
			if (imageUrl.charAt(0) == '~'){
				imageUrl = imageUrl.replace('~', rootUrl);
			}else if (imageUrl.charAt(0) == '@'){
				imageUrl = imageUrl.replace('@', publishmentSystemUrl);
			}
		}
		if(imageUrl && imageUrl.substr(0,2)=='//'){
			imageUrl = imageUrl.replace('//', '/');
		}
		document.getElementById(imageID).setAttribute('href', imageUrl);
	}
}

function DeleteAttachment(attributeName)
{
	document.getElementById("add_" + attributeName).style.display='';
	document.getElementById("del_" + attributeName).style.display='none';
	document.getElementById(attributeName).value = '';
}

function SelectAttachment(attributeName, attachmentUrl, fileViewUrl)
{
	document.getElementById("add_" + attributeName).style.display = 'none';
	document.getElementById("del_" + attributeName).style.display = '';
	document.getElementById("del_" + attributeName).innerHTML = "<input type=button class=btn onclick=\"" + fileViewUrl + "\" style=\"margin-top:-10px;\" value=\"查看\" /> <input type=button class=btn onclick=\"DeleteAttachment('" + attributeName + "');\" style=\"margin-top:-10px;\" value=\"移除\" />";
	document.getElementById(attributeName).value = attachmentUrl;
}

function _toggleVisible(targetID, image, imageUrl, imageCollapsedUrl)
{
	target = _get(targetID);
	try {
			$('#' + targetID).toggle();
		} catch (e) {
			if (target.style.display == "none") {
				target.style.display = "";
			} else {
				target.style.display = "none";
			}
		}
	
	if (!_isNull(imageCollapsedUrl) && imageCollapsedUrl != "") {
		if (target.style.display == "none") {
			image.src = imageCollapsedUrl;
		} else {
			image.src = imageUrl;
		}
	}
	return false;
}

function _toggleTab(no, totalNum) {
	$('#tab' + no).addClass("active");
	$('#column' + no).show();
	for (var i = 1; i <= totalNum; i++) {
		if (i != no) {
			$('#tab' + i).removeClass("active");
			$('#column' + i).hide();
		}
	}
}
function _checkCol(column, className, bcheck) {
	var elements = $('#' + column + ' .' + className);
	for (var i = 0; i < elements.length; i++) {
		_checkAll(elements[i], bcheck);
	}
}

function _checkFormAll(isChecked)
{
	$(":checkbox").each(function() {
		$(this).attr("checked", isChecked);
	});
}

function _checkAll(layer, bcheck)
{
	for(var i=0; i<layer.childNodes.length; i++)
	{
		if (layer.childNodes[i].childNodes.length>0)
		{
			_checkAll(layer.childNodes[i],bcheck);
		}
		else
		{
			if (layer.childNodes[i].type=="checkbox")
			{
				layer.childNodes[i].checked = bcheck;
			}
		}
	}
}

function _getCheckBoxCollectionValue(checklist) {
   
	var retval = '';
	if (checklist){
		if (!checklist.length){
			if(checklist.checked){
			    retval = encodeURI(checklist.value) + '';
			}
		}else{
			var hasValue = false;
			for (var i = 0; i < checklist.length; i++){
				if(checklist[i].checked){
					hasValue = true;
					retval += encodeURI(checklist[i].value) + ',';
				}
			}
			if (hasValue){
				retval = retval.substring(0, retval.length - 1);
			}
		}
	}
	return retval;
}

function _alertCheckBoxCollection(checklist, alertString){
	var collectionValue = _getCheckBoxCollectionValue(checklist);
	if (collectionValue.length == 0){
		alert(alertString);
		return true;
	}
	return false;
}

var _get = function(id)
{
	return document.getElementById(id);
};

var _goto = function (url) {
 
    window.location.href = url;
};

function _refresh() { window.location.reload( false );}

function _setCookie(name, value, hoursToExpire)   //Ð´
{
	var expire = '';
    if (hoursToExpire != undefined) {
      var d = new Date();
      d.setTime(d.getTime() + (3600000 * parseFloat(hoursToExpire)));
      expire = '; expires=' + d.toGMTString();
    }

	window.document.cookie = name + "=" + escape(value) + expire +";path=/" ;
} 


function _getCookie(name) 
{ 
   var cookieString = "" + window.document.cookie;
   var search = name + "="; 
   if (cookieString.length > 0) 
   {
		offset = cookieString.indexOf(search); 
		if (offset != -1) 
		{
			offset += search.length;
			end = cookieString.indexOf(";", offset);
			if (end == -1) end = cookieString.length; 
			return unescape(cookieString.substring(offset, end)); 
		} 
	}
	return null;
}

//usercenter

function switchFolder( id ) {	
	var id = id + "_td";
	var folders = $$('#folder li');
	for ( var i = folders.length - 1 ; i >= 0 ; i-- ) {
		var folder = folders[ i ];
		if ( !folder.id ) {
			continue;
		}
		else if ( folder.id == id ) {
			folder.removeClassName("fs");
			folder.addClassName("fn");
		}else{
			folder.removeClassName("fn");
			folder.addClassName("fs");
		}
	}
}

function clearFolder( ) {	
	var folders = $$('#folder li');
	for ( var i = folders.length - 1 ; i >= 0 ; i-- ) {
		var folder = folders[ i ];
		if (!folder.hasClassName('sepline')){
			folder.removeClassName("fn");
			folder.addClassName("fs");
		}
	}
}

function selectAll(elementID, flag) {
   
	var cbs = document.getElementsByName(elementID);
	if(cbs.length){
		for(var i = cbs.length - 1;i >= 0; i--) {
			cbs[i].checked = flag;
		}
	}else{
		cbs.checked = flag;
	}
}

function checkAll(elementID) {
	var cbs = document.getElementsByName(elementID);
	if(cbs.length){
		for ( var i = cbs.length - 1; i >= 0; i-- ) {
			setListCheck(cbs[i]);
		}
	}else{
		setListCheck(cbs);
	}
}

function setListCheck( input, checked ) {
	if ( input.type != "checkbox" )
	return;
	
	if ( checked == null ) {
		checked = input.checked;
	}
	else {
		input.checked = checked;
	}
	
	var obj = input.parentNode.parentNode;
	
	if (checked){
		Element.addClassName(obj, "B");
	}else{
		Element.removeClassName(obj, "B");
	}
}

function welcome(){
	var hour = (new Date()).getHours();
	if (hour < 4) {
		hello = "夜深了，";
	}
	else if (hour < 7) {
		hello = "早安，";
	}
	else if (hour < 9) {
		hello = "早上好，"; 
	}
	else if (hour < 12) {
		hello = "上午好，";
	}
	else if (hour < 14) {
		hello = "中午好，";
	}
	else if (hour < 17) {
		hello = "下午好，";
	}
	else if (hour < 19) {
		hello = "您好，";
	}
	else if (hour < 22) {
		hello = "晚上好，";
	}
	else {
		hello = "夜深了，";
	}
	document.write(hello);	
}

function __Help_OnClick(sHelpID)
{
	var oHelp = document.getElementById(sHelpID);
	if (oHelp != null)
	{
		if (oHelp.style.display == 'none')
			oHelp.style.display = '';
		else
			oHelp.style.display = 'none';

		return true;
	}
	return false;
}