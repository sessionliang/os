<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundBasePage" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
</head>
<frameset framespacing="0" border="false" cols="260,*" frameborder="0" scrolling="yes">
	<frame scrolling="auto" marginwidth="0" marginheight="0" src="background_tagStyleLeft.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>&tableStyle=<%=Request.QueryString["tableStyle"]%>&type=<%=Request.QueryString["type"]%>" >
    <frame name="management" scrolling="auto" marginwidth="0" marginheight="0" src="/siteserver/cms/background_blank.html">
</frameset><noframes></noframes>
</html>
<!-- check for 3.6 html permissions -->