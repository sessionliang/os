var consultationService = {};

if (top.$pageInfo.apiUrl)
    consultationService.baseUrl = top.$pageInfo.apiUrl + '/consultation/';
else if (top.$pageInfo.rootUrl)
    consultationService.baseUrl = top.$pageInfo.rootUrl + '/consultation/';
else
    consultationService.baseUrl = $pageInfo.rootUrl + '/consultation/';

consultationService.getUrl = function (action, id) {
    if (id) {
        return consultationService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&callback=?';
    }
    return consultationService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&callback=?';
}

consultationService.getConsultationList = function (keywords, type, pageIndex, prePageNum, success) {
    var data = { channelID: $pageInfo.channelID, contentID: $pageInfo.contentID, keywords: keywords, type: type, pageIndex: pageIndex, prePageNum: prePageNum };
    $.getJSON(consultationService.getUrl('getConsultationList'), data, success);
};

consultationService.saveConsultation = function (question, type, success) {
    var data = { channelID: $pageInfo.channelID, contentID: $pageInfo.contentID, question: question, type: type };
    $.getJSON(consultationService.getUrl('saveConsultation'), data, success);
};
