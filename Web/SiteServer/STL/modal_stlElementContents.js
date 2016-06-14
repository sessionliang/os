var controller = {};

controller.channels = channels;
controller.channelNames = channels;
controller.filters = filters;

controller.addFilter = function(){
  controller.filters.push({});
  setTimeout(function() {
    $('[rel=tooltip]').tooltip({placement:"right"});
  }, 100);
  utilService.render('controller', controller);
};

utilService.render('controller', controller);