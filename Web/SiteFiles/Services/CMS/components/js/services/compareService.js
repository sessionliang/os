
var compareService = {};

if (top.$pageInfo.apiUrl)
    compareService.baseUrl = top.$pageInfo.apiUrl + '/Compare/';
else if (top.$pageInfo.rootUrl)
    compareService.baseUrl = top.$pageInfo.rootUrl + '/Compare/';
else
    compareService.baseUrl = $pageInfo.rootUrl + '/Compare/';

compareService.getUrl = function (action, id) {
    if (id) {
        return compareService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&contentID=' + $pageInfo.contentID + '&callback=?';
    }
    return compareService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&contentID=' + $pageInfo.contentID + '&callback=?';
}

compareService.getUrl = function (action, id, tableStyle) {
    if (id) {
        return compareService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&contentID=' + $pageInfo.contentID + '&tableStyle=' + tableStyle + '&callback=?';
    }
    return compareService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&contentID=' + $pageInfo.contentID + '&tableStyle=' + tableStyle + '&callback=?';
}

compareService.getFormUrl = function (action, id) {
    if (id) {
        return compareService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&ID=' + $pageInfo.ID + '&callback=?';
    }
    return compareService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&ID=' + $pageInfo.ID + '&callback=?';
}

 

compareService.getFunctionFiles = function (success) {
    $.getJSON(compareService.getUrl('GetFunctionFiles'), null, success);
};

compareService.submitCompare = function (applypram, success) {
    var data = applypram;
    $.getJSON(compareService.getUrl('SubmitCompare'), data, success);
};