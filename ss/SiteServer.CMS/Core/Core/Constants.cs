using BaiRong.Core;
using System.Reflection;
namespace SiteServer.CMS.Core
{
    public sealed class Constants
    {
        private Constants() { }

        public const int PageSize = 25;//后台分页数
        public const string TitleImageAppendix = "t_";
        public const string SmallImageAppendix = "s_";

        public const string HIDE_ELEMENT_STYLE = "display:none";
        public const string SHOW_ELEMENT_STYLE = "display:";

        //缓存使用的键值
        public const string Cache_PublishmentSystemInfoSortedList = "Cache_PublishmentSystemInfoSortedList";
        public const string Cache_ChannelDisplayHashtable = "Cache_ChannelDisplayHashtable";
        public const string Cache_ClassSortedList = "Cache_ClassSortedList";
        public const string Cache_ContentDisplayHashtable = "Cache_ContentDisplayHashtable";
        //public const string Cache_OwningNodeIDArrayList = "Cache_OwningNodeIDArrayList";
        public const string Cache_PopedomBackgroundHashtable = "Cache_PopedomBackgroundHashtable";
        public const string Cache_ResourceReferenceSortedList = "Cache_ResourceReferenceSortedList";
        public const string Cache_UserNameSortedList = "Cache_UserNameSortedList";

        //生成页面缓存使用的键值
        public const string CACHE_CREATE_CHANNELS_NODE_ID_ARRAYLIST = "_CreateChannels_NodeIDArrayList";
        public const string CACHE_CREATE_CONTENTS_NODE_ID_ARRAYLIST = "_CreateContents_NodeIDArrayList";
        public const string CACHE_CREATE_CONTENTS_CONTENT_ID_ARRAYLIST = "_CreateContents_ContentIDArrayList";
        public const string CACHE_CREATE_IDS_COLLECTION = "_CreateIDS_COLLECTION";
        public const string CACHE_CREATE_FILES_TEMPLATE_ID_ARRAYLIST = "_CreateFiles_TemplateIDArrayList";
        public const string CACHE_CREATE_FILES_INCLUDE_FILE_ARRAYLIST = "_CreateFiles_IncludeFileArrayList";

        public const string CACHE_PUBLISH_CHANNELS_NODE_ID_ARRAYLIST = "_PublishContents_NodeIDArrayList";
        public const string CACHE_PUBLISH_CONTENTS_CONTENT_ID_ARRAYLIST = "_PublishContents_ContentIDArrayList";

        public class STLElementName
        {
            public const string StlResume = "stl:resume";
            public const string StlGovInteractApply = "stl:govinteractapply";
            public const string StlGovInteractQuery = "stl:govinteractquery";
            public const string StlCommentInput = "stl:commentinput";
            public const string StlComments = "stl:comments";
        }

        public class StlTemplateManagerActionType
        {
            public const string ActionType_Input = "input";
            public const string ActionType_WebsiteMessage = "websitemessage";
            public const string ActionType_Content = "content";
            public const string ActionType_Login = "login";
            public const string ActionType_Register = "register";
            public const string ActionType_Resume = "resume";
            public const string ActionType_GovPublicApply = "govpublicapply";
            public const string ActionType_GovPublicQuery = "govpublicquery";
            public const string ActionType_GovInteractApply = "govinteractapply";
            public const string ActionType_GovInteractQuery = "govinteractquery";
            public const string ActionType_Vote = "vote";



            // by 20151127 sofuny 
            public const string ActionType_SubscribeApply = "subscribeapply";//信息订阅会员订阅
            public const string ActionType_SubscribeQuery = "subscribequery";//信息订阅查询订阅内容  

            public const string Type_AddTrackerCount = "AddTrackerCount";
            public const string Type_AddCountHits = "AddCountHits";
            public const string Type_IsVisible = "IsVisible";
            public const string Type_LoadingChannels = "LoadingChannels";
            public const string Type_IsVisitAllowed = "IsVisitAllowed";
            public const string Type_GetChannels = "GetChannels";
            public const string Type_GetVote = "GetVote";
            public const string Type_Login = "Login";

            public const string Type_GetSearchwords = "Type_GetSearchwords";
            public const string ActionType_GetTotalNum = "GetTotalNum";

            public const string Type_AddIntelligentPushCount = "AddIntelligentPushCount";//by 20151125 sofuny 培生智能推送--会员浏览内容统计内容上一级栏目
            public const string ActionType_OrganizationQuery = "organizationquery";//分支机构查询
            public const string ActionType_OrganizationShark = "organizationshark";//分支机构范围搜索
            public const string ActionType_OrganizationClassifyQuery = "organizationclassifyquery";//分支机构分类查询
            public const string ActionType_OrganizationAreaByClassifyQuery = "organizationareabyclassifyquery";//分支机构区域按分类查询
            public const string ActionType_OrganizationAreaByParentIDQuery = "organizationareabybyparentidquery";//分支机构区域按父级查询

            public const string ActionType_Ad = "ad";

            #region 投稿管理
            public const string ActionType_UserMLibDraftContent = "usermlibdraftcontent";//分支机构区域按父级查询
            #endregion

            #region by 20160229 sofuny 评价管理

            public const string ActionType_EvaluationApply = "evaluationapply";// 评价管理

            #endregion

            #region add by sessonliang 20160315 siteserver.cms.services.utils
            public const string ActionType_Redirect = "Redirect";
            public const string ActionType_Download = "Download";
            public const string ActionType_StlTrigger = "StlTrigger";
            #endregion
        }
    }
}
