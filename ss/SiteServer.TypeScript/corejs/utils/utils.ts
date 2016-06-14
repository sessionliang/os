/// <reference path="../../defs/jquery.d.ts" />
/// <reference path="../../defs/jquery.cookie.d.ts" />
/// <reference path="../../defs/jquery.fileupload.d.ts" />
/// <reference path="../../defs/tmodjs.d.ts" />

class Utils {

    static isAnonymous(): boolean {
        return $.cookie("SITESERVER.LOGIN") != "YES";
    }

    static setLoginStatus(value: boolean): void {
        if (value) {
            $.cookie("SITESERVER.LOGIN", "YES", { expires: 7, path: '/', secure: false });
        } else {
            $.cookie("SITESERVER.LOGIN", "", { expires: 7, path: '/', secure: false });
        }
    }

    static fileUpload(elementID: string, uploadUrl: string, success: any, progress?: any, submit?: any, callback?: string): void {
        $('#' + elementID).attr('data-url', uploadUrl);
        $('#' + elementID).fileupload({
            dataType: 'json',
            acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
            maxFileSize: 5000000, // 5 MB
            xhrFields: { withCredentials: true },
            type: 'POST',
            add: (e, data) => {
                data.submit().done(success);
            },
            progress: (e, data) => {
                if (progress)
                    progress(data);
            },
            submit: (e, data) => {
                if (submit)
                    return submit(data);
            }
        });
    }

    static commonUpload(elementID: string, uploadUrl: string, success: (e: JQueryEventObject) => void): void {
        if ($("#" + elementID) && $("#_filePosition"))
            $("#_filePosition").insertAfter($("#" + elementID).css("display:none;"));
        if ($("#_commonFileForm"))
            $("#_commonFileForm").remove();
        if ($("#" + elementID))
            $("#" + elementID).css("display:inline;");
        if ($("#_filePosition"))
            $("#_filePosition").remove();

        var formHtml = "<form method='post' style='display:none;' enctype='multipart/form-data' id='_commonFileForm' target='_commonFileTarget'></form>";
        var targetPannel = "<iframe id='_commonFileTarget' style='display:none;'></iframe>";

        $("#" + elementID).insertBefore($("<div id='_filePosition' style='display:none;'></div>"));//元素定位
        $("#" + elementID).wrap($(formHtml));//文件表单
        $("#_filePosition").append($(targetPannel));//上传之后返回信息放置地方

        $("#_commonFileForm").attr("action", uploadUrl);
        $("#_commonFileForm").submit();

    }

    static getDate(d: string) {
        return d.substr(0, d.indexOf('T'));
    }

    static getDateTime(d: string): string {
        return d.substr(0, d.indexOf('T')) + ' ' + d.substr(d.indexOf('T') + 1);
    }

    static random(): number {
        return Math.floor(1000 * Math.random());
    }

    static isEmail(str: string): boolean {
        return /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/.test(str);
    }

    static isMobile(str: string): boolean {
        return /^0{0,1}1[0-9]{10}$/.test(str);
    }

    static ajaxGet(parameters, url, done: (data) => void): void {
        $.ajax({
            url: url,
            type: 'GET',
            data: parameters,
            dataType: 'jsonp',
            xhrFields: { withCredentials: true },
            crossDomain: true
        }).done((data) => {
            done(data);
        }).fail((e) => {
            alert(JSON.stringify(e));
        });
    }

    static ajaxGetSync(parameters, url, done: (data) => void): void {
        $.ajax({
            url: url,
            async: false,
            type: 'GET',
            data: parameters,
            dataType: 'jsonp'
            //xhrFields: { withCredentials: false }
            //crossDomain: true
        }).done((data) => {
            done(data);
        }).fail((e) => {
            alert(JSON.stringify(e));
        });
    }

    static ajaxPost(parameters, url, done: (data) => void): void {
        $.ajax({
            url: url,
            type: 'POST',
            data: parameters,
            dataType: 'json',
            //xhrFields: { withCredentials: true },
            //crossDomain: true
        }).done((data) => {
            done(data);
        }).fail((e) => {
            console.log(JSON.stringify(e));
            alert(JSON.stringify(e));
        });
    }

    static ajaxPostForm(form: HTMLElement, url, done: (data) => void): void {
        $.ajax({
            url: url,
            type: 'POST',
            data: $(form).serializeArray(),
            dataType: 'json'
        }).done((data) => {
            done(data);
        }).fail((e) => {
            console.log(JSON.stringify(e));
            alert(JSON.stringify(e));
        });
    }

    static toJSON(data): any {
        return $.parseJSON(JSON.stringify(data));
    }

    static detectIE(): number {
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf('MSIE ');
        var trident = ua.indexOf('Trident/');

        if (msie > 0) {
            // IE 10 or older => return version number
            return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
        }

        if (trident > 0) {
            // IE 11 (or newer) => return version number
            var rv = ua.indexOf('rv:');
            return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
        }

        // other browser
        return 0;
    }

    static setDisplay(tipsID: string): void {
        $("#" + tipsID).removeClass("nerror");
        $("#" + tipsID + " img.tipImg").hide();
        $("#" + tipsID + " .nmf_st1").hide();
    }

