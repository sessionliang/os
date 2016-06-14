/// <reference path="../../../defs/jquery.d.ts" />

class HomeUrlUtils {
    private static apiUrl: string = "/api/";
    public static homeUrl: string;


    public static getHomeUrl(fn:Function): void {
        HomeUrlUtils.homeUrl = "/home/";
        var userService = new UserService();
        userService.getHomeUrl((data) => {
            if (data.isSuccess) {
                HomeUrlUtils.homeUrl = "/" + data.homeUrl + "/";
                if (fn)
                    fn();
            }
        });
    }

    static getAPI(controllerName: string, action: string, id?: number): string {
        if (id) {
            return HomeUrlUtils.apiUrl + controllerName + '/' + action + '/' + id;
        }
        return HomeUrlUtils.apiUrl + controllerName + '/' + action;
    }

    static getAbsoluteUrl(url: string) {
        return "http://" + window.location.hostname.toLowerCase() + "/" + url.replace(/(^\/*)|(\/*$)/g, "");
    }

    static getReturnUrl(defaultUrl?: string): string {
        var reg = new RegExp("(^|&)" + "returnUrl" + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) {
            return decodeURIComponent(r[2]);
        } else {
            return defaultUrl || window.location.href;
        }
    }

    static getUrlVar(key: string): string {
        var result = new RegExp(key + "=([^&]*)", "i").exec(window.location.search);
        return result && decodeURIComponent(result[1]) || "";
    }

    static redirectToReturnUrl(defaultUrl?: string): void {
        location.href = HomeUrlUtils.getReturnUrl(defaultUrl);
    }

    static redirectToIndex(): void {
        location.href = HomeUrlUtils.homeUrl + 'index.htm';
    }

    static redirectToWapIndex(): void {
        location.href = HomeUrlUtils.homeUrl + 'wap/index.htm';
    }

    static redirectToLogin(returnUrl?: string): void {
        returnUrl = returnUrl || window.location.href;
        location.href = HomeUrlUtils.homeUrl + 'login.html?returnUrl=' + returnUrl;
    }

    static redirectToWapLogin(returnUrl?: string): void {
        returnUrl = returnUrl || window.location.href;
        location.href = HomeUrlUtils.homeUrl + 'wap/login.html?returnUrl=' + returnUrl;
    }

    static reload(): void {
        location.reload(true);
    }
}