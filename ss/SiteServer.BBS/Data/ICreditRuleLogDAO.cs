using System;
using System.Data;
using System.Collections;
using SiteServer.BBS.Model;

namespace SiteServer.BBS
{
	public interface ICreditRuleLogDAO
	{
        void Insert(CreditRuleLogInfo logInfo);

        void Update(CreditRuleLogInfo logInfo);

        void Delete(int publishmentSystemID, string userName);

        ArrayList GetCreditRuleLogInfoArrayList(int publishmentSystemID, string userName);

        CreditRuleLogInfo GetCreditRuleLogInfo(int publishmentSystemID, string userName, ECreditRule ruleType);

        string GetSqlString(int publishmentSystemID, string userName);

        string GetSortFieldName();
	}
}
