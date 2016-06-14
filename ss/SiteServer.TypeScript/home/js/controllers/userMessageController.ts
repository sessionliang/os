class UserMessageController {
    public userService: UserService;

    constructor() {
        this.userService = new UserService();
    }

    init(): void {
        //$("#channelUL").children().eq(3).children().addClass("nav_cuta");
        //var locationUrl = window.location.href.toLowerCase();;
        //if (locationUrl.indexOf("message.html") != -1) {
        //    $("#accountMsgUrl li a").removeClass("m2menu_cuta");
        //    $("#accountMsgUrl").children().eq(0).children().addClass("m2menu_cuta");
        //}
        this.getBasicUserInfo();
        this.getUserMessage(1, 10);
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
                    //系统消息
                    if (json.user.newMsgCount > 0)
                        $("#systemMsg").html($("#systemMsg").html() + "<img src='" + HomeUrlUtils.homeUrl + "/images/icon-2.png' class='message_tip'/>");
                    //系统公告
                    if (json.user.systemNoticeCount > 0) {
                        $("#systemNotice").html($("#systemNotice").html() + "<img src='" + HomeUrlUtils.homeUrl + "/images/icon-2.png' class='message_tip'/>");
                    }
                }
                $("#btnLogout").click((e) => {
                    this.userService.logout(() => {
                        HomeUrlUtils.redirectToLogin(HomeUrlUtils.homeUrl);
                    });
                });
            }
        });
    }

    getUserMessage(pageIndex: number, prePageNum: number): void {
        this.userService.getUserMessage('SystemAnnouncement', pageIndex, prePageNum,(json) => {
            if (json.isSuccess) {
                $("#ulMessage").html("");
                var innerHtml = "";
                for (var i = 0; i < json.userMessageList.length; i++) {
                    innerHtml += "<li><a href='" + MessageDetailController.GetRedirectString(json.userMessageList[i].id, HomeUrlUtils.getReturnUrl()) + "' >" + json.userMessageList[i].title + "</a><span class='fr'>" + Utils.formatTime(json.userMessageList[i].addDate.replace('T', '  '), "yyyy-MM-dd HH:mm:ss") + "</span></li>";
                }
                $("#ulMessage").html(innerHtml);
            }
            else {
                Utils.tipAlert(false, "获取消息出错-" + json.errorMessage);
            }

            //分页链接
            var pageHtml = pageDataUtils.getPageHtml(json.pageJson, 'getUserMessageData');
            if (!!pageHtml)
                $("#divPageLink").html(pageHtml);

        });
    }
}


