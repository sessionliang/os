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
    public class BackgroundFilePathRule : BackgroundBasePage
    {
        public Literal ltlScript;
        public Repeater rptContents;

        public Literal ltlFilePathRuleForum;
        public Literal ltlEditUrl;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_filePathRule.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Settings, "页面命名规则", AppManager.BBS.Permission.BBS_Settings);

                this.ltlScript.Text = NodeTreeItem.GetScript(null);
                BindGrid();

                this.ltlFilePathRuleForum.Text = base.Additional.FilePathRule;
                ltlEditUrl.Text = string.Format(@"<a href=""javascript:void(0);"" onclick=""{0}"">更改路径</a>", Modal.FilePathRuleSet.GetOpenWindowString(base.PublishmentSystemID, 0));
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

        public void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int forumID = (int)e.Item.DataItem;

            ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, forumID);

            Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
            Literal ltlFilePath = e.Item.FindControl("ltlFilePath") as Literal;
            Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;

            NodeTreeItem nodeTreeItem = NodeTreeItem.CreateInstance(base.PublishmentSystemID, forumInfo, true);
            ltlTitle.Text = nodeTreeItem.GetItemHtml();
            ltlFilePath.Text = PageUtilityBBS.GetInputForumUrl(base.PublishmentSystemID, forumInfo);
            ltlEditUrl.Text = string.Format(@"<a href=""javascript:void(0);"" onclick=""{0}"">更改路径</a>", Modal.FilePathRuleSet.GetOpenWindowString(base.PublishmentSystemID, forumID));
        }
    }
}