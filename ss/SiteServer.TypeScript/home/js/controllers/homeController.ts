class HomeController {
    public userService: UserService;

    constructor() {
        this.userService = new UserService();
    }

    init(): void {
        //$("#channelUL").children().eq(0).children().addClass("nav_cuta");

        this.getBasicUserInfo();
        this.getUserLoginLog(1, 10);
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
                $("#divUserName").html(json.user.userName);
                $("#userImg").attr("src", json.user.avatarMiddle);
                $("#divLastLoginTime").html(json.user.lastActivityDate.replace("T", "  "));
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

    getUserLoginLog(pageIndex: number, prePageNum: number): void {

        this.userService.getUserLoginLog(pageIndex, prePageNum,(json) => {
            if (json.isSuccess) {
                $("#tbUserLog").html("");
                var innerHtml = "<tr class='m2r_th'><td>IP地址</td><td>日期</td><td>备注</td></tr>";//<td>城市</td>

                for (var i = 0; i < json.userLoginInfoList.length; i++) {
                    innerHtml += "<tr>";
                    innerHtml += "<td>" + json.userLoginInfoList[i].ipAddress + "</td>";
                    innerHtml += "<td>" + Utils.formatTime(json.userLoginInfoList[i].addDate.replace("T", "  "), "yyyy-MM-dd HH:mm:ss") + "</td>";
                    //innerHtml += "<td>" + json.userLoginInfoList[i].city + "</td>";
                    innerHtml += "<td>" + json.userLoginInfoList[i].summary + "</td>";
                    innerHtml += "</tr>";
                }

                $("#tbUserLog").html(innerHtml);

                //分页链接
                $("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getUserLoginLogData'));
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

                    $(".bindEmail").html("修改");
                    $("#iconEmail").addClass("m2r_ico1");
                    $("#iconEmail").removeClass("m2r_ico2");
                }
                else {
                    $(".bindEmail").html("绑定");
                    $("#iconEmail").removeClass("m2r_ico1");
                    $("#iconEmail").addClass("m2r_ico2");
                }
                if (json.isBindPhone) {
                    $(".bindPhone").html("修改");
                    $("#iconPhone").addClass("m2r_ico1");
                    $("#iconPhone").removeClass("m2r_ico2");
                }
                else {
                    $(".bindPhone").html("绑定");
                    $("#iconPhone").removeClass("m2r_ico1");
                    $("#iconPhone").addClass("m2r_ico2");
                }
                if (json.isSetSQCU) {
                    $(".bindQsuc").html("修改");
                    $("#iconQscu").addClass("m2r_ico1");
                    $("#iconQscu").removeClass("m2r_ico2");
                }
                else {
                    $(".bindQsuc").html("绑定");
                    $("#iconQscu").removeClass("m2r_ico1");
                    $("#iconQscu").addClass("m2r_ico2");
                }
                if (json.pwdComplex) {
                    $(".bindpwd").html("修改");
                    $("#iconPwd").addClass("m2r_ico1");
                    $("#iconPwd").removeClass("m2r_ico2");
                }
                else {
                    $(".bindpwd").html("绑定");
                    $("#iconPwd").removeClass("m2r_ico1");
                    $("#iconPwd").addClass("m2r_ico2");
                }
            }
        });
    }
}



