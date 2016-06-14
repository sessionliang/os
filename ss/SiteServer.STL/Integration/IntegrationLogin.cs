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
using System.Web.UI.WebControls;
using SiteServer.CMS.Core.Security;
using System.Web.Script.Serialization;
using BaiRong.Core.Integration;
using System.Collections.Generic;

namespace SiteServer.STL.Pages
{
    public class IntegrationLogin : Page
    {
        public Literal ltlScript;

        public void Page_Load(object sender, System.EventArgs e)
        {
            EIntegrationType integrationType = FileConfigManager.Instance.SSOConfig.IntegrationType;

            if (integrationType == EIntegrationType.QCloud)
            {
                string redirectUrl = Action_QCloud();
                if (!string.IsNullOrEmpty(redirectUrl))
                {
                    ltlScript.Text = string.Format("<script>location.href = '{0}';</script>", redirectUrl);
                }
            }
            else if (integrationType == EIntegrationType.Aliyun)
            {
                string retString = Action_Aliyun();
                Page.Response.Write(retString);
                Page.Response.End();
            }
            else
            {
                string errorMessage = string.Empty;
                string redirectUrl = string.Empty;
                bool isSuccess = Action_Integration(integrationType, out redirectUrl, out errorMessage);
                if (isSuccess)
                {
                    ltlScript.Text = string.Format("<script>location.href = '{0}';</script>", redirectUrl);
                }
                else
                {
                    ltlScript.Text = string.Format("<p>{0}</p>", errorMessage);
                }
            }
        }

        private bool Action_Integration(EIntegrationType integrationType, out string redirectUrl, out string errorMessage)
        {
            errorMessage = string.Empty;
            redirectUrl = string.Empty;

            string token = base.Request.QueryString["token"];
            int publishmentSystemID = TranslateUtils.ToInt(base.Request.QueryString["publishmentSystemID"]);

            bool isSuccess = false;

            try
            {
                string userName = IntegrationManager.GetIntegrationUserName(token);

                        if (!FileConfigManager.Instance.SSOConfig.IsUser)
                        {
                    if (!BaiRongDataProvider.AdministratorDAO.IsUserNameExists(userName))
                            {
                                AdministratorInfo administratorInfo = new AdministratorInfo();
                        administratorInfo.UserName = userName;
                        administratorInfo.DisplayName = userName;
                                administratorInfo.Password = StringUtils.GetShortGUID();
                                administratorInfo.Question = string.Empty;
                                administratorInfo.Answer = string.Empty;
                                administratorInfo.Email = string.Empty;

                                AdminManager.CreateAdministrator(administratorInfo, out errorMessage);
                                RoleManager.AddUserToRole(administratorInfo.UserName, EPredefinedRoleUtils.GetValue(EPredefinedRole.SystemAdministrator));
                            }

                    BaiRongDataProvider.AdministratorDAO.Login(userName, true);

                            if (FileConfigManager.Instance.OEMConfig.IsOEM || !PublishmentSystemManager.IsExists(publishmentSystemID))
                            {
                                redirectUrl = PageUtils.GetAdminDirectoryUrl("initialization.aspx");
                            }
                            else
                            {
                                if (publishmentSystemID > 0)
                                {
                            BaiRongDataProvider.AdministratorDAO.UpdateLastActivityDateAndPublishmentSystemID(userName, publishmentSystemID);
                                    redirectUrl = PageUtils.GetAdminDirectoryUrl("initialization.aspx");
                                }
                                else
                                {
                                    redirectUrl = PageUtils.GetAdminDirectoryUrl("stl/console_appAdd.aspx");
                                }
                            }

                            isSuccess = true;
                        }
                        else
                        {
                    string integrationUserName = FileConfigManager.Instance.SSOConfig.UserPrefix + userName;

                            if (!BaiRongDataProvider.UserDAO.IsUserExists(string.Empty, integrationUserName))
                            {
                                UserInfo userInfo = new UserInfo();
                                userInfo.GroupSN = string.Empty;
                                userInfo.UserName = integrationUserName;
                        userInfo.DisplayName = userName;
                                userInfo.Password = StringUtils.GetShortGUID();
                                userInfo.Email = string.Empty;
                                userInfo.CreateDate = DateTime.Now;
                                userInfo.IsChecked = true;

                                BaiRongDataProvider.UserDAO.InsertWithoutValidation(userInfo);
                                isSuccess = true;
                            }

                            BaiRongDataProvider.UserDAO.Login(string.Empty, integrationUserName, true);
                            redirectUrl = "/";
                            isSuccess = true;
                        }


                //IntegrationCloudInfo cloudInfo = IntegrationManager.API_GEXIA_COM.GetIntegrationCloudInfo(token, out errorMessage);
                //string url = string.Empty;
                //bool isToSync = SaasUtils.Sync(cloudInfo, out url);

                //if (cloudInfo != null)
                //{
                //    if (cloudInfo.IsSuccess && !string.IsNullOrEmpty(cloudInfo.UserName))
                //    {
                //        if (!FileConfigManager.Instance.SSOConfig.IsUser)
                //        {
                //            if (!BaiRongDataProvider.AdministratorDAO.IsUserNameExists(cloudInfo.UserName))
                //            {
                //                AdministratorInfo administratorInfo = new AdministratorInfo();
                //                administratorInfo.UserName = cloudInfo.UserName;
                //                administratorInfo.DisplayName = cloudInfo.DisplayName;
                //                administratorInfo.Password = StringUtils.GetShortGUID();
                //                administratorInfo.Question = string.Empty;
                //                administratorInfo.Answer = string.Empty;
                //                administratorInfo.Email = string.Empty;

                //                AdminManager.CreateAdministrator(administratorInfo, out errorMessage);
                //                RoleManager.AddUserToRole(administratorInfo.UserName, EPredefinedRoleUtils.GetValue(EPredefinedRole.SystemAdministrator));
                //            }

                //            BaiRongDataProvider.AdministratorDAO.Login(cloudInfo.UserName, true);

                //            if (FileConfigManager.Instance.OEMConfig.IsOEM || !PublishmentSystemManager.IsExists(publishmentSystemID))
                //            {
                //                redirectUrl = PageUtils.GetAdminDirectoryUrl("initialization.aspx");
                //            }
                //            else
                //            {
                //                if (publishmentSystemID > 0)
                //                {
                //                    BaiRongDataProvider.AdministratorDAO.UpdateLastActivityDateAndPublishmentSystemID(cloudInfo.UserName, publishmentSystemID);
                //                    redirectUrl = PageUtils.GetAdminDirectoryUrl("initialization.aspx");
                //                }
                //                else
                //                {
                //                    redirectUrl = PageUtils.GetAdminDirectoryUrl("stl/console_appAdd.aspx");
                //                }
                //            }

                //            isSuccess = true;
                //        }
                //        else
                //        {
                //            string integrationUserName = FileConfigManager.Instance.SSOConfig.UserPrefix + cloudInfo.UserName;

                //            if (!BaiRongDataProvider.UserDAO.IsExists(string.Empty, integrationUserName))
                //            {
                //                UserInfo userInfo = new UserInfo();
                //                userInfo.GroupSN = string.Empty;
                //                userInfo.UserName = integrationUserName;
                //                userInfo.DisplayName = cloudInfo.DisplayName;
                //                userInfo.AvatarLarge = userInfo.AvatarMiddle = userInfo.AvatarSmall = cloudInfo.AvatorUrl;
                //                userInfo.Password = StringUtils.GetShortGUID();
                //                userInfo.Email = string.Empty;
                //                userInfo.CreateDate = DateTime.Now;
                //                userInfo.IsChecked = true;

                //                BaiRongDataProvider.UserDAO.InsertWithoutValidation(userInfo);
                //                isSuccess = true;
                //            }

                //            BaiRongDataProvider.UserDAO.Login(string.Empty, integrationUserName, true);
                //            redirectUrl = "/";
                //            isSuccess = true;
                //        }
                //    }
                //    else
                //    {
                //        isSuccess = false;
                //    }
                //}
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }

            return isSuccess;
        }

