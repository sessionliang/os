using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.ListTemplate;
using SiteServer.CMS.Model;
using System;
using System.Collections.Generic;
using SiteServer.CMS.Core;
using System.Collections;

namespace SiteServer.STL.Parser.StlElement
{
    /// <summary>
    /// by 20151125 sofuny
    /// ������������
    /// �����������������б��ǩ
    /// </summary>
    public class StlIPushContents
    {
        public const string ElementName = "stl:ipushcontents";//�����б�

        public const string Attribute_ChannelIndex = "channelindex";			//��Ŀ����
        public const string Attribute_ChannelName = "channelname";				//��Ŀ����
        public const string Attribute_ChannelIndexNot = "channelindexnot";	            //ָ������ʾ����Ŀ����
        public const string Attribute_UpLevel = "uplevel";						//�ϼ���Ŀ�ļ���
        public const string Attribute_TopLevel = "toplevel";					//����ҳ���µ���Ŀ����
        public const string Attribute_Scope = "scope";							//���ݷ�Χ
        public const string Attribute_Group = "group";		                    //ָ����ʾ��������
        public const string Attribute_GroupNot = "groupnot";	                //ָ������ʾ��������
        public const string Attribute_GroupChannel = "groupchannel";		    //ָ����ʾ����Ŀ��
        public const string Attribute_GroupChannelNot = "groupchannelnot";	    //ָ������ʾ����Ŀ��
        public const string Attribute_GroupContent = "groupcontent";		    //ָ����ʾ��������
        public const string Attribute_GroupContentNot = "groupcontentnot";	    //ָ������ʾ��������
        public const string Attribute_Tags = "tags";	                        //ָ����ǩ

        public const string Attribute_IsTop = "istop";                          //����ʾ�ö�����
        public const string Attribute_IsRecommend = "isrecommend";              //����ʾ�Ƽ�����
        public const string Attribute_IsHot = "ishot";                          //����ʾ�ȵ�����
        public const string Attribute_IsColor = "iscolor";                      //����ʾ��Ŀ����

        public const string Attribute_TotalNum = "totalnum";					//��ʾ������Ŀ
        public const string Attribute_StartNum = "startnum";					//�ӵڼ�����Ϣ��ʼ��ʾ
        public const string Attribute_TitleWordNum = "titlewordnum";			//���ݱ�����������
        public const string Attribute_Order = "order";						    //����
        public const string Attribute_IsImage = "isimage";					    //����ʾͼƬ����
        public const string Attribute_IsVideo = "isvideo";					    //����ʾ��Ƶ����
        public const string Attribute_IsFile = "isfile";                        //����ʾ��������
        public const string Attribute_IsNoDup = "isnodup";                      //����ʾ�ظ����������
        public const string Attribute_IsRelatedContents = "isrelatedcontents";  //��ʾ��������б�
        public const string Attribute_Where = "where";                          //��ȡ�����б�������ж�
        public const string Attribute_IsDynamic = "isdynamic";                  //�Ƿ�̬��ʾ

        public const string Attribute_Columns = "columns";
        public const string Attribute_Direction = "direction";
        public const string Attribute_Height = "height";
        public const string Attribute_Width = "width";
        public const string Attribute_Align = "align";
        public const string Attribute_ItemHeight = "itemheight";
        public const string Attribute_ItemWidth = "itemwidth";
        public const string Attribute_ItemAlign = "itemalign";
        public const string Attribute_ItemVerticalAlign = "itemverticalalign";
        public const string Attribute_ItemClass = "itemclass";
        public const string Attribute_Layout = "layout";

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();

                attributes.Add("cellpadding", "���");
                attributes.Add("cellspacing", "���");
                attributes.Add("class", "Css��");
                attributes.Add(Attribute_Columns, "����");
                attributes.Add(Attribute_Direction, "����");
                attributes.Add(Attribute_Layout, "ָ���б��ַ�ʽ");

