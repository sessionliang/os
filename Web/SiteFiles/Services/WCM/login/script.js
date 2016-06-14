function stlUserLogin(pageUrl, pars, serviceUrl, isAdmin, isCheckCode, isCrossDomain, ajaxDivID)
{
	var oForm = $(ajaxDivID);
	
	try
	{
		if (checkFormValueById(ajaxDivID) == false){
			return;	
		}
		var serviceParameters = "userName=" + $F('UserName') + "&password=" + $F('Password');
		if ($('validateCode'))
		{
			serviceParameters = serviceParameters + "&validateCode=" + $F('validateCode');
		}else{
			serviceParameters = serviceParameters + "&validateCode=";
		}
		if ($('validateCodeHidden'))
		{
			serviceParameters = serviceParameters + "&validateCodeHidden=" + $F('validateCodeHidden');
		}else{
			serviceParameters = serviceParameters + "&validateCodeHidden=";
		}
		if ($('isRemember'))
		{
			serviceParameters = serviceParameters + "&isRemember=" + $('isRemember').checked;
		}else{
			serviceParameters = serviceParameters + "&isRemember=false";
		}
		serviceParameters = serviceParameters + "&isAdmin=" + isAdmin + "&isCheckCode=" + isCheckCode + "&isCrossDomain=" + isCrossDomain;
		var myAjax = new Ajax.Request(
		serviceUrl,
		{
			method: 'post', 
			parameters: serviceParameters,
			onComplete: function(x)
						{
							var documentElement = stlGetXmlDocumentElement(x.responseText);				
							var isLoggedIn = stlGetTextContent(documentElement.getElementsByTagName("string")[0]);
							var errorMessage = stlGetTextContent(documentElement.getElementsByTagName("string")[1]);
							
							if (isLoggedIn == 'true')
							{
								new Ajax.Updater({ success: ajaxDivID }, pageUrl, {method: 'post', parameters: pars, evalScripts:true });
							}
							else
							{
								if (oForm.select('#errorMessage'))
								{
									var e = oForm.select('#errorMessage').first();
									if (e.innerHTML.toLowerCase().indexOf('{user.errormessage}') != -1 || e.innerHTML.toLowerCase().indexOf('<!-- errorstar -->') != -1){
										e.innerHTML = e.innerHTML.replace(/{user.errormessage}/ig, '<!-- errorstar --><!-- errorend -->');
										e.innerHTML = e.innerHTML.replace(/<!-- errorstar -->[^!]*<!-- errorend -->/ig, '<!-- errorstar -->' + errorMessage + '<!-- errorend -->');
									}else{
										e.innerHTML = errorMessage;
									}
									e.show();
								}else{
									alert(errorMessage);
								}
							}
						}
		});
	}catch(e){alert(e)}
}

function stlUserLogout(pageUrl, parameters, ajaxDivID)
{
	var oForm = $(ajaxDivID);
	var pars = parameters + "&isLogout=True";
	try
	{
		new Ajax.Updater({ success: ajaxDivID }, pageUrl, {method: 'post', parameters: pars,
						 onComplete: function(x) 
						 {
							if (oForm.select('#errorMessage'))
							{
								oForm.select('#errorMessage').first().hide();
							}
						 },
						 evalScripts:true });
	}catch(e){alert(e)}
}