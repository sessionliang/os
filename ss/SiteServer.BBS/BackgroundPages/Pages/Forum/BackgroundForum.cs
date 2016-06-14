using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;

using SiteServer.BBS.Model;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.BackgroundPages {

    public class BackgroundForum : BackgroundBasePage
    {
        public Repeater rptContents;
        public Button AddForum;
        public Button Translate;
        public Button Import;
        public Button Export;
        public Button Delete;

        public static string GetRedirectUrl(int publishmentSystemID)
        {
            return PageUtils.GetBBSUrl(string.Format("background_forum.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("ForumID") != null)
            { // 排序或删除某一版块
                int forumID = base.GetIntQueryString("ForumID");
                if (forumID > 0)
                {
                    if (base.GetQueryString("Subtract") != null)
                    { // 排序
                        string strSubtract = base.GetQueryString("Subtract").ToLower();
                        UpdateTaxis(strSubtract, forumID);
                    }
                    if (base.GetQueryString("Delete") != null)
                    {
                        DeleteForum(forumID);
                    }
                }
            }
            else if (base.GetQueryString("ForumIDCollection") != null)
            { // 操作多个版块
                string strForumIDCollection = base.GetQueryString("ForumIDCollection");
                ArrayList formIDList = TranslateUtils.StringCollectionToArrayList(strForumIDCollection, ',');
                if (base.GetQueryString("Delete") != null)
                {
                    DeleteForumByForumIDList(formIDList);
                }                    
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Forum, "版块管理", AppManager.BBS.Permission.BBS_Forum);

                if (!Page.IsStartupScriptRegistered("NodeTreeScript"))
                {
                    Page.RegisterClientScriptBlock("NodeTreeScript", NodeTreeItem.GetScript(null));
                }
                ButtonPreLoad();
                BindGrid();
            }
        }

        private void ButtonPreLoad()
        {
            NameValueCollection arguments = new NameValueCollection();

            string forumUrl = BackgroundForum.GetRedirectUrl(base.PublishmentSystemID);

            this.AddForum.Attributes.Add("onclick", Modal.ForumAdd.GetOpenWindowString(base.PublishmentSystemID, 0));
            this.Translate.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValue(BackgroundForumTranslate.GetRedirectUrl(base.PublishmentSystemID, forumUrl), "ForumIDCollection", "ForumIDCollection", "请选择需要合并的版块！"));
            this.Delete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(string.Format("{0}&Delete=True", forumUrl), "ForumIDCollection", "ForumIDCollection", "请选择需要删除的版块！", "确定要删除选中的版块吗？"));

            //this.Import.Attributes.Add("onclick", Modal.ChannelImport.GetOpenWindowString(base.PublishmentSystemID, base.PublishmentSystemID));
            //this.Export.Attributes.Add("onclick", Modal.ExportMessage.GetOpenWindowStringToChannel(base.PublishmentSystemID, "ChannelIDCollection", "请选择需要导出的栏目！"));
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

            Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
            Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;
            Literal ltlModerator = e.Item.FindControl("ltlModerator") as Literal;
            Literal ltlIndexName = e.Item.FindControl("ltlIndexName") as Literal;
            Literal ltlUpLink = e.Item.FindControl("ltlUpLink") as Literal;
            Literal ltlDownLink = e.Item.FindControl("ltlDownLink") as Literal;
            Literal ltlDeleteLink = e.Item.FindControl("ltlDeleteLink") as Literal;
            Literal ltlCheckBox = e.Item.FindControl("ltlCheckBox") as Literal;

            if (forumInfo.ParentID == 0)
            {
                ltlEditUrl.Text = string.Format("<a href=\"{0}\">编辑</a>", BackgroundForumClassEdit.GetRedirectUrl(base.PublishmentSystemID, forumInfo.ForumID));
            }
            else
            {
                ltlEditUrl.Text = string.Format("<a href=\"{0}\">编辑</a>", BackgroundForumEdit.GetRedirectUrl(base.PublishmentSystemID, forumInfo.ForumID));
            }            

            NodeTreeItem nodeTreeItem = NodeTreeItem.CreateInstance(base.PublishmentSystemID, forumInfo, true);
            ltlTitle.Text = nodeTreeItem.GetItemHtml();
            if (string.IsNullOrEmpty(forumInfo.Additional.Moderators))
            {
                ltlModerator.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">无 / 添加版主</a>", Modal.ModeratorAdd.GetOpenWindowString(base.PublishmentSystemID, forumID));
            }
            else
            {
                ltlModerator.Text = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">{1}</a>", Modal.ModeratorAdd.GetOpenWindowString(base.PublishmentSystemID, forumID), forumInfo.Additional.Moderators);
            }
            ltlIndexName.Text = forumInfo.IndexName;

            string forumUrl = BackgroundForum.GetRedirectUrl(base.PublishmentSystemID);

            ltlUpLink.Text = string.Format(@"<a href=""{0}&Subtract=True&ForumID={1}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", forumUrl, forumInfo.ForumID);
            ltlDownLink.Text = string.Format(@"<a href=""{0}&Subtract=False&ForumID={1}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", forumUrl, forumInfo.ForumID);
            ltlDeleteLink.Text = string.Format(@"<a href=""{0}&Delete=True&ForumID={1}"" onclick=""return confirm('此操作将删除所选板块及其子板块，确定吗？');"">删除</a>", forumUrl, forumInfo.ForumID);
            ltlCheckBox.Text = string.Format("<input type='checkbox' name='ForumIDCollection' value='{0}' />", forumInfo.ForumID);
        }

        private void UpdateTaxis(string subtract, int forumID)
        {

            if (!string.IsNullOrEmpty(subtract))
            {
                bool isSubtract = TranslateUtils.ToBool(subtract);
                DataProvider.ForumDAO.UpdateTaxis(base.PublishmentSystemID, forumID, isSubtract);
            }
        }

        private void DeleteForum(int forumID)
        {
            try
            {
                ArrayList forumIDArrayList = DataProvider.ForumDAO.GetForumIDArrayListForDescendant(base.PublishmentSystemID, forumID);
                foreach (int theForumID in forumIDArrayList)
                {
                    DataProvider.ForumDAO.Delete(base.PublishmentSystemID, theForumID);
                }
                DataProvider.ForumDAO.Delete(base.PublishmentSystemID, forumID);
                base.SuccessMessage("成功删除版块");
            }
            catch (Exception ex)
            {
                base.SuccessMessage(string.Format("删除版块失败，{0}", ex.Message));
            }
        }

        private void DeleteForumByForumIDList(ArrayList formIDList)
        {

            if (formIDList.Count > 0)
            {
                for (int i = 0; i < formIDList.Count; i++)
                {
                    int forumID = TranslateUtils.ToInt(formIDList[i].ToString());
                    ArrayList forumIDArrayList = DataProvider.ForumDAO.GetForumIDArrayListForDescendant(base.PublishmentSystemID, forumID);
                    foreach (int theForumID in forumIDArrayList)
                    {
                        DataProvider.ForumDAO.Delete(base.PublishmentSystemID, theForumID);
                    }
                    DataProvider.ForumDAO.Delete(base.PublishmentSystemID, forumID);
                }
                base.SuccessMessage("成功删除版块");
                base.AddWaitAndRedirectScript(BackgroundForum.GetRedirectUrl(base.PublishmentSystemID));
                return;
            }
        }
    }
}