using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Web.UI;


namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundMoban : BackgroundBasePage
    {
        public DropDownList ddlTaxis;
        public TextBox tbSN;
        public TextBox tbKeyword;
        public Literal ltlTotalCount;

        public HyperLink hlAdd;
        public HyperLink hlSetting;
        public HyperLink hlDelete;

        public Repeater rptContents;
        public SqlPager spContents;

        public void Page_Load(object sender, EventArgs E)
        {
            if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.QueryString["IDCollection"]);
                if (arraylist.Count > 0)
                {
                    try
                    {
                        DataProvider.MobanDAO.Delete(arraylist);
                        base.SuccessMessage("删除成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "删除失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = 30;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = this.GetSelectString();
            this.spContents.SortField = DataProvider.MobanDAO.GetSortFieldName();
            this.spContents.SortMode = this.GetSortMode();
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

            if (!IsPostBack)
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

                this.hlAdd.NavigateUrl = BackgroundMobanAdd.GetAddUrl(this.PageUrl);

                this.hlSetting.Attributes.Add("onclick", Modal.MobanSetting.GetShowPopWinString());

                this.hlDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Delete=True", "IDCollection", "IDCollection", "请选择需要删除的模板！", "此操作将删除所选模板，确定吗？"));

                this.ltlTotalCount.Text = this.spContents.TotalCount.ToString();
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
                Literal ltlDownloadUrl = e.Item.FindControl("ltlDownloadUrl") as Literal;
                Literal ltlIsAliyun = e.Item.FindControl("ltlIsAliyun") as Literal;
                Literal ltlIsInitializationForm = e.Item.FindControl("ltlIsInitializationForm") as Literal;
                Literal ltlInitializationFormUrl = e.Item.FindControl("ltlInitializationFormUrl") as Literal;
                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

                ltlSN.Text = string.Format(@"<a href=""{0}"" target=""_blank"">{1}</a>", DataProvider.MobanDAO.GetMobanUrl(mobanInfo), mobanInfo.SN);
                ltlCover.Text = string.Format(@"<img class=""cover"" smallUrl=""http://moban.download.siteserver.cn/cover/214_156/{0}.jpg"" largeUrl=""http://moban.download.siteserver.cn/cover/415_303/{0}.jpg"" src=""http://moban.download.siteserver.cn/cover/214_156/{0}.jpg"" />", mobanInfo.SN);
                ltlCategory.Text = this.GetCategory(mobanInfo.Category);
                ltlIndustry.Text = mobanInfo.Industry;
                ltlColor.Text = mobanInfo.Color;
                ltlSummary.Text = mobanInfo.Summary;
                ltlAddDate.Text = DateUtils.GetDateString(mobanInfo.AddDate);
                ltlDownloadUrl.Text = string.Format(@"<a href=""http://moban.download.siteserver.cn/all/T_{0}.zip"" target=""_blank""><i class=""icon-download-alt""></i></a>", mobanInfo.SN);
                ltlIsAliyun.Text = StringUtils.GetTrueImageHtml(mobanInfo.IsAliyun);
                ltlIsInitializationForm.Text = StringUtils.GetTrueImageHtml(mobanInfo.IsInitializationForm);

                if (mobanInfo.IsInitializationForm)
                {
                    ltlInitializationFormUrl.Text = string.Format(@"<a href=""{0}"">设置表单</a>", string.Format("background_mobanFormMain.aspx?MobanID={0}", mobanInfo.ID));
                }
                else
                {
                    ltlInitializationFormUrl.Text = string.Format(@"<a href=""{0}"" style=""color:red"">添加表单</a>", string.Format("background_mobanFormMain.aspx?MobanID={0}", mobanInfo.ID));
                }
                ltlEditUrl.Text = string.Format(@"<a href=""{0}""><i class=""icon-edit""></i></a>", BackgroundMobanAdd.GetEditUrl(mobanInfo.ID, this.PageUrl));
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
                    _pageUrl = BackgroundMoban.GetRedirectUrl(TranslateUtils.ToBool(this.ddlTaxis.SelectedValue), this.tbSN.Text, this.tbKeyword.Text, TranslateUtils.ToInt(Request.QueryString["page"], 1));
                }
                return _pageUrl;
            }
        }

        public static string GetRedirectUrl(bool isTaxisDESC, string sn, string keyword, int page)
        {
            return string.Format("background_moban.aspx?isTaxisDESC={0}&sn={1}&keyword={2}&page={3}", isTaxisDESC, sn, keyword, page);
        }
    }
}
