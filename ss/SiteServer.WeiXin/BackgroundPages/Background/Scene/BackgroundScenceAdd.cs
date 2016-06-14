using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundScenceAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;

        public PlaceHolder phStep1;
        public TextBox tbKeywords;
        public TextBox tbTitle;
        public TextBox tbSummary;
        public DateTimeTextBox dtbStartDate;
        public DateTimeTextBox dtbEndDate;
        public CheckBox cbIsEnabled;
        public Literal ltlImageUrl;

        public Button btnSubmit;

        private int scenceID;
        public static string GetRedirectUrl(int publishmentSystemID, int scenceID)
        {
            return PageUtils.GetWXUrl(string.Format("background_scenceAdd.aspx?publishmentSystemID={0}&scenceID={1}", publishmentSystemID, scenceID));
        }
        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.scenceID = TranslateUtils.ToInt(base.GetQueryString("scenceID"));

            if (!IsPostBack)
            {
                string pageTitle = this.scenceID > 0 ? "编辑应用场景" : "添加应用场景";
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, pageTitle, AppManager.Platform.Permission.Platform_Site);
                this.ltlPageTitle.Text = pageTitle;

                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", CouponManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));

                if (this.scenceID > 0)
                {
                    ScenceInfo scenceInfo = DataProviderWX.ScenceDAO.GetScenceInfo(this.scenceID);

                    this.tbKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(scenceInfo.KeywordID);
                    this.cbIsEnabled.Checked = !scenceInfo.IsDisabled;
                    this.dtbStartDate.DateTime = scenceInfo.StartDate;
                    this.dtbEndDate.DateTime = scenceInfo.EndDate;
                    this.tbTitle.Text = scenceInfo.Title;
                    if (!string.IsNullOrEmpty(scenceInfo.ImageUrl))
                    {
                        this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, scenceInfo.ImageUrl));
                    }
                    this.tbSummary.Text = scenceInfo.Summary;

                }
                else
                {
                    this.dtbEndDate.DateTime = DateTime.Now.AddMonths(1);
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                int selectedStep = 0;
                if (this.phStep1.Visible)
                {
                    selectedStep = 1;
                }
                if (selectedStep == 1)
                {
                    bool isConflict = false;
                    string conflictKeywords = string.Empty;
                    if (!string.IsNullOrEmpty(this.tbKeywords.Text))
                    {
                        if (this.scenceID > 0)
                        {
                            CouponActInfo actInfo = DataProviderWX.CouponActDAO.GetActInfo(this.scenceID);
                            isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, actInfo.KeywordID, this.tbKeywords.Text, out conflictKeywords);
                        }
                        else
                        {
                            isConflict = KeywordManager.IsKeywordInsertConflict(base.PublishmentSystemID, this.tbKeywords.Text, out conflictKeywords);
                        }
                    }

                    if (isConflict)
                    {
                        base.FailMessage(string.Format("触发关键词“{0}”已存在，请设置其他关键词", conflictKeywords));
                        this.phStep1.Visible = true;
                    }
                }
            }
        }
    }
}
