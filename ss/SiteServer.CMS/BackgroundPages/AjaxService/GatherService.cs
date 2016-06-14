using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Collections;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Web.UI;

using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;
using System.Collections.Specialized;

namespace SiteServer.CMS.BackgroundPages.Service
{
    public class GatherService : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request.QueryString["type"];
            string userKeyPrefix = base.Request["userKeyPrefix"];
            NameValueCollection retval = new NameValueCollection();

            if (type == "GetCountArray")
            {
                retval = GetCountArray(userKeyPrefix);
            }
            else if (type == "Gather")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                string gatherRuleNameCollection = base.Request.Form["gatherRuleNameCollection"];
                retval = Gather(publishmentSystemID, gatherRuleNameCollection, userKeyPrefix);
            }
            else if (type == "GatherDatabase")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                string gatherRuleNameCollection = base.Request.Form["gatherRuleNameCollection"];
                retval = GatherDatabase(publishmentSystemID, gatherRuleNameCollection, userKeyPrefix);
            }
            else if (type == "GatherFile")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request.Form["publishmentSystemID"]);
                string gatherRuleNameCollection = base.Request.Form["gatherRuleNameCollection"];
                retval = GatherFile(publishmentSystemID, gatherRuleNameCollection, userKeyPrefix);
            }

            string jsonString = TranslateUtils.NameValueCollectionToJsonString(retval);
            Page.Response.Write(jsonString);
            Page.Response.End();
        }

        public NameValueCollection GetCountArray(string userKeyPrefix)//进度及显示
        {
            NameValueCollection retval = new NameValueCollection();
            if (CacheUtils.Get(userKeyPrefix + CACHE_TOTAL_COUNT) != null && CacheUtils.Get(userKeyPrefix + CACHE_CURRENT_COUNT) != null && CacheUtils.Get(userKeyPrefix + CACHE_MESSAGE) != null)
            {
                int totalCount = TranslateUtils.ToInt((string)CacheUtils.Get(userKeyPrefix + CACHE_TOTAL_COUNT));
                int currentCount = TranslateUtils.ToInt((string)CacheUtils.Get(userKeyPrefix + CACHE_CURRENT_COUNT));
                string message = (string)CacheUtils.Get(userKeyPrefix + CACHE_MESSAGE);
                retval = JsManager.AjaxService.GetCountArrayNameValueCollection(totalCount, currentCount, message);
            }
            return retval;
        }

        public const string CACHE_TOTAL_COUNT = "_TotalCount";
        public const string CACHE_CURRENT_COUNT = "_CurrentCount";
        public const string CACHE_MESSAGE = "_Message";

        #region 信息采集

        public NameValueCollection Gather(int publishmentSystemID, string gatherRuleNameCollection, string userKeyPrefix)
        {
            StringBuilder resultBuilder = new StringBuilder();
            StringBuilder errorBuilder = new StringBuilder();
            ArrayList gatherRuleNameArrayList = TranslateUtils.StringCollectionToArrayList(gatherRuleNameCollection);
            foreach (string gatherRuleName in gatherRuleNameArrayList)
            {
                GatherUtility.GatherWeb(publishmentSystemID, gatherRuleName, resultBuilder, errorBuilder, true, userKeyPrefix);
            }
            return JsManager.AjaxService.GetProgressTaskNameValueCollection(resultBuilder.ToString(), errorBuilder.ToString());
        }

        public NameValueCollection GatherDatabase(int publishmentSystemID, string gatherRuleNameCollection, string userKeyPrefix)
        {
            StringBuilder resultBuilder = new StringBuilder();
            StringBuilder errorBuilder = new StringBuilder();
            ArrayList gatherRuleNameArrayList = TranslateUtils.StringCollectionToArrayList(gatherRuleNameCollection);
            foreach (string gatherRuleName in gatherRuleNameArrayList)
            {
                GatherUtility.GatherDatabase(publishmentSystemID, gatherRuleName, resultBuilder, errorBuilder, true, userKeyPrefix);
            }
            return JsManager.AjaxService.GetProgressTaskNameValueCollection(resultBuilder.ToString(), errorBuilder.ToString());
        }

        public NameValueCollection GatherFile(int publishmentSystemID, string gatherRuleNameCollection, string userKeyPrefix)
        {
            StringBuilder resultBuilder = new StringBuilder();
            StringBuilder errorBuilder = new StringBuilder();
            ArrayList gatherRuleNameArrayList = TranslateUtils.StringCollectionToArrayList(gatherRuleNameCollection);
            foreach (string gatherRuleName in gatherRuleNameArrayList)
            {
                GatherUtility.GatherFile(publishmentSystemID, gatherRuleName, resultBuilder, errorBuilder, true, userKeyPrefix);
            }
            return JsManager.AjaxService.GetProgressTaskNameValueCollection(resultBuilder.ToString(), errorBuilder.ToString());
        }

        #endregion
    }
}
