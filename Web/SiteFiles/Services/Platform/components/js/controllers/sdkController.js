var sdkController = {};

sdkController.isLoading = false;
sdkController.publishmentSystemID = utilService.getUrlVar('publishmentSystemID');
sdkController.returnUrl = utilService.getUrlVar('returnurl');

sdkController.login = function (sdkType) {
 
    if (sdkController.isLoading) return false;
    sdkController.isLoading = true;

    sdkController.render();
    userService.sdkLogin(sdkType, sdkController.returnUrl, function (data) {
        sdkController.isLoading = false;
        if (data.length > 0) {
            window.top.location.href = data;
        }
        else {
            sdkController.render();
            notifyService.error("第三方登陆失败!");
        }
    });
};

sdkController.render = function () {
    utilService.render('sdkController', sdkController);
};

sdkController.render();