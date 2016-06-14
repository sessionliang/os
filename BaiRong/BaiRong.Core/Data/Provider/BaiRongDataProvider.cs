using System;
using System.Configuration;
using System.Reflection;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Configuration;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;

namespace BaiRong.Core
{
    public class BaiRongDataProvider
    {
        private static EDatabaseType databaseType;
        private static string adoType;
        private static string namespaceString;
        private const string assemblyString = "BaiRong.Provider";
        private static string databaseOwner;
        private static string connectionString;

        public const string NAME_DATABASE_TYPE = "DatabaseType";
        public const string NAME_CONNECTION_STRING = "ConnectionString";
        public const string NAME_DATABASE_OWNER = "DatabaseOwner";

        private const string NS_SQLSERVER = "BaiRong.Provider.Data.SqlServer";
        private const string NS_ORACLE = "BaiRong.Provider.Data.Oracle";

        private static ITableCollectionDAO tableCollectionDAO;
        private static ITableMetadataDAO tableMetadataDAO;
        private static IAuxiliaryTableDataDAO auxiliaryTableDataDAO;
        private static IDatabaseDAO databaseDAO;
        private static ITableStructureDAO tableStructureDAO;
        private static ICountDAO countDAO;
        private static IDbCacheDAO dbCacheDAO;
        private static ITableMatchDAO tableMatchDAO;
        private static ITableStyleDAO tableStyleDAO;
        private static IIP2CityDAO iP2CityDAO;
        private static IDiggDAO diggDAO;
        private static ILogDAO logDAO;
        private static IErrorLogDAO errorLogDAO;
        private static IConfigDAO configDAO;
        private static IContentModelDAO contentModelDAO;
        private static IContentDAO contentDAO;
        private static ITagDAO tagDAO;
        private static ISMSMessageDAO smsMessageDAO;
        private static IContentCheckDAO contentCheckDAO;
        private static IDepartmentDAO departmentDAO;
        private static IAreaDAO areaDAO;
        private static ITaskDAO taskDAO;
        private static ITaskLogDAO taskLogDAO;
        private static IStorageDAO storageDAO;
        private static IFTPStorageDAO ftpStorageDAO;
        private static ILocalStorageDAO localStorageDAO;
        private static IAjaxUrlDAO ajaxUrlDAO;

        private static IRoleDAO roleDAO;
        private static IPermissionsInRolesDAO permissionsInRolesDAO;
        private static IUserConfigDAO userConfigDAO;
        private static IUserMessageDAO userMessageDAO;
        private static IUserCreditsLogDAO userCreditsLogDAO;
        private static IAdministratorDAO administratorDAO;
        private static IUserDAO userDAO;
        private static IUserBindingDAO userBindingDAO;
        private static IUserContactDAO userContactDAO;
        private static IUserDownloadDAO userDownloadDAO;
        private static IUserGroupDAO userGroupDAO;

        private static IBaiRongThirdLoginDAO bairongThirdLoginDAO;

        private static IUserLevelDAO userLevelDAO;
        public static ILevelRuleDAO levelRuleDAO;
        private static IUserNoticeTemplateDAO userNoticeTemplateDAO;
        private static IUserSecurityQuestionDAO userSecurityQuestionDAO;
        private static IUserLogDAO userLogDAO;
        private static ISMSServerDAO smsServerDAO;

        /// <summary>
        /// 生成页面任务（用于服务组件生成页面）
        /// </summary>
        private static ICreateTaskDAO createTaskDAO;

        static BaiRongDataProvider()
        {
            databaseType = EDatabaseTypeUtils.GetEnumType(ConfigUtils.Instance.GetAppSettings(NAME_DATABASE_TYPE));
            connectionString = ConfigUtils.Instance.GetAppSettings(NAME_CONNECTION_STRING);
            databaseOwner = ConfigUtils.Instance.GetAppSettings(NAME_DATABASE_OWNER);

            if (databaseType == EDatabaseType.SqlServer)
            {
                adoType = SqlUtils.SQL_SERVER;
                namespaceString = NS_SQLSERVER;
            }
            else if (databaseType == EDatabaseType.Oracle)
            {
                adoType = SqlUtils.ORACLE;
                namespaceString = NS_ORACLE;
            }
        }

