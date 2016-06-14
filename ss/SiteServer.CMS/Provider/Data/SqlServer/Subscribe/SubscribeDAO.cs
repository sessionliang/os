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
    public class SubscribeDAO : TreeDAO, ISubscribeDAO
    {
        public SubscribeDAO()
            : base(TABLE_NAME)
        {

        }
        public const string TABLE_NAME = "siteserver_Subscribe";

        private const string PARM_ITEMNAME = "@ItemName";

        private const string SQL_SELECT_SUBSCRIBE = "SELECT * FROM " + TABLE_NAME + " WHERE ItemName = @ItemName";

        public string TableName
        {
            get
            {
                return "siteserver_Subscribe";
            }
        }

        public int Insert(SubscribeInfo info)
        {
            int contentID = 0;

            info.Taxis = this.GetMaxTaxis() + 1;
            info.ParentsCount = 1;

            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);
            contentID = this.ExecuteNonQuery(SQL_INSERT, parms);

            //修改上一级的子级个数
            SubscribeInfo pInfo = this.GetContentInfo(info.PublishmentSystemID, info.ParentID);
            pInfo.ChildrenCount = pInfo.ChildrenCount + 1;
            this.Update(pInfo);

            return contentID;
        }

        public void Update(SubscribeInfo info)
        {
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }
        public bool IsExists(string subscribeName)
        {
            bool isExists = false;

            IDataParameter[] parms = new IDataParameter[]
            {
                this.GetParameter(PARM_ITEMNAME, EDataType.NVarChar, 50, subscribeName)
            };
            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_SUBSCRIBE, parms))
            {
                if (rdr.Read())
                {
                    isExists = true;
                }
                rdr.Close();
            }
            return isExists;
        }

        /// <summary>
        /// 修改分类下表单数量
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="classifyID"></param>
        /// <param name="type"></param>
        public void UpdateSubscribeNum(int publishmentSystemID, string classifyID, int type)
        {
            string i = "ContentNum";
            if (type == 1)
                i = "ContentNum+1";
            else
                i = "ContentNum -1";

            string ContentNum_SQL = string.Format("update {0} set ContentNum={1},SubscribeNum={1} where PublishmentSystemID={2} and ItemID in ({3}) ", TABLE_NAME, i, publishmentSystemID, classifyID);
            this.ExecuteNonQuery(ContentNum_SQL);

        }


        /// <summary>
        /// 修改分类下表单数量
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="classifyID"></param>
        /// <param name="type"></param>
        public string UpdateSubscribeNumStr(int publishmentSystemID, string classifyID, bool type)
        {
            string i = "ContentNum";
            if (type)
                i = "ContentNum+1";
            else
                i = "ContentNum -1";

            string ContentNum_SQL = string.Format("update {0} set ContentNum={1},SubscribeNum={1} where PublishmentSystemID={2} and ItemID in ({3}) ", TABLE_NAME, i, publishmentSystemID, classifyID);

            return ContentNum_SQL;
        }


        public void Delete(int publishmentSystemID, ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ItemID IN ({1}) and PublishmentSystemID={2}", TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList), publishmentSystemID);
            this.ExecuteNonQuery(sqlString);

            DataProvider.SubscribeUserDAO.UpdateHasSubscribe(publishmentSystemID, deleteIDArrayList);
            //将没有订阅内容的会员信息删除(先不做删除，因为所有内容的统计原因)
            //DataProvider.SubscribeUserDAO.DeleteByWhereStr(publishmentSystemID, " AND (SubscribeName='' OR SubscribeName=' ' OR SubscribeName=',')");
            //将没有订阅内容的会员订阅状态修改为取消订阅
            DataProvider.SubscribeUserDAO.UpdateSubscribeStatu(publishmentSystemID, " AND (SubscribeName='' OR SubscribeName=' ' OR SubscribeName=',')", EBoolean.False);
            //修改全部内容 
            SubscribeInfo info = DataProvider.SubscribeDAO.GetDefaultInfo(publishmentSystemID);
            SubscribeInfo pInfo = this.GetContentInfo(publishmentSystemID, info.ItemID);
            pInfo.ChildrenCount = pInfo.ChildrenCount - deleteIDArrayList.Count;
            this.Update(pInfo);
        }

        public void Delete(int publishmentSystemID, int subscribeID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ItemID ={1} and PublishmentSystemID={2} ", TableName, subscribeID, publishmentSystemID);
            this.ExecuteNonQuery(sqlString);
            ArrayList list = new ArrayList();
            list.Add(subscribeID.ToString());
            DataProvider.SubscribeUserDAO.UpdateHasSubscribe(publishmentSystemID, list);
            //将没有订阅内容的会员信息删除
            //DataProvider.SubscribeUserDAO.DeleteByWhereStr(publishmentSystemID, " AND (SubscribeName='' OR SubscribeName=' ' OR SubscribeName=',')");
            //将没有订阅内容的会员订阅状态修改为取消订阅
            DataProvider.SubscribeUserDAO.UpdateSubscribeStatu(publishmentSystemID, " AND (SubscribeName='' OR SubscribeName=' ' OR SubscribeName=',')", EBoolean.False);
            //修改全部内容 
            SubscribeInfo info = DataProvider.SubscribeDAO.GetDefaultInfo(publishmentSystemID);
            SubscribeInfo pInfo = this.GetContentInfo(publishmentSystemID, info.ItemID);
            pInfo.ChildrenCount = pInfo.ChildrenCount - 1;
            this.Update(pInfo);
        }
        public SubscribeInfo GetContentInfo(int publishmentSystemID, int contentID)
        {
            SubscribeInfo info = null;
            string SQL_WHERE = string.Format("WHERE PublishmentSystemID={1} and  ItemID = {0}", contentID, publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new SubscribeInfo();

                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    // new SubscribeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), rdr.GetInt32(4), rdr.GetInt32(6), ESubscribeContentTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetValue(5).ToString(), rdr.GetDateTime(7), rdr.GetInt32(8), rdr.GetValue(9).ToString());
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }
        public SubscribeInfo GetContentInfo(int contentID)
        {
            SubscribeInfo info = null;
            string SQL_WHERE = string.Format("WHERE ItemID = {0}", contentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new SubscribeInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }



        public ArrayList GetInfoList(int publishmentSystemID, string whereStr)
        {
            ArrayList infoList = new ArrayList();

            string sqlString = string.Format("SELECT * FROM {0} WHERE PublishmentSystemID = @PublishmentSystemID {1} ORDER BY Taxis ", TableName, whereStr);

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter("@PublishmentSystemID", EDataType.NVarChar, 255,publishmentSystemID)
            };
            using (IDataReader rdr = this.ExecuteReader(sqlString, selectParms))
            {
                while (rdr.Read())
                {
                    SubscribeInfo info = new SubscribeInfo();
                    //new SubscribeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), rdr.GetInt32(3), rdr.GetInt32(4), ESubscribeContentTypeUtils.GetEnumType(rdr.GetValue(5).ToString()), rdr.GetValue(6).ToString(), rdr.GetDateTime(5), rdr.GetInt32(6), rdr.GetValue(7).ToString());

                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    infoList.Add(info);
                }
                rdr.Close();
            }
            return infoList;
        }


        private int GetMaxTaxis()
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM {0} ", TableName);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }



        public string GetAllString(int publishmentSystemID, string whereString)
        {
            string orderByString = ETaxisTypeUtils.GetInputContentOrderByString(ETaxisType.OrderByTaxisDesc);
            string where = string.Format("WHERE (PublishmentSystemID = {0} {1}) {2}", publishmentSystemID, whereString, orderByString);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, where);
        }

        public string GetSortFieldName()
        {
            return "Taxis";
        }

        public string GetName(int publishmentSystemID, string itemIDs)
        {
            string name = "";
            string sqlString = string.Format("SELECT   ItemID,ItemName FROM {0} WHERE ItemID in ({1}) and PublishmentSystemID ={2} ", TableName, itemIDs, publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    name = name + rdr[1].ToString() + ",";
                }
                rdr.Close();
            }
            if (name.Length > 0)
                return name.Substring(0, name.Length - 1);
            return "";
        }
        public bool GetValueByUserID(int publishmentSystemID, string userID, out string columnValue, out  string cabelValue)
        {
            columnValue = "";
            cabelValue = "";
            string sqlString = string.Format("select SubscribeValue,ContentType from  {0} where ItemID in ({1}) and PublishmentSystemID ={2} and Enabled='{3}' ", TableName, userID, publishmentSystemID, EBoolean.True);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    if (ESubscribeContentTypeUtils.Equals(ESubscribeContentType.Column, rdr[1].ToString()))
                        columnValue = columnValue + rdr[0].ToString() + ",";
                    if (ESubscribeContentTypeUtils.Equals(ESubscribeContentType.Label, rdr[1].ToString()))
                        cabelValue = cabelValue + rdr[0].ToString() + ",";
                }
                rdr.Close();
            }
            columnValue = columnValue.Replace(' ', ',');
            cabelValue = cabelValue.Replace(' ', ',');
            return true;
        }

        public int SetDefaultInfo(int publishmentSystemID)
        {
            int contentID = 0;
            if (this.GetItemIDArrayListByParentID(publishmentSystemID, 0).Count == 0)
            {
                SubscribeInfo info = new SubscribeInfo();
                info.PublishmentSystemID = publishmentSystemID;
                info.ItemName = "所有内容";
                info.ItemIndexName = "所有内容";

                info.Taxis = this.GetMaxTaxis() + 1;
                info.ParentsCount = 0;

                info.BeforeExecuteNonQuery();
                IDbDataParameter[] parms = null;
                string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);
                contentID = this.ExecuteNonQuery(SQL_INSERT, parms);
            }
            return contentID;
        }

        public SubscribeInfo GetDefaultInfo(int publishmentSystemID)
        {
            SubscribeInfo info = null;
            string SQL_WHERE = string.Format("WHERE PublishmentSystemID={0} and  ParentID = 0", publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new SubscribeInfo();

                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }


        /// <summary>
        /// 修改分类下表单数量
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="itemIDs"></param>
        /// <param name="type"></param>
        public void UpdateEnabled(int publishmentSystemID, ArrayList itemIDs, EBoolean type)
        {

            string ContentNum_SQL = string.Format("update {0} set Enabled='{1}' where PublishmentSystemID={2} and ItemID in ({3}) ", TABLE_NAME, type, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(itemIDs));
            this.ExecuteNonQuery(ContentNum_SQL);

        }
    }
}
