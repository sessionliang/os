using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundSearchwordSetting : BackgroundBasePage
    {
        #region 其他设置
        protected TextBox tbSearchResultCountLimit;
        protected TextBox tbSearchCountLimit;
        protected TextBox tbSearchOutputLimit;
        protected RadioButtonList rblSearchSort;
        #endregion

        private SearchwordSettingInfo searchwordSettingInfo;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.searchwordSettingInfo = DataProvider.SearchwordSettingDAO.GetSearchwordSettingInfo(base.PublishmentSystemID);
            if (this.searchwordSettingInfo == null)
            {
                this.searchwordSettingInfo = new SearchwordSettingInfo();
            }
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Searchword, "其他设置", AppManager.CMS.Permission.WebSite.Searchword);

                #region 其他设置
                if (searchwordSettingInfo != null)
                {
                    this.tbSearchResultCountLimit.Text = searchwordSettingInfo.SearchResultCountLimit.ToString();
                    this.tbSearchCountLimit.Text = searchwordSettingInfo.SearchCountLimit.ToString();
                    this.tbSearchOutputLimit.Text = searchwordSettingInfo.SearchOutputLimit.ToString();

                    this.rblSearchSort.Items.Add(new ListItem("搜索结果数", SearchwordAttribute.SearchResultCount));
                    this.rblSearchSort.Items.Add(new ListItem("搜索次数", SearchwordAttribute.SearchCount));
                    ControlUtils.SelectListItems(this.rblSearchSort, string.IsNullOrEmpty(searchwordSettingInfo.SearchSort) ? SearchwordAttribute.SearchResultCount : searchwordSettingInfo.SearchSort);
                }
                #endregion
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                #region 其他设置
                searchwordSettingInfo.PublishmentSystemID = base.PublishmentSystemID;
                searchwordSettingInfo.SearchResultCountLimit = TranslateUtils.ToInt(this.tbSearchResultCountLimit.Text);
                searchwordSettingInfo.SearchCountLimit = TranslateUtils.ToInt(this.tbSearchCountLimit.Text);
                searchwordSettingInfo.SearchOutputLimit = TranslateUtils.ToInt(this.tbSearchOutputLimit.Text);
                searchwordSettingInfo.SearchSort = this.rblSearchSort.SelectedValue;
                #endregion

                try
                {
                    if (this.searchwordSettingInfo == null)
                    {
                        this.searchwordSettingInfo = new SearchwordSettingInfo();
                    }
                    if (this.searchwordSettingInfo != null && this.searchwordSettingInfo.ID == 0)
                    {
                        DataProvider.SearchwordSettingDAO.Insert(this.searchwordSettingInfo);
                    }
                    else
                    {
                        DataProvider.SearchwordSettingDAO.Update(this.searchwordSettingInfo);
                    }
                    base.SuccessMessage("设置修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "设置修改失败," + ex.Message);
                }
            }
        }
    }
}
