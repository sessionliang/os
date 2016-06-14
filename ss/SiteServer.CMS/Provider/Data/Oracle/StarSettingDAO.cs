using BaiRong.Core.Data;
using BaiRong.Model;

namespace SiteServer.CMS.Provider.Data.Oracle
{
    public class StarSettingDAO : SiteServer.CMS.Provider.Data.SqlServer.StarSettingDAO
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
