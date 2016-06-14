app.factory('notifyService', function () {
    return {
        successCallback: function (e) {
            toastr.success('操作成功');
        },
        errorCallback: function (e) {
            var error = e.data.exceptionMessage;
            if (!error){
                error = e.data.message;
            }
            toastr.error('操作失败：' + error);
        },
        success: function (text) {
            toastr.success(text);
        },
        error: function (text) {
            toastr.error(text);
        }
    };
});