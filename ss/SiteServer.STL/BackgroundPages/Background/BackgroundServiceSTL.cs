using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.STL.Parser.TemplateDesign;
using SiteServer.CMS.Model;
using SiteServer.CMS.Services;
using SiteServer.STL.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;

namespace SiteServer.STL.BackgroundPages
{
    public class BackgroundServiceSTL : Page
    {
        public const string TYPE_StlTemplate = "StlTemplate";
        public const string TYPE_GetLoadingTemplates = "GetLoadingTemplates";
        public const string TYPE_AjaxUrlFSO = "AjaxUrlFSO";
        public const string TYPE_AjaxUrlFSONext = "AjaxUrlFSONext";

        public static string GetRedirectUrl(string type)
        {
            return PageUtils.GetSTLUrl(string.Format("background_serviceSTL.aspx?type={0}", type));
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request["type"];
            NameValueCollection retval = new NameValueCollection();
            string retString = string.Empty;

            if (type == TYPE_StlTemplate)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                int templateID = TranslateUtils.ToInt(base.Request["templateID"]);
                string includeUrl = base.Request["includeUrl"];
                string operation = base.Request["operation"];
                retval = TemplateDesignOperation.Operate(publishmentSystemID, templateID, includeUrl, operation, base.Request.Form);
            }
            else if (type == TYPE_GetLoadingTemplates)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                string templateType = base.Request["templateType"];
                retString = GetLoadingTemplates(publishmentSystemID, templateType);
            }
            else if (type == TYPE_AjaxUrlFSO)
            {
                retval = AjaxUrlFSO();
            }
            else if (type == TYPE_AjaxUrlFSONext)
            {
                retval = AjaxUrlNext();
            }

