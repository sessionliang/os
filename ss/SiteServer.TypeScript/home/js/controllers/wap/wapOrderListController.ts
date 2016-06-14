/// <reference path="wapBaseController.ts" />

/**********
* 订单
**********/
class WapOrderListController extends WapBaseController {

    public static orderService: OrderService;
    public static orderList: any;
    public static pageIndex: number = 1;
    public static prePageNum: number = 10;
    public isCompleted: string = "false";
    public isPayment: string = "false";
    public isPC: string = Utils.isPC();
    public orderTime: number = 0;
    public keywords: string = StringUtils.empty;

    public isEmpty: boolean = true;
    public estimate: Object;
    public loading: boolean = true;

    public parmMap: any = Utils.urlToMap(window.location.href.split('?').length > 1 ? window.location.href.split('?')[1] : "");

    constructor() {
        super();
        WapOrderListController.orderService = new OrderService();
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
        if ($("#selStatus").length > 0) {
            $("#selStatus").change(() => {
                this.orderStatusChange();
            });
        }

        if ($("#selTime").length > 0) {
            $("#selTime").change(() => {
                this.orderTimeChange();
            });
        }

        if ($("#btnSearch").length > 0) {
            $("#btnSearch").click(() => {
                this.btnSearchClick();
            });
        }
    }

    //绑定数据
    bindData(): void {
        this.bindOrderList(WapOrderListController.pageIndex, WapOrderListController.prePageNum);
        this.bindOrderStatus();
        this.bindOrderTime();
        this.bindOrderKeywords();
    }

    //绑定订单数据
    bindOrderList(pageIndex: number, prePageNum: number): void {
        WapOrderListController.pageIndex = pageIndex || WapOrderListController.pageIndex;
        WapOrderListController.prePageNum = prePageNum || WapOrderListController.prePageNum;
        this.isCompleted = <string>UrlUtils.getUrlVar("isCompleted");
        this.isPayment = <string>UrlUtils.getUrlVar("isPayment");
        this.keywords = <string>UrlUtils.getUrlVar("keywords");
        this.orderTime = <number>UrlUtils.getUrlVar("orderTime");

        WapOrderListController.orderService.getAllOrderList(this.isCompleted, this.isPayment, this.isPC, this.orderTime, this.keywords, WapOrderListController.pageIndex, WapOrderListController.prePageNum,(json) => {
            json.pageJson = eval("(" + json.pageJson + ")");
            if (json.pageJson.last < WapOrderListController.pageIndex) {
                WapOrderListController.pageIndex--;
                Utils.tipAlert(false, "到底了！");
                return;
            }
            WapOrderListController.orderList = json.orderInfoList;
            this.loading = false;
            if (WapOrderListController.orderList && WapOrderListController.orderList.length > 0) {
                this.isEmpty = false;
            }
            else {
                if (!this.isCompleted && !this.isPayment) {
                    this.isEmpty = true;
                }
                else {
                    this.isEmpty = false;
                }
            }
            //加载数据
            this.render();
            //分页链接
            //$("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getAllOrderList'));
            
        });
    }

    //瀑布流加载
    waterfallList(): void {
        WapOrderListController.pageIndex++;
        this.bindOrderList(WapOrderListController.pageIndex, WapOrderListController.prePageNum);
    }

    //订单状态帅选
    orderStatusChange(): void {
        var status = $("#selStatus").val();
        switch (status) {
            case "all":
                window.location.href = this.getUrl(StringUtils.empty, StringUtils.empty, UrlUtils.getUrlVar("orderTime"));
                break;
            case "noPay":
                window.location.href = this.getUrl("false", "false", UrlUtils.getUrlVar("orderTime"));
                break;
            case "pay":
                window.location.href = this.getUrl("false", "true", UrlUtils.getUrlVar("orderTime"));
                break;
            case "completed":
                window.location.href = this.getUrl("true", StringUtils.empty, UrlUtils.getUrlVar("orderTime"));
                break;
        }
    }

    //订单时间帅选
    orderTimeChange(): void {
        var time = $("#selTime").val();
        switch (time) {
            case "all":
                window.location.href = this.getUrl(UrlUtils.getUrlVar("isCompleted"), UrlUtils.getUrlVar("isPayment"), "all");
                break;
            case "90":
                window.location.href = this.getUrl(UrlUtils.getUrlVar("isCompleted"), UrlUtils.getUrlVar("isPayment"), "90");
                break;
            case "180":
                window.location.href = this.getUrl(UrlUtils.getUrlVar("isCompleted"), UrlUtils.getUrlVar("isPayment"), "180");
                break;
            case "365":
                window.location.href = this.getUrl(UrlUtils.getUrlVar("isCompleted"), UrlUtils.getUrlVar("isPayment"), "365");
                break;
        }
    }

    //订单关键字帅选
    btnSearchClick(): void {
        window.location.href = this.getUrl(UrlUtils.getUrlVar("isCompleted"), UrlUtils.getUrlVar("isPayment"), UrlUtils.getUrlVar("orderTime"), $("#txtKeywords").val());
    }

