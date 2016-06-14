class BcHistoryController extends baseController {
    public userService: UserService;
    public invoices;

    constructor() {
        super();
        this.userService = new UserService();
    }

    init(): void {
        baseController.userAuthValidate(() => {
            this.getUserHistory(1, 12);
        });
    }


    getUserHistory(pageIndex: number, prePageNum: number): void {
        this.userService.getUserHistory(pageIndex, prePageNum,(json) => {
            if (json.isSuccess) {
                $("#tbUserHistory").html("");
                var innerHtml = "";
                for (var i = 0; i < json.historyList.length; i++) {

                    innerHtml += '<li>';
                    innerHtml += '  <a href="' + json.historyList[i].navigationUrl + '"><img src="' + json.historyList[i].imageUrl + '" width="160" height="160"></a>';
                    innerHtml += '  <div class="bmrc_ss1">';
                    innerHtml += '    <input class="bmra_rad bmra_rad2" name="cb" type="checkbox" value="' + json.historyList[i].id + '">';
                    innerHtml += '    <a class="cor_blue" href="' + json.historyList[i].navigationUrl + '">' + json.historyList[i].goodsName + '</a>';
                    innerHtml += '  </div>';
                    innerHtml += '  <div class="bmrc_ss2">' + json.historyList[i].goodsPrice + '</div>';
                    innerHtml += '  <div class="bmrc_ss3"><span class="fl">' + json.historyList[i].goodsCommentsCount + '人评价</span><span class="fr">好评度' + json.historyList[i].goodsPraiseRate + '</span></div>';
                    innerHtml += '  <div class="bmrc_btnBox">';
                    innerHtml += '    <dl>';
                    if (json.historyList[i].firstGoodID > 0) {
                        innerHtml += '      <dd><a class="bmrc_a1" href="javascript:;" onclick="new BcHistoryController().addToCart(' + json.historyList[i].goodsPublishmentSystemID + ',' + json.historyList[i].goodsNodeID + ',' + json.historyList[i].goodsID + ',' + json.historyList[i].firstGoodID + ');return false;">加入购物车</a></dd>';
                    }
                    innerHtml += '      <dd><a class="bmrc_a1" href="javascript:;" onclick="new BcHistoryController().removeUserHistory(\'' + json.historyList[i].id + '\')">取消</a></dd>';
                    innerHtml += '    </dl>';
                    innerHtml += '  </div>';
                    innerHtml += '</li>';

                }
                $("#tbUserHistory").html(innerHtml);

                //分页链接
                $("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getUserHistory'));
            }
        });
    }

    addToCart(publishmentSystemID, channelID, contentID, goodsID) {
        //内容页面
        var purchaseNum = 1;

        var cart = {
            publishmentSystemID: publishmentSystemID,
            channelID: channelID,
            contentID: contentID,
            goodsID: goodsID,
            purchaseNum: "1"
        };
        this.userService.addToCart(cart, function (data) {
            if (data.isSuccess) {
                Utils.tipAlert(true, "添加成功");
                baseController.getUserCart();
            } else {
                Utils.tipAlert(true, "添加失败");
            }
        });
    }


    removeUserHistory(ids: string): void {
        this.userService.removeUserHistory(ids,(data) => {
            if (data.isSuccess) {
                Utils.tipAlert(true, "删除成功");
                this.getUserHistory(1, 12);
            } else {
                Utils.tipAlert(true, data.errorMessage);
            }
        });
    }

    historyAddToCart(ids: string) {
        this.userService.historyAddToCart(ids,(data) => {
            if (data.isSuccess) {
                Utils.tipAlert(true, "添加成功");
                baseController.getUserCart();
            } else {
                Utils.tipAlert(true, data.errorMessage);
            }
        });

    }

}
