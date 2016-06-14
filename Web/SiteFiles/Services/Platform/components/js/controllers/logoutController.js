var logoutController = {};

logoutController.getReturnUrl = function()
{
    var reg = new RegExp("(^|&)"+ "returnurl" +"=([^&]*)(&|$)");
    var r = window.location.search.toLowerCase().substr(1).match(reg);
    if (r!=null) return unescape(r[2]); return $pageInfo.siteUrl;
};

logoutController.main = function(){
    userService.logout(function (data) {
        location.href = logoutController.getReturnUrl();
    });
};

logoutController.main();