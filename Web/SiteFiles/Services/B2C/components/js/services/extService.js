var extService = {};

if (top.$pageInfo.apiUrl)
    extService.baseUrl = top.$pageInfo.apiUrl + '/ext/';
else if (top.$pageInfo.rootUrl)
    extService.baseUrl = top.$pageInfo.rootUrl + '/ext/';
else
    extService.baseUrl = $pageInfo.rootUrl + '/ext/';

extService.getUrl = function (action, id) {
    if (id) {
        return extService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&callback=?';
    }
    return extService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&callback=?';
}

extService.getGuesses = function (success) {
    $.get(extService.getUrl('GetGuesses'), null, success);
};
