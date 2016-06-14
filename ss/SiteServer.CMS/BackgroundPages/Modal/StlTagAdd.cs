using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class StlTagAdd : BackgroundBasePage
	{
		protected TextBox TagName;
		protected TextBox Description;
        protected TextBox Content;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("����Զ���ģ���ǩ", "modal_stlTagAdd.aspx", arguments, 580, 520);
        }

        public static string GetOpenWindowString(int publishmentSystemID, string tagName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("TagName", tagName);
            return PageUtility.GetOpenWindowString("�޸��Զ���ģ���ǩ", "modal_stlTagAdd.aspx", arguments, 580, 520);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
				if (base.GetQueryString("TagName") != null)
				{
                    string tagName = base.GetQueryString("TagName");
                    StlTagInfo stlTagInfo = DataProvider.StlTagDAO.GetStlTagInfo(base.PublishmentSystemID, tagName);
                    if (stlTagInfo != null)
					{
                        this.TagName.Text = stlTagInfo.TagName;
                        this.TagName.Enabled = false;
                        this.Description.Text = stlTagInfo.TagDescription;
                        this.Content.Text = stlTagInfo.TagContent;
					}
				}
				
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

            StlTagInfo stlTagInfo = new StlTagInfo();
            stlTagInfo.TagName = this.TagName.Text;
            stlTagInfo.PublishmentSystemID = base.PublishmentSystemID;
            stlTagInfo.TagDescription = this.Description.Text;
            stlTagInfo.TagContent = this.Content.Text;

            if (base.GetQueryString("TagName") != null)
			{
				try
				{
                    DataProvider.StlTagDAO.Update(stlTagInfo);
                    StringUtility.AddLog(base.PublishmentSystemID, "�޸��Զ���ģ������", string.Format("ģ���ǩ��:{0}", stlTagInfo.TagName));
					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "�Զ���ģ���ǩ�޸�ʧ�ܣ�");
				}
			}
			else
			{
                ArrayList stlTagNameArrayList = DataProvider.StlTagDAO.GetStlTagNameArrayList(base.PublishmentSystemID);
                if (stlTagNameArrayList.IndexOf(this.TagName.Text) != -1)
				{
                    base.FailMessage("�Զ���ģ���ǩ���ʧ�ܣ��Զ���ģ���ǩ���Ѵ��ڣ�");
				}
				else
				{
					try
					{
                        DataProvider.StlTagDAO.Insert(stlTagInfo);
                        StringUtility.AddLog(base.PublishmentSystemID, "����Զ���ģ������", string.Format("ģ���ǩ��:{0}", stlTagInfo.TagName));
						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "�Զ���ģ���ǩ���ʧ�ܣ�");
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
