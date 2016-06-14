app.controller("touchController", function($scope, $http, touchService, notifyService){

    $scope.touches = [];
    $scope.touch = {touchBy : 'Action'};
    $scope.filterText = '';
    $scope.addMode = true;
    $scope.loading = true;

    $scope.getTouches = function () {
        touchService.query(function (data) {
            $scope.touches = data;
            $scope.loading = false;
            $('.all').show();
        });
    };

    $scope.getTouches();

    $scope.getTouchBy = function(touchBy){
        if (touchBy == "Action") return "内部动作";
        if (touchBy == "Phone") return "电话";
        if (touchBy == "QQ") return "QQ";
        if (touchBy == "AliWangWang") return "阿里旺旺";
        if (touchBy == "SMS") return "短信";
        if (touchBy == "Email") return "邮件";
        if (touchBy == "Meeting") return "见面";
        return touchBy;
    };

    $scope.toggleAddMode = function () {
        $scope.addMode = !$scope.addMode;
        $scope.touch = {touchBy : 'Action'};
    };

    $scope.toggleEditMode = function (touch) {
        touch.editMode = !touch.editMode;
    };

    $scope.addTouch = function () {
        touchService.save($scope.touch, function(e){
            toastr.success('操作成功');
            $scope.getTouches();
            $scope.toggleAddMode();
        }, notifyService.errorCallback);
    };

    $scope.updateTouch = function (touch) {
        touchService.update(touch, notifyService.successCallback, notifyService.errorCallback);
    };

    $scope.deleteTouch = function (index) {
        var touch = $scope.touches.splice(index, 1);
        touchService.delete({ id: touch[0].id }, notifyService.successCallback, notifyService.errorCallback);
    };

    $scope.removeItem = function(index) {
        var cart = $scope.carts.splice(index, 1);
        cartService.delete({ id: cart[0].cartID }, notifyService.successDeleteCart, notifyService.errorCallback);
    };

    $scope.removeSelected = function() {
        $('input:checkbox[name=itemIndex]').each(function() 
        {    
            if($(this).is(':checked'))
                $scope.carts.splice($(this).val(), 1);
        });
        notifyService.success('商品删除成功');
    };

    $scope.total = function() {
        var total = 0;
        angular.forEach($scope.carts, function(item) {
            total += item.purchaseNum * item.price;
        })
        return total;
    };

    $scope.updateCarts = function (orderUrl, loginUrl) {
        var cartIDWithPurchaseNumArray = [];
        $.each($scope.carts, function(index, cart) {
             cartIDWithPurchaseNumArray.push({"cartID": cart.cartID, "purchaseNum": cart.purchaseNum});
        });
        cartService.update(cartIDWithPurchaseNumArray, function(){
            if (dataService.user.userName){
                location.href = orderUrl;
            }else{
                location.href = loginUrl;
            }
        }, notifyService.errorCallback);
    };

});

