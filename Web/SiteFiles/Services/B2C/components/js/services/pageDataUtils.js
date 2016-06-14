var pageDataUtils = {};

pageDataUtils.getPageHtml = function (pageJson, getData) {

    var pageHtml = "";
    if (pageJson.length == 0)
        return pageHtml;
    pageJson = eval("(" + pageJson + ")");
    var total = pageJson.total;
    var pageIndex = pageJson.pageIndex;
    var prePageNum = pageJson.prePageNum;

    pageHtml += "&nbsp;<span class='mpage_Nonum' id='spanTotal'>共" + total + "条</span>&nbsp;";
    pageHtml += "&nbsp;<a href='javascript:void(0)' class='mpage_num' pageIndex='" + pageJson.pre + "' prePageNum='" + prePageNum + "' onclick='" + getData + "(" + pageJson.pre + "," + prePageNum + ")'>上一页</a>&nbsp;";
    for (var j = 0; j < pageJson.list.length; j++) {
        if (pageJson.list[j] == pageIndex)
            pageHtml += "&nbsp;<a href='javascript:void(0)' class='mpage_num mpage_cutnum' pageIndex='" + pageJson.list[j] + "' prePageNum='" + prePageNum + "'>" + pageJson.list[j] + "</a>&nbsp;";
        else
            pageHtml += "&nbsp;<a href='javascript:void(0)' class='mpage_num' pageIndex='" + pageJson.list[j] + "' prePageNum='" + prePageNum + "' onclick='" + getData + "(" + pageJson.list[j] + "," + prePageNum + ")'>" + pageJson.list[j] + "</a>&nbsp;";
    }
    pageHtml += "&nbsp;<a href='javascript:void(0)' class='mpage_num' pageIndex='" + pageJson.next + "' prePageNum='" + prePageNum + "' onclick='" + getData + "(" + pageJson.next + "," + prePageNum + ")'>下一页</a>&nbsp;";
    return pageHtml;
}


//==========getData========================
function initConsultation(pageIndex, prePageNum) {
    consultationController.initConsultation("", pageIndex, prePageNum);
}

function initOrderComment(pageIndex, prePageNum) {
    consultationController.initOrderComment("", pageIndex, prePageNum);
}
