using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System.Text.RegularExpressions;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlEvaluationInput
    {
        private StlEvaluationInput() { }
        public const string ElementName = "stl:evaluationinput"; 

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary(); 
                return attributes;
            }
        }

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
        {

            //判断当前内容的栏目是否开启评价
            NodeInfo nodeinfo = NodeManager.GetNodeInfo(contextInfo.PublishmentSystemInfo.PublishmentSystemID, contextInfo.ChannelID);
            if (!nodeinfo.Additional.IsUseEvaluation)
            {
                return StlParserUtility.GetStlErrorMessage(ElementName, new Exception("当前内容所属栏目未开启评价功能，评价标签无效！"));
            }

            string parsedContent = string.Empty;
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();

                bool isLoginFirst = true;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    //if (attributeName.Equals(Attribute_IsLoginFirst))
                    //{
                    //    isLoginFirst = TranslateUtils.ToBool(attr.Value);
                    //}
                }

                parsedContent = ParseImpl(pageInfo, contextInfo, node, isLoginFirst);
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

            return parsedContent;
        }

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo, XmlNode node, bool isLoginFirst)
        {
            string parsedContent = string.Empty;

            pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC);
            pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_USER);
            pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.B_CMS_EVALUATION);
            pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JQuery.B_Validate);

            string innerHtml = node.InnerXml;
            if (string.IsNullOrEmpty(innerHtml))
            {
                string filePath = PathUtils.GetSiteFilesPath("services/cms/components/evaluation/evaluationInputTemplate.html");
                innerHtml = FileUtils.ReadText(filePath, ECharset.utf_8);
                innerHtml = StlParserUtility.HtmlToXml(innerHtml);
            }

            StringBuilder builder = new StringBuilder(innerHtml);


             #region 内容评价字段

            //int publishmentSystemID = contextInfo.PublishmentSystemInfo.PublishmentSystemID;
            //int nodeID = contextInfo.ChannelID;
            //int contentID = contextInfo.ContentID;
            //ETableStyle tableStyle = ETableStyle.EvaluationContent;
            //ArrayList ftsList = DataProvider.FunctionTableStylesDAO.GetInfoList(publishmentSystemID, nodeID, contentID, ETableStyle.EvaluationContent.ToString(), "files");

            //ArrayList relatedIdentities = RelatedIdentities.GetRelatedIdentities(tableStyle, publishmentSystemID, nodeID);

            //ArrayList tableStyleList = BaiRongDataProvider.TableStyleDAO.GetFunctionTableStyle(EvaluationContentInfo.TableName, nodeID, publishmentSystemID, contentID, ETableStyle.EvaluationContent.ToString());

            //ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(tableStyle, EvaluationContentInfo.TableName, relatedIdentities);


            //ArrayList evaluationTslist = new ArrayList();
            //if (ftsList.Count > 0)//如果内容设置了评价字段
            //{
            //    foreach (TableStyleInfo info in tableStyleList)
            //    {
            //        if (ftsList.Contains(info.TableStyleID))
            //        {
            //            evaluationTslist.Add(info);
            //        }
            //    }
            //}
            ////未设置字段，则取栏目的评价字段
            //else
            //{
            //    foreach (TableStyleInfo info in tableStyleInfoArrayList)
            //    {
            //        if (info.IsVisible)
            //        {
            //            evaluationTslist.Add(info);
            //        }
            //    }
            //}

            //if (evaluationTslist.Count > 0)
            //{

            //    StringBuilder sbtablestyle = new StringBuilder();
            //    sbtablestyle.AppendFormat("<form id=\"evaluation_form\"> ");
            //    #region 拼接字段文本
            //    foreach (TableStyleInfo info in tableStyleList)
            //    {
            //        StringBuilder ss = new StringBuilder();
            //        if (info.IsSingleLine)
            //            ss.Append("<div class=\"evaluation_item\">");

            //        if (EStatisticsInputTypeUtils.Equals(EStatisticsInputType.Text, info.InputType.ToString()))
            //        {
            //            //if(isLoginFirst&&info.tr)
            //            //ss.AppendFormat("{2}：<input class=\"comment_input\" id=\"{0}\" name=\"{0}\" placeholder=\"{1}\" value=\"{3}\" width=\"{4}\" height=\"{5}\" />", info.AttributeName, info.HelpText, info.DisplayName, info.DefaultValue, info.Additional.Width, info.Additional.Height);
            //            //else
            //            ss.AppendFormat("{2}：<input class=\"comment_input\" id=\"{0}\" name=\"{0}\" placeholder=\"{1}\" value=\"{3}\" width=\"{4}\" height=\"{5}\" />", info.AttributeName, info.HelpText, info.DisplayName, info.DefaultValue, info.Additional.Width, info.Additional.Height);
            //        }
            //        else if (EStatisticsInputTypeUtils.Equals(EInputType.TextArea, info.InputType.ToString()))
            //        {
            //            string options = string.Empty;
            //            ArrayList items = info.StyleItems;
            //            foreach (TableStyleItemInfo item in items)
            //            {
            //                options += string.Format("<option value=\"{0}\" {2}>{1}</option>", item.ItemValue, item.ItemTitle, item.IsSelected ? "selected" : "");
            //            }

            //            ss.AppendFormat("{2}：<textarea class=\"comment_input\" id=\"{0}\" name=\"{0}\" placeholder=\"{1}\" value=\"{3}\" width=\"{4}\" height=\"{5}\" ></textarea>", info.AttributeName, info.HelpText, info.DisplayName, info.DefaultValue, info.Additional.Width, info.Additional.Height);
            //        }
            //        else if (EStatisticsInputTypeUtils.Equals(EStatisticsInputType.SelectOne, info.InputType.ToString()))
            //        {
            //            string options = string.Empty;
            //            ArrayList items = info.StyleItems;
            //            foreach (TableStyleItemInfo item in items)
            //            {
            //                options += string.Format("<option value=\"{0}\" {2}>{1}</option>", item.ItemValue, item.ItemTitle, item.IsSelected ? "selected" : "");
            //            }

            //            ss.AppendFormat("{2}：<select id=\"{0}\" name=\"{0}\" class=\"comment_select\"  width=\"{3}\" height=\"{4}\">{5}</select><span for=\"{0}\" class=\"common_span\">{1}</span>", info.AttributeName, info.HelpText, info.DisplayName, info.Additional.Width, info.Additional.Height, options);
            //        }
            //        else if (EStatisticsInputTypeUtils.Equals(EStatisticsInputType.Radio, info.InputType.ToString()))
            //        {
            //            string options = string.Empty;
            //            ArrayList items = info.StyleItems;
            //            if (items.Count > 0)
            //                for (int i = 0; i < items.Count; i++)
            //                {
            //                    TableStyleItemInfo item = items[i] as TableStyleItemInfo;
            //                    options += string.Format("<input type=\"radio\" id=\"{0}_{1}\" name=\"{0}\" value=\"{1}\" {3} /> <span for=\"{0}_{1}\">{2}</span>", info.AttributeName, item.ItemValue, item.ItemTitle, item.IsSelected ? "checked" : "");
            //                }

            //            ss.AppendFormat(" <span id=\"{0}_span\"> {1}：</span> {3} <span for=\"{0}_span\" class=\"common_span\">{2}</span>", info.AttributeName, info.DisplayName, info.HelpText, options);
            //        }
            //        else if (EStatisticsInputTypeUtils.Equals(EStatisticsInputType.CheckBox, info.InputType.ToString()))
            //        {
            //            string options = string.Empty;
            //            ArrayList items = info.StyleItems; //TableStyleManager.GetStyleItemArrayList(info.TableStyleID);
            //            if (items.Count > 0)
            //                for (int i = 0; i < items.Count; i++)
            //                {
            //                    TableStyleItemInfo item = items[i] as TableStyleItemInfo;
            //                    options += string.Format("<input type=\"checkbox\" id=\"{0}_{1}\" name=\"{0}\" value=\"{1}\" {3} /> <span for=\"{0}_{1}\">{2}</span>", info.AttributeName, item.ItemValue, item.ItemTitle, item.IsSelected ? "checked" : "");
            //                }

            //            ss.AppendFormat(" <span id=\"{0}_span\"> {1}：</span> {3} <span for=\"{0}_span\" class=\"common_span\">{2}</span>", info.AttributeName, info.DisplayName, info.HelpText, options);
            //        }
            //        if (info.IsSingleLine)
            //            ss.Append("</div>");
            //        sbtablestyle.Append(ss.ToString());
            //    }
            //    #endregion


            //    builder.Replace("<form id=\"evaluation_form\"> ", sbtablestyle.ToString());
            //}

             #endregion

            StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

            parsedContent = string.Format(@"
<script type=""text/html"" class=""evaluationController"">
    {0}
</script>
<script>function isLoginFirst(){{return {1};}}</script>
", builder, isLoginFirst ? "true" : "false");

            return parsedContent;
        }
    }
}
