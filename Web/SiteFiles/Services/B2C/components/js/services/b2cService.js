var b2cService = {};

if (top.$pageInfo.apiUrl)
    b2cService.baseUrl = top.$pageInfo.apiUrl + '/b2c/';
else if (top.$pageInfo.rootUrl)
    b2cService.baseUrl = top.$pageInfo.rootUrl + '/b2c/';
else
    b2cService.baseUrl = $pageInfo.rootUrl + '/b2c/';

b2cService.getUrl = function (action, id) {
    if (id) {
        return b2cService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&callback=?';
    }
    return b2cService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&callback=?';
}

b2cService.getB2CParameter = function (success) {
    $.getJSON(b2cService.getUrl('GetB2CParameter'), null, success);
};

b2cService.deleteCart = function (cartID, success) {
    $.getJSON(b2cService.getUrl('DeleteCart', cartID), null, success);
};

b2cService.updateCart = function (cartIDWithPurchaseNum, success) {
    $.getJSON(b2cService.getUrl('UpdateCart'), cartIDWithPurchaseNum, success);
};

b2cService.addToCart = function (cart, success, isAsync) {
    if (arguments.length == 2)
        isAsync = true;
    $.ajaxSettings.async = isAsync;
    $.getJSON(b2cService.getUrl('AddToCart'), cart, success);
    $.ajaxSettings.async = true;
};

b2cService.addToFollow = function (follow, success) {
    $.getJSON(b2cService.getUrl('AddToFollow'), follow, success);
};

b2cService.updateSetting = function (setting, success) {
    $.getJSON(b2cService.getUrl('UpdateSetting'), setting, success);
};
b2cService.addToHistory = function (history, success) {
    $.getJSON(b2cService.getUrl('AddToHistory'), history, success);
};