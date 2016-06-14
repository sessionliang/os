using System;
using System.Data;
using System.Collections;

using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
    public interface ITableStyleDAO
    {
        int Insert(TableStyleInfo styleInfo, ETableStyle tableStyle);

        int InsertWithTaxis(TableStyleInfo styleInfo, ETableStyle tableStyle);

        void InsertWithTransaction(TableStyleInfo info, ETableStyle tableStyle, IDbTransaction trans);

        void InsertStyleItems(ArrayList styleItems);

        void DeleteStyleItems(int tableStyleID);

        ArrayList GetStyleItemArrayList(int tableStyleID);

        void Update(TableStyleInfo info);

        void Delete(int relatedIdentity, string tableName, string attributeName);

        void Delete(ArrayList relatedIdentities, string tableName);

        ArrayList GetTableStyleInfoArrayList(ArrayList relatedIdentities, string tableName);

        bool IsExists(int relatedIdentity, string tableName, string attributeName);

        TableStyleInfo GetTableStyleInfo(int tableStyleID);

        TableStyleInfo GetTableStyleInfo(int relatedIdentity, string tableName, string attributeName);

        PairArrayList GetAllTableStyleInfoPairs();

        ArrayList GetTableStyleInfoWithItemsArrayList(string tableName, string attributeName);

        void TaxisUp(int tableStyleID);

        void TaxisDown(int tableStyleID);

        #region by 20160228  sofuny ���ܹ������ӵĹ������͵��ֶΣ����۹���

        /// <summary>
        /// ��ȡ���ܱ���ֶ�
        /// </summary>
        /// <param name="tableStyle"></param>
        /// <param name="relatedIdentity"></param>
        /// <returns></returns>
        ArrayList GetFunctionTableStyle(string tableName, int relatedIdentity, int publishmentSystemID, int contentID, string tableStyle);
        #endregion
    }
}
