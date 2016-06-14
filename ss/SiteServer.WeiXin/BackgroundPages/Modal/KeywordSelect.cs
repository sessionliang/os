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
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Controls;


using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using System.Text;
using System.Collections.Generic;
using SiteServer.WeiXin.Model;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
	public class KeywordSelect : BackgroundBasePage
	{
        public DropDownList ddlKeywordType;
        public TextBox tbKeyword;

        public Repeater rptContents;
        public SqlPager spContents;

        private string jsMethod;

        public static string GetOpenWindowString(int publishmentSystemID, string jsMethod)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("jsMethod", jsMethod);
            return PageUtilityWX.GetOpenWindowString("选择关键词", "modal_keywordSelect.aspx", arguments);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.jsMethod = Request.QueryString["jsMethod"];

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
            this.spContents.ItemsPerPage = 50;
            this.spContents.SortField = DataProviderWX.KeywordMatchDAO.GetSortField();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if(!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "关键词搜索", string.Empty);

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

                ltlKeyword.Text = string.Format(@"<div class=""alert alert-success pull-left"" style=""margin:5px;padding-right:14px; cursor:pointer;"" onclick=""window.parent.{0}('{1}');{2}""><strong style=""color: #468847"">{1}</strong>&nbsp;({3})</div>", this.jsMethod, keyword, JsUtils.OpenWindow.HIDE_POP_WIN, EKeywordTypeUtils.GetText(keywordType));
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
                    this._pageUrl = string.Format("modal_keywordSelect.aspx?publishmentSystemID={0}&keywordType={1}&keyword={2}&jsMethod={3}", base.PublishmentSystemID, this.ddlKeywordType.SelectedValue, this.tbKeyword.Text, this.jsMethod);
                }
                return this._pageUrl;
            }
        }
	}
}
