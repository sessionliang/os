
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

                    public const string Auxiliary = "siteserver_auxiliary";                        //���������
                    public const string Site = "siteserver_site";                                  //ϵͳӦ�ù���
                    public const string User = "siteserver_user";                                  //Ӧ���û�����
                    public const string SiteSettings = "siteserver_siteSettings";                  //Ӧ��ͨ������
                    public const string Log = "siteserver_log";                                    //��̨���м�¼
                }

                public class WebSite
                {
                    private WebSite() { }

                    public const string ContentModel = "siteserver_contentModel";                  //����ģ��
                    public const string Category = "siteserver_category";                          //�������
                    public const string SiteAnalysis = "siteserver_siteAnalysis";                  //Ӧ������ͳ��
                    public const string ContentTrash = "siteserver_contentTrash";                  //���ݻ���վ

                    public const string InputContentView = "siteserver_inputContentView";    //�ύ���鿴
                    public const string InputContentEdit = "siteserver_inputContentEdit";    //�ύ��ά��
                    public const string Input = "siteserver_input";                  //�ύ������
                    public const string Gather = "siteserver_gather";                //��Ϣ�ɼ�����
                    public const string Advertisement = "siteserver_advertisement";  //������
                    public const string Resume = "siteserver_resume";                 //������Ƹ����
                    public const string Mail = "siteserver_mail";                    //�ʼ����͹���
                    public const string SEO = "siteserver_seo";                      //���������Ż�
                    public const string Tracking = "siteserver_tracking";            //����ͳ�ƹ���
                    public const string InnerLink = "siteserver_innerLink";          //վ�����ӹ���
                    public const string Restriction = "siteserver_restriction";                  //ҳ���������
                    public const string Backup = "siteserver_backup";                //���ݱ��ݻָ�
                    public const string Signin = "siteserver_signin";            //����ǩ�չ���
                    public const string Archive = "siteserver_archive";            //�鵵���ݹ���
                    public const string FileManagement = "siteserver_fileManagement";            //Ӧ���ļ�����
                    public const string AllPublish = "siteserver_allPublish";            //�����������
                    public const string BShare = "siteserver_bShare";            //bShare������

                    public const string Template = "siteserver_template";            //��ʾ����

                    public const string Configration = "siteserver_configration";    //���ù���

                    public const string Create = "siteserver_create";                //���ɹ���
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
                /******************�û�����********************/
                public const string ID_UserCenter = "UserCenter";
                public const string ID_Statistics = "Statistics";

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Admin)
                    {
                        retval = "����Ա����";
                    }
                    else if (menuID == ID_Platform)
                    {
                        retval = "ƽ̨����";
                    }
                    else if (menuID == ID_Service)
                    {
                        retval = "�������";
                    }
                    else if (menuID == ID_UserCenter)
                    {
                        retval = "�û�����";
                    }
                    else if (menuID == ID_Statistics)
                    {
                        retval = "����ͳ��";
                    }
                    return retval;
                }
            }

            public class LeftMenu
            {
                public const string ID_Administrator = "Administrator";     //����Ա����
                public const string ID_AdminAttributes = "ID_AdminAttributes";   //�û���������

                public const string ID_Configuration = "Configuration";     //ƽ̨��������
                public const string ID_Product = "Product";                 //ƽ̨��Ʒ����
                public const string ID_Authentication = "Authentication";   //�û���֤����
                public const string ID_SMS = "SMS";                         //����ͨ����
                public const string ID_Restriction = "Restriction";         //��̨��������
                public const string ID_Database = "Database";               //���ݿ⹤��
                public const string ID_Utility = "Utility";                 //ʵ�ù���
                public const string ID_Log = "Log";                         //��̨���м�¼

                public const string ID_Service = "Service";                 //������
                public const string ID_Storage = "Storage";                 //�ƴ洢����

                public class Configuration
                {
                    public const string ID_ConfigurationMenu = "ConfigurationMenu";     //ƽ̨�˵�����
                }

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Administrator)
                    {
                        retval = "����Ա����";
                    }
                    else if (menuID == ID_AdminAttributes)
                    {
                        retval = "��������������";
                    }
                    else if (menuID == ID_Configuration)
                    {
                        retval = "ƽ̨��������";
                    }
                    else if (menuID == ID_Product)
                    {
                        retval = "ƽ̨��Ʒ����";
                    }
                    else if (menuID == ID_Authentication)
                    {
                        retval = "�û���֤����";
                    }
                    else if (menuID == ID_SMS)
                    {
                        retval = "����ͨ����";
                    }
                    else if (menuID == ID_Restriction)
                    {
                        retval = "��̨��������";
                    }
                    else if (menuID == ID_Database)
                    {
                        retval = "���ݿ⹤��";
                    }
                    else if (menuID == ID_Utility)
                    {
                        retval = "ʵ�ù���";
                    }
                    else if (menuID == ID_Log)
                    {
                        retval = "��̨���м�¼";
                    }
                    else if (menuID == ID_Service)
                    {
                        retval = "������";
                    }
                    else if (menuID == ID_Storage)
                    {
                        retval = "�ƴ洢����";
                    }
                    return retval;
                }

                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    //Configuration
                    if (menuID == Configuration.ID_ConfigurationMenu)
                    {
                        retval = "ƽ̨�˵�����";
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
                        retval = "Ӧ�ù���";
                    }
                    else if (menuID == ID_SiteConfiguration)
                    {
                        retval = "ϵͳ����";
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
                    public const string ID_GovPublicContent = "GovPublicContent";     //������Ϣ����
                    public const string ID_GovPublicApply = "GovPublicApply";         //�����빫��
                    public const string ID_GovPublicContentConfiguration = "GovPublicContentConfiguration";         //������Ϣ��������
                    public const string ID_GovPublicApplyConfiguration = "GovPublicApplyConfiguration";         //�����빫������
                    public const string ID_GovPublicAnalysis = "GovPublicAnalysis";         //����ͳ�Ʒ���
                }

                public class GovInteract
                {
                    public const string ID_GovInteractConfiguration = "GovInteractConfiguration";     //������������
                    public const string ID_GovInteractAnalysis = "GovInteractAnalysis";             //����ͳ�Ʒ���
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
                    public const string ID_Subscribe = "Subscribe";//by 20151030 sofuny ��Ϣ����
                    public const string ID_IntelligentPush = "IntelligentPush";//by 20151124 sofuny ��������
                    public const string ID_Evaluation = "Evaluation";//by 20160224 sofuny ���۹���
                    public const string ID_Trial = "Trial";//by 20160303 sofuny ���ù���
                    public const string ID_Survey = "Survey";//by 20160309 sofuny �����ʾ����
                    public const string ID_Compare = "Compare";//by 20160316 sofuny �ȽϷ�������
                    public const string ID_Searchword = "Searchword";
                    public const string ID_Special = "Special";
                    public const string ID_AdvImage = "AdvImage";
                }

                public class Template
                {
                    public const string ID_TagStyle = "TagStyle";     //ģ���ǩ��ʽ
                }

                public class Configuration
                {
                    public const string ID_ConfigurationCreate = "ConfigurationCreate";         //ҳ����������
                    public const string ID_ConfigurationStorage = "ConfigurationStorage";       //�洢�ռ�����
                    public const string ID_ConfigurationTask = "ConfigurationTask";             //��ʱ�������
                    public const string ID_ConfigurationTouGao = "ConfigurationTouGao";         //����Ͷ������
                    public const string ID_ConfigurationComment = "ConfigurationComment";       //������������
                    public const string ID_ConfigurationMachine = "ConfigurationMachine";       //����������
                }

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Content)
                    {
                        retval = "��Ϣ����";
                    }
                    else if (menuID == ID_GovPublic)
                    {
                        retval = "��Ϣ����";
                    }
                    else if (menuID == ID_GovInteract)
                    {
                        retval = "��������";
                    }
                    else if (menuID == ID_Function)
                    {
                        retval = "���ܹ���";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "��ʾ����";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "�û�����";
                    }
                    else if (menuID == ID_Configration)
                    {
                        retval = "���ù���";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "���ɹ���";
                    }

                    else if (menuID == ID_Auxiliary)
                    {
                        retval = "���������";
                    }
                    else if (menuID == ID_Site)
                    {
                        retval = "ϵͳӦ�ù���";
                    }
                    else if (menuID == ID_SiteSettings)
                    {
                        retval = "Ӧ��ͨ������";
                    }
                    else if (menuID == ID_Log)
                    {
                        retval = "��̨���м�¼";
                    }
                    else if (menuID == ID_Organization)
                    {
                        retval = "��֧��������";
                    }
                    return retval;
                }

                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    //Content
                    if (menuID == Content.ID_ContentModel)
                    {
                        retval = "����ģ��";
                    }
                    else if (menuID == Content.ID_Category)
                    {
                        retval = "�������";
                    }
                    else if (menuID == Content.ID_SiteAnalysis)
                    {
                        retval = "Ӧ������ͳ��";
                    }
                    //GovPublic
                    else if (menuID == GovPublic.ID_GovPublicContent)
                    {
                        retval = "������Ϣ����";
                    }
                    else if (menuID == GovPublic.ID_GovPublicApply)
                    {
                        retval = "�����빫��";
                    }
                    else if (menuID == GovPublic.ID_GovPublicContentConfiguration)
                    {
                        retval = "������Ϣ��������";
                    }
                    else if (menuID == GovPublic.ID_GovPublicApplyConfiguration)
                    {
                        retval = "�����빫������";
                    }
                    else if (menuID == GovPublic.ID_GovPublicAnalysis)
                    {
                        retval = "����ͳ�Ʒ���";
                    }
                    //GovInteract
                    else if (menuID == GovInteract.ID_GovInteractConfiguration)
                    {
                        retval = "������������";
                    }
                    else if (menuID == GovInteract.ID_GovInteractAnalysis)
                    {
                        retval = "����ͳ�Ʒ���";
                    }
                    //Function
                    else if (menuID == Function.ID_Input)
                    {
                        retval = "�ύ��";
                    }
                    else if (menuID == Function.ID_WebsiteMessage)
                    {
                        retval = "��վ����";
                    }
                    else if (menuID == Function.ID_Searchword)
                    {
                        retval = "�����ؼ���";
                    }
                    else if (menuID == Function.ID_Special)
                    {
                        retval = "ר�����";
                    }
                    else if (menuID == Function.ID_AdvImage)
                    {
                        retval = "������";
                    }
                    else if (menuID == Function.ID_Gather)
                    {
                        retval = "��Ϣ�ɼ�����";
                    }
                    else if (menuID == Function.ID_Advertisement)
                    {
                        retval = "������";
                    }
                    else if (menuID == Function.ID_Resume)
                    {
                        retval = "��������";
                    }
                    else if (menuID == Function.ID_Mail)
                    {
                        retval = "�ʼ����͹���";
                    }
                    else if (menuID == Function.ID_SEO)
                    {
                        retval = "���������Ż�";
                    }
                    else if (menuID == Function.ID_Tracking)
                    {
                        retval = "����ͳ�ƹ���";
                    }
                    else if (menuID == Function.ID_InnerLink)
                    {
                        retval = "վ�����ӹ���";
                    }
                    else if (menuID == Function.ID_Restriction)
                    {
                        retval = "ҳ���������";
                    }
                    else if (menuID == Function.ID_Backup)
                    {
                        retval = "���ݱ��ݻָ�";
                    }
                    else if (menuID == Function.ID_BShare)
                    {
                        retval = "bShare������";
                    }
                    else if (menuID == Function.ID_Subscribe)// by 20151030 sofuny 
                    {
                        retval = "��Ϣ����";
                    }
                    else if (menuID == Function.ID_IntelligentPush)// by 20151124 sofuny 
                    {
                        retval = "��������";
                    }
                    else if (menuID == Function.ID_Evaluation)// by 20160224 sofuny 
                    {
                        retval = "���۹���";
                    }
                    else if (menuID == Function.ID_Trial)// by 20160224 sofuny 
                    {
                        retval = "���ù���";
                    }
                    else if (menuID == Function.ID_Survey)// by 20160309 sofuny 
                    {
                        retval = "�����ʾ����";
                    }
                    else if (menuID == Function.ID_Compare)// by 20160316 sofuny 
                    {
                        retval = "�ȽϷ�������";
                    }
                    //Template
                    else if (menuID == Template.ID_TagStyle)
                    {
                        retval = "ģ���ǩ��ʽ";
                    }
                    //Configuration
                    else if (menuID == Configuration.ID_ConfigurationCreate)
                    {
                        retval = "ҳ����������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationStorage)
                    {
                        retval = "�洢�ռ�����";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTask)
                    {
                        retval = "��ʱ�������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTouGao)
                    {
                        retval = "����Ͷ������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationComment)
                    {
                        retval = "������������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationMachine)
                    {
                        retval = "����������";
                    }
                    return retval;
                }
            }

            public class Permission
            {
                //public class General
                //{
                //    private General() { }

                //    public const string Auxiliary = "cms_auxiliary";                        //���������
                //    public const string Site = "cms_site";                                  //ϵͳӦ�ù���
                //    public const string User = "cms_user";                                  //Ӧ���û�����
                //    public const string SiteSettings = "cms_siteSettings";                  //Ӧ��ͨ������
                //    public const string Log = "cms_log";                                    //��̨���м�¼
                //}

                public class WebSite
                {
                    private WebSite() { }

                    public const string ContentModel = "cms_contentModel";                  //����ģ��
                    public const string Category = "cms_category";                          //�������
                    public const string SiteAnalysis = "cms_siteAnalysis";                  //Ӧ������ͳ��
                    public const string ContentTrash = "cms_contentTrash";                  //���ݻ���վ

                    public const string GovPublicContent = "cms_govPublicContent";          //������Ϣ����
                    public const string GovPublicApply = "cms_govPublicApply";          //�����빫��
                    public const string GovPublicContentConfiguration = "cms_govPublicContentConfiguration";          //������Ϣ��������
                    public const string GovPublicApplyConfiguration = "cms_govPublicApplyConfiguration";          //�����빫������
                    public const string GovPublicAnalysis = "cms_govPublicAnalysis";          //��Ϣ����ͳ��

                    public const string GovInteract = "cms_govInteract";                     //������������
                    public const string GovInteractConfiguration = "cms_govInteractConfiguration";                     //������������
                    public const string GovInteractAnalysis = "cms_govInteractAnalysis";                     //��������ͳ��

                    public const string InputContentView = "cms_inputContentView";    //�ύ���鿴
                    public const string InputContentEdit = "cms_inputContentEdit";    //�ύ��ά��
                    public const string Input = "cms_input";                  //�ύ������
                    public const string InputPermission = "cms_inputPermission";    // ��Ȩ�޹��� by 20151205 sofuny
                    public const string InputClassifyView = "cms_inputClassifyView";    //�ύ������鿴 by 20151205 sofuny
                    public const string InputClassifyEdit = "cms_inputClassifyEdit";    //�ύ������ά�� by 20151205 sofuny
                    public const string Gather = "cms_gather";                //��Ϣ�ɼ�����
                    public const string Advertisement = "cms_advertisement";  //������
                    public const string Resume = "cms_resume";                 //������Ƹ����
                    public const string Mail = "cms_mail";                    //�ʼ����͹���
                    public const string SEO = "cms_seo";                      //���������Ż�
                    public const string Tracking = "cms_tracking";            //����ͳ�ƹ���
                    public const string InnerLink = "cms_innerLink";          //վ�����ӹ���
                    public const string Restriction = "cms_restriction";                  //ҳ���������
                    public const string Backup = "cms_backup";                //���ݱ��ݻָ�
                    public const string Archive = "cms_archive";            //�鵵���ݹ���
                    public const string FileManagement = "cms_fileManagement";            //Ӧ���ļ�����
                    public const string AllPublish = "cms_allPublish";            //�����������
                    public const string BShare = "cms_bShare";            //bShare������

                    public const string WebsiteMessageContentView = "cms_websiteMessageContentView";    //վ�����Բ鿴
                    public const string WebsiteMessageContentEdit = "cms_websiteMessageContentEdit";    //վ������ά��
                    public const string WebsiteMessage = "cms_websiteMessage";                  //վ�����Թ���

                    public const string Searchword = "cms_searchword";                  //�����ؼ��ʹ���

                    public const string SpecialContentView = "cms_specialContentView";    //ר�����ݲ鿴
                    public const string SpecialContentEdit = "cms_specialContentEdit";    //ר������ά��
                    public const string Special = "cms_special";                  //ר�����

                    public const string AdvImageContentView = "cms_advImageContentView";    //������ݲ鿴
                    public const string AdvImageContentEdit = "cms_advImageContentEdit";    //�������ά��
                    public const string AdvImage = "cms_advImage";                  //������


                    public const string Template = "cms_template";            //��ʾ����

                    public const string User = "cms_user";            //�û�����

                    public const string Configration = "cms_configration";    //���ù���

                    public const string Create = "cms_create";                //���ɹ���


                    // by sofuny
                    public const string Subscribe = "cms_subscribe";                  //��Ϣ����
                    public const string SubscribeView = "cms_subscribeView";    //��Ϣ���Ĳ鿴
                    public const string IntelligentPush = "cms_intelligentPush";    //��������
                    public const string Evaluation = "cms_evaluation";    //���۹���
                    public const string Trial = "cms_trial";    //���ù���
                    public const string Survey = "cms_survey";    //�����ʾ����
                    public const string Compare = "cms_compare";    //�ȽϷ�������
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
                            retval = "������";
                        }
                        else if (permission == GovInteractAdd)
                        {
                            retval = "�������";
                        }
                        else if (permission == GovInteractEdit)
                        {
                            retval = "�༭���";
                        }
                        else if (permission == GovInteractDelete)
                        {
                            retval = "ɾ�����";
                        }
                        else if (permission == GovInteractSwitchToTranslate)
                        {
                            retval = "ת��ת��";
                        }
                        else if (permission == GovInteractComment)
                        {
                            retval = "��ʾ���";
                        }
                        else if (permission == GovInteractAccept)
                        {
                            retval = "������";
                        }
                        else if (permission == GovInteractReply)
                        {
                            retval = "������";
                        }
                        else if (permission == GovInteractCheck)
                        {
                            retval = "��˰��";
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
                        retval = "Ӧ�ù���";
                    }
                    else if (menuID == ID_SiteConfiguration)
                    {
                        retval = "ϵͳ����";
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
                    public const string ID_GovPublicContent = "GovPublicContent";     //������Ϣ����
                    public const string ID_GovPublicApply = "GovPublicApply";         //�����빫��
                    public const string ID_GovPublicContentConfiguration = "GovPublicContentConfiguration";         //������Ϣ��������
                    public const string ID_GovPublicApplyConfiguration = "GovPublicApplyConfiguration";         //�����빫������
                    public const string ID_GovPublicAnalysis = "GovPublicAnalysis";         //����ͳ�Ʒ���
                }

                public class GovInteract
                {
                    public const string ID_GovInteractConfiguration = "GovInteractConfiguration";     //������������
                    public const string ID_GovInteractAnalysis = "GovInteractAnalysis";             //����ͳ�Ʒ���
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
                    public const string ID_TagStyle = "TagStyle";     //ģ���ǩ��ʽ
                }

                public class Configuration
                {
                    public const string ID_ConfigurationCreate = "ConfigurationCreate";         //ҳ����������
                    public const string ID_ConfigurationStorage = "ConfigurationStorage";       //�洢�ռ�����
                    public const string ID_ConfigurationTask = "ConfigurationTask";             //��ʱ�������
                    public const string ID_ConfigurationTouGao = "ConfigurationTouGao";   //����Ͷ������
                    public const string ID_ConfigurationComment = "ConfigurationComment";   //������������
                    public const string ID_ConfigurationMachine = "ConfigurationMachine";   //����������
                }

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Content)
                    {
                        retval = "��Ϣ����";
                    }
                    else if (menuID == ID_GovPublic)
                    {
                        retval = "��Ϣ����";
                    }
                    else if (menuID == ID_GovInteract)
                    {
                        retval = "��������";
                    }
                    else if (menuID == ID_Function)
                    {
                        retval = "���ܹ���";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "��ʾ����";
                    }
                    else if (menuID == ID_Configration)
                    {
                        retval = "���ù���";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "���ɹ���";
                    }

                    else if (menuID == ID_Auxiliary)
                    {
                        retval = "���������";
                    }
                    else if (menuID == ID_Site)
                    {
                        retval = "ϵͳӦ�ù���";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "Ӧ���û�����";
                    }
                    else if (menuID == ID_SiteSettings)
                    {
                        retval = "Ӧ��ͨ������";
                    }
                    else if (menuID == ID_Log)
                    {
                        retval = "��̨���м�¼";
                    }
                    return retval;
                }

                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    //Content
                    if (menuID == Content.ID_ContentModel)
                    {
                        retval = "����ģ��";
                    }
                    else if (menuID == Content.ID_Category)
                    {
                        retval = "�������";
                    }
                    else if (menuID == Content.ID_SiteAnalysis)
                    {
                        retval = "Ӧ������ͳ��";
                    }
                    //GovPublic
                    else if (menuID == GovPublic.ID_GovPublicContent)
                    {
                        retval = "������Ϣ����";
                    }
                    else if (menuID == GovPublic.ID_GovPublicApply)
                    {
                        retval = "�����빫��";
                    }
                    else if (menuID == GovPublic.ID_GovPublicContentConfiguration)
                    {
                        retval = "������Ϣ��������";
                    }
                    else if (menuID == GovPublic.ID_GovPublicApplyConfiguration)
                    {
                        retval = "�����빫������";
                    }
                    else if (menuID == GovPublic.ID_GovPublicAnalysis)
                    {
                        retval = "����ͳ�Ʒ���";
                    }
                    //GovInteract
                    else if (menuID == GovInteract.ID_GovInteractConfiguration)
                    {
                        retval = "������������";
                    }
                    else if (menuID == GovInteract.ID_GovInteractAnalysis)
                    {
                        retval = "����ͳ�Ʒ���";
                    }
                    //Function
                    else if (menuID == Function.ID_Input)
                    {
                        retval = "�ύ��";
                    }
                    else if (menuID == Function.ID_Gather)
                    {
                        retval = "��Ϣ�ɼ�����";
                    }
                    else if (menuID == Function.ID_Advertisement)
                    {
                        retval = "������";
                    }
                    else if (menuID == Function.ID_Resume)
                    {
                        retval = "��������";
                    }
                    else if (menuID == Function.ID_Mail)
                    {
                        retval = "�ʼ����͹���";
                    }
                    else if (menuID == Function.ID_SEO)
                    {
                        retval = "���������Ż�";
                    }
                    else if (menuID == Function.ID_Tracking)
                    {
                        retval = "����ͳ�ƹ���";
                    }
                    else if (menuID == Function.ID_InnerLink)
                    {
                        retval = "վ�����ӹ���";
                    }
                    else if (menuID == Function.ID_Restriction)
                    {
                        retval = "ҳ���������";
                    }
                    else if (menuID == Function.ID_Backup)
                    {
                        retval = "���ݱ��ݻָ�";
                    }
                    else if (menuID == Function.ID_Signin)
                    {
                        retval = "����ǩ�չ���";
                    }
                    else if (menuID == Function.ID_BShare)
                    {
                        retval = "bShare������";
                    }
                    //Template
                    else if (menuID == Template.ID_TagStyle)
                    {
                        retval = "ģ���ǩ��ʽ";
                    }
                    //Configuration
                    else if (menuID == Configuration.ID_ConfigurationCreate)
                    {
                        retval = "ҳ����������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationStorage)
                    {
                        retval = "�洢�ռ�����";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTask)
                    {
                        retval = "��ʱ�������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTouGao)
                    {
                        retval = "����Ͷ������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationComment)
                    {
                        retval = "������������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationMachine)
                    {
                        retval = "����������";
                    }
                    return retval;
                }
            }

            public class Permission
            {
                public class General
                {
                    private General() { }

                    public const string Auxiliary = "wcm_auxiliary";                        //���������
                    public const string Site = "wcm_site";                                  //ϵͳӦ�ù���
                    public const string User = "wcm_user";                                  //Ӧ���û�����
                    public const string SiteSettings = "wcm_siteSettings";                  //Ӧ��ͨ������
                    public const string Log = "wcm_log";                                    //��̨���м�¼
                }

                public class WebSite
                {
                    private WebSite() { }

                    public const string ContentModel = "wcm_contentModel";                  //����ģ��
                    public const string Category = "wcm_category";                          //�������
                    public const string SiteAnalysis = "wcm_siteAnalysis";                  //Ӧ������ͳ��
                    public const string ContentTrash = "wcm_contentTrash";                  //���ݻ���վ

                    public const string GovPublicContent = "wcm_govPublicContent";          //������Ϣ����
                    public const string GovPublicApply = "wcm_govPublicApply";          //�����빫��
                    public const string GovPublicContentConfiguration = "wcm_govPublicContentConfiguration";          //������Ϣ��������
                    public const string GovPublicApplyConfiguration = "wcm_govPublicApplyConfiguration";          //�����빫������
                    public const string GovPublicAnalysis = "wcm_govPublicAnalysis";          //��Ϣ����ͳ��

                    public const string GovInteract = "wcm_govInteract";                     //������������
                    public const string GovInteractConfiguration = "wcm_govInteractConfiguration";                     //������������
                    public const string GovInteractAnalysis = "wcm_govInteractAnalysis";                     //��������ͳ��

                    public const string InputContentView = "wcm_inputContentView";    //�ύ���鿴
                    public const string InputContentEdit = "wcm_inputContentEdit";    //�ύ��ά��
                    public const string Input = "wcm_input";                  //�ύ������
                    public const string Gather = "wcm_gather";                //��Ϣ�ɼ�����
                    public const string Advertisement = "wcm_advertisement";  //������
                    public const string Resume = "wcm_resume";                 //������Ƹ����
                    public const string Mail = "wcm_mail";                    //�ʼ����͹���
                    public const string SEO = "wcm_seo";                      //���������Ż�
                    public const string Tracking = "wcm_tracking";            //����ͳ�ƹ���
                    public const string InnerLink = "wcm_innerLink";          //վ�����ӹ���
                    public const string Restriction = "wcm_restriction";                  //ҳ���������
                    public const string Backup = "wcm_backup";                //���ݱ��ݻָ�
                    public const string Signin = "wcm_signin";            //����ǩ�չ���
                    public const string Archive = "wcm_archive";            //�鵵���ݹ���
                    public const string FileManagement = "wcm_fileManagement";            //Ӧ���ļ�����
                    public const string AllPublish = "wcm_allPublish";            //�����������
                    public const string BShare = "wcm_bShare";            //bShare������

                    public const string Template = "wcm_template";            //��ʾ����

                    public const string Configration = "wcm_configration";    //���ù���

                    public const string Create = "wcm_create";                //���ɹ���
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
                            retval = "������";
                        }
                        else if (permission == GovInteractAdd)
                        {
                            retval = "�������";
                        }
                        else if (permission == GovInteractEdit)
                        {
                            retval = "�༭���";
                        }
                        else if (permission == GovInteractDelete)
                        {
                            retval = "ɾ�����";
                        }
                        else if (permission == GovInteractSwitchToTranslate)
                        {
                            retval = "ת��ת��";
                        }
                        else if (permission == GovInteractComment)
                        {
                            retval = "��ʾ���";
                        }
                        else if (permission == GovInteractAccept)
                        {
                            retval = "������";
                        }
                        else if (permission == GovInteractReply)
                        {
                            retval = "������";
                        }
                        else if (permission == GovInteractCheck)
                        {
                            retval = "��˰��";
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
                        retval = "��̳����";
                    }
                    return retval;
                }
            }

            public class LeftMenu
            {
                public const string ID_Forum = "Forum";             //������
                public const string ID_Content = "Content";         //���ݹ���
                public const string ID_User = "User";               //�û�����
                public const string ID_Settings = "Settings";       //ϵͳ����
                public const string ID_Template = "Template";       //�������
                public const string ID_Create = "Create";           //���ɹ���

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Forum)
                    {
                        retval = "������";
                    }
                    else if (menuID == ID_Content)
                    {
                        retval = "���ݹ���";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "�û�����";
                    }
                    else if (menuID == ID_Settings)
                    {
                        retval = "ϵͳ����";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "�������";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "���ɹ���";
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
                        retval = "�̳ǹ���";
                    }
                    else if (menuID == ID_SiteConfiguration)
                    {
                        retval = "�̳�����";
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
                    public const string ID_GovInteractConfiguration = "GovInteractConfiguration";     //������������
                    public const string ID_GovInteractAnalysis = "GovInteractAnalysis";             //����ͳ�Ʒ���
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
                    public const string ID_TagStyle = "TagStyle";     //ģ���ǩ��ʽ
                }

                public class Configuration
                {
                    public const string ID_ConfigurationCreate = "ConfigurationCreate";         //ҳ����������
                    public const string ID_ConfigurationStorage = "ConfigurationStorage";       //�洢�ռ�����
                    public const string ID_ConfigurationTask = "ConfigurationTask";             //��ʱ�������
                    public const string ID_ConfigurationTouGao = "ConfigurationTouGao";   //����Ͷ������
                    public const string ID_ConfigurationComment = "ConfigurationComment";   //������������
                    public const string ID_ConfigurationMachine = "ConfigurationMachine";   //����������
                }

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Content)
                    {
                        retval = "��Ϣ����";
                    }
                    else if (menuID == ID_Order)
                    {
                        retval = "��������";
                    }
                    else if (menuID == ID_Function)
                    {
                        retval = "���ܹ���";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "��ʾ����";
                    }
                    else if (menuID == ID_ConfigrationB2C)
                    {
                        retval = "�̳�����";
                    }
                    else if (menuID == ID_ConfigrationSite)
                    {
                        retval = "Ӧ������";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "���ɹ���";
                    }

                    else if (menuID == ID_Auxiliary)
                    {
                        retval = "���������";
                    }
                    else if (menuID == ID_Site)
                    {
                        retval = "ϵͳӦ�ù���";
                    }
                    else if (menuID == ID_PaymentShipment)
                    {
                        retval = "֧�������ͷ�ʽ";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "Ӧ���û�����";
                    }
                    else if (menuID == ID_SiteSettings)
                    {
                        retval = "Ӧ��ͨ������";
                    }
                    else if (menuID == ID_Log)
                    {
                        retval = "��̨���м�¼";
                    }
                    return retval;
                }

                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    //Content
                    if (menuID == Content.ID_ContentModel)
                    {
                        retval = "����ģ��";
                    }
                    else if (menuID == Content.ID_Category)
                    {
                        retval = "�������";
                    }
                    else if (menuID == Content.ID_SiteAnalysis)
                    {
                        retval = "Ӧ������ͳ��";
                    }
                    //GovInteract
                    else if (menuID == GovInteract.ID_GovInteractConfiguration)
                    {
                        retval = "������������";
                    }
                    else if (menuID == GovInteract.ID_GovInteractAnalysis)
                    {
                        retval = "����ͳ�Ʒ���";
                    }
                    //Function
                    else if (menuID == Function.ID_Input)
                    {
                        retval = "�ύ��";
                    }
                    else if (menuID == Function.ID_Gather)
                    {
                        retval = "��Ϣ�ɼ�����";
                    }
                    else if (menuID == Function.ID_Advertisement)
                    {
                        retval = "������";
                    }
                    else if (menuID == Function.ID_Resume)
                    {
                        retval = "��������";
                    }
                    else if (menuID == Function.ID_Mail)
                    {
                        retval = "�ʼ����͹���";
                    }
                    else if (menuID == Function.ID_SEO)
                    {
                        retval = "���������Ż�";
                    }
                    else if (menuID == Function.ID_Tracking)
                    {
                        retval = "����ͳ�ƹ���";
                    }
                    else if (menuID == Function.ID_InnerLink)
                    {
                        retval = "վ�����ӹ���";
                    }
                    else if (menuID == Function.ID_Restriction)
                    {
                        retval = "ҳ���������";
                    }
                    else if (menuID == Function.ID_Backup)
                    {
                        retval = "���ݱ��ݻָ�";
                    }
                    else if (menuID == Function.ID_Signin)
                    {
                        retval = "����ǩ�չ���";
                    }
                    else if (menuID == Function.ID_BShare)
                    {
                        retval = "bShare������";
                    }
                    //Template
                    else if (menuID == Template.ID_TagStyle)
                    {
                        retval = "ģ���ǩ��ʽ";
                    }
                    //Configuration
                    else if (menuID == Configuration.ID_ConfigurationCreate)
                    {
                        retval = "ҳ����������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationStorage)
                    {
                        retval = "�洢�ռ�����";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTask)
                    {
                        retval = "��ʱ�������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTouGao)
                    {
                        retval = "����Ͷ������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationComment)
                    {
                        retval = "������������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationMachine)
                    {
                        retval = "����������";
                    }
                    return retval;
                }
            }

            public class Permission
            {
                public class General
                {
                    private General() { }

                    public const string Auxiliary = "wcm_auxiliary";                        //���������
                    public const string Site = "wcm_site";                                  //ϵͳӦ�ù���
                    public const string User = "wcm_user";                                  //Ӧ���û�����
                    public const string SiteSettings = "wcm_siteSettings";                  //Ӧ��ͨ������
                    public const string Log = "wcm_log";                                    //��̨���м�¼
                }

                public class WebSite
                {
                    private WebSite() { }

                    public const string ContentModel = "wcm_contentModel";                  //����ģ��
                    public const string Category = "wcm_category";                          //�������
                    public const string SiteAnalysis = "wcm_siteAnalysis";                  //Ӧ������ͳ��
                    public const string ContentTrash = "wcm_contentTrash";                  //���ݻ���վ

                    public const string GovPublicContent = "wcm_govPublicContent";          //������Ϣ����
                    public const string GovPublicApply = "wcm_govPublicApply";          //�����빫��
                    public const string GovPublicContentConfiguration = "wcm_govPublicContentConfiguration";          //������Ϣ��������
                    public const string GovPublicApplyConfiguration = "wcm_govPublicApplyConfiguration";          //�����빫������
                    public const string GovPublicAnalysis = "wcm_govPublicAnalysis";          //��Ϣ����ͳ��

                    public const string GovInteract = "wcm_govInteract";                     //������������
                    public const string GovInteractConfiguration = "wcm_govInteractConfiguration";                     //������������
                    public const string GovInteractAnalysis = "wcm_govInteractAnalysis";                     //��������ͳ��

                    public const string InputContentView = "wcm_inputContentView";    //�ύ���鿴
                    public const string InputContentEdit = "wcm_inputContentEdit";    //�ύ��ά��
                    public const string Input = "wcm_input";                  //�ύ������
                    public const string Gather = "wcm_gather";                //��Ϣ�ɼ�����
                    public const string Advertisement = "wcm_advertisement";  //������
                    public const string Resume = "wcm_resume";                 //������Ƹ����
                    public const string Mail = "wcm_mail";                    //�ʼ����͹���
                    public const string SEO = "wcm_seo";                      //���������Ż�
                    public const string Tracking = "wcm_tracking";            //����ͳ�ƹ���
                    public const string InnerLink = "wcm_innerLink";          //վ�����ӹ���
                    public const string Restriction = "wcm_restriction";                  //ҳ���������
                    public const string Backup = "wcm_backup";                //���ݱ��ݻָ�
                    public const string Signin = "wcm_signin";            //����ǩ�չ���
                    public const string ContentArchive = "wcm_contentArchive";            //�鵵���ݹ���
                    public const string FileManagement = "wcm_fileManagement";            //Ӧ���ļ�����
                    public const string AllPublish = "wcm_allPublish";            //�����������
                    public const string BShare = "wcm_bShare";            //bShare������

                    public const string Template = "wcm_template";            //��ʾ����

                    public const string Configration = "wcm_configration";    //���ù���

                    public const string Create = "wcm_create";                //���ɹ���
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
                            retval = "������";
                        }
                        else if (permission == GovInteractAdd)
                        {
                            retval = "�������";
                        }
                        else if (permission == GovInteractEdit)
                        {
                            retval = "�༭���";
                        }
                        else if (permission == GovInteractDelete)
                        {
                            retval = "ɾ�����";
                        }
                        else if (permission == GovInteractSwitchToTranslate)
                        {
                            retval = "ת��ת��";
                        }
                        else if (permission == GovInteractComment)
                        {
                            retval = "��ʾ���";
                        }
                        else if (permission == GovInteractAccept)
                        {
                            retval = "������";
                        }
                        else if (permission == GovInteractReply)
                        {
                            retval = "������";
                        }
                        else if (permission == GovInteractCheck)
                        {
                            retval = "��˰��";
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
                        retval = "��̳����";
                    }
                    return retval;
                }
            }

            public class LeftMenu
            {
                public const string ID_Forum = "Forum";             //������
                public const string ID_Content = "Content";         //���ݹ���
                public const string ID_User = "User";               //�û�����
                public const string ID_Settings = "Settings";       //ϵͳ����
                public const string ID_Template = "Template";       //�������
                public const string ID_Create = "Create";           //���ɹ���

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Forum)
                    {
                        retval = "������";
                    }
                    else if (menuID == ID_Content)
                    {
                        retval = "���ݹ���";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "�û�����";
                    }
                    else if (menuID == ID_Settings)
                    {
                        retval = "ϵͳ����";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "�������";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "���ɹ���";
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
                        retval = "΢�Ź���";
                    }
                    else if (menuID == ID_SiteConfiguration)
                    {
                        retval = "΢������";
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
                        retval = "�����˺�";
                    }
                    else if (menuID == ID_Function)
                    {
                        retval = "΢����";
                    }
                    return retval;
                }

                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    //Accounts
                    if (menuID == Function.ID_Info)
                    {
                        retval = "�˻���Ϣ";
                    }
                    else if (menuID == Function.ID_Chart)
                    {
                        retval = "��Ӫͼ��";
                    }
                    else if (menuID == Function.ID_Menu)
                    {
                        retval = "�Զ���˵�";
                    }
                    else if (menuID == Function.ID_TextReply)
                    {
                        retval = "�ؼ����ı��ظ�";
                    }
                    else if (menuID == Function.ID_ImageReply)
                    {
                        retval = "�ؼ���ͼ�Ļظ�";
                    }
                    else if (menuID == Function.ID_SetReply)
                    {
                        retval = "�ؼ��ʻظ�����";
                    }
                    //Function
                    else if (menuID == Function.ID_Coupon)
                    {
                        retval = "�Ż�ȯ";
                    }
                    else if (menuID == Function.ID_Scratch)
                    {
                        retval = "�ιο�";
                    }
                    else if (menuID == Function.ID_BigWheel)
                    {
                        retval = "��ת��";
                    }
                    else if (menuID == Function.ID_GoldEgg)
                    {
                        retval = "�ҽ�";
                    }
                    else if (menuID == Function.ID_Flap)
                    {
                        retval = "����";
                    }
                    else if (menuID == Function.ID_YaoYao)
                    {
                        retval = "ҡҡ��";
                    }
                    else if (menuID == Function.ID_Vote)
                    {
                        retval = "΢ͶƱ";
                    }
                    else if (menuID == Function.ID_Message)
                    {
                        retval = "΢����";
                    }
                    else if (menuID == Function.ID_Appointment)
                    {
                        retval = "΢ԤԼ";
                    }
                    else if (menuID == Function.ID_Conference)
                    {
                        retval = "΢����";
                    }
                    else if (menuID == Function.ID_Map)
                    {
                        retval = "΢����";
                    }
                    else if (menuID == Function.ID_View360)
                    {
                        retval = "360ȫ��";
                    }
                    else if (menuID == Function.ID_Album)
                    {
                        retval = "΢���";
                    }
                    else if (menuID == Function.ID_Search)
                    {
                        retval = "΢����";
                    }
                    else if (menuID == Function.ID_Store)
                    {
                        retval = "΢�ŵ�";
                    }
                    else if (menuID == Function.ID_Wifi)
                    {
                        retval = "΢Wifi";
                    }
                    else if (menuID == Function.ID_Card)
                    {
                        retval = "��Ա��";
                    }
                    else if (menuID == Function.ID_Collect)
                    {
                        retval = "΢�ռ�";
                    }
                    return retval;
                }
            }

            public class Permission
            {
                public class WebSite
                {
                    private WebSite() { }


                    public const string Info = "weixin_info";                       //�˻���Ϣ
                    public const string Chart = "weixin_chart";                     //��Ӫͼ��
                    public const string Menu = "weixin_menu";                       //�˵�
                    public const string TextReply = "weixin_textReply";             //�ı��ظ�
                    public const string ImageReply = "weixin_imageReply";           //ͼ�Ļظ�
                    public const string SetReply = "weixin_setReply";               //�ظ�����


                    public const string Coupon = "weixin_coupon";               //�Ż�ȯ����
                    public const string Scratch = "weixin_scratch";             //�ιο�����
                    public const string BigWheel = "weixin_bigWheel";           //��ת�̹���
                    public const string GoldEgg = "weixin_goldEgg";             //�ҽ𵰹���
                    public const string Flap = "weixin_flap";                   //���ƹ���
                    public const string YaoYao = "weixin_yaoYao";               //ҡҡ�ֹ���
                    public const string Vote = "weixin_vote";                   //΢ͶƱ����
                    public const string Message = "weixin_message";             //΢���Թ���
                    public const string Appointment = "weixin_appointment";            //΢ԤԼ����

                    public const string Conference = "weixin_conference";       //΢�������
                    public const string Map = "weixin_map";                     //΢��������
                    public const string View360 = "weixin_view360";             //ȫ������
                    public const string Album = "weixin_album";                 //΢������
                    public const string Search = "weixin_search";               //΢��������
                    public const string Store = "weixin_store";                 //΢�ŵ����
                    public const string Wifi = "weixin_wifi";                   //΢wifi����
                    public const string Card = "weixin_card";                   //΢��Ա����
                    public const string Collect = "weixin_collect";             //΢��������
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
                        retval = "΢�Ź���";
                    }
                    else if (menuID == ID_SiteConfiguration)
                    {
                        retval = "΢������";
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
                    public const string ID_GovInteractConfiguration = "GovInteractConfiguration";     //������������
                    public const string ID_GovInteractAnalysis = "GovInteractAnalysis";             //����ͳ�Ʒ���
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
                    public const string ID_TagStyle = "TagStyle";     //ģ���ǩ��ʽ
                }

                public class Configuration
                {
                    public const string ID_ConfigurationCreate = "ConfigurationCreate";         //ҳ����������
                    public const string ID_ConfigurationStorage = "ConfigurationStorage";       //�洢�ռ�����
                    public const string ID_ConfigurationTask = "ConfigurationTask";             //��ʱ�������
                    public const string ID_ConfigurationTouGao = "ConfigurationTouGao";   //����Ͷ������
                    public const string ID_ConfigurationComment = "ConfigurationComment";   //������������
                    public const string ID_ConfigurationMachine = "ConfigurationMachine";   //����������
                }

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Content)
                    {
                        retval = "��Ϣ����";
                    }
                    else if (menuID == ID_Order)
                    {
                        retval = "��������";
                    }
                    else if (menuID == ID_Function)
                    {
                        retval = "���ܹ���";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "��ʾ����";
                    }
                    else if (menuID == ID_ConfigrationB2C)
                    {
                        retval = "�̳�����";
                    }
                    else if (menuID == ID_ConfigrationSite)
                    {
                        retval = "Ӧ������";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "���ɹ���";
                    }

                    else if (menuID == ID_Auxiliary)
                    {
                        retval = "���������";
                    }
                    else if (menuID == ID_Site)
                    {
                        retval = "ϵͳӦ�ù���";
                    }
                    else if (menuID == ID_PaymentShipment)
                    {
                        retval = "֧�������ͷ�ʽ";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "Ӧ���û�����";
                    }
                    else if (menuID == ID_SiteSettings)
                    {
                        retval = "Ӧ��ͨ������";
                    }
                    else if (menuID == ID_Log)
                    {
                        retval = "��̨���м�¼";
                    }
                    return retval;
                }

                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    //Content
                    if (menuID == Content.ID_ContentModel)
                    {
                        retval = "����ģ��";
                    }
                    else if (menuID == Content.ID_Category)
                    {
                        retval = "�������";
                    }
                    else if (menuID == Content.ID_SiteAnalysis)
                    {
                        retval = "Ӧ������ͳ��";
                    }
                    //GovInteract
                    else if (menuID == GovInteract.ID_GovInteractConfiguration)
                    {
                        retval = "������������";
                    }
                    else if (menuID == GovInteract.ID_GovInteractAnalysis)
                    {
                        retval = "����ͳ�Ʒ���";
                    }
                    //Function
                    else if (menuID == Function.ID_Input)
                    {
                        retval = "�ύ��";
                    }
                    else if (menuID == Function.ID_Gather)
                    {
                        retval = "��Ϣ�ɼ�����";
                    }
                    else if (menuID == Function.ID_Advertisement)
                    {
                        retval = "������";
                    }
                    else if (menuID == Function.ID_Resume)
                    {
                        retval = "��������";
                    }
                    else if (menuID == Function.ID_Mail)
                    {
                        retval = "�ʼ����͹���";
                    }
                    else if (menuID == Function.ID_SEO)
                    {
                        retval = "���������Ż�";
                    }
                    else if (menuID == Function.ID_Tracking)
                    {
                        retval = "����ͳ�ƹ���";
                    }
                    else if (menuID == Function.ID_InnerLink)
                    {
                        retval = "վ�����ӹ���";
                    }
                    else if (menuID == Function.ID_Restriction)
                    {
                        retval = "ҳ���������";
                    }
                    else if (menuID == Function.ID_Backup)
                    {
                        retval = "���ݱ��ݻָ�";
                    }
                    else if (menuID == Function.ID_Signin)
                    {
                        retval = "����ǩ�չ���";
                    }
                    else if (menuID == Function.ID_BShare)
                    {
                        retval = "bShare������";
                    }
                    //Template
                    else if (menuID == Template.ID_TagStyle)
                    {
                        retval = "ģ���ǩ��ʽ";
                    }
                    //Configuration
                    else if (menuID == Configuration.ID_ConfigurationCreate)
                    {
                        retval = "ҳ����������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationStorage)
                    {
                        retval = "�洢�ռ�����";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTask)
                    {
                        retval = "��ʱ�������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTouGao)
                    {
                        retval = "����Ͷ������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationComment)
                    {
                        retval = "������������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationMachine)
                    {
                        retval = "����������";
                    }
                    return retval;
                }
            }

            public class Permission
            {
                public class General
                {
                    private General() { }

                    public const string Auxiliary = "wcm_auxiliary";                        //���������
                    public const string Site = "wcm_site";                                  //ϵͳӦ�ù���
                    public const string User = "wcm_user";                                  //Ӧ���û�����
                    public const string SiteSettings = "wcm_siteSettings";                  //Ӧ��ͨ������
                    public const string Log = "wcm_log";                                    //��̨���м�¼
                }

                public class WebSite
                {
                    private WebSite() { }

                    public const string Info = "weixin_info";                       //�˻���Ϣ
                    public const string Chart = "weixin_chart";                     //��Ӫͼ��
                    public const string Menu = "weixin_menu";                       //�˵�
                    public const string TextReply = "weixin_textReply";             //�ı��ظ�
                    public const string ImageReply = "weixin_imageReply";           //ͼ�Ļظ�
                    public const string SetReply = "weixin_setReply";               //�ظ�����


                    public const string Coupon = "weixin_coupon";               //�Ż�ȯ����
                    public const string Scratch = "weixin_scratch";             //�ιο�����
                    public const string BigWheel = "weixin_bigWheel";           //��ת�̹���
                    public const string GoldEgg = "weixin_goldEgg";             //�ҽ𵰹���
                    public const string Flap = "weixin_flap";                   //���ƹ���
                    public const string YaoYao = "weixin_yaoYao";               //ҡҡ�ֹ���
                    public const string Vote = "weixin_vote";                   //΢ͶƱ����
                    public const string Message = "weixin_message";             //΢���Թ���
                    public const string Appointment = "weixin_appointment";            //΢ԤԼ����

                    public const string Conference = "weixin_conference";       //΢�������
                    public const string Map = "weixin_map";                     //΢��������
                    public const string View360 = "weixin_view360";             //ȫ������
                    public const string Album = "weixin_album";                 //΢������
                    public const string Search = "weixin_search";               //΢��������
                    public const string Store = "weixin_store";                 //΢�ŵ����
                    public const string Wifi = "weixin_wifi";                   //΢wifi����
                    public const string Card = "weixin_card";                   //΢��Ա����
                    public const string Collect = "weixin_collect";             //΢��������

                    public const string ContentModel = "wcm_contentModel";                  //����ģ��
                    public const string Category = "wcm_category";                          //�������
                    public const string SiteAnalysis = "wcm_siteAnalysis";                  //Ӧ������ͳ��
                    public const string ContentTrash = "wcm_contentTrash";                  //���ݻ���վ

                    public const string GovPublicContent = "wcm_govPublicContent";          //������Ϣ����
                    public const string GovPublicApply = "wcm_govPublicApply";          //�����빫��
                    public const string GovPublicContentConfiguration = "wcm_govPublicContentConfiguration";          //������Ϣ��������
                    public const string GovPublicApplyConfiguration = "wcm_govPublicApplyConfiguration";          //�����빫������
                    public const string GovPublicAnalysis = "wcm_govPublicAnalysis";          //��Ϣ����ͳ��

                    public const string GovInteract = "wcm_govInteract";                     //������������
                    public const string GovInteractConfiguration = "wcm_govInteractConfiguration";                     //������������
                    public const string GovInteractAnalysis = "wcm_govInteractAnalysis";                     //��������ͳ��

                    public const string InputContentView = "wcm_inputContentView";    //�ύ���鿴
                    public const string InputContentEdit = "wcm_inputContentEdit";    //�ύ��ά��
                    public const string Input = "wcm_input";                  //�ύ������
                    public const string Gather = "wcm_gather";                //��Ϣ�ɼ�����
                    public const string Advertisement = "wcm_advertisement";  //������
                    public const string Resume = "wcm_resume";                 //������Ƹ����
                    public const string Mail = "wcm_mail";                    //�ʼ����͹���
                    public const string SEO = "wcm_seo";                      //���������Ż�
                    public const string Tracking = "wcm_tracking";            //����ͳ�ƹ���
                    public const string InnerLink = "wcm_innerLink";          //վ�����ӹ���
                    public const string Restriction = "wcm_restriction";                  //ҳ���������
                    public const string Backup = "wcm_backup";                //���ݱ��ݻָ�
                    public const string Signin = "wcm_signin";            //����ǩ�չ���
                    public const string ContentArchive = "wcm_contentArchive";            //�鵵���ݹ���
                    public const string FileManagement = "wcm_fileManagement";            //Ӧ���ļ�����
                    public const string AllPublish = "wcm_allPublish";            //�����������
                    public const string BShare = "wcm_bShare";            //bShare������

                    public const string Template = "wcm_template";            //��ʾ����

                    public const string Configration = "wcm_configration";    //���ù���

                    public const string Create = "wcm_create";                //���ɹ���
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
                            retval = "������";
                        }
                        else if (permission == GovInteractAdd)
                        {
                            retval = "�������";
                        }
                        else if (permission == GovInteractEdit)
                        {
                            retval = "�༭���";
                        }
                        else if (permission == GovInteractDelete)
                        {
                            retval = "ɾ�����";
                        }
                        else if (permission == GovInteractSwitchToTranslate)
                        {
                            retval = "ת��ת��";
                        }
                        else if (permission == GovInteractComment)
                        {
                            retval = "��ʾ���";
                        }
                        else if (permission == GovInteractAccept)
                        {
                            retval = "������";
                        }
                        else if (permission == GovInteractReply)
                        {
                            retval = "������";
                        }
                        else if (permission == GovInteractCheck)
                        {
                            retval = "��˰��";
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
                        retval = "�û�����";
                    }
                    else if (menuID == ID_SiteConfiguration)
                    {
                        retval = "ϵͳ����";
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
                    public const string ID_GovPublicContent = "GovPublicContent";     //������Ϣ����
                    public const string ID_GovPublicApply = "GovPublicApply";         //�����빫��
                    public const string ID_GovPublicContentConfiguration = "GovPublicContentConfiguration";         //������Ϣ��������
                    public const string ID_GovPublicApplyConfiguration = "GovPublicApplyConfiguration";         //�����빫������
                    public const string ID_GovPublicAnalysis = "GovPublicAnalysis";         //����ͳ�Ʒ���
                }

                public class GovInteract
                {
                    public const string ID_GovInteractConfiguration = "GovInteractConfiguration";     //������������
                    public const string ID_GovInteractAnalysis = "GovInteractAnalysis";             //����ͳ�Ʒ���
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
                    public const string ID_TagStyle = "TagStyle";     //ģ���ǩ��ʽ
                }

                public class Configuration
                {
                    public const string ID_ConfigurationCreate = "ConfigurationCreate";         //ҳ����������
                    public const string ID_ConfigurationStorage = "ConfigurationStorage";       //�洢�ռ�����
                    public const string ID_ConfigurationTask = "ConfigurationTask";             //��ʱ�������
                    public const string ID_ConfigurationTouGao = "ConfigurationTouGao";         //����Ͷ������
                    public const string ID_ConfigurationComment = "ConfigurationComment";       //������������
                    public const string ID_ConfigurationMachine = "ConfigurationMachine";       //����������
                }

                public static string GetText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Content)
                    {
                        retval = "��Ϣ����";
                    }
                    else if (menuID == ID_GovPublic)
                    {
                        retval = "��Ϣ����";
                    }
                    else if (menuID == ID_GovInteract)
                    {
                        retval = "��������";
                    }
                    else if (menuID == ID_Function)
                    {
                        retval = "���ܹ���";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "��ʾ����";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "�û�����";
                    }
                    else if (menuID == ID_Configration)
                    {
                        retval = "���ù���";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "���ɹ���";
                    }

                    else if (menuID == ID_Auxiliary)
                    {
                        retval = "���������";
                    }
                    else if (menuID == ID_Site)
                    {
                        retval = "ϵͳӦ�ù���";
                    }
                    else if (menuID == ID_SiteSettings)
                    {
                        retval = "Ӧ��ͨ������";
                    }
                    else if (menuID == ID_Log)
                    {
                        retval = "��̨���м�¼";
                    }
                    else if (menuID == ID_UserLevel)
                    {
                        retval = "�û��ȼ�����";
                    }
                    return retval;
                }

                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    //Content
                    if (menuID == Content.ID_ContentModel)
                    {
                        retval = "����ģ��";
                    }
                    else if (menuID == Content.ID_Category)
                    {
                        retval = "�������";
                    }
                    else if (menuID == Content.ID_SiteAnalysis)
                    {
                        retval = "Ӧ������ͳ��";
                    }
                    //GovPublic
                    else if (menuID == GovPublic.ID_GovPublicContent)
                    {
                        retval = "������Ϣ����";
                    }
                    else if (menuID == GovPublic.ID_GovPublicApply)
                    {
                        retval = "�����빫��";
                    }
                    else if (menuID == GovPublic.ID_GovPublicContentConfiguration)
                    {
                        retval = "������Ϣ��������";
                    }
                    else if (menuID == GovPublic.ID_GovPublicApplyConfiguration)
                    {
                        retval = "�����빫������";
                    }
                    else if (menuID == GovPublic.ID_GovPublicAnalysis)
                    {
                        retval = "����ͳ�Ʒ���";
                    }
                    //GovInteract
                    else if (menuID == GovInteract.ID_GovInteractConfiguration)
                    {
                        retval = "������������";
                    }
                    else if (menuID == GovInteract.ID_GovInteractAnalysis)
                    {
                        retval = "����ͳ�Ʒ���";
                    }
                    //Function
                    else if (menuID == Function.ID_Input)
                    {
                        retval = "�ύ��";
                    }
                    else if (menuID == Function.ID_Gather)
                    {
                        retval = "��Ϣ�ɼ�����";
                    }
                    else if (menuID == Function.ID_Advertisement)
                    {
                        retval = "������";
                    }
                    else if (menuID == Function.ID_Resume)
                    {
                        retval = "��������";
                    }
                    else if (menuID == Function.ID_Mail)
                    {
                        retval = "�ʼ����͹���";
                    }
                    else if (menuID == Function.ID_SEO)
                    {
                        retval = "���������Ż�";
                    }
                    else if (menuID == Function.ID_Tracking)
                    {
                        retval = "����ͳ�ƹ���";
                    }
                    else if (menuID == Function.ID_InnerLink)
                    {
                        retval = "վ�����ӹ���";
                    }
                    else if (menuID == Function.ID_Restriction)
                    {
                        retval = "ҳ���������";
                    }
                    else if (menuID == Function.ID_Backup)
                    {
                        retval = "���ݱ��ݻָ�";
                    }
                    else if (menuID == Function.ID_BShare)
                    {
                        retval = "bShare������";
                    }
                    //Template
                    else if (menuID == Template.ID_TagStyle)
                    {
                        retval = "ģ���ǩ��ʽ";
                    }
                    //Configuration
                    else if (menuID == Configuration.ID_ConfigurationCreate)
                    {
                        retval = "ҳ����������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationStorage)
                    {
                        retval = "�洢�ռ�����";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTask)
                    {
                        retval = "��ʱ�������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationTouGao)
                    {
                        retval = "����Ͷ������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationComment)
                    {
                        retval = "������������";
                    }
                    else if (menuID == Configuration.ID_ConfigurationMachine)
                    {
                        retval = "����������";
                    }
                    return retval;
                }
            }

            public class Permission
            {
                //public class General
                //{
                //    private General() { }

                //    public const string Auxiliary = "cms_auxiliary";                        //���������
                //    public const string Site = "cms_site";                                  //ϵͳӦ�ù���
                //    public const string User = "cms_user";                                  //Ӧ���û�����
                //    public const string SiteSettings = "cms_siteSettings";                  //Ӧ��ͨ������
                //    public const string Log = "cms_log";                                    //��̨���м�¼
                //}

                public class WebSite
                {
                    private WebSite() { }

                    public const string ContentModel = "cms_contentModel";                  //����ģ��
                    public const string Category = "cms_category";                          //�������
                    public const string SiteAnalysis = "cms_siteAnalysis";                  //Ӧ������ͳ��
                    public const string ContentTrash = "cms_contentTrash";                  //���ݻ���վ

                    public const string GovPublicContent = "cms_govPublicContent";          //������Ϣ����
                    public const string GovPublicApply = "cms_govPublicApply";          //�����빫��
                    public const string GovPublicContentConfiguration = "cms_govPublicContentConfiguration";          //������Ϣ��������
                    public const string GovPublicApplyConfiguration = "cms_govPublicApplyConfiguration";          //�����빫������
                    public const string GovPublicAnalysis = "cms_govPublicAnalysis";          //��Ϣ����ͳ��

                    public const string GovInteract = "cms_govInteract";                     //������������
                    public const string GovInteractConfiguration = "cms_govInteractConfiguration";                     //������������
                    public const string GovInteractAnalysis = "cms_govInteractAnalysis";                     //��������ͳ��

                    public const string InputContentView = "cms_inputContentView";    //�ύ���鿴
                    public const string InputContentEdit = "cms_inputContentEdit";    //�ύ��ά��
                    public const string Input = "cms_input";                  //�ύ������
                    public const string Gather = "cms_gather";                //��Ϣ�ɼ�����
                    public const string Advertisement = "cms_advertisement";  //������
                    public const string Resume = "cms_resume";                 //������Ƹ����
                    public const string Mail = "cms_mail";                    //�ʼ����͹���
                    public const string SEO = "cms_seo";                      //���������Ż�
                    public const string Tracking = "cms_tracking";            //����ͳ�ƹ���
                    public const string InnerLink = "cms_innerLink";          //վ�����ӹ���
                    public const string Restriction = "cms_restriction";                  //ҳ���������
                    public const string Backup = "cms_backup";                //���ݱ��ݻָ�
                    public const string Archive = "cms_archive";            //�鵵���ݹ���
                    public const string FileManagement = "cms_fileManagement";            //Ӧ���ļ�����
                    public const string AllPublish = "cms_allPublish";            //�����������
                    public const string BShare = "cms_bShare";            //bShare������

                    public const string Template = "cms_template";            //��ʾ����

                    public const string User = "cms_user";            //�û�����

                    public const string Configration = "cms_configration";    //���ù���

                    public const string Create = "cms_create";                //���ɹ���
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
                            retval = "������";
                        }
                        else if (permission == GovInteractAdd)
                        {
                            retval = "�������";
                        }
                        else if (permission == GovInteractEdit)
                        {
                            retval = "�༭���";
                        }
                        else if (permission == GovInteractDelete)
                        {
                            retval = "ɾ�����";
                        }
                        else if (permission == GovInteractSwitchToTranslate)
                        {
                            retval = "ת��ת��";
                        }
                        else if (permission == GovInteractComment)
                        {
                            retval = "��ʾ���";
                        }
                        else if (permission == GovInteractAccept)
                        {
                            retval = "������";
                        }
                        else if (permission == GovInteractReply)
                        {
                            retval = "������";
                        }
                        else if (permission == GovInteractCheck)
                        {
                            retval = "��˰��";
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
                        retval = "��Ϣ����";
                    }
                    else if (menuID == ID_GovPublic)
                    {
                        retval = "��Ϣ����";
                    }
                    else if (menuID == ID_GovInteract)
                    {
                        retval = "��������";
                    }
                    else if (menuID == ID_Function)
                    {
                        retval = "���ܹ���";
                    }
                    else if (menuID == ID_Template)
                    {
                        retval = "��ʾ����";
                    }
                    else if (menuID == ID_User)
                    {
                        retval = "�û�����";
                    }
                    else if (menuID == ID_Configration)
                    {
                        retval = "���ù���";
                    }
                    else if (menuID == ID_Create)
                    {
                        retval = "���ɹ���";
                    }

                    else if (menuID == ID_Auxiliary)
                    {
                        retval = "���������";
                    }
                    else if (menuID == ID_Site)
                    {
                        retval = "ϵͳӦ�ù���";
                    }
                    else if (menuID == ID_SiteSettings)
                    {
                        retval = "Ӧ��ͨ������";
                    }
                    else if (menuID == ID_Log)
                    {
                        retval = "��̨���м�¼";
                    }
                    else if (menuID == ID_UserLevel)
                    {
                        retval = "�û��ȼ�����";
                    }
                    return retval;
                }

            }

            public class Permission
            {
                //public class General
                //{
                //    private General() { }

                //    public const string Auxiliary = "cms_auxiliary";                        //���������
                //    public const string Site = "cms_site";                                  //ϵͳӦ�ù���
                //    public const string User = "cms_user";                                  //Ӧ���û�����
                //    public const string SiteSettings = "cms_siteSettings";                  //Ӧ��ͨ������
                //    public const string Log = "cms_log";                                    //��̨���м�¼
                //}

                public class WebSite
                {
                    private WebSite() { }

                    public const string ContentModel = "cms_contentModel";                  //����ģ��
                    public const string Category = "cms_category";                          //�������
                    public const string SiteAnalysis = "cms_siteAnalysis";                  //Ӧ������ͳ��
                    public const string ContentTrash = "cms_contentTrash";                  //���ݻ���վ

                    public const string GovPublicContent = "cms_govPublicContent";          //������Ϣ����
                    public const string GovPublicApply = "cms_govPublicApply";          //�����빫��
                    public const string GovPublicContentConfiguration = "cms_govPublicContentConfiguration";          //������Ϣ��������
                    public const string GovPublicApplyConfiguration = "cms_govPublicApplyConfiguration";          //�����빫������
                    public const string GovPublicAnalysis = "cms_govPublicAnalysis";          //��Ϣ����ͳ��

                    public const string GovInteract = "cms_govInteract";                     //������������
                    public const string GovInteractConfiguration = "cms_govInteractConfiguration";                     //������������
                    public const string GovInteractAnalysis = "cms_govInteractAnalysis";                     //��������ͳ��

                    public const string InputContentView = "cms_inputContentView";    //�ύ���鿴
                    public const string InputContentEdit = "cms_inputContentEdit";    //�ύ��ά��
                    public const string Input = "cms_input";                  //�ύ������
                    public const string Gather = "cms_gather";                //��Ϣ�ɼ�����
                    public const string Advertisement = "cms_advertisement";  //������
                    public const string Resume = "cms_resume";                 //������Ƹ����
                    public const string Mail = "cms_mail";                    //�ʼ����͹���
                    public const string SEO = "cms_seo";                      //���������Ż�
                    public const string Tracking = "cms_tracking";            //����ͳ�ƹ���
                    public const string InnerLink = "cms_innerLink";          //վ�����ӹ���
                    public const string Restriction = "cms_restriction";                  //ҳ���������
                    public const string Backup = "cms_backup";                //���ݱ��ݻָ�
                    public const string Archive = "cms_archive";            //�鵵���ݹ���
                    public const string FileManagement = "cms_fileManagement";            //Ӧ���ļ�����
                    public const string AllPublish = "cms_allPublish";            //�����������
                    public const string BShare = "cms_bShare";            //bShare������

                    public const string Template = "cms_template";            //��ʾ����

                    public const string User = "cms_user";            //�û�����

                    public const string Configration = "cms_configration";    //���ù���

                    public const string Create = "cms_create";                //���ɹ���
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
                            retval = "������";
                        }
                        else if (permission == GovInteractAdd)
                        {
                            retval = "�������";
                        }
                        else if (permission == GovInteractEdit)
                        {
                            retval = "�༭���";
                        }
                        else if (permission == GovInteractDelete)
                        {
                            retval = "ɾ�����";
                        }
                        else if (permission == GovInteractSwitchToTranslate)
                        {
                            retval = "ת��ת��";
                        }
                        else if (permission == GovInteractComment)
                        {
                            retval = "��ʾ���";
                        }
                        else if (permission == GovInteractAccept)
                        {
                            retval = "������";
                        }
                        else if (permission == GovInteractReply)
                        {
                            retval = "������";
                        }
                        else if (permission == GovInteractCheck)
                        {
                            retval = "��˰��";
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

                public const string ID_User = "User";                         //�û�����
                public const string ID_UserMessage = "UserMessage";           //��Ϣ����
                public const string ID_UserBasicSetting = "UserBasicSetting"; //�û���������
                public const string ID_UserLevelCredit = "UserLevelCredit";   //�û��ȼ�����
                public const string ID_UserCenterSetting = "UserCenterSetting";//�û���������

                public const string ID_UserGroup = "UserGroup";//�û������
                #region Ͷ����� by 20160108 sofuny
                public const string ID_MLibManage = "MLibManage";//Ͷ�����
                public const string ID_Sub_MLibManageSetting = "MLibManageSetting";//Ͷ������
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
                        retval = "�û�����";
                    }
                    else if (menuID == ID_UserMessage)
                    {
                        retval = "��Ϣ����";
                    }
                    else if (menuID == ID_UserBasicSetting)
                    {
                        retval = "�û���������";
                    }
                    else if (menuID == ID_UserLevelCredit)
                    {
                        retval = "�û��ȼ�����";
                    }
                    else if (menuID == ID_UserCenterSetting)
                    {
                        retval = "�û���������";
                    }
                    else if (menuID == ID_UserGroup)
                    {
                        retval = "�û������";
                    }
                    else if (menuID == ID_MLibManage)
                    {
                        retval = "Ͷ�����";
                    }

                    return retval;
                }
                public static string GetSubText(string menuID)
                {
                    string retval = string.Empty;
                    if (menuID == ID_Sub_UserLevel)
                    {
                        retval = "�û��ȼ�����";
                    }
                    else if (menuID == ID_Sub_LevelPermission)
                    {
                        retval = "�û��ȼ�Ȩ��";
                    }
                    else if (menuID == ID_Sub_UserTableStyle)
                    {
                        retval = "�û��ֶ�����";
                    }
                    else if (menuID == ID_Sub_UserConfigRegister)
                    {
                        retval = "�û�ע������";
                    }
                    else if (menuID == ID_Sub_UserConfigLogin)
                    {
                        retval = "�û���¼����";
                    }
                    else if (menuID == ID_Sub_UserConfigForget)
                    {
                        retval = "������������";
                    }
                    else if (menuID == ID_Sub_ThirdLogin)
                    {
                        retval = "��Ȩ��¼����";
                    }
                    else if (menuID == ID_Sub_NoticeSetting)
                    {
                        retval = "��Ϣ����";
                    }
                    else if (menuID == ID_Sub_UserConfigMessage)
                    {
                        retval = "ϵͳ֪ͨ";
                    }
                    else if (menuID == ID_Sub_LevelRule)
                    {
                        retval = "�û��ȼ�����";
                    }
                    else if (menuID == ID_Sub_MLibManageSetting)
                    {
                        retval = "Ͷ������";
                    }

                    return retval;
                }
            }

            public class Permission
            {
                /******************�û�����********************/
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

                public const string Usercenter_User = "usercenter_user";//�û�����

                public const string Usercenter_Content = "usercenter_content";//��Ϣ����

                public const string Usercenter_Template = "usercenter_template";//��ʾ����
                public const string Usercenter_Create = "usercenter_create";//���ɹ���

                public const string Usercenter_Msg = "usercenter_msg";//��Ϣ����

                public const string Usercenter_Setting = "usercenter_setting";//���ù���


                public const string Usercenter_UserGroup = "usercenter_userGroup";//�û������
                #region Ͷ����� by 20160108 sofuny
                public const string Usercenter_MLibManage = "usercenter_mLibManage";//Ͷ�����
                public const string Usercenter_MLibManageSetting = "usercenter_mLibManageSetting";//Ͷ������
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
                if (isFullName) retval += " ���ݹ���ϵͳ";
            }
            else if (StringUtils.EqualsIgnoreCase(appID, AppManager.WCM.AppID))
            {
                retval = "SiteServer WCM";
                if (isFullName) retval += " ����Э��ƽ̨";
            }
            else if (StringUtils.EqualsIgnoreCase(appID, AppManager.BBS.AppID))
            {
                retval = "SiteServer BBS";
                if (isFullName) retval += " ��̳ϵͳ";
            }
            else if (StringUtils.EqualsIgnoreCase(appID, AppManager.B2C.AppID))
            {
                retval = "SiteServer B2C";
                if (isFullName) retval += " ��������ϵͳ";
            }
            else if (StringUtils.EqualsIgnoreCase(appID, AppManager.CRM.AppID))
            {
                retval = "SiteServer CRM";
                if (isFullName) retval += " �ͻ���ϵ����ϵͳ";
            }
            else if (StringUtils.EqualsIgnoreCase(appID, AppManager.WeiXin.AppID))
            {
                retval = "SiteServer WeiXin";
                if (isFullName) retval += " ΢��������ϵͳ";
            }
            else if (StringUtils.EqualsIgnoreCase(appID, AppManager.UserCenter.AppID))
            {
                retval = "SiteServer UserCenter";
                if (isFullName) retval += " �û�����ϵͳ";
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

                //�������ݿ�
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

            //���°汾���ݿ�汾
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

                    //����������
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
