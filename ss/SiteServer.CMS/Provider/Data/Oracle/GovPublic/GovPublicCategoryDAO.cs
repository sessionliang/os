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
    public class GovPublicCategoryDAO : SiteServer.CMS.Provider.Data.SqlServer.GovPublicCategoryDAO
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
