using System.Reflection;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.B2C.Core
{
	public class DataProviderB2C
	{
        private static string assemblyString;
        private static string namespaceString;

        static DataProviderB2C()
		{
            assemblyString = "SiteServer.B2C";
            if (BaiRongDataProvider.DatabaseType == EDatabaseType.SqlServer)
            {
                namespaceString = "SiteServer.B2C.Provider.Data.SqlServer";
            }
            else if (BaiRongDataProvider.DatabaseType == EDatabaseType.Oracle)
            {
                namespaceString = "SiteServer.B2C.Provider.Data.Oracle";
            }
		}

        //private static INodeGroupDAO nodeGroupDAO;
        //public static INodeGroupDAO NodeGroupDAO
        //{
        //    get
        //    {
        //        if (nodeGroupDAO == null)
        //        {
        //            string className = namespaceString + ".NodeGroupDAO";
        //            nodeGroupDAO = (INodeGroupDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return nodeGroupDAO;
        //    }
        //}

        //private static IContentGroupDAO contentGroupDAO;
        //public static IContentGroupDAO ContentGroupDAO
        //{
        //    get
        //    {
        //        if (contentGroupDAO == null)
        //        {
        //            string className = namespaceString + ".ContentGroupDAO";
        //            contentGroupDAO = (IContentGroupDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return contentGroupDAO;
        //    }
        //}

        //private static INodeDAO nodeDAO;
        //public static INodeDAO NodeDAO
        //{
        //    get
        //    {
        //        if (nodeDAO == null)
        //        {
        //            string className = namespaceString + ".NodeDAO";
        //            nodeDAO = (INodeDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return nodeDAO;
        //    }
        //}

        //private static IBackgroundContentDAO backgroundContentDAO;
        //public static IBackgroundContentDAO BackgroundContentDAO
        //{
        //    get
        //    {
        //        if (backgroundContentDAO == null)
        //        {
        //            string className = namespaceString + ".BackgroundContentDAO";
        //            backgroundContentDAO = (IBackgroundContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return backgroundContentDAO;
        //    }
        //}

        //private static IJobContentDAO jobContentDAO;
        //public static IJobContentDAO JobContentDAO
        //{
        //    get
        //    {
        //        if (jobContentDAO == null)
        //        {
        //            string className = namespaceString + ".JobContentDAO";
        //            jobContentDAO = (IJobContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return jobContentDAO;
        //    }
        //}

        //private static IContentDAO contentDAO;
        //public static IContentDAO ContentDAO
        //{
        //    get
        //    {
        //        if (contentDAO == null)
        //        {
        //            string className = namespaceString + ".ContentDAO";
        //            contentDAO = (IContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return contentDAO;
        //    }
        //}

        //private static IPublishmentSystemDAO publishmentSystemDAO;
        //public static IPublishmentSystemDAO PublishmentSystemDAO
        //{
        //    get
        //    {
        //        if (publishmentSystemDAO == null)
        //        {
        //            string className = namespaceString + ".PublishmentSystemDAO";
        //            publishmentSystemDAO = (IPublishmentSystemDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return publishmentSystemDAO;
        //    }
        //}

        private static IB2CConfigurationDAO b2cConfigurationDAO;
        public static IB2CConfigurationDAO B2CConfigurationDAO
        {
            get
            {
                if (b2cConfigurationDAO == null)
                {
                    string className = namespaceString + ".B2CConfigurationDAO";
                    b2cConfigurationDAO = (IB2CConfigurationDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return b2cConfigurationDAO;
            }
        }

        //private static ITemplateDAO templateDAO;
        //public static ITemplateDAO TemplateDAO
        //{
        //    get
        //    {
        //        if (templateDAO == null)
        //        {
        //            string className = namespaceString + ".TemplateDAO";
        //            templateDAO = (ITemplateDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return templateDAO;
        //    }
        //}

        //private static IMenuDisplayDAO menuDisplayDAO;
        //public static IMenuDisplayDAO MenuDisplayDAO
        //{
        //    get
        //    {
        //        if (menuDisplayDAO == null)
        //        {
        //            string className = namespaceString + ".MenuDisplayDAO";
        //            menuDisplayDAO = (IMenuDisplayDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return menuDisplayDAO;
        //    }
        //}

        //private static ITrackingDAO trackingDAO;
        //public static ITrackingDAO TrackingDAO
        //{
        //    get
        //    {
        //        if (trackingDAO == null)
        //        {
        //            string className = namespaceString + ".TrackingDAO";
        //            trackingDAO = (ITrackingDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return trackingDAO;
        //    }
        //}

        //private static IGatherRuleDAO gatherRuleDAO;
        //public static IGatherRuleDAO GatherRuleDAO
        //{
        //    get
        //    {
        //        if (gatherRuleDAO == null)
        //        {
        //            string className = namespaceString + ".GatherRuleDAO";
        //            gatherRuleDAO = (IGatherRuleDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return gatherRuleDAO;
        //    }
        //}

        //private static IGatherDatabaseRuleDAO gatherDatabaseRuleDAO;
        //public static IGatherDatabaseRuleDAO GatherDatabaseRuleDAO
        //{
        //    get
        //    {
        //        if (gatherDatabaseRuleDAO == null)
        //        {
        //            string className = namespaceString + ".GatherDatabaseRuleDAO";
        //            gatherDatabaseRuleDAO = (IGatherDatabaseRuleDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return gatherDatabaseRuleDAO;
        //    }
        //}

        //private static IGatherFileRuleDAO gatherFileRuleDAO;
        //public static IGatherFileRuleDAO GatherFileRuleDAO
        //{
        //    get
        //    {
        //        if (gatherFileRuleDAO == null)
        //        {
        //            string className = namespaceString + ".GatherFileRuleDAO";
        //            gatherFileRuleDAO = (IGatherFileRuleDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return gatherFileRuleDAO;
        //    }
        //}

        ////private static IAdvertisementDAO advertisementDAO;
        ////public static IAdvertisementDAO AdvertisementDAO
        ////{
        ////    get
        ////    {
        ////        if (advertisementDAO == null)
        ////        {
        ////            string className = namespaceString + ".AdvertisementDAO";
        ////            advertisementDAO = (IAdvertisementDAO)Assembly.Load(assemblyString).CreateInstance(className);
        ////        }
        ////        return advertisementDAO;
        ////    }
        ////}

        ////private static IAdDAO adDAO;
        ////public static IAdDAO AdDAO
        ////{
        ////    get
        ////    {
        ////        if (adDAO == null)
        ////        {
        ////            string className = namespaceString + ".AdDAO";
        ////            adDAO = (IAdDAO)Assembly.Load(assemblyString).CreateInstance(className);
        ////        }
        ////        return adDAO;
        ////    }
        ////}

        //private static ISystemPermissionsDAO systemPermissionsDAO;
        //public static ISystemPermissionsDAO SystemPermissionsDAO
        //{
        //    get
        //    {
        //        if (systemPermissionsDAO == null)
        //        {
        //            string className = namespaceString + ".SystemPermissionsDAO";
        //            systemPermissionsDAO = (ISystemPermissionsDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return systemPermissionsDAO;
        //    }
        //}

        //private static IPermissionsDAO permissionsDAO;
        //public static IPermissionsDAO PermissionsDAO
        //{
        //    get
        //    {
        //        if (permissionsDAO == null)
        //        {
        //            string className = namespaceString + ".PermissionsDAO";
        //            permissionsDAO = (IPermissionsDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return permissionsDAO;
        //    }
        //}

        //private static ISeoMetaDAO seoMetaDAO;
        //public static ISeoMetaDAO SeoMetaDAO
        //{
        //    get
        //    {
        //        if (seoMetaDAO == null)
        //        {
        //            string className = namespaceString + ".SeoMetaDAO";
        //            seoMetaDAO = (ISeoMetaDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return seoMetaDAO;
        //    }
        //}

        //private static IPagePermissionsDAO pagePermissionsDAO;
        //public static IPagePermissionsDAO PagePermissionsDAO
        //{
        //    get
        //    {
        //        if (pagePermissionsDAO == null)
        //        {
        //            string className = namespaceString + ".PagePermissionsDAO";
        //            pagePermissionsDAO = (IPagePermissionsDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return pagePermissionsDAO;
        //    }
        //}

        //private static IStlTagDAO stlTagDAO;
        //public static IStlTagDAO StlTagDAO
        //{
        //    get
        //    {
        //        if (stlTagDAO == null)
        //        {
        //            string className = namespaceString + ".StlTagDAO";
        //            stlTagDAO = (IStlTagDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return stlTagDAO;
        //    }
        //}

        //private static IMachineDAO machineDAO;
        //public static IMachineDAO MachineDAO
        //{
        //    get
        //    {
        //        if (machineDAO == null)
        //        {
        //            string className = namespaceString + ".MachineDAO";
        //            machineDAO = (IMachineDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return machineDAO;
        //    }
        //}

        //private static ITaskDAO taskDAO;
        //public static ITaskDAO TaskDAO
        //{
        //    get
        //    {
        //        if (taskDAO == null)
        //        {
        //            string className = namespaceString + ".TaskDAO";
        //            taskDAO = (ITaskDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return taskDAO;
        //    }
        //}

        //private static IInnerLinkDAO innerLinkDAO;
        //public static IInnerLinkDAO InnerLinkDAO
        //{
        //    get
        //    {
        //        if (innerLinkDAO == null)
        //        {
        //            string className = namespaceString + ".InnerLinkDAO";
        //            innerLinkDAO = (IInnerLinkDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return innerLinkDAO;
        //    }
        //}

        //private static IInputDAO inputDAO;
        //public static IInputDAO InputDAO
        //{
        //    get
        //    {
        //        if (inputDAO == null)
        //        {
        //            string className = namespaceString + ".InputDAO";
        //            inputDAO = (IInputDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return inputDAO;
        //    }
        //}

        //private static IInputContentDAO inputContentDAO;
        //public static IInputContentDAO InputContentDAO
        //{
        //    get
        //    {
        //        if (inputContentDAO == null)
        //        {
        //            string className = namespaceString + ".InputContentDAO";
        //            inputContentDAO = (IInputContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return inputContentDAO;
        //    }
        //}

        //private static IStarDAO starDAO;
        //public static IStarDAO StarDAO
        //{
        //    get
        //    {
        //        if (starDAO == null)
        //        {
        //            string className = namespaceString + ".StarDAO";
        //            starDAO = (IStarDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return starDAO;
        //    }
        //}

        //private static IStarSettingDAO starSettingDAO;
        //public static IStarSettingDAO StarSettingDAO
        //{
        //    get
        //    {
        //        if (starSettingDAO == null)
        //        {
        //            string className = namespaceString + ".StarSettingDAO";
        //            starSettingDAO = (IStarSettingDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return starSettingDAO;
        //    }
        //}

        ////private static ITemplateRuleDAO templateRuleDAO;
        ////public static ITemplateRuleDAO TemplateRuleDAO
        ////{
        ////    get
        ////    {
        ////        if (templateRuleDAO == null)
        ////        {
        ////            string className = namespaceString + ".TemplateRuleDAO";
        ////            templateRuleDAO = (ITemplateRuleDAO)Assembly.Load(assemblyString).CreateInstance(className);
        ////        }
        ////        return templateRuleDAO;
        ////    }
        ////}

        //private static ITemplateMatchDAO templateMatchDAO;
        //public static ITemplateMatchDAO TemplateMatchDAO
        //{
        //    get
        //    {
        //        if (templateMatchDAO == null)
        //        {
        //            string className = namespaceString + ".TemplateMatchDAO";
        //            templateMatchDAO = (ITemplateMatchDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return templateMatchDAO;
        //    }
        //}

        //private static ILogDAO logDAO;
        //public static ILogDAO LogDAO
        //{
        //    get
        //    {
        //        if (logDAO == null)
        //        {
        //            string className = namespaceString + ".LogDAO";
        //            logDAO = (ILogDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return logDAO;
        //    }
        //}

        //private static ITagStyleDAO tagStyleDAO;
        //public static ITagStyleDAO TagStyleDAO
        //{
        //    get
        //    {
        //        if (tagStyleDAO == null)
        //        {
        //            string className = namespaceString + ".TagStyleDAO";
        //            tagStyleDAO = (ITagStyleDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return tagStyleDAO;
        //    }
        //}

        //private static ITopicDAO topicDAO;
        //public static ITopicDAO TopicDAO
        //{
        //    get
        //    {
        //        if (topicDAO == null)
        //        {
        //            string className = namespaceString + ".TopicDAO";
        //            topicDAO = (ITopicDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return topicDAO;
        //    }
        //}

        //private static ITaskLogDAO taskLogDAO;
        //public static ITaskLogDAO TaskLogDAO
        //{
        //    get
        //    {
        //        if (taskLogDAO == null)
        //        {
        //            string className = namespaceString + ".TaskLogDAO";
        //            taskLogDAO = (ITaskLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return taskLogDAO;
        //    }
        //}

        //private static IMailSendLogDAO mailSendLogDAO;
        //public static IMailSendLogDAO MailSendLogDAO
        //{
        //    get
        //    {
        //        if (mailSendLogDAO == null)
        //        {
        //            string className = namespaceString + ".MailSendLogDAO";
        //            mailSendLogDAO = (IMailSendLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return mailSendLogDAO;
        //    }
        //}

        //private static IMailSubscribeDAO mailSubscribeDAO;
        //public static IMailSubscribeDAO MailSubscribeDAO
        //{
        //    get
        //    {
        //        if (mailSubscribeDAO == null)
        //        {
        //            string className = namespaceString + ".MailSubscribeDAO";
        //            mailSubscribeDAO = (IMailSubscribeDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return mailSubscribeDAO;
        //    }
        //}

        //private static IResumeContentDAO resumeContentDAO;
        //public static IResumeContentDAO ResumeContentDAO
        //{
        //    get
        //    {
        //        if (resumeContentDAO == null)
        //        {
        //            string className = namespaceString + ".ResumeContentDAO";
        //            resumeContentDAO = (IResumeContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return resumeContentDAO;
        //    }
        //}

        //private static IPhotoDAO photoDAO;
        //public static IPhotoDAO PhotoDAO
        //{
        //    get
        //    {
        //        if (photoDAO == null)
        //        {
        //            string className = namespaceString + ".PhotoDAO";
        //            photoDAO = (IPhotoDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return photoDAO;
        //    }
        //}

        //private static IRelatedFieldDAO relatedFieldDAO;
        //public static IRelatedFieldDAO RelatedFieldDAO
        //{
        //    get
        //    {
        //        if (relatedFieldDAO == null)
        //        {
        //            string className = namespaceString + ".RelatedFieldDAO";
        //            relatedFieldDAO = (IRelatedFieldDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return relatedFieldDAO;
        //    }
        //}

        //private static IRelatedFieldItemDAO relatedFieldItemDAO;
        //public static IRelatedFieldItemDAO RelatedFieldItemDAO
        //{
        //    get
        //    {
        //        if (relatedFieldItemDAO == null)
        //        {
        //            string className = namespaceString + ".RelatedFieldItemDAO";
        //            relatedFieldItemDAO = (IRelatedFieldItemDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return relatedFieldItemDAO;
        //    }
        //}

        //private static IUserGroupDAO userGroupDAO;
        //public static IUserGroupDAO UserGroupDAO
        //{
        //    get
        //    {
        //        if (userGroupDAO == null)
        //        {
        //            string className = namespaceString + ".UserGroupDAO";
        //            userGroupDAO = (IUserGroupDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return userGroupDAO;
        //    }
        //}

        //private static IProductUserDAO productUserDAO;
        //public static IProductUserDAO ProductUserDAO
        //{
        //    get
        //    {
        //        if (productUserDAO == null)
        //        {
        //            string className = namespaceString + ".ProductUserDAO";
        //            productUserDAO = (IProductUserDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return productUserDAO;
        //    }
        //}

        //private static ICommentDAO commentDAO;
        //public static ICommentDAO CommentDAO
        //{
        //    get
        //    {
        //        if (commentDAO == null)
        //        {
        //            string className = namespaceString + ".CommentDAO";
        //            commentDAO = (ICommentDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return commentDAO;
        //    }
        //}

        ////private static IBrandDAO brandDAO;
        ////public static IBrandDAO BrandDAO
        ////{
        ////    get
        ////    {
        ////        if (brandDAO == null)
        ////        {
        ////            string className = namespaceString + ".BrandDAO";
        ////            brandDAO = (IBrandDAO)Assembly.Load(assemblyString).CreateInstance(className);
        ////        }
        ////        return brandDAO;
        ////    }
        ////}

        private static IFilterDAO filterDAO;
        public static IFilterDAO FilterDAO
        {
            get
            {
                if (filterDAO == null)
                {
                    string className = namespaceString + ".FilterDAO";
                    filterDAO = (IFilterDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return filterDAO;
            }
        }

        private static IFilterItemDAO filterItemDAO;
        public static IFilterItemDAO FilterItemDAO
        {
            get
            {
                if (filterItemDAO == null)
                {
                    string className = namespaceString + ".FilterItemDAO";
                    filterItemDAO = (IFilterItemDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return filterItemDAO;
            }
        }

        private static IGoodsContentDAO goodsContentDAO;
        public static IGoodsContentDAO GoodsContentDAO
        {
            get
            {
                if (goodsContentDAO == null)
                {
                    string className = namespaceString + ".GoodsContentDAO";
                    goodsContentDAO = (IGoodsContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return goodsContentDAO;
            }
        }

        private static IBrandContentDAO brandContentDAO;
        public static IBrandContentDAO BrandContentDAO
        {
            get
            {
                if (brandContentDAO == null)
                {
                    string className = namespaceString + ".BrandContentDAO";
                    brandContentDAO = (IBrandContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return brandContentDAO;
            }
        }

        //private static IStorageDAO storageDAO;
        //public static IStorageDAO StorageDAO
        //{
        //    get
        //    {
        //        if (storageDAO == null)
        //        {
        //            string className = namespaceString + ".StorageDAO";
        //            storageDAO = (IStorageDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return storageDAO;
        //    }
        //}

        private static ISpecDAO specDAO;
        public static ISpecDAO SpecDAO
        {
            get
            {
                if (specDAO == null)
                {
                    string className = namespaceString + ".SpecDAO";
                    specDAO = (ISpecDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return specDAO;
            }
        }

        private static ISpecItemDAO specItemDAO;
        public static ISpecItemDAO SpecItemDAO
        {
            get
            {
                if (specItemDAO == null)
                {
                    string className = namespaceString + ".SpecItemDAO";
                    specItemDAO = (ISpecItemDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return specItemDAO;
            }
        }

        //private static ISpecChannelDAO specChannelDAO;
        //public static ISpecChannelDAO SpecChannelDAO
        //{
        //    get
        //    {
        //        if (specChannelDAO == null)
        //        {
        //            string className = namespaceString + ".SpecChannelDAO";
        //            specChannelDAO = (ISpecChannelDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return specChannelDAO;
        //    }
        //}

        private static ISpecComboDAO specComboDAO;
        public static ISpecComboDAO SpecComboDAO
        {
            get
            {
                if (specComboDAO == null)
                {
                    string className = namespaceString + ".SpecComboDAO";
                    specComboDAO = (ISpecComboDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return specComboDAO;
            }
        }

        private static IGoodsDAO goodsDAO;
        public static IGoodsDAO GoodsDAO
        {
            get
            {
                if (goodsDAO == null)
                {
                    string className = namespaceString + ".GoodsDAO";
                    goodsDAO = (IGoodsDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return goodsDAO;
            }
        }

        private static ICartDAO cartDAO;
        public static ICartDAO CartDAO
        {
            get
            {
                if (cartDAO == null)
                {
                    string className = namespaceString + ".CartDAO";
                    cartDAO = (ICartDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return cartDAO;
            }
        }

        private static IOrderDAO orderDAO;
        public static IOrderDAO OrderDAO
        {
            get
            {
                if (orderDAO == null)
                {
                    string className = namespaceString + ".OrderDAO";
                    orderDAO = (IOrderDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return orderDAO;
            }
        }

        private static IOrderItemDAO orderItemDAO;
        public static IOrderItemDAO OrderItemDAO
        {
            get
            {
                if (orderItemDAO == null)
                {
                    string className = namespaceString + ".OrderItemDAO";
                    orderItemDAO = (IOrderItemDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return orderItemDAO;
            }
        }

        private static IOrderItemCommentDAO orderItemCommentDAO;
        public static IOrderItemCommentDAO OrderItemCommentDAO
        {
            get
            {
                if (orderItemCommentDAO == null)
                {
                    string className = namespaceString + ".OrderItemCommentDAO";
                    orderItemCommentDAO = (IOrderItemCommentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return orderItemCommentDAO;
            }
        }

        //private static ICommentContentDAO commentContentDAO;
        //public static ICommentContentDAO CommentContentDAO
        //{
        //    get
        //    {
        //        if (commentContentDAO == null)
        //        {
        //            string className = namespaceString + ".CommentContentDAO";
        //            commentContentDAO = (ICommentContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return commentContentDAO;
        //    }
        //}

        //private static IKeywordDAO keywordDAO;
        //public static IKeywordDAO KeywordDAO
        //{
        //    get
        //    {
        //        if (keywordDAO == null)
        //        {
        //            string className = namespaceString + ".KeywordDAO";
        //            keywordDAO = (IKeywordDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return keywordDAO;
        //    }
        //}

        //private static IPhotoContentDAO photoContentDAO;
        //public static IPhotoContentDAO PhotoContentDAO
        //{
        //    get
        //    {
        //        if (photoContentDAO == null)
        //        {
        //            string className = namespaceString + ".PhotoContentDAO";
        //            photoContentDAO = (IPhotoContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return photoContentDAO;
        //    }
        //}

        private static IPaymentDAO paymentDAO;
        public static IPaymentDAO PaymentDAO
        {
            get
            {
                if (paymentDAO == null)
                {
                    string className = namespaceString + ".PaymentDAO";
                    paymentDAO = (IPaymentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return paymentDAO;
            }
        }

        private static IShipmentDAO shipmentDAO;
        public static IShipmentDAO ShipmentDAO
        {
            get
            {
                if (shipmentDAO == null)
                {
                    string className = namespaceString + ".ShipmentDAO";
                    shipmentDAO = (IShipmentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return shipmentDAO;
            }
        }

        private static ILocationDAO locationDAO;
        public static ILocationDAO LocationDAO
        {
            get
            {
                if (locationDAO == null)
                {
                    string className = namespaceString + ".LocationDAO";
                    locationDAO = (ILocationDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return locationDAO;
            }
        }

        private static IConsigneeDAO consigneeDAO;
        public static IConsigneeDAO ConsigneeDAO
        {
            get
            {
                if (consigneeDAO == null)
                {
                    string className = namespaceString + ".ConsigneeDAO";
                    consigneeDAO = (IConsigneeDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return consigneeDAO;
            }
        }

        private static IInvoiceDAO invoiceDAO;
        public static IInvoiceDAO InvoiceDAO
        {
            get
            {
                if (invoiceDAO == null)
                {
                    string className = namespaceString + ".InvoiceDAO";
                    invoiceDAO = (IInvoiceDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return invoiceDAO;
            }
        }

        private static IPromotionDAO promotionDAO;
        public static IPromotionDAO PromotionDAO
        {
            get
            {
                if (promotionDAO == null)
                {
                    string className = namespaceString + ".PromotionDAO";
                    promotionDAO = (IPromotionDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return promotionDAO;
            }
        }

        private static IRequestDAO requestDAO;
        public static IRequestDAO RequestDAO
        {
            get
            {
                if (requestDAO == null)
                {
                    string className = namespaceString + ".RequestDAO";
                    requestDAO = (IRequestDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return requestDAO;
            }
        }

        private static IRequestAnswerDAO requestAnswerDAO;
        public static IRequestAnswerDAO RequestAnswerDAO
        {
            get
            {
                if (requestAnswerDAO == null)
                {
                    string className = namespaceString + ".RequestAnswerDAO";
                    requestAnswerDAO = (IRequestAnswerDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return requestAnswerDAO;
            }
        }

        private static IFollowDAO followDAO;
        public static IFollowDAO FollowDAO
        {
            get
            {
                if (followDAO == null)
                {
                    string className = namespaceString + ".FollowDAO";
                    followDAO = (IFollowDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return followDAO;
            }
        }

        private static IUserSettingDAO userSettingDAO;
        public static IUserSettingDAO UserSettingDAO
        {
            get
            {
                if (userSettingDAO == null)
                {
                    string className = namespaceString + ".UserSettingDAO";
                    userSettingDAO = (IUserSettingDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return userSettingDAO;
            }
        }

        private static IConsultationDAO consultationDAO;
        public static IConsultationDAO ConsultationDAO
        {
            get
            {
                if (consultationDAO == null)
                {
                    string className = namespaceString + ".ConsultationDAO";
                    consultationDAO = (IConsultationDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return consultationDAO;
            }
        }

        private static IOrderItemReturnDAO orderItemReturnDAO;
        public static IOrderItemReturnDAO OrderItemReturnDAO
        {
            get
            {
                if (orderItemReturnDAO == null)
                {
                    string className = namespaceString + ".OrderItemReturnDAO";
                    orderItemReturnDAO = (IOrderItemReturnDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return orderItemReturnDAO;
            }
        }

        private static IHistoryDAO historyDAO;
        public static IHistoryDAO HistoryDAO
        {
            get
            {
                if (historyDAO == null)
                {
                    string className = namespaceString + ".HistoryDAO";
                    historyDAO = (IHistoryDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return historyDAO;
            }
        }
        
    }
}
