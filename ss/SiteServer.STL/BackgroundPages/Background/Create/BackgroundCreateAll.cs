using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

using BaiRong.Core.Data.Provider;
using SiteServer.CMS.BackgroundPages;
using BaiRong.Model;

namespace SiteServer.STL.BackgroundPages
{
    public class BackgroundCreateAll : BackgroundBasePage
    {
        public ListBox NodeIDCollectionToCreate;
        public DropDownList ChooseScope;
        public Button DeleteAllNodeButton;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                ListItem listitem = new ListItem("所有选中的栏目", "All");
                this.ChooseScope.Items.Add(listitem);
                listitem = new ListItem("一个月内更新的栏目", "Month");
                this.ChooseScope.Items.Add(listitem);
                listitem = new ListItem("一天内更新的栏目", "Day");
                this.ChooseScope.Items.Add(listitem);
                listitem = new ListItem("2小时内更新的栏目", "2Hour");
                this.ChooseScope.Items.Add(listitem);

                NodeManager.AddListItems(this.NodeIDCollectionToCreate.Items, base.PublishmentSystemInfo, false, true);
                this.DeleteAllNodeButton.Attributes.Add("onclick", "return confirm(\"此操作将删除所有已生成的栏目页面，确定吗？\");");
            }
        }


        public void CreateNodeButton_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                ArrayList nodeIDArrayList = new ArrayList();
                ArrayList selectedNodeIDArrayList = ControlUtils.GetSelectedListControlValueArrayList(this.NodeIDCollectionToCreate);
                string tableName = base.PublishmentSystemInfo.AuxiliaryTableForContent;

                if (this.ChooseScope.SelectedValue == "Month")
                {
                    ArrayList lastEditArrayList = BaiRongDataProvider.ContentDAO.GetNodeIDArrayListCheckedByLastEditDateHour(tableName, base.PublishmentSystemID, 720);
                    foreach (int nodeID in lastEditArrayList)
                    {
                        if (selectedNodeIDArrayList.Contains(nodeID.ToString()))
                        {
                            nodeIDArrayList.Add(nodeID);
                        }
                    }
                }
                else if (this.ChooseScope.SelectedValue == "Day")
                {
                    ArrayList lastEditArrayList = BaiRongDataProvider.ContentDAO.GetNodeIDArrayListCheckedByLastEditDateHour(tableName, base.PublishmentSystemID, 24);
                    foreach (int nodeID in lastEditArrayList)
                    {
                        if (selectedNodeIDArrayList.Contains(nodeID.ToString()))
                        {
                            nodeIDArrayList.Add(nodeID);
                        }
                    }
                }
                else if (this.ChooseScope.SelectedValue == "2Hour")
                {
                    ArrayList lastEditArrayList = BaiRongDataProvider.ContentDAO.GetNodeIDArrayListCheckedByLastEditDateHour(tableName, base.PublishmentSystemID, 2);
                    foreach (int nodeID in lastEditArrayList)
                    {
                        if (selectedNodeIDArrayList.Contains(nodeID.ToString()))
                        {
                            nodeIDArrayList.Add(nodeID);
                        }
                    }
                }
                else
                {
                    nodeIDArrayList = selectedNodeIDArrayList;
                }
                ProcessCreateChannel(nodeIDArrayList);
            }
        }

        public void DeleteAllNodeButton_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                string url = PageUtils.GetSTLUrl(string.Format("background_progressBar.aspx?PublishmentSystemID={0}&DeleteAllPage=True&TemplateType={1}", base.PublishmentSystemID, ETemplateTypeUtils.GetValue(ETemplateType.ChannelTemplate)));
                PageUtils.RedirectToLoadingPage(url);
            }
        }

        private void ProcessCreateChannel(ICollection nodeIDArrayList)
        {
            if (nodeIDArrayList.Count == 0)
            {
                base.FailMessage("没有栏目需要生成页面！");
                return;
            }
            string userKeyPrefix = StringUtils.GUID();
            if (!ConfigManager.Instance.Additional.IsSiteServerServiceCreate)
            {

                DbCacheManager.Insert(userKeyPrefix + Constants.CACHE_CREATE_CHANNELS_NODE_ID_ARRAYLIST, TranslateUtils.ObjectCollectionToString(nodeIDArrayList));
                string url = PageUtils.GetSTLUrl(string.Format("background_progressBar.aspx?PublishmentSystemID={0}&CreateContentsByService=True&UserKeyPrefix={1}", base.PublishmentSystemID, userKeyPrefix));
                PageUtils.Redirect(url);
            }
            else
            {

            }
        }

    }
}
