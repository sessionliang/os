/**********
* 订单退货表单
**********/
class BcReturnApplyController extends baseController {

    public static orderItemReturnService: OrderItemReturnService;
    public orderService: OrderService;
    public orderItemInfo: any;
    public orderInfo: any;
    public publishmentSystemInfo: any;

    public static orderItemID: any = UrlUtils.getUrlVar("orderItemID");
    public static publishmentSystemID: any = UrlUtils.getUrlVar("publishmentSystemID");
    public static mobile: string = "";
    public static purchaseNum: number = 1;
    public static imageNum: number = 0;

    public isPC: string = Utils.isPC();

    public static parmMap: any = Utils.urlToMap(window.location.href.split('?').length > 1 ? window.location.href.split('?')[1] : "");

    constructor() {
        super();
        this.orderService = new OrderService();
        BcReturnApplyController.orderItemReturnService = new OrderItemReturnService();
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


        this.registClickSave();


        //提交数量 - 
        $("#downCount").click(() => {
            var num = $("#purchaseNum").val();
            if (num > 1)
                num--;
            $("#purchaseNum").val(num);
        });
        //提交数量 + 
        $("#upCount").click(() => {
            var num = $("#purchaseNum").val();
            if (num < this.orderItemInfo.purchaseNum)
                num++;
            $("#purchaseNum").val(num);
        });

        //上传图片
        Utils.fileUpload('fileupload', BcReturnApplyController.orderItemReturnService.getUploadImgUrl('UploadReturnImage', BcReturnApplyController.orderItemID, BcReturnApplyController.publishmentSystemID),(data) => {
            if (data.isSuccess) {
                var imgHtml = "";
                imgHtml += '<li>';
                imgHtml += '<a href="{0}" target= "_blank">';
                imgHtml += '<img class= "err-product" width= "50" height= "50" src= "{0}" data-img="1">';
                imgHtml += '</a>';
                imgHtml += '<b>×</b>';
                imgHtml += '<input type="hidden" value="{0}" />';
                imgHtml += '</li>';
                imgHtml = StringUtils.format(imgHtml, data.imageUrl);
                $("#imagePanel").append(imgHtml);

                BcReturnApplyController.imageNum++;

                $("#imagePanel").find("li").unbind("hover");
                $("#imagePanel").find("li").hover(function () {
                    $(this).addClass("hover");
                }, function () {
                        $(this).removeClass("hover");
                    });

                $("#imagePanel").find("b").unbind("click");
                $("#imagePanel").find("b").click(function () {
                    $(this).parent("li").remove();
                    BcReturnApplyController.imageNum--;
                });
            }
        });
        $("#btnUpload").click(function () {
            if (BcReturnApplyController.imageNum >= 5) {
                Utils.tipAlert(false, "最多可以上传5张图片！");
                return;
            }
            $("#fileupload").click();
        });

        //手机号码
        $("#samePhone").change(function () {
            if ($(this).is(":checked")) {
                $("#contactPhone").val(BcReturnApplyController.mobile);
                $("#contactPhone").attr("disabled", "disabled");
            }
            else {
                $("#contactPhone").removeAttr("disabled");
            }
        });

    }

    //绑定数据
    bindData(): void {

        //服务类型
        var attrs = new _map;
        attrs.set("name", "returnType");
        eOrderItemReturnTypeUtils.addItemsToEle($("#returnTypePanle").get(0), "radio", "bmra_rad", attrs);
        $("input:radio[name='returnType']").eq(eOrderItemReturnType.Return).attr("checked", "checked");

        this.bindOrderItemInfo();


    }

    //绑定订单详情数据
    bindOrderItemInfo(): void {
        this.orderService.getOrderItem(BcReturnApplyController.orderItemID, BcReturnApplyController.publishmentSystemID,(json) => {
            this.orderItemInfo = json.orderItemInfo;
            this.publishmentSystemInfo = json.publishmentSystemInfo;
            this.orderInfo = json.orderInfo;
            BcReturnApplyController.mobile = json.orderInfo.consignee.mobile;
            BcReturnApplyController.purchaseNum = json.orderItemInfo.purchaseNum;
            this.render();
        });
    }

    //渲染数据
    render(): void {
        var html;
        //order body
        var bodyHtml = '<tr class= "bmr_mtr3"  data="true">';
        bodyHtml += '<td align="left">';
        bodyHtml += '<a href= "{0}" class= "cor_blue">';
        bodyHtml += '<img src="{1}" width= "50" height= "50"> <br>{2}';
        bodyHtml += '</a></td> ';
        bodyHtml += '<td align="center" class= "bmr_mtd1">';
        bodyHtml += '{3}';
        bodyHtml += '</td>';
        bodyHtml += '<td class= "bmr_mtd1" align= "center">{4}</td>';
        bodyHtml += '<td align= "center">{5}</td>';
        bodyHtml += '</tr>';

        html = StringUtils.format(bodyHtml,
            this.orderItemInfo.navigationUrl,
            this.orderItemInfo.thumbUrl,
            this.orderItemInfo.title,
            this.orderItemInfo.purchaseNum,
            this.orderItemInfo.priceSale,
            ((<number>this.orderItemInfo.priceSale) * (<number>this.orderItemInfo.purchaseNum)).toString());

        //添加之前删除原有的tr
        $("tr[data='true']").remove();
        //添加
        $("#orderItemTab").append(html);

        //卖家
        $("#publishmentSystemName").html(this.publishmentSystemInfo.publishmentSystemName);

        //买家
        $("#contact").val(this.orderInfo.consignee.userName);
        $("#contactPhone").val(this.orderInfo.consignee.mobile);
    }

    //保存
    registClickSave(): void {
        $("#submitReturn").click(function (e) {

            var returnType = $("input:radio[name='returnType']:checked").val();
            var returnCount = $("#purchaseNum").val();

            var inspectReport = $("#inspectReport").is(":checked");
            var description = $("#description").val();
            var images = "";
            $("#imagePanel").find("input[type='hidden']").map(function (index, ele) {
                if (ele.getAttribute("value"))
                    images += ele.getAttribute("value") + ",";
            });
            var contact = $("#contact").val();
            var contactPhone = $("#contactPhone").val();
            if (returnCount < 0 || returnCount > BcReturnApplyController.purchaseNum) {
                Utils.tipAlert(false, "退货数量不正确！");
                return;
            }
            if (!description) {
                Utils.tipAlert(false, "问题描述必须填写！");
                return;
            }
            if (!contact) {
                Utils.tipAlert(false, "联系人姓名必须填写！");
                return;
            }
            if (!contactPhone) {
                Utils.tipAlert(false, "手机号码必须填写！");
                return;
            }

            BcReturnApplyController.orderItemReturnService.saveOrderItemReturn(BcReturnApplyController.orderItemID, BcReturnApplyController.publishmentSystemID, returnType, returnCount, inspectReport, description, images, contact, contactPhone);
        });
    }

    //获取评价订单详情地址
    static getUrl(orderItemID: number, publishmentSystemID: number): string {
        this.parmMap['orderItemID'] = orderItemID;
        this.parmMap['publishmentSystemID'] = publishmentSystemID;
        return "returnApply.html?" + Utils.mapToUrl(this.parmMap);
    }
}