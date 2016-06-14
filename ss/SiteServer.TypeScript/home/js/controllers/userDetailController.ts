class UserDetailController {
    public userService: UserService;

    constructor() {
        this.userService = new UserService();
    }


    init(): void {
        //$("#channelUL").children().eq(2).children().addClass("nav_cuta");
        //var locationUrl = window.location.href.toLowerCase();;
        //if (locationUrl.indexOf("detail.html") != -1) {
        //    $("#accountInfoUrl li a").removeClass("m2menu_cuta");
        //    $("#accountInfoUrl").children().eq(2).children().addClass("m2menu_cuta");
        //}


        this.getDetailUserInfo();
        $("#btnSaveUserDetailInfo").click(() => {
            this.saveDetailUserInfo();
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

        this.userService.updateAutoDetailUserInfo(keyValueStr, (data) => {
            if (data.isSuccess) {
                Utils.tipAlert(true, "用户的详细资料修改成功！");
            }
            else {
                Utils.tipAlert(false, data.errorMessage);
            }
        });

    }

    getDetailUserInfo(): void {
        this.userService.getUser((json) => {
            if (json.isAnonymous) {
                HomeUrlUtils.redirectToLogin();
            }
            else {
                $("#spanUserName").html(json.user.userName);
                $("#spanUserName").attr("href", HomeUrlUtils.homeUrl);
                $("#txtUserName").val(json.user.userName);
                if (json.user.hasNewMsg) {
                    $("#userMsgTip").css("display", "inline");
                    $("#userMsgCount").html(json.user.newMsgCount);
                }

                $("#btnLogout").click((e) => {
                    this.userService.logout(() => {
                        HomeUrlUtils.redirectToLogin(HomeUrlUtils.homeUrl);
                    });
                });
                this.userService.loadUserProperty((json) => {
                    if (json.isSuccess) {
                        $("#ulUserProperty").html(json.userPropertys);
                        $("#ulUserProperty input").addClass("mcr_int1 mcr_int2");
                        $("#ulUserProperty select").addClass("mcr_sel");
                    }
                });
            }
        });

    }
}


