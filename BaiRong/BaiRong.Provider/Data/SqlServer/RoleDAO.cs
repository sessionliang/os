using System;
using System.Data;
using System.Collections;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Generic;

namespace BaiRong.Provider.Data.SqlServer
{
    public class RoleDAO : DataProviderBase, IRoleDAO
	{
        private const string PARM_ROLE_NAME = "@RoleName";
        private const string PARM_PRODUCTID_COLLECTION = "@ProductIDCollection";
        private const string PARM_CREATOR_USERNAME= "@CreatorUserName";
        private const string PARM_DESCRIPTION = "@Description";

		public string GetRoleDescription(string roleName)
		{
			string roleDescription = string.Empty;
            string sqlString = "SELECT Description FROM bairong_Roles WHERE RoleName = @RoleName";
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ROLE_NAME, EDataType.NVarChar, 255, roleName)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
			{
				if (rdr.Read())
				{
					if (!rdr.IsDBNull(0))
					{
                        roleDescription = rdr.GetValue(0).ToString();
					}
				}
				rdr.Close();
			}
			return roleDescription;
		}

		public string GetRolesCreatorUserName(string roleName)
		{
			string creatorUserName = string.Empty;
            string sqlString = "SELECT CreatorUserName FROM bairong_Roles WHERE RoleName = @RoleName";
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ROLE_NAME, EDataType.NVarChar, 255, roleName)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms)) 
			{
				if (rdr.Read()) 
				{
                    creatorUserName = rdr.GetValue(0).ToString();
				}
				rdr.Close();
			}
			return creatorUserName;
		}

        public string[] GetAllRoles()
        {
            string tmpUserNames = string.Empty;
            string SQL_SELECT = "SELECT RoleName FROM bairong_Roles ORDER BY RoleName";

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                while (rdr.Read())
                {
                    tmpUserNames += rdr.GetValue(0).ToString() + ",";
                }
                rdr.Close();
            }

            if (tmpUserNames.Length > 0)
            {
                tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
                return tmpUserNames.Split(',');
            }

            return new string[0];
        }

		public ArrayList GetRoleNameArrayListByCreatorUserName(string creatorUserName)
		{
			ArrayList arraylist = new ArrayList();

			if (!string.IsNullOrEmpty(creatorUserName))
			{
                string sqlString = "SELECT RoleName FROM bairong_Roles WHERE CreatorUserName = @CreatorUserName";
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

		public string[] GetAllRolesByCreatorUserName(string creatorUserName)
		{
			ArrayList roleNameArrayList = GetRoleNameArrayListByCreatorUserName(creatorUserName);
			string[] roleArray = new string[roleNameArrayList.Count];
			roleNameArrayList.CopyTo(roleArray);
			return roleArray;
		}

        public string[] GetRolesForUser(string userName)
        {
            string tmpRoleNames = string.Empty;
            string sqlString = "SELECT RoleName FROM bairong_AdministratorsInRoles WHERE UserName = @UserName ORDER BY RoleName";
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter("@UserName", EDataType.NVarChar, 255, userName)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                while (rdr.Read())
                {
                    tmpRoleNames += rdr.GetValue(0).ToString() + ",";
                }
                rdr.Close();
            }

            if (tmpRoleNames.Length > 0)
            {
                tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
                return tmpRoleNames.Split(',');
            }

            return new string[0];
        }

        public string[] GetUsersInRole(string roleName)
        {
            string tmpUserNames = string.Empty;
            string sqlString = "SELECT UserName FROM bairong_AdministratorsInRoles WHERE RoleName = @RoleName ORDER BY userName";
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ROLE_NAME, EDataType.NVarChar, 255, roleName)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                while (rdr.Read())
                {
                    tmpUserNames += rdr.GetValue(0).ToString() + ",";
                }
                rdr.Close();
            }

            if (tmpUserNames.Length > 0)
            {
                tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
                return tmpUserNames.Split(',');
            }

            return new string[0];
        }

        public void RemoveUserFromRoles(string userName, string[] roleNames)
        {
            string sqlString = "DELETE bairong_AdministratorsInRoles WHERE UserName = @UserName AND RoleName = @RoleName";
            foreach (string roleName in roleNames)
            {
                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter("@UserName", EDataType.NVarChar, 255, userName),
                    this.GetParameter("@RoleName", EDataType.NVarChar, 255, roleName)
			    };
                this.ExecuteNonQuery(sqlString, parms);
            }
        }

        public void RemoveUserFromRole(string userName, string roleName)
        {
            string sqlString = "DELETE bairong_AdministratorsInRoles WHERE UserName = @UserName AND RoleName = @RoleName";
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter("@UserName", EDataType.NVarChar, 255, userName),
                this.GetParameter("@RoleName", EDataType.NVarChar, 255, roleName)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void InsertRole(string roleName, ArrayList productIDArrayList, string creatorUserName, string description)
        {
            string sqlString = "INSERT INTO bairong_Roles (RoleName, ProductIDCollection, CreatorUserName, Description) VALUES (@RoleName, @ProductIDCollection, @CreatorUserName, @Description)";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ROLE_NAME, EDataType.NVarChar, 255, roleName),
                this.GetParameter(PARM_PRODUCTID_COLLECTION, EDataType.VarChar, 200, TranslateUtils.ObjectCollectionToString(productIDArrayList)),
                this.GetParameter(PARM_CREATOR_USERNAME, EDataType.NVarChar, 255, creatorUserName),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, description)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public List<string> GetProductIDList(string roleName)
        {
            List<string> productIDList = new List<string>();
            string sqlString = "SELECT ProductIDCollection FROM bairong_Roles WHERE RoleName = @RoleName";
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ROLE_NAME, EDataType.NVarChar, 255, roleName)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    string productString = rdr.GetValue(0).ToString();
                    if (!string.IsNullOrEmpty(productString))
                    {
                        List<string> list = TranslateUtils.StringCollectionToStringList(productString.ToLower());
                        foreach (string productID in list)
                        {
                            if (!productIDList.Contains(productID))
                            {
                                productIDList.Add(productID);
                            }
                        }
                    }
                }
                rdr.Close();
            }

            return productIDList;
        }

        public virtual void UpdateRole(string roleName, ArrayList productIDArrayList, string description) 
		{
            string sqlString = "UPDATE bairong_Roles SET ProductIDCollection = @ProductIDCollection, Description = @Description WHERE RoleName = @RoleName";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PRODUCTID_COLLECTION, EDataType.VarChar, 200, TranslateUtils.ObjectCollectionToString(productIDArrayList)),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, description),
                this.GetParameter(PARM_ROLE_NAME, EDataType.NVarChar, 255, roleName)
			};

            this.ExecuteNonQuery(sqlString, parms);
		}


		public bool DeleteRole(string roleName)
		{
            bool isSuccess = false;
            try
            {
                string sqlString = "DELETE FROM bairong_Roles WHERE RoleName = @RoleName";

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
                    this.GetParameter(PARM_ROLE_NAME, EDataType.NVarChar, 255, roleName)
			    };

                this.ExecuteNonQuery(sqlString, parms);
                isSuccess = true;
            }
            catch { }
            return isSuccess;
		}

        public bool IsRoleExists(string roleName)
        {
            bool exists = false;
            string sqlString = "SELECT RoleName FROM bairong_Roles WHERE RoleName = @RoleName";
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@RoleName", EDataType.NVarChar, 255, roleName)
			};
            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        exists = true;
                    }
                }
                rdr.Close();
            }
            return exists;
        }

        public string[] FindUsersInRole(string roleName, string userNameToMatch)
        {
            string tmpUserNames = string.Empty;
            string sqlString = string.Format("SELECT UserName FROM bairong_AdministratorsInRoles WHERE RoleName = @RoleName AND UserName LIKE '%{0}%'", PageUtils.FilterSql(userNameToMatch));

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@RoleName", EDataType.NVarChar, 255, roleName)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                while (rdr.Read())
                {
                    tmpUserNames += rdr.GetValue(0).ToString() + ",";
                }
                rdr.Close();
            }

            if (tmpUserNames.Length > 0)
            {
                tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
                return tmpUserNames.Split(',');
            }

            return new string[0];
        }

        public bool IsUserInRole(string userName, string roleName)
        {
            bool isUserInRole = false;
            string sqlString = "SELECT * FROM bairong_AdministratorsInRoles WHERE UserName = @UserName AND RoleName = @RoleName";
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@UserName", EDataType.NVarChar, 255, userName),
                this.GetParameter("@RoleName", EDataType.NVarChar, 255, roleName)
			};
            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        isUserInRole = true;
                    }
                }
                rdr.Close();
            }
            return isUserInRole;
        }

        public void AddUserToRoles(string userName, string[] roleNames)
        {
            foreach (string roleName in roleNames)
            {
                this.AddUserToRole(userName, roleName);
            }
        }

        public void AddUserToRole(string userName, string roleName)
        {
            if (!IsRoleExists(roleName)) return;
            if (!BaiRongDataProvider.AdministratorDAO.IsUserNameExists(userName)) return;
            if (!IsUserInRole(userName, roleName))
            {
                string sqlString = "INSERT INTO bairong_AdministratorsInRoles (UserName, RoleName) VALUES (@UserName, @RoleName)";

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
                    this.GetParameter("@UserName", EDataType.NVarChar, 255, userName),
                    this.GetParameter("@RoleName", EDataType.NVarChar, 255, roleName)
			    };

                this.ExecuteNonQuery(sqlString, parms);
            }
        }
	}
}
