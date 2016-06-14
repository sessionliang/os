using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;


using System.Data;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.BackgroundPages.Modal;

namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundGovPublicContent : BackgroundGovPublicBasePage
	{
        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlColumnHeadRows;

        public PlaceHolder phContentButtons;
        public Literal ltlContentButtons;

        public DateTimeTextBox DateFrom;
        public DropDownList SearchType;
        public TextBox Keyword;

        private int departmentID;
        private string classCode;
        private int categoryID;
        private ArrayList tableStyleInfoArrayList;
        private StringCollection attributesOfDisplay;

        private readonly Hashtable valueHashtable = new Hashtable();

		public void Page_Load(object sender, EventArgs E)
        {
            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.departmentID = TranslateUtils.ToInt(base.Request.QueryString["DepartmentID"]);
            this.classCode = base.Request.QueryString["ClassCode"];
            this.categoryID = TranslateUtils.ToInt(base.Request.QueryString["CategoryID"]);

            ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, base.PublishmentSystemInfo.Additional.GovPublicNodeID);
            this.tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.GovPublicContent, base.PublishmentSystemInfo.AuxiliaryTableForGovPublic, relatedIdentities);
            this.attributesOfDisplay = TranslateUtils.StringCollectionToStringCollection(NodeManager.GetContentAttributesOfDisplay(base.PublishmentSystemID, base.PublishmentSystemInfo.Additional.GovPublicNodeID));

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            
            if (!string.IsNullOrEmpty(base.Request.QueryString["SearchType"]))
            {
                ArrayList owningNodeIDArrayList = new ArrayList();
                owningNodeIDArrayList.Add(base.PublishmentSystemInfo.Additional.GovPublicNodeID);
                this.spContents.SelectCommand = DataProvider.ContentDAO.GetSelectCommend(ETableStyle.GovPublicContent, base.PublishmentSystemInfo.AuxiliaryTableForGovPublic, base.PublishmentSystemID, base.PublishmentSystemInfo.Additional.GovPublicNodeID, PermissionsManager.Current.IsSystemAdministrator, owningNodeIDArrayList, base.Request.QueryString["SearchType"], base.Request.QueryString["Keyword"], base.Request.QueryString["DateFrom"], string.Empty, false, ETriState.All, false, false);
            }
            else
            {                
                if (this.departmentID > 0)
                {
                    this.spContents.SelectCommand = DataProvider.GovPublicContentDAO.GetSelectCommendByDepartmentID(base.PublishmentSystemInfo, this.departmentID);
                }
                else if (!string.IsNullOrEmpty(this.classCode) && this.categoryID > 0)
                {
                    this.spContents.SelectCommand = DataProvider.GovPublicContentDAO.GetSelectCommendByCategoryID(base.PublishmentSystemInfo, this.classCode, this.categoryID);
                }
            }

            this.spContents.SortField = ContentAttribute.ID;
            this.spContents.SortMode = SortMode.DESC;

			if(!IsPostBack)
			{
                string nodeName = string.Empty;
                if (this.departmentID > 0)
                {
                    nodeName = DepartmentManager.GetDepartmentName(this.departmentID);
                }
                else if (!string.IsNullOrEmpty(this.classCode) && this.categoryID > 0)
                {
                    nodeName = DataProvider.GovPublicCategoryDAO.GetCategoryName(this.categoryID);
                }

                base.BreadCrumbWithItemTitle(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicContent, "信息管理", nodeName, AppManager.CMS.Permission.WebSite.GovPublicContent);
                
                this.spContents.DataBind();

                if (this.tableStyleInfoArrayList != null)
                {
                    foreach (TableStyleInfo styleInfo in this.tableStyleInfoArrayList)
                    {
                        if (styleInfo.IsVisible)
                        {
                            ListItem listitem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName);
                            this.SearchType.Items.Add(listitem);
                        }
                    }
                }

                //添加隐藏属性
                this.SearchType.Items.Add(new ListItem("内容ID", ContentAttribute.ID));
                this.SearchType.Items.Add(new ListItem("添加者", ContentAttribute.AddUserName));
                this.SearchType.Items.Add(new ListItem("最后修改者", ContentAttribute.LastEditUserName));
                this.SearchType.Items.Add(new ListItem("内容组", ContentAttribute.ContentGroupNameCollection));

                if (!string.IsNullOrEmpty(base.Request.QueryString["SearchType"]))
                {
                    this.DateFrom.Text = base.Request.QueryString["DateFrom"];
                    ControlUtils.SelectListItems(this.SearchType, base.Request.QueryString["SearchType"]);
                    this.Keyword.Text = base.Request.QueryString["Keyword"];
                    this.ltlContentButtons.Text += @"
<script>
$(document).ready(function() {
	$('#contentSearch').show();
});
</script>
";
                }

                this.ltlColumnHeadRows.Text = ContentUtility.GetColumnHeadRowsHtml(this.tableStyleInfoArrayList, this.attributesOfDisplay, ETableStyle.GovPublicContent, base.PublishmentSystemInfo);
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlItemTitle = e.Item.FindControl("ltlItemTitle") as Literal;
                Literal ltlColumnItemRows = e.Item.FindControl("ltlColumnItemRows") as Literal;
                Literal ltlItemStatus = e.Item.FindControl("ltlItemStatus") as Literal;
                Literal ltlItemEditUrl = e.Item.FindControl("ltlItemEditUrl") as Literal;

                ContentInfo contentInfo = new ContentInfo(e.Item.DataItem);

                ltlItemTitle.Text = WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, this.PageUrl);

                string showPopWinString = CheckState.GetOpenWindowString(base.PublishmentSystemID, contentInfo, this.PageUrl);
                ltlItemStatus.Text = string.Format(@"<a href=""javascript:;"" title=""设置内容状态"" onclick=""{0}"">{1}</a>", showPopWinString, LevelManager.GetCheckState(base.PublishmentSystemInfo, contentInfo.IsChecked, contentInfo.CheckedLevel));

                if (base.HasChannelPermissions(contentInfo.NodeID, AppManager.CMS.Permission.Channel.ContentEdit) || AdminManager.Current.UserName == contentInfo.AddUserName)
                {
                    ltlItemEditUrl.Text = string.Format("<a href=\"{0}\">编辑</a>", BackgroundGovPublicContentAdd.GetRedirectUrlOfEdit(base.PublishmentSystemID, contentInfo.NodeID, contentInfo.ID, this.PageUrl));
                }

                ltlColumnItemRows.Text = ContentUtility.GetColumnItemRowsHtml(this.tableStyleInfoArrayList, this.attributesOfDisplay, this.valueHashtable, ETableStyle.GovPublicContent, base.PublishmentSystemInfo, contentInfo);
            }
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.PageUrl);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    if (this.departmentID > 0)
                    {
                        _pageUrl = PageUtils.GetWCMUrl(string.Format("background_govPublicContent.aspx?PublishmentSystemID={0}&DepartmentID={1}&page={2}", base.PublishmentSystemID, this.departmentID, TranslateUtils.ToInt(Request.QueryString["page"], 1)));
                    }
                    else if (!string.IsNullOrEmpty(this.classCode) && this.categoryID > 0)
                    {
                        _pageUrl = PageUtils.GetWCMUrl(string.Format("background_govPublicContent.aspx?PublishmentSystemID={0}&ClassCode={1}&CategoryID={2}&page={3}", base.PublishmentSystemID, this.classCode, this.categoryID, TranslateUtils.ToInt(Request.QueryString["page"], 1)));
                    }
                }
                return _pageUrl;
            }
        }
	}
}
