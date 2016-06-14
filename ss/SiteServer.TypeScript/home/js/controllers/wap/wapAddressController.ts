/// <reference path="WapBaseController.ts" />
class WapAddressController extends WapBaseController {
    constructor() {
        super();
    }

    init(): void {
        WapBaseController.userAuthValidate(() => {
            this.getUserAddress();
        });
    }


    getUserAddress(): void {
        super.getUserService().getUserAddress((json) => {
            if (json.isSuccess) {
                $("#ulAddressList").html('');
                var innerHtml = "";//<td>城市</td>
                for (var i = 0; i < json.consignees.length; i++) {
                    innerHtml += '<li>';
                    innerHtml += '    <div class="mad_bx1"><span class="fl f16">' + json.consignees[i].consignee + '</span><span class="cor_red fl">' + json.consignees[i].mobile + '</span>';
                    if (json.consignees[i].isDefault) {
                        innerHtml += '<span class="fr cor_red"><i class="fa fa-map-marker cor_888"></i> 默认地址</span>';
                    }
                    innerHtml += '</div>    <div class="mad_adrxt cor_888">' + json.consignees[i].province + ' ' + json.consignees[i].city + ' ' + json.consignees[i].area + ' ' + json.consignees[i].address + '</div>';
                    innerHtml += '    <div class="mad_fun"><a href="editAddress.html?id=' + json.consignees[i].id + '">编辑</a> | <a href="javascript:;" onclick="new WapAddressController().removeUserAdress(\'' + json.consignees[i].id + '\');">删除</a></div>';
                    innerHtml += '</li>';
                }
                $("#ulAddressList").html(innerHtml);

                //分页链接
                //$("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getUserFollow'));
            }
        });
    }

    removeUserAdress(id: number): void {
        super.getUserService().deleteUserAddress(id,(data) => {
            if (data.isSuccess) {
                Utils.tipAlert(true, "删除成功");
                this.getUserAddress();
            } else {
                Utils.tipAlert(true, data.errorMessage);
            }
        });
    }
}
