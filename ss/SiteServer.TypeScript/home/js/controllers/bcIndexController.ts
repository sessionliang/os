/**********
* 商城用户中心首页
**********/
class BcIndexController extends baseController {

    public static orderService: OrderService;
    public static followService: BcFollowController;
    public static orderList: any;

    public pageIndex: number = 1;
    public prePageNum: number = 10;
    public isCompleted: string = "";
    public isPayment: string = "";

    public isEmpty: boolean = true;
    public estimate: Object;
    public loading: boolean = true;
    public isPC: string = Utils.isPC();
    public parmMap: any = Utils.urlToMap(window.location.href.split('?').length > 1 ? window.location.href.split('?')[1] : "");

    constructor() {
        super();
        BcIndexController.orderService = new OrderService();
    }

    //初始化页面元素以及事件
    init(): void {
        baseController.userAuthValidate(() => {
            this.bindData();
            this.bindEvent();
        });
    }


    //绑定事件
    bindEvent(): void {
        if ($("#selStatus").length > 0) {
            $("#selStatus").change(() => {
                this.orderStatusChange();
            });
        }
    }

    //绑定数据
    bindData(): void {
        //this.bindOrderList(1, 1);//加载最近的一个订单
        this.bindOrderList(this.pageIndex, this.prePageNum);//加载订单列表
        this.bindOrderStatus();
        $("#userAvatar").attr("src", super.getUser()["avatarMiddle"]);
        $("#userName").html(super.getUser()["userName"]);
        super.getUserService().accountSafeLevel((json) => {
            if (json.isSuccess) {
                if (json.level == 1) {
                    $("#accountSafeLevel").html("低");
                    $("#accountSafePercent").attr("style", "width:30%");
                }
                if (json.level == 2) {
                    $("#accountSafeLevel").html("中");
                    $("#accountSafePercent").attr("style", "width:60%");
                }
                if (json.level == 3) {
                    $("#accountSafeLevel").html("高");
                    $("#accountSafePercent").attr("style", "width:90%");
                }
            }
        });

        //统计信息
        BcIndexController.orderService.getOrderStatistic((json) => {
            $("#noPay").html(json.noPay);
            $("#noCompleted").html(json.noCompleted);
            $("#total").html(json.total);
            $("#noComment").html(json.noComment);
        });

        //关注商品
        super.getUserService().getUserFollows(1, 20,(json) => {
            var html = '';
            var htmlTemplate = StringUtils.format('<li>');
            htmlTemplate += StringUtils.format('<a href="{0}"><img src="{1}" width="130" height="130"> </a>');
            htmlTemplate += StringUtils.format('<div class="bmr_ixbms1 bmr_ixbms1a" >￥{2} </div>');//<span class= "bmr_bmtag1" > 直降 < /span>
            // htmlTemplate += StringUtils.format('< div class= "bmr_ixbms2" > <span class= "bmr_bmtag2" > 直降￥118.00</span></div>');
            htmlTemplate += StringUtils.format('</li>');
            for (var i = 0; i < json.followList.length; i++) {
                html += StringUtils.format(htmlTemplate,
                    json.followList[i].navigationUrl,
                    json.followList[i].imageUrl,
                    json.followList[i].goodsPrice);
            }
            $("#followList").html(html);
        });

    }

