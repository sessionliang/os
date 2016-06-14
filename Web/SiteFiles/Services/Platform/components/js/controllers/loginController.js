var loginController = {};

loginController.login = {};
loginController.isLoading = false;
loginController.publishmentSystemID = utilService.getUrlVar('publishmentSystemID');
loginController.thirdLoginList = [];

loginController.getReturnUrl = function () {
    var reg = new RegExp("(^|&)" + "returnurl" + "=([^&]*)(&|$)");
    var r = window.location.search.toLowerCase().substr(1).match(reg);
    if (r != null) return unescape(r[2]); return $pageInfo.siteUrl;
};

loginController.redirectToRegister = function () {
    var url = 'register.html?returnUrl=' + loginController.getReturnUrl();
    if (loginController.publishmentSystemID) {
        url += '&publishmentSystemID=' + loginController.publishmentSystemID;
    }
    location.href = url;
};

loginController.redirectToForgetPassword = function () {
    var url = 'forget.html?returnUrl=' + loginController.getReturnUrl();
    if (loginController.publishmentSystemID) {
        url += '&publishmentSystemID=' + loginController.publishmentSystemID;
    }
    location.href = url;
};

loginController.submit = function () {

    loginController.login.loginName = $('#loginName').val();
    loginController.login.password = $('#password').val();
    loginController.login.isPersistent = $('#isPersistent').is(':checked');

    var isValid = true;
    if (!loginController.login.loginName && !loginController.login.password) {
        notifyService.error('请输入登录账号及密码');
        $('#loginName').focus();
        isValid = false;
    }
    else if (!loginController.login.loginName) {
        notifyService.error('请输入登录账号');
        $('#loginName').focus();
        isValid = false;
    }
    else if (!loginController.login.password) {
        notifyService.error('请输入登录密码');
        $('#password').focus();
        isValid = false;
    }
    if (!isValid) return false;

    if (loginController.isLoading) return false;
    loginController.isLoading = true;

    loginController.render();

    userService.login(loginController.login, function (data) {
        loginController.isLoading = false;
        if (data.isSuccess) {
            window.top.location.href = loginController.getReturnUrl();
        } else {
            loginController.render();
            notifyService.error(data.errorMessage);
        }
    });
};

loginController.render = function () {
    utilService.render('loginController', loginController);
};

loginController.load = function () {
    userService.getUser(function (data) {
        if (data.isAnonymous == false) {
            $("#divCurrent").show();
            $("#currentImg").attr("src", data.user.avatarMiddle);
            $("#currentUser").text(data.user.userName);
        }
        else {
            $("#divLogin").show();
        }
    });


    userService.getThirdLoginTypeParameter(function (data) {
        //alert(data.isSucess);
        if (data.isSucess == true) {
            loginController.thirdLoginList = data.thirdLoginList;
            loginController.render();
        }
        else {
            loginController.render();
            notifyService.error("尚未配置第三方登陆信息！");
        }
    });
};

loginController.load();