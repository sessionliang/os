using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class GovPublicCategoryClassDAO : DataProviderBase, IGovPublicCategoryClassDAO
	{
        private const string SQL_INSERT = "INSERT INTO siteserver_GovPublicCategoryClass (ClassCode, PublishmentSystemID, ClassName, IsSystem, IsEnabled, ContentAttributeName, Taxis, Description) VALUES (@ClassCode, @PublishmentSystemID, @ClassName, @IsSystem, @IsEnabled, @ContentAttributeName, @Taxis, @Description)";

        private const string SQL_UPDATE = "UPDATE siteserver_GovPublicCategoryClass SET ClassName = @ClassName, IsEnabled = @IsEnabled, Description = @Description WHERE ClassCode = @ClassCode AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_DELETE = "DELETE FROM siteserver_GovPublicCategoryClass WHERE ClassCode = @ClassCode AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT = "SELECT ClassCode, PublishmentSystemID, ClassName, IsSystem, IsEnabled, ContentAttributeName, Taxis, Description FROM siteserver_GovPublicCategoryClass WHERE ClassCode = @ClassCode AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ALL = "SELECT ClassCode, PublishmentSystemID, ClassName, IsSystem, IsEnabled, ContentAttributeName, Taxis, Description FROM siteserver_GovPublicCategoryClass WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis";

        private const string SQL_SELECT_CLASS_CODE = "SELECT ClassCode FROM siteserver_GovPublicCategoryClass WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis";

        private const string SQL_SELECT_CLASS_NAME = "SELECT ClassName FROM siteserver_GovPublicCategoryClass WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis";

        private const string PARM_CLASS_CODE = "@ClassCode";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_CLASS_NAME = "@ClassName";
        private const string PARM_IS_SYSTEM = "@IsSystem";
        private const string PARM_IS_ENABLED = "@IsEnabled";
        private const string PARM_CONTENT_ATTRIBUTE_NAME = "@ContentAttributeName";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_DESCRIPTION = "@Description";

        private string GetContentAttributeNameNotUsed(int publishmentSystemID)
        {
            string contentAttributeName = string.Empty;

            for (int i = 1; i <= 6; i++)
            {
                string sqlString = string.Format("SELECT ContentAttributeName FROM siteserver_GovPublicCategoryClass WHERE PublishmentSystemID = {0} AND  ContentAttributeName = 'Category{1}ID'", publishmentSystemID, i);

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    if (!rdr.Read())
                    {
                        contentAttributeName = string.Format("Category{0}ID", i);
                    }
                    rdr.Close();
                }
            }

            return contentAttributeName;
        }

		public void Insert(GovPublicCategoryClassInfo categoryClassInfo) 
		{
            if (categoryClassInfo.IsSystem)
            {
                if (EGovPublicCategoryClassTypeUtils.Equals(EGovPublicCategoryClassType.Channel, categoryClassInfo.ClassCode))
                {
                    categoryClassInfo.ContentAttributeName = ContentAttribute.NodeID;
                }
                else if (EGovPublicCategoryClassTypeUtils.Equals(EGovPublicCategoryClassType.Department, categoryClassInfo.ClassCode))
                {
                    categoryClassInfo.ContentAttributeName = GovPublicContentAttribute.DepartmentID;
                }
            }
            else
            {
                categoryClassInfo.ContentAttributeName = this.GetContentAttributeNameNotUsed(categoryClassInfo.PublishmentSystemID);
            }
            int taxis = this.GetMaxTaxis(categoryClassInfo.PublishmentSystemID) + 1;
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CLASS_CODE, EDataType.NVarChar, 50, categoryClassInfo.ClassCode),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, categoryClassInfo.PublishmentSystemID),
                this.GetParameter(PARM_CLASS_NAME, EDataType.NVarChar, 255, categoryClassInfo.ClassName),
                this.GetParameter(PARM_IS_SYSTEM, EDataType.VarChar, 18, categoryClassInfo.IsSystem.ToString()),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, categoryClassInfo.IsEnabled.ToString()),
                this.GetParameter(PARM_CONTENT_ATTRIBUTE_NAME, EDataType.VarChar, 50, categoryClassInfo.ContentAttributeName),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, categoryClassInfo.Description)
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);
		}

        public void Update(GovPublicCategoryClassInfo categoryClassInfo) 
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_CLASS_NAME, EDataType.NVarChar, 255, categoryClassInfo.ClassName),
                this.GetParameter(PARM_IS_ENABLED, EDataType.VarChar, 18, categoryClassInfo.IsEnabled.ToString()),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, categoryClassInfo.Description),
                this.GetParameter(PARM_CLASS_CODE, EDataType.NVarChar, 50, categoryClassInfo.ClassCode),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, categoryClassInfo.PublishmentSystemID),
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
		}

		public void Delete(string classCode, int publishmentSystemID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_CLASS_CODE, EDataType.NVarChar, 50, classCode),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
		}

        public GovPublicCategoryClassInfo GetCategoryClassInfo(string classCode, int publishmentSystemID)
		{
            GovPublicCategoryClassInfo categoryClassInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CLASS_CODE, EDataType.NVarChar, 50, classCode),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    categoryClassInfo = new GovPublicCategoryClassInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString(), rdr.GetInt32(6), rdr.GetValue(7).ToString());
                }
                rdr.Close();
            }

            return categoryClassInfo;
		}

        public string GetContentAttributeName(string classCode, int publishmentSystemID)
        {
            string contentAttributeName = string.Empty;

            string sqlString = string.Format("SELECT ContentAttributeName FROM siteserver_GovPublicCategoryClass WHERE PublishmentSystemID = {0} AND  ClassCode = '{1}'", publishmentSystemID, classCode);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    contentAttributeName = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }

            return contentAttributeName;
        }

        public bool IsExists(string classCode, int publishmentSystemID)
		{
			bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CLASS_CODE, EDataType.NVarChar, 50, classCode),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
			{
				if (rdr.Read()) 
				{					
					exists = true;
				}
				rdr.Close();
			}

			return exists;
		}

		public IEnumerable GetDataSource(int publishmentSystemID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL, parms);
			return enumerable;
		}

        public int GetCount(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM siteserver_GovPublicCategoryClass WHERE PublishmentSystemID = {0}", publishmentSystemID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public ArrayList GetCategoryClassInfoArrayList(int publishmentSystemID, ETriState isSystem, ETriState isEnabled)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, selectParms))
            {
                while (rdr.Read())
                {
                    GovPublicCategoryClassInfo categoryClassInfo = new GovPublicCategoryClassInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetValue(2).ToString(), TranslateUtils.ToBool(rdr.GetValue(3).ToString()), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString(), rdr.GetInt32(6), rdr.GetValue(7).ToString());
                    if (isSystem == ETriState.False)
                    {
                        if (categoryClassInfo.IsSystem) continue;
                    }
                    else if (isSystem == ETriState.True)
                    {
                        if (!categoryClassInfo.IsSystem) continue;
                    }
                    if (isEnabled == ETriState.False)
                    {
                        if (categoryClassInfo.IsEnabled) continue;
                    }
                    else if (isEnabled == ETriState.True)
                    {
                        if (!categoryClassInfo.IsEnabled) continue;
                    }
                    arraylist.Add(categoryClassInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetClassCodeArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_CLASS_CODE, selectParms))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetClassNameArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_CLASS_NAME, selectParms))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return arraylist;
        }

        public bool UpdateTaxisToUp(string classCode, int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT TOP 1 ClassCode, Taxis FROM siteserver_GovPublicCategoryClass WHERE ((Taxis > (SELECT Taxis FROM siteserver_GovPublicCategoryClass WHERE ClassCode = '{0}' AND PublishmentSystemID = {1})) AND PublishmentSystemID ={1}) ORDER BY Taxis", classCode, publishmentSystemID);
            string higherClassCode = string.Empty;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherClassCode = rdr.GetValue(0).ToString();
                    higherTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(classCode, publishmentSystemID);

            if (!string.IsNullOrEmpty(higherClassCode))
            {
                SetTaxis(classCode, publishmentSystemID, higherTaxis);
                SetTaxis(higherClassCode, publishmentSystemID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(string classCode, int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT TOP 1 ClassCode, Taxis FROM siteserver_GovPublicCategoryClass WHERE ((Taxis < (SELECT Taxis FROM siteserver_GovPublicCategoryClass WHERE ClassCode = '{0}' AND PublishmentSystemID = {1})) AND PublishmentSystemID = {1}) ORDER BY Taxis DESC", classCode, publishmentSystemID);
            string lowerClassCode = string.Empty;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerClassCode = rdr.GetValue(0).ToString();
                    lowerTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(classCode, publishmentSystemID);

            if (!string.IsNullOrEmpty(lowerClassCode))
            {
                SetTaxis(classCode, publishmentSystemID, lowerTaxis);
                SetTaxis(lowerClassCode, publishmentSystemID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM siteserver_GovPublicCategoryClass WHERE PublishmentSystemID = {0}", publishmentSystemID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(string classCode, int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT Taxis FROM siteserver_GovPublicCategoryClass WHERE ClassCode = '{0}' AND PublishmentSystemID = {1}", classCode, publishmentSystemID);
            int taxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    taxis = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(string classCode, int publishmentSystemID, int taxis)
        {
            string sqlString = string.Format("UPDATE siteserver_GovPublicCategoryClass SET Taxis = {0} WHERE ClassCode = '{1}' AND PublishmentSystemID = {2}", taxis, classCode, publishmentSystemID);
            this.ExecuteNonQuery(sqlString);
        }
	}
}