                attributes.Add(Attribute_Height, "����߶�");
                attributes.Add(Attribute_Width, "������");
                attributes.Add(Attribute_Align, "�������");
                attributes.Add(Attribute_ItemHeight, "��߶�");
                attributes.Add(Attribute_ItemWidth, "����");
                attributes.Add(Attribute_ItemAlign, "��ˮƽ����");
                attributes.Add(Attribute_ItemVerticalAlign, "�ֱ����");
                attributes.Add(Attribute_ItemClass, "��Css��");

                attributes.Add(Attribute_TotalNum, "��ʾ������Ŀ");
                attributes.Add(Attribute_StartNum, "�ӵڼ�����Ϣ��ʼ��ʾ");
                attributes.Add(Attribute_TitleWordNum, "���ݱ�����������");
                attributes.Add(Attribute_Order, "����");
                attributes.Add(Attribute_IsImage, "����ʾͼƬ����");
                attributes.Add(Attribute_IsVideo, "����ʾ��Ƶ����");
                attributes.Add(Attribute_IsFile, "����ʾ��������");
                attributes.Add(Attribute_IsNoDup, "����ʾ�ظ����������");
                attributes.Add(Attribute_IsRelatedContents, "��ʾ��������б�");
                attributes.Add(Attribute_Where, "��ȡ�����б�������ж�");
                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");

                attributes.Add(Attribute_ChannelIndex, "��Ŀ����");
                attributes.Add(Attribute_ChannelName, "��Ŀ����");
                attributes.Add(Attribute_ChannelIndexNot, "ָ������ʾ����Ŀ����");
                attributes.Add(Attribute_UpLevel, "�ϼ���Ŀ�ļ���");
                attributes.Add(Attribute_TopLevel, "����ҳ���µ���Ŀ����");
                attributes.Add(Attribute_Scope, "���ݷ�Χ");
                attributes.Add(Attribute_GroupChannel, "ָ����ʾ����Ŀ��");
                attributes.Add(Attribute_GroupChannelNot, "ָ������ʾ����Ŀ��");
                attributes.Add(Attribute_GroupContent, "ָ����ʾ��������");
                attributes.Add(Attribute_GroupContentNot, "ָ������ʾ��������");
                attributes.Add(Attribute_Tags, "ָ����ǩ");
                attributes.Add(Attribute_IsTop, "����ʾ�ö�����");
                attributes.Add(Attribute_IsRecommend, "����ʾ�Ƽ�����");
                attributes.Add(Attribute_IsHot, "����ʾ�ȵ�����");
                attributes.Add(Attribute_IsColor, "����ʾ��Ŀ����");

