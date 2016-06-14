/// <reference path="../../defs/jquery.d.ts" />
/// <reference path="../utils/utils.ts" />
/// <reference path="../utils/stringUtils.ts" />
/// <reference path="../utils/urlUtils.ts" />

class OrderItemReturnService {

    private getUrl(action: string, id?: number): string {
        return UrlUtils.getAPI('orderItemReturn', action, id);
    }

    deleteOrderItemReturn(orderItemReturnID: number): void {
        var parms = { id: orderItemReturnID };
        Utils.ajaxGet(parms, this.getUrl("DeleteOrderItemReturn"),(json) => {
            if (json.isSuccess)
                Utils.tipAlert(true, "操作成功")
            else
                Utils.tipAlert(false, json.errorMessage)
        });
    }

    saveOrderItemReturn(orderItemID: number, publishmentSystemID: number, returnType: string, returnCount: number, inspectReport: boolean, description: string, images: string, contact: string, contactPhone: string): void {
        var parms = { orderItemID: orderItemID, publishmentSystemID: publishmentSystemID, returnType: returnType, returnCount: returnCount, inspectReport: inspectReport, description: description, imageUrl: images, contact: contact, contactPhone: contactPhone };
        Utils.ajaxGet(parms, this.getUrl("SaveOrderItemReturn"),(json) => {
            if (json.isSuccess)
                Utils.tipAlert(true, "操作成功")
            else
                Utils.tipAlert(false, json.errorMessage)
        });
    }

    getUploadImgUrl(action: string, orderItemID: number, publishmentSystemID: number): string {
        return this.getUrl(action) + "?orderItemID=" + orderItemID.toString() + "&publishmentSystemID=" + publishmentSystemID.toString();
    }

    getOrderItemReturnRecordList(isPC: string, orderTime: number, keywords: string, pageIndex: number, prePageNum: number, done: (data) => void): void {
        Utils.ajaxGet({ isPC: isPC, orderTime: orderTime, keywords: keywords, pageIndex: pageIndex, prePageNum: prePageNum }, this.getUrl('getOrderItemReturnRecordList'), done);
    }
}