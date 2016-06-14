using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface IAppointmentItemDAO
    {
        int Insert(AppointmentItemInfo appointmentItemInfo);

        void Update(AppointmentItemInfo appointmentItemInfo);

        void UpdateAppointmentID(int publishmentSystemID, int appointmentID);

        void Delete(int publishmentSystemID, int appointmentItemID);

        void Delete(int publishmentSystemID, List<int> appointmentItemIDList);

        void DeleteAll(int appointmentID);

        void AddUserCount(int itemID);

        void UpdateUserCount(int publishmentSystemID, Dictionary<int, int> itemIDWithCount);

        AppointmentItemInfo GetItemInfo(int appointmentItemID);

        AppointmentItemInfo GetItemInfo(int publishmentSystemID, int appointmentID);

        int GetItemID(int publishmentSystemID, int appointmentID);

        List<AppointmentItemInfo> GetItemInfoList(int publishmentSystemID, int appointmentID);

        List<AppointmentItemInfo> GetItemInfoList(string wxOpenID, string userName);

        string GetTitle(int appointmentItemID);

        string GetSelectString(int publishmentSystemID, int appointmentID);

        List<AppointmentItemInfo> GetAppointmentItemInfoList(int publishmentSystemID);

    }
}
