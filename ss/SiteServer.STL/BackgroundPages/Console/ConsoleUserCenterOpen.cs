using System;
using System.Collections;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

using SiteServer.STL.ImportExport;
using SiteServer.CMS.BackgroundPages;
using System.Collections.Generic;

namespace SiteServer.STL.BackgroundPages
{
    public class ConsoleUserCenterOpen : BackgroundBasePage
    {
        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public Literal ltlPageTitle;
        public DropDownList ddlSiteTemplate;
        public CheckBox UseSiteTemplate;

        public PlaceHolder CreateSiteParameters;
        public TextBox UserCenterName;
        public TextBox UserCenterDir;

        public PlaceHolder OperatingError;
        public Literal ltlErrorMessage;

        public Button Previous;
        public Button Next;

        private EPublishmentSystemType publishmentSystemType = EPublishmentSystemType.UserCenter;
        SortedList sortedlist = new SortedList();

        public static string GetRedirectUrl()
        {
            return PageUtils.GetSTLUrl(string.Format("console_userCenterOpen.aspx"));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            //this.UserCenterDir.Enabled = false;
            //模板集合
            this.sortedlist = SiteTemplateManager.Instance.GetSiteTemplateSortedList(this.publishmentSystemType);

            if (!IsPostBack)
            {
                BaiRongDataProvider.TableCollectionDAO.CreateAllAuxiliaryTableIfNotExists();

                string pageTitle = string.Format("开启{0}应用", EPublishmentSystemTypeUtils.GetText(this.publishmentSystemType));
                this.ltlPageTitle.Text = pageTitle;
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, pageTitle, AppManager.Platform.Permission.Platform_Site);

                BindGrid();

                this.UseSiteTemplate.Attributes.Add("onclick", "displaySiteTemplateDiv(this)");
            }
        }

