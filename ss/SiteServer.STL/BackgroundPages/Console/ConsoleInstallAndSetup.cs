using System;
using System.Collections;
using System.Text;
using System.Web.UI;
using BaiRong.Core;
using System.Collections.Specialized;
using BaiRong.Core.Net;
using BaiRong.Core.IO;
using System.Data.SqlClient;
using BaiRong.Core.Data.Provider;

using System.Xml;
using System.Web;
using BaiRong.Model;
using BaiRong.Core.Configuration;

using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.STL.ImportExport;

namespace SiteServer.STL.BackgroundPages
{
    public class ConsoleInstallAndSetup : Page
    {
        public const string CACHE_CURRENT_COUNT = "_CurrentCount";

        public void Page_Load(object sender, System.EventArgs e)
        {
            string action = base.Request.QueryString["action"];
            
            string retString = string.Empty;

            if (action == "setup")
            {
                retString = Action_Setup();
            }
            else if (action == "check")
            {
                retString = Action_Check();
            }
            else if (action == "addadmin")
            {
                retString = Action_AddAdmin();
            }
            else if (action == "qxblogin")
            {
                retString = Action_QxbLogin();
            }

            Page.Response.Write(retString);
            Page.Response.End();
        }

        #region Setup

        private string Action_Setup()
        {
            try
            {
                string dbHost = base.Request.QueryString["dbHost"];
                string dbName = base.Request.QueryString["dbName"];
                string dbUser = base.Request.QueryString["dbUser"];
                string dbPassword = base.Request.QueryString["dbPassword"];
                string userName = base.Request.QueryString["userName"];
                string password = base.Request.QueryString["password"];
                string settings = StringUtils.ValueFromUrl(base.Request.QueryString["settings"]);
                string guid = base.Request.QueryString["guid"];

                bool isInstalled = !ProductManager.IsNeedInstall();

                if (isInstalled)
                {
                    return "系统已安装成功，安装页面被禁用";
                }

                if (string.IsNullOrEmpty(dbHost) || string.IsNullOrEmpty(dbName) || string.IsNullOrEmpty(dbUser) || string.IsNullOrEmpty(dbPassword))
                {
                    return PageUtils.GetHost() + "/setup.aspx?action=setup&dbHost=数据库服务器&dbName=数据库名称&dbUser=数据库账号&dbPassword=数据库密码&userName=管理员账号&password=管理员密码&settings=template_equals_W1BDYIYUAN&guid=6F9619FF-8B8-D011-B42D-00C04FC964FF";
                }

                string cacheCurrentCountKey = guid + CACHE_CURRENT_COUNT;
                bool isTemplate = false;
                string siteTemplateDir = string.Empty;
                EPublishmentSystemType publishmentSystemType = EPublishmentSystemType.CMS;

                if (!string.IsNullOrEmpty(settings))
                {
                    NameValueCollection parameters = TranslateUtils.ToNameValueCollection(settings);

                    isTemplate = !string.IsNullOrEmpty(parameters["template"]);
                    siteTemplateDir = "T_" + parameters["template"];
                    publishmentSystemType = EPublishmentSystemTypeUtils.GetEnumType(parameters["publishmentSystemType"]);

                    if (isTemplate)
                    {
                        if (!SiteTemplateManager.Instance.IsSiteTemplateDirectoryExists(siteTemplateDir))
                        {
                            string downloadUrl = string.Format("http://moban.download.siteserver.cn/all/{0}.zip", siteTemplateDir);
                            string siteTemplatePath = PathUtility.GetSiteTemplatesPath(siteTemplateDir + ".zip");
                            try
                            {
                                WebClientUtils.SaveRemoteFileToLocal(downloadUrl, siteTemplatePath);
                                ZipUtils.UnpackFiles(siteTemplatePath, PathUtility.GetSiteTemplatesPath(siteTemplateDir));
                            }
                            catch { }
                        }
                    }
                }

                CacheUtils.Max(cacheCurrentCountKey, "0");//当前数

                CacheUtils.Max(cacheCurrentCountKey, "33");

                string connectionString = string.Format("server={0};uid={1};pwd={2};database={3}", dbHost, dbUser, dbPassword, dbName);
                string errorMessage = string.Empty;
                bool isSuccess = false;
                if (Action_Setup_UpdateWebConfig(connectionString, out errorMessage))
                {
                    CacheUtils.Max(cacheCurrentCountKey, "66");

                    isSuccess = this.Action_Setup_InstallDatabase(connectionString, userName, password, out errorMessage);
                }

                if (isSuccess)
                {
                    if (isTemplate)
                    {
                        string groupSN = GroupSNManager.GetCurrentGroupSN();

                        PublishmentSystemInfo publishmentSystemInfo = new PublishmentSystemInfo(0, "主站", publishmentSystemType, EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.BackgroundContent), EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.GoodsContent), EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.BrandContent), EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.GovPublicContent), EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.GovInteractContent), EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.VoteContent), EAuxiliaryTableTypeUtils.GetDefaultTableName(EAuxiliaryTableType.JobContent), false, 0, string.Empty, "/", true, 0, groupSN, 0, string.Empty);

                        NodeInfo nodeInfo = new NodeInfo();

                        nodeInfo.NodeName = nodeInfo.NodeIndexName = "首页";
                        nodeInfo.NodeType = ENodeType.BackgroundPublishNode;

                        int publishmentSystemID = DataProvider.NodeDAO.InsertPublishmentSystemInfo(nodeInfo, publishmentSystemInfo);

                        SiteTemplateManager.Instance.ImportSiteTemplateToEmptyPublishmentSystem(publishmentSystemID, siteTemplateDir, false, true, true);
                    }

                    CacheUtils.Max(cacheCurrentCountKey, "100");

                    return @"<?xml version=""1.0"" encoding=""utf-8""?>
