class BcAccountSafeController extends baseController {
    public userService: UserService;

    constructor() {
        super();
        this.userService = new UserService();
    }

    init(): void {
        baseController.userAuthValidate(() => {
            this.accountSafeLevel();
        });
    }
    accountSafeLevel(): void {
        this.userService.accountSafeLevel((json) => {
            if (json.isSuccess) {
                if (json.level == 1) {
                    $("#spanAccountLevel").html("低");
                    $("#spanAccountNum").attr("style", "width:30%");
                }
                if (json.level == 2) {
                    $("#spanAccountLevel").html("中");
                    $("#spanAccountNum").attr("style", "width:60%");
                }
                if (json.level == 3) {
                    $("#spanAccountLevel").html("高");
                    $("#spanAccountNum").attr("style", "width:90%");
                }

                if (json.isBindEmai) {
                    $("#linkEmail").html("修改")
                    $("#iconEmail").attr("src", "images/mbm_ico1.jpg");
                }
                else {
                    $("#linkEmail").html("绑定").addClass("bmr_pa3");
                    $("#iconEmail").attr("src", "images/mbm_ico2.jpg");
                }

                if (json.isBindPhone) {
                    $("#linkPhone").html("修改")
                    $("#iconPhone").attr("src", "images/mbm_ico1.jpg");
                }
                else {
                    $("#linkPhone").html("绑定").addClass("bmr_pa3");
                    $("#iconPhone").attr("src", "images/mbm_ico2.jpg");
                }
                if (json.isSetSQCU) {
                    $("#linkQscu").html("修改")
                    $("#iconQscu").attr("src", "images/mbm_ico1.jpg");
                }
                else {
                    $("#linkQscu").html("绑定").addClass("bmr_pa3");
                    $("#iconQscu").attr("src", "images/mbm_ico2.jpg");
                }
                if (json.pwdComplex) {
                    $("#linkPwd").html("修改")
                    $("#iconPwd").attr("src", "images/mbm_ico1.jpg");
                }
                else {
                    $("#linkPwd").html("绑定").addClass("bmr_pa3");
                    $("#iconPwd").attr("src", "images/mbm_ico2.jpg");
                }
            }
        });
    }
}


