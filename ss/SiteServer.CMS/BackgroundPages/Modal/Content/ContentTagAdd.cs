using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class ContentTagAdd : BackgroundBasePage
	{
		protected TextBox tbTags;

        private string tagName;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());

            return PageUtility.GetOpenWindowString("添加标签", "modal_contentTagAdd.aspx", arguments, 380, 360);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, string tagName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("TagName", tagName);

            return PageUtility.GetOpenWindowString("修改标签", "modal_contentTagAdd.aspx", arguments, 380, 360);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.tagName = base.GetQueryString("TagName");

			if (!IsPostBack)
			{
                if (!string.IsNullOrEmpty(this.tagName))
                {
                    this.tbTags.Text = this.tagName;

                    int count = BaiRongDataProvider.TagDAO.GetTagCount(this.tagName, AppManager.CMS.AppID, base.PublishmentSystemID);

                    base.InfoMessage(string.Format(@"标签“<strong>{0}</strong>”被使用 {1} 次，编辑此标签将更新所有使用此标签的内容。", tagName, count));
                }
                else
                {
                    base.InfoMessage("在此可添加新标签");
                }
				
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
			if (!string.IsNullOrEmpty(this.tagName))
			{
				try
				{
                    if (!string.Equals(this.tagName, this.tbTags.Text))
                    {
                        StringCollection tagCollection = TagUtils.ParseTagsString(this.tbTags.Text);
                        ArrayList contentIDArrayList = BaiRongDataProvider.TagDAO.GetContentIDArrayListByTag(this.tagName, AppManager.CMS.AppID, base.PublishmentSystemID);
                        if (contentIDArrayList.Count > 0)
                        {
                            foreach (int contentID in contentIDArrayList)
                            {
                                if (!tagCollection.Contains(this.tagName))//删除
                                {
                                    TagInfo tagInfo = BaiRongDataProvider.TagDAO.GetTagInfo(AppManager.CMS.AppID, base.PublishmentSystemID, this.tagName);
                                    if (tagInfo != null)
                                    {
                                        ArrayList idArrayList = TranslateUtils.StringCollectionToIntArrayList(tagInfo.ContentIDCollection);
                                        idArrayList.Remove(contentID);
                                        tagInfo.ContentIDCollection = TranslateUtils.ObjectCollectionToString(idArrayList);
                                        tagInfo.UseNum = idArrayList.Count;
                                        BaiRongDataProvider.TagDAO.Update(tagInfo);
                                    }
                                }

                                TagUtils.AddTags(tagCollection, AppManager.CMS.AppID, base.PublishmentSystemID, contentID);

                                string contentTags = BaiRongDataProvider.ContentDAO.GetValue(base.PublishmentSystemInfo.AuxiliaryTableForContent, contentID, ContentAttribute.Tags);
                                ArrayList contentTagArrayList = TranslateUtils.StringCollectionToArrayList(contentTags);
                                contentTagArrayList.Remove(this.tagName);
                                foreach (string theTag in tagCollection)
                                {
                                    if (!contentTagArrayList.Contains(theTag))
                                    {
                                        contentTagArrayList.Add(theTag);
                                    }
                                }
                                BaiRongDataProvider.ContentDAO.SetValue(base.PublishmentSystemInfo.AuxiliaryTableForContent, contentID, ContentAttribute.Tags, TranslateUtils.ObjectCollectionToString(contentTagArrayList));
                            }
                        }
                        else
                        {
                            BaiRongDataProvider.TagDAO.DeleteTag(this.tagName, AppManager.CMS.AppID, base.PublishmentSystemID);
                        }
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, "修改内容标签", string.Format("内容标签:{0}", this.tbTags.Text));

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "标签修改失败！");
				}
			}
			else
			{
                try
                {
                    StringCollection tagCollection = TagUtils.ParseTagsString(this.tbTags.Text);
                    TagUtils.AddTags(tagCollection, AppManager.CMS.AppID, base.PublishmentSystemID, 0);
                    StringUtility.AddLog(base.PublishmentSystemID, "添加内容标签", string.Format("内容标签:{0}", this.tbTags.Text));
                    isChanged = true;
                }
                catch(Exception ex)
                {
                    base.FailMessage(ex, "标签添加失败！");
                }
			}

			if (isChanged)
			{
				JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
