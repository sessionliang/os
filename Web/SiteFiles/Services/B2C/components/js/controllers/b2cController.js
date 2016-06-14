var b2cController = {};

b2cController.amount = {};
b2cController.carts = [];
b2cController.user = {};
b2cController.setting = {};
b2cController.isAnonymous = true;
b2cController.guesses = [];

b2cController.getAllPurchaseNum = function () {
    var total = 0;
    $.each(b2cController.carts, function (index, val) {
        total += val.purchaseNum;
    });
    return total;
};

b2cController.deleteCart = function (index) {
    debugger;
    var cart = b2cController.carts.splice(index, 1);
    b2cService.deleteCart(cart[0].cartID, function (data) {
        b2cController.amount = data;
        notifyService.success('商品删除成功');
        utilService.render('b2cController', b2cController);
    });
};

b2cController.removeSelected = function () {
    $('input:checkbox[name=itemIndex]').each(function () {
        if ($(this).is(':checked'))
            b2cController.carts.splice($(this).val(), 1);7777
    });
    notifyService.success('商品删除成功');
    utilService.render('b2cController', b2cController);
};

b2cController.updateCart = function (index, val, op) {
    var cart = b2cController.carts[index];
    if (op) {
        if (op == 'minus') {
            if (cart.purchaseNum > 1) {
                cart.purchaseNum -= parseInt(val);
            } else {
                return;
            }
        } else if (op == 'plus') {
            cart.purchaseNum += parseInt(val);
        }
    } else {
        cart.purchaseNum = parseInt(val);
    }

    var cartIDWithPurchaseNum = { "cartID": cart.cartID, "purchaseNum": cart.purchaseNum };
    b2cService.updateCart(cartIDWithPurchaseNum, function (data) {
        b2cController.amount = data;
        utilService.render('b2cController', b2cController);
    });
};

b2cController.updateSetting = function (province) {
    b2cController.setting.province = province;
    b2cService.updateSetting(b2cController.setting, function (data) {
        notifyService.success('配送区域设置成功');
        utilService.render('b2cController', b2cController);
    });
};

b2cController.getB2CParameter = function () {
    b2cService.getB2CParameter(function (data) {
        b2cController.amount = data.amount;
        b2cController.carts = data.carts;
        b2cController.user = data.user;
        b2cController.setting = data.setting;
        b2cController.isAnonymous = data.isAnonymous;
        b2cController.addToHistory();

        utilService.render('b2cController', b2cController);
    });
};
b2cController.addToHistory = function () {
    if (!!$pageInfo.contentID) {
        //内容页面
        var history = {
            channelID: $pageInfo.channelID,
            contentID: $pageInfo.contentID
        };
    }
    else
        return false;

    b2cService.addToHistory(history, function (data) {
        if (data.isSuccess) {

        } else {
            notifyService.error(data.errorMessage);
        }
    });
}
b2cController.GetGuesses = function (success) {
    $.getJSON(b2cService.getUrl('GetGuesses'), null, success);
}

$(function () {
    b2cController.getB2CParameter();
});