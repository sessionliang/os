using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using SiteServer.CMS.Core.Security;
using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public class SourceManager
	{
        public const int CaiJi = -2;        //采集
        public const int Preview = -99;     //预览
        public const int Default = 0;       //正常录入

        public static string GetSourceName(int sourceID)
        {
            if (sourceID == SourceManager.Default)
            {
                return "正常录入";
            }
            else if (sourceID == SourceManager.CaiJi)
            {
                return "系统采集";
            }
            else if (sourceID == SourceManager.Preview)
            {
                return "预览插入";
            }
            else if (sourceID > 0)
            {
                int publishmentSystemID = DataProvider.NodeDAO.GetPublishmentSystemID(sourceID);
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                if (publishmentSystemInfo != null)
                {
                    string nodeNames = NodeManager.GetNodeNameNavigation(publishmentSystemID, sourceID);
                    if (!string.IsNullOrEmpty(nodeNames))
                    {
                        return publishmentSystemInfo.PublishmentSystemName + "：" + nodeNames;
                    }
                    else
                    {
                        return publishmentSystemInfo.PublishmentSystemName;
                    }
                }
                else
                {
                    return "内容转移";
                }
            }

            return string.Empty;
        }
	}
}
