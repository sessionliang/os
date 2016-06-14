/// <reference path="../../defs/jquery.d.ts" />
/// <reference path="../utils/utils.ts" />
/// <reference path="../utils/stringUtils.ts" />
/// <reference path="../utils/urlUtils.ts" />

class ConsultationService {

    private getUrl(action: string, id?: number): string {
        return UrlUtils.getAPI('consultation', action, id);
    }



    getAllConsultationList(keywords: string, pageIndex: number, prePageNum: number, done: (data) => void): void {
        Utils.ajaxGet({ keywords: keywords, pageIndex: pageIndex, prePageNum: prePageNum }, this.getUrl('GetAllConsultationList'), done);
    }

    deleteConsultation(consultationID: number): void {
        var parms = { id: consultationID };
        Utils.ajaxGet(parms, this.getUrl("DeleteConsultation"),() => { Utils.tipAlert(true, "操作成功") });
    }

    SaveConsultation(type: string, question: string, publishmentSystemID: number, channelID: number, contentID: number): void {
        var parms = { type: type, question: question, publishmentSystemID: publishmentSystemID, channelID: channelID, contentID: contentID };
        Utils.ajaxGet(parms, this.getUrl("SaveConsultation"),() => { Utils.tipAlert(true, "操作成功") });
    }
}