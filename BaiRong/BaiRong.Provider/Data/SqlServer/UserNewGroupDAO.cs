using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;

namespace BaiRong.Provider.Data.SqlServer
{
    public class UserNewGroupDAO : DataProviderBase, IUserNewGroupDAO
    {

        public const string TABLE_NAME = "bairong_UserNewGroup";


        public ArrayList GetInfoList(string whereStr)
        {
            ArrayList infoList = new ArrayList();

            string sqlString = string.Format("SELECT * FROM {0} WHERE {1} ORDER BY Taxis ", TABLE_NAME, whereStr);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    UserNewGroupInfo info = new UserNewGroupInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    infoList.Add(info);
                }
                rdr.Close();
            }
            return infoList;
        }




        /// <summary>
        /// 修改分类内容数量
        /// </summary>
        /// <param name="publishmentSystemInfo"></param>
        /// <param name="itemID"></param>
        /// <param name="isRemoveCache"></param>
        public virtual void UpdateContentNum(int itemID, int contentNum)
        {

            string sqlString = string.Empty;

            sqlString = string.Format("UPDATE {2} SET ContentNum = {0} WHERE (itemID = {1})", contentNum, itemID, TABLE_NAME);

            if (!string.IsNullOrEmpty(sqlString))
            {
                this.ExecuteNonQuery(sqlString);
            }
        }


        public UserNewGroupInfo GetInfo(int groupID)
        {
            UserNewGroupInfo info = null;

            string sqlString = string.Format("SELECT * FROM {0} WHERE ItemID={1} ORDER BY Taxis ", TABLE_NAME, groupID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    info = new UserNewGroupInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }
            return info;
        }
    }
}