        private string GetQCloudRedirectUrl(string menuID, string key)
        {
            string redirectUrl = string.Empty;

            int publishmentSystemID = 0;
            if (ProductPermissionsManager.Current.PublishmentSystemIDList.Count > 0)
            {
                if (ProductPermissionsManager.Current.PublishmentSystemIDList.Contains(AdminManager.Current.PublishmentSystemID))
                {
                    publishmentSystemID = AdminManager.Current.PublishmentSystemID;
                }
                else
                {
                    publishmentSystemID = ProductPermissionsManager.Current.PublishmentSystemIDList[0];
                }
            }

            if (!string.IsNullOrEmpty(menuID))
            {
                if (publishmentSystemID == 0)
                {
                    string returnUrl = StringUtils.ValueToUrl(PageUtils.GetWXUrl("background_navWeb.aspx"));
                    redirectUrl = PageUtils.GetSTLUrl(string.Format("console_publishmentSystemAddSaas.aspx?publishmentSystemType=Weixin&returnUrl={0}", returnUrl));
                }
                else
                {
                    string queryString = string.Empty;
                    if (menuID.Contains("__"))
                    {
                        menuID = menuID.Substring(0, menuID.IndexOf("__"));
                        queryString = "&" + menuID.Substring(menuID.IndexOf("__") + 2);
                    }
                    redirectUrl = PageUtils.GetAdminDirectoryUrl(string.Format("{0}.aspx?isQCloud=True&publishmentSystemID={1}{2}", menuID.Replace("-", "/"), publishmentSystemID, queryString));
                }
            }
            else if (!string.IsNullOrEmpty(key) && publishmentSystemID > 0)
            {
                int channelID = DataProvider.NodeDAO.GetNodeIDByNodeIndexName(publishmentSystemID, key);
                if (channelID == 0)
                {
                    Hashtable hashtable = NodeManager.GetNodeInfoHashtableByPublishmentSystemID(publishmentSystemID);
                    foreach (int nodeID in hashtable.Keys)
                    {
                        NodeInfo nodeInfo = hashtable[nodeID] as NodeInfo;
                        if (nodeInfo.NodeName == key)
                        {
                            channelID = nodeID;
                        }
                    }
                }
                if (channelID == 0)
                {
                    channelID = publishmentSystemID;
                }
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                NodeInfo channelInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                redirectUrl = PageUtils.AddProtocolToUrl(PageUtility.GetChannelUrl(publishmentSystemInfo, channelInfo, EVisualType.Static));
            }

            return redirectUrl;
        }

