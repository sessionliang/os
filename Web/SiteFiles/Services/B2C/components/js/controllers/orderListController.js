var orderListController = {};

orderListController.isCompleted = utilService.getUrlVar('isCompleted');
orderListController.isPayment = utilService.getUrlVar('isPayment');
orderListController.isPC = utilService.isPC();

orderListController.isEmpty = true;
orderListController.orderList = [];
orderListController.estimate = {};
orderListController.loading = true;

orderListController.render = function () {
    utilService.renderd/('orderListController', orderListController);
};

orderListController.getOrderList = function () {
    orderService.getOrderList(orderListController.isCompleted, orderListController.isPayment, orderListController.isPC, function (data) {
        orderListController.loading = false;
        orderListController.orderList = data;
        if (orderListController.orderList && orderListController.orderList.length > 0) {
            orderListController.isEmpty = false;
        } else {
            if (!orderListController.isCompleted && !orderListController.isPayment) {
                orderListController.isEmpty = true;
            } else {
                orderListController.isEmpty = false;
            }
        }
        orderListController.render();
    });
};

orderListController.submitOrderPayment = function (orderID) {
    $.each(orderListController.orderList, function (i, value) {
        if (value.orderInfo.id + '' == orderID + '') {
            var $div = $('<div style="display:none" />').appendTo('body');
            $div.html(value.paymentForm);
            eval(value.clickString);
            return;
        }
    });
};

orderListController.removeOrder = function (orderID) {
    var item = {};
    $.each(orderListController.orderList, function (i, value) {
        if (value.id + '' == orderID + '') {
            item = value;
        }
    });
    orderListController.orderList.splice($.inArray(item, orderListController.orderList), 1);
    orderService.deleteOrder(orderID);
    orderListController.render();
};

orderListController.getDate = function (d) {
    return d.substr(0, d.indexOf('T'));
};

orderListController.getDateTime = function (d) {
    return d.substr(0, d.indexOf('T')) + ' ' + d.substr(d.indexOf('T') + 1);
};

$(function () {
    orderListController.getOrderList();
});

