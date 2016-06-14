<%@ Page Language="C#" Trace="false" EnableViewState="false" Inherits="SiteServer.STL.BackgroundPages.BackgroundTemplateLeft" %>

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

        <script language="JavaScript">
            function getTreeLevel(e) {
                var length = 0;
                if (!isNull(e)) {
                    if (e.tagName == 'TR') {
                        length = parseInt(e.getAttribute('treeItemLevel'));
                    }
                }
                return length;
            }

            function getTrElement(element) {
                if (isNull(element)) return;
                for (element = element.parentNode; ;) {
                    if (element != null && element.tagName == 'TR') {
                        break;
                    } else {
                        element = element.parentNode;
                    }
                }
                return element;
            }

            function getImgClickableElementByTr(element) {
                if (isNull(element) || element.tagName != 'TR') return;
                var img = null;
                if (!isNull(element.childNodes)) {
                    var imgCol = element.getElementsByTagName('IMG');
                    if (!isNull(imgCol)) {
                        for (x = 0; x < imgCol.length; x++) {
                            if (!isNull(imgCol.item(x).getAttribute('isOpen'))) {
                                img = imgCol.item(x);
                                break;
                            }
                        }
                    }
                }
                return img;
            }

            var weightedLink = null;

            function fontWeightLink(element) {
                if (weightedLink != null) {
                    weightedLink.style.fontWeight = 'normal';
                }
                element.style.fontWeight = 'bold';
                weightedLink = element;
            }

            function displayChildren(img) {
                if (isNull(img)) return;

                var tr = getTrElement(img);

                var isToOpen = img.getAttribute('isOpen') == 'false';
                var isByAjax = img.getAttribute('isAjax') == 'true';
                var templateType = img.getAttribute('templateType');

                if (!isNull(img) && img.getAttribute('isOpen') != null) {
                    if (img.getAttribute('isOpen') == 'false') {
                        img.setAttribute('isOpen', 'true');
                        img.setAttribute('src', '../../SiteFiles/bairong/icons/tree/minus.gif');
                    } else {
                        img.setAttribute('isOpen', 'false');
                        img.setAttribute('src', '../../SiteFiles/bairong/icons/tree/plus.gif');
                    }
                }

                if (isToOpen && isByAjax) {
                    var div = document.createElement('div');
                    div.innerHTML = "<img align='absmiddle' border='0' src='../../SiteFiles/bairong/icons/loading.gif' /> 栏目加载中，请稍候...";
                    img.parentNode.appendChild(div);
                    $(div).addClass('loading');
                    loadingChannels(tr, img, div, templateType);
                }
                else {
                    var level = getTreeLevel(tr);

                    var collection = new Array();
                    var index = 0;

                    for (var e = tr.nextSibling; !isNull(e) ; e = e.nextSibling) {
                        if (!isNull(e) && !isNull(e.tagName) && e.tagName == 'TR') {
                            var currentLevel = getTreeLevel(e);
                            if (currentLevel <= level) break;
                            if (e.style.display == '') {
                                e.style.display = 'none';
                            } else {
                                if (currentLevel != level + 1) continue;
                                e.style.display = '';
                                var imgClickable = getImgClickableElementByTr(e);
                                if (!isNull(imgClickable)) {
                                    if (!isNull(imgClickable.getAttribute('isOpen')) && imgClickable.getAttribute('isOpen') == 'true') {
                                        imgClickable.setAttribute('isOpen', 'false');
                                        imgClickable.setAttribute('src', '../../SiteFiles/bairong/icons/tree/plus.gif');
                                        collection[index] = imgClickable;
                                        index++;
                                    }
                                }
                            }
                        }
                    }

                    if (index > 0) {
                        for (i = 0; i <= index; i++) {
                            displayChildren(collection[i]);
                        }
                    }
                }
            }

            function loadingChannels(tr, img, div, templateType) {
                var url = '<%=GetServiceUrl()%>';
                var pars = 'publishmentSystemID=<%=base.Request.QueryString["PublishmentSystemID"]%>&templateType=' + templateType;

                jQuery.post(url, pars, function (data, textStatus) {
                    $($.parseHTML(data)).insertAfter($(tr));
                    img.setAttribute('isAjax', 'false');
                    img.parentNode.removeChild(div);

                    $('a[rel=tooltip]').tooltip({
                        placement: "top"
                    });
                });
            }

        </script>

        <table class="table noborder table-condensed table-hover">
            <tr class="info thead">
                <td style="text-align: center" onclick="location.reload();">
                    <lan>模板列表</lan>
                </td>
            </tr>

            <tr treeitemlevel="1">
                <td align="left" nowrap>
                    <img align="absmiddle" style="cursor: pointer" onclick="displayChildren(this);" isajax="false" isopen="true" src="/SiteFiles/bairong/icons/tree/minus.gif" /><img align="absmiddle" border="0" src="../../sitefiles/bairong/Icons/tree/folder.gif" />&nbsp;<a href='background_template.aspx?PublishmentSystemID=<%=base.Request.QueryString["PublishmentSystemID"]%>' islink='true' onclick='fontWeightLink(this)' target='management'>所有模板</a></a>&nbsp;<span style="font-size: 8pt; font-family: arial" class="gray">(<%=GetCount("")%>)</span>
                </td>
            </tr>

            <tr treeitemlevel="2">
                <td align="left" nowrap>
                    <img align="absmiddle" src="/SiteFiles/bairong/icons/tree/empty.gif" /><img align="absmiddle" style="cursor: pointer" onclick="displayChildren(this);" isajax="true" isopen="false" templatetype="IndexPageTemplate" src="/SiteFiles/bairong/icons/tree/plus.gif" /><img align="absmiddle" border="0" src="/SiteFiles/bairong/icons/tree/folder.gif" />&nbsp;<a href='background_template.aspx?PublishmentSystemID=<%=base.Request.QueryString["PublishmentSystemID"]%>&TemplateType=IndexPageTemplate' islink='true' onclick='fontWeightLink(this)' target='management'>首页模板</a>&nbsp;&nbsp;<span style="font-size: 8pt; font-family: arial" class="gray">(<%=GetCount("IndexPageTemplate")%>)</span>
                </td>
            </tr>

            <tr treeitemlevel="2">
                <td align="left" nowrap>
                    <img align="absmiddle" src="/SiteFiles/bairong/icons/tree/empty.gif" /><img align="absmiddle" style="cursor: pointer" onclick="displayChildren(this);" isajax="true" isopen="false" templatetype="ChannelTemplate" src="/SiteFiles/bairong/icons/tree/plus.gif" /><img align="absmiddle" border="0" src="/SiteFiles/bairong/icons/tree/folder.gif" />&nbsp;<a href='background_template.aspx?PublishmentSystemID=<%=base.Request.QueryString["PublishmentSystemID"]%>&TemplateType=ChannelTemplate' islink='true' onclick='fontWeightLink(this)' target='management'>栏目模板</a>&nbsp;&nbsp;<span style="font-size: 8pt; font-family: arial" class="gray">(<%=GetCount("ChannelTemplate")%>)</span>
                </td>
            </tr>

            <tr treeitemlevel="2">
                <td align="left" nowrap>
                    <img align="absmiddle" src="/SiteFiles/bairong/icons/tree/empty.gif" /><img align="absmiddle" style="cursor: pointer" onclick="displayChildren(this);" isajax="true" isopen="false" templatetype="ContentTemplate" src="/SiteFiles/bairong/icons/tree/plus.gif" /><img align="absmiddle" border="0" src="/SiteFiles/bairong/icons/tree/folder.gif" />&nbsp;<a href='background_template.aspx?PublishmentSystemID=<%=base.Request.QueryString["PublishmentSystemID"]%>&TemplateType=ContentTemplate' islink='true' onclick='fontWeightLink(this)' target='management'>内容模板</a>&nbsp;&nbsp;<span style="font-size: 8pt; font-family: arial" class="gray">(<%=GetCount("ContentTemplate")%>)</span>
                </td>
            </tr>

            <tr treeitemlevel="2">
                <td align="left" nowrap>
                    <img align="absmiddle" src="/SiteFiles/bairong/icons/tree/empty.gif" /><img align="absmiddle" style="cursor: pointer" onclick="displayChildren(this);" isajax="true" isopen="false" templatetype="FileTemplate" src="/SiteFiles/bairong/icons/tree/plus.gif" /><img align="absmiddle" border="0" src="/SiteFiles/bairong/icons/tree/folder.gif" />&nbsp;<a href='background_template.aspx?PublishmentSystemID=<%=base.Request.QueryString["PublishmentSystemID"]%>&TemplateType=FileTemplate' islink='true' onclick='fontWeightLink(this)' target='management'>单页模板</a>&nbsp;&nbsp;<span style="font-size: 8pt; font-family: arial" class="gray">(<%=GetCount("FileTemplate")%>)</span>
                </td>
            </tr>
        </table>

        <hr />
        <table class="table table-noborder table-hover">
            <tr>
                <td class="center">
                    <asp:Literal ID="ltlReplaceUserCenterTemplate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="center">
                    <asp:Literal ID="ltlReplaceIndexPageTemplate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="center">
                    <asp:Literal ID="ltlReplaceChannelTemplate" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="center">
                    <asp:Literal ID="ltlReplaceContentTemplate" runat="server" />
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
