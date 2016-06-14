using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;

namespace BaiRong.Provider.Data.Oracle
{
	public class TableStyleDAO : BaiRong.Provider.Data.SqlServer.TableStyleDAO
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
