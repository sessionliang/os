var orderSuccessController = {};

orderSuccessController.order = {};
orderSuccessController.payment = {};

orderSuccessController.getLatestOrder = function (isPC) {
    orderService.getLatestOrder(isPC, function (data) {
        if (data.isSuccess) {
            orderSuccessController.order = data.order;
            orderSuccessController.payment.paymentName = data.paymentName;
            orderSuccessController.payment.paymentForm = data.paymentForm;
            orderSuccessController.payment.clickString = data.clickString;

            utilService.render('orderSuccessController', orderSuccessController);
        }
    });
};

orderSuccessController.submitPayment = function () {
    var $div = $('<div style="display:none" />').appendTo('body');
    $div.html(orderSuccessController.payment.paymentForm);
    eval(orderSuccessController.payment.clickString);
};


orderSuccessController.getLatestOrder(IsPC());


function IsPC() {
    var userAgentInfo = navigator.userAgent;
    var Agents = ["Android", "iPhone",
                "SymbianOS", "Windows Phone",
                "iPad", "iPod"];
    var flag = true;
    for (var v = 0; v < Agents.length; v++) {
        if (userAgentInfo.indexOf(Agents[v]) > 0) {
            flag = false;
            break;
        }
    }
    return flag;
}
