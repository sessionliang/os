using System;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using System.Text;
using BaiRong.Model;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;

using System.Security.Cryptography;
using BaiRong.Core.Cryptography;
using System.Web.Security;
using System.Collections.Generic;

namespace BaiRong.Provider.Data.SqlServer
{
    public class UserDAO : DataProviderBase, IUserDAO
    {
        public string TABLE_NAME { get { return "bairong_Users"; } }

        private const string SQL_SELECT_EMAIL = "SELECT [Email] FROM bairong_Users WHERE [UserID] = @UserID";

        private const string SQL_SELECT_USER_NAME = "SELECT [UserName] FROM bairong_Users WHERE [UserID] = @UserID";

        private const string SQL_SELECT_USER_ID = "SELECT [UserID] FROM bairong_Users WHERE [GroupSN] = @GroupSN AND [UserName] = @UserName";

        private const string SQL_SELECT_MOBILE = "SELECT [Mobile] FROM bairong_Users WHERE [UserID] = @UserID";

        private const string SQL_SELECT_BY_LOGIN_NAME = "SELECT [UserID], [GroupSN], [UserName], [Password], [PasswordFormat], [PasswordSalt], [GroupID], [Credits], [CreateDate], [CreateIPAddress], [LastActivityDate], [IsChecked], [IsLockedOut], [IsTemporary], [DisplayName], [Email], [Mobile], [OnlineSeconds], [AvatarLarge], [AvatarMiddle], [AvatarSmall], [Signature], [SettingsXML],[LoginNum], [Birthday], [BloodType], [Gender], [MaritalStatus], [Education], [Graduation], [Profession], [Address], [QQ], [WeiBo], [WeiXin], [Interests], [Organization], [Department], [Position], [NewGroupID], [MLibNum], [MLibValidityDate] FROM bairong_Users WHERE [GroupSN] = @GroupSN AND (UserName = @UserName OR Email = @Email OR Mobile = @Mobile)";

        private const string SQL_SELECT_BY_USER_NAME = "SELECT [UserID], [GroupSN], [UserName], [Password], [PasswordFormat], [PasswordSalt], [GroupID], [Credits], [CreateDate], [CreateIPAddress], [LastActivityDate], [IsChecked], [IsLockedOut], [IsTemporary], [DisplayName], [Email], [Mobile], [OnlineSeconds], [AvatarLarge], [AvatarMiddle], [AvatarSmall], [Signature], [SettingsXML], [LoginNum], [Birthday], [BloodType], [Gender], [MaritalStatus], [Education], [Graduation], [Profession], [Address], [QQ], [WeiBo], [WeiXin], [Interests], [Organization], [Department], [Position], [NewGroupID], [MLibNum], [MLibValidityDate]  FROM bairong_Users WHERE [GroupSN] = @GroupSN AND UserName = @UserName";


        private const string SQL_SELECT_BY_USER_NAME_EMAIL_PHONE = "SELECT [UserID], [GroupSN], [UserName], [Password], [PasswordFormat], [PasswordSalt], [GroupID], [Credits], [CreateDate], [CreateIPAddress], [LastActivityDate], [IsChecked], [IsLockedOut], [IsTemporary], [DisplayName], [Email], [Mobile], [OnlineSeconds], [AvatarLarge], [AvatarMiddle], [AvatarSmall], [Signature], [SettingsXML], [LoginNum], [Birthday], [BloodType], [Gender], [MaritalStatus], [Education], [Graduation], [Profession], [Address], [QQ], [WeiBo], [WeiXin], [Interests], [Organization], [Department], [Position], [NewGroupID], [MLibNum], [MLibValidityDate]  FROM bairong_Users WHERE [GroupSN] = @GroupSN AND (UserName = @UserName OR Email = @UserName OR Mobile = @UserName)";

        private const string SQL_INSERT_USER = "INSERT INTO bairong_Users ([GroupSN], [UserName], [Password], [PasswordFormat], [PasswordSalt], [GroupID], [Credits], [CreateDate], [CreateIPAddress], [LastActivityDate], [IsChecked], [IsLockedOut], [IsTemporary], [DisplayName], [Email], [Mobile], [OnlineSeconds], [AvatarLarge], [AvatarMiddle], [AvatarSmall], [Signature], [SettingsXML], [LoginNum], [Birthday], [BloodType], [Gender], [MaritalStatus], [Education], [Graduation], [Profession], [Address], [QQ], [WeiBo], [WeiXin], [Interests], [Organization], [Department], [Position],[NewGroupID],[MLibNum], [MLibValidityDate] ) VALUES (@GroupSN, @UserName, @Password, @PasswordFormat, @PasswordSalt, @GroupID, @Credits, @CreateDate, @CreateIPAddress, @LastActivityDate, @IsChecked, @IsLockedOut, @IsTemporary, @DisplayName, @Email, @Mobile, @OnlineSeconds, @AvatarLarge, @AvatarMiddle, @AvatarSmall, @Signature, @SettingsXML,@LoginNum, @Birthday, @BloodType, @Gender, @MaritalStatus, @Education, @Graduation, @Profession, @Address, @QQ, @WeiBo, @WeiXin, @Interests, @Organization, @Department, @Position,@NewGroupID,@MLibNum, @MLibValidityDate)";


        private const string SQL_UPDATE_USER = "UPDATE bairong_Users SET [GroupID] = @GroupID, [Credits] = @Credits, [LastActivityDate] = @LastActivityDate, [IsChecked] = @IsChecked, [IsLockedOut] = @IsLockedOut, [IsTemporary]=@IsTemporary,[DisplayName] = @DisplayName, [Email] = @Email, [Mobile] = @Mobile, [OnlineSeconds] = @OnlineSeconds,[AvatarLarge] = @AvatarLarge, [AvatarMiddle] = @AvatarMiddle, [AvatarSmall] = @AvatarSmall, [Signature] = @Signature, [SettingsXML] = @SettingsXML, [LoginNum] = @LoginNum, [Birthday]=@Birthday, [BloodType]=@BloodType,[Gender]=@Gender, [MaritalStatus]=@MaritalStatus, [Education]=@Education,[Graduation]=@Graduation, [Profession]=@Profession, [Address]=@Address, [QQ]=@QQ, [WeiBo]=@WeiBo, [WeiXin]=@WeiXin, [Interests]=@Interests, [Organization]=@Organization, [Department]=@Department, [Position]=@Position,[NewGroupID]=@NewGroupID,[MLibNum]=@MLibNum, [MLibValidityDate]=@MLibValidityDate WHERE [UserID] = @UserID";

        private const string SQL_UPDATEBASIC_USER = "UPDATE bairong_Users SET [Signature] = @Signature WHERE [UserID] = @UserID";

        private const string SQL_UPDATE_PASSWORD = "UPDATE bairong_Users SET [Password] = @Password, [PasswordFormat] = @PasswordFormat, [PasswordSalt] = @PasswordSalt WHERE [UserID] = @UserID";

        private const string SQL_DELETE_USER = "DELETE FROM bairong_Users WHERE [UserID] = @UserID";

        private const string PARM_USER_ID = "@UserID";
        private const string PARM_GROUP_SN = "@GroupSN";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_PASSWORD = "@Password";
        private const string PARM_PASSWORD_FORMAT = "@PasswordFormat";
        private const string PARM_PASSWORD_SALT = "@PasswordSalt";
        private const string PARM_GROUP_ID = "@GroupID";
        private const string PARM_CREDITS = "@Credits";
        private const string PARM_CREATE_DATE = "@CreateDate";
        private const string PARM_CREATE_IPADDRESS = "@CreateIPAddress";
        private const string PARM_LAST_ACTIVITY_DATE = "@LastActivityDate";
        private const string PARM_IS_CHECKED = "@IsChecked";
        private const string PARM_IS_LOCKED_OUT = "@IsLockedOut";
        private const string PARM_IS_TEMPORARY = "@IsTemporary";
        private const string PARM_DISPLAYNAME = "@DisplayName";
        private const string PARM_EMAIL = "@Email";
        private const string PARM_MOBILE = "@Mobile";
        private const string PARM_ONLINE_SECONDS = "@OnlineSeconds";
        private const string PARM_AVATAR_LARGE = "@AvatarLarge";
        private const string PARM_AVATAR_MIDDLE = "@AvatarMiddle";
        private const string PARM_AVATAR_SMALL = "@AvatarSmall";
        private const string PARM_SIGNATURE = "@Signature";
        private const string PARM_SETTINGS_XML = "@SettingsXML";

