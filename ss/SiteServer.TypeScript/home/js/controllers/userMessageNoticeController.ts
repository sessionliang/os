class UserMessageNoticeController {
    public userService: UserService;

    constructor() {
        this.userService = new UserService();
    }

    init(): void {
        //$("#channelUL").children().eq(3).children().addClass("nav_cuta");
        //var locationUrl = window.location.href.toLowerCase();;
        //if (locationUrl.indexOf("usermessagenotice.html") != -1) {
        //    $("#accountMsgUrl li a").removeClass("m2menu_cuta");
        //    $("#accountMsgUrl").children().eq(1).children().addClass("m2menu_cuta");
        //}

        this.getBasicUserInfo();
        this.getUserMessageNotice(1, 10);
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

    getUserMessageNotice(pageIndex: number, prePageNum: number): void {
        this.userService.getUserMessage('System', pageIndex, prePageNum,(json) => {
            if (json.isSuccess) {
                $("#ulMessageNotice").html("");
                var innerHtml = "";
                for (var i = 0; i < json.userMessageList.length; i++) {
                    if (json.userMessageList[i].isViewed)
                        innerHtml += "<li><a href='" + MessageDetailController.GetRedirectString(json.userMessageList[i].id, HomeUrlUtils.getReturnUrl()) + "' class='fl'>" + json.userMessageList[i].title + "</a><span class='fr'>" + Utils.formatTime(json.userMessageList[i].addDate.replace('T', '  '), "yyyy-MM-dd HH:mm:ss") + "</span></li>";
                    else
                        innerHtml += "<li><a href='" + MessageDetailController.GetRedirectString(json.userMessageList[i].id, HomeUrlUtils.getReturnUrl()) + "' class='fl_b'>" + json.userMessageList[i].title + "</a><span class='fr'>" + Utils.formatTime(json.userMessageList[i].addDate.replace('T', '  '), "yyyy-MM-dd HH:mm:ss") + "</span></li>";
                }

                $("#ulMessageNotice").html(innerHtml);

            }
            else {
                Utils.tipAlert(false, "获取消息出错-" + json.errorMessage);
            }

            //分页链接
            var pageHtml = pageDataUtils.getPageHtml(json.pageJson, 'getUserMessageNotice');
            if (!!pageHtml)
                $("#divPageLink").html(pageHtml);
        });
    }
}


