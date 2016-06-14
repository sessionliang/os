var orderCommentService = {};

if (top.$pageInfo.apiUrl)
    orderCommentService.baseUrl = top.$pageInfo.apiUrl + '/orderComment/';
else if (top.$pageInfo.rootUrl)
    orderCommentService.baseUrl = top.$pageInfo.rootUrl + '/orderComment/';
else
    orderCommentService.baseUrl = $pageInfo.rootUrl + '/orderComment/';

orderCommentService.getUrl = function (action, id) {
    if (id) {
        return orderCommentService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&callback=?';
    }
    return orderCommentService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&callback=?';
}

orderCommentService.getOrderCommentList = function (type, pageIndex, prePageNum, success) {
    var data = { channelID: $pageInfo.channelID, contentID: $pageInfo.contentID, type: type, pageIndex: pageIndex, prePageNum: prePageNum };
    $.getJSON(orderCommentService.getUrl('getOrderCommentList'), data, success);
};
