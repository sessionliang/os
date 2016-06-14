using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;
using ECountType = SiteServer.WeiXin.Model.ECountType;

namespace SiteServer.WeiXin.Core
{
	public interface ICountDAO
	{
        int Insert(CountInfo countInfo);

        void AddCount(int publishmentSystemID, ECountType countType);

        int GetCount(int publishmentSystemID, ECountType countType);

        int GetCount(int publishmentSystemID, int year, int month, ECountType countType);

        Dictionary<int, int> GetDayCount(int publishmentSystemID, int year, int month, ECountType countType);

        List<CountInfo> GetCountInfoList(int publishmentSystemID);


	}
}
