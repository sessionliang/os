var commentService = {};

if (top.$pageInfo.apiUrl)
    commentService.baseUrl = top.$pageInfo.apiUrl + '/comment/';
else if (top.$pageInfo.rootUrl)
    commentService.baseUrl = top.$pageInfo.rootUrl + '/comment/';
else
    commentService.baseUrl = $pageInfo.rootUrl + '/comment/';

commentService.getUrl = function(action, id){
  if (id){
    return commentService.baseUrl + action + '/' + id + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&contentID=' + $pageInfo.contentID + '&callback=?';
  }
  return commentService.baseUrl + action + '?publishmentSystemID=' + $pageInfo.publishmentSystemID + '&channelID=' + $pageInfo.channelID + '&contentID=' + $pageInfo.contentID + '&callback=?';
}

commentService.getCommentParameter = function(success){
  $.getJSON(commentService.getUrl('GetCommentParameter'), null, success);
};

commentService.good = function(id){
  $.getJSON(commentService.getUrl('Good', id), null, notifyService.successCallback);
};

commentService.submitComment = function(content, success){
  var data = {content : content};
  $.getJSON(commentService.getUrl('SubmitComment'), data, success);
};