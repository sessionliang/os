
var surveyService = {};

if (top.$pageInfo.apiUrl)
    surveyService.baseUrl = top.$pageInfo.apiUrl + '/survey/';
else if (top.$pageInfo.rootUrl)
    surveyService.baseUrl = top.$pageInfo.rootUrl + '/survey/';
else
    surveyService.baseUrl = $pageInfo.rootUrl + '/survey/';

surveyService.getUrl = function (action, id) {
    if (id) {
        return surveyService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&contentID=' + $pageInfo.contentID + '&callback=?';
    }
    return surveyService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&contentID=' + $pageInfo.contentID + '&callback=?';
}

surveyService.getUrl = function (action, id, tableStyle) {
    if (id) {
        return surveyService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&contentID=' + $pageInfo.contentID + '&tableStyle=' + tableStyle + '&callback=?';
    }
    return surveyService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&contentID=' + $pageInfo.contentID + '&tableStyle=' + tableStyle + '&callback=?';
}

surveyService.getFormUrl = function (action, id) {
    if (id) {
        return surveyService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&ID=' + $pageInfo.ID + '&callback=?';
    }
    return surveyService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&ID=' + $pageInfo.ID + '&callback=?';
}

 

surveyService.getFunctionFiles = function (success) {
    $.getJSON(surveyService.getUrl('GetFunctionFiles'), null, success);
};

surveyService.submitSurveyQuestionnaire = function (applypram, success) {
    var data = applypram;
    $.getJSON(surveyService.getUrl('SubmitSurveyQuestionnaire'), data, success);
};