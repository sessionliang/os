using System.Collections;
using System.Collections.Generic;
using SiteServer.B2C.Model;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.Core
{
	public interface ISpecComboDAO
	{
        int Insert(SpecComboInfo comboInfo);

        void Update(SpecComboInfo comboInfo);

        void Delete(int publishmentSystemID, int contentID, int specID);

        List<SpecComboInfo> GetSpecComboInfoList(int publishmentSystemID, int contentID, int specID);

        List<SpecComboInfo> GetSpecComboInfoList(string comboIDCollection);

        ArrayList GetSpecItemIDArrayList(int publishmentSystemID, int contentID, int specID);

        IEnumerable GetDataSource(int publishmentSystemID, int contentID, int specID);

        void GetContentSpec(int publishmentSystemID, int contentID, out List<int> specIDList, out List<int> specItemIDList);

        void GetSpec(string comboIDCollection, out List<int> specIDList, out List<int> specItemIDList);

        IEnumerable GetStlDataSource(PublishmentSystemInfo publishmentSystemInfo, int nodeID, int contentID, int specID, int startNum, int totalNum);
	}
}
