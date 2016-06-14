using System;
using System.Data;
using System.Collections;
using System.Text;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;

namespace BaiRong.Provider.Data.Oracle
{
    public class TableCollectionDAO : BaiRong.Provider.Data.SqlServer.TableCollectionDAO
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
