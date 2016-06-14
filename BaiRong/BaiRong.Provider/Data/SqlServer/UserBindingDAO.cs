using System;
using System.Data;
using System.Collections;

using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core;

namespace BaiRong.Provider.Data.SqlServer
{
    public class UserBindingDAO : DataProviderBase, IUserBindingDAO
    {
        private const string SQL_DELETE_BY_USERID = "DELETE bairong_UserBinding WHERE ThirdLoginType = @ThirdLoginType AND UserID = @UserID";

        private const string SQL_DELETE_BY_THIRD_USERID = "DELETE bairong_UserBinding WHERE ThirdLoginUserID = @ThirdLoginUserID";

        private const string SQL_GET_BINDID_BY_USERID_TYPE = "SELECT ThirdLoginUserID FROM bairong_UserBinding WHERE ThirdLoginType = @ThirdLoginType AND UserID = @UserID";

        //private const string SQL_SELECT_BY_BINDING_ID = "SELECT UserName, BindingType, BindingID, BindingName FROM bairong_UserBinding WHERE UserName!='' AND BindingType = @BindingType AND BindingID = @BindingID";

        //private const string SQL_SELECT_BY_BINDING_USERNAME = "SELECT UserName, BindingType, BindingID, BindingName FROM bairong_UserBinding WHERE  BindingType = @BindingType AND BindingID = @BindingID";

        //private const string SQL_SELECT_BY_USERNAME = "SELECT UserName, BindingType, BindingID, BindingName FROM bairong_UserBinding WHERE BindingType = @BindingType AND UserName = @UserName";

        //private const string SQL_SELECT_BY_BINDUSERNAME = "SELECT UserName, BindingType, BindingID, BindingName FROM bairong_UserBinding WHERE BindingName = @BindingName";

        //private const string SQL_SELECT_ALL_UB = "SELECT UserName, BindingType, BindingID, BindingName FROM bairong_UserBinding WHERE UserName = @UserName";

        //private const string SQL_SELECT_ALL_BNAME = "SELECT UserName, BindingType, BindingID, BindingName FROM bairong_UserBinding WHERE BindingName = @BindingName";

        //private const string PARM_USERNAME = "@UserName";
        //private const string PARM_BINDING_TYPE = "@BindingType";
        //private const string PARM_BINDING_ID = "@BindingID";
        //private const string PARM_BINDING_NAME = "@BindingName";

        private const string PARM_USERID = "@UserID";
        private const string PARM_THIRD_LOGIN_TYPE = "@ThirdLoginType";
        private const string PARM_THIRD_LOGIN_USER_ID = "@ThirdLoginUserID";

        //public void Insert(UserBindingInfo info)
        //{
        //    string sqlString = "INSERT INTO bairong_UserBinding (UserName, BindingType, BindingID, BindingName) VALUES (@UserName, @BindingType, @BindingID, @BindingName)";

        //    IDbDataParameter[] parms = new IDbDataParameter[]
        //    {
        //        this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, info.UserName),
        //        this.GetParameter(PARM_BINDING_TYPE, EDataType.VarChar, 50, info.BindingType),
        //        this.GetParameter(PARM_BINDING_ID, EDataType.Integer, info.BindingID),
        //        this.GetParameter(PARM_BINDING_NAME, EDataType.NVarChar, 255, info.BindingName)
        //    };

        //    this.ExecuteNonQuery(sqlString, parms);
        //}

        //public void Update(UserBindingInfo info)
        //{
        //    string sqlString = "UPDATE bairong_UserBinding SET UserName=@UserName, BindingType=@BindingType, BindingID=@BindingID, BindingName=@BindingName WHERE BindingType=@BindingType AND BindingID=@BindingID";

        //    IDbDataParameter[] parms = new IDbDataParameter[]
        //    {
        //        this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, info.UserName),
        //        this.GetParameter(PARM_BINDING_TYPE, EDataType.VarChar, 50, info.BindingType),
        //        this.GetParameter(PARM_BINDING_ID, EDataType.Integer, info.BindingID),
        //        this.GetParameter(PARM_BINDING_NAME, EDataType.NVarChar, 255, info.BindingName)
        //    };

        //    this.ExecuteNonQuery(sqlString, parms);
        //}

        //public void Delete(int bindingID)
        //{
        //    string sqlString = "DELETE bairong_UserBinding WHERE BindingID = @BindingID";

        //    IDbDataParameter[] parms = new IDbDataParameter[]
        //    {
        //        this.GetParameter(PARM_BINDING_ID, EDataType.Integer, bindingID)
        //    };

        //    this.ExecuteNonQuery(sqlString, parms);
        //}


        //public UserBindingInfo GetUserBindingInfo(string bindingName)
        //{
        //    UserBindingInfo info = null;

