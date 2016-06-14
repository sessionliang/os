<%@ Page language="c#" trace="false" enableViewState="false" Inherits="SiteServer.WCM.Services.CookieQuery" %>
<script>
var expireDate=new Date(new Date().getTime()+12*3600000);
top.document.cookie = "<%=GetCookie()%>; path=/; expires=" + expireDate.toGMTString();
</script>