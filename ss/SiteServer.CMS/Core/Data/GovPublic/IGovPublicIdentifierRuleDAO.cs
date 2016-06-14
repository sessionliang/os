using System.Collections;
using SiteServer.CMS.Model;
using BaiRong.Model;

namespace SiteServer.CMS.Core
{
	public interface IGovPublicIdentifierRuleDAO
	{
        void Insert(GovPublicIdentifierRuleInfo ruleInfo);

        void Update(GovPublicIdentifierRuleInfo ruleInfo);

        void Delete(int ruleID);

        GovPublicIdentifierRuleInfo GetIdentifierRuleInfo(int ruleID);

        int GetCount(int publishmentSystemID);

        ArrayList GetRuleInfoArrayList(int publishmentSystemID);

        bool UpdateTaxisToUp(int ruleID, int publishmentSystemID);

        bool UpdateTaxisToDown(int ruleID, int publishmentSystemID);
	}
}
