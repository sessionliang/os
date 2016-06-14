String.prototype.trim=function(){return this.replace(/^\s+|\s+$/g,"");};
String.prototype.nl2br=function(){return this.split("\n").join("<br />\n");};

//Cookie Start
function _setCookie(name, value, hoursToExpire)   //写
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
//Cookie End

//DHTML Start
var writeDebug=function(){};

var _get = function(id)
{
	return document.getElementById(id);
};

var _goto = function (url) {
 
    window.location.href = url;
};

function _refresh() { window.location.reload( false );}

var _decorate=function()
{
	var el=arguments[0];
	if(!el)
	{
		return;
	}
	arguments[0]=el;
	var _154="decorate_"+el.className;
	if(window[_154])
	{
		return window[_154](arguments);
	}
	writeDebug(_154+" is not a function");
};

function _isNull(obj){
	if (typeof(obj) == "undefined")
	  return true;
	  
	if (obj == undefined)
	  return true;
	  
	if (obj == null)
	  return true;
	 
	return false;
}

function _random()
{
    var rnd="";
    for(var i=0;i<10;i++)
        rnd+=Math.floor(Math.random()*10);
    return rnd;
}

function _help(sHelpID)
{
	var oHelp = _get(sHelpID);
	if (oHelp != null)
	{
		if (oHelp.style.display == 'none')
			oHelp.style.display = '';
		else
			oHelp.style.display = 'none';

		return true;	//cancel postback
	}
	return false;	//failed so do postback
}