        private const string PARM_LOGIN_NUM = "@LoginNum";
        private const string PARM_BIRTHDAY = "@Birthday";
        private const string PARM_GENDER = "@Gender";
        private const string PARM_BLOOD_TYPE = "@BloodType";
        //private const string PARM_HEIGHT = "@Height";
        private const string PARM_MARITAL_STATUS = "@MaritalStatus";
        private const string PARM_EDUCATION = "@Education";
        private const string PARM_GRADUATION = "@Graduation";
        //private const string PARM_BODY_TYPE = "@BodyType";
        private const string PARM_PROFESSION = "@Profession";
        //private const string PARM_INCOME_LEVEL = "@IncomeLevel";

        private const string PARM_ADDRESS = "@Address";
        private const string PARM_QQ = "@QQ";
        private const string PARM_WEIBO = "@WeiBo";
        private const string PARM_WEIXIN = "@WeiXin";
        private const string PARM_INTERESTS = "@Interests";

        private const string PARM_ORGANIZATION = "@Organization";
        private const string PARM_DEPARTMENT = "@Department";
        private const string PARM_POSITION = "@Position";
        private const string PARM_NEWGROUPID = "@NewGroupID";
        private const string PARM_MLIBNUM = "@MLibNum";
        private const string PARM_MLIBVALIDITYDATE = "@MLibValidityDate";

