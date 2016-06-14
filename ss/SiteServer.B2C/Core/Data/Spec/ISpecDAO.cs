using System.Collections;
using System.Collections.Generic;
using SiteServer.B2C.Model;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.Core
{
	public interface ISpecDAO
	{
        int Insert(SpecInfo specInfo);

        void Update(SpecInfo specInfo);

        void Delete(int publishmentSystemID, int channelID, int specID);

        void DeleteAll(int publishmentSystemID, int channelID);

        bool IsExists(int publishmentSystemID, int channelID, string specName);

		IEnumerable GetDataSource(int publishmentSystemID, int channelID);

        Dictionary<int, SpecInfo> GetSpecInfoDictionary(int publishmentSystemID);

        IEnumerable GetStlDataSource(PublishmentSystemInfo publishmentSystemInfo, int channelID, int contentID, int startNum, int totalNum);

        SpecInfo GetSpecInfo(int publishmentSystemID, int channelID, string specName);

        List<SpecInfo> GetSpecInfoList(int publishmentSystemID, int channelID);

        List<int> GetSpecIDList(int publishmentSystemID, int channelID);

        string GetImportSpecName(int publishmentSystemID, int channelID, string specName);

        bool UpdateTaxisToUp(int publishmentSystemID, int channelID, int specID);

        bool UpdateTaxisToDown(int publishmentSystemID, int channelID, int specID);
	}
}
