using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Text;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Core.Office;


using System.Collections.Generic;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundVoteContentAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;

        public TextBox tbSummary;
        public DropDownList ddlMaxSelectNum;
        public DateTimeTextBox dtbAddDate;
        public DropDownList ddlEndDate;
        public DateTimeTextBox dtbEndDate;
        public RadioButtonList rblIsVotedView;
        public TextBox tbHiddenContent;
        public AuxiliaryControl acAttributes;

        public Literal ltlScript;
        public Button Submit;

        private int nodeID;
        private int contentID;
        private string returnUrl;
        private bool isSummary;
        private ArrayList voteOptionInfoArrayList;
        private ArrayList relatedIdentities;
        private ETableStyle tableStyle;
        private string tableName;

        public bool IsSummary
        {
            get { return this.isSummary; }
        }

        public string GetOptionTitle(int itemIndex)
        {
            if (this.voteOptionInfoArrayList == null || this.voteOptionInfoArrayList.Count <= itemIndex) return string.Empty;
            VoteOptionInfo optionInfo = voteOptionInfoArrayList[itemIndex] as VoteOptionInfo;
            return optionInfo.Title;
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"));
            this.contentID = TranslateUtils.ToInt(base.GetQueryString("ID"));
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));

            this.tableStyle = ETableStyle.VoteContent;
            this.tableName = base.PublishmentSystemInfo.AuxiliaryTableForVote;
            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, this.nodeID);

            if (!IsPostBack)
            {
                string pageTitle = (contentID == 0) ? "添加投票" : "修改投票";
                string nodeNames = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, this.nodeID);
                base.BreadCrumbWithItemTitle(AppManager.CMS.LeftMenu.ID_Content, pageTitle, nodeNames, string.Empty);

                this.ltlPageTitle.Text = pageTitle;

                ArrayList excludeAttributeNames = TableManager.GetExcludeAttributeNames(this.tableStyle);
                this.acAttributes.AddExcludeAttributeNames(excludeAttributeNames);

                if (contentID == 0)
                {
                    ListItem listItem = new ListItem("无结束日期", string.Empty);
                    this.ddlEndDate.Items.Add(listItem);
                    listItem = new ListItem("一周", DateTime.Now.AddDays(7).ToShortDateString());
                    this.ddlEndDate.Items.Add(listItem);
                    listItem = new ListItem("一月", DateTime.Now.AddDays(30).ToShortDateString());
                    listItem.Selected = true;
                    this.ddlEndDate.Items.Add(listItem);
                    listItem = new ListItem("半年", DateTime.Now.AddDays(180).ToShortDateString());
                    this.ddlEndDate.Items.Add(listItem);
                    listItem = new ListItem("一年", DateTime.Now.AddDays(365).ToShortDateString());
                    this.ddlEndDate.Items.Add(listItem);
                    listItem = new ListItem("自定义", DateTime.Now.AddDays(30).ToShortDateString());
                    this.ddlEndDate.Items.Add(listItem);

                    this.dtbEndDate.DateTime = DateTime.Now.AddDays(30);

                    NameValueCollection formCollection = new NameValueCollection();

                    this.acAttributes.SetParameters(formCollection, base.PublishmentSystemInfo, this.nodeID, relatedIdentities, this.tableStyle, this.tableName, false, base.IsPostBack);
                }
                else
                {
                    VoteContentInfo contentInfo = DataProvider.VoteContentDAO.GetContentInfo(base.PublishmentSystemInfo, this.contentID);

                    this.voteOptionInfoArrayList = DataProvider.VoteOptionDAO.GetVoteOptionInfoArrayList(base.PublishmentSystemID, this.nodeID, this.contentID);
                    string script = string.Empty;
                    for (int i = 2; i < this.voteOptionInfoArrayList.Count; i++ )
                    {
                        VoteOptionInfo optionInfo = this.voteOptionInfoArrayList[i] as VoteOptionInfo;
                        script += string.Format("addItem('{0}');", optionInfo.Title);
                    }
                    if (!string.IsNullOrEmpty(script))
                    {
                        this.ltlScript.Text = string.Format(@"<script>{0}</script>", script);
                    }

                    this.isSummary = contentInfo.IsSummary;
                    this.tbSummary.Text = contentInfo.Summary;

                    for (int i = 2; i < this.voteOptionInfoArrayList.Count; i++)
                    {
                        ListItem listItem = new ListItem(string.Format("最多选{0}项", i), i.ToString());
                        this.ddlMaxSelectNum.Items.Add(listItem);
                    }

                    ControlUtils.SelectListItems(this.ddlMaxSelectNum, contentInfo.MaxSelectNum.ToString());
                    this.dtbAddDate.DateTime = contentInfo.AddDate;
                    this.ddlEndDate.Visible = false;
                    this.dtbEndDate.DateTime = contentInfo.EndDate;
                    ControlUtils.SelectListItemsIgnoreCase(this.rblIsVotedView, contentInfo.IsVotedView.ToString());
                    this.tbHiddenContent.Text = contentInfo.HiddenContent;

                    this.acAttributes.SetParameters(contentInfo.Attributes, base.PublishmentSystemInfo, this.nodeID, relatedIdentities, this.tableStyle, this.tableName, true, base.IsPostBack);
                }

                this.Submit.Attributes.Add("onclick", InputParserUtils.GetValidateSubmitOnClickScript("myForm"));
            }
            else
            {
                if (contentID == 0)
                {
                    this.acAttributes.SetParameters(base.Request.Form, base.PublishmentSystemInfo, this.nodeID, relatedIdentities, this.tableStyle, this.tableName, false, base.IsPostBack);
                }
                else
                {
                    this.acAttributes.SetParameters(base.Request.Form, base.PublishmentSystemInfo, this.nodeID, relatedIdentities, this.tableStyle, this.tableName, true, base.IsPostBack);
                }
            }
            base.DataBind();
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (this.contentID == 0)
                {
                    VoteContentInfo contentInfo = new VoteContentInfo();
                    try
                    {
                        InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);

                        contentInfo.NodeID = this.nodeID;
                        contentInfo.PublishmentSystemID = base.PublishmentSystemID;
                        contentInfo.AddUserName = AdminManager.Current.UserName;
                        if (contentInfo.AddDate.Year == DateUtils.SqlMinValue.Year)
                        {
                            base.FailMessage(string.Format("投票添加失败：系统时间不能为{0}年", DateUtils.SqlMinValue.Year));
                            return;
                        }
                        contentInfo.LastEditUserName = contentInfo.AddUserName;
                        contentInfo.LastEditDate = DateTime.Now;
                        contentInfo.IsChecked = false;

                        contentInfo.IsSummary = TranslateUtils.ToBool(base.Request.Form["IsSummary"]);
                        contentInfo.Summary = this.tbSummary.Text;
                        contentInfo.MaxSelectNum = TranslateUtils.ToInt(this.ddlMaxSelectNum.SelectedValue);
                        contentInfo.AddDate = this.dtbAddDate.DateTime;
                        if (string.IsNullOrEmpty(this.ddlEndDate.SelectedValue))
                        {
                            contentInfo.EndDate = this.dtbEndDate.DateTime;
                        }
                        else
                        {
                            contentInfo.EndDate = TranslateUtils.ToDateTime(this.ddlEndDate.SelectedValue);
                        }
                        contentInfo.IsVotedView = TranslateUtils.ToBool(this.rblIsVotedView.SelectedValue);
                        contentInfo.HiddenContent = this.tbHiddenContent.Text;

                        int checkedLevel = 0;
                        contentInfo.IsChecked = CheckManager.GetUserCheckLevel(base.PublishmentSystemInfo, this.nodeID, out checkedLevel);
                        contentInfo.CheckedLevel = checkedLevel;

                        int contentID = DataProvider.ContentDAO.Insert(tableName, base.PublishmentSystemInfo, contentInfo);

                        int itemCount = TranslateUtils.ToInt(base.Request.Form["itemCount"]);
                        ArrayList voteOptionInfoArrayList = new ArrayList();
                        for (int i = 0; i < itemCount; i++)
                        {
                            string title = base.Request.Form["options[" + i + "]"];
                            if (!string.IsNullOrEmpty(title))
                            {
                                VoteOptionInfo optionInfo = new VoteOptionInfo(0, base.PublishmentSystemID, this.nodeID, contentID, title, string.Empty, string.Empty, 0);
                                voteOptionInfoArrayList.Add(optionInfo);
                            }
                        }
                        DataProvider.VoteOptionDAO.Insert(voteOptionInfoArrayList);

                        contentInfo.ID = contentID;
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, string.Format("投票添加失败：{0}", ex.Message));
                        LogUtils.AddErrorLog(ex);
                        return;
                    }

                    if (contentInfo.IsChecked)
                    {
                        string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Add, ETemplateType.ContentTemplate, this.nodeID, contentInfo.ID, 0);
                        AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, contentInfo.ID, "添加投票", string.Format("栏目:{0},投票标题:{1}", NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID), contentInfo.Title));

                    PageUtils.Redirect(BackgroundContentAddAfter.GetRedirectUrl(base.PublishmentSystemID, this.nodeID, contentInfo.ID, this.returnUrl));
                }
                else
                {
                    VoteContentInfo contentInfo = DataProvider.VoteContentDAO.GetContentInfo(base.PublishmentSystemInfo, contentID);
                    try
                    {                        
                        InputTypeParser.AddValuesToAttributes(this.tableStyle, this.tableName, base.PublishmentSystemInfo, this.relatedIdentities, base.Request.Form, contentInfo.Attributes, ContentAttribute.HiddenAttributes);

                        contentInfo.LastEditUserName = AdminManager.Current.UserName;
                        contentInfo.LastEditDate = DateTime.Now;

                        contentInfo.IsSummary = TranslateUtils.ToBool(base.Request.Form["IsSummary"]);
                        contentInfo.Summary = this.tbSummary.Text;
                        contentInfo.MaxSelectNum = TranslateUtils.ToInt(this.ddlMaxSelectNum.SelectedValue);
                        contentInfo.AddDate = this.dtbAddDate.DateTime;
                        contentInfo.EndDate = this.dtbEndDate.DateTime;
                        contentInfo.IsVotedView = TranslateUtils.ToBool(this.rblIsVotedView.SelectedValue);
                        contentInfo.HiddenContent = this.tbHiddenContent.Text;

                        DataProvider.ContentDAO.Update(tableName, base.PublishmentSystemInfo, contentInfo);

                        int itemCount = TranslateUtils.ToInt(base.Request.Form["itemCount"]);
                        ArrayList voteOptionInfoArrayList = new ArrayList();
                        for (int i = 0; i < itemCount; i++)
                        {
                            string title = base.Request.Form["options[" + i + "]"];
                            if (!string.IsNullOrEmpty(title))
                            {
                                VoteOptionInfo optionInfo = new VoteOptionInfo(0, base.PublishmentSystemID, this.nodeID, contentID, title, string.Empty, string.Empty, 0);
                                voteOptionInfoArrayList.Add(optionInfo);
                            }
                        }
                        DataProvider.VoteOptionDAO.UpdateVoteOptionInfoArrayList(base.PublishmentSystemID, this.nodeID, this.contentID, voteOptionInfoArrayList);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, string.Format("投票修改失败：{0}", ex.Message));
                        LogUtils.AddErrorLog(ex);
                        return;
                    }

                    if (contentInfo.IsChecked)
                    {
                        string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Edit, ETemplateType.ContentTemplate, this.nodeID, contentID, 0);
                        AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, this.nodeID, this.contentID, "修改投票", string.Format("栏目:{0},投票标题:{1}", NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID), contentInfo.Title));

                    PageUtils.Redirect(this.returnUrl);
                }
            }
        }

        public string ReturnUrl { get { return this.returnUrl; } }

    }
}
