<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundBasePage" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
</head>
<frameset framespacing="0" border="false" cols="30%,70%" frameborder="0" scrolling="yes">
  <frame scrolling="auto" marginwidth="0" marginheight="0" src="background_formPage.aspx?MobanID=<%=GetQueryStringNoXss("MobanID")%>" >
    <frame name="field" scrolling="auto" marginwidth="0" marginheight="0" src="background_blank.html">
</frameset>
</html>
<!-- check for 3.6 html permissions -->