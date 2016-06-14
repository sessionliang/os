var utilService = {
    getDate: function (d) {
        return d.substr(0, d.indexOf('T'));
    },
    getDateTime: function (d) {
        return d.substr(0, d.indexOf('T')) + ' ' + d.substr(d.indexOf('T') + 1);
    },
    formatTime: function (time, fmt) {
        var S = '';
        if (time.indexOf("-") > 0)
            time = time.replace(/-/g, "/");
        if (time.indexOf(".") > 0) {
            time = time.substring(0, time.indexOf("."));
            S = time.substring(time.indexOf(".") + 1);
        }
        var datetime = eval("(new Date('" + time + "'))")
        var o = {
            "M+": datetime.getMonth() + 1,
            "d+": datetime.getDate(),
            "h+": datetime.getHours() % 12 == 0 ? 12 : datetime.getHours() % 12,
            "H+": datetime.getHours(),
            "m+": datetime.getMinutes(),
            "s+": datetime.getSeconds(),
            "q+": Math.floor((datetime.getMonth() + 3) / 3),
            "S": S
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
    },
    /*
    * 添加两个处理事件
    * onPreLoad -- 渲染之前，对数据源做处理
    * onLoaded  -- 渲染之后，执行自定义数据
    */
    render: function (controllerName, controller, onPreLoad, onLoaded) {
        if (typeof (onPreLoad) == "function") {
            onPreLoad(controller);
        }

        $("." + controllerName + "Html").remove();
        var i = 0;
        $("." + controllerName).each(function () {
            $(this).attr('id', controllerName + i++);
            var html = template.render($(this).attr('id'), controller);
            var div = $('<div>' + html + '</div>');
            div.children().addClass(controllerName + 'Html');
            $(this).after(div.html());
        });

        if (typeof (onLoaded) == "function") {
            onLoaded(controller);
        }
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
    random: function () {
        return parseInt(1000 * Math.random());
    },
    mapToUrl: function (map) {
        return $.param(map);
    },
    clone: function (obj) {
        return $.extend(true, {}, obj);
    },
    isEmail: function (str) {
        return /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/.test(str);
    },
    isMobile: function (str) {
        return /^0{0,1}1[0-9]{10}$/.test(str);
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

if (!$pageInfo) {
    var $pageInfo = { siteUrl: utilService.getUrlVar('siteUrl'), rootUrl: '', publishmentSystemID: utilService.getUrlVar('publishmentSystemID'), apiUrl: "/api" };
}



var eInputType = {
    CheckBox: 'CheckBox',
    Radio: 'Radio',
    SelectOne: 'SelectOne',
    SelectMultiple: 'SelectMultiple',
    Date: 'Date',
    DateTime: 'DateTime',
    Image: 'Image',
    Video: 'Video',
    File: 'File',
    Text: 'Text',
    TextArea: 'TextArea',
    TextEditor: 'TextEditor',
    RelatedField: 'RelatedField',
    SpecifiedValue: 'SpecifiedValue'
};

var eInputTypeUtils = eInputTypeUtils || {};

eInputTypeUtils.getValue = function (type) {

    if (type == eInputType.CheckBox) {
        return "CheckBox";
    }
    else if (type == eInputType.Radio) {
        return "Radio";
    }
    else if (type == eInputType.SelectOne) {
        return "SelectOne";
    }
    else if (type == eInputType.SelectMultiple) {
        return "SelectMultiple";
    }
    else if (type == eInputType.Date) {
        return "Date";
    }
    else if (type == eInputType.DateTime) {
        return "DateTime";
    }
    else if (type == eInputType.Image) {
        return "Image";
    }
    else if (type == eInputType.Video) {
        return "Video";
    }
    else if (type == eInputType.File) {
        return "File";
    }
    else if (type == eInputType.Text) {
        return "Text";
    }
    else if (type == eInputType.TextArea) {
        return "TextArea";
    }
    else if (type == eInputType.TextEditor) {
        return "TextEditor";
    }
    else if (type == eInputType.RelatedField) {
        return "RelatedField";
    }
    else if (type == eInputType.SpecifiedValue) {
        return "SpecifiedValue";
    }
    else {
        return "";
    }
}

eInputTypeUtils.equals = function (_valStr, _typeStr) {
    if (_valStr in eInputType) {
        if (!_typeStr) return false;
        if (eInputTypeUtils.getValue(_valStr).toLowerCase() == _typeStr.toLowerCase()) {
            return true;
        }
    } else {
        return false;
    }
    return false;
}


var typeStyleUtils = typeStyleUtils || {};
typeStyleUtils.getInput = function (_typestyleobj) {
    var divobj;
    var selectobj = $("<select/>");
    var checkboxobj = $("<input type=\"checkbox\"/>");
    var redioobj = $("<input type=\"redio\"/>");
    var typestyleinfo = _typestyleobj;
    var returnObj;
    if (typestyleinfo.isSingleLine)
        divobj = $("<div class='comment_item'/>");
    else
        divobj = $("<div style='float:left;' />");
    if (eInputTypeUtils.equals(typestyleinfo.inputType, eInputType.Text)) {
        var inputobj = $('<input />', {
            id: typestyleinfo.attributeName,
            name: typestyleinfo.attributeName,
            type: "text",
            value: typestyleinfo.defaultValue,
            class: "comment_input",
            placeholder: typestyleinfo.helpText
        });
        var span = $('<span />', { text: typestyleinfo.displayName, for: typestyleinfo.attributeName });
        divobj.append(span).append(inputobj);
        return divobj;
    }
    else if (eInputTypeUtils.equals(typestyleinfo.inputType, eInputType.TextArea)) {
        var inputobj = $("<TextArea />", {
            id: typestyleinfo.attributeName,
            name: typestyleinfo.attributeName,
            type: "text",
            value: typestyleinfo.defaultValue,
            class: "comment_input",
            placeholder: typestyleinfo.helpText
        });
        var span = $('<span />', { text: typestyleinfo.displayName, for: typestyleinfo.attributeName });
        divobj.append(span).append(inputobj);
        return divobj;
    }
    else if (eInputTypeUtils.equals(typestyleinfo.inputType, eInputType.SelectOne)) {

        var items = typestyleinfo.styleItems;
        var options = '';
        for (var item in items) {
            options += string.Format("<option value=\"{0}\" {2}>{1}</option>", item.itemValue, item.itemTitle, item.isSelected ? "selected" : "");
        }
        var inputobj = $("<select />", {
            id: typestyleinfo.attributeName,
            name: typestyleinfo.attributeName,
            type: "text",
            value: typestyleinfo.defaultValue,
            class: "comment_input",
            html: options
        });
        var span = $('<span />', { text: typestyleinfo.displayName + "：", for: typestyleinfo.attributeName });
        var spanHelpText = typeStyleUtils.spanHelpText(typestyleinfo.helpText,
             typestyleinfo.attributeName);
        divobj.append(span).append(inputobj).append(spanHelpText);
        return divobj;
    }
    else if (eInputTypeUtils.equals(typestyleinfo.inputType, eInputType.Radio)) {
        var span = $('<span />', { id: typestyleinfo.attributeName + "_span", text: typestyleinfo.displayName, for: typestyleinfo.attributeName });
        divobj.append(span);

        if (!typestyleinfo.isHorizontal)
            divobj.append("<br />");
        var items = typestyleinfo.styleItems;
        for (i in items) {
            var options = $('<input />', {
                id: typestyleinfo.attributeName + "_" + items[i].itemValue,
                name: typestyleinfo.attributeName,
                type: "radio",
                value: items[i].itemValue,
                checked: items[i].isSelected,
                text: items[i].itemTitle
            });
            span = $('<span />', { text: items[i].itemTitle, for: typestyleinfo.attributeName + "_" + items[i].itemValue });
            divobj.append(options).append(span);
            if (!typestyleinfo.isHorizontal)
                divobj.append("<br />");
        }
        var spanHelpText = typeStyleUtils.spanHelpText(typestyleinfo.helpText,
             typestyleinfo.attributeName);
        divobj.append(spanHelpText);
        return divobj;
    }
    else if (eInputTypeUtils.equals(typestyleinfo.inputType, eInputType.CheckBox)) {
        var span = $('<span />', { id: typestyleinfo.attributeName + "_span", text: typestyleinfo.displayName, for: typestyleinfo.attributeName });
        divobj.append(span);

        if (!typestyleinfo.isHorizontal)
            divobj.append("<br />");
        var items = typestyleinfo.styleItems;
        for (i in items) {
            var options = $('<input />', {
                id: typestyleinfo.attributeName + "_" + items[i].itemValue,
                name: typestyleinfo.attributeName,
                type: "checkbox",
                value: items[i].itemValue,
                checked: items[i].isSelected
            });
            span = $('<span />', { text: items[i].temTitle, for: typestyleinfo.attributeName + "_" + items[i].itemValue });
            divobj.append(options).append(span);
            if (!typestyleinfo.isHorizontal)
                divobj.append("<br />");
        }
        var spanHelpText = typeStyleUtils.spanHelpText(typestyleinfo.helpText,
             typestyleinfo.attributeName);
        divobj.append(inputobj).append(spanHelpText);
        return divobj;
    }
}

typeStyleUtils.spanHelpText = function (helpText, attributeName) {
    if (helpText == "")
        return "";
    var spanHelpText = $('<span />', {
        text: helpText,
        for: attributeName,
        class: "common_span"
    });
    return spanHelpText;
}

typeStyleUtils.getValidateAttributes = function (isValidate, displayName, isRequire, minNum, maxNum, validateType, regExp, errorMessage) {
    var str = "";
    if (isValidate) {
        str = "isValidate=" + isValidate + " displayName=" + displayName + " isRequire=" + isRequire + " minNum=" + minNum + " maxNum=" + maxNum + " validateType=" + validateType + " regExp=" + regExp + " errorMessage=" + errorMessage;
    }
    return str;
}

typeStyleUtils.getValidateHtmlString = function (styleInfo) {
    var str = "";
    if (styleInfo.Additional.IsValidate && styleInfo.InputType != eInputType.TextEditor) {
        str = '&nbsp;<span id="' + styleInfo.AttributeName + '_msg" style=""color:red;display:none;"">*</span>';
        str = + "<script>event_observe('" + styleInfo.AttributeName + "', 'blur', checkAttributeValue);</script> ";
    }
    return builder.ToString();
}