var trialApplyController = trialApplyController || {};

trialApplyController.totalNum = 0;
trialApplyController.evaluations = [];
trialApplyController.applyFiles = [];
trialApplyController.isAnonymous = true;
trialApplyController.user = {};
trialApplyController.html = "";

trialApplyController.isFlow = false;
trialApplyController.showNum = trialApplyController.showNum || 0;
trialApplyController.showIndex = 0;


trialApplyController.submitApply = function () {
    //if (isLoginFirst() && trialApplyController.isAnonymous) {
    //    userController.login();
    //    return;
    //}
    //var description = $('#description').val();
    //var compositeScore = $('#compositeScore').val();

    //if (!compositeScore) {
    //    $('#compositeScore').focus();
    //    return;
    //}
    //if (description) {
    trialApplyService.submitApply($('#trialapply_form').serialize(), function (data) { 
        if (data.isSuccess) {
            if (!trialApplyController.isFlow) {
                trialApplyController.totalNum += 1;
                data.evaluation.floor = trialApplyController.totalNum;
                trialApplyController.evaluations.splice(0, 0, data.evaluation);

                //显示多少条
                if (trialApplyController.showNum > 0) {
                    if (trialApplyController.totalNum > trialApplyController.showNum) {
                        trialApplyController.evaluations = trialApplyController.evaluations.slice(trialApplyController.evaluations - trialApplyController.showNum, trialApplyController.showNum);
                    }
                }

                notifyService.success(data.successMessage);
                utilService.render('trialApplyController', trialApplyController);
            }
        } else {
            notifyService.error(data.errorMessage);
        }
    });
    //} else {
    //    $('#description').focus();
    //}
};

trialApplyController.getCommentParameter = function () {
    trialApplyService.getCommentParameter(function (data) {

        if (data.isSuccess) {
            trialApplyController.totalNum = data.totalNum;
            if (data.evaluations) {
                trialApplyController.evaluations = data.evaluations;
            }
            trialApplyController.user = data.user;
            trialApplyController.isAnonymous = data.isAnonymous;

            //显示多少条
            if (trialApplyController.showNum > 0) {
                if (trialApplyController.totalNum > trialApplyController.showNum) {
                    trialApplyController.evaluations = trialApplyController.evaluations.slice(trialApplyController.evaluations - trialApplyController.showNum, trialApplyController.showNum);
                }
            }
            if (trialApplyController.isFlow) {
                trialApplyController.showIndex = trialApplyController.showIndex + trialApplyController.showNum;
            }

            utilService.render('trialApplyController', trialApplyController);
        }

    });
};


trialApplyController.getFunctionFiles = function () {
    trialApplyService.getFunctionFiles(function (data) { 
        if (data.isSuccess) {

            trialApplyController.totalNum = data.totalNum;
           
            trialApplyController.user = data.user;
            trialApplyController.isAnonymous = data.isAnonymous;

            if (data.trialapplyFiles) {
                trialApplyController.applyFiles = data.trialapplyFiles;

                var trialapplyInfo_div = $("<div />");
                for (var p in trialApplyController.applyFiles) {
                    var applyFileInfo = trialApplyController.applyFiles[p];

                    var input = typeStyleUtils.getInput(applyFileInfo);
                    //input.appendTo($("#trialapplyinfo_div"));

                    input.appendTo(trialapplyInfo_div);
                }
                trialApplyController.html = trialapplyInfo_div;
            }

            utilService.render('trialApplyController', trialApplyController);

            $("#trialapply_form").html(trialApplyController.html);
        }

    });
};

$(function () {
    trialApplyController.getFunctionFiles();
});