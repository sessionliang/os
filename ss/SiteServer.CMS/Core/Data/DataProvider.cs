using System.Reflection;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.CMS.Core
{
    public class DataProvider
    {
        private static string assemblyString;
        private static string namespaceString;

        static DataProvider()
        {
            assemblyString = "SiteServer.CMS";
            if (BaiRongDataProvider.DatabaseType == EDatabaseType.SqlServer)
            {
                namespaceString = "SiteServer.CMS.Provider.Data.SqlServer";
            }
            else if (BaiRongDataProvider.DatabaseType == EDatabaseType.Oracle)
            {
                namespaceString = "SiteServer.CMS.Provider.Data.Oracle";
            }
        }

        private static INodeGroupDAO nodeGroupDAO;
        public static INodeGroupDAO NodeGroupDAO
        {
            get
            {
                if (nodeGroupDAO == null)
                {
                    string className = namespaceString + ".NodeGroupDAO";
                    nodeGroupDAO = (INodeGroupDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return nodeGroupDAO;
            }
        }

        private static IContentGroupDAO contentGroupDAO;
        public static IContentGroupDAO ContentGroupDAO
        {
            get
            {
                if (contentGroupDAO == null)
                {
                    string className = namespaceString + ".ContentGroupDAO";
                    contentGroupDAO = (IContentGroupDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return contentGroupDAO;
            }
        }

        private static INodeDAO nodeDAO;
        public static INodeDAO NodeDAO
        {
            get
            {
                if (nodeDAO == null)
                {
                    string className = namespaceString + ".NodeDAO";
                    nodeDAO = (INodeDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return nodeDAO;
            }
        }

        private static IBackgroundContentDAO backgroundContentDAO;
        public static IBackgroundContentDAO BackgroundContentDAO
        {
            get
            {
                if (backgroundContentDAO == null)
                {
                    string className = namespaceString + ".BackgroundContentDAO";
                    backgroundContentDAO = (IBackgroundContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return backgroundContentDAO;
            }
        }

        private static IGovPublicContentDAO govPublicContentDAO;
        public static IGovPublicContentDAO GovPublicContentDAO
        {
            get
            {
                if (govPublicContentDAO == null)
                {
                    string className = namespaceString + ".GovPublicContentDAO";
                    govPublicContentDAO = (IGovPublicContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govPublicContentDAO;
            }
        }

        private static IGovPublicCategoryClassDAO govPublicCategoryClassDAO;
        public static IGovPublicCategoryClassDAO GovPublicCategoryClassDAO
        {
            get
            {
                if (govPublicCategoryClassDAO == null)
                {
                    string className = namespaceString + ".GovPublicCategoryClassDAO";
                    govPublicCategoryClassDAO = (IGovPublicCategoryClassDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govPublicCategoryClassDAO;
            }
        }

        private static IGovPublicCategoryDAO govPublicCategoryDAO;
        public static IGovPublicCategoryDAO GovPublicCategoryDAO
        {
            get
            {
                if (govPublicCategoryDAO == null)
                {
                    string className = namespaceString + ".GovPublicCategoryDAO";
                    govPublicCategoryDAO = (IGovPublicCategoryDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govPublicCategoryDAO;
            }
        }

        private static IGovPublicChannelDAO govPublicChannelDAO;
        public static IGovPublicChannelDAO GovPublicChannelDAO
        {
            get
            {
                if (govPublicChannelDAO == null)
                {
                    string className = namespaceString + ".GovPublicChannelDAO";
                    govPublicChannelDAO = (IGovPublicChannelDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govPublicChannelDAO;
            }
        }

        private static IGovPublicIdentifierRuleDAO govPublicIdentifierRuleDAO;
        public static IGovPublicIdentifierRuleDAO GovPublicIdentifierRuleDAO
        {
            get
            {
                if (govPublicIdentifierRuleDAO == null)
                {
                    string className = namespaceString + ".GovPublicIdentifierRuleDAO";
                    govPublicIdentifierRuleDAO = (IGovPublicIdentifierRuleDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govPublicIdentifierRuleDAO;
            }
        }

        private static IGovPublicIdentifierSeqDAO govPublicIdentifierSeqDAO;
        public static IGovPublicIdentifierSeqDAO GovPublicIdentifierSeqDAO
        {
            get
            {
                if (govPublicIdentifierSeqDAO == null)
                {
                    string className = namespaceString + ".GovPublicIdentifierSeqDAO";
                    govPublicIdentifierSeqDAO = (IGovPublicIdentifierSeqDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govPublicIdentifierSeqDAO;
            }
        }

        private static IGovPublicApplyDAO govPublicApplyDAO;
        public static IGovPublicApplyDAO GovPublicApplyDAO
        {
            get
            {
                if (govPublicApplyDAO == null)
                {
                    string className = namespaceString + ".GovPublicApplyDAO";
                    govPublicApplyDAO = (IGovPublicApplyDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govPublicApplyDAO;
            }
        }

        private static IGovPublicApplyLogDAO govPublicApplyLogDAO;
        public static IGovPublicApplyLogDAO GovPublicApplyLogDAO
        {
            get
            {
                if (govPublicApplyLogDAO == null)
                {
                    string className = namespaceString + ".GovPublicApplyLogDAO";
                    govPublicApplyLogDAO = (IGovPublicApplyLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govPublicApplyLogDAO;
            }
        }

        private static IGovPublicApplyRemarkDAO govPublicApplyRemarkDAO;
        public static IGovPublicApplyRemarkDAO GovPublicApplyRemarkDAO
        {
            get
            {
                if (govPublicApplyRemarkDAO == null)
                {
                    string className = namespaceString + ".GovPublicApplyRemarkDAO";
                    govPublicApplyRemarkDAO = (IGovPublicApplyRemarkDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govPublicApplyRemarkDAO;
            }
        }

        private static IGovPublicApplyReplyDAO govPublicApplyReplyDAO;
        public static IGovPublicApplyReplyDAO GovPublicApplyReplyDAO
        {
            get
            {
                if (govPublicApplyReplyDAO == null)
                {
                    string className = namespaceString + ".GovPublicApplyReplyDAO";
                    govPublicApplyReplyDAO = (IGovPublicApplyReplyDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govPublicApplyReplyDAO;
            }
        }

        private static IGovInteractChannelDAO govInteractChannelDAO;
        public static IGovInteractChannelDAO GovInteractChannelDAO
        {
            get
            {
                if (govInteractChannelDAO == null)
                {
                    string className = namespaceString + ".GovInteractChannelDAO";
                    govInteractChannelDAO = (IGovInteractChannelDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govInteractChannelDAO;
            }
        }

        private static IGovInteractContentDAO govInteractContentDAO;
        public static IGovInteractContentDAO GovInteractContentDAO
        {
            get
            {
                if (govInteractContentDAO == null)
                {
                    string className = namespaceString + ".GovInteractContentDAO";
                    govInteractContentDAO = (IGovInteractContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govInteractContentDAO;
            }
        }

        private static IGovInteractLogDAO govInteractApplyLogDAO;
        public static IGovInteractLogDAO GovInteractLogDAO
        {
            get
            {
                if (govInteractApplyLogDAO == null)
                {
                    string className = namespaceString + ".GovInteractLogDAO";
                    govInteractApplyLogDAO = (IGovInteractLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govInteractApplyLogDAO;
            }
        }

        private static IGovInteractRemarkDAO govInteractRemarkDAO;
        public static IGovInteractRemarkDAO GovInteractRemarkDAO
        {
            get
            {
                if (govInteractRemarkDAO == null)
                {
                    string className = namespaceString + ".GovInteractRemarkDAO";
                    govInteractRemarkDAO = (IGovInteractRemarkDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govInteractRemarkDAO;
            }
        }

        private static IGovInteractReplyDAO govInteractApplyReplyDAO;
        public static IGovInteractReplyDAO GovInteractReplyDAO
        {
            get
            {
                if (govInteractApplyReplyDAO == null)
                {
                    string className = namespaceString + ".GovInteractReplyDAO";
                    govInteractApplyReplyDAO = (IGovInteractReplyDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govInteractApplyReplyDAO;
            }
        }

        private static IGovInteractTypeDAO govInteractTypeDAO;
        public static IGovInteractTypeDAO GovInteractTypeDAO
        {
            get
            {
                if (govInteractTypeDAO == null)
                {
                    string className = namespaceString + ".GovInteractTypeDAO";
                    govInteractTypeDAO = (IGovInteractTypeDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govInteractTypeDAO;
            }
        }

        private static IGovInteractPermissionsDAO govInteractPermissionsDAO;
        public static IGovInteractPermissionsDAO GovInteractPermissionsDAO
        {
            get
            {
                if (govInteractPermissionsDAO == null)
                {
                    string className = namespaceString + ".GovInteractPermissionsDAO";
                    govInteractPermissionsDAO = (IGovInteractPermissionsDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return govInteractPermissionsDAO;
            }
        }

        private static IVoteContentDAO voteContentDAO;
        public static IVoteContentDAO VoteContentDAO
        {
            get
            {
                if (voteContentDAO == null)
                {
                    string className = namespaceString + ".VoteContentDAO";
                    voteContentDAO = (IVoteContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return voteContentDAO;
            }
        }

        private static IVoteOptionDAO voteOptionDAO;
        public static IVoteOptionDAO VoteOptionDAO
        {
            get
            {
                if (voteOptionDAO == null)
                {
                    string className = namespaceString + ".VoteOptionDAO";
                    voteOptionDAO = (IVoteOptionDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return voteOptionDAO;
            }
        }

        private static IVoteOperationDAO voteOperationDAO;
        public static IVoteOperationDAO VoteOperationDAO
        {
            get
            {
                if (voteOperationDAO == null)
                {
                    string className = namespaceString + ".VoteOperationDAO";
                    voteOperationDAO = (IVoteOperationDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return voteOperationDAO;
            }
        }

        private static IJobContentDAO jobContentDAO;
        public static IJobContentDAO JobContentDAO
        {
            get
            {
                if (jobContentDAO == null)
                {
                    string className = namespaceString + ".JobContentDAO";
                    jobContentDAO = (IJobContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return jobContentDAO;
            }
        }

        private static IContentDAO contentDAO;
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

        private static IPublishmentSystemDAO publishmentSystemDAO;
        public static IPublishmentSystemDAO PublishmentSystemDAO
        {
            get
            {
                if (publishmentSystemDAO == null)
                {
                    string className = namespaceString + ".PublishmentSystemDAO";
                    publishmentSystemDAO = (IPublishmentSystemDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return publishmentSystemDAO;
            }
        }

        private static ITemplateDAO templateDAO;
        public static ITemplateDAO TemplateDAO
        {
            get
            {
                if (templateDAO == null)
                {
                    string className = namespaceString + ".TemplateDAO";
                    templateDAO = (ITemplateDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return templateDAO;
            }
        }

        private static ITemplateLogDAO templateLogDAO;
        public static ITemplateLogDAO TemplateLogDAO
        {
            get
            {
                if (templateLogDAO == null)
                {
                    string className = namespaceString + ".TemplateLogDAO";
                    templateLogDAO = (ITemplateLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return templateLogDAO;
            }
        }

        private static IMenuDisplayDAO menuDisplayDAO;
        public static IMenuDisplayDAO MenuDisplayDAO
        {
            get
            {
                if (menuDisplayDAO == null)
                {
                    string className = namespaceString + ".MenuDisplayDAO";
                    menuDisplayDAO = (IMenuDisplayDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return menuDisplayDAO;
            }
        }

        private static ITrackingDAO trackingDAO;
        public static ITrackingDAO TrackingDAO
        {
            get
            {
                if (trackingDAO == null)
                {
                    string className = namespaceString + ".TrackingDAO";
                    trackingDAO = (ITrackingDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return trackingDAO;
            }
        }

        private static IGatherRuleDAO gatherRuleDAO;
        public static IGatherRuleDAO GatherRuleDAO
        {
            get
            {
                if (gatherRuleDAO == null)
                {
                    string className = namespaceString + ".GatherRuleDAO";
                    gatherRuleDAO = (IGatherRuleDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return gatherRuleDAO;
            }
        }

        private static IGatherDatabaseRuleDAO gatherDatabaseRuleDAO;
        public static IGatherDatabaseRuleDAO GatherDatabaseRuleDAO
        {
            get
            {
                if (gatherDatabaseRuleDAO == null)
                {
                    string className = namespaceString + ".GatherDatabaseRuleDAO";
                    gatherDatabaseRuleDAO = (IGatherDatabaseRuleDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return gatherDatabaseRuleDAO;
            }
        }

        private static IGatherFileRuleDAO gatherFileRuleDAO;
        public static IGatherFileRuleDAO GatherFileRuleDAO
        {
            get
            {
                if (gatherFileRuleDAO == null)
                {
                    string className = namespaceString + ".GatherFileRuleDAO";
                    gatherFileRuleDAO = (IGatherFileRuleDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return gatherFileRuleDAO;
            }
        }

        private static IAdvertisementDAO advertisementDAO;
        public static IAdvertisementDAO AdvertisementDAO
        {
            get
            {
                if (advertisementDAO == null)
                {
                    string className = namespaceString + ".AdvertisementDAO";
                    advertisementDAO = (IAdvertisementDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return advertisementDAO;
            }
        }

        private static IAdAreaDAO adAreaDAO;
        public static IAdAreaDAO AdAreaDAO
        {
            get
            {
                if (adAreaDAO == null)
                {
                    string className = namespaceString + ".AdAreaDAO";
                    adAreaDAO = (IAdAreaDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return adAreaDAO;
            }
        }

        private static IAdvDAO advDAO;
        public static IAdvDAO AdvDAO
        {
            get
            {
                if (advDAO == null)
                {
                    string className = namespaceString + ".AdvDAO";
                    advDAO = (IAdvDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return advDAO;
            }
        }

        private static IAdMaterialDAO adMaterialDAO;
        public static IAdMaterialDAO AdMaterialDAO
        {
            get
            {
                if (adMaterialDAO == null)
                {
                    string className = namespaceString + ".AdMaterialDAO";
                    adMaterialDAO = (IAdMaterialDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return adMaterialDAO;
            }
        }

        private static ISystemPermissionsDAO systemPermissionsDAO;
        public static ISystemPermissionsDAO SystemPermissionsDAO
        {
            get
            {
                if (systemPermissionsDAO == null)
                {
                    string className = namespaceString + ".SystemPermissionsDAO";
                    systemPermissionsDAO = (ISystemPermissionsDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return systemPermissionsDAO;
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

        private static ISeoMetaDAO seoMetaDAO;
        public static ISeoMetaDAO SeoMetaDAO
        {
            get
            {
                if (seoMetaDAO == null)
                {
                    string className = namespaceString + ".SeoMetaDAO";
                    seoMetaDAO = (ISeoMetaDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return seoMetaDAO;
            }
        }

        private static IStlTagDAO stlTagDAO;
        public static IStlTagDAO StlTagDAO
        {
            get
            {
                if (stlTagDAO == null)
                {
                    string className = namespaceString + ".StlTagDAO";
                    stlTagDAO = (IStlTagDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return stlTagDAO;
            }
        }

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

        private static IInnerLinkDAO innerLinkDAO;
        public static IInnerLinkDAO InnerLinkDAO
        {
            get
            {
                if (innerLinkDAO == null)
                {
                    string className = namespaceString + ".InnerLinkDAO";
                    innerLinkDAO = (IInnerLinkDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return innerLinkDAO;
            }
        }

        private static IInputDAO inputDAO;
        public static IInputDAO InputDAO
        {
            get
            {
                if (inputDAO == null)
                {
                    string className = namespaceString + ".InputDAO";
                    inputDAO = (IInputDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return inputDAO;
            }
        }

        private static IInputContentDAO inputContentDAO;
        public static IInputContentDAO InputContentDAO
        {
            get
            {
                if (inputContentDAO == null)
                {
                    string className = namespaceString + ".InputContentDAO";
                    inputContentDAO = (IInputContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return inputContentDAO;
            }
        }

        /// <summary>
        /// by 20151029 sofuny
        /// </summary>
        private static IInputClassifyDAO inputClassifyDAO;
        public static IInputClassifyDAO InputClassifyDAO
        {
            get
            {
                if (inputClassifyDAO == null)
                {
                    string className = namespaceString + ".InputClassifyDAO";
                    inputClassifyDAO = (IInputClassifyDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return inputClassifyDAO;
            }
        }

        private static IStarDAO starDAO;
        public static IStarDAO StarDAO
        {
            get
            {
                if (starDAO == null)
                {
                    string className = namespaceString + ".StarDAO";
                    starDAO = (IStarDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return starDAO;
            }
        }

        private static IStarSettingDAO starSettingDAO;
        public static IStarSettingDAO StarSettingDAO
        {
            get
            {
                if (starSettingDAO == null)
                {
                    string className = namespaceString + ".StarSettingDAO";
                    starSettingDAO = (IStarSettingDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return starSettingDAO;
            }
        }

        private static ITemplateMatchDAO templateMatchDAO;
        public static ITemplateMatchDAO TemplateMatchDAO
        {
            get
            {
                if (templateMatchDAO == null)
                {
                    string className = namespaceString + ".TemplateMatchDAO";
                    templateMatchDAO = (ITemplateMatchDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return templateMatchDAO;
            }
        }

        private static ILogDAO logDAO;
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

        private static ITagStyleDAO tagStyleDAO;
        public static ITagStyleDAO TagStyleDAO
        {
            get
            {
                if (tagStyleDAO == null)
                {
                    string className = namespaceString + ".TagStyleDAO";
                    tagStyleDAO = (ITagStyleDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return tagStyleDAO;
            }
        }

        private static IMailSendLogDAO mailSendLogDAO;
        public static IMailSendLogDAO MailSendLogDAO
        {
            get
            {
                if (mailSendLogDAO == null)
                {
                    string className = namespaceString + ".MailSendLogDAO";
                    mailSendLogDAO = (IMailSendLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return mailSendLogDAO;
            }
        }

        private static IMailSubscribeDAO mailSubscribeDAO;
        public static IMailSubscribeDAO MailSubscribeDAO
        {
            get
            {
                if (mailSubscribeDAO == null)
                {
                    string className = namespaceString + ".MailSubscribeDAO";
                    mailSubscribeDAO = (IMailSubscribeDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return mailSubscribeDAO;
            }
        }

        private static IResumeContentDAO resumeContentDAO;
        public static IResumeContentDAO ResumeContentDAO
        {
            get
            {
                if (resumeContentDAO == null)
                {
                    string className = namespaceString + ".ResumeContentDAO";
                    resumeContentDAO = (IResumeContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return resumeContentDAO;
            }
        }

        private static IPhotoDAO photoDAO;
        public static IPhotoDAO PhotoDAO
        {
            get
            {
                if (photoDAO == null)
                {
                    string className = namespaceString + ".PhotoDAO";
                    photoDAO = (IPhotoDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return photoDAO;
            }
        }


        private static ITeleplayDAO teleplayDAO;
        public static ITeleplayDAO TeleplayDAO
        {
            get
            {
                if (teleplayDAO == null)
                {
                    string className = namespaceString + ".TeleplayDAO";
                    teleplayDAO = (ITeleplayDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return teleplayDAO;
            }
        }

        private static IRelatedFieldDAO relatedFieldDAO;
        public static IRelatedFieldDAO RelatedFieldDAO
        {
            get
            {
                if (relatedFieldDAO == null)
                {
                    string className = namespaceString + ".RelatedFieldDAO";
                    relatedFieldDAO = (IRelatedFieldDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return relatedFieldDAO;
            }
        }

        private static IRelatedFieldItemDAO relatedFieldItemDAO;
        public static IRelatedFieldItemDAO RelatedFieldItemDAO
        {
            get
            {
                if (relatedFieldItemDAO == null)
                {
                    string className = namespaceString + ".RelatedFieldItemDAO";
                    relatedFieldItemDAO = (IRelatedFieldItemDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return relatedFieldItemDAO;
            }
        }

        private static ICommentDAO commentDAO;
        public static ICommentDAO CommentDAO
        {
            get
            {
                if (commentDAO == null)
                {
                    string className = namespaceString + ".CommentDAO";
                    commentDAO = (ICommentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return commentDAO;
            }
        }

        #region 敏感词 sessionliang
        private static IKeywordDAO keywordDAO;
        public static IKeywordDAO KeywordDAO
        {
            get
            {
                if (keywordDAO == null)
                {
                    string className = namespaceString + ".KeywordDAO";
                    keywordDAO = (IKeywordDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return keywordDAO;
            }
        }


        private static IKeywordClassifyDAO keywordClassifyDAO;
        public static IKeywordClassifyDAO KeywordClassifyDAO
        {
            get
            {
                if (keywordClassifyDAO == null)
                {
                    string className = namespaceString + ".KeywordClassifyDAO";
                    keywordClassifyDAO = (IKeywordClassifyDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return keywordClassifyDAO;
            }
        }
        #endregion

        #region 网站留言 sessionliang
        private static IWebsiteMessageClassifyDAO websiteMessageClassifyDAO;
        public static IWebsiteMessageClassifyDAO WebsiteMessageClassifyDAO
        {
            get
            {
                if (websiteMessageClassifyDAO == null)
                {
                    string className = namespaceString + ".WebsiteMessageClassifyDAO";
                    websiteMessageClassifyDAO = (IWebsiteMessageClassifyDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return websiteMessageClassifyDAO;
            }
        }


        private static IWebsiteMessageDAO websiteMessageDAO;
        public static IWebsiteMessageDAO WebsiteMessageDAO
        {
            get
            {
                if (websiteMessageDAO == null)
                {
                    string className = namespaceString + ".WebsiteMessageDAO";
                    websiteMessageDAO = (IWebsiteMessageDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return websiteMessageDAO;
            }
        }

        private static IWebsiteMessageContentDAO websiteMessageContentDAO;
        public static IWebsiteMessageContentDAO WebsiteMessageContentDAO
        {
            get
            {
                if (websiteMessageContentDAO == null)
                {
                    string className = namespaceString + ".WebsiteMessageContentDAO";
                    websiteMessageContentDAO = (IWebsiteMessageContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return websiteMessageContentDAO;
            }
        }

        private static IWebsiteMessageReplayTemplateDAO websiteMessageReplayTemplateDAO;
        public static IWebsiteMessageReplayTemplateDAO WebsiteMessageReplayTemplateDAO
        {
            get
            {
                if (websiteMessageReplayTemplateDAO == null)
                {
                    string className = namespaceString + ".WebsiteMessageReplayTemplateDAO";
                    websiteMessageReplayTemplateDAO = (IWebsiteMessageReplayTemplateDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return websiteMessageReplayTemplateDAO;
            }
        }
        #endregion

        #region 搜索关键字 sessionliang
        private static ISearchwordDAO searchwordDAO;
        public static ISearchwordDAO SearchwordDAO
        {
            get
            {
                if (searchwordDAO == null)
                {
                    string className = namespaceString + ".SearchwordDAO";
                    searchwordDAO = (ISearchwordDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return searchwordDAO;
            }
        }
        private static ISearchwordSettingDAO searchwordSettingDAO;
        public static ISearchwordSettingDAO SearchwordSettingDAO
        {
            get
            {
                if (searchwordSettingDAO == null)
                {
                    string className = namespaceString + ".SearchwordSettingDAO";
                    searchwordSettingDAO = (ISearchwordSettingDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return searchwordSettingDAO;
            }
        }
        #endregion

        private static IMlibDAO mlibDAO;
        public static IMlibDAO MlibDAO
        {
            get
            {
                if (mlibDAO == null)
                {
                    string className = namespaceString + ".MlibDAO";
                    mlibDAO = (IMlibDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return mlibDAO;
            }
        }


        /// <summary>
        /// by sofuny 20151104
        /// </summary>
        private static ISubscribeDAO subscribeDAO;
        public static ISubscribeDAO SubscribeDAO
        {
            get
            {
                if (subscribeDAO == null)
                {
                    string className = namespaceString + ".SubscribeDAO";
                    subscribeDAO = (ISubscribeDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return subscribeDAO;
            }
        }


        /// <summary>
        /// by sofuny 20151104
        /// </summary>
        private static ISubscribeUserDAO subscribeUserDAO;
        public static ISubscribeUserDAO SubscribeUserDAO
        {
            get
            {
                if (subscribeUserDAO == null)
                {
                    string className = namespaceString + ".SubscribeUserDAO";
                    subscribeUserDAO = (ISubscribeUserDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return subscribeUserDAO;
            }
        }

        /// <summary>
        /// by sofuny 20151104
        /// </summary>
        private static ISubscribeSetDAO subscribeSetDAO;
        public static ISubscribeSetDAO SubscribeSetDAO
        {
            get
            {
                if (subscribeSetDAO == null)
                {
                    string className = namespaceString + ".SubscribeSetDAO";
                    subscribeSetDAO = (ISubscribeSetDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return subscribeSetDAO;
            }
        }

        /// <summary>
        /// by sofuny 20151105
        /// </summary>
        private static ISubscribePushRecordDAO subscribePushRecordDAO;
        public static ISubscribePushRecordDAO SubscribePushRecordDAO
        {
            get
            {
                if (subscribePushRecordDAO == null)
                {
                    string className = namespaceString + ".SubscribePushRecordDAO";
                    subscribePushRecordDAO = (ISubscribePushRecordDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return subscribePushRecordDAO;
            }
        }
         

        /// <summary>
        /// by sofuny 20151124 培生智能推送
        /// 会员浏览量统计
        /// </summary>
        private static IViewsStatisticsDAO viewsStatisticsDAO;
        public static IViewsStatisticsDAO ViewsStatisticsDAO
        {
            get
            {
                if (viewsStatisticsDAO == null)
                {
                    string className = namespaceString + ".ViewsStatisticsDAO";
                    viewsStatisticsDAO = (IViewsStatisticsDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return viewsStatisticsDAO;
            }
        }


        #region 分支机构管理

        /// <summary>
        /// 机构分类
        /// </summary>
        private static IOrganizationClassifyDAO organizationClassifyDAO;
        public static IOrganizationClassifyDAO OrganizationClassifyDAO
        {
            get
            {
                if (organizationClassifyDAO == null)
                {
                    string className = namespaceString + ".OrganizationClassifyDAO";
                    organizationClassifyDAO = (IOrganizationClassifyDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return organizationClassifyDAO;
            }
        }

        /// <summary>
        /// 机构区域
        /// </summary>
        private static IOrganizationAreaDAO organizationAreaDAO;
        public static IOrganizationAreaDAO OrganizationAreaDAO
        {
            get
            {
                if (organizationAreaDAO == null)
                {
                    string className = namespaceString + ".OrganizationAreaDAO";
                    organizationAreaDAO = (IOrganizationAreaDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return organizationAreaDAO;
            }
        }

        /// <summary>
        /// 机构信息
        /// </summary>
        private static IOrganizationInfoDAO organizationInfoDAO;
        public static IOrganizationInfoDAO OrganizationInfoDAO
        {
            get
            {
                if (organizationInfoDAO == null)
                {
                    string className = namespaceString + ".OrganizationInfoDAO";
                    organizationInfoDAO = (IOrganizationInfoDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return organizationInfoDAO;
            }
        }
        #endregion


        #region 投稿管理 by 20160112 sofuny 

        /// <summary>
        /// 投稿范围
        /// </summary>
        private static IMLibScopeDAO mLibScopeDAO;
        public static IMLibScopeDAO MLibScopeDAO
        {
            get
            {
                if (mLibScopeDAO == null)
                {
                    string className = namespaceString + ".MLibScopeDAO";
                    mLibScopeDAO = (IMLibScopeDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return mLibScopeDAO;
            }
        }

        /// <summary>
        /// 投稿草稿
        /// </summary>
        private static IMLibDraftContentDAO mLibDraftContentDAO;
        public static IMLibDraftContentDAO MLibDraftContentDAO
        {
            get
            {
                if (mLibDraftContentDAO == null)
                {
                    string className = namespaceString + ".MLibDraftContentDAO";
                    mLibDraftContentDAO = (IMLibDraftContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return mLibDraftContentDAO;
            }
        }

        /// <summary>
        /// 新用户组
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

        #region 评价管理 by 20160226 sofuny


        /// <summary>
        /// 功能字段 
        /// </summary>
        private static IFunctionTableStylesDAO functionTableStylesDAO;
        public static IFunctionTableStylesDAO FunctionTableStylesDAO
        {
            get
            {
                if (functionTableStylesDAO == null)
                {
                    string className = namespaceString + ".FunctionTableStylesDAO";
                    functionTableStylesDAO = (IFunctionTableStylesDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return functionTableStylesDAO;
            }
        }

        /// <summary>
        /// 评价管理 
        /// </summary>
        private static IEvaluationContentDAO evaluationContentDAO;
        public static IEvaluationContentDAO EvaluationContentDAO
        {
            get
            {
                if (evaluationContentDAO == null)
                {
                    string className = namespaceString + ".EvaluationContentDAO";
                    evaluationContentDAO = (IEvaluationContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return evaluationContentDAO;
            }
        }


        /// <summary>
        /// 功能操作记录 
        /// </summary>
        private static IConsoleLogDAO consoleLogDAO;
        public static IConsoleLogDAO ConsoleLogDAO
        {
            get
            {
                if (consoleLogDAO == null)
                {
                    string className = namespaceString + ".ConsoleLogDAO";
                    consoleLogDAO = (IConsoleLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return consoleLogDAO;
            }
        }

        #endregion

        #region 试用管理 by 20160304 sofuny


        /// <summary>
        /// 试用申请 
        /// </summary>
        private static ITrialApplyDAO trialApplyDAO;
        public static ITrialApplyDAO TrialApplyDAO
        {
            get
            {
                if (trialApplyDAO == null)
                {
                    string className = namespaceString + ".TrialApplyDAO";
                    trialApplyDAO = (ITrialApplyDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return trialApplyDAO;
            }
        }

        /// <summary>
        /// 试用报告 
        /// </summary>
        private static ITrialReportDAO trialReportDAO;
        public static ITrialReportDAO TrialReportDAO
        {
            get
            {
                if (trialReportDAO == null)
                {
                    string className = namespaceString + ".TrialReportDAO";
                    trialReportDAO = (ITrialReportDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return trialReportDAO;
            }
        }
        #endregion

        #region 调查问卷 

        /// <summary>
        /// 调查问卷 
        /// </summary>
        private static ISurveyQuestionnaireDAO surveyQuestionnaireDAO;
        public static ISurveyQuestionnaireDAO SurveyQuestionnaireDAO
        {
            get
            {
                if (surveyQuestionnaireDAO == null)
                {
                    string className = namespaceString + ".SurveyQuestionnaireDAO";
                    surveyQuestionnaireDAO = (ISurveyQuestionnaireDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return surveyQuestionnaireDAO;
            }
        }
        #endregion

        #region 比较吧

        /// <summary>
        /// 比较反馈内容
        /// </summary>
        private static ICompareContentDAO compareContentDAO;
        public static ICompareContentDAO CompareContentDAO
        {
            get
            {
                if (compareContentDAO == null)
                {
                    string className = namespaceString + ".CompareContentDAO";
                    compareContentDAO = (ICompareContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return compareContentDAO;
            }
        }
        #endregion

        //private static ISiteserverThirdLoginDAO siteserverThirdLoginDAO;
        //public static ISiteserverThirdLoginDAO SiteserverThirdLoginDAO
        //{
        //    get
        //    {
        //        if (siteserverThirdLoginDAO == null)
        //        {
        //            string className = namespaceString + ".SiteserverThirdLoginDAO";
        //            siteserverThirdLoginDAO = (ISiteserverThirdLoginDAO)Assembly.Load(assemblyString).CreateInstance(className);
        //        }
        //        return siteserverThirdLoginDAO;
        //    }
        //}


    }
}
