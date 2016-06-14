/**********
* 订单
**********/
class BcSuggestionController extends baseController {

    public static orderService: OrderService;
    public static orderList: any;
    public pageIndex: number = 1;
    public prePageNum: number = 10;
    public isPC: string = Utils.isPC();
    public parmMap: any = Utils.urlToMap(window.location.href.split('?').length > 1 ? window.location.href.split('?')[1] : "");
    public keywords: string = StringUtils.empty;

    constructor() {
        super();
        BcOrderListController.orderService = new OrderService();
    }

    //初始化页面元素以及事件
    init(): void {
        baseController.userAuthValidate(() => {
            this.bindData();
            this.bindEvent();
        });
    }


    bindEvent(): void {

        if ($("#btnSearch").length > 0) {
            $("#btnSearch").click(() => {
                this.btnSearchClick();
            });
        }
    }


    btnSearchClick(): void {
        this.parmMap['keywords'] = $("#txtKeywords").val();
        window.location.href = "?" + Utils.mapToUrl(this.parmMap);
    }
    //绑定数据
    bindData(): void {
        this.bindOrderList(this.pageIndex, this.prePageNum);
    }

    //绑定订单数据
    bindOrderList(pageIndex: number, prePageNum: number): void {
        this.pageIndex = pageIndex || this.pageIndex;
        this.prePageNum = prePageNum || this.prePageNum;
        this.keywords = <string>UrlUtils.getUrlVar("keywords");
        if ($("#txtKeywords").length > 0) {
            $("#txtKeywords").val(this.keywords);
        }

        BcOrderListController.orderService.getAllOrderListWithSiteInfo("", "", this.isPC, 0, this.keywords, this.pageIndex, this.prePageNum,(json) => {
            BcOrderListController.orderList = json.orderInfoList;
            //加载数据
            this.render();
            //分页链接
            $("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getAllOrderList1'));
        });


    }
    
    
    //渲染数据
    render(): void {
        var innerHtml = '';
        var htmlTemplate = '';
        var itemTemplate = '';

        htmlTemplate += '<tr class="bmr_mtr3" data="true">';
        htmlTemplate += '    <td align="center"><span class="cor_blue">{0}</span></td>';
        htmlTemplate += '    <td class="bmr_mtd1">';
        htmlTemplate += '{1}';
        itemTemplate += '        <div class="bmrd_pro">';
        itemTemplate += '            <a href="{0}"><img src="{1}" width="50" height="50"></a>';
        itemTemplate += '        </div>';
        htmlTemplate += '    </td>';
        htmlTemplate += '    <td class="cor_999 bmr_mtd1" align="center">{2}</td>';
        htmlTemplate += '    <td class="bmrd_pa1" align="center"><a href="javascript:new BcSuggestionController().openNewWindow(\'{3}\');" class="bmr_pa1">意见建议</a></td>';
        htmlTemplate += '</tr>';



        for (var i = 0; i < BcOrderListController.orderList.length; i++) {

            var itemHtml = '';
            for (var j = 0; j < BcOrderListController.orderList[i].items.length; j++) {
                itemHtml += StringUtils.format(itemTemplate,
                    BcOrderListController.orderList[i].items[j].navigationUrl,
                    BcOrderListController.orderList[i].items[j].thumbUrl);
            }
            innerHtml += StringUtils.format(htmlTemplate,
                BcOrderListController.orderList[i].orderInfo.orderSN,
                itemHtml,
                BcOrderListController.orderList[i].orderInfo.timeOrder.replace('T', '<br/>'),
                BcOrderListController.orderList[i].qiaoUrl);

        }
        //添加之前删除原有的tr
        $("tr[data='true']").remove();
        //添加
        $("#orderTab").append(innerHtml);
    }



    openNewWindow(url: string): boolean {

        var w = window.open();
        setTimeout(function () {
            w.location.replace(url);
        }, 500);

        return false;
    }

}