        //    IDbDataParameter[] parms = new IDbDataParameter[]
        //    {
        //        this.GetParameter(PARM_BINDING_NAME, EDataType.NVarChar, 255, bindingName)

        //    };

        //    using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_BINDUSERNAME, parms))
        //    {
        //        if (rdr.Read())
        //        {
        //            info = new UserBindingInfo(rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetValue(3).ToString());
        //        }
        //        rdr.Close();
        //    }

        //    return info;
        //}

        //public UserBindingInfo GetUserBindingInfo(string bindingType, string userName)
        //{
        //    UserBindingInfo info = null;

        //    IDbDataParameter[] parms = new IDbDataParameter[]
        //    {
        //        this.GetParameter(PARM_BINDING_TYPE, EDataType.VarChar, 50, bindingType),
        //        this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
        //    };

        //    using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_USERNAME, parms))
        //    {
        //        if (rdr.Read())
        //        {
        //            info = new UserBindingInfo(rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetValue(3).ToString());
        //        }
        //        rdr.Close();
        //    }

        //    return info;
        //}

        //public UserBindingInfo GetUserBindingInfo(string bindingType, int bindingID)
        //{
        //    UserBindingInfo info = null;

        //    IDbDataParameter[] parms = new IDbDataParameter[]
        //    {
        //        this.GetParameter(PARM_BINDING_TYPE, EDataType.VarChar, 50, bindingType),
        //        this.GetParameter(PARM_BINDING_ID, EDataType.Integer, bindingID)
        //    };

        //    using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_BINDING_ID, parms))
        //    {
        //        if (rdr.Read())
        //        {
        //            info = new UserBindingInfo(rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetValue(3).ToString());
        //        }
        //        rdr.Close();
        //    }

        //    return info;
        //}

        //public UserBindingInfo GetUserBindingInfoByBindIDAndType(string bindingType, int bindingID)
        //{
        //    UserBindingInfo info = null;

        //    IDbDataParameter[] parms = new IDbDataParameter[]
        //    {
        //        this.GetParameter(PARM_BINDING_TYPE, EDataType.VarChar, 50, bindingType),
        //        this.GetParameter(PARM_BINDING_ID, EDataType.Integer, bindingID)
        //    };

        //    using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_BINDING_USERNAME, parms))
        //    {
        //        if (rdr.Read())
        //        {
        //            info = new UserBindingInfo(rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetValue(3).ToString());
        //        }
        //        rdr.Close();
        //    }

        //    return info;
        //}

        //public int GetBindingCount(string userName)
        //{
        //    int count = 0;

        //    string sqlString = "SELECT COUNT(*) AS NUM FROM bairong_UserBinding WHERE UserName = @UserName";

        //    IDbDataParameter[] parms = new IDbDataParameter[]
        //    {
        //        this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
        //    };

        //    using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
        //    {
        //        if (rdr.Read() && !rdr.IsDBNull(0))
        //        {
        //            count = rdr.GetInt32(0);
        //        }
        //        rdr.Close();
        //    }

        //    return count;
        //}

        //public IEnumerable GetDataSource(string userName)
        //{
        //    IDbDataParameter[] parms = new IDbDataParameter[]
        //    {
        //        this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
        //    };

        //    IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_UB, parms);
        //    return enumerable;
        //}

        //public IEnumerable GetBindNameDataSource(string bindName)
        //{
        //    IDbDataParameter[] parms = new IDbDataParameter[]
        //    {
        //        this.GetParameter(PARM_BINDING_NAME, EDataType.NVarChar, 255, bindName)
        //    };

        //    IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_BNAME, parms);
        //    return enumerable;
        //}



        public void DeleteByUserID(int userID, string thirdLoginType)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
				this.GetParameter(PARM_USERID, EDataType.Integer, userID),
                this.GetParameter(PARM_THIRD_LOGIN_TYPE,EDataType.VarChar,50,thirdLoginType)
			};

            this.ExecuteNonQuery(SQL_DELETE_BY_USERID, parms);
        }

        public void DeleteByThirdUserID(string thirdUserID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
				this.GetParameter(PARM_THIRD_LOGIN_USER_ID, EDataType.NVarChar,200, thirdUserID)
			};

            this.ExecuteNonQuery(SQL_DELETE_BY_THIRD_USERID, parms);
        }

        public string GetUserBindByUserAndType(int userID, string thirdLoginType)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERID,EDataType.Integer,userID),
                this.GetParameter(PARM_THIRD_LOGIN_TYPE,EDataType.VarChar,50,thirdLoginType)
            };

            object result = this.ExecuteScalar(SQL_GET_BINDID_BY_USERID_TYPE, parms);
            return result != null ? result.ToString() : string.Empty;
        }
    }
}