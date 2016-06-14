var userController = {};

userController.user = {};
userController.isAnonymous = true;

userController.getReturnUrl = function () {
    var reg = new RegExp("(^|&)" + "returnurl" + "=([^&]*)(&|$)");
    var r = window.location.search.toLowerCase().substr(1).match(reg);
    if (r != null) return unescape(r[2]); return $pageInfo.siteUrl;
};

userController.redirectToRegister = function () {
    location.href = $pageInfo.siteUrl + '/register.html?returnUrl=' + userController.getReturnUrl();
};

userController.redirectToLogin = function () {
    location.href = $pageInfo.siteUrl + '/login.html?returnUrl=' + userController.getReturnUrl();
};

userController.redirectToForgetPassword = function () {
    location.href = $pageInfo.siteUrl + '/forgetpassword.html?returnUrl=' + userController.getReturnUrl();
};

userController.logout = function (redirectUrl) {
    userService.logout(function (data) {
        if (redirectUrl == undefined)
            location.reload();
        else if (redirectUrl.indexOf('?') == -1) {
            location.href = redirectUrl + '?r=' + utilService.random();
        } else {
            location.href = redirectUrl + '&r=' + utilService.random();
        }
    });
};

userController.login = function () {
    $.layer({
        type: 2,
        title: '',
        maxmin: false,
        shadeClose: true,
        area: ['480px', '440px'],
        offset: ['100px', ''],
        iframe: {
            src: $pageInfo.rootUrl + '/sitefiles/services/platform/iframe/login.html?publishmentsystemid=' + $pageInfo.publishmentSystemID + '&siteurl=' + $pageInfo.siteUrl + '&returnurl=' + $pageInfo.currentUrl
        }
    });
};

userController.loginFirst = function () {
    if (userController.isAnonymous) {
        //userController.redirectToLogin();
        userController.login();
    }
};

userController.load = function () {
    userService.getUser(function (data) {
        userController.user = data.user;
        userController.isAnonymous = data.isAnonymous;

        utilService.render('userController', userController);
    });
};

userController.load();