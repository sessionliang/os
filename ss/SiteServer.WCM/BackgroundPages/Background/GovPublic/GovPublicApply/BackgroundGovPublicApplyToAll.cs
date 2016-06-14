using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Web.UI;


namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovPublicApplyToAll : BackgroundGovPublicApplyToBasePage
    {
        public DropDownList ddlTaxis;
        public DropDownList ddlState;
        public TextBox tbKeyword;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicApply, "所有申请", AppManager.CMS.Permission.WebSite.GovPublicApply);

                bool isTaxisDESC = true;
                EBooleanUtils.AddListItems(this.ddlTaxis, "倒序", "正序");
                if (!string.IsNullOrEmpty(base.Request.QueryString["isTaxisDESC"]))
                {
                    isTaxisDESC = TranslateUtils.ToBool(base.Request.QueryString["isTaxisDESC"]);
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlTaxis, isTaxisDESC.ToString());
                }
                ListItem listItem = new ListItem("全部", string.Empty);
                this.ddlState.Items.Add(listItem);
                EGovPublicApplyStateUtils.AddListItems(this.ddlState);
                if (!string.IsNullOrEmpty(base.Request.QueryString["state"]))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlState, base.Request.QueryString["state"]);
                }
                this.tbKeyword.Text = base.Request.QueryString["keyword"];
            }
        }

        public void Search_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        protected override string GetSelectString()
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["state"]) || !string.IsNullOrEmpty(base.Request.QueryString["keyword"]))
            {
                return DataProvider.GovPublicApplyDAO.GetSelectString(base.PublishmentSystemID, base.Request.QueryString["state"], base.Request.QueryString["keyword"]);
            }
            else
            {
                return DataProvider.GovPublicApplyDAO.GetSelectString(base.PublishmentSystemID);
            }
        }

        protected override SortMode GetSortMode()
        {
            bool isTaxisDESC = true;
            if (!string.IsNullOrEmpty(base.Request.QueryString["isTaxisDESC"]))
            {
                isTaxisDESC = TranslateUtils.ToBool(base.Request.QueryString["isTaxisDESC"]);
            }
            return isTaxisDESC ? SortMode.DESC : SortMode.ASC;
        }

        private string _pageUrl;
        protected override string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetWCMUrl(string.Format("background_govPublicApplyToAll.aspx?PublishmentSystemID={0}&isTaxisDESC={1}&state={2}&keyword={3}&page={4}", base.PublishmentSystemID, this.ddlTaxis.SelectedValue, this.ddlState.SelectedValue, this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1)));
                }
                return _pageUrl;
            }
        }
    }
}
