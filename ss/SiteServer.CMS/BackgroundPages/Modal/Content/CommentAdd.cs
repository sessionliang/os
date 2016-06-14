using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;


namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class CommentAdd : BackgroundBasePage
	{
        private int channelID;
        private int contentID;
        private int commentID;

        public TextBox tbContent;   

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, int channelID, int contentID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ChannelID", channelID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            return PageUtility.GetOpenWindowString("添加评论", "modal_commentAdd.aspx", arguments, 600, 550);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int channelID, int contentID, int commentID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ChannelID", channelID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("CommentID", commentID.ToString());
            return PageUtility.GetOpenWindowString("修改评论", "modal_commentAdd.aspx", arguments, 600, 550);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.channelID = TranslateUtils.ToInt(base.GetQueryString("ChannelID"));
            this.contentID = TranslateUtils.ToInt(base.GetQueryString("ContentID"));
            this.commentID = TranslateUtils.ToInt(base.GetQueryString("CommentID"));

            if (!IsPostBack)
            {
                if (this.commentID != 0)
                {
                    CommentInfo commentInfo = DataProvider.CommentDAO.GetCommentInfo(this.commentID);
                    if (commentInfo != null)
                    {
                        this.tbContent.Text = commentInfo.Content;
                    }
                }
            }
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
			if (this.commentID != 0)
			{
                CommentInfo commentInfo = DataProvider.CommentDAO.GetCommentInfo(this.commentID);
                commentInfo.Content = this.tbContent.Text;

                if (string.IsNullOrEmpty(commentInfo.Content))
                {
                    base.FailMessage("评论修改失败，评论正文必须填写！");
                }
                else
                {
                    try
                    {
                        DataProvider.CommentDAO.Update(commentInfo, base.PublishmentSystemID);
                        StringUtility.AddLog(base.PublishmentSystemID, this.channelID, this.contentID, "修改评论", string.Empty);
                        isChanged = true;
                    }
                    catch(Exception ex)
                    {
                        base.FailMessage(ex, "评论修改失败！");
                    }
                }
			}
			else
			{
                CommentInfo commentInfo = new CommentInfo();

                commentInfo.Content = this.tbContent.Text;

                commentInfo.PublishmentSystemID = base.PublishmentSystemID;
                commentInfo.NodeID = this.channelID;
                commentInfo.ContentID = this.contentID;
                commentInfo.UserName = BaiRongDataProvider.UserDAO.CurrentUserName;
                commentInfo.IsChecked = true;
                commentInfo.AddDate = DateTime.Now;
                commentInfo.IPAddress = PageUtils.GetIPAddress();
                commentInfo.AdminName = AdminManager.Current.UserName;

                if (string.IsNullOrEmpty(commentInfo.Content))
                {
                    base.FailMessage("评论添加失败，评论正文必须填写！");
                }
                else
                {
                    try
                    {
                        DataProvider.CommentDAO.Insert(base.PublishmentSystemID, commentInfo);
                        StringUtility.AddLog(base.PublishmentSystemID, this.channelID, this.contentID, "添加评论", string.Empty);
                        isChanged = true;
                    }
                    catch(Exception ex)
                    {
                        base.FailMessage(ex, "评论添加失败！");
                    }
                }
			}

			if (isChanged)
			{
				JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
