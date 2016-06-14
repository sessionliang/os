/// <reference path="../../../corejs/siteserver/siteserver.cms.ts" />
/// <reference path="../../../corejs/siteserver/siteserver.ts" />
class TrialReportAddController {
    public userService: UserService;
    public trialService: TrialService;

    private trialApplyID: number = UrlUtils.getUrlVar("trialApplyID");

    constructor() {
        this.userService = new UserService();
        this.trialService = new TrialService();
    }

    static getRedirectUrl(trialApplyID: number) {
        return StringUtils.format("trialReportAdd.html?trialApplyID={0}", String(trialApplyID));
    }

    init(): void {
        this.getBasicUserInfo();
        this.getTrialReportFormInfo();
        $("#btnTrialReportAdd").click(() => {
            ss.extendjs.checkFormValueById("addPannel");
            this.trialReportAdd();
        });
    }

    trialReportAdd(): void {
        this.trialService.addTrialReport($("#frmAdd")[0], this.trialApplyID, (data) => {
            if (data.isSuccess) {
                Utils.tipAlert(true, "试用报告提交成功！");
                UrlUtils.redirect("trialMessageList.html");
            } else {
                Utils.tipAlert(false, "试用报告提交失败！" + data.errorMessage);
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

    //初始化表单
    getTrialReportFormInfo(): void {
        this.trialService.getTrialReportFormInfoArray(this.trialApplyID, (json) => {
            var html: string = ss.cms.parserForm(null, json.styleInfoArray, false, true, $("#addPanel")[0]);
            $("#addPanel").html(html);
        });
    }
}


