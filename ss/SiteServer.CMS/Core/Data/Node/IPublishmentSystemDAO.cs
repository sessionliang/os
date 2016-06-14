using System.Collections;
using System.Data;
using SiteServer.CMS.Model;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.CMS.Core
{
	public interface IPublishmentSystemDAO
	{
        string TableName { get; }
		void InsertWithTrans(PublishmentSystemInfo info, IDbTransaction trans);

        void Delete(int publishmentSystemID);

		void Update(PublishmentSystemInfo systemInfo);

        void UpdateParentPublishmentSystemIDToZero(int parentPublishmentSystemID);

        DictionaryEntryArrayList GetPublishmentSystemInfoDictionaryEntryArrayList();

        ArrayList GetLowerPublishmentSystemDirArrayListThatNotIsHeadquarters();

        ArrayList GetPublishmentSystemIDArrayListByParent(int parentPublishmentSystemID);

        string GetDatabasePublishmentSystemUrl(int publishmentSystemID);

		IEnumerable GetDataSource();

        int GetPublishmentSystemCount();

		int GetPublishmentSystemIDByIsHeadquarters();

		int GetPublishmentSystemIDByPublishmentSystemDir(string publishmentSystemDir);

        IEnumerable GetStlDataSource(string siteName, string directory, int startNum, int totalNum, string whereString, EScopeType scopeType, string orderByString, string since);

        bool UpdateTaxisToUp(int publishmentSystemID);

        bool UpdateTaxisToDown(int publishmentSystemID);

        string GetSelectCommand();
    }
}