function _checkFormAll(isChecked)
{
	if (!_isNull(document.forms)){
		var form = document.forms[0];
		if (!_isNull(form)){
			for(var i=0; i<form.elements.length; i++)
			{
				if (form.elements[i].type=="checkbox")
				{
					form.elements[i].checked = isChecked;
				}
			}
		}
	}
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
   
	var retval = new String("");
	if (!_isNull(checklist)){
		if (_isNull(checklist.length)){
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

function _showImage(obj, imageID){
	if(obj.value=="") return false;
	if(obj.value.search(/\.jpg|\.jpeg|\.bmp|\.gif|\.emf|\.wmf|\.xbm|\.png$/i) == -1) {
		alert("图片文件格式不正确");
		return false;
	}
	var prev=document.getElementById(imageID);
	prev.src=obj.value;
}

function _collapseElementByID(elementID){
	var oObject = document.all.item(elementID);
	if (!_isNull(oObject)){
	   if (oObject.length != null){
		  for (i = 0; i < oObject.length; i++){
			 if(oObject(i).style.display == "") {
			 	oObject(i).style.display = "none";
			 }else{
			 	oObject(i).style.display = "";
			 }
		  }
	   }
	   else{
			 if(oObject.style.display == "") {
			 	oObject.style.display = "none";
			 }else{
			 	oObject.style.display = "";
			 }
	   }
	}
}

function _toggleVisible(targetID, image, imageUrl, imageCollapsedUrl)
{
	target = _get(targetID);
	try {
			Effect.toggle(targetID,'slide',{duration: 0.5});
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

//DHTML End

//Ajax Start

function highlight(div)
{
	if(div.hideTimer)
	{
		clearTimeout(div.hideTimer);
	}
	div.style.color="gray";
	div.style.backgroundColor="#ffffd3";
}

function unhighlight(div)
{
	if(div.hideTimer)
	{
		clearTimeout(div.hideTimer);
	}
	div.hideTimer=setTimeout("var el = document.getElementById(\""+div.id+"\"); if (el) unhighlight2(el)",500);
}

function unhighlight2(div)
{
	if(div.hideTimer)
	{
		clearTimeout(div.hideTimer);
	}
	div.style.color="";
	div.style.backgroundColor="";
}

/* Ajax */

function Ajax_RefreshWhenResponseIsTrue(originalRequest)
{
	var xmlDoc = originalRequest.responseXML.documentElement;
	var theText = xmlDoc.text;
	if(theText == 'true')
	{
		_refresh();
	}
	else
	{
		alert('Error');
	}
}

function Ajax_GetXMLHttpRequest() {
	if (window.XMLHttpRequest) {
		return new XMLHttpRequest();
	} else {
		if (window.Ajax_XMLHttpRequestProgID) {
			return new ActiveXObject(window.Ajax_XMLHttpRequestProgID);
		} else {
			var progIDs = ["Msxml2.XMLHTTP.5.0", "Msxml2.XMLHTTP.4.0", "MSXML2.XMLHTTP.3.0", "MSXML2.XMLHTTP", "Microsoft.XMLHTTP"];
			for (var i = 0; i < progIDs.length; ++i) {
				var progID = progIDs[i];
				try {
					var x = new ActiveXObject(progID);
					window.Ajax_XMLHttpRequestProgID = progID;
					return x;
				} catch (e) {
				}
			}
		}
	}
	return null;
}

//return Ajax_CallBack('ASP.PhotoContent_aspx', null, 'AddTags', [contentID,tagsString], clientCallBack, false, false, false, false,'/ls/cc/photos/26.aspx?Ajax_CallBack=true');
function Ajax_CallBack(type, id, method, args, clientCallBack, debugRequestText, debugResponseText, debugErrors, includeControlValuesWithCallBack, url) {
	var x = Ajax_GetXMLHttpRequest();
	var result = null;
	if (!x) {
		result = { "value":null, "error": "NOXMLHTTP"};
		if (debugErrors) {
			alert("error: " + result.error);
		}
		if (clientCallBack) {
			clientCallBack(result);
		}
		return result;
	}

	x.open("POST", url, clientCallBack ? true : false);
	x.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
	if (clientCallBack) {
		x.onreadystatechange = function() {
			var result = null;
		
			if (x.readyState != 4) {
				return;
			}
			
			if (debugResponseText) {
				alert(x.responseText);
			}
			
			try
			{
				var result = eval("(" + x.responseText + ")");
				if (debugErrors && result.error) {
					alert("error: " + result.error);
				}
			}
			catch (err)
			{
				if (window.confirm('The following error occured while processing an AJAX request: ' + err.message + '\n\nWould you like to see the response?'))
				{
					var w = window.open();
					w.document.open('text/plain');
					w.document.write(x.responseText);
					w.document.close();
				}
				
				result = new Object();
				result.error = 'An AJAX error occured.  The response is invalid.';
			}
			
			clientCallBack(result);			
		}
	}
	var encodedData = "Ajax_CallBackType=" + type;
	if (id) {
		encodedData += "&Ajax_CallBackID=" + id.split("$").join(":");
	}
	encodedData += "&Ajax_CallBackMethod=" + method;
	if (args) {
		for (var index = 0;index < args.length; index++){
			encodedData += "&Ajax_CallBackArgument" + index + "=" + encodeURIComponent(args[index]);
		}
        //args.each(function(value, index){
        //    encodedData += "&Ajax_CallBackArgument" + index + "=" + encodeURIComponent(value);
        //});
	}
	if (includeControlValuesWithCallBack && document.forms.length > 0) {
		var form = document.forms[0];
		for (var i = 0; i < form.length; ++i) {
			var element = form.elements[i];
			if (element.name) {
				var elementValue = null;
				if (element.nodeName == "INPUT") {
					var inputType = element.getAttribute("TYPE").toUpperCase();
					if (inputType == "TEXT" || inputType == "PASSWORD" || inputType == "HIDDEN") {
						elementValue = element.value;
					} else if (inputType == "CHECKBOX" || inputType == "RADIO") {
						if (element.checked) {
							elementValue = element.value;
						}
					}
				} else if (element.nodeName == "SELECT") {
					elementValue = element.value;
				} else if (element.nodeName == "TEXTAREA") {
					elementValue = element.value;
				}
				if (elementValue) {
					encodedData += "&" + element.name + "=" + encodeURIComponent(elementValue);
				}
			}
		}
	}
	if (debugRequestText) {
		alert(encodedData);
	}
	x.send(encodedData);
	if (!clientCallBack) {
		if (debugResponseText) {
			alert(x.responseText);
		}
		result = eval("(" + x.responseText + ")");
		if (debugErrors && result.error) {
			alert("error: " + result.error);
		}
	}
	delete x;
	return result;
}

//Ajax End

//IFrame Start

function getDocHeight(doc) {
  var docHt = 0, sh, oh;
  if (doc.height) docHt = doc.height;
  else if (doc.body) {
    if (doc.body.scrollHeight) docHt = sh = doc.body.scrollHeight;
    if (doc.body.offsetHeight) docHt = oh = doc.body.offsetHeight;
    if (sh && oh) docHt = Math.max(sh, oh);
  }
  return docHt;
}

function setIframeHeight(iframeName) {
  var iframeWin = window.frames[iframeName];
  var iframeEl = document.getElementById? document.getElementById(iframeName): document.all? document.all[iframeName]: null;
  var loadingEl = document.getElementById? document.getElementById('loading_' + iframeName): document.all? document.all['loading_' + iframeName]: null;
  if ( iframeEl && iframeWin ) {
    iframeEl.style.height = "auto"; // helps resize (for some) if new doc shorter than previous  
    var docHt = getDocHeight(iframeWin.document);
    // need to add to height to be sure it will all show
    if (docHt) iframeEl.style.height = docHt + 1 + "px";
	if (loadingEl) loadingEl.style.display = 'none';
  }
}

function loadIframe(iframeName, url) {
  if ( window.frames[iframeName] ) {
    window.frames[iframeName].location = url;   
    return false;
  }
  else return true;
}

//IFrame End

//StringBuilder Start

function StringBuffer()
{
	this.clear();
	if(arguments.length>0)
	{
		arguments.join=this.buffer.join;
		this.buffer[this.buffer.length]=arguments.join("");
	}
}
StringBuffer.prototype={
	toString:function()
	{
		return this.buffer.join("");
	},join:function(_3)
	{
		if(_3==null)
		{
			_3="";
		}
		return this.buffer.join(_3);
	},append:function(){
		arguments.join=this.buffer.join;
		this.buffer[this.buffer.length]=arguments.join("");
		return this;
	},set:function(_4){
		this.buffer=[_4];
	},clear:function(){
		this.buffer=[];
	}
};
StringBuffer.concat=function()
{
	arguments.join=Array.prototype.join;
	return arguments.join("");
};
StringBuffer.append=StringBuffer.concat;

//StringBuilder End