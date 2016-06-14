class MyPicController {
    public userService: UserService;

    constructor() {
        this.userService = new UserService();
    }

    init(): void {
        //$("#channelUL").children().eq(2).children().addClass("nav_cuta");
        //var locationUrl = window.location.href.toLowerCase();;
        //if (locationUrl.indexOf("mypic.html") != -1) {
        //    $("#accountInfoUrl li a").removeClass("m2menu_cuta");
        //    $("#accountInfoUrl").children().eq(0).children().addClass("m2menu_cuta");
        //}
        this.getBasicUserInfo();

        Utils.fileUpload('fileupload', this.userService.getUploadImgUrl('UpdateUserAvatar'), this.setUserAvatar(),this.progressForUserAvatar(), this.beforeSubmitUserAvatar());

        $("#btnUpload").click(function () {
            $("#fileupload").click();
        });
        //普通上传按钮
        $("#commonUploadSwitch").click(() => {
            $("#commonUploadPannel").show();
            $("#commonUploadSwitch").hide();
        });
        $("#commonUploadBtn").click(() => {
            this.commonUpload();
        });
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

    commonUpload(): void {
        $.ajax({
            url: this.userService.getUploadImgUrl('UpdateUserAvatar'),
            data: $('#commonUploadForm').serializeArray(),
            type: "post",
            cache: false,
            success: function (data)
            { alert(data); }
        });
    }

    setUserAvatar(): (json) => void {
        return (json) => {
            if (json.isSuccess) {
                $("#myPic").attr("src", json.avatarLarge);
                $("#myMidPic").attr("src", json.avatarMiddle);
                $("#mySmallPic").attr("src", json.avatarSmall);

            } else {
                Utils.tipAlert(false, json.errorMessage);
            }
        }
    }

    getBasicUserInfo(): void {
        this.userService.getUser((json) => {
            if (json.isAnonymous) {
                HomeUrlUtils.redirectToLogin();
            }
            else {

                $("#spanUserName").html(json.user.userName);
                $("#spanUserName").attr("href", HomeUrlUtils.homeUrl);
                $("#myPic").attr("src", json.user.avatarLarge);
                $("#myMidPic").attr("src", json.user.avatarMiddle);
                $("#mySmallPic").attr("src", json.user.avatarSmall);
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

}