        public void BindGrid()
        {
            try
            {
                ArrayList directoryArrayList = new ArrayList();
                Dictionary<string, string> directoryDic = new Dictionary<string, string>();
                foreach (string directoryName in this.sortedlist.Keys)
                {
                    SiteTemplateInfo tInfo = this.sortedlist[directoryName] as SiteTemplateInfo;
                    if (directoryDic.ContainsKey(tInfo.SiteTemplateName))
                    {
                        directoryDic.Add(PathUtils.GetFileName(directoryName), directoryName);
                    }
                    else
                    {
                        directoryDic.Add(PathUtils.GetFileName(directoryName), directoryName);
                    }
                }

                this.ddlSiteTemplate.DataSource = directoryDic;
                this.ddlSiteTemplate.DataValueField = "Value";
                this.ddlSiteTemplate.DataTextField = "Key";
                this.ddlSiteTemplate.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        public int Validate_PublishmentSystemInfo(out string errorMessage)
        {
            try
            {
                string userCenterDir = string.Empty;

                if (DirectoryUtils.IsSystemDirectory(UserCenterDir.Text))
                {
                    errorMessage = "文件夹名称不能为系统文件夹名称！";
                    return 0;
                }

                userCenterDir = this.UserCenterDir.Text;

                ArrayList arraylist = DataProvider.NodeDAO.GetLowerSystemDirArrayList(0);
                if (arraylist.IndexOf(userCenterDir.ToLower()) != -1)
                {
                    errorMessage = "已存在相同的发布路径！";
                    return 0;
                }

                if (!DirectoryUtils.IsDirectoryNameCompliant(userCenterDir))
                {
                    errorMessage = "文件夹名称不符合系统要求！";
                    return 0;
                }


                NodeInfo nodeInfo = new NodeInfo();

                nodeInfo.NodeName = nodeInfo.NodeIndexName = "首页";
                nodeInfo.NodeType = ENodeType.BackgroundPublishNode;
                nodeInfo.ContentModelID = EContentModelTypeUtils.GetValue(EContentModelTypeUtils.GetEnumTypeByPublishmentSystemType(this.publishmentSystemType));

                string userCenterUrl = PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, userCenterDir);

                string groupSN = GroupSNManager.GetCurrentGroupSN();

                #region 多个用户中心（已注释）
                ////只有创建用户中心的时候，才有GroupSN；其他应用创建，默认关联到一个用户中心或者通过下拉列表选择用户中心，同时也可以更改用户中心
                //if (EPublishmentSystemTypeUtils.Equals(EPublishmentSystemType.UserCenter, this.publishmentSystemType))
                //{
                //    //用户中心
                //    groupSN = GroupSNManager.GetCurrentGroupSN();
                //}
                //else
                //{
                //    //其他应用
                //    if (EPublishmentSystemTypeUtils.IsEnabled(this.publishmentSystemType))
                //    {
                //        //唯一用户中心
                //        PublishmentSystemInfo defaultUserCenter = PublishmentSystemManager.GetUniqueUserCenter();
                //        if (defaultUserCenter != null)
                //            groupSN = defaultUserCenter.GroupSN;
                //    }
                //} 
                #endregion

                string contentTableName = string.Empty;
                ArrayList tableCollection = BaiRong.Core.BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.BackgroundContent);
                if (tableCollection.Count > 0)
                    contentTableName = ((AuxiliaryTableInfo)tableCollection[0]).TableENName;
                else
                    contentTableName = EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.BackgroundContent);
                if (!BaiRong.Core.BaiRongDataProvider.TableCollectionDAO.IsTableExists(contentTableName))
                {
                    //判断在数据库是否已经创建
                    AuxiliaryTableInfo existsTableInfo = BaiRong.Core.BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableInfo(contentTableName);
                    if (existsTableInfo == null)
                    {
                        existsTableInfo = new AuxiliaryTableInfo();
                        existsTableInfo.TableENName = contentTableName;
                        existsTableInfo.TableCNName = "内容";
                        existsTableInfo.Description = "";
                        existsTableInfo.AuxiliaryTableType = EAuxiliaryTableType.BackgroundContent;
                        try
                        {
                            BaiRongDataProvider.TableCollectionDAO.Insert(existsTableInfo);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加辅助表", string.Format("辅助表:{0}", contentTableName));

                            base.SuccessMessage("辅助表添加成功！");
                        }
                        catch (Exception ex)
                        {
                            base.FailMessage(ex, "辅助表添加失败！");
                        }
                    }
                    if (existsTableInfo != null && !existsTableInfo.IsCreatedInDB)
                    {
                        BaiRongDataProvider.TableMetadataDAO.CreateAuxiliaryTable(contentTableName);
                    }
                }


                PublishmentSystemInfo psInfo = BaseTable.GetDefaultPublishmentSystemInfo(string.Format("{0}", PageUtils.FilterXSS(string.IsNullOrEmpty(this.UserCenterName.Text) ? "用户中心" : this.UserCenterName.Text)), this.publishmentSystemType, contentTableName, "", "", "", "", EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.VoteContent), EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.JobContent), userCenterDir, userCenterUrl, 0, groupSN);

                if (psInfo.ParentPublishmentSystemID > 0)
                {
                    PublishmentSystemInfo parentPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(psInfo.ParentPublishmentSystemID);
                    psInfo.PublishmentSystemUrl = PageUtils.Combine(parentPublishmentSystemInfo.PublishmentSystemUrl, psInfo.PublishmentSystemDir);
                }

                psInfo.IsHeadquarters = false;

                psInfo.Additional.Charset = ECharsetUtils.GetValue(ECharset.utf_8);
                psInfo.IsCheckContentUseLevel = false;

                int theUserCenterID = DataProvider.NodeDAO.InsertPublishmentSystemInfo(nodeInfo, psInfo);

                if (PermissionsManager.Current.IsSystemAdministrator && !PermissionsManager.Current.IsConsoleAdministrator)
                {
                    List<int> publishmentSystemIDList = ProductPermissionsManager.Current.PublishmentSystemIDList;
                    if (publishmentSystemIDList == null)
                    {
                        publishmentSystemIDList = new List<int>();
                    }
                    publishmentSystemIDList.Add(theUserCenterID);
                    BaiRongDataProvider.AdministratorDAO.UpdatePublishmentSystemIDCollection(AdminManager.Current.UserName, TranslateUtils.ObjectCollectionToString(publishmentSystemIDList));
                }

                #region 第三方登录
                BaiRongDataProvider.BaiRongThirdLoginDAO.SetDefaultThirdLogin();
                #endregion

                #region 密保问题
                BaiRongDataProvider.UserSecurityQuestionDAO.SetDefaultQuestion();
                #endregion

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, string.Format("开启{0}应用", EPublishmentSystemTypeUtils.GetText(this.publishmentSystemType)), string.Format("用户中心名称:{0}", string.Format("{0}", PageUtils.FilterXSS(string.IsNullOrEmpty(this.UserCenterName.Text) ? "用户中心" : this.UserCenterName.Text))));
                errorMessage = string.Empty;
                return theUserCenterID;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return 0;
            }
        }

        public void NextPlaceHolder(Object sender, EventArgs e)
        {
            string errorMessage;
            int thePublishmentSystemID = Validate_PublishmentSystemInfo(out errorMessage);
            if (thePublishmentSystemID > 0)
            {
                BaiRongDataProvider.AdministratorDAO.UpdatePublishmentSystemID(AdminManager.Current.UserName, 0);
                string returnUrl = StringUtils.ValueToUrl(PageUtils.GetAdminDirectoryUrl(string.Format("main.aspx?menuID=UserCenter")));
                string url = BackgroundProgressBar.GetCreatePublishmentSystemUrl(thePublishmentSystemID, this.UseSiteTemplate.Checked, true, true, this.ddlSiteTemplate.SelectedValue, false, StringUtils.GUID(), returnUrl, true, true);
                PageUtils.Redirect(url);
            }
            else
            {
                this.ltlErrorMessage.Text = errorMessage;
            }

        }

    }
}
