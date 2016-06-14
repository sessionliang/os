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
    public class BackgroundAdAreaAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public TextBox AdAreaName;
        public RadioButtonList IsEnabled;
        public TextBox Width;
        public TextBox Height;
        public TextBox Summary;

        private bool isEdit = false;
        private string theAdAreaName;

        public void Page_Load(object sender, EventArgs E)
        {
            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (base.GetQueryString("AdAreaName") != null)
            {
                this.isEdit = true;
                this.theAdAreaName = base.GetQueryString("AdAreaName");
            }

            if (!Page.IsPostBack)
            {
                string pageTitle = this.isEdit ? "编辑广告位" : "添加广告位";
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Advertisement, pageTitle, AppManager.CMS.Permission.WebSite.Advertisement);

                this.ltlPageTitle.Text = pageTitle;

                EBooleanUtils.AddListItems(this.IsEnabled);
                ControlUtils.SelectListItems(this.IsEnabled, true.ToString());
                if (this.isEdit)
                {
                    AdAreaInfo adAreaInfo = DataProvider.AdAreaDAO.GetAdAreaInfo(this.theAdAreaName, base.PublishmentSystemID);
                    this.AdAreaName.Text = adAreaInfo.AdAreaName;
                    this.IsEnabled.SelectedValue = adAreaInfo.IsEnabled.ToString();
                     
                    this.Width.Text = adAreaInfo.Width.ToString();
                    this.Height.Text = adAreaInfo.Height.ToString();
                    this.Summary.Text = adAreaInfo.Summary;
                }
            }

            base.SuccessMessage(string.Empty);
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (this.isEdit == false)
                {
                    if (DataProvider.AdAreaDAO.IsExists(this.AdAreaName.Text, base.PublishmentSystemID))
                    {
                        base.FailMessage(string.Format("名称为“{0}”的广告位已存在，请更改广告位名称！", this.AdAreaName.Text));
                        return;
                    }
                }
                try
                {
                    if (this.isEdit)
                    { 
                        AdAreaInfo adAreaInfo = DataProvider.AdAreaDAO.GetAdAreaInfo(this.theAdAreaName, base.PublishmentSystemID);
                        adAreaInfo.AdAreaName = this.AdAreaName.Text;
                        adAreaInfo.IsEnabled = TranslateUtils.ToBool(this.IsEnabled.SelectedValue);
                        adAreaInfo.Width = TranslateUtils.ToInt(this.Width.Text);
                        adAreaInfo.Height = TranslateUtils.ToInt(this.Height.Text);
                        adAreaInfo.Summary = this.Summary.Text;
                        
                        if (adAreaInfo.AdAreaName != this.AdAreaName.Text.Trim())
                        {
                            if (DataProvider.AdAreaDAO.IsExists(this.AdAreaName.Text, base.PublishmentSystemID))
                            {
                                base.FailMessage(string.Format("名称为“{0}”的广告位已存在，请更改广告位名称！", this.AdAreaName.Text));
                                return;
                            }
                        }

                        DataProvider.AdAreaDAO.Update(adAreaInfo);
                        StringUtility.AddLog(base.PublishmentSystemID, "修改固定广告", string.Format("广告名称：{0}", adAreaInfo.AdAreaName));
                        base.SuccessMessage("修改广告成功！");
                    }
                    else
                    {
                         AdAreaInfo adAreaInfo = new AdAreaInfo(0, base.PublishmentSystemID, this.AdAreaName.Text, TranslateUtils.ToInt(this.Width.Text), TranslateUtils.ToInt(this.Height.Text), this.Summary.Text, TranslateUtils.ToBool(this.IsEnabled.SelectedValue), DateTime.Now);
                         DataProvider.AdAreaDAO.Insert(adAreaInfo);
                         StringUtility.AddLog(base.PublishmentSystemID, "新增固定广告位", string.Format("广告位名称：{0}", adAreaInfo.AdAreaName));
                        base.SuccessMessage("新增广告位成功！");
                    }

                    base.AddWaitAndRedirectScript(PageUtils.GetCMSUrl("background_adArea.aspx?PublishmentSystemID=" + base.PublishmentSystemID));
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, string.Format("操作失败：{0}", ex.Message));
                }
            }
        }
    }
}
