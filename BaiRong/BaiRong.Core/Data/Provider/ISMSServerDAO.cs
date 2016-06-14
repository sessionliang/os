using BaiRong.Model;
using BaiRong.Model.Service;
using System.Collections;

namespace BaiRong.Core.Data.Provider
{
    public interface ISMSServerDAO
	{
        int Insert(SMSServerInfo info);

        void Update(SMSServerInfo info);

        void UpdateState(int SMSServerID, bool isEnabled);

        void Delete(int SMSServerID);

        void Delete(string SMSServerEName);

        SMSServerInfo GetSMSServerInfo(int SMSServerID);

        SMSServerInfo GetSMSServerInfo(string SMSServerEName);

        ArrayList GetSMSServerInfoArrayList();

        bool IsExists(string SMSServerEName);
	}
}
