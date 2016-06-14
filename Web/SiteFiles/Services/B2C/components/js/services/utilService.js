var utilService = {
    getDate: function (d) {
        return d.substr(0, d.indexOf('T'));
    },
    getDateTime: function (d) {
        return d.substr(0, d.indexOf('T')) + ' ' + d.substr(d.indexOf('T') + 1);
    },
    render: function (controllerName, controller, isAppend) {
        var i = 0;
        if (!isAppend) {
            $("." + controllerName + "Html").remove();
        }
        $("." + controllerName).each(function () {
            $(this).attr('id', controllerName + i++);
            var html = template.render($(this).attr('id'), controller);
            var div = $('<div>' + html + '</div>');
            div.children().addClass(controllerName + 'Html');
            $(this).after(div.html());
        });

    },
    getUrlVar: function (key) {
        var result = new RegExp(key + "=([^&]*)", "i").exec(window.location.search);
        return result && decodeURIComponent(result[1]) || "";
    },
    urlToMap: function (url) {
        url = decodeURIComponent(url);
        var objResult = {};
        $.each(url.split('&'), function (i, item) {
            var prm = item.split('=');
            if (prm[0] && prm[1]) {
                objResult[prm[0]] = prm[1];
            }
        });
        return objResult;
    },
    mapToUrl: function (map) {
        return $.param(map);
    },
    clone: function (obj) {
        return $.extend(true, {}, obj);
    },
    isPC: function () {
        var userAgentInfo = navigator.userAgent;
        var Agents = ["Android", "iPhone",
                    "SymbianOS", "Windows Phone",
                    "iPad", "iPod"];
        var flag = true;
        for (var v = 0; v < Agents.length; v++) {
            if (userAgentInfo.indexOf(Agents[v]) > 0) {
                flag = false;
                break;
            }
        }
        return flag;
    }
};