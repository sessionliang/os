using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundService : Page
    {
        //_____________________CMS_____________________
        private const string TYPE_GetTitles = "GetTitles";
        private const string TYPE_RelatedField = "RelatedField";
        private const string TYPE_GetWordSpliter = "GetWordSpliter";
        private const string TYPE_GetDetection = "GetDetection";
        private const string TYPE_GetDetectionReplace = "GetDetectionReplace";
        private const string TYPE_GetTags = "GetTags";

        public static string GetRedirectUrl(string type)
        {
            return PageUtils.GetCMSUrl(string.Format("background_service.aspx?type={0}", type));
        }

        public static string GetTitlesUrl(int publishmentSystemID, int nodeID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_service.aspx?type={0}&publishmentSystemID={1}&nodeID={2}", TYPE_GetTitles, publishmentSystemID, nodeID));
        }

        public static string GetRelatedFieldUrlPrefix(PublishmentSystemInfo publishmentSystemInfo, int relatedFieldID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_service.aspx?type={0}&relatedFieldID={1}&parentID=", TYPE_RelatedField, relatedFieldID));
        }

        public static string GetWordSpliterUrl(int publishmentSystemID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_service.aspx?type={0}&publishmentSystemID={1}", TYPE_GetWordSpliter, publishmentSystemID));
        }

        public static string GetDetectionUrl(int publishmentSystemID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_service.aspx?type={0}&publishmentSystemID={1}", TYPE_GetDetection, publishmentSystemID));
        }

        public static string GetDetectionReplaceUrl(int publishmentSystemID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_service.aspx?type={0}&publishmentSystemID={1}", TYPE_GetDetectionReplace, publishmentSystemID));
        }

        public static string GetTagsUrl(int publishmentSystemID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_service.aspx?type={0}&publishmentSystemID={1}", TYPE_GetTags, publishmentSystemID));
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request["type"];
            NameValueCollection retval = new NameValueCollection();
            string retString = string.Empty;

            if (type == TYPE_GetTitles)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                int channelID = TranslateUtils.ToInt(base.Request["channelID"]);
                int nodeID = TranslateUtils.ToInt(base.Request["nodeID"]);
                if (channelID > 0)
                {
                    nodeID = channelID;
                }
                string title = base.Request["title"];
                string titles = GetTitles(publishmentSystemID, nodeID, title);

                Page.Response.Write(titles);
                Page.Response.End();

                return;
            }
            else if (type == TYPE_RelatedField)
            {
                string callback = base.Request["callback"];
                int relatedFieldID = TranslateUtils.ToInt(base.Request["relatedFieldID"]);
                int parentID = TranslateUtils.ToInt(base.Request["parentID"]);
                string jsonString = GetRelatedField(relatedFieldID, parentID);
                string call = callback + "(" + jsonString + ")";

                Page.Response.Write(call);
                Page.Response.End();

                return;
            }
            else if (type == TYPE_GetWordSpliter)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                string contents = base.Request.Form["content"];
                string tags = WordSpliter.GetKeywords(contents, AppManager.CMS.AppID, publishmentSystemID, 10);

                Page.Response.Write(tags);
                Page.Response.End();

                return;
            }
            else if (type == TYPE_GetDetection)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                string content = base.Request.Form["content"];
                ArrayList arraylist = DataProvider.KeywordDAO.GetKeywordArrayListByContent(content);
                string keywords = TranslateUtils.ObjectCollectionToString(arraylist);

                Page.Response.Write(keywords);
                Page.Response.End();
            }
            else if (type == TYPE_GetDetectionReplace)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                string content = base.Request.Form["content"];
                ArrayList arraylist = DataProvider.KeywordDAO.GetKeywordArrayListByContent(content);
                string keywords = string.Empty;

                List<KeywordInfo> list = DataProvider.KeywordDAO.GetKeywordInfoList(arraylist);
                foreach (var keywordInfo in list)
                {
                    keywords += keywordInfo.Keyword + "|" + keywordInfo.Alternative + ",";
                }
                keywords = keywords.TrimEnd(',');
                Page.Response.Write(keywords);
                Page.Response.End();
            }
            else if (type == TYPE_GetTags)
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                string tag = base.Request["tag"];
                string tags = GetTags(publishmentSystemID, tag);

                Page.Response.Write(tags);
                Page.Response.End();

                return;
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

        public string GetTitles(int publishmentSystemID, int nodeID, string title)
        {
            StringBuilder retval = new StringBuilder();

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
            ArrayList titleArrayList = BaiRongDataProvider.ContentDAO.GetValueArrayListByStartString(tableName, nodeID, ContentAttribute.Title, title, 10);
            if (titleArrayList.Count > 0)
            {
                foreach (string value in titleArrayList)
                {
                    retval.Append(value);
                    retval.Append("|");
                }
                retval.Length -= 1;
            }

            return retval.ToString();
        }

        public string GetRelatedField(int relatedFieldID, int parentID)
        {
            StringBuilder jsonString = new StringBuilder();

            jsonString.Append("[");

            ArrayList arraylist = DataProvider.RelatedFieldItemDAO.GetRelatedFieldItemInfoArrayList(relatedFieldID, parentID);
            if (arraylist.Count > 0)
            {
                foreach (RelatedFieldItemInfo itemInfo in arraylist)
                {
                    jsonString.AppendFormat(@"{{""id"":""{0}"",""name"":""{1}"",""value"":""{2}""}},", itemInfo.ID, StringUtils.ToJsString(itemInfo.ItemName), StringUtils.ToJsString(itemInfo.ItemValue));
                }
                jsonString.Length -= 1;
            }

            jsonString.Append("]");
            return jsonString.ToString();
        }

        public string GetTags(int publishmentSystemID, string tag)
        {
            StringBuilder retval = new StringBuilder();

            ArrayList tagArrayList = BaiRongDataProvider.TagDAO.GetTagArrayListByStartString(AppManager.CMS.AppID, publishmentSystemID, tag, 10);
            if (tagArrayList.Count > 0)
            {
                foreach (string value in tagArrayList)
                {
                    retval.Append(value);
                    retval.Append("|");
                }
                retval.Length -= 1;
            }

            return retval.ToString();
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
