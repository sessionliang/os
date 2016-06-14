class BcLoginRecordController extends baseController {
    public userService: UserService;

    constructor() {
        super();
        this.userService = new UserService();
    }

    init(): void {
        baseController.userAuthValidate(() => {
            this.getUserLoginLog(1, 10);
        });
    }

    getUserLoginLog(pageIndex: number, prePageNum: number): void {
        this.userService.getUserLoginLog(pageIndex, prePageNum,(json) => {
            if (json.isSuccess) {
                $("#tbUserLog").html("");
                var innerHtml = "<tr class='bmra_trh1'><td>IP地址</td><td>日期</td><td>备注</td></tr>";//<td>城市</td>

                for (var i = 0; i < json.userLoginInfoList.length; i++) {
                    innerHtml += "<tr>";
                    innerHtml += "<td>" + json.userLoginInfoList[i].ipAddress + "</td>";
                    innerHtml += "<td>" + Utils.formatTime(json.userLoginInfoList[i].addDate.replace("T", "  "), "yyyy-MM-dd HH:mm:ss") + "</td>";
                    innerHtml += "<td>" + json.userLoginInfoList[i].summary + "</td>";
                    innerHtml += "</tr>";
                }
                $("#tbUserLog").html(innerHtml);

                //分页链接
                $("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getUserLoginLogData'));
            }
        });
    }

}