        private string Action_QCloud()
        {
            string redirectUrl = string.Empty;

            string token = base.Request.QueryString["token"];
            string menuID = base.Request.QueryString["menuID"];
            string key = base.Request.QueryString["key"];
            string openID = base.Request.QueryString["openID"];

            bool isSuccess = false;
            string errorMessage = string.Empty;

            if (AdminManager.Current.UserName == openID)
            {
                redirectUrl = this.GetQCloudRedirectUrl(menuID, key);
            }

            try
            {
                if (!BaiRongDataProvider.AdministratorDAO.IsUserNameExists(openID))
                {
                    AdministratorInfo administratorInfo = new AdministratorInfo();
                    administratorInfo.UserName = openID;
                    administratorInfo.Password = StringUtils.GetShortGUID();
                    administratorInfo.Question = string.Empty;
                    administratorInfo.Answer = string.Empty;
                    administratorInfo.Email = string.Empty;

                    isSuccess = AdminManager.CreateAdministrator(administratorInfo, out errorMessage);
                    RoleManager.AddUserToRole(administratorInfo.UserName, EPredefinedRoleUtils.GetValue(EPredefinedRole.SystemAdministrator));
                }

                BaiRongDataProvider.AdministratorDAO.Login(openID, true);
                redirectUrl = this.GetQCloudRedirectUrl(menuID, key);

                HttpContext.Current.Response.AddHeader("P3P", "CP=CAO PSA OUR");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            //string isvKey = FileConfigManager.Instance.SSOConfig.ISVKey;
            //string callBackUrl = FileConfigManager.Instance.SSOConfig.CallbackUrl;
            //callBackUrl = callBackUrl.Replace("${token}", token);
            //callBackUrl = callBackUrl.Replace("${isvKey}", isvKey);
            //callBackUrl = callBackUrl.Replace("${integrationType}", EIntegrationTypeUtils.GetValue(EIntegrationType.QCloud));

            //try
            //{
            //    string jsonString = WebClientUtils.GetRemoteFileSource(callBackUrl, ECharset.utf_8);
            //    NameValueCollection attributes = TranslateUtils.ParseJsonStringToNameValueCollection(jsonString);
            //    if (attributes != null)
            //    {
            //        isSuccess = TranslateUtils.ToBool(attributes["isSuccess"]);
            //        string userName = attributes["userName"];
            //        if (isSuccess && !string.IsNullOrEmpty(userName))
            //        {
            //            if (!BaiRongDataProvider.AdministratorDAO.IsUserNameExists(userName))
            //            {
            //                AdministratorInfo administratorInfo = new AdministratorInfo();
            //                administratorInfo.UserName = userName;
            //                administratorInfo.Password = StringUtils.GetShortGUID();
            //                administratorInfo.Question = string.Empty;
            //                administratorInfo.Answer = string.Empty;
            //                administratorInfo.Email = string.Empty;

            //                isSuccess = AdminManager.CreateAdministrator(administratorInfo, out errorMessage);
            //                RoleManager.AddUserToRole(administratorInfo.UserName, EPredefinedRoleUtils.GetValue(EPredefinedRole.SystemAdministrator));
            //            }

            //            BaiRongDataProvider.AdministratorDAO.Login(userName, true);

            //            PageUtils.Redirect(this.GetQCloudRedirectUrl(menuID, key));
            //        }
            //        else
            //        {
            //            isSuccess = false;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    errorMessage = ex.Message;
            //}
            return redirectUrl;
        }

        private string Action_Aliyun()
        {
            string token = base.Request.QueryString["token"];

            if (string.IsNullOrEmpty(token))
            {
                return "?token=token";
            }

            bool isSuccess = false;
            string errorMessage = string.Empty;

            string isvKey = FileConfigManager.Instance.SSOConfig.ISVKey;
            string qxbUrl = FileConfigManager.Instance.SSOConfig.CallbackUrl;
            qxbUrl = qxbUrl.Replace("${token}", token);
            qxbUrl = qxbUrl.Replace("${isvKey}", isvKey);

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
            catch (Exception ex)
            {
                errorMessage = ex.Message;
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
    }
}
