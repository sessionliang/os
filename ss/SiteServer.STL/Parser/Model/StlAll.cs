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

                dictionary.Add(StlA.ElementName, "��ȡ����");
                dictionary.Add(StlAction.ElementName, "ִ�ж���");
                dictionary.Add(StlAd.ElementName, "�̶����");
                dictionary.Add(StlAnalysis.ElementName, "��ʾ�����");
                dictionary.Add(StlAudio.ElementName, "������Ƶ");
                dictionary.Add(StlChannel.ElementName, "��Ŀֵ");
                dictionary.Add(StlChannels.ElementName, "��Ŀ�б�");
                dictionary.Add(StlComment.ElementName, "����ֵ");
                dictionary.Add(StlCommentInput.ElementName, "�ύ����");
                dictionary.Add(StlComments.ElementName, "�����б�");
                dictionary.Add(StlContainer.ElementName, "����");
                dictionary.Add(StlContent.ElementName, "����ֵ");
                dictionary.Add(StlContentInput.ElementName, "�ύ����");
                dictionary.Add(StlContents.ElementName, "�����б�");
                dictionary.Add(StlCount.ElementName, "��ʾ��ֵ");
                dictionary.Add(StlCompareInput.ElementName, "���ݱȽϷ���");
                dictionary.Add(StlDigg.ElementName, "���");
                dictionary.Add(StlDynamic.ElementName, "��̬����");
                dictionary.Add(StlEach.ElementName, "��ѭ��");
                dictionary.Add(StlEvaluationInput.ElementName, "�ύ����");
                dictionary.Add(StlFile.ElementName, "�ļ���������");
                dictionary.Add(StlFlash.ElementName, "��ʾFlash");
                dictionary.Add(StlFocusViewer.ElementName, "��������ͼ");
                dictionary.Add(StlIf.ElementName, "�����ж�");
                dictionary.Add(StlImage.ElementName, "��ʾͼƬ");
                dictionary.Add(StlInclude.ElementName, "�����ļ�");
                dictionary.Add(StlInput.ElementName, "�ύ��");
                dictionary.Add(StlInputContent.ElementName, "�ύ��ֵ");
                dictionary.Add(StlInputContents.ElementName, "�ύ���б�");
                dictionary.Add(StlItemTemplate.ElementName, "�б���");
                dictionary.Add(StlIPushContents.ElementName, "�������������б�");//by 20151125 sofuny �����������������б�
                dictionary.Add(StlLayout.ElementName, "����");
                dictionary.Add(StlLocation.ElementName, "��ǰλ��");
                dictionary.Add(StlMarquee.ElementName, "�޼������");
                dictionary.Add(StlMenu.ElementName, "�����˵�");
                dictionary.Add(StlNavigation.ElementName, "��ʾ����");
                dictionary.Add(StlPageChannels.ElementName, "��ҳ��Ŀ�б�");
                dictionary.Add(StlPageComments.ElementName, "��ҳ�����б�");
                dictionary.Add(StlPageContents.ElementName, "��ҳ�����б�");
                dictionary.Add(StlPageInputContents.ElementName, "��ҳ�ύ���б�");
                dictionary.Add(StlPageItem.ElementName, "��ҳ��");
                dictionary.Add(StlPageItems.ElementName, "��ҳ������");
                dictionary.Add(StlPageIPushContents.ElementName, "�������ͷ�ҳ�����б�");//by 20151125 sofuny ������������
                dictionary.Add(StlPageSqlContents.ElementName, "��ҳ���ݿ������б�");
                dictionary.Add(StlPhoto.ElementName, "����ͼƬ");
                dictionary.Add(StlPlayer.ElementName, "������Ƶ");
                dictionary.Add(StlPrinter.ElementName, "��ӡ");
                dictionary.Add(StlQueryString.ElementName, "SQL��ѯ���");
                dictionary.Add(StlResume.ElementName, "�ύ����");
                dictionary.Add(StlRss.ElementName, "Rss����");
                dictionary.Add(StlSearchInput.ElementName, "������");
                dictionary.Add(StlSearchOutput.ElementName, "�������");
                dictionary.Add(StlSelect.ElementName, "�����б�");
                dictionary.Add(StlSite.ElementName, "Ӧ��ֵ");
                dictionary.Add(StlSites.ElementName, "Ӧ���б�");
                dictionary.Add(StlSlide.ElementName, "ͼƬ�õ�Ƭ");
                dictionary.Add(StlSqlContent.ElementName, "���ݿ�����ֵ");
                dictionary.Add(StlSqlContents.ElementName, "���ݿ������б�");
                dictionary.Add(StlStar.ElementName, "����");
                dictionary.Add(StlSubscribe.ElementName, "��Ϣ����");
                dictionary.Add(StlSurveyInput.ElementName, "�����ʾ�");
                dictionary.Add(StlTabs.ElementName, "ҳǩ�л�");
                dictionary.Add(StlTags.ElementName, "��ǩ");
                dictionary.Add(StlTree.ElementName, "��״����");
                dictionary.Add(StlTrialApplyInput.ElementName, "��ǰ��������");
                dictionary.Add(StlUser.ElementName, "�û����");
                dictionary.Add(StlValue.ElementName, "��ȡֵ");
                dictionary.Add(StlVideo.ElementName, "������Ƶ");
                dictionary.Add(StlVisible.ElementName, "�����Ƿ����");
                dictionary.Add(StlVote.ElementName, "ͶƱ");
                dictionary.Add(StlZoom.ElementName, "��������");

                #region B2C
                dictionary.Add(StlFilter.ElementName, "��ȡɸѡ��");
                dictionary.Add(StlFilters.ElementName, "��ȡɸѡ�б�");
                dictionary.Add(StlSpec.ElementName, "��ȡ��Ʒ���");
                dictionary.Add(StlSpecs.ElementName, "��ȡ��Ʒ����б�");
                #endregion

                #region WCM
                dictionary.Add(StlGovInteractApply.ElementName, "���������ύ");
                dictionary.Add(StlGovInteractQuery.ElementName, "����������ѯ");
                dictionary.Add(StlGovPublicApply.ElementName, "�����빫���ύ");
                dictionary.Add(StlGovPublicQuery.ElementName, "�����빫����ѯ");
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
                dictionary.Add(StlIPushContents.ElementName, StlIPushContents.AttributeList);//by 20151125 sofuny ������������
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
                dictionary.Add(StlPageIPushContents.ElementName, StlPageIPushContents.AttributeList);//by 20151125 sofuny ������������
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

                dictionary.Add(SiteServer.STL.Parser.StlEntity.StlEntities.EntityName, "ͨ��ʵ��");
                dictionary.Add(StlChannelEntities.EntityName, "��Ŀʵ��");
                dictionary.Add(StlContentEntities.EntityName, "����ʵ��");
                dictionary.Add(StlCommentEntities.EntityName, "����ʵ��");
                dictionary.Add(StlNavigationEntities.EntityName, "����ʵ��");
                dictionary.Add(StlPhotoEntities.EntityName, "ͼƬʵ��");
                dictionary.Add(StlRequestEntities.EntityName, "����ʵ��");
                dictionary.Add(StlSqlEntities.EntityName, "���ݿ�ʵ��");
                dictionary.Add(StlUserEntities.EntityName, "�û�ʵ��");

                #region B2C
                dictionary.Add(StlB2CEntities.EntityName, "B2Cʵ��");
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
