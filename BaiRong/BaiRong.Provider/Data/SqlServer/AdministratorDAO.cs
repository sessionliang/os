using System;
using System.Data;
using System.Collections;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Web;
using System.Text;
using BaiRong.Core.ActiveDirectory;
using System.Web.Security;
using System.Collections.Generic;

namespace BaiRong.Provider.Data.SqlServer
{
    public class AdministratorDAO : DataProviderBase, IAdministratorDAO
    {
        public const string TableName = "bairong_Administrator";

        private const string SQL_SELECT_USER = "SELECT [UserName], [Password], [PasswordFormat], [PasswordSalt], [CreationDate], [LastActivityDate], [LastProductID], [CountOfLogin], [CreatorUserName], [IsLockedOut], [PublishmentSystemIDCollection], [PublishmentSystemID], [DepartmentID], [AreaID], [DisplayName], [Question], [Answer], [Email], [Mobile], [Theme], [Language] FROM bairong_Administrator WHERE [UserName] = @UserName";

        private const string SQL_SELECT_USERNAME = "SELECT [UserName] FROM bairong_Administrator WHERE [UserName] = @UserName";

        private const string SQL_SELECT_CREATOR_USER_NAME = "SELECT [CreatorUserName] FROM bairong_Administrator WHERE [UserName] = @UserName";

        private const string SQL_SELECT_DISPLAY_NAME = "SELECT [DisplayName] FROM bairong_Administrator WHERE [UserName] = @UserName";

        private const string SQL_SELECT_DEPARTMENT_ID = "SELECT [DepartmentID] FROM bairong_Administrator WHERE [UserName] = @UserName";

        private const string SQL_SELECT_AREA_ID = "SELECT [AreaID] FROM bairong_Administrator WHERE [UserName] = @UserName";

        private const string SQL_SELECT_LAST_MODULEID = "SELECT [LastProductID] FROM bairong_Administrator WHERE [UserName] = @UserName";

        private const string SQL_SELECT_THEME = "SELECT [Theme] FROM bairong_Administrator WHERE [UserName] = @UserName";

        private const string SQL_SELECT_LANGUAGE = "SELECT [Language] FROM bairong_Administrator WHERE [UserName] = @UserName";

        private const string SQL_SELECT_PUBLISHMENTSYSTEMID_COLLECTION = "SELECT [PublishmentSystemIDCollection] FROM bairong_Administrator WHERE [UserName] = @UserName";

        private const string SQL_SELECT_PUBLISHMENTSYSTEMID = "SELECT [PublishmentSystemID] FROM bairong_Administrator WHERE [UserName] = @UserName";

        private const string SQL_SELECT_ALL_USER = "SELECT [UserName], [Password], [PasswordFormat], [PasswordSalt], [CreationDate], [LastActivityDate], [LastProductID], [CountOfLogin], [CreatorUserName], [IsLockedOut], [PublishmentSystemIDCollection], [PublishmentSystemID], [DepartmentID], [AreaID], [DisplayName], [Question], [Answer], [Email], [Mobile], [Theme], [Language] FROM bairong_Administrator ORDER BY [UserName]";

        private const string SQL_INSERT_USER = "INSERT INTO bairong_Administrator ([UserName], [Password], [PasswordFormat], [PasswordSalt], [CreationDate], [LastActivityDate], [LastProductID], [CountOfLogin], [CreatorUserName], [IsLockedOut], [PublishmentSystemIDCollection], [PublishmentSystemID], [DepartmentID], [AreaID], [DisplayName], [Question], [Answer], [Email], [Mobile], [Theme], [Language]) VALUES (@UserName, @Password, @PasswordFormat, @PasswordSalt, @CreationDate, @LastActivityDate, @LastProductID, @CountOfLogin, @CreatorUserName, @IsLockedOut, @PublishmentSystemIDCollection, @PublishmentSystemID, @DepartmentID, @AreaID, @DisplayName, @Question, @Answer, @Email, @Mobile, @Theme, @Language)";

        private const string SQL_UPDATE_USER = "UPDATE bairong_Administrator SET [LastActivityDate] = @LastActivityDate, [LastProductID] = @LastProductID, [CountOfLogin] = @CountOfLogin, [IsLockedOut] = @IsLockedOut, [PublishmentSystemIDCollection] = @PublishmentSystemIDCollection, [PublishmentSystemID] = @PublishmentSystemID, [DepartmentID] = @DepartmentID, [AreaID] = @AreaID, [DisplayName] = @DisplayName, [Question] = @Question, [Answer] = @Answer, [Email] = @Email, [Mobile] = @Mobile, [Theme] = @Theme, [Language] = @Language WHERE [UserName] = @UserName";

