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
    public class BackgroundCreateContent : BackgroundBasePage
    {
        public ListBox NodeIDList;
        public DropDownList ChooseScope;
        public Button DeleteAllContentButton;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Create, "生成内容页", AppManager.CMS.Permission.WebSite.Create);

                ListItem listitem = new ListItem("所有选中的栏目", "All");
                this.ChooseScope.Items.Add(listitem);
                listitem = new ListItem("一个月内更新的内容", "Month");
                this.ChooseScope.Items.Add(listitem);
                listitem = new ListItem("一天内更新的内容", "Day");
                this.ChooseScope.Items.Add(listitem);
                listitem = new ListItem("2小时内更新的内容", "2Hour");
                this.ChooseScope.Items.Add(listitem);

                NodeManager.AddListItems(this.NodeIDList.Items, base.PublishmentSystemInfo, false, true);
                this.DeleteAllContentButton.Attributes.Add("onclick", "return confirm(\"此操作将删除所有已生成的内容页面，确定吗？\");");
            }
        }


        public void CreateContentButton_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                ArrayList nodeIDArrayList = new ArrayList();
                ArrayList selectedNodeIDArrayList = ControlUtils.GetSelectedListControlValueArrayList(this.NodeIDList);

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
                ProcessCreateContent(nodeIDArrayList);
            }
        }

        public void DeleteAllContentButton_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                string url = PageUtils.GetSTLUrl(string.Format("background_progressBar.aspx?PublishmentSystemID={0}&DeleteAllPage=True&TemplateType={1}", base.PublishmentSystemID, ETemplateTypeUtils.GetValue(ETemplateType.ContentTemplate)));
                PageUtils.RedirectToLoadingPage(url);
            }
        }

        private void ProcessCreateContent(ICollection nodeIDArrayList)
        {
            if (nodeIDArrayList.Count == 0)
            {
                base.FailMessage("请首先选中希望生成内容页面的栏目！");
                return;
            }
            string userKeyPrefix = StringUtils.GUID();
            if (!ConfigManager.Instance.Additional.IsSiteServerServiceCreate)
            {
                DbCacheManager.Insert(userKeyPrefix + Constants.CACHE_CREATE_CONTENTS_NODE_ID_ARRAYLIST, TranslateUtils.ObjectCollectionToString(nodeIDArrayList));
                string url = PageUtils.GetSTLUrl(string.Format("background_progressBar.aspx?PublishmentSystemID={0}&CreateContents=True&UserKeyPrefix={1}", base.PublishmentSystemID, userKeyPrefix));
                PageUtils.Redirect(url);
            }
            else
            {
                #region 添加到服务组件生成队列中，同时添加一个任务记录
                int totalCount = 0;
                Hashtable nodeIDWithContentIDArrayListMap = new Hashtable();
                foreach (int nodeID in nodeIDArrayList)
                {
                    string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
                    string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);
                    ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeID, orderByString);
                    nodeIDWithContentIDArrayListMap.Add(nodeID, contentIDArrayList);
                    totalCount += contentIDArrayList.Count;
                }
                //生成页面任务
                CreateTaskInfo createTaskInfo = new CreateTaskInfo();
                createTaskInfo.UserName = AdminManager.Current.UserName;
                createTaskInfo.TotalCount = totalCount;
                createTaskInfo.State = ECreateTaskType.Queuing;
                createTaskInfo.Summary = string.Format(@"生成内容<br />站点：{0}", PublishmentSystemInfo.PublishmentSystemName);
                if (createTaskInfo.TotalCount == 0)
                {
                    string url = PageUtils.GetSTLUrl(string.Format("background_progressBar.aspx?PublishmentSystemID={0}&CreateContents=True&UserKeyPrefix={1}", base.PublishmentSystemID, userKeyPrefix));
                    PageUtils.Redirect(url);
                }
                else
                {
                    int createTaskID = BaiRongDataProvider.CreateTaskDAO.Insert(createTaskInfo);
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        ArrayList contentIDArrayList = nodeIDWithContentIDArrayListMap[nodeID] as ArrayList;
                        foreach (int contentID in contentIDArrayList)
                        {
                            AjaxUrlManager.AddQueueCreateUrl(base.PublishmentSystemID, nodeID, contentID, 0, createTaskID);
                        }
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
