/**********
* 订单评价
**********/
class BcOrderCommentController extends baseController {

    public static orderService: OrderService;
    public static orderInfoID: number = UrlUtils.getUrlVar("orderID");
    public static publishmentSystemID: number = UrlUtils.getUrlVar("publishmentSystemID");
    //public static orderInfo: any;
    public static orderItemList: any;
    public static orderList: any;

    public pageIndex: number = 1;
    public prePageNum: number = 10;

    public isPC: string = Utils.isPC();

    public static parmMap: any = Utils.urlToMap(window.location.href.split('?').length > 1 ? window.location.href.split('?')[1] : "");

    constructor() {
        super();
        BcOrderCommentController.orderService = new OrderService();
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
        this.bindOrderItemList(this.pageIndex, this.prePageNum);
    }

    //绑定订单详情数据
    bindOrderItemList(pageIndex: number, prePageNum: number): void {
        this.pageIndex = pageIndex || this.pageIndex;
        this.prePageNum = prePageNum || this.prePageNum;
        if (BcOrderCommentController.orderInfoID > 0)
            BcOrderCommentController.orderService.getOrderItemList(BcOrderCommentController.orderInfoID, BcOrderCommentController.publishmentSystemID,(json) => {
                BcOrderCommentController.orderList = json.orderList;
                BcOrderCommentController.orderItemList = json.orderItemList;
                this.render();
                //分页链接
                $("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getOrderItemList'));
            });
        else
            BcOrderCommentController.orderService.getAllOrderItemList(this.pageIndex, this.prePageNum,(json) => {
                BcOrderCommentController.orderList = json.orderList;
                BcOrderCommentController.orderItemList = json.orderItemList;
                this.render();
                //分页链接
                $("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getOrderItemList'));
            });
    }

    //渲染数据
    render(): void {
        var html = "";
        //orderItem
        var orderItemHtml = '<div data="true" class="bmrp_d2">';
        orderItemHtml += '<div class="bmrp_d2Top clearfix">';
        orderItemHtml += '<a class= "bmrp_pimg" href= "{0}" > <img src="{1}" width= "50" height= "50" > </a><a href= "{0}" class= "bmrp_pname cor_blue" > {2} </a>';
        orderItemHtml += '<span class= "bmrp_stime" > {3} </span><span class= "bmrp_ds3" >';
        orderItemHtml += '<a href="javascript:void(0);" class= "bmrp_tkBtn cor_blue" orderItemID= "{4}" orderSN= "{6}" commentClick= "true" > {5} </a></span > </div>';
        //orderComment
        var orderCommentHtml = '<div class="bmrp_dTk clearfix" id="comment_{0}" style="display:none;">';
        orderCommentHtml += '<i class= "bmrp_ico1" > </i>';
        orderCommentHtml += '<div class= "bmrp_ssd1" orderItemID= "{0}" orderSN= "{3}" starPanel= "true" id= "starPanel_{0}" >';
        orderCommentHtml += '<span class= "bmrp_s3" >';
        orderCommentHtml += '<strong class= "cor_red" >*</strong> 评分：</span>';
        orderCommentHtml += '<i class= "bmrp_xx bmrp_cutxx" > </i>';
        orderCommentHtml += '<i class= "bmrp_xx" > </i>';
        orderCommentHtml += '<i class= "bmrp_xx" > </i>';
        orderCommentHtml += '<i class= "bmrp_xx" > </i>';
        orderCommentHtml += '<i class= "bmrp_xx" > </i>';
        orderCommentHtml += '</div>';
        orderCommentHtml += '<div class= "bmrp_ssd2 clearfix" >';
        orderCommentHtml += '<span class= "bmrp_s3" >';
        orderCommentHtml += '<strong class= "cor_red" >*</strong> 标签：</span>';
        orderCommentHtml += '<div class= "bmrp_tagBox" tagPanel= "true" orderItemID="{0}" orderSN= "{3}" id= "tagPanel_{0}" >';
        orderCommentHtml += '<input type="text" maxlength= "12" style= "width:80px;height:28px;margin-right:2px;border:2px solid #CC0000;float:left;display:none;" orderItemID= "{0}" orderSN= "{3}" extTag= "true" id= "extTag_{0}" />';
        orderCommentHtml += '<a href= "javascript:void(0)" class= "bmrp_a2" style= "display:inline-block;" > 自定义 </a>';
        orderCommentHtml += '</div >';
        orderCommentHtml += '</div>';
        orderCommentHtml += '<div class= "bmrp_ssd3 clearfix" >';
        orderCommentHtml += '<span class= "bmrp_s3" >';
        orderCommentHtml += '<strong class= "cor_red" >*</strong> 心得：</span>';
        orderCommentHtml += '<textarea id="txt_{0}" class= "bmrp_area" name= "" cols= "" rows= "" placeholder= "商品是否给力？快分享你的购买心得吧~" > </textarea>';
        orderCommentHtml += '</div> <div class= "bmrp_ssd4" > 10 - 500字</div>';

        //晒单图片
        orderCommentHtml += '<div class= "bmrp_ssd3 clearfix">';
        orderCommentHtml += '<span class= "bmrp_s3">';
        orderCommentHtml += '<strong class= "cor_red">*</strong> 晒单：</span>';
        orderCommentHtml += '<div class= "bmrf_txt">';
        orderCommentHtml += '<a href="javascript:;" id= "btnUpload_{0}" class="btnUpload"> <img src="images/upImg.jpg" width= "73" height= "25"> </a>';
        orderCommentHtml += '<input id= "fileupload_{0}" type= "file" name= "file" style= "display:none;" />';
        orderCommentHtml += '<div class= "order_return_img">';
        orderCommentHtml += '<ul id="imagePanel_{0}">';
        orderCommentHtml += '</ul>';
        orderCommentHtml += '</div>';
        orderCommentHtml += '<br><div style="clear:both;">为了帮助我们更好的解决问题，请您上传图片<br>';
        orderCommentHtml += '<span class="cor_999">最多可上传5张图片，每张图片大小不超过5M，支持bmp, gif, jpg, png, jpeg格式文件</span></div>';
        orderCommentHtml += '</div>';
        orderCommentHtml += '</div>';

        orderCommentHtml += '<div class= "bmrp_ssd3 clearfix" >';
        orderCommentHtml += '<span class= "bmrp_s3" >&nbsp; </span>';
        orderCommentHtml += '<a class= "bmr_pa1 fl" href= "javascript:void(0);" orderItemID= "{0}" orderSN= "{3}" save= "true" id= "save_{0}" >{1} </a>';
        orderCommentHtml += '<span class= "bmrp_c2x" > <input id="isAnonymous_{0}" class= "bmra_rad" name= "" type= "checkbox" value= "" {2}><label for="isAnonymous_{0}" title= "匿名评价不会展示您的用户昵称，该评价也不会被第三方网站应用" > 匿名评价 <img src= "images/mwh.jpg" width= "16" height= "16" > </label></span>';
        orderCommentHtml += '</div></div> </div>';
        for (var o = 0; o < BcOrderCommentController.orderList.length; o++) {
            var orderInfo = BcOrderCommentController.orderList[o].orderInfo;
            var orderItemList = BcOrderCommentController.orderItemList[(<string>orderInfo.orderSN).toLowerCase()];
            for (var i = 0; i < orderItemList.length; i++) {
                var date = orderInfo.timeOrder.split('T');
                if (orderItemList[i].orderItemCommentList.length == 0) {
                    html += StringUtils.format(orderItemHtml,
                        orderItemList[i].navigationUrl,
                        orderItemList[i].thumbUrl,
                        orderItemList[i].title,
                        date[0],
                        orderItemList[i].orderItemID,
                        "发表评价",
                        orderInfo.orderSN);
                    html += StringUtils.format(orderCommentHtml, orderItemList[i].orderItemID, "保存", StringUtils.empty, orderInfo.orderSN);

                }
                else {
                    html += StringUtils.format(orderItemHtml,
                        orderItemList[i].navigationUrl,
                        orderItemList[i].thumbUrl,
                        orderItemList[i].title,
                        date[0],
                        orderItemList[i].orderItemID,
                        "已评价",
                        orderInfo.orderSN);
                    html += StringUtils.format(orderCommentHtml, orderItemList[i].orderItemID, "已评价", "disabled='disabled'", orderInfo.orderSN);
                }
            }
        }

        //添加之前删除原有的tr
        $("div[data='true']").remove();
        //添加
        $("#orderItemList").append(html);
        for (var o = 0; o < BcOrderCommentController.orderList.length; o++) {
            var orderInfo = BcOrderCommentController.orderList[o].orderInfo;
            var orderItemList = BcOrderCommentController.orderItemList[(<string>orderInfo.orderSN).toLowerCase()];
            //设置评分star,标签tags,评论comment,匿名状态,评价图片
            for (var i = 0; i < orderItemList.length; i++) {
                var orderItemID: string = orderItemList[i].orderItemID;
                if (UrlUtils.getUrlVar("publishmentSystemID") == StringUtils.empty) {
                    BcOrderCommentController.publishmentSystemID = orderInfo.publishmentSystemID;
                }
                var defaultTagArr: string[] = orderItemList[i].defaultTags;
                if (orderItemList[i].orderItemCommentList.length > 0) {
                    var firstComment = orderItemList[i].orderItemCommentList[0];
                    for (var j = 0; j < firstComment.star; j++) {
                        $("#starPanel_" + orderItemID).find("i").eq(j).addClass("bmrp_cutxx");
                    }

                    var tagArr: string[] = firstComment.tags.split(",");
                    for (var k = 0; k < defaultTagArr.length; k++) {
                        var newTag;
                        if (tagArr.indexOf(defaultTagArr[k]) >= 0) {
                            newTag = StringUtils.format('<a href="javascript:void (0);" class= "bmrp_a1 bmrp_cuta1" value="{0}"> {0} </a>', defaultTagArr[k]);
                            var t: string = tagArr[k];
                            tagArr.splice($.inArray(tagArr[k], tagArr), 1);
                        }
                        else {
                            newTag = StringUtils.format('<a href="javascript:void (0);" class= "bmrp_a1" value="{0}"> {0} </a>', defaultTagArr[k]);
                        }
                        $(newTag).insertBefore($("#extTag_" + orderItemID));
                    }
                    for (var l = 0; l < tagArr.length; l++) {
                        newTag = StringUtils.format('<a href="javascript:void (0);" class= "bmrp_a1 bmrp_cuta1" value="{0}"> {0} </a>', tagArr[l]);
                        $(newTag).insertBefore($("#extTag_" + orderItemID));
                    }
                    $("#txt_" + orderItemID).val(firstComment.comment);
                    if (firstComment.isAnonymous)
                        $("#isAnonymous_" + orderItemID).attr("checked", "checked");
                    else
                        $("#isAnonymous_" + orderItemID).removeAttr("checked");
                } else {
                    for (var k = 0; k < defaultTagArr.length; k++) {
                        newTag = StringUtils.format('<a href="javascript:void (0);" class= "bmrp_a1" value="{0}"> {0} </a>', defaultTagArr[k]);
                        $(newTag).insertBefore($("#extTag_" + orderItemID));
                    }
                }


                if (orderItemList[i].orderItemCommentList.length == 0) {
                    //上传图片
                    Utils.fileUpload('fileupload_' + orderItemID, BcOrderCommentController.orderService.getUploadImgUrl('UploadCommentImage', orderItemID, BcOrderCommentController.publishmentSystemID),(data) => {
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
                            $("#imagePanel_" + data.orderItemID).append(imgHtml);

                            BcReturnApplyController.imageNum++;

                            $("#imagePanel_" + data.orderItemID).find("li").unbind("hover");
                            $("#imagePanel_" + data.orderItemID).find("li").hover(function () {
                                $(this).addClass("hover");
                            }, function () {
                                    $(this).removeClass("hover");
                                });

                            $("#imagePanel_" + data.orderItemID).find("b").unbind("click");
                            $("#imagePanel_" + data.orderItemID).find("b").click(function () {
                                $(this).parent("li").remove();
                                BcReturnApplyController.imageNum--;
                            });
                        }
                    });
                } else {
                    //已经评价过
                    $("#fileupload_" + orderItemID).remove();
                    if (firstComment.imageUrl.length > 0) {
                        //展示图片地址
                        var li = "";
                        var images = firstComment.imageUrl.split(',');
                        for (var l = 0; l < images.length; l++) {
                            li += StringUtils.format('<li class=""><a href="{0}" target="_blank"><img class="err-product" width="50" height="50" src="{0}" data-img="1"></a></li>', images[l]);
                        }
                        $("#imagePanel_" + orderItemID).html(li);
                    }
                }
            }

        }



        $(".btnUpload").click(function () {
            var oid = $(this).attr("id").split("_")[1];
            if (BcReturnApplyController.imageNum >= 5) {
                Utils.tipAlert(false, "最多可以上传5张图片！");
                return;
            }
            $("#fileupload_" + oid).click();
        });

        //绑定事件
        this.registClickStar();
        this.registshowOrHideComment();
        this.registClickTag();
        this.registClickExtTag();
        this.registClickSave();
    }

    //显示/隐藏评价框
    registshowOrHideComment(): void {
        $("a[commentClick='true']").click(function () {
            var orderItemID: string = $(this).attr("orderItemID");
            var orderSN: string = $(this).attr("orderSN").toLowerCase();
            var commentPanel = $("#comment_" + orderItemID);
            if (commentPanel.css("display") == "none")
                commentPanel.css("display", "block");
            else
                commentPanel.css("display", "none");
        });
    }

    //星星点击
    registClickStar(): void {
        $("div[starPanel='true']").click(function (e) {
            var orderItemID: string = $(this).attr("orderItemID");
            var orderSN: string = $(this).attr("orderSN").toLowerCase();
            var event: any = e;
            var iList = $("#starPanel_" + orderItemID).find("i");
            var orderItemList = BcOrderCommentController.orderItemList[orderSN];
            if (!e && window.event)
                event = window.event;
            if (event.toElement && event.toElement.nodeName != "I") {
                return;
            }
            else {
                for (var j = 0; j < orderItemList.length; j++) {
                    if (orderItemList[j].orderItemID == orderItemID && orderItemList[j].orderItemCommentList.length > 0) {
                        return;
                    }
                }
                $("#starPanel_" + orderItemID).find("i").removeClass("bmrp_cutxx");
                for (var i = 0; i < iList.length; i++) {
                    $("#starPanel_" + orderItemID).find("i").eq(i).addClass("bmrp_cutxx");
                    if (event.toElement === iList[i]) {
                        for (var j = 0; j < orderItemList.length; j++) {
                            var star: number = orderItemList[j]["star"] || 1;
                            if (orderItemList[j].orderItemID == orderItemID && orderItemList[j].orderItemCommentList.length == 0) {
                                star = i + 1;
                                orderItemList[j]["star"] = star;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        });
    }

    //标签点击事件
    registClickTag(): void {
        $("div[tagPanel='true']").click(function (e) {
            var orderItemID: string = $(this).attr("orderItemID");
            var orderSN: string = $(this).attr("orderSN").toLowerCase();
            var event: any = e;
            var orderItemList = BcOrderCommentController.orderItemList[orderSN];
            var iList = $("#tagPanel_" + orderItemID).find("a");
            if (!e && window.event)
                event = window.event;
            if (event.toElement && event.toElement.nodeName != "A") {
                return;
            }
            else if ($(event.toElement).hasClass("bmrp_a2")) {
                for (var j = 0; j < orderItemList.length; j++) {
                    if (orderItemList[j].orderItemID == orderItemID && orderItemList[j].orderItemCommentList.length == 0) {
                        $("#extTag_" + orderItemID).css("display", "inline-block");
                        $("#extTag_" + orderItemID).focus();
                        break;
                    }
                }
            }
            else {
                for (var i = 0; i < iList.length; i++) {
                    if (event.toElement === iList[i]) {
                        var currentA = $("#tagPanel_" + orderItemID).find("a").eq(i);
                        for (var j = 0; j < orderItemList.length; j++) {
                            if (orderItemList[j].orderItemID == orderItemID && orderItemList[j].orderItemCommentList.length == 0) {
                                var tags: string[] = orderItemList[j]["tags"] || [];
                                if (currentA.hasClass("bmrp_cuta1")) {
                                    currentA.removeClass("bmrp_cuta1");
                                    if (currentA.attr("value") && tags.indexOf(currentA.attr("value")) >= 0) {
                                        tags.splice($.inArray(currentA.attr("value"), tags), 1);
                                    }
                                }
                                else {
                                    currentA.addClass("bmrp_cuta1");
                                    tags.push(currentA.attr("value"));
                                }
                                orderItemList[j]["tags"] = tags;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        });
    }

    //自定义标签回车
    registClickExtTag(): void {
        $("input[extTag='true']").keydown(function (e) {
            var orderItemID: string = $(this).attr("orderItemID");
            var orderSN: string = $(this).attr("orderSN").toLowerCase();
            var event: any = e;
            var orderItemList = BcOrderCommentController.orderItemList[orderSN];
            if (!e && window.event)
                event = window.event;
            if (e.keyCode == 13) {
                for (var j = 0; j < orderItemList.length; j++) {
                    if (orderItemList[j].orderItemID == orderItemID && orderItemList[j].orderItemCommentList.length == 0) {
                        var newTag = StringUtils.format('<a href="javascript:void (0);" class= "bmrp_a1 bmrp_cuta1" value="{0}"> {0} </a>', $(this).val());
                        $(newTag).insertBefore(this);
                        var tags: string[] = orderItemList[j]["tags"] || [];
                        if ($(this).val() && tags.indexOf($(this).val()) < 0) {
                            tags.push($(this).val());
                        }
                        orderItemList[j]["tags"] = tags;
                    }
                }

                $(this).css("display", "none");
                $(this).val("");
            }
        });
    }

    //保存
    registClickSave(): void {
        $("a[save='true']").click(function (e) {
            var orderItemID: string = $(this).attr("orderItemID");
            var orderSN: string = $(this).attr("orderSN").toLowerCase();
            var orderItemList = BcOrderCommentController.orderItemList[orderSN];
            var orderItemInfo: any;
            for (var j = 0; j < orderItemList.length; j++) {
                if (orderItemList[j].orderItemID == orderItemID && orderItemList[j].orderItemCommentList.length == 0) {
                    orderItemInfo = orderItemList[j];
                    var comment = $("#txt_" + orderItemID).val();
                    orderItemList[j]["comment"] = comment;
                    var isAnonymous = $("#isAnonymous_" + orderItemID).is(":checked");
                    orderItemList[j]["isAnonymous"] = isAnonymous;
                    var tags = [];
                    if (!orderItemInfo["tags"])
                        orderItemInfo["tags"] = tags;
                    var star = 1;
                    if (!orderItemInfo["star"])
                        orderItemInfo["star"] = star;

                    //晒单图片
                    var images = "";
                    $("#imagePanel_" + orderItemID).find("input[type='hidden']").map(function (index, ele) {
                        if (ele.getAttribute("value"))
                            images += ele.getAttribute("value") + ",";
                    });
                    if (!orderItemInfo["images"])
                        orderItemInfo["images"] = images;

                    orderItemID = orderItemInfo.orderItemID;
                    BcOrderCommentController.orderService.saveOrderItemComment(BcOrderCommentController.orderInfoID, orderItemID, BcOrderCommentController.publishmentSystemID, orderItemInfo["star"], orderItemInfo["tags"].join(","), orderItemInfo["comment"], orderItemInfo["isAnonymous"], orderItemInfo["images"]);
                    break;
                }
            }
        });
    }

    //获取评价订单详情地址
    static getUrl(orderID: number, publishmentSystemID: number): string {
        BcOrderCommentController.parmMap['orderID'] = orderID;
        BcOrderCommentController.parmMap['publishmentSystemID'] = publishmentSystemID;
        return "myComment.html?" + Utils.mapToUrl(BcOrderCommentController.parmMap);
    }
}