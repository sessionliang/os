// JavaScript Document

checkUserName = function(value)
{
	$("username_msg").style.display = "none";
	$("usernamevalid_msg_succ").style.display = "none";
	$("usernamevalid_msg_error").style.display = "none";
	
	value = $('UserName').value;
	
	if ( !value || value.length == 0 )
	{
		$("username_msg").innerHTML = "用户名不能为空";	
		$("username_msg").style.display = "";
		return false;
	}
	
	if ( value.length < 4 )
	{
		$("username_msg").innerHTML = '用户名长度不能小于4个字符';
		$("username_msg").style.display = "";
		return false;
	}

	return true;
}

checkUserNameValid = function(ucUrl)
{
	$("usernamevalid_msg_succ").style.display = "none";
	$("usernamevalid_msg_error").style.display = "none";
	
	var value = $("UserName").value;
	
	if (!checkUserName(value)) return;
	
	var url = ucUrl + "/js/ajax.aspx?type=checkusername&username=" + escape(value);

	new Ajax.Request(url, {
	  method: 'get',
	  onSuccess: function(x) {
		var result = stlGetNodeText(x.responseXML.documentElement);
		if (result == "false")
		{
			$("usernamevalid_msg_succ").style.display = "none";
			$("usernamevalid_msg_error").style.display = "";
		}else{
			$("usernamevalid_msg_succ").style.display = "";
			$("usernamevalid_msg_error").style.display = "none";
		}
	  }
	});
}

checkDisplayName = function()
{
	$("displayname_msg").style.display = "none";
	
	value = $('DisplayName').value;
	
	if ( !value || value.length == 0 )
	{
		$("displayname_msg").innerHTML = "昵称不能为空";	
		$("displayname_msg").style.display = "";
		return false;
	}
	
	if ( value.length < 2 )
	{
		$("displayname_msg").innerHTML = '昵称长度不能小于2个字符';
		$("displayname_msg").style.display = "";
		return false;
	}

	return true;
}

checkEmail = function()
{
	$("email_msg").style.display = "none";
	
	value = $('Email').value;
	
	if ( !value || value.length == 0 )
	{
		$('email_msg').innerHTML = '邮箱不能为空';
		$('email_msg').style.display = "";
		return false;
	}
	
	if ( value.search(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/) == -1 )
	{
		$('email_msg').innerHTML = '请正确填写邮箱';
		$('email_msg').style.display = "";
		return false;	
	}
	
	return true;
}

checkPassword = function()
{
	$('password_msg').style.display = "none";

	var strPassword = $('Password').value;
	
	if ( !strPassword || strPassword.length == 0 )
	{
		$("password_msg").innerHTML = "密码不能为空";
		$("password_msg").style.display = "";
		return false;
	}
	
	if ( strPassword.length < 6 || strPassword.length>20 )
	{
		$('password_msg').innerHTML = '密码长度小于6个或者大于20个字符';
		$('password_msg').style.display = "";
		return false;
	}
	
	setPasswordLevel(strPassword, $('passwordLevel'))
	return true;
}

checkConfirmPassword = function()
{
	$('confirmPassword_msg').style.display = "none";
	
	var strPassword = $('Password').value;
	var confirmPassword = $('ComfirmPassword').value;
	
	if ( !confirmPassword || confirmPassword.length == 0 )
	{
		$("confirmPassword_msg").innerHTML = "确认密码不能为空";
		$("confirmPassword_msg").style.display = "";
		return false;
	}
	
	else if ( strPassword != confirmPassword )
	{
		$('confirmPassword_msg').innerHTML = "确认密码与密码不一致";
		$('confirmPassword_msg').style.display = "";
		return false;
	}
	
	return true;
}

checkVerify = function()
{
	$("verifycode_msg").style.display = "none";
	
	value = $('ValidateCode').value;
	
	if ( !value || value.length == 0 )
	{
		$("verifycode_msg").innerHTML = "请填写验证码";	
		$("verifycode_msg").style.display = "";
		return false;
	}

	return true;		
}

checkAgree = function()
{
	$("agree_msg").style.display = "none";
	
	if ( $("agree").checked == true )
	{
		$("agree_msg").style.display = "none";
		return true;
	}

	$("agree_msg").innerHTML = "请同意使用条款";
	$("agree_msg").style.display = "";
	return false;				
}

