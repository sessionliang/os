/// <reference path="WapBaseController.ts" />
class WapUserMessageController extends WapBaseController {

    public static pageIndex: number = 1;
    public static prePageNum: number = 10;

    constructor() {
        super();
    }

    init(): void {
        WapBaseController.userAuthValidate(() => {
            this.getUserMessage(1, 5);
        });
    }

    getUserMessage(pageIndex: number, prePageNum: number): void {
        super.getUserService().getUserAllMessage('SystemAnnouncement', pageIndex, prePageNum,(json) => {
            if (json.isSuccess) {
                json.pageJson = eval("(" + json.pageJson + ")");
                if (json.pageJson.last < WapUserMessageController.pageIndex) {
                    WapUserMessageController.pageIndex--;
                    Utils.tipAlert(false, "到底了！");
                    return;
                }

                var innerHtml = "";
                for (var i = 0; i < json.userMessageList.length; i++) {
                    innerHtml += '<div class="mxx_bx" id="msg' + json.userMessageList[i].id + '" ' + (json.userMessageList[i].isViewed ? '' : 'style="font-weight: bold;"') + ' onclick="new WapUserMessageController().updateMsgStatus(' + json.userMessageList[i].id + ');">';
                    innerHtml += '    <div class="mxx_top"><span class="fl">' + (json.userMessageList[i].MessageType == 'SystemAnnouncement' ? '[公告]' : '') + json.userMessageList[i].title + '</span><span class="fr">' + Utils.formatTime(json.userMessageList[i].addDate.replace('T', '  '), "yyyy-MM-dd HH:mm:ss") + '</span></div>';
                    innerHtml += '    <div class="mxx_txt">';
                    innerHtml += json.userMessageList[i].content
                    innerHtml += '    </div>';
                    innerHtml += '</div>';

                }
                $("#msgtop").after(innerHtml);
            }
            else {
                Utils.tipAlert(false, "获取消息出错-" + json.errorMessage);
            }
            //分页链接

        });
    }

    //瀑布流加载
    waterfallList(): void {
        WapUserMessageController.pageIndex++;
        this.getUserMessage(WapUserMessageController.pageIndex, WapUserMessageController.prePageNum);
    }


    updateMsgStatus(id: any): void {
        super.getUserService().getUserMessageDetail(id,(data) => {
            if (data.isSuccess) {
                $('#msg' + id).css('font-weight', '100');
            }
        });

    }
}
