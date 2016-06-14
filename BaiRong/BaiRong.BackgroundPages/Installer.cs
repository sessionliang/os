using System;
using System.Collections;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using BaiRong.Core.Cryptography;


using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Xml;
using System.Web;
using System.IO;
using BaiRong.Core.Data;
using System.Data.OracleClient;
using BaiRong.Core.Configuration;

namespace BaiRong.BackgroundPages
{
    public class Installer : Page
	{
        public Literal ltlVersionInfo;
        public Literal ltlStepTitle;

        public Literal ltlErrorMessage;

        public PlaceHolder phStep1;
        public CheckBox chkIAgree;

        public PlaceHolder phStep2;
        public Literal ltlDomain;
        public Literal ltlVersion;
        public Literal ltlNETVersion;
        public Literal ltlPhysicalApplicationPath;
        public Literal ltlRootWrite;
        public Literal ltlSiteFielsWrite;
        public Button btnStep2;

        public PlaceHolder phStep3;
        public DropDownList ddlDatabaseType;

        public PlaceHolder phSqlServer;
        public PlaceHolder phSqlServer1;
        public TextBox tbSqlServer;
        public TextBox tbSqlUserName;
        public TextBox tbSqlPassword;
        public HtmlInputHidden hihSqlHiddenPassword;
        public PlaceHolder phSqlServer2;
        public DropDownList ddlSqlDatabaseName;

        public PlaceHolder phOracle;
        public TextBox tbOraHost;
        public TextBox tbOraPort;
        public TextBox tbOraServiceName;
        public TextBox tbOraUserName;
        public TextBox tbOraPassword;
        public HtmlInputHidden hihOraHiddenPassword;

        public PlaceHolder phStep4;
        public TextBox tbAdminName;
        public TextBox tbAdminPassword;
        public TextBox tbComfirmAdminPassword;

        public PlaceHolder phStep5;
        public Literal ltlSuccessMessage;

        private string GetSetpTitleString(int step)
        {
            this.phStep1.Visible = this.phStep2.Visible = this.phStep3.Visible = this.phStep4.Visible = this.phStep5.Visible = false;
            if (step == 1)
            {
                this.phStep1.Visible = true;
            }
            else if (step == 2)
            {
                this.phStep2.Visible = true;
            }
            else if (step == 3)
            {
                this.phStep3.Visible = true;
                this.phSqlServer.Visible = true;
                this.phSqlServer1.Visible = true;
                this.phSqlServer2.Visible = false;
                this.phOracle.Visible = false;
            }
            else if (step == 4)
            {
                this.phStep4.Visible = true;
            }
            else if (step == 5)
            {
                this.phStep5.Visible = true;
            }

            StringBuilder builder = new StringBuilder();

            for (int i = 1; i <= 5; i++)
            {
                string liClass = string.Empty;
                if (i == step)
                {
                    liClass = @" class=""current""";
                }
                string imageUrl = string.Format("images/step{0}{1}.gif", i, (i <= step) ? "a" : "b");
                string title = string.Empty;
                if (i == 1)
                {
                    title = "许可协议";
                }
                else if (i == 2)
                {
                    title = "环境检测";
                }
                else if (i == 3)
                {
                    title = "数据库设置";
                }
                else if (i == 4)
                {
                    title = "安装产品";
                }
                else if (i == 5)
                {
                    title = "安装完成";
                }
                builder.AppendFormat(@"<li{0}><img src=""{1}"" />{2}</li>", liClass, imageUrl, title);
            }

            return builder.ToString();
        }
	
		public void Page_Load(object sender, System.EventArgs e)
		{
            if (!IsPostBack)
            {
                bool isInstalled = !ProductManager.IsNeedInstall();

                if (isInstalled)
                {
                    Page.Response.Write("系统已安装成功，向导被禁用");
                    Page.Response.End();
                    return;
                }

                EDatabaseTypeUtils.AddListItemsToInstall(this.ddlDatabaseType);
                this.ddlDatabaseType_SelectedIndexChanged(null, EventArgs.Empty);

                this.ltlVersionInfo.Text = string.Format("SITESERVER {0}", ProductManager.GetFullVersion());
                this.ltlStepTitle.Text = this.GetSetpTitleString(1);
            }
		}

