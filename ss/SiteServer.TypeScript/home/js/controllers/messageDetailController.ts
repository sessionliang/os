class MessageDetailController {
    public static url: string = "messageDetail.html";
    public msgID: number;
    public returnUrl: string;
    public userService: UserService;

    init(): void {

        this.getBasicUserInfo();

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
        $("#btnReplyMsgOpen").click(function () {
            $(".mlay_sm").slideDown(200);
            $(".mlayBg").show();
        });
        /*弹窗 结束*/

        //绑定事件
        $("#btnReturn").click(() => {
            HomeUrlUtils.redirectToReturnUrl();
        });
        $("#btnDeleteMsg").click(() => {
            this.deleteMsg(this.msgID);
        });

        $("#btnReplyMsg").click(() => {
            this.sendMessage();
        });

        //绑定信息
        this.userService.getUserMessageDetail(this.msgID, function (data) {
            if (data.isSuccess) {
                if (data.info.messageType == '1') {
                    $("#msgType").html("站内信");
                    $("#btnReplyMsgOpen").css("display", "inline");
                }
                else if (data.info.messageType == '2') {
                    $("#msgType").html("系统通知");
                    $("#btnReplyMsgOpen").remove();
                    $("#btnReplyMsg").remove();
                }
                else if (data.info.messageType == '3') {
                    $("#msgType").html("系统公告");
                    $("#btnReplyMsgOpen").remove();
                    $("#btnReplyMsg").remove();
                    $("#btnDeleteMsg").remove();
                }
                $("#title").html(data.info.title);
                $("#msg").html(data.info.content);
                $("#from").html(data.info.messageFrom);
                $("#addDate").html(data.info.addDate.replace("T", " "));
                $("#replyUserName").val(data.info.messageFrom);
            }
        });
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

    deleteMsg(msgID: number) {
        if (!confirm("确定要删除该消息吗？"))
            return;
        this.userService.deleteUserMessage(msgID, function (data) {
            if (data.isSuccess) {
                Utils.tipAlert(true, "删除成功！");
                HomeUrlUtils.redirectToReturnUrl();
            }
            else {
                Utils.tipAlert(false, data.errorMessage);
            }
        });
    }

    sendMessage(): void {
        var userName = $("#replyUserName").val();
        var msg = $("#replyMsg").val();
        var title = $("#replyTitle").val();
        var parentID = this.msgID;
        if (!userName) {
            Utils.tipShow($("#replyUserName"), false, "请输入正确的接收人！");
            return;
        }
        else
            Utils.tipShow($("#replyUserName"));
        if (!title) {
            Utils.tipShow($("#replyTitle"), false, "请输入标题！");
            return;
        }
        else
            Utils.tipShow($("#replyTitle"));

        if (!msg) {
            Utils.tipShow($("#replyMsg"), false, "请输入内容！");
            return;
        }
        else
            Utils.tipShow($("#replyMsg"));

        this.userService.sendMessage(userName, title, msg, function (data) {
            if (data.isSuccess)
                Utils.tipAlert(true, "发送成功！");
            else
                Utils.tipAlert(false, data.errorMessage);
        }, parentID);
    }

    static GetRedirectString(msgID: number, returnUrl: string): string {
        var retUrl = MessageDetailController.url;
        retUrl += "?msgID=" + msgID + "&returnUrl=" + returnUrl;
        return retUrl;
    }

    constructor() {
        //获取ID，返回地址
        this.userService = new UserService();
        this.msgID = parseInt(HomeUrlUtils.getUrlVar("msgID"));
        this.returnUrl = HomeUrlUtils.getUrlVar("returnUrl");
    }
}