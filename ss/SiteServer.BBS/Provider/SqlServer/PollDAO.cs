using System;
using System.Data;
using System.Collections;
using BaiRong.Core.Data.Provider;
using SiteServer.BBS.Model;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class PollDAO : DataProviderBase, IPollDAO
    {
        // Static constants
        private const string SQL_SELECT_POLL = "SELECT ID, PublishmentSystemID, ThreadID, IsVoteFirst, MaxNum, RestrictType, AddDate, Deadline FROM bbs_Poll WHERE ID = @ID";

        private const string SQL_SELECT_POLL_BY_THREAD_ID = "SELECT ID, PublishmentSystemID, ThreadID, IsVoteFirst, MaxNum, RestrictType, AddDate, Deadline FROM bbs_Poll WHERE ThreadID = @ThreadID";

        private const string SQL_SELECT_ALL_POLL_ITEM = "SELECT ID, PublishmentSystemID, PollID, Title, Num FROM bbs_PollItem WHERE PollID = @PollID";

        private const string SQL_SELECT_POLL_NUM = "SELECT SUM(Num) AS TotalNum FROM bbs_PollItem WHERE PublishmentSystemID = @PublishmentSystemID AND PollID = @PollID";

        private const string SQL_SELECT_ID_POLL_USER = "SELECT ID FROM bbs_PollUser WHERE PublishmentSystemID = @PublishmentSystemID AND PollID = @PollID AND UserName = @UserName";

        private const string SQL_SELECT_ID_COLLECTION_POLL_USER = "SELECT PollItemIDCollection FROM bbs_PollUser WHERE PublishmentSystemID = @PublishmentSystemID AND PollID = @PollID AND UserName = @UserName";

        private const string SQL_UPDATE_POLL = "UPDATE bbs_Poll SET ThreadID = @ThreadID, IsVoteFirst = @IsVoteFirst, MaxNum = @MaxNum, RestrictType = @RestrictType, AddDate = @AddDate, Deadline = @Deadline WHERE ID = @ID";

        private const string SQL_DELETE_POLL = "DELETE FROM bbs_Poll WHERE PollID = @PollID";

        private const string SQL_DELETE_POLL_ITEMS = "DELETE FROM bbs_PollItem WHERE PollID = @PollID";

        //PollInfo
        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_THREAD_ID = "@ThreadID";
        private const string PARM_IS_VOTE_FIRST = "@IsVoteFirst";
        private const string PARM_MAX_NUM = "@MaxNum";
        private const string PARM_RESTRICT_TYPE = "@RestrictType";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_DEAD_LINE = "@Deadline";

        //PollItemInfo
        private const string PARM_POLL_ID = "@PollID";
        private const string PARM_TITLE = "@Title";
        private const string PARM_NUM = "@Num";

        //PollIPAddressInfo
        private const string PARM_POLL_ITEMID_COLLECTION = "@PollItemIDCollection";
        private const string PARM_IP_ADDRESS = "@IPAddress";
        private const string PARM_USER_NAME = "@UserName";

        private string GetInsertPollItemSqlString()
        {
            string SQL_INSERT_POLL_ITEM = "INSERT INTO bbs_PollItem (PublishmentSystemID, PollID, Title, Num) VALUES (@PublishmentSystemID, @PollID, @Title, @Num)";

            return SQL_INSERT_POLL_ITEM;
        }

        public int Insert(PollInfo info, ArrayList pollItems)
        {
            int pollID = 0;

            string SQL_INSERT_POLL = "INSERT INTO bbs_Poll (PublishmentSystemID, ThreadID, IsVoteFirst, MaxNum, RestrictType, AddDate, Deadline) VALUES (@PublishmentSystemID, @ThreadID, @IsVoteFirst, @MaxNum, @RestrictType, @AddDate, @Deadline)";

            string SQL_INSERT_POLL_ITEM = this.GetInsertPollItemSqlString();

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter(PARM_THREAD_ID, EDataType.Integer, info.ThreadID),
                this.GetParameter(PARM_IS_VOTE_FIRST, EDataType.VarChar, 18, info.IsVoteFirst.ToString()),
				this.GetParameter(PARM_MAX_NUM, EDataType.Integer, info.MaxNum),
				this.GetParameter(PARM_RESTRICT_TYPE, EDataType.VarChar, 50, EPollRestrictTypeUtils.GetValue(info.RestrictType)),
				this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, info.AddDate),
                this.GetParameter(PARM_DEAD_LINE, EDataType.DateTime, info.Deadline)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT_POLL, insertParms);
                        pollID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bbs_Poll");
                        foreach (string title in pollItems)
                        {
                            IDbDataParameter[] insertItemParms = new IDbDataParameter[]
							{
                                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, info.PublishmentSystemID),
								this.GetParameter(PARM_POLL_ID, EDataType.Integer, pollID),
								this.GetParameter(PARM_TITLE, EDataType.NVarChar, 255, title),
								this.GetParameter(PARM_NUM, EDataType.Integer, 0)
							};
                            this.ExecuteNonQuery(trans, SQL_INSERT_POLL_ITEM, insertItemParms);
                        }

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            return pollID;
        }

        public void Update(PollInfo info, ArrayList pollItems)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_THREAD_ID, EDataType.Integer, info.ThreadID),
                this.GetParameter(PARM_IS_VOTE_FIRST, EDataType.VarChar, 18, info.IsVoteFirst.ToString()),
				this.GetParameter(PARM_MAX_NUM, EDataType.Integer, info.MaxNum),
				this.GetParameter(PARM_RESTRICT_TYPE, EDataType.VarChar, 50, EPollRestrictTypeUtils.GetValue(info.RestrictType)),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, info.AddDate),
                this.GetParameter(PARM_DEAD_LINE, EDataType.DateTime, info.Deadline),
				this.GetParameter(PARM_ID, EDataType.Integer, info.ID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_POLL, updateParms);

            ArrayList pollItemInfoArrayList = this.GetPollItemInfoArrayList(info.ID);

            if (pollItems.Count <= pollItemInfoArrayList.Count)
            {
                if (pollItems.Count < pollItemInfoArrayList.Count)
                {
                    for (int i = pollItems.Count; i < pollItemInfoArrayList.Count; i++)
                    {
                        PollItemInfo itemInfo = (PollItemInfo)pollItemInfoArrayList[i];
                        this.DeletePollItemInfo(itemInfo.ID);
                    }
                }
                for (int i = 0; i < pollItems.Count; i++)
                {
                    string title = (string)pollItems[i];
                    PollItemInfo itemInfo = (PollItemInfo)pollItemInfoArrayList[i];
                    if (itemInfo.Title != title)
                    {
                        this.UpdatePollItemTitle(title, itemInfo.ID);
                    }
                }
            }
            else
            {
                for (int i = 0; i < pollItems.Count; i++)
                {
                    string title = (string)pollItems[i];

                    PollItemInfo itemInfo = null;
                    if (i < pollItemInfoArrayList.Count)
                    {
                        itemInfo = pollItemInfoArrayList[i] as PollItemInfo;
                    }
                    if (itemInfo != null)
                    {
                        if (itemInfo.Title != title)
                        {
                            this.UpdatePollItemTitle(title, itemInfo.ID);
                        }
                    }
                    else
                    {
                        itemInfo = new PollItemInfo(0, info.PublishmentSystemID, info.ID, title, 0);
                        this.InsertPollItemInfo(itemInfo);
                    }
                }
            }
        }

        private void UpdatePollItemTitle(string title, int pollItemID)
        {
            string sqlString = string.Format("UPDATE bbs_PollItem SET Title = @Title WHERE ID = {1}", pollItemID);

            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TITLE, EDataType.NVarChar,255, title)    
			};

            base.ExecuteNonQuery(sqlString, updateParms);
        }

        private void DeletePollItemInfo(int pollItemID)
        {
            string sqlString = string.Format("DELETE bbs_PollItem WHERE ID = {0}", pollItemID);
            base.ExecuteNonQuery(sqlString);
        }

        private void InsertPollItemInfo(PollItemInfo itemInfo)
        {
            string SQL_INSERT_POLL_ITEM = this.GetInsertPollItemSqlString();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, itemInfo.PublishmentSystemID),
                this.GetParameter(PARM_POLL_ID, EDataType.Integer, itemInfo.PollID),
				this.GetParameter(PARM_TITLE, EDataType.NVarChar, 255, itemInfo.Title),
				this.GetParameter(PARM_NUM, EDataType.Integer, itemInfo.Num)
			};

            this.ExecuteNonQuery(SQL_INSERT_POLL_ITEM, parms);
        }

        public void InsertPollUser(PollUserInfo pollUser)
        {
            string SQL_INSERT_POLL_IP_ADDRESS = "INSERT INTO bbs_PollUser (PollID, PublishmentSystemID, PollItemIDCollection, IPAddress, UserName, AddDate) VALUES (@PollID, @PublishmentSystemID, @PollItemIDCollection, @IPAddress, @UserName, @AddDate)";

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_POLL_ID, EDataType.Integer, pollUser.PollID),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, pollUser.PublishmentSystemID),
				this.GetParameter(PARM_POLL_ITEMID_COLLECTION, EDataType.VarChar, 200, pollUser.PollItemIDCollection),
                this.GetParameter(PARM_IP_ADDRESS, EDataType.VarChar, 50, pollUser.IPAddress),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, pollUser.UserName),
				this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, pollUser.AddDate)
			};

            this.ExecuteNonQuery(SQL_INSERT_POLL_IP_ADDRESS, insertParms);
        }


        public void AddPollNum(ArrayList pollItemIDArrayList)
        {
            string sqlString = string.Format("UPDATE bbs_PollItem SET Num = Num + 1 WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(pollItemIDArrayList));

            this.ExecuteNonQuery(sqlString);
        }


        public void DeletePollInfo(int pollID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, pollID)
			};

            this.ExecuteNonQuery(SQL_DELETE_POLL, parms);
        }


        public void DeletePollItems(int pollID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_POLL_ID, EDataType.Integer, pollID)
			};

            this.ExecuteNonQuery(SQL_DELETE_POLL_ITEMS, parms);
        }


        public PollInfo GetPollInfo(int threadID)
        {
            PollInfo pollInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_THREAD_ID, EDataType.Integer, threadID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_POLL_BY_THREAD_ID, parms))
            {
                if (rdr.Read())
                {
                    pollInfo = new PollInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetInt32(4), EPollRestrictTypeUtils.GetEnumType(rdr.GetValue(5).ToString()), rdr.GetDateTime(6), rdr.GetDateTime(7));
                }
                rdr.Close();
            }
            return pollInfo;
        }

        public ArrayList GetPollItemInfoArrayList(int pollID)
        {
            ArrayList pollItems = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_POLL_ID, EDataType.Integer, pollID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_POLL_ITEM, parms))
            {
                while (rdr.Read())
                {
                    PollItemInfo info = new PollItemInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString(), rdr.GetInt32(4));
                    pollItems.Add(info);
                }
                rdr.Close();
            }
            return pollItems;
        }

        public PollItemInfo GetPollItemInfo(DataRow dataRow)
        {
            PollItemInfo pollItemInfo = null;

            if (dataRow != null)
            {
                pollItemInfo = new PollItemInfo((int)dataRow["ID"], (int)dataRow["PublishmentSystemID"], (int)dataRow["PollID"], dataRow["Title"] as string, (int)dataRow["Num"]);
            }

            return pollItemInfo;
        }

        public int GetTotalPollNum(int pollID)
        {
            int totalPollNum = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_POLL_ID, EDataType.Integer, pollID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_POLL_NUM, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        totalPollNum = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return totalPollNum;
        }

        public bool IsUserExists(int pollID, string userName)
        {
            bool isExists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_POLL_ID, EDataType.Integer, pollID),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ID_POLL_USER, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        isExists = true;
                    }
                }
                rdr.Close();
            }
            return isExists;
        }

        public ArrayList GetPollItemIDArrayList(int pollID, string userName)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_POLL_ID, EDataType.Integer, pollID),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ID_COLLECTION_POLL_USER, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        arraylist = TranslateUtils.StringCollectionToIntArrayList(rdr.GetValue(0).ToString());
                    }
                }
                rdr.Close();
            }
            return arraylist;
        }
    }
}
