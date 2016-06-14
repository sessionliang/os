using System;
using System.Collections;
using System.Text;
using BaiRong.Core;
using System.Collections.Specialized;
using SiteServer.CMS.Core.Security;
using BaiRong.Model;
using SiteServer.CMS.Model;


namespace SiteServer.CMS.Core
{
    /// <summary>
    /// ≤ø√≈
    /// </summary>
    public class DeptLoading
    {


        public static string GetDeptRowHtml(PublishmentSystemInfo publishmentSystemInfo, DepartmentInfo info, bool enabled, EDepartmentLoadingType loadingType, NameValueCollection additional)
        {
            DepartmentTreeItem nodeTreeItem = DepartmentTreeItem.CreateInstance(info);
            string title = nodeTreeItem.GetItemHtml(loadingType, additional, true);

            string rowHtml = string.Empty;
            if (loadingType == EDepartmentLoadingType.List)
            {
                string contentAddNum = string.Empty;
                string contentUpdateNum = string.Empty;

                DateTime startDate = TranslateUtils.ToDateTime(additional["StartDate"]);
                DateTime endDate = TranslateUtils.ToDateTime(additional["EndDate"]);

                int num = 0;//DataProvider.ContentDAO.GetCountOfContentAdd(tableName, publishmentSystemInfo.PublishmentSystemID, info.NodeID, startDate, endDate, string.Empty);
                contentAddNum = (num == 0) ? "0" : string.Format("<strong>{0}</strong>", num);

                num = 0;// DataProvider.ContentDAO.GetCountOfContentUpdate(tableName, publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID, startDate, endDate, string.Empty);
                contentUpdateNum = (num == 0) ? "0" : string.Format("<strong>{0}</strong>", num);


                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
	<td>
		<nobr>{1}</nobr>
	</td>
	<td>
		{2}
	</td>
	<td>
		{3}
	</td> 
</tr>
", info.ParentsCount + 1, title, contentAddNum, contentUpdateNum);
            }
            return rowHtml;
        }

        public static string GetScript(PublishmentSystemInfo publishmentSystemInfo, ELoadingType loadingType, NameValueCollection additional)
        {
            return NodeTreeItem.GetScript(publishmentSystemInfo, loadingType, additional);
        }

        public static string GetScriptOnLoad(int publishmentSystemID, int currentNodeID)
        {
            if (currentNodeID != 0 && currentNodeID != publishmentSystemID)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, currentNodeID);
                if (nodeInfo != null)
                {
                    string path = string.Empty;
                    if (nodeInfo.ParentID == publishmentSystemID)
                    {
                        path = currentNodeID.ToString();
                    }
                    else
                    {
                        path = nodeInfo.ParentsPath.Substring(nodeInfo.ParentsPath.IndexOf(",") + 1) + "," + currentNodeID.ToString();
                    }
                    return NodeTreeItem.GetScriptOnLoad(path);
                }
            }
            return string.Empty;
        }

    }
}