        public void ddlDatabaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EDatabaseType databaseType = EDatabaseTypeUtils.GetEnumType(this.ddlDatabaseType.SelectedValue);
            if (databaseType == EDatabaseType.SqlServer)
            {
                this.phSqlServer.Visible = true;
                this.phOracle.Visible = false;
            }
            else if (databaseType == EDatabaseType.Oracle)
            {
                this.phSqlServer.Visible = false;
                this.phOracle.Visible = true;
            }
        }

        protected void btnStep1_Click(object sender, System.EventArgs e)
        {
            if (this.chkIAgree.Checked)
            {
                this.btnStep2.Enabled = true;
                this.ltlErrorMessage.Text = string.Empty;

                this.ltlDomain.Text = PageUtils.GetHost();
                this.ltlVersion.Text = ProductManager.GetFullVersion();
                this.ltlNETVersion.Text = string.Format("{0}.{1}", System.Environment.Version.Major, System.Environment.Version.Minor);
                this.ltlPhysicalApplicationPath.Text = ConfigUtils.Instance.PhysicalApplicationPath;

                bool isRootWritable = false;
                try
                {
                    string filePath = PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, "robots.txt");
                    FileUtils.WriteText(filePath, ECharset.utf_8, @"User-agent: *
Disallow: /SiteServer/
Disallow: /SiteFiles/
Disallow: /UserCenter/");
                    isRootWritable = true;
                }
                catch { }
                bool isSiteFilesWritable = false;
                try
                {
                    string filePath = PathUtils.Combine(ConfigUtils.Instance.PhysicalApplicationPath, DirectoryUtils.SiteFiles.DirectoryName, "index.htm");
                    FileUtils.WriteText(filePath, ECharset.utf_8, StringUtils.Constants.Html5Empty);
                    isSiteFilesWritable = true;
                }
                catch { }

                if (isRootWritable)
                {
                    this.ltlRootWrite.Text = "<FONT color=green>[√]</FONT>";
                }
                else
                {
                    this.ltlRootWrite.Text = "<FONT color=red>[×]</FONT>";
                }

                if (isSiteFilesWritable)
                {
                    this.ltlSiteFielsWrite.Text = "<FONT color=green>[√]</FONT>";
                }
                else
                {
                    this.ltlSiteFielsWrite.Text = "<FONT color=red>[×]</FONT>";
                }

                if (!isRootWritable || !isSiteFilesWritable)
                {
                    this.ShowErrorMessage(string.Format(@"系统检测到文件夹权限不足，您需要赋予可写权限，请参考:<a href=""{0}"" target=""_blank"">目录权限设置</a>", StringUtils.Constants.Url_HelpSetup));
                    this.btnStep2.Enabled = false;
                }

                this.ltlStepTitle.Text = this.GetSetpTitleString(2);
            }
            else
            {
                this.ShowErrorMessage("您必须同意软件许可协议才能安装！");
            }
        }

        protected void btnStep2_Click(object sender, System.EventArgs e)
        {
            this.ltlErrorMessage.Text = string.Empty;
            this.ltlStepTitle.Text = this.GetSetpTitleString(3);
        }

        protected void btnStep3_Click(object sender, System.EventArgs e)
        {
            this.ltlErrorMessage.Text = string.Empty;

            EDatabaseType databaseType = EDatabaseTypeUtils.GetEnumType(this.ddlDatabaseType.SelectedValue);

            if (databaseType == EDatabaseType.SqlServer)
            {
                BaiRongDataProvider.SetDatabaseType(EDatabaseType.SqlServer);
                //DataProvider.SetDatabaseType();
                BaiRongDataProvider.SetDatabaseType(EDatabaseType.SqlServer);
                if (this.phSqlServer1.Visible)
                {
                    this.hihSqlHiddenPassword.Value = this.tbSqlPassword.Text;
                    string errorMessage = string.Empty;
                    if (this.Validate_ConnectToSqlServer(out errorMessage))
                    {
                        if (this.Validate_SelectDb_ListDatabases(out errorMessage))
                        {
                            this.phSqlServer1.Visible = false;
                            this.phSqlServer2.Visible = true;
                        }
                        else
                        {
                            this.ShowErrorMessage(errorMessage);
                        }
                    }
                    else
                    {
                        this.ShowErrorMessage(errorMessage);
                    }
                }
                else
                {
                    this.ltlStepTitle.Text = this.GetSetpTitleString(4);
                }
            }
            else if (databaseType == EDatabaseType.Oracle)
            {
                BaiRongDataProvider.SetDatabaseType(EDatabaseType.Oracle);
                //DataProvider.SetDatabaseType();
                BaiRongDataProvider.SetDatabaseType(EDatabaseType.Oracle);

                this.hihOraHiddenPassword.Value = this.tbOraPassword.Text;
                string errorMessage = string.Empty;
                if (this.Validate_ConnectToOracle(out errorMessage))
                {
                    this.ltlStepTitle.Text = this.GetSetpTitleString(4);
                }
                else
                {
                    this.ShowErrorMessage(errorMessage);
                }
            }
        }

