using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Collections;
using SiteServer.BBS.Core;
using BaiRong.Core;
using System.Web.UI;

namespace SiteServer.BBS.BackgroundPages.Modal
{
    public class FilePathRuleSet : BackgroundBasePage
    {
        public Control FilePathRow;
        public TextBox FilePath;
        public TextBox FilePathRule;

        public Button btnCreateForumRule;

        private int forumID;

        public static string GetOpenWindowString(int publishmentSystemID, int forumID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ForumID", forumID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("页面命名规则", PageUtils.GetBBSUrl("modal_filePathRuleSet.aspx"), arguments, 580, 500);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.forumID = base.GetIntQueryString("forumID");

            if (!IsPostBack)
            {
                if (this.forumID == 0)
                {
                    this.FilePathRow.Visible = false;
                    this.FilePathRule.Text = base.Additional.FilePathRule;
                }
                else
                {
                    ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, this.forumID);

                    if (string.IsNullOrEmpty(forumInfo.FilePath))
                    {
                        this.FilePath.Text = PageUtilityBBS.GetInputForumUrl(base.PublishmentSystemID, forumInfo);
                    }
                    else
                    {
                        this.FilePath.Text = forumInfo.FilePath;
                    }

                    if (string.IsNullOrEmpty(forumInfo.FilePathRule))
                    {
                        this.FilePathRule.Text = PathUtilityBBS.GetFilePathRule(base.PublishmentSystemID, this.forumID);
                    }
                    else
                    {
                        this.FilePathRule.Text = forumInfo.FilePathRule;
                    }
                }

                string showPopWinString = Modal.FilePathRuleMake.GetOpenWindowString(base.PublishmentSystemID, this.FilePathRule.ClientID);
                this.btnCreateForumRule.Attributes.Add("onclick", showPopWinString);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isSuccess = false;

            try
            {
                if (!string.IsNullOrEmpty(this.FilePathRule.Text))
                {
                    if (!DirectoryUtils.IsDirectoryNameCompliant(this.FilePathRule.Text))
                    {
                        base.FailMessage("板块页面命名规则不符合系统要求！");
                        return;
                    }
                    if (PathUtils.IsDirectoryPath(this.FilePathRule.Text))
                    {
                        this.FilePathRule.Text = PageUtils.Combine(this.FilePathRule.Text, PathUtilityBBS.FilePathRulesForum.ForumID + ".aspx");
                    }
                    else if (!StringUtils.Contains(this.FilePathRule.Text.ToLower(), PathUtilityBBS.FilePathRulesForum.ForumID.ToLower()))
                    {
                        base.FailMessage(string.Format("板块页面命名规则必须包含<strong>{0}</strong>！", PathUtilityBBS.FilePathRulesForum.ForumID));
                        return;
                    }
                }

                if (this.forumID == 0)
                {
                    base.Additional.FilePathRule = this.FilePathRule.Text;

                    ConfigurationManager.Update(base.PublishmentSystemID);
                }
                else
                {
                    ForumInfo forumInfo = ForumManager.GetForumInfo(base.PublishmentSystemID, this.forumID);

                    string filePath = forumInfo.FilePath;

                    if (this.FilePathRow.Visible)
                    {
                        this.FilePath.Text = this.FilePath.Text.Trim();
                        if (!string.IsNullOrEmpty(this.FilePath.Text) && !StringUtils.EqualsIgnoreCase(filePath, FilePath.Text))
                        {
                            if (!DirectoryUtils.IsDirectoryNameCompliant(FilePath.Text))
                            {
                                base.FailMessage("板块页面路径不符合系统要求！");
                                return;
                            }

                            if (PathUtils.IsDirectoryPath(this.FilePath.Text))
                            {
                                this.FilePath.Text = PageUtils.Combine(this.FilePath.Text, "index.html");
                            }

                            ArrayList filePathArrayList = DataProvider.ForumDAO.GetAllFilePath(base.PublishmentSystemID);
                            if (filePathArrayList.IndexOf(this.FilePath.Text) != -1)
                            {
                                base.FailMessage("板块修改失败，板块页面路径已存在！");
                                return;
                            }
                        }
                    }

                    if (this.FilePath.Text != PageUtilityBBS.GetInputForumUrl(base.PublishmentSystemID, forumInfo))
                    {
                        forumInfo.FilePath = this.FilePath.Text;
                    }
                    if (this.FilePathRule.Text != PathUtilityBBS.GetFilePathRule(base.PublishmentSystemID, this.forumID))
                    {
                        forumInfo.FilePathRule = this.FilePathRule.Text;
                    }

                    DataProvider.ForumDAO.UpdateForumInfo(base.PublishmentSystemID, forumInfo);

                    //FileSystemObject FSO = new FileSystemObject(base.PublishmentSystemID);
                    //FSO.CreateImmediately(EChangedType.Edit, ETemplateType.ChannelTemplate, this.nodeID, 0, 0);

                    //StringUtilityBBS.AddLog(base.PublishmentSystemID, this.nodeID, 0, "设置页面命名规则", string.Format("板块:{0}", nodeInfo.NodeName));
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }

            if (isSuccess)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, BackgroundFilePathRule.GetRedirectUrl(base.PublishmentSystemID));
            }
        }
    }
}
