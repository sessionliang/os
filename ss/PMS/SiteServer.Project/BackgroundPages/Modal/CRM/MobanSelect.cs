using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;


using SiteServer.Project.Model;
using SiteServer.Project.Core;
using BaiRong.Controls;

namespace SiteServer.Project.BackgroundPages.Modal
{
	public class MobanSelect : BackgroundBasePage
	{
        public DropDownList ddlTaxis;
        public TextBox tbSN;
        public TextBox tbKeyword;

        public Repeater rptContents;
        public SqlPager spContents;

        private string scriptName;

        public static string GetShowPopWinString(string scriptName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("scriptName", scriptName);
            return JsUtils.OpenWindow.GetOpenWindowString("选择模板", "modal_mobanSelect.aspx", arguments, true);
        }

		public void Page_Load(object sender, EventArgs E)
		{
            this.scriptName = base.Request.QueryString["scriptName"];

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = this.GetSelectString();
            this.spContents.SortField = DataProvider.MobanDAO.GetSortFieldName();
            this.spContents.SortMode = this.GetSortMode();
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if (!IsPostBack)
			{
                if (!string.IsNullOrEmpty(base.Request.QueryString["MobanID"]))
                {
                    int mobanID = TranslateUtils.ToInt(base.Request.QueryString["MobanID"]);
                    MobanInfo mobanInfo = DataProvider.MobanDAO.GetMobanInfo(mobanID);
                    string scripts = string.Format("window.parent.{0}('{1}');", this.scriptName, mobanInfo.SN);
                    JsUtils.OpenWindow.CloseModalPageWithoutRefresh(base.Page, scripts);
                }
                else
                {
                    bool isTaxisDESC = true;
                    EBooleanUtils.AddListItems(this.ddlTaxis, "倒序", "正序");
                    if (!string.IsNullOrEmpty(base.Request.QueryString["isTaxisDESC"]))
                    {
                        isTaxisDESC = TranslateUtils.ToBool(base.Request.QueryString["isTaxisDESC"]);
                        ControlUtils.SelectListItemsIgnoreCase(this.ddlTaxis, isTaxisDESC.ToString());
                    }

                    this.tbSN.Text = base.Request.QueryString["sn"];
                    this.tbKeyword.Text = base.Request.QueryString["keyword"];

                    this.spContents.DataBind();
                }
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                MobanInfo mobanInfo = new MobanInfo(e.Item.DataItem);

                Literal ltlSN = e.Item.FindControl("ltlSN") as Literal;
                Literal ltlCover = e.Item.FindControl("ltlCover") as Literal;
                Literal ltlCategory = e.Item.FindControl("ltlCategory") as Literal;
                Literal ltlIndustry = e.Item.FindControl("ltlIndustry") as Literal;
                Literal ltlColor = e.Item.FindControl("ltlColor") as Literal;
                Literal ltlSummary = e.Item.FindControl("ltlSummary") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlIsAliyun = e.Item.FindControl("ltlIsAliyun") as Literal;
                Literal ltlIsInitializationForm = e.Item.FindControl("ltlIsInitializationForm") as Literal;

                ltlSN.Text = string.Format(@"<a href=""modal_mobanSelect.aspx?scriptName={0}&MobanID={1}"">{2}</a>", this.scriptName, mobanInfo.ID, mobanInfo.SN);
                ltlCover.Text = string.Format(@"<img class=""cover"" smallUrl=""http://moban.download.siteserver.cn/cover/214_156/{0}.jpg"" largeUrl=""http://moban.download.siteserver.cn/cover/415_303/{0}.jpg"" src=""http://moban.download.siteserver.cn/cover/214_156/{0}.jpg"" />", mobanInfo.SN);
                ltlCategory.Text = this.GetCategory(mobanInfo.Category);
                ltlIndustry.Text = mobanInfo.Industry;
                ltlColor.Text = mobanInfo.Color;
                ltlSummary.Text = mobanInfo.Summary;
                ltlAddDate.Text = DateUtils.GetDateString(mobanInfo.AddDate);
                ltlIsAliyun.Text = StringUtils.GetTrueImageHtml(mobanInfo.IsAliyun);
                ltlIsInitializationForm.Text = StringUtils.GetTrueImageHtml(mobanInfo.IsInitializationForm);
            }
        }

        private string GetCategory(string category)
        {
            string retval = string.Empty;
            if (category == "W1")
            {
                retval = "2013内部制作";
            }
            else if (category == "V4")
            {
                retval = "2013模板大赛";
            }
            else if (category == "V3")
            {
                retval = "2012模板大赛";
            }
            else if (category == "V2")
            {
                retval = "2011模板大赛";
            }
            else if (category == "V1")
            {
                retval = "2010模板大赛";
            }
            else if (category == "V0")
            {
                retval = "2010之前模板";
            }
            return retval + "（" + category + "）";
        }

        public void Search_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        protected string GetSelectString()
        {
            if (base.Request.QueryString["keyword"] != null)
            {
                return DataProvider.MobanDAO.GetSelectString(base.Request.QueryString["sn"], base.Request.QueryString["keyword"]);
            }
            else
            {
                return DataProvider.MobanDAO.GetSelectString();
            }
        }

        protected SortMode GetSortMode()
        {
            bool isTaxisDESC = true;
            if (!string.IsNullOrEmpty(base.Request.QueryString["isTaxisDESC"]))
            {
                isTaxisDESC = TranslateUtils.ToBool(base.Request.QueryString["isTaxisDESC"]);
            }
            return isTaxisDESC ? SortMode.DESC : SortMode.ASC;
        }

        private string _pageUrl;
        protected string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = MobanSelect.GetRedirectUrl(this.scriptName, TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), this.tbSN.Text, this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(string scriptName, bool isTaxisDESC, string sn, string keyword, int page)
        {
            return string.Format("modal_mobanSelect.aspx?scriptName={0}&isTaxisDESC={1}&sn={2}&keyword={3}&page={4}", scriptName, isTaxisDESC, sn, keyword, page);
        }
	}
}
