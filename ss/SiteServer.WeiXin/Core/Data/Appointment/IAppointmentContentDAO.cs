using System.Collections;
using BaiRong.Model;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Core
{
    public interface IAppointmentContentDAO
    {

        int Insert(AppointmentContentInfo appointmentContentInfo);

        void Update(AppointmentContentInfo appointmentContentInfo);

        void Delete(int publishmentSystemID, int appointmentContentID);

        void Delete(int publishmentSystemID, List<int> appointmentContentIDList);

        void DeleteAll(int appointmentID);

        bool IsExist(int appointmentItemID, string cookieSN, string wxOpenID, string userName);

        AppointmentContentInfo GetContentInfo(int appointmentContentID);

        AppointmentContentInfo GetLatestContentInfo(int itemID, string cookieSN, string wxOpenID, string userName);

        List<AppointmentContentInfo> GetLatestContentInfoList(int appointmentID, string cookieSN, string wxOpenID, string userName);

        string GetSelectString(int publishmentSystemID, int appointmentID);

        List<AppointmentContentInfo> GetAppointmentContentInfoList(int publishmentSystemID, int appointmentID);

        List<AppointmentContentInfo> GetAppointmentContentInfoList(int publishmentSystemID, int appointmentID, int appointmentItemID);


    }
}
