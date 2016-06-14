class EvaluationMessageController {
    public userService: UserService;
    public evaluationService: EvaluationService;

    constructor() {
        this.userService = new UserService();
        this.evaluationService = new EvaluationService();
    }

    init(): void {
        //$("#channelUL").children().eq(3).children().addClass("nav_cuta");
        //var locationUrl = window.location.href.toLowerCase();;
        //if (locationUrl.indexOf("message.html") != -1) {
        //    $("#accountMsgUrl li a").removeClass("m2menu_cuta");
        //    $("#accountMsgUrl").children().eq(0).children().addClass("m2menu_cuta");
        //}
        this.getBasicUserInfo();
        this.getEvaluationMessage(1, 10);
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

    getEvaluationMessage(pageIndex: number, prePageNum: number): void {
        this.evaluationService.getUserEvaluationRecord(pageIndex, prePageNum, (json) => {
            if (json.isSuccess) {
                $("#ulMessage").html("");
                var innerHtml = "";
                for (var i = 0; i < json.EvaluationMessageList.length; i++) {
                    innerHtml += StringUtils.format("<li>{0} 针对<a href='{2}' href='_blank'>{1}</a>进行了评价</li>", json.infoList[i].addDate, json.infoList[i].contentTitle, json.infoList[i].redirectUrl);
                }
                $("#ulMessage").html(innerHtml);
            }
            else {
                Utils.tipAlert(false, "获取消息出错-" + json.errorMessage);
            }

            //分页链接
            var pageHtml = pageDataUtils.getPageHtml(json.pageJson, 'getEvaluationMessageData');
            if (!!pageHtml)
                $("#divPageLink").html(pageHtml);

        });
    }
}


