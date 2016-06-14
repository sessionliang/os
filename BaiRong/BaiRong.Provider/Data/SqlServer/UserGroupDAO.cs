using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;


namespace BaiRong.Provider.Data.SqlServer
{
    public class UserGroupDAO : DataProviderBase, IUserGroupDAO
    {
        private const string SQL_UPDATE = "UPDATE bairong_UserGroup SET GroupName = @GroupName, GroupType = @GroupType, Stars = @Stars, Color = @Color, ExtendValues = @ExtendValues WHERE GroupID = @GroupID";

        private const string SQL_DELETE = "DELETE FROM bairong_UserGroup WHERE GroupID = @GroupID";

        private const string SQL_SELECT_BY_ID = "SELECT GroupID, GroupSN, GroupName, GroupType, CreditsFrom, CreditsTo, Stars, Color, ExtendValues FROM bairong_UserGroup WHERE GroupID = @GroupID";

        private const string SQL_SELECT_ALL = "SELECT GroupID, GroupSN, GroupName, GroupType, CreditsFrom, CreditsTo, Stars, Color, ExtendValues FROM bairong_UserGroup WHERE GroupSN = @GroupSN ORDER BY GroupType DESC, CreditsTo";

        private const string SQL_SELECT_ALL_ID = "SELECT GroupID FROM bairong_UserGroup WHERE GroupSN = @GroupSN ORDER BY CreditsTo, GroupID";

        private const string PARM_GROUP_ID = "@GroupID";
        private const string PARM_GROUP_SN = "@GroupSN";
        private const string PARM_GROUP_NAME = "@GroupName";
        private const string PARM_GROUP_TYPE = "@GroupType";
        private const string PARM_CREDITS_FROM = "@CreditsFrom";
        private const string PARM_CREDITS_TO = "@CreditsTo";
        private const string PARM_STARS = "@Stars";
        private const string PARM_COLOR = "@Color";
        private const string PARM_EXTEND_VALUES = "@ExtendValues";

