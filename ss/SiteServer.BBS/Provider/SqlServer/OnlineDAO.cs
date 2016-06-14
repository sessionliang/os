using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using SiteServer.BBS.Model;
using System.Data;
using BaiRong.Model;
using System.Collections;
using SiteServer.BBS.Core;
using BaiRong.Core;

using SiteServer.CMS.Core;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class OnlineDAO : DataProviderBase, IOnlineDAO
    {
        public void Insert(int publishmentSystemID, OnlineInfo onlineInfo)
        {
            onlineInfo.ActiveTime = DateTime.Now;

            string sqlString = "INSERT INTO bbs_Online(PublishmentSystemID, UserName, SessionID, ActiveTime, IPAddress, IsHide) VALUES(@PublishmentSystemID, @UserName, @SessionID, @ActiveTime, @IPAddress, @IsHide)";

            IDbDataParameter[] param = new IDbDataParameter[]
			{
                this.GetParameter("@PublishmentSystemID", EDataType.Integer, onlineInfo.PublishmentSystemID),
                this.GetParameter("@UserName", EDataType.NVarChar, 50, onlineInfo.UserName),
                this.GetParameter("@SessionID", EDataType.VarChar, 50, onlineInfo.SessionID),
                this.GetParameter("@ActiveTime", EDataType.DateTime, onlineInfo.ActiveTime),
                this.GetParameter("@IPAddress", EDataType.VarChar, 50, onlineInfo.IPAddress),
                this.GetParameter("@IsHide", EDataType.VarChar, 18, onlineInfo.IsHide.ToString())
            };

            this.ExecuteNonQuery(sqlString, param);

            OnlineManager.InsertCache(publishmentSystemID, onlineInfo);
        }

        public void DeleteByUserName(int publishmentSystemID, string userName)
        {
            string sqlString = string.Format("DELETE bbs_Online WHERE PublishmentSystemID = {0} AND UserName=@UserName", publishmentSystemID);

            IDbDataParameter[] deleteParam = new IDbDataParameter[]
			{                 
                this.GetParameter("@UserName", EDataType.NVarChar, 50, userName.Trim())                 
            };

            int result = base.ExecuteNonQuery(sqlString, deleteParam);
            if (result > 0)
            {
                OnlineManager.DeleteCache(publishmentSystemID, userName, string.Empty);
            }
        }

        public void DeleteBySessionID(int publishmentSystemID, string sessionID)
        {
            string sqlString = string.Format("DELETE bbs_Online WHERE PublishmentSystemID = {0} AND SessionID='{1}'", publishmentSystemID, sessionID);

            int result = base.ExecuteNonQuery(sqlString);
            if (result > 0)
            {
                OnlineManager.DeleteCache(publishmentSystemID, string.Empty, sessionID);
            }
        }

        public void RemoveTimeOutUsers(int publishmentSystemID)
        {
            ConfigurationInfoExtend additional = ConfigurationManager.GetAdditional(publishmentSystemID);

            string sqlString = string.Format("DELETE bbs_Online WHERE PublishmentSystemID = {0} AND ActiveTime < '{1}'", publishmentSystemID, DateTime.Now.AddMinutes(0 - additional.OnlineTimeout));

            int result = base.ExecuteNonQuery(sqlString);
            if (result > 0)
            {
                OnlineManager.ClearCache(publishmentSystemID);
            }
        }

        /// <summary>
        ///更新用户活动时间。
        /// </summary>
        public void ActiveTime(OnlineInfo onlineInfo)
        {
            if (onlineInfo != null && onlineInfo.ID > 0)
            {
                if (onlineInfo.ID > 2000000000)
                {
                    string sqlString = "TRUNCATE TABLE bbs_Online";
                    base.ExecuteNonQuery(sqlString);
                }
                else if (onlineInfo.ActiveTime.AddMinutes(5) < DateTime.Now)
                {
                    if (!string.IsNullOrEmpty(onlineInfo.UserName))
                    {
                        int seconds = new TimeSpan(DateTime.Now.Ticks - onlineInfo.ActiveTime.Ticks).Seconds;
                        string groupSN = PublishmentSystemManager.GetGroupSN(onlineInfo.PublishmentSystemID);
                        BaiRongDataProvider.UserDAO.AddOnlineSeconds(groupSN, onlineInfo.UserName, seconds);
                    }

                    string sqlString = "UPDATE bbs_Online SET ActiveTime = getdate() WHERE ID=" + onlineInfo.ID;
                    base.ExecuteNonQuery(sqlString);
                }
            }
        }

        public Hashtable GetOnlineInfoHashtable(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();

            string sqlString = string.Format("SELECT ID, PublishmentSystemID, UserName, SessionID, ActiveTime, IPAddress, IsHide FROM bbs_Online WHERE PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    OnlineInfo onlineInfo = new OnlineInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), rdr.GetDateTime(4), rdr.GetValue(5).ToString(), TranslateUtils.ToBool(rdr.GetValue(6).ToString()));

                    if (!string.IsNullOrEmpty(onlineInfo.UserName))
                    {
                        hashtable[onlineInfo.UserName] = onlineInfo;
                    }
                    else
                    {
                        hashtable[onlineInfo.SessionID] = onlineInfo;
                    }
                }
                rdr.Close();
            }

            return hashtable;
        }
    }
}
