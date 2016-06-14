
using System;
using System.Text;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using System.Collections;
using System.Collections.Generic;
using BaiRong.Model;

namespace BaiRong.Core
{
    public class AppManager
    {
        private AppManager() { }

        public static void InstallApp(string appID)
        {
            string sqlPath = PathUtils.GetInstallerAppSqlFilePath(appID, BaiRongDataProvider.DatabaseType);
            if (FileUtils.IsFileExists(sqlPath))
            {
                StringBuilder errorBuilder = new StringBuilder();
                BaiRongDataProvider.DatabaseDAO.ExecuteSqlInFile(sqlPath, errorBuilder);
            }
            if (appID == AppManager.Platform.AppID)
            {
                BaiRongDataProvider.TableCollectionDAO.CreateAllAuxiliaryTableIfNotExists();
            }
        }

        public class Apps
        {
            public class Permission
            {
                public class General
                {
                    private General() { }

                    public const string Auxiliary = "siteserver_auxiliary";                        //辅助表管理
                    public const string Site = "siteserver_site";                                  //系统应用管理
                    public const string User = "siteserver_user";                                  //应用用户管理
                    public const string SiteSettings = "siteserver_siteSettings";                  //应用通用设置
                    public const string Log = "siteserver_log";                                    //后台运行记录
                }

                public class WebSite
                {
                    private WebSite() { }

                    public const string ContentModel = "siteserver_contentModel";                  //内容模型
                    public const string Category = "siteserver_category";                          //归类管理
                    public const string SiteAnalysis = "siteserver_siteAnalysis";                  //应用数据统计
                    public const string ContentTrash = "siteserver_contentTrash";                  //内容回收站

                    public const string InputContentView = "siteserver_inputContentView";    //提交表单查看
                    public const string InputContentEdit = "siteserver_inputContentEdit";    //提交表单维护
                    public const string Input = "siteserver_input";                  //提交表单管理
                    public const string Gather = "siteserver_gather";                //信息采集管理
                    public const string Advertisement = "siteserver_advertisement";  //广告管理
                    public const string Resume = "siteserver_resume";                 //在线招聘管理
                    public const string Mail = "siteserver_mail";                    //邮件发送管理
                    public const string SEO = "siteserver_seo";                      //搜索引擎优化
                    public const string Tracking = "siteserver_tracking";            //流量统计管理
                    public const string InnerLink = "siteserver_innerLink";          //站内链接管理
                    public const string Restriction = "siteserver_restriction";                  //页面访问限制
                    public const string Backup = "siteserver_backup";                //数据备份恢复
                    public const string Signin = "siteserver_signin";            //内容签收管理
                    public const string Archive = "siteserver_archive";            //归档内容管理
                    public const string FileManagement = "siteserver_fileManagement";            //应用文件管理
                    public const string AllPublish = "siteserver_allPublish";            //多服务器发布
                    public const string BShare = "siteserver_bShare";            //bShare分享插件

                    public const string Template = "siteserver_template";            //显示管理

                    public const string Configration = "siteserver_configration";    //设置管理

                    public const string Create = "siteserver_create";                //生成管理
                }

                public class Channel
                {
                    private Channel() { }
                    public const string ContentView = "siteserver_contentView";
                    public const string ContentAdd = "siteserver_contentAdd";
                    public const string ContentEdit = "siteserver_contentEdit";
                    public const string ContentDelete = "siteserver_contentDelete";
                    public const string ContentTranslate = "siteserver_contentTranslate";
                    public const string ContentArchive = "siteserver_contentArchive";
                    public const string ContentOrder = "siteserver_contentOrder";
                    public const string ChannelAdd = "siteserver_channelAdd";
                    public const string ChannelEdit = "siteserver_channelEdit";
                    public const string ChannelDelete = "siteserver_channelDelete";
                    public const string ChannelTranslate = "siteserver_channelTranslate";
                    public const string CommentCheck = "siteserver_commentCheck";
                    public const string CommentDelete = "siteserver_commentDelete";
                    public const string CreatePage = "siteserver_createPage";
                    public const string PublishPage = "siteserver_publishPage";
                    public const string ContentCheck = "siteserver_contentCheck";
                    public const string ContentCheckLevel1 = "siteserver_contentCheckLevel1";
                    public const string ContentCheckLevel2 = "siteserver_contentCheckLevel2";
                    public const string ContentCheckLevel3 = "siteserver_contentCheckLevel3";
                    public const string ContentCheckLevel4 = "siteserver_contentCheckLevel4";
                    public const string ContentCheckLevel5 = "siteserver_contentCheckLevel5";
                }
            }
        }

        public class Platform
        {
            public const string AppID = "platform";

            public class TopMenu
            {
                public const string ID_Admin = "Admin";
                public const string ID_Platform = "Platform";
                public const string ID_Service = "Service";
                /******************用户中心********************/
                public const string ID_UserCenter = "UserCenter";
                public const string ID_Statistics = "Statistics";

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Admin)
                    {
                        retval = "管理员管理";
                    }
                    else if (menuID == ID_Platform)
                    {
                        retval = "平台设置";
                    }
                    else if (menuID == ID_Service)
                    {
                        retval = "服务管理";
                    }
                    else if (menuID == ID_UserCenter)
                    {
                        retval = "用户中心";
                    }
                    else if (menuID == ID_Statistics)
                    {
                        retval = "数据统计";
                    }
                    return retval;
                }
            }

            public class LeftMenu
            {
                public const string ID_Administrator = "Administrator";     //管理员管理
                public const string ID_AdminAttributes = "ID_AdminAttributes";   //用户属性设置

                public const string ID_Configuration = "Configuration";     //平台参数设置
                public const string ID_Product = "Product";                 //平台产品管理
                public const string ID_Authentication = "Authentication";   //用户认证设置
                public const string ID_SMS = "SMS";                         //短信通设置
                public const string ID_Restriction = "Restriction";         //后台访问限制
                public const string ID_Database = "Database";               //数据库工具
                public const string ID_Utility = "Utility";                 //实用工具
                public const string ID_Log = "Log";                         //后台运行记录

                public const string ID_Service = "Service";                 //服务监控
                public const string ID_Storage = "Storage";                 //云存储管理

