using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface IGatherDatabaseRuleDAO
	{
		void Delete(string gatherRuleName, int publishmentSystemID);
		IEnumerable GetDataSource(int publishmentSystemID);
		ArrayList GetGatherRuleNameArrayList(int publishmentSystemID);
		GatherDatabaseRuleInfo GetGatherDatabaseRuleInfo(string gatherRuleName, int publishmentSystemID);
        int GetTableMatchID(string gatherRuleName, int publishmentSystemID);
		ArrayList GetGatherDatabaseRuleInfoArrayList(int publishmentSystemID);
		string GetImportGatherRuleName(int publishmentSystemID, string gatherRuleName);
        void Insert(GatherDatabaseRuleInfo gatherDatabaseRuleInfo);
		void UpdateLastGatherDate(string gatherRuleName, int publishmentSystemID);
        void Update(GatherDatabaseRuleInfo gatherDatabaseRuleInfo);

        void OpenAuto(int publishmentSystemID, ArrayList gatherRuleNameCollection);
        void CloseAuto(int publishmentSystemID, ArrayList gatherRuleNameCollection);
    }
}
