using System.Collections;
using System.Data;
using System;
using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
    public interface ILogDAO
    {
        void Insert(LogInfo log);

        void Delete(ArrayList logIDArrayList);

        void Delete(int days, int counter);

        void DeleteAll();

        string GetSelectCommend();

        string GetSelectCommend(string userName, string keyword, string dateFrom, string dateTo);

        int GetCount();

        DateTime GetLastLoginDate(string userName);

        DateTime GetLastRemoveLogDate(string userName);

        /// <summary>
        /// ͳ�ƹ���ԱactionType�Ĳ�������
        /// ��������ͳ��
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="xType"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        Hashtable GetAdminLoginHashtableByDate(DateTime dateFrom, DateTime dateTo, string xType, string actionType);

        /// <summary>
        /// ͳ�ƹ���ԱactionType�Ĳ�������
        /// ���չ���Աͳ��
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="xType"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        Hashtable GetAdminLoginHashtableByName(DateTime dateFrom, DateTime dateTo, string actionType);
    }
}
