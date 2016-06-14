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
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundServiceB2C : Page
    {
        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request["type"];
            NameValueCollection retval = new NameValueCollection();
            string retString = string.Empty;

            if (type == "GetLoadingChannels")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                int parentID = TranslateUtils.ToInt(base.Request["parentID"]);
                string loadingType = base.Request["loadingType"];
                string additional = base.Request["additional"];
                retString = GetLoadingChannels(publishmentSystemID, parentID, loadingType, additional);
            }
            else if (type == "GetBrands")
            {
                int publishmentSystemID = TranslateUtils.ToInt(base.Request["publishmentSystemID"]);
                int nodeID = TranslateUtils.ToInt(base.Request["nodeID"]);
                retString = GetBrands(publishmentSystemID, nodeID);
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

        public string GetLoadingChannels(int publishmentSystemID, int parentID, string loadingType, string additional)
        {
            ArrayList arraylist = new ArrayList();

            ELoadingType eLoadingType = ELoadingTypeUtils.GetEnumType(loadingType);

            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(publishmentSystemID, parentID);
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);

            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(RuntimeUtils.DecryptStringByTranslate(additional));

            foreach (int nodeID in nodeIDArrayList)
            {
                bool enabled = true;
                enabled = AdminUtility.IsOwningNodeID(nodeID);
                if (!enabled)
                {
                    if (!AdminUtility.IsHasChildOwningNodeID(nodeID))
                    {
                        continue;
                    }
                }
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);

                arraylist.Add(ChannelLoadingB2C.GetChannelRowHtml(publishmentSystemInfo, nodeInfo, enabled, eLoadingType, nameValueCollection));
            }

            //arraylist.Reverse();

            StringBuilder builder = new StringBuilder();
            foreach (string html in arraylist)
            {
                builder.Append(html);
            }
            return builder.ToString();
        }

        public string GetBrands(int publishmentSystemID, int nodeID)
        {
            StringBuilder sb = new StringBuilder();
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);

            ListItemCollection itemCollection = DataProviderB2C.BrandContentDAO.GetListItemCollection(publishmentSystemID, nodeID, true);
            sb.Append("[");
            foreach (ListItem item in itemCollection)
            {
                sb.AppendFormat("{{\"text\":\"{0}\",\"value\":\"{1}\"}},", item.Text, item.Value);
            }
            if (itemCollection.Count > 0)
                sb.Length = sb.Length - 1;
            sb.Append("]");
            return sb.ToString();
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
