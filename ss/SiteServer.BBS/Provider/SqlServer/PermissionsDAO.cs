using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SiteServer.BBS.Model;
using BaiRong.Core.Data.Provider;
using SiteServer.BBS.Core;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;



namespace SiteServer.BBS.Provider.SqlServer
{
    public class PermissionsDAO : DataProviderBase, IPermissionsDAO
    {
        private const string SQL_SELECT_ALL_BY_FORUM_ID = "SELECT UserGroupID, PublishmentSystemID, Forbidden FROM bbs_Permissions WHERE ForumID = @ForumID";

        private const string SQL_SELECT_ALL = "SELECT UserGroupID, PublishmentSystemID, ForumID, Forbidden FROM bbs_Permissions WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_INSERT = "INSERT INTO bbs_Permissions (UserGroupID, PublishmentSystemID, ForumID, Forbidden) VALUES (@UserGroupID, @PublishmentSystemID, @ForumID, @Forbidden)";

        private const string SQL_DELETE = "DELETE FROM bbs_Permissions WHERE ForumID = @ForumID";

        private const string PARM_USERGROUP_ID = "@UserGroupID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_FORUM_ID = "@ForumID";
        private const string PARM_FORBIDDEN = "@Forbidden";

        private void InsertWithTrans(PermissionsInfo info, IDbTransaction trans)
        {
            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_USERGROUP_ID, EDataType.Integer, info.UserGroupID),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, info.PublishmentSystemID),
				this.GetParameter(PARM_FORUM_ID, EDataType.Integer, info.ForumID),
                this.GetParameter(PARM_FORBIDDEN, EDataType.Text, info.Forbidden)
			};

            this.ExecuteNonQuery(trans, SQL_INSERT, insertParms);
        }

        private void DeleteWithTrans(int forumID, IDbTransaction trans)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_FORUM_ID, EDataType.Integer, forumID)
			};

            this.ExecuteNonQuery(trans, SQL_DELETE, parms);
        }

        public Hashtable GetForbiddenHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    int userGroupID = rdr.GetInt32(0);
                    int forumID = rdr.GetInt32(1);
                    string forbidden = rdr.GetValue(2).ToString();
                    hashtable[userGroupID + "_" + forumID] = forbidden;
                }
                rdr.Close();
            }

            return hashtable;
        }

        public SortedList GetUserGroupIDWithForbiddenSortedList(string groupSN, int forumID)
        {
            SortedList sortedlist = new SortedList();

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_FORUM_ID, EDataType.Integer, forumID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_BY_FORUM_ID, parms))
            {
                while (rdr.Read())
                {
                    int userGroupID = rdr.GetInt32(0);
                    string forbidden = rdr.GetValue(2).ToString();
                    sortedlist[userGroupID] = forbidden;
                }
                rdr.Close();
            }

            if (sortedlist.Count == 0)
            {
                sortedlist = this.GetDefaultUserGroupIDWithForbiddenSortedList(groupSN, forumID);
            }

            return sortedlist;
        }

        private SortedList GetDefaultUserGroupIDWithForbiddenSortedList(string groupSN, int forumID)
        {
            SortedList sortedlist = new SortedList();

            ArrayList groupInfoArrayList = UserGroupManager.GetGroupInfoArrayList(groupSN);
            foreach (UserGroupInfo groupInfo in groupInfoArrayList)
            {
                ArrayList forbiddenArrayList = EPermissionUtils.GetForbiddenArrayList(groupInfo.GroupType);
                if (forbiddenArrayList.Count > 0)
                {
                    sortedlist.Add(groupInfo.GroupID, TranslateUtils.ObjectCollectionToString(forbiddenArrayList));
                }
            }

            return sortedlist;
        }

        public void Save(int publishmentSystemID, int forumID, SortedList userGroupIDWithForbidden)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.DeleteWithTrans(forumID, trans);

                        foreach (int userGroupID in userGroupIDWithForbidden.Keys)
                        {
                            string forbidden = userGroupIDWithForbidden[userGroupID] as string;
                            if (!string.IsNullOrEmpty(forbidden))
                            {
                                PermissionsInfo info = new PermissionsInfo(userGroupID, publishmentSystemID, forumID, forbidden);
                                this.InsertWithTrans(info, trans);
                            }
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

            PermissionManager.RemoveCache(publishmentSystemID);
        }
    }
}
