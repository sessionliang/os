<%@ Page Language="C#" Inherits="SiteServer.WCM.BackgroundPages.BackgroundBasePage" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
</head>
<frameset framespacing="0" border="false" cols="40%,60%" frameborder="0" scrolling="yes">
	<frame scrolling="auto" marginwidth="0" marginheight="0" src="background_govPublicCategoryClass.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>" >
    <frame name="category" scrolling="auto" marginwidth="0" marginheight="0" src="background_blank.html">
</frameset>
</html>
<!-- check for 3.6 html permissions -->