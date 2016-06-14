/**********
* 订单退换货
**********/
class BcOrderItemReturnRecordListController extends baseController {

    public static orderItemReturnService: OrderItemReturnService;
    public static orderItemReturnRecordList: any;
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
        BcOrderItemReturnRecordListController.orderItemReturnService = new OrderItemReturnService();
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
        this.bindOrderItemReturnRecordList(this.pageIndex, this.prePageNum);
        this.bindOrderTime();
        this.bindOrderKeywords();
    }

    //绑定订单数据
    bindOrderItemReturnRecordList(pageIndex: number, prePageNum: number): void {
        this.pageIndex = pageIndex || this.pageIndex;
        this.prePageNum = prePageNum || this.prePageNum;

        this.keywords = <string>UrlUtils.getUrlVar("keywords");
        this.orderTime = <number>UrlUtils.getUrlVar("orderTime");

        BcOrderItemReturnRecordListController.orderItemReturnService.getOrderItemReturnRecordList(this.isPC, this.orderTime, this.keywords, this.pageIndex, this.prePageNum,(json) => {
            BcOrderItemReturnRecordListController.orderItemReturnRecordList = json.orderItemReturnRecordList;
            this.loading = false;
            if (BcOrderItemReturnRecordListController.orderItemReturnRecordList && BcOrderItemReturnRecordListController.orderItemReturnRecordList.length > 0) {
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
            $("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getOrderItemReturnRecordList'));
        });
    }


    //订单时间帅选
    orderTimeChange(): void {
        var time = $("#selTime").val();
        switch (time) {
            case "all":
                window.location.href = this.getUrl("all", UrlUtils.getUrlVar("keywords"));
                break;
            case "90":
                window.location.href = this.getUrl("90", UrlUtils.getUrlVar("keywords"));
                break;
            case "180":
                window.location.href = this.getUrl("180", UrlUtils.getUrlVar("keywords"));
                break;
            case "365":
                window.location.href = this.getUrl("365", UrlUtils.getUrlVar("keywords"));
                break;
        }
    }

    //订单关键字帅选
    btnSearchClick(): void {
        window.location.href = this.getUrl(UrlUtils.getUrlVar("orderTime"), $("#txtKeywords").val());
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
        bodyHtml += '<td align="center" class= "bmr_mtd1"> <span class= "cor_blue"> {1} </span></td>';
        bodyHtml += '<td class= "bmr_mtd1">';
        bodyHtml += '<a class= "cor_blue" href= "{2}">{3}</a>';
        bodyHtml += '</td>';
        bodyHtml += '<td align="center" class= "bmr_mtd1"> {4} </td>';
        bodyHtml += '<td align= "center" class= "bmr_mtd1"> {5} </td>';
        //bodyHtml += '<td class= "cor_999" align= "center">';
        //bodyHtml += '<span class= "cor_blue">';
        //bodyHtml += '<a href="" class= "cor_blue"> 查看 </a>';
        //bodyHtml += '</span>';
        //bodyHtml += '</td>';
        bodyHtml += '</tr>';


        for (var i = 0; i < BcOrderItemReturnRecordListController.orderItemReturnRecordList.length; i++) {
            var date = BcOrderItemReturnRecordListController.orderItemReturnRecordList[i].applyDate.split('T');
            //记录详情
            html += StringUtils.format(bodyHtml,
                BcOrderItemReturnRecordListController.orderItemReturnRecordList[i].id,
                BcOrderItemReturnRecordListController.orderItemReturnRecordList[i].goodsSN,
                BcOrderItemReturnRecordListController.orderItemReturnRecordList[i].navigationUrl,
                BcOrderItemReturnRecordListController.orderItemReturnRecordList[i].title,
                date[0],
                BcOrderItemReturnRecordListController.orderItemReturnRecordList[i].detailStatus);
        }
        //添加之前删除原有的tr
        $("tr[data='true']").remove();
        //添加
        $("#returnRecordTab").append(html);
    }

    getUrl(orderTime: string, keywords?: string): string {

        this.parmMap['orderTime'] = orderTime;

        this.parmMap['keywords'] = keywords;

        return "?" + Utils.mapToUrl(this.parmMap);
    }
}