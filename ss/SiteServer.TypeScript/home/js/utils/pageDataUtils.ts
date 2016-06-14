class pageDataUtils {

    static getPageHtml(pageJson: any, getData: string): string {
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
}


//==========getData========================
function getUserLoginLogData(pageIndex, prePageNum): void {
    var hc = new HomeController();
    hc.getUserLoginLog(pageIndex, prePageNum);
}

function getUserMessageData(pageIndex, prePageNum): void {
    var uc = new UserMessageController();
    uc.getUserMessage(pageIndex, prePageNum);
}

function getUserMessageNotice(pageIndex, prePageNum): void {
    var unc = new UserMessageNoticeController();
    unc.getUserMessageNotice(pageIndex, prePageNum);
}

function getAllOrderList(pageIndex, prePageNum): void {
    var ol = new BcOrderListController();
    ol.bindOrderList(pageIndex, prePageNum);
}

function getAllOrderList1(pageIndex, prePageNum): void {
    var ol = new BcSuggestionController();
    ol.bindOrderList(pageIndex, prePageNum);
}

function getAllConsultationList(pageIndex, prePageNum): void {
    var cl = new BcConsultationListController();
    cl.bindConsultationList(pageIndex, prePageNum);
}

function getUserFollow(pageIndex, prePageNum): void {
    var flc = new BcFollowController();
    flc.getUserFollow(pageIndex, prePageNum);
}

function getUserHistory(pageIndex, prePageNum): void {
    var flc = new BcHistoryController();
    flc.getUserHistory(pageIndex, prePageNum);
} 

function getOrderItemList(pageIndex, prePageNum): void {
    var bc = new BcOrderCommentController();
    bc.bindOrderItemList(pageIndex, prePageNum);
}

function getSiteMessage(pageIndex, prePageNum): void {
    var sm = new SiteMessageController();
    sm.getSiteMessage(pageIndex, prePageNum);
}

function getEvaluationMessageData(pageIndex, prePageNum): void {
    var em = new EvaluationMessageController();
    em.getEvaluationMessage(pageIndex, prePageNum);
}

function getTrialApplyMessageData(pageIndex, prePageNum): void {
    var tam = new TrialApplyMessageController();
    tam.getTrialApplyMessage(pageIndex, prePageNum);
}

function getTrialReportMessageData(pageIndex, prePageNum): void {
    var ctr = new TrialReportMessageController();
    ctr.getTrialReportMessage(pageIndex, prePageNum);
}

/**投稿中心
*/
//function getMLibDraftData(pageIndex, prePageNum) {
//    var mlibc = new UserMLibDraftController();
//    mlibc.getUserMLibDraftContent(pageIndex, prePageNum);
//}