//检测所有
checkAll = function()
{
	var isOk = true;
	if (!checkUserName())
	{
		isOk = false;
	}
	if (!checkDisplayName())
	{
		isOk = false;
	}
	if (!checkEmail())
	{
		isOk = false;
	}
	if (!checkPassword())
	{
		isOk = false;
	}
	if (!checkConfirmPassword())
	{
		isOk = false;
	}	
	if (!checkVerify())
	{
		isOk = false;
	}
	return isOk;
}


/******************************************************************************************
 * 检查密码强度
 ******************************************************************************************/
checkPasswordLevel = function(strPassword)
{
	//check length
	var result = 0;
	if ( strPassword.length == 0)
		result += 0;
	else if ( strPassword.length<8 && strPassword.length >0 )
		result += 5;
	else if (strPassword.length>10)
		result += 25;
	else
		result += 10;
	//alert("检查长度:"+strPassword.length+"-"+result);
	
	//check letter
	var bHave = false;
	var bAll = false;
	var capital = strPassword.match(/[A-Z]{1}/);//找大写字母
	var small = strPassword.match(/[a-z]{1}/);//找小写字母
	if ( capital == null && small == null )
	{
		result += 0; //没有字母
		bHave = false;
	}
	else if ( capital != null && small != null )
	{
		result += 20;
		bAll = true;
	}
	else
	{	
		result += 10;
		bAll = true;
	}
	//alert("检查字母："+result);
	
	//检查数字
	var bDigi = false;
	var digitalLen = 0;
	for ( var i=0; i<strPassword.length; i++)
	{
	
		if ( strPassword.charAt(i) <= '9' && strPassword.charAt(i) >= '0' )
		{
			bDigi = true;
			digitalLen += 1;
			//alert(strPassword[i]);
		}
		
	}
	if ( digitalLen==0 )//没有数字
	{
		result += 0;
		bDigi = false;
	}
	else if (digitalLen>2)//2个数字以上
	{
		result += 20 ;
		bDigi = true;
	}
	else
	{
		result += 10;
		bDigi = true;
	}
	//alert("数字个数：" + digitalLen);
	//alert("检查数字："+result);
	
	//检查非单词字符
	var bOther = false;
	var otherLen = 0;
	for (var i=0; i<strPassword.length; i++)
	{
		if ( (strPassword.charAt(i)>='0' && strPassword.charAt(i)<='9') ||  
			(strPassword.charAt(i)>='A' && strPassword.charAt(i)<='Z') ||
			(strPassword.charAt(i)>='a' && strPassword.charAt(i)<='z'))
			continue;
		otherLen += 1;
		bOther = true;
	}
	if ( otherLen == 0 )//没有非单词字符
	{
		result += 0;
		bOther = false;
	}
	else if ( otherLen >1)//1个以上非单词字符
	{
		result +=25 ;
		bOther = true;
	}
	else
	{
		result +=10;
		bOther = true;
	}
	//alert("检查非单词："+result);
	
	//检查额外奖励
	if ( bAll && bDigi && bOther)
		result += 5;
	else if (bHave && bDigi && bOther)
		result += 3;
	else if (bHave && bDigi )
		result += 2;
	//alert("检查额外奖励："+result);

	var level = "";
	//根据分数来算密码强度的等级
	if ( result >=70 )
		level = "rank r7";
	else if ( result>=60)
		level = "rank r6";
	else if ( result>=50)
		level = "rank r5";
	else if ( result>=40)
		level = "rank r4";
	else if ( result>=30)
		level = "rank r3";
	else if ( result>20)
		level = "rank r2";
	else if ( result>10)
		level = "rank r1";
	else
		level = "rank r0";

//		alert("return:"+level);
	return level.toString();
}	


/******************************************************************************************
 * 设置密码强度样式
 ******************************************************************************************/
setPasswordLevel = function(strPassword, levelObj)
{
	var level = "rank r0";
	level = checkPasswordLevel(strPassword);
	levelObj.className = level;
}


function stlUserRegister(pageUrl, ajaxDivID, isSessionExists, isTypeChanged, current)
{
	try{
		var myhash = getStlInputParameters(ajaxDivID);
		myhash.set('ajaxDivID', ajaxDivID);
		myhash.set('isSessionExists', isSessionExists);
		myhash.set('isTypeChanged', isTypeChanged);
		myhash.set('current', current);
		var option = {
			method:'post',
			parameters: myhash,
			evalScripts:true
		};
		new Ajax.Updater ({success:ajaxDivID}, pageUrl, option);
	}catch(e){alert(e);}
}