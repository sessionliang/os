using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using SiteServer.B2C.Model;
using System.Collections.Specialized;

using SiteServer.B2C.Core;
using System.Web;
using BaiRong.Core.Drawing;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.BackgroundPages
{
    public class ConsoleService : Page
    {
        public static string GetLoadingLocationsUrl()
        {
            return PageUtils.GetB2CUrl("console_service.aspx?type=GetLoadingLocations");
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request["type"];
            NameValueCollection retval = new NameValueCollection();
            string retString = string.Empty;

            if (type == "GetLoadingLocations")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                int parentID = TranslateUtils.ToInt(base.Request["parentID"]);
                string loadingType = base.Request["loadingType"];
                string additional = base.Request["additional"];
                retString = GetLoadingLocations(publishmentSystemID, parentID, loadingType, additional);
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

        public string GetLoadingLocations(int publishmentSystemID, int parentID, string loadingType, string additional)
        {
            ArrayList arraylist = new ArrayList();

            ELocationLoadingType eLoadingType = ELocationLoadingTypeUtils.GetEnumType(loadingType);

            ArrayList locationIDArrayList = DataProviderB2C.LocationDAO.GetIDArrayListByParentID(publishmentSystemID, parentID);
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(RuntimeUtils.DecryptStringByTranslate(additional));
            ArrayList allLocationIDArrayList = new ArrayList();
            if (!string.IsNullOrEmpty(nameValueCollection["LocationIDCollection"]))
            {
                allLocationIDArrayList = TranslateUtils.StringCollectionToIntArrayList(nameValueCollection["LocationIDCollection"]);
                nameValueCollection.Remove("LocationIDCollection");
                foreach (int locationID in locationIDArrayList)
                {
                    LocationInfo locationInfo = LocationManager.GetLocationInfo(publishmentSystemID, locationID);
                    if (locationInfo.ParentID != 0 || allLocationIDArrayList.Contains(locationID))
                    {
                        arraylist.Add(BackgroundLocation.GetLocationRowHtml(publishmentSystemID, locationInfo, eLoadingType, nameValueCollection));
                    }
                }
            }
            else
            {
                foreach (int locationID in locationIDArrayList)
                {
                    LocationInfo locationInfo = LocationManager.GetLocationInfo(publishmentSystemID, locationID);
                    arraylist.Add(BackgroundLocation.GetLocationRowHtml(publishmentSystemID, locationInfo, eLoadingType, nameValueCollection));
                }
            }

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
