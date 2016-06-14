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
using BaiRong.Core.Data.Provider;
using System.Collections.Generic;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundContentsGroup : BackgroundBasePage
    {
        public Literal ltlContentGroupName;

        public Repeater rptContents;
        public SqlPager spContents;

        private string tableName;
        private NodeInfo nodeInfo;
        private string contentGroupName;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            int publishmentSystemID = base.GetIntQueryString("publishmentSystemID");
            this.contentGroupName = base.GetQueryString("contentGroupName");
            this.nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, publishmentSystemID);
            this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeInfo);

            if (base.GetQueryString("Remove") != null)
            {
                string groupName = base.GetQueryString("ContentGroupName");
                int contentID = base.GetIntQueryString("ContentID");
                try
                {
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(ETableStyle.BackgroundContent, this.tableName, contentID);
                    ArrayList groupList = TranslateUtils.StringCollectionToArrayList(contentInfo.ContentGroupNameCollection);
                    if (groupList.Contains(groupName))
                        groupList.Remove(groupName);

                    contentInfo.ContentGroupNameCollection = TranslateUtils.ObjectCollectionToString(groupList);
                    DataProvider.ContentDAO.Update(this.tableName, base.PublishmentSystemInfo, contentInfo);
                    StringUtility.AddLog(base.PublishmentSystemID, "移除内容", string.Format("内容:{0}", contentInfo.Title));
                    base.SuccessMessage("移除成功");
                    base.AddWaitAndRedirectScript(this.PageUrl);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "移除失败");
                }
            }



            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            this.spContents.SelectCommand = DataProvider.ContentDAO.GetSelectCommendByContentGroup(tableName, contentGroupName, publishmentSystemID);
            this.spContents.SortField = string.Format("AddDate");
            this.spContents.SortMode = SortMode.DESC;



            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_Category, "查看内容组", AppManager.CMS.Permission.WebSite.Category);
                this.ltlContentGroupName.Text = "内容组：" + base.GetQueryString("ContentGroupName");
                this.spContents.DataBind();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal ltlItemTitle = e.Item.FindControl("ltlItemTitle") as Literal;
                Literal ltlItemChannel = e.Item.FindControl("ltlItemChannel") as Literal;
                Literal ltlItemAuthor = e.Item.FindControl("ltlItemAuthor") as Literal;
                Literal ltlItemAddDate = e.Item.FindControl("ltlItemAddDate") as Literal;
                Literal ltlItemStatus = e.Item.FindControl("ltlItemStatus") as Literal;
                Literal ltlItemEditUrl = e.Item.FindControl("ltlItemEditUrl") as Literal;
                Literal ltlItemDeleteUrl = e.Item.FindControl("ltlItemDeleteUrl") as Literal;

                ContentInfo contentInfo = new ContentInfo(e.Item.DataItem);

                ltlItemTitle.Text = WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, this.PageUrl);
                ltlItemChannel.Text = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, contentInfo.NodeID);
                ltlItemAuthor.Text = AdminManager.GetDisplayName(contentInfo.AddUserName, true);
                ltlItemAddDate.Text = DateUtils.GetDateAndTimeString(contentInfo.AddDate);
                ltlItemStatus.Text = LevelManager.GetCheckState(base.PublishmentSystemInfo, contentInfo.IsChecked, contentInfo.CheckedLevel);

                if (base.HasChannelPermissions(contentInfo.NodeID, AppManager.CMS.Permission.Channel.ContentEdit) || AdminManager.Current.UserName == contentInfo.AddUserName)
                {
                    //编辑
                    ltlItemEditUrl.Text = string.Format("<a href=\"{0}\">编辑</a>", WebUtils.GetContentAddEditUrl(base.PublishmentSystemID, this.nodeInfo, contentInfo.ID, this.PageUrl));

                    //移除
                    ltlItemDeleteUrl.Text = this.GetRemoveHtml(this.contentGroupName, contentInfo.ID);
                }
            }
        }

        public string GetRemoveHtml(string groupName, int contentID)
        {
            string urlGroup = PageUtils.GetCMSUrl(string.Format("background_contentsGroup.aspx?PublishmentSystemID={0}&Remove=True&contentGroupName={1}&ContentID={2}", base.PublishmentSystemID, groupName, contentID));
            return string.Format("<a href=\"{0}\" onClick=\"javascript:return confirm('此操作将从内容组“{1}”移除该内容，确认吗？');\">移除</a>", urlGroup, groupName, contentID);
        }

        private string _pageUrl;
        private string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetCMSUrl(string.Format("background_contentsGroup.aspx?publishmentSystemID={0}&contentGroupName={1}", base.PublishmentSystemID, base.GetQueryString("contentGroupName")));
                }
                return _pageUrl;
            }
        }

        public void Return_OnClick(object sender, EventArgs e)
        {
            PageUtils.Redirect(BackgroundContentGroup.GetRedirectUrl(base.PublishmentSystemID));
        }
    }
}
