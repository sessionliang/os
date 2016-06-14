using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
	public interface IGatherFileRuleDAO
	{
        void Insert(GatherFileRuleInfo gatherFileRuleInfo);

        void UpdateLastGatherDate(string gatherRuleName, int publishmentSystemID);

        void Update(GatherFileRuleInfo gatherFileRuleInfo);

        void Delete(string gatherRuleName, int publishmentSystemID);

        GatherFileRuleInfo GetGatherFileRuleInfo(string gatherRuleName, int publishmentSystemID);

        string GetImportGatherRuleName(int publishmentSystemID, string gatherRuleName);

        IEnumerable GetDataSource(int publishmentSystemID);

        ArrayList GetGatherFileRuleInfoArrayList(int publishmentSystemID);

        ArrayList GetGatherRuleNameArrayList(int publishmentSystemID);

        void OpenAuto(int publishmentSystemID, ArrayList gatherRuleNameCollection);
        void CloseAuto(int publishmentSystemID, ArrayList gatherRuleNameCollection);
    }
}
