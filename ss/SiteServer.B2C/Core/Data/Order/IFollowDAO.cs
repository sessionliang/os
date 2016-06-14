using System.Collections;
using SiteServer.B2C.Model;
using System.Collections.Generic;

namespace SiteServer.B2C.Core
{
	public interface IFollowDAO
	{
        int Insert(FollowInfo followInfo);

        void Delete(ArrayList deleteIDArrayList);

        FollowInfo GetFollowInfo(int followID);

        List<FollowInfo> GetFollowInfoList(string userName);

        int GetCount(string userName);

        string GetSelectString(string userName);

        string GetSortFieldName();

        List<FollowInfo> GetUserFollowsByPage(string userName, int pageIndex, int prePageNum);
    }
}
