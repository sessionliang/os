using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Model;
using SiteServer.B2C.Core;
using System.Collections.Generic;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class SpecComboDAO : DataProviderBase, ISpecComboDAO
	{
        private const string SQL_UPDATE = "UPDATE b2c_SpecCombo SET ItemIDCollection = @ItemIDCollection WHERE PublishmentSystemID = @PublishmentSystemID AND ContentID = @ContentID AND SpecID = @SpecID";

        private const string SQL_DELETE = "DELETE FROM b2c_SpecCombo WHERE PublishmentSystemID = @PublishmentSystemID AND ContentID = @ContentID AND SpecID = @SpecID";

        private const string SQL_SELECT_ALL = "SELECT ComboID, PublishmentSystemID, ContentID, SpecID, ItemID, Title, IconUrl, PhotoIDCollection, Taxis FROM b2c_SpecCombo WHERE PublishmentSystemID = @PublishmentSystemID AND ContentID = @ContentID AND SpecID = @SpecID ORDER BY Taxis DESC";

        private const string SQL_SELECT_ITEM_ID = "SELECT ItemID FROM b2c_SpecCombo WHERE PublishmentSystemID = @PublishmentSystemID AND ContentID = @ContentID AND SpecID = @SpecID ORDER BY Taxis DESC";

        private const string PARM_COMBO_ID = "@ComboID";
		private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_CONTENT_ID = "@ContentID";
        private const string PARM_SPEC_ID = "@SpecID";
        private const string PARM_ITEM_ID = "@ItemID";
        private const string PARM_TITLE = "@Title";
        private const string PARM_ICON_URL = "@IconUrl";
        private const string PARM_PHOTO_ID_COLLECTION = "@PhotoIDCollection";
        private const string PARM_TAXIS = "@Taxis";

        public int Insert(SpecComboInfo comboInfo) 
		{
            int comboID = 0;

            string sqlString = "INSERT INTO b2c_SpecCombo (PublishmentSystemID, ContentID, SpecID, ItemID, Title, IconUrl, PhotoIDCollection, Taxis) VALUES (@PublishmentSystemID, @ContentID, @SpecID, @ItemID, @Title, @IconUrl, @PhotoIDCollection, @Taxis)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO b2c_SpecCombo (ComboID, PublishmentSystemID, ContentID, SpecID, ItemID, Title, IconUrl, PhotoIDCollection, Taxis) VALUES (b2c_SpecCombo_SEQ.NEXTVAL, @PublishmentSystemID, @ContentID, @SpecID, @ItemID, @Title, @IconUrl, @PhotoIDCollection, @Taxis)";
            }

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, comboInfo.PublishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, comboInfo.ContentID),
                this.GetParameter(PARM_SPEC_ID, EDataType.Integer, comboInfo.SpecID),
                this.GetParameter(PARM_ITEM_ID, EDataType.Integer, comboInfo.ItemID),
                this.GetParameter(PARM_TITLE, EDataType.NVarChar, 50, comboInfo.Title),
                this.GetParameter(PARM_ICON_URL, EDataType.VarChar, 200, comboInfo.IconUrl),
                this.GetParameter(PARM_PHOTO_ID_COLLECTION, EDataType.VarChar, 200, comboInfo.PhotoIDCollection),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, comboInfo.Taxis)
			};

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, parms);
                        comboID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "b2c_SpecCombo");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return comboID;
		}

        public void Update(SpecComboInfo comboInfo) 
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, comboInfo.PublishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, comboInfo.ContentID),
                this.GetParameter(PARM_SPEC_ID, EDataType.Integer, comboInfo.SpecID),
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
		}

        public void Delete(int publishmentSystemID, int contentID, int specID)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID),
                this.GetParameter(PARM_SPEC_ID, EDataType.Integer, specID),
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
		}

        public List<SpecComboInfo> GetSpecComboInfoList(int publishmentSystemID, int contentID, int specID)
        {
            List<SpecComboInfo> list = new List<SpecComboInfo>();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID),
                this.GetParameter(PARM_SPEC_ID, EDataType.Integer, specID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    SpecComboInfo comboInfo = new SpecComboInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetInt32(8));
                    list.Add(comboInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public List<SpecComboInfo> GetSpecComboInfoList(string comboIDCollection)
        {
            List<SpecComboInfo> list = new List<SpecComboInfo>();

            if (!string.IsNullOrEmpty(comboIDCollection))
            {
                string sqlString = string.Format("SELECT ComboID, PublishmentSystemID, ContentID, SpecID, ItemID, Title, IconUrl, PhotoIDCollection, Taxis FROM b2c_SpecCombo WHERE ComboID IN ({0}) ORDER BY Taxis DESC", comboIDCollection);

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        SpecComboInfo comboInfo = new SpecComboInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetInt32(8));
                        list.Add(comboInfo);
                    }
                    rdr.Close();
                }
            }

            return list;
        }

        public ArrayList GetSpecItemIDArrayList(int publishmentSystemID, int contentID, int specID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID),
                this.GetParameter(PARM_SPEC_ID, EDataType.Integer, specID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ITEM_ID, parms))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    arraylist.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return arraylist;
        }

        public IEnumerable GetDataSource(int publishmentSystemID, int contentID, int specID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID),
                this.GetParameter(PARM_SPEC_ID, EDataType.Integer, specID)
			};

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL, parms);
            return enumerable;
        }

        public void GetContentSpec(int publishmentSystemID, int contentID, out List<int> specIDList, out List<int> specItemIDList)
        {
            specIDList = new List<int>();
            specItemIDList = new List<int>();

            string sqlString = string.Format("SELECT SpecID, ItemID FROM b2c_SpecCombo WHERE PublishmentSystemID = {0} AND ContentID = {1} ORDER BY Taxis DESC", publishmentSystemID, contentID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read() && !rdr.IsDBNull(0))
                {
                    int specID = rdr.GetInt32(0);
                    int specItemID = rdr.GetInt32(1);

                    if (!specIDList.Contains(specID))
                    {
                        specIDList.Add(specID);
                    }
                    if (!specItemIDList.Contains(specItemID))
                    {
                        specItemIDList.Add(specItemID);
                    }
                }
                rdr.Close();
            }
        }

        public void GetSpec(string comboIDCollection, out List<int> specIDList, out List<int> specItemIDList)
        {
            specIDList = new List<int>();
            specItemIDList = new List<int>();

            if (!string.IsNullOrEmpty(comboIDCollection))
            {
                string sqlString = string.Format("SELECT SpecID, ItemID FROM b2c_SpecCombo WHERE ComboID IN({0}) ORDER BY Taxis DESC", comboIDCollection);

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        int specID = rdr.GetInt32(0);
                        int specItemID = rdr.GetInt32(1);

                        if (!specIDList.Contains(specID))
                        {
                            specIDList.Add(specID);
                        }
                        if (!specItemIDList.Contains(specItemID))
                        {
                            specItemIDList.Add(specItemID);
                        }
                    }
                    rdr.Close();
                }
            }
        }

        public IEnumerable GetStlDataSource(PublishmentSystemInfo publishmentSystemInfo, int nodeID, int contentID, int specID, int startNum, int totalNum)
        {
            string orderByString = "ORDER BY Taxis DESC";

            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
            string specItemIDCollection = BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, GoodsContentAttribute.SpecItemIDCollection);

            string whereString = string.Format("WHERE PublishmentSystemID = {0} AND ContentID = {1} AND SpecID = {2} AND ItemID IN ({3})", publishmentSystemInfo.PublishmentSystemID, contentID, specID, specItemIDCollection);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString("b2c_SpecCombo", startNum, totalNum, SqlUtils.Asterisk, whereString, orderByString);

            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }
	}
}