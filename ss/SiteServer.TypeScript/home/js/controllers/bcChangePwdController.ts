class BcChangePwdController extends baseController {
    public userService: UserService;

    constructor() {
        super();
        this.userService = new UserService();
    }

    init(): void {
        baseController.userAuthValidate(() => {
            $("#btnChangePwd").click(() => {
                this.ChangePwd();
            });
            this.blurCheck();
        });
    }

    blurCheck(): void {
        $("#txtOldPwd").blur(() => {
            if (!$("#txtOldPwd").val()) {
                Utils.tipShow($("#txtOldPwd"), false, "请输入正确的原始密码！");
            }
            else {
                Utils.tipShow($("#txtOldPwd"), true);
            }
        });

        $("#txtNewPwd").blur(() => {
            if (!$("#txtNewPwd").val()) {
                Utils.tipShow($("#txtNewPwd"), false, "请输入正确的新密码！");
            }
            else {
                Utils.tipShow($("#txtNewPwd"), true);
            }
            
            if ($("#txtConfimNewPwd").val() != $("#txtNewPwd").val()) {
                Utils.tipShow($("#txtConfimNewPwd"), false, "两次输入的新密码不一致！");
            }
            else {
                Utils.tipShow($("#txtConfimNewPwd"), true);
            }
        });

        $("#txtConfimNewPwd").blur(() => {
            if (!$("#txtConfimNewPwd").val()) {
                Utils.tipShow($("#txtConfimNewPwd"), false, "请输入再一次输入新密码！");
            }
            else if ($("#txtConfimNewPwd").val() != $("#txtNewPwd").val()) {
                Utils.tipShow($("#txtConfimNewPwd"), false, "两次输入的新密码不一致！");
            }
            else {
                Utils.tipShow($("#txtConfimNewPwd"), true);
            }
        });
    }

    ChangePwd(): void {
        var currentPassword: string = $('#txtOldPwd').val();
        var newPassword: string = $('#txtNewPwd').val();
        var confimPassword: string = $('#txtConfimNewPwd').val();
        if (!currentPassword) {
            Utils.tipShow($('#txtOldPwd'), false, "请输入正确的原始密码");
            return;
        }
        else {
            Utils.tipShow($('#txtOldPwd'));
        }
        if (!newPassword) {
            Utils.tipShow($('#txtNewPwd'), false, "请输入正确的新密码");
            return;
        }
        else {
            Utils.tipShow($('#txtNewPwd'));
        }
        if (!confimPassword) {
            Utils.tipShow($('#txtConfimNewPwd'), false, "请输入再一次输入新密码");
            return;
        }

        else if (newPassword != confimPassword) {
            Utils.tipShow($('#txtConfimNewPwd'), false, "两次输入的新密码不一致");
            return;
        }
        else {
            Utils.tipShow($('#txtConfimNewPwd'));
        }
        this.userService.changePassword(currentPassword, newPassword, (data) => {
            if (data.isSuccess) {
                Utils.tipAlert(true, "用户密码修改成功！");
                $("#txtOldPwd").val("");
                $("#txtNewPwd").val("");
                $("#txtConfimNewPwd").val("");
            } else {
                Utils.tipAlert(false, "用户密码修改失败！" + data.errorMessage);
                Utils.tipShow($('#txtOldPwd'), false, "请输入正确的原始密码");
            }
        });
    }
    
}


