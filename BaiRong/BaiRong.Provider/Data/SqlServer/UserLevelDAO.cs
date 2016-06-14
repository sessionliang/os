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
    public class UserLevelDAO : DataProviderBase, IUserLevelDAO
    {
        private const string SQL_UPDATE = "UPDATE bairong_UserLevel SET LevelName = @LevelName, LevelType = @LevelType, Stars = @Stars, Color = @Color, ExtendValues = @ExtendValues WHERE ID = @ID";

        private const string SQL_DELETE = "DELETE FROM bairong_UserLevel WHERE ID = @ID";

        private const string SQL_SELECT_BY_ID = "SELECT ID, LevelSN, LevelName, LevelType, MinNum, MaxNum, Stars, Color, ExtendValues FROM bairong_UserLevel WHERE ID = @ID";

        private const string SQL_SELECT_ALL = "SELECT ID, LevelSN, LevelName, LevelType, MinNum, MaxNum, Stars, Color, ExtendValues FROM bairong_UserLevel WHERE LevelSN = @LevelSN ORDER BY LevelType DESC, MaxNum";

        private const string SQL_SELECT_ALL_ID = "SELECT ID FROM bairong_UserLevel WHERE LevelSN = @LevelSN ORDER BY MaxNum, ID";

        private const string PARM_LEVEL_ID = "@ID";
        private const string PARM_LEVEL_SN = "@LevelSN";
        private const string PARM_LEVEL_NAME = "@LevelName";
        private const string PARM_LEVEL_TYPE = "@LevelType";
        private const string PARM_MIN_NUM = "@MinNum";
        private const string PARM_MAX_NUM = "@MaxNum";
        private const string PARM_STARS = "@Stars";
        private const string PARM_COLOR = "@Color";
        private const string PARM_EXTEND_VALUES = "@ExtendValues";

        public int Insert(UserLevelInfo levelInfo)
        {
            int levelID = 0;

            string sqlString = "INSERT INTO bairong_UserLevel (LevelSN, LevelName, LevelType, Stars, Color, ExtendValues) VALUES (@LevelSN, @LevelName, @LevelType, @Stars, @Color, @ExtendValues)";

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_LEVEL_SN, EDataType.NVarChar, 255, levelInfo.LevelSN),
                this.GetParameter(PARM_LEVEL_NAME, EDataType.NVarChar, 50, levelInfo.LevelName),
                this.GetParameter(PARM_LEVEL_TYPE, EDataType.VarChar, 50, EUserLevelTypeUtils.GetValue(levelInfo.LevelType)),
                this.GetParameter(PARM_STARS, EDataType.Integer, levelInfo.Stars),
				this.GetParameter(PARM_COLOR, EDataType.VarChar, 10, levelInfo.Color),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, levelInfo.Additional.ToString())
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, insertParms);

                        levelID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bairong_UserLevel");

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            if (levelInfo.LevelType == EUserLevelType.Credits)
            {
                int theID = this.GetIDByMinNum(levelInfo.LevelSN, levelInfo.MinNum, true);
                if (theID > 0)
                {
                    UserLevelInfo theLevelInfo = this.GetUserLevelInfo(theID);

                    if (theLevelInfo.MinNum == levelInfo.MinNum)
                    {
                        this.UpdateCredits(levelInfo.LevelSN, theID, true, levelInfo.MaxNum);
                    }
                    else//大于
                    {
                        if (levelInfo.MaxNum == theLevelInfo.MaxNum)
                        {
                            this.UpdateCredits(levelInfo.LevelSN, theID, false, levelInfo.MinNum);
                        }
                        else//小于
                        {
                            this.UpdateCredits(levelInfo.LevelSN, theID, false, levelInfo.MinNum);
                            int theID2 = this.GetIDByMinNum(levelInfo.LevelSN, levelInfo.MaxNum, false);
                            if (theID2 > 0)
                            {
                                this.UpdateCredits(levelInfo.LevelSN, theID2, true, levelInfo.MaxNum);
                            }
                        }
                    }
                }
                this.UpdateCreditsRange(levelInfo.LevelSN, levelID, levelInfo.MinNum, levelInfo.MaxNum);
            }

            UserLevelManager.ClearCache(levelInfo.LevelSN);

            return levelID;
        }

        public void UpdateWithCredits(string levelSN, UserLevelInfo levelInfo, int oldMinNum, int oldMaxNum)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_LEVEL_NAME, EDataType.NVarChar, 50, levelInfo.LevelName),
                this.GetParameter(PARM_LEVEL_TYPE, EDataType.VarChar, 50, EUserLevelTypeUtils.GetValue(levelInfo.LevelType)),
                this.GetParameter(PARM_STARS, EDataType.Integer, levelInfo.Stars),
				this.GetParameter(PARM_COLOR, EDataType.VarChar, 10, levelInfo.Color),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, levelInfo.Additional.ToString()),
				this.GetParameter(PARM_LEVEL_ID, EDataType.Integer, levelInfo.ID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, updateParms);

            if (levelInfo.MinNum > oldMinNum)
            {
                int levelID = this.GetIDByCredits(levelInfo.LevelSN, true, levelInfo.MinNum);
                if (levelID > 0)
                {
                    this.UpdateCredits(levelSN, levelID, false, levelInfo.MinNum);
                }
                this.UpdateCredits(levelSN, levelInfo.ID, true, levelInfo.MinNum);
            }

            if (levelInfo.MaxNum < oldMaxNum)
            {
                int levelID = this.GetIDByCredits(levelInfo.LevelSN, false, levelInfo.MaxNum);
                if (levelID > 0)
                {
                    this.UpdateCredits(levelSN, levelID, true, levelInfo.MaxNum);
                }
                this.UpdateCredits(levelSN, levelInfo.ID, false, levelInfo.MaxNum);
            }

            UserLevelManager.ClearCache(levelSN);
        }

        public void UpdateWithoutCredits(string levelSN, UserLevelInfo userLevelInfo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_LEVEL_NAME, EDataType.NVarChar, 50, userLevelInfo.LevelName),
                this.GetParameter(PARM_LEVEL_TYPE, EDataType.VarChar, 50, EUserLevelTypeUtils.GetValue(userLevelInfo.LevelType)),
                this.GetParameter(PARM_STARS, EDataType.Integer, userLevelInfo.Stars),
				this.GetParameter(PARM_COLOR, EDataType.VarChar, 10, userLevelInfo.Color),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, userLevelInfo.Additional.ToString()),
				this.GetParameter(PARM_LEVEL_ID, EDataType.Integer, userLevelInfo.ID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, updateParms);

            UserLevelManager.ClearCache(levelSN);
        }

        private void UpdateCredits(string levelSN, int levelID, bool isMinNum, int credits)
        {
            if (isMinNum)
            {
                string sqlString = "UPDATE bairong_UserLevel SET MinNum = @MinNum WHERE ID = @ID";

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter(PARM_MIN_NUM, EDataType.Integer, credits),
				    this.GetParameter(PARM_LEVEL_ID, EDataType.Integer, levelID)
			    };

                this.ExecuteNonQuery(sqlString, parms);
            }
            else
            {
                string sqlString = "UPDATE bairong_UserLevel SET MaxNum = @MaxNum WHERE ID = @ID";

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter(PARM_MAX_NUM, EDataType.Integer, credits),
				    this.GetParameter(PARM_LEVEL_ID, EDataType.Integer, levelID)
			    };

                this.ExecuteNonQuery(sqlString, parms);
            }

            UserLevelManager.ClearCache(levelSN);
        }

        private void UpdateCreditsRange(string levelSN, int levelID, int minNum, int maxNum)
        {
            string sqlString = "UPDATE bairong_UserLevel SET MinNum = @MinNum, MaxNum = @MaxNum WHERE ID = @ID";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_MIN_NUM, EDataType.Integer, minNum),
				this.GetParameter(PARM_MAX_NUM, EDataType.Integer, maxNum),
				this.GetParameter(PARM_LEVEL_ID, EDataType.Integer, levelID)
			};

            this.ExecuteNonQuery(sqlString, parms);

            UserLevelManager.ClearCache(levelSN);
        }

        private int GetIDByCredits(string levelSN, bool isSetMinNum, int credits)
        {
            int levelID = 0;

            string sqlString = string.Empty;
            if (isSetMinNum)
            {
                sqlString = string.Format("SELECT TOP 1 ID FROM bairong_UserLevel WHERE LevelSN = '{0}' AND LevelType = '{1}' AND MaxNum < {2} ORDER BY MaxNum DESC", PageUtils.FilterSql(levelSN), true.ToString(), credits);
            }
            else
            {
                sqlString = string.Format("SELECT TOP 1 ID FROM bairong_UserLevel WHERE LevelSN = '{0}' AND LevelType = '{1}' AND MinNum > {2} ORDER BY MinNum", PageUtils.FilterSql(levelSN), true.ToString(), credits);
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    levelID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return levelID;
        }

        private int GetIDByMinNum(string levelSN, int credits, bool isLessThan)
        {
            int levelID = 0;

            string sqlString = string.Empty;
            if (isLessThan)
            {
                sqlString = string.Format("SELECT TOP 1 ID FROM bairong_UserLevel WHERE LevelSN = '{0}' AND (LevelType = '{1}') AND (MinNum <= {2}) ORDER BY MinNum DESC", PageUtils.FilterSql(levelSN), true.ToString(), credits);
            }
            else
            {
                sqlString = string.Format("SELECT TOP 1 ID FROM bairong_UserLevel WHERE LevelSN = '{0}' AND (LevelType = '{1}') AND (MinNum >= {2}) ORDER BY MinNum", PageUtils.FilterSql(levelSN), true.ToString(), credits);
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    levelID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return levelID;
        }

        public void Delete(string levelSN, int levelID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_LEVEL_ID, EDataType.Integer, levelID)
			};

            UserLevelInfo levelInfo = this.GetUserLevelInfo(levelID);

            if (levelInfo.LevelType == EUserLevelType.Credits)
            {
                int theID = 0;
                string sqlString = string.Format("SELECT ID FROM bairong_UserLevel WHERE LevelSN = '{0}' AND (LevelType = '{1}') AND (MinNum = {2})", PageUtils.FilterSql(levelSN), true.ToString(), levelInfo.MaxNum);

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        theID = rdr.GetInt32(0);
                    }
                    rdr.Close();
                }

                if (theID > 0)
                {
                    this.UpdateCredits(levelSN, theID, true, levelInfo.MinNum);
                }
            }

            this.ExecuteNonQuery(SQL_DELETE, parms);

            UserLevelManager.ClearCache(levelSN);
        }

        public UserLevelInfo GetUserLevelInfo(int levelID)
        {
            UserLevelInfo levelInfo = null;

            if (levelID > 0)
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_LEVEL_ID, EDataType.Integer, levelID)
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_ID, parms))
                {
                    if (rdr.Read())
                    {
                        levelInfo = new UserLevelInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), EUserLevelTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString());
                    }
                    rdr.Close();
                }
            }

            return levelInfo;
        }

        public bool IsExists(string levelSN, string levelName)
        {
            bool exists = false;

            string sqlString = "SELECT LevelName FROM bairong_UserLevel WHERE LevelSN = @LevelSN AND LevelName = @LevelName";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_LEVEL_SN, EDataType.NVarChar, 255, levelSN),
                this.GetParameter(PARM_LEVEL_NAME, EDataType.NVarChar, 50, levelName)
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

        public UserLevelInfo GetUserLevelByLevelName(string levelSN, string levelName)
        {
            string sqlString = "SELECT ID, LevelSN, LevelName, LevelType, MinNum, MaxNum, Stars, Color, ExtendValues  FROM bairong_UserLevel WHERE LevelSN = @LevelSN AND LevelName = @LevelName";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_LEVEL_SN, EDataType.NVarChar, 255, levelSN),
                this.GetParameter(PARM_LEVEL_NAME, EDataType.NVarChar, 50, levelName)
			};

            UserLevelInfo levelInfo = null;
            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    levelInfo = new UserLevelInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), EUserLevelTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString());
                }
                rdr.Close();
            }

            return levelInfo;
        }

        public bool IsCreditsValid(string levelSN, int creditsFrom, int creditsTo)
        {
            bool valid = true;

            string sqlString = string.Format("SELECT * FROM bairong_UserLevel WHERE LevelSN = '{0}' AND (LevelType = 'True') AND ((MinNum >= {1} AND MaxNum < {2}) OR (MinNum > {1} AND MaxNum <= {2})) OR (MinNum = {1} AND MaxNum = {2})", PageUtils.FilterSql(levelSN), creditsFrom, creditsTo);

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

        private ArrayList GetUserIDArrayList(string levelSN)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_LEVEL_SN, EDataType.NVarChar, 255, levelSN)
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

        public DictionaryEntryArrayList GetUserLevelInfoDictionaryEntryArrayList(string levelSN)
        {
            DictionaryEntryArrayList dictionary = new DictionaryEntryArrayList();

            ArrayList levelIDArrayList = this.GetUserIDArrayList(levelSN);
            foreach (int levelID in levelIDArrayList)
            {
                UserLevelInfo levelInfo = this.GetUserLevelInfo(levelID);
                DictionaryEntry entry = new DictionaryEntry(levelID, levelInfo);
                dictionary.Add(entry);
            }

            return dictionary;
        }

        public List<int> GetLevelIDList(string levelSN)
        {
            List<int> list = new List<int>();

            string sqlString = "SELECT ID FROM bairong_UserLevel WHERE LevelSN = @LevelSN";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_LEVEL_SN, EDataType.NVarChar, 255, levelSN)
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

        public void CreateDefaultUserLevel(string levelSN)
        {
            bool isExists = false;

            string sqlString = "SELECT ID FROM bairong_UserLevel WHERE LevelSN = @LevelSN";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_LEVEL_SN, EDataType.NVarChar, 255, levelSN)
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
                UserLevelInfo levelInfo = new UserLevelInfo(0, levelSN, "新手上路", EUserLevelType.Credits, 0, 50, 1, string.Empty, string.Empty);
                levelInfo.Additional = new UserLevelInfoExtend(true, false, false, ETriState.True, 10, true, true, true, false, 0, 5, ETriState.True, ETriState.True, false, 2000, 0, 50, string.Empty);
                Insert(levelInfo);
                levelInfo = new UserLevelInfo(0, levelSN, "注册会员", EUserLevelType.Credits, 50, 200, 2, string.Empty, string.Empty);
                levelInfo.Additional = new UserLevelInfoExtend(true, false, true, ETriState.True, 10, true, true, true, true, 0, 5, ETriState.True, ETriState.True, false, 2000, 0, 50, string.Empty);
                Insert(levelInfo);
                levelInfo = new UserLevelInfo(0, levelSN, "中级会员", EUserLevelType.Credits, 200, 500, 3, string.Empty, string.Empty);
                levelInfo.Additional = new UserLevelInfoExtend(true, false, true, ETriState.All, 10, true, true, true, true, 0, 3, ETriState.True, ETriState.True, false, 2000, 0, 50, string.Empty);
                Insert(levelInfo);
                levelInfo = new UserLevelInfo(0, levelSN, "高级会员", EUserLevelType.Credits, 500, 1000, 4, string.Empty, string.Empty);
                levelInfo.Additional = new UserLevelInfoExtend(true, true, true, ETriState.All, 10, true, true, true, true, 0, 3, ETriState.True, ETriState.True, true, 5120, 0, 50, string.Empty);
                Insert(levelInfo);
                levelInfo = new UserLevelInfo(0, levelSN, "金牌会员", EUserLevelType.Credits, 1000, 3000, 5, string.Empty, string.Empty);
                levelInfo.Additional = new UserLevelInfoExtend(true, true, true, ETriState.All, 10, true, true, true, true, 0, 3, ETriState.True, ETriState.True, true, 5120, 0, 60, string.Empty);
                Insert(levelInfo);
                levelInfo = new UserLevelInfo(0, levelSN, "元老会员", EUserLevelType.Credits, 3000, 9999999, 6, string.Empty, string.Empty);
                levelInfo.Additional = new UserLevelInfoExtend(true, true, true, ETriState.All, 10, true, true, true, true, 0, 3, ETriState.True, ETriState.True, true, 10240, 0, 80, string.Empty);
                Insert(levelInfo);

                levelInfo = new UserLevelInfo(0, levelSN, EUserLevelTypeUtils.GetText(EUserLevelType.Administrator), EUserLevelType.Administrator, 0, 0, 12, string.Empty, string.Empty);
                levelInfo.Additional = new UserLevelInfoExtend(true, true, true, ETriState.All, 0, true, true, true, true, 0, 0, ETriState.All, ETriState.All, true, 0, 0, 0, string.Empty);
                Insert(levelInfo);
                levelInfo = new UserLevelInfo(0, levelSN, EUserLevelTypeUtils.GetText(EUserLevelType.SuperModerator), EUserLevelType.SuperModerator, 0, 0, 8, string.Empty, string.Empty);
                levelInfo.Additional = new UserLevelInfoExtend(true, true, true, ETriState.All, 0, true, true, true, true, 0, 0, ETriState.All, ETriState.All, true, 0, 0, 0, string.Empty);
                Insert(levelInfo);
                levelInfo = new UserLevelInfo(0, levelSN, EUserLevelTypeUtils.GetText(EUserLevelType.Moderator), EUserLevelType.Moderator, 0, 0, 7, string.Empty, string.Empty);
                levelInfo.Additional = new UserLevelInfoExtend(true, true, true, ETriState.All, 0, true, true, true, true, 0, 0, ETriState.All, ETriState.All, true, 0, 0, 0, string.Empty);
                Insert(levelInfo);
                levelInfo = new UserLevelInfo(0, levelSN, EUserLevelTypeUtils.GetText(EUserLevelType.WriteForbidden), EUserLevelType.WriteForbidden, 0, 0, 0, string.Empty, string.Empty);
                levelInfo.Additional = new UserLevelInfoExtend(true, false, false, ETriState.True, 10, true, false, false, false, 0, 5, ETriState.False, ETriState.True, false, 1000, 0, 0, string.Empty);
                Insert(levelInfo);
                levelInfo = new UserLevelInfo(0, levelSN, EUserLevelTypeUtils.GetText(EUserLevelType.ReadForbidden), EUserLevelType.ReadForbidden, 0, 0, 0, string.Empty, string.Empty);
                levelInfo.Additional = new UserLevelInfoExtend(true, false, false, ETriState.False, 10, false, false, false, false, 0, 5, ETriState.False, ETriState.False, false, 1000, 0, 0, string.Empty);
                Insert(levelInfo);
                levelInfo = new UserLevelInfo(0, levelSN, EUserLevelTypeUtils.GetText(EUserLevelType.Guest), EUserLevelType.Guest, 0, 0, 0, string.Empty, string.Empty);
                levelInfo.Additional = new UserLevelInfoExtend(true, false, false, ETriState.False, 0, true, false, false, false, 0, 0, ETriState.False, ETriState.False, false, 1000, 0, 0, string.Empty);
                Insert(levelInfo);
            }
        }

    }
}
