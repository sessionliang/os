using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using System.Collections.Specialized;

using System.Web;
using BaiRong.Core.Drawing;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

namespace SiteServer.WCM.BackgroundPages
{
    public class BackgroundServiceWCM : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request["type"];
            NameValueCollection retval = new NameValueCollection();
            string retString = string.Empty;

            if (type == "GetLoadingGovPublicCategories")
            {
                string classCode = base.Request["classCode"];
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                int parentID = TranslateUtils.ToInt(base.Request["parentID"]);
                string loadingType = base.Request["loadingType"];
                string additional = base.Request["additional"];
                retString = GetLoadingGovPublicCategories(classCode, publishmentSystemID, parentID, loadingType, additional);
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

        public string GetLoadingGovPublicCategories(string classCode, int publishmentSystemID, int parentID, string loadingType, string additional)
        {
            ArrayList arraylist = new ArrayList();

            EGovPublicCategoryLoadingType eLoadingType = EGovPublicCategoryLoadingTypeUtils.GetEnumType(loadingType);

            ArrayList categoryIDArrayList = DataProvider.GovPublicCategoryDAO.GetCategoryIDArrayListByParentID(classCode, publishmentSystemID, parentID);
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(RuntimeUtils.DecryptStringByTranslate(additional));

            foreach (int categoryID in categoryIDArrayList)
            {
                bool enabled = true;
                GovPublicCategoryInfo categoryInfo = DataProvider.GovPublicCategoryDAO.GetCategoryInfo(categoryID);

                arraylist.Add(BackgroundGovPublicCategory.GetCategoryRowHtml(categoryInfo, enabled, eLoadingType, nameValueCollection));
            }

            //arraylist.Reverse();

            StringBuilder builder = new StringBuilder();
            foreach (string html in arraylist)
            {
                builder.Append(html);
            }
            return builder.ToString();
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
