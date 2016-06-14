/**********
* 控制类基类
**********/
class WapBaseController {

    //用户仓储
    static userService: UserService;
    getUserService(): UserService {
        return WapBaseController.userService;
    }

    //用户
    static user: any;
    getUser(): any {
        return WapBaseController.user;
    }

    constructor() {
        WapBaseController.userService = new UserService();
        //WapBaseController.userAuthValidate();
    }

    //验证用户是否登录
    static userAuthValidate(fn: Function): void {
        WapBaseController.userService.getUser((json) => {
            if (json.isAnonymous) {
                HomeUrlUtils.redirectToWapLogin();
            }
            else {
                WapBaseController.user = json.user;
                $('#fUserName').html(json.user.userName);
                $('#linkLogout').click(() => {
                    WapBaseController.userService.logout((data) => {
                        location.href = location.href;
                    });
                });
                if (fn)
                    fn();
            }
        });
    }

}