/**********
* 订单
**********/
class BcOrderListController extends baseController {

    public static orderService: OrderService;
    public static orderList: any;
    public pageIndex: number = 1;
    public prePageNum: number = 10;
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
        BcOrderListController.orderService = new OrderService();
    }

    //初始化页面元素以及事件
    init(): void {
        baseController.userAuthValidate(() => {
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
        this.bindOrderList(this.pageIndex, this.prePageNum);
        this.bindOrderStatus();
        this.bindOrderTime();
        this.bindOrderKeywords();
    }

    //绑定订单数据
    bindOrderList(pageIndex: number, prePageNum: number): void {
        this.pageIndex = pageIndex || this.pageIndex;
        this.prePageNum = prePageNum || this.prePageNum;
        this.isCompleted = <string>UrlUtils.getUrlVar("isCompleted");
        this.isPayment = <string>UrlUtils.getUrlVar("isPayment");
        this.keywords = <string>UrlUtils.getUrlVar("keywords");
        this.orderTime = <number>UrlUtils.getUrlVar("orderTime");

        BcOrderListController.orderService.getAllOrderList(this.isCompleted, this.isPayment, this.isPC, this.orderTime, this.keywords, this.pageIndex, this.prePageNum,(json) => {
            BcOrderListController.orderList = json.orderInfoList;
            this.loading = false;
            if (BcOrderListController.orderList && BcOrderListController.orderList.length > 0) {
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
            $("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getAllOrderList'));
        });
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
        for (var i = 0; i < BcOrderListController.orderList.length; i++) {
            if (BcOrderListController.orderList[i].orderInfo.id == orderID) {
                item = BcOrderListController.orderList[i];
                break;
            }
        }
        BcOrderListController.orderList.splice($.inArray(item, BcOrderListController.orderList), 1);
        BcOrderListController.orderService.deleteOrder(orderID);
        this.render();
    }

    //渲染数据
    render(): void {
        var html;
        //order head
        var headHtml = '<tr class= "bmr_mtr2" data="true">';
        headHtml += '<td colspan="6" >';
        headHtml += '<span class= "bmrc_s2" > 订单编号: <a href="{3}" class="cor_blue" > {0}</a></span >';
        //headHtml += '<span class= "bmrc_s3" >';
        //headHtml += '<a href="{2}" class= "fl cor_blue" > {1}</a>';
        //headHtml += '</span > <a href="#" class= "bmrc_kf" > </a> <span class= "bmrc_tel" >';
        headHtml += '</span> </td> </tr> ';
        //order body
        var bodyHtml = '<tr class= "bmr_mtr3" data="true">';
        bodyHtml += '<td class= "bmr_mtd1" >';
        bodyHtml += '<a class= "bmr_mp1" href= "{0}" >';
        bodyHtml += '<img src="{1}" width= "50" height= "50" > </a>';
        bodyHtml += '</td>';
        bodyHtml += '<td align="center" > {2} </td>';
        bodyHtml += '<td class= "bmr_mtd1" align= "center" >￥{3} <br>在线支付 </td >';
        bodyHtml += '<td class= "bmr_mtd1 cor_999" align= "center" > {4}<br /> {5} </td>';
        bodyHtml += '<td class= "bmr_mtd1" align= "center" > <span class= "cor_999" > {6} </span > </td > ';
        //action
        var actionHtml = '<td align="center" rowspan="{0}">';
        //actionHtml += '<a class= "cor_blue" href= "#" > 查看 </a>';
        actionHtml += ' | <a class= "cor_blue" onclick= "BcOrderListController.prototype.removeOrder({1}); " href= "javascript: void (0); " > 删除 </a >';
        actionHtml += '<br>';
        actionHtml += '<a class= "cor_blue" href= "#" > 评价晒单 </a >';
        actionHtml += '<br>';
        actionHtml += '<a class= "cor_blue" href= "#" style= "display:none;" > 申请返修 / 退换货 </a>';
        actionHtml += '<br><a href="#" class= "bmr_mby" > 还要买 </a >';
        actionHtml += '</td > ';
        for (var i = 0; i < BcOrderListController.orderList.length; i++) {
            html += StringUtils.format(headHtml, BcOrderListController.orderList[i].orderInfo.orderSN, BcOrderListController.orderList[i].publishmentSystemInfo.publishmentSystemName,
                BcOrderListController.orderList[i].publishmentSystemInfo.publishmentSystemUrl,
                BcOrderItemListController.getRedirectUrl(BcOrderListController.orderList[i].orderInfo.id, BcOrderListController.orderList[i].publishmentSystemInfo.publishmentSystemID));

            var orderDetailStatus = "";
            if (BcOrderListController.orderList[i].items.length == 0)
                continue;
            //根据订单状态，得到操作权限
            if (BcOrderListController.orderList[i].orderInfo.orderStatus == "已完成") {
                actionHtml = '<td align="center" rowspan="{0}"><a class="cor_blue" style="display:none;" href= "javascript: void (0);" onclick="{1}"> 查看 </a> <br><a class="cor_blue" href= "{2}"> 评价晒单</a> <br><a class="cor_blue" href= "javascript: void (0);"  onclick="{3}" style="display:none;"> 申请返修/退换货</a> <br><a href="{4}" class="bmr_mby"> 还要买 </a></td>';
                actionHtml = StringUtils.format(actionHtml, BcOrderListController.orderList[i].items.length, "alert('查看')", BcOrderCommentController.getUrl(BcOrderListController.orderList[i].orderInfo.id, BcOrderListController.orderList[i].publishmentSystemInfo.publishmentSystemID), "alert('返修退换货')", BcOrderListController.orderList[i].items[0].navigationUrl);
                orderDetailStatus = "已完成";
            }
            else if (BcOrderListController.orderList[i].orderInfo.orderStatus == "处理中" && BcOrderListController.orderList[i].orderInfo.paymentStatus == "已支付") {
                actionHtml = '<td align="center" rowspan="{0}"><a class="cor_blue" style="display:none;" href= "javascript: void (0);" onclick="{1}"> 查看 </a> <br><a href="{2}" class="bmr_mby"> 还要买 </a></td>';
                actionHtml = StringUtils.format(actionHtml, BcOrderListController.orderList[i].items.length, "alert('查看')", BcOrderListController.orderList[i].items[0].navigationUrl);
                orderDetailStatus = "等待发货";
            }
            else if (BcOrderListController.orderList[i].orderInfo.orderStatus == "处理中" && BcOrderListController.orderList[i].orderInfo.paymentStatus == "未支付") {
                actionHtml = '<td align="center" rowspan="{0}"><a class="cor_blue" style="display:none;" href= "javascript: void (0);"  onclick="{1}"> 查看 </a> <a class="cor_blue" onclick="{2}" href="javascript: void (0);">删除</a>';
                actionHtml = StringUtils.format(actionHtml, BcOrderListController.orderList[i].items.length, "alert('查看')", "BcOrderListController.prototype.removeOrder(" + BcOrderListController.orderList[i].orderInfo.id + ")");
                if (BcOrderListController.orderList[i].clickString) {
                    actionHtml += StringUtils.format('<br><a href="javascript:void (0);" onclick="{0}" class="cor_blue"> 立即付款 </a>{1}', BcOrderListController.orderList[i].clickString, BcOrderListController.orderList[i].paymentForm);
                } else {
                    actionHtml += StringUtils.format('<br><a href="javascript:void (0);" class="cor_blue" style="color:red;"> 货到付款 </a>');
                }
                actionHtml += StringUtils.format('<br><a href="{0}" class="bmr_mby"> 还要买 </a></td>', BcOrderListController.orderList[i].items[0].navigationUrl);
                orderDetailStatus = "等待支付";
            }
            else if (BcOrderListController.orderList[i].orderInfo.orderStatus == "已作废") {
                actionHtml = StringUtils.format('<td><a href="{0}" class="bmr_mby"> 还要买 </a></td>', BcOrderListController.orderList[i].items[0].navigationUrl);
                orderDetailStatus = "已作废";
            }
            for (var j = 0; j < BcOrderListController.orderList[i].items.length; j++) {
                var date = BcOrderListController.orderList[i].orderInfo.timeOrder.split('T');
                html += StringUtils.format(bodyHtml, BcOrderListController.orderList[i].items[j].navigationUrl, BcOrderListController.orderList[i].items[j].thumbUrl, BcOrderListController.orderList[i].orderInfo.userName, BcOrderListController.orderList[i].items[j].priceSale, date[0], date[1], orderDetailStatus);
                if (j == 0) {
                    html += actionHtml;
                }
                html += "</tr>"
            }
        }
        //添加之前删除原有的tr
        $("tr[data='true']").remove();
        //添加
        $("#orderTab").append(html);
    }

    getUrl(isCompleted: string, isPayment: string, orderTime: string, keywords?: string): string {

        this.parmMap['isCompleted'] = isCompleted;

        this.parmMap['isPayment'] = isPayment;

        this.parmMap['orderTime'] = orderTime;

        this.parmMap['keywords'] = keywords;

        return "?" + Utils.mapToUrl(this.parmMap);
    }

    static getRedirectUrl(): string {

        return "myOrder.html";
    }
}