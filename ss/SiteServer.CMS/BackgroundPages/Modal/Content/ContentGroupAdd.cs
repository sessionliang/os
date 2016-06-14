using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class ContentGroupAdd : BackgroundBasePage
	{
		protected TextBox ContentGroupName;
		protected TextBox Description;

        public static string GetOpenWindowString(int publishmentSystemID, string groupName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("GroupName", groupName);
            return PageUtility.GetOpenWindowString("修改内容组", "modal_contentGroupAdd.aspx", arguments, 400, 280);
        }

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("添加内容组", "modal_contentGroupAdd.aspx", arguments, 400, 280);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
				if (base.GetQueryString("GroupName") != null)
				{
                    string groupName = base.GetQueryString("GroupName");
                    ContentGroupInfo contentGroupInfo = DataProvider.ContentGroupDAO.GetContentGroupInfo(groupName, base.PublishmentSystemID);
					if (contentGroupInfo != null)
					{
						ContentGroupName.Text = contentGroupInfo.ContentGroupName;
						ContentGroupName.Enabled = false;
						Description.Text = contentGroupInfo.Description;
					}
				}
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

			ContentGroupInfo contentGroupInfo = new ContentGroupInfo();
			contentGroupInfo.ContentGroupName =PageUtils.FilterXSS(ContentGroupName.Text);
            contentGroupInfo.PublishmentSystemID = base.PublishmentSystemID;
			contentGroupInfo.Description = Description.Text;

            if (base.GetQueryString("GroupName") != null)
			{
				try
				{
                    DataProvider.ContentGroupDAO.Update(contentGroupInfo);
                    StringUtility.AddLog(base.PublishmentSystemID, "修改内容组", string.Format("内容组:{0}", contentGroupInfo.ContentGroupName));
					isChanged = true;
				}
                catch (Exception ex)
                {
                    base.FailMessage(ex, "内容组修改失败！");
				}
			}
			else
			{
                ArrayList ContentGroupNameArrayList = DataProvider.ContentGroupDAO.GetContentGroupNameArrayList(base.PublishmentSystemID);
				if (ContentGroupNameArrayList.IndexOf(ContentGroupName.Text) != -1)
				{
                    base.FailMessage("内容组添加失败，内容组名称已存在！");
				}
				else
				{
					try
					{
                        DataProvider.ContentGroupDAO.Insert(contentGroupInfo);
                        StringUtility.AddLog(base.PublishmentSystemID, "添加内容组", string.Format("内容组:{0}", contentGroupInfo.ContentGroupName));
						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "内容组添加失败！");
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