<rsp>
  <code>200</code>
  <msg>ok</msg>
</rsp>
";
                }
                else
                {
                    return string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<rsp>
  <code>113</code>
  <msg>{0}</msg>
</rsp>
", errorMessage);
                }
            }
            catch (Exception ex)
            {
                return string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<rsp>
  <code>113</code>
  <msg>{0}</msg>
</rsp>
", ex.Message);
            }
        }

        private bool Action_Setup_InstallDatabase(string connectionString, string userName, string password, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                BaiRongDataProvider.SetDatabaseType(EDatabaseType.SqlServer);
                BaiRongDataProvider.ConnectionString = connectionString;

                //bool isInstalled = !ProductManager.IsNeedInstall();
                //if (isInstalled)
                //{
                //    BaiRongDataProvider.DatabaseDAO.ClearDatabase(connectionString);
                //}

                AppManager.InstallApp(AppManager.Platform.AppID);

                BaiRongDataProvider.ConfigDAO.Initialize();

                BaiRongDataProvider.UserConfigDAO.InitializeUserRole(userName, password);

                foreach (string appID in AppManager.GetAppIDList())
                {
                    try
                    {
                        AppManager.InstallApp(appID);
                    }
                    catch { }
                }

                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }

        private bool Action_Setup_UpdateWebConfig(string connectionString, out string errorMessage)
        {
            errorMessage = string.Empty;

            bool returnValue = false;

            try
            {
                string configFilePath = PathUtils.MapPath("~/web.config");
                if (FileUtils.IsFileExists(configFilePath))
                {
                    PathUtils.UpdateWebConfig(configFilePath, "SqlServer", connectionString);
                }

                configFilePath = PathUtils.MapPath("~/api/web.config");
                if (FileUtils.IsFileExists(configFilePath))
                {
                    PathUtils.UpdateWebConfig(configFilePath, "SqlServer", connectionString);
                }

                returnValue = true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return returnValue;
        }

        #endregion

        #region Check

        private string Action_Check()
        {
            string guid = base.Request.QueryString["guid"];

            if (ProductManager.IsNeedInstall())
            {
                if (CacheUtils.Get(guid + CACHE_CURRENT_COUNT) != null)
                {
                    int currentCount = TranslateUtils.ToInt((string)CacheUtils.Get(guid + CACHE_CURRENT_COUNT));

                    return string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<rsp>
  <code>200</code>
  <msg>{0}</msg>
</rsp>
", currentCount);
                }
                else
                {
                    return @"<?xml version=""1.0"" encoding=""utf-8""?>
<rsp>
  <code>200</code>
  <msg>0</msg>
</rsp>
";
                }
            }
            else
            {
                return @"<?xml version=""1.0"" encoding=""utf-8""?>
<rsp>
  <code>200</code>
  <msg>100</msg>
</rsp>
";
            }
        }

        #endregion

        #region AddAdmin

        private string Action_AddAdmin()
        {
            string adminuser = base.Request.QueryString["adminuser"];
            string adminpassword = base.Request.QueryString["adminpassword"];
            string guid = base.Request.QueryString["guid"];

            if (string.IsNullOrEmpty(adminuser) || string.IsNullOrEmpty(adminpassword))
            {
                return "setup.aspx?action=addadmin&adminuser=账号&adminpassword=密码&guid=6F9619FF-8B86-D011-B42D-00C04FC964FF";
            }

            bool isSuccess = false;
            string errorMessage = string.Empty;

            if (adminuser != "siteserver")
            {
                AdministratorInfo administratorInfo = new AdministratorInfo();
                administratorInfo.UserName = adminuser;
                administratorInfo.Password = adminpassword;
                administratorInfo.Question = string.Empty;
                administratorInfo.Answer = string.Empty;
                administratorInfo.Email = string.Empty;

                isSuccess = AdminManager.CreateAdministrator(administratorInfo, out errorMessage);
                RoleManager.AddUserToRole(adminuser, EPredefinedRoleUtils.GetValue(EPredefinedRole.ConsoleAdministrator));

                if (isSuccess)
                {
                    BaiRongDataProvider.AdministratorDAO.Delete("siteserver");
                }
            }
            else
            {
                isSuccess = BaiRongDataProvider.AdministratorDAO.ChangePassword(adminuser, EPasswordFormat.Encrypted, adminpassword);
            }

            if (isSuccess)
            {
                return @"<?xml version=""1.0"" encoding=""utf-8""?>
<rsp>
  <code>200</code>
  <msg>ok</msg>
</rsp>
";
            }
            else
            {
                return string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<rsp>
  <code>131</code>
  <msg>{0}</msg>
</rsp>
", errorMessage);
            }
        }

        #endregion

        #region QxbLogin

        private string Action_QxbLogin()
        {
            string token = base.Request.QueryString["token"];

            if (string.IsNullOrEmpty(token))
            {
                return "setup.aspx?action=qxblogin&token=token";
            }

            bool isSuccess = false;
            string errorMessage = string.Empty;

            string isvKey = ConfigUtils.Instance.GetAppSettings("ALIYUN_ISVKey");

            string qxbUrl = string.Format("http://qxbproducer.aliyun.com/auth/get_login_user.do?token={0}&isvKey={1}", token, isvKey);

            try
            {
                string jsonString = WebClientUtils.GetRemoteFileSource(qxbUrl, ECharset.utf_8);
                NameValueCollection attributes = TranslateUtils.ParseJsonStringToNameValueCollection(jsonString);
                if (attributes != null && TranslateUtils.ToBool(attributes["ret"]))
                {
                    if (!BaiRongDataProvider.AdministratorDAO.IsUserNameExists("siteserver"))
                    {
                        AdministratorInfo administratorInfo = new AdministratorInfo();
                        administratorInfo.UserName = "siteserver";
                        administratorInfo.Password = "siteserver";
                        administratorInfo.Question = string.Empty;
                        administratorInfo.Answer = string.Empty;
                        administratorInfo.Email = string.Empty;

                        isSuccess = AdminManager.CreateAdministrator(administratorInfo, out errorMessage);
                        RoleManager.AddUserToRole(administratorInfo.UserName, EPredefinedRoleUtils.GetValue(EPredefinedRole.ConsoleAdministrator));
                    }
                    BaiRongDataProvider.AdministratorDAO.RedirectFromLoginPage("siteserver", true);
                    isSuccess = true;
                }
            }
            catch { }

            if (!isSuccess)
            {
                return @"<?xml version=""1.0"" encoding=""utf-8""?>
<rsp>
  <code>200</code>
  <msg>ok</msg>
</rsp>
";
            }
            else
            {
                return string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<rsp>
  <code>131</code>
  <msg>{0}</msg>
</rsp>
", errorMessage);
            }
        }

        #endregion

    }
}
