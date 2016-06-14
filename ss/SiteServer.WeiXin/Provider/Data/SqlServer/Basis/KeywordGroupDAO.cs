using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System.Collections.Generic;

namespace SiteServer.WeiXin.Provider.Data.SqlServer
{
    public class KeywordGroupDAO : DataProviderBase, IKeywordGroupDAO
	{
        private const string SQL_UPDATE = "UPDATE wx_KeywordGroup SET PublishmentSystemID = @PublishmentSystemID, GroupName = @GroupName, Taxis = @Taxis WHERE GroupID = @GroupID";

        private const string SQL_DELETE = "DELETE FROM wx_KeywordGroup WHERE GroupID = @GroupID";

        private const string SQL_SELECT = "SELECT GroupID, PublishmentSystemID, GroupName, Taxis FROM wx_KeywordGroup WHERE GroupID = @GroupID";

        private const string SQL_SELECT_ALL = "SELECT GroupID, PublishmentSystemID, GroupName, Taxis FROM wx_KeywordGroup ORDER BY Taxis";

        private const string PARM_GROUP_ID = "@GroupID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_GROUP_NAME = "@GroupName";
        private const string PARM_TAXIS = "@Taxis";

		public int Insert(KeywordGroupInfo groupInfo) 
		{
            int groupID = 0;

            string sqlString = "INSERT INTO wx_KeywordGroup (PublishmentSystemID, GroupName, Taxis) VALUES (@PublishmentSystemID, @GroupName, @Taxis)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO wx_KeywordGroup(GroupID, PublishmentSystemID, GroupName, Taxis) VALUES (wx_KeywordGroup_SEQ.NEXTVAL, @PublishmentSystemID, @GroupName, @Taxis)";
            }

            int taxis = this.GetMaxTaxis() + 1;
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, groupInfo.PublishmentSystemID),
                this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupInfo.GroupName),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        groupID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "wx_KeywordGroup");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return groupID;
		}

        public void Update(KeywordGroupInfo groupInfo) 
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, groupInfo.PublishmentSystemID),
                this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupInfo.GroupName),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, groupInfo.Taxis),
                this.GetParameter(PARM_GROUP_ID, EDataType.Integer, groupInfo.GroupID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
		}

		public void Delete(int groupID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_ID, EDataType.Integer, groupID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
		}

        public KeywordGroupInfo GetKeywordGroupInfo(int groupID)
		{
            KeywordGroupInfo groupInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_ID, EDataType.Integer, groupID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    groupInfo = new KeywordGroupInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetInt32(3));
                }
                rdr.Close();
            }

            return groupInfo;
		}

		public IEnumerable GetDataSource()
		{
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL);
			return enumerable;
		}

        public int GetCount(int parentID)
        {
            string sqlString = "SELECT COUNT(*) FROM wx_KeywordGroup WHERE ParentID = " + parentID;
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public List<KeywordGroupInfo> GetKeywordGroupInfoList()
        {
            List<KeywordGroupInfo> list = new List<KeywordGroupInfo>();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL))
            {
                while (rdr.Read())
                {
                    KeywordGroupInfo groupInfo = new KeywordGroupInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetInt32(3));
                    list.Add(groupInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public bool UpdateTaxisToUp(int parentID, int groupID)
        {
            string sqlString = string.Format("SELECT TOP 1 GroupID, Taxis FROM wx_KeywordGroup WHERE (Taxis > (SELECT Taxis FROM wx_KeywordGroup WHERE GroupID = {0} AND ParentID = {1})) AND ParentID = {1} ORDER BY Taxis", groupID, parentID);
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = rdr.GetInt32(0);
                    higherTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(groupID);

            if (higherID > 0)
            {
                SetTaxis(groupID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int parentID, int groupID)
        {
            string sqlString = string.Format("SELECT TOP 1 GroupID, Taxis FROM wx_KeywordGroup WHERE (Taxis < (SELECT Taxis FROM wx_KeywordGroup WHERE GroupID = {0} AND ParentID = {1})) AND ParentID = {1} ORDER BY Taxis DESC", groupID, parentID);
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = rdr.GetInt32(0);
                    lowerTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(groupID);

            if (lowerID > 0)
            {
                SetTaxis(groupID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis()
        {
            string sqlString = "SELECT MAX(Taxis) FROM wx_KeywordGroup";
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int groupID)
        {
            string sqlString = string.Format("SELECT Taxis FROM wx_KeywordGroup WHERE GroupID = {0}", groupID);
            int taxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    taxis = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(int groupID, int taxis)
        {
            string sqlString = string.Format("UPDATE wx_KeywordGroup SET Taxis = {0} WHERE GroupID = {1}", taxis, groupID);
            this.ExecuteNonQuery(sqlString);
        }
	}
}