using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;


using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Provider.Data.Oracle
{
    public class GovInteractChannelDAO : SiteServer.CMS.Provider.Data.SqlServer.GovInteractChannelDAO
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
