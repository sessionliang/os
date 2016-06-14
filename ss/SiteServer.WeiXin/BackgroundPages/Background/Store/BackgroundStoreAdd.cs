using BaiRong.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using SiteServer.WeiXin.Model;
using SiteServer.WeiXin.Core;

using System.Web.UI.HtmlControls;
using StoreManager = SiteServer.WeiXin.Core.StoreManager;
using SiteServer.CMS.Core;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundStoreAdd : BackgroundBasePageWX
    {
        public Literal ltlPageTitle;

        public PlaceHolder phStep1;
        public TextBox tbKeywords;
        public TextBox tbTitle;
        public TextBox tbSummary;
        public CheckBox cbIsEnabled;
        public Literal ltlImageUrl;

        public Button btnSubmit;
        public Button btnReturn;

        public HtmlInputHidden imageUrl;


        private int storeID;

        public static string GetRedirectUrl(int publishmentSystemID, int storeID)
        {
            return PageUtils.GetWXUrl(string.Format("background_StoreAdd.aspx?publishmentSystemID={0}&storeID={1}", publishmentSystemID, storeID));
        }

        public string GetUploadUrl()
        {
            return BackgroundAjaxUpload.GetImageUrlUploadUrl(base.PublishmentSystemID);
        }


        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.storeID = TranslateUtils.ToInt(base.GetQueryString("storeID"));

            if (!IsPostBack)
            {
                string pageTitle = this.storeID > 0 ? "编辑微门店" : "添加微门店";
                base.BreadCrumb(AppManager.WeiXin.LeftMenu.ID_Function, AppManager.WeiXin.LeftMenu.Function.ID_Store, pageTitle, AppManager.WeiXin.Permission.WebSite.Store);
                this.ltlPageTitle.Text = pageTitle;

                this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", StoreManager.GetImageUrl(base.PublishmentSystemInfo, string.Empty));

                if (this.storeID > 0)
                {
                    StoreInfo storeInfo = DataProviderWX.StoreDAO.GetStoreInfo(this.storeID);

                    this.tbKeywords.Text = DataProviderWX.KeywordDAO.GetKeywords(storeInfo.KeywordID);
                    this.cbIsEnabled.Checked = !storeInfo.IsDisabled;
                    this.tbTitle.Text = storeInfo.Title;
                    if (!string.IsNullOrEmpty(storeInfo.ImageUrl))
                    {
                        this.ltlImageUrl.Text = string.Format(@"<img id=""preview_imageUrl"" src=""{0}"" width=""370"" align=""middle"" />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, storeInfo.ImageUrl));
                    }
                    this.tbSummary.Text = storeInfo.Summary;

                    this.imageUrl.Value = storeInfo.ImageUrl;
                }

                this.btnReturn.Attributes.Add("onclick", string.Format(@"location.href=""{0}"";return false", BackgroundStore.GetRedirectUrl(base.PublishmentSystemID)));
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                bool isConflict = false;
                string conflictKeywords = string.Empty;
                if (!string.IsNullOrEmpty(this.tbKeywords.Text))
                {
                    if (this.storeID > 0)
                    {
                        StoreInfo storeInfo = DataProviderWX.StoreDAO.GetStoreInfo(this.storeID);
                        isConflict = KeywordManager.IsKeywordUpdateConflict(base.PublishmentSystemID, storeInfo.KeywordID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
                    }
                    else
                    {
                        isConflict = KeywordManager.IsKeywordInsertConflict(base.PublishmentSystemID, PageUtils.FilterXSS(this.tbKeywords.Text), out conflictKeywords);
                    }
                }

                if (isConflict)
                {
                    base.FailMessage(string.Format("触发关键词“{0}”已存在，请设置其他关键词", conflictKeywords));
                }
                else
                {
                    StoreInfo storeInfo = new StoreInfo();
                    if (this.storeID > 0)
                    {
                        storeInfo = DataProviderWX.StoreDAO.GetStoreInfo(this.storeID);
                    }
                    storeInfo.PublishmentSystemID = base.PublishmentSystemID;

                    storeInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordID(base.PublishmentSystemID, this.storeID > 0, this.tbKeywords.Text, EKeywordType.Store, storeInfo.KeywordID);
                    storeInfo.IsDisabled = !this.cbIsEnabled.Checked;

                    storeInfo.Title = PageUtils.FilterXSS(this.tbTitle.Text);
                    storeInfo.ImageUrl = this.imageUrl.Value; ;
                    storeInfo.Summary = this.tbSummary.Text;

                    try
                    {
                        if (this.storeID > 0)
                        {
                            DataProviderWX.StoreDAO.Update(storeInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改微门店", string.Format("微门店:{0}", this.tbTitle.Text));
                            base.SuccessMessage("修改微门店成功！");
                        }
                        else
                        {
                            this.storeID = DataProviderWX.StoreDAO.Insert(storeInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加微门店", string.Format("微门店:{0}", this.tbTitle.Text));
                            base.SuccessMessage("添加微门店成功！");
                        }

                        string redirectUrl = PageUtils.GetWXUrl(string.Format("background_storeItem.aspx?publishmentSystemID={0}&storeID={1}", base.PublishmentSystemID, this.storeID));
                        base.AddWaitAndRedirectScript(redirectUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "微门店设置失败！");
                    }
                }
            }
        }
    }
}
