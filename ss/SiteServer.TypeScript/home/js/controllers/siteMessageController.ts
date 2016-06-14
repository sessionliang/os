class SiteMessageController {
    public userService: UserService;

    constructor() {
        this.userService = new UserService();
    }

    init(): void {
        //$("#channelUL").children().eq(3).children().addClass("nav_cuta");
        //var locationUrl = window.location.href.toLowerCase();;
        //if (locationUrl.indexOf("sitemessage.html") != -1) {
        //    $("#accountMsgUrl li a").removeClass("m2menu_cuta");
        //    $("#accountMsgUrl").children().eq(2).children().addClass("m2menu_cuta");
        //}

        /*弹窗 开始*/
        $(".mlayBg").height($(document).height());
        $(".mbody2").css("padding-top",($(document).height() - 655) / 2);
        $(".mbody3").css("padding-top",($(document).height() - 755) / 2);
        $(window).resize(function () {
            $(".mlayBg").height($(document).height());
            $(".mbody2").css("padding-top",($(document).height() - 655) / 2);
            $(".mbody3").css("padding-top",($(document).height() - 755) / 2);
        });
        $(".mrclose").click(function () {
            $(".mlay_sm").slideUp(200);
            $(".mlayBg").hide();
        });
        $("#btnSendMsgOpen").click(function () {
            $(".mlay_sm").slideDown(200);
            $(".mlayBg").show();
        });
        /*弹窗 结束*/

        $("#btnSendMsg").click(() => {
            this.sendMessage();
        });

        this.getBasicUserInfo();
        this.getSiteMessage(1, 10);

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

    getSiteMessage(pageIndex: number, prePageNum: number): void {
        this.userService.getSiteMessage(pageIndex, prePageNum,(json) => {
            if (json.isSuccess) {
                $("#ulSiteMessage").html("");
                var innerHtml = "";
                for (var i = 0; i < json.userMessageList.length; i++) {
                    if (json.userMessageList[i].isViewed)
                        innerHtml += "<li><a href='" + MessageDetailController.GetRedirectString(json.userMessageList[i].id, HomeUrlUtils.getReturnUrl()) + "' class='fl'>" + json.userMessageList[i].title + "</a><span class='fr'>" + Utils.formatTime(json.userMessageList[i].addDate.replace('T', '  '), "yyyy-MM-dd HH:mm:ss") + "</span></li>";
                    else
                        innerHtml += "<li><a href='" + MessageDetailController.GetRedirectString(json.userMessageList[i].id, HomeUrlUtils.getReturnUrl()) + "' class='class='fl_b'>" + json.userMessageList[i].title + "</a><span class='fr'>" + Utils.formatTime(json.userMessageList[i].addDate.replace('T', '  '), "yyyy-MM-dd HH:mm:ss") + "</span></li>";
                }

                $("#ulSiteMessage").html(innerHtml);

            }
            else {
                Utils.tipAlert(false, "获取消息出错-" + json.errorMessage);
            }

            //分页链接
            var pageHtml = pageDataUtils.getPageHtml(json.pageJson, 'getSiteMessage');
            if (!!pageHtml)
                $("#divPageLink").html(pageHtml);
        });
    }

    sendMessage(): void {
        var userName = $("#userName").val();
        var msg = $("#msg").val();
        var title = $("#title").val();
        var parentID = 0;
        if (!userName) {
            Utils.tipShow($("#userName"), false, "请输入正确的接收人！");
            return;
        }
        else
            Utils.tipShow($("#userName"));
        if (!title) {
            Utils.tipShow($("#title"), false, "请输入标题！");
            return;
        }
        else
            Utils.tipShow($("#title"));

        if (!msg) {
            Utils.tipShow($("#msg"), false, "请输入内容！");
            return;
        }
        else
            Utils.tipShow($("#msg"));


        this.userService.sendMessage(userName, title, msg, function (data) {
            if (data.isSuccess)
                Utils.tipAlert(true, "发送成功！");
            else
                Utils.tipAlert(false, data.errorMessage);
        }, parentID);
    }

}


