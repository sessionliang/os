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
	public class NodeGroupAdd : BackgroundBasePage
	{
		protected TextBox tbNodeGroupName;
		protected TextBox tbDescription;

        public static string GetOpenWindowString(int publishmentSystemID, string groupName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("GroupName", groupName);
            return PageUtility.GetOpenWindowString("修改栏目组", "modal_nodeGroupAdd.aspx", arguments, 400, 280);
        }

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("添加栏目组", "modal_nodeGroupAdd.aspx", arguments, 400, 280);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
				if (base.GetQueryString("GroupName") != null)
				{
                    string groupName = base.GetQueryString("GroupName");
                    NodeGroupInfo nodeGroupInfo = DataProvider.NodeGroupDAO.GetNodeGroupInfo(base.PublishmentSystemID, groupName);
					if (nodeGroupInfo != null)
					{
                        this.tbNodeGroupName.Text = nodeGroupInfo.NodeGroupName;
                        this.tbNodeGroupName.Enabled = false;
						this.tbDescription.Text = nodeGroupInfo.Description;
					}
				}
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;

			NodeGroupInfo nodeGroupInfo = new NodeGroupInfo();
            nodeGroupInfo.NodeGroupName = this.tbNodeGroupName.Text;
            nodeGroupInfo.PublishmentSystemID = base.PublishmentSystemID;
            nodeGroupInfo.Description = this.tbDescription.Text;

            if (base.GetQueryString("GroupName") != null)
			{
				try
				{
                    DataProvider.NodeGroupDAO.Update(nodeGroupInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改栏目组", string.Format("栏目组:{0}", nodeGroupInfo.NodeGroupName));

					isChanged = true;
				}
				catch(Exception ex)
				{
                    base.FailMessage(ex, "栏目组修改失败！");
				}
			}
			else
			{
                ArrayList NodeGroupNameArrayList = DataProvider.NodeGroupDAO.GetNodeGroupNameArrayList(base.PublishmentSystemID);
				if (NodeGroupNameArrayList.IndexOf(this.tbNodeGroupName.Text) != -1)
				{
                    base.FailMessage("栏目组添加失败，栏目组名称已存在！");
				}
				else
				{
					try
					{
						DataProvider.NodeGroupDAO.Insert(nodeGroupInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, "添加栏目组", string.Format("栏目组:{0}", nodeGroupInfo.NodeGroupName));

						isChanged = true;
					}
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "栏目组添加失败！");
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
