/// <reference path="evaluationService.js" />
var evaluationService = {};

if (top.$pageInfo.apiUrl)
    evaluationService.baseUrl = top.$pageInfo.apiUrl + '/evaluation/';
else if (top.$pageInfo.rootUrl)
    evaluationService.baseUrl = top.$pageInfo.rootUrl + '/evaluation/';
else
    evaluationService.baseUrl = $pageInfo.rootUrl + '/evaluation/';

evaluationService.getUrl = function (action, id) {
    if (id) {
        return evaluationService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&ID=' + $pageInfo.contentID + '&callback=?';
    }
    return evaluationService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&ID=' + $pageInfo.contentID + '&callback=?';
}

evaluationService.getFormUrl = function (action, id) {
    if (id) {
        return evaluationService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&ID=' + $pageInfo.contentID + '&callback=?';
    }
    return evaluationService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&ID=' + $pageInfo.contentID + '&callback=?';
}


evaluationService.GetEvaluationParameter = function (success) {
    $.getJSON(evaluationService.getUrl('GetEvaluationParameter'), null, success);
};


evaluationService.submitEvaluation = function (description, compositeScore, success) {
    var data = { description: description, compositeScore: compositeScore };
    $.getJSON(evaluationService.getUrl('SubmitEvaluation'), data, success);
};


evaluationService.submitEvaluation = function (evaluationpram, success) {
    var data = evaluationpram;
    $.getJSON(evaluationService.getUrl('SubmitEvaluation'), data, success);
};

evaluationService.getFunctionFiles = function (success) {
    $.getJSON(evaluationService.getUrl('GetFunctionFiles'), null, success);
};