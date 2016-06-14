/// <reference path="WapBaseController.ts" />
class WapHistoryController extends WapBaseController {
    constructor() {
        super();
    }

    init(): void {
        WapBaseController.userAuthValidate(() => {
            this.getUserHistory(1, 12);
        });
    }


    getUserHistory(pageIndex: number, prePageNum: number): void {
        super.getUserService().getUserHistory(pageIndex, prePageNum,(json) => {
            if (json.isSuccess) {

                $("#tbUserHistory").html("");
                var innerHtml = "";//<td>城市</td>
                for (var i = 0; i < json.historyList.length; i++) {
                    

                    innerHtml += '<li>';
                    innerHtml += '    <div class="mdf_top">';
                    innerHtml += '        <img src="' + json.historyList[i].imageUrl + '">';
                    innerHtml += '        <div class="mdf_tnm">' + json.historyList[i].goodsName + '</div>';
                    innerHtml += '        <div class="mdf_tprice"><span class="fl">¥' + json.historyList[i].goodsPrice + '</span>';
                    innerHtml += '          <a href="javascript:;" onclick="new WapHistoryController().removeUserHistory(\'' + json.historyList[i].id + '\')" class="mdf_del">删除</a></div>';
                    innerHtml += '    </div>';
                    innerHtml += '</li>';

                    
                }
                $("#tbUserHistory").html(innerHtml);

                //分页链接
                //$("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getUserFollow'));
            }
        });
    }

    removeUserHistory(ids: string): void {
        super.getUserService().removeUserHistory(ids, (data) => {
            if (data.isSuccess) {
                Utils.tipAlert(true, "删除成功");
                this.getUserHistory(1, 12);
            } else {
                Utils.tipAlert(true, data.errorMessage);
            }
        });
    }
}