        private const string SQL_UPDATE_LAST_ACTIVITY_DATE = "UPDATE bairong_Administrator SET [LastActivityDate] = @LastActivityDate, [LastProductID] = @LastProductID, [CountOfLogin] = [CountOfLogin] + 1 WHERE [UserName] = @UserName";

        private const string SQL_UPDATE_LAST_ACTIVITY_DATE_AND_PUBLISHMENTSYSTEMID = "UPDATE bairong_Administrator SET [LastActivityDate] = @LastActivityDate, [LastProductID] = @LastProductID, [PublishmentSystemID] = @PublishmentSystemID, [CountOfLogin] = [CountOfLogin] + 1 WHERE [UserName] = @UserName";

        private const string SQL_UPDATE_LAST_ACTIVITY_DATE_AND_PRODUCTID = "UPDATE bairong_Administrator SET [LastActivityDate] = @LastActivityDate, [LastProductID] = @LastProductID, [CountOfLogin] = [CountOfLogin] + 1 WHERE [UserName] = @UserName";

        private const string SQL_UPDATE_PUBLISHMENTSYSTEMID_COLLECTION = "UPDATE bairong_Administrator SET [PublishmentSystemIDCollection] = @PublishmentSystemIDCollection WHERE [UserName] = @UserName";

        private const string SQL_UPDATE_PUBLISHMENTSYSTEMID = "UPDATE bairong_Administrator SET [PublishmentSystemID] = @PublishmentSystemID WHERE [UserName] = @UserName";

        private const string SQL_UPDATE_PASSWORD = "UPDATE bairong_Administrator SET [Password] = @Password, [PasswordFormat] = @PasswordFormat, [PasswordSalt] = @PasswordSalt WHERE [UserName] = @UserName";

        private const string SQL_DELETE_USER = "DELETE FROM bairong_Administrator WHERE [UserName] = @UserName";

        private const string PARM_USERNAME = "@UserName";
        private const string PARM_PASSWORD = "@Password";
        private const string PARM_PASSWORD_FORMAT = "@PasswordFormat";
        private const string PARM_PASSWORD_SALT = "@PasswordSalt";
        private const string PARM_CREATION_DATE = "@CreationDate";
        private const string PARM_LAST_ACTIVITY_DATE = "@LastActivityDate";
        private const string PARM_LAST_PRODUCT_ID = "@LastProductID";
        private const string PARM_COUNT_OF_LOGIN = "@CountOfLogin";
        private const string PARM_CREATOR_USERNAME = "@CreatorUserName";
        private const string PARM_IS_LOCKED_OUT = "@IsLockedOut";
        private const string PARM_PUBLISHMENTSYSTEMID_COLLECTION = "@PublishmentSystemIDCollection";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_DEPARTMENT_ID = "@DepartmentID";
        private const string PARM_AREA_ID = "@AreaID";
        private const string PARM_DISPLAYNAME = "@DisplayName";
        private const string PARM_QUESTION = "@Question";
        private const string PARM_ANSWER = "@Answer";
        private const string PARM_EMAIL = "@Email";
        private const string PARM_MOBILE = "@Mobile";
        private const string PARM_THEME = "@Theme";
        private const string PARM_LANGUAGE = "@Language";

