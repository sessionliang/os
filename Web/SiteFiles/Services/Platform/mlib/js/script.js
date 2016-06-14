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