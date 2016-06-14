class FindPwdController {
    public userService: UserService;

    constructor() {
        this.userService = new UserService();
    }

    init(): void {
        $("#btnSubmitFindStep1").click(() => {
            this.findPasswordByEmailStep1();
        });

        $("#btnSumbitNewPwd").click(() => {
            var findTypeValue = $(':radio[name="findType"]:checked').val();
            if (findTypeValue == 1) {
                this.findPassword("phone");
            }
            else if (findTypeValue == 2) {
                this.findPassword("email");
            }
            else if (findTypeValue == 3) {
                this.findPassword("sqcu");
            }
        });


        //this.getSecurityQuestionList("selQuestion1");
        //this.getSecurityQuestionList("selQuestion2");
        //this.getSecurityQuestionList("selQuestion3");
        this.getEnablePathList();


        $("#btnSumbitQue").click(() => {
            this.findPasswordBySCQUStep2();
        });

    }

    findPasswordByEmailStep1(): void {
        var txtFindPwdUserName = $("#txtFindPwdUserName").val();
        var txtValidateCode = $("#validateCode").val();
        if (!txtFindPwdUserName) {
            Utils.tipAlert(false, "请填写要找回密码的用户名！");
            return;
        }
        if (!txtValidateCode) {
            Utils.tipAlert(false, "请填写验证码！");
            return;
        }
        var findTypeValue = $(':radio[name="findType"]:checked').val();
        if (!findTypeValue) {
            Utils.tipAlert(false, "请选择找回密码的方式！");
            return;
        }

        if (findTypeValue == 1) {
            this.userService.findPasswordByPhoneStep1(txtFindPwdUserName,(json) => {
                if (json.isSuccess) {
                    $("#btnSendEmail").unbind("click").click(() => {
                        this.findPasswordByPhoneStep2();
                    });
                    $("#btnSubmitEmail").unbind("click").click(() => {
                        this.findPasswordByPhoneStep3();
                    });
                    $("#login").attr("placeholder", "请输入您的登录手机");
                    $("#loginKey").html("登录手机：");
                    $("#loginDes").html("我们会向您的手机发送一封验证码，请注意查收。");
                    $("#findPwdStep1").hide();
                    $("#findPwdStep2").show();
                    $("#findPwdStep3").hide();
                    $("#findPwdStep4").hide();
                    $("#findPwdStep5").hide();
                    $("#findStepKey1").val(json.key);

                    $("#btnPre1").click(function () {
                        $("#findPwdStep2").hide();
                        $("#findPwdStep1").show();
                    });
                }
                else {
                    Utils.tipAlert(false, json.errorMessage);
                }
            }, txtValidateCode);
        }
        if (findTypeValue == 2) {
            this.userService.findPasswordByEmailStep1(txtFindPwdUserName,(json) => {
                if (json.isSuccess) {
                    $("#btnSendEmail").unbind("click").click(() => {
                        this.findPasswordByEmailStep2();
                    });
                    $("#btnSubmitEmail").unbind("click").click(() => {
                        this.findPasswordByEmailStep3();
                    });
                    $("#login").attr("placeholder", "请输入您的登录邮箱");
                    $("#loginKey").html("登录邮箱：");
                    $("#loginDes").html("我们会向您的邮箱发送一封验证码，请注意查收。");
                    $("#findPwdStep1").hide();
                    $("#findPwdStep2").show();
                    $("#findPwdStep3").hide();
                    $("#findPwdStep4").hide();
                    $("#findPwdStep5").hide();
                    $("#findStepKey1").val(json.key);

                    $("#btnPre1").click(function () {
                        $("#findPwdStep2").hide();
                        $("#findPwdStep1").show();
                    });
                }
                else {
                    Utils.tipAlert(false, json.errorMessage);
                }
            }, txtValidateCode);
        }
        if (findTypeValue == 3) {
            this.userService.findPasswordBySCQUStep1(txtFindPwdUserName,(json) => {
                if (json.isSuccess) {
                    $("#findPwdStep1").hide();
                    $("#findPwdStep2").hide();
                    $("#findPwdStep3").show();
                    $("#findPwdStep4").hide();
                    $("#findPwdStep5").hide();
                    $("#findStepKey1").val(json.key);

                    this.getValidateSecurityQuestionList("selValidate", txtFindPwdUserName);
                    $("#btnPre2").click(function () {
                        $("#findPwdStep3").hide();
                        $("#findPwdStep1").show();
                    });
                }
                else {
                    Utils.tipAlert(false, json.errorMessage);
                }
            }, txtValidateCode);
        }
    }

    findPasswordByEmailStep2(): void {
        var txtFindPwdUserName = $("#txtFindPwdUserName").val();
        var txtFindPwdEmail = $("#txtFindPwdByEmail").val();
        if (!txtFindPwdEmail) {
            Utils.tipAlert(false, "请填写需要验证的邮箱！");
            return;
        }
        else if (!Utils.isEmail(txtFindPwdEmail)) {
            Utils.tipAlert(false, "请填写正确格式的邮箱！");
            return;
        }
        var key = $("#findStepKey1").val();

        this.userService.findPasswordByEmailStep2(txtFindPwdUserName, txtFindPwdEmail, key,(json) => {
            if (json.isSuccess) {
                $("#findStepKey2").val(json.key);
                $("#spanSendEmail").html("邮件发送成功请查收！");
            }
            else {
                Utils.tipAlert(false, json.errorMessage);
            }
        });
    }

    findPasswordByEmailStep3(): void {
        var txtFindPwdUserName = $("#txtFindPwdUserName").val();
        var txtFindPwdEmail = $("#txtFindPwdByEmail").val();
        if (!txtFindPwdUserName) {
            Utils.tipAlert(false, "请填写需要验证的邮箱！");
            return;
        }
        var txtValidateCode = $("#txtValidateCode").val();
        var key = $("#findStepKey2").val();

        this.userService.findPasswordByEmailStep3(txtFindPwdUserName, txtValidateCode, txtFindPwdEmail, key,(json) => {
            if (json.isSuccess) {
                $("#findPwdStep1").hide();
                $("#findPwdStep2").hide();
                $("#findPwdStep3").hide();
                $("#findPwdStep4").show();
                $("#findPwdStep5").hide();
                $("#findStepKey0").val(json.key);

                $("#btnPre3").click(function () {
                    $("#findPwdStep4").hide();
                    var findTypeValue = $(':radio[name="findType"]:checked').val();
                    if (findTypeValue == 1 || findTypeValue == 2) {
                        $("#findPwdStep2").show();
                    }
                    else if (findTypeValue == 3) {
                        $("#findPwdStep3").show();
                    }

                });
            }
            else {
                Utils.tipAlert(false, json.errorMessage);
            }
        });
    }

    findPasswordByPhoneStep2(): void {
        var txtFindPwdUserName = $("#txtFindPwdUserName").val();
        var txtFindPwdPhone = $("#txtFindPwdByEmail").val();
        if (!txtFindPwdPhone) {
            Utils.tipAlert(false, "请填写需要验证的手机号！");
            return;
        }
        else if (!Utils.isMobile(txtFindPwdPhone)) {
            Utils.tipAlert(false, "请填写正确格式的手机号！");
            return;
        }
        var key = $("#findStepKey1").val();

        this.userService.findPasswordByPhoneStep2(txtFindPwdUserName, txtFindPwdPhone, key,(json) => {
            if (json.isSuccess) {
                $("#findStepKey2").val(json.key);
                Utils.tipAlert(true, "邮件发送成功请查收！");
            }
            else {
                Utils.tipAlert(false, json.errorMessage);
            }
        });
    }

    findPasswordByPhoneStep3(): void {
        var txtFindPwdUserName = $("#txtFindPwdUserName").val();
        var txtFindPwdPhone = $("#txtFindPwdByEmail").val();
        if (!txtFindPwdPhone) {
            Utils.tipAlert(false, "请填写需要验证的手机号！");
            return;
        }
        var txtValidateCode = $("#txtValidateCode").val();
        if (!txtValidateCode) {
            Utils.tipAlert(false, "请填写短信验证码！");
            return;
        }
        var key = $("#findStepKey2").val();

        this.userService.findPasswordByPhoneStep3(txtFindPwdUserName, txtValidateCode, txtFindPwdPhone, key,(json) => {
            if (json.isSuccess) {
                $("#findPwdStep1").hide();
                $("#findPwdStep2").hide();
                $("#findPwdStep3").hide();
                $("#findPwdStep4").show();
                $("#findPwdStep5").hide();
                $("#findStepKey0").val(json.key);

                $("#btnPre3").click(function () {
                    $("#findPwdStep4").hide();
                    var findTypeValue = $(':radio[name="findType"]:checked').val();
                    if (findTypeValue == 1 || findTypeValue == 2) {
                        $("#findPwdStep2").show();
                    }
                    else if (findTypeValue == 3) {
                        $("#findPwdStep3").show();
                    }

                });
            }
            else {
                Utils.tipAlert(false, json.errorMessage);
            }
        });
    }

    findPassword(type: string): void {
        var userName = $("#txtFindPwdUserName").val();
        var email = $("#txtFindPwdByEmail").val();
        var key = $("#findStepKey0").val();
        var newPassword = $("#txtNewPwd").val();
        var newConfimPwd = $("#txtNewConfimPwd").val();
        if (!newPassword) {
            Utils.tipAlert(false, "请输入新的密码");
            return;
        }
        if (!newConfimPwd) {
            Utils.tipAlert(false, "请再一次输入新的密码");
            return;
        }
        if (newPassword != newConfimPwd) {
            Utils.tipAlert(false, "两次输入的密码不一致");
            return;
        }
        this.userService.findPassword(newPassword, userName, email, type, key,(json) => {
            if (json.isSuccess) {
                $("#findPwdStep1").hide();
                $("#findPwdStep2").hide();
                $("#findPwdStep3").hide();
                $("#findPwdStep4").hide();
                $("#findPwdStep5").show();
            }
            else {
                Utils.tipAlert(false, json.errorMessage);
            }
        });
    }

    //获取用户设置的密保问题
    getValidateSecurityQuestionList(selectID: string, userName: string): void {
        this.userService.getSecurityQuestionList((json) => {
            if (json.isSuccess) {
                $("#" + selectID).html("");
                var innerHtml = "<option value='0'>请选择问题</option>";
                for (var i = 0; i < json.securityQuestionList.length; i++) {
                    if (json.securityQuestionList[i].question == eval("(json.que1)")) {
                        innerHtml += " <option value=" + json.securityQuestionList[i].id + ">" + json.securityQuestionList[i].question + "</option>";
                    }
                    else if (json.securityQuestionList[i].question == eval("(json.que2)")) {
                        innerHtml += " <option value=" + json.securityQuestionList[i].id + ">" + json.securityQuestionList[i].question + "</option>";
                    }
                    else if (json.securityQuestionList[i].question == eval("(json.que3)")) {
                        innerHtml += " <option value=" + json.securityQuestionList[i].id + ">" + json.securityQuestionList[i].question + "</option>";
                    }
                }
                $("#" + selectID).html(innerHtml);
            }
        }, userName);
    }

    getSecurityQuestionList(selectID: string): void {
        this.userService.getSecurityQuestionList((json) => {
            if (json.isSuccess) {
                $("#" + selectID).html("");
                var innerHtml = "<option value='0'>请选择问题</option>";
                for (var i = 0; i < json.securityQuestionList.length; i++) {
                    innerHtml += " <option value=" + json.securityQuestionList[i].id + ">" + json.securityQuestionList[i].question + "</option>";
                }
                $("#" + selectID).html(innerHtml);
            }
        });
    }

    getEnablePathList(): void {
        this.userService.getEnablePathList((json) => {
            if (json.isSuccess) {
                for (var i = 0; i < json.list.length; i++) {
                    $("#" + json.list[i]).css("display", "");
                }
            }
        });
    }

    findPasswordBySCQUStep2(): void {
        var userName = $("#txtFindPwdUserName").val();
        var txtFindPwdEmail = $("#txtFindPwdByEmail").val();
        var KeyStep1 = $("#findStepKey1").val();
        var que: string = $("#selValidate").find("option:selected").text();
        var anw: string = $('#txtValidateAnswer').val();
        this.userService.findPasswordBySCQUStep2(userName, KeyStep1, que, anw,(json) => {
            if (json.isSuccess) {
                $("#findPwdStep1").hide();
                $("#findPwdStep2").hide();
                $("#findPwdStep3").hide();
                $("#findPwdStep4").show();
                $("#findPwdStep5").hide();
                $("#findStepKey0").val(json.key);

                $("#btnPre3").click(function () {
                    $("#findPwdStep4").hide();
                    var findTypeValue = $(':radio[name="findType"]:checked').val();
                    if (findTypeValue == 1 || findTypeValue == 2) {
                        $("#findPwdStep2").show();
                    }
                    else if (findTypeValue == 3) {
                        $("#findPwdStep3").show();
                    }

                });
            }
            else {
                Utils.tipAlert(false, json.errorMessage);
            }
        });
    }
}


