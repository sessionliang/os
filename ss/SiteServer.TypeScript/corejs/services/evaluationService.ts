/// <reference path="../../defs/jquery.d.ts" />
/// <reference path="../utils/utils.ts" />
/// <reference path="../utils/stringUtils.ts" />
/// <reference path="../utils/urlUtils.ts" />
/// <reference path="serviceBase.ts" />

class EvaluationService extends ServiceBase {
    
    constructor() {
        super("Evaluation");
    }

    //get user evaluation message list 评价信息
    getUserEvaluationRecord(pageIndex: number, prePageNum: number, done: (data) => void): void {
        var parameters = { pageIndex: pageIndex, prePageNum: prePageNum };
        Utils.ajaxGet(parameters, super.getUrl('GetUserEvaluationRecord'), done);
    }
} 