using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CRM.Core;
using SiteServer.CRM.Model;
using System.Web.UI;


namespace SiteServer.CRM.BackgroundPages
{
    public class BackgroundApplyToWork : BackgroundApplyToBasePage
    {
        public DropDownList ddlTaxis;
        public DropDownList ddlState;
        public DropDownList ddlTypeID;
        public DropDownList ddlAddUserName;
        public DropDownList ddlUserName;
        public TextBox tbKeyword;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["IsAddUserName"]))
            {
                string pageUrl = BackgroundApplyToWork.GetRedirectUrl(base.PublishmentSystemID, TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), this.ddlState.SelectedValue, this.ddlTypeID.SelectedValue, AdminManager.Current.UserName, this.ddlUserName.SelectedValue, this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                PageUtils.Redirect(pageUrl);
                return;
            }
            else if (!string.IsNullOrEmpty(base.Request.QueryString["IsUserName"]))
            {
                string pageUrl = BackgroundApplyToWork.GetRedirectUrl(base.PublishmentSystemID, TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), EApplyStateUtils.GetValue(EApplyState.Accepted), this.ddlTypeID.SelectedValue, this.ddlAddUserName.SelectedValue, AdminManager.Current.UserName, this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                PageUtils.Redirect(pageUrl);
                return;
            }
            if (!IsPostBack)
            {
                bool isTaxisDESC = true;
                EBooleanUtils.AddListItems(this.ddlTaxis, "倒序", "正序");
                if (!string.IsNullOrEmpty(base.Request.QueryString["isTaxisDESC"]))
                {
                    isTaxisDESC = TranslateUtils.ToBool(base.Request.QueryString["isTaxisDESC"]);
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlTaxis, isTaxisDESC.ToString());
                }

                ListItem listItem = new ListItem("全部", string.Empty);
                this.ddlState.Items.Add(listItem);
                EApplyStateUtils.AddListItemsToWork(this.ddlState);
                if (!string.IsNullOrEmpty(base.Request.QueryString["state"]))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlState, base.Request.QueryString["state"]);
                }

                listItem = new ListItem("全部", string.Empty);
                this.ddlTypeID.Items.Add(listItem);
                ArrayList typeInfoArrayList = DataProvider.TypeDAO.GetTypeInfoArrayList(base.PublishmentSystemID);
                foreach (TypeInfo typeInfo in typeInfoArrayList)
                {
                    this.ddlTypeID.Items.Add(new ListItem(typeInfo.TypeName, typeInfo.TypeID.ToString()));
                }
                if (!string.IsNullOrEmpty(base.Request.QueryString["typeID"]))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlTypeID, base.Request.QueryString["typeID"]);
                }

                listItem = new ListItem("全部", string.Empty);
                this.ddlAddUserName.Items.Add(listItem);
                ArrayList addUserNameArrayList = DataProvider.ApplyDAO.GetAddUserNameArrayList(base.PublishmentSystemID);
                foreach (string addUserName in addUserNameArrayList)
                {
                    listItem = new ListItem(AdminManager.GetDisplayName(addUserName, true), addUserName);
                    this.ddlAddUserName.Items.Add(listItem);
                }
                if (!string.IsNullOrEmpty(base.Request.QueryString["addUserName"]))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlAddUserName, base.Request.QueryString["addUserName"]);
                }

                listItem = new ListItem("全部", string.Empty);
                this.ddlUserName.Items.Add(listItem);
                ArrayList userNameArrayList = DataProvider.ApplyDAO.GetUserNameArrayList(base.PublishmentSystemID);
                foreach (string userName in userNameArrayList)
                {
                    listItem = new ListItem(AdminManager.GetDisplayName(userName, true), userName);
                    this.ddlUserName.Items.Add(listItem);
                }
                if (!string.IsNullOrEmpty(base.Request.QueryString["userName"]))
                {
                    ControlUtils.SelectListItemsIgnoreCase(this.ddlUserName, base.Request.QueryString["userName"]);
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
            if (!string.IsNullOrEmpty(base.Request.QueryString["state"]) || !string.IsNullOrEmpty(base.Request.QueryString["typeID"]) || !string.IsNullOrEmpty(base.Request.QueryString["addUserName"]) || !string.IsNullOrEmpty(base.Request.QueryString["userName"]) || !string.IsNullOrEmpty(base.Request.QueryString["keyword"]))
            {
                return DataProvider.ApplyDAO.GetSelectStringToWork(base.projectID, base.Request.QueryString["state"], TranslateUtils.ToInt(base.Request.QueryString["typeID"]), base.Request.QueryString["addUserName"], base.Request.QueryString["userName"], base.Request.QueryString["keyword"]);
            }
            else
            {
                return DataProvider.ApplyDAO.GetSelectStringToWork(base.projectID);
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
                    _pageUrl = BackgroundApplyToWork.GetRedirectUrl(base.PublishmentSystemID, TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), this.ddlState.SelectedValue, this.ddlTypeID.SelectedValue, this.ddlAddUserName.SelectedValue, this.ddlUserName.SelectedValue, this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(int projectID)
        {
            return string.Format("background_ApplyToWork.aspx?ProjectID={0}", projectID);
        }

        private static string GetRedirectUrl(int projectID, bool isTaxisDESC, string state, string typeID, string addUserName, string userName, string keyword, int page)
        {
            return string.Format("background_ApplyToWork.aspx?ProjectID={0}&isTaxisDESC={1}&state={2}&typeID={3}&addUserName={4}&userName={5}&keyword={6}&page={7}", projectID, isTaxisDESC, state, typeID, addUserName, userName, keyword, page);
        }
    }
}
