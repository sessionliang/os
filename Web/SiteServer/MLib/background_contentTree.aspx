<%@ Page Language="C#" Trace="false" EnableViewState="false" Inherits="SiteServer.CMS.BackgroundPages.MLib.ContentTree" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <!--#include file="../inc/header.aspx"-->
    <!--[if lte IE 6]>
 <style type="text/css">
    img{width:18px;height:22px;}
 </style>
<![endif]-->

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

        var completedNodeID = null;
        function displayChildren(img) {
            if (isNull(img)) return;

            var tr = getTrElement(img);

            var isToOpen = img.getAttribute('isOpen') == 'false';
            var isByAjax = img.getAttribute('isAjax') == 'true';
            var nodeID = img.getAttribute('id');

            if (!isNull(img) && img.getAttribute('isOpen') != null) {
                if (img.getAttribute('isOpen') == 'false') {
                    img.setAttribute('isOpen', 'true');
                    img.setAttribute('src', '/SiteFiles/bairong/icons/tree/minus.gif');
                } else {
                    img.setAttribute('isOpen', 'false');
                    img.setAttribute('src', '/SiteFiles/bairong/icons/tree/plus.gif');
                }
            }

            if (isToOpen && isByAjax) {
                var div = document.createElement('div');
                div.innerHTML = "<img align='absmiddle' border='0' src='/SiteFiles/bairong/icons/loading.gif' /> 栏目加载中，请稍候...";
                img.parentNode.appendChild(div);
                $(div).addClass('loading');
                loadingChannels(tr, img, div, nodeID);
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
                                    imgClickable.setAttribute('src', '/SiteFiles/bairong/icons/tree/plus.gif');
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

        function loadingChannels(tr, img, div, nodeID) {
            var url = '/siteserver/mlib/services/OtherService.aspx?publishmentSystemId=<%=base.PublishmentSystemID%>';
            var pars = '';

            jQuery.post(url, pars, function (data, textStatus) {
                $(data).insertAfter($(tr));
                img.setAttribute('isAjax', 'false');
                img.parentNode.removeChild(div);
            });
            completedNodeID = nodeID;
        }

        function loadingChannelsOnLoad(paths) {
            if (paths && paths.length > 0) {
                var nodeIDs = paths.split(',');
                var nodeID = nodeIDs[0];
                var img = $('#' + nodeID);
                if (img.attr('isOpen') == 'false') {
                    displayChildren(img[0]);
                    if (completedNodeID && completedNodeID == nodeID) {
                        if (paths.indexOf(',') != -1) {
                            paths = paths.substring(paths.indexOf(',') + 1);
                            setTimeout("loadingChannelsOnLoad('" + paths + "')", 1000);
                        }
                    }
                }
            }
        }

        $(function () {
            $.post('services/OtherService.aspx', { action: 'GetDraftCount', publishmentSystemId: '<%=base.PublishmentSystemID%>' }, function (result) {
                if (result == '-1') {
                    $('#drafttr').hide();
                } else {
                    $('#drafttr .gray').text('(' + result + ')');
                }

            });
        })
    </script>
</head>

<body>
    <!--#include file="../inc/openWindow.html"-->
    <form runat="server">
        <table class="table noborder table-condensed table-hover">
            <tr class="info">
                <td style="padding-left: 60px;">
                    <a href="javascript:;" onclick="location.reload();" title="点击刷新栏目树">
                        <lan>分类树&nbsp;&nbsp;&nbsp;&nbsp;<a href="http://cms.siteserver.cn/help/20150910/421.html" target='_blank' style="color:#F88;">查看帮助</a></lan>
                    </a></td>
            </tr>
            <tr treeitemlevel="1">
                <td align="left" nowrap>
                    <img align="absmiddle" style="cursor: pointer" onclick="displayChildren(this);" isajax="false" isopen="true" id="5185"
                        src="/SiteFiles/bairong/icons/tree/minus.gif" />
                    <a href='javascript:;' target="_blank">
                        <img align="absmiddle" border="0" src="/SiteFiles/bairong/icons/tree/folder.gif" />

                    </a>&nbsp;
                    <a href='javascript:;' islink='false' onclick='fontWeightLink(this)' target='content'>稿件分类</a>
                    &nbsp;<img align="absmiddle" alt="站点" border="0" src="/SiteFiles/bairong/icons/tree/site.gif" />
                    </a>&nbsp;
                    <span style="font-size: 8pt; font-family: arial" class="gray"></span>
                </td>
            </tr>

            <tr treeitemlevel="2" id="drafttr">
                <td align="left" nowrap>
                    <img align="absmiddle" src="/SiteFiles/bairong/icons/tree/empty.gif" />
                    <img align="absmiddle" src="/SiteFiles/bairong/icons/tree/empty.gif" />
                    <a href='javascript:;'>
                        <img align="absmiddle" border="0" src="/SiteFiles/bairong/icons/tree/folder.gif" /></a>&nbsp;
                    <a href='DraftList.aspx?PublishmentSystemID=<%=base.PublishmentSystemID%>' islink='true' onclick='fontWeightLink(this)' target='content'>投稿箱</a>&nbsp;&nbsp;
                    <span style="font-size: 8pt; font-family: arial" class="gray">(0)</span>
                </td>
            </tr>

            <tr treeitemlevel="2">
                <td align="left" nowrap>
                    <img align="absmiddle" src="/SiteFiles/bairong/icons/tree/empty.gif" />
                    <img align="absmiddle" style="cursor: pointer" onclick="displayChildren(this);" isajax="true" isopen="false" src="/SiteFiles/bairong/icons/tree/plus.gif" />
                    <a href='javascript:;'>
                        <img align="absmiddle" border="0" src="/SiteFiles/bairong/icons/tree/folder.gif" />
                    </a>&nbsp;
                    <a href='javascript:;' islink='true' onclick='fontWeightLink(this)' target='content'>稿件分类</a>&nbsp;&nbsp;
                    <span style="font-size: 8pt; font-family: arial" class="gray">(0)</span>
                </td>
            </tr>

        </table>
    </form>
</body>
</html>
<!-- check for 3.6 html permissions -->
