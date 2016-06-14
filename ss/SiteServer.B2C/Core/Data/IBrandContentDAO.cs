using System;
using System.Collections;
using System.Data;
using BaiRong.Model;
using SiteServer.B2C.Model;
using System.Web.UI.WebControls;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.Core
{
	public interface IBrandContentDAO
	{
        ArrayList GetCountArrayListUnChecked(bool isSystemAdministrator, int publishmentSystemID, ArrayList owningNodeIDArrayList, string tableName);

        GoodsContentInfo GetContentInfo(string tableName, int contentID);

        int GetCountCheckedImage(int publishmentSystemID, int nodeID);

        string GetStlWhereString(int publishmentSystemID, string group, string groupNot, string tags, AttributesInfo attributesInfo, string where);

        string GetSelectCommendByDownloads(string tableName, int publishmentSystemID);

        ListItemCollection GetListItemCollection(int publishmentSystemID, int nodeID, bool isTotal);

        ArrayList GetBrandContentArrayList(int publishmentSystemID, int nodeID, bool isTotal);

        int GetNodeID(int publishmentSystemID, int contentID);
	}
}
