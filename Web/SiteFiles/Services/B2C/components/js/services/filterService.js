var filterService = {};

if (top.$pageInfo.apiUrl)
    filterService.baseUrl = top.$pageInfo.apiUrl + '/filter/';
else if (top.$pageInfo.rootUrl)
    filterService.baseUrl = top.$pageInfo.rootUrl + '/filter/';
else
    filterService.baseUrl = $pageInfo.rootUrl + '/filter/';

filterService.getUrl = function(action, id){
  if (id){
    return filterService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&callback=?';
  }
  return filterService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&callback=?';
}

filterService.getResults = function(postParams, success){
  $.ajax({
    type: 'POST',
    url: filterService.getUrl('PostResults'),
    data: postParams,
    success: success
  });
};