                return attributes;
            }
        }

        public static Dictionary<string, string> BooleanAttributeList
        {
            get
            {
                Dictionary<string, string> attributes = new Dictionary<string, string>();

                attributes.Add(Attribute_IsTop, "����ʾ�ö�����");
                attributes.Add(Attribute_IsRecommend, "����ʾ�Ƽ�����");
                attributes.Add(Attribute_IsHot, "����ʾ�ȵ�����");
                attributes.Add(Attribute_IsColor, "����ʾ��Ŀ����");

                attributes.Add(Attribute_IsImage, "����ʾͼƬ����");
                attributes.Add(Attribute_IsVideo, "����ʾ��Ƶ����");
                attributes.Add(Attribute_IsFile, "����ʾ��������");
                attributes.Add(Attribute_IsNoDup, "����ʾ�ظ����������");
                attributes.Add(Attribute_IsRelatedContents, "��ʾ��������б�");

                attributes.Add(Attribute_IsDynamic, "�Ƿ�̬��ʾ");

                return attributes;
            }
        }

        //�Բ��ܹ���ҳ�ġ������б���stl:contents��Ԫ�ؽ��н���
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                IPushContentsDisplayInfo displayInfo = IPushContentsDisplayInfo.GetContentsDisplayInfoByXmlNode(node, pageInfo, contextInfo, EContextType.Content);

                if (displayInfo.IsDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo, displayInfo);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        public static IEnumerable GetDataSource(PageInfo pageInfo, ContextInfo contextInfo, IPushContentsDisplayInfo displayInfo)
        {
            int channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, displayInfo.UpLevel, displayInfo.TopLevel);

            channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, displayInfo.ChannelIndex, displayInfo.ChannelName);

            return StlDataUtility.GetContentsDataSource(pageInfo.PublishmentSystemInfo, channelID, contextInfo.ContentID, displayInfo.GroupContent, displayInfo.GroupContentNot, displayInfo.Tags, displayInfo.IsImageExists, displayInfo.IsImage, displayInfo.IsVideoExists, displayInfo.IsVideo, displayInfo.IsFileExists, displayInfo.IsFile, displayInfo.IsNoDup, displayInfo.IsRelatedContents, displayInfo.StartNum, displayInfo.TotalNum, displayInfo.OrderByString, displayInfo.IsTopExists, displayInfo.IsTop, displayInfo.IsRecommendExists, displayInfo.IsRecommend, displayInfo.IsHotExists, displayInfo.IsHot, displayInfo.IsColorExists, displayInfo.IsColor, displayInfo.Where, displayInfo.Scope, displayInfo.GroupChannel, displayInfo.GroupChannelNot, displayInfo.OtherAttributes);
        }
         

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, IPushContentsDisplayInfo displayInfo)
        {
            string parsedContent = string.Empty;

            int titleWordNum = contextInfo.TitleWordNum;
            contextInfo.TitleWordNum = displayInfo.TitleWordNum;

            #region by 20151125 sofuny ������������
            //��ѯ��Ա�����ͳ�Ʊ��ó�����nodeID���ϣ��ٵõ�У�������������ƣ����¸�displayInfo�����ChannelIndex,ChannelName��ֵ���ٵõ�����
            UserInfo uinfo = UserManager.Current;//��ȡ��ǰ��¼�û�
            //uinfo = BaiRongDataProvider.UserDAO.GetUserInfo(1);//test user
            PublishmentSystemInfo pubinfo = PublishmentSystemManager.GetPublishmentSystemInfo(pageInfo.PublishmentSystemID);
            if (uinfo.UserID > 0 && pubinfo.Additional.IsIntelligentPushCount)//�û���¼����վ��������������
            {

                //��ȡ���ų�����Ŀ
                ArrayList channelNots = new ArrayList();
                string whereStr = string.Empty;
                if (!string.IsNullOrEmpty(displayInfo.ChannelIndexNot))
                {
                    ArrayList notInfos = DataProvider.NodeDAO.GetNodeInfoArrayListByPublishmentSystemID(pageInfo.PublishmentSystemID, string.Format(" and NodeIndexName in ({0}) ", TranslateUtils.ObjectCollectionToSqlInStringWithQuote(TranslateUtils.StringCollectionToArrayList(displayInfo.ChannelIndexNot))));
                    if (notInfos.Count > 0)
                        whereStr = string.Format(" and  NodeID not in ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithQuote(notInfos));
                }



                ArrayList nodeIDList = DataProvider.ViewsStatisticsDAO.GetMaxNode(pageInfo.PublishmentSystemID, uinfo.UserID, whereStr);//��ȡ��ǰ��¼�û�����������node

                if (nodeIDList.Count > 0)
                {
                    ArrayList nodeList = DataProvider.NodeDAO.GetNodeInfoArrayListByPublishmentSystemID(pageInfo.PublishmentSystemID, string.Format(" and NodeID in ({0}) ", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDList)));

                    /**���  δ��ɣ�displayInfo.ChannelIndexֻ�ܷ�һ��ֵ�������Ҫ���������Ҫ��дһЩ���Ƶķ���
                    //ArrayList ChannelIndexs = new ArrayList();
                    //ArrayList ChannelNames = new ArrayList();
                    //foreach (NodeInfo ninfo in nodeList)
                    //{
                    //    if (!string.IsNullOrEmpty(ninfo.NodeIndexName))
                    //        ChannelIndexs.Add(ninfo.NodeIndexName);
                    //    if (!string.IsNullOrEmpty(ninfo.NodeName))
                    //        ChannelNames.Add(ninfo.NodeName);
                    //}

                    //displayInfo.ChannelIndex = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(ChannelIndexs);
                    //displayInfo.ChannelName = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(ChannelNames);
                    */

                    NodeInfo noinfo = nodeList[0] as NodeInfo;

                    displayInfo.ChannelIndex = noinfo.NodeIndexName;
                    displayInfo.ChannelName = noinfo.NodeName;
                }
            }

            #endregion

            IEnumerable dataSource = StlIPushContents.GetDataSource(pageInfo, contextInfo, displayInfo);

            if (displayInfo.Layout == ELayout.None)
            {
                Repeater rptContents = new Repeater();

                rptContents.ItemTemplate = new RepeaterTemplate(displayInfo.ItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Content, contextInfo);
                if (!string.IsNullOrEmpty(displayInfo.HeaderTemplate))
                {
                    rptContents.HeaderTemplate = new SeparatorTemplate(displayInfo.HeaderTemplate);
                }
                if (!string.IsNullOrEmpty(displayInfo.FooterTemplate))
                {
                    rptContents.FooterTemplate = new SeparatorTemplate(displayInfo.FooterTemplate);
                }
                if (!string.IsNullOrEmpty(displayInfo.SeparatorTemplate))
                {
                    rptContents.SeparatorTemplate = new SeparatorTemplate(displayInfo.SeparatorTemplate);
                }
                if (!string.IsNullOrEmpty(displayInfo.AlternatingItemTemplate))
                {
                    rptContents.AlternatingItemTemplate = new RepeaterTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Content, contextInfo);
                }

                rptContents.DataSource = dataSource;
                rptContents.DataBind();

                if (rptContents.Items.Count > 0)
                {
                    parsedContent = ControlUtils.GetControlRenderHtml(rptContents);
                }
            }
            else
            {
                ParsedDataList pdlContents = new ParsedDataList();

                TemplateUtility.PutContentsDisplayInfoToMyDataList(pdlContents, displayInfo);

                pdlContents.ItemTemplate = new DataListTemplate(displayInfo.ItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Content, contextInfo);
                if (!string.IsNullOrEmpty(displayInfo.HeaderTemplate))
                {
                    pdlContents.HeaderTemplate = new SeparatorTemplate(displayInfo.HeaderTemplate);
                }
                if (!string.IsNullOrEmpty(displayInfo.FooterTemplate))
                {
                    pdlContents.FooterTemplate = new SeparatorTemplate(displayInfo.FooterTemplate);
                }
                if (!string.IsNullOrEmpty(displayInfo.SeparatorTemplate))
                {
                    pdlContents.SeparatorTemplate = new SeparatorTemplate(displayInfo.SeparatorTemplate);
                }
                if (!string.IsNullOrEmpty(displayInfo.AlternatingItemTemplate))
                {
                    pdlContents.AlternatingItemTemplate = new DataListTemplate(displayInfo.AlternatingItemTemplate, displayInfo.SelectedItems, displayInfo.SelectedValues, displayInfo.SeparatorRepeatTemplate, displayInfo.SeparatorRepeat, pageInfo, EContextType.Content, contextInfo);
                }

                pdlContents.DataSource = dataSource;
                pdlContents.DataKeyField = ContentAttribute.ID;
                pdlContents.DataBind();

                if (pdlContents.Items.Count > 0)
                {
                    parsedContent = ControlUtils.GetControlRenderHtml(pdlContents);
                }
            }

            contextInfo.TitleWordNum = titleWordNum;

            return parsedContent;
        }

    }
}
