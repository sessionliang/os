var surveyController = surveyController || {};

surveyController.totalNum = 0;
surveyController.evaluations = [];
surveyController.applyFiles = [];
surveyController.isAnonymous = true;
surveyController.user = {};
surveyController.html = "";

surveyController.isSurvey = false;


surveyController.submitSurveyQuestionnaire = function () {
    surveyService.submitSurveyQuestionnaire($('#commit_form').serialize(), function (data) {
        if (data.isSuccess) {
            notifyService.success(data.successMessage);
            utilService.render('surveyController', surveyController);
            $(".comment_box").html("您的调查问卷提交完成，感谢您的参与！");
        } else {
            notifyService.error(data.errorMessage);
        }
    });
};


surveyController.getFunctionFiles = function () {
    surveyService.getFunctionFiles(function (data) { 
        if (data.isSuccess) {

            surveyController.totalNum = data.totalNum;

            surveyController.user = data.user;
            surveyController.isAnonymous = data.isAnonymous;

            surveyController.isSurvey = data.isSurvey;
            if (surveyController.isSurvey) {
                utilService.render('surveyController', surveyController);
                $(".comment_box").html("您已经提交过调查问卷，感谢您的参与！");
            }
            else {
                if (data.trialapplyFiles) {
                    surveyController.applyFiles = data.trialapplyFiles;

                    var trialapplyInfo_div = $("<div />");
                    for (var p in surveyController.applyFiles) {
                        var applyFileInfo = surveyController.applyFiles[p];

                        var input = surveyController.getInput(applyFileInfo);

                        input.appendTo(trialapplyInfo_div);
                    }
                    surveyController.html = trialapplyInfo_div;
                }

                utilService.render('surveyController', surveyController);

                $("#commit_form").html(surveyController.html);
            }
        }
    });
};

$(function () {
    surveyController.getFunctionFiles();
});


surveyController.getInput = function (_typestyleobj) {
    var divobj;
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
        var type = "span";
        if (typestyleinfo.displayName.length > 10)
            type = "div";
        var span = $('<' + type + ' />', { text: typestyleinfo.displayName  , for: typestyleinfo.attributeName });
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
        var type = "span";
        if (typestyleinfo.displayName.length > 10)
            type = "div";
        var span = $('<' + type + ' />', { text: typestyleinfo.displayName , for: typestyleinfo.attributeName });
        divobj.append(span).append(inputobj);
        return divobj;
    }
    else if (eInputTypeUtils.equals(typestyleinfo.inputType, eInputType.Radio)) {
        var span = $('<div />', { id: typestyleinfo.attributeName + "_span", text: typestyleinfo.displayName , for: typestyleinfo.attributeName });
        divobj.append(span);

        var items = typestyleinfo.styleItems;
        for  (item in items) {
            var options = $('<input />', {
                id: typestyleinfo.attributeName + "_" + items[i].itemValue,
                    name: typestyleinfo.attributeName,
                    type: "radio",
                    value: items[i].itemValue,
                    checked: items[i].isSelected
        });
        span = $('<span />', { text: items[i].itemTitle, for: typestyleinfo.attributeName + "_" +items[i].itemValue });
            divobj.append(options).append(span);
        }
        var spanHelpText = typeStyleUtils.spanHelpText(typestyleinfo.helpText,
             typestyleinfo.attributeName);
        divobj.append(inputobj).append(spanHelpText);
        return divobj;
    }
    else if (eInputTypeUtils.equals(typestyleinfo.inputType, eInputType.CheckBox)) {
        var span = $('<div />', { id: typestyleinfo.attributeName + "_span", text: typestyleinfo.displayName , for: typestyleinfo.attributeName });
        divobj.append(span);

        var items = typestyleinfo.styleItems;
        for (i in items) {
            var options = $('<input />', {
                id: typestyleinfo.attributeName + "_" + items[i].itemValue,
                name: typestyleinfo.attributeName,
                type: "checkbox",
                value: items[i].itemValue,
                checked: items[i].isSelected
            });
            span = $('<span />', { text: items[i].itemTitle, for: typestyleinfo.attributeName + "_" +items[i].itemValue });
            divobj.append(options).append(span);
        }
        var spanHelpText = typeStyleUtils.spanHelpText(typestyleinfo.helpText,
             typestyleinfo.attributeName);
        divobj.append(inputobj).append(spanHelpText);
        return divobj;
    }
}
