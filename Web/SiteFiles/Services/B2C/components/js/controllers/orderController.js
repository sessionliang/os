var orderController = {};

orderController.render = function () {
    utilService.render('orderController', orderController);
    $("#location").attr("selectedValue", orderController.consignee.province + "," + orderController.consignee.city + "," + orderController.consignee.area);
    if ($("#location").ProvinceCity) {
        $("#location").ProvinceCity();
    }
    $("#location select").attr('class', 'input-small').css('margin-right', '5px');

    $('.order_form input').on('ifChecked', function (event) {
        eval($(this).attr('onclick'));
    }).iCheck({
        checkboxClass: 'icheckbox_flat-red',
        radioClass: 'iradio_flat-red'
    });

};

orderController.consignees = [];
orderController.payments = [];
orderController.shipments = [];
orderController.invoices = [];
orderController.consignee = {};
orderController.payment = {};
orderController.shipment = {};
orderController.invoice = {};
orderController.isInvoice = false;

orderController.consigneeSelectMode = true;
orderController.consigneeAddOrEditMode = true;
orderController.paymentShipmentSelectMode = false;
orderController.invoiceSelectMode = false;
orderController.invoiceAddOrEditMode = true;

orderController.initConsignee = function (isAdd) {
    if (orderController.consignee == undefined || isAdd) {
        orderController.consignee = { 'id': 0 };
        orderController.consigneeAddOrEditMode = true;
    } else {
        orderController.consigneeAddOrEditMode = false;
    }
};

orderController.initInvoice = function (isAdd) {
    if (orderController.invoice == undefined || isAdd) {
        orderController.invoice = { 'id': 0, 'isVat': false, 'isCompany': false };
        orderController.invoiceAddOrEditMode = true;
    } else {
        orderController.invoiceAddOrEditMode = false;
    }
};

orderController.getOrderParameter = function () {

    orderService.getOrderParameter(function (data) {
        if (data.isAnonymous) {
            var loginUrl = $pageInfo.homeUrl + '/login.html?returnUrl=' + $pageInfo.currentUrl;
            location.href = loginUrl;
        }
        orderController.consignees = data.consignees;
        orderController.payments = data.payments;
        orderController.shipments = data.shipments;
        orderController.invoices = data.invoices;
        orderController.consignee = orderController.consignees[0];
        orderController.payment = orderController.payments[0];
        if (!orderController.payment) {
            orderController.payment = {};
        }
        orderController.shipment = orderController.shipments[0];
        if (!orderController.shipment) {
            orderController.shipment = {};
        }
        orderController.invoice = orderController.invoices[0];

        orderController.initConsignee(false);
        orderController.initInvoice(false);
        orderController.render();
    });
};

orderController.getOrderParameter();

orderController.getConsigneeName = function (item) {
    if (!item) {
        item = orderController.consignee;
        if (item) {
            return item.consignee + " &nbsp; " + item.mobile + " &nbsp; " + item.email + "<br />" + item.province + " " + item.city + " " + item.area + " " + item.address;
        }
    } else {
        return item.province + " " + item.city + " " + item.area + " " + item.address + " &nbsp; " + item.mobile;
    }
};

orderController.editConsignee = function (itemID) {
    $.each(orderController.consignees, function (i, value) {
        if (value.id + '' == itemID + '') {
            orderController.consignee = value;
        }
    });
    orderController.consigneeAddOrEditMode = true;
    orderController.render();
};

orderController.selectConsignee = function (mode) {
    orderController.consigneeSelectMode = mode;
    orderController.render();
};

orderController.removeConsignee = function (e, itemID) {
    var item = {};
    $.each(orderController.consignees, function (i, value) {
        if (value.id + '' == itemID + '') {
            item = value;
        }
    });
    orderController.consignees.splice($.inArray(item, orderController.consignees), 1);
    if (itemID + '' == orderController.consignee.id + '') {
        orderController.consignee = orderController.consignees[0];
    }
    orderService.deleteConsignee(itemID);
    orderController.initConsignee(false);
    orderController.render();
};

orderController.addConsignee = function () {
    orderController.initConsignee(true);
    orderController.render();
};

orderController.changeConsignee = function (itemID) {
    var item = {};
    $.each(orderController.consignees, function (i, value) {
        if (value.id + '' == itemID + '') {
            item = value;
        }
    });
    orderController.consignee = item;
    orderController.consigneeAddOrEditMode = false;
    orderController.render();
};

orderController.saveConsignee = function () {
    if (orderController.consigneeAddOrEditMode) {

        $('#consigneeEdit .order_tips').hide();

        if ($('#location select').length == 3) {
            orderController.consignee.province = $($('#location select').get(0)).find("option:selected").val();
            orderController.consignee.city = $($('#location select').get(1)).find("option:selected").val();
            orderController.consignee.area = $($('#location select').get(2)).find("option:selected").val();
        }
        orderController.consignee.consignee = $('#consignee').val();
        orderController.consignee.address = $('#address').val();
        orderController.consignee.mobile = $('#mobile').val();
        orderController.consignee.tel = $('#tel').val();
        orderController.consignee.email = $('#email').val();

        var isValidate = true;

        if (!orderController.consignee.province) {
            $('#location').next('.order_tips').show();
            isValidate = false;
        }

        if (!orderController.consignee.consignee) {
            $('#consignee').next('.order_tips').show();
            isValidate = false;
        }
        if (!orderController.consignee.address) {
            $('#address').next('.order_tips').show();
            isValidate = false;
        }
        if (!orderController.consignee.mobile) {
            $('#mobile').next('.order_tips').show();
            isValidate = false;
        }

        if (!isValidate) return;

        if (orderController.consignee.id == 0) {
            orderService.addConsignee(orderController.consignee, function (response) {
                orderController.consignee.id = response.id;
                orderController.consignees.splice(0, 0, orderController.consignee);
                orderController.consigneeAddOrEditMode = false;
                notifyService.success();
            });
        } else {
            orderService.updateConsignee(orderController.consignee, function (response) {
                orderController.consigneeAddOrEditMode = false;
                notifyService.success();
            });
        }
    }

    orderController.consigneeSelectMode = false;
    orderController.paymentShipmentSelectMode = true;
    orderController.render();
};

