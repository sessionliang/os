var registerController = {};

registerController.login = {};
registerController.isSuccess = false;
registerController.successMessage;
registerController.publishmentSystemID = utilService.getUrlVar('publishmentSystemID');

registerController.getReturnUrl = function () {
    var reg = new RegExp("(^|&)" + "returnurl" + "=([^&]*)(&|$)");
    var r = window.location.search.toLowerCase().substr(1).match(reg);
    if (r != null) return unescape(r[2]); return $pageInfo.siteUrl;
};

registerController.redirectToLogin = function () {
    var url = 'login.html?returnUrl=' + registerController.getReturnUrl();
    if (registerController.publishmentSystemID) {
        url += '&publishmentSystemID=' + registerController.publishmentSystemID;
    }
    location.href = url;
};

registerController.submit = function () {

    registerController.login.loginName = $('#loginName').val();
    registerController.login.email = $('#email').val();
    registerController.login.mobile = $('#mobile').val();
    registerController.login.password = $('#password').val();
    registerController.login.password_confirm = $('#password_confirm').val();

    var isValid = true;
    if (!registerController.login.loginName) {
        notifyService.error('请输入登录账号');
        $('#loginName').focus();
        isValid = false;
    }
    else if (!registerController.login.email) {
        notifyService.error('请输入邮箱');
        $('#email').focus();
        isValid = false;
    }
    else if (!utilService.isEmail(registerController.login.email)) {
        notifyService.error('请输入正确的邮箱地址');
        $('#email').focus();
        isValid = false;
    }
    else if (!registerController.login.mobile) {
        notifyService.error('请输入手机号码');
        $('#mobile').focus();
        isValid = false;
    }
    else if (!utilService.isMobile(registerController.login.mobile)) {
        notifyService.error('请输入正确的手机号码');
        $('#mobile').focus();
        isValid = false;
    }
    else if (!registerController.login.password) {
        notifyService.error('请输入登录密码');
        $('#password').focus();
        isValid = false;
    }
    else if (registerController.login.password.length < 6 || registerController.login.password.length > 20) {
        notifyService.error('密码长度只能在6-20位字符之间');
        $('#password').focus();
        isValid = false;
    }
    else if ($("#password_confirm").length > 0 && registerController.login.password != registerController.login.password_confirm) {
        notifyService.error('确认密码与密码不一致，请重新输入');
        $('#password_confirm').focus();
        isValid = false;
    }
    if (!isValid) return false;

    userService.register(registerController.login, function (data) {
        registerController.isSuccess = data.isSuccess;
        if (registerController.isSuccess) {
            registerController.successMessage = data.successMessage;
            registerController.render();
            notifyService.success(registerController.successMessage);
            window.setTimeout(function () {
                registerController.redirectToLogin();
            }, 2000);
        } else {
            notifyService.error(data.errorMessage);
        }
    });
};

registerController.render = function () {
    utilService.render('registerController', registerController);
};



registerController.load = function () {
    userService.getThirdLoginTypeParameter(function (data) {
        if (data.isSucess == true) {
            registerController.thirdLoginList = data.thirdLoginList;
            registerController.render();
        }
        else {
            registerController.render();
            notifyService.error("尚未配置第三方登陆信息！");
        }
    });
};

registerController.load();