        protected void btnStep4_Click(object sender, System.EventArgs e)
        {
            this.ltlErrorMessage.Text = string.Empty;

            string errorMessage = string.Empty;
            if (this.CheckLoginValid(out errorMessage))
            {
                if (this.InstallDatabase(out errorMessage))
                {
                    this.ltlStepTitle.Text = this.GetSetpTitleString(5);
                }
                else
                {
                    this.ShowErrorMessage(errorMessage);
                }
            }
            else
            {
                this.ShowErrorMessage(errorMessage);
            }
        }

        protected void btnPrevious_Click(object sender, System.EventArgs e)
		{
            this.ltlErrorMessage.Text = string.Empty;

            if (phStep4.Visible)
            {
                this.ltlStepTitle.Text = this.GetSetpTitleString(3);
                this.phSqlServer1.Visible = true;
                this.phSqlServer2.Visible = false;
                this.phOracle.Visible = false;
                this.ddlDatabaseType_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else if (phStep3.Visible)
            {
                if (this.phSqlServer2.Visible)
                {
                    this.phSqlServer1.Visible = true;
                    this.phSqlServer2.Visible = false;
                }
                else
                {
                    this.ltlStepTitle.Text = this.GetSetpTitleString(2);
                }
            }
            else if (phStep2.Visible)
            {
                this.ltlStepTitle.Text = this.GetSetpTitleString(1);
            }
		}

        private string GetConnectionString(bool isDatabaseName)
        {
            EDatabaseType databaseType = EDatabaseTypeUtils.GetEnumType(this.ddlDatabaseType.SelectedValue);

            if (databaseType == EDatabaseType.SqlServer)
            {
                if (isDatabaseName)
                {
                    return string.Format("server={0};uid={1};pwd={2};database={3}", this.tbSqlServer.Text, this.tbSqlUserName.Text, this.hihSqlHiddenPassword.Value, this.ddlSqlDatabaseName.SelectedValue);
                }
                else
                {
                    return string.Format("server={0};uid={1};pwd={2}", this.tbSqlServer.Text, this.tbSqlUserName.Text, this.hihSqlHiddenPassword.Value);
                }
            }
            else if (databaseType == EDatabaseType.Oracle)
            {
                return string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0}) (PORT={1})))(CONNECT_DATA=(SERVICE_NAME={2})));User Id={3};Password={4};Integrated Security=no;", this.tbOraHost.Text, this.tbOraPort.Text, this.tbOraServiceName.Text, this.tbOraUserName.Text, this.hihOraHiddenPassword.Value);
            }
            return string.Empty;
        }

        private bool Validate_ConnectToSqlServer(out string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(this.tbSqlServer.Text) || string.IsNullOrEmpty(this.tbSqlUserName.Text))
                {
                    errorMessage = "数据库主机及数据库用户必须填写。";
                    return false;
                }
                SqlConnection connection = new SqlConnection(GetConnectionString(false));
                connection.Open();
                connection.Close();

                errorMessage = "";

                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }

        private bool Validate_ConnectToOracle(out string errorMessage)
        {
            try
            {
                if (string.IsNullOrEmpty(this.tbOraHost.Text) || string.IsNullOrEmpty(this.tbOraPort.Text) || string.IsNullOrEmpty(this.tbOraServiceName.Text) || string.IsNullOrEmpty(this.tbOraUserName.Text) || string.IsNullOrEmpty(this.tbOraPassword.Text))
                {
                    errorMessage = "Oracle连接信息必须填写。";
                    return false;
                }
                OracleConnection connection = new OracleConnection(GetConnectionString(false));
                connection.Open();
                connection.Close();

                errorMessage = "";

                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }

        private bool Validate_SelectDb_ListDatabases(out string errorMessage)
        {
            try
            {
                SqlConnection connection = new SqlConnection(GetConnectionString(false));
                SqlDataReader dr;
                SqlCommand command = new SqlCommand("select name from master..sysdatabases order by name asc", connection);

                connection.Open();

                connection.ChangeDatabase("master");

                dr = command.ExecuteReader();

                this.ddlSqlDatabaseName.Items.Clear();

                while (dr.Read())
                {
                    string dbName = dr["name"] as String;
                    if (dbName != null)
                    {
                        if (dbName != "master" &&
                            dbName != "msdb" &&
                            dbName != "tempdb" &&
                            dbName != "model")
                        {
                            this.ddlSqlDatabaseName.Items.Add(dbName);
                        }
                    }
                }

                connection.Close();

                errorMessage = "";

                return true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }
        }

        private bool CheckLoginValid(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrEmpty(this.tbAdminName.Text))
            {
                errorMessage = "管理员用户名不能为空！";
                return false;
            }

            if (string.IsNullOrEmpty(this.tbAdminPassword.Text))
            {
                errorMessage = "管理员密码不能为空！";
                return false;
            }

            if (this.tbAdminPassword.Text.Length < 6)
            {
                errorMessage = "管理员密码必须大于6位！";
                return false;
            }

            if (this.tbAdminPassword.Text != this.tbComfirmAdminPassword.Text)
            {
                errorMessage = "两次输入的管理员密码不一致！";
                return false;
            }

            return true;
        }

        private bool InstallDatabase(out string errorMessage)
        {
            if (!UpdateWebConfig(out errorMessage)) return false;

            try
            {
                EDatabaseType databaseType = EDatabaseTypeUtils.GetEnumType(this.ddlDatabaseType.SelectedValue);

                BaiRongDataProvider.SetDatabaseType(databaseType);
                BaiRongDataProvider.ConnectionString = this.GetConnectionString(true);

                bool isDatabase = true;

                if (isDatabase)
                {
                    AppManager.InstallApp(AppManager.Platform.AppID);
                }

                BaiRongDataProvider.ConfigDAO.Initialize();

                BaiRongDataProvider.UserConfigDAO.InitializeUserRole(this.tbAdminName.Text, this.tbAdminPassword.Text);

                //BaiRongDataProvider.InitializeManual();

                foreach (string appID in AppManager.GetAppIDList())
                {
                    try
                    {
                        if (isDatabase)
                        {
                            AppManager.InstallApp(appID);
                        }

                        //if (StringUtils.EqualsIgnoreCase(appID, AppManager.Platform.SSO.AppID))
                        //{
                        //    SSOAppInfo appInfo = new SSOAppInfo(0, ESSOAppType.SiteServer, "localhost", PageUtils.GetHost(), SSOUtils.GenerateAuthKey(), string.Empty, string.Empty, true, DateTime.Now, string.Empty);
                        //    int ssoAppID = BaiRongDataProvider.SSOAppDAO.Insert(appInfo);
                        //    configInfo = FileConfigManager.Instance.SSOConfig;
                        //    configInfo.AuthenticationType = EAuthenticationType.SSORemote;
                        //    configInfo.SSOAuthKey = appInfo.AuthKey;
                        //    configInfo.SSOConnectionString = BaiRongDataProvider.ConnectionString;
                        //    configInfo.SSODatabaseType = BaiRongDataProvider.DatabaseType;
                        //    configInfo.SSOID = ssoAppID.ToString();
                        //    configInfo.SSOUrl = appInfo.Url;
                        //    FileConfigManager.UpdateSSOConfigFile(configInfo);
                        //}
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

        private bool UpdateWebConfig(out string errorMessage)
        {
            errorMessage = string.Empty;

            bool returnValue = false;
            
            try
            {
                string databaseType = this.ddlDatabaseType.SelectedValue;
                string connectionString = GetConnectionString(true);

                string configFilePath = PathUtils.MapPath("~/web.config");
                if (FileUtils.IsFileExists(configFilePath))
                {
                    PathUtils.UpdateWebConfig(configFilePath, databaseType, connectionString);
                }

                configFilePath = PathUtils.MapPath("~/api/web.config");
                if (FileUtils.IsFileExists(configFilePath))
                {
                    PathUtils.UpdateWebConfig(configFilePath, databaseType, connectionString);
                }
                
                returnValue = true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return returnValue;
        }

        public string GetSiteServerUrl()
        {
            return PageUtils.GetAdminDirectoryUrl(string.Empty);
        }

        private void ShowErrorMessage(string errorMessage)
        {
            this.ltlErrorMessage.Text = string.Format(@"<img src=""images/check_error.gif"" /> {0}", errorMessage);
        }
	}
}
