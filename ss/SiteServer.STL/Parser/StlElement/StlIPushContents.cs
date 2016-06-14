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
    /// 培生智能推送
    /// 增加智能推送内容列表标签
    /// </summary>
    public class StlIPushContents
    {
        public const string ElementName = "stl:ipushcontents";//内容列表

        public const string Attribute_ChannelIndex = "channelindex";			//栏目索引
        public const string Attribute_ChannelName = "channelname";				//栏目名称
        public const string Attribute_ChannelIndexNot = "channelindexnot";	            //指定不显示的栏目索引
        public const string Attribute_UpLevel = "uplevel";						//上级栏目的级别
        public const string Attribute_TopLevel = "toplevel";					//从首页向下的栏目级别
        public const string Attribute_Scope = "scope";							//内容范围
        public const string Attribute_Group = "group";		                    //指定显示的内容组
        public const string Attribute_GroupNot = "groupnot";	                //指定不显示的内容组
        public const string Attribute_GroupChannel = "groupchannel";		    //指定显示的栏目组
        public const string Attribute_GroupChannelNot = "groupchannelnot";	    //指定不显示的栏目组
        public const string Attribute_GroupContent = "groupcontent";		    //指定显示的内容组
        public const string Attribute_GroupContentNot = "groupcontentnot";	    //指定不显示的内容组
        public const string Attribute_Tags = "tags";	                        //指定标签

        public const string Attribute_IsTop = "istop";                          //仅显示置顶内容
        public const string Attribute_IsRecommend = "isrecommend";              //仅显示推荐内容
        public const string Attribute_IsHot = "ishot";                          //仅显示热点内容
        public const string Attribute_IsColor = "iscolor";                      //仅显示醒目内容

        public const string Attribute_TotalNum = "totalnum";					//显示内容数目
        public const string Attribute_StartNum = "startnum";					//从第几条信息开始显示
        public const string Attribute_TitleWordNum = "titlewordnum";			//内容标题文字数量
        public const string Attribute_Order = "order";						    //排序
        public const string Attribute_IsImage = "isimage";					    //仅显示图片内容
        public const string Attribute_IsVideo = "isvideo";					    //仅显示视频内容
        public const string Attribute_IsFile = "isfile";                        //仅显示附件内容
        public const string Attribute_IsNoDup = "isnodup";                      //不显示重复标题的内容
        public const string Attribute_IsRelatedContents = "isrelatedcontents";  //显示相关内容列表
        public const string Attribute_Where = "where";                          //获取内容列表的条件判断
        public const string Attribute_IsDynamic = "isdynamic";                  //是否动态显示

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

                attributes.Add("cellpadding", "填充");
                attributes.Add("cellspacing", "间距");
                attributes.Add("class", "Css类");
                attributes.Add(Attribute_Columns, "列数");
                attributes.Add(Attribute_Direction, "方向");
                attributes.Add(Attribute_Layout, "指定列表布局方式");

                attributes.Add(Attribute_Height, "整体高度");
                attributes.Add(Attribute_Width, "整体宽度");
                attributes.Add(Attribute_Align, "整体对齐");
                attributes.Add(Attribute_ItemHeight, "项高度");
                attributes.Add(Attribute_ItemWidth, "项宽度");
                attributes.Add(Attribute_ItemAlign, "项水平对齐");
                attributes.Add(Attribute_ItemVerticalAlign, "项垂直对齐");
                attributes.Add(Attribute_ItemClass, "项Css类");

                attributes.Add(Attribute_TotalNum, "显示内容数目");
                attributes.Add(Attribute_StartNum, "从第几条信息开始显示");
                attributes.Add(Attribute_TitleWordNum, "内容标题文字数量");
                attributes.Add(Attribute_Order, "排序");
                attributes.Add(Attribute_IsImage, "仅显示图片内容");
                attributes.Add(Attribute_IsVideo, "仅显示视频内容");
                attributes.Add(Attribute_IsFile, "仅显示附件内容");
                attributes.Add(Attribute_IsNoDup, "不显示重复标题的内容");
                attributes.Add(Attribute_IsRelatedContents, "显示相关内容列表");
                attributes.Add(Attribute_Where, "获取内容列表的条件判断");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");

                attributes.Add(Attribute_ChannelIndex, "栏目索引");
                attributes.Add(Attribute_ChannelName, "栏目名称");
                attributes.Add(Attribute_ChannelIndexNot, "指定不显示的栏目索引");
                attributes.Add(Attribute_UpLevel, "上级栏目的级别");
                attributes.Add(Attribute_TopLevel, "从首页向下的栏目级别");
                attributes.Add(Attribute_Scope, "内容范围");
                attributes.Add(Attribute_GroupChannel, "指定显示的栏目组");
                attributes.Add(Attribute_GroupChannelNot, "指定不显示的栏目组");
                attributes.Add(Attribute_GroupContent, "指定显示的内容组");
                attributes.Add(Attribute_GroupContentNot, "指定不显示的内容组");
                attributes.Add(Attribute_Tags, "指定标签");
                attributes.Add(Attribute_IsTop, "仅显示置顶内容");
                attributes.Add(Attribute_IsRecommend, "仅显示推荐内容");
                attributes.Add(Attribute_IsHot, "仅显示热点内容");
                attributes.Add(Attribute_IsColor, "仅显示醒目内容");

                return attributes;
            }
        }

        public static Dictionary<string, string> BooleanAttributeList
        {
            get
            {
                Dictionary<string, string> attributes = new Dictionary<string, string>();

                attributes.Add(Attribute_IsTop, "仅显示置顶内容");
                attributes.Add(Attribute_IsRecommend, "仅显示推荐内容");
                attributes.Add(Attribute_IsHot, "仅显示热点内容");
                attributes.Add(Attribute_IsColor, "仅显示醒目内容");

                attributes.Add(Attribute_IsImage, "仅显示图片内容");
                attributes.Add(Attribute_IsVideo, "仅显示视频内容");
                attributes.Add(Attribute_IsFile, "仅显示附件内容");
                attributes.Add(Attribute_IsNoDup, "不显示重复标题的内容");
                attributes.Add(Attribute_IsRelatedContents, "显示相关内容列表");

                attributes.Add(Attribute_IsDynamic, "是否动态显示");

                return attributes;
            }
        }

        //对不能够翻页的“内容列表”（stl:contents）元素进行解析
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

            #region by 20151125 sofuny 培生智能推送
            //查询会员浏览量统计表，得出最大的nodeID集合，再得到校正的索引或名称，重新给displayInfo对象的ChannelIndex,ChannelName赋值，再得到集合
            UserInfo uinfo = UserManager.Current;//获取当前登录用户
            //uinfo = BaiRongDataProvider.UserDAO.GetUserInfo(1);//test user
            PublishmentSystemInfo pubinfo = PublishmentSystemManager.GetPublishmentSystemInfo(pageInfo.PublishmentSystemID);
            if (uinfo.UserID > 0 && pubinfo.Additional.IsIntelligentPushCount)//用户登录了且站点启用智能推送
            {

                //获取被排除的栏目
                ArrayList channelNots = new ArrayList();
                string whereStr = string.Empty;
                if (!string.IsNullOrEmpty(displayInfo.ChannelIndexNot))
                {
                    ArrayList notInfos = DataProvider.NodeDAO.GetNodeInfoArrayListByPublishmentSystemID(pageInfo.PublishmentSystemID, string.Format(" and NodeIndexName in ({0}) ", TranslateUtils.ObjectCollectionToSqlInStringWithQuote(TranslateUtils.StringCollectionToArrayList(displayInfo.ChannelIndexNot))));
                    if (notInfos.Count > 0)
                        whereStr = string.Format(" and  NodeID not in ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithQuote(notInfos));
                }



                ArrayList nodeIDList = DataProvider.ViewsStatisticsDAO.GetMaxNode(pageInfo.PublishmentSystemID, uinfo.UserID, whereStr);//获取当前登录用户最大浏览量的node

                if (nodeIDList.Count > 0)
                {
                    ArrayList nodeList = DataProvider.NodeDAO.GetNodeInfoArrayListByPublishmentSystemID(pageInfo.PublishmentSystemID, string.Format(" and NodeID in ({0}) ", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDList)));

                    /**多个  未完成，displayInfo.ChannelIndex只能放一个值，如果需要多个，则需要再写一些完善的方法
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
