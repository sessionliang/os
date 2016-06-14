class TrialReportMessageController {
    public userService: UserService;
    public TrialReportService: TrialService;

    constructor() {
        this.userService = new UserService();
        this.TrialReportService = new TrialService();
    }

    init(): void {
        //$("#channelUL").children().eq(3).children().addClass("nav_cuta");
        //var locationUrl = window.location.href.toLowerCase();;
        //if (locationUrl.indexOf("message.html") != -1) {
        //    $("#accountMsgUrl li a").removeClass("m2menu_cuta");
        //    $("#accountMsgUrl").children().eq(0).children().addClass("m2menu_cuta");
        //}
        this.getBasicUserInfo();
        this.getTrialReportMessage(1, 10);
    }

    getBasicUserInfo(): void {
        this.userService.getUser((json) => {
            if (json.isAnonymous) {
                HomeUrlUtils.redirectToLogin();
            }
            else {

                $("#spanUserName").html(json.user.userName);
                $("#spanUserName").attr("href", HomeUrlUtils.homeUrl);
                
                $("#btnLogout").click((e) => {
                    this.userService.logout(() => {
                        HomeUrlUtils.redirectToLogin(HomeUrlUtils.homeUrl);
                    });
                });
            }
        });
    }

    getTrialReportMessage(pageIndex: number, prePageNum: number): void {
        this.TrialReportService.getUserTrialReportRecord(pageIndex, prePageNum,(json) => {
            if (json.isSuccess) {
                console.log(json);
                $("#ulMessage").html("");
                var innerHtml = "";
                for (var i = 0; i < json.infoList.length; i++) {
                    innerHtml += StringUtils.format("<li>{0} 提交了<a href='{2}' href='_blank'>{1}</a>试用活动的活动报告</li>", json.infoList[i].addDate, json.infoList[i].contentTitle, json.infoList[i].redirectUrl);
                }
                $("#ulMessage").html(innerHtml);
            }
            else {
                Utils.tipAlert(false, "获取消息出错-" + json.errorMessage);
            }

            //分页链接
            var pageHtml = pageDataUtils.getPageHtml(json.pageJson, 'getTrialReportMessageData');
            if (!!pageHtml)
                $("#divPageLink").html(pageHtml);

        });
    }
}


