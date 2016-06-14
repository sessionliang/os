class HelpCenterController {
    public userService: UserService;

    constructor() {
        this.userService = new UserService();
    }

    init(): void {
        //$("#channelUL").children().eq(4).children().addClass("nav_cuta");
        //var locationUrl = window.location.href.toLowerCase();;
        //if (locationUrl.indexOf("help1.html") != -1) {
        //    $("#accountHelpUrl li a").removeClass("m2menu_cuta");
        //    $("#accountHelpUrl").children().eq(0).children().addClass("m2menu_cuta");
        //}
        //if (locationUrl.indexOf("help2.html") != -1) {
        //    $("#accountHelpUrl li a").removeClass("m2menu_cuta");
        //    $("#accountHelpUrl").children().eq(1).children().addClass("m2menu_cuta");
        //}
        this.getBasicUserInfo();

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

}


