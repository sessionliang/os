using System.Collections;
using BaiRong.Model;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface ISeoMetaDAO
	{
		void Insert(SeoMetaInfo info);
		void Update(SeoMetaInfo info);
		void SetDefault(int publishmentSystemID, int seoMetaID, bool defaultValue);
		void Delete(int seoMetaID);
		SeoMetaInfo GetSeoMetaInfo(int seoMetaID);
		SeoMetaInfo GetSeoMetaInfoBySeoMetaName(int publishmentSystemID, string seoMetaName);
		string GetImportSeoMetaName(int publishmentSystemID, string seoMetaName);
		int GetCount(int publishmentSystemID);
		SeoMetaInfo GetDefaultSeoMetaInfo(int publishmentSystemID);
		int GetDefaultSeoMetaID(int publishmentSystemID);
		IEnumerable GetDataSource(int publishmentSystemID);
		ArrayList GetSeoMetaInfoArrayListByPublishmentSystemID(int publishmentSystemID);
		string GetSeoMetaName(int seoMetaID);
		int GetSeoMetaIDBySeoMetaName(int publishmentSystemID, string seoMetaName);
		ArrayList GetSeoMetaNameArrayList(int publishmentSystemID);

		//∆•≈‰
        void InsertMatch(int publishmentSystemID, int nodeID, int seoMetaID, bool isChannel);
        void DeleteMatch(int publishmentSystemID, int nodeID, bool isChannel);
        int GetSeoMetaIDByNodeID(int nodeID, bool isChannel);

        ArrayList[] GetSeoMetaArrayLists(int publishmentSystemID);
	}
}
