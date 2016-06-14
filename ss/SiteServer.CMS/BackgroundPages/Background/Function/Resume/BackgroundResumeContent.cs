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


namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundResumeContent : BackgroundBasePage
	{
        public Repeater rptContents;
        public SqlPager spContents;

		public Button Delete;
        public Button SetIsView;
        public Button SetNotView;
        public Button Return;

        private int styleID;
        private int jobContentID;
        private string returnUrl;
        private Hashtable hashtable = new Hashtable();

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            string pageTitle = string.Empty;
            if (!string.IsNullOrEmpty(base.GetQueryString("JobContentID")))
            {
                this.jobContentID = TranslateUtils.ToInt(base.GetQueryString("JobContentID"));
                this.returnUrl = base.GetQueryString("ReturnUrl");
                if (this.jobContentID == 0) return;

                pageTitle = BaiRongDataProvider.ContentDAO.GetValue(base.PublishmentSystemInfo.AuxiliaryTableForJob, this.jobContentID, ContentAttribute.Title);
                this.Return.Visible = true;
            }
            else if (!string.IsNullOrEmpty(base.GetQueryString("StyleName")))
            {
                string styleName = base.GetQueryString("StyleName");

                TagStyleInfo styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(base.PublishmentSystemID, Constants.STLElementName.StlResume, styleName);
                if (styleInfo == null) return;

                this.styleID = styleInfo.StyleID;
                pageTitle = styleName;
                this.Return.Visible = false;
            }
            else if (!string.IsNullOrEmpty(base.GetQueryString("StyleID")))
            {
                this.styleID = TranslateUtils.ToInt(base.GetQueryString("StyleID"));
                if (this.styleID == 0) return;

                TagStyleInfo styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(this.styleID);
                if (styleInfo == null) return;

                pageTitle = styleInfo.StyleName;
                this.Return.Visible = false;
            }

            if (!string.IsNullOrEmpty(base.GetQueryString("Delete")))
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("ContentIDCollection"));
                if (arraylist.Count > 0)
                {
                    try
                    {
                        DataProvider.ResumeContentDAO.Delete(arraylist);
                        StringUtility.AddLog(base.PublishmentSystemID, "删除简历");
                        base.SuccessMessage("删除成功！");
                        PageUtils.Redirect(this.PageUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "删除失败！");
                    }
                }
            }
            else if (!string.IsNullOrEmpty(base.GetQueryString("SetView")))
            {
                bool isView = TranslateUtils.ToBool(base.GetQueryString("IsView"));
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("ContentIDCollection"));
                if (arraylist.Count > 0)
                {
                    try
                    {
                        DataProvider.ResumeContentDAO.SetIsView(arraylist, isView);
                        base.SuccessMessage("设置成功！");
                        PageUtils.Redirect(this.PageUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "设置失败！");
                    }
                }
            }

            this.spContents.ControlToPaginate = this.rptContents;
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            if (this.styleID > 0)
            {
                this.spContents.SelectCommand = DataProvider.ResumeContentDAO.GetSelectStringOfID(this.styleID, string.Empty);
            }
            else
            {
                this.spContents.SelectCommand = DataProvider.ResumeContentDAO.GetSelectStringOfID(base.PublishmentSystemID, this.jobContentID, string.Empty);
            }
            this.spContents.SortField = DataProvider.ResumeContentDAO.GetSortFieldName();
            this.spContents.SortMode = SortMode.DESC;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);

			if(!IsPostBack)
			{
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Resume, string.Format("{0}({1})", pageTitle, this.spContents.TotalCount), AppManager.CMS.Permission.WebSite.Resume);

                this.spContents.DataBind();

                this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(this.PageUrl + "&Delete=True", "ContentIDCollection", "ContentIDCollection", "请选择需要删除的简历！", "此操作将删除所选内容，确定吗？"));

                this.SetIsView.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValue(this.PageUrl + "&SetView=True&IsView=True", "ContentIDCollection", "ContentIDCollection", "请选择需要设置的简历！"));

                this.SetNotView.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValue(this.PageUrl + "&SetView=True&IsView=False", "ContentIDCollection", "ContentIDCollection", "请选择需要设置的简历！"));
			}			
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int contentID = TranslateUtils.EvalInt(e.Item.DataItem, "ID");
                ResumeContentInfo contentInfo = DataProvider.ResumeContentDAO.GetContentInfo(contentID);

                Literal ltlTr = e.Item.FindControl("ltlTr") as Literal;
                Literal ltlRealName = e.Item.FindControl("ltlRealName") as Literal;
                Literal ltlGender = e.Item.FindControl("ltlGender") as Literal;
                Literal ltlMobilePhone = e.Item.FindControl("ltlMobilePhone") as Literal;
                Literal ltlEmail = e.Item.FindControl("ltlEmail") as Literal;
                Literal ltlEducation = e.Item.FindControl("ltlEducation") as Literal;
                Literal ltlJobTitle = e.Item.FindControl("ltlJobTitle") as Literal;
                Literal ltlLastSchoolName = e.Item.FindControl("ltlLastSchoolName") as Literal;
                Literal ltlAddDate = e.Item.FindControl("ltlAddDate") as Literal;
                Literal ltlViewUrl = e.Item.FindControl("ltlViewUrl") as Literal;

                ltlTr.Text = string.Format(@"<tr style=""height:25px;font-weight:{0}"">", contentInfo.IsView ? "normal" : "bold");

                ltlRealName.Text = contentInfo.GetExtendedAttribute(ResumeContentAttribute.RealName);
                if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(ResumeContentAttribute.ImageUrl)))
                {
                    ltlRealName.Text += string.Format(@"&nbsp;<a id=""preview_{0}"" href=""{1}""><img src='../../sitefiles/bairong/icons/img.gif' alt='预览相片' align='absmiddle' border=0 /></a>
<script type=""text/javascript"">
$(document).ready(function() {{
	$(""#preview_{0}"").fancybox();
}});
</script>", contentID, PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, contentInfo.GetExtendedAttribute(ResumeContentAttribute.ImageUrl)));
                }
                ltlGender.Text = contentInfo.GetExtendedAttribute(ResumeContentAttribute.Gender);
                ltlMobilePhone.Text = contentInfo.GetExtendedAttribute(ResumeContentAttribute.MobilePhone);
                ltlEmail.Text = contentInfo.GetExtendedAttribute(ResumeContentAttribute.Email);
                ltlEducation.Text = contentInfo.GetExtendedAttribute(ResumeContentAttribute.Education);
                if (contentInfo.JobContentID > 0)
                {
                    string title = hashtable[contentInfo.JobContentID] as string;
                    if (title == null)
                    {
                        title = BaiRongDataProvider.ContentDAO.GetValue(base.PublishmentSystemInfo.AuxiliaryTableForJob, contentInfo.JobContentID, ContentAttribute.Title);
                        hashtable[contentInfo.JobContentID] = title;
                    }
                    ltlJobTitle.Text = title;
                }
                ltlLastSchoolName.Text = contentInfo.GetExtendedAttribute(ResumeContentAttribute.LastSchoolName);
                ltlAddDate.Text = DateUtils.GetDateString(contentInfo.AddDate);

                ltlViewUrl.Text = string.Format(@"<a href=""{0}"" target=""_blank"" onclick=""this.parentNode.parentNode.style.fontWeight='normal';"">查看</a>", PageUtility.GetResumePreviewUrl(base.PublishmentSystemID, contentID));
            }
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    if (this.styleID > 0)
                    {
                        _pageUrl = PageUtils.GetCMSUrl(string.Format("background_resumeContent.aspx?PublishmentSystemID={0}&StyleID={1}", base.PublishmentSystemID, this.styleID));
                    }
                    else
                    {
                        _pageUrl = PageUtils.GetCMSUrl(string.Format("background_resumeContent.aspx?PublishmentSystemID={0}&JobContentID={1}&ReturnUrl={2}", base.PublishmentSystemID, this.jobContentID, this.returnUrl));
                    }
                }
                return _pageUrl;
            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(StringUtils.ValueFromUrl(this.returnUrl));
        }
	}
}
