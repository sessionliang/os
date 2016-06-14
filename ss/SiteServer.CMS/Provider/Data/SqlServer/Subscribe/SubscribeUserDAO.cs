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
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class SubscribeUserDAO : DataProviderBase, ISubscribeUserDAO
    {
        public string TableName
        {
            get
            {
                return "siteserver_SubscribeUser";
            }
        }

        private const string PARM_EMAIL = "@Email";
        private const string TABLE_NAME = "siteserver_SubscribeUser";

        private const string SQL_SELECT_SUBSCRIBEUSER = "SELECT * FROM " + TABLE_NAME + " WHERE Email = @Email";


        public int Insert(SubscribeUserInfo info)
        {
            int contentID = 0;

            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);
            contentID = this.ExecuteNonQuery(SQL_INSERT, parms);

            return contentID;
        }

        public void Update(SubscribeUserInfo info)
        {
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }



        public string GetSelectCommend(int publishmentSystemID, string subscribeName, string mobile, string email, string dateFrom, string dateTo, ETriState checkedState)
        {


            string dateString = string.Empty;
            if (!string.IsNullOrEmpty(dateFrom))
            {
                dateString = string.Format(" AND AddDate >= '{0}' ", dateFrom);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString = string.Format(" AND to_char(AddDate,'YYYY-MM-DD') >= '{0}' ", dateFrom);
                }
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                dateTo = DateUtils.GetDateString(TranslateUtils.ToDateTime(dateTo).AddDays(1));
                dateString += string.Format(" AND AddDate <= '{0}' ", dateTo);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString += string.Format(" AND to_char(AddDate,'YYYY-MM-DD') <= '{0}' ", dateTo);
                }
            }
            StringBuilder whereString = new StringBuilder("WHERE 1=1");


            whereString.AppendFormat(" and PublishmentSystemID = {0} ", publishmentSystemID);

            //判断是不是所有内容
            SubscribeInfo info = DataProvider.SubscribeDAO.GetContentInfo(publishmentSystemID, int.Parse(subscribeName));
            if (info.ParentID != 0)
                whereString.AppendFormat(" and  ','+SubscribeName+',' like '%,{0},%' ", subscribeName);

            whereString.Append(dateString);

            if (!string.IsNullOrEmpty(mobile))
            {
                whereString.AppendFormat("AND (Mobile LIKE '%{0}%')  ", mobile);
            }

            if (!string.IsNullOrEmpty(email))
            {
                whereString.AppendFormat("AND (Email LIKE '%{0}%')  ", email);
            }

            if (!ETriStateUtils.Equals(ETriState.All, checkedState))
            {
                whereString.AppendFormat("AND SubscribeStatu='{0}' ", checkedState);
            }



            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString.ToString());
        }



        public void Delete(int publishmentSystemID, ArrayList deleteIDArrayList)
        {
            #region 修改会员订阅的内容的订阅次数

            //循环会员信息
            ArrayList userList = this.GetSubscribeUserList(publishmentSystemID, deleteIDArrayList);
            string updateNumStr = "";
            foreach (SubscribeUserInfo info in userList)
            {
                if (!string.IsNullOrEmpty(info.SubscribeName))
                    updateNumStr = updateNumStr + DataProvider.SubscribeDAO.UpdateSubscribeNumStr(publishmentSystemID, info.SubscribeName, false) + ";";
            }

            if (updateNumStr != "")
                this.ExecuteNonQuery(updateNumStr);

            #endregion
            string sqlString = string.Format("DELETE FROM {0} WHERE SubscribeUserID IN ({1}) and PublishmentSystemID={2}", TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList), publishmentSystemID);
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(int publishmentSystemID, int subscribeID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE SubscribeUserID ={1} and PublishmentSystemID={2} ", TableName, subscribeID, publishmentSystemID);
            this.ExecuteNonQuery(sqlString);
        }

        public void DeleteByWhereStr(int publishmentSystemID, string whereStr)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE  PublishmentSystemID={2} {1}", TableName, whereStr, publishmentSystemID);
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(int publishmentSystemID, string subscribeIDs)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE SubscribeUserID in ({1}) and PublishmentSystemID={2} ", TableName, subscribeIDs, publishmentSystemID);
            this.ExecuteNonQuery(sqlString);
        }

        public void ChangeSubscribeStatu(int publishmentSystemID, int subscribeID, EBoolean subscribeStatu)
        {
            string sqlString = string.Format("update {0} set SubscribeStatu='{3}' WHERE SubscribeUserID ={1} and PublishmentSystemID={2} ", TableName, subscribeID, publishmentSystemID, subscribeStatu);
            this.ExecuteNonQuery(sqlString);
        }
        public void ChangeSubscribeStatu(int publishmentSystemID, ArrayList arrayList, EBoolean subscribeStatu)
        {
            string sqlString = string.Format("update {0} set SubscribeStatu='{3}' WHERE SubscribeUserID in ({1}) and PublishmentSystemID={2} ", TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(arrayList), publishmentSystemID, subscribeStatu);
            this.ExecuteNonQuery(sqlString);

            #region 修改会员订阅的内容的订阅次数

            //循环会员信息
            ArrayList userList = this.GetSubscribeUserList(publishmentSystemID, arrayList);
            string updateNumStr = "";
            foreach (SubscribeUserInfo info in userList)
            {
                if (!string.IsNullOrEmpty(info.SubscribeName))
                    updateNumStr = updateNumStr + DataProvider.SubscribeDAO.UpdateSubscribeNumStr(publishmentSystemID, info.SubscribeName, bool.Parse(subscribeStatu.ToString())) + ";";
            }

            if (updateNumStr != "")
                this.ExecuteNonQuery(updateNumStr);

            #endregion

        }

        public SubscribeUserInfo GetContentInfo(int contentID)
        {
            SubscribeUserInfo info = null;
            string SQL_WHERE = string.Format("WHERE SubscribeUserID = {0}", contentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new SubscribeUserInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public SubscribeUserInfo GetContentInfo(int publishmentSystemID, string email)
        {
            SubscribeUserInfo info = null;
            string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0} and Email ='{1}' ", publishmentSystemID, email);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new SubscribeUserInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        private DataSet GetDataSetByWhereString(string whereString)
        {
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString);
            return this.ExecuteDataset(SQL_SELECT);
        }


        private IEnumerable GetDataSourceByContentNumAndWhereString(int totalNum, string whereString, string orderByString)
        {
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, totalNum, SqlUtils.Asterisk, whereString, orderByString);
            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }


        public ArrayList GetSubscribeUserList(int publishmentSystemID, ArrayList arrayList)
        {
            ArrayList subscribeUserList = new ArrayList();

            string sqlString = string.Format("SELECT * FROM {0} WHERE PublishmentSystemID = {1} and SubscribeUserID in ({2}) ", TableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(arrayList));
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    SubscribeUserInfo info = new SubscribeUserInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    subscribeUserList.Add(info);
                }
                rdr.Close();
            }
            return subscribeUserList;
        }
        /// <summary>
        /// 获取更多条件下的会员列表
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="arrayList"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public ArrayList GetSubscribeUserList(int publishmentSystemID, ArrayList arrayList, string whereStr)
        {
            ArrayList subscribeUserList = new ArrayList();

            string sqlString = string.Format("SELECT * FROM {0} WHERE PublishmentSystemID = {1} and SubscribeUserID in ({2}) {3} ", TableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(arrayList), whereStr);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    SubscribeUserInfo info = new SubscribeUserInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    subscribeUserList.Add(info);
                }
                rdr.Close();
            }
            return subscribeUserList;
        }

        public string GetSortFieldName()
        {
            return "Taxis";
        }


        public bool IsExists(string email)
        {
            bool isExists = false;

            IDataParameter[] parms = new IDataParameter[]
            {
                this.GetParameter(PARM_EMAIL, EDataType.NVarChar, 50, email)
            };
            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SUBSCRIBEUSER, parms))
            {
                if (rdr.Read())
                {
                    isExists = true;
                }
                rdr.Close();
            }
            return isExists;
        }


        public void UpdatePushNum(int publishmentSystemID, int subscribeUserID)
        {
            string sqlString = string.Format("update {0} set PushNum=PushNum+1 WHERE SubscribeUserID in ({1}) and PublishmentSystemID={2} ", TableName, subscribeUserID, publishmentSystemID);
            this.ExecuteNonQuery(sqlString);
        }
        public void UpdatePushNumByEmail(int publishmentSystemID, string email)
        {
            string sqlString = string.Format("update {0} set PushNum=PushNum+1 WHERE Email = '{1}' and PublishmentSystemID={2} ", TableName, email, publishmentSystemID);
            this.ExecuteNonQuery(sqlString);
        }

        public ArrayList GetSubscribeUserList(int publishmentSystemID, EBoolean state)
        {
            ArrayList subscribeUserList = new ArrayList();

            string sqlString = string.Format("SELECT * FROM {0} WHERE PublishmentSystemID = {1} and SubscribeStatu = '{2}' ", TableName, publishmentSystemID, state);
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    SubscribeUserInfo info = new SubscribeUserInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    subscribeUserList.Add(info);
                }
                rdr.Close();
            }
            return subscribeUserList;
        }


        public void UpdateHasSubscribe(int publishmentSystemID, ArrayList subscribeIDs)
        {
            foreach (string subscribeID in subscribeIDs)
            {
                //获取订阅了这个内容的会员订阅信息
                StringBuilder whereString = new StringBuilder("WHERE 1=1");
                whereString.AppendFormat(" and PublishmentSystemID = {0} ", publishmentSystemID);
                whereString.AppendFormat(" and  ','+SubscribeName+',' like '%,{0},%' ", subscribeID);
                ArrayList subscribeUserList = new ArrayList();

                using (IDataReader rdr = this.ExecuteReader(BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString.ToString())))
                {
                    while (rdr.Read())
                    {
                        SubscribeUserInfo info = new SubscribeUserInfo();
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                        subscribeUserList.Add(info);
                    }
                    rdr.Close();
                }

                string updateStr = "";

                //循环会员订阅信息
                foreach (SubscribeUserInfo info in subscribeUserList)
                {
                    string subscribeName = "," + info.SubscribeName + ",";
                    //替换
                    subscribeName = subscribeName.Replace("," + subscribeID + ",", ",");
                    //截取前尾
                    subscribeName = subscribeName.TrimStart(',').TrimEnd(',');

                    updateStr += string.Format(" update {2} set SubscribeName ='{0}' where SubscribeUserID={1} and PublishmentSystemID={3}; ", subscribeName, info.SubscribeUserID, TABLE_NAME, publishmentSystemID);
                }
                if (updateStr != "")
                    this.ExecuteNonQuery(updateStr);
            }
        }

        /// <summary>
        /// 修改订阅了某些内容的某些会员的订阅内容字段
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="subscribeIDs"></param>
        /// <param name="userIDs"></param>
        /// <param name="state"></param>
        public void UpdateHasSubscribe(int publishmentSystemID, ArrayList subscribeIDs, ArrayList userIDs, EBoolean state)
        {
            //获取订阅了这个内容的会员订阅信息
            StringBuilder whereString = new StringBuilder("WHERE 1=1");
            whereString.AppendFormat(" and PublishmentSystemID = {0} ", publishmentSystemID);
            whereString.AppendFormat(" and  SubscribeUserID in ({0}) ", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(userIDs));
            ArrayList subscribeUserList = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString.ToString())))
            {
                while (rdr.Read())
                {
                    SubscribeUserInfo info = new SubscribeUserInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    subscribeUserList.Add(info);
                }
                rdr.Close();
            }

            string updateCStr = "";
            string updateStr = "";
            foreach (string subscribeID in subscribeIDs)
            {
                //循环会员订阅信息
                foreach (SubscribeUserInfo info in subscribeUserList)
                {
                    string subscribeName = "," + info.SubscribeName + ",";

                    if (EBooleanUtils.Equals(EBoolean.True, state))
                    {
                        subscribeName = info.SubscribeName;
                        subscribeName = subscribeName + ',' + subscribeID;
                        subscribeName = subscribeName.TrimStart(',').TrimEnd(',');
                    }
                    else
                    {
                        //替换
                        subscribeName = subscribeName.Replace("," + subscribeID + ",", ",").Replace("," + subscribeID + ",", ",").Replace("," + subscribeID + ",", ",");
                        //截取前尾
                        subscribeName = subscribeName.TrimStart(',').TrimEnd(',');
                    }

                    updateStr += string.Format(" update {2} set SubscribeName ='{0}' where SubscribeUserID={1} and PublishmentSystemID={3}; ", subscribeName, info.SubscribeUserID, TABLE_NAME, publishmentSystemID);

                    //修改内容下的数量
                    updateCStr = updateCStr + DataProvider.SubscribeDAO.UpdateSubscribeNumStr(publishmentSystemID, subscribeID, EBooleanUtils.Equals(EBoolean.True, state)) + " ; ";
                }
            }
            if (updateStr != "" || updateCStr != "")
                this.ExecuteNonQuery(updateStr + updateCStr);
            //将没有订阅内容的会员订阅状态修改为取消订阅
            DataProvider.SubscribeUserDAO.UpdateSubscribeStatu(publishmentSystemID, " AND (SubscribeName='' OR SubscribeName=' ' OR SubscribeName=',')", EBoolean.False);
        }

        /// <summary>
        /// 获取所有内容的会员数量
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public int GetCount(int publishmentSystemID, string whereStr)
        {
            int count = 0;

            string countSQL = string.Format(" select COUNT(*) as countNum from {0} where PublishmentSystemID = {1} {2}", TABLE_NAME, publishmentSystemID, whereStr);
            count = int.Parse(this.ExecuteScalar(countSQL).ToString());
            return count;
        }


        /// <summary>
        /// 更新某些条件下的会员状态 
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public void UpdateSubscribeStatu(int publishmentSystemID, string whereStr, EBoolean subscribeStatu)
        {
            string sql = string.Format(" update {0} set SubscribeStatu = '{3}' where PublishmentSystemID = {1} {2}", TABLE_NAME, publishmentSystemID, whereStr, subscribeStatu);
            this.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 清空某些条件下会员的订阅内容 
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        public void ClearSubscribeName(int publishmentSystemID, ArrayList userList, string whereStr)
        {
            //修改订阅内容下的数量 
            StringBuilder whereString = new StringBuilder("WHERE 1=1");
            whereString.AppendFormat(" and PublishmentSystemID = {0} ", publishmentSystemID);
            whereString.AppendFormat(" and  SubscribeUserID in ({0}) ", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(userList));
            ArrayList subscribeUserList = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString.ToString())))
            {
                while (rdr.Read())
                {
                    SubscribeUserInfo info = new SubscribeUserInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    subscribeUserList.Add(info);
                }
                rdr.Close();
            }

            string updateNumStr = "";

            //循环会员订阅信息
            foreach (SubscribeUserInfo info in subscribeUserList)
            {
                if (!string.IsNullOrEmpty(info.SubscribeName))
                    updateNumStr = updateNumStr + DataProvider.SubscribeDAO.UpdateSubscribeNumStr(publishmentSystemID, info.SubscribeName, false) + ";";
            }


            string sql = string.Format(" update {0} set SubscribeName = '' where PublishmentSystemID = {1} and SubscribeUserID in ({2}) {3}", TABLE_NAME, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(userList), whereStr);
            this.ExecuteNonQuery(updateNumStr + " ; " + sql);
        }

        /// <summary>
        /// 复杂的修改
        /// </summary>
        /// <param name="info"></param>
        /// <param name="oldSubID"></param>
        public void Update(SubscribeUserInfo info, string oldSubID)
        {
            string old = string.IsNullOrEmpty(oldSubID) ? " " : DataProvider.SubscribeDAO.UpdateSubscribeNumStr(info.PublishmentSystemID, oldSubID, false) + " ; ";

            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, TableName, out parms) + " ; ";

            string newStr = string.IsNullOrEmpty(info.SubscribeName) ? " " : DataProvider.SubscribeDAO.UpdateSubscribeNumStr(info.PublishmentSystemID, info.SubscribeName, true);

            this.ExecuteNonQuery(old + SQL_UPDATE + newStr, parms);
        }

        /// <summary>
        /// 给会员重新订阅内容
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="subscribeIDs"></param>
        /// <param name="userIDs"></param> 
        public void UpdateUserSubscribe(int publishmentSystemID, ArrayList subscribeIDs, ArrayList userIDs)
        {
            #region 给已经订阅了这些内容的会员减少订阅内容

            string oldUpdateStr = "";
            string oldUpdateCStr = "";

            foreach (string subscribeID in subscribeIDs)
            {
                //获取传来的会员订阅信息
                StringBuilder whereOldString = new StringBuilder("WHERE 1=1");
                whereOldString.AppendFormat(" and PublishmentSystemID = {0} ", publishmentSystemID);
                whereOldString.AppendFormat(" and  ','+SubscribeName+',' like '%,{0},%' ", subscribeID);
                whereOldString.AppendFormat(" and  SubscribeUserID in ({0}) ", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(userIDs));
                ArrayList subscribeOldUserList = new ArrayList();

                using (IDataReader rdr = this.ExecuteReader(BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereOldString.ToString())))
                {
                    while (rdr.Read())
                    {
                        SubscribeUserInfo info = new SubscribeUserInfo();
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                        subscribeOldUserList.Add(info);
                    }
                    rdr.Close();
                }
                //循环会员订阅信息
                foreach (SubscribeUserInfo info in subscribeOldUserList)
                {
                    string subscribeName = "," + info.SubscribeName + ",";

                    //替换
                    subscribeName = subscribeName.Replace("," + subscribeID + ",", ",").Replace("," + subscribeID + ",", ",").Replace("," + subscribeID + ",", ",");
                    //截取前尾
                    subscribeName = subscribeName.TrimStart(',').TrimEnd(',');

                    oldUpdateStr += string.Format(" update {2} set SubscribeName ='{0}' where SubscribeUserID={1} and PublishmentSystemID={3}; ", subscribeName, info.SubscribeUserID, TABLE_NAME, publishmentSystemID);
                    //减少内容下的数量
                    oldUpdateCStr = oldUpdateCStr + DataProvider.SubscribeDAO.UpdateSubscribeNumStr(publishmentSystemID, subscribeID, EBooleanUtils.Equals(EBoolean.True, false)) + " ; ";
                }
            }

            #endregion

            #region 给会员增加订阅内容
            //新的订阅内容
            string newSubscribeName = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(subscribeIDs);

            //获取传来的会员订阅信息
            StringBuilder whereString = new StringBuilder("WHERE 1=1");
            whereString.AppendFormat(" and PublishmentSystemID = {0} ", publishmentSystemID);
            whereString.AppendFormat(" and  SubscribeUserID in ({0}) ", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(userIDs));
            ArrayList subscribeUserList = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString.ToString())))
            {
                while (rdr.Read())
                {
                    SubscribeUserInfo info = new SubscribeUserInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    subscribeUserList.Add(info);
                }
                rdr.Close();
            }
            string updateCStr = "";
            string updateStr = "";
            //循环会员订阅信息
            foreach (SubscribeUserInfo info in subscribeUserList)
            {
                string subscribeName = info.SubscribeName;
                subscribeName = subscribeName + ',' + newSubscribeName;
                subscribeName = subscribeName.TrimStart(',').TrimEnd(',');

                updateStr += string.Format(" update {2} set SubscribeName ='{0}',SubscribeStatu='{3}' where SubscribeUserID={1} and PublishmentSystemID={4}; ", subscribeName, info.SubscribeUserID, TABLE_NAME, EBoolean.True, publishmentSystemID);
                //增加内容下的数量
                updateCStr = updateCStr + DataProvider.SubscribeDAO.UpdateSubscribeNumStr(publishmentSystemID, newSubscribeName, true) + " ; ";
            }
            string SQL = (oldUpdateStr != "" ? oldUpdateStr : "") + (oldUpdateCStr != "" ? oldUpdateCStr : "") + (updateStr != "" ? updateStr : "") + (updateCStr != "" ? updateCStr : "");
            if (SQL != "")
                this.ExecuteNonQuery(SQL);
            #endregion
        }
    }
}
