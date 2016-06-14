using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface IAppointmentDAO
	{
        int Insert(AppointmentInfo appointmentInfo);

        void Update(AppointmentInfo appointmentInfo);

        void Delete(int publishmentSystemID, int appointmentID);

        void Delete(int publishmentSystemID, List<int> appointmentIDList);

        void AddUserCount(int appointmentID);

        void UpdateUserCount(int publishmentSystemID, Dictionary<int, int> appointmentIDWithCount);

        void AddPVCount(int appointmentID);

        AppointmentInfo GetAppointmentInfo(int appointmentID);

        List<AppointmentInfo> GetAppointmentInfoListByKeywordID(int publishmentSystemID, int keywordID);

        int GetFirstIDByKeywordID(int publishmentSystemID, int keywordID);

        string GetTitle(int appointmentID);

        string GetSelectString(int publishmentSystemID);

        List<AppointmentInfo> GetAppointmentInfoList(int publishmentSystemID);
        
	}
}
