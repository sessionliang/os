var orderService = {};

if (top.$pageInfo.apiUrl)
    orderService.baseUrl = top.$pageInfo.apiUrl + '/order/';
else if (top.$pageInfo.rootUrl)
    orderService.baseUrl = top.$pageInfo.rootUrl + '/order/';
else
    orderService.baseUrl = $pageInfo.rootUrl + '/order/';

orderService.getUrl = function (action, id) {
    if (id) {
        return orderService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&callback=?';
    }
    return orderService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&callback=?';
}

orderService.getOrderParameter = function (success) {
    $.getJSON(orderService.getUrl('GetOrderParameter'), null, success);
};

orderService.deleteConsignee = function (id) {
    $.getJSON(orderService.getUrl('DeleteConsignee', id), null, notifyService.successCallback);
};

orderService.addConsignee = function (consignee, success) {
    $.getJSON(orderService.getUrl('AddConsignee'), consignee, success);
};

orderService.updateConsignee = function (consignee, success) {
    $.getJSON(orderService.getUrl('UpdateConsignee'), consignee, success);
};

orderService.deleteInvoice = function (id) {
    $.getJSON(orderService.getUrl('DeleteInvoice', id), null, notifyService.successCallback);
};

orderService.addInvoice = function (invoice, success) {
    $.getJSON(orderService.getUrl('AddInvoice'), invoice, success);
};

orderService.updateInvoice = function (invoice, success) {
    $.getJSON(orderService.getUrl('UpdateInvoice'), invoice, success);
};

orderService.submitOrder = function (consigneeID, paymentID, invoiceID, success) {
    var data = { consigneeID: consigneeID, paymentID: paymentID, invoiceID: invoiceID };
    $.getJSON(orderService.getUrl('SubmitOrder'), data, success);
};

orderService.getLatestOrder = function (isPC, success) {
    $.getJSON(orderService.getUrl('GetLatestOrder'), { "isPC": isPC }, success);
};

orderService.getReturnValue = function (success) {
    var search = location.href.substring(location.href.indexOf('?') + 1);
    var url = orderService.getUrl('GetReturnValue') + "&" + search;
    $.getJSON(url, null, success);
};

orderService.getOrderList = function (isCompleted, isPayment, isPC, success) {
    var param = '&isCompleted=' + isCompleted + '&isPayment=' + isPayment + "&isPC=" + isPC;
    $.getJSON(orderService.getUrl('GetOrderList') + param, null, success);
};

orderService.getOrder = function (id, success) {
    $.getJSON(orderService.getUrl('GetOrder', id), null, success);
};

orderService.deleteOrder = function (id) {
    $.getJSON(orderService.getUrl('DeleteOrder', id), null, notifyService.successCallback);
};