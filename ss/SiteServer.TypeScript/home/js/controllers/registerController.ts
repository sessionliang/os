/// <reference path="sdkController.ts" />

class RegisterController {
    public userService: UserService;
    public sdkController: SDKController;

    constructor() {
        this.userService = new UserService();
    }

    init(): void {

        this.IsPersistent();

        $("#btnRegister").click(() => {
            this.submitRegister();
        });

        $("#txtUserName").keydown(function (event) {
            if (event.keyCode == 13) {
                $("#btnLogin").click();
            }
        });

        $("#txtPassword").keydown(function (event) {
            if (event.keyCode == 13) {
                $("#btnLogin").click();
            }
        });

        $("#btnLogin").click(() => {
            this.submitLogin();
        });

        $("#ckGreen").change(() => {
            this.ckGreenChange();
        });

        this.blurCheck();

        this.sdkController = new SDKController();
        this.sdkController.init();
    }

    IsPersistent(): void {
        this.userService.IsPersistent((data) => {
            if (data.isSuccess) {
                HomeUrlUtils.redirectToIndex();
            }
        });
    }

    ckGreenChange(): void {
        if ($("#ckGreen:checked").length > 0)
            $("#btnRegister").removeAttr("disabled").removeClass("mfm_submit_disabled").addClass("mfm_submit");
        else
            $("#btnRegister").attr("disabled", "disabled").removeClass("mfm_submit").addClass("mfm_submit_disabled");
    }

    blurCheck(): void {
        $("#txtUserName").blur(() => {
            if (!$("#txtUserName").val()) {
                Utils.tipShow($("#txtUserName"), false, "请输入正确的用户名！");
            }
            else {
                Utils.tipShow($("#txtUserName"), true);
            }
        });

        $("#txtPassword").blur(() => {
            if (!$("#txtPassword").val()) {
                Utils.tipShow($("#txtPassword"), false, "密码不能为空！");
            }
            else {
                Utils.tipShow($("#txtPassword"), true);
            }
        });

        $("#txtEmail").blur(() => {
            if (!$("#txtEmail").val()) {
                Utils.tipShow($("#txtEmail"), false, "邮箱不能为空！");
            }
            else if (!Utils.isEmail($("#txtEmail").val())) {
                Utils.tipShow($("#txtEmail"), false, "请输入正确的邮箱！");
            }
            else {
                Utils.tipShow($("#txtEmail"), true);
            }
        });

        $("#txtConfimPassword").blur(() => {
            if ($("#txtPassword").val() != $("#txtConfimPassword").val()) {
                Utils.tipShow($("#txtConfimPassword"), false, "密码输入不一致重新输入！");
            }
            else {
                Utils.tipShow($("#txtConfimPassword"), true);
            }
        });

        $("#validateCode").blur(() => {
            if (!$("#validateCode").val()) {
                Utils.tipShow($("#validateCode"), false, "请输入正确的验证码！");
            }
            else {
                Utils.tipShow($("#validateCode"), true);
            }
        });
    }

    submitRegister(): void {
        var userName: string = $('#txtUserName').val();
        var email: string = $('#txtEmail').val();
        var password: string = $('#txtPassword').val();
        var confimPassword: string = $('#txtConfimPassword').val();
        var validCode: string = $('#validateCode').val();
        var returnUrl: string = "/wait.html";
        if ($("#ckGreen:checked").length == 0)
            return;
        if (!userName) {
            Utils.tipShow($('#txtUserName'), false, "请输入正确的用户名！");
            return;
        }
        else {
            Utils.tipShow($('#txtUserName'), true);
        }

        if (!email) {
            Utils.tipShow($('#txtEmail'), false, "请输入正确的邮箱！");
            return;
        }
        else if (!Utils.isEmail(email)) {
            Utils.tipShow($('#txtEmail'), false, "请输入正确的邮箱！");
            return;
        }
        else {
            Utils.tipShow($('#txtEmail'), true);
        }

        if (!password) {
            Utils.tipShow($('#txtPassword'), false, "请输入正确的密码！");
            return;
        }
        else {
            Utils.tipShow($('#txtPassword'), true);
        }
        if (!confimPassword) {
            Utils.tipShow($('#txtConfimPassword'), false, "请再一次确认密码！");
            return;
        }
        else if (password != confimPassword) {
            Utils.tipShow($('#txtConfimPassword'), false, "密码输入不一致重新输入！");
            return;
        }
        else {
            Utils.tipShow($('#txtConfimPassword'), true);
        }
        if (!validCode) {
            Utils.tipShow($('#validateCode'), false, "请输入正确的验证码！");
            return;
        }
        else {
            Utils.tipShow($('#validateCode'), true);
        }

        this.userService.register(userName, email, password, validCode, returnUrl,(data) => {
            if (data.isSuccess) {
                var returnUrl = HomeUrlUtils.getReturnUrl();
                if (data.isRedirectToLogin)
                    UrlUtils.redirect(returnUrl);
                else
                    Utils.tipAlert(false, data.successMessage);
            } else {
                Utils.tipAlert(false, data.errorMessage);
            }
        });
    }

    submitLogin(): void {
        var userName: string = $('#txtUserName').val();
        var password: string = $('#txtPassword').val();
        var isPersistent: string = "false";
        if ($('#isPersistent').attr("checked"))
            isPersistent = "true";
        if (!userName) {
            Utils.tipShow($('#txtUserName'), false, "请输入正确的用户名！");
            return;
        }
        else {
            Utils.tipShow($('#txtUserName'), true);
        }
        if (!password) {
            Utils.tipShow($('#txtPassword'), false, "请输入正确的密码！");
            return;
        }
        else {
            Utils.tipShow($('#txtPassword'), true);
        }
        this.userService.login(userName, StringUtils.base64encode(password), isPersistent,true,(data) => {
            if (data.isSuccess) {
                var returnUrl = HomeUrlUtils.getReturnUrl();
                UrlUtils.redirect(returnUrl);
            } else {
                Utils.tipAlert(false, data.errorMessage);
            }
        });
    }


}


