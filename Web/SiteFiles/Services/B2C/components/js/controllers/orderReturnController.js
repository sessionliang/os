var orderReturnController = {};

orderReturnController.order = {};

orderService.getReturnValue(function (data) {
  if (data.isSuccess){
    orderReturnController.order = data.order;
    utilService.render('orderReturnController', orderReturnController);
  }
});