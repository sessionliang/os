/// <reference path="baseController.ts" />
class BcPersonalprofileController extends baseController {
    public userService: UserService;
    public sdkController: SDKController;
    constructor() {
        super();
        this.userService = new UserService();
    }

    init(): void {
        baseController.userAuthValidate(() => {
            $(".bmrb_li01 img").attr("src", baseController.user.avatarLarge);
            $(".bmrb_li02 img").attr("src", baseController.user.avatarMiddle);
            $(".bmrb_li03 img").attr("src", baseController.user.avatarSmall);

            $("#txtUserName").val(baseController.user.userName);
            $("#txtRemark").val(baseController.user.signature);

            this.userService.loadUserProperty((json) => {
                if (json.isSuccess) {
                    $("#ulUserProperty").html(json.userPropertys);
                    $("#ulUserProperty span").addClass("bmra_s1");
                    $("#ulUserProperty input").addClass("bmra_int");
                    $("#ulUserProperty select").addClass("bmra_sel");
                }
            });


            Utils.fileUpload('fileupload', this.userService.getUploadImgUrl('UpdateUserAvatar'), this.setUserAvatar(), this.progressForUserAvatar(),this.beforeSubmitUserAvatar());
            $("#btnUpload").click(function () {
                $("#fileupload").click();
            });

            $("#btnSaveUserInfo").click(() => {
                this.saveBasicUserInfo();
            });

            $("#btnSaveUserDetailInfo").click(() => {
                this.saveDetailUserInfo();
            });

            this.getThirdBindInfo();
        });
    }
    setUserAvatar(): (json) => void {
        return (json) => {
            if (json.isSuccess) {
                $(".bmrb_li01 img").attr("src", json.avatarLarge);
                $(".bmrb_li02 img").attr("src", json.avatarMiddle);
                $(".bmrb_li03 img").attr("src", json.avatarSmall);

            } else {
                Utils.tipAlert(false, json.errorMessage);
            }
        }
    }

    progressForUserAvatar(): (json) => void {
        return (json) => {
            if (json.total > json.loaded)
                $("#uploadProgress").html("正在上传..." + Math.floor(json.loaded / json.total * 100) + "%");
            else
                $("#uploadProgress").html("上传完成！");
        }
    }

    beforeSubmitUserAvatar(): (json) => boolean {
        return (json) => {
            if (!/(\.|\/)(gif|jpe?g|png)$/i.test(json.files[0].name)) {
                alert("只能上传gif,jpeg,png格式的图片！");
                return false;
            }
        }
    }

    saveBasicUserInfo(): void {
        var userName: string = $("#txtUserName").val();
        var remark: string = $("#txtRemark").val();

        if (!userName) {
            Utils.tipAlert(false, "请输入正确的用户名！");
            return;
            this.userService.getUser((json) => {
                if (userName != json.user.userName)
                    Utils.tipAlert(false, "用户名不能被修改！");
                return;
            });
        }

        this.userService.updateBasicUserInfo(userName, remark,(data) => {
            if (data.isSuccess) {
                Utils.tipAlert(true, "用户的基本信息修改成功！");
            }
            else {
                Utils.tipAlert(false, data.errorMessage);
            }
        });
    }

    saveDetailUserInfo(): void {
        var keyValueArray = [];
        $("#ulUserProperty input").map(function () {
            keyValueArray.push($(this).attr('name') + "=" + $(this).val());
        });
        $("#ulUserProperty select").map(function () {
            keyValueArray.push($(this).attr('name') + "=" + $(this).val());
        });
        $("#ulUserProperty textarea").map(function () {
            keyValueArray.push($(this).attr('name') + "=" + $(this).html());
        });
        var keyValueStr = keyValueArray.join('&');

        this.userService.updateAutoDetailUserInfo(keyValueStr,(data) => {
            if (data.isSuccess) {
                Utils.tipAlert(true, "用户的详细资料修改成功！");
            }
            else {
                Utils.tipAlert(false, data.errorMessage);
            }
        });

    }

    getThirdBindInfo(): void {
        this.sdkController = new SDKController();
        this.sdkController.initBind();
    }




}