        public int Insert(UserGroupInfo groupInfo)
        {
            int groupID = 0;

            string sqlString = "INSERT INTO bairong_UserGroup (GroupSN, GroupName, GroupType, Stars, Color, ExtendValues) VALUES (@GroupSN, @GroupName, @GroupType, @Stars, @Color, @ExtendValues)";

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupInfo.GroupSN),
                this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 50, groupInfo.GroupName),
                this.GetParameter(PARM_GROUP_TYPE, EDataType.VarChar, 50, EUserGroupTypeUtils.GetValue(groupInfo.GroupType)),
                this.GetParameter(PARM_STARS, EDataType.Integer, groupInfo.Stars),
				this.GetParameter(PARM_COLOR, EDataType.VarChar, 10, groupInfo.Color),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, groupInfo.Additional.ToString())
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, insertParms);

                        groupID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bairong_UserGroup");

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            if (groupInfo.GroupType == EUserGroupType.Credits)
            {
                int theGroupID = this.GetGroupIDByCreditsFrom(groupInfo.GroupSN, groupInfo.CreditsFrom, true);
                if (theGroupID > 0)
                {
                    UserGroupInfo theGroupInfo = this.GetUserGroupInfo(theGroupID);

                    if (theGroupInfo.CreditsFrom == groupInfo.CreditsFrom)
                    {
                        this.UpdateCredits(groupInfo.GroupSN, theGroupID, true, groupInfo.CreditsTo);
                    }
                    else//大于
                    {
                        if (groupInfo.CreditsTo == theGroupInfo.CreditsTo)
                        {
                            this.UpdateCredits(groupInfo.GroupSN, theGroupID, false, groupInfo.CreditsFrom);
                        }
                        else//小于
                        {
                            this.UpdateCredits(groupInfo.GroupSN, theGroupID, false, groupInfo.CreditsFrom);
                            int theGroupID2 = this.GetGroupIDByCreditsFrom(groupInfo.GroupSN, groupInfo.CreditsTo, false);
                            if (theGroupID2 > 0)
                            {
                                this.UpdateCredits(groupInfo.GroupSN, theGroupID2, true, groupInfo.CreditsTo);
                            }
                        }
                    }
                }
                this.UpdateCreditsRange(groupInfo.GroupSN, groupID, groupInfo.CreditsFrom, groupInfo.CreditsTo);
            }

            UserGroupManager.ClearCache(groupInfo.GroupSN);

            return groupID;
        }

        public void UpdateWithCredits(string groupSN, UserGroupInfo groupInfo, int oldCreditsFrom, int oldCreditsTo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 50, groupInfo.GroupName),
                this.GetParameter(PARM_GROUP_TYPE, EDataType.VarChar, 50, EUserGroupTypeUtils.GetValue(groupInfo.GroupType)),
                this.GetParameter(PARM_STARS, EDataType.Integer, groupInfo.Stars),
				this.GetParameter(PARM_COLOR, EDataType.VarChar, 10, groupInfo.Color),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, groupInfo.Additional.ToString()),
				this.GetParameter(PARM_GROUP_ID, EDataType.Integer, groupInfo.GroupID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, updateParms);

            if (groupInfo.CreditsFrom > oldCreditsFrom)
            {
                int groupID = this.GetGroupIDByCredits(groupInfo.GroupSN, true, groupInfo.CreditsFrom);
                if (groupID > 0)
                {
                    this.UpdateCredits(groupSN, groupID, false, groupInfo.CreditsFrom);
                }
                this.UpdateCredits(groupSN, groupInfo.GroupID, true, groupInfo.CreditsFrom);
            }

            if (groupInfo.CreditsTo < oldCreditsTo)
            {
                int groupID = this.GetGroupIDByCredits(groupInfo.GroupSN, false, groupInfo.CreditsTo);
                if (groupID > 0)
                {
                    this.UpdateCredits(groupSN, groupID, true, groupInfo.CreditsTo);
                }
                this.UpdateCredits(groupSN, groupInfo.GroupID, false, groupInfo.CreditsTo);
            }

            UserGroupManager.ClearCache(groupSN);
        }

        public void UpdateWithoutCredits(string groupSN, UserGroupInfo userGroupInfo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 50, userGroupInfo.GroupName),
                this.GetParameter(PARM_GROUP_TYPE, EDataType.VarChar, 50, EUserGroupTypeUtils.GetValue(userGroupInfo.GroupType)),
                this.GetParameter(PARM_STARS, EDataType.Integer, userGroupInfo.Stars),
				this.GetParameter(PARM_COLOR, EDataType.VarChar, 10, userGroupInfo.Color),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, userGroupInfo.Additional.ToString()),
				this.GetParameter(PARM_GROUP_ID, EDataType.Integer, userGroupInfo.GroupID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, updateParms);

            UserGroupManager.ClearCache(groupSN);
        }

        private void UpdateCredits(string groupSN, int groupID, bool isCreditsFrom, int credits)
        {
            if (isCreditsFrom)
            {
                string sqlString = "UPDATE bairong_UserGroup SET CreditsFrom = @CreditsFrom WHERE GroupID = @GroupID";

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter(PARM_CREDITS_FROM, EDataType.Integer, credits),
				    this.GetParameter(PARM_GROUP_ID, EDataType.Integer, groupID)
			    };

                this.ExecuteNonQuery(sqlString, parms);
            }
            else
            {
                string sqlString = "UPDATE bairong_UserGroup SET CreditsTo = @CreditsTo WHERE GroupID = @GroupID";

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter(PARM_CREDITS_TO, EDataType.Integer, credits),
				    this.GetParameter(PARM_GROUP_ID, EDataType.Integer, groupID)
			    };

                this.ExecuteNonQuery(sqlString, parms);
            }

            UserGroupManager.ClearCache(groupSN);
        }

        private void UpdateCreditsRange(string groupSN, int groupID, int creditsFrom, int creditsTo)
        {
            string sqlString = "UPDATE bairong_UserGroup SET CreditsFrom = @CreditsFrom, CreditsTo = @CreditsTo WHERE GroupID = @GroupID";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_CREDITS_FROM, EDataType.Integer, creditsFrom),
				this.GetParameter(PARM_CREDITS_TO, EDataType.Integer, creditsTo),
				this.GetParameter(PARM_GROUP_ID, EDataType.Integer, groupID)
			};

            this.ExecuteNonQuery(sqlString, parms);

            UserGroupManager.ClearCache(groupSN);
        }

        private int GetGroupIDByCredits(string groupSN, bool isSetCreditsFrom, int credits)
        {
            int groupID = 0;

            string sqlString = string.Empty;
            if (isSetCreditsFrom)
            {
                sqlString = string.Format("SELECT TOP 1 GroupID FROM bairong_UserGroup WHERE GroupSN = '{0}' AND GroupType = '{1}' AND CreditsTo < {2} ORDER BY CreditsTo DESC", PageUtils.FilterSql(groupSN), true.ToString(), credits);
            }
            else
            {
                sqlString = string.Format("SELECT TOP 1 GroupID FROM bairong_UserGroup WHERE GroupSN = '{0}' AND GroupType = '{1}' AND CreditsFrom > {2} ORDER BY CreditsFrom", PageUtils.FilterSql(groupSN), true.ToString(), credits);
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    groupID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return groupID;
        }

        private int GetGroupIDByCreditsFrom(string groupSN, int credits, bool isLessThan)
        {
            int groupID = 0;

            string sqlString = string.Empty;
            if (isLessThan)
            {
                sqlString = string.Format("SELECT TOP 1 GroupID FROM bairong_UserGroup WHERE GroupSN = '{0}' AND (GroupType = '{1}') AND (CreditsFrom <= {2}) ORDER BY CreditsFrom DESC", PageUtils.FilterSql(groupSN), true.ToString(), credits);
            }
            else
            {
                sqlString = string.Format("SELECT TOP 1 GroupID FROM bairong_UserGroup WHERE GroupSN = '{0}' AND (GroupType = '{1}') AND (CreditsFrom >= {2}) ORDER BY CreditsFrom", PageUtils.FilterSql(groupSN), true.ToString(), credits);
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    groupID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return groupID;
        }

        public void Delete(string groupSN, int groupID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_ID, EDataType.Integer, groupID)
			};

            UserGroupInfo groupInfo = this.GetUserGroupInfo(groupID);

            if (groupInfo.GroupType == EUserGroupType.Credits)
            {
                int theGroupID = 0;
                string sqlString = string.Format("SELECT GroupID FROM bairong_UserGroup WHERE GroupSN = '{0}' AND (GroupType = '{1}') AND (CreditsFrom = {2})", PageUtils.FilterSql(groupSN), true.ToString(), groupInfo.CreditsTo);

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        theGroupID = rdr.GetInt32(0);
                    }
                    rdr.Close();
                }

                if (theGroupID > 0)
                {
                    this.UpdateCredits(groupSN, theGroupID, true, groupInfo.CreditsFrom);
                }
            }

            this.ExecuteNonQuery(SQL_DELETE, parms);

            UserGroupManager.ClearCache(groupSN);
        }

        public UserGroupInfo GetUserGroupInfo(int groupID)
        {
            UserGroupInfo groupInfo = null;

            if (groupID > 0)
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_GROUP_ID, EDataType.Integer, groupID)
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_ID, parms))
                {
                    if (rdr.Read())
                    {
                        groupInfo = new UserGroupInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), EUserGroupTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString());
                    }
                    rdr.Close();
                }
            }

            return groupInfo;
        }

        public bool IsExists(string groupSN, string groupName)
        {
            bool exists = false;

            string sqlString = "SELECT GroupName FROM bairong_UserGroup WHERE GroupSN = @GroupSN AND GroupName = @GroupName";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 50, groupName)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

        public UserGroupInfo GetUserGroupByGroupName(string groupSN, string groupName)
        {
            string sqlString = "SELECT GroupID, GroupSN, GroupName, GroupType, CreditsFrom, CreditsTo, Stars, Color, ExtendValues  FROM bairong_UserGroup WHERE GroupSN = @GroupSN AND GroupName = @GroupName";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 50, groupName)
			};

            UserGroupInfo groupInfo = null;
            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    groupInfo = new UserGroupInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), EUserGroupTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString());
                }
                rdr.Close();
            }

            return groupInfo;
        }

        public bool IsCreditsValid(string groupSN, int creditsFrom, int creditsTo)
        {
            bool valid = true;

            string sqlString = string.Format("SELECT * FROM bairong_UserGroup WHERE GroupSN = '{0}' AND (GroupType = 'True') AND ((CreditsFrom >= {1} AND CreditsTo < {2}) OR (CreditsFrom > {1} AND CreditsTo <= {2})) OR (CreditsFrom = {1} AND CreditsTo = {2})", PageUtils.FilterSql(groupSN), creditsFrom, creditsTo);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    valid = false;
                }
                rdr.Close();
            }

            return valid;
        }

        private ArrayList GetUserGroupIDArrayList(string groupSN)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_ID, parms))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return arraylist;
        }

        public DictionaryEntryArrayList GetUserGroupInfoDictionaryEntryArrayList(string groupSN)
        {
            DictionaryEntryArrayList dictionary = new DictionaryEntryArrayList();

            ArrayList groupIDArrayList = this.GetUserGroupIDArrayList(groupSN);
            foreach (int groupID in groupIDArrayList)
            {
                UserGroupInfo groupInfo = this.GetUserGroupInfo(groupID);
                DictionaryEntry entry = new DictionaryEntry(groupID, groupInfo);
                dictionary.Add(entry);
            }

            return dictionary;
        }

        public List<int> GetGroupIDList(string groupSN)
        {
            List<int> list = new List<int>();

            string sqlString = "SELECT GroupID FROM bairong_UserGroup WHERE GroupSN = @GroupSN";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return list;
        }

        public void CreateDefaultUserGroup(string groupSN)
        {
            bool isExists = false;

            string sqlString = "SELECT GroupID FROM bairong_UserGroup WHERE GroupSN = @GroupSN";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    isExists = true;
                }
                rdr.Close();
            }

            if (!isExists)
            {
                UserGroupInfo groupInfo = new UserGroupInfo(0, groupSN, "新手上路", EUserGroupType.Credits, 0, 50, 1, string.Empty, string.Empty);
                groupInfo.Additional = new UserGroupInfoExtend(true, false, false, ETriState.True, 10, true, true, true, false, 0, 5, ETriState.True, ETriState.True, false, 2000, 0, 50, string.Empty);
                Insert(groupInfo);
                groupInfo = new UserGroupInfo(0, groupSN, "注册会员", EUserGroupType.Credits, 50, 200, 2, string.Empty, string.Empty);
                groupInfo.Additional = new UserGroupInfoExtend(true, false, true, ETriState.True, 10, true, true, true, true, 0, 5, ETriState.True, ETriState.True, false, 2000, 0, 50, string.Empty);
                Insert(groupInfo);
                groupInfo = new UserGroupInfo(0, groupSN, "中级会员", EUserGroupType.Credits, 200, 500, 3, string.Empty, string.Empty);
                groupInfo.Additional = new UserGroupInfoExtend(true, false, true, ETriState.All, 10, true, true, true, true, 0, 3, ETriState.True, ETriState.True, false, 2000, 0, 50, string.Empty);
                Insert(groupInfo);
                groupInfo = new UserGroupInfo(0, groupSN, "高级会员", EUserGroupType.Credits, 500, 1000, 4, string.Empty, string.Empty);
                groupInfo.Additional = new UserGroupInfoExtend(true, true, true, ETriState.All, 10, true, true, true, true, 0, 3, ETriState.True, ETriState.True, true, 5120, 0, 50, string.Empty);
                Insert(groupInfo);
                groupInfo = new UserGroupInfo(0, groupSN, "金牌会员", EUserGroupType.Credits, 1000, 3000, 5, string.Empty, string.Empty);
                groupInfo.Additional = new UserGroupInfoExtend(true, true, true, ETriState.All, 10, true, true, true, true, 0, 3, ETriState.True, ETriState.True, true, 5120, 0, 60, string.Empty);
                Insert(groupInfo);
                groupInfo = new UserGroupInfo(0, groupSN, "元老会员", EUserGroupType.Credits, 3000, 9999999, 6, string.Empty, string.Empty);
                groupInfo.Additional = new UserGroupInfoExtend(true, true, true, ETriState.All, 10, true, true, true, true, 0, 3, ETriState.True, ETriState.True, true, 10240, 0, 80, string.Empty);
                Insert(groupInfo);

                groupInfo = new UserGroupInfo(0, groupSN, EUserGroupTypeUtils.GetText(EUserGroupType.Administrator), EUserGroupType.Administrator, 0, 0, 12, string.Empty, string.Empty);
                groupInfo.Additional = new UserGroupInfoExtend(true, true, true, ETriState.All, 0, true, true, true, true, 0, 0, ETriState.All, ETriState.All, true, 0, 0, 0, string.Empty);
                Insert(groupInfo);
                groupInfo = new UserGroupInfo(0, groupSN, EUserGroupTypeUtils.GetText(EUserGroupType.SuperModerator), EUserGroupType.SuperModerator, 0, 0, 8, string.Empty, string.Empty);
                groupInfo.Additional = new UserGroupInfoExtend(true, true, true, ETriState.All, 0, true, true, true, true, 0, 0, ETriState.All, ETriState.All, true, 0, 0, 0, string.Empty);
                Insert(groupInfo);
                groupInfo = new UserGroupInfo(0, groupSN, EUserGroupTypeUtils.GetText(EUserGroupType.Moderator), EUserGroupType.Moderator, 0, 0, 7, string.Empty, string.Empty);
                groupInfo.Additional = new UserGroupInfoExtend(true, true, true, ETriState.All, 0, true, true, true, true, 0, 0, ETriState.All, ETriState.All, true, 0, 0, 0, string.Empty);
                Insert(groupInfo);
                groupInfo = new UserGroupInfo(0, groupSN, EUserGroupTypeUtils.GetText(EUserGroupType.WriteForbidden), EUserGroupType.WriteForbidden, 0, 0, 0, string.Empty, string.Empty);
                groupInfo.Additional = new UserGroupInfoExtend(true, false, false, ETriState.True, 10, true, false, false, false, 0, 5, ETriState.False, ETriState.True, false, 1000, 0, 0, string.Empty);
                Insert(groupInfo);
                groupInfo = new UserGroupInfo(0, groupSN, EUserGroupTypeUtils.GetText(EUserGroupType.ReadForbidden), EUserGroupType.ReadForbidden, 0, 0, 0, string.Empty, string.Empty);
                groupInfo.Additional = new UserGroupInfoExtend(true, false, false, ETriState.False, 10, false, false, false, false, 0, 5, ETriState.False, ETriState.False, false, 1000, 0, 0, string.Empty);
                Insert(groupInfo);
                groupInfo = new UserGroupInfo(0, groupSN, EUserGroupTypeUtils.GetText(EUserGroupType.Guest), EUserGroupType.Guest, 0, 0, 0, string.Empty, string.Empty);
                groupInfo.Additional = new UserGroupInfoExtend(true, false, false, ETriState.False, 0, true, false, false, false, 0, 0, ETriState.False, ETriState.False, false, 1000, 0, 0, string.Empty);
                Insert(groupInfo);
            }
        }
    }
}
