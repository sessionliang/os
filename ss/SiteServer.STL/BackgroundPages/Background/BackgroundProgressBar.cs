using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections;
using System.Collections.Specialized;
using SiteServer.STL.IO;
using SiteServer.CMS.BackgroundPages;
using BaiRong.Model;
using SiteServer.STL.BackgroundPages.Service;

namespace SiteServer.STL.BackgroundPages
{
    public class BackgroundProgressBar : BackgroundBasePage
    {
        public Literal ltlTitle;
        public Literal RegisterScripts;

        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public static readonly string Cookie_NodeIDCollection = "SiteServer.CMS.BackgroundPages.BackgroundProgressBar.NodeIDCollection";

        public static readonly string Cookie_ContentIdentityCollection = "SiteServer.CMS.BackgroundPages.BackgroundProgressBar.ContentIdentityCollection";

        public static readonly string Cookie_TemplateIDCollection = "SiteServer.CMS.BackgroundPages.BackgroundProgressBar.TemplateIDCollection";

        public static string GetCreatePublishmentSystemUrl(int publishmentSystemID, bool isUseSiteTemplate, bool isImportContents, bool isImportTableStyles, string siteTemplateDir, bool isUseTables, string userKeyPrefix, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("CreatePublishmentSystem", "True");
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("IsUseSiteTemplate", isUseSiteTemplate.ToString());
            arguments.Add("IsImportContents", isImportContents.ToString());
            arguments.Add("IsImportTableStyles", isImportTableStyles.ToString());
            arguments.Add("SiteTemplateDir", siteTemplateDir);
            arguments.Add("isUseTables", isUseTables.ToString());
            arguments.Add("UserKeyPrefix", userKeyPrefix);
            arguments.Add("returnUrl", returnUrl);

            return PageUtils.AddQueryString(PageUtils.GetSTLUrl("background_progressBar.aspx"), arguments);
        }

        public static string GetCreatePublishmentSystemUrl(int publishmentSystemID, bool isUseSiteTemplate, bool isImportContents, bool isImportTableStyles, string siteTemplateDir, bool isUseTables, string userKeyPrefix, string returnUrl, bool isTop)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("CreatePublishmentSystem", "True");
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("IsUseSiteTemplate", isUseSiteTemplate.ToString());
            arguments.Add("IsImportContents", isImportContents.ToString());
            arguments.Add("IsImportTableStyles", isImportTableStyles.ToString());
            arguments.Add("SiteTemplateDir", siteTemplateDir);
            arguments.Add("isUseTables", isUseTables.ToString());
            arguments.Add("UserKeyPrefix", userKeyPrefix);
            arguments.Add("returnUrl", returnUrl);
            arguments.Add("isTop", isTop.ToString());

            return PageUtils.AddQueryString(PageUtils.GetSTLUrl("background_progressBar.aspx"), arguments);
        }

        public static string GetCreatePublishmentSystemUrl(int publishmentSystemID, bool isUseSiteTemplate, bool isImportContents, bool isImportTableStyles, string siteTemplateDir, bool isUseTables, string userKeyPrefix, string returnUrl, bool isTop, bool isCreateAll)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("CreatePublishmentSystem", "True");
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("IsUseSiteTemplate", isUseSiteTemplate.ToString());
            arguments.Add("IsImportContents", isImportContents.ToString());
            arguments.Add("IsImportTableStyles", isImportTableStyles.ToString());
            arguments.Add("SiteTemplateDir", siteTemplateDir);
            arguments.Add("isUseTables", isUseTables.ToString());
            arguments.Add("UserKeyPrefix", userKeyPrefix);
            arguments.Add("returnUrl", returnUrl);
            arguments.Add("isTop", isTop.ToString());
            arguments.Add("isCreateAll", isCreateAll.ToString());