        public static void InitializeManual(EDatabaseType databaseType, string connectionString)
        {
            BaiRongDataProvider.databaseType = databaseType;
            BaiRongDataProvider.connectionString = connectionString;

            if (BaiRongDataProvider.databaseType == EDatabaseType.SqlServer)
            {
                BaiRongDataProvider.adoType = SqlUtils.SQL_SERVER;
                BaiRongDataProvider.namespaceString = NS_SQLSERVER;
            }
            else if (BaiRongDataProvider.databaseType == EDatabaseType.Oracle)
            {
                BaiRongDataProvider.adoType = SqlUtils.ORACLE;
                BaiRongDataProvider.namespaceString = NS_ORACLE;
            }
        }

        public static EDatabaseType DatabaseType
        {
            get { return databaseType; }
        }

        public static void SetDatabaseType(EDatabaseType databaseType)
        {
            BaiRongDataProvider.databaseType = databaseType;
            if (BaiRongDataProvider.databaseType == EDatabaseType.SqlServer)
            {
                BaiRongDataProvider.adoType = SqlUtils.SQL_SERVER;
                BaiRongDataProvider.namespaceString = NS_SQLSERVER;
            }
            else if (BaiRongDataProvider.databaseType == EDatabaseType.Oracle)
            {
                BaiRongDataProvider.adoType = SqlUtils.ORACLE;
                BaiRongDataProvider.namespaceString = NS_ORACLE;
            }

            tableCollectionDAO = null;
            tableMetadataDAO = null;
            auxiliaryTableDataDAO = null;
            databaseDAO = null;
            tableStructureDAO = null;
            countDAO = null;
            dbCacheDAO = null;
            tableMatchDAO = null;
            tableStyleDAO = null;
            iP2CityDAO = null;
            diggDAO = null;
            logDAO = null;
            configDAO = null;
            contentModelDAO = null;
            contentDAO = null;
            tagDAO = null;
            smsMessageDAO = null;
            contentCheckDAO = null;
            departmentDAO = null;
            areaDAO = null;
            taskDAO = null;
            taskLogDAO = null;
            storageDAO = null;
            ftpStorageDAO = null;
            localStorageDAO = null;
            ajaxUrlDAO = null;

            roleDAO = null;
            permissionsInRolesDAO = null;
            userConfigDAO = null;
            userMessageDAO = null;
            userCreditsLogDAO = null;
            administratorDAO = null;
            userDAO = null;
            userBindingDAO = null;
            userContactDAO = null;
            userDownloadDAO = null;
            userGroupDAO = null;

            bairongThirdLoginDAO = null;
            userLevelDAO = null;
            levelRuleDAO=null;
            userNoticeTemplateDAO = null;
            userSecurityQuestionDAO = null;
            userLogDAO = null;
            smsServerDAO = null;

        }

        public static string ADOType
        {
            get { return adoType; }
        }

        public static string DatabaseOwner
        {
            get { return databaseOwner; }
        }

        public static string ConnectionString
        {
            set
            {
                connectionString = value;
                configDAO = null;
            }
            get { return connectionString; }
        }
        
