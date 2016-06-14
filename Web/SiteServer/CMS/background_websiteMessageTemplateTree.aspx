<%@ Page Language="C#" Trace="false" EnableViewState="false" Inherits="SiteServer.CMS.BackgroundPages.BackgroundBasePage" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>


<html>
<head>
    <meta charset="utf-8">
    <!--#include file="../inc/header.aspx"-->
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->
    <form runat="server">
        <table class="table noborder table-condensed table-hover">
            <tr class="info thead">
                <td style="text-align: center" onclick="location.reload();">
                    <lan>留言模版列表</lan>
                </td>
            </tr>

            <tr treeitemlevel="2">
                <td align="left" nowrap>
                    <img align="absmiddle" src="/SiteFiles/bairong/icons/tree/empty.gif" /><img align="absmiddle" border="0" src="/SiteFiles/bairong/icons/tree/folder.gif" />&nbsp;<a href='/siteserver/stl/background_websiteMessageTemplateSubmit.aspx?PublishmentSystemID=<%=base.Request.QueryString["PublishmentSystemID"]%>&WebsiteMessageName=Default' islink='true' target='content'>提交留言模板</a>
                </td>
            </tr>

            <tr treeitemlevel="2">
                <td align="left" nowrap>
                    <img align="absmiddle" src="/SiteFiles/bairong/icons/tree/empty.gif" /><img align="absmiddle" border="0" src="/SiteFiles/bairong/icons/tree/folder.gif" />&nbsp;<a href='/siteserver/stl/background_websiteMessageTemplateList.aspx?PublishmentSystemID=<%=base.Request.QueryString["PublishmentSystemID"]%>&WebsiteMessageName=Default' islink='true' target='content'>留言列表模板</a>
                </td>
            </tr>

            <tr treeitemlevel="2">
                <td align="left" nowrap>
                    <img align="absmiddle" src="/SiteFiles/bairong/icons/tree/empty.gif" /><img align="absmiddle" border="0" src="/SiteFiles/bairong/icons/tree/folder.gif" />&nbsp;<a href='/siteserver/stl/background_websiteMessageTemplateDetail.aspx?PublishmentSystemID=<%=base.Request.QueryString["PublishmentSystemID"]%>&WebsiteMessageName=Default' islink='true' target='content'>留言详情模板</a>
                </td>
            </tr>

            <tr treeitemlevel="2">
                <td align="left" nowrap>
                    <img align="absmiddle" src="/SiteFiles/bairong/icons/tree/empty.gif" /><img align="absmiddle" border="0" src="/SiteFiles/bairong/icons/tree/folder.gif" />&nbsp;<a href='/siteserver/stl/background_websiteMessageTemplateEmail.aspx?PublishmentSystemID=<%=base.Request.QueryString["PublishmentSystemID"]%>&WebsiteMessageName=Default' islink='true' target='content'>邮件回复模板</a>
                </td>
            </tr>

            <tr treeitemlevel="2">
                <td align="left" nowrap>
                    <img align="absmiddle" src="/SiteFiles/bairong/icons/tree/empty.gif" /><img align="absmiddle" border="0" src="/SiteFiles/bairong/icons/tree/folder.gif" />&nbsp;<a href='/siteserver/stl/background_websiteMessageTemplateSMS.aspx?PublishmentSystemID=<%=base.Request.QueryString["PublishmentSystemID"]%>&WebsiteMessageName=Default' islink='true' target='content'>短信回复模板</a>
                </td>
            </tr>

            <tr treeitemlevel="2">
                <td align="left" nowrap>
                    <img align="absmiddle" src="/SiteFiles/bairong/icons/tree/empty.gif" /><img align="absmiddle" border="0" src="/SiteFiles/bairong/icons/tree/folder.gif" />&nbsp;<a href='background_websiteMessageReplayTemplate.aspx?PublishmentSystemID=<%=base.Request.QueryString["PublishmentSystemID"]%>&WebsiteMessageName=Default' islink='true' target='content'>留言回复模板</a>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
