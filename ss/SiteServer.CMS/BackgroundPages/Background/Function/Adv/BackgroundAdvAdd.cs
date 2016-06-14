using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Controls;




namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundAdvAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public TextBox AdvName;
        public RadioButtonList IsEnabled;
        public CheckBox IsDateLimited;
        public HtmlTableRow StartDateRow;
        public HtmlTableRow EndDateRow;
        public DateTimeTextBox StartDate;
        public DateTimeTextBox EndDate;
        public ListBox NodeIDCollectionToChannel;
        public ListBox NodeIDCollectionToContent;
        public HtmlTableRow FileTemplateIDCollectionRow;
        public CheckBoxList FileTemplateIDCollection;
        public DropDownList LevelType;
        public DropDownList Level;
        public CheckBox IsWeight;
        public DropDownList Weight ;

        public DropDownList RotateType;
        public HtmlTableRow RotateIntervalRow;
        public TextBox RotateInterval;
        public TextBox Summary;

        private int advID = 0;
        private int adAreadID = 0;
        private bool isEdit = false;
        private bool[] isLastNodeArray;

        public void Page_Load(object sender, EventArgs E)
        {
            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.adAreadID = TranslateUtils.ToInt(base.GetQueryString("AdAreaID"));
            if (base.GetQueryString("AdvID") != null)
            {
                this.isEdit = true;
                this.advID = TranslateUtils.ToInt(base.GetQueryString("AdvID"));
            }
             
            if (!Page.IsPostBack)
            {
                string pageTitle = this.isEdit ? "编辑广告" : "添加广告";
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Advertisement, pageTitle, AppManager.CMS.Permission.WebSite.Advertisement);
                 
                this.ltlPageTitle.Text = pageTitle;

                this.StartDate.Text = DateUtils.GetDateAndTimeString(DateTime.Now);
                this.EndDate.Text = DateUtils.GetDateAndTimeString(DateTime.Now.AddMonths(1));
                 
                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
                int nodeCount = nodeIDArrayList.Count;
                this.isLastNodeArray = new bool[nodeCount];
                foreach (int theNodeID in nodeIDArrayList)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, theNodeID);

                    string title = WebUtils.GetChannelListBoxTitle(base.PublishmentSystemID, nodeInfo.NodeID, nodeInfo.NodeName, nodeInfo.NodeType, nodeInfo.ParentsCount, nodeInfo.IsLastNode, this.isLastNodeArray);
                    ListItem listitem = new ListItem(title, nodeInfo.NodeID.ToString());
                    this.NodeIDCollectionToChannel.Items.Add(listitem);
                    title = title + string.Format("({0})", nodeInfo.ContentNum);
                    ListItem listitem2 = new ListItem(title, nodeInfo.NodeID.ToString());
                    this.NodeIDCollectionToContent.Items.Add(listitem2);
                }

                ArrayList fileTemplateInfoArrayList = DataProvider.TemplateDAO.GetTemplateInfoArrayListByType(base.PublishmentSystemID, ETemplateType.FileTemplate);
                if (fileTemplateInfoArrayList.Count > 0)
                {
                    foreach (TemplateInfo fileTemplateInfo in fileTemplateInfoArrayList)
                    {
                        ListItem listitem = new ListItem(fileTemplateInfo.CreatedFileFullName, fileTemplateInfo.TemplateID.ToString());
                        this.FileTemplateIDCollection.Items.Add(listitem);
                    }
                }
                else
                {
                    this.FileTemplateIDCollectionRow.Visible = false;
                }

                EBooleanUtils.AddListItems(this.IsEnabled);
                ControlUtils.SelectListItems(this.IsEnabled, true.ToString());

                EAdvLevelTypeUtils.AddListItems(this.LevelType);
                ControlUtils.SelectListItems(this.LevelType, EAdvLevelTypeUtils.GetValue(EAdvLevelType.Hold));

                EAdvLevelUtils.AddListItems(this.Level);
                ControlUtils.SelectListItems(this.Level, EAdvLevelUtils.GetValue(EAdvLevel.Level1));

                EAdvWeightUtils.AddListItems(this.Weight );
                ControlUtils.SelectListItems(this.Weight , EAdvWeightUtils.GetValue(EAdvWeight .Level1));

                EAdvRotateTypeUtils.AddListItems(this.RotateType);
                ControlUtils.SelectListItems(this.RotateType, EAdvRotateTypeUtils.GetValue(EAdvRotateType.HandWeight));

                if (this.isEdit)
                {
                    AdvInfo advInfo = DataProvider.AdvDAO.GetAdvInfo(this.advID, base.PublishmentSystemID);
                    this.AdvName.Text = advInfo.AdvName;
                    this.IsEnabled.SelectedValue = advInfo.IsEnabled.ToString();
                    this.IsDateLimited.Checked = advInfo.IsDateLimited;
                    this.StartDate.Text = DateUtils.GetDateAndTimeString(advInfo.StartDate);
                    this.EndDate.Text = DateUtils.GetDateAndTimeString(advInfo.EndDate);
                    ControlUtils.SelectListItems(this.NodeIDCollectionToChannel, TranslateUtils.StringCollectionToArrayList(advInfo.NodeIDCollectionToChannel));
                    ControlUtils.SelectListItems(this.NodeIDCollectionToContent, TranslateUtils.StringCollectionToArrayList(advInfo.NodeIDCollectionToContent));
                    ControlUtils.SelectListItems(this.FileTemplateIDCollection, TranslateUtils.StringCollectionToArrayList(advInfo.FileTemplateIDCollection));
                    this.LevelType.SelectedValue = EAdvLevelTypeUtils.GetValue(advInfo.LevelType);
                    this.Level.SelectedValue = advInfo.Level.ToString();
                    this.IsWeight.Checked = advInfo.IsWeight;
                    this.Weight .SelectedValue = advInfo.Weight .ToString();
                    this.RotateType.SelectedValue = EAdvRotateTypeUtils.GetValue(advInfo.RotateType);
                    this.RotateInterval.Text = advInfo.RotateInterval.ToString();
                    this.Summary.Text = advInfo.Summary;
                }

                this.ReFresh(null, EventArgs.Empty);
            }

            base.SuccessMessage(string.Empty);
        }

        public void ReFresh(object sender, EventArgs E)
        {
            if (this.IsDateLimited.Checked)
            {
                this.StartDateRow.Visible = true;
                this.EndDateRow.Visible = true;
            }
            else
            {
                this.StartDateRow.Visible = false;
                this.EndDateRow.Visible = false;
            }

            this.Level.Visible = this.Weight .Visible = false;
            this.IsDateLimited.Enabled = true;

            EAdvLevelType levelType = EAdvLevelTypeUtils.GetEnumType(this.LevelType.SelectedValue);
            if (levelType == EAdvLevelType.Standard)
            {
                this.Level.Visible = true;
            }
            else
            {
                this.Level.Visible = false;
            }

            if (this.IsWeight.Checked == true)
            {
                this.Weight .Visible = true;
            }
            else
            {
                this.Weight .Visible = false;
            }

            EAdvRotateType rotateType = EAdvRotateTypeUtils.GetEnumType(this.RotateType.SelectedValue);
            if (rotateType == EAdvRotateType.SlideRotate)
            {
                this.RotateIntervalRow.Visible = true;
            }
            else
            {
                this.RotateIntervalRow.Visible = false;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            { 
                try
                {
                    if (this.isEdit)
                    {
                        AdvInfo advInfo = DataProvider.AdvDAO.GetAdvInfo(this.advID, base.PublishmentSystemID);
                        advInfo.AdvName = this.AdvName.Text;
                        advInfo.IsEnabled = TranslateUtils.ToBool(this.IsEnabled.SelectedValue);
                        advInfo.IsDateLimited = this.IsDateLimited.Checked;
                        advInfo.StartDate = TranslateUtils.ToDateTime(this.StartDate.Text);
                        advInfo.EndDate = TranslateUtils.ToDateTime(this.EndDate.Text);
                        advInfo.NodeIDCollectionToChannel = ControlUtils.GetSelectedListControlValueCollection(this.NodeIDCollectionToChannel);
                        advInfo.NodeIDCollectionToContent = ControlUtils.GetSelectedListControlValueCollection(this.NodeIDCollectionToContent);
                        advInfo.FileTemplateIDCollection = ControlUtils.GetSelectedListControlValueCollection(this.FileTemplateIDCollection);
                        advInfo.LevelType = EAdvLevelTypeUtils.GetEnumType(this.LevelType.SelectedValue);
                        advInfo.Level = TranslateUtils.ToInt(this.Level.SelectedValue);
                        advInfo.IsWeight = this.IsWeight.Checked;
                        advInfo.Weight  = TranslateUtils.ToInt(this.Weight .SelectedValue);
                        advInfo.RotateType = EAdvRotateTypeUtils.GetEnumType(this.RotateType.SelectedValue);
                        advInfo.RotateInterval = TranslateUtils.ToInt(this.RotateInterval.Text);
                        advInfo.Summary = this.Summary.Text;

                        DataProvider.AdvDAO.Update(advInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "修改固定广告", string.Format("广告名称：{0}", advInfo.AdvName));
                        base.SuccessMessage("修改广告成功！");
                    }
                    else
                    {
                        AdvInfo advInfo = new AdvInfo();
                        advInfo.AdAreaID = this.adAreadID;
                        advInfo.PublishmentSystemID = base.PublishmentSystemID;
                        advInfo.AdvName = this.AdvName.Text;
                        advInfo.Summary = this.Summary.Text;
                        advInfo.IsEnabled = TranslateUtils.ToBool(this.IsEnabled.SelectedValue);
                        advInfo.IsDateLimited = this.IsDateLimited.Checked;
                        advInfo.StartDate = TranslateUtils.ToDateTime(this.StartDate.Text);
                        advInfo.EndDate = TranslateUtils.ToDateTime(this.EndDate.Text);
                        advInfo.NodeIDCollectionToChannel = ControlUtils.GetSelectedListControlValueCollection(this.NodeIDCollectionToChannel);
                        advInfo.NodeIDCollectionToContent = ControlUtils.GetSelectedListControlValueCollection(this.NodeIDCollectionToContent);
                        advInfo.FileTemplateIDCollection = ControlUtils.GetSelectedListControlValueCollection(this.FileTemplateIDCollection);
                        advInfo.LevelType = EAdvLevelTypeUtils.GetEnumType(this.LevelType.SelectedValue);
                        advInfo.Level = TranslateUtils.ToInt(this.Level.SelectedValue);
                        advInfo.IsWeight = this.IsWeight.Checked;
                        advInfo.Weight  = TranslateUtils.ToInt(this.Weight .SelectedValue);
                        advInfo.RotateType = EAdvRotateTypeUtils.GetEnumType(this.RotateType.SelectedValue);
                        advInfo.RotateInterval = TranslateUtils.ToInt(this.RotateInterval.Text);

                        DataProvider.AdvDAO.Insert(advInfo);
                        StringUtility.AddLog(base.PublishmentSystemID, "新增固定广告", string.Format("广告名称：{0}", advInfo.AdvName));
                        base.SuccessMessage("新增广告成功！");
                    }

                    base.AddWaitAndRedirectScript(PageUtils.GetCMSUrl("background_adv.aspx?PublishmentSystemID=" + base.PublishmentSystemID + "&AdAreaID=" + this.adAreadID));
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("操作失败：{0}", ex.Message));
                }
            }
        }
    }
}
