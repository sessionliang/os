function submit_query()
{
	if (checkFormValueById('frmQuery'))
	{
		$('#frmQuery').showLoading();
		var frmQuery = document.getElementById('frmQuery');
		frmQuery.action = govPublicQueryActionUrl;
		frmQuery.target = 'iframeQuery';
		frmQuery.submit();
	}
}