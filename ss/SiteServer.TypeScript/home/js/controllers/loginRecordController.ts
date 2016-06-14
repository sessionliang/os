class LoginRecordController {
    public userService: UserService;

    constructor() {
        this.userService = new UserService();
    }

    init(): void {
        //$("#channelUL").children().eq(1).children().addClass("nav_cuta");
        //var locationUrl = window.location.href.toLowerCase();;
        //if (locationUrl.indexOf("loginrecord.html") != -1) {
        //    $("#accountSafeUrl li a").removeClass("m2menu_cuta");
        //    $("#accountSafeUrl").children().eq(4).children().addClass("m2menu_cuta");
        //}
        this.getBasicUserInfo();
        this.getUserLoginLog(1, 10);
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

}


