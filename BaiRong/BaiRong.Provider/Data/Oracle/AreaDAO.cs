using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data;
using BaiRong.Model;

namespace BaiRong.Provider.Data.Oracle
{
    public class AreaDAO : BaiRong.Provider.Data.SqlServer.AreaDAO
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