    //绑定订单状态帅选
    bindOrderStatus(): void {
        if ($("#selStatus").length > 0) {

            if (this.isCompleted == "false" && this.isPayment == "false") {
                $("#selStatus").val("noPay");
            }
            else if (this.isCompleted == "false" && this.isPayment == "true") {
                $("#selStatus").val("pay");
            }
            else if (this.isCompleted == "true") {
                $("#selStatus").val("completed");
            }
        }
    }

    //绑定订单时间帅选
    bindOrderTime(): void {
        if ($("#selTime").length > 0 && this.orderTime) {
            $("#selTime").val(UrlUtils.getUrlVar("orderTime"));
        }
    }

    //绑定订单关键字帅选
    bindOrderKeywords(): void {
        if ($("#txtKeywords").length > 0) {
            $("#txtKeywords").val(UrlUtils.getUrlVar("keywords"));
        }
    }

    //删除订单
    removeOrder(orderID: number): void {
        var item = {};
        for (var i = 0; i < WapOrderListController.orderList.length; i++) {
            if (WapOrderListController.orderList[i].orderInfo.id == orderID) {
                item = WapOrderListController.orderList[i];
                break;
            }
        }
        WapOrderListController.orderList.splice($.inArray(item, WapOrderListController.orderList), 1);
        WapOrderListController.orderService.deleteOrder(orderID);
        this.render();
    }

    //渲染数据
    render(): void {
        var html = '';
        var itemHtml = '';

        //order body
        var bodyHtml = '<li data= "true" >';
        bodyHtml += '<div class= "mdf_top">';
        bodyHtml += '<img src="{1}" alt="{0}">';
        bodyHtml += '<div class= "mdf_tnm">订单号： {0}</div>';
        bodyHtml += '<div class="mdf_tprice">¥{2} </div>';
        bodyHtml += '<div class="mdf_time"> {3}&nbsp;{4} </div>';
        bodyHtml += '<a class="mx_more" href= "{6}"> <i class="fa fa-chevron-right"> </i></a>';
        bodyHtml += '</div>';
        bodyHtml += '{5}';
        bodyHtml += '</li>';

        //action
        var actionHtml = '<a class= "mdf_go" href= "javascript: void (0);"> {0} </a>';

        for (var i = 0; i < WapOrderListController.orderList.length; i++) {
            if (WapOrderListController.orderList[i].items.length == 0)
                continue;
            //第一个订单明细的图片
            var imageUrl = WapOrderListController.orderList[i].items[0].thumbUrl;
            //下单时间
            var date = WapOrderListController.orderList[i].orderInfo.timeOrder.split('T');
            //代付款-已付款
            var payStatus = "待付款";
            //根据订单状态，得到操作权限
            if (WapOrderListController.orderList[i].orderInfo.orderStatus == "已完成") {
                actionHtml = '<a class= "mdf_go" href= "javascript: void (0);"> {0} </a>';
                actionHtml = StringUtils.format(actionHtml, "已完成");
                $("#orderStatus").html("已完成订单");
            }
            else if (WapOrderListController.orderList[i].orderInfo.orderStatus == "处理中" && WapOrderListController.orderList[i].orderInfo.paymentStatus == "已支付") {
                actionHtml = '<a class= "mdf_go" href= "javascript: void (0);"> {0} </a>';
                actionHtml = StringUtils.format(actionHtml, "待发货");
                $("#orderStatus").html("待发货订单");
            }
            else if (WapOrderListController.orderList[i].orderInfo.orderStatus == "处理中" && WapOrderListController.orderList[i].orderInfo.paymentStatus == "未支付") {
                if (WapOrderListController.orderList[i].clickString) {
                    actionHtml = '<a class= "mdf_go" href= "javascript: void (0);" onclick="{1}"> {0} </a>{2}';
                    actionHtml = StringUtils.format(actionHtml, "立即付款", WapOrderListController.orderList[i].clickString, WapOrderListController.orderList[i].paymentForm);
                    $("#orderStatus").html("待付款订单");
                }
            }

            html += StringUtils.format(bodyHtml,
                WapOrderListController.orderList[i].orderInfo.orderSN,//订单SN码
                imageUrl,//订单第一个图片
                WapOrderListController.orderList[i].orderInfo.priceTotal,//订单金额
                date[0],
                date[1],
                actionHtml,
                WapOrderItemListController.getRedirectUrl(WapOrderListController.orderList[i].orderInfo.id, WapOrderListController.orderList[i].publishmentSystemInfo.publishmentSystemID));
        }

        //添加
        $("#orderUl").append(html);
    }

    getUrl(isCompleted: string, isPayment: string, orderTime: string, keywords?: string): string {

        this.parmMap['isCompleted'] = isCompleted;

        this.parmMap['isPayment'] = isPayment;

        this.parmMap['orderTime'] = orderTime;

        this.parmMap['keywords'] = keywords;

        return "?" + Utils.mapToUrl(this.parmMap);
    }
}