        public void Insert(AdministratorInfo info)
        {
            IDbDataParameter[] insertParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, info.UserName),
                this.GetParameter(PARM_PASSWORD, EDataType.NVarChar, 255, info.Password),
                this.GetParameter(PARM_PASSWORD_FORMAT, EDataType.VarChar, 50, EPasswordFormatUtils.GetValue(info.PasswordFormat)),
                this.GetParameter(PARM_PASSWORD_SALT, EDataType.NVarChar, 128, info.PasswordSalt),
                this.GetParameter(PARM_CREATION_DATE, EDataType.DateTime, info.CreationDate),
                this.GetParameter(PARM_LAST_ACTIVITY_DATE, EDataType.DateTime, info.LastActivityDate),
                this.GetParameter(PARM_LAST_PRODUCT_ID, EDataType.VarChar, 50, info.LastProductID),
                this.GetParameter(PARM_COUNT_OF_LOGIN, EDataType.Integer, info.CountOfLogin),
                this.GetParameter(PARM_CREATOR_USERNAME, EDataType.NVarChar, 255, info.CreatorUserName),
                this.GetParameter(PARM_IS_LOCKED_OUT, EDataType.VarChar, 18, info.IsLockedOut.ToString()),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID_COLLECTION, EDataType.VarChar, 50, info.PublishmentSystemIDCollection),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter(PARM_DEPARTMENT_ID, EDataType.Integer, info.DepartmentID),
                this.GetParameter(PARM_AREA_ID, EDataType.Integer, info.AreaID),
                this.GetParameter(PARM_DISPLAYNAME, EDataType.NVarChar, 255, info.DisplayName),
                this.GetParameter(PARM_QUESTION, EDataType.NVarChar, 255, info.Question),
                this.GetParameter(PARM_ANSWER, EDataType.NVarChar, 255, info.Answer),
                this.GetParameter(PARM_EMAIL, EDataType.NVarChar, 255, info.Email),
                this.GetParameter(PARM_MOBILE, EDataType.VarChar, 20, info.Mobile),
                this.GetParameter(PARM_THEME, EDataType.VarChar, 50, info.Theme),
                this.GetParameter(PARM_LANGUAGE, EDataType.VarChar, 50, info.Language)
            };

            this.ExecuteNonQuery(SQL_INSERT_USER, insertParms);

            BaiRongDataProvider.DepartmentDAO.UpdateCountOfAdmin();
            BaiRongDataProvider.AreaDAO.UpdateCountOfAdmin();
        }

        public void Update(AdministratorInfo info)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_LAST_ACTIVITY_DATE, EDataType.DateTime, info.LastActivityDate),
                this.GetParameter(PARM_LAST_PRODUCT_ID, EDataType.VarChar, 50, info.LastProductID),
                this.GetParameter(PARM_COUNT_OF_LOGIN, EDataType.Integer, info.CountOfLogin),
                this.GetParameter(PARM_IS_LOCKED_OUT, EDataType.VarChar, 18, info.IsLockedOut.ToString()),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID_COLLECTION, EDataType.VarChar, 50, info.PublishmentSystemIDCollection),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter(PARM_DEPARTMENT_ID, EDataType.Integer, info.DepartmentID),
                this.GetParameter(PARM_AREA_ID, EDataType.Integer, info.AreaID),
                this.GetParameter(PARM_DISPLAYNAME, EDataType.NVarChar, 255, info.DisplayName),
                this.GetParameter(PARM_QUESTION, EDataType.NVarChar, 255, info.Question),
                this.GetParameter(PARM_ANSWER, EDataType.NVarChar, 255, info.Answer),
                this.GetParameter(PARM_EMAIL, EDataType.NVarChar, 255, info.Email),
                this.GetParameter(PARM_MOBILE, EDataType.VarChar, 20, info.Mobile),
                this.GetParameter(PARM_THEME, EDataType.VarChar, 50, info.Theme),
                this.GetParameter(PARM_LANGUAGE, EDataType.VarChar, 50, info.Language),
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, info.UserName)
            };

            this.ExecuteNonQuery(SQL_UPDATE_USER, parms);

            BaiRongDataProvider.DepartmentDAO.UpdateCountOfAdmin();
            BaiRongDataProvider.AreaDAO.UpdateCountOfAdmin();

            AdminManager.RemoveCache(info.UserName);
        }

        public void UpdateLastActivityDateAndPublishmentSystemID(string userName, int publishmentSystemID)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                IDbDataParameter[] updateParms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_LAST_ACTIVITY_DATE, EDataType.DateTime, DateTime.Now),
                    this.GetParameter(PARM_LAST_PRODUCT_ID, EDataType.VarChar, 50, ProductManager.Apps.ProductID),
                    this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                    this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
                };

                this.ExecuteNonQuery(SQL_UPDATE_LAST_ACTIVITY_DATE_AND_PUBLISHMENTSYSTEMID, updateParms);

                AdminManager.RemoveCache(userName);
            }
        }

        public void UpdateLastActivityDateAndProductID(string userName, string lastProductID)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                if (string.IsNullOrEmpty(lastProductID))
                {
                    lastProductID = ProductManager.Apps.ProductID;
                }
                IDbDataParameter[] updateParms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_LAST_ACTIVITY_DATE, EDataType.DateTime, DateTime.Now),
                    this.GetParameter(PARM_LAST_PRODUCT_ID, EDataType.VarChar, 50, lastProductID),
                    this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
                };

                this.ExecuteNonQuery(SQL_UPDATE_LAST_ACTIVITY_DATE_AND_PRODUCTID, updateParms);

                AdminManager.RemoveCache(userName);
            }
        }

        public void UpdateLastActivityDate(string userName, string lastProductID)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                if (string.IsNullOrEmpty(lastProductID))
                {
                    lastProductID = string.Empty;
                }
                IDbDataParameter[] updateParms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_LAST_ACTIVITY_DATE, EDataType.DateTime, DateTime.Now),
                    this.GetParameter(PARM_LAST_PRODUCT_ID, EDataType.VarChar, 50, lastProductID),
                    this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
                };

                this.ExecuteNonQuery(SQL_UPDATE_LAST_ACTIVITY_DATE, updateParms);

                AdminManager.RemoveCache(userName);
            }
        }

        public void UpdatePublishmentSystemIDCollection(string userName, string publishmentSystemIDCollection)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                IDbDataParameter[] updateParms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_PUBLISHMENTSYSTEMID_COLLECTION, EDataType.VarChar, 50, publishmentSystemIDCollection),
                    this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
                };

                this.ExecuteNonQuery(SQL_UPDATE_PUBLISHMENTSYSTEMID_COLLECTION, updateParms);

                AdminManager.RemoveCache(userName);
            }
        }

        public void UpdatePublishmentSystemID(string userName, int publishmentSystemID)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                IDbDataParameter[] updateParms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                    this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
                };

                this.ExecuteNonQuery(SQL_UPDATE_PUBLISHMENTSYSTEMID, updateParms);

                AdminManager.RemoveCache(userName);
            }
        }

        private bool ChangePassword(string userName, EPasswordFormat passwordFormat, string passwordSalt, string password)
        {
            bool isSuccess = false;
            IDbDataParameter[] updateParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PASSWORD, EDataType.NVarChar, 255, password),
                this.GetParameter(PARM_PASSWORD_FORMAT, EDataType.VarChar, 50, EPasswordFormatUtils.GetValue(passwordFormat)),
                this.GetParameter(PARM_PASSWORD_SALT, EDataType.NVarChar, 128, passwordSalt),
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
            };

            try
            {
                this.ExecuteNonQuery(SQL_UPDATE_PASSWORD, updateParms);

                AdminManager.RemoveCache(userName);

                //更新cache中密码版本
                var newAuth = Guid.NewGuid().ToString().Replace("-", "");

                Dictionary<string, string> PWDEditionCache = (Dictionary<string, string>)System.Web.HttpContext.Current.Application.Get("PECACHE");
                if (PWDEditionCache == null)
                {
                    PWDEditionCache = new Dictionary<string, string>();
                }
                PWDEditionCache[userName] = newAuth;
                System.Web.HttpContext.Current.Application.Set("PECACHE", PWDEditionCache);

                if (userName == UserName)
                {
                    //更新用户cookie中的密码版本
                    HttpCookie cookie = new HttpCookie("PEValue", newAuth);
                    cookie.Path = AdminAuthConfig.FormsCookiePath;
                    cookie.Secure = false;
                    cookie.Expires = DateTime.Now.AddYears(50);
                    CookieUtils.SetCookie(cookie);
                }
                isSuccess = true;
            }
            catch { }
            return isSuccess;
        }

        public void Delete(string userName)
        {
            IDbDataParameter[] deleteParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
            };

            this.ExecuteNonQuery(SQL_DELETE_USER, deleteParms);

            AdminManager.RemoveCache(userName);

            BaiRongDataProvider.DepartmentDAO.UpdateCountOfAdmin();
            BaiRongDataProvider.AreaDAO.UpdateCountOfAdmin();
        }

        public void LockUsers(ArrayList userNameArrayList, bool isLockOut)
        {
            string parameterNameList = string.Empty;
            List<IDbDataParameter> parameterList = this.GetINParameterList(PARM_USERNAME, EDataType.NVarChar, 255, userNameArrayList, out parameterNameList);

            string SQL_UPDATE_LOCK = string.Format("UPDATE bairong_Administrator SET IsLockedOut = @IsLockedOut WHERE [UserName] IN ({0})", parameterNameList);

            List<IDbDataParameter> paramList = new List<IDbDataParameter>();
            paramList.Add(this.GetParameter(PARM_IS_LOCKED_OUT, EDataType.VarChar, 18, isLockOut.ToString()));
            paramList.AddRange(parameterList);

            //string sqlString = string.Format("UPDATE bairong_Administrator SET IsLockedOut = '{0}' WHERE [UserName] IN ({1})", isLockOut.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithQuote(userNameArrayList));

            this.ExecuteNonQuery(SQL_UPDATE_LOCK, paramList.ToArray());

            AdminManager.Clear();
        }

        public AdministratorInfo GetAdministratorInfo(string userName)
        {
            AdministratorInfo info = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_USER, parms))
            {
                if (rdr.Read())
                {
                    info = new AdministratorInfo(rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString(), EPasswordFormatUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetValue(3).ToString(), rdr.GetDateTime(4), rdr.GetDateTime(5), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetValue(8).ToString(), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetValue(10).ToString(), rdr.GetInt32(11), rdr.GetInt32(12), rdr.GetInt32(13), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetValue(16).ToString(), rdr.GetValue(17).ToString(), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString(), rdr.GetValue(20).ToString());
                }
                rdr.Close();
            }

            return info;
        }

        public int GetDepartmentID(string userName)
        {
            int departmentID = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_DEPARTMENT_ID, parms))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    departmentID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return departmentID;
        }

        public int GetAreaID(string userName)
        {
            int areaID = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_AREA_ID, parms))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    areaID = rdr.GetInt32(0);
                }
                rdr.Close();
            }

            return areaID;
        }

        public string GetSelectCommand(bool isConsoleAdministrator, string creatorUserName, int departmentID)
        {
            string sqlString = string.Empty;
            if (departmentID == 0)
            {
                sqlString = "SELECT [UserName], [Password], [PasswordFormat], [PasswordSalt], [CreationDate], [LastActivityDate], [LastProductID], [CountOfLogin], [CreatorUserName], [IsLockedOut], [PublishmentSystemIDCollection], [PublishmentSystemID], [DepartmentID], [AreaID], [DisplayName], [Question], [Answer], [Email], [Mobile], [Theme], [Language] FROM bairong_Administrator";
                if (!isConsoleAdministrator)
                {
                    sqlString = string.Format("SELECT [UserName], [Password], [PasswordFormat], [PasswordSalt], [CreationDate], [LastActivityDate], [LastProductID], [CountOfLogin], [CreatorUserName], [IsLockedOut], [PublishmentSystemIDCollection], [PublishmentSystemID], [DepartmentID], [AreaID], [DisplayName], [Question], [Answer], [Email], [Mobile], [Theme], [Language] FROM bairong_Administrator WHERE CreatorUserName = '{0}'", PageUtils.FilterSql(creatorUserName));
                }
            }
            else
            {
                sqlString = string.Format("SELECT [UserName], [Password], [PasswordFormat], [PasswordSalt], [CreationDate], [LastActivityDate], [LastProductID], [CountOfLogin], [CreatorUserName], [IsLockedOut], [PublishmentSystemIDCollection], [PublishmentSystemID], [DepartmentID], [AreaID], [DisplayName], [Question], [Answer], [Email], [Mobile], [Theme], [Language] FROM bairong_Administrator WHERE [DepartmentID] = {0}", departmentID);
                if (!isConsoleAdministrator)
                {
                    sqlString = string.Format("SELECT [UserName], [Password], [PasswordFormat], [PasswordSalt], [CreationDate], [LastActivityDate], [LastProductID], [CountOfLogin], [CreatorUserName], [IsLockedOut], [PublishmentSystemIDCollection], [PublishmentSystemID], [DepartmentID], [AreaID], [DisplayName], [Question], [Answer], [Email], [Mobile], [Theme], [Language] FROM bairong_Administrator WHERE CreatorUserName = '{0}' AND [DepartmentID] = {1}", PageUtils.FilterSql(creatorUserName), departmentID);
                }
            }
            return sqlString;
        }

        public string GetSelectCommand(string searchWord, string roleName, int dayOfLastActivity, bool isConsoleAdministrator, string creatorUserName, int departmentID, int areaID)
        {
            StringBuilder whereBuilder = new StringBuilder();

            if (dayOfLastActivity > 0)
            {
                DateTime dateTime = DateTime.Now.AddDays(-dayOfLastActivity);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereBuilder.AppendFormat("(to_char(LastActivityDate,'YYYY-MM-DD') >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
                }
                whereBuilder.AppendFormat("(LastActivityDate >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(searchWord))
            {
                if (whereBuilder.Length > 0)
                {
                    whereBuilder.AppendFormat(" AND ");
                }
                whereBuilder.AppendFormat("(UserName LIKE '%{0}%' OR EMAIL LIKE '%{0}%' OR DisplayName LIKE '%{0}%')", PageUtils.FilterSql(searchWord));
            }

            if (!isConsoleAdministrator)
            {
                if (whereBuilder.Length > 0)
                {
                    whereBuilder.AppendFormat(" AND ");
                }
                whereBuilder.AppendFormat("CreatorUserName = '{0}'", PageUtils.FilterSql(creatorUserName));
            }

            if (departmentID != 0)
            {
                if (whereBuilder.Length > 0)
                {
                    whereBuilder.AppendFormat(" AND ");
                }
                whereBuilder.AppendFormat("DepartmentID = {0}", departmentID);
            }

            if (areaID != 0)
            {
                if (whereBuilder.Length > 0)
                {
                    whereBuilder.AppendFormat(" AND ");
                }
                whereBuilder.AppendFormat("AreaID = {0}", areaID);
            }

            string whereString = string.Empty;
            if (!string.IsNullOrEmpty(roleName))
            {
                if (whereBuilder.Length > 0)
                {
                    whereString = string.Format("AND {0}", whereBuilder.ToString());
                }
                whereString = string.Format("WHERE (UserName IN (SELECT UserName FROM bairong_AdministratorsInRoles WHERE RoleName = '{0}')) {1}", PageUtils.FilterSql(roleName), whereString);
            }
            else
            {
                if (whereBuilder.Length > 0)
                {
                    whereString = string.Format("WHERE {0}", whereBuilder.ToString());
                }
            }

            string sqlString = "SELECT [UserName], [Password], [PasswordFormat], [PasswordSalt], [CreationDate], [LastActivityDate], [LastProductID], [CountOfLogin], [CreatorUserName], [IsLockedOut], [PublishmentSystemIDCollection], [PublishmentSystemID], [DepartmentID], [AreaID], [DisplayName], [Question], [Answer], [Email], [Mobile], [Theme], [Language] FROM bairong_Administrator " + whereString;

            return sqlString;
        }

        public string GetSortFieldName()
        {
            return "UserName";
        }

        public int GetNumberOfUsersOnline(int userIsOnlineTimeWindow)
        {
            int count = 0;
            string sqlString = @"SELECT COUNT(*) FROM bairong_Administrator WHERE [LastActivityDate] > DATEADD(minute,  -(@MinutesSinceLastInActive), @CurrentTimeUtc)";

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter("@MinutesSinceLastInActive", EDataType.Integer, userIsOnlineTimeWindow),
                this.GetParameter("@CurrentTimeUtc", EDataType.DateTime, DateTime.Now)
            };

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        count = Convert.ToInt32(rdr.GetInt32(0));
                    }
                }
                rdr.Close();
            }

            return count;
        }

        public bool IsUserNameExists(string userName)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_USERNAME, parms))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    exists = true;
                }
                rdr.Close();
            }
            return exists;
        }

        public string GetCreatorUserName(string userName)
        {
            string creatorUserName = string.Empty;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_CREATOR_USER_NAME, parms))
            {
                if (rdr.Read())
                {
                    creatorUserName = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return creatorUserName;
        }

        public string GetDisplayName(string userName)
        {
            string displayName = string.Empty;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_DISPLAY_NAME, parms))
            {
                if (rdr.Read())
                {
                    displayName = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return (!string.IsNullOrEmpty(displayName)) ? displayName : userName;
        }

        public string GetTheme(string userName)
        {
            string theme = string.Empty;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_THEME, parms))
            {
                if (rdr.Read())
                {
                    theme = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return theme;
        }

        public string GetLanguage(string userName)
        {
            string language = string.Empty;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_LANGUAGE, parms))
            {
                if (rdr.Read())
                {
                    language = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return language;
        }

        public string GetLastProductID(string userName)
        {
            string lastModuleID = string.Empty;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_LAST_MODULEID, parms))
            {
                if (rdr.Read())
                {
                    lastModuleID = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return lastModuleID;
        }

        public List<int> GetPublishmentSystemIDList(string userName)
        {
            List<int> publishmentSystemIDList = new List<int>();

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PUBLISHMENTSYSTEMID_COLLECTION, parms))
            {
                if (rdr.Read())
                {
                    string collection = rdr.GetValue(0).ToString();
                    if (!string.IsNullOrEmpty(collection))
                    {
                        publishmentSystemIDList = TranslateUtils.StringCollectionToIntList(collection);
                    }
                }
                rdr.Close();
            }
            return publishmentSystemIDList;
        }

        public int GetPublishmentSystemID(string userName)
        {
            int publishmentSystemID = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USERNAME, EDataType.NVarChar, 255, userName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PUBLISHMENTSYSTEMID, parms))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    publishmentSystemID = rdr.GetInt32(0);
                }
                rdr.Close();
            }
            return publishmentSystemID;
        }

        public ArrayList GetUserNameArrayListByCreatorUserName(string creatorUserName)
        {
            ArrayList arraylist = new ArrayList();
            if (creatorUserName != null)
            {
                string sqlString = "SELECT UserName FROM bairong_Administrator WHERE [CreatorUserName] = @CreatorUserName";

                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_CREATOR_USERNAME, EDataType.NVarChar, 255, creatorUserName)
                };

                using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
                {
                    while (rdr.Read())
                    {
                        arraylist.Add(rdr.GetValue(0).ToString());
                    }
                    rdr.Close();
                }
            }
            return arraylist;
        }

        public ArrayList GetUserNameArrayList()
        {
            ArrayList arraylist = new ArrayList();
            string SQL_SELECT = "SELECT UserName FROM bairong_Administrator";

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetUserNameArrayList(List<int> departmentIDList)
        {
            ArrayList arraylist = new ArrayList();
            string SQL_SELECT = string.Format("SELECT UserName FROM bairong_Administrator WHERE DepartmentID IN ({0}) ORDER BY DepartmentID", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(departmentIDList));

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetUserNameArrayList(int departmentID, bool isAll)
        {
            ArrayList arraylist = new ArrayList();
            string SQL_SELECT = string.Format("SELECT UserName FROM bairong_Administrator WHERE DepartmentID = {0}", departmentID);
            if (isAll)
            {
                ArrayList departmentIDArrayList = BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListForDescendant(departmentID);
                departmentIDArrayList.Add(departmentID);
                SQL_SELECT = string.Format("SELECT UserName FROM bairong_Administrator WHERE DepartmentID IN ({0})", TranslateUtils.ObjectCollectionToString(departmentIDArrayList));
            }

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetUserNameArrayList(string searchWord, int dayOfCreation, int dayOfLastActivity, bool isChecked)
        {
            ArrayList arraylist = new ArrayList();

            string whereString = string.Empty;
            if (dayOfCreation > 0)
            {
                DateTime dateTime = DateTime.Now.AddDays(-dayOfCreation);
                whereString += string.Format(" AND (CreationDate >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
            }
            if (dayOfLastActivity > 0)
            {
                DateTime dateTime = DateTime.Now.AddDays(-dayOfLastActivity);
                whereString += string.Format(" AND (LastActivityDate >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(searchWord))
            {
                whereString += string.Format(" AND (UserName LIKE '%{0}%' OR EMAIL LIKE '%{0}%') ", PageUtils.FilterSql(searchWord));
            }

            string sqlString = string.Format("SELECT * FROM bairong_Administrator WHERE IsChecked = '{0}' {1}", isChecked.ToString(), whereString);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }
            return arraylist;
        }


        #region 方法

        public bool ChangePassword(string userName, EPasswordFormat passwordFormat, string password)
        {
            string passwordSalt;
            password = AdminAuthUtils.EncodePassword(password, passwordFormat, out passwordSalt);
            return this.ChangePassword(userName, passwordFormat, passwordSalt, password);
        }

        public bool CreateUser(AdministratorInfo userInfo, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrEmpty(userInfo.UserName))
            {
                errorMessage = "用户名不能为空。";
                return false;
            }
            if (string.IsNullOrEmpty(userInfo.Password))
            {
                errorMessage = "密码不能为空。";
                return false;
            }
            if (userInfo.Password.Length < 4)
            {
                errorMessage = "密码长度必须大于等于4。";
                return false;
            }
            if (this.IsUserNameExists(userInfo.UserName))
            {
                errorMessage = "用户名已存在。";
                return false;
            }
            try
            {
                string passwordSalt;
                userInfo.Password = AdminAuthUtils.EncodePassword(userInfo.Password, userInfo.PasswordFormat, out passwordSalt);
                userInfo.PasswordSalt = passwordSalt;
                this.Insert(userInfo);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public bool ValidateUser(string userName, string password, out string errorMessage)
        {
            bool isValid = false;
            errorMessage = string.Empty;

            AdministratorInfo adminInfo = this.GetAdministratorInfo(userName);
            if (adminInfo == null)
            {
                errorMessage = "用户名不存在。";
                return false;
            }
            if (adminInfo.IsLockedOut == true)
            {
                errorMessage = "用户被锁定，无法登录。";
                return false;
            }

            string pwd = adminInfo.Password;

            if (this.CheckPassword(password, pwd, adminInfo.PasswordFormat, adminInfo.PasswordSalt))
            {
                isValid = true;
            }
            else
            {
                errorMessage = "用户名或密码不正确。";
                return false;
            }

            return isValid;
        }

        public bool CheckPassword(string password, string dbpassword, EPasswordFormat passwordFormat, string passwordSalt)
        {
            string pass1 = password;
            string pass2 = AdminAuthUtils.DecodePassword(dbpassword, passwordFormat, passwordSalt);

            if (pass1 == pass2)
            {
                return true;
            }

            return false;
        }

        public string GetPassword(string password, EPasswordFormat passwordFormat, string passwordSalt)
        {
            return AdminAuthUtils.DecodePassword(password, passwordFormat, passwordSalt);
        }

        #endregion

        public string UserName
        {
            get
            {
                string userName = PageUtils.UrlDecode(CookieUtils.GetCookie(AdminAuthConfig.UserNameCookieName));
                if (!string.IsNullOrEmpty(userName))
                {
                    userName = userName.Replace("'", string.Empty);
                }
                else
                {
                    userName = string.Empty;
                }
                return userName;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                try
                {
                    string encryptedTicket = CookieUtils.GetCookie(AdminAuthConfig.AuthCookieName);
                    if (string.IsNullOrEmpty(encryptedTicket))
                    {
                        return false;
                    }

                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(encryptedTicket);
                    if (ticket == null)
                    {
                        return false;
                    }

                    if (ticket.Name != this.UserName)
                    {
                        return false;
                    }

                    string peValue = CookieUtils.GetCookie("PEValue");
                    if (string.IsNullOrEmpty(peValue))
                    {
                        return false;
                    }
                    Dictionary<string, string> PWDEditionCache = (Dictionary<string, string>)System.Web.HttpContext.Current.Application.Get("PECACHE");
                    if (PWDEditionCache != null)
                    {
                        if (PWDEditionCache.ContainsKey(this.UserName))
                        {
                            if (PWDEditionCache[this.UserName] != peValue)
                            {
                                return false;
                            }
                        }
                    }
                }
                catch
                {
                    return false;
                }

                return true;

                //return (CookieUtils.IsExists(AdminAuthConfig.AuthCookieName)) ? true : false;
            }
        }

        public string GetRedirectUrl(string userName, bool createPersistentCookie)
        {
            if (userName == null)
            {
                return null;
            }
            HttpContext current = HttpContext.Current;
            string str = current.Request["ReturnUrl"];
            if (str == null)
            {
                str = PageUtils.ParseNavigationUrl(string.Format("~/{0}/default.aspx", FileConfigManager.Instance.AdminDirectoryName));
            }
            return str;
        }

        public void RedirectFromLoginPage(string userName, bool createPersistentCookie)
        {
            if (userName != null)
            {
                Login(userName, createPersistentCookie);
                HttpContext.Current.Response.Redirect(GetRedirectUrl(userName, createPersistentCookie), false);
            }
        }

        public void Login(string userName, bool persistent)
        {
            CookieUtils.SetCookie(AdminAuthUtils.GetAuthCookie(userName, persistent));
            CookieUtils.SetCookie(AdminAuthUtils.GetUserNameCookie(userName, persistent));

            Dictionary<string, string> PWDEditionCache = (Dictionary<string, string>)System.Web.HttpContext.Current.Application.Get("PECACHE");
            if (PWDEditionCache == null)
            {
                PWDEditionCache = new Dictionary<string, string>();
            }
            if (!PWDEditionCache.ContainsKey(userName))
            {
                var newAuth = Guid.NewGuid().ToString().Replace("-", "");
                PWDEditionCache[userName] = newAuth;
                System.Web.HttpContext.Current.Application.Set("PECACHE", PWDEditionCache);
            }

            HttpCookie cookie = new HttpCookie("PEValue", PWDEditionCache[userName]);
            cookie.Path = AdminAuthConfig.FormsCookiePath;
            cookie.Secure = false;
            cookie.Expires = DateTime.Now.AddYears(50);
            CookieUtils.SetCookie(cookie);
        }

        public void Logout()
        {
            CookieUtils.Erase(AdminAuthConfig.AuthCookieName);
            CookieUtils.Erase(AdminAuthConfig.UserNameCookieName);
        }
    }
}