        public void InsertWithoutValidation(UserInfo userInfo)
        {
            IDbDataParameter[] insertParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_ID, EDataType.Integer, userInfo.UserID),
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, userInfo.GroupSN),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userInfo.UserName),
                this.GetParameter(PARM_PASSWORD, EDataType.NVarChar, 255, userInfo.Password),
                this.GetParameter(PARM_PASSWORD_FORMAT, EDataType.VarChar, 50, EPasswordFormatUtils.GetValue(userInfo.PasswordFormat)),
                this.GetParameter(PARM_PASSWORD_SALT, EDataType.NVarChar, 128, userInfo.PasswordSalt),
                this.GetParameter(PARM_GROUP_ID, EDataType.Integer, userInfo.GroupID),
                this.GetParameter(PARM_CREDITS, EDataType.Integer, userInfo.Credits),
                this.GetParameter(PARM_CREATE_DATE, EDataType.DateTime, userInfo.CreateDate),
                this.GetParameter(PARM_CREATE_IPADDRESS, EDataType.VarChar, 50, userInfo.CreateIPAddress),
                this.GetParameter(PARM_LAST_ACTIVITY_DATE, EDataType.DateTime, userInfo.LastActivityDate),
                this.GetParameter(PARM_IS_CHECKED, EDataType.VarChar, 18, userInfo.IsChecked.ToString()),
                this.GetParameter(PARM_IS_LOCKED_OUT, EDataType.VarChar, 18, userInfo.IsLockedOut.ToString()),
                this.GetParameter(PARM_IS_TEMPORARY, EDataType.VarChar, 18, userInfo.IsTemporary.ToString()),
                this.GetParameter(PARM_DISPLAYNAME, EDataType.NVarChar, 255, userInfo.DisplayName),
                this.GetParameter(PARM_EMAIL, EDataType.NVarChar, 255, userInfo.Email),
                this.GetParameter(PARM_MOBILE, EDataType.VarChar, 20, userInfo.Mobile),
                this.GetParameter(PARM_ONLINE_SECONDS, EDataType.Integer, userInfo.OnlineSeconds),
                this.GetParameter(PARM_AVATAR_LARGE, EDataType.VarChar, 200, userInfo.AvatarLarge),
                this.GetParameter(PARM_AVATAR_MIDDLE, EDataType.VarChar, 200, userInfo.AvatarMiddle),
                this.GetParameter(PARM_AVATAR_SMALL, EDataType.VarChar, 200, userInfo.AvatarSmall),
                this.GetParameter(PARM_SIGNATURE, EDataType.NVarChar, 255, userInfo.Signature),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, userInfo.GetSettingsXML()),
                this.GetParameter(PARM_LOGIN_NUM,EDataType.Integer,userInfo.LoginNum),
                this.GetParameter(PARM_BIRTHDAY,EDataType.DateTime,userInfo.Birthday),
                this.GetParameter(PARM_BLOOD_TYPE,EDataType.NVarChar,255,userInfo.BloodType),
                this.GetParameter(PARM_GENDER,EDataType.NVarChar,255,userInfo.Gender),
                this.GetParameter(PARM_MARITAL_STATUS,EDataType.NVarChar,255,userInfo.MaritalStatus),
                this.GetParameter(PARM_EDUCATION,EDataType.NVarChar,255,userInfo.Education),
                this.GetParameter(PARM_GRADUATION,EDataType.NVarChar,255,userInfo.Graduation),
                this.GetParameter(PARM_PROFESSION,EDataType.NVarChar,255,userInfo.Profession),
                this.GetParameter(PARM_ADDRESS  ,EDataType.NVarChar,255,userInfo.Address),
                this.GetParameter(PARM_QQ,EDataType.NVarChar,255,userInfo.QQ),
                this.GetParameter(PARM_WEIBO,EDataType.NVarChar,255,userInfo.WeiBo),
                this.GetParameter(PARM_WEIXIN,EDataType.NVarChar,255,userInfo.WeiXin),
                this.GetParameter(PARM_INTERESTS,EDataType.NText,userInfo.Interests),
                this.GetParameter(PARM_ORGANIZATION,EDataType.NVarChar,255,userInfo.Organization),
                this.GetParameter(PARM_DEPARTMENT,EDataType.NVarChar,255,userInfo.Department),
                this.GetParameter(PARM_POSITION,EDataType.NVarChar,255,userInfo.Position),
                this.GetParameter(PARM_NEWGROUPID,EDataType.Integer,userInfo.NewGroupID),//by 20160119 增加新的用户组功能,
                this.GetParameter(PARM_MLIBNUM,EDataType.Integer,userInfo.MLibNum),//by 20160119 增加新的用户组功能
                this.GetParameter(PARM_MLIBVALIDITYDATE,EDataType.DateTime,userInfo.MLibValidityDate)//by 20160119 增加新的用户组功能
            };

            this.ExecuteNonQuery(SQL_INSERT_USER, insertParms);
        }

        public bool Insert(UserInfo userInfo, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrEmpty(userInfo.UserName))
            {
                errorMessage = "用户名不能为空";
                return false;
            }
            if (UserConfigManager.Additional.RegisterUserNameMinLength > 0 && userInfo.UserName.Length < UserConfigManager.Additional.RegisterUserNameMinLength)
            {
                errorMessage = string.Format("用户名长度必须大于等于{0}", UserConfigManager.Additional.RegisterUserNameMinLength);
                return false;
            }
            if (!userInfo.IsTemporary)
            {
                if (string.IsNullOrEmpty(userInfo.Password))
                {
                    errorMessage = "密码不能为空";
                    return false;
                }
                if (userInfo.Password.Length < UserConfigManager.Additional.UserMinPasswordLength)
                {
                    errorMessage = string.Format("密码长度必须大于等于{0}", UserConfigManager.Additional.UserMinPasswordLength);
                    return false;
                }
                if (!EUserPasswordRestrictionUtils.IsValid(userInfo.Password, UserConfigManager.Additional.RegisterPasswordRestriction))
                {
                    errorMessage = string.Format("密码不符合规则，请包含{0}", EUserPasswordRestrictionUtils.GetText(UserConfigManager.Additional.RegisterPasswordRestriction));
                    return false;
                }
            }
            if (!this.IsUserNameCompliant(userInfo.UserName.Replace("@", string.Empty).Replace(".", string.Empty)))
            {
                errorMessage = "用户名包含不规则字符，请更换用户名";
                return false;
            }
            if (!string.IsNullOrEmpty(userInfo.Email) && this.IsEmailExists(userInfo.GroupSN, userInfo.Email))
            {
                errorMessage = "电子邮件地址已被注册，请更换邮箱";
                return false;
            }
            if (!string.IsNullOrEmpty(userInfo.Mobile) && this.IsMobileExists(userInfo.GroupSN, userInfo.Mobile))
            {
                errorMessage = "手机号码已被注册，请更换手机号码";
                return false;
            }
            if (!string.IsNullOrEmpty(UserConfigManager.Additional.ReservedUserNames))
            {
                if (StringUtils.In(UserConfigManager.Additional.ReservedUserNames, userInfo.UserName))
                {
                    errorMessage = "用户名为系统保留关键字，请更换用户名";
                    return false;
                }
            }
            if (this.IsUserExists(userInfo.GroupSN, userInfo.UserName))
            {
                errorMessage = "用户名已被注册，请更换用户名";
                return false;
            }
            if (UserConfigManager.Additional.RegisterMinHoursOfIPAddress > 0)
            {
                UserInfo userInfoByIPAddress = this.GetUserInfoByCreateIPAddress(userInfo.CreateIPAddress);
                if (userInfoByIPAddress != null)
                {
                    TimeSpan ts = DateTime.Now - userInfoByIPAddress.CreateDate;
                    if (ts.Hours < UserConfigManager.Additional.RegisterMinHoursOfIPAddress)
                    {
                        errorMessage = "注册间隔过于频繁，请稍后再试";
                        return false;
                    }
                }
            }
            try
            {
                if (!userInfo.IsTemporary)
                {
                    string passwordSalt = GenerateSalt();
                    userInfo.Password = EncodePassword(userInfo.Password, userInfo.PasswordFormat, passwordSalt);
                    userInfo.PasswordSalt = passwordSalt;
                }

                this.InsertWithoutValidation(userInfo);

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public void Update(UserInfo userInfo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GROUP_ID, EDataType.Integer, userInfo.GroupID),
                this.GetParameter(PARM_CREDITS, EDataType.Integer, userInfo.Credits),
                this.GetParameter(PARM_LAST_ACTIVITY_DATE, EDataType.DateTime, userInfo.LastActivityDate),
                this.GetParameter(PARM_IS_CHECKED, EDataType.VarChar, 18, userInfo.IsChecked.ToString()),
                this.GetParameter(PARM_IS_LOCKED_OUT, EDataType.VarChar, 18, userInfo.IsLockedOut.ToString()),
                this.GetParameter(PARM_IS_TEMPORARY, EDataType.VarChar, 18, userInfo.IsTemporary.ToString()),
                this.GetParameter(PARM_DISPLAYNAME, EDataType.NVarChar, 255, userInfo.DisplayName),
                this.GetParameter(PARM_EMAIL, EDataType.NVarChar, 255, userInfo.Email),
                this.GetParameter(PARM_MOBILE, EDataType.VarChar, 20, userInfo.Mobile),
                this.GetParameter(PARM_ONLINE_SECONDS, EDataType.Integer, userInfo.OnlineSeconds),
                this.GetParameter(PARM_AVATAR_LARGE, EDataType.VarChar, 200, userInfo.AvatarLarge),
                this.GetParameter(PARM_AVATAR_MIDDLE, EDataType.VarChar, 200, userInfo.AvatarMiddle),
                this.GetParameter(PARM_AVATAR_SMALL, EDataType.VarChar, 200, userInfo.AvatarSmall),
                this.GetParameter(PARM_SIGNATURE, EDataType.NVarChar, 255, userInfo.Signature),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, userInfo.GetSettingsXML()),
                this.GetParameter(PARM_LOGIN_NUM,EDataType.Integer,userInfo.LoginNum),
                this.GetParameter(PARM_BIRTHDAY,EDataType.DateTime,userInfo.Birthday),
                this.GetParameter(PARM_BLOOD_TYPE,EDataType.NVarChar,255,userInfo.BloodType),
                this.GetParameter(PARM_GENDER,EDataType.NVarChar,255,userInfo.Gender),
                this.GetParameter(PARM_MARITAL_STATUS,EDataType.NVarChar,255,userInfo.MaritalStatus),
                this.GetParameter(PARM_EDUCATION,EDataType.NVarChar,255,userInfo.Education),
                this.GetParameter(PARM_GRADUATION,EDataType.NVarChar,255,userInfo.Graduation),
                this.GetParameter(PARM_PROFESSION,EDataType.NVarChar,255,userInfo.Profession),
                this.GetParameter(PARM_ADDRESS  ,EDataType.NVarChar,255,userInfo.Address),
                this.GetParameter(PARM_QQ,EDataType.NVarChar,255,userInfo.QQ),
                this.GetParameter(PARM_WEIBO,EDataType.NVarChar,255,userInfo.WeiBo),
                this.GetParameter(PARM_WEIXIN,EDataType.NVarChar,255,userInfo.WeiXin),
                this.GetParameter(PARM_INTERESTS,EDataType.NText,userInfo.Interests),
                this.GetParameter(PARM_ORGANIZATION,EDataType.NVarChar,255,userInfo.Organization),
                this.GetParameter(PARM_DEPARTMENT,EDataType.NVarChar,255,userInfo.Department),
                this.GetParameter(PARM_POSITION,EDataType.NVarChar,255,userInfo.Position),
                this.GetParameter(PARM_NEWGROUPID,EDataType.Integer,userInfo.NewGroupID),//by 20160119 增加新的用户组功能
                this.GetParameter(PARM_MLIBNUM,EDataType.Integer,userInfo.MLibNum),//by 20160119 增加新的投稿管理
                this.GetParameter(PARM_MLIBVALIDITYDATE,EDataType.DateTime,userInfo.MLibValidityDate),//by 20160119 增加新的用户组功能
                this.GetParameter(PARM_USER_ID, EDataType.Integer, userInfo.UserID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_USER, updateParms);

            UserManager.RemoveCache(true, userInfo.GroupSN, userInfo.UserName);
        }

        public bool UpdateBasic(UserInfo userInfo, out string errorMessage)
        {
            bool result = true;
            errorMessage = "";
            IDbDataParameter[] updateParms = new IDbDataParameter[]
            {
                //this.GetParameter(PARM_EMAIL, EDataType.NVarChar, 255, userInfo.Email),
                //this.GetParameter(PARM_MOBILE, EDataType.VarChar, 20, userInfo.Mobile),                 
                this.GetParameter(PARM_SIGNATURE, EDataType.NVarChar, 255, userInfo.Signature),
                this.GetParameter(PARM_USER_ID, EDataType.Integer, userInfo.UserID)
            };

            this.ExecuteNonQuery(SQL_UPDATEBASIC_USER, updateParms);

            UserManager.RemoveCache(true, userInfo.GroupSN, userInfo.UserName);
            return result;
        }

        public void AddOnlineSeconds(string groupSN, string userName, int seconds)
        {
            if (seconds != 0 && !string.IsNullOrEmpty(userName))
            {
                string sqlString = string.Format("UPDATE bairong_Users SET OnlineSeconds = OnlineSeconds + {0}, LastActivityDate = {1} WHERE [UserName] = @UserName", seconds, SqlUtils.GetDefaultDateString(base.DataBaseType));

                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    //this.GetParameter(PARM_LAST_ACTIVITY_DATE, EDataType.DateTime, SqlUtils.GetDefaultDateString(base.DataBaseType)),
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
                };

                this.ExecuteNonQuery(sqlString, parms);

                UserManager.RemoveCache(groupSN, userName);
            }
        }

        public void AddCredits(string groupSN, string userName, int credits)
        {
            string sqlString = string.Format("UPDATE bairong_Users SET Credits = Credits + {0} WHERE GroupSN = @GroupSN AND UserName = @UserName", credits);

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
            };

            this.ExecuteNonQuery(sqlString, parms);

            UserManager.RemoveCache(groupSN, userName);
        }

        public string EncodePassword(string password, EPasswordFormat passwordFormat, string passwordSalt)
        {
            string retval = string.Empty;

            if (passwordFormat == EPasswordFormat.Clear)
            {
                retval = password;
            }
            else if (passwordFormat == EPasswordFormat.Hashed)
            {
                byte[] src = Encoding.Unicode.GetBytes(password);
                byte[] buffer2 = Convert.FromBase64String(passwordSalt);
                byte[] dst = new byte[buffer2.Length + src.Length];
                byte[] inArray = null;
                Buffer.BlockCopy(buffer2, 0, dst, 0, buffer2.Length);
                Buffer.BlockCopy(src, 0, dst, buffer2.Length, src.Length);
                HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
                inArray = algorithm.ComputeHash(dst);

                retval = Convert.ToBase64String(inArray);
            }
            else if (passwordFormat == EPasswordFormat.Encrypted)
            {
                DESEncryptor encryptor = new DESEncryptor();
                encryptor.InputString = password;
                encryptor.EncryptKey = passwordSalt;
                encryptor.DesEncrypt();

                retval = encryptor.OutString;
            }
            else if (passwordFormat == EPasswordFormat.DiscuzNT)
            {
                byte[] b = Encoding.UTF8.GetBytes(password);
                b = new MD5CryptoServiceProvider().ComputeHash(b);
                for (int i = 0; i < b.Length; i++)
                    retval += b[i].ToString("x").PadLeft(2, '0');
            }
            return retval;
        }

        public string DecodePassword(string password, EPasswordFormat passwordFormat, string passwordSalt)
        {
            string retval = string.Empty;
            if (passwordFormat == EPasswordFormat.Clear)
            {
                retval = password;
            }
            else if (passwordFormat == EPasswordFormat.Hashed)
            {
                throw new Exception("can not decode hashed password");
            }
            else if (passwordFormat == EPasswordFormat.Encrypted)
            {
                DESEncryptor encryptor = new DESEncryptor();
                encryptor.InputString = password;
                encryptor.DecryptKey = passwordSalt;
                encryptor.DesDecrypt();

                retval = encryptor.OutString;
            }
            return retval;
        }

        private string GenerateSalt()
        {
            byte[] data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }

        public bool ChangePassword(int userID, string password)
        {
            EPasswordFormat passwordFormat = EPasswordFormat.Encrypted;
            string passwordSalt = this.GenerateSalt();
            password = EncodePassword(password, passwordFormat, passwordSalt);
            return this.ChangePassword(userID, passwordFormat, passwordSalt, password);
        }

        private bool ChangePassword(int userID, EPasswordFormat passwordFormat, string passwordSalt, string password)
        {
            bool isSuccess = false;
            IDbDataParameter[] updateParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PASSWORD, EDataType.NVarChar, 255, password),
                this.GetParameter(PARM_PASSWORD_FORMAT, EDataType.VarChar, 50, EPasswordFormatUtils.GetValue(passwordFormat)),
                this.GetParameter(PARM_PASSWORD_SALT, EDataType.NVarChar, 128, passwordSalt),
                this.GetParameter(PARM_USER_ID, EDataType.Integer, userID)
            };

            try
            {
                this.ExecuteNonQuery(SQL_UPDATE_PASSWORD, updateParms);
                isSuccess = true;
            }
            catch { }
            return isSuccess;
        }

        public void Delete(int userID)
        {
            IDbDataParameter[] deleteParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_ID, EDataType.Integer, userID)
            };

            this.ExecuteNonQuery(SQL_DELETE_USER, deleteParms);
        }

        public void Check(List<int> userIDList)
        {
            string sqlString = string.Format("UPDATE bairong_Users SET IsChecked = '{0}' WHERE [UserID] IN ({1})", true.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(userIDList));

            this.ExecuteNonQuery(sqlString);

            UserManager.Clear();
        }

        public void Check(int userID)
        {
            string sqlString = string.Format("UPDATE bairong_Users SET IsChecked = '{0}' WHERE [UserID] = {1}", true.ToString(), userID);

            this.ExecuteNonQuery(sqlString);

            UserManager.Clear();
        }

        public void Lock(List<int> userIDList, bool isLockOut)
        {
            string sqlString = string.Format("UPDATE bairong_Users SET IsLockedOut = '{0}' WHERE [UserID] IN ({1})", isLockOut.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithQuote(userIDList));

            this.ExecuteNonQuery(sqlString);

            UserManager.Clear();
        }

        public UserInfo GetUserInfoByLoginName(string groupSN, string loginName)
        {
            UserInfo userInfo = null;

            if (!string.IsNullOrEmpty(loginName))
            {
                //string SQL_WHERE = string.Format("WHERE {0} = '{1}' AND ({2} = '{3}' OR {4} = '{3}' OR {5} = '{3}')", UserAttribute.GroupSN, groupSN, UserAttribute.UserName, loginName, UserAttribute.Email, UserAttribute.Mobile);

                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, loginName),
                    this.GetParameter(PARM_EMAIL, EDataType.NVarChar, 255, loginName),
                    this.GetParameter(PARM_MOBILE, EDataType.VarChar, 20, loginName)
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_LOGIN_NAME, parms))
                {
                    if (rdr.Read())
                    {
                        userInfo = new UserInfo();
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, userInfo);
                    }
                    rdr.Close();
                }
                if (userInfo != null) userInfo.AfterExecuteReader();
            }
            return userInfo;
        }

        public UserInfo GetUserInfo(string groupSN, string userName)
        {
            UserInfo userInfo = null;
            if (!string.IsNullOrEmpty(userName))
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_USER_NAME, parms))
                {
                    if (rdr.Read())
                    {
                        userInfo = new UserInfo();
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, userInfo);
                    }
                    rdr.Close();
                }
                if (userInfo != null) userInfo.AfterExecuteReader();
            }
            return userInfo;
        }

        public UserInfo GetUserInfoByNameOrEmailOrMobile(string groupSN, string userName)
        {
            UserInfo userInfo = null;
            if (!string.IsNullOrEmpty(userName))
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_BY_USER_NAME_EMAIL_PHONE, parms))
                {
                    if (rdr.Read())
                    {
                        userInfo = new UserInfo();
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, userInfo);
                    }
                    rdr.Close();
                }
                if (userInfo != null) userInfo.AfterExecuteReader();
            }
            return userInfo;
        }

        public UserInfo GetUserInfo(int userID)
        {
            UserInfo userInfo = null;
            if (userID > 0)
            {
                string SQL_WHERE = string.Format("WHERE UserID = {0}", userID);
                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, SQL_WHERE);

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                {
                    if (rdr.Read())
                    {
                        userInfo = new UserInfo();
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, userInfo);
                    }
                    rdr.Close();
                }
                if (userInfo != null) userInfo.AfterExecuteReader();
            }
            return userInfo;
        }

        public UserInfo GetUserInfoByCreateIPAddress(string ipAddress)
        {
            UserInfo info = null;
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string SQL_WHERE = string.Format("WHERE CreateIPAddress = '{0}'", PageUtils.FilterSql(ipAddress));
                string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, SQL_WHERE);

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                {
                    if (rdr.Read())
                    {
                        info = new UserInfo();
                        BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    }
                    rdr.Close();
                }
                if (info != null) info.AfterExecuteReader();
            }
            return info;
        }

        public bool IsExists(string groupSN, string user)
        {
            bool exists = false;

            if (!string.IsNullOrEmpty(user))
            {
                UserInfo info = this.GetUserInfoByNameOrEmailOrMobile(groupSN, user);
                if (info != null && info.UserID > 0)
                    exists = true;
            }

            return exists;
        }

        public bool IsUserExists(string groupSN, string userName)
        {
            bool exists = false;

            if (!string.IsNullOrEmpty(userName))
            {
                string sqlString = "SELECT UserID FROM bairong_Users WHERE GroupSN = @GroupSN AND lower(UserName) = @UserName";

                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                    this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName.ToLower())
                };

                using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
                {
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        exists = true;
                    }
                    rdr.Close();
                }
            }

            return exists;
        }

        public bool IsUserNameCompliant(string userName)
        {
            if (userName.IndexOf("　") != -1 || userName.IndexOf(" ") != -1 || userName.IndexOf("'") != -1 || userName.IndexOf(":") != -1 || userName.IndexOf(".") != -1)
            {
                return false;
            }
            return DirectoryUtils.IsDirectoryNameCompliant(userName);
        }

        public bool IsEmailExists(string groupSN, string email)
        {
            bool exists = false;

            if (!string.IsNullOrEmpty(email))
            {
                string SQL_SELECT = "SELECT [Email] FROM bairong_Users WHERE GroupSN = @GroupSN AND lower([Email]) = @Email";

                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                    this.GetParameter(PARM_EMAIL, EDataType.VarChar, 200, email.ToLower())
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
                {
                    if (rdr.Read())
                    {
                        exists = true;
                    }
                    rdr.Close();
                }
            }

            return exists;
        }

        public bool IsMobileExists(string groupSN, string mobile)
        {
            bool exists = false;

            if (!string.IsNullOrEmpty(mobile))
            {
                string sqlString = "SELECT Mobile FROM bairong_Users WHERE GroupSN = @GroupSN AND Mobile = @Mobile";

                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                    this.GetParameter(PARM_MOBILE, EDataType.VarChar, 20, mobile)
                };

                using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
                {
                    if (rdr.Read())
                    {
                        exists = true;
                    }
                    rdr.Close();
                }
            }

            return exists;
        }

        public string GetUserName(int userID)
        {
            string userName = string.Empty;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_ID, EDataType.Integer, userID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_USER_NAME, parms))
            {
                if (rdr.Read())
                {
                    userName = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return userName;
        }

        public string GetEmail(int userID)
        {
            string email = string.Empty;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_ID, EDataType.Integer, userID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_EMAIL, parms))
            {
                if (rdr.Read())
                {
                    email = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return email;
        }

        public int GetUserID(string groupSN, string userName)
        {
            int userID = 0;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_USER_ID, parms))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    userID = rdr.GetInt32(0);
                }
                rdr.Close();
            }
            return userID;
        }

        public int GetUserIDByEmailOrMobile(string groupSN, string email, string mobile)
        {
            int userID = 0;

            if (!string.IsNullOrEmpty(email))
            {
                string sqlString = @"SELECT UserID FROM bairong_Users WHERE GroupSN = @GroupSN AND Email = @Email";

                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                    this.GetParameter(PARM_EMAIL, EDataType.VarChar, 200, email)
                };

                using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
                {
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        userID = rdr.GetInt32(0);
                    }
                    rdr.Close();
                }
            }
            else if (!string.IsNullOrEmpty(mobile))
            {
                string sqlString = "SELECT UserID FROM bairong_Users WHERE GroupSN = @GroupSN AND Mobile = @Mobile";

                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                    this.GetParameter(PARM_MOBILE, EDataType.VarChar, 20, mobile)
                };

                using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
                {
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        userID = rdr.GetInt32(0);
                    }
                    rdr.Close();
                }
            }

            return userID;
        }

        public string GetMobile(int userID)
        {
            string mobile = string.Empty;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_USER_ID, EDataType.Integer, userID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_MOBILE, parms))
            {
                if (rdr.Read())
                {
                    mobile = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return mobile;
        }

        public int GetTotalCount()
        {
            int count = 0;

            using (IDataReader rdr = this.ExecuteReader("SELECT COUNT(*) AS TotalNum FROM bairong_Users"))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    count = rdr.GetInt32(0);
                }
                rdr.Close();
            }
            return count;
        }

        public ArrayList GetUserInfoArrayList(ETriState checkedState)
        {
            ArrayList arraylist = new ArrayList();

            string SQL_WHERE = string.Empty;
            if (checkedState != ETriState.All)
            {
                SQL_WHERE = string.Format("WHERE IsChecked = '{0}'", ETriStateUtils.GetValue(checkedState));
            }

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    UserInfo info = new UserInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    info.AfterExecuteReader();
                    arraylist.Add(info);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetUserNameArrayList(bool isChecked)
        {
            ArrayList arraylist = new ArrayList();
            string SQL_SELECT = string.Format("SELECT UserName FROM bairong_Users WHERE IsChecked = '{0}' ORDER BY CreateDate DESC", isChecked);

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

        public List<int> GetUserIDList(bool isChecked)
        {
            List<int> userIDList = new List<int>();

            string SQL_SELECT = string.Format("SELECT UserID FROM bairong_Users WHERE IsChecked = '{0}' ORDER BY CreateDate DESC", isChecked);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    userIDList.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return userIDList;
        }

        public ArrayList GetUserNameArrayListByGroupIDCollection(string groupIDCollection)
        {
            ArrayList arraylist = new ArrayList();
            if (!string.IsNullOrEmpty(groupIDCollection))
            {
                string SQL_SELECT = string.Format("SELECT UserName FROM bairong_Users WHERE GroupID IN ({0}) ORDER BY CreateDate DESC", groupIDCollection);

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
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

        public List<int> GetUserIDListByGroupIDCollection(string groupIDCollection)
        {
            List<int> userIDList = new List<int>();
            if (!string.IsNullOrEmpty(groupIDCollection))
            {
                string SQL_SELECT = string.Format("SELECT UserID FROM bairong_Users WHERE GroupID IN ({0}) ORDER BY CreateDate DESC", groupIDCollection);

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                {
                    while (rdr.Read())
                    {
                        userIDList.Add(rdr.GetInt32(0));
                    }
                    rdr.Close();
                }
            }
            return userIDList;
        }

        public ArrayList GetUserNameArrayList(string searchWord, int dayOfCreate, int dayOfLastActivity, bool isChecked)
        {
            ArrayList arraylist = new ArrayList();

            string whereString = string.Empty;

            if (dayOfCreate > 0)
            {
                DateTime dateTime = DateTime.Now.AddDays(-dayOfCreate);
                whereString += string.Format(" AND (CreateDate >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
            }
            if (dayOfLastActivity > 0)
            {
                DateTime dateTime = DateTime.Now.AddDays(-dayOfLastActivity);
                whereString += string.Format(" AND (LastActivityDate >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(searchWord))
            {
                whereString += string.Format(" AND (UserName LIKE '%{0}%' OR EMAIL LIKE '%{0}%' OR MOBILE = '{0}') ", PageUtils.FilterSql(searchWord));
            }
            string sqlString = string.Format("SELECT UserName FROM bairong_Users WHERE IsChecked = '{0}' {1} ORDER BY CreateDate DESC", isChecked, whereString);

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

        public string GetSelectCommand(string groupSN, bool isChecked)
        {
            string whereString = string.Format("WHERE GroupSN = '{0}' AND IsChecked = '{1}'", PageUtils.FilterSql(groupSN), isChecked);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public string GetSelectCommand(string groupSN, string searchWord, int dayOfCreate, int dayOfLastActivity, bool isChecked, int groupID)
        {
            StringBuilder whereBuilder = new StringBuilder();
            whereBuilder.AppendFormat("(GroupSN = '{0}')", PageUtils.FilterSql(groupSN));

            if (dayOfCreate > 0)
            {
                whereBuilder.AppendFormat(" AND ");

                DateTime dateTime = DateTime.Now.AddDays(-dayOfCreate);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereBuilder.AppendFormat("(to_char(CreateDate,'YYYY-MM-DD') >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
                }
                whereBuilder.AppendFormat("(CreateDate >= '{0}')", dateTime.ToString("yyyy-MM-dd"));
            }
            if (dayOfLastActivity > 0)
            {
                whereBuilder.AppendFormat(" AND ");

                DateTime dateTime = DateTime.Now.AddDays(-dayOfLastActivity);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereBuilder.AppendFormat("(to_char(LastActivityDate,'YYYY-MM-DD') >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
                }

                whereBuilder.AppendFormat("(LastActivityDate >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(searchWord))
            {
                whereBuilder.AppendFormat(" AND ");

                whereBuilder.AppendFormat("(UserName LIKE '%{0}%' OR DisplayName LIKE '%{0}%' OR EMAIL LIKE '%{0}%')", PageUtils.FilterSql(searchWord));
            }

            if (groupID > 0)
            {
                whereBuilder.AppendFormat(" AND ");

                whereBuilder.AppendFormat(" GroupID = {0}", groupID);
            }

            string whereString = string.Empty;
            if (whereBuilder.Length > 0)
            {
                whereString = string.Format("WHERE {0}", whereBuilder.ToString());
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public string GetSelectCommand(string groupSN, int levelID, string searchWord, int dayOfCreate, int dayOfLastActivity, bool isChecked)
        {
            StringBuilder whereBuilder = new StringBuilder();
            whereBuilder.AppendFormat("(GroupSN = '{0}')", PageUtils.FilterSql(groupSN));

            if (dayOfCreate > 0)
            {
                whereBuilder.AppendFormat(" AND ");

                DateTime dateTime = DateTime.Now.AddDays(-dayOfCreate);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereBuilder.AppendFormat("(to_char(CreateDate,'YYYY-MM-DD') >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
                }
                whereBuilder.AppendFormat("(CreateDate >= '{0}')", dateTime.ToString("yyyy-MM-dd"));
            }
            if (dayOfLastActivity > 0)
            {
                whereBuilder.AppendFormat(" AND ");

                DateTime dateTime = DateTime.Now.AddDays(-dayOfLastActivity);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereBuilder.AppendFormat("(to_char(LastActivityDate,'YYYY-MM-DD') >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
                }
                whereBuilder.AppendFormat("(LastActivityDate >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(searchWord))
            {
                whereBuilder.AppendFormat(" AND ");

                whereBuilder.AppendFormat("(UserName LIKE '%{0}%' OR EMAIL LIKE '%{0}%')", PageUtils.FilterSql(searchWord));
            }

            if (levelID > 0)
            {
                whereBuilder.AppendFormat(" AND ");

                whereBuilder.AppendFormat("(LevelID = {0})", levelID);
            }

            string whereString = string.Empty;
            if (whereBuilder.Length > 0)
            {
                whereString = string.Format("WHERE {0}", whereBuilder.ToString());
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public string GetSelectCommand(string groupSN, int levelID, string searchWord, int dayOfCreate, int dayOfLastActivity, bool isChecked, int loginNum)
        {
            StringBuilder whereBuilder = new StringBuilder();
            whereBuilder.AppendFormat("(GroupSN = '{0}')", PageUtils.FilterSql(groupSN));

            if (dayOfCreate > 0)
            {
                whereBuilder.AppendFormat(" AND ");

                DateTime dateTime = DateTime.Now.AddDays(-dayOfCreate);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereBuilder.AppendFormat("(to_char(CreateDate,'YYYY-MM-DD') >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
                }
                whereBuilder.AppendFormat("(CreateDate >= '{0}')", dateTime.ToString("yyyy-MM-dd"));
            }
            if (dayOfLastActivity > 0)
            {
                whereBuilder.AppendFormat(" AND ");

                DateTime dateTime = DateTime.Now.AddDays(-dayOfLastActivity);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereBuilder.AppendFormat("(to_char(LastActivityDate,'YYYY-MM-DD') >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
                }
                whereBuilder.AppendFormat("(LastActivityDate >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
            }
            if (!string.IsNullOrEmpty(searchWord))
            {
                whereBuilder.AppendFormat(" AND ");

                whereBuilder.AppendFormat("(UserName LIKE '%{0}%' OR EMAIL LIKE '%{0}%')", PageUtils.FilterSql(searchWord));
            }

            if (levelID > 0)
            {
                whereBuilder.AppendFormat(" AND ");

                whereBuilder.AppendFormat("(LevelID = {0})", levelID);
            }

            if (loginNum > 0)
            {
                whereBuilder.AppendFormat(" AND ");

                whereBuilder.AppendFormat("(LoginNum > {0})", loginNum);
            }

            string whereString = string.Empty;
            if (whereBuilder.Length > 0)
            {
                whereString = string.Format("WHERE {0}", whereBuilder.ToString());
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public string GetSelectCommand(string groupSN, int levelID, string searchWord, int dayOfCreate, int dayOfLastActivity, bool isChecked, int loginNum, string searchType)
        {
            StringBuilder whereBuilder = new StringBuilder();
            whereBuilder.AppendFormat("(GroupSN = '{0}')", PageUtils.FilterSql(groupSN));

            if (dayOfCreate > 0)
            {
                whereBuilder.AppendFormat(" AND ");

                DateTime dateTime = DateTime.Now.AddDays(-dayOfCreate);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereBuilder.AppendFormat("(to_char(CreateDate,'YYYY-MM-DD') >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
                }
                whereBuilder.AppendFormat("(CreateDate >= '{0}')", dateTime.ToString("yyyy-MM-dd"));
            }
            if (dayOfLastActivity > 0)
            {
                whereBuilder.AppendFormat(" AND ");

                DateTime dateTime = DateTime.Now.AddDays(-dayOfLastActivity);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    whereBuilder.AppendFormat("(to_char(LastActivityDate,'YYYY-MM-DD') >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
                }
                whereBuilder.AppendFormat("(LastActivityDate >= '{0}') ", dateTime.ToString("yyyy-MM-dd"));
            }


            if (string.IsNullOrEmpty(searchType))
            {
                whereBuilder.AppendFormat(" AND ");

                whereBuilder.AppendFormat("(UserName LIKE '%{0}%' OR EMAIL LIKE '%{0}%')", PageUtils.FilterSql(searchWord));
            }
            else
            {
                bool columnExists = false;
                ArrayList columnNameArrayList = BaiRongDataProvider.TableStructureDAO.GetColumnNameArrayList(this.TABLE_NAME);
                foreach (string columnName in columnNameArrayList)
                {
                    if (StringUtils.EqualsIgnoreCase(columnName, searchType))
                    {
                        columnExists = true;
                        whereBuilder.AppendFormat("AND ([{0}] LIKE '%{1}%') ", searchType, searchWord);
                        break;
                    }
                }
                if (!columnExists)
                {
                    whereBuilder.AppendFormat("AND (SettingsXML LIKE '%{0}={1}%') ", searchType, searchWord);
                }

                //if (TableManager.IsAttributeNameExists(tableStyle, tableName, searchType))
                //{
                //    whereString.AppendFormat("AND ([{0}] LIKE '%{1}%') {2} ", searchType, keyword, dateString);
                //}
                //else
                //{
                //    whereString.AppendFormat("AND (SettingsXML LIKE '%{0}={1}%') {2} ", searchType, keyword, dateString);
                //}
            }

            if (levelID > 0)
            {
                whereBuilder.AppendFormat(" AND ");

                whereBuilder.AppendFormat("(LevelID = {0})", levelID);
            }

            if (loginNum > 0)
            {
                whereBuilder.AppendFormat(" AND ");

                whereBuilder.AppendFormat("(LoginNum > {0})", loginNum);
            }

            string whereString = string.Empty;
            if (whereBuilder.Length > 0)
            {
                whereString = string.Format("WHERE {0}", whereBuilder.ToString());
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, whereString);
        }

        public string GetSortFieldName()
        {
            return "CreateDate";
        }

        public IEnumerable GetStlDataSource(int startNum, int totalNum, string orderByString, string whereString)
        {
            string sqlWhereString = string.Format("WHERE IsChecked = '{0}' {1}", true, whereString);
            if (string.IsNullOrEmpty(orderByString))
            {
                orderByString = "ORDER BY CreateDate DESC";
            }

            IEnumerable enumerable = null;

            string connectionString = BaiRongDataProvider.ConnectionString;
            if (!string.IsNullOrEmpty(connectionString))
            {
                if (startNum <= 1)
                {
                    string sqlString = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(connectionString, TABLE_NAME, totalNum, SqlUtils.Asterisk, sqlWhereString, orderByString);
                    enumerable = (IEnumerable)this.ExecuteReader(connectionString, sqlString);
                }
                else
                {
                    string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(connectionString, TABLE_NAME, startNum, totalNum, SqlUtils.Asterisk, sqlWhereString, orderByString);
                    enumerable = (IEnumerable)this.ExecuteReader(connectionString, SQL_SELECT);
                }
            }

            return enumerable;
        }

        public void SetGroupID(string groupSN, string userName, int groupID)
        {
            string sqlString = "UPDATE bairong_Users SET GroupID = @GroupID WHERE GroupSN = @GroupSN AND [UserName] = @UserName";

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_GROUP_ID, EDataType.Integer, groupID),
                this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN),
                this.GetParameter(PARM_USER_NAME, EDataType.NVarChar, 255, userName)
            };

            this.ExecuteNonQuery(sqlString, parms);
            UserManager.RemoveCache(groupSN, userName);
        }

        public void SetGroupID(string groupSN, ArrayList userNameArrayList, int groupID)
        {
            //string sqlString = string.Format("UPDATE bairong_Users SET GroupID = {0} WHERE GroupSN = '{1}' AND [UserName] IN ({2})", groupID, groupSN, TranslateUtils.ObjectCollectionToSqlInStringWithQuote(userNameArrayList));

            string parameterNameList = string.Empty;
            List<IDbDataParameter> parameterList = this.GetINParameterList(PARM_USER_NAME, EDataType.NVarChar, 255, userNameArrayList, out parameterNameList);

            string SQL_UPDATE = string.Format("UPDATE bairong_Users SET GroupID = @GroupID WHERE GroupSN = @GroupSN AND [UserName] IN ({0})", parameterNameList);

            List<IDbDataParameter> paramList = new List<IDbDataParameter>();
            paramList.Add(this.GetParameter(PARM_GROUP_ID, EDataType.Integer, groupID));
            paramList.Add(this.GetParameter(PARM_GROUP_SN, EDataType.NVarChar, 255, groupSN));
            paramList.AddRange(parameterList);

            this.ExecuteNonQuery(SQL_UPDATE, parameterNameList);

            foreach (string userName in userNameArrayList)
            {
                UserManager.RemoveCache(groupSN, userName);
            }
        }

        public bool CheckPassword(string password, string dbpassword, EPasswordFormat passwordFormat, string passwordSalt)
        {
            if (passwordFormat == EPasswordFormat.Hashed || passwordFormat == EPasswordFormat.DiscuzNT)
            {
                string encodePassword = EncodePassword(password, passwordFormat, passwordSalt);
                if (dbpassword == encodePassword)
                {
                    return true;
                }
                return false;
            }
            else if (passwordFormat == EPasswordFormat.bbsMax)
            {
                string passwordToMd5 = EncryptUtils.Md5(password).ToUpper().Replace("0", "");
                string newdbPassword = dbpassword.Replace("0", "");
                if (newdbPassword == passwordToMd5)
                {
                    return true;
                }
                return false;
            }
            else
            {
                string decodePassword = DecodePassword(dbpassword, passwordFormat, passwordSalt);
                if (password == decodePassword)
                {
                    return true;
                }
                return false;
            }
        }

        public string GetPassword(int userID)
        {
            string password = string.Empty;
            UserInfo userInfo = this.GetUserInfo(userID);
            if (userInfo != null)
            {
                if (userInfo.PasswordFormat != EPasswordFormat.Hashed)
                {
                    password = this.DecodePassword(userInfo.Password, userInfo.PasswordFormat, userInfo.PasswordSalt);
                }
                else
                {
                    password = "123456";
                    this.ChangePassword(userID, password);
                }
            }
            return password;
        }

        public bool Import(UserInfo userInfo)
        {
            if (string.IsNullOrEmpty(userInfo.UserName))
            {
                return false;
            }
            if (string.IsNullOrEmpty(userInfo.Password))
            {
                return false;
            }
            if (this.IsUserExists(userInfo.GroupSN, userInfo.UserName))
            {
                return false;
            }
            try
            {
                this.InsertWithoutValidation(userInfo);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Validate(string groupSN, string userName, string password, out string errorMessage)
        {
            bool isValid = false;
            errorMessage = string.Empty;

            UserInfo userInfo = this.GetUserInfo(groupSN, userName);
            if (userInfo == null)
            {
                errorMessage = "帐号或密码错误";
                return false;
            }
            if (userInfo.IsChecked == false)
            {
                errorMessage = "用户未审核，无法登录";
                return false;
            }
            if (userInfo.IsLockedOut == true)
            {
                if (UserConfigManager.Additional.LockingType == EUserLockTypeUtils.GetValue(EUserLockType.Day))
                {
                    //如果不是永久锁定，那么计算天数，如果已经超过锁定时间，那么用户解锁
                    if (userInfo.LockedTime != DateUtils.SqlMinValue && (DateTime.Now - userInfo.LockedTime).TotalMilliseconds >= 3600 * 24 * UserConfigManager.Additional.LockingTime)
                    {
                        userInfo.IsLockedOut = false;
                        userInfo.LockedTime = DateUtils.SqlMinValue;
                        this.Update(userInfo);
                    }
                    else
                    {
                        errorMessage = "用户被锁定，无法登录";
                        return false;
                    }
                }
                else
                {
                    errorMessage = "用户被锁定，无法登录";
                    return false;
                }
            }
            if (userInfo.LoginFailCounter >= UserConfigManager.Additional.LoginFailCount)
            {
                userInfo.IsLockedOut = true;
                userInfo.LockedTime = DateTime.Now;
                userInfo.LoginFailCounter = 0;
                this.Update(userInfo);
                errorMessage = "用户被锁定，无法登录";
                return false;
            }
            if (CheckPassword(password, userInfo.Password, userInfo.PasswordFormat, userInfo.PasswordSalt))
            {
                isValid = true;

                userInfo.LastActivityDate = DateTime.Now;
                //一旦登录成功，失败次数清0
                userInfo.LoginFailCounter = 0;
                userInfo.LockedTime = DateUtils.SqlMinValue;
                this.Update(userInfo);
            }
            else
            {
                if (UserConfigManager.Additional.IsFailToLock)
                {
                    //登录失败，记录失败次数
                    userInfo.LoginFailCounter++;
                    this.Update(userInfo);
                    errorMessage = string.Format("帐号或密码错误,还剩余{0}次登录机会", UserConfigManager.Additional.LoginFailCount - userInfo.LoginFailCounter + 1);
                }
                else
                {
                    errorMessage = string.Format("帐号或密码错误");
                }
                return false;
            }

            return isValid;
        }

        public bool ValidateByLoginName(string groupSN, string loginName, string password, out string userName, out string errorMessage)
        {
            bool isValid = false;
            userName = string.Empty;
            errorMessage = string.Empty;

            UserInfo userInfo = this.GetUserInfoByLoginName(groupSN, loginName);

            string loginValidateType = string.Empty;
            if (UserConfigManager.Additional.UserLoginValidateFields.Contains(ELoginValidateTypeUtils.GetValue(ELoginValidateType.Email)))
                loginValidateType += "邮箱/";
            if (UserConfigManager.Additional.UserLoginValidateFields.Contains(ELoginValidateTypeUtils.GetValue(ELoginValidateType.Phone)))
                loginValidateType += "手机号/";
            if (UserConfigManager.Additional.UserLoginValidateFields.Contains(ELoginValidateTypeUtils.GetValue(ELoginValidateType.UserName)))
                loginValidateType += "用户名/";
            loginValidateType = loginValidateType.TrimEnd(new char[] { '/' });
            if (!UserConfigManager.Additional.UserLoginValidateFields.Contains(ELoginValidateTypeUtils.GetValue(ELoginValidateType.Email)) && StringUtils.IsEmail(loginName))
            {
                errorMessage = "不能用邮箱登录，请用" + loginValidateType + "登录";
                return false;
            }

            if (!UserConfigManager.Additional.UserLoginValidateFields.Contains(ELoginValidateTypeUtils.GetValue(ELoginValidateType.Phone)) && StringUtils.IsMobile(loginName))
            {
                errorMessage = "不能用手机登录，请用" + loginValidateType + "登录";
                return false;
            }

            if (!UserConfigManager.Additional.UserLoginValidateFields.Contains(ELoginValidateTypeUtils.GetValue(ELoginValidateType.UserName)) && !StringUtils.IsEmail(loginName) && !StringUtils.IsMobile(loginName))
            {
                errorMessage = "不能用用户名登录，请用" + loginValidateType + "登录";
                return false;
            }

            if (userInfo == null)
            {
                errorMessage = "帐号或密码错误";
                return false;
            }
            else
            {
                userName = userInfo.UserName;
            }
            if (userInfo.IsChecked == false)
            {
                errorMessage = "用户未审核，无法登录";
                return false;
            }
            if (userInfo.IsLockedOut == true)
            {

                if (UserConfigManager.Additional.LockingType == EUserLockTypeUtils.GetValue(EUserLockType.Day))
                {
                    //如果不是永久锁定，那么计算天数，如果已经超过锁定时间，那么用户解锁
                    if (userInfo.LockedTime != DateUtils.SqlMinValue && (DateTime.Now - userInfo.LockedTime).TotalSeconds >= 3600 * 24 * UserConfigManager.Additional.LockingTime)
                    {
                        userInfo.IsLockedOut = false;
                        userInfo.LockedTime = DateUtils.SqlMinValue;
                        this.Update(userInfo);
                    }
                    else
                    {
                        errorMessage = "用户被锁定，无法登录";
                        return false;
                    }
                }
                else
                {
                    errorMessage = "用户被锁定，无法登录";
                    return false;
                }
            }
            if (userInfo.LoginFailCounter >= UserConfigManager.Additional.LoginFailCount)
            {
                userInfo.IsLockedOut = true;
                userInfo.LockedTime = DateTime.Now;
                userInfo.LoginFailCounter = 0;
                this.Update(userInfo);
                errorMessage = "用户被锁定，无法登录";
                return false;
            }

            if (CheckPassword(password, userInfo.Password, userInfo.PasswordFormat, userInfo.PasswordSalt))
            {
                isValid = true;

                userInfo.LastActivityDate = DateTime.Now;
                //一旦登录成功，失败次数清0
                userInfo.LoginFailCounter = 0;
                userInfo.LockedTime = DateUtils.SqlMinValue;
                this.Update(userInfo);
            }
            else
            {
                if (UserConfigManager.Additional.IsFailToLock)
                {
                    //登录失败，记录失败次数
                    userInfo.LoginFailCounter++;
                    this.Update(userInfo);
                    errorMessage = string.Format("帐号或密码错误,还剩余{0}次登录机会", UserConfigManager.Additional.LoginFailCount - userInfo.LoginFailCounter + 1);
                }
                else
                {
                    errorMessage = string.Format("帐号或密码错误");
                }
                return false;
            }

            return isValid;
        }

        public void Login(string groupSN, string userName, bool persistent)
        {
            CookieUtils.SetCookie(UserAuthUtils.GetAuthCookie(groupSN, userName, persistent));
            CookieUtils.SetCookie(UserAuthUtils.GetGroupSNCookie(groupSN, persistent));
            CookieUtils.SetCookie(UserAuthUtils.GetUserNameCookie(userName, persistent));
        }

        public void Logout()
        {
            CookieUtils.Erase(UserAuthConfig.AuthCookieName);
            CookieUtils.Erase(UserAuthConfig.GroupSNCookieName);
            CookieUtils.Erase(UserAuthConfig.UserNameCookieName);
        }

        public virtual string CurrentGroupSN
        {
            get
            {
                string currentGroupSN = PageUtils.UrlDecode(CookieUtils.GetCookie(UserAuthConfig.GroupSNCookieName));
                if (currentGroupSN == null)
                {
                    currentGroupSN = string.Empty;
                }
                return currentGroupSN;
            }
        }

        public virtual string CurrentUserName
        {
            get
            {
                string currentUserName = PageUtils.UrlDecode(CookieUtils.GetCookie(UserAuthConfig.UserNameCookieName));
                if (!string.IsNullOrEmpty(currentUserName))
                {
                    currentUserName = currentUserName.Replace("'", string.Empty);
                }
                else
                {
                    currentUserName = string.Empty;
                }
                return currentUserName;
            }
        }

        public virtual bool IsAnonymous
        {
            get
            {
                string encryptedTicket = CookieUtils.GetCookie(UserAuthConfig.AuthCookieName);
                if (string.IsNullOrEmpty(encryptedTicket))
                {
                    return true;
                }

                try
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(encryptedTicket);
                    if (ticket == null)
                    {
                        return true;
                    }

                    string ticketName = this.CurrentGroupSN + "." + this.CurrentUserName;

                    if (ticket.Name != ticketName)
                    {
                        return true;
                    }
                }
                catch { }

                return false;

                //return (CookieUtils.IsExists(UserAuthConfig.AuthCookieName)) ? false : true;
            }
        }

        /// <summary>
        /// 获取用户新增统计数据
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Hashtable GetTrackingHashtable(DateTime dateFrom, DateTime dateTo, string xType)
        {
            Hashtable hashtable = new Hashtable();
            if (string.IsNullOrEmpty(xType))
            {
                xType = EStatictisXTypeUtils.GetValue(EStatictisXType.Day);
            }

            StringBuilder builder = new StringBuilder();
            if (dateFrom != null)
            {
                builder.AppendFormat(" AND CreateDate >= '{0}'", dateFrom.ToString());
            }
            if (dateTo != null)
            {
                builder.AppendFormat(" AND CreateDate < '{0}'", dateTo.ToString());
            }

            string SQL_SELECT_TRACKING_DAY = string.Format(@"
SELECT COUNT(*) AS AddNum, AddYear, AddMonth, AddDay
FROM (SELECT DATEPART([year], CreateDate) AS AddYear, DATEPART([Month], 
              CreateDate) AS AddMonth, DATEPART([Day], CreateDate) 
              AS AddDay
        FROM [dbo].[bairong_Users]
        WHERE (DATEDIFF([Day], CreateDate, {0}) < 30)  {1}) 
      DERIVEDTBL
GROUP BY AddYear, AddMonth, AddDay
ORDER BY AddYear, AddMonth, AddDay
", SqlUtils.GetDefaultDateString(this.DataBaseType), builder);//添加日统计

            if (EStatictisXTypeUtils.Equals(xType, EStatictisXType.Month))
            {
                SQL_SELECT_TRACKING_DAY = string.Format(@"
SELECT COUNT(*) AS AddNum, AddYear, AddMonth
FROM (SELECT DATEPART([year], CreateDate) AS AddYear, DATEPART([Month], 
              CreateDate) AS AddMonth
        FROM [dbo].[bairong_Users]
        WHERE (DATEDIFF([Month], CreateDate, {0}) < 12) {1}) 
      DERIVEDTBL
GROUP BY AddYear, AddMonth
ORDER BY AddYear, AddMonth
", SqlUtils.GetDefaultDateString(this.DataBaseType), builder);//添加月统计
            }
            else if (EStatictisXTypeUtils.Equals(xType, EStatictisXType.Year))
            {
                SQL_SELECT_TRACKING_DAY = string.Format(@"
SELECT COUNT(*) AS AddNum, AddYear
FROM (SELECT DATEPART([year], CreateDate) AS AddYear
        FROM [dbo].[bairong_Users]
        WHERE (DATEDIFF([Year], CreateDate, {0}) < 10) {1}) 
      DERIVEDTBL
GROUP BY AddYear
ORDER BY AddYear
", SqlUtils.GetDefaultDateString(this.DataBaseType), builder);//添加年统计
            }


            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TRACKING_DAY))
            {
                while (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        int accessNum = Convert.ToInt32(rdr[0]);
                        if (EStatictisXTypeUtils.Equals(xType, EStatictisXType.Day))
                        {
                            string year = rdr[1].ToString();
                            string month = rdr[2].ToString();
                            string day = rdr[3].ToString();
                            DateTime dateTime = TranslateUtils.ToDateTime(string.Format("{0}-{1}-{2}", year, month, day));
                            hashtable.Add(dateTime, accessNum);
                        }
                        else if (EStatictisXTypeUtils.Equals(xType, EStatictisXType.Month))
                        {
                            string year = rdr[1].ToString();
                            string month = rdr[2].ToString();

                            DateTime dateTime = TranslateUtils.ToDateTime(string.Format("{0}-{1}-1", year, month));
                            hashtable.Add(dateTime, accessNum);
                        }
                        else if (EStatictisXTypeUtils.Equals(xType, EStatictisXType.Year))
                        {
                            string year = rdr[1].ToString();
                            DateTime dateTime = TranslateUtils.ToDateTime(string.Format("{0}-1-1", year));
                            hashtable.Add(dateTime, accessNum);
                        }
                    }
                }
                rdr.Close();
            }
            return hashtable;
        }


        /// <summary>
        /// 修改用户投稿有效期
        /// </summary>
        /// <param name="userIDs"></param>
        /// <param name="validityDate"></param>
        public void UpdateMLibValidityDate(ArrayList userIDs, DateTime validityDate)
        {
            if (userIDs.Count > 0)
            {
                string sql = string.Format(@"update bairong_Users set MLibValidityDate ='{0}' where UserID in ({1})", validityDate.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(userIDs));
                this.ExecuteNonQuery(sql);
            }
        }


        public string GetSelectCommandByNewGroup(string groupSN, string searchWord, bool isChecked, int newGroupID)
        {
            StringBuilder whereBuilder = new StringBuilder();
            whereBuilder.AppendFormat("(GroupSN = '{0}')", PageUtils.FilterSql(groupSN));

            if (!string.IsNullOrEmpty(searchWord))
            {
                whereBuilder.AppendFormat(" AND ");

                whereBuilder.AppendFormat("(UserName LIKE '%{0}%' OR EMAIL LIKE '%{0}%')", PageUtils.FilterSql(searchWord));
            }

            if (newGroupID > 0)
            {
                whereBuilder.AppendFormat(" AND  NewGroupID={0}", newGroupID);
            }

            string whereString = string.Empty;
            if (whereBuilder.Length > 0)
            {
                whereString = string.Format("WHERE {0}", whereBuilder.ToString());
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, whereString);
        }
    }
}
