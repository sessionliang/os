var color = "" ;
var SelRGB = color;
var DrRGB = '';
var SelGRAY = '120';

var hexch = new Array('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F');

function ToHex(n) {	
	var h, l;

	n = Math.round(n);
	l = n % 16;
	h = Math.floor((n / 16)) % 16;
	return (hexch[h] + hexch[l]);
}

function DoColor(c, l){
	var r=1, g=2, b=3;

     
 		r = '0x' + c.substring(1, 3);
		g = '0x' + c.substring(3, 5);
		b = '0x' + c.substring(5, 7);

		if(l > 120)
		{
			l = l - 120;
			r = (r * (120 - l) + 255 * l) / 120;
			g = (g * (120 - l) + 255 * l) / 120;
			b = (b * (120 - l) + 255 * l) / 120;
		}
		else
		{
			r = (r * l) / 120;
			g = (g * l) / 120;
			b = (b * l) / 120;
		}
		
    var aaa='#' + ToHex(r) + ToHex(g) + ToHex(b);
    if(aaa=='#NaNNaNNaN')
    {
		return '#FFFFFF';
    }
    else
    {
        return '#' + ToHex(r) + ToHex(g) + ToHex(b);
    }
	
}



function EndColor(){

	var i;

	if(DrRGB != SelRGB){
		DrRGB = SelRGB;
		for(i = 0; i <= 30; i ++)
		{
		   $("GrayTable").rows[i].bgColor = DoColor(SelRGB, 240 - i * 8);
		}
	}

	$('SelColor').value = DoColor($('RGB').innerHTML, $('GRAY').innerHTML);
	$('ShowColor').bgColor = $('SelColor').value;
	//$('formatColor').value = $('SelColor').value;
}

function ColorTableMouseDown(e)
{
    $('highlight_colorselect').selectedIndex=0;
    $('highlight_colorselect').style.background='#FFFFFF';
   // $('s_bgcolor').style.background=e.title;
	SelRGB = e.title;
	EndColor();
}

function ColorTableMouseOver(e)
{
 	$('RGB').innerHTML = e.title;
	EndColor();
}

function ColorTableMouseOut(e)
{
	$('RGB').innerHTML = SelRGB;
	EndColor();
}



function GrayTableMouseDown(e)
{
	$('highlight_colorselect').selectedIndex=0;
	$('highlight_colorselect').style.background='#FFFFFF';
	//$('s_bgcolor').style.background=$('SelColor').value;
	SelGRAY = e.title;
	EndColor();
}

function GrayTableMouseOver(e)
{
	$('GRAY').innerHTML = e.title;
	EndColor();
}

function GrayTableMouseOut(e)
{
	$('GRAY').innerHTML = SelGRAY;
	EndColor();
}

function ColorPickerOK()
{
    var selectcolor=$('SelColor').value;
    $('formatColor').value=selectcolor;
    obj=$('formatColor');
    $('s_bgcolor').style.background=selectcolor;
	obj.focus();
	obj.select();
//	if(navigator.appName.indexOf("Explorer") > -1)
//    {
//		obj.createTextRange().execCommand("Copy");
//		window.status = '将模版内容复制到剪贴板';
//		setTimeout("window.status=''", 1800);
//	}
	HideColorPanel();
}

function HideColorPanel()
{  
    $('ColorPicker').style.display = 'none';
}



function ShowColorPanel()
{ 
	
   	var p = getposition($('formatColor'));
  	//alert(p['x']);
	$('ColorPicker').style.display = 'block';
	$('ColorPicker').style.left = (p['x'] - 140)+'px';
	$('ColorPicker').style.top = (p['y'] + 20)+'px';
}


function IsShowColorPanel()
{
	if($('ColorPicker').style.display == 'none')
	{
		var p = getposition($('formatColor'));
		$('ColorPicker').style.display = 'block';
		$('ColorPicker').style.left = (p['x'] - 140)+'px';
		$('ColorPicker').style.top = (p['y'] + 20)+'px';
	}
	else
	{
		$('ColorPicker').style.display = 'none';
	}
}
	
function getposition(obj) {
	var r = new Array();
	r['x'] = obj.offsetLeft;
	r['y'] = obj.offsetTop;
	while(obj = obj.offsetParent) {
		r['x'] += obj.offsetLeft;
		r['y'] += obj.offsetTop;
	}
	return r;
}


