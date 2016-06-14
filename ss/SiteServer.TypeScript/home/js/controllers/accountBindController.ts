class AccountBindController {
    public userService: UserService;
    public sdkController: SDKController;

    constructor() {
        this.userService = new UserService();
    }

    init(): void {
        $("#channelUL").children().eq(2).children().addClass("nav_cuta");
        var locationUrl = window.location.href.toLowerCase();
        if (locationUrl.indexOf("accountbind.html") != -1) {
            $("#accountInfoUrl li a").removeClass("m2menu_cuta");
            $("#accountInfoUrl").children().eq(3).children().addClass("m2menu_cuta");
        }
        this.getBasicUserInfo();
        this.getThirdBindInfo();

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

    getThirdBindInfo(): void {
        this.sdkController = new SDKController();
        this.sdkController.initBind();
    }

}



