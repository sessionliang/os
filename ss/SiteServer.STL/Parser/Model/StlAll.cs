using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using SiteServer.STL.Parser.StlEntity;
using SiteServer.STL.Parser.StlElement;

namespace SiteServer.STL.Parser.Model
{
    public class StlAll
    {
        public class StlElements
        {
            private StlElements()
            {
            }

            public static IDictionary GetElementsNameDictionary()
            {
                SortedList dictionary = new SortedList();

                dictionary.Add(StlA.ElementName, "获取链接");
                dictionary.Add(StlAction.ElementName, "执行动作");
                dictionary.Add(StlAd.ElementName, "固定广告");
                dictionary.Add(StlAnalysis.ElementName, "显示浏览量");
                dictionary.Add(StlAudio.ElementName, "播放音频");
                dictionary.Add(StlChannel.ElementName, "栏目值");
                dictionary.Add(StlChannels.ElementName, "栏目列表");
                dictionary.Add(StlComment.ElementName, "评论值");
                dictionary.Add(StlCommentInput.ElementName, "提交评论");
                dictionary.Add(StlComments.ElementName, "评论列表");
                dictionary.Add(StlContainer.ElementName, "容器");
                dictionary.Add(StlContent.ElementName, "内容值");
                dictionary.Add(StlContentInput.ElementName, "提交内容");
                dictionary.Add(StlContents.ElementName, "内容列表");
                dictionary.Add(StlCount.ElementName, "显示数值");
                dictionary.Add(StlCompareInput.ElementName, "内容比较反馈");
                dictionary.Add(StlDigg.ElementName, "掘客");
                dictionary.Add(StlDynamic.ElementName, "动态加载");
                dictionary.Add(StlEach.ElementName, "项循环");
                dictionary.Add(StlEvaluationInput.ElementName, "提交评价");
                dictionary.Add(StlFile.ElementName, "文件下载链接");
                dictionary.Add(StlFlash.ElementName, "显示Flash");
                dictionary.Add(StlFocusViewer.ElementName, "滚动焦点图");
                dictionary.Add(StlIf.ElementName, "条件判断");
                dictionary.Add(StlImage.ElementName, "显示图片");
                dictionary.Add(StlInclude.ElementName, "包含文件");
                dictionary.Add(StlInput.ElementName, "提交表单");
                dictionary.Add(StlInputContent.ElementName, "提交表单值");
                dictionary.Add(StlInputContents.ElementName, "提交表单列表");
                dictionary.Add(StlItemTemplate.ElementName, "列表项");
                dictionary.Add(StlIPushContents.ElementName, "智能推送内容列表");//by 20151125 sofuny 培生智能推送内容列表
                dictionary.Add(StlLayout.ElementName, "布局");
                dictionary.Add(StlLocation.ElementName, "当前位置");
                dictionary.Add(StlMarquee.ElementName, "无间隔滚动");
                dictionary.Add(StlMenu.ElementName, "下拉菜单");
                dictionary.Add(StlNavigation.ElementName, "显示导航");
                dictionary.Add(StlPageChannels.ElementName, "翻页栏目列表");
                dictionary.Add(StlPageComments.ElementName, "翻页评论列表");
                dictionary.Add(StlPageContents.ElementName, "翻页内容列表");
                dictionary.Add(StlPageInputContents.ElementName, "翻页提交表单列表");
                dictionary.Add(StlPageItem.ElementName, "翻页项");
                dictionary.Add(StlPageItems.ElementName, "翻页项容器");
                dictionary.Add(StlPageIPushContents.ElementName, "智能推送翻页内容列表");//by 20151125 sofuny 培生智能推送
                dictionary.Add(StlPageSqlContents.ElementName, "翻页数据库数据列表");
                dictionary.Add(StlPhoto.ElementName, "内容图片");
                dictionary.Add(StlPlayer.ElementName, "播放视频");
                dictionary.Add(StlPrinter.ElementName, "打印");
                dictionary.Add(StlQueryString.ElementName, "SQL查询语句");
                dictionary.Add(StlResume.ElementName, "提交简历");
                dictionary.Add(StlRss.ElementName, "Rss订阅");
                dictionary.Add(StlSearchInput.ElementName, "搜索框");
                dictionary.Add(StlSearchOutput.ElementName, "搜索结果");
                dictionary.Add(StlSelect.ElementName, "下拉列表");
                dictionary.Add(StlSite.ElementName, "应用值");
                dictionary.Add(StlSites.ElementName, "应用列表");
                dictionary.Add(StlSlide.ElementName, "图片幻灯片");
                dictionary.Add(StlSqlContent.ElementName, "数据库数据值");
                dictionary.Add(StlSqlContents.ElementName, "数据库数据列表");
                dictionary.Add(StlStar.ElementName, "评分");
                dictionary.Add(StlSubscribe.ElementName, "信息订阅");
                dictionary.Add(StlSurveyInput.ElementName, "调查问卷");
                dictionary.Add(StlTabs.ElementName, "页签切换");
                dictionary.Add(StlTags.ElementName, "标签");
                dictionary.Add(StlTree.ElementName, "树状导航");
                dictionary.Add(StlTrialApplyInput.ElementName, "提前试用申请");
                dictionary.Add(StlUser.ElementName, "用户相关");
                dictionary.Add(StlValue.ElementName, "获取值");
                dictionary.Add(StlVideo.ElementName, "播放视频");
                dictionary.Add(StlVisible.ElementName, "区域是否可视");
                dictionary.Add(StlVote.ElementName, "投票");
                dictionary.Add(StlZoom.ElementName, "文字缩放");

                #region B2C
                dictionary.Add(StlFilter.ElementName, "获取筛选项");
                dictionary.Add(StlFilters.ElementName, "获取筛选列表");
                dictionary.Add(StlSpec.ElementName, "获取商品规格");
                dictionary.Add(StlSpecs.ElementName, "获取商品规格列表");
                #endregion

                #region WCM
                dictionary.Add(StlGovInteractApply.ElementName, "互动交流提交");
                dictionary.Add(StlGovInteractQuery.ElementName, "互动交流查询");
                dictionary.Add(StlGovPublicApply.ElementName, "依申请公开提交");
                dictionary.Add(StlGovPublicQuery.ElementName, "依申请公开查询");
                #endregion

                return dictionary;
            }

