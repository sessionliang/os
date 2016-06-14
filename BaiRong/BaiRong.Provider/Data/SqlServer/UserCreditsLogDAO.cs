using System;
using System.Data;
using System.Collections;

using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;
using System.Collections.Generic;
using BaiRong.Core.Data;

namespace BaiRong.Provider.Data.SqlServer
{
    public class UserCreditsLogDAO : DataProviderBase, IUserCreditsLogDAO
    {
        private const string PARM_ID = "@ID";
        private const string PARM_USERNAME = "@UserName";
        private const string PARM_PRODUCT_ID = "@ProductID";
        private const string PARM_IS_INCREASED = "@IsIncreased";
        private const string PARM_NUM = "@Num";
        private const string PARM_ACTION = "@Action";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_ADD_DATE = "@AddDate";

        public void Insert(UserCreditsLogInfo info)
        {
            string sqlString = "INSERT INTO bairong_UserCreditsLog (UserName, ProductID, IsIncreased, Num, Action, Description, AddDate) VALUES (@UserName, @ProductID, @IsIncreased, @Num, @Action, @Description, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO bairong_UserCreditsLog (ID, UserName, ProductID, IsIncreased, Num, Action, Description, AddDate) VALUES (bairong_UserCreditsLog_SEQ.NEXTVAL, @UserName, @ProductID, @IsIncreased, @Num, @Action, @Description, @AddDate)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
				this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, info.UserName),
				this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, info.ProductID),
                this.GetParameter(PARM_IS_INCREASED, EDataType.VarChar, 18, info.IsIncreased.ToString()),
                this.GetParameter(PARM_NUM, EDataType.Integer, info.Num),
                this.GetParameter(PARM_ACTION, EDataType.NVarChar, 255, info.Action),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, info.Description),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, info.AddDate)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(ArrayList idArrayList)
        {
            if (idArrayList != null && idArrayList.Count > 0)
            {
                string deleteSqlString = string.Format("DELETE bairong_UserCreditsLog WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idArrayList));
                this.ExecuteNonQuery(deleteSqlString);
            }
        }

        public List<UserCreditsLogInfo> GetUserCreditsLogInfoList(string userName, string productID, string action)
        {
            List<UserCreditsLogInfo> userCreditsLogInfoList = new List<UserCreditsLogInfo>();

            string sqlString = string.Format("SELECT * FROM bairong_UserCreditsLog where UserName='{0}'", userName);

            if (!string.IsNullOrEmpty(productID))
            {
                sqlString += string.Format(" AND ProductID='{0}'", productID);
            }
            if (!string.IsNullOrEmpty(action))
            {
                sqlString += string.Format(" AND Action='{0}'", action);
            }
            sqlString += string.Format(" ORDER BY {0} DESC", "AddDate");

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    UserCreditsLogInfo info = new UserCreditsLogInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetDateTime(7));
                    userCreditsLogInfoList.Add(info);
                }
                rdr.Close();
            }

            return userCreditsLogInfoList;
        }

        public string GetSqlString(string userName)
        {
            return string.Format("SELECT ID, UserName, ProductID, IsIncreased, Num, Action, Description, AddDate FROM bairong_UserCreditsLog WHERE UserName = '{0}'", userName);
        }

        public string GetSqlString(string productID, ArrayList userNameArrayList)
        {

            string whereString = string.Format("WHERE ProductID = '{0}' AND  [UserName] IN ({1}) ", productID, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(userNameArrayList));

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString("bairong_UserCreditsLog", "*", whereString);

        }

        public string GetSortFieldName()
        {
            return "ID";
        }
    }
}