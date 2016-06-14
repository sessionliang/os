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
	public class InnerLinkAdd : BackgroundBasePage
	{
		protected TextBox InnerLinkName;
		protected TextBox LinkUrl;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("添加站内链接", "modal_innerLinkAdd.aspx", arguments, 500, 280);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, string innerLinkName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("InnerLinkName", innerLinkName);
            return PageUtility.GetOpenWindowString("修改站内链接", "modal_innerLinkAdd.aspx", arguments, 500, 280);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{  
                if (base.GetQueryString("InnerLinkName") != null)
				{
                    string innerLinkName = base.GetQueryString("InnerLinkName");
                    InnerLinkInfo innerLinkInfo = DataProvider.InnerLinkDAO.GetInnerLinkInfo(innerLinkName, base.PublishmentSystemID);
                    if (innerLinkInfo != null)
					{
                        InnerLinkName.Text = innerLinkInfo.InnerLinkName;
                        LinkUrl.Text = innerLinkInfo.LinkUrl;
					}
				}
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

            InnerLinkInfo innerLinkInfo = new InnerLinkInfo();
            innerLinkInfo.InnerLinkName = this.InnerLinkName.Text;
            innerLinkInfo.PublishmentSystemID = base.PublishmentSystemID;
            innerLinkInfo.LinkUrl = this.LinkUrl.Text;

            if (base.GetQueryString("InnerLinkName") != null)
			{
				try
				{
                    if (base.GetQueryString("InnerLinkName") == innerLinkInfo.InnerLinkName)
                    {
                        DataProvider.InnerLinkDAO.Update(innerLinkInfo);
                        StringUtility.AddLog(base.PublishmentSystemID, "修改站内链接", string.Format("站内链接:{0}", innerLinkInfo.InnerLinkName));
                        isChanged = true;
                    }
                    else
                    {
                        ArrayList innerLinkNameArrayList = DataProvider.InnerLinkDAO.GetInnerLinkNameArrayList(base.PublishmentSystemID);
                        if (innerLinkNameArrayList.IndexOf(this.InnerLinkName.Text) != -1)
                        {
                            base.FailMessage("站内链接修改失败，站内链接名称已存在！");
                        }
                        else
                        {
                            DataProvider.InnerLinkDAO.Delete(base.GetQueryString("InnerLinkName"), base.PublishmentSystemID);
                            DataProvider.InnerLinkDAO.Insert(innerLinkInfo);
                            StringUtility.AddLog(base.PublishmentSystemID, "修改站内链接", string.Format("站内链接:{0}", innerLinkInfo.InnerLinkName));
                            isChanged = true;
                        }
                    }
					
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "站内链接修改失败！");
				}
			}
			else
			{
                ArrayList innerLinkNameArrayList = DataProvider.InnerLinkDAO.GetInnerLinkNameArrayList(base.PublishmentSystemID);
                if (innerLinkNameArrayList.IndexOf(this.InnerLinkName.Text) != -1)
				{
                    base.FailMessage("站内链接添加失败，站内链接名称已存在！");
				}
				else
				{
					try
					{
                        DataProvider.InnerLinkDAO.Insert(innerLinkInfo);
                        StringUtility.AddLog(base.PublishmentSystemID, "添加站内链接", string.Format("站内链接:{0}", innerLinkInfo.InnerLinkName));
						isChanged = true;
					}
					catch(Exception ex)
					{
                        base.FailMessage(ex, "站内链接添加失败！");
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
