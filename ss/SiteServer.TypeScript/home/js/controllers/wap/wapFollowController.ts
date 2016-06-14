/// <reference path="WapBaseController.ts" />
class WapFollowController extends WapBaseController {
    constructor() {
        super();
    }

    init(): void {
        WapBaseController.userAuthValidate(() => {
            this.getUserFollow(1, 12);
        });
    }


    getUserFollow(pageIndex: number, prePageNum: number): void {
        super.getUserService().getUserFollows(pageIndex, prePageNum,(json) => {
            if (json.isSuccess) {
                $("#tbUserFollow").html("");
                var innerHtml = "";//<td>城市</td>
                for (var i = 0; i < json.followList.length; i++) {
                    

                    innerHtml += '<li>';
                    innerHtml += '    <div class="mdf_top">';
                    innerHtml += '        <img src="' + json.followList[i].imageUrl + '">';
                    innerHtml += '        <div class="mdf_tnm">' + json.followList[i].goodsName + '</div>';
                    innerHtml += '        <div class="mdf_tprice"><span class="fl">¥' + json.followList[i].goodsPrice + '</span>';
                    innerHtml += '          <a href="javascript:;" onclick="new WapFollowController().removeUserFollow(\'' + json.followList[i].id + '\')" class="mdf_del">取消关注</a></div>';
                    innerHtml += '    </div>';
                    innerHtml += '</li>';

                    
                }
                $("#tbUserFollow").html(innerHtml);

                //分页链接
                //$("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getUserFollow'));
            }
        });
    }

    removeUserFollow(ids: string): void {
        super.getUserService().removeUserFollows(ids, (data) => {
            if (data.isSuccess) {
                Utils.tipAlert(true, "删除成功");
                this.getUserFollow(1, 12);
            } else {
                Utils.tipAlert(true, data.errorMessage);
            }
        });
    }
}
