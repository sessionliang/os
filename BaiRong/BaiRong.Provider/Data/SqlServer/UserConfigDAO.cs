using System.Data;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using System.Collections.Specialized;


namespace BaiRong.Provider.Data.SqlServer
{
    public class UserConfigDAO : DataProviderBase, IUserConfigDAO
	{
        private const string SQL_INSERT_USER_CONFIG = "INSERT INTO bairong_UserConfig (SettingsXML) VALUES (@SettingsXML)";

        private const string SQL_SELECT_USER_CONFIG = "SELECT SettingsXML FROM bairong_UserConfig";

        private const string SQL_UPDATE_USER_CONFIG = "UPDATE bairong_UserConfig SET SettingsXML = @SettingsXML";

		private const string PARM_SETTINGS_XML = "@SettingsXML";

		public void Update(UserConfigInfo info) 
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, info.Additional.ToString())
			};

            if (this.IsExists())
            {
                this.ExecuteNonQuery(SQL_UPDATE_USER_CONFIG, parms);
            }
            else
            {
                this.ExecuteNonQuery(SQL_INSERT_USER_CONFIG, parms);
            }
            
            UserConfigManager.IsChanged = true;
		}

		public bool IsExists()
		{
            bool isExists = false;

			try
			{
                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_USER_CONFIG))
				{
					if (rdr.Read()) 
					{
                        isExists = true;
					}
					rdr.Close();
				}
			}
			catch{}

            return isExists;
		}

		public UserConfigInfo GetUserConfigInfo()
		{
			UserConfigInfo info = null;

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_USER_CONFIG))
            {
                if (rdr.Read())
                {
                    info = new UserConfigInfo(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            if (info == null)
            {
                info = new UserConfigInfo();
            }

			return info;
		}

        public void InitializeUserRole(string consoleUserName, string consolePassword)
        {
            RoleManager.CreatePredefinedRoles();

            AdministratorInfo administratorInfo = new AdministratorInfo();
            administratorInfo.UserName = consoleUserName;
            administratorInfo.Password = consolePassword;
            administratorInfo.Question = string.Empty;
            administratorInfo.Answer = string.Empty;
            administratorInfo.Email = string.Empty;

            string errorMessage;
            AdminManager.CreateAdministrator(administratorInfo, out errorMessage);
            RoleManager.AddUserToRole(consoleUserName, EPredefinedRoleUtils.GetValue(EPredefinedRole.ConsoleAdministrator));
        }

	}
}