    /******
     * 信息弹窗提示
     * @param msg         <> 提示信息
     * @param success     <> 成功OR失败
     * @param autoDisplay <> 是否自动隐藏
     * @param interval    <> 自动隐藏时间
     * 
     ******/
    static tipAlert(success: boolean, msg: string, autoDisplay?: boolean, interval?: number, url?: string): void {
        if (autoDisplay == undefined)
            autoDisplay = true;
        if (!interval)
            interval = 1500;
        //容器
        var tipHtml = '';
        if (success)
            tipHtml += '<div id="tipPannel" class="mLayOk" style="color:#187abc;font-family:Microsoft Yahei">';//成功
        else
            tipHtml += '<div id="tipPannel" class="mLayOk" style="color:#e42222;font-family:Microsoft Yahei">';//失败
        tipHtml += '<span class="mLayOk_s1" >';

        if (success)
            tipHtml += '<span style="top:0px;"><image style="margin:10px;" src="' + HomeUrlUtils.homeUrl + '/images/success.png"/>' + msg + '</span>';
        else
            tipHtml += '<span style="top:0px;"><image style="margin:10px;" src="' + HomeUrlUtils.homeUrl + '/images/error.png"/>' + msg + '</span>';
        tipHtml += '</span >';
        tipHtml += '</div>';

        $(document.body).append($(tipHtml));

        //if (!autoDisplay) {
        //手动关闭
        $("#tipPannel").click(() => {
            Utils.closeTip("tipPannel");
        });
        //}

        var toID;
        if (autoDisplay) {
            toID = autoCloseTip(interval);
        }

        $("#tipPannel").mouseover(() => {
            if (toID)
                clearTimeout(toID);
        }).mouseout(() => {
            autoCloseTip(interval);
        });

        function autoCloseTip(interval) {
            var toID = setTimeout(function () {
                Utils.closeTip("tipPannel");
                if (url) {
                    location.href = url;
                }
            }, interval);
            return toID;
        }
    }

    /******
     * 信息显示，用于表单验证
     * @param dom         <> 验证表单（表单需要id）
     * @param msg         <> 提示信息
     * @param success     <> 成功OR失败
     * @param autoDisplay <> 是否自动隐藏
     * @param interval    <> 自动隐藏时间
     * 
     ******/
    static tipShow(dom: JQuery, success?: boolean, msg?: string, autoDisplay?: boolean, interval?: number): void {
        $("#tipPannel_" + dom.attr("name")).remove();
        if (success == undefined)
            success = true;
        if (autoDisplay == undefined)
            autoDisplay = false;
        if (!interval)
            interval = 3000;
        //容器
        var tipHtml = '<div class="nmf_aler" id="tipPannel_' + dom.attr("id") + '">';
        if (success)
            tipHtml += '<img class="tipImg" src="' + HomeUrlUtils.homeUrl + '/images/nico3.png">';//成功
        else {
            tipHtml += '<img class="tipImg" src="' + HomeUrlUtils.homeUrl + '/images/nico2.png">';//失败
            tipHtml += '<div class="nmf_st1">' + msg + '</div>';
        }
        tipHtml += '</div>';
        $(tipHtml).insertAfter(dom);

        if (autoDisplay) {
            autoCloseTip(interval);
        }

        function autoCloseTip(interval) {
            setTimeout(function () {
                Utils.closeTip("tipPannel");
            }, interval);
        }
    }

    static closeTip(id: string): void {
        $("#" + id).hide();
        $("#" + id).remove();
    }

    static isPC(): string {
        var userAgentInfo = navigator.userAgent;
        var Agents = ["Android", "iPhone",
            "SymbianOS", "Windows Phone",
            "iPad", "iPod"];
        var flag = "true";
        for (var v = 0; v < Agents.length; v++) {
            if (userAgentInfo.indexOf(Agents[v]) > 0) {
                flag = "false";
                break;
            }
        }
        return flag;
    }
    static urlToMap(url: string): any {
        url = decodeURIComponent(url);
        var objResult = {};
        $.each(url.split('&'), function (i, item) {
            var prm = item.split('=');
            if (prm[0] && prm[1]) {
                objResult[prm[0]] = prm[1];
            }
        });
        return objResult;
    }
    static mapToUrl(map: any): string {
        return $.param(map);
    }
    static clone(obj: any): any {
        return $.extend(true, {}, obj);
    }

    /******
    * 时间格式
    * @param time         <> 时间字符串
    * @param format       <> 时间格式
    * 
    ******/
    static formatTime(time: string, fmt: string): string {
        var S = '';
        if (time.indexOf("-") >= 0)
            time = time.replace(/-/g, "/");
        if (time.indexOf(".") > 0) {
            time = time.substring(0, time.indexOf("."));
            S = time.substring(time.indexOf(".") + 1);
        }
        var datetime = eval("(new Date('" + time + "'))")
        var o = {
            "M+": datetime.getMonth() + 1, //月份         
            "d+": datetime.getDate(), //日         
            "h+": datetime.getHours() % 12 == 0 ? 12 : datetime.getHours() % 12, //小时         
            "H+": datetime.getHours(), //小时         
            "m+": datetime.getMinutes(), //分         
            "s+": datetime.getSeconds(), //秒         
            "q+": Math.floor((datetime.getMonth() + 3) / 3), //季度         
            "S": S //毫秒         
        };
        var week = {
            "0": "/u65e5",
            "1": "/u4e00",
            "2": "/u4e8c",
            "3": "/u4e09",
            "4": "/u56db",
            "5": "/u4e94",
            "6": "/u516d"
        };
        if (/(y+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (datetime.getFullYear() + "").substr(4 - RegExp.$1.length));
        }
        if (/(E+)/.test(fmt)) {
            fmt = fmt.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "/u661f/u671f" : "/u5468") : "") + week[datetime.getDay() + ""]);
        }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(fmt)) {
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            }
        }
        return fmt;
    }
}