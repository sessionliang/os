using System.Collections;
using System.Collections.Generic;
using BaiRong.Model;
using SiteServer.B2C.Model;

namespace SiteServer.B2C.Core
{
	public interface ISpecItemDAO
	{
		int Insert(int publishmentSystemID, SpecItemInfo itemInfo);

        void Update(int publishmentSystemID, SpecItemInfo itemInfo);

		void Delete(int publishmentSystemID, int itemID);

        //SpecItemInfo GetSpecItemInfo(int itemID);

        IEnumerable GetDataSource(int specID);

        List<SpecItemInfo> GetSpecItemInfoList(int specID);

        //List<int> GetItemIDList(int specID, ETriState isDefaultState);

        bool UpdateTaxisToUp(int specID, int itemID);

        bool UpdateTaxisToDown(int specID, int itemID);

        Dictionary<int, SpecItemInfo> GetSpecItemInfoDictionary(int publishmentSystemID);
	}
}
