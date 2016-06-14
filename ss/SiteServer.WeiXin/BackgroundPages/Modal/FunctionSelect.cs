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
	public class FunctionSelect : BackgroundBasePage
	{
        public DropDownList ddlKeywordType;
        public TextBox tbTitle;

        public PlaceHolder phFunction;
        public Repeater rptContents;
        public SqlPager spContents;

        private string jsMethod;
        private int itemIndex;

        public static string GetOpenWindowStringByItemIndex(int publishmentSystemID, string jsMethod, string itemIndex)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("jsMethod", jsMethod);
            arguments.Add("itemIndex", itemIndex);
            return PageUtilityWX.GetOpenWindowString("选择微功能", "modal_functionSelect.aspx", arguments, true);
        }

        public static string GetOpenWindowString(int publishmentSystemID, string jsMethod)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("jsMethod", jsMethod);
            return PageUtilityWX.GetOpenWindowString("选择微功能", "modal_functionSelect.aspx", arguments, true);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.jsMethod = base.Request.QueryString["jsMethod"];
            this.itemIndex = TranslateUtils.ToInt(base.GetQueryString("itemIndex"));

			if(!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "微功能搜索", string.Empty);
                 
                EKeywordTypeUtils.AddListItemsUrlOnly(this.ddlKeywordType);
                this.ddlKeywordType.Items.Insert(0, new ListItem("<选择微功能类型>", string.Empty));

                this.ReFresh(null, EventArgs.Empty);
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;

                int functionID = TranslateUtils.EvalInt(e.Item.DataItem, "ID");
                EKeywordType keywordType = EKeywordTypeUtils.GetEnumType(this.ddlKeywordType.SelectedValue);
                string pageTitle = KeywordManager.GetFunctionName(keywordType, functionID);

                string clickString = string.Empty;

                if (base.Request.QueryString["itemIndex"] != null)
                {
                    clickString = string.Format(@"window.parent.{0}({1}, '{2}', {3}, '{4}');{5}", this.jsMethod, this.itemIndex, EKeywordTypeUtils.GetValue(keywordType), functionID, pageTitle, JsUtils.OpenWindow.HIDE_POP_WIN);
                }
                else
                {
                    clickString = string.Format(@"window.parent.{0}('{1},{2},{3}');{4}", this.jsMethod, EKeywordTypeUtils.GetValue(keywordType), functionID, pageTitle, JsUtils.OpenWindow.HIDE_POP_WIN);
                }

                ltlTitle.Text = string.Format(@"
<div class=""alert alert-success pull-left"" style=""margin:5px;padding-right:14px; cursor:pointer;"" onclick=""{0}"">
    <strong style=""color: #468847"">{1}</strong>
</div>", clickString, pageTitle);
                 
            }
        }

        public void ReFresh(object sender, EventArgs E)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.ddlKeywordType.SelectedValue))
                {
                    this.spContents.ControlToPaginate = this.rptContents;
                    this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

                    EKeywordType keywordType = EKeywordTypeUtils.GetEnumType(this.ddlKeywordType.SelectedValue);

                    this.spContents.SelectCommand = KeywordManager.GetFunctionSqlString(base.PublishmentSystemID, keywordType);

                    this.spContents.ItemsPerPage = 50;
                    this.spContents.SortField = "ID";
                    this.spContents.SortMode = SortMode.DESC;
                    this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

                    this.spContents.DataBind();

                    this.phFunction.Visible = this.rptContents.Items.Count > 0;
                }
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }
	}
}