function selectoptioncolor(obj)
{
    //alert(obj.value);
      $('formatColor').value=obj.value;
      $('s_bgcolor').style.background=obj.value;
      //if(navigator.appName.indexOf("Explorer") <0)
      {
          $('highlight_colorselect').style.background=obj.value;
      }
      $('formatColor').focus();
}

function wc(r, g, b, n){
	r = ((r * 16 + r) * 3 * (15 - n) + 0x80 * n) / 15;
	g = ((g * 16 + g) * 3 * (15 - n) + 0x80 * n) / 15;
	b = ((b * 16 + b) * 3 * (15 - n) + 0x80 * n) / 15;
	document.write('<td BGCOLOR=#' + ToHex(r) + ToHex(g) + ToHex(b) + ' title="#' + ToHex(r) + ToHex(g) + ToHex(b) + '" height=8 width=8 onmouseover="ColorTableMouseOver(this)" onmousedown="ColorTableMouseDown(this)"  onmouseout="ColorTableMouseOut(this)" ></TD>');
}

function outputColor1(){
	var cnum = new Array(1, 0, 0, 1, 1, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0, 0);
	for(i = 0; i < 16; i ++){
		document.write('<TR>');
		for(j = 0; j < 30; j ++){
			n1 = j % 5;
			n2 = Math.floor(j / 5) * 3;
			n3 = n2 + 3;
			wc((cnum[n3] * n1 + cnum[n2] * (5 - n1)),
			(cnum[n3 + 1] * n1 + cnum[n2 + 1] * (5 - n1)),
			(cnum[n3 + 2] * n1 + cnum[n2 + 2] * (5 - n1)), i);
		}
		document.writeln('</TR>');
	}
}

function outputColor2(){
	for(i = 255; i >= 0; i -= 8.5)
		document.write('<tr BGCOLOR=#' + ToHex(i) + ToHex(i) + ToHex(i) + '><td TITLE=' + Math.floor(i * 16 / 17) + ' height=4 width=20 onmouseover="GrayTableMouseOver(this)" onmousedown="GrayTableMouseDown(this)"  onmouseout="GrayTableMouseOut(this)" ></td></tr>');	
}

function toggleTitleFormat(){
	Effect.toggle('titleFormatRow', 'Appear')
}


//TAGS

function ShowTagsPanel()
{
	var p = getposition($('Tags'));
	$('TagPicker').style.display = 'block';
	$('TagPicker').style.left = (p['x'] + 259)+'px';
	$('TagPicker').style.top = (p['y'] - 255)+'px';
}

function HideTagsPanel()
{
	$('TagPicker').style.display = 'none';
}

function select(t) {
	$(t).addClassName('selected');
}
function deselect(t) {
	$(t).removeClassName('selected');
}

// focus the caret to end of a form input (+ optionally select some text)
var range=0 //ie
function focusTo(obj, selectFrom) {
	if (typeof selectFrom == 'undefined') selectFrom = obj.value.length
	if(obj.createTextRange){ //ie + opera
		if (range == 0) range = obj.createTextRange()
		range.moveEnd("character",obj.value.length)
		range.moveStart("character",selectFrom)
		//obj.select()
		//range.select()
		setTimeout('range.select()', 10)
	} else if (obj.setSelectionRange){ //ff
		obj.select()
		obj.setSelectionRange(selectFrom,obj.value.length)
	} else { //safari :(
	 obj.blur()
}}		  

function swap(tag,force){
	var tagArray = $('Tags').value.trim().split(','), present=false, t, tl=tag.toLowerCase();
	if (tagArray[0].trim() == '') tagArray.splice(0,1);
	if (tagArray.length>1){
		if(tagArray[tagArray.length-1].trim() == '')	tagArray.pop();
	}
	for (t=0; t<tagArray.length; t++) {
		if (tagArray[t].toLowerCase() == tl) { 
			if(force == false) {
				tagArray.splice(t,1); deselect(tag); present=true; t-=1  
			} else {
				present=true;
			}
		}
	}
	
	if (!present) {
		tagArray.push(tag); select(tag) 
	}
	var content = tagArray.join(',')
	
	lastEdit = $('Tags').value = content;
	focusTo($('Tags'));
}