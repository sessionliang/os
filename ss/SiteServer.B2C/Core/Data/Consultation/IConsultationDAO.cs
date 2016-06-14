using System.Collections;
using SiteServer.B2C.Model;
using System.Collections.Generic;
using System.Data;

namespace SiteServer.B2C.Core
{
    public interface IConsultationDAO
    {
        int Insert(ConsultationInfo ConsultationInfo);

        void Update(ConsultationInfo ConsultationInfo);

        void Delete(int ConsultationID);

        ConsultationInfo GetConsultationInfo(int ConsultationID);

        List<ConsultationInfo> GetConsultationInfoList(string userName, string keywords, int pageIndex, int prePageNum);

        int GetCountByUser(string userName, string keywords);

        int GetCount(string where);

        List<ConsultationInfo> GetConsultationInfoList(string where, int pageIndex, int prePageNum);
        void Delete(ArrayList arraylist);
        string GetSelectString(bool isReply, string keywords);
        string GetSelectString();
    }
}
