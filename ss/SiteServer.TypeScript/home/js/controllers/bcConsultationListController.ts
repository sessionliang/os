/**********
* 购买咨询
**********/
class BcConsultationListController extends baseController {

    public static consultationService: ConsultationService;
    public static consultationList: any;
    public pageIndex: number = 1;
    public prePageNum: number = 10;

    public keywords: string = StringUtils.empty;

    public loading: boolean = true;

    public parmMap: any = Utils.urlToMap(window.location.href.split('?').length > 1 ? window.location.href.split('?')[1] : "");

    constructor() {
        super();
        BcConsultationListController.consultationService = new ConsultationService();
    }

    //初始化页面元素以及事件
    init(): void {
        baseController.userAuthValidate(() => {
            this.bindEvent();
            this.bindData();
        });
    }

    //绑定事件
    bindEvent(): void {

        if ($("#btnSearch").length > 0) {
            $("#btnSearch").click(() => {
                this.btnSearchClick();
            });
        }
    }

    //绑定数据
    bindData(): void {
        this.bindConsultationList(this.pageIndex, this.prePageNum);
        this.bindConsultationKeywords();
    }

    //绑定购买咨询数据
    bindConsultationList(pageIndex: number, prePageNum: number): void {
        this.pageIndex = pageIndex || this.pageIndex;
        this.prePageNum = prePageNum || this.prePageNum;
        this.keywords = <string>UrlUtils.getUrlVar("keywords");

        BcConsultationListController.consultationService.getAllConsultationList(this.keywords, this.pageIndex, this.prePageNum,(json) => {
            BcConsultationListController.consultationList = json.consultationList;
            this.loading = false;
            //加载数据
            this.render();
            //分页链接
            $("#divPageLink").html(pageDataUtils.getPageHtml(json.pageJson, 'getAllConsultationList'));
        });
    }

    //购买咨询关键字帅选
    btnSearchClick(): void {
        window.location.href = this.getUrl($("#txtKeywords").val());
    }

    //绑定购买咨询关键字帅选
    bindConsultationKeywords(): void {
        if ($("#txtKeywords").length > 0) {
            $("#txtKeywords").val(UrlUtils.getUrlVar("keywords"));
        }
    }

    //删除购买咨询
    removeConsultation(consultationID: number): void {
        var item = {};
        for (var i = 0; i < BcConsultationListController.consultationList.length; i++) {
            if (BcConsultationListController.consultationList[i].consultationInfo.id == consultationID) {
                item = BcConsultationListController.consultationList[i];
                break;
            }
        }
        BcConsultationListController.consultationList.splice($.inArray(item, BcConsultationListController.consultationList), 1);
        BcConsultationListController.consultationService.deleteConsultation(consultationID);
        this.render();
    }

    //渲染数据
    render(): void {
        var html: string = StringUtils.empty;
        var htmlTemplate: string = StringUtils.empty;

        //htmlTemplate = $("#consultationTemplate").html();

        htmlTemplate += '<tr data="true">';
        htmlTemplate += '<td class="bmrc_std1"><a href="{0}"><img src="{1}" width="50" height="50"></a></td>';
        htmlTemplate += '<td><a href="{0}" class="cor_blue">{2}</a></td>';
        htmlTemplate += '<td valign="top" class="bmrc_std2">';
        htmlTemplate += '<div class="bmrc_sd1"><span class="bmrc_s1">我的咨询：{3}</span><span class="fr cor_999">{4}</span></div>';
        htmlTemplate += '<div class="bmrc_sd2" style="display:{8}">';
        htmlTemplate += '<p><span class="cor_org">京东回复：</span>{5}</p>';
        htmlTemplate += '</div>';
        htmlTemplate += '<div class="bmrc_time" style="display:{8}"><span class="cor_999">{6}</span></div>';
        htmlTemplate += '<div style="display:{7}">暂无回复</div>';
        htmlTemplate += '</td>';
        htmlTemplate += '<tr>';

        for (var i = 0; i < BcConsultationListController.consultationList.length; i++) {
            var addDate = Utils.formatTime(BcConsultationListController.consultationList[i].addDate.replace("T", " "), "yyyy-MM-dd HH:mm:ss");
            var replyDate = Utils.formatTime(BcConsultationListController.consultationList[i].replyDate.replace("T", " "), "yyyy-MM-dd HH:mm:ss");
            if (BcConsultationListController.consultationList[i].answer.length == 0) {
                html += StringUtils.format(htmlTemplate,
                    BcConsultationListController.consultationList[i].navigationUrl,
                    BcConsultationListController.consultationList[i].thumbUrl,
                    BcConsultationListController.consultationList[i].title,
                    BcConsultationListController.consultationList[i].question,
                    addDate,
                    BcConsultationListController.consultationList[i].answer,
                    replyDate,
                    "block",
                    "none");
            } else {
                html += StringUtils.format(htmlTemplate,
                    BcConsultationListController.consultationList[i].navigationUrl,
                    BcConsultationListController.consultationList[i].thumbUrl,
                    BcConsultationListController.consultationList[i].title,
                    BcConsultationListController.consultationList[i].question,
                    addDate,
                    BcConsultationListController.consultationList[i].answer,
                    replyDate,
                    "none",
                    "block");
            }
        }
        //添加之前删除原有的tr
        $("tr[data='true']").remove();
        //添加
        $("#consultationTab").append(html);
    }

    getUrl(keywords?: string): string {

        this.parmMap['keywords'] = keywords;

        return "?" + Utils.mapToUrl(this.parmMap);
    }
}