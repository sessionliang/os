class LoginEmailController {
    public userService: UserService;

    constructor() {
        this.userService = new UserService();
    }

    init(): void {
        //$("#channelUL").children().eq(1).children().addClass("nav_cuta");
        //var locationUrl = window.location.href.toLowerCase();;
        //if (locationUrl.indexOf("loginemail.html") != -1) {
        //    $("#accountSafeUrl li a").removeClass("m2menu_cuta");
        //    $("#accountSafeUrl").children().eq(2).children().addClass("m2menu_cuta");
        //}

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

        this.getBasicUserInfo();

        $("#btnSetEmail").click(function () {
            $(".mlay").slideDown(200);
            $(".mlayBg").show();
        });
        $("#btnUpdateEmail").click(function () {
            $(".mlay").slideDown(200);
            $(".mlayBg").show();
        });
        $("#btnSendEmail").click(() => {
            this.bindEmail();
        });

        $("#btnSubmitEmail").click(() => {
            this.SendEmail();
        });

        this.getEnablePathListForMessage();

    }

    getEnablePathListForMessage(): void {
        this.userService.getEnablePathListForMessage((json) => {
            if (json.isSuccess) {
                var enable = false;
                for (var i = 0; i < json.list.length; i++) {
                    if (json.list[i] == "ByEmail") {
                        $("#btnSetEmail").click(function () {
                            $(".mlay").slideDown(200);
                            $(".mlayBg").show();
                        });
                        $("#btnUpdateEmail").click(function () {
                            $(".mlay").slideDown(200);
                            $(".mlayBg").show();
                        });
                        enable = true;
                        break;
                    }
                }
                if (!enable) {
                    $("#btnSetEmail").unbind("click");
                    $("#btnSetEmail").css("background", "#BFBFBF");
                    $("#btnSetEmail").html("未开通");
                    $("#btnUpdateEmail").unbind("click");
                    $("#btnUpdateEmail").css("background", "#BFBFBF");
                    $("#btnUpdateEmail").html("未开通");
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
                $("#spanUserBindEmail").html(json.user.email);
                if (!json.user.email) {
                    $("#divNoBindEmail").show();
                    $("#divBindEmail").hide();
                }
                else if (json.user.email && !json.user.isBindEmail) {
                    $("#divNoBindEmail").hide();
                    $("#divBindEmail").show();
                    $("#btnUpdateEmail").html("绑定");
                    $("#txtSendEmail").val(json.user.email);
                }
                else {
                    $("#divNoBindEmail").hide();
                    $("#divBindEmail").show();
                    $("#btnUpdateEmail").html("修改");
                }
                if (json.user.hasNewMsg) {
                    $("#userMsgTip").css("display", "inline");
                    $("#userMsgCount").html(json.user.newMsgCount);
                }

                $("#btnLogout").click((e) => {
                    this.userService.logout(() => {
                        HomeUrlUtils.redirectToLogin(HomeUrlUtils.homeUrl);
                    });
                });
            }
        });
    }

    bindEmail(): void {
        var email = $("#txtSendEmail").val();
        if (!Utils.isEmail(email)) {
            Utils.tipAlert(false, "请填写正确的邮箱！");
            return;
        }
        this.userService.bindEmail(email,(json) => {
            if (json.isSuccess) {
                Utils.tipAlert(true, "发送成功,请登录邮箱查看");
                $("#btnSendEmail").css("display", "none").unbind("click");
                var counterHtml = '<span class="mlay_alr2" id="spanMessage">120秒后可重新获取</span>';
                $(counterHtml).insertAfter($("#txtValidateCode"));
                var interval = 1000;
                var counter = 120;
                var interl = setInterval(function () {
                    counter--;
                    $("#spanMessage").html(counter + "秒后可重新获取");
                    if (counter == 0) {
                        clearInterval(interl);
                        $("#spanMessage").remove();
                        $("#btnSendEmail").css("display", "inline").click(() => {
                            this.bindEmail();
                        });
                    }
                }, interval);
            }
            else {
                Utils.tipAlert(false, "发送失败，" + json.errorMessage);
            }
        });
    }

    SendEmail(): void {
        var email = $("#txtSendEmail").val();
        var validateCode = $("#txtValidateCode").val();
        if (!email || !validateCode) {
            Utils.tipAlert(false, "邮箱绑定数据填写不完整！");
            return;
        }
        this.userService.bindEmailValidate(email, validateCode,(json) => {
            if (json.isSuccess) {
                $(".mlay").hide();
                Utils.tipAlert(true, "邮箱绑定成功！");
                HomeUrlUtils.reload();
            }
            else {
                Utils.tipAlert(false, json.errorMessage);
            }
        });
    }

}


