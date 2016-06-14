/// <reference path="WapBaseController.ts" />
class WapChangePwdController extends WapBaseController {
    constructor() {
        super();
    }

    init(): void {
        WapBaseController.userAuthValidate(() => {
            $("#btnChangePwd").click(() => {
                this.ChangePwd();
            });
        });
    }



    ChangePwd(): void {
        var currentPassword: string = $('#txtOldPwd').val();
        var newPassword: string = $('#txtNewPwd').val();
        var confimPassword: string = $('#txtConfimNewPwd').val();
        if (!currentPassword) {
            Utils.tipAlert(false, "请输入正确的原始密码");
            return;
        }
        else {
            //Utils.tipShow($('#txtOldPwd'));
        }
        if (!newPassword) {
            Utils.tipAlert(false, "请输入正确的新密码");
            return;
        }
        else {
            //Utils.tipShow($('#txtNewPwd'));
        }
        if (!confimPassword) {
            Utils.tipAlert(false, "请输入再一次输入新密码");
            return;
        }

        else if (newPassword != confimPassword) {
            Utils.tipAlert(false, "两次输入的新密码不一致");
            return;
        }
        else {
            //Utils.tipShow($('#txtConfimNewPwd'));
        }
        super.getUserService().changePassword(currentPassword, newPassword, (data) => {
            if (data.isSuccess) {
                Utils.tipAlert(true, "用户密码修改成功！");
                $("#txtOldPwd").val("");
                $("#txtNewPwd").val("");
                $("#txtConfimNewPwd").val("");
            } else {
                Utils.tipAlert(false, "用户密码修改失败！" + data.errorMessage);
            }
        });
    }
}