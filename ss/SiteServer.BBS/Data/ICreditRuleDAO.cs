using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using System.Collections;

namespace SiteServer.BBS
{
    public interface ICreditRuleDAO
    {
        void Insert(int publishmentSystemID, CreditRuleInfo creditRuleInfo);

        void Update(int publishmentSystemID, CreditRuleInfo creditRuleInfo);

        void Delete(int publishmentSystemID, int id);

        CreditRuleInfo GetCreditRuleInfo(int id);

        Hashtable GetCreditRuleInfoHashtable(int publishmentSystemID);
    }
}