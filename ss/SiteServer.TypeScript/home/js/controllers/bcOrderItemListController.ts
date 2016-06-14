/// <reference path="baseController.ts" />

/**********
* 订单详情
**********/
class BcOrderItemListController extends baseController {

    public static orderService: OrderService;
    public static orderInfoID: number = UrlUtils.getUrlVar("orderID");
    public static publishmentSystemID: number = UrlUtils.getUrlVar("publishmentSystemID");
    public static orderInfo: any;
    public static orderItemList: any;
    public static orderSN: string;
    public static orderStatus: number = 0;// 0-已下单 1-等待付款 2-等待发货 3-等待收货 4-完成 
    public static orderStatusDescription: string;
    public static urlForComment: string;
    public static paymentType: number;// 0-在线付款  1-货到付款
    public static clickString: string;
    public static isPaymentClick: boolean;
    public static paymentForm: string;
    public isPC: string = Utils.isPC();

    public isEmpty: boolean = true;
    public estimate: Object;
    public loading: boolean = true;

    public parmMap: any = Utils.urlToMap(window.location.href.split('?').length > 1 ? window.location.href.split('?')[1] : "");

    constructor() {
        super();
        BcOrderItemListController.orderService = new OrderService();
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
    }

    //绑定数据
    bindData(): void {
        this.bindOrderItemList();
    }

    //绑定订单数据
    bindOrderItemList(): void {
        BcOrderItemListController.orderService.getOrderItemList(BcOrderItemListController.orderInfoID, BcOrderItemListController.publishmentSystemID,(json) => {
            BcOrderItemListController.orderInfo = json.orderList[0].orderInfo;
            BcOrderItemListController.orderItemList = json.orderItemList;
            BcOrderItemListController.clickString = json.orderList[0].clickString;
            BcOrderItemListController.isPaymentClick = json.orderList[0].isPaymentClick;
            BcOrderItemListController.paymentForm = json.orderList[0].paymentForm;
            //加载数据
            this.render();
            //分页链接
            //$("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getAllOrderItemList'));

            //绑定订单信息
            $(".orderSN").html(BcOrderItemListController.orderSN);//订单SN
            $(".orderStatusDescription").html(BcOrderItemListController.orderStatusDescription);//订单状态
            $(".orderComment").attr("href", BcOrderItemListController.urlForComment);//发表评价

            //设置付款信息
            if (BcOrderItemListController.paymentType == 0) {
                $(".payment").html("在线付款");
            }
            else {
                $(".payment").html("货到付款");
            }
            $(".priceTotal").html(BcOrderItemListController.orderInfo.priceTotal);
            $(".priceShipment").html(BcOrderItemListController.orderInfo.priceShipment);
            $(".priceReturn").html(BcOrderItemListController.orderInfo.priceReturn);
            $(".priceActual").html(BcOrderItemListController.orderInfo.priceActual);
            if (BcOrderItemListController.orderStatus >= 2) {
                $(".paidDate").html(BcOrderItemListController.orderInfo.timePayment.replace("T", " "));
            } else {
                $(".paidDate").html("-- --");
            }

            //设置收货人信息
            baseController.userService.getUserAddressOne(BcOrderItemListController.orderInfo.consigneeID,(json) => {
                if (json.isSuccess && json.consignee) {
                    $(".consignee").html(json.consignee.consignee);
                    $(".address").html(json.consignee.province + " " + json.consignee.city + " " + json.consignee.area + " " + json.consignee.address);
                    $(".mobile").html(json.consignee.mobile);
                }
            });

            //设置配送方式
            baseController.userService.getUserShipmentOne(BcOrderItemListController.orderInfo.shipmentID,(json) => {
                if (json.isSuccess && json.shipment) {
                    $(".shipment").html(json.shipment);
                }
            });

            //设置发票类型
            baseController.userService.getUserInvoicesOne(BcOrderItemListController.orderInfo.invoiceID,(json) => {
                if (json.isSuccess && json.invoice) {
                    $(".invoice").html(this.getInvoiceName(json.invoice));
                }
            });
        });
    }

    getInvoiceName(invoice): string {
        var showInvoice = invoice.isInvoice;
        if (!invoice) {
            showInvoice = false;
        } else {
            showInvoice = true;
        }
        if (showInvoice) {
            if (invoice.isVat) {
                return "增值税发票 " + invoice.vatCompanyName;
            } else {
                return "普通发票 " + (invoice.isCompany ? invoice.companyName : "个人");
            }
        }
        else {
            return "不需要发票"
        }
    }

