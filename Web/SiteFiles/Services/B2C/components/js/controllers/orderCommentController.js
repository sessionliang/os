//****************
//* 订单评价
//* 2015-07-07
//* sessionliang
//****************
var orderCommentController = {};

//渲染数据（带有class='orderCommentController'的容器）
orderCommentController.render = function () {
    utilService.render('orderCommentController', orderCommentController);

    //评分
    var starTable = $("#orderCommentDiv table");

    for (var i = 0; i < starTable.length; i++) {
        var stars = $(starTable[i]).find("i");
        var star = $(starTable[i]).find(".commentStar").val();
        for (var j = 0; j < stars.length; j++) {
            if (j < star) {
                $(stars[j]).addClass("bmrp_cutxx");
            }
        }
    }

    //分页链接
    $("#divCommentPageLink").html(pageDataUtils.getPageHtml(orderCommentController.pageJson, 'initOrderComment'));
    $("#orderCommentDiv").css("display", "block");
};

//评论集合
orderCommentController.orderComments = [];
//分页数据
orderCommentController.pageJson = "";

//评论类型
orderCommentController.type = "";

orderCommentController.TYPE_GOOD = "Good";
orderCommentController.TYPE_Middle = "Middle";
orderCommentController.TYPE_Bad = "Bad";


orderCommentController.init = function () {
    //all
    $("#allCommentBtn").click(function () {
        orderCommentController.initOrderComment("", 1, 10);
    });

    //good
    $("#goodCommentBtn").click(function () {
        orderCommentController.initOrderComment(orderCommentController.TYPE_GOOD, 1, 10);
    });

    //middle
    $("#middleCommentBtn").click(function () {
        orderCommentController.initOrderComment(orderCommentController.TYPE_Middle, 1, 10);
    });

    //bad
    $("#badCommentBtn").click(function () {
        orderCommentController.initOrderComment(orderCommentController.TYPE_Bad, 1, 10);
    });

    orderCommentController.initOrderComment("", 1, 10);
};

orderCommentController.initOrderComment = function (type, pageIndex, prePageNum) {
    orderCommentController.type = type;
    orderCommentService.getOrderCommentList(type, pageIndex, prePageNum, function (json) {
        orderCommentController.orderComments = json.orderCommentList;
        orderCommentController.pageJson = json.pageJson;
        orderCommentController.render();
    });
};

orderCommentController.init();