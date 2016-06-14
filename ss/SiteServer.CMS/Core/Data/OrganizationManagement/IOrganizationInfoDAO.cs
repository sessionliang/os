using SiteServer.CMS.Model;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

namespace SiteServer.CMS.Core
{
    public interface IOrganizationInfoDAO
    {
        string TableName
        {
            get;
        }

        int Insert(OrganizationInfo info);

        void Update(OrganizationInfo info);


        bool UpdateTaxisToUp(int classifyID, int ID);

        bool UpdateTaxisToDown(int classifyID, int  ID);

        void Delete(ArrayList deleteIDArrayList);

        void Delete(int ID);

        void DeleteByClassifyID(int classifyID);


        OrganizationInfo GetContentInfo(int  ID);

        ArrayList GetContentIDArrayListByUserName(string userName);


        string GetSortFieldName();

        /// <summary>
        /// ת������
        /// </summary>
        /// <param name="contentIDArrayList"></param>
        /// <param name="classifyID"></param>
        void TranslateContent(ArrayList contentIDArrayList, int classifyID);

        /// <summary>
        /// ת������
        /// </summary>
        /// <param name="contentIDArrayList"></param>
        /// <param name="classifyID"></param>
        /// <param name="areaID"></param>
        void TranslateContent(ArrayList contentIDArrayList, int cassifyID, int areaID);


        string GetSelectCommend(int publishmentSystemID, int classifyID, string areaID, string keyword);

        /// <summary>
        /// ��ѯ�������Ƿ����д˻���
        /// </summary>
        /// <param name="classifyID"></param>
        /// <param name="orgnID"></param>
        /// <param name="orgnName"></param>
        /// <returns></returns>
        bool IsExists(int classifyID, int orgnID, string orgnName);


        ArrayList GetInfoList(int classifyID, int areaPrentID, int areaID);

        /// <summary>
        /// ��ȡһ����Χ�ڵ�ѧϰ����
        /// </summary>
        /// <param name="classifyID"></param>
        /// <param name="minLat"></param>
        /// <param name="maxLat"></param>
        /// <param name="minLng"></param>
        /// <param name="maxLng"></param>
        /// <returns></returns>
        ArrayList GetInfoList(int classifyID, double minLat, double maxLat, double minLng, double maxLng);

        int GetCount();

        /// <summary>
        /// ��ҳ
        /// </summary>
        /// <param name="classifyID"></param>
        /// <param name="areaPrentID"></param>
        /// <param name="areaID"></param>
        /// <param name="pageNum"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        ArrayList GetInfoListByPage(int classifyID, int areaPrentID, int areaID, int pageNum, int pageIndex);

    }
}
