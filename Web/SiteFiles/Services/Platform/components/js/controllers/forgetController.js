var forgetController = {};

forgetController.login = {};
forgetController.successMessage;
forgetController.isLoading = false;

forgetController.submit = function(){

    forgetController.login.loginName = $('#loginName').val();

    var isValid = true;
    if (!forgetController.login.loginName){
        notifyService.error('请输入登录账号');
        $('#loginName').focus();
        isValid = false;
    }
    if(!isValid) return false;

    if (forgetController.isLoading) return false;
    forgetController.isLoading = true;

    forgetController.render();

    userService.forgetPassword(forgetController.login, function (data) {
      forgetController.isLoading = false;
      if (data.isSuccess){
          forgetController.successMessage = data.successMessage;
          forgetController.isLoading = false;
          notifyService.success(data.successMessage);
      }else{
        forgetController.render();
        notifyService.error(data.errorMessage);
        $('#loginName').focus();
      }
      forgetController.render();
    });
};

forgetController.render = function(){
    utilService.render('forgetController', forgetController);
};

forgetController.render();