class BcBindEmailController extends baseController {
    public userService: UserService;

    constructor() {
        super();
        this.userService = new UserService();
    }

    init(): void {

        baseController.userAuthValidate(() => {

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


            $("#spanUserBindEmail").html(baseController.user.email);


            if (!baseController.user.email) {
                $("#divNoBindEmail").show();
                $("#divBindEmail").hide();
            }
            else if (baseController.user.email && !baseController.user.isBindEmail) {
                $("#divNoBindEmail").hide();
                $("#divBindEmail").show();
                $("#btnUpdateEmail").html("现在绑定");
                $("#txtSendEmail").val(baseController.user.email);
            }
            else {
                $("#divNoBindEmail").hide();
                $("#divBindEmail").show();
                $("#btnUpdateEmail").html("修改绑定");
            }


            $("#btnSetEmail").click(function () {
                $("#verifybox").slideDown(200);
            });
            $("#btnUpdateEmail").click(function () {
                $("#verifybox").slideDown(200);
            });
            $("#btnSendEmail").click(() => {
                this.bindEmail();
            });

            $("#btnSubmitEmail").click(() => {
                this.SendEmail();
            });
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