            public static IDictionary GetElementsAttributesDictionary()
            {
                SortedList dictionary = new SortedList();

                dictionary.Add(StlA.ElementName, StlA.AttributeList);
                dictionary.Add(StlAction.ElementName, StlAction.AttributeList);
                dictionary.Add(StlAd.ElementName, StlAd.AttributeList);
                dictionary.Add(StlAnalysis.ElementName, StlAnalysis.AttributeList);
                dictionary.Add(StlAudio.ElementName, StlAudio.AttributeList);
                dictionary.Add(StlChannel.ElementName, StlChannel.AttributeList);
                dictionary.Add(StlChannels.ElementName, StlChannels.AttributeList);
                dictionary.Add(StlComment.ElementName, StlComment.AttributeList);
                dictionary.Add(StlCommentInput.ElementName, StlCommentInput.AttributeList);
                dictionary.Add(StlComments.ElementName, StlComments.AttributeList);
                dictionary.Add(StlContainer.ElementName, StlContainer.AttributeList);
                dictionary.Add(StlContent.ElementName, StlContent.AttributeList);
                dictionary.Add(StlContentInput.ElementName, StlContentInput.AttributeList);
                dictionary.Add(StlContents.ElementName, StlContents.AttributeList);
                dictionary.Add(StlCount.ElementName, StlCount.AttributeList);
                dictionary.Add(StlCompareInput.ElementName, StlCount.AttributeList);
                dictionary.Add(StlDigg.ElementName, StlDigg.AttributeList);
                dictionary.Add(StlDynamic.ElementName, StlDynamic.AttributeList);
                dictionary.Add(StlEach.ElementName, StlEach.AttributeList);
                dictionary.Add(StlEvaluationInput.ElementName, StlEvaluationInput.AttributeList);
                dictionary.Add(StlFile.ElementName, StlFile.AttributeList);
                dictionary.Add(StlFlash.ElementName, StlFlash.AttributeList);
                dictionary.Add(StlFocusViewer.ElementName, StlFocusViewer.AttributeList);
                dictionary.Add(StlIf.ElementName, StlIf.AttributeList);
                dictionary.Add(StlImage.ElementName, StlImage.AttributeList);
                dictionary.Add(StlInclude.ElementName, StlInclude.AttributeList);
                dictionary.Add(StlInput.ElementName, StlInput.AttributeList);
                dictionary.Add(StlInputContent.ElementName, StlInputContent.AttributeList);
                dictionary.Add(StlInputContents.ElementName, StlInputContents.AttributeList);
                dictionary.Add(StlItemTemplate.ElementName, StlItemTemplate.AttributeList);
                dictionary.Add(StlIPushContents.ElementName, StlIPushContents.AttributeList);//by 20151125 sofuny 培生智能推送
                dictionary.Add(StlLayout.ElementName, StlLayout.AttributeList);
                dictionary.Add(StlLocation.ElementName, StlLocation.AttributeList);
                dictionary.Add(StlMarquee.ElementName, StlMarquee.AttributeList);
                dictionary.Add(StlMenu.ElementName, StlMenu.AttributeList);
                dictionary.Add(StlNavigation.ElementName, StlNavigation.AttributeList);
                dictionary.Add(StlPageChannels.ElementName, StlPageChannels.AttributeList);
                dictionary.Add(StlPageComments.ElementName, StlPageComments.AttributeList);
                dictionary.Add(StlPageContents.ElementName, StlPageContents.AttributeList);
                dictionary.Add(StlPageItem.ElementName, StlPageItem.AttributeList);
                dictionary.Add(StlPageItems.ElementName, StlPageItems.AttributeList);
                dictionary.Add(StlPageIPushContents.ElementName, StlPageIPushContents.AttributeList);//by 20151125 sofuny 培生智能推送
                dictionary.Add(StlPageSqlContents.ElementName, StlPageSqlContents.AttributeList);
                dictionary.Add(StlPhoto.ElementName, StlPhoto.AttributeList);
                dictionary.Add(StlPlayer.ElementName, StlPlayer.AttributeList);
                dictionary.Add(StlPrinter.ElementName, StlPrinter.AttributeList);
                dictionary.Add(StlQueryString.ElementName, StlQueryString.AttributeList);
                dictionary.Add(StlResume.ElementName, StlResume.AttributeList);
                dictionary.Add(StlRss.ElementName, StlRss.AttributeList);
                dictionary.Add(StlSearchInput.ElementName, StlSearchInput.AttributeList);
                dictionary.Add(StlSearchOutput.ElementName, StlSearchOutput.AttributeList);
                dictionary.Add(StlSelect.ElementName, StlSelect.AttributeList);
                dictionary.Add(StlSite.ElementName, StlSite.AttributeList);
                dictionary.Add(StlSites.ElementName, StlSites.AttributeList);
                dictionary.Add(StlSlide.ElementName, StlSlide.AttributeList);
                dictionary.Add(StlSqlContent.ElementName, StlSqlContent.AttributeList);
                dictionary.Add(StlSqlContents.ElementName, StlSqlContents.AttributeList);
                dictionary.Add(StlStar.ElementName, StlStar.AttributeList);
                dictionary.Add(StlSubscribe.ElementName, StlSubscribe.AttributeList);
                dictionary.Add(StlSurveyInput.ElementName, StlSurveyInput.AttributeList);
                dictionary.Add(StlTabs.ElementName, StlTabs.AttributeList);
                dictionary.Add(StlTags.ElementName, StlTags.AttributeList);
                dictionary.Add(StlTree.ElementName, StlTree.AttributeList);
                dictionary.Add(StlTrialApplyInput.ElementName, StlTrialApplyInput.AttributeList);
                dictionary.Add(StlUser.ElementName, StlUser.AttributeList);
                dictionary.Add(StlValue.ElementName, StlValue.AttributeList);
                dictionary.Add(StlVideo.ElementName, StlVideo.AttributeList);
                dictionary.Add(StlVisible.ElementName, StlVisible.AttributeList);
                dictionary.Add(StlVote.ElementName, StlVote.AttributeList);
                dictionary.Add(StlZoom.ElementName, StlZoom.AttributeList);

                #region B2C
                dictionary.Add(StlFilter.ElementName, StlFilter.AttributeList);
                dictionary.Add(StlFilters.ElementName, StlFilters.AttributeList);
                dictionary.Add(StlSpec.ElementName, StlSpec.AttributeList);
                dictionary.Add(StlSpecs.ElementName, StlSpecs.AttributeList);
                #endregion

                #region WCM
                dictionary.Add(StlGovInteractApply.ElementName, StlGovInteractApply.AttributeList);
                dictionary.Add(StlGovInteractQuery.ElementName, StlGovInteractQuery.AttributeList);
                dictionary.Add(StlGovPublicApply.ElementName, StlGovPublicApply.AttributeList);
                dictionary.Add(StlGovPublicQuery.ElementName, StlGovPublicQuery.AttributeList);
                #endregion

                return dictionary;
            }
        }

