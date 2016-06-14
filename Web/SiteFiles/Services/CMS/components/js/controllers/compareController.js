var compareController = compareController || {};

compareController.totalNum = 0;
compareController.evaluations = [];
compareController.applyFiles = [];
compareController.isAnonymous = true;
compareController.user = {};
compareController.html = "";

compareController.isCompare = false;


compareController.submitCompare = function () {
    compareService.submitCompare($('#commit_form').serialize(), function (data) {
        if (data.isSuccess) {
            notifyService.success(data.successMessage);
            utilService.render('compareController', compareController);
            $(".comment_box").html("您的反馈提交完成，感谢您的参与！");
        } else {
            notifyService.error(data.errorMessage);
        }
    }); 
};


compareController.getFunctionFiles = function () {
    compareService.getFunctionFiles(function (data) { 
        if (data.isSuccess) {

            compareController.totalNum = data.totalNum;

            compareController.user = data.user;
            compareController.isAnonymous = data.isAnonymous;

            compareController.isCompare = data.isCompare;
            if (compareController.isCompare) {
                utilService.render('compareController', compareController);
                $(".comment_box").html("您已经提交过反馈，感谢您的参与！");
            }
            else {
                if (data.trialapplyFiles) {
                    compareController.applyFiles = data.trialapplyFiles;

                    var trialapplyInfo_div = $("<div />");
                    for (var p in compareController.applyFiles) {
                        var applyFileInfo = compareController.applyFiles[p];

                        var input = typeStyleUtils.getInput(applyFileInfo);

                        input.appendTo(trialapplyInfo_div);
                    }
                    compareController.html = trialapplyInfo_div;
                }

                utilService.render('compareController', compareController);

                $("#commit_form").html(compareController.html);
            }
        }
    });
};

$(function () {
    compareController.getFunctionFiles();
});


compareController.getInput = function (_typestyleobj) {
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
        var span = $('<div />', { id: typestyleinfo.AttributeName + "_span", text: typestyleinfo.displayName + "：", for: typestyleinfo.attributeName });
        divobj.append(span);

        var items = typestyleinfo.styleItems;
        for (var item in items) {
            var options = $('<input />', {
                id: typestyleinfo.attributeName + "_" + item.itemValue,
                name: typestyleinfo.attributeName,
                type: "radio",
                value: item.ItemValue,
                class: "comment_input",
                checked: item.IsSelected
            });
            span = $('<span />', { text: item.ItemTitle, for: typestyleinfo.attributeName + "_" + item.itemValue }); 
        }
        var spanHelpText = typeStyleUtils.spanHelpText(typestyleinfo.helpText,
             typestyleinfo.attributeName);
        divobj.append(inputobj).append(spanHelpText);
        return divobj;
    }
    else if (eInputTypeUtils.equals(typestyleinfo.inputType, eInputType.CheckBox)) {
        var span = $('<div />', { id: typestyleinfo.AttributeName + "_span", text: typestyleinfo.displayName + "：", for: typestyleinfo.attributeName });
        divobj.append(span);

        var items = typestyleinfo.styleItems;
        for (var item in items) {
            var options = $('<input />', {
                id: typestyleinfo.attributeName + "_" + item.itemValue,
                name: typestyleinfo.attributeName,
                type: "checkbox",
                value: item.ItemValue,
                class: "comment_input",
                checked: item.IsSelected
            });
            span = $('<span />', { text: item.ItemTitle, for: typestyleinfo.attributeName + "_" + item.itemValue }); 
        }
        var spanHelpText = typeStyleUtils.spanHelpText(typestyleinfo.helpText,
             typestyleinfo.attributeName);
        divobj.append(inputobj).append(spanHelpText);
        return divobj;
    }
}
