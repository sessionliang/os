using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;

using SiteServer.BBS.Model;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundConfigurationHandle : BackgroundBasePage
    {
        public Literal ltlScript;
        public Repeater rptContents;

        private ArrayList forumIDArrayList;

        public static string GetRedirectUrl(int publishmentSystemID, int forumID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_configurationHandle.aspx?publishmentSystemID={0}&isHandle=True&forumID={1}", publishmentSystemID, forumID));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            this.forumIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Additional.NotHandleForumIDCollection);

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Settings, "新帖监控设置", AppManager.BBS.Permission.BBS_Settings);

                if (TranslateUtils.ToBool(base.GetQueryString("IsHandle")))
                {
                    int forumID = base.GetIntQueryString("ForumID");
                    if (forumID > 0)
                    {
                        ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, forumID);
                        if (forumIDArrayList.Contains(forumID))
                        {
                            forumIDArrayList.Remove(forumID);
                        }
                        else
                        {
                            forumIDArrayList.Add(forumID);
                        }

                        base.Additional.NotHandleForumIDCollection = TranslateUtils.ObjectCollectionToString(forumIDArrayList);

                        ConfigurationManager.Update(base.PublishmentSystemID);
                    }
                }
                this.ltlScript.Text = NodeTreeItem.GetScript(null);
                BindGrid();
            }
        }

        public void BindGrid()
        {

            this.rptContents.DataSource = DataProvider.ForumDAO.GetForumIDArrayList(base.PublishmentSystemID);
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.rptContents.DataBind();
        }

        public int GetTreeItemLevel(int forumID)
        {
            return ForumManager.GetForumInfo(base.PublishmentSystemID, forumID).ParentsCount + 1;
        }

        private void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int forumID = (int)e.Item.DataItem;

            ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, forumID);

            Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
            Literal ltlIsHandle = e.Item.FindControl("ltlIsHandle") as Literal;
            Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

            NodeTreeItem nodeTreeItem = NodeTreeItem.CreateInstance(base.PublishmentSystemID, forumInfo, true);
            ltlTitle.Text = nodeTreeItem.GetItemHtml();
            bool isHandle = !this.forumIDArrayList.Contains(forumID);
            ltlIsHandle.Text = StringUtilityBBS.GetTrueOrFalseImageHtml(isHandle);
            ltlEditUrl.Text = string.Format(@"<a href=""{0}"">更改</a>", BackgroundConfigurationHandle.GetRedirectUrl(base.PublishmentSystemID, forumID));
        }
    }
}