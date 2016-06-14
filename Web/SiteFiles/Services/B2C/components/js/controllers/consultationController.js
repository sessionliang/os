//****************
//* 购买咨询
//* 2015-07-07
//* sessionliang
//****************
var consultationController = {};

//渲染数据（带有class='consultationController'的容器）
consultationController.render = function () {
    utilService.render('consultationController', consultationController);
    //分页链接
    $("#divPageLink").html(pageDataUtils.getPageHtml(consultationController.pageJson, 'initConsultation'));
    $("#consultationDiv").css("display", "block");
};

//购买咨询集合
consultationController.consultations = [];
//分页数据
consultationController.pageJson = "";

//查询关键字
consultationController.keywords = "";
consultationController.type = "";

consultationController.TYPE_GOODS = "Goods";
consultationController.TYPE_STOCK_SHIPMENT = "StockShipment";
consultationController.TYPE_PAYMENT = "Payment";
consultationController.TYPE_INVOICE = "Invoice";
consultationController.TYPE_PROMOTION = "Promotion";


consultationController.init = function () {
    //保存按钮
    $("#saveConsultation").click(function () {
        consultationController.saveConsultation();
    });


    //all
    $("#allConBtn").click(function () {
        consultationController.initConsultation("", 1, 10);
    });

    //goods
    $("#goodsConBtn").click(function () {
        consultationController.initConsultation(consultationController.TYPE_GOODS, 1, 10);
    });

    //stockShipment
    $("#stockShipmentConBtn").click(function () {
        consultationController.initConsultation(consultationController.TYPE_STOCK_SHIPMENT, 1, 10);
    });

    //payment
    $("#paymentConBtn").click(function () {
        consultationController.initConsultation(consultationController.TYPE_PAYMENT, 1, 10);
    });

    //invoice
    $("#invoiceConBtn").click(function () {
        consultationController.initConsultation(consultationController.TYPE_INVOICE, 1, 10);
    });

    //promotion
    $("#promotionConBtn").click(function () {
        consultationController.initConsultation(consultationController.TYPE_PROMOTION, 1, 10);
    });

    $("#consultationSearchBtn").click(function () {
        consultationController.initConsultation(consultationController.type, 1, 10);
    });

    consultationController.initConsultation("", 1, 10);
};

consultationController.initConsultation = function (type, pageIndex, prePageNum) {
    var keywords = $("#consultationKeyword").val();
    consultationController.type = type;
    consultationService.getConsultationList(keywords, type, pageIndex, prePageNum, function (json) {
        consultationController.consultations = json.consultationList;
        consultationController.pageJson = json.pageJson;
        consultationController.render();
    });
};

consultationController.saveConsultation = function () {

    consultationController.consultation = $("#consultationContent").val();
    consultationController.type = $("input[name='pointType']:checked").val();

    var isValidate = true;

    if (!consultationController.consultation) {
        notifyService.error("必须填写咨询信息");
        isValidate = false;
    }

    if (!consultationController.type) {
        notifyService.error("必须选择咨询类型");
        isValidate = false;
    }

    if (!isValidate) return;

    consultationService.saveConsultation(consultationController.consultation, consultationController.type, function (json) {
        if (json.isSuccess) {
            consultationController.consultations = json.consultationList;
            notifyService.success("操作成功");
        } else {
            notifyService.error("操作失败");
        }

    });
    consultationController.render();
};

consultationController.hoverConsultationDiv = function () {
    if ($("#consultation").css("display") == "none") {
        $("#consultation").css("display", "block");
        $("html,body").animate({ scrollTop: $("#consultation").offset().top }, 1000);
    }
    else {
        $("#consultation").css("display", "none");
    }
};

consultationController.init();