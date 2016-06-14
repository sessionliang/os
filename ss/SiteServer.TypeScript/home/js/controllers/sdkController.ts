class SDKController {
    public userService: UserService;
    public isLoading: boolean = false;
    //public publishmentSystemID: number = 0;//HomeUrlUtils.publishSystemID;
    public returnUrl = HomeUrlUtils.getReturnUrl();

    init(): void {
        this.userService.getThirdLoginTypeParameter((data) => {
            this.getThirdLoginTypeParameter(data);
        });
    }

    initBind(): void {
        this.userService.getThirdLoginTypeParameter((data) => {
            this.getThirdBindTypeParameter(data);
        });
    }

    getThirdLoginTypeParameter(data): void {
        var thirdHtml = '';
        if (data.thirdLoginList.length > 0) {
            thirdHtml += '<span class="fl">其他帐号登录注册:&nbsp;</span>';
        }
        for (var i = 0; i < data.thirdLoginList.length; i++) {
            if (data.thirdLoginList[i].thirdLoginType == 0) {
                thirdHtml += '<a id="qq" href="#" class="mfm_lga"><img src="images/mn_ico2.png" width="35" height="35" /></a>';
            }
            if (data.thirdLoginList[i].thirdLoginType == 1) {
                thirdHtml += '<a id="wb" href="#" class="mfm_lga"><img src="images/mn_ico3.png" width="35" height="35" /></a> ';

            }
            if (data.thirdLoginList[i].thirdLoginType == 2) {
                thirdHtml += '<a id="wx" href="#" class="mfm_lga"><img src="images/mn_ico4.png" width="35" height="35" /></a>';

            }
        }
        $("#thirdLoginPanel").append($(thirdHtml));
        if ($("a[id='qq']"))
            $("a[id='qq']").click(() => {
                this.qqLogin();
            });
        if ($("#eb"))
            $("#wb").click(() => {
                this.wbLogin();
            });
        if ($("#wx"))
            $("#wx").click(() => {
                this.wxLogin();
            });
    }

    qqLogin(): void {
        this.sdkLogin(1);
    }

    wbLogin(): void {
        this.sdkLogin(2);
    }

    wxLogin(): void {
        this.sdkLogin(3);
    }

    sdkLogin(sdkType): void {
        this.userService.sdkLogin(sdkType, this.returnUrl, function (data) {
            if (data.length > 0) {
                window.top.location.href = data;
            }
            else {
                Utils.tipAlert(false, "第三方登陆失败!");
            }
        });
    }

    getThirdBindTypeParameter(data): void {
        if (data.thirdLoginList.length == 0) {
            $("a[id='qq']").parents(".m2r_dl").html("<span style='color:red;font-size:15px;'>该功能没有启用！</span>");
            $("a[id='qq']").unbind("click").click(() => {
                alert("QQ绑定功能没有启用");
            });
            $("#wb").unbind("click").click(() => {
                alert("微博绑定功能没有启用");
            });
            $("#wx").unbind("click").click(() => {
                alert("微信绑定功能没有启用");
            });
        }
        else {
            for (var i = 0; i < data.thirdLoginList.length; i++) {
                if (data.thirdLoginList[i].thirdLoginType == 0) {

                    if (data.bindedThirdLoginList.indexOf(data.thirdLoginList[i].thirdLoginType) != -1) {
                        $("a[id='qq']").unbind("click").click(() => {
                            this.qqUnBind();
                        });
                        $("a[id='qq']").html("解除绑定");
                        $("a[id='qq']").removeClass("m2l_isbtn").addClass("m2l_isUnbtn");
                    }
                    else {
                        $("a[id='qq']").unbind("click").click(() => {
                            this.qqBind();
                        });
                        $("a[id='qq']").html("点击绑定");
                        $("a[id='qq']").removeClass("m2l_isUnbtn").addClass("m2l_isbtn");
                    }

                }
                else {
                    $("a[id='qq']").html("未启用").css("background", "#C4C4C4");
                    $("a[id='qq']").unbind("click");
                }
                if (data.thirdLoginList[i].thirdLoginType == 1) {
                    if (data.bindedThirdLoginList.indexOf(data.thirdLoginList[i].thirdLoginType) != -1) {
                        $("#wb").unbind("click").click(() => {
                            this.wbUnBind();
                        });
                        $("#wb").html("解除绑定");
                        $("#wb").removeClass("m2l_isbtn").addClass("m2l_isUnbtn");
                    }
                    else {
                        $("#wb").unbind("click").click(() => {
                            this.wbBind();
                        });
                        $("#wb").html("点击绑定");
                        $("#wb").removeClass("m2l_isUnbtn").addClass("m2l_isbtn");
                    }
                }
                else {
                    $("#wb").html("未启用").css("background", "#C4C4C4");
                    $("#wb").unbind("click");
                }
                if (data.thirdLoginList[i].thirdLoginType == 2) {
                    if (data.bindedThirdLoginList.indexOf(data.thirdLoginList[i].thirdLoginType) != -1) {
                        $("#wx").unbind("click").click(() => {
                            this.wxUnBind();
                        });
                        $("#wx").html("解除绑定");
                        $("#wx").removeClass("m2l_isbtn").addClass("m2l_isUnbtn");
                    }
                    else {
                        $("#wx").unbind("click").click(() => {
                            this.wxBind();
                        });
                        $("#wx").html("点击绑定");
                        $("#wx").removeClass("m2l_isUnbtn").addClass("m2l_isbtn");
                    }
                }
                else {
                    $("#wx").html("未启用").css("background", "#C4C4C4");
                    $("#wx").unbind("click");
                }
            }
        }
    }

    qqBind(): void {
        this.sdkBind(1);
    }

    wbBind(): void {
        this.sdkBind(2);
    }

    wxBind(): void {
        this.sdkBind(3);
    }

    qqUnBind(): void {
        this.sdkUnBind(1);
    }

    wbUnBind(): void {
        this.sdkUnBind(2);
    }

    wxUnBind(): void {
        this.sdkUnBind(3);
    }

    sdkBind(sdkType): void {
        this.userService.sdkBind(sdkType, this.returnUrl, function (data) {
            if (data.length > 0) {
                window.top.location.href = data;
            }
            else {
                Utils.tipAlert(false, "第三方绑定失败!");
            }
        });
    }

    sdkUnBind(sdkType): void {
        this.userService.sdkUnBind(sdkType, function (data) {
            if (data.isSuccess) {
                Utils.tipAlert(true, "解绑成功!");
                HomeUrlUtils.reload();
            }
            else {
                Utils.tipAlert(false, data.errorMessage);
            }
        });
    }

    constructor() {
        this.userService = new UserService();
    }
}