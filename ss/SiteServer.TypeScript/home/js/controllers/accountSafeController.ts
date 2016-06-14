class AccountSafeController {
    public userService: UserService;

    constructor() {
        this.userService = new UserService();
    }

    init(): void {
        $("#channelUL").children().eq(1).children().addClass("nav_cuta");
        var locationUrl = window.location.href.toLowerCase();;
        if (locationUrl.indexOf("accountsafe.html") != -1) {
            $("#accountSafeUrl li a").removeClass("m2menu_cuta");
            $("#accountSafeUrl").children().eq(3).children().addClass("m2menu_cuta");
        }
        this.getBasicUserInfo();
        this.accountSafeLevel();
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
            }
        });
    }

    accountSafeLevel(): void {
        this.userService.accountSafeLevel((json) => {
            if (json.isSuccess) {
                if (json.level == 1) {
                    $("#spanAccountLevel").html("低");
                    $("#spanAccountNum").attr("style", "width:30%");
                }
                if (json.level == 2) {
                    $("#spanAccountLevel").html("中");
                    $("#spanAccountNum").attr("style", "width:60%");
                }
                if (json.level == 3) {
                    $("#spanAccountLevel").html("高");
                    $("#spanAccountNum").attr("style", "width:90%");
                }

                if (json.isBindEmai) {

                    $("#bindEmail").html("修改");
                    $("#iconEmail").addClass("m2r_ico1");
                    $("#iconEmail").removeClass("m2r_ico2");
                }
                else {
                    $("#bindEmail").html("绑定");
                    $("#iconEmail").removeClass("m2r_ico1");
                    $("#iconEmail").addClass("m2r_ico2");
                }
                if (json.isBindPhone) {
                    $("#bindPhone").html("修改");
                    $("#iconPhone").addClass("m2r_ico1");
                    $("#iconPhone").removeClass("m2r_ico2");
                }
                else {
                    $("#bindPhone").html("绑定");
                    $("#iconPhone").removeClass("m2r_ico1");
                    $("#iconPhone").addClass("m2r_ico2");
                }
                if (json.isSetSQCU) {
                    $("#bindQsuc").html("修改");
                    $("#iconQscu").addClass("m2r_ico1");
                    $("#iconQscu").removeClass("m2r_ico2");
                }
                else {
                    $("#bindQsuc").html("绑定");
                    $("#iconQscu").removeClass("m2r_ico1");
                    $("#iconQscu").addClass("m2r_ico2");
                }
                if (json.pwdComplex) {
                    $("#bindpwd").html("修改");
                    $("#iconPwd").addClass("m2r_ico1");
                    $("#iconPwd").removeClass("m2r_ico2");
                }
                else {
                    $("#bindpwd").html("绑定");
                    $("#iconPwd").removeClass("m2r_ico1");
                    $("#iconPwd").addClass("m2r_ico2");
                }
            }
        });
    }
}


