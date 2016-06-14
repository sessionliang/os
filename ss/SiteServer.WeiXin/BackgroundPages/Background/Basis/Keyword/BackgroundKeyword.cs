using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using System.Web.UI;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.CMS.Core;
using SiteServer.WeiXin.Model;
using BaiRong.Controls;
using BaiRong.Core.Data.Provider;
using System.Collections.Generic;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundKeyword : BackgroundBasePageWX
	{
        public DropDownList ddlKeywordType;
        public TextBox tbKeyword;

        public Repeater rptContents;
        public SqlPager spContents;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetWXUrl(string.Format("background_keyword.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (base.Request.QueryString["delete"] != null)
			{
                int keywordID = TranslateUtils.ToInt(base.Request.QueryString["deleteKeywordID"]);
                string keyword = base.Request.QueryString["deleteKeyword"];
			
				try
				{
                    KeywordInfo keywordInfo = DataProviderWX.KeywordDAO.GetKeywordInfo(keywordID);
                    List<string> keywordList = TranslateUtils.StringCollectionToStringList(keywordInfo.Keywords, ' ');
                    if (keywordList.Remove(keyword))
                    {
                        keywordInfo.Keywords = TranslateUtils.ObjectCollectionToString(keywordList, " ");
                        DataProviderWX.KeywordDAO.Update(keywordInfo);
                        base.SuccessDeleteMessage();
                    }
				}
				catch(Exception ex)
				{
                    base.FailDeleteMessage(ex);
				}
			}

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            if (base.Request.QueryString["keywordType"] == null)
            {
                this.spContents.SelectCommand = DataProviderWX.KeywordMatchDAO.GetSelectString(base.PublishmentSystemID);
            }
            else
            {
                string keywordType = base.Request.QueryString["keywordType"];
                string keyword = base.Request.QueryString["keyword"];
                this.spContents.SelectCommand = DataProviderWX.KeywordMatchDAO.GetSelectString(base.PublishmentSystemID, keywordType, keyword);
            }
            this.spContents.ItemsPerPage = 60;
            this.spContents.SortField = "KeywordID";
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_Category, "关键字管理", AppManager.CMS.Permission.WebSite.Category);

                ListItem listItem = new ListItem("所有类型", string.Empty);
                this.ddlKeywordType.Items.Add(listItem);

                EKeywordTypeUtils.AddListItems(this.ddlKeywordType);

                if (base.Request.QueryString["keywordType"] != null)
                {
                    string keywordType = base.Request.QueryString["keywordType"];
                    string keyword = base.Request.QueryString["keyword"];

                    ControlUtils.SelectListItems(this.ddlKeywordType, keywordType);
                    this.tbKeyword.Text = keyword;
                }

                this.spContents.DataBind();                
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int keywordID = TranslateUtils.EvalInt(e.Item.DataItem, "KeywordID");
                string keyword = TranslateUtils.EvalString(e.Item.DataItem, "Keyword");
                EKeywordType keywordType = EKeywordTypeUtils.GetEnumType(TranslateUtils.EvalString(e.Item.DataItem, "KeywordType"));

                Literal ltlKeyword = e.Item.FindControl("ltlKeyword") as Literal;

                string urlDelete = string.Format(@"{0}&delete=true&deleteKeywordID={1}&deleteKeyword={2}", BackgroundKeyword.GetRedirectUrl(base.PublishmentSystemID), keywordID, keyword);

                ltlKeyword.Text = string.Format(@"<div class=""alert alert-success pull-left"" style=""margin:5px;padding-right:14px;""><strong style=""color: #468847"">{0}</strong>&nbsp;({1})&nbsp;<a href=""javascript:;"" onclick=""{2}""><i class=""icon-edit""></i></a>&nbsp;<a href=""{3}"" onclick=""javascript:return confirm('此操作将删除关键字“{0}”，确认吗？');""><i class=""icon-remove""></i></a></div>", keyword, EKeywordTypeUtils.GetText(keywordType), Modal.KeywordEdit.GetOpenWindowString(base.PublishmentSystemID, keywordID, keyword), urlDelete);
            }
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            base.Response.Redirect(this.PageUrl, true);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    this._pageUrl = string.Format("background_keyword.aspx?publishmentSystemID={0}&keywordType={1}&keyword={2}", base.PublishmentSystemID, this.ddlKeywordType.SelectedValue, this.tbKeyword.Text);
                }
                return this._pageUrl;
            }
        }
	}
}
