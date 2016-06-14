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
    public class StlTrialApplyInput
    {
        private StlTrialApplyInput() { }
        public const string ElementName = "stl:trialapplyinput";

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
            //判断当前内容的栏目是否开启试用
            NodeInfo nodeinfo = NodeManager.GetNodeInfo(contextInfo.PublishmentSystemInfo.PublishmentSystemID, contextInfo.ChannelID);
            if (!nodeinfo.Additional.IsUseTrial)
            {
                return StlParserUtility.GetStlErrorMessage(ElementName, new Exception("当前内容所属栏目未开启评价功能，试用申请标签无效！"));
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

            bool cantrial = true;
            bool cantrialb = true;
            bool cantriale = true;
            if (!string.IsNullOrEmpty(contextInfo.ContentInfo.GetExtendedAttribute(ContentAttribute.Trial_BeginDate)) || !string.IsNullOrEmpty(contextInfo.ContentInfo.GetExtendedAttribute(ContentAttribute.Trial_EndDate)))
            {

                try
                {
                    DateTime bt = Convert.ToDateTime(contextInfo.ContentInfo.GetExtendedAttribute(ContentAttribute.Trial_BeginDate));

                    if (DateTime.Now < bt)
                        cantrialb = false;

                }
                catch
                {
                    cantrial = true;
                }

                try
                {
                    DateTime et = Convert.ToDateTime(contextInfo.ContentInfo.GetExtendedAttribute(ContentAttribute.Trial_EndDate));

                    if (DateTime.Now > et)
                        cantriale = true;
                }
                catch
                {
                    cantrial = true;
                }
            }
            else
                cantrial = true;


            if (cantrial && cantrialb && cantriale)
            {

                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_BASIC);
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.A_Platform_USER);
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JsServiceComponents.B_CMS_TTIALAPPLY);
                pageInfo.AddPageEndScriptsIfNotExists(PageInfo.JQuery.B_Validate);

                string innerHtml = node.InnerXml;
                if (string.IsNullOrEmpty(innerHtml))
                {
                    string filePath = PathUtils.GetSiteFilesPath("services/cms/components/trial/trialApplyInputTemplate.html");
                    innerHtml = FileUtils.ReadText(filePath, ECharset.utf_8);
                    innerHtml = StlParserUtility.HtmlToXml(innerHtml);
                }

                StringBuilder builder = new StringBuilder(innerHtml);



                StlParserManager.ParseInnerContent(builder, pageInfo, contextInfo);

                parsedContent = string.Format(@"
<script type=""text/html"" class=""trialApplyController"">
    {0}
</script>
<script>function isLoginFirst(){{return {1};}}</script>
", builder, isLoginFirst ? "true" : "false");

            }
            else
            {
                if (!cantrialb)
                    parsedContent = "<div>试用活动未开始！</div>";
                if (!cantriale)
                    parsedContent = "<div>试用活动已结束！</div>";
            }
            return parsedContent;
        }
    }
}
