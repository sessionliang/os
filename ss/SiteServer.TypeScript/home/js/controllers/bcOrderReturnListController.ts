/**********
* 订单退换货
**********/
class BcOrderReturnListController extends baseController {

    public static orderService: OrderService;
    public static orderReturnList: any;
    public pageIndex: number = 1;
    public prePageNum: number = 10;
    public isCompleted: string = "true";
    public isPayment: string = "true";
    public isPC: string = Utils.isPC();
    public orderTime: number = 0;
    public keywords: string = StringUtils.empty;

    public isEmpty: boolean = true;
    public estimate: Object;
    public loading: boolean = true;

    public parmMap: any = Utils.urlToMap(window.location.href.split('?').length > 1 ? window.location.href.split('?')[1] : "");

    constructor() {
        super();
        BcOrderReturnListController.orderService = new OrderService();
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
        this.bindOrderReturnList(this.pageIndex, this.prePageNum);
        this.bindOrderTime();
        this.bindOrderKeywords();
    }

    //绑定订单数据
    bindOrderReturnList(pageIndex: number, prePageNum: number): void {
        this.pageIndex = pageIndex || this.pageIndex;
        this.prePageNum = prePageNum || this.prePageNum;
        this.isCompleted = <string>UrlUtils.getUrlVar("isCompleted");
        this.isPayment = <string>UrlUtils.getUrlVar("isPayment");
        this.keywords = <string>UrlUtils.getUrlVar("keywords");
        this.orderTime = <number>UrlUtils.getUrlVar("orderTime");

        BcOrderReturnListController.orderService.getAllOrderList("true", "true", this.isPC, this.orderTime, this.keywords, this.pageIndex, this.prePageNum,(json) => {
            BcOrderReturnListController.orderReturnList = json.orderInfoList;
            this.loading = false;
            if (BcOrderReturnListController.orderReturnList && BcOrderReturnListController.orderReturnList.length > 0) {
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
            $("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getAllOrderReturnList'));
        });
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

    //渲染数据
    render(): void {
        var html;
        //order body
        var bodyHtml = '<tr class= "bmr_mtr3"  data="true">';
        bodyHtml += '<td align="center"><span class= "cor_blue"> {0} </span></td>';

        //action
        var actionHtml = '<td class= "bmr_mtd1">';
        actionHtml += "{2}";
        actionHtml += '</td>';
        actionHtml += '<td class= "cor_999" align= "center">{0} <br> {1}</td>';
        actionHtml += '</tr>';

        var applyHtml = '<div class= "bmrd_pro">';
        applyHtml += '<a href="{0}"><img src="{1}" width= "50" height= "50"> </a>';
        applyHtml += '{2}';
        applyHtml += '</div>';

        for (var i = 0; i < BcOrderReturnListController.orderReturnList.length; i++) {
            var date = BcOrderReturnListController.orderReturnList[i].orderInfo.timeOrder.split('T');
            //订单
            html += StringUtils.format(bodyHtml, BcOrderReturnListController.orderReturnList[i].orderInfo.orderSN);
            html += StringUtils.format(actionHtml, date[0], date[1], '{0}');
            var orderDetail = "";
            for (var j = 0; j < BcOrderReturnListController.orderReturnList[i].items.length; j++) {
                var applyA = '<a href= "{0}" {1} class= "bmr_pa1"> {2} </a>';
                applyA = StringUtils.format(applyA,
                    BcOrderReturnListController.orderReturnList[i].items[j].isApplyReturn ? "javascript:;" : BcReturnApplyController.getUrl(BcOrderReturnListController.orderReturnList[i].items[j].orderItemID, BcOrderReturnListController.orderReturnList[i].publishmentSystemInfo.publishmentSystemID),
                    BcOrderReturnListController.orderReturnList[i].items[j].isApplyReturn ? " style='background:#ccc; ' " : "",
                    BcOrderReturnListController.orderReturnList[i].items[j].isApplyReturn ? "已申请" : "申请");
                //订单详情
                orderDetail += StringUtils.format(applyHtml,
                    BcOrderReturnListController.orderReturnList[i].items[j].navigationUrl,
                    BcOrderReturnListController.orderReturnList[i].items[j].thumbUrl,
                    applyA
                    );
            }
            html = StringUtils.format(html, orderDetail);
            html += "</tr>"
        }
        //添加之前删除原有的tr
        $("tr[data='true']").remove();
        //添加
        $("#orderTab").append(html);
    }

    getUrl(isCompleted: string, isPayment: string, orderTime: string, keywords?: string): string {

        this.parmMap['isCompleted'] = "true";

        this.parmMap['isPayment'] = "true";

        this.parmMap['orderTime'] = orderTime;

        this.parmMap['keywords'] = keywords;

        return "?" + Utils.mapToUrl(this.parmMap);
    }
}