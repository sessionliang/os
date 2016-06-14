/// <reference path="../../defs/jquery.d.ts" />

class UrlUtils {
    private static apiUrl: string = eval("($pageInfo.apiUrl)") + "/";// + "/api/";

    static getAPI(controllerName: string, action: string, id?: number): string {
        if (id) {
            return UrlUtils.apiUrl + controllerName + '/' + action + '/' + id;
        }
        return UrlUtils.apiUrl + controllerName + '/' + action;
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
            return defaultUrl || "/index.html";
        }
    }

    static getViewUrl(sessionType: string, sn: string) {
        return "/app/static/" + sessionType + ".html?sn=" + sn;
    }

    static getUrlVar(key: string): any {
        var result = new RegExp(key + "=([^&]*)", "i").exec(window.location.search);
        return result && decodeURIComponent(result[1]) || "";
    }

    static getSessionType(): string {
        return UrlUtils.getUrlVar('sessionType');
    }

    static getSN(): string {
        return UrlUtils.getUrlVar('sn');
    }

    static isSN(): boolean {
        if (UrlUtils.getSN()) {
            return true;
        }
        return false;
    }

    static getToken(): string {
        return UrlUtils.getUrlVar('token');
    }

    static isToken(): boolean {
        if (UrlUtils.getToken()) {
            return true;
        }
        return false;
    }

    static redirectToReturnUrl(defaultUrl?: string): void {
        location.href = UrlUtils.getReturnUrl(defaultUrl);
    }

    static redirect(url: string): void {
        location.href = url;
    }

    static redirectToIndex(): void {
        UrlUtils.redirect('/index.html');
    }

    static redirectToLogin(returnUrl?: string): void {
        returnUrl = returnUrl || window.location.href;
        UrlUtils.redirect('/login.html?returnUrl=' + returnUrl);
    }

    static getThirdLoginUrl(loginType: string, returnUrl: string, userID?: string): string {
        if (userID) {
            return 'http://gx.com/home/authLogin.html?isStart=true&loginType=' + loginType + "&userID=" + userID + '&returnUrl=' + returnUrl;
        } else {
            return 'http://gx.com/home/authLogin.html?isStart=true&loginType=' + loginType + '&returnUrl=' + returnUrl;
        }
    }

    static reload(): void {
        location.reload(true);
    }
}