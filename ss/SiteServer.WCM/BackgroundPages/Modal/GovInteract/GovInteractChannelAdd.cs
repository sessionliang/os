using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WCM.Core;

namespace SiteServer.WCM.BackgroundPages.Modal
{
	public class GovInteractChannelAdd : BackgroundBasePage
	{
        public TextBox ChannelName;
        public PlaceHolder phParentID;
        public DropDownList ParentID;
        public TextBox Summary;

        private int channelID = 0;
        private string returnUrl = string.Empty;
        private bool[] isLastNodeArray;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtilityWCM.GetOpenWindowString("添加节点", "modal_govInteractChannelAdd.aspx", arguments, 460, 300);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int channelID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ChannelID", channelID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtilityWCM.GetOpenWindowString("修改节点", "modal_govInteractChannelAdd.aspx", arguments, 460, 300);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.channelID = TranslateUtils.ToInt(Request.QueryString["ChannelID"]);
            this.returnUrl = StringUtils.ValueFromUrl(base.Request.QueryString["ReturnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = PageUtils.GetWCMUrl(string.Format("background_govInteractChannel.aspx?PublishmentSystemID={0}", base.PublishmentSystemID));
            }

			if (!IsPostBack)
			{
                if (this.channelID == 0)
                {
                    this.ParentID.Items.Add(new ListItem("<无上级节点>", "0"));

                    ArrayList channelIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(base.PublishmentSystemID, base.PublishmentSystemInfo.Additional.GovInteractNodeID);
                    int count = channelIDArrayList.Count;
                    this.isLastNodeArray = new bool[count + 2];
                    foreach (int theChannelID in channelIDArrayList)
                    {
                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, theChannelID);
                        ListItem listitem = new ListItem(this.GetTitle(nodeInfo), theChannelID.ToString());
                        this.ParentID.Items.Add(listitem);
                    }
                }
                else
                {
                    this.phParentID.Visible = false;
                }

                if (this.channelID != 0)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.channelID);

                    this.ChannelName.Text = nodeInfo.NodeName;
                    this.ParentID.SelectedValue = nodeInfo.ParentID.ToString();
                    GovInteractChannelInfo channelInfo = DataProvider.GovInteractChannelDAO.GetChannelInfo(base.PublishmentSystemID, this.channelID);
                    if (channelInfo != null)
                    {
                        this.Summary.Text = channelInfo.Summary;
                    }
                }

				
			}
		}

        public string GetTitle(NodeInfo nodeInfo)
        {
            string str = "";
            if (nodeInfo.IsLastNode == false)
            {
                this.isLastNodeArray[nodeInfo.ParentsCount] = false;
            }
            else
            {
                this.isLastNodeArray[nodeInfo.ParentsCount] = true;
            }
            for (int i = 0; i < nodeInfo.ParentsCount; i++)
            {
                if (this.isLastNodeArray[i])
                {
                    str = String.Concat(str, "　");
                }
                else
                {
                    str = String.Concat(str, "│");
                }
            }
            if (nodeInfo.IsLastNode)
            {
                str = String.Concat(str, "└");
            }
            else
            {
                str = String.Concat(str, "├");
            }
            str = String.Concat(str, nodeInfo.NodeName);
            return str;
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                if (this.channelID == 0)
                {
                    int parentID = TranslateUtils.ToInt(this.ParentID.SelectedValue);
                    if (parentID == 0)
                    {
                        parentID = base.PublishmentSystemInfo.Additional.GovInteractNodeID;
                    }
                    int nodeID = DataProvider.NodeDAO.InsertNodeInfo(base.PublishmentSystemID, parentID, this.ChannelName.Text, string.Empty, EContentModelTypeUtils.GetValue(EContentModelType.GovInteract));

                    GovInteractChannelInfo channelInfo = new GovInteractChannelInfo(nodeID, base.PublishmentSystemID, 0, 0, string.Empty, this.Summary.Text);

                    DataProvider.GovInteractChannelDAO.Insert(channelInfo);
                }
                else
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.channelID);
                    nodeInfo.NodeName = this.ChannelName.Text;
                    DataProvider.NodeDAO.UpdateNodeInfo(nodeInfo);

                    GovInteractChannelInfo channelInfo = DataProvider.GovInteractChannelDAO.GetChannelInfo(base.PublishmentSystemID, this.channelID);
                    if (channelInfo == null)
                    {
                        channelInfo = new GovInteractChannelInfo(this.channelID, base.PublishmentSystemID, 0, 0, string.Empty, this.Summary.Text);
                        DataProvider.GovInteractChannelDAO.Insert(channelInfo);
                    }
                    else
                    {
                        channelInfo.Summary = this.Summary.Text;
                        DataProvider.GovInteractChannelDAO.Update(channelInfo);
                    }
                }

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "维护分类信息");

                base.SuccessMessage("分类设置成功！");
                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "分类设置失败！");
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
            }
        }
	}
}
