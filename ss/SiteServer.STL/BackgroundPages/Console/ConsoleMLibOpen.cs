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
    public class ConsoleMLibOpen : BackgroundBasePage
    {
        protected override bool IsSinglePage
        {
            get { return true; }
        }

        public Literal ltlPageTitle;
        public DropDownList ddlSiteTemplate;
        public CheckBox UseSiteTemplate;

        public PlaceHolder CreateSiteParameters;
        public TextBox MLibName;
        public TextBox MLibDir;
        public TextBox tbCheckLevel;

        public PlaceHolder OperatingError;
        public Literal ltlErrorMessage;

        public Button Previous;
        public Button Next;

        private EPublishmentSystemType publishmentSystemType = EPublishmentSystemType.MLib;
        SortedList sortedlist = new SortedList();

        public static string GetRedirectUrl()
        {
            return PageUtils.GetSTLUrl(string.Format("console_MLibOpen.aspx"));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;
            this.MLibDir.Enabled = false;
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
                string mlibDir = string.Empty;

                if (DirectoryUtils.IsSystemDirectory(MLibDir.Text))
                {
                    errorMessage = "文件夹名称不能为系统文件夹名称！";
                    return 0;
                }

                mlibDir = this.MLibDir.Text;

                ArrayList arraylist = DataProvider.NodeDAO.GetLowerSystemDirArrayList(0);
                if (arraylist.IndexOf(mlibDir.ToLower()) != -1)
                {
                    errorMessage = "已存在相同的发布路径！";
                    return 0;
                }

                if (!DirectoryUtils.IsDirectoryNameCompliant(mlibDir))
                {
                    errorMessage = "文件夹名称不符合系统要求！";
                    return 0;
                }


                NodeInfo nodeInfo = new NodeInfo();

                nodeInfo.NodeName = nodeInfo.NodeIndexName = "首页";
                nodeInfo.NodeType = ENodeType.BackgroundPublishNode;
                nodeInfo.ContentModelID = EContentModelTypeUtils.GetValue(EContentModelTypeUtils.GetEnumTypeByPublishmentSystemType(this.publishmentSystemType));

                string mlibUrl = PageUtils.Combine(ConfigUtils.Instance.ApplicationPath, mlibDir);

                string groupSN = GroupSNManager.GetCurrentGroupSN();



                PublishmentSystemInfo psInfo = BaseTable.GetDefaultPublishmentSystemInfo(string.Format("{0}稿件库", PageUtils.FilterXSS(this.MLibName.Text)), this.publishmentSystemType, "ml_Content", "", "", "", "", EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.VoteContent), EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.JobContent), mlibDir, mlibUrl, 0, groupSN);

                if (psInfo.ParentPublishmentSystemID > 0)
                {
                    PublishmentSystemInfo parentPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(psInfo.ParentPublishmentSystemID);
                    psInfo.PublishmentSystemUrl = PageUtils.Combine(parentPublishmentSystemInfo.PublishmentSystemUrl, psInfo.PublishmentSystemDir);
                }

                psInfo.IsHeadquarters = false;

                psInfo.Additional.Charset = ECharsetUtils.GetValue(ECharset.utf_8);
                psInfo.IsCheckContentUseLevel = false;

                int theMLibID = DataProvider.NodeDAO.InsertPublishmentSystemInfo(nodeInfo, psInfo);

                DataProvider.MlibDAO.UpdateConfigAttr("CheckLevel", tbCheckLevel.Text);

                if (PermissionsManager.Current.IsSystemAdministrator && !PermissionsManager.Current.IsConsoleAdministrator)
                {
                    List<int> publishmentSystemIDList = ProductPermissionsManager.Current.PublishmentSystemIDList;
                    if (publishmentSystemIDList == null)
                    {
                        publishmentSystemIDList = new List<int>();
                    }
                    publishmentSystemIDList.Add(theMLibID);
                    //BaiRongDataProvider.AdministratorDAO.UpdatePublishmentSystemIDCollection(AdminManager.Current.UserName, TranslateUtils.ObjectCollectionToString(publishmentSystemIDList));
                }

                #region 设置系统初始值
                string nodeIDCollection = theMLibID.ToString();

                //创建初始分类
                int defaultNode1 = DataProvider.NodeDAO.InsertNodeInfo(theMLibID, theMLibID, "新闻资讯", "1", EContentModelTypeUtils.GetValue(EContentModelType.Content));
                int defaultNode2 = DataProvider.NodeDAO.InsertNodeInfo(theMLibID, theMLibID, "行业动态", "2", EContentModelTypeUtils.GetValue(EContentModelType.Content));
                nodeIDCollection = nodeIDCollection + "," + defaultNode1 + "," + defaultNode2;
                //创建角色
                ArrayList modules = new ArrayList() { "cms", "apps" };

                SystemPermissionsInfo info = new SystemPermissionsInfo()
                {
                    ChannelPermissions = "cms_contentView,cms_contentAdd,cms_contentEdit,cms_contentDelete,cms_contentTranslate,cms_contentArchive,cms_contentOrder,cms_channelAdd,cms_channelEdit,cms_channelDelete,cms_channelTranslate,cms_commentCheck,cms_commentDelete,cms_createPage,cms_publishPage,cms_contentCheck",
                    WebsitePermissions = "cms_siteAnalysis,cms_contentTrash,cms_inputContentView,cms_input,cms_gather,cms_advertisement,cms_resume,cms_mail,cms_seo,cms_tracking,cms_innerLink,cms_restriction,cms_backup,cms_archive,cms_fileManagement,cms_allPublish,cms_bShare,cms_template,cms_user,cms_configration,cms_create,b2c_order,b2c_request,b2c_configuration",
                    NodeIDCollection = nodeIDCollection,
                    PublishmentSystemID = theMLibID
                };

                int checkLevel = TranslateUtils.ToInt(tbCheckLevel.Text);
                for (int i = 0; i < checkLevel; i++)
                {
                    try
                    {
                        info.RoleName = "投稿系统" + Number2Chinese(i + 1) + "审角色";
                        DataProvider.PermissionsDAO.InsertRoleAndPermissions(info.RoleName, modules,
                            AdminManager.Current.UserName, "", new ArrayList() { "platform_mlib" }, new ArrayList() { info });

                        AdministratorInfo adminInfo = new AdministratorInfo();
                        adminInfo.UserName = "mlibadmin" + (i + 1);
                        adminInfo.DisplayName = "投稿系统" + Number2Chinese(i + 1) + "审管理员";
                        adminInfo.Password = "123456";
                        adminInfo.CreatorUserName = "";
                        AdminManager.CreateAdministrator(adminInfo, out errorMessage);
                        BaiRongDataProvider.RoleDAO.AddUserToRole(adminInfo.UserName, info.RoleName);

                        DataProvider.MlibDAO.UpdateRoleCheckLevel(info.RoleName, new[] { (i + 1).ToString() });
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;

                    }
                }


                var ds = DataProvider.MlibDAO.GetRoleCheckLevel("1=1");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataProvider.MlibDAO.UpdateNodeAdminRoles(TranslateUtils.ToInt(ds.Tables[0].Rows[i]["ID"].ToString()), new[] { defaultNode1.ToString(), defaultNode2.ToString() });
                }
                #endregion


                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, string.Format("开启{0}应用", EPublishmentSystemTypeUtils.GetText(this.publishmentSystemType)), string.Format("用户中心名称:{0}", string.Format("{0}用户中心", PageUtils.FilterXSS(this.MLibName.Text))));
                errorMessage = string.Empty;
                return theMLibID;
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
                string returnUrl = StringUtils.ValueToUrl(PageUtils.GetAdminDirectoryUrl(string.Format("main.aspx?menuID=MLib")));
                string url = BackgroundProgressBar.GetCreatePublishmentSystemUrl(thePublishmentSystemID, this.UseSiteTemplate.Checked, true, true, this.ddlSiteTemplate.SelectedValue, false, StringUtils.GUID(), returnUrl, true);
                PageUtils.Redirect(url);
            }
            else
            {
                this.ltlErrorMessage.Text = errorMessage;
            }

        }
        public string Number2Chinese(int n)
        {

            int MaxCheckLevel = TranslateUtils.ToInt(DataProvider.MlibDAO.GetConfigAttr("CheckLevel"));
            if (n == MaxCheckLevel)
            {
                return "终";
            }
            var chinese = new string[] { "", "初", "二", "三", "四", "五", "六", "七", "八", "九" };
            return chinese[n];
        }

    }
}
