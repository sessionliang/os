using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Provider.Data.SqlServer
{
    public class ContentModelDAO : DataProviderBase, IContentModelDAO
    {
        public ContentModelDAO()
        {
        }

        private const string SQL_INSERT = "INSERT INTO bairong_ContentModel (ModelID, ProductID, SiteID, ModelName, IsSystem, TableName, TableType, IconUrl, Description) VALUES (@ModelID, @ProductID, @SiteID, @ModelName, @IsSystem, @TableName, @TableType, @IconUrl, @Description)";

        private const string SQL_UPDATE = "UPDATE bairong_ContentModel SET ModelName = @ModelName, TableName = @TableName, TableType = @TableType, IconUrl = @IconUrl, Description = @Description WHERE ModelID = @ModelID AND ProductID = @ProductID AND SiteID = @SiteID";

        private const string SQL_DELETE = "DELETE FROM bairong_ContentModel WHERE ModelID = @ModelID AND ProductID = @ProductID AND SiteID = @SiteID";

        private const string SQL_SELECT = "SELECT ModelID, ProductID, SiteID, ModelName, IsSystem, TableName, TableType, IconUrl, Description FROM bairong_ContentModel WHERE ModelID = @ModelID AND ProductID = @ProductID AND SiteID = @SiteID";

        private const string PARM_MODEL_ID = "@ModelID";
        private const string PARM_PRODUCT_ID = "@ProductID";
        private const string PARM_SITE_ID = "@SiteID";
        private const string PARM_MODEL_NAME = "@ModelName";
        private const string PARM_IS_SYSTEM = "@IsSystem";
        private const string PARM_TABLE_NAME = "@TableName";
        private const string PARM_TABLE_TYPE = "@TableType";
        private const string PARM_ICON_URL = "@IconUrl";
        private const string PARM_DESCRIPTION = "@Description";

        public void Insert(ContentModelInfo contentModelInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_MODEL_ID, EDataType.VarChar, 50, contentModelInfo.ModelID),
                this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, contentModelInfo.ProductID),
				this.GetParameter(PARM_SITE_ID, EDataType.Integer, contentModelInfo.SiteID),
                this.GetParameter(PARM_MODEL_NAME, EDataType.NVarChar, 50, contentModelInfo.ModelName),
                this.GetParameter(PARM_IS_SYSTEM, EDataType.VarChar, 18, contentModelInfo.IsSystem.ToString()),
				this.GetParameter(PARM_TABLE_NAME, EDataType.VarChar, 200, contentModelInfo.TableName),
                this.GetParameter(PARM_TABLE_TYPE, EDataType.VarChar, 50, EAuxiliaryTableTypeUtils.GetValue(contentModelInfo.TableType)),
                this.GetParameter(PARM_ICON_URL, EDataType.VarChar, 50, contentModelInfo.IconUrl),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, contentModelInfo.Description)
			};

            base.ExecuteNonQuery(SQL_INSERT, parms);

            ContentModelUtils.RemoveCache(contentModelInfo.ProductID, contentModelInfo.SiteID);
        }

        public void Update(ContentModelInfo contentModelInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_MODEL_NAME, EDataType.NVarChar, 50, contentModelInfo.ModelName),
                this.GetParameter(PARM_TABLE_NAME, EDataType.VarChar, 200, contentModelInfo.TableName),
                this.GetParameter(PARM_TABLE_TYPE, EDataType.VarChar, 50, EAuxiliaryTableTypeUtils.GetValue(contentModelInfo.TableType)),
                this.GetParameter(PARM_ICON_URL, EDataType.VarChar, 50, contentModelInfo.IconUrl),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, contentModelInfo.Description),
                this.GetParameter(PARM_MODEL_ID, EDataType.VarChar, 50, contentModelInfo.ModelID),
                this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, contentModelInfo.ProductID),
				this.GetParameter(PARM_SITE_ID, EDataType.Integer, contentModelInfo.SiteID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);

            ContentModelUtils.RemoveCache(contentModelInfo.ProductID, contentModelInfo.SiteID);
        }

        public void Delete(string modelID, string productID, int siteID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_MODEL_ID, EDataType.VarChar, 50, modelID),
                this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, productID),
				this.GetParameter(PARM_SITE_ID, EDataType.Integer, siteID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);

            ContentModelUtils.RemoveCache(productID, siteID);
        }

        public ContentModelInfo GetContentModelInfo(string modelID, string productID, int siteID)
        {
            ContentModelInfo contentModelInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_MODEL_ID, EDataType.VarChar, 50, modelID),
                this.GetParameter(PARM_PRODUCT_ID, EDataType.VarChar, 50, productID),
				this.GetParameter(PARM_SITE_ID, EDataType.Integer, siteID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    contentModelInfo = new ContentModelInfo(rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetValue(3).ToString(), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString(), EAuxiliaryTableTypeUtils.GetEnumType(rdr.GetValue(6).ToString()), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString());
                }
                rdr.Close();
            }

            return contentModelInfo;
        }

        public IEnumerable GetDataSource(string productID, int siteID)
        {
            string sqlString = string.Format("SELECT ModelID, ProductID, SiteID, ModelName, IsSystem, TableName, TableType, IconUrl, Description FROM bairong_ContentModel WHERE ProductID = '{0}' AND SiteID = {1}", PageUtils.FilterSql(productID), siteID);
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }

        public ArrayList GetContentModelInfoArrayList(string productID, int siteID)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT ModelID, ProductID, SiteID, ModelName, IsSystem, TableName, TableType, IconUrl, Description FROM bairong_ContentModel WHERE ProductID = '{0}' AND SiteID = {1}", PageUtils.FilterSql(productID), siteID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    arraylist.Add(new ContentModelInfo(rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetValue(3).ToString(), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString(), EAuxiliaryTableTypeUtils.GetEnumType(rdr.GetValue(6).ToString()), rdr.GetValue(7).ToString(), rdr.GetValue(8).ToString()));
                }
                rdr.Close();
            }

            return arraylist;
        }

        public string GetImportContentModelID(int publishmentSystemID, string modelID, string productID)
        {
            string importModelID;
            if (modelID.IndexOf("_") != -1)
            {
                int modelID_Count = 0;
                string lastModeID = modelID.Substring(modelID.LastIndexOf("_") + 1);
                string firstModeID = modelID.Substring(0, modelID.Length - lastModeID.Length);
                try
                {
                    modelID_Count = int.Parse(lastModeID);
                }
                catch { }
                modelID_Count++;
                importModelID = firstModeID + modelID_Count;
            }
            else
            {
                importModelID = modelID + "_1";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_MODEL_ID, EDataType.NVarChar, 50, importModelID),
				this.GetParameter(PARM_SITE_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_PRODUCT_ID,EDataType.NVarChar,50,productID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    importModelID = GetImportContentModelID(publishmentSystemID, importModelID,productID);
                }
                rdr.Close();
            }

            return importModelID;
        }
    }
}
