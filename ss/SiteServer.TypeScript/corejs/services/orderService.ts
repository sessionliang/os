/// <reference path="../../defs/jquery.d.ts" />
/// <reference path="../utils/utils.ts" />
/// <reference path="../utils/stringUtils.ts" />
/// <reference path="../utils/urlUtils.ts" />

class OrderService {

    private getUrl(action: string, id?: number): string {
        return UrlUtils.getAPI('order', action, id);
    }

    getOrderList(pageIndex: number, prePageNum: number, done: (data) => void): void {
        Utils.ajaxGet({ pageIndex: pageIndex, prePageNum: prePageNum }, this.getUrl('GetOrderList'), done);
    }

    getAllOrderList(isCompleted: string, isPayment: string, isPC: string, orderTime: number, keywords: string, pageIndex: number, prePageNum: number, done: (data) => void): void {
        Utils.ajaxGet({ isCompleted: isCompleted, isPayment: isPayment, isPC: isPC, orderTime: orderTime, keywords: keywords, pageIndex: pageIndex, prePageNum: prePageNum }, this.getUrl('GetAllOrderList'), done);
    }

    deleteOrder(orderID: number): void {
        var parms = { id: orderID };
        Utils.ajaxGet(parms, this.getUrl("DeleteOrder"),(data) => { if (data.isSuccess) Utils.tipAlert(true, "操作成功"); else Utils.tipAlert(true, "操作失败") });
    }

    getOrderItemList(orderID: number, publishmentSystemID: number, done: (data) => void): void {
        var parms = { orderID: orderID, publishmentSystemID: publishmentSystemID };
        Utils.ajaxGet(parms, this.getUrl("GetOrderItemList"), done);
    }

    getAllOrderItemList(pageIndex: number, prePageNum: number, done: (data) => void): void {
        var parms = { pageIndex: pageIndex, prePageNum: prePageNum };
        Utils.ajaxGet(parms, this.getUrl("GetAllOrderItemList"), done);
    }

    saveOrderItemComment(orderID: number, orderItemID: string, publishmentSystemID: number, star: number, tags: string, comment: string, isAnonymous: boolean, imageUrl: string): void {
        var parms = { orderID: orderID, orderItemID: orderItemID, publishmentSystemID: publishmentSystemID, star: star, tags: tags, comment: comment, isAnonymous: isAnonymous, imageUrl: imageUrl };
        Utils.ajaxGet(parms, this.getUrl("SaveOrderItemComment"),(json) => { if (json.isSuccess) Utils.tipAlert(true, "操作成功"); else Utils.tipAlert(false, "操作失败"); });
    }

    getLatestOrderInAll(isPC: string, done: (json) => void) {
        Utils.ajaxGet({ isPC: isPC }, this.getUrl('GetLatestOrderInAll'), done);
    }

    getOrderStatistic(done: (json) => void) {
        Utils.ajaxGet({}, this.getUrl('GetOrderStatistic'), done);
    }

    getOrderItem(orderItemID: string, publishmentSystemID: number, done: (data) => void): void {
        var parms = { orderItemID: orderItemID, publishmentSystemID: publishmentSystemID };
        Utils.ajaxGet(parms, this.getUrl("GetOrderItem"), done);
    }

    getAllOrderListWithSiteInfo(isCompleted: string, isPayment: string, isPC: string, orderTime: number, keywords: string, pageIndex: number, prePageNum: number, done: (data) => void): void {
        Utils.ajaxGet({ isCompleted: isCompleted, isPayment: isPayment, isPC: isPC, orderTime: orderTime, keywords: keywords, pageIndex: pageIndex, prePageNum: prePageNum }, this.getUrl('GetAllOrderListWithQiao'), done);
    }

    //晒单图片
    getUploadImgUrl(action: string, orderItemID: any, publishmentSystemID: number): string {
        return this.getUrl(action) + "?orderItemID=" + orderItemID.toString() + "&publishmentSystemID=" + publishmentSystemID.toString();
    }

    //获取第一个B2C站点信息
    getFirstB2CPageInfo(done: (data) => void, pID?: any): void {
        Utils.ajaxGetSync({ publishmentSystemID: pID }, UrlUtils.getAPI('b2c', "GetFirstB2CPageInfo"), done);
    }
}