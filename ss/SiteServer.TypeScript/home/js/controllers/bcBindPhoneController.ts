class BcBindPhoneController extends baseController {
    public userService: UserService;

    constructor() {
        super();
        this.userService = new UserService();
    }

    init(): void {

        baseController.userAuthValidate(() => {
            if (baseController.user.isBindPhone) {
                $("#phone").html(baseController.user.mobile);

                $("#btnRemoveBindPhoneOpen").css("display", "inline").click(() => {
                    this.removeBindPhone();
                });

                $("#btnReBindPhoneOpen").css("display", "inline").click(() => {
                    $('#verifybox').show();
                });
            }

            $("#btnBindPhoneOpen").click(function () {
                $('#verifybox').show();
            });

            //提交绑定
            $("#btnBindPhone").click(() => {
                this.bindPhoneValidate();
            });

            //发送验证码
            $("#btnSendPhone").click(() => {
                this.bindPhone();
            });
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


