/**********
* 控制类基类
**********/
class baseController {

    //用户仓储
    static userService: UserService = new UserService();
    getUserService(): UserService {
        return baseController.userService;
    }

    //用户
    static user: any;
    getUser(): any {
        return baseController.user;
    }

    constructor() {
        baseController.userService = new UserService();
        baseController.getUserCart();
    }

    //验证用户是否登录
    static userAuthValidate(fn: Function): void {
        baseController.userService.getUser((json) => {
            if (json.isAnonymous) {
                HomeUrlUtils.redirectToLogin();
            }
            else {
                baseController.user = json.user;
                if (fn)
                    fn();
            }
        });
    }

    //购物车
    static carts: any;

    //站点信息
    static publishmentSystemInfo: any;

    static getUserCart(): void {
        baseController.userService.getUserCart((json) => {
            if (json.isSuccess) {
                $("#cart").html("");
                var innerHtml = "";//<td>城市</td>
                var tatalCount = 0;
                var totalMoney = 0;

                baseController.carts = json.carts;
                baseController.publishmentSystemInfo = json.publishmentSystemInfo;
                var result = baseController.renderCart();

                $("#cart").html(result.innerHtml);

                $('#totalCount').html(result.tatalCount + '');
                $('#totalMoney').html(result.totalMoney + '');
                $("#indexPage").attr("href", json.publishmentSystemInfo.publishmentSystemUrl);
                $(".homecart").attr("href", json.publishmentSystemInfo.publishmentSystemUrl + "/cart.html");
            }
        });
    }

    static renderCart(): any {
        var result: any = {
            tatalCount: 0,
            totalMoney: 0,
            innerHtml: StringUtils.empty
        };
        if (baseController.carts.length == 0) {
            result.innerHtml += '<div style="display:table;height: 300px;width: 100%;">';
            result.innerHtml += '    <div style="display:table-cell;vertical-align:middle;text-align: center;">';
            result.innerHtml += '        <p class="f24" style="margin-bottom: 30px">你的购物车竟然还是空的？赶紧选购吧!</p>';
            result.innerHtml += '        <button class="btn btn-primary" onclick="location=\'' + baseController.publishmentSystemInfo.publishmentSystemUrl + '\';">马上去购物</button>';
            result.innerHtml += '    </div>';
            result.innerHtml += '</div>';
            $('.msc_lay2box').html(result.innerHtml);
        } else {
            for (var i = 0; i < baseController.carts.length; i++) {

                result.innerHtml += '<li>';
                result.innerHtml += '    <a class="fl" href="' + baseController.carts[i].navigationUrl + '"><img src="' + baseController.carts[i].imageUrl + '" width="50" height="50" /></a>';
                result.innerHtml += '    <div class="msc_lay2txt">';
                result.innerHtml += '        ' + baseController.carts[i].title;
                result.innerHtml += '    </div>';
                result.innerHtml += '    <div class="msc_layprice">';
                result.innerHtml += '        <strong class="msc_red">¥' + baseController.carts[i].price + '</strong><span class="msc_999"> x' + baseController.carts[i].purchaseNum + '</span><br />';
                result.innerHtml += '        <a class="msc_blue" href="javascript:;" onclick="baseController.deleteCart(' + i + ')">删除</a>';
                result.innerHtml += '    </div>';
                result.innerHtml += '</li>';
                result.tatalCount += baseController.carts[i].purchaseNum;
                result.totalMoney += baseController.carts[i].purchaseNum * baseController.carts[i].price;
            }
        }
        $("#allCount").html(result.tatalCount);
        return result;
    }

    static deleteCart(index): void {
        var carts = baseController.carts.splice(index, 1);
        baseController.userService.deleteCart(carts[0].cartID, baseController.publishmentSystemInfo.publishmentSystemID, function (data) {
            Utils.tipAlert(true, '商品删除成功');
            var result = baseController.renderCart();
            $("#cart").html(result.innerHtml);
            $("#allCount").html(result.tatalCount);
            $('#totalCount').html(result.tatalCount + '');
            $('#totalMoney').html(result.totalMoney + '');
        });
    }

    static getFirstB2CPageInfo(pID?: any, fn?: Function): void {
        var orderService = new OrderService();
        orderService.getFirstB2CPageInfo((json) => {
            if (json.isSuccess) {
                var pageInfo = 'var $pageInfo = { publishmentSystemID : {0}, channelID : {1}, contentID : {2}, siteUrl : "{3}", homeUrl : "{4}", currentUrl : "{5}", rootUrl : "{6}", apiUrl : "{7}" };';
                pageInfo = StringUtils.formatIngnoreFalse(pageInfo,
                    json.pageInfo.publishmentSystemID,
                    json.pageInfo.channelID,
                    json.pageInfo.contentID,
                    json.pageInfo.siteUrl,
                    json.pageInfo.homeUrl,
                    json.pageInfo.currentUrl,
                    json.pageInfo.rootUrl,
                    json.pageInfo.apiUrl);
                var pageScript = document.createElement("script");
                pageScript.innerHTML = pageInfo;
                document.body.appendChild(pageScript);
                if (fn)
                    fn();
            }
        }, pID);
    }
}