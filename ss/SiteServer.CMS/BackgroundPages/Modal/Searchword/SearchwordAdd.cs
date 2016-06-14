using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class SearchwordAdd : BackgroundBasePage
    {
        protected TextBox tbSearchword;
        protected TextBox tbSearchCount;

        private string returnUrl;
        private int searchwordID;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtility.GetOpenWindowString("添加搜索关键词", "modal_searchwordAdd.aspx", arguments, 400, 250);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int searchwordID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("SearchwordID", searchwordID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtility.GetOpenWindowString("修改搜索关键词", "modal_searchwordAdd.aspx", arguments, 400, 250);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.searchwordID = TranslateUtils.ToInt(base.GetQueryStringNoSqlAndXss("SearchwordID"));

            if (!IsPostBack)
            {
                if (this.searchwordID != 0)
                {
                    SearchwordInfo contentInfo = DataProvider.SearchwordDAO.GetSearchwordInfo(this.searchwordID);
                    if (contentInfo != null)
                    {
                        this.tbSearchword.Text = contentInfo.Searchword;
                        this.tbSearchCount.Text = contentInfo.SearchCount.ToString();
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isSuccess = false;
            if (this.searchwordID != 0)
            {
                try
                {
                    SearchwordInfo contentInfo = DataProvider.SearchwordDAO.GetSearchwordInfo(this.searchwordID);
                    contentInfo.Searchword = this.tbSearchword.Text;
                    contentInfo.SearchCount = TranslateUtils.ToInt(this.tbSearchCount.Text);
                    contentInfo.PublishmentSystemID = base.PublishmentSystemID;

                    SearchwordInfo existSearchwordInfo = DataProvider.SearchwordDAO.GetSearchwordInfo(base.PublishmentSystemID, contentInfo.Searchword);
                    if (existSearchwordInfo == null || (existSearchwordInfo != null && existSearchwordInfo.ID == contentInfo.ID))
                    {
                        DataProvider.SearchwordDAO.Update(contentInfo);
                        isSuccess = true;
                    }
                    else
                    {
                        base.FailMessage(string.Format("搜索关键词修改失败:已经存在【{0}】搜索关键词", contentInfo.Searchword));
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "搜索关键词修改失败:" + ex.Message);
                }
            }
            else
            {
                try
                {
                    SearchwordInfo contentInfo = new SearchwordInfo();
                    contentInfo.Searchword = this.tbSearchword.Text;
                    contentInfo.SearchCount = TranslateUtils.ToInt(this.tbSearchCount.Text);
                    contentInfo.PublishmentSystemID = base.PublishmentSystemID;

                    SearchwordInfo existSearchwordInfo = DataProvider.SearchwordDAO.GetSearchwordInfo(base.PublishmentSystemID, contentInfo.Searchword);
                    if (existSearchwordInfo == null)
                    {
                        DataProvider.SearchwordDAO.Insert(contentInfo);
                        isSuccess = true;
                    }
                    else
                    {
                        base.FailMessage(string.Format("搜索关键词添加失败:已经存在【{0}】搜索关键词", contentInfo.Searchword));
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "搜索关键词添加失败:" + ex.Message);
                }
            }
            if (isSuccess)
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
        }
    }
}
