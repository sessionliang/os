using System.Collections;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Core
{
    public interface IGatherRuleDAO
    {
        void Delete(string gatherRuleName, int publishmentSystemID);
        IEnumerable GetDataSource(int publishmentSystemID);
        ArrayList GetGatherRuleNameArrayList(int publishmentSystemID);
        GatherRuleInfo GetGatherRuleInfo(string gatherRuleName, int publishmentSystemID);
        ArrayList GetGatherRuleInfoArrayList(int publishmentSystemID);
        string GetImportGatherRuleName(int publishmentSystemID, string gatherRuleName);
        void Insert(GatherRuleInfo gatherRuleInfo);
        void UpdateLastGatherDate(string gatherRuleName, int publishmentSystemID);
        void Update(GatherRuleInfo gatherRuleInfo);

        void OpenAuto(int publishmentSystemID, ArrayList gatherRuleNameCollection);
        void CloseAuto(int publishmentSystemID, ArrayList gatherRuleNameCollection);
    }
}
