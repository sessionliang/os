/// <reference path="WapBaseController.ts" />
class WapEditAddressController extends WapBaseController {

    public id: number = 0;
    private consignee: any = {
        "groupSN": "",
        "userName": "",
        "isOrder": false,
        "ipAddress": "",
        "isDefault": false,
        "consignee": "",
        "country": "",
        "province": "",
        "city": "",
        "area": "",
        "address": "",
        "zipcode": "",
        "mobile": "",
        "tel": "",
        "email": "",
        "id": 0
    };
    constructor() {
        super();
    }

    init(): void {
        WapBaseController.userAuthValidate(() => {
            this.id = <number>UrlUtils.getUrlVar("id");
            if (this.id > 0) {
                this.initConsignee(this.id);
            }

            $('#saveConsignee').click(() => {
                this.saveConsignee();
            });
        });
    }

    initConsignee(id: number): void {
        super.getUserService().getUserAddressOne(id, (json) => {
            if (json.isSuccess) {
                this.consignee = json.consignee;

                $('#consignee').val(json.consignee.consignee);
                $('#mobile').val(json.consignee.mobile);
                $('#address').val(json.consignee.address);
            }
        });
    }


    saveConsignee(): void {
        this.consignee.id = this.id;
        this.consignee.consignee = $('#consignee').val();
        this.consignee.mobile = $('#mobile').val();
        this.consignee.province = $('#province').val();
        this.consignee.city = $('#city').val();
        this.consignee.area = $('#area').val();
        this.consignee.address = $('#address').val();
        if (this.id > 0) {
            super.getUserService().updateUserAddress(this.consignee, (json) => {
                if (json.isSuccess) {

                    Utils.tipAlert(true, "地址保存成功！");
                }
            });
        } else {
            super.getUserService().addUserAddress(this.consignee, (json) => {
                if (json.isSuccess) {
                    Utils.tipAlert(true, "地址保存成功！",true,1500,"address.html");
                }
            });
        }
    }
}
