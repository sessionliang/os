using System;
using System.Collections;
using System.Data;
using BaiRong.Model;
using SiteServer.CMS.Model;
using System.Collections.Generic;

namespace SiteServer.CMS.Core
{
	public interface IBackgroundContentDAO
	{
        ArrayList GetCountArrayListUnChecked(bool isSystemAdministrator, List<int> publishmentSystemIDList, ArrayList owningNodeIDArrayList, string tableName);

        int GetCountCheckedImage(int publishmentSystemID, int nodeID);

        string GetStlWhereString(PublishmentSystemInfo publishmentSystemInfo, string tableName, string group, string groupNot, string tags, bool isImageExists, bool isImage, bool isVideoExists, bool isVideo, bool isFileExists, bool isFile, bool isTopExists, bool isTop, bool isRecommendExists, bool isRecommend, bool isHotExists, bool isHot, bool isColorExists, bool isColor, string where);

        string GetSelectCommendByDownloads(string tableName, int publishmentSystemID);
	}
}