            if (!string.IsNullOrEmpty(retString))
            {
                Page.Response.Write(retString);
                Page.Response.End();
            }
            else
            {
                string jsonString = TranslateUtils.NameValueCollectionToJsonString(retval);
                Page.Response.Write(jsonString);
                Page.Response.End();
            }
        }

        private NameValueCollection AjaxUrlNext()
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            AjaxUrlInfo ajaxUrlInfo = AjaxUrlManager.FetchAjaxUrlInfo();
            if (ajaxUrlInfo != null)
            {
                nameValueCollection["isNext"] = "true";
                nameValueCollection["ajaxUrl"] = ajaxUrlInfo.AjaxUrl;
                nameValueCollection["parameters"] = ajaxUrlInfo.Parameters;
            }
            else
            {
                nameValueCollection["isNext"] = "false";
            }
            return nameValueCollection;
        }

        public string GetLoadingTemplates(int publishmentSystemID, string templateType)
        {
            ArrayList arraylist = new ArrayList();

            ETemplateType eTemplateType = ETemplateTypeUtils.GetEnumType(templateType);

            ArrayList templateInfoArrayList = DataProvider.TemplateDAO.GetTemplateInfoArrayListByType(publishmentSystemID, eTemplateType);

            foreach (TemplateInfo templateInfo in templateInfoArrayList)
            {
                string designHTML = string.Empty;
                if (templateInfo.TemplateType == ETemplateType.IndexPageTemplate || templateInfo.TemplateType == ETemplateType.FileTemplate)
                {
                    string designUrl = PageUtility.DynamicPage.GetDesignUrl(PageUtility.DynamicPage.GetRedirectUrl(templateInfo.PublishmentSystemID, 0, 0, templateInfo.TemplateType == ETemplateType.FileTemplate ? templateInfo.TemplateID : 0, 0));
                    designHTML = string.Format(@"<a href=""{0}"" rel=""tooltip"" title=""可视化编辑"" target=""_blank""><img align=""absmiddle"" border=""0"" src=""../../SiteFiles/bairong/icons/menu/template.gif""></a>", designUrl);
                }
                else
                {
                    designHTML = string.Format(@"<a href=""javascript:;"" rel=""tooltip"" title=""可视化编辑"" onclick=""parent.management.{0}""><img align=""absmiddle"" border=""0"" src=""../../SiteFiles/bairong/icons/menu/template.gif""></a>", Modal.StlTemplate.StlTemplateSelect.GetOpenLayerString(templateInfo.PublishmentSystemID, templateInfo.TemplateType, templateInfo.TemplateID, false));
                }

                string templateAddUrl = PageUtils.GetSTLUrl(string.Format(@"background_templateAdd.aspx?PublishmentSystemID={0}&amp;TemplateID={1}&amp;TemplateType={2}", publishmentSystemID, templateInfo.TemplateID, templateType));
                arraylist.Add(string.Format(@"
<tr treeitemlevel=""3"">
	<td align=""left"" nowrap="""">
		<img align=""absmiddle"" src=""../../SiteFiles/bairong/icons/tree/empty.gif""><img align=""absmiddle"" src=""../../SiteFiles/bairong/icons/tree/empty.gif"">&nbsp;&nbsp;{0}&nbsp;<a href=""{1}"" onclick=""fontWeightLink(this)"" target=""management"">{2}</a>
	</td>
</tr>
", designHTML, templateAddUrl, templateInfo.TemplateName));
            }

            StringBuilder builder = new StringBuilder();
            foreach (string html in arraylist)
            {
                builder.Append(html);
            }
            return builder.ToString();
        }

        public NameValueCollection AjaxUrlFSO()
        {
            string method = base.Request["method"];
            int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
            FileSystemObject FSO = new FileSystemObject(publishmentSystemID);

            if (method == "CreateContent")
            {
                int channelID = TranslateUtils.ToInt(base.Request["channelID"]);
                int contentID = TranslateUtils.ToInt(base.Request["contentID"]);

                if (contentID > 0)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, channelID);
                    ETableStyle tableStyle = NodeManager.GetTableStyle(FSO.PublishmentSystemInfo, nodeInfo);
                    string tableName = NodeManager.GetTableName(FSO.PublishmentSystemInfo, nodeInfo);

                    FSO.CreateContent(tableStyle, tableName, channelID, contentID);
                }
            }
            else if (method == "CreateChannel")
            {
                int channelID = TranslateUtils.ToInt(base.Request["channelID"]);

                if (channelID > 0)
                {
                    FSO.CreateChannel(channelID);
                }
            }
            else if (method == "CreateIndex")
            {
                if (publishmentSystemID > 0)
                {
                    FSO.CreateIndex();
                }
            }
            else if (method == "CreateImmediately")
            {
                EChangedType changeType = EChangedTypeUtils.GetEnumType(base.Request["changeType"]);
                ETemplateType templateType = ETemplateTypeUtils.GetEnumType(base.Request["templateType"]);
                int channelID = TranslateUtils.ToInt(base.Request["channelID"]);
                int contentID = TranslateUtils.ToInt(base.Request["contentID"]);
                int fileTemplateID = TranslateUtils.ToInt(base.Request["fileTemplateID"]);

                FSO.CreateImmediately(changeType, templateType, channelID, contentID, fileTemplateID);
            }

            NameValueCollection nameValueCollection = new NameValueCollection();
            AjaxUrlInfo ajaxUrlInfo = AjaxUrlManager.FetchAjaxUrlInfo();
            if (ajaxUrlInfo != null)
            {
                nameValueCollection["isNext"] = "true";
                nameValueCollection["ajaxUrl"] = ajaxUrlInfo.AjaxUrl;
                nameValueCollection["parameters"] = ajaxUrlInfo.Parameters;
            }
            else
            {
                nameValueCollection["isNext"] = "false";
            }

            return nameValueCollection;
        }

        #region Helper

        private void ResponseText(string text)
        {
            base.Response.Clear();
            base.Response.Write(text);
            base.Response.End();
        }

        /// <summary>
        /// 向页面输出xml内容
        /// </summary>
        /// <param name="xmlnode">xml内容</param>
        private void ResponseXML(StringBuilder xmlnode)
        {
            base.Response.Clear();
            base.Response.ContentType = "Text/XML";
            base.Response.Expires = 0;

            base.Response.Cache.SetNoStore();
            base.Response.Write(xmlnode.ToString());
            base.Response.End();
        }

        /// <summary>
        /// 输出json内容
        /// </summary>
        /// <param name="json"></param>
        private void ResponseJSON(string json)
        {
            base.Response.Clear();
            base.Response.ContentType = "application/json";
            base.Response.Expires = 0;

            base.Response.Cache.SetNoStore();
            base.Response.Write(json);
            base.Response.End();
        }
        #endregion

    }
}
