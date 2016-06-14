/**********
* 商城用户中心首页
**********/
class WapIndexController extends WapBaseController {


    public static orderService: OrderService;


    constructor() {
        super();
        WapIndexController.orderService = new OrderService();
    }

    init(): void {
        WapBaseController.userAuthValidate(() => {
            this.getUserGuesses();
            this.getOrderStatistic();
            $('#hUserName').html(super.getUser().userName);
        });
    }


    getUserGuesses(): void {
        super.getUserService().getUserGuesses((json) => {
            if (json.isSuccess) {
                $("#userguesses").html("");
                var innerHtml = "";//<td>城市</td>

                for (var i = 0; i < json.guesses.length && i < 3; i++) {

                    innerHtml += '<li><a href="' + json.guesses[i].navigationUrl + '" ><img src="' + json.guesses[i].imageurl + '">' + json.guesses[i].title + '</a></li>';
                }

                $("#userguesses").html(innerHtml);
            }
        });
    }
    getOrderStatistic(): void {
        WapIndexController.orderService.getOrderStatistic((json) => {
            $("#noPay").html(json.noPay + "<br>待付款");
            $("#noCompleted").html(json.noCompleted + "<br>待确认收货");
            //super.getUserService()
            super.getUserService().getUserAllMessage("", 1, 1,(data) => {
                if (data.isSuccess) {
                    if (data.pageJson.total) {
                        $("#msgCount").html(data.pageJson.total + "<br>我的消息");
                    }
                }
            });
        });
    }
}