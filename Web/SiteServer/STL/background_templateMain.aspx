<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundBasePage" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
</head>
<frameset framespacing="0" border="false" cols="180,*" frameborder="0" scrolling="yes">
	<frame scrolling="auto" marginwidth="0" marginheight="0" src="background_templateLeft.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>" >
    <frame id="management" name="management" scrolling="auto" marginwidth="0" marginheight="0" src="background_template.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>">
</frameset><noframes></noframes>
</html>
<!-- check for 3.6 html permissions -->