    //渲染数据
    render(): void {
        var itemHtml = '';

        var itemBodyHtml = '<tr><td>{0}</td>';
        itemBodyHtml += '<td>';
        itemBodyHtml += '<div class= "img-list">';
        itemBodyHtml += '<a class= "img-box" target= "_blank" href= "{3}"> <img width="50" height= "50"src= "{2}" title= "{1}" /> </a> ';
        itemBodyHtml += '</div> </td>';
        itemBodyHtml += '<td>';
        itemBodyHtml += '<div class= "al fl">';
        itemBodyHtml += '<a class= "flk13" target= "_blank" href= "{3}" clstag= "click|keycount|orderinfo|product_name">{1}</a> ';
        itemBodyHtml += '</div>';
        itemBodyHtml += '<div class="clr"> </div> ';
        itemBodyHtml += '<div id= "coupon_{0}" class="fl"> </div> </td>';
        itemBodyHtml += '<td><span class="ftx04"> &yen; {4} </span></td>';
        itemBodyHtml += '<td>{5}</td>';

        //action
        var actionHtml = '<td align="center" rowspan="{0}">';
        //actionHtml += '<a class= "cor_blue" href= "#" > 查看 </a>';
        actionHtml += ' <a class= "cor_blue" onclick= "BcOrderItemListController.prototype.removeOrder({1}); " href= "javascript: void (0); " > 删除 </a >';
        actionHtml += '<br>';
        actionHtml += '<a class= "cor_blue" href= "#" > 评价晒单 </a >';
        actionHtml += '<br>';
        actionHtml += '<a class= "cor_blue" href= "#" style= "display:none;" > 申请返修 / 退换货 </a>';
        actionHtml += '<br><a href="#" class= "bmr_mby" > 还要买 </a >';
        actionHtml += '</td > ';

        var orderSN: string = (<string>BcOrderItemListController.orderInfo["orderSN"]).toLocaleLowerCase();
        BcOrderItemListController.orderSN = orderSN;
        BcOrderItemListController.urlForComment = "javascript:alert('订单未完成，不能评价！');";

        //根据订单状态，得到操作权限
        if (BcOrderItemListController.orderInfo.orderStatus == "Completed") {
            actionHtml = '<td align="center" rowspan="{0}"><a class="cor_blue" style="display:none;" href= "javascript: void (0);" onclick="{1}"> 查看 </a> <br><a class="cor_blue" href= "{2}"> 评价晒单</a> <br><a class="cor_blue" href= "javascript: void (0);"  onclick="{3}" style="display:none;"> 申请返修/退换货</a> <br><a href="{4}" class="bmr_mby"> 还要买 </a></td>';

            var publishmentSystemID = 0;
            if (BcOrderItemListController.orderInfo.publishmentSystemInfo)
                publishmentSystemID = BcOrderItemListController.orderInfo.publishmentSystemInfo.publishmentSystemID;

            actionHtml = StringUtils.format(actionHtml, BcOrderItemListController.orderItemList[orderSN].length, "alert('查看')", BcOrderCommentController.getUrl(BcOrderItemListController.orderInfo.id, publishmentSystemID), "alert('返修退换货')", BcOrderItemListController.orderItemList[orderSN][0].navigationUrl);
            BcOrderItemListController.orderStatusDescription = "已完成";
            BcOrderItemListController.orderStatus = 4;
            BcOrderItemListController.urlForComment = BcOrderCommentController.getUrl(BcOrderItemListController.orderInfo.id, publishmentSystemID);
        }
        else if (BcOrderItemListController.orderInfo.orderStatus == "Handling" && BcOrderItemListController.orderInfo.paymentStatus == "Paid" && BcOrderItemListController.orderInfo.shipmentStatus == "Shipment") {
            actionHtml = '<td align="center" rowspan="{0}"><a class="cor_blue" style="display:none;" href= "javascript: void (0);" onclick="{1}"> 查看 </a> <br><a href="{2}" class="bmr_mby"> 还要买 </a></td>';
            actionHtml = StringUtils.format(actionHtml, BcOrderItemListController.orderItemList[orderSN].length, "alert('查看')", BcOrderItemListController.orderItemList[orderSN][0].navigationUrl);
            BcOrderItemListController.orderStatusDescription = "等待收货";
            BcOrderItemListController.orderStatus = 3;
        }
        else if (BcOrderItemListController.orderInfo.orderStatus == "Handling" && BcOrderItemListController.orderInfo.paymentStatus == "Paid" && BcOrderItemListController.orderInfo.shipmentStatus == "UnShipment") {
            actionHtml = '<td align="center" rowspan="{0}"><a class="cor_blue" style="display:none;" href= "javascript: void (0);" onclick="{1}"> 查看 </a> <br><a href="{2}" class="bmr_mby"> 还要买 </a></td>';
            actionHtml = StringUtils.format(actionHtml, BcOrderItemListController.orderItemList[orderSN].length, "alert('查看')", BcOrderItemListController.orderItemList[orderSN][0].navigationUrl);
            BcOrderItemListController.orderStatusDescription = "等待发货";
            BcOrderItemListController.orderStatus = 2;
        }
        else if (BcOrderItemListController.orderInfo.orderStatus == "Handling" && BcOrderItemListController.orderInfo.paymentStatus == "Unpaid") {
            actionHtml = '<td align="center" rowspan="{0}"><a class="cor_blue" style="display:none;" href= "javascript: void (0);"  onclick="{1}"> 查看 </a> <a class="cor_blue" onclick="{2}" href="javascript: void (0);">删除</a>';
            actionHtml = StringUtils.format(actionHtml, BcOrderItemListController.orderItemList[orderSN].length, "alert('查看')", "BcOrderItemListController.prototype.removeOrder(" + BcOrderItemListController.orderInfo.id + ")");
            if (BcOrderItemListController.isPaymentClick) {
                actionHtml += StringUtils.format('<br><a href="javascript:void (0);" onclick="{0}" class="cor_blue"> 立即付款 </a>{1}', BcOrderItemListController.clickString, BcOrderItemListController.paymentForm);
                BcOrderItemListController.paymentType = 0;
            } else {
                actionHtml += StringUtils.format('<br><a href="javascript:void (0);" class="cor_blue" style="color:red;"> 货到付款 </a>');
                BcOrderItemListController.paymentType = 1;
            }
            actionHtml += StringUtils.format('<br><a href="{0}" class="bmr_mby"> 还要买 </a></td>', BcOrderItemListController.orderItemList[orderSN][0].navigationUrl);
            BcOrderItemListController.orderStatusDescription = "等待支付";
            BcOrderItemListController.orderStatus = 1;
        }
        else if (BcOrderItemListController.orderInfo.orderStatus == "Canceled") {
            actionHtml = StringUtils.format('<td><a href="{0}" class="bmr_mby"> 还要买 </a></td>', BcOrderItemListController.orderItemList[orderSN][0].navigationUrl);
            BcOrderItemListController.orderStatusDescription = "已作废";
            BcOrderItemListController.orderStatus = -1;
        }

        //设置订单流程图
        for (var l = 0; l < BcOrderItemListController.orderStatus; l++) {
            $(".process").find(".ready").eq(l * 2 + 1).css("background-position-y", "0px");
            $(".process").find(".ready").eq(l * 2).css("background-position-y", "0px");
        }
        if (BcOrderItemListController.orderStatus == 4) {
            //完成
            $(".process").find(".ready").eq(4 * 2).css("background-position-y", "0px");
        }

        for (var i = 0; i < BcOrderItemListController.orderItemList[orderSN].length; i++) {
            //第一个订单明细的图片
            var imageUrl = BcOrderItemListController.orderItemList[orderSN][i].thumbUrl;
            //下单时间
            var date = BcOrderItemListController.orderInfo.timeOrder.split('T');

            itemHtml += StringUtils.format(itemBodyHtml,
                BcOrderItemListController.orderItemList[orderSN][i].goodsSN,//商品编号
                BcOrderItemListController.orderItemList[orderSN][i].title,//订单标题
                imageUrl,//订单图片
                BcOrderItemListController.orderItemList[orderSN][i].navigationUrl,//商品链接
                BcOrderItemListController.orderItemList[orderSN][i].priceSale,//商品价格
                BcOrderItemListController.orderItemList[orderSN][i].purchaseNum//商品数量
                );
            if (i == 0)
                itemHtml += actionHtml;
        }
        itemHtml += '</tr>';
        //添加
        $("#tbOrderItem").append(itemHtml);
    }

    //删除订单
    removeOrder(orderID: number): void {
        BcOrderItemListController.orderService.deleteOrder(orderID);
        alert("删除成功！");
        location.href = BcOrderListController.getRedirectUrl();
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