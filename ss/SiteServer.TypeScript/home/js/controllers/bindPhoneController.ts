class BindPhoneController {
    public userService: UserService;

    constructor() {
        this.userService = new UserService();
    }

    init(): void {
        //$("#channelUL").children().eq(1).children().addClass("nav_cuta");
        //var locationUrl = window.location.href.toLowerCase();;
        //if (locationUrl.indexOf("bindphone.html") != -1) {
        //    $("#accountSafeUrl li a").removeClass("m2menu_cuta");
        //    $("#accountSafeUrl").children().eq(1).children().addClass("m2menu_cuta");
        //}

        /*弹窗 开始*/
        $(".mlayBg").height($(document).height());
        $(".mbody2").css("padding-top",($(document).height() - 655) / 2);
        $(".mbody3").css("padding-top",($(document).height() - 755) / 2);
        $(window).resize(function () {
            $(".mlayBg").height($(document).height());
            $(".mbody2").css("padding-top",($(document).height() - 655) / 2);
            $(".mbody3").css("padding-top",($(document).height() - 755) / 2);
        });
        $(".mrclose").click(function () {
            $(".mlay").slideUp(200);
            $(".mlayBg").hide();
        });


        this.getEnablePathListForMessage();


        /*弹窗 结束*/

        this.getBasicUserInfo();

        //提交绑定
        $("#btnBindPhone").click(() => {
            this.bindPhoneValidate();
        });

        //发送验证码
        $("#btnSendPhone").click(() => {
            this.bindPhone();
        });
    }

    getEnablePathListForMessage(): void {
        this.userService.getEnablePathListForMessage((json) => {
            if (json.isSuccess) {
                var enable = false;
                for (var i = 0; i < json.list.length; i++) {
                    if (json.list[i] == "ByPhone") {
                        $("#btnBindPhoneOpen").click(function () {
                            $(".mlay").slideDown(200);
                            $(".mlayBg").show();
                        });
                        enable = true;
                        break;
                    }
                }
                if (!enable) {
                    $("#btnBindPhoneOpen").unbind("click");
                    $("#btnBindPhoneOpen").css("background", "#BFBFBF");
                    $("#btnBindPhoneOpen").html("未开通");
                }
            }
        });
    }

    getBasicUserInfo(): void {
        this.userService.getUser((json) => {
            if (json.isAnonymous) {
                HomeUrlUtils.redirectToLogin();
            }
            else {

                $("#spanUserName").html(json.user.userName);
                $("#spanUserName").attr("href", HomeUrlUtils.homeUrl);
                if (json.user.hasNewMsg) {
                    $("#userMsgTip").css("display", "inline");
                    $("#userMsgCount").html(json.user.newMsgCount);
                }
                $("#btnLogout").click((e) => {
                    this.userService.logout(() => {
                        HomeUrlUtils.redirectToLogin(HomeUrlUtils.homeUrl);
                    });
                });

                //绑定信息
                if (json.user.isBindPhone) {
                    $("#phone").html(json.user.mobile);

                    $("#btnBindPhoneOpen").css("display", "none").unbind("click");

                    $("#btnRemoveBindPhoneOpen").css("display", "inline").click(() => {
                        this.removeBindPhone();
                    });

                    $("#btnReBindPhoneOpen").css("display", "inline").click(() => {
                        $(".mlay").slideDown(200);
                        $(".mlayBg").show();
                    });
                }
            }
        });
    }

    bindPhoneValidate(): void {
        var phoneNum = $("#phoneNum").val();
        var validateCode = $("#validateCode").val();
        if (!phoneNum) {
            Utils.tipAlert(false, "请填写手机号");
            return;
        }
        if (!validateCode) {
            Utils.tipAlert(false, "请填写验证码");
            return;
        }
        this.userService.bindPhoneValidate(phoneNum, validateCode, function (data) {
            if (data.isSuccess) {
                Utils.tipAlert(true, "绑定成功");
                HomeUrlUtils.reload();
            }
            else {
                Utils.tipAlert(false, data.errorMessage);
            }
        });
    }

    bindPhone(): void {
        var phoneNum = $("#phoneNum").val();
        if (!phoneNum) {
            Utils.tipAlert(false, "请填写手机号");
            return;
        }
        this.userService.bindPhone(phoneNum, function (data) {
            if (data.isSuccess) {
                Utils.tipAlert(true, "校验码已经发送到您的手机，请注意查收");
                $("#btnSendPhone").css("display", "none").unbind("click");
                var counterHtml = '<span class="mlay_alr2" id="spanMessage">120秒后可重新获取</span>';
                $(counterHtml).insertAfter($("#validateCode"));
                var interval = 1000;
                var counter = 120;
                var interl = setInterval(function () {
                    counter--;
                    $("#spanMessage").html(counter + "秒后可重新获取");
                    if (counter == 0) {
                        clearInterval(interl);
                        $("#spanMessage").remove();
                        $("#btnSendPhone").css("display", "inline").click(() => {
                            this.bindPhone();
                        });
                    }
                }, interval);
            }
            else {
                Utils.tipAlert(false, data.errorMessage);
            }
        });
    }

    removeBindPhone(): void {
        this.userService.removeBindPhone(function (data) {
            if (data.isSuccess) {
                Utils.tipAlert(true, "解除绑定成功");
                HomeUrlUtils.reload();
            }
            else {
                Utils.tipAlert(false, data.errorMessage);
            }
        });
    }

}


