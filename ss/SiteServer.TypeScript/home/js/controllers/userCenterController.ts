class UserCenterController {
    public userService: UserService;

    constructor() {
        this.userService = new UserService();
    }


    init(): void {
        //$("#channelUL").children().eq(2).children().addClass("nav_cuta");
        //var locationUrl = window.location.href.toLowerCase();;
        //if (locationUrl.indexOf("info.html") != -1) {
        //    $("#accountInfoUrl li a").removeClass("m2menu_cuta");
        //    $("#accountInfoUrl").children().eq(1).children().addClass("m2menu_cuta");
        //}


        this.getBasicUserInfo();
        $("#btnSaveUserInfo").click(() => {
            this.saveBasicUserInfo();
        });
    }

    saveBasicUserInfo(): void {
        var userName: string = $("#txtUserName").val();
        //var email: string = $("#txtEmail").val();
        //var phone: string = $("#txtPhone").val();
        var remark: string = $("#txtRemark").val();
        if (!userName) {
            Utils.tipAlert(false, "请输入正确的用户名！");
            return;
            this.userService.getUser((json) => {
                if (userName != json.user.userName)
                    Utils.tipAlert(false, "用户名不能被修改！");
                return;
            });
        }

        //if (!email && !Utils.isEmail(email)) {
        //    Utils.tipAlert("请输入正确格式的邮箱！");
        //    return;
        //}

        //if (!phone && !Utils.isMobile(phone)) {
        //    Utils.tipAlert("请输入正确格式的手机号码！");
        //    return;
        //}

        this.userService.updateBasicUserInfo(userName, remark, (data) => {
            if (data.isSuccess) {
                Utils.tipAlert(true, "用户的基本信息修改成功！");
            }
            else {
                Utils.tipAlert(false, data.errorMessage);
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
                $("#txtUserName").val(json.user.userName);
                $("#txtEmail").val(json.user.email);
                $("#txtPhone").val(json.user.mobile);
                $("#txtRemark").val(json.user.signature);
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

}


