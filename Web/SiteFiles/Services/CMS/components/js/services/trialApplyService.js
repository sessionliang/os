 
var trialApplyService = {};

if (top.$pageInfo.apiUrl)
    trialApplyService.baseUrl = top.$pageInfo.apiUrl + '/trial/';
else if (top.$pageInfo.rootUrl)
    trialApplyService.baseUrl = top.$pageInfo.rootUrl + '/trial/';
else
    trialApplyService.baseUrl = $pageInfo.rootUrl + '/trial/';

trialApplyService.getUrl = function (action, id) {
    if (id) {
        return trialApplyService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&contentID=' + $pageInfo.contentID + '&callback=?';
    }
    return trialApplyService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&contentID=' + $pageInfo.contentID + '&callback=?';
}

trialApplyService.getUrl = function (action, id, tableStyle) {
    if (id) {
        return trialApplyService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&contentID=' + $pageInfo.contentID + '&tableStyle=' + tableStyle + '&callback=?';
    }
    return trialApplyService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&contentID=' + $pageInfo.contentID + '&tableStyle=' + tableStyle + '&callback=?';
}

trialApplyService.getFormUrl = function (action, id) {
    if (id) {
        return trialApplyService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&ID=' + $pageInfo.ID + '&callback=?';
    }
    return trialApplyService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&ID=' + $pageInfo.ID + '&callback=?';
}


trialApplyService.getEvaluationParameter = function (success) {
    $.getJSON(trialApplyService.getUrl('GetEvaluationParameter'), null, success);
};


trialApplyService.getFunctionFiles = function (success) {
    $.getJSON(trialApplyService.getUrl('GetFunctionFiles', '', "TrialApplyContent"), null, success);
};

trialApplyService.submitApply = function (applypram, success) {
    var data = applypram;
    $.getJSON(trialApplyService.getUrl('SubmitApply'), data, success);
};