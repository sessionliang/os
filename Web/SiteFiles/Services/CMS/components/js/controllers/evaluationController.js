var evaluationController = evaluationController || {};

evaluationController.totalNum = 0;
evaluationController.evaluations = [];
evaluationController.isAnonymous = true;
evaluationController.user = {};
evaluationController.html = "";

evaluationController.isSubmit = false;

evaluationController.isFlow = false;
evaluationController.showNum = evaluationController.showNum || 0;
evaluationController.showIndex = 0;

evaluationController.reference = function (ID) {
    var item;
    $.each(evaluationController.evaluations, function (index, val) {
        if (val.ID == ID) {
            item = val;
        }
    });
    var obj = $('.comment_textarea');
    if (item && obj.length > 0) {
        var val = "\r\n[quote] 原帖由 " + item.userName;
        val += " 于 " + item.dateTime + " 发表\r\n";
        val += item.content + "[/quote]\r\n";
        obj.text(val);
        obj.focus();
    }
};

evaluationController.submitEvaluation = function () {
    if (isLoginFirst() && evaluationController.isAnonymous) {
        userService.login();
        return;
    }
    evaluationService.submitEvaluation($('#evaluation_form').serialize(), function (data) {
        if (data.isSuccess) {
            if (!evaluationController.isFlow) {
                evaluationController.totalNum += 1; 
                evaluationController.evaluations.splice(0, 0, data.evaluation);

                //显示多少条
                if (evaluationController.showNum > 0) {
                    if (evaluationController.totalNum > evaluationController.showNum) {
                        evaluationController.evaluations = evaluationController.evaluations.slice(evaluationController.evaluations - evaluationController.showNum, evaluationController.showNum);
                    }
                }

                notifyService.success(data.successMessage);
                utilService.render('evaluationController', evaluationController);
            }
            $("#btnSubmit").hide();
        } else {
            notifyService.error(data.errorMessage);
        }
    });
};

evaluationController.GetEvaluationParameter = function () {
    evaluationService.GetEvaluationParameter(function (data) {

        if (data.isSuccess) {
            evaluationController.totalNum = data.totalNum;
            if (data.evaluations) {
                evaluationController.evaluations = data.evaluations;
            }
            evaluationController.user = data.user;
            evaluationController.isAnonymous = data.isAnonymous;

            //显示多少条
            if (evaluationController.showNum > 0) {
                if (evaluationController.totalNum > evaluationController.showNum) {
                    evaluationController.evaluations = evaluationController.evaluations.slice(evaluationController.evaluations - evaluationController.showNum, evaluationController.showNum);
                }
            }
            if (evaluationController.isFlow) {
                evaluationController.showIndex = evaluationController.showIndex + evaluationController.showNum;
            }

            utilService.render('evaluationController', evaluationController);
        }

    });
};

evaluationController.getFunctionFiles = function () {
    evaluationService.getFunctionFiles(function (data) { 
        if (data.isSuccess) { 
            evaluationController.totalNum = data.totalNum;

            evaluationController.user = data.user;
            evaluationController.isAnonymous = data.isAnonymous;

            evaluationController.isSubmit = data.isSubmit;
            if (!evaluationController.isSubmit) {
                utilService.render('evaluationController', evaluationController);
                $(".comment_box").html("您已经提交过评价，感谢您的参与！");
            }
            else {
                if (data.trialapplyFiles) {
                    evaluationController.applyFiles = data.trialapplyFiles;

                    var trialapplyInfo_div = $("<div />");
                    for (var p in evaluationController.applyFiles) {
                        var applyFileInfo = evaluationController.applyFiles[p];

                        var input = typeStyleUtils.getInput(applyFileInfo);

                        input.appendTo(trialapplyInfo_div);
                    }
                    evaluationController.html = trialapplyInfo_div;
                }

                utilService.render('evaluationController', evaluationController);

                $("#evaluation_form").html(evaluationController.html);
            }
        }
    });
};

$(function () {
});


$(function () {
    evaluationController.getFunctionFiles();
    evaluationController.GetEvaluationParameter();
});