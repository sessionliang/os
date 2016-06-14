class BcFollowController extends baseController {
    public userService: UserService;
    public invoices;

    constructor() {
        super();
        this.userService = new UserService();
    }

    init(): void {
        baseController.userAuthValidate(() => {
            this.getUserFollow(1, 12);
        });
    }


    getUserFollow(pageIndex: number, prePageNum: number): void {
        this.userService.getUserFollows(pageIndex, prePageNum,(json) => {
            if (json.isSuccess) {
                $("#tbUserFollow").html("");
                var innerHtml = "";//<td>城市</td>
                for (var i = 0; i < json.followList.length; i++) {

                    innerHtml += '<li>';
                    innerHtml += '  <a href="' + json.followList[i].navigationUrl + '"><img src="' + json.followList[i].imageUrl + '" width="160" height="190"></a>';
                    innerHtml += '  <div class="bmrc_ss1">';
                    innerHtml += '    <input class="bmra_rad bmra_rad2" name="cb" type="checkbox" value="' + json.followList[i].id + '">';
                    innerHtml += '    <a class="cor_blue" href="' + json.followList[i].navigationUrl + '">' + json.followList[i].goodsName + '</a>';
                    innerHtml += '  </div>';
                    innerHtml += '  <div class="bmrc_ss2">' + json.followList[i].goodsPrice + '</div>';
                    innerHtml += '  <div class="bmrc_ss3"><span class="fl">' + json.followList[i].goodsCommentsCount + '人评价</span><span class="fr">好评度' + json.followList[i].goodsPraiseRate + '</span></div>';
                    innerHtml += '  <div class="bmrc_btnBox">';
                    innerHtml += '    <dl>';
                    if (json.followList[i].firstGoodID > 0) {
                        innerHtml += '      <dd><a class="bmrc_a1" href="javascript:;" onclick="new BcFollowController().addToCart(' + json.followList[i].goodsPublishmentSystemID + ',' + json.followList[i].goodsNodeID + ',' + json.followList[i].goodsID + ',' + json.followList[i].firstGoodID + ');return false;">加入购物车</a></dd>';
                    }
                    innerHtml += '      <dd><a class="bmrc_a1" href="javascript:;" onclick="new BcFollowController().removeUserFollow(\'' + json.followList[i].id + '\')">取消</a></dd>';
                    innerHtml += '    </dl>';
                    innerHtml += '  </div>';
                    innerHtml += '</li>';

                }
                $("#tbUserFollow").html(innerHtml);

                //分页链接
                $("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getUserFollow'));
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


    removeUserFollow(ids: string): void {
        this.userService.removeUserFollows(ids,(data) => {
            if (data.isSuccess) {
                Utils.tipAlert(true, "删除成功");
                this.getUserFollow(1, 12);
            } else {
                Utils.tipAlert(true, data.errorMessage);
            }
        });
    }

    followAddToCart(ids: string) {
        this.userService.followAddToCart(ids,(data) => {
            if (data.isSuccess) {
                Utils.tipAlert(true, "添加成功");
                baseController.getUserCart();
            } else {
                Utils.tipAlert(true, data.errorMessage);
            }
        });

    }

}
