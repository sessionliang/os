using System;
using System.Collections.Generic;
using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
    public interface ISMSMessageDAO
    {
        void Insert(SMSMessageInfo smsMessageInfo);

        void Update(SMSMessageInfo smsMessageInfo);

        void Delete(int id);

        void DeleteAll();

        SMSMessageInfo GetSMSMessageInfo(int id);

        string GetSelectCommand();
    }
}
