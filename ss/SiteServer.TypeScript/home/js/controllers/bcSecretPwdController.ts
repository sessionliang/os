class BcSecretPwdController extends baseController {
    public userService: UserService;
    public queArr: string[];

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
            $("#setSecretPwd").click(function () {
                $(".mlay").slideDown(200);
                $(".mlayBg").show();
            });
            $("#btnUpdate").click(function () {
                $(".mlay").slideDown(200);
                $(".mlayBg").show();
            });

            if (!baseController.user.isSetSQCU) {
                $("#setSecretPwd").html("现在设置");
            }
            else {
                $("#setSecretPwd").html("修改");
            }

            this.getBasicUserInfo();

            this.getSecurityQuestionList("selQuestion1");
            this.getSecurityQuestionList("selQuestion2");
            this.getSecurityQuestionList("selQuestion3");

            this.getValidateSecurityQuestionList("selValidate");

            $("#btnValidateQuestion").click(() => {
                this.validateSecurityQuestion();
            });

            $("#btnSubmitQuestion").click(() => {
                this.updateSecurityQuestion(1);
            });
            $("#btnUpdateQuestion").click(() => {
                this.updateSecurityQuestion(2);
            });

            $("#selQuestion1").unbind("change").bind("change",() => {
                this.checkUserSelectQue("selQuestion1");
            });
            $("#selQuestion2").unbind("change").bind("change",() => {
                this.checkUserSelectQue("selQuestion2");
            });
            $("#selQuestion3").unbind("change").bind("change",() => {
                this.checkUserSelectQue("selQuestion3");
            });
        });
    }

    checkUserSelectQue(selectID: string): boolean {
        var selectedVal = $("#" + selectID).val();
        var selectArr = $("select");
        for (var i = 0; i < selectArr.length; i++) {
            if ($(selectArr[i]).attr("id") == "selValidate")
                continue;
            if ($(selectArr[i]).attr("id") == selectedVal)
                continue;
            if ($(selectArr[i]).attr("id") != selectID && $(selectArr[i]).val() == selectedVal) {
                Utils.tipAlert(false, "不能选择重复的问题，请重新选择");
                $("#" + selectID).val("0");
                return false;
            }
        }
        return true;
    }

    checkUserSelectQues(): boolean {
        return this.checkUserSelectQue("selQuestion1")
            && this.checkUserSelectQue("selQuestion2")
            && this.checkUserSelectQue("selQuestion3");
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

                if (!json.user.isSetSQCU) {
                    $("#setSecretPwd").html("现在设置");
                }
                else {
                    $("#setSecretPwd").html("修改");
                }
                SecretPwdController.isSetSQCU = json.user.isSetSQCU;
            }
        });
    }

    getSecurityQuestionList(selectID: string): void {
        this.userService.getSecurityQuestionList((json) => {
            if (json.isSuccess) {
                $("#" + selectID).html("");
                var selectValue = "0";
                var num = selectID.substr(selectID.length - 1, 1);
                var innerHtml = "<option value='0'>请选择问题</option>";
                for (var i = 0; i < json.securityQuestionList.length; i++) {
                    innerHtml += " <option value=" + json.securityQuestionList[i].id + ">" + json.securityQuestionList[i].question + "</option>";
                    if (json.securityQuestionList[i].question == eval("(json.que" + num + ")")) {
                        selectValue = json.securityQuestionList[i].id;
                    }
                }
                $("#" + selectID).html(innerHtml);
                $("#" + selectID).val(selectValue);
            }
        });
    }

    //获取用户设置的密保问题
    getValidateSecurityQuestionList(selectID: string): void {
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
        });
    }

    updateSecurityQuestion(siteID: number): void {
        if (siteID == 1) {
            var que1: string = $("#selQuestion1").find("option:selected").text();
            var que2: string = $("#selQuestion2").find("option:selected").text();
            var que3: string = $("#selQuestion3").find("option:selected").text();
            var queV1: string = $("#selQuestion1").find("option:selected").val();
            var queV2: string = $("#selQuestion2").find("option:selected").val();
            var queV3: string = $("#selQuestion3").find("option:selected").val();
        }
        else {
            var que1: string = $("#spanQuestion1").html();
            var que2: string = $("#spanQuestion2").html();
            var que3: string = $("#spanQuestion3").html();
        }
        var anw1: string = $('#txtAnswer1').val();
        var anw2: string = $('#txtAnswer2').val();
        var anw3: string = $('#txtAnswer3').val();
        if (queV1 == '0' || queV2 == '0' || queV3 == '0') {
            Utils.tipAlert(false, "问题不能为空");
            return;
        }
        if (!anw1 || !anw2 || !anw3) {
            Utils.tipAlert(false, "答案不能为空");
            return;
        }

        if (!this.checkUserSelectQues()) {
            Utils.tipAlert(false, "密保设置失败-不能选择重复的问题");
            return;
        }
        this.userService.updateSecurityQuestion(que1, que2, que3, anw1, anw2, anw3,(data) => {
            if (data.isSuccess) {
                $(".mlay").slideUp(200);
                $(".mlayBg").hide();
                Utils.tipAlert(true, "密保设置成功！");
                //location.href = "secretpwd2.html";
            } else {
                Utils.tipAlert(false, "密保设置失败-" + data.errorMessage);
            }
        });
        $(".mrclose").click();
    }

    validateSecurityQuestion(): void {
        var que: string = $("#selValidate").find("option:selected").text();
        var anw: string = $('#txtValidateAnswer').val();
        if (que == '0') {
            Utils.tipAlert(false, "问题不能为空");
            return;
        }
        if (!anw) {
            Utils.tipAlert(false, "答案不能为空");
            return;
        }
        this.userService.validateSecurityQuestion(que, anw,(data) => {
            if (data.isSuccess) {
                if (data.isValidate) {
                    //验证通过
                    $("#valDiv").css("display", "none");
                    $("#setDiv").css("display", "block");
                    $("#btnSubmitQuestion").click(() => {
                        this.updateSecurityQuestion(1);
                    });
                    $("#btnUpdateQuestion").click(() => {
                        this.updateSecurityQuestion(2);
                    });
                }
                else {
                    //验证不通过
                    Utils.tipAlert(false, "密保验证失败！请重新输入");
                }
            } else {
                Utils.tipAlert(false, "密保验证失败-" + data.errorMessage);
            }
        });
    }

    getUserSecurityQuestionAnwser(): void {
        this.userService.getUserSecurityQuestionAnwser((json) => {
            if (json.isSuccess) {
                $("#selQuestion1").find("option[text='" + json.que1 + "']").attr("selected", "true");
                $("#selQuestion2").find("option[text='" + json.que2 + "']").attr("selected", "true");
                $("#selQuestion3").find("option[text='" + json.que3 + "']").attr("selected", "true");
            }
        });
    }

    

}


