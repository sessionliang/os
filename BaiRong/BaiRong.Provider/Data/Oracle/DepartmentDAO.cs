using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;

namespace BaiRong.Provider.Data.Oracle
{
    public class DepartmentDAO : BaiRong.Provider.Data.SqlServer.DepartmentDAO
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
