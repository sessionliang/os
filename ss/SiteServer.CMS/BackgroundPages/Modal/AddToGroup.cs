using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class AddToGroup : BackgroundBasePage
	{
		protected CheckBoxList cblGroupNameCollection;
        protected Button btnAddGroup;

        private bool isContent;
        private Hashtable idsHashtable = new Hashtable();
        private ArrayList nodeIDArrayList = new ArrayList();

        public static string GetOpenWindowStringToContentForMultiChannels(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("IsContent", "True");

            return PageUtility.GetOpenWindowStringWithCheckBoxValue("添加到内容组", "modal_addToGroup.aspx", arguments, "IDsCollection", "请选择需要设置组别的内容！", 450, 420);
        }

        public static string GetOpenWindowStringToContent(int publishmentSystemID, int nodeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("IsContent", "True");
            return PageUtility.GetOpenWindowStringWithCheckBoxValue("添加到内容组", "modal_addToGroup.aspx", arguments, "ContentIDCollection", "请选择需要设置组别的内容！", 450, 420);
        }

        public static string GetOpenWindowStringToChannel(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("IsContent", "False");
            return PageUtility.GetOpenWindowStringWithCheckBoxValue("添加到栏目组", "modal_addToGroup.aspx", arguments, "ChannelIDCollection", "请选择需要设置组别的栏目！", 450, 420);
        }
        
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (base.GetQueryString("IsContent") != null)
            {
                this.isContent = TranslateUtils.ToBool(base.GetQueryString("IsContent"));
            }
            if (this.isContent)
			{
                this.btnAddGroup.Text = " 新建内容组";
                this.idsHashtable = ContentUtility.GetIDsHashtable(base.Request.QueryString);
			}
			else
			{
                this.btnAddGroup.Text = " 新建栏目组";
                this.nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ChannelIDCollection"));
			}
			if (!IsPostBack)
			{
                if (this.isContent)
                {
                    ArrayList contentGroupNameArrayList = DataProvider.ContentGroupDAO.GetContentGroupNameArrayList(base.PublishmentSystemID);
                    foreach (string groupName in contentGroupNameArrayList)
                    {
                        ListItem item = new ListItem(groupName, groupName);
                        this.cblGroupNameCollection.Items.Add(item);
                    }
                    string showPopWinString = Modal.ContentGroupAdd.GetOpenWindowString(base.PublishmentSystemID);
                    this.btnAddGroup.Attributes.Add("onclick", showPopWinString);
                }
                else
                {
                    ArrayList nodeGroupNameArrayList = DataProvider.NodeGroupDAO.GetNodeGroupNameArrayList(base.PublishmentSystemID);
                    foreach (string groupName in nodeGroupNameArrayList)
                    {
                        ListItem item = new ListItem(groupName, groupName);
                        this.cblGroupNameCollection.Items.Add(item);
                    }

                    string showPopWinString = Modal.NodeGroupAdd.GetOpenWindowString(base.PublishmentSystemID);
                    this.btnAddGroup.Attributes.Add("onclick", showPopWinString);
                }
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			bool isChanged = false;
				
            try
            {
                if (this.isContent)
                {
                    ArrayList groupNameArrayList = new ArrayList();
                    foreach (ListItem item in this.cblGroupNameCollection.Items)
                    {
                        if (item.Selected) groupNameArrayList.Add(item.Value);
                    }

                    foreach (int nodeID in this.idsHashtable.Keys)
                    {
                        string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
                        ArrayList contentIDArrayList = this.idsHashtable[nodeID] as ArrayList;
                        foreach (int contentID in contentIDArrayList)
                        {
                            BaiRongDataProvider.ContentDAO.AddContentGroupArrayList(tableName, contentID, groupNameArrayList);
                        }
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, "添加内容到内容组", string.Format("内容组:{0}", TranslateUtils.ObjectCollectionToString(groupNameArrayList)));

                    isChanged = true;
                }
                else
                {

                    ArrayList groupNameArrayList = new ArrayList();
                    foreach (ListItem item in this.cblGroupNameCollection.Items)
                    {
                        if (item.Selected) groupNameArrayList.Add(item.Value);
                    }

                    foreach (int nodeID in this.nodeIDArrayList)
                    {
                        DataProvider.NodeDAO.AddNodeGroupArrayList(base.PublishmentSystemID, nodeID, groupNameArrayList);
                    }

                    StringUtility.AddLog(base.PublishmentSystemID, "添加栏目到栏目组", string.Format("栏目组:{0}", TranslateUtils.ObjectCollectionToString(groupNameArrayList)));

                    isChanged = true;
                }
            }
			catch(Exception ex)
			{
                base.FailMessage(ex, ex.Message);
			    isChanged = false;
			}

			if (isChanged)
			{
                JsUtils.OpenWindow.CloseModalPage(Page);
			}
		}
	}
}
