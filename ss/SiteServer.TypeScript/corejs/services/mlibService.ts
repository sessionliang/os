/// <reference path="../../defs/jquery.d.ts" />
/// <reference path="../utils/utils.ts" />
/// <reference path="../utils/stringUtils.ts" />
/// <reference path="../utils/urlUtils.ts" />

class MLibService {

    private getUrl(action: string, id?: number): string {
        return UrlUtils.getAPI('mlib', action, id);
    }

    /******
     * 当前用户是否可以投稿  
     ******/
    canMLib(done: (json) => void) {
        Utils.ajaxGet({}, this.getUrl('CantMLib'), done);
    } 

    /******
      * 获取当前用户的可投稿站点  
      ******/
    getMLibPublishmentSystemList(done: (data) => void): void {
        Utils.ajaxGet({}, this.getUrl('GetUserMLibPublishmentSystem'), done);
    }

    /******
      * 获取当前用户的可投稿栏目   
      ******/
    getMLibNodeList(publishmentSystemID: number, done: (data) => void): void {
        Utils.ajaxGet({ publishmentSystemID: publishmentSystemID }, this.getUrl('GetUserMLibNodeInfo'), done);
    }

    /******
      * 获取投稿字段  
      ******/
    getMLibFileds(publishmentSystemID: number, nodeID: number, done: (data) => void): void {
        var parms = { publishmentSystemID: publishmentSystemID, nodeID: nodeID };
        Utils.ajaxGet(parms, this.getUrl("GetMLibFileds"), done);
    }
    
    /******
      * 获取当前用户的草稿列表   
      ******/
    getUserMLibDraftContentList(title: string, startdate: string, enddate: string, pageIndex: number, prePageNum: number, done: (data) => void): void {
        Utils.ajaxGet({ title: title, startdate: startdate, enddate: enddate, pageIndex: pageIndex, prePageNum: prePageNum }, this.getUrl('GetUserMLibDraftContents'), done);
    }

    /******
      * 获取当前用户的投稿列表   
      ******/
    getUserMLibContentList(publishmentSystemID: number, nodeID: number, title: string, startdate: string, enddate: string, pageIndex: number, prePageNum: number, done: (data) => void): void {
        Utils.ajaxGet({ publishmentSystemID: publishmentSystemID, nodeID: nodeID, title: title, startdate: startdate, enddate: enddate, pageIndex: pageIndex, prePageNum: prePageNum }, this.getUrl('GetUserMLibContent'), done);
    } 

    /******
      * 获取某条草稿信息   
      ******/
    getMLibDraftContentItem(contentID: number, publishmentSystemID: number, nodeID: number, done: (data) => void): void {
        var parms = { contentID: contentID, publishmentSystemID: publishmentSystemID, nodeID: nodeID };
        Utils.ajaxGet(parms, this.getUrl("GetUserMLibDraftContent"), done);
    }

    /******
      * 保存草稿信息   
      ******/
    saveMLibDraftContent(contentID: number, publishmentSystemID: number, nodeID: number, form: string): void {
        var parms = { contentID: contentID, publishmentSystemID: publishmentSystemID, nodeID: nodeID, form: form };
        if (contentID > 0) {
            Utils.ajaxGet(parms, this.getUrl("UpdateMLibDraftContent"), (json) => { if (json.isSuccess) Utils.tipAlert(true, "操作成功"); else Utils.tipAlert(false, "操作失败"); });
        } else
            Utils.ajaxGet(parms, this.getUrl("CreateMLibDraftContent"), (json) => { if (json.isSuccess) Utils.tipAlert(true, "操作成功"); else Utils.tipAlert(false, "操作失败"); });
    }

    /******
      * 删除草稿信息   
      ******/
    deleteMLibDraftContent(deleteStr: string, idsCollection: number): void {
        var parms = { deleteStr: deleteStr, idsCollection: idsCollection };
        Utils.ajaxGet(parms, this.getUrl("DeleteMLibDraftContent"), (data) => { if (data.isSuccess) Utils.tipAlert(true, "操作成功"); else Utils.tipAlert(true, "操作失败") });
    }

    /******
      * 提交投稿信息   
      ******/
    submitMLibDraftContent(publishmentSystemID: number, nodeID: number, form: string): void {
        var parms = { publishmentSystemID: publishmentSystemID, nodeID: nodeID, form: form };
        Utils.ajaxGet(parms, this.getUrl("CreateMLibContent"), (json) => { if (json.isSuccess) Utils.tipAlert(true, "操作成功"); else Utils.tipAlert(false, "操作失败"); });
    } 

    /******
      * 提交投稿信息   
      ******/
    exportMLibContent(publishmentSystemID: number, nodeID: number, form: string): void {
        var parms = { publishmentSystemID: publishmentSystemID, nodeID: nodeID, form: form };
        Utils.ajaxGet(parms, this.getUrl("ExportUserMLibContent"), (json) => { if (json.isSuccess) Utils.tipAlert(true, "操作成功"); else Utils.tipAlert(false, "操作失败"); });
    }





}