using System.Reflection;
using System.Configuration;

namespace SiteServer.BBS
{
    public class DataProvider
    {
        public static readonly string assemblyString = "SiteServer.BBS";
        public const string namespaceString = "SiteServer.BBS.Provider.SqlServer";

        private static string connectionString;

        static DataProvider()
        {
            connectionString = ConfigurationSettings.AppSettings.Get("ConnectionString");
        }

        public static string ConnectionString
        {
            set { connectionString = value; }
            get { return connectionString; }
        }

        private static IAnnouncementDAO announcementDAO;
        public static IAnnouncementDAO AnnouncementDAO
        {
            get
            {
                if (announcementDAO == null)
                {
                    string className = namespaceString + ".AnnouncementDAO";
                    announcementDAO = (IAnnouncementDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return announcementDAO;
            }
        }

        private static IConfigurationDAO configurationDAO;
        public static IConfigurationDAO ConfigurationDAO
        {
            get
            {
                if (configurationDAO == null)
                {
                    string className = namespaceString + ".ConfigurationDAO";
                    configurationDAO = (IConfigurationDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return configurationDAO;
            }
        }

        private static IAdDAO adDAO;
        public static IAdDAO AdDAO
        {
            get
            {
                if (adDAO == null)
                {
                    string className = namespaceString + ".AdDAO";
                    adDAO = (IAdDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return adDAO;
            }
        }

        private static ILinkDAO linkDAO;
        public static ILinkDAO LinkDAO
        {
            get
            {
                if (linkDAO == null)
                {
                    string className = namespaceString + ".LinkDAO";
                    linkDAO = (ILinkDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return linkDAO;
            }
        }

        private static IForumDAO forumDAO;
        public static IForumDAO ForumDAO
        {
            get
            {
                if (forumDAO == null)
                {
                    string className = namespaceString + ".ForumDAO";
                    forumDAO = (IForumDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return forumDAO;
            }
        }

        private static IBBSLogDAO bbsLogDAO;
        public static IBBSLogDAO BBSLogDAO
        {
            get
            {
                if (bbsLogDAO == null)
                {
                    string className = namespaceString + ".BBSLogDAO";
                    bbsLogDAO = (IBBSLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return bbsLogDAO;
            }
        }
        private static IThreadCategoryDAO threadCategoryDAO;
        public static IThreadCategoryDAO ThreadCategoryDAO {
            get {
                if(threadCategoryDAO == null) {
                    string className = namespaceString + ".ThreadCategoryDAO";
                    threadCategoryDAO = (IThreadCategoryDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return threadCategoryDAO;
            }
        } 
        private static IThreadDAO threadDAO;
        public static IThreadDAO ThreadDAO {
            get {
                if(threadDAO == null) {
                    string className = namespaceString + ".ThreadDAO";
                    threadDAO = (IThreadDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return threadDAO;
            }
        }
        private static IPostDAO postDAO;
        public static IPostDAO PostDAO {
            get {
                if(postDAO == null) {
                    string className = namespaceString + ".PostDAO";
                    postDAO = (IPostDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return postDAO;
            }
        }

        private static IFaceDAO faceDAO;
        public static IFaceDAO FaceDAO
        {
            get
            {
                if (faceDAO == null)
                {
                    string className = namespaceString + ".FaceDAO";
                    faceDAO = (IFaceDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return faceDAO;
            }
        }

        private static ICreditRuleDAO creditRuleDAO;
        public static ICreditRuleDAO CreditRuleDAO
        {
            get
            {
                if (creditRuleDAO == null)
                {
                    string className = namespaceString + ".CreditRuleDAO";
                    creditRuleDAO = (ICreditRuleDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return creditRuleDAO;
            }
        }

        private static ICreditRuleLogDAO creditRuleLogDAO;
        public static ICreditRuleLogDAO CreditRuleLogDAO
        {
            get
            {
                if (creditRuleLogDAO == null)
                {
                    string className = namespaceString + ".CreditRuleLogDAO";
                    creditRuleLogDAO = (ICreditRuleLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return creditRuleLogDAO;
            }
        }

        private static IPollDAO pollDAO;
        public static IPollDAO PollDAO
        {
            get
            {
                if (pollDAO == null)
                {
                    string className = namespaceString + ".PollDAO";
                    pollDAO = (IPollDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return pollDAO;
            }
        }

        private static IPermissionsDAO permissionsDAO;
        public static IPermissionsDAO PermissionsDAO
        {
            get
            {
                if (permissionsDAO == null)
                {
                    string className = namespaceString + ".PermissionsDAO";
                    permissionsDAO = (IPermissionsDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return permissionsDAO;
            }
        }

        private static IAttachmentDAO attachmentDAO;
        public static IAttachmentDAO AttachmentDAO
        {
            get
            {
                if (attachmentDAO == null)
                {
                    string className = namespaceString + ".AttachmentDAO";
                    attachmentDAO = (IAttachmentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return attachmentDAO;
            }
        }

        private static IAttachmentTypeDAO attachmentTypeDAO;
        public static IAttachmentTypeDAO AttachmentTypeDAO
        {
            get
            {
                if (attachmentTypeDAO == null)
                {
                    string className = namespaceString + ".AttachmentTypeDAO";
                    attachmentTypeDAO = (IAttachmentTypeDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return attachmentTypeDAO;
            }
        }

        private static INavigationDAO navigationDAO;
        public static INavigationDAO NavigationDAO
        {
            get
            {
                if (navigationDAO == null)
                {
                    string className = namespaceString + ".NavigationDAO";
                    navigationDAO = (INavigationDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return navigationDAO;
            }
        }

        private static IReportDAO reportDAO;
        public static IReportDAO ReportDAO
        {
            get {
                if (reportDAO == null)
                {
                    string className = namespaceString + ".ReportDAO";
                    reportDAO = (IReportDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return reportDAO;
            }
        }

        private static IIdentifyDAO identifyDAO;
        public static IIdentifyDAO IdentifyDAO
        {
            get
            {
                if (identifyDAO == null)
                {
                    string className = namespaceString + ".IdentifyDAO";
                    identifyDAO = (IIdentifyDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return identifyDAO;
            }
        }

        private static IKeywordsCategoryDAO keywordsCategoryDAO;
        public static IKeywordsCategoryDAO KeywordsCategoryDAO
        {
            get
            {
                if (keywordsCategoryDAO == null)
                {
                    string className = namespaceString + ".KeywordsCategoryDAO";
                    keywordsCategoryDAO = (IKeywordsCategoryDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return keywordsCategoryDAO;
            }
        }

        private static IKeywordsFilterDAO keywordsFilterDAO;
        public static IKeywordsFilterDAO KeywordsFilterDAO
        {
            get 
            {
                if (keywordsFilterDAO == null)
                {
                    string className = namespaceString + ".KeywordsFilterDAO";
                    keywordsFilterDAO = (IKeywordsFilterDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return keywordsFilterDAO;
            }
        }

        private static IOnlineDAO onlineDAO;
        public static IOnlineDAO OnlineDAO
        {
            get
            {
                if (onlineDAO == null)
                {
                    string className = namespaceString + ".OnlineDAO";
                    onlineDAO = (IOnlineDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return onlineDAO;
            }
        }

        private static IBBSUserDAO bbsUserDAO;
        public static IBBSUserDAO BBSUserDAO
        {
            get
            {
                if (bbsUserDAO == null)
                {
                    string className = namespaceString + ".BBSUserDAO";
                    bbsUserDAO = (IBBSUserDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return bbsUserDAO;
            }
        }
    }
}