using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;


using SiteServer.BBS.Model;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class BBSUserDAO : DataProviderBase, SiteServer.BBS.IBBSUserDAO
    {
        private const string SQL_INSERT = "INSERT INTO bbs_Users (UserID, GroupSN, UserName, PostCount, PostDigestCount, Prestige, Contribution, Currency, ExtCredit1, ExtCredit2, ExtCredit3, LastPostDate) VALUES (@UserID, @GroupSN, @UserName, @PostCount, @PostDigestCount, @Prestige, @Contribution, @Currency, @ExtCredit1, @ExtCredit2, @ExtCredit3, @LastPostDate)";

        private const string SQL_SELECT = "SELECT UserID, GroupSN, UserName, PostCount, PostDigestCount, Prestige, Contribution, Currency, ExtCredit1, ExtCredit2, ExtCredit3, LastPostDate FROM bbs_Users WHERE GroupSN = @GroupSN AND UserName = @UserName";

        private const string SQL_SELECT_BY_USER_ID = "SELECT UserID, GroupSN, UserName, PostCount, PostDigestCount, Prestige, Contribution, Currency, ExtCredit1, ExtCredit2, ExtCredit3, LastPostDate FROM bbs_Users WHERE UserID = @UserID";

        private const string SQL_UPDATE = "UPDATE bbs_Users SET PostCount = @PostCount, PostDigestCount = @PostDigestCount, Prestige = @Prestige, Contribution = @Contribution, Currency = @Currency, ExtCredit1 = @ExtCredit1, ExtCredit2 = @ExtCredit2, ExtCredit3 = @ExtCredit3, LastPostDate = @LastPostDate WHERE UserID = @UserID";

        private const string SQL_DELETE = "DELETE bbs_Users WHERE GroupSN = @GroupSN AND UserName = @UserName";

        private const string PARM_USER_ID = "@UserID";
        private const string PARM_GROUP_SN = "@GroupSN";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_POST_COUNT = "@PostCount";
        private const string PARM_POST_DIGEST_COUNT = "@PostDigestCount";
        private const string PARM_PRESTIGE = "@Prestige";
        private const string PARM_CONTRIBUTION = "@Contribution";
        private const string PARM_CURRENCY = "@Currency";
        private const string PARM_EXT_CREDIT1 = "@ExtCredit1";
        private const string PARM_EXT_CREDIT2 = "@ExtCredit2";
        private const string PARM_EXT_CREDIT3 = "@ExtCredit3";
        private const string PARM_LAST_POST_DATE = "@LastPostDate";

        public void Insert(BBSUserInfo bbsUserInfo)
        {
            IDbDataParameter[] insertParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_ID, EDataType.Integer, bbsUserInfo.UserID),
				this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, bbsUserInfo.GroupSN),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, bbsUserInfo.UserName),
                this.GetParameter(PARM_POST_COUNT, EDataType.Integer, bbsUserInfo.PostCount),
                this.GetParameter(PARM_POST_DIGEST_COUNT, EDataType.Integer, bbsUserInfo.PostDigestCount),
                this.GetParameter(PARM_PRESTIGE, EDataType.Integer, bbsUserInfo.Prestige),
                this.GetParameter(PARM_CONTRIBUTION, EDataType.Integer, bbsUserInfo.Contribution),
                this.GetParameter(PARM_CURRENCY, EDataType.Integer, bbsUserInfo.Currency),
                this.GetParameter(PARM_EXT_CREDIT1, EDataType.Integer, bbsUserInfo.ExtCredit1),
                this.GetParameter(PARM_EXT_CREDIT2, EDataType.Integer, bbsUserInfo.ExtCredit2),
                this.GetParameter(PARM_EXT_CREDIT3, EDataType.Integer, bbsUserInfo.ExtCredit3),
                this.GetParameter(PARM_LAST_POST_DATE, EDataType.DateTime, bbsUserInfo.LastPostDate)
			};

            this.ExecuteNonQuery(SQL_INSERT, insertParms);

            //ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);

            //additional.StatUserNameRecently = bbsUserInfo.UserName;
            //additional.StatUserCount += 1;

            //ConfigurationManager.Update(publishmentSystemID);
        }

        public void Update(BBSUserInfo bbsUserInfo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_POST_COUNT, EDataType.Integer, bbsUserInfo.PostCount),
                this.GetParameter(PARM_POST_DIGEST_COUNT, EDataType.Integer, bbsUserInfo.PostDigestCount),
                this.GetParameter(PARM_PRESTIGE, EDataType.Integer, bbsUserInfo.Prestige),
                this.GetParameter(PARM_CONTRIBUTION, EDataType.Integer, bbsUserInfo.Contribution),
                this.GetParameter(PARM_CURRENCY, EDataType.Integer, bbsUserInfo.Currency),
                this.GetParameter(PARM_EXT_CREDIT1, EDataType.Integer, bbsUserInfo.ExtCredit1),
                this.GetParameter(PARM_EXT_CREDIT2, EDataType.Integer, bbsUserInfo.ExtCredit2),
                this.GetParameter(PARM_EXT_CREDIT3, EDataType.Integer, bbsUserInfo.ExtCredit3),
                this.GetParameter(PARM_LAST_POST_DATE, EDataType.DateTime, bbsUserInfo.LastPostDate),
		        this.GetParameter(PARM_USER_ID, EDataType.Integer, bbsUserInfo.UserID)
	        };

            this.ExecuteNonQuery(SQL_UPDATE, updateParms);

            BBSUserManager.RemoveCache(bbsUserInfo.GroupSN, bbsUserInfo.UserName);
        }

        public void UpdateCredit(string groupSN, string userName, int credits, int prestige, int contribution, int currency, int extCredit1, int extCredit2, int extCredit3)
        {
            BaiRongDataProvider.UserDAO.AddCredits(groupSN, userName, credits);

            StringBuilder builder = new StringBuilder();
            if (prestige != 0)
            {
                builder.AppendFormat("Prestige = Prestige + {0}, ", prestige);
            }
            if (contribution != 0)
            {
                builder.AppendFormat("Contribution = Contribution + {0}, ", contribution);
            }
            if (currency != 0)
            {
                builder.AppendFormat("Currency = Currency + {0}, ", currency);
            }
            if (extCredit1 != 0)
            {
                builder.AppendFormat("ExtCredit1 = ExtCredit1 + {0}, ", extCredit1);
            }
            if (extCredit2 != 0)
            {
                builder.AppendFormat("ExtCredit2 = ExtCredit2 + {0}, ", extCredit2);
            }
            if (extCredit3 != 0)
            {
                builder.AppendFormat("ExtCredit3 = ExtCredit3 + {0}, ", extCredit3);
            }

            if (builder.Length > 0)
            {
                builder.Length -= 2;

                string sqlString = string.Format("UPDATE bbs_Users SET {0} WHERE GroupSN = '{1}' AND UserName = '{2}'", builder, groupSN, userName);

                this.ExecuteNonQuery(sqlString);

                BBSUserManager.RemoveCache(groupSN, userName);
            }
        }

        public void AddPostCount(string groupSN, string userName, DateTime dateTime, int creditMultiplierPostCount, IDbTransaction trans)
        {
            BaiRongDataProvider.UserDAO.AddCredits(groupSN, userName, creditMultiplierPostCount);

            string sqlString = string.Format("UPDATE bbs_Users SET PostCount = PostCount + 1, LastPostDate = @LastPostDate WHERE GroupSN = @GroupSN AND UserName = @UserName");

            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_LAST_POST_DATE, EDataType.DateTime, dateTime),
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)	  
			};

            this.ExecuteNonQuery(trans, sqlString, updateParms);

            BBSUserManager.RemoveCache(groupSN, userName);
        }

        public void Delete(string groupSN, string userName)
        {
            IDbDataParameter[] deleteParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
			};

            this.ExecuteNonQuery(SQL_DELETE, deleteParms);
            BBSUserManager.RemoveCache(groupSN, userName);
        }

        public BBSUserInfo GetBBSUserInfo(string groupSN, string userName)
        {
            BBSUserInfo bbsUserInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    bbsUserInfo = new BBSUserInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetInt32(9), rdr.GetInt32(10), rdr.GetDateTime(11));
                }
                rdr.Close();
            }

            if (bbsUserInfo == null)
            {
                UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(groupSN, userName);
                if (userInfo != null)
                {
                    bbsUserInfo = new BBSUserInfo(userInfo.UserID, userInfo.GroupSN, userInfo.UserName);
                    Insert(bbsUserInfo);
                }
            }

            return bbsUserInfo;
        }

        public BBSUserInfo GetBBSUserInfo(int userID)
        {
            BBSUserInfo bbsUserInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_ID, EDataType.Integer, userID),
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_USER_ID, parms))
            {
                if (rdr.Read())
                {
                    bbsUserInfo = new BBSUserInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), rdr.GetInt32(9), rdr.GetInt32(10), rdr.GetDateTime(11));
                }
                rdr.Close();
            }

            if (bbsUserInfo == null)
            {
                UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(userID);
                if (userInfo != null)
                {
                    bbsUserInfo = new BBSUserInfo(userInfo.UserID, userInfo.GroupSN, userInfo.UserName);
                    Insert(bbsUserInfo);
                }
            }

            return bbsUserInfo;
        }

        public bool IsExists(string groupSN, string userName)
        {
            bool exists = false;

            string sqlString = string.Format("SELECT [UserName] FROM bbs_Users WHERE GroupSN = @GroupSN AND lower([UserName]) = lower(@UserName)", groupSN, userName);

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, selectParms))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }
            return exists;
        }

        public ArrayList GetUserNameList(string groupSN, ArrayList threadIDArrayList)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT UserName FROM bbs_Users WHERE GroupSN = @GroupSN AND UserName IN (SELECT DISTINCT UserName FROM bbs_Post WHERE ID IN ({0}))", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(threadIDArrayList));

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN)                
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, selectParms))
            {
                while (rdr.Read())
                {
                    string userName = rdr.GetValue(0).ToString();
                    arraylist.Add(userName);
                }
                rdr.Close();
            }

            return arraylist;
        }
    }
}
