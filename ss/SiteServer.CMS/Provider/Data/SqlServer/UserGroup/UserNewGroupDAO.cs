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
    public class UserNewGroupDAO : TreeDAO, SiteServer.CMS.Core.IUserNewGroupDAO
    {
        public UserNewGroupDAO()
            : base(TABLE_NAME)
        {

        }
        public const string TABLE_NAME = SiteServer.CMS.Model.UserNewGroupInfo.TableName;

        private const string PARM_ITEMNAME = "@ItemName";

        private const string SQL_SELECT_SUBSCRIBE = "SELECT * FROM " + TABLE_NAME + " WHERE ItemName = @ItemName";

        public string TableName
        {
            get
            {
                return SiteServer.CMS.Model.UserNewGroupInfo.TableName;
            }
        }

        public int Insert(SiteServer.CMS.Model.UserNewGroupInfo info)
        {
            int contentID = 0;

            info.Taxis = this.GetMaxTaxis() + 1;
            info.ParentsCount = 1;

            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);
            contentID = this.ExecuteNonQuery(SQL_INSERT, parms);

            //修改上一级的子级个数
            SiteServer.CMS.Model.UserNewGroupInfo pInfo = this.GetContentInfo(info.ParentID);
            pInfo.ChildrenCount = pInfo.ChildrenCount + 1;
            this.Update(pInfo);

            return contentID;
        }

        public void Update(SiteServer.CMS.Model.UserNewGroupInfo info)
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


        public void Delete(ArrayList deleteIDArrayList)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ItemID IN ({1})  ", TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);

            //修改全部内容 
            SiteServer.CMS.Model.UserNewGroupInfo info = GetDefaultInfo();
            SiteServer.CMS.Model.UserNewGroupInfo pInfo = this.GetContentInfo(info.ItemID);
            pInfo.ChildrenCount = pInfo.ChildrenCount - deleteIDArrayList.Count;
            this.Update(pInfo);
        }

        public void Delete(int id)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ItemID ={1}  ", TableName, id);
            this.ExecuteNonQuery(sqlString);
            //修改全部内容 
            SiteServer.CMS.Model.UserNewGroupInfo info = GetDefaultInfo();
            SiteServer.CMS.Model.UserNewGroupInfo pInfo = this.GetContentInfo(info.ItemID);
            pInfo.ChildrenCount = pInfo.ChildrenCount - 1;
            this.Update(pInfo);
        }

        public SiteServer.CMS.Model.UserNewGroupInfo GetContentInfo(int contentID)
        {
            SiteServer.CMS.Model.UserNewGroupInfo info = null;
            string SQL_WHERE = string.Format("WHERE ItemID = {0}", contentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new SiteServer.CMS.Model.UserNewGroupInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }



        public ArrayList GetInfoList(string whereStr)
        {
            ArrayList infoList = new ArrayList();

            string sqlString = string.Format("SELECT * FROM {0} WHERE {1} ORDER BY Taxis ", TableName, whereStr);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    SiteServer.CMS.Model.UserNewGroupInfo info = new SiteServer.CMS.Model.UserNewGroupInfo();
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



        public string GetAllString(string whereString)
        {
            string where = string.Format("WHERE ParentID!=0 {0} ", whereString);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, where);
        }

        public string GetSortFieldName()
        {
            return "Taxis";
        }

        public string GetName(string itemIDs)
        {
            string name = "";
            string sqlString = string.Format("SELECT   ItemID,ItemName FROM {0} WHERE ItemID in ({1})  ", TableName, itemIDs);

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

        public int SetDefaultInfo()
        {
            int contentID = 0;
            if (this.GetItemIDArrayListByParentID(0).Count == 0)
            {
                SiteServer.CMS.Model.UserNewGroupInfo info = new SiteServer.CMS.Model.UserNewGroupInfo();
                info.ItemName = "全部用户组";
                info.ItemIndexName = "全部用户组";

                info.Taxis = this.GetMaxTaxis() + 1;
                info.ParentsCount = 0;
                info.ParentID = 0;
                info.PublishmentSystemID = 0;

                info.BeforeExecuteNonQuery();
                IDbDataParameter[] parms = null;
                string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);
                contentID = this.ExecuteNonQuery(SQL_INSERT, parms);
            }
            return contentID;
        }



        public ArrayList GetItemIDArrayListByParentID(int parentID)
        {
            ArrayList list = new ArrayList();
            string sqlString = string.Empty;

            sqlString = string.Format(@"SELECT ItemID
FROM {1}
WHERE (ParentID = {0}) 
ORDER BY Taxis", parentID, TABLE_NAME);

            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int itemID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(itemID);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public SiteServer.CMS.Model.UserNewGroupInfo GetDefaultInfo()
        {
            SiteServer.CMS.Model.UserNewGroupInfo info = null;
            string SQL_WHERE = string.Format("WHERE   ParentID = 0");
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new SiteServer.CMS.Model.UserNewGroupInfo();

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
        public void UpdateEnabled(ArrayList itemIDs, EBoolean type)
        {

            string ContentNum_SQL = string.Format("update {0} set Enabled='{1}' where  ItemID in ({3}) ", TABLE_NAME, type, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(itemIDs));
            this.ExecuteNonQuery(ContentNum_SQL);

        }


        /// <summary>
        /// 修改分类内容数量
        /// </summary>
        /// <param name="publishmentSystemInfo"></param>
        /// <param name="itemID"></param>
        /// <param name="isRemoveCache"></param>
        public virtual void UpdateContentNum(int itemID, int contentNum)
        {

            SiteServer.CMS.Model.UserNewGroupInfo itemInfo = this.GetContentInfo(itemID);
            string sqlString = string.Empty;

            sqlString = string.Format("UPDATE {2} SET ContentNum = {0} WHERE (itemID = {1})", contentNum, itemID, TABLE_NAME);

            if (!string.IsNullOrEmpty(sqlString))
            {
                this.ExecuteNonQuery(sqlString);
            }
        }



        public string GetAllNewGroupString(string whereString)
        {
            string where = string.Format("select  [ItemID] ,[ItemName] ,[ItemIndexName] ,[ParentID] ,[ParentsPath] ,[ParentsCount] ,[ChildrenCount],(select COUNT(1) from dbo.bairong_Users where NewGroupID=s.itemid) as ContentNum,[ClassifyID]  ,[GroupType] ,[Enabled]  ,[IsLastItem] ,[Taxis] ,[AddDate] ,[UserName] ,[Description] ,[SetXML]  from dbo.bairong_UserNewGroup s WHERE ParentID!=0 {0} ", whereString, TableName);
            return where;
        }

    }
}
