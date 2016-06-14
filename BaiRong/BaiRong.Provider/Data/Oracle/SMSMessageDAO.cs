using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data;
using BaiRong.Model;

namespace BaiRong.Provider.Data.Oracle
{
    public class SMSMessageDAO : BaiRong.Provider.Data.SqlServer.SMSMessageDAO
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
