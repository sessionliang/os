using System;
using System.Data;
using System.Collections;

using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using BaiRong.Core.Data;

namespace BaiRong.Provider.Data.Oracle
{
    public class UserBindingDAO : BaiRong.Provider.Data.SqlServer.UserBindingDAO
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