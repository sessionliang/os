/// <reference path="../../defs/jquery.d.ts" />
/// <reference path="../../defs/weixinjsbridge.d.ts" />
/// <reference path="../../defs/jqueryqrcode.d.ts" />

class ShareUtils {

    constructor(private title: string, private img_url: string, private link: string, private desc: string) {
        this.link = UrlUtils.getAbsoluteUrl(this.link);
    }

    sendAppMessage(): void {
        WeixinJSBridge.invoke('sendAppMessage', {
            "img_url": this.img_url,
            "img_width": "120",
            "img_height": "120",
            "link": this.link,
            "desc": this.desc,
            "title": this.title
        }, function (res) {

            });
    }

    shareTimeline(): void {
        WeixinJSBridge.invoke('shareTimeline', {
            "img_url": this.img_url,
            "img_width": "120",
            "img_height": "120",
            "link": this.link,
            "desc": this.desc,
            "title": this.title
        }, function (res) {

            });
    }

    pageLoad(): void {
        document.addEventListener('WeixinJSBridgeReady', () => {
            WeixinJSBridge.on('menu:share:appmessage', (argv) => {
                this.sendAppMessage();
            });
            WeixinJSBridge.on('menu:share:timeline', (argv) => {
                this.shareTimeline();
            });
        }, false);

        $("#btnShare").click(() => {
            Utils.mask();
            $("#popShare").show();
            return false;
        });

        $("#popShare a").click(() => {
            $("#popShare").hide();
            Utils.unMask();
        });

        $("#popShare .share_weixin").click(() => {
            Utils.mask();
            if (!$('#qrCode').html()) {
                $('#qrCode').qrcode({ width: 200, height: 200, text: location.href });
            }
            $('#popQRCode').show();
        });

        $("#popQRCode .btn-default").click(() => {
            $("#popQRCode").hide();
            Utils.unMask();
        });

        window["_bd_share_config"] =
        {
            common: {
                bdText: this.desc,
                bdDesc: this.desc,
                bdUrl: this.link,
                bdPic: this.img_url
            },
            share: [{
                "bdSize": 16
            }]
        };

        $.getScript("http://bdimg.share.baidu.com/static/api/js/share.js");

        var int = setInterval(() => {
            if ($('#divShareIcons').hasClass("bdshare-button-style0-16")) {
                $('#divShareIcons').removeClass('bdshare-button-style0-16').show();
                clearInterval(int);
            }
        }, 1000);
    }

}