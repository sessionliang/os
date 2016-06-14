var userService = {};

//if ($pageInfo.apiUrl)
//userService.baseUrl = $pageInfo.apiUrl + '/user/';
//else
if (top.$pageInfo.apiUrl)
    userService.baseUrl = top.$pageInfo.apiUrl + '/user/';
else if (top.$pageInfo.rootUrl)
    userService.baseUrl = top.$pageInfo.rootUrl + '/user/';
else
    userService.baseUrl = $pageInfo.rootUrl + '/user/';

userService.getUrl = function (action, id) {
    if (id) {
        return userService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&callback=?';
    }
    if ($pageInfo.publishmentSystemID > 0) {
        return userService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&callback=?';
    }
    return userService.baseUrl + action + '?callback=?';
}

userService.getUser = function (success) {
    $.getJSON(userService.getUrl('GetUser'), null, success);
};

userService.logout = function (success) {
    $.getJSON(userService.getUrl('Logout'), null, success);
};

userService.login = function (login, success) {
    $.getJSON(userService.getUrl('Login'), login, success);
};

userService.register = function (login, success) {
    $.getJSON(userService.getUrl('Register'), login, success);
};

userService.forgetPassword = function (login, success) {
    $.getJSON(userService.getUrl('ForgetPassword'), login, success);
};

userService.changePassword = function (password, success) {
    $.getJSON(userService.getUrl('ChangePassword'), password, success);
};


// 第三方登陆  begin wujiaqiang

userService.getThirdLoginTypeParameter = function (success) {
    $.getJSON(userService.getUrl('GetThirdLoginTypeParameter'), null, success);
};

userService.sdkLogin = function (loginType, returnUrl, success) {
    $.getJSON(userService.getUrl('SdkLogin'), { 'sdkType': loginType, 'returnUrl': returnUrl }, success);
};

userService.authLogin = function (auth, success) {
    $.getJSON(userService.getUrl('AuthLogin'), auth, success);
};

userService.authBind = function (auth, success) {
    $.getJSON(userService.getUrl('AuthBind'), auth, success);
};

// 第三方登陆 end wujiaqiang