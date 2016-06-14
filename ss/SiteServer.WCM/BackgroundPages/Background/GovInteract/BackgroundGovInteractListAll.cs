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
    public class BackgroundGovInteractListAll : BackgroundGovInteractListBasePage
    {
        public DropDownList ddlTaxis;
        public DropDownList ddlState;
        public DateTimeTextBox tbDateFrom;
        public DateTimeTextBox tbDateTo;
        public TextBox tbKeyword;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovInteract, "所有办件", AppManager.CMS.Permission.WebSite.GovInteract);

                bool isTaxisDESC = true;
                EBooleanUtils.AddListItems(this.ddlTaxis, "倒序", "正序");
                if (!string.IsNullOrEmpty(base.Request.QueryString["isTaxisDESC"]))
                {
                    isTaxisDESC = TranslateUtils.ToBool(base.Request.QueryString["isTaxisDESC"]);
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlTaxis, isTaxisDESC.ToString());
                }
                ListItem listItem = new ListItem("全部", string.Empty);
                this.ddlState.Items.Add(listItem);
                EGovInteractStateUtils.AddListItems(this.ddlState);
                if (!string.IsNullOrEmpty(base.Request.QueryString["state"]))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlState, base.Request.QueryString["state"]);
                }
                this.tbDateFrom.Text = base.Request.QueryString["dateFrom"];
                this.tbDateTo.Text = base.Request.QueryString["dateTo"];
                this.tbKeyword.Text = base.Request.QueryString["keyword"];
            }
        }

        public void Search_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        protected override string GetSelectString()
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["state"]) || !string.IsNullOrEmpty(base.Request.QueryString["dateFrom"]) || !string.IsNullOrEmpty(base.Request.QueryString["dateTo"]) || !string.IsNullOrEmpty(base.Request.QueryString["keyword"]))
            {
                return DataProvider.GovInteractContentDAO.GetSelectString(base.PublishmentSystemInfo, base.nodeID, base.Request.QueryString["state"], base.Request.QueryString["dateFrom"], base.Request.QueryString["dateTo"], base.Request.QueryString["keyword"]);
            }
            else
            {
                return DataProvider.GovInteractContentDAO.GetSelectString(base.PublishmentSystemInfo, base.nodeID);
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
                    _pageUrl = PageUtils.GetWCMUrl(string.Format("background_govInteractListAll.aspx?PublishmentSystemID={0}&NodeID={1}&isTaxisDESC={2}&state={3}&dateFrom={4}&dateTo={5}&keyword={6}&page={7}", base.PublishmentSystemID, base.nodeID, this.ddlTaxis.SelectedValue, this.ddlState.SelectedValue, this.tbDateFrom.Text, this.tbDateTo.Text, this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1)));
                }
                return _pageUrl;
            }
        }
    }
}
