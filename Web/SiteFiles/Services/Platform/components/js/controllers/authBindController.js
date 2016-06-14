var authController = {};
authController.auth = {};
authController.isLoading = false;
authController.publishmentSystemID = "";
authController.auth.code = utilService.getUrlVar('code');
authController.auth.state = utilService.getUrlVar('state');
authController.auth.loginType = utilService.getUrlVar('loginType');


authController.load = function () {
    userService.authBind(authController.auth, function (data) {
        if (data.isSuccess == true) {
           
            $("#spanSiteName").html(data.siteName);
            $("#spanSiteName1").html(data.siteName);
            if (data.thirdLoginType == "QQ") {
                $("#spanLoginTypeName").html("腾讯QQ");
            }
            if (data.thirdLoginType == "Weibo") {
                $("#spanLoginTypeName").html("新浪微博");
            }
            if (data.thirdLoginType == "WeiXin") {
                $("#spanLoginTypeName").html("微信");
            }
            $("#imgThirdLoginUserHeadUrl").attr("src", data.thirdLoginUserHeadUrl);
            $("#divThirdLoginNickName").html(data.thirdLoginNickName);
            $("#thirdloginName").val(data.thirdLoginNickName);
            $("#thirdpassword").val(data.thirdLoginPassword);
            $("#thirdlocationUrl").val(data.indexPageUrl);
            authController.publishmentSystemID = data.publishmentSystemID;
            $("#btnThirdBind").unbind("click").click(function () { ThirdBind(); });

            ThirdBind();
        }
        else {
            utilService.render('authController', authController);
            notifyService.error(data.errorMessage);
        }

    });

};

authController.load();

function ThirdBind() {
    var loginController = {};
    loginController.login = {};
    loginController.login.loginName = $('#thirdloginName').val();
    loginController.login.password = $('#thirdpassword').val();
    var thirdlocationUrl = $("#thirdlocationUrl").val();
    loginController.login.isPersistent = true;
    loginController.login.publishmentSystemID = authController.publishmentSystemID;
    userService.login(loginController.login, function (data) {
        authController.isLoading = false;
        if (data.isSuccess) {
            window.top.location.href = thirdlocationUrl;
        }
        else {
            utilService.render('authController', authController);
            notifyService.error(data.errorMessage);
        }
    });
}