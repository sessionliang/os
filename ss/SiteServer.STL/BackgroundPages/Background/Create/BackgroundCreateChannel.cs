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
using SiteServer.STL.BackgroundPages.Service;

namespace SiteServer.STL.BackgroundPages
{
    public class BackgroundCreateChannel : BackgroundBasePage
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
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Create, "生成栏目页", AppManager.CMS.Permission.WebSite.Create);

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
                    nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(TranslateUtils.ObjectCollectionToString(selectedNodeIDArrayList));
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
                base.FailMessage("请首先选中希望生成页面的栏目！");
                return;
            }
            string userKeyPrefix = StringUtils.GUID();
            if (!ConfigManager.Instance.Additional.IsSiteServerServiceCreate)
            {
                DbCacheManager.Insert(userKeyPrefix + Constants.CACHE_CREATE_CHANNELS_NODE_ID_ARRAYLIST, TranslateUtils.ObjectCollectionToString(nodeIDArrayList));
                string url = PageUtils.GetSTLUrl(string.Format("background_progressBar.aspx?PublishmentSystemID={0}&CreateChannels=True&UserKeyPrefix={1}", base.PublishmentSystemID, userKeyPrefix));
                PageUtils.Redirect(url);
            }
            else
            {
                #region 添加到服务组件生成队列中，同时添加一个任务记录
                //生成页面任务
                CreateTaskInfo createTaskInfo = new CreateTaskInfo();
                createTaskInfo.UserName = AdminManager.Current.UserName;
                createTaskInfo.TotalCount = nodeIDArrayList.Count;
                createTaskInfo.State = ECreateTaskType.Queuing;
                createTaskInfo.Summary = string.Format(@"生成栏目<br />站点：{0}", PublishmentSystemInfo.PublishmentSystemName);
                if (createTaskInfo.TotalCount == 0)
                {
                    string url = PageUtils.GetSTLUrl(string.Format("background_progressBar.aspx?PublishmentSystemID={0}&CreateChannels=True&UserKeyPrefix={1}", base.PublishmentSystemID, userKeyPrefix));
                    PageUtils.Redirect(url);
                }
                else
                {
                    int createTaskID = BaiRongDataProvider.CreateTaskDAO.Insert(createTaskInfo);
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        AjaxUrlManager.AddQueueCreateUrl(base.PublishmentSystemID, nodeID, 0, 0, createTaskID);
                    }
                    AjaxUrlManager.OpenQueueCreateChange();
                    string url = PageUtils.GetSTLUrl(string.Format("background_progressBar.aspx?PublishmentSystemID={0}&CreateContentsByService=True&UserKeyPrefix={1}&CreateTaskID={2}", base.PublishmentSystemID, userKeyPrefix, createTaskID));
                    PageUtils.Redirect(url);
                }
                #endregion
            }
        }

    }
}
