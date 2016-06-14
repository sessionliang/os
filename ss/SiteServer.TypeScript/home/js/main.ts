/// <reference path="controllers/registercontroller.ts" />
/// <reference path="controllers/usercentercontroller.ts" />
/// <reference path="controllers/userdetailcontroller.ts" />
/// <reference path="controllers/changepwdcontroller.ts" />
/// <reference path="controllers/homecontroller.ts" />
/// <reference path="controllers/mypiccontroller.ts" />
/// <reference path="controllers/accountbindcontroller.ts" />
/// <reference path="controllers/accountsafecontroller.ts" />
/// <reference path="controllers/bindphonecontroller.ts" />
/// <reference path="controllers/loginemailcontroller.ts" />
/// <reference path="controllers/loginrecordcontroller.ts" /> 
/// <reference path="controllers/usermessagecontroller.ts" />
/// <reference path="controllers/usermessagenoticecontroller.ts" />
/// <reference path="controllers/secretpwdcontroller.ts" />
/// <reference path="controllers/findpwdcontroller.ts" />
/// <reference path="controllers/baseController.ts" />
/// <reference path="controllers/siteMessageController.ts" />
/// <reference path="controllers/bcPersonalprofileController.ts" />
/// <reference path="controllers/evaluationMessageController.ts" />
/// <reference path="controllers/trialApplyMessageController.ts" />
window.onload = () => {
    HomeUrlUtils.getHomeUrl(() => {

        var controllerName = $("#controllerName").attr("data-controller-name");

        if (controllerName === 'register') {
            (new RegisterController()).init();
        }
        else if (controllerName == 'userCenter') {
            (new UserCenterController()).init();
        }
        else if (controllerName == 'userDetail') {
            (new UserDetailController()).init();
        }
        else if (controllerName == 'changePwd') {
            (new ChangePwdController()).init();
        }
        else if (controllerName == 'index') {
            (new HomeController()).init();
        }
        else if (controllerName == 'myPic') {
            (new MyPicController()).init();
        }
        else if (controllerName == 'accountBind') {
            (new AccountBindController()).init();
        }
        else if (controllerName == 'accountSafe') {
            (new AccountSafeController()).init();
        }
        else if (controllerName == 'bindPhone') {
            (new BindPhoneController()).init();
        }
        else if (controllerName == 'loginEmail') {
            (new LoginEmailController()).init();
        }
        else if (controllerName == 'loginRecord') {
            (new LoginRecordController()).init();
        }
        else if (controllerName == 'userMessage') {
            (new UserMessageController()).init();
        }
        else if (controllerName == 'userMessageNotice') {
            (new UserMessageNoticeController()).init();
        }
        else if (controllerName == 'secretPwd') {
            (new SecretPwdController()).init();
        }
        else if (controllerName == 'findpwd') {
            (new FindPwdController()).init();
        }
        else if (controllerName == 'wait') {
            (new WaitController()).init();
        }
        else if (controllerName == 'siteMessage') {
            (new SiteMessageController()).init();
        }
        else if (controllerName == 'helpCenter') {
            (new HelpCenterController()).init();
        }
        else if (controllerName == 'messageDetail') {
            (new MessageDetailController()).init();
        }
        else if (controllerName == 'orderList') {
            (new BcOrderListController()).init();
        }
        else if (controllerName == 'orderComment') {
            (new BcOrderCommentController()).init();
        }
        else if (controllerName == 'consultationList') {
            (new BcConsultationListController()).init();
        }
        else if (controllerName == 'b2c_index') {
            (new BcIndexController()).init();
        }
        else if (controllerName == 'bcPersonalprofile') {
            (new BcPersonalprofileController()).init();
        }
        else if (controllerName == 'bcChangePwd') {
            (new BcChangePwdController()).init();
        }
        else if (controllerName == 'bcBindPhone') {
            (new BcBindPhoneController()).init();
        }
        else if (controllerName == 'bcBindEmail') {
            (new BcBindEmailController()).init();
        }
        else if (controllerName == 'bcLoginRecord') {
            (new BcLoginRecordController()).init();
        }
        else if (controllerName == 'bcSecretPwd') {
            (new BcSecretPwdController()).init();
        }
        else if (controllerName == 'bcAccountSafe') {
            (new BcAccountSafeController()).init();
        }
        else if (controllerName == 'bcInvoice') {
            (new BcInvoiceController()).init();
        }
        else if (controllerName == 'bcFollow') {
            (new BcFollowController()).init();
        }
        else if (controllerName == 'bcHistory') {
            (new BcHistoryController()).init();
        }
        else if (controllerName == 'bcSuggestion') {
            (new BcSuggestionController()).init();
        }
        else if (controllerName == 'orderReturnList') {
            (new BcOrderReturnListController()).init();
        }
        else if (controllerName == 'returnApply') {
            (new BcReturnApplyController()).init();
        }
        else if (controllerName == 'orderItemReturnRecordList') {
            (new BcOrderItemReturnRecordListController()).init();
        }
        else if (controllerName == 'orderItemList') {
            (new BcOrderItemListController()).init();
        }
        else if (controllerName == 'wapOrderList') {
            (new WapOrderListController()).init();
        }
        else if (controllerName == 'wapOrderItemList') {
            (new WapOrderItemListController()).init();
        }
        else if (controllerName == 'wapIndex') {
            (new WapIndexController()).init();
        }
        else if (controllerName == 'wapAddress') {
            (new WapAddressController()).init();
        }
        else if (controllerName == 'wapChangePwd') {
            (new WapIndexController()).init();
        }
        else if (controllerName == 'wapEditAddress') {
            (new WapEditAddressController()).init();
        }
        else if (controllerName == 'wapFollow') {
            (new WapFollowController()).init();
        }
        else if (controllerName == 'wapHistory') {
            (new WapHistoryController()).init();
        }
        else if (controllerName == 'wapUserMessage') {
            (new WapUserMessageController()).init();
        }
        else if (controllerName == 'wapProfiles') {
            (new WapBaseController());
        }
        //else if (controllerName == 'mlibContent') {
        //    (new MLibContentController()).init();
        //}
        else if (controllerName == 'evaluationMessage') {
            (new EvaluationMessageController()).init();
        }
        else if (controllerName == 'trialApplyMessage') {
            (new TrialApplyMessageController()).init();
        }
        else if (controllerName == 'trialReportMessage') {
            (new TrialReportMessageController()).init();
        }
        else if (controllerName == 'trialReportAdd') {
            (new TrialReportAddController()).init();
        }
    });
};