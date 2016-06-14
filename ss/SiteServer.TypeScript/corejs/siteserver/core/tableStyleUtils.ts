/// <reference path="../../../corejs/siteserver/siteserver.cms.ts" />
/// <reference path="../../../corejs/siteserver/siteserver.ts" />
//初始化表单
class TableStyleUtils {
    static render(formCollection: _map, styleInfoArray: Object[], isEdit?: boolean, isValidate?: boolean, pannel?: HTMLElement): string {
        var parsedHtml: string = StringUtils.empty;
        if (!pannel)
            pannel = new HTMLTableElement();
        var htmlTemplate: string = "<tr><td>{0}</td><td>{1}</td></tr>";
        if (pannel.nodeName === "UL")
            htmlTemplate = "<li><span class='mcr_s1'>{0}</span>{1}</li>";
        for (var i = 0; i < styleInfoArray.length; i++) {
            var styleInfo = styleInfoArray[i];
            var text = InputTypeParser.text(styleInfo["displayName"]);
            var html = InputTypeParser.parse(styleInfo, formCollection, isEdit, isValidate);
            parsedHtml += StringUtils.formatIngnoreFalse(htmlTemplate, text, html);
        }
        return parsedHtml;
    }
}

//解析表单元素
class InputTypeParser {
    static parse(styleInfo: Object, formCollection: _map, isEdit?: boolean, isValidate?: boolean): string {
        var retval: string = StringUtils.empty;//返回值

        if (isValidate == undefined || isValidate == null)
            styleInfo["isValidate"] = false;
        else
            styleInfo["isValidate"] = isValidate;
        switch (styleInfo["inputType"]) {
            case eInputType.Text:
                retval = InputTypeParser.parseText(styleInfo, formCollection, isEdit, styleInfo["isValidate"]);
                break;
            case eInputType.TextArea:
                retval = InputTypeParser.parseTextArea(styleInfo, formCollection, isEdit, styleInfo["isValidate"]);
        }

        return retval;
    }
    static text(displayName: string): string {
        var html: string = "{0}:";
        return StringUtils.formatIngnoreFalse(html, displayName);
    }

    //获取验证属性
    static getValidateAttributes(isValidate: boolean, displayName: string, isRequired: boolean, minNum: number, maxNum: number, validateType: string, regExp: string, errorMessage: string): string {
        var retval: string = StringUtils.empty;//返回值
        if (isValidate) {
            retval = StringUtils.formatIngnoreFalse("isValidate='{0}' displayName='{1}' isRequire='{2}' minNum='{3}' maxNum='{4}' validateType='{5}' regExp='{6}' errorMessage='{7}'", String(isValidate), displayName, String(isRequired), String(minNum), String(maxNum), validateType, regExp, errorMessage);
        }
        return retval;
    }

    //获取表单的值
    static getValue(attributeName: string, formCollection: _map, defaultValue: any, isEdit?: boolean): string {
        var retval: string = StringUtils.empty;//返回值
        if (formCollection != null && formCollection[attributeName] != null) {
            retval = formCollection[attributeName];
        }
        if (isEdit && StringUtils.isNullOrEmpty(retval)) {
            retval = defaultValue;
        }
        return retval;
    }

    //添加帮助文字
    static addHelpText(inputHtml: string, helpText: string): string {
        if (!StringUtils.isNullOrEmpty(helpText)) {
            inputHtml += StringUtils.formatIngnoreFalse("&nbsp;<span style='color:#999'>{0}</span>", helpText);
        }
        return inputHtml;
    }

    //解析文本框
    static parseText(styleInfo: Object, formCollection: _map, isEdit?: boolean, isValidate?: boolean, additionalAttributes?: string): string {
        var retval: string = StringUtils.empty;//返回值
        var attributeName = styleInfo["attributeName"];
        //获取表单验证属性
        var validateAttributes = InputTypeParser.getValidateAttributes(isValidate, styleInfo["displayName"], styleInfo["additional"]["isRequired"], styleInfo["additional"]["minNum"], styleInfo["additional"]["maxNum"], styleInfo["additional"]["validateType"], styleInfo["additional"]["regExp"], styleInfo["additional"]["errorMessage"]);

        //获取表单值
        var value = InputTypeParser.getValue(attributeName, formCollection, styleInfo["defaultValue"], isEdit);
        //对表单值进行decode
        value = decodeURIComponent(value);
        //表单宽度
        var width: string = styleInfo["additional"]["width"];
        if (StringUtils.isNullOrEmpty(width)) {
            width = styleInfo["isSingleLine"] ? "380px" : "220px";
        }
        var style = StringUtils.formatIngnoreFalse("style='width:{0};'", StringUtils.toWidth(width));
        retval = StringUtils.formatIngnoreFalse("<input id='{0}' name='{0}' type='text' class='input_text' value='{1}' {2} {3} {4} />",
            attributeName,
            value,
            additionalAttributes,
            style,
            validateAttributes);

        //添加帮助信息
        retval = InputTypeParser.addHelpText(retval, styleInfo["helpText"]);

        //添加验证信息
        if (isValidate) {
            retval += StringUtils.formatIngnoreFalse("&nbsp;<span id='{0}_msg' style='color:red;display:none;'>*</span>", attributeName);
            retval += StringUtils.formatIngnoreFalse("<script>ss.extendjs.event_observe('{0}', 'blur', window.ss.extendjs.validater.checkAttributeValue);</script>", attributeName);
        }

        return retval;
    }

    //解析文本域
    static parseTextArea(styleInfo: Object, formCollection: _map, isEdit?: boolean, isValidate?: boolean, additionalAttributes?: string): string {
        var retval: string = StringUtils.empty;//返回值
        var attributeName = styleInfo["attributeName"];
        //获取表单验证属性
        var validateAttributes = InputTypeParser.getValidateAttributes(isValidate, styleInfo["displayName"], styleInfo["additional"]["isRequired"], styleInfo["additional"]["minNum"], styleInfo["additional"]["maxNum"], styleInfo["additional"]["validateType"], styleInfo["additional"]["regExp"], styleInfo["additional"]["errorMessage"]);

        //获取表单值
        var value = InputTypeParser.getValue(attributeName, formCollection, styleInfo["defaultValue"], isEdit);
        //对表单值进行decode
        value = decodeURIComponent(value);
        //表单宽度
        var width: string = styleInfo["additional"]["width"];
        if (StringUtils.isNullOrEmpty(width)) {
            width = styleInfo["isSingleLine"] ? "380px" : "220px";
        }
        var height: string = styleInfo["additional"]["height"];
        if (StringUtils.isNullOrEmpty(height) || height == "0") {
            height = "110px";
        }
        var style = StringUtils.formatIngnoreFalse("style='width:{0};height:{1};'", StringUtils.toWidth(width), StringUtils.toHeight(height));
        retval = StringUtils.formatIngnoreFalse("<textarea id='{0}' name='{0}' class='input_text' value='{1}' {2} {3} {4} >{1}</textarea>",
            attributeName,
            value,
            additionalAttributes,
            style,
            validateAttributes);

        //添加帮助信息
        retval = InputTypeParser.addHelpText(retval, styleInfo["helpText"]);

        //添加验证信息
        if (isValidate) {
            retval += StringUtils.formatIngnoreFalse("&nbsp;<span id='{0}_msg' style='color:red;display:none;'>*</span>", attributeName);
            retval += StringUtils.formatIngnoreFalse("<script>ss.extendjs.event_observe('{0}', 'blur', window.ss.extendjs.validater.checkAttributeValue);</script>", attributeName);
        }

        return retval;
    }
}