orderController.savePaymentShipment = function () {
    orderController.consigneeSelectMode = false;
    orderController.paymentShipmentSelectMode = false;
    orderController.invoiceSelectMode = true;
    orderController.render();
};

orderController.selectPaymentShipment = function (mode) {
    orderController.paymentShipmentSelectMode = mode;
    if (orderController.invoice.id == 0) {
        orderController.invoiceSelectMode = true;
    }
    orderController.render();
};

orderController.changePayment = function (index) {
    var i = 0;
    $.each(orderController.payments, function (i, value) {
        if (index == i++) {
            orderController.payment = value;
        }
    });
    orderController.render();
};

orderController.changeShipment = function (index) {
    var i = 0;
    $.each(orderController.shipments, function (i, value) {
        if (index == i++) {
            orderController.shipment = value;
        }
    });
    orderController.render();
};

orderController.changeIsInvoice = function (mode) {
    orderController.isInvoice = mode;
    orderController.render();
};

orderController.selectInvoice = function (mode) {
    orderController.invoiceSelectMode = mode;
    orderController.render();
};

orderController.getInvoiceName = function (item) {
    var showInvoice = orderController.isInvoice;
    if (!item) {
        item = orderController.invoice;
    } else {
        showInvoice = true;
    }
    if (showInvoice) {
        if (item.isVat) {
            return "增值税发票 " + item.vatCompanyName;
        } else {
            return "普通发票 " + (item.isCompany ? item.companyName : "个人");
        }
    }
    else {
        return "不需要发票"
    }
};

orderController.editInvoice = function (itemID) {
    $.each(orderController.invoices, function (i, value) {
        if (value.id + '' == itemID + '') {
            orderController.invoice = value;
        }
    });
    orderController.invoiceAddOrEditMode = true;
    orderController.render();
};

orderController.removeInvoice = function (e, itemID) {
    var item = {};
    $.each(orderController.invoices, function (i, value) {
        if (value.id + '' == itemID + '') {
            item = value;
        }
    });
    orderController.invoices.splice($.inArray(item, orderController.invoices), 1);
    if (itemID + '' == orderController.invoice.id + '') {
        orderController.invoice = orderController.invoices[0];
    }
    orderService.deleteInvoice(itemID);
    orderController.initInvoice(false);
    orderController.render();
};

orderController.addInvoice = function () {
    orderController.initInvoice(true);
    orderController.render();
};

orderController.changeInvoice = function (itemID) {
    $.each(orderController.invoices, function (i, value) {
        if (value.id + '' == itemID + '') {
            orderController.invoice = value;
        }
    });
    orderController.invoiceAddOrEditMode = false;
    orderController.render();
};

orderController.isVatInvoice = function (isVat) {
    orderController.invoice.isVat = isVat;
    orderController.render();
};

orderController.isCompanyInvoice = function (isCompany) {
    orderController.invoice.isCompany = isCompany;
    orderController.render();
};

orderController.saveInvoice = function () {
    if (orderController.isInvoice && orderController.invoiceAddOrEditMode) {

        orderController.invoice.companyName = $('#companyName').val();
        orderController.invoice.vatCompanyName = $('#vatCompanyName').val();
        orderController.invoice.vatCode = $('#vatCode').val();
        orderController.invoice.vatAddress = $('#vatAddress').val();
        orderController.invoice.vatPhone = $('#vatPhone').val();
        orderController.invoice.vatBankName = $('#vatBankName').val();
        orderController.invoice.vatBankAccount = $('#vatBankAccount').val();
        orderController.invoice.consigneeName = $('#consigneeName').val();
        orderController.invoice.consigneeMobile = $('#consigneeMobile').val();
        orderController.invoice.consigneeAddress = $('#consigneeAddress').val();


        if (orderController.invoice.id == 0) {
            orderService.addInvoice(orderController.invoice, function (response) {
                orderController.invoice.id = response.id;
                orderController.invoices.splice(0, 0, orderController.invoice);
                orderController.invoiceAddOrEditMode = false;
                notifyService.success();
            });
        } else {
            orderService.updateInvoice(orderController.invoice, function (response) {
                orderController.invoiceAddOrEditMode = false;
                notifyService.success();
            });
        }
    }

    orderController.selectInvoice(false);
};

orderController.submitOrder = function (successUrl) {
    var invoiceID = 0;
    if (orderController.isInvoice) {
        invoiceID = orderController.invoice.id;
    }
    if (orderController.consignees.length == 0 || orderController.consignee.id == 0) {
        alert("收货人信息不能为空");
        return;
    }
    orderService.submitOrder(orderController.consignee.id, orderController.payment.id, invoiceID, function (data) {
        if (data.isSuccess)
            location.href = successUrl;
        else
            alert(data.errorMessage);
    });
};