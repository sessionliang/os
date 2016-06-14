/// <reference path="wapBaseController.ts" />

/**********
* 订单详情
**********/
class WapOrderItemListController extends WapBaseController {

    public static orderService: OrderService;
    public static orderInfoID: number = UrlUtils.getUrlVar("orderID");
    public static publishmentSystemID: number = UrlUtils.getUrlVar("publishmentSystemID");
    public static orderInfo: any;
    public static orderItemList: any;
    public isPC: string = Utils.isPC();

    public isEmpty: boolean = true;
    public estimate: Object;
    public loading: boolean = true;

    public parmMap: any = Utils.urlToMap(window.location.href.split('?').length > 1 ? window.location.href.split('?')[1] : "");

    constructor() {
        super();
        WapOrderItemListController.orderService = new OrderService();
    }

    //初始化页面元素以及事件
    init(): void {
        WapBaseController.userAuthValidate(() => {
            this.bindEvent();
            this.bindData();
        });
    }

    //绑定事件
    bindEvent(): void {
    }

    //绑定数据
    bindData(): void {
        this.bindOrderItemList();
    }

    //绑定订单数据
    bindOrderItemList(): void {
        WapOrderItemListController.orderService.getOrderItemList(WapOrderItemListController.orderInfoID, WapOrderItemListController.publishmentSystemID,(json) => {
            WapOrderItemListController.orderInfo = json.orderList[0].orderInfo;
            WapOrderItemListController.orderItemList = json.orderItemList;
            //加载数据
            this.render();
            //分页链接
            //$("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getAllOrderItemList'));
            
        });
    }

    //删除订单
    removeOrder(orderID: number): void {
        var item = {};
        var orderSN: string = (<string>WapOrderItemListController.orderInfo["orderSN"]).toLocaleLowerCase();
        for (var i = 0; i < WapOrderItemListController.orderItemList[orderSN].length; i++) {
            if (WapOrderItemListController.orderItemList[orderSN][i].orderInfo.id == orderID) {
                item = WapOrderItemListController.orderItemList[orderSN][i];
                break;
            }
        }
        WapOrderItemListController.orderItemList[orderSN].splice($.inArray(item, WapOrderItemListController.orderItemList[orderSN]), 1);
        WapOrderItemListController.orderService.deleteOrder(orderID);
        this.render();
    }

    //渲染数据
    render(): void {
        var itemHtml = '';

        var itemBodyHtml = '<li data= "true" >';
        itemBodyHtml += '<div class= "mdf_top">';
        itemBodyHtml += '<a href="{5}" target="_blank"><img src="{1}" alt="{0}"></a>';
        itemBodyHtml += '<div class= "mdf_tnm">{0}</div>';
        itemBodyHtml += '<div class="mdf_tprice">¥{2} </div>';
        itemBodyHtml += '<div class="mdf_time"> {3}&nbsp;{4} </div>';
        itemBodyHtml += '</div>';
        itemBodyHtml += '</li>';
        var orderSN: string = (<string>WapOrderItemListController.orderInfo["orderSN"]).toLocaleLowerCase();
        for (var i = 0; i < WapOrderItemListController.orderItemList[orderSN].length; i++) {
            //第一个订单明细的图片
            var imageUrl = WapOrderItemListController.orderItemList[orderSN][i].thumbUrl;
            //下单时间
            var date = WapOrderItemListController.orderInfo.timeOrder.split('T');

            itemHtml += StringUtils.format(itemBodyHtml,
                WapOrderItemListController.orderItemList[orderSN][i].title,//订单标题
                imageUrl,//订单图片
                (parseInt(WapOrderItemListController.orderItemList[orderSN][i].priceSale) * parseInt(WapOrderItemListController.orderItemList[orderSN][i].purchaseNum)).toString(),//订单金额
                date[0],
                date[1],
                WapOrderItemListController.orderItemList[orderSN][i].navigationUrl);

        }

        //添加
        $("#orderItemUl").append(itemHtml);
    }

    getUrl(orderID: any, publishmentSystemID: any): string {

        this.parmMap['orderID'] = orderID;

        this.parmMap['publishmentSystemID'] = publishmentSystemID;

        return "myOrderItem.html?" + Utils.mapToUrl(this.parmMap);
    }


    static getRedirectUrl(orderID: any, publishmentSystemID: any): string {

        return "myOrderItem.html?orderID=" + orderID + "&publishmentSystemID=" + publishmentSystemID;
    }
}