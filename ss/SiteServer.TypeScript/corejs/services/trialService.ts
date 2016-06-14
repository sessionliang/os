/// <reference path="../../defs/jquery.d.ts" />
/// <reference path="../utils/utils.ts" />
/// <reference path="../utils/stringUtils.ts" />
/// <reference path="../utils/urlUtils.ts" />
/// <reference path="serviceBase.ts" />

class TrialService extends ServiceBase {

    constructor() {
        super("Trial");
    }

    //get user TrialApply message list 申请试用信息
    getUserTrialApplyRecord(pageIndex: number, prePageNum: number, done: (data) => void): void {
        var parameters = { pageIndex: pageIndex, prePageNum: prePageNum };
        Utils.ajaxGet(parameters, super.getUrl('GetUserTrialApplyRecord'), done);
    }

    //get user TrialReport message list 试用报告信息
    getUserTrialReportRecord(pageIndex: number, prePageNum: number, done: (data) => void): void {
        var parameters = { pageIndex: pageIndex, prePageNum: prePageNum };
        Utils.ajaxGet(parameters, super.getUrl('GetUserTrialReportRecord'), done);
    }

    //get trial report table style info array
    getTrialReportFormInfoArray(trialApplyID: number, done: (data) => void): void {
        var parameters = { trialApplyID: trialApplyID };
        Utils.ajaxGet(parameters, super.getUrl('getTrialReportFormInfoArray'), done);
    }

    //add trial report
    addTrialReport(form: HTMLElement, applyId: number, done: (data) => void): void {
        Utils.ajaxPostForm(form, super.getUrl('addTrialReport') + "?trialApplyID=" + applyId, done);
    }
} 