                public class Configuration
                {
                    public const string ID_ConfigurationMenu = "ConfigurationMenu";     //平台菜单设置
                }

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Administrator)
                    {
                        retval = "管理员管理";
                    }
                    else if (menuID == ID_AdminAttributes)
                    {
                        retval = "部门与区域设置";
                    }
                    else if (menuID == ID_Configuration)
                    {
                        retval = "平台参数设置";
                    }
                    else if (menuID == ID_Product)
                    {
                        retval = "平台产品管理";
                    }
                    else if (menuID == ID_Authentication)
                    {
                        retval = "用户认证设置";
                    }
                    else if (menuID == ID_SMS)
                    {
                        retval = "短信通设置";
                    }
                    else if (menuID == ID_Restriction)
                    {
                        retval = "后台访问限制";
                    }
                    else if (menuID == ID_Database)
                    {
                        retval = "数据库工具";
                    }
                    else if (menuID == ID_Utility)
                    {
                        retval = "实用工具";
                    }
                    else if (menuID == ID_Log)
                    {
                        retval = "后台运行记录";
                    }
                    else if (menuID == ID_Service)
                    {
                        retval = "服务监控";
                    }
                    else if (menuID == ID_Storage)
                    {
                        retval = "云存储管理";
                    }
                    return retval;
                }

                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    //Configuration
                    if (menuID == Configuration.ID_ConfigurationMenu)
                    {
                        retval = "平台菜单设置";
                    }
                    return retval;
                }
            }

            public class Permission
            {
                public const string Platform_Auxiliary = "platform_auxiliary";
                public const string Platform_Site = "platform_site";
                public const string Platform_SiteSettings = "platform_siteSettings";

                public const string Platform_Administrator = "platform_administrator";
                public const string Platform_AdminAttributes = "platform_adminAttributes";
                public const string Platform_Users = "platform_users";

                public const string Platform_Configuration = "platform_configuration";
                public const string Platform_Product = "platform_product";
                public const string Platform_Authentication = "platform_authentication";
                public const string platform_SMS = "platform_sms";
                public const string platform_Email = "platform_email";
                public const string Platform_Restriction = "platform_restriction";
                public const string Platform_Utility = "platform_utility";
                public const string Platform_Log = "platform_log";

                public const string Platform_Service = "platform_service";
                public const string Platform_Storage = "platform_storage";

                public const string Platform_Organization = "platform_organization";

                public const string Platform_Statistics = "platform_statistics";
            }

            public class Service
            {
                public const string AppID = "service";
            }
        }

        public class CMS
        {
            public const string AppID = "cms";

            public class TopMenu
            {
                public const string ID_SiteManagement = "SiteManagement";
                public const string ID_SiteConfiguration = "SiteConfiguration";

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_SiteManagement)
                    {
                        retval = "应用管理";
                    }
                    else if (menuID == ID_SiteConfiguration)
                    {
                        retval = "系统管理";
                    }
                    return retval;
                }
            }

            public class LeftMenu
            {
                public const string ID_Content = "Content";
                public const string ID_GovPublic = "GovPublic";
                public const string ID_GovInteract = "GovInteract";
                public const string ID_Function = "Function";
                public const string ID_Template = "Template";
                public const string ID_User = "User";
                public const string ID_Configration = "Configration";
                public const string ID_Create = "Create";

                public const string ID_Auxiliary = "Auxiliary";
                public const string ID_Site = "Site";
                public const string ID_SiteSettings = "SiteSettings";
                public const string ID_Log = "Log";
                public const string ID_ThirdLog = "ThirdLog";

                public const string ID_Organization = "Organization";

                public const string ID_Statistics = "Statistics";

                public class Content
                {
                    public const string ID_ContentModel = "ContentModel";
                    public const string ID_Category = "Category";
                    public const string ID_SiteAnalysis = "SiteAnalysis";
                }

                public class GovPublic
                {
                    public const string ID_GovPublicContent = "GovPublicContent";     //主动信息公开
                    public const string ID_GovPublicApply = "GovPublicApply";         //依申请公开
                    public const string ID_GovPublicContentConfiguration = "GovPublicContentConfiguration";         //主动信息公开设置
                    public const string ID_GovPublicApplyConfiguration = "GovPublicApplyConfiguration";         //依申请公开设置
                    public const string ID_GovPublicAnalysis = "GovPublicAnalysis";         //数据统计分析
                }

                public class GovInteract
                {
                    public const string ID_GovInteractConfiguration = "GovInteractConfiguration";     //互动交流设置
                    public const string ID_GovInteractAnalysis = "GovInteractAnalysis";             //数据统计分析
                }

                public class Function
                {
                    public const string ID_Input = "Input";
                    public const string ID_Gather = "Gather";
                    public const string ID_Advertisement = "Advertisement";
                    public const string ID_Resume = "Resume";
                    public const string ID_Mail = "Mail";
                    public const string ID_SEO = "SEO";
                    public const string ID_Tracking = "Tracking";
                    public const string ID_InnerLink = "InnerLink";
                    public const string ID_Restriction = "Restriction";
                    public const string ID_Backup = "Backup";
                    public const string ID_BShare = "BShare";
                    public const string ID_WebsiteMessage = "WebsiteMessage";
                    public const string ID_Subscribe = "Subscribe";//by 20151030 sofuny 信息订阅
                    public const string ID_IntelligentPush = "IntelligentPush";//by 20151124 sofuny 智能推送
                    public const string ID_Evaluation = "Evaluation";//by 20160224 sofuny 评价管理
                    public const string ID_Trial = "Trial";//by 20160303 sofuny 试用管理
                    public const string ID_Survey = "Survey";//by 20160309 sofuny 调查问卷管理
                    public const string ID_Compare = "Compare";//by 20160316 sofuny 比较反馈管理
                    public const string ID_Searchword = "Searchword";
                    public const string ID_Special = "Special";
                    public const string ID_AdvImage = "AdvImage";
                }

                public class Template
                {
                    public const string ID_TagStyle = "TagStyle";     //模板标签样式
                }

                public class Configuration
                {
                    public const string ID_ConfigurationCreate = "ConfigurationCreate";         //页面生成设置
                    public const string ID_ConfigurationStorage = "ConfigurationStorage";       //存储空间设置
                    public const string ID_ConfigurationTask = "ConfigurationTask";             //定时任务管理
                    public const string ID_ConfigurationTouGao = "ConfigurationTouGao";         //内容投稿设置
                    public const string ID_ConfigurationComment = "ConfigurationComment";       //内容评论设置
                    public const string ID_ConfigurationMachine = "ConfigurationMachine";       //服务器管理
                }

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Content)
                    {
                        retval = "信息管理";
                    }
                    else if (menuID == ID_GovPublic)
                    {
                        retval = "信息公开";
                    }
                    else if (menuID == ID_GovInteract)
                    {
                        retval = "互动交流";
                    }
                    else if (menuID == ID_Function)
                    {
                        retval = "功能管理";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "显示管理";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "用户管理";
                    }
                    else if (menuID == ID_Configration)
                    {
                        retval = "设置管理";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "生成管理";
                    }

                    else if (menuID == ID_Auxiliary)
                    {
                        retval = "辅助表管理";
                    }
                    else if (menuID == ID_Site)
                    {
                        retval = "系统应用管理";
                    }
                    else if (menuID == ID_SiteSettings)
                    {
                        retval = "应用通用设置";
                    }
                    else if (menuID == ID_Log)
                    {
                        retval = "后台运行记录";
                    }
                    else if (menuID == ID_Organization)
                    {
                        retval = "分支机构管理";
                    }
                    return retval;
                }

                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    //Content
                    if (menuID == Content.ID_ContentModel)
                    {
                        retval = "内容模型";
                    }
                    else if (menuID == Content.ID_Category)
                    {
                        retval = "归类管理";
                    }
                    else if (menuID == Content.ID_SiteAnalysis)
                    {
                        retval = "应用数据统计";
                    }
                    //GovPublic
                    else if (menuID == GovPublic.ID_GovPublicContent)
                    {
                        retval = "主动信息公开";
                    }
                    else if (menuID == GovPublic.ID_GovPublicApply)
                    {
                        retval = "依申请公开";
                    }
                    else if (menuID == GovPublic.ID_GovPublicContentConfiguration)
                    {
                        retval = "主动信息公开设置";
                    }
                    else if (menuID == GovPublic.ID_GovPublicApplyConfiguration)
                    {
                        retval = "依申请公开设置";
                    }
                    else if (menuID == GovPublic.ID_GovPublicAnalysis)
                    {
                        retval = "数据统计分析";
                    }
                    //GovInteract
                    else if (menuID == GovInteract.ID_GovInteractConfiguration)
                    {
                        retval = "互动交流设置";
                    }
                    else if (menuID == GovInteract.ID_GovInteractAnalysis)
                    {
                        retval = "数据统计分析";
                    }
                    //Function
                    else if (menuID == Function.ID_Input)
                    {
                        retval = "提交表单";
                    }
                    else if (menuID == Function.ID_WebsiteMessage)
                    {
                        retval = "网站留言";
                    }
                    else if (menuID == Function.ID_Searchword)
                    {
                        retval = "搜索关键字";
                    }
                    else if (menuID == Function.ID_Special)
                    {
                        retval = "专题管理";
                    }
                    else if (menuID == Function.ID_AdvImage)
                    {
                        retval = "广告管理";
                    }
                    else if (menuID == Function.ID_Gather)
                    {
                        retval = "信息采集管理";
                    }
                    else if (menuID == Function.ID_Advertisement)
                    {
                        retval = "广告管理";
                    }
                    else if (menuID == Function.ID_Resume)
                    {
                        retval = "简历管理";
                    }
                    else if (menuID == Function.ID_Mail)
                    {
                        retval = "邮件发送管理";
                    }
                    else if (menuID == Function.ID_SEO)
                    {
                        retval = "搜索引擎优化";
                    }
                    else if (menuID == Function.ID_Tracking)
                    {
                        retval = "流量统计管理";
                    }
                    else if (menuID == Function.ID_InnerLink)
                    {
                        retval = "站内链接管理";
                    }
                    else if (menuID == Function.ID_Restriction)
                    {
                        retval = "页面访问限制";
                    }
                    else if (menuID == Function.ID_Backup)
                    {
                        retval = "数据备份恢复";
                    }
                    else if (menuID == Function.ID_BShare)
                    {
                        retval = "bShare分享插件";
                    }
                    else if (menuID == Function.ID_Subscribe)// by 20151030 sofuny 
                    {
                        retval = "信息订阅";
                    }
                    else if (menuID == Function.ID_IntelligentPush)// by 20151124 sofuny 
                    {
                        retval = "智能推送";
                    }
                    else if (menuID == Function.ID_Evaluation)// by 20160224 sofuny 
                    {
                        retval = "评价管理";
                    }
                    else if (menuID == Function.ID_Trial)// by 20160224 sofuny 
                    {
                        retval = "试用管理";
                    }
                    else if (menuID == Function.ID_Survey)// by 20160309 sofuny 
                    {
                        retval = "调查问卷管理";
                    }
                    else if (menuID == Function.ID_Compare)// by 20160316 sofuny 
                    {
                        retval = "比较反馈管理";
                    }
                    //Template
                    else if (menuID == Template.ID_TagStyle)
                    {
                        retval = "模板标签样式";
                    }
                    //Configuration
                    else if (menuID == Configuration.ID_ConfigurationCreate)
                    {
                        retval = "页面生成设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationStorage)
                    {
                        retval = "存储空间设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTask)
                    {
                        retval = "定时任务管理";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTouGao)
                    {
                        retval = "内容投稿设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationComment)
                    {
                        retval = "内容评论设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationMachine)
                    {
                        retval = "服务器管理";
                    }
                    return retval;
                }
            }

            public class Permission
            {
                //public class General
                //{
                //    private General() { }

                //    public const string Auxiliary = "cms_auxiliary";                        //辅助表管理
                //    public const string Site = "cms_site";                                  //系统应用管理
                //    public const string User = "cms_user";                                  //应用用户管理
                //    public const string SiteSettings = "cms_siteSettings";                  //应用通用设置
                //    public const string Log = "cms_log";                                    //后台运行记录
                //}

                public class WebSite
                {
                    private WebSite() { }

                    public const string ContentModel = "cms_contentModel";                  //内容模型
                    public const string Category = "cms_category";                          //归类管理
                    public const string SiteAnalysis = "cms_siteAnalysis";                  //应用数据统计
                    public const string ContentTrash = "cms_contentTrash";                  //内容回收站

                    public const string GovPublicContent = "cms_govPublicContent";          //主动信息公开
                    public const string GovPublicApply = "cms_govPublicApply";          //依申请公开
                    public const string GovPublicContentConfiguration = "cms_govPublicContentConfiguration";          //主动信息公开设置
                    public const string GovPublicApplyConfiguration = "cms_govPublicApplyConfiguration";          //依申请公开设置
                    public const string GovPublicAnalysis = "cms_govPublicAnalysis";          //信息公开统计

                    public const string GovInteract = "cms_govInteract";                     //互动交流管理
                    public const string GovInteractConfiguration = "cms_govInteractConfiguration";                     //互动交流设置
                    public const string GovInteractAnalysis = "cms_govInteractAnalysis";                     //互动交流统计

                    public const string InputContentView = "cms_inputContentView";    //提交表单查看
                    public const string InputContentEdit = "cms_inputContentEdit";    //提交表单维护
                    public const string Input = "cms_input";                  //提交表单管理
                    public const string InputPermission = "cms_inputPermission";    // 表单权限管理 by 20151205 sofuny
                    public const string InputClassifyView = "cms_inputClassifyView";    //提交表单分类查看 by 20151205 sofuny
                    public const string InputClassifyEdit = "cms_inputClassifyEdit";    //提交表单分类维护 by 20151205 sofuny
                    public const string Gather = "cms_gather";                //信息采集管理
                    public const string Advertisement = "cms_advertisement";  //广告管理
                    public const string Resume = "cms_resume";                 //在线招聘管理
                    public const string Mail = "cms_mail";                    //邮件发送管理
                    public const string SEO = "cms_seo";                      //搜索引擎优化
                    public const string Tracking = "cms_tracking";            //流量统计管理
                    public const string InnerLink = "cms_innerLink";          //站内链接管理
                    public const string Restriction = "cms_restriction";                  //页面访问限制
                    public const string Backup = "cms_backup";                //数据备份恢复
                    public const string Archive = "cms_archive";            //归档内容管理
                    public const string FileManagement = "cms_fileManagement";            //应用文件管理
                    public const string AllPublish = "cms_allPublish";            //多服务器发布
                    public const string BShare = "cms_bShare";            //bShare分享插件

                    public const string WebsiteMessageContentView = "cms_websiteMessageContentView";    //站内留言查看
                    public const string WebsiteMessageContentEdit = "cms_websiteMessageContentEdit";    //站内留言维护
                    public const string WebsiteMessage = "cms_websiteMessage";                  //站内留言管理

                    public const string Searchword = "cms_searchword";                  //搜索关键词管理

                    public const string SpecialContentView = "cms_specialContentView";    //专题内容查看
                    public const string SpecialContentEdit = "cms_specialContentEdit";    //专题内容维护
                    public const string Special = "cms_special";                  //专题管理

                    public const string AdvImageContentView = "cms_advImageContentView";    //广告内容查看
                    public const string AdvImageContentEdit = "cms_advImageContentEdit";    //广告内容维护
                    public const string AdvImage = "cms_advImage";                  //广告管理


                    public const string Template = "cms_template";            //显示管理

                    public const string User = "cms_user";            //用户管理

                    public const string Configration = "cms_configration";    //设置管理

                    public const string Create = "cms_create";                //生成管理


                    // by sofuny
                    public const string Subscribe = "cms_subscribe";                  //信息订阅
                    public const string SubscribeView = "cms_subscribeView";    //信息订阅查看
                    public const string IntelligentPush = "cms_intelligentPush";    //智能推送
                    public const string Evaluation = "cms_evaluation";    //评价管理
                    public const string Trial = "cms_trial";    //试用管理
                    public const string Survey = "cms_survey";    //调查问卷管理
                    public const string Compare = "cms_compare";    //比较反馈管理
                }

                public class Channel
                {
                    private Channel() { }
                    public const string ContentView = "cms_contentView";
                    public const string ContentAdd = "cms_contentAdd";
                    public const string ContentEdit = "cms_contentEdit";
                    public const string ContentDelete = "cms_contentDelete";
                    public const string ContentTranslate = "cms_contentTranslate";
                    public const string ContentArchive = "cms_contentArchive";
                    public const string ContentOrder = "cms_contentOrder";
                    public const string ChannelAdd = "cms_channelAdd";
                    public const string ChannelEdit = "cms_channelEdit";
                    public const string ChannelDelete = "cms_channelDelete";
                    public const string ChannelTranslate = "cms_channelTranslate";
                    public const string CommentCheck = "cms_commentCheck";
                    public const string CommentDelete = "cms_commentDelete";
                    public const string CreatePage = "cms_createPage";
                    public const string PublishPage = "cms_publishPage";
                    public const string ContentCheck = "cms_contentCheck";
                    public const string ContentCheckLevel1 = "cms_contentCheckLevel1";
                    public const string ContentCheckLevel2 = "cms_contentCheckLevel2";
                    public const string ContentCheckLevel3 = "cms_contentCheckLevel3";
                    public const string ContentCheckLevel4 = "cms_contentCheckLevel4";
                    public const string ContentCheckLevel5 = "cms_contentCheckLevel5";
                }

                public class GovInteract
                {
                    private GovInteract() { }
                    public const string GovInteractView = "cms_govInteractView";
                    public const string GovInteractAdd = "cms_govInteractAdd";
                    public const string GovInteractEdit = "cms_govInteractEdit";
                    public const string GovInteractDelete = "cms_govInteractDelete";
                    public const string GovInteractSwitchToTranslate = "cms_govInteractSwitchToTranslate";
                    public const string GovInteractComment = "cms_govInteractComment";
                    public const string GovInteractAccept = "cms_govInteractAccept";
                    public const string GovInteractReply = "cms_govInteractReply";
                    public const string GovInteractCheck = "cms_govInteractCheck";

                    public static string GetPermissionName(string permission)
                    {
                        string retval = string.Empty;
                        if (permission == GovInteractView)
                        {
                            retval = "浏览办件";
                        }
                        else if (permission == GovInteractAdd)
                        {
                            retval = "新增办件";
                        }
                        else if (permission == GovInteractEdit)
                        {
                            retval = "编辑办件";
                        }
                        else if (permission == GovInteractDelete)
                        {
                            retval = "删除办件";
                        }
                        else if (permission == GovInteractSwitchToTranslate)
                        {
                            retval = "转办转移";
                        }
                        else if (permission == GovInteractComment)
                        {
                            retval = "批示办件";
                        }
                        else if (permission == GovInteractAccept)
                        {
                            retval = "受理办件";
                        }
                        else if (permission == GovInteractReply)
                        {
                            retval = "办理办件";
                        }
                        else if (permission == GovInteractCheck)
                        {
                            retval = "审核办件";
                        }
                        return retval;
                    }
                }
            }
        }

        public class WCM
        {
            public const string AppID = "wcm";

            public class TopMenu
            {
                public const string ID_SiteManagement = "SiteManagement";
                public const string ID_SiteConfiguration = "SiteConfiguration";

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_SiteManagement)
                    {
                        retval = "应用管理";
                    }
                    else if (menuID == ID_SiteConfiguration)
                    {
                        retval = "系统管理";
                    }
                    return retval;
                }
            }

            public class LeftMenu
            {
                public const string ID_Content = "Content";
                public const string ID_GovPublic = "GovPublic";
                public const string ID_GovInteract = "GovInteract";
                public const string ID_Function = "Function";
                public const string ID_Template = "Template";
                public const string ID_Configration = "Configration";
                public const string ID_Create = "Create";

                public const string ID_Auxiliary = "Auxiliary";
                public const string ID_Site = "Site";
                public const string ID_User = "User";
                public const string ID_SiteSettings = "SiteSettings";
                public const string ID_Log = "Log";

                public class Content
                {
                    public const string ID_ContentModel = "ContentModel";
                    public const string ID_Category = "Category";
                    public const string ID_SiteAnalysis = "SiteAnalysis";
                }

                public class GovPublic
                {
                    public const string ID_GovPublicContent = "GovPublicContent";     //主动信息公开
                    public const string ID_GovPublicApply = "GovPublicApply";         //依申请公开
                    public const string ID_GovPublicContentConfiguration = "GovPublicContentConfiguration";         //主动信息公开设置
                    public const string ID_GovPublicApplyConfiguration = "GovPublicApplyConfiguration";         //依申请公开设置
                    public const string ID_GovPublicAnalysis = "GovPublicAnalysis";         //数据统计分析
                }

                public class GovInteract
                {
                    public const string ID_GovInteractConfiguration = "GovInteractConfiguration";     //互动交流设置
                    public const string ID_GovInteractAnalysis = "GovInteractAnalysis";             //数据统计分析
                }

                public class Function
                {
                    public const string ID_Input = "Input";
                    public const string ID_Gather = "Gather";
                    public const string ID_Advertisement = "Advertisement";
                    public const string ID_Resume = "Resume";
                    public const string ID_Mail = "Mail";
                    public const string ID_SEO = "SEO";
                    public const string ID_Tracking = "Tracking";
                    public const string ID_InnerLink = "InnerLink";
                    public const string ID_Restriction = "Restriction";
                    public const string ID_Backup = "Backup";
                    public const string ID_Signin = "Signin";
                    public const string ID_BShare = "BShare";
                }

                public class Template
                {
                    public const string ID_TagStyle = "TagStyle";     //模板标签样式
                }

                public class Configuration
                {
                    public const string ID_ConfigurationCreate = "ConfigurationCreate";         //页面生成设置
                    public const string ID_ConfigurationStorage = "ConfigurationStorage";       //存储空间设置
                    public const string ID_ConfigurationTask = "ConfigurationTask";             //定时任务管理
                    public const string ID_ConfigurationTouGao = "ConfigurationTouGao";   //内容投稿设置
                    public const string ID_ConfigurationComment = "ConfigurationComment";   //内容评论设置
                    public const string ID_ConfigurationMachine = "ConfigurationMachine";   //服务器管理
                }

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Content)
                    {
                        retval = "信息管理";
                    }
                    else if (menuID == ID_GovPublic)
                    {
                        retval = "信息公开";
                    }
                    else if (menuID == ID_GovInteract)
                    {
                        retval = "互动交流";
                    }
                    else if (menuID == ID_Function)
                    {
                        retval = "功能管理";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "显示管理";
                    }
                    else if (menuID == ID_Configration)
                    {
                        retval = "设置管理";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "生成管理";
                    }

                    else if (menuID == ID_Auxiliary)
                    {
                        retval = "辅助表管理";
                    }
                    else if (menuID == ID_Site)
                    {
                        retval = "系统应用管理";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "应用用户管理";
                    }
                    else if (menuID == ID_SiteSettings)
                    {
                        retval = "应用通用设置";
                    }
                    else if (menuID == ID_Log)
                    {
                        retval = "后台运行记录";
                    }
                    return retval;
                }

                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    //Content
                    if (menuID == Content.ID_ContentModel)
                    {
                        retval = "内容模型";
                    }
                    else if (menuID == Content.ID_Category)
                    {
                        retval = "归类管理";
                    }
                    else if (menuID == Content.ID_SiteAnalysis)
                    {
                        retval = "应用数据统计";
                    }
                    //GovPublic
                    else if (menuID == GovPublic.ID_GovPublicContent)
                    {
                        retval = "主动信息公开";
                    }
                    else if (menuID == GovPublic.ID_GovPublicApply)
                    {
                        retval = "依申请公开";
                    }
                    else if (menuID == GovPublic.ID_GovPublicContentConfiguration)
                    {
                        retval = "主动信息公开设置";
                    }
                    else if (menuID == GovPublic.ID_GovPublicApplyConfiguration)
                    {
                        retval = "依申请公开设置";
                    }
                    else if (menuID == GovPublic.ID_GovPublicAnalysis)
                    {
                        retval = "数据统计分析";
                    }
                    //GovInteract
                    else if (menuID == GovInteract.ID_GovInteractConfiguration)
                    {
                        retval = "互动交流设置";
                    }
                    else if (menuID == GovInteract.ID_GovInteractAnalysis)
                    {
                        retval = "数据统计分析";
                    }
                    //Function
                    else if (menuID == Function.ID_Input)
                    {
                        retval = "提交表单";
                    }
                    else if (menuID == Function.ID_Gather)
                    {
                        retval = "信息采集管理";
                    }
                    else if (menuID == Function.ID_Advertisement)
                    {
                        retval = "广告管理";
                    }
                    else if (menuID == Function.ID_Resume)
                    {
                        retval = "简历管理";
                    }
                    else if (menuID == Function.ID_Mail)
                    {
                        retval = "邮件发送管理";
                    }
                    else if (menuID == Function.ID_SEO)
                    {
                        retval = "搜索引擎优化";
                    }
                    else if (menuID == Function.ID_Tracking)
                    {
                        retval = "流量统计管理";
                    }
                    else if (menuID == Function.ID_InnerLink)
                    {
                        retval = "站内链接管理";
                    }
                    else if (menuID == Function.ID_Restriction)
                    {
                        retval = "页面访问限制";
                    }
                    else if (menuID == Function.ID_Backup)
                    {
                        retval = "数据备份恢复";
                    }
                    else if (menuID == Function.ID_Signin)
                    {
                        retval = "内容签收管理";
                    }
                    else if (menuID == Function.ID_BShare)
                    {
                        retval = "bShare分享插件";
                    }
                    //Template
                    else if (menuID == Template.ID_TagStyle)
                    {
                        retval = "模板标签样式";
                    }
                    //Configuration
                    else if (menuID == Configuration.ID_ConfigurationCreate)
                    {
                        retval = "页面生成设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationStorage)
                    {
                        retval = "存储空间设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTask)
                    {
                        retval = "定时任务管理";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTouGao)
                    {
                        retval = "内容投稿设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationComment)
                    {
                        retval = "内容评论设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationMachine)
                    {
                        retval = "服务器管理";
                    }
                    return retval;
                }
            }

            public class Permission
            {
                public class General
                {
                    private General() { }

                    public const string Auxiliary = "wcm_auxiliary";                        //辅助表管理
                    public const string Site = "wcm_site";                                  //系统应用管理
                    public const string User = "wcm_user";                                  //应用用户管理
                    public const string SiteSettings = "wcm_siteSettings";                  //应用通用设置
                    public const string Log = "wcm_log";                                    //后台运行记录
                }

                public class WebSite
                {
                    private WebSite() { }

                    public const string ContentModel = "wcm_contentModel";                  //内容模型
                    public const string Category = "wcm_category";                          //归类管理
                    public const string SiteAnalysis = "wcm_siteAnalysis";                  //应用数据统计
                    public const string ContentTrash = "wcm_contentTrash";                  //内容回收站

                    public const string GovPublicContent = "wcm_govPublicContent";          //主动信息公开
                    public const string GovPublicApply = "wcm_govPublicApply";          //依申请公开
                    public const string GovPublicContentConfiguration = "wcm_govPublicContentConfiguration";          //主动信息公开设置
                    public const string GovPublicApplyConfiguration = "wcm_govPublicApplyConfiguration";          //依申请公开设置
                    public const string GovPublicAnalysis = "wcm_govPublicAnalysis";          //信息公开统计

                    public const string GovInteract = "wcm_govInteract";                     //互动交流管理
                    public const string GovInteractConfiguration = "wcm_govInteractConfiguration";                     //互动交流设置
                    public const string GovInteractAnalysis = "wcm_govInteractAnalysis";                     //互动交流统计

                    public const string InputContentView = "wcm_inputContentView";    //提交表单查看
                    public const string InputContentEdit = "wcm_inputContentEdit";    //提交表单维护
                    public const string Input = "wcm_input";                  //提交表单管理
                    public const string Gather = "wcm_gather";                //信息采集管理
                    public const string Advertisement = "wcm_advertisement";  //广告管理
                    public const string Resume = "wcm_resume";                 //在线招聘管理
                    public const string Mail = "wcm_mail";                    //邮件发送管理
                    public const string SEO = "wcm_seo";                      //搜索引擎优化
                    public const string Tracking = "wcm_tracking";            //流量统计管理
                    public const string InnerLink = "wcm_innerLink";          //站内链接管理
                    public const string Restriction = "wcm_restriction";                  //页面访问限制
                    public const string Backup = "wcm_backup";                //数据备份恢复
                    public const string Signin = "wcm_signin";            //内容签收管理
                    public const string Archive = "wcm_archive";            //归档内容管理
                    public const string FileManagement = "wcm_fileManagement";            //应用文件管理
                    public const string AllPublish = "wcm_allPublish";            //多服务器发布
                    public const string BShare = "wcm_bShare";            //bShare分享插件

                    public const string Template = "wcm_template";            //显示管理

                    public const string Configration = "wcm_configration";    //设置管理

                    public const string Create = "wcm_create";                //生成管理
                }

                public class Channel
                {
                    private Channel() { }
                    public const string ContentView = "wcm_contentView";
                    public const string ContentAdd = "wcm_contentAdd";
                    public const string ContentEdit = "wcm_contentEdit";
                    public const string ContentDelete = "wcm_contentDelete";
                    public const string ContentTranslate = "wcm_contentTranslate";
                    public const string ContentArchive = "wcm_contentArchive";
                    public const string ContentOrder = "wcm_contentOrder";
                    public const string ChannelAdd = "wcm_channelAdd";
                    public const string ChannelEdit = "wcm_channelEdit";
                    public const string ChannelDelete = "wcm_channelDelete";
                    public const string ChannelTranslate = "wcm_channelTranslate";
                    public const string CommentCheck = "wcm_commentCheck";
                    public const string CommentDelete = "wcm_commentDelete";
                    public const string CreatePage = "wcm_createPage";
                    public const string PublishPage = "wcm_publishPage";
                    public const string ContentCheck = "wcm_contentCheck";
                    public const string ContentCheckLevel1 = "wcm_contentCheckLevel1";
                    public const string ContentCheckLevel2 = "wcm_contentCheckLevel2";
                    public const string ContentCheckLevel3 = "wcm_contentCheckLevel3";
                    public const string ContentCheckLevel4 = "wcm_contentCheckLevel4";
                    public const string ContentCheckLevel5 = "wcm_contentCheckLevel5";
                }

                public class GovInteract
                {
                    private GovInteract() { }
                    public const string GovInteractView = "wcm_govInteractView";
                    public const string GovInteractAdd = "wcm_govInteractAdd";
                    public const string GovInteractEdit = "wcm_govInteractEdit";
                    public const string GovInteractDelete = "wcm_govInteractDelete";
                    public const string GovInteractSwitchToTranslate = "wcm_govInteractSwitchToTranslate";
                    public const string GovInteractComment = "wcm_govInteractComment";
                    public const string GovInteractAccept = "wcm_govInteractAccept";
                    public const string GovInteractReply = "wcm_govInteractReply";
                    public const string GovInteractCheck = "wcm_govInteractCheck";

                    public static string GetPermissionName(string permission)
                    {
                        string retval = string.Empty;
                        if (permission == GovInteractView)
                        {
                            retval = "浏览办件";
                        }
                        else if (permission == GovInteractAdd)
                        {
                            retval = "新增办件";
                        }
                        else if (permission == GovInteractEdit)
                        {
                            retval = "编辑办件";
                        }
                        else if (permission == GovInteractDelete)
                        {
                            retval = "删除办件";
                        }
                        else if (permission == GovInteractSwitchToTranslate)
                        {
                            retval = "转办转移";
                        }
                        else if (permission == GovInteractComment)
                        {
                            retval = "批示办件";
                        }
                        else if (permission == GovInteractAccept)
                        {
                            retval = "受理办件";
                        }
                        else if (permission == GovInteractReply)
                        {
                            retval = "办理办件";
                        }
                        else if (permission == GovInteractCheck)
                        {
                            retval = "审核办件";
                        }
                        return retval;
                    }
                }
            }

            public class AuxiliaryTableName
            {
                public const string BackgroundContent = AppID + "_Content";
                public const string GovPublicContent = AppID + "_ContentGovPublic";
                public const string GovInteractContent = AppID + "_ContentGovInteract";
                public const string JobContent = AppID + "_ContentJob";
                public const string VoteContent = AppID + "_ContentVote";
                public const string UserDefined = AppID + "_ContentCustom";
            }
        }

        public class BBS
        {
            public const string AppID = "bbs";

            public class TopMenu
            {
                public const string ID_Forums = "Forums";

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Forums)
                    {
                        retval = "论坛管理";
                    }
                    return retval;
                }
            }

            public class LeftMenu
            {
                public const string ID_Forum = "Forum";             //版块管理
                public const string ID_Content = "Content";         //内容管理
                public const string ID_User = "User";               //用户管理
                public const string ID_Settings = "Settings";       //系统设置
                public const string ID_Template = "Template";       //界面管理
                public const string ID_Create = "Create";           //生成管理

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Forum)
                    {
                        retval = "版块管理";
                    }
                    else if (menuID == ID_Content)
                    {
                        retval = "内容管理";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "用户管理";
                    }
                    else if (menuID == ID_Settings)
                    {
                        retval = "系统设置";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "界面管理";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "生成管理";
                    }
                    return retval;
                }
            }

            public class Permission
            {
                public const string BBS_Forum = "bbs_forum";
                public const string BBS_Content = "bbs_content";
                public const string BBS_User = "bbs_user";
                public const string BBS_Settings = "bbs_settings";
                public const string BBS_Template = "bbs_template";
                public const string BBS_Create = "bbs_create";
            }
        }

        public class B2C
        {
            public const string AppID = "b2c";

            public class TopMenu
            {
                public const string ID_SiteManagement = "SiteManagement";
                public const string ID_SiteConfiguration = "SiteConfiguration";

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_SiteManagement)
                    {
                        retval = "商城管理";
                    }
                    else if (menuID == ID_SiteConfiguration)
                    {
                        retval = "商城设置";
                    }
                    return retval;
                }
            }

            public class LeftMenu
            {
                public const string ID_Content = "Content";
                public const string ID_Order = "Order";
                public const string ID_Function = "Function";
                public const string ID_Template = "Template";
                public const string ID_ConfigrationB2C = "ConfigrationB2C";
                public const string ID_ConfigrationSite = "ConfigrationSite";
                public const string ID_Create = "Create";

                public const string ID_Auxiliary = "Auxiliary";
                public const string ID_Site = "Site";
                public const string ID_PaymentShipment = "PaymentShipment";
                public const string ID_User = "User";
                public const string ID_SiteSettings = "SiteSettings";
                public const string ID_Log = "Log";

                public class Content
                {
                    public const string ID_ContentModel = "ContentModel";
                    public const string ID_Category = "Category";
                    public const string ID_SiteAnalysis = "SiteAnalysis";
                }

                public class GovInteract
                {
                    public const string ID_GovInteractConfiguration = "GovInteractConfiguration";     //互动交流设置
                    public const string ID_GovInteractAnalysis = "GovInteractAnalysis";             //数据统计分析
                }

                public class Function
                {
                    public const string ID_Input = "Input";
                    public const string ID_Gather = "Gather";
                    public const string ID_Advertisement = "Advertisement";
                    public const string ID_Resume = "Resume";
                    public const string ID_Mail = "Mail";
                    public const string ID_SEO = "SEO";
                    public const string ID_Tracking = "Tracking";
                    public const string ID_InnerLink = "InnerLink";
                    public const string ID_Restriction = "Restriction";
                    public const string ID_Backup = "Backup";
                    public const string ID_Signin = "Signin";
                    public const string ID_BShare = "BShare";
                }

                public class Template
                {
                    public const string ID_TagStyle = "TagStyle";     //模板标签样式
                }

                public class Configuration
                {
                    public const string ID_ConfigurationCreate = "ConfigurationCreate";         //页面生成设置
                    public const string ID_ConfigurationStorage = "ConfigurationStorage";       //存储空间设置
                    public const string ID_ConfigurationTask = "ConfigurationTask";             //定时任务管理
                    public const string ID_ConfigurationTouGao = "ConfigurationTouGao";   //内容投稿设置
                    public const string ID_ConfigurationComment = "ConfigurationComment";   //内容评论设置
                    public const string ID_ConfigurationMachine = "ConfigurationMachine";   //服务器管理
                }

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Content)
                    {
                        retval = "信息管理";
                    }
                    else if (menuID == ID_Order)
                    {
                        retval = "订单管理";
                    }
                    else if (menuID == ID_Function)
                    {
                        retval = "功能管理";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "显示管理";
                    }
                    else if (menuID == ID_ConfigrationB2C)
                    {
                        retval = "商城设置";
                    }
                    else if (menuID == ID_ConfigrationSite)
                    {
                        retval = "应用设置";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "生成管理";
                    }

                    else if (menuID == ID_Auxiliary)
                    {
                        retval = "辅助表管理";
                    }
                    else if (menuID == ID_Site)
                    {
                        retval = "系统应用管理";
                    }
                    else if (menuID == ID_PaymentShipment)
                    {
                        retval = "支付及配送方式";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "应用用户管理";
                    }
                    else if (menuID == ID_SiteSettings)
                    {
                        retval = "应用通用设置";
                    }
                    else if (menuID == ID_Log)
                    {
                        retval = "后台运行记录";
                    }
                    return retval;
                }

                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    //Content
                    if (menuID == Content.ID_ContentModel)
                    {
                        retval = "内容模型";
                    }
                    else if (menuID == Content.ID_Category)
                    {
                        retval = "归类管理";
                    }
                    else if (menuID == Content.ID_SiteAnalysis)
                    {
                        retval = "应用数据统计";
                    }
                    //GovInteract
                    else if (menuID == GovInteract.ID_GovInteractConfiguration)
                    {
                        retval = "互动交流设置";
                    }
                    else if (menuID == GovInteract.ID_GovInteractAnalysis)
                    {
                        retval = "数据统计分析";
                    }
                    //Function
                    else if (menuID == Function.ID_Input)
                    {
                        retval = "提交表单";
                    }
                    else if (menuID == Function.ID_Gather)
                    {
                        retval = "信息采集管理";
                    }
                    else if (menuID == Function.ID_Advertisement)
                    {
                        retval = "广告管理";
                    }
                    else if (menuID == Function.ID_Resume)
                    {
                        retval = "简历管理";
                    }
                    else if (menuID == Function.ID_Mail)
                    {
                        retval = "邮件发送管理";
                    }
                    else if (menuID == Function.ID_SEO)
                    {
                        retval = "搜索引擎优化";
                    }
                    else if (menuID == Function.ID_Tracking)
                    {
                        retval = "流量统计管理";
                    }
                    else if (menuID == Function.ID_InnerLink)
                    {
                        retval = "站内链接管理";
                    }
                    else if (menuID == Function.ID_Restriction)
                    {
                        retval = "页面访问限制";
                    }
                    else if (menuID == Function.ID_Backup)
                    {
                        retval = "数据备份恢复";
                    }
                    else if (menuID == Function.ID_Signin)
                    {
                        retval = "内容签收管理";
                    }
                    else if (menuID == Function.ID_BShare)
                    {
                        retval = "bShare分享插件";
                    }
                    //Template
                    else if (menuID == Template.ID_TagStyle)
                    {
                        retval = "模板标签样式";
                    }
                    //Configuration
                    else if (menuID == Configuration.ID_ConfigurationCreate)
                    {
                        retval = "页面生成设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationStorage)
                    {
                        retval = "存储空间设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTask)
                    {
                        retval = "定时任务管理";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTouGao)
                    {
                        retval = "内容投稿设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationComment)
                    {
                        retval = "内容评论设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationMachine)
                    {
                        retval = "服务器管理";
                    }
                    return retval;
                }
            }

            public class Permission
            {
                public class General
                {
                    private General() { }

                    public const string Auxiliary = "wcm_auxiliary";                        //辅助表管理
                    public const string Site = "wcm_site";                                  //系统应用管理
                    public const string User = "wcm_user";                                  //应用用户管理
                    public const string SiteSettings = "wcm_siteSettings";                  //应用通用设置
                    public const string Log = "wcm_log";                                    //后台运行记录
                }

                public class WebSite
                {
                    private WebSite() { }

                    public const string ContentModel = "wcm_contentModel";                  //内容模型
                    public const string Category = "wcm_category";                          //归类管理
                    public const string SiteAnalysis = "wcm_siteAnalysis";                  //应用数据统计
                    public const string ContentTrash = "wcm_contentTrash";                  //内容回收站

                    public const string GovPublicContent = "wcm_govPublicContent";          //主动信息公开
                    public const string GovPublicApply = "wcm_govPublicApply";          //依申请公开
                    public const string GovPublicContentConfiguration = "wcm_govPublicContentConfiguration";          //主动信息公开设置
                    public const string GovPublicApplyConfiguration = "wcm_govPublicApplyConfiguration";          //依申请公开设置
                    public const string GovPublicAnalysis = "wcm_govPublicAnalysis";          //信息公开统计

                    public const string GovInteract = "wcm_govInteract";                     //互动交流管理
                    public const string GovInteractConfiguration = "wcm_govInteractConfiguration";                     //互动交流设置
                    public const string GovInteractAnalysis = "wcm_govInteractAnalysis";                     //互动交流统计

                    public const string InputContentView = "wcm_inputContentView";    //提交表单查看
                    public const string InputContentEdit = "wcm_inputContentEdit";    //提交表单维护
                    public const string Input = "wcm_input";                  //提交表单管理
                    public const string Gather = "wcm_gather";                //信息采集管理
                    public const string Advertisement = "wcm_advertisement";  //广告管理
                    public const string Resume = "wcm_resume";                 //在线招聘管理
                    public const string Mail = "wcm_mail";                    //邮件发送管理
                    public const string SEO = "wcm_seo";                      //搜索引擎优化
                    public const string Tracking = "wcm_tracking";            //流量统计管理
                    public const string InnerLink = "wcm_innerLink";          //站内链接管理
                    public const string Restriction = "wcm_restriction";                  //页面访问限制
                    public const string Backup = "wcm_backup";                //数据备份恢复
                    public const string Signin = "wcm_signin";            //内容签收管理
                    public const string ContentArchive = "wcm_contentArchive";            //归档内容管理
                    public const string FileManagement = "wcm_fileManagement";            //应用文件管理
                    public const string AllPublish = "wcm_allPublish";            //多服务器发布
                    public const string BShare = "wcm_bShare";            //bShare分享插件

                    public const string Template = "wcm_template";            //显示管理

                    public const string Configration = "wcm_configration";    //设置管理

                    public const string Create = "wcm_create";                //生成管理
                }

                public class Channel
                {
                    private Channel() { }
                    public const string ContentView = "wcm_contentView";
                    public const string ContentAdd = "wcm_contentAdd";
                    public const string ContentEdit = "wcm_contentEdit";
                    public const string ContentDelete = "wcm_contentDelete";
                    public const string ContentTranslate = "wcm_contentTranslate";
                    public const string ContentArchive = "wcm_contentArchive";
                    public const string ContentCheck = "wcm_contentCheck";
                    public const string ChannelAdd = "wcm_channelAdd";
                    public const string ChannelEdit = "wcm_channelEdit";
                    public const string ChannelDelete = "wcm_channelDelete";
                    public const string ChannelTranslate = "wcm_channelTranslate";
                    public const string CommentCheck = "wcm_commentCheck";
                    public const string CommentDelete = "wcm_commentDelete";
                    public const string ContentCheckLevel1 = "wcm_contentCheckLevel1";
                    public const string ContentCheckLevel2 = "wcm_contentCheckLevel2";
                    public const string ContentCheckLevel3 = "wcm_contentCheckLevel3";
                    public const string ContentCheckLevel4 = "wcm_contentCheckLevel4";
                    public const string ContentCheckLevel5 = "wcm_contentCheckLevel5";
                    public const string CreatePage = "wcm_CreatePage";
                    public const string PublishPage = "wcm_PublishPage";
                    public const string ContentOrder = "wcm_ContentOrder";
                }

                public class GovInteract
                {
                    private GovInteract() { }
                    public const string GovInteractView = "wcm_govInteractView";
                    public const string GovInteractAdd = "wcm_govInteractAdd";
                    public const string GovInteractEdit = "wcm_govInteractEdit";
                    public const string GovInteractDelete = "wcm_govInteractDelete";
                    public const string GovInteractSwitchToTranslate = "wcm_govInteractSwitchToTranslate";
                    public const string GovInteractComment = "wcm_govInteractComment";
                    public const string GovInteractAccept = "wcm_govInteractAccept";
                    public const string GovInteractReply = "wcm_govInteractReply";
                    public const string GovInteractCheck = "wcm_govInteractCheck";

                    public static string GetPermissionName(string permission)
                    {
                        string retval = string.Empty;
                        if (permission == GovInteractView)
                        {
                            retval = "浏览办件";
                        }
                        else if (permission == GovInteractAdd)
                        {
                            retval = "新增办件";
                        }
                        else if (permission == GovInteractEdit)
                        {
                            retval = "编辑办件";
                        }
                        else if (permission == GovInteractDelete)
                        {
                            retval = "删除办件";
                        }
                        else if (permission == GovInteractSwitchToTranslate)
                        {
                            retval = "转办转移";
                        }
                        else if (permission == GovInteractComment)
                        {
                            retval = "批示办件";
                        }
                        else if (permission == GovInteractAccept)
                        {
                            retval = "受理办件";
                        }
                        else if (permission == GovInteractReply)
                        {
                            retval = "办理办件";
                        }
                        else if (permission == GovInteractCheck)
                        {
                            retval = "审核办件";
                        }
                        return retval;
                    }
                }
            }

            public class AuxiliaryTableName
            {
                public const string GoodsContent = AppID + "_GoodsContent";
                public const string BrandContent = AppID + "_BrandContent";
                public const string BackgroundContent = AppID + "_Content";
                public const string UserDefined = AppID + "_ContentCustom";
            }
        }

        public class CRM
        {
            public const string AppID = "crm";

            public class TopMenu
            {
                public const string ID_Forums = "Forums";

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Forums)
                    {
                        retval = "论坛管理";
                    }
                    return retval;
                }
            }

            public class LeftMenu
            {
                public const string ID_Forum = "Forum";             //版块管理
                public const string ID_Content = "Content";         //内容管理
                public const string ID_User = "User";               //用户管理
                public const string ID_Settings = "Settings";       //系统设置
                public const string ID_Template = "Template";       //界面管理
                public const string ID_Create = "Create";           //生成管理

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Forum)
                    {
                        retval = "版块管理";
                    }
                    else if (menuID == ID_Content)
                    {
                        retval = "内容管理";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "用户管理";
                    }
                    else if (menuID == ID_Settings)
                    {
                        retval = "系统设置";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "界面管理";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "生成管理";
                    }
                    return retval;
                }
            }

            public class Permission
            {
                public const string CRM_Forum = "crm_forum";
                public const string CRM_Content = "crm_content";
                public const string CRM_User = "crm_user";
                public const string CRM_Settings = "crm_settings";
                public const string CRM_Template = "crm_template";
                public const string CRM_Create = "crm_create";
            }
        }

        public class WeiXin
        {
            public const string AppID = "weixin";

            public class TopMenu
            {
                public const string ID_SiteManagement = "SiteManagement";
                public const string ID_SiteConfiguration = "SiteConfiguration";

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_SiteManagement)
                    {
                        retval = "微信管理";
                    }
                    else if (menuID == ID_SiteConfiguration)
                    {
                        retval = "微信设置";
                    }
                    return retval;
                }
            }

            public class LeftMenu
            {
                public const string ID_Accounts = "Accounts";

                public const string ID_Function = "Function";

                public class Function
                {
                    //Accounts
                    public const string ID_Info = "Info";
                    public const string ID_Chart = "Chart";
                    public const string ID_Menu = "Menu";
                    public const string ID_TextReply = "TextReply";
                    public const string ID_ImageReply = "ImageReply";
                    public const string ID_SetReply = "SetReply";

                    //Function
                    public const string ID_Coupon = "Coupon";
                    public const string ID_Scratch = "Scratch";
                    public const string ID_BigWheel = "BigWheel";
                    public const string ID_GoldEgg = "GoldEgg";
                    public const string ID_Flap = "Flap";
                    public const string ID_YaoYao = "YaoYao";
                    public const string ID_Vote = "Vote";
                    public const string ID_Message = "Message";
                    public const string ID_Appointment = "Appointment";
                    public const string ID_Conference = "Conference";
                    public const string ID_Map = "Map";
                    public const string ID_View360 = "View360";
                    public const string ID_Album = "Album";
                    public const string ID_Search = "Search";
                    public const string ID_Store = "Store";
                    public const string ID_Wifi = "Wifi";
                    public const string ID_Card = "Card";
                    public const string ID_Collect = "Collect";
                }

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Accounts)
                    {
                        retval = "公共账号";
                    }
                    else if (menuID == ID_Function)
                    {
                        retval = "微功能";
                    }
                    return retval;
                }

                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    //Accounts
                    if (menuID == Function.ID_Info)
                    {
                        retval = "账户信息";
                    }
                    else if (menuID == Function.ID_Chart)
                    {
                        retval = "运营图表";
                    }
                    else if (menuID == Function.ID_Menu)
                    {
                        retval = "自定义菜单";
                    }
                    else if (menuID == Function.ID_TextReply)
                    {
                        retval = "关键词文本回复";
                    }
                    else if (menuID == Function.ID_ImageReply)
                    {
                        retval = "关键词图文回复";
                    }
                    else if (menuID == Function.ID_SetReply)
                    {
                        retval = "关键词回复设置";
                    }
                    //Function
                    else if (menuID == Function.ID_Coupon)
                    {
                        retval = "优惠券";
                    }
                    else if (menuID == Function.ID_Scratch)
                    {
                        retval = "刮刮卡";
                    }
                    else if (menuID == Function.ID_BigWheel)
                    {
                        retval = "大转盘";
                    }
                    else if (menuID == Function.ID_GoldEgg)
                    {
                        retval = "砸金蛋";
                    }
                    else if (menuID == Function.ID_Flap)
                    {
                        retval = "大翻牌";
                    }
                    else if (menuID == Function.ID_YaoYao)
                    {
                        retval = "摇摇乐";
                    }
                    else if (menuID == Function.ID_Vote)
                    {
                        retval = "微投票";
                    }
                    else if (menuID == Function.ID_Message)
                    {
                        retval = "微留言";
                    }
                    else if (menuID == Function.ID_Appointment)
                    {
                        retval = "微预约";
                    }
                    else if (menuID == Function.ID_Conference)
                    {
                        retval = "微会议";
                    }
                    else if (menuID == Function.ID_Map)
                    {
                        retval = "微导航";
                    }
                    else if (menuID == Function.ID_View360)
                    {
                        retval = "360全景";
                    }
                    else if (menuID == Function.ID_Album)
                    {
                        retval = "微相册";
                    }
                    else if (menuID == Function.ID_Search)
                    {
                        retval = "微搜索";
                    }
                    else if (menuID == Function.ID_Store)
                    {
                        retval = "微门店";
                    }
                    else if (menuID == Function.ID_Wifi)
                    {
                        retval = "微Wifi";
                    }
                    else if (menuID == Function.ID_Card)
                    {
                        retval = "会员卡";
                    }
                    else if (menuID == Function.ID_Collect)
                    {
                        retval = "微收集";
                    }
                    return retval;
                }
            }

            public class Permission
            {
                public class WebSite
                {
                    private WebSite() { }


                    public const string Info = "weixin_info";                       //账户信息
                    public const string Chart = "weixin_chart";                     //运营图表
                    public const string Menu = "weixin_menu";                       //菜单
                    public const string TextReply = "weixin_textReply";             //文本回复
                    public const string ImageReply = "weixin_imageReply";           //图文回复
                    public const string SetReply = "weixin_setReply";               //回复设置


                    public const string Coupon = "weixin_coupon";               //优惠券管理
                    public const string Scratch = "weixin_scratch";             //刮刮卡管理
                    public const string BigWheel = "weixin_bigWheel";           //大转盘管理
                    public const string GoldEgg = "weixin_goldEgg";             //砸金蛋管理
                    public const string Flap = "weixin_flap";                   //大翻牌管理
                    public const string YaoYao = "weixin_yaoYao";               //摇摇乐管理
                    public const string Vote = "weixin_vote";                   //微投票管理
                    public const string Message = "weixin_message";             //微留言管理
                    public const string Appointment = "weixin_appointment";            //微预约管理

                    public const string Conference = "weixin_conference";       //微会议管理
                    public const string Map = "weixin_map";                     //微导航管理
                    public const string View360 = "weixin_view360";             //全景管理
                    public const string Album = "weixin_album";                 //微相册管理
                    public const string Search = "weixin_search";               //微搜索管理
                    public const string Store = "weixin_store";                 //微门店管理
                    public const string Wifi = "weixin_wifi";                   //微wifi管理
                    public const string Card = "weixin_card";                   //微会员管理
                    public const string Collect = "weixin_collect";             //微征集管理
                }
            }
        }

        public class WeiXinB2C
        {
            public const string AppID = "weixinb2c";

            public class TopMenu
            {
                public const string ID_SiteManagement = "SiteManagement";
                public const string ID_SiteConfiguration = "SiteConfiguration";

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_SiteManagement)
                    {
                        retval = "微信管理";
                    }
                    else if (menuID == ID_SiteConfiguration)
                    {
                        retval = "微信设置";
                    }
                    return retval;
                }
            }

            public class LeftMenu
            {
                public const string ID_Content = "Content";
                public const string ID_Order = "Order";
                public const string ID_Function = "Function";
                public const string ID_Template = "Template";
                public const string ID_ConfigrationB2C = "ConfigrationB2C";
                public const string ID_ConfigrationSite = "ConfigrationSite";
                public const string ID_Create = "Create";

                public const string ID_Auxiliary = "Auxiliary";
                public const string ID_Site = "Site";
                public const string ID_PaymentShipment = "PaymentShipment";
                public const string ID_User = "User";
                public const string ID_SiteSettings = "SiteSettings";
                public const string ID_Log = "Log";

                public class Content
                {
                    public const string ID_ContentModel = "ContentModel";
                    public const string ID_Category = "Category";
                    public const string ID_SiteAnalysis = "SiteAnalysis";
                }

                public class GovInteract
                {
                    public const string ID_GovInteractConfiguration = "GovInteractConfiguration";     //互动交流设置
                    public const string ID_GovInteractAnalysis = "GovInteractAnalysis";             //数据统计分析
                }

                public class Function
                {
                    public const string ID_Input = "Input";
                    public const string ID_Gather = "Gather";
                    public const string ID_Advertisement = "Advertisement";
                    public const string ID_Resume = "Resume";
                    public const string ID_Mail = "Mail";
                    public const string ID_SEO = "SEO";
                    public const string ID_Tracking = "Tracking";
                    public const string ID_InnerLink = "InnerLink";
                    public const string ID_Restriction = "Restriction";
                    public const string ID_Backup = "Backup";
                    public const string ID_Signin = "Signin";
                    public const string ID_BShare = "BShare";
                }

                public class Template
                {
                    public const string ID_TagStyle = "TagStyle";     //模板标签样式
                }

                public class Configuration
                {
                    public const string ID_ConfigurationCreate = "ConfigurationCreate";         //页面生成设置
                    public const string ID_ConfigurationStorage = "ConfigurationStorage";       //存储空间设置
                    public const string ID_ConfigurationTask = "ConfigurationTask";             //定时任务管理
                    public const string ID_ConfigurationTouGao = "ConfigurationTouGao";   //内容投稿设置
                    public const string ID_ConfigurationComment = "ConfigurationComment";   //内容评论设置
                    public const string ID_ConfigurationMachine = "ConfigurationMachine";   //服务器管理
                }

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Content)
                    {
                        retval = "信息管理";
                    }
                    else if (menuID == ID_Order)
                    {
                        retval = "订单管理";
                    }
                    else if (menuID == ID_Function)
                    {
                        retval = "功能管理";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "显示管理";
                    }
                    else if (menuID == ID_ConfigrationB2C)
                    {
                        retval = "商城设置";
                    }
                    else if (menuID == ID_ConfigrationSite)
                    {
                        retval = "应用设置";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "生成管理";
                    }

                    else if (menuID == ID_Auxiliary)
                    {
                        retval = "辅助表管理";
                    }
                    else if (menuID == ID_Site)
                    {
                        retval = "系统应用管理";
                    }
                    else if (menuID == ID_PaymentShipment)
                    {
                        retval = "支付及配送方式";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "应用用户管理";
                    }
                    else if (menuID == ID_SiteSettings)
                    {
                        retval = "应用通用设置";
                    }
                    else if (menuID == ID_Log)
                    {
                        retval = "后台运行记录";
                    }
                    return retval;
                }

                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    //Content
                    if (menuID == Content.ID_ContentModel)
                    {
                        retval = "内容模型";
                    }
                    else if (menuID == Content.ID_Category)
                    {
                        retval = "归类管理";
                    }
                    else if (menuID == Content.ID_SiteAnalysis)
                    {
                        retval = "应用数据统计";
                    }
                    //GovInteract
                    else if (menuID == GovInteract.ID_GovInteractConfiguration)
                    {
                        retval = "互动交流设置";
                    }
                    else if (menuID == GovInteract.ID_GovInteractAnalysis)
                    {
                        retval = "数据统计分析";
                    }
                    //Function
                    else if (menuID == Function.ID_Input)
                    {
                        retval = "提交表单";
                    }
                    else if (menuID == Function.ID_Gather)
                    {
                        retval = "信息采集管理";
                    }
                    else if (menuID == Function.ID_Advertisement)
                    {
                        retval = "广告管理";
                    }
                    else if (menuID == Function.ID_Resume)
                    {
                        retval = "简历管理";
                    }
                    else if (menuID == Function.ID_Mail)
                    {
                        retval = "邮件发送管理";
                    }
                    else if (menuID == Function.ID_SEO)
                    {
                        retval = "搜索引擎优化";
                    }
                    else if (menuID == Function.ID_Tracking)
                    {
                        retval = "流量统计管理";
                    }
                    else if (menuID == Function.ID_InnerLink)
                    {
                        retval = "站内链接管理";
                    }
                    else if (menuID == Function.ID_Restriction)
                    {
                        retval = "页面访问限制";
                    }
                    else if (menuID == Function.ID_Backup)
                    {
                        retval = "数据备份恢复";
                    }
                    else if (menuID == Function.ID_Signin)
                    {
                        retval = "内容签收管理";
                    }
                    else if (menuID == Function.ID_BShare)
                    {
                        retval = "bShare分享插件";
                    }
                    //Template
                    else if (menuID == Template.ID_TagStyle)
                    {
                        retval = "模板标签样式";
                    }
                    //Configuration
                    else if (menuID == Configuration.ID_ConfigurationCreate)
                    {
                        retval = "页面生成设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationStorage)
                    {
                        retval = "存储空间设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTask)
                    {
                        retval = "定时任务管理";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTouGao)
                    {
                        retval = "内容投稿设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationComment)
                    {
                        retval = "内容评论设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationMachine)
                    {
                        retval = "服务器管理";
                    }
                    return retval;
                }
            }

            public class Permission
            {
                public class General
                {
                    private General() { }

                    public const string Auxiliary = "wcm_auxiliary";                        //辅助表管理
                    public const string Site = "wcm_site";                                  //系统应用管理
                    public const string User = "wcm_user";                                  //应用用户管理
                    public const string SiteSettings = "wcm_siteSettings";                  //应用通用设置
                    public const string Log = "wcm_log";                                    //后台运行记录
                }

                public class WebSite
                {
                    private WebSite() { }

                    public const string Info = "weixin_info";                       //账户信息
                    public const string Chart = "weixin_chart";                     //运营图表
                    public const string Menu = "weixin_menu";                       //菜单
                    public const string TextReply = "weixin_textReply";             //文本回复
                    public const string ImageReply = "weixin_imageReply";           //图文回复
                    public const string SetReply = "weixin_setReply";               //回复设置


                    public const string Coupon = "weixin_coupon";               //优惠券管理
                    public const string Scratch = "weixin_scratch";             //刮刮卡管理
                    public const string BigWheel = "weixin_bigWheel";           //大转盘管理
                    public const string GoldEgg = "weixin_goldEgg";             //砸金蛋管理
                    public const string Flap = "weixin_flap";                   //大翻牌管理
                    public const string YaoYao = "weixin_yaoYao";               //摇摇乐管理
                    public const string Vote = "weixin_vote";                   //微投票管理
                    public const string Message = "weixin_message";             //微留言管理
                    public const string Appointment = "weixin_appointment";            //微预约管理

                    public const string Conference = "weixin_conference";       //微会议管理
                    public const string Map = "weixin_map";                     //微导航管理
                    public const string View360 = "weixin_view360";             //全景管理
                    public const string Album = "weixin_album";                 //微相册管理
                    public const string Search = "weixin_search";               //微搜索管理
                    public const string Store = "weixin_store";                 //微门店管理
                    public const string Wifi = "weixin_wifi";                   //微wifi管理
                    public const string Card = "weixin_card";                   //微会员管理
                    public const string Collect = "weixin_collect";             //微征集管理

                    public const string ContentModel = "wcm_contentModel";                  //内容模型
                    public const string Category = "wcm_category";                          //归类管理
                    public const string SiteAnalysis = "wcm_siteAnalysis";                  //应用数据统计
                    public const string ContentTrash = "wcm_contentTrash";                  //内容回收站

                    public const string GovPublicContent = "wcm_govPublicContent";          //主动信息公开
                    public const string GovPublicApply = "wcm_govPublicApply";          //依申请公开
                    public const string GovPublicContentConfiguration = "wcm_govPublicContentConfiguration";          //主动信息公开设置
                    public const string GovPublicApplyConfiguration = "wcm_govPublicApplyConfiguration";          //依申请公开设置
                    public const string GovPublicAnalysis = "wcm_govPublicAnalysis";          //信息公开统计

                    public const string GovInteract = "wcm_govInteract";                     //互动交流管理
                    public const string GovInteractConfiguration = "wcm_govInteractConfiguration";                     //互动交流设置
                    public const string GovInteractAnalysis = "wcm_govInteractAnalysis";                     //互动交流统计

                    public const string InputContentView = "wcm_inputContentView";    //提交表单查看
                    public const string InputContentEdit = "wcm_inputContentEdit";    //提交表单维护
                    public const string Input = "wcm_input";                  //提交表单管理
                    public const string Gather = "wcm_gather";                //信息采集管理
                    public const string Advertisement = "wcm_advertisement";  //广告管理
                    public const string Resume = "wcm_resume";                 //在线招聘管理
                    public const string Mail = "wcm_mail";                    //邮件发送管理
                    public const string SEO = "wcm_seo";                      //搜索引擎优化
                    public const string Tracking = "wcm_tracking";            //流量统计管理
                    public const string InnerLink = "wcm_innerLink";          //站内链接管理
                    public const string Restriction = "wcm_restriction";                  //页面访问限制
                    public const string Backup = "wcm_backup";                //数据备份恢复
                    public const string Signin = "wcm_signin";            //内容签收管理
                    public const string ContentArchive = "wcm_contentArchive";            //归档内容管理
                    public const string FileManagement = "wcm_fileManagement";            //应用文件管理
                    public const string AllPublish = "wcm_allPublish";            //多服务器发布
                    public const string BShare = "wcm_bShare";            //bShare分享插件

                    public const string Template = "wcm_template";            //显示管理

                    public const string Configration = "wcm_configration";    //设置管理

                    public const string Create = "wcm_create";                //生成管理
                }

                public class Channel
                {
                    private Channel() { }
                    public const string ContentView = "wcm_contentView";
                    public const string ContentAdd = "wcm_contentAdd";
                    public const string ContentEdit = "wcm_contentEdit";
                    public const string ContentDelete = "wcm_contentDelete";
                    public const string ContentTranslate = "wcm_contentTranslate";
                    public const string ContentArchive = "wcm_contentArchive";
                    public const string ContentCheck = "wcm_contentCheck";
                    public const string ChannelAdd = "wcm_channelAdd";
                    public const string ChannelEdit = "wcm_channelEdit";
                    public const string ChannelDelete = "wcm_channelDelete";
                    public const string ChannelTranslate = "wcm_channelTranslate";
                    public const string CommentCheck = "wcm_commentCheck";
                    public const string CommentDelete = "wcm_commentDelete";
                    public const string ContentCheckLevel1 = "wcm_contentCheckLevel1";
                    public const string ContentCheckLevel2 = "wcm_contentCheckLevel2";
                    public const string ContentCheckLevel3 = "wcm_contentCheckLevel3";
                    public const string ContentCheckLevel4 = "wcm_contentCheckLevel4";
                    public const string ContentCheckLevel5 = "wcm_contentCheckLevel5";
                    public const string CreatePage = "wcm_CreatePage";
                    public const string PublishPage = "wcm_PublishPage";
                    public const string ContentOrder = "wcm_ContentOrder";
                }

                public class GovInteract
                {
                    private GovInteract() { }
                    public const string GovInteractView = "wcm_govInteractView";
                    public const string GovInteractAdd = "wcm_govInteractAdd";
                    public const string GovInteractEdit = "wcm_govInteractEdit";
                    public const string GovInteractDelete = "wcm_govInteractDelete";
                    public const string GovInteractSwitchToTranslate = "wcm_govInteractSwitchToTranslate";
                    public const string GovInteractComment = "wcm_govInteractComment";
                    public const string GovInteractAccept = "wcm_govInteractAccept";
                    public const string GovInteractReply = "wcm_govInteractReply";
                    public const string GovInteractCheck = "wcm_govInteractCheck";

                    public static string GetPermissionName(string permission)
                    {
                        string retval = string.Empty;
                        if (permission == GovInteractView)
                        {
                            retval = "浏览办件";
                        }
                        else if (permission == GovInteractAdd)
                        {
                            retval = "新增办件";
                        }
                        else if (permission == GovInteractEdit)
                        {
                            retval = "编辑办件";
                        }
                        else if (permission == GovInteractDelete)
                        {
                            retval = "删除办件";
                        }
                        else if (permission == GovInteractSwitchToTranslate)
                        {
                            retval = "转办转移";
                        }
                        else if (permission == GovInteractComment)
                        {
                            retval = "批示办件";
                        }
                        else if (permission == GovInteractAccept)
                        {
                            retval = "受理办件";
                        }
                        else if (permission == GovInteractReply)
                        {
                            retval = "办理办件";
                        }
                        else if (permission == GovInteractCheck)
                        {
                            retval = "审核办件";
                        }
                        return retval;
                    }
                }
            }

            public class AuxiliaryTableName
            {
                public const string GoodsContent = AppID + "_GoodsContent";
                public const string BrandContent = AppID + "_BrandContent";
                public const string BackgroundContent = AppID + "_Content";
                public const string UserDefined = AppID + "_ContentCustom";
            }
        }

        public class UserCenter
        {
            public const string AppID = "usercenter";

            public class TopMenu
            {
                //public const string ID_SiteManagement = "SiteManagement";
                public const string ID_SiteConfiguration = "SiteConfiguration";
                public const string ID_UserManagement = "UserManagement";

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_UserManagement)
                    {
                        retval = "用户管理";
                    }
                    else if (menuID == ID_SiteConfiguration)
                    {
                        retval = "系统管理";
                    }
                    return retval;
                }
            }

            public class LeftMenu
            {
                public const string ID_Content = "Content";
                public const string ID_GovPublic = "GovPublic";
                public const string ID_GovInteract = "GovInteract";
                public const string ID_Function = "Function";
                public const string ID_Template = "Template";
                public const string ID_User = "User";
                public const string ID_Configration = "Configration";
                public const string ID_Create = "Create";

                public const string ID_Auxiliary = "Auxiliary";
                public const string ID_Site = "Site";
                public const string ID_SiteSettings = "SiteSettings";
                public const string ID_Log = "Log";
                public const string ID_ThirdLog = "ThirdLog";

                public const string ID_UserLevel = "UserLevel";

                public class Content
                {
                    public const string ID_ContentModel = "ContentModel";
                    public const string ID_Category = "Category";
                    public const string ID_SiteAnalysis = "SiteAnalysis";
                }

                public class GovPublic
                {
                    public const string ID_GovPublicContent = "GovPublicContent";     //主动信息公开
                    public const string ID_GovPublicApply = "GovPublicApply";         //依申请公开
                    public const string ID_GovPublicContentConfiguration = "GovPublicContentConfiguration";         //主动信息公开设置
                    public const string ID_GovPublicApplyConfiguration = "GovPublicApplyConfiguration";         //依申请公开设置
                    public const string ID_GovPublicAnalysis = "GovPublicAnalysis";         //数据统计分析
                }

                public class GovInteract
                {
                    public const string ID_GovInteractConfiguration = "GovInteractConfiguration";     //互动交流设置
                    public const string ID_GovInteractAnalysis = "GovInteractAnalysis";             //数据统计分析
                }

                public class Function
                {
                    public const string ID_Input = "Input";
                    public const string ID_Gather = "Gather";
                    public const string ID_Advertisement = "Advertisement";
                    public const string ID_Resume = "Resume";
                    public const string ID_Mail = "Mail";
                    public const string ID_SEO = "SEO";
                    public const string ID_Tracking = "Tracking";
                    public const string ID_InnerLink = "InnerLink";
                    public const string ID_Restriction = "Restriction";
                    public const string ID_Backup = "Backup";
                    public const string ID_BShare = "BShare";
                }

                public class Template
                {
                    public const string ID_TagStyle = "TagStyle";     //模板标签样式
                }

                public class Configuration
                {
                    public const string ID_ConfigurationCreate = "ConfigurationCreate";         //页面生成设置
                    public const string ID_ConfigurationStorage = "ConfigurationStorage";       //存储空间设置
                    public const string ID_ConfigurationTask = "ConfigurationTask";             //定时任务管理
                    public const string ID_ConfigurationTouGao = "ConfigurationTouGao";         //内容投稿设置
                    public const string ID_ConfigurationComment = "ConfigurationComment";       //内容评论设置
                    public const string ID_ConfigurationMachine = "ConfigurationMachine";       //服务器管理
                }

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Content)
                    {
                        retval = "信息管理";
                    }
                    else if (menuID == ID_GovPublic)
                    {
                        retval = "信息公开";
                    }
                    else if (menuID == ID_GovInteract)
                    {
                        retval = "互动交流";
                    }
                    else if (menuID == ID_Function)
                    {
                        retval = "功能管理";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "显示管理";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "用户管理";
                    }
                    else if (menuID == ID_Configration)
                    {
                        retval = "设置管理";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "生成管理";
                    }

                    else if (menuID == ID_Auxiliary)
                    {
                        retval = "辅助表管理";
                    }
                    else if (menuID == ID_Site)
                    {
                        retval = "系统应用管理";
                    }
                    else if (menuID == ID_SiteSettings)
                    {
                        retval = "应用通用设置";
                    }
                    else if (menuID == ID_Log)
                    {
                        retval = "后台运行记录";
                    }
                    else if (menuID == ID_UserLevel)
                    {
                        retval = "用户等级积分";
                    }
                    return retval;
                }

                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    //Content
                    if (menuID == Content.ID_ContentModel)
                    {
                        retval = "内容模型";
                    }
                    else if (menuID == Content.ID_Category)
                    {
                        retval = "归类管理";
                    }
                    else if (menuID == Content.ID_SiteAnalysis)
                    {
                        retval = "应用数据统计";
                    }
                    //GovPublic
                    else if (menuID == GovPublic.ID_GovPublicContent)
                    {
                        retval = "主动信息公开";
                    }
                    else if (menuID == GovPublic.ID_GovPublicApply)
                    {
                        retval = "依申请公开";
                    }
                    else if (menuID == GovPublic.ID_GovPublicContentConfiguration)
                    {
                        retval = "主动信息公开设置";
                    }
                    else if (menuID == GovPublic.ID_GovPublicApplyConfiguration)
                    {
                        retval = "依申请公开设置";
                    }
                    else if (menuID == GovPublic.ID_GovPublicAnalysis)
                    {
                        retval = "数据统计分析";
                    }
                    //GovInteract
                    else if (menuID == GovInteract.ID_GovInteractConfiguration)
                    {
                        retval = "互动交流设置";
                    }
                    else if (menuID == GovInteract.ID_GovInteractAnalysis)
                    {
                        retval = "数据统计分析";
                    }
                    //Function
                    else if (menuID == Function.ID_Input)
                    {
                        retval = "提交表单";
                    }
                    else if (menuID == Function.ID_Gather)
                    {
                        retval = "信息采集管理";
                    }
                    else if (menuID == Function.ID_Advertisement)
                    {
                        retval = "广告管理";
                    }
                    else if (menuID == Function.ID_Resume)
                    {
                        retval = "简历管理";
                    }
                    else if (menuID == Function.ID_Mail)
                    {
                        retval = "邮件发送管理";
                    }
                    else if (menuID == Function.ID_SEO)
                    {
                        retval = "搜索引擎优化";
                    }
                    else if (menuID == Function.ID_Tracking)
                    {
                        retval = "流量统计管理";
                    }
                    else if (menuID == Function.ID_InnerLink)
                    {
                        retval = "站内链接管理";
                    }
                    else if (menuID == Function.ID_Restriction)
                    {
                        retval = "页面访问限制";
                    }
                    else if (menuID == Function.ID_Backup)
                    {
                        retval = "数据备份恢复";
                    }
                    else if (menuID == Function.ID_BShare)
                    {
                        retval = "bShare分享插件";
                    }
                    //Template
                    else if (menuID == Template.ID_TagStyle)
                    {
                        retval = "模板标签样式";
                    }
                    //Configuration
                    else if (menuID == Configuration.ID_ConfigurationCreate)
                    {
                        retval = "页面生成设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationStorage)
                    {
                        retval = "存储空间设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTask)
                    {
                        retval = "定时任务管理";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTouGao)
                    {
                        retval = "内容投稿设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationComment)
                    {
                        retval = "内容评论设置";
                    }
                    else if (menuID == Configuration.ID_ConfigurationMachine)
                    {
                        retval = "服务器管理";
                    }
                    return retval;
                }
            }

            public class Permission
            {
                //public class General
                //{
                //    private General() { }

                //    public const string Auxiliary = "cms_auxiliary";                        //辅助表管理
                //    public const string Site = "cms_site";                                  //系统应用管理
                //    public const string User = "cms_user";                                  //应用用户管理
                //    public const string SiteSettings = "cms_siteSettings";                  //应用通用设置
                //    public const string Log = "cms_log";                                    //后台运行记录
                //}

                public class WebSite
                {
                    private WebSite() { }

                    public const string ContentModel = "cms_contentModel";                  //内容模型
                    public const string Category = "cms_category";                          //归类管理
                    public const string SiteAnalysis = "cms_siteAnalysis";                  //应用数据统计
                    public const string ContentTrash = "cms_contentTrash";                  //内容回收站

                    public const string GovPublicContent = "cms_govPublicContent";          //主动信息公开
                    public const string GovPublicApply = "cms_govPublicApply";          //依申请公开
                    public const string GovPublicContentConfiguration = "cms_govPublicContentConfiguration";          //主动信息公开设置
                    public const string GovPublicApplyConfiguration = "cms_govPublicApplyConfiguration";          //依申请公开设置
                    public const string GovPublicAnalysis = "cms_govPublicAnalysis";          //信息公开统计

                    public const string GovInteract = "cms_govInteract";                     //互动交流管理
                    public const string GovInteractConfiguration = "cms_govInteractConfiguration";                     //互动交流设置
                    public const string GovInteractAnalysis = "cms_govInteractAnalysis";                     //互动交流统计

                    public const string InputContentView = "cms_inputContentView";    //提交表单查看
                    public const string InputContentEdit = "cms_inputContentEdit";    //提交表单维护
                    public const string Input = "cms_input";                  //提交表单管理
                    public const string Gather = "cms_gather";                //信息采集管理
                    public const string Advertisement = "cms_advertisement";  //广告管理
                    public const string Resume = "cms_resume";                 //在线招聘管理
                    public const string Mail = "cms_mail";                    //邮件发送管理
                    public const string SEO = "cms_seo";                      //搜索引擎优化
                    public const string Tracking = "cms_tracking";            //流量统计管理
                    public const string InnerLink = "cms_innerLink";          //站内链接管理
                    public const string Restriction = "cms_restriction";                  //页面访问限制
                    public const string Backup = "cms_backup";                //数据备份恢复
                    public const string Archive = "cms_archive";            //归档内容管理
                    public const string FileManagement = "cms_fileManagement";            //应用文件管理
                    public const string AllPublish = "cms_allPublish";            //多服务器发布
                    public const string BShare = "cms_bShare";            //bShare分享插件

                    public const string Template = "cms_template";            //显示管理

                    public const string User = "cms_user";            //用户管理

                    public const string Configration = "cms_configration";    //设置管理

                    public const string Create = "cms_create";                //生成管理
                }

                public class Channel
                {
                    private Channel() { }
                    public const string ContentView = "cms_contentView";
                    public const string ContentAdd = "cms_contentAdd";
                    public const string ContentEdit = "cms_contentEdit";
                    public const string ContentDelete = "cms_contentDelete";
                    public const string ContentTranslate = "cms_contentTranslate";
                    public const string ContentArchive = "cms_contentArchive";
                    public const string ContentOrder = "cms_contentOrder";
                    public const string ChannelAdd = "cms_channelAdd";
                    public const string ChannelEdit = "cms_channelEdit";
                    public const string ChannelDelete = "cms_channelDelete";
                    public const string ChannelTranslate = "cms_channelTranslate";
                    public const string CommentCheck = "cms_commentCheck";
                    public const string CommentDelete = "cms_commentDelete";
                    public const string CreatePage = "cms_createPage";
                    public const string PublishPage = "cms_publishPage";
                    public const string ContentCheck = "cms_contentCheck";
                    public const string ContentCheckLevel1 = "cms_contentCheckLevel1";
                    public const string ContentCheckLevel2 = "cms_contentCheckLevel2";
                    public const string ContentCheckLevel3 = "cms_contentCheckLevel3";
                    public const string ContentCheckLevel4 = "cms_contentCheckLevel4";
                    public const string ContentCheckLevel5 = "cms_contentCheckLevel5";
                }

                public class GovInteract
                {
                    private GovInteract() { }
                    public const string GovInteractView = "cms_govInteractView";
                    public const string GovInteractAdd = "cms_govInteractAdd";
                    public const string GovInteractEdit = "cms_govInteractEdit";
                    public const string GovInteractDelete = "cms_govInteractDelete";
                    public const string GovInteractSwitchToTranslate = "cms_govInteractSwitchToTranslate";
                    public const string GovInteractComment = "cms_govInteractComment";
                    public const string GovInteractAccept = "cms_govInteractAccept";
                    public const string GovInteractReply = "cms_govInteractReply";
                    public const string GovInteractCheck = "cms_govInteractCheck";

                    public static string GetPermissionName(string permission)
                    {
                        string retval = string.Empty;
                        if (permission == GovInteractView)
                        {
                            retval = "浏览办件";
                        }
                        else if (permission == GovInteractAdd)
                        {
                            retval = "新增办件";
                        }
                        else if (permission == GovInteractEdit)
                        {
                            retval = "编辑办件";
                        }
                        else if (permission == GovInteractDelete)
                        {
                            retval = "删除办件";
                        }
                        else if (permission == GovInteractSwitchToTranslate)
                        {
                            retval = "转办转移";
                        }
                        else if (permission == GovInteractComment)
                        {
                            retval = "批示办件";
                        }
                        else if (permission == GovInteractAccept)
                        {
                            retval = "受理办件";
                        }
                        else if (permission == GovInteractReply)
                        {
                            retval = "办理办件";
                        }
                        else if (permission == GovInteractCheck)
                        {
                            retval = "审核办件";
                        }
                        return retval;
                    }
                }
            }
        }


        public class MLib
        {
            public const string AppID = "mlib";

            public class LeftMenu
            {
                public const string ID_Content = "Content";
                public const string ID_GovPublic = "GovPublic";
                public const string ID_GovInteract = "GovInteract";
                public const string ID_Function = "Function";
                public const string ID_Template = "Template";
                public const string ID_User = "User";
                public const string ID_Configration = "Configration";
                public const string ID_Create = "Create";

                public const string ID_Auxiliary = "Auxiliary";
                public const string ID_Site = "Site";
                public const string ID_SiteSettings = "SiteSettings";
                public const string ID_Log = "Log";
                public const string ID_ThirdLog = "ThirdLog";

                public const string ID_UserLevel = "UserLevel";

                public class Content
                {
                    public const string ID_ContentModel = "ContentModel";
                    public const string ID_Category = "Category";
                    public const string ID_SiteAnalysis = "SiteAnalysis";
                }


                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Content)
                    {
                        retval = "信息管理";
                    }
                    else if (menuID == ID_GovPublic)
                    {
                        retval = "信息公开";
                    }
                    else if (menuID == ID_GovInteract)
                    {
                        retval = "互动交流";
                    }
                    else if (menuID == ID_Function)
                    {
                        retval = "功能管理";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "显示管理";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "用户管理";
                    }
                    else if (menuID == ID_Configration)
                    {
                        retval = "设置管理";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "生成管理";
                    }

                    else if (menuID == ID_Auxiliary)
                    {
                        retval = "辅助表管理";
                    }
                    else if (menuID == ID_Site)
                    {
                        retval = "系统应用管理";
                    }
                    else if (menuID == ID_SiteSettings)
                    {
                        retval = "应用通用设置";
                    }
                    else if (menuID == ID_Log)
                    {
                        retval = "后台运行记录";
                    }
                    else if (menuID == ID_UserLevel)
                    {
                        retval = "用户等级积分";
                    }
                    return retval;
                }

            }

            public class Permission
            {
                //public class General
                //{
                //    private General() { }

                //    public const string Auxiliary = "cms_auxiliary";                        //辅助表管理
                //    public const string Site = "cms_site";                                  //系统应用管理
                //    public const string User = "cms_user";                                  //应用用户管理
                //    public const string SiteSettings = "cms_siteSettings";                  //应用通用设置
                //    public const string Log = "cms_log";                                    //后台运行记录
                //}

                public class WebSite
                {
                    private WebSite() { }

                    public const string ContentModel = "cms_contentModel";                  //内容模型
                    public const string Category = "cms_category";                          //归类管理
                    public const string SiteAnalysis = "cms_siteAnalysis";                  //应用数据统计
                    public const string ContentTrash = "cms_contentTrash";                  //内容回收站

                    public const string GovPublicContent = "cms_govPublicContent";          //主动信息公开
                    public const string GovPublicApply = "cms_govPublicApply";          //依申请公开
                    public const string GovPublicContentConfiguration = "cms_govPublicContentConfiguration";          //主动信息公开设置
                    public const string GovPublicApplyConfiguration = "cms_govPublicApplyConfiguration";          //依申请公开设置
                    public const string GovPublicAnalysis = "cms_govPublicAnalysis";          //信息公开统计

                    public const string GovInteract = "cms_govInteract";                     //互动交流管理
                    public const string GovInteractConfiguration = "cms_govInteractConfiguration";                     //互动交流设置
                    public const string GovInteractAnalysis = "cms_govInteractAnalysis";                     //互动交流统计

                    public const string InputContentView = "cms_inputContentView";    //提交表单查看
                    public const string InputContentEdit = "cms_inputContentEdit";    //提交表单维护
                    public const string Input = "cms_input";                  //提交表单管理
                    public const string Gather = "cms_gather";                //信息采集管理
                    public const string Advertisement = "cms_advertisement";  //广告管理
                    public const string Resume = "cms_resume";                 //在线招聘管理
                    public const string Mail = "cms_mail";                    //邮件发送管理
                    public const string SEO = "cms_seo";                      //搜索引擎优化
                    public const string Tracking = "cms_tracking";            //流量统计管理
                    public const string InnerLink = "cms_innerLink";          //站内链接管理
                    public const string Restriction = "cms_restriction";                  //页面访问限制
                    public const string Backup = "cms_backup";                //数据备份恢复
                    public const string Archive = "cms_archive";            //归档内容管理
                    public const string FileManagement = "cms_fileManagement";            //应用文件管理
                    public const string AllPublish = "cms_allPublish";            //多服务器发布
                    public const string BShare = "cms_bShare";            //bShare分享插件

                    public const string Template = "cms_template";            //显示管理

                    public const string User = "cms_user";            //用户管理

                    public const string Configration = "cms_configration";    //设置管理

                    public const string Create = "cms_create";                //生成管理
                }

                public class Channel
                {
                    private Channel() { }
                    public const string ContentView = "cms_contentView";
                    public const string ContentAdd = "cms_contentAdd";
                    public const string ContentEdit = "cms_contentEdit";
                    public const string ContentDelete = "cms_contentDelete";
                    public const string ContentTranslate = "cms_contentTranslate";
                    public const string ContentArchive = "cms_contentArchive";
                    public const string ContentOrder = "cms_contentOrder";
                    public const string ChannelAdd = "cms_channelAdd";
                    public const string ChannelEdit = "cms_channelEdit";
                    public const string ChannelDelete = "cms_channelDelete";
                    public const string ChannelTranslate = "cms_channelTranslate";
                    public const string CommentCheck = "cms_commentCheck";
                    public const string CommentDelete = "cms_commentDelete";
                    public const string CreatePage = "cms_createPage";
                    public const string PublishPage = "cms_publishPage";
                    public const string ContentCheck = "cms_contentCheck";
                    public const string ContentCheckLevel1 = "cms_contentCheckLevel1";
                    public const string ContentCheckLevel2 = "cms_contentCheckLevel2";
                    public const string ContentCheckLevel3 = "cms_contentCheckLevel3";
                    public const string ContentCheckLevel4 = "cms_contentCheckLevel4";
                    public const string ContentCheckLevel5 = "cms_contentCheckLevel5";
                }

                public class GovInteract
                {
                    private GovInteract() { }
                    public const string GovInteractView = "cms_govInteractView";
                    public const string GovInteractAdd = "cms_govInteractAdd";
                    public const string GovInteractEdit = "cms_govInteractEdit";
                    public const string GovInteractDelete = "cms_govInteractDelete";
                    public const string GovInteractSwitchToTranslate = "cms_govInteractSwitchToTranslate";
                    public const string GovInteractComment = "cms_govInteractComment";
                    public const string GovInteractAccept = "cms_govInteractAccept";
                    public const string GovInteractReply = "cms_govInteractReply";
                    public const string GovInteractCheck = "cms_govInteractCheck";

                    public static string GetPermissionName(string permission)
                    {
                        string retval = string.Empty;
                        if (permission == GovInteractView)
                        {
                            retval = "浏览办件";
                        }
                        else if (permission == GovInteractAdd)
                        {
                            retval = "新增办件";
                        }
                        else if (permission == GovInteractEdit)
                        {
                            retval = "编辑办件";
                        }
                        else if (permission == GovInteractDelete)
                        {
                            retval = "删除办件";
                        }
                        else if (permission == GovInteractSwitchToTranslate)
                        {
                            retval = "转办转移";
                        }
                        else if (permission == GovInteractComment)
                        {
                            retval = "批示办件";
                        }
                        else if (permission == GovInteractAccept)
                        {
                            retval = "受理办件";
                        }
                        else if (permission == GovInteractReply)
                        {
                            retval = "办理办件";
                        }
                        else if (permission == GovInteractCheck)
                        {
                            retval = "审核办件";
                        }
                        return retval;
                    }
                }
            }
        }

        public class User
        {
            public const string AppID = "user";

            public class LeftMenu
            {

                public const string ID_User = "User";                         //用户管理
                public const string ID_UserMessage = "UserMessage";           //消息管理
                public const string ID_UserBasicSetting = "UserBasicSetting"; //用户基本设置
                public const string ID_UserLevelCredit = "UserLevelCredit";   //用户等级积分
                public const string ID_UserCenterSetting = "UserCenterSetting";//用户中心设置

                public const string ID_UserGroup = "UserGroup";//用户组管理
                #region 投稿管理 by 20160108 sofuny
                public const string ID_MLibManage = "MLibManage";//投稿管理
                public const string ID_Sub_MLibManageSetting = "MLibManageSetting";//投稿设置
                #endregion

                public const string ID_Sub_UserLevel = "UserLevel";
                public const string ID_Sub_LevelPermission = "LevelPermission";
                public const string ID_Sub_LevelRule = "LevelRule";
                public const string ID_Sub_UserTableStyle = "UserTableStyle";
                public const string ID_Sub_UserConfigRegister = "UserConfigRegister";
                public const string ID_Sub_UserConfigLogin = "UserConfigLogin";
                public const string ID_Sub_UserConfigForget = "UserConfigForget";
                public const string ID_Sub_ThirdLogin = "ThirdLogin";
                public const string ID_Sub_NoticeSetting = "NoticeSetting";
                public const string ID_Sub_UserConfigMessage = "UserConfigMessage";
                public const string ID_Sub_NoticeTemplate = "NoticeTemplate";

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_User)
                    {
                        retval = "用户管理";
                    }
                    else if (menuID == ID_UserMessage)
                    {
                        retval = "消息管理";
                    }
                    else if (menuID == ID_UserBasicSetting)
                    {
                        retval = "用户基本设置";
                    }
                    else if (menuID == ID_UserLevelCredit)
                    {
                        retval = "用户等级积分";
                    }
                    else if (menuID == ID_UserCenterSetting)
                    {
                        retval = "用户中心设置";
                    }
                    else if (menuID == ID_UserGroup)
                    {
                        retval = "用户组管理";
                    }
                    else if (menuID == ID_MLibManage)
                    {
                        retval = "投稿管理";
                    }

                    return retval;
                }
                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Sub_UserLevel)
                    {
                        retval = "用户等级积分";
                    }
                    else if (menuID == ID_Sub_LevelPermission)
                    {
                        retval = "用户等级权限";
                    }
                    else if (menuID == ID_Sub_UserTableStyle)
                    {
                        retval = "用户字段配置";
                    }
                    else if (menuID == ID_Sub_UserConfigRegister)
                    {
                        retval = "用户注册配置";
                    }
                    else if (menuID == ID_Sub_UserConfigLogin)
                    {
                        retval = "用户登录配置";
                    }
                    else if (menuID == ID_Sub_UserConfigForget)
                    {
                        retval = "忘记密码配置";
                    }
                    else if (menuID == ID_Sub_ThirdLogin)
                    {
                        retval = "授权登录管理";
                    }
                    else if (menuID == ID_Sub_NoticeSetting)
                    {
                        retval = "消息提醒";
                    }
                    else if (menuID == ID_Sub_UserConfigMessage)
                    {
                        retval = "系统通知";
                    }
                    else if (menuID == ID_Sub_LevelRule)
                    {
                        retval = "用户等级规则";
                    }
                    else if (menuID == ID_Sub_MLibManageSetting)
                    {
                        retval = "投稿设置";
                    }

                    return retval;
                }
            }

            public class Permission
            {
                /******************用户中心********************/
                //public const string UserMessage = "bairong_userMessage";
                //public const string UserBasicSetting = "bairong_userBasicSetting";
                //public const string UserLevelCredit = "bairong_userLevelCredit";
                //public const string UserCenterSetting = "bairong_userCenterSetting";


                //public const string UserLevel = "bairong_userLevel";
                //public const string LevelPermission = "bairong_levelPermission";

                //public const string UserTableStyle = "bairong_userTableStyle";
                //public const string UserConfigRegister = "bairong_userConfigRegister";
                //public const string UserConfigLogin = "bairong_userConfigLogin";
                //public const string UserConfigForget = "bairong_userConfigForget";
                //public const string ThirdLogin = "bairong_thirdLogin";
                //public const string NoticeSetting = "bairong_noticeSetting";
                //public const string UserConfigMessage = "bairong_userConfigMessage";
                //public const string NoticeTemplate = "bairong_noticeTemplate";
                //public const string SecurityQuestion = "bairong_securityQuestion";

                //public const string LevelRule = "bairong_LevelRule";

                public const string Usercenter_User = "usercenter_user";//用户管理

                public const string Usercenter_Content = "usercenter_content";//信息管理

                public const string Usercenter_Template = "usercenter_template";//显示管理
                public const string Usercenter_Create = "usercenter_create";//生成管理

                public const string Usercenter_Msg = "usercenter_msg";//消息管理

                public const string Usercenter_Setting = "usercenter_setting";//设置管理


                public const string Usercenter_UserGroup = "usercenter_userGroup";//用户组管理
                #region 投稿管理 by 20160108 sofuny
                public const string Usercenter_MLibManage = "usercenter_mLibManage";//投稿管理
                public const string Usercenter_MLibManageSetting = "usercenter_mLibManageSetting";//投稿设置
                #endregion

            }

        }

        public static List<string> GetAppIDList()
        {
            return GetAppIDList(false);
        }

        public static List<string> GetAppIDList(bool isPlatform)
        {
            List<string> list = new List<string>();
            if (isPlatform)
            {
                list.Add(AppManager.Platform.AppID);
            }
            list.Add(AppManager.CMS.AppID);
            list.Add(AppManager.WCM.AppID);
            list.Add(AppManager.WeiXin.AppID);
            list.Add(AppManager.BBS.AppID);
            list.Add(AppManager.B2C.AppID);
            list.Add(AppManager.CRM.AppID);
            list.Add(AppManager.UserCenter.AppID);
            //arraylist.Add(AppManager.Ask.AppID);
            //arraylist.Add(AppManager.Space.AppID);
            return list;
        }

        public static string GetAppName(string appID, bool isFullName)
        {
            string retval = string.Empty;
            if (StringUtils.EqualsIgnoreCase(appID, AppManager.CMS.AppID))
            {
                retval = "SiteServer CMS";
                if (isFullName) retval += " 内容管理系统";
            }
            else if (StringUtils.EqualsIgnoreCase(appID, AppManager.WCM.AppID))
            {
                retval = "SiteServer WCM";
                if (isFullName) retval += " 内容协作平台";
            }
            else if (StringUtils.EqualsIgnoreCase(appID, AppManager.BBS.AppID))
            {
                retval = "SiteServer BBS";
                if (isFullName) retval += " 论坛系统";
            }
            else if (StringUtils.EqualsIgnoreCase(appID, AppManager.B2C.AppID))
            {
                retval = "SiteServer B2C";
                if (isFullName) retval += " 电子商务系统";
            }
            else if (StringUtils.EqualsIgnoreCase(appID, AppManager.CRM.AppID))
            {
                retval = "SiteServer CRM";
                if (isFullName) retval += " 客户关系管理系统";
            }
            else if (StringUtils.EqualsIgnoreCase(appID, AppManager.WeiXin.AppID))
            {
                retval = "SiteServer WeiXin";
                if (isFullName) retval += " 微官网管理系统";
            }
            else if (StringUtils.EqualsIgnoreCase(appID, AppManager.UserCenter.AppID))
            {
                retval = "SiteServer UserCenter";
                if (isFullName) retval += " 用户管理系统";
            }

            return retval;
        }

        public static bool IsSiteServerApp(string appID)
        {
            foreach (string ssAppID in AppManager.GetAppIDList())
            {
                if (StringUtils.EqualsIgnoreCase(appID, ssAppID))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsPredefinedModule(string AppID)
        {
            if (StringUtils.EqualsIgnoreCase(AppID, AppManager.Platform.AppID) || StringUtils.EqualsIgnoreCase(AppID, AppManager.CMS.AppID) || StringUtils.EqualsIgnoreCase(AppID, AppManager.BBS.AppID) || StringUtils.EqualsIgnoreCase(AppID, AppManager.WCM.AppID) || StringUtils.EqualsIgnoreCase(AppID, AppManager.B2C.AppID) || StringUtils.EqualsIgnoreCase(AppID, AppManager.CRM.AppID)
                || StringUtils.EqualsIgnoreCase(AppID, AppManager.UserCenter.AppID))
            {
                return true;
            }
            return false;
        }

        public static bool IsPublishmentSystem(string AppID)
        {
            if (StringUtils.EqualsIgnoreCase(AppID, AppManager.CMS.AppID) || StringUtils.EqualsIgnoreCase(AppID, AppManager.WCM.AppID) || StringUtils.EqualsIgnoreCase(AppID, AppManager.B2C.AppID))
            {
                return true;
            }
            return false;
        }

        public static bool IsDirectoryExists(string appID)
        {
            string directoryPath = PathUtils.GetAppPath(appID);
            return DirectoryUtils.IsDirectoryExists(directoryPath);
        }

        #region Upgrade

        public static void Upgrade(bool isHotfix, string version, out string errorMessage)
        {
            ConfigInfo configInfo = BaiRongDataProvider.ConfigDAO.GetConfigInfo();

            errorMessage = string.Empty;
            if (!string.IsNullOrEmpty(version) && BaiRongDataProvider.ConfigDAO.GetDatabaseVersion() != version)
            {
                StringBuilder errorBuilder = new StringBuilder();

                //升级数据库
                List<string> appIDList = AppManager.GetAppIDList(true);
                foreach (string appID in appIDList)
                {
                    if (AppManager.IsDirectoryExists(appID))
                    {
                        AppManager.UpgradeAppDatabase(isHotfix, appID, errorBuilder);
                        AppManager.UpgradeAppFiles(isHotfix, appID);
                    }
                }

                errorMessage = string.Format(@"
<!--
{0}
-->
", errorBuilder.ToString());

                configInfo.DatabaseVersion = version;
            }

            //更新版本数据库版本
            configInfo.IsInitialized = true;
            configInfo.UpdateDate = DateTime.Now;
            BaiRongDataProvider.ConfigDAO.Update(configInfo);
        }

        public static void UpgradeAppFiles(bool isHotfix, string appID)
        {
            string sourcePath = PathUtils.Combine(PathUtils.GetAppPath(isHotfix, appID), DirectoryUtils.Products.Apps.Directory_Files);
            string targetPath = PathUtils.MapPath("~/");

            if (DirectoryUtils.IsDirectoryExists(sourcePath))
            {
                DirectoryUtils.Copy(sourcePath, targetPath, true);
            }
        }

        private static void UpgradeAppDatabase(bool isHotfix, string appID, StringBuilder errorBuilder)
        {
            string directoryPath = PathUtils.GetUpgradeDirectoryPath(isHotfix, BaiRongDataProvider.DatabaseType, appID);

            if (DirectoryUtils.IsDirectoryExists(directoryPath))
            {
                double version = ProductManager.GetVersionDouble(BaiRongDataProvider.ConfigDAO.GetDatabaseVersion());
                string[] directoryNames = DirectoryUtils.GetDirectoryNames(directoryPath);
                SortedList sortedlist = new SortedList();
                foreach (string directoryName in directoryNames)
                {
                    double ver = ProductManager.GetVersionDouble(directoryName);
                    if (ver > version)
                    {
                        sortedlist.Add(ver, directoryName);
                    }
                }

                foreach (double ver in sortedlist.Keys)
                {
                    string directoryName = (string)sortedlist[ver];
                    string filePath = PathUtils.Combine(directoryPath, directoryName, "upgrade.sql");
                    if (FileUtils.IsFileExists(filePath))
                    {
                        BaiRongDataProvider.DatabaseDAO.ExecuteSqlInFile(filePath, errorBuilder);
                    }

                    //升级辅助表
                    filePath = PathUtils.Combine(directoryPath, directoryName, "auxiliarytables.sql");

                    if (FileUtils.IsFileExists(filePath))
                    {
                        try
                        {
                            ArrayList tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDB();
                            foreach (AuxiliaryTableInfo table in tableArrayList)
                            {
                                BaiRongDataProvider.DatabaseDAO.ExecuteSqlInFile(filePath, table.TableENName, errorBuilder);
                            }
                        }
                        catch { }
                    }
                }
            }

            BaiRongDataProvider.TableCollectionDAO.CreateAllAuxiliaryTableIfNotExists();
        }

        #endregion
    }
}