            return PageUtils.AddQueryString(PageUtils.GetSTLUrl("background_progressBar.aspx"), arguments);
        }

        public static string GetBackupUrl(int publishmentSystemID, string backupType, string userKeyPrefix)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("Backup", "True");
            arguments.Add("BackupType", backupType);
            arguments.Add("UserKeyPrefix", userKeyPrefix);

            return PageUtils.AddQueryString(PageUtils.GetSTLUrl("background_progressBar.aspx"), arguments);
        }

        public static string GetRecoveryUrl(int publishmentSystemID, string isDeleteChannels, string isDeleteTemplates, string isDeleteFiles, bool isZip, string path, string isOverride, string isUseTable, string userKeyPrefix)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("Recovery", "True");
            arguments.Add("IsDeleteChannels", isDeleteChannels);
            arguments.Add("IsDeleteTemplates", isDeleteTemplates);
            arguments.Add("IsDeleteFiles", isDeleteFiles);
            arguments.Add("IsZip", isZip.ToString());
            arguments.Add("Path", path);
            arguments.Add("IsOverride", isOverride);
            arguments.Add("IsUseTable", isUseTable);
            arguments.Add("UserKeyPrefix", userKeyPrefix);

            return PageUtils.AddQueryString(PageUtils.GetSTLUrl("background_progressBar.aspx"), arguments);
        }

        public static string GetIndependentTemplateReplaceUrl(int publishmentSystemID, string directoryName, string userKeyPrefix)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("IndependentTemplateReplace", "True");
            arguments.Add("directoryName", directoryName);
            arguments.Add("UserKeyPrefix", userKeyPrefix);

            return PageUtils.AddQueryString(PageUtils.GetSTLUrl("background_progressBar.aspx"), arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            string userKeyPrefix = base.GetQueryString("UserKeyPrefix");

            if (base.GetQueryString("CreateChannels") != null && userKeyPrefix != null)
            {
                this.ltlTitle.Text = "生成栏目页";
                string pars = string.Format("publishmentSystemID={0}&userKeyPrefix={1}", base.PublishmentSystemID, userKeyPrefix);
                this.RegisterScripts.Text = JsManager.STLService.RegisterProgressTaskScript(JsManager.STLService.CreateServiceName, "CreateChannels", pars, userKeyPrefix);
            }
            else if (base.GetQueryString("CreateContents") != null && userKeyPrefix != null)
            {
                this.ltlTitle.Text = "生成内容页";
                string pars = string.Format("publishmentSystemID={0}&userKeyPrefix={1}", base.PublishmentSystemID, userKeyPrefix);
                this.RegisterScripts.Text = JsManager.STLService.RegisterProgressTaskScript(JsManager.STLService.CreateServiceName, "CreateContents", pars, userKeyPrefix);
            }
            else if (base.GetQueryString("CreateContentsByService") != null && userKeyPrefix != null)//服务组件生成
            {
                this.ltlTitle.Text = "生成内容页";
                string pars = string.Format("publishmentSystemID={0}&userKeyPrefix={1}&userName={2}&createTaskID={3}", base.PublishmentSystemID, userKeyPrefix, AdminManager.Current.UserName, base.GetQueryString("CreateTaskID"));
                this.RegisterScripts.Text = JsManager.STLService.RegisterProgressTaskScriptForService(JsManager.STLService.CreateServiceName, "CreateContentsByService", pars, userKeyPrefix);
            }
            else if (base.GetQueryString("CreateFiles") != null && userKeyPrefix != null)
            {
                this.ltlTitle.Text = "生成文件页";
                string pars = string.Format("publishmentSystemID={0}&userKeyPrefix={1}", base.PublishmentSystemID, userKeyPrefix);
                this.RegisterScripts.Text = JsManager.STLService.RegisterProgressTaskScript(JsManager.STLService.CreateServiceName, "CreateFiles", pars, userKeyPrefix);
            }
            else if (base.GetQueryString("CreatePublishmentSystem") != null && userKeyPrefix != null)
            {
                this.ltlTitle.Text = "新建应用";
                string pars = string.Format("publishmentSystemID={0}&isUseSiteTemplate={1}&isImportContents={2}&isImportTableStyles={3}&siteTemplateDir={4}&isUseTables={5}&userKeyPrefix={6}&returnUrl={7}&isTop={8}&isCreateAll={9}", base.PublishmentSystemID, base.GetQueryString("IsUseSiteTemplate"), base.GetQueryString("IsImportContents"), base.GetQueryString("IsImportTableStyles"), base.GetQueryString("SiteTemplateDir"), base.GetQueryString("IsUseTables"), userKeyPrefix, base.GetQueryString("returnUrl"), base.GetQueryString("isTop"), base.GetQueryString("isCreateAll"));
                this.RegisterScripts.Text = JsManager.STLService.RegisterProgressTaskScript(JsManager.STLService.CreateServiceName, "CreatePublishmentSystem", pars, userKeyPrefix, true);
            }
            else if (base.GetQueryString("Backup") != null && base.GetQueryString("BackupType") != null && userKeyPrefix != null)
            {
                this.ltlTitle.Text = "数据备份";

                string parameters = string.Format("publishmentSystemID={0}&backupTypeString={1}&userKeyPrefix={2}", base.PublishmentSystemID, base.GetQueryString("BackupType"), userKeyPrefix);
                this.RegisterScripts.Text = JsManager.STLService.RegisterWaitingTaskScript(JsManager.STLService.BackupServiceName, "Backup", parameters);
            }
            else if (base.GetQueryString("Recovery") != null && base.GetQueryString("IsZip") != null && userKeyPrefix != null)
            {
                if (base.PublishmentSystemInfo != null && base.PublishmentSystemInfo.PublishmentSystemType == BaiRong.Model.EPublishmentSystemType.UserCenter)
                {
                    this.ltlTitle.Text = "用户中心模板切换成功！";
                }
                else
                {
                    this.ltlTitle.Text = "数据恢复";
                }
                string parameters = string.Format("publishmentSystemID={0}&isDeleteChannels={1}&isDeleteTemplates={2}&isDeleteFiles={3}&isZip={4}&path={5}&isOverride={6}&isUseTable={7}&userKeyPrefix={8}", base.PublishmentSystemID, base.GetQueryString("IsDeleteChannels"), base.GetQueryString("IsDeleteTemplates"), base.GetQueryString("IsDeleteFiles"), base.GetQueryString("IsZip"), PageUtils.UrlEncode(base.GetQueryString("Path")), base.GetQueryString("IsOverride"), base.GetQueryString("IsUseTable"), userKeyPrefix);
                this.RegisterScripts.Text = JsManager.STLService.RegisterWaitingTaskScript(JsManager.STLService.BackupServiceName, "Recovery", parameters);
            }
            else if (base.GetQueryString("IndependentTemplateReplace") != null && base.GetQueryString("directoryName") != null && userKeyPrefix != null)
            {
                this.ltlTitle.Text = "导入模板";
                string parameters = string.Format("publishmentSystemID={0}&directoryName={1}&userKeyPrefix={2}", base.PublishmentSystemID, base.GetQueryString("directoryName"), userKeyPrefix);
                this.RegisterScripts.Text = JsManager.STLService.RegisterWaitingTaskScript(JsManager.STLService.BackupServiceName, "IndependentTemplateReplace", parameters);
            }
            else if (base.GetQueryString("DeleteAllPage") != null && base.GetQueryString("TemplateType") != null)
            {
                DeleteAllPage();
            }
            else if (base.GetQueryString("CreateIndex") != null || base.GetQueryString("isQCloud") != null)
            {
                CreateIndex();
            }
            else if (base.GetQueryString("CreateAll") != null)
            {
                if (!ConfigManager.Instance.Additional.IsSiteServerServiceCreate)
                {
                    this.ltlTitle.Text = "生成全部";
                    string pars = string.Format("publishmentSystemID={0}&userKeyPrefix={1}", base.PublishmentSystemID, userKeyPrefix);
                    this.RegisterScripts.Text = JsManager.STLService.RegisterProgressTaskScript(JsManager.STLService.CreateServiceName, "CreateAll", pars, userKeyPrefix);
                }
                else
                {
                    #region 设置生成页面总数

                    //首页
                    int totalCount = 1;

                    //栏目页数量
                    ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
                    totalCount += nodeIDArrayList.Count;

                    //内容页数量
                    Hashtable nodeIDWithContentIDArrayListMap = new Hashtable();
                    foreach (int nodeID in nodeIDArrayList)
                    {
                        string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
                        string orderByString = ETaxisTypeUtils.GetContentOrderByString(ETaxisType.OrderByTaxisDesc);
                        ArrayList contentIDArrayList = DataProvider.ContentDAO.GetContentIDArrayListChecked(tableName, nodeID, orderByString);
                        nodeIDWithContentIDArrayListMap.Add(nodeID, contentIDArrayList);
                        totalCount += contentIDArrayList.Count;
                    }

                    //文件页数量
                    ArrayList templateIDArrayList = DataProvider.TemplateDAO.GetTemplateIDArrayListByType(base.PublishmentSystemID, ETemplateType.FileTemplate);
                    //string directoryPath = PathUtility.MapPath(base.PublishmentSystemInfo, "@/include");
                    //DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);
                    //string[] fileNames = DirectoryUtils.GetFileNames(directoryPath);
                    //ArrayList includeFileArrayList = new ArrayList();
                    //foreach (string fileName in fileNames)
                    //{
                    //    if (!fileName.Contains("_parsed"))
                    //    {
                    //        includeFileArrayList.Add(fileName);
                    //    }
                    //}
                    totalCount += templateIDArrayList.Count;// + includeFileArrayList.Count;

                    #endregion

                    CreateTaskInfo createTaskInfo = new CreateTaskInfo();
                    createTaskInfo.UserName = AdminManager.Current.UserName;
                    createTaskInfo.TotalCount = totalCount;
                    createTaskInfo.State = ECreateTaskType.Queuing;
                    createTaskInfo.Summary = string.Format(@"一键生成<br />站点：{0}", PublishmentSystemInfo.PublishmentSystemName);
                    if (createTaskInfo.TotalCount == 0)
                    {
                        string url = PageUtils.GetSTLUrl(string.Format("background_progressBar.aspx?PublishmentSystemID={0}&CreateChannelsByService=True&UserKeyPrefix={1}&CreateTaskID={2}", base.PublishmentSystemID, userKeyPrefix, 0));
                        PageUtils.Redirect(url);
                    }
                    else
                    {
                        int createTaskID = BaiRongDataProvider.CreateTaskDAO.Insert(createTaskInfo);
                        #region 首页
                        AjaxUrlManager.AddQueueCreateUrl(base.PublishmentSystemID, 0, 0, 0, createTaskID);
                        #endregion

                        #region 栏目页
                        foreach (int nodeID in nodeIDArrayList)
                        {
                            AjaxUrlManager.AddQueueCreateUrl(base.PublishmentSystemID, nodeID, 0, 0, createTaskID);
                        }
                        #endregion

                        #region 内容页
                        foreach (int nodeID in nodeIDArrayList)
                        {
                            ArrayList contentIDArrayList = nodeIDWithContentIDArrayListMap[nodeID] as ArrayList;
                            if (contentIDArrayList != null)
                            {
                                foreach (int contentID in contentIDArrayList)
                                {
                                    AjaxUrlManager.AddQueueCreateUrl(base.PublishmentSystemID, nodeID, contentID, 0, createTaskID);
                                }
                            }
                        }
                        #endregion

                        #region 模板页
                        if (templateIDArrayList.Count > 0)
                        {

                            foreach (int templateIDToCreate in templateIDArrayList)
                            {
                                AjaxUrlManager.AddQueueCreateUrl(base.PublishmentSystemID, 0, 0, templateIDToCreate, createTaskID);
                            }
                        }
                        #endregion
                        AjaxUrlManager.OpenQueueCreateChange();

                        string url = PageUtils.GetSTLUrl(string.Format("background_progressBar.aspx?PublishmentSystemID={0}&CreateContentsByService=True&UserKeyPrefix={1}&CreateTaskID={2}", base.PublishmentSystemID, userKeyPrefix, createTaskID));
                        PageUtils.Redirect(url);
                    }
                }
            }
        }

        //生成首页
        private void CreateIndex()
        {
            this.ltlTitle.Text = "生成首页";
            HyperLink link = new HyperLink();
            link.NavigateUrl = PageUtility.GetIndexPageUrl(base.PublishmentSystemInfo, base.PublishmentSystemInfo.Additional.VisualType);
            link.Text = "浏览";
            if (link.NavigateUrl != PageUtils.UNCLICKED_URL)
            {
                link.Target = "_blank";
            }
            link.Style.Add("text-decoration", "underline");
            try
            {
                FileSystemObject FSO = new FileSystemObject(base.PublishmentSystemID);

                if (base.PublishmentSystemInfo.Additional.IsCreateRedirectPage)
                {
                    FSO.AddIndexToWaitingCreate();
                }
                else
                {
                    FSO.CreateIndex();
                }

                this.RegisterScripts.Text = @"
<script>
$(document).ready(function(){
    writeResult('首页生成成功。', '');
})
</script>
";
            }
            catch (Exception ex)
            {
                this.RegisterScripts.Text = string.Format(@"
<script>
$(document).ready(function(){{
    writeResult('', '{0}');
}})
</script>
", ex.Message);
            }
        }

        private void DeleteAllPage()
        {
            ETemplateType templateType = ETemplateTypeUtils.GetEnumType(base.GetQueryString("TemplateType"));

            if (templateType == ETemplateType.ChannelTemplate)
            {
                this.ltlTitle.Text = "删除已生成的栏目页文件";
            }
            else if (templateType == ETemplateType.ContentTemplate)
            {
                this.ltlTitle.Text = "删除所有已生成的内容页文件";
            }
            else if (templateType == ETemplateType.FileTemplate)
            {
                this.ltlTitle.Text = "删除所有已生成的文件页";
            }

            try
            {
                if (templateType == ETemplateType.ChannelTemplate)
                {
                    ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
                    DirectoryUtility.DeleteChannelsByPage(base.PublishmentSystemInfo, nodeIDArrayList);
                }
                else if (templateType == ETemplateType.ContentTemplate)
                {
                    ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
                    DirectoryUtility.DeleteContentsByPage(base.PublishmentSystemInfo, nodeIDArrayList);
                }
                else if (templateType == ETemplateType.FileTemplate)
                {
                    DirectoryUtility.DeleteFiles(base.PublishmentSystemInfo, DataProvider.TemplateDAO.GetTemplateIDArrayListByType(base.PublishmentSystemID, ETemplateType.FileTemplate));
                }

                StringUtility.AddLog(base.PublishmentSystemID, this.ltlTitle.Text);

                this.RegisterScripts.Text = @"
<script>
$(document).ready(function(){
    writeResult('任务执行成功。', '');
})
</script>
";
            }
            catch (Exception ex)
            {
                this.RegisterScripts.Text = string.Format(@"
<script>
$(document).ready(function(){{
    writeResult('', '{0}');
}})
</script>
", ex.Message);
            }
        }
    }
}
