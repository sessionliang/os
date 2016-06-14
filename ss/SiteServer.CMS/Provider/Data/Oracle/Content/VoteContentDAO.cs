using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using BaiRong.Core.AuxiliaryTable;

using SiteServer.CMS.Core.Security;

namespace SiteServer.CMS.Provider.Data.Oracle
{
    public class VoteContentDAO : SiteServer.CMS.Provider.Data.SqlServer.VoteContentDAO
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
