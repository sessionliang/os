using System;
using System.Data;
using System.Collections;
using BaiRong.Model;
using BaiRong.Core.Data;

namespace BaiRong.Provider.Data.Oracle
{
    public class UserContactDAO : BaiRong.Provider.Data.SqlServer.UserContactDAO
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
