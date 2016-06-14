<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundBasePage" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
</head>
<frameset id="frame" framespacing="0" border="false" cols="180,*" frameborder="0" scrolling="yes">
	<frame name="tree" scrolling="auto" marginwidth="0" marginheight="0" src="background_fileTree.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>&RightPageURL=background_filemanagement.aspx" >
	<frame name="content" scrolling="auto" marginwidth="0" marginheight="0" src="background_blank.html">
</frameset>
<noframes>
<body>
<p>This page uses frames, but your browser doesn't support them.</p>
</body>
</noframes>
</html>
<!-- check for 3.6 html permissions -->
