using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.Oracle
{
    public class VoteOperationDAO : SiteServer.CMS.Provider.Data.SqlServer.VoteOperationDAO
    {
        protected override string ADOType
        {
            get
            {
                return SqlUtils.ORACLE;
            }
        }

        protected override EDatabaseType DataBaseType
        {
            get
            {
                return EDatabaseType.Oracle;
            }
        }
    }
}