    //绑定订单数据
    bindOrderList(pageIndex: number, prePageNum: number): void {

        this.isCompleted = <string>UrlUtils.getUrlVar("isCompleted");
        this.isPayment = <string>UrlUtils.getUrlVar("isPayment");

        //BcIndexController.orderService.getLatestOrderInAll(this.isPC,(json) => {
        BcIndexController.orderService.getAllOrderList(this.isCompleted, this.isPayment, this.isPC, 0, '', this.pageIndex, this.prePageNum,(json) => {
            BcIndexController.orderList = json.orderInfoList;
            this.loading = false;
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

    //删除订单
    removeOrder(orderID: number): void {
        var item = {};
        for (var i = 0; i < BcIndexController.orderList.length; i++) {
            if (BcIndexController.orderList[i].orderInfo.id == orderID) {
                item = BcIndexController.orderList[i];
                break;
            }
        }
        BcIndexController.orderList.splice($.inArray(item, BcIndexController.orderList), 1);
        BcIndexController.orderService.deleteOrder(orderID);
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
        actionHtml += ' | <a class= "cor_blue" onclick= "BcIndexController.prototype.removeOrder({1}); " href= "javascript: void (0); " > 删除 </a >';
        actionHtml += '<br>';
        actionHtml += '<a class= "cor_blue" href= "#" > 评价晒单 </a >';
        actionHtml += '<br>';
        actionHtml += '<a class= "cor_blue" href= "#" style= "display:none;" > 申请返修 / 退换货 </a>';
        actionHtml += '<br><a href="#" class= "bmr_mby" > 还要买 </a >';
        actionHtml += '</td > ';
        for (var i = 0; i < BcIndexController.orderList.length; i++) {
            html += StringUtils.format(headHtml, BcIndexController.orderList[i].orderInfo.orderSN, BcIndexController.orderList[i].publishmentSystemInfo.publishmentSystemName, BcIndexController.orderList[i].publishmentSystemInfo.publishmentSystemUrl,
                BcOrderItemListController.getRedirectUrl(BcIndexController.orderList[i].orderInfo.id, BcIndexController.orderList[i].publishmentSystemInfo.publishmentSystemID));

            var orderDetailStatus = "";
            if (BcIndexController.orderList[i].items.length == 0)
                continue;
            //根据订单状态，得到操作权限
            if (BcIndexController.orderList[i].orderInfo.orderStatus == "已完成") {
                actionHtml = '<td align="center" rowspan="{0}"><a class="cor_blue" href= "javascript: void (0);" onclick="{1}"  style="display:none;"> 查看 </a> <br><a class="cor_blue" href= "{2}"> 评价晒单</a> <br><a class="cor_blue" href= "javascript: void (0);"  onclick="{3}" style="display:none;"> 申请返修/退换货</a> <br><a href="{4}" class="bmr_mby"> 还要买 </a></td>';
                actionHtml = StringUtils.format(actionHtml, BcIndexController.orderList[i].items.length, "alert('查看')", BcOrderCommentController.getUrl(BcIndexController.orderList[i].orderInfo.id, BcIndexController.orderList[i].publishmentSystemInfo.publishmentSystemID), "alert('返修退换货')", BcIndexController.orderList[i].items[0].navigationUrl);
                orderDetailStatus = "已完成";
            }
            else if (BcIndexController.orderList[i].orderInfo.orderStatus == "处理中" && BcIndexController.orderList[i].orderInfo.paymentStatus == "已支付") {
                actionHtml = '<td align="center" rowspan="{0}"><a class="cor_blue" href= "javascript: void (0);" onclick="{1}"  style="display:none;"> 查看 </a> <br><a href="{2}" class="bmr_mby"> 还要买 </a></td>';
                actionHtml = StringUtils.format(actionHtml, BcIndexController.orderList[i].items.length, "alert('查看')", BcIndexController.orderList[i].items[0].navigationUrl);
                orderDetailStatus = "等待发货";
            }
            else if (BcIndexController.orderList[i].orderInfo.orderStatus == "处理中" && BcIndexController.orderList[i].orderInfo.paymentStatus == "未支付") {
                actionHtml = '<td align="center" rowspan="{0}"><a class="cor_blue" href= "javascript: void (0);"  onclick="{1}"  style="display:none;"> 查看 </a> <a class="cor_blue" onclick="{2}" href="javascript: void (0);">删除</a>';
                actionHtml = StringUtils.format(actionHtml, BcIndexController.orderList[i].items.length, "alert('查看')", "BcIndexController.prototype.removeOrder(" + BcIndexController.orderList[i].orderInfo.id + ")");
                if (BcIndexController.orderList[i].clickString) {
                    actionHtml += StringUtils.format('<br><a href="javascript:void (0);" onclick="{0}" class="cor_blue"> 立即付款 </a>{1}', BcIndexController.orderList[i].clickString, BcIndexController.orderList[i].paymentForm);
                }
                else {
                    actionHtml += StringUtils.format('<br><a href="javascript:void (0);" class="cor_blue" style="color:red;"> 货到付款 </a>');
                }
                actionHtml += StringUtils.format('<br><a href="{0}" class="bmr_mby"> 还要买 </a></td>', BcIndexController.orderList[i].items[0].navigationUrl);
                orderDetailStatus = "等待支付";
            }
            else if (BcOrderListController.orderList[i].orderInfo.orderStatus == "已作废") {
                actionHtml = StringUtils.format('<td><a href="{0}" class="bmr_mby"> 还要买 </a></td>', BcOrderListController.orderList[i].items[0].navigationUrl);
                orderDetailStatus = "已作废";
            }
            for (var j = 0; j < BcIndexController.orderList[i].items.length; j++) {
                var date = BcIndexController.orderList[i].orderInfo.timeOrder.split('T');
                html += StringUtils.format(bodyHtml, BcIndexController.orderList[i].items[j].navigationUrl, BcIndexController.orderList[i].items[j].thumbUrl, BcIndexController.orderList[i].orderInfo.userName, BcIndexController.orderList[i].items[j].priceSale, date[0], date[1], orderDetailStatus);
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

        return "?" + Utils.mapToUrl(this.parmMap);
    }
}