        public static ITableCollectionDAO TableCollectionDAO
        {
            get
            {
                if (tableCollectionDAO == null)
                {
                    string className = namespaceString + ".TableCollectionDAO";
                    tableCollectionDAO = (ITableCollectionDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return tableCollectionDAO;
            }
        }

        public static ITableMetadataDAO TableMetadataDAO
        {
            get
            {
                if (tableMetadataDAO == null)
                {
                    string className = namespaceString + ".TableMetadataDAO";
                    tableMetadataDAO = (ITableMetadataDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return tableMetadataDAO;
            }
        }

        public static IAuxiliaryTableDataDAO AuxiliaryTableDataDAO
        {
            get
            {
                if (auxiliaryTableDataDAO == null)
                {
                    string className = namespaceString + ".AuxiliaryTableDataDAO";
                    auxiliaryTableDataDAO = (IAuxiliaryTableDataDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return auxiliaryTableDataDAO;
            }
        }

        public static IDatabaseDAO DatabaseDAO
        {
            get
            {
                if (databaseDAO == null)
                {
                    string className = namespaceString + ".DatabaseDAO";
                    databaseDAO = (IDatabaseDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return databaseDAO;
            }
        }

        public static IDatabaseDAO CreateDatabaseDAO(EDatabaseType databaseType)
        {
            string theNameSpaceString = string.Empty;
            if (databaseType == EDatabaseType.SqlServer)
            {
                theNameSpaceString = NS_SQLSERVER;
            }
            else if (databaseType == EDatabaseType.Oracle)
            {
                theNameSpaceString = NS_ORACLE;
            }
            string className = theNameSpaceString + ".DatabaseDAO";
            IDatabaseDAO dao = (IDatabaseDAO)CacheUtils.Get(className);
            if (dao == null)
            {
                dao = (IDatabaseDAO)Assembly.Load(assemblyString).CreateInstance(className);
                CacheUtils.Max(className, dao);
            }
            return dao;
        }

        public static ITableStructureDAO TableStructureDAO
        {
            get
            {
                if (tableStructureDAO == null)
                {
                    string className = namespaceString + ".TableStructureDAO";
                    tableStructureDAO = (ITableStructureDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return tableStructureDAO;
            }
        }

        public static ITableStructureDAO CreateTableStructureDAO(EDatabaseType databaseType)
        {
            string theNameSpaceString = string.Empty;
            if (databaseType == EDatabaseType.SqlServer)
            {
                theNameSpaceString = NS_SQLSERVER;
            }
            else if (databaseType == EDatabaseType.Oracle)
            {
                theNameSpaceString = NS_ORACLE;
            }
            string className = theNameSpaceString + ".TableStructureDAO";
            ITableStructureDAO dao = (ITableStructureDAO)CacheUtils.Get(className);
            if (dao == null)
            {
                dao = (ITableStructureDAO)Assembly.Load(assemblyString).CreateInstance(className);
                CacheUtils.Max(className, dao);
            }
            return dao;
        }

        internal static ICountDAO CountDAO
        {
            get
            {
                if (countDAO == null)
                {
                    string className = namespaceString + ".CountDAO";
                    countDAO = (ICountDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return countDAO;
            }
        }

        internal static IDbCacheDAO DbCacheDAO
        {
            get
            {
                if (dbCacheDAO == null)
                {
                    string className = namespaceString + ".DbCacheDAO";
                    dbCacheDAO = (IDbCacheDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return dbCacheDAO;
            }
        }

        public static ITableMatchDAO TableMatchDAO
        {
            get
            {
                if (tableMatchDAO == null)
                {
                    string className = namespaceString + ".TableMatchDAO";
                    tableMatchDAO = (ITableMatchDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return tableMatchDAO;
            }
        }

        public static ITableStyleDAO TableStyleDAO
        {
            get
            {
                if (tableStyleDAO == null)
                {
                    string className = namespaceString + ".TableStyleDAO";
                    tableStyleDAO = (ITableStyleDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return tableStyleDAO;
            }
        }

        public static IIP2CityDAO IP2CityDAO
        {
            get
            {
                if (iP2CityDAO == null)
                {
                    string className = namespaceString + ".IP2CityDAO";
                    iP2CityDAO = (IIP2CityDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return iP2CityDAO;
            }
        }

        public static IDiggDAO DiggDAO
        {
            get
            {
                if (diggDAO == null)
                {
                    string className = namespaceString + ".DiggDAO";
                    diggDAO = (IDiggDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return diggDAO;
            }
        }

        public static ILogDAO LogDAO
        {
            get
            {
                if (logDAO == null)
                {
                    string className = namespaceString + ".LogDAO";
                    logDAO = (ILogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return logDAO;
            }
        }

        public static IErrorLogDAO ErrorLogDAO
        {
            get
            {
                if (errorLogDAO == null)
                {
                    string className = namespaceString + ".ErrorLogDAO";
                    errorLogDAO = (IErrorLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return errorLogDAO;
            }
        }



        public static IConfigDAO ConfigDAO
        {
            get
            {
                if (configDAO == null)
                {
                    string className = namespaceString + ".ConfigDAO";
                    configDAO = (IConfigDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return configDAO;
            }
        }

        public static IContentModelDAO ContentModelDAO
        {
            get
            {
                if (contentModelDAO == null)
                {
                    string className = namespaceString + ".ContentModelDAO";
                    contentModelDAO = (IContentModelDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return contentModelDAO;
            }
        }

        public static IContentDAO ContentDAO
        {
            get
            {
                if (contentDAO == null)
                {
                    string className = namespaceString + ".ContentDAO";
                    contentDAO = (IContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return contentDAO;
            }
        }

        public static ITagDAO TagDAO
        {
            get
            {
                if (tagDAO == null)
                {
                    string className = namespaceString + ".TagDAO";
                    tagDAO = (ITagDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return tagDAO;
            }
        }

        //public static ISSOAppDAO SSOAppDAO
        //{
        //    get
        //    {
        //        if (ssoAppDAO == null)
        //        {
        //            string className = namespaceString + ".SSOAppDAO";
        //            ssoAppDAO = (ISSOAppDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return ssoAppDAO;
        //    }
        //}

        public static ISMSMessageDAO SMSMessageDAO
        {
            get
            {
                if (smsMessageDAO == null)
                {
                    string className = namespaceString + ".SMSMessageDAO";
                    smsMessageDAO = (ISMSMessageDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return smsMessageDAO;
            }
        }

        public static IContentCheckDAO ContentCheckDAO
        {
            get
            {
                if (contentCheckDAO == null)
                {
                    string className = namespaceString + ".ContentCheckDAO";
                    contentCheckDAO = (IContentCheckDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return contentCheckDAO;
            }
        }

        public static IDepartmentDAO DepartmentDAO
        {
            get
            {
                if (departmentDAO == null)
                {
                    string className = namespaceString + ".DepartmentDAO";
                    departmentDAO = (IDepartmentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return departmentDAO;
            }
        }

        public static IAreaDAO AreaDAO
        {
            get
            {
                if (areaDAO == null)
                {
                    string className = namespaceString + ".AreaDAO";
                    areaDAO = (IAreaDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return areaDAO;
            }
        }

        public static ITaskDAO TaskDAO
        {
            get
            {
                if (taskDAO == null)
                {
                    string className = namespaceString + ".TaskDAO";
                    taskDAO = (ITaskDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return taskDAO;
            }
        }

        public static ITaskLogDAO TaskLogDAO
        {
            get
            {
                if (taskLogDAO == null)
                {
                    string className = namespaceString + ".TaskLogDAO";
                    taskLogDAO = (ITaskLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return taskLogDAO;
            }
        }

        public static IStorageDAO StorageDAO
        {
            get
            {
                if (storageDAO == null)
                {
                    string className = namespaceString + ".StorageDAO";
                    storageDAO = (IStorageDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return storageDAO;
            }
        }

        public static IFTPStorageDAO FTPStorageDAO
        {
            get
            {
                if (ftpStorageDAO == null)
                {
                    string className = namespaceString + ".FTPStorageDAO";
                    ftpStorageDAO = (IFTPStorageDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return ftpStorageDAO;
            }
        }

        public static ILocalStorageDAO LocalStorageDAO
        {
            get
            {
                if (localStorageDAO == null)
                {
                    string className = namespaceString + ".LocalStorageDAO";
                    localStorageDAO = (ILocalStorageDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return localStorageDAO;
            }
        }

        public static IAjaxUrlDAO AjaxUrlDAO
        {
            get
            {
                if (ajaxUrlDAO == null)
                {
                    string className = namespaceString + ".AjaxUrlDAO";
                    ajaxUrlDAO = (IAjaxUrlDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return ajaxUrlDAO;
            }
        }

        public static IRoleDAO RoleDAO
        {
            get
            {
                if (roleDAO == null)
                {
                    string className = namespaceString + ".RoleDAO";
                    roleDAO = (IRoleDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return roleDAO;
            }
        }

        public static IPermissionsInRolesDAO PermissionsInRolesDAO
        {
            get
            {
                if (permissionsInRolesDAO == null)
                {
                    string className = namespaceString + ".PermissionsInRolesDAO";
                    permissionsInRolesDAO = (IPermissionsInRolesDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return permissionsInRolesDAO;
            }
        }

        public static IUserConfigDAO UserConfigDAO
        {
            get
            {
                if (userConfigDAO == null)
                {
                    string className = namespaceString + ".UserConfigDAO";
                    userConfigDAO = (IUserConfigDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return userConfigDAO;
            }
        }

        public static IUserMessageDAO UserMessageDAO
        {
            get
            {
                if (userMessageDAO == null)
                {
                    string className = namespaceString + ".UserMessageDAO";
                    userMessageDAO = (IUserMessageDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return userMessageDAO;
            }
        }

        public static IUserCreditsLogDAO UserCreditsLogDAO
        {
            get
            {
                if (userCreditsLogDAO == null)
                {
                    string className = namespaceString + ".UserCreditsLogDAO";
                    userCreditsLogDAO = (IUserCreditsLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return userCreditsLogDAO;
            }
        }

        public static IAdministratorDAO AdministratorDAO
        {
            get
            {
                if (administratorDAO == null)
                {
                    string className = namespaceString + ".AdministratorDAO";
                    administratorDAO = (IAdministratorDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return administratorDAO;
            }
        }

        public static IUserDAO UserDAO
        {
            get
            {
                if (userDAO == null)
                {
                    string className = namespaceString + ".UserDAO";
                    userDAO = (IUserDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return userDAO;
            }
        }

        public static IUserBindingDAO UserBindingDAO
        {
            get
            {
                if (userBindingDAO == null)
                {
                    string className = namespaceString + ".UserBindingDAO";
                    userBindingDAO = (IUserBindingDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return userBindingDAO;
            }
        }

        public static IUserContactDAO UserContactDAO
        {
            get
            {
                if (userContactDAO == null)
                {
                    string className = namespaceString + ".UserContactDAO";
                    userContactDAO = (IUserContactDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return userContactDAO;
            }
        }

        public static IUserDownloadDAO UserDownloadDAO
        {
            get
            {
                if (userDownloadDAO == null)
                {
                    string className = namespaceString + ".UserDownloadDAO";
                    userDownloadDAO = (IUserDownloadDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return userDownloadDAO;
            }
        }

        public static IUserGroupDAO UserGroupDAO
        {
            get
            {
                if (userGroupDAO == null)
                {
                    string className = namespaceString + ".UserGroupDAO";
                    userGroupDAO = (IUserGroupDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return userGroupDAO;
            }
        }

        public static IBaiRongThirdLoginDAO BaiRongThirdLoginDAO
        {
            get
            {
                if (bairongThirdLoginDAO == null)
                {
                    string className = namespaceString + ".BaiRongThirdLoginDAO";
                    bairongThirdLoginDAO = (IBaiRongThirdLoginDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return bairongThirdLoginDAO;
            }
        }

        public static IUserLevelDAO UserLevelDAO
        {
            get
            {
                if (userLevelDAO == null)
                {
                    string className = namespaceString + ".UserLevelDAO";
                    userLevelDAO = (IUserLevelDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return userLevelDAO;
            }
        }

        public static ILevelRuleDAO LevelRuleDAO
        {
            get
            {
                if (levelRuleDAO == null)
                {
                    string className = namespaceString + ".LevelRuleDAO";
                    levelRuleDAO = (ILevelRuleDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return levelRuleDAO;
            }
        }

        public static IUserNoticeTemplateDAO UserNoticeTemplateDAO
        {
            get
            {
                if (userNoticeTemplateDAO == null)
                {
                    string className = namespaceString + ".UserNoticeTemplateDAO";
                    userNoticeTemplateDAO = (IUserNoticeTemplateDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return userNoticeTemplateDAO;
            }
        }

        public static IUserSecurityQuestionDAO UserSecurityQuestionDAO
        {
            get
            {
                if (userSecurityQuestionDAO == null)
                {
                    string className = namespaceString + ".UserSecurityQuestionDAO";
                    userSecurityQuestionDAO = (IUserSecurityQuestionDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return userSecurityQuestionDAO;
            }
        }

        public static IUserLogDAO UserLogDAO
        {
            get
            {
                if (userLogDAO == null)
                {
                    string className = namespaceString + ".UserLogDAO";
                    userLogDAO = (IUserLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return userLogDAO;
            }
        }

        public static ISMSServerDAO SMSServerDAO
        {
            get
            {
                if (smsServerDAO == null)
                {
                    string className = namespaceString + ".SMSServerDAO";
                    smsServerDAO = (ISMSServerDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return smsServerDAO;
            }
        }

        public static ICreateTaskDAO CreateTaskDAO
        {
            get
            {
                if (createTaskDAO == null)
                {
                    string className = namespaceString + ".CreateTaskDAO";
                    createTaskDAO = (ICreateTaskDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return createTaskDAO;
            }
        }



        #region by sofuny
        /// <summary>
        /// 新用户组
        /// 20160120
        /// </summary>
        private static IUserNewGroupDAO userNewGroupDAO;
        public static IUserNewGroupDAO UserNewGroupDAO
        {
            get
            {
                if (userNewGroupDAO == null)
                {
                    string className = namespaceString + ".UserNewGroupDAO";
                    userNewGroupDAO = (IUserNewGroupDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return userNewGroupDAO;
            }
        }


        #endregion


    }
}