        public class StlEntities
        {
            private StlEntities()
            {
            }

            public static IDictionary GetEntitiesNameDictionary()
            {
                SortedList dictionary = new SortedList();

                dictionary.Add(SiteServer.STL.Parser.StlEntity.StlEntities.EntityName, "通用实体");
                dictionary.Add(StlChannelEntities.EntityName, "栏目实体");
                dictionary.Add(StlContentEntities.EntityName, "内容实体");
                dictionary.Add(StlCommentEntities.EntityName, "评论实体");
                dictionary.Add(StlNavigationEntities.EntityName, "导航实体");
                dictionary.Add(StlPhotoEntities.EntityName, "图片实体");
                dictionary.Add(StlRequestEntities.EntityName, "请求实体");
                dictionary.Add(StlSqlEntities.EntityName, "数据库实体");
                dictionary.Add(StlUserEntities.EntityName, "用户实体");

                #region B2C
                dictionary.Add(StlB2CEntities.EntityName, "B2C实体");
                #endregion

                return dictionary;
            }

            public static IDictionary GetEntitiesAttributesDictionary()
            {
                SortedList dictionary = new SortedList();

                dictionary.Add(SiteServer.STL.Parser.StlEntity.StlEntities.EntityName, SiteServer.STL.Parser.StlEntity.StlEntities.AttributeList);
                dictionary.Add(StlChannelEntities.EntityName, StlChannelEntities.AttributeList);
                dictionary.Add(StlContentEntities.EntityName, StlContentEntities.AttributeList);
                dictionary.Add(StlCommentEntities.EntityName, StlCommentEntities.AttributeList);
                dictionary.Add(StlNavigationEntities.EntityName, StlNavigationEntities.AttributeList);
                dictionary.Add(StlPhotoEntities.EntityName, StlPhotoEntities.AttributeList);
                dictionary.Add(StlSqlEntities.EntityName, StlSqlEntities.AttributeList);
                dictionary.Add(StlUserEntities.EntityName, StlUserEntities.AttributeList);

                #region B2C
                dictionary.Add(StlB2CEntities.EntityName, StlB2CEntities.AttributeList);
                #endregion

                return dictionary;
            }
        }
    }
}
