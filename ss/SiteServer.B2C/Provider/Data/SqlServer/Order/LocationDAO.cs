using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using System.Data;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;

namespace SiteServer.B2C.Provider.Data.SqlServer
{
    public class LocationDAO : DataProviderBase, ILocationDAO
    {
        private const string SQL_SELECT = "SELECT ID, PublishmentSystemID, LocationName, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis FROM b2c_Location WHERE ID = @ID";

        private const string SQL_SELECT_ALL = "SELECT ID, PublishmentSystemID, LocationName, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis FROM b2c_Location WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY TAXIS";

        private const string SQL_SELECT_NAME = "SELECT LocationName FROM b2c_Location WHERE ID = @ID";

        private const string SQL_SELECT_ID = "SELECT ID FROM b2c_Location WHERE ID = @ID";

        private const string SQL_SELECT_PARENT_ID = "SELECT ParentID FROM b2c_Location WHERE ID = @ID";

        private const string SQL_SELECT_COUNT = "SELECT COUNT(*) FROM b2c_Location WHERE PublishmentSystemID = @PublishmentSystemID AND ParentID = @ParentID";

        private const string SQL_UPDATE = "UPDATE b2c_Location SET LocationName = @LocationName, ParentsPath = @ParentsPath, ParentsCount = @ParentsCount, ChildrenCount = @ChildrenCount, IsLastNode = @IsLastNode WHERE ID = @ID";

        private const string PARM_ID = "@ID";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_NAME = "@LocationName";
        private const string PARM_PARENT_ID = "@ParentID";
        private const string PARM_PARENTS_PATH = "@ParentsPath";
        private const string PARM_PARENTS_COUNT = "@ParentsCount";
        private const string PARM_CHILDREN_COUNT = "@ChildrenCount";
        private const string PARM_IS_LAST_NODE = "@IsLastNode";
        private const string PARM_TAXIS = "@Taxis";

        private void InsertWithTrans(int publishmentSystemID, LocationInfo parentInfo, LocationInfo locationInfo, IDbTransaction trans)
        {
            if (parentInfo != null)
            {
                locationInfo.ParentsPath = parentInfo.ParentsPath + "," + parentInfo.ID;
                locationInfo.ParentsCount = parentInfo.ParentsCount + 1;
                locationInfo.PublishmentSystemID = publishmentSystemID;

                int maxTaxis = this.GetMaxTaxisByParentPath(publishmentSystemID, locationInfo.ParentsPath);
                if (maxTaxis == 0)
                {
                    maxTaxis = parentInfo.Taxis;
                }
                locationInfo.Taxis = maxTaxis + 1;
            }
            else
            {
                locationInfo.ParentsPath = "0";
                locationInfo.ParentsCount = 0;
                int maxTaxis = this.GetMaxTaxisByParentPath(publishmentSystemID, "0");
                locationInfo.Taxis = maxTaxis + 1;
            }

            string SQL_INSERT = "INSERT INTO b2c_Location (PublishmentSystemID, LocationName, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis) VALUES (@PublishmentSystemID, @LocationName, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @IsLastNode, @Taxis)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT = "INSERT INTO b2c_Location (ID, PublishmentSystemID, LocationName, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, Taxis) VALUES (b2c_Location_SEQ.NEXTVAL, @PublishmentSystemID, @LocationName, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @IsLastNode, @Taxis)";
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, locationInfo.PublishmentSystemID),
				this.GetParameter(PARM_NAME, EDataType.NVarChar, 255, locationInfo.LocationName),
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, locationInfo.ParentID),
				this.GetParameter(PARM_PARENTS_PATH, EDataType.NVarChar, 255, locationInfo.ParentsPath),
				this.GetParameter(PARM_PARENTS_COUNT, EDataType.Integer, locationInfo.ParentsCount),
				this.GetParameter(PARM_CHILDREN_COUNT, EDataType.Integer, 0),
				this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, true.ToString()),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, locationInfo.Taxis)
			};

            string sqlString = string.Format("UPDATE b2c_Location SET Taxis = Taxis + 1 WHERE PublishmentSystemID = {0} AND Taxis >= {1}", publishmentSystemID, locationInfo.Taxis);
            this.ExecuteNonQuery(trans, sqlString);

            this.ExecuteNonQuery(trans, SQL_INSERT, insertParms);

            locationInfo.ID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "b2c_Location");

            if (!string.IsNullOrEmpty(locationInfo.ParentsPath))
            {
                sqlString = string.Format("UPDATE b2c_Location SET ChildrenCount = ChildrenCount + 1 WHERE PublishmentSystemID = {0} AND ID in ({1})", publishmentSystemID, locationInfo.ParentsPath);

                this.ExecuteNonQuery(trans, sqlString);
            }

            sqlString = string.Format("UPDATE b2c_Location SET IsLastNode = 'False' WHERE PublishmentSystemID = {0} AND ParentID = {1}", publishmentSystemID, locationInfo.ParentID);

            this.ExecuteNonQuery(trans, sqlString);

            sqlString = string.Format("UPDATE b2c_Location SET IsLastNode = 'True' WHERE (ID IN (SELECT TOP 1 ID FROM b2c_Location WHERE PublishmentSystemID = {0} AND ParentID = {1} ORDER BY Taxis DESC))", publishmentSystemID, locationInfo.ParentID);
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = string.Format(@"UPDATE b2c_Location SET IsLastNode = 'True' WHERE (ID IN (
SELECT ID FROM (
    SELECT ID FROM b2c_Location WHERE PublishmentSystemID = {0} AND ParentID = {1} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", publishmentSystemID, locationInfo.ParentID);
            }
            this.ExecuteNonQuery(trans, sqlString);

            LocationManager.ClearCache(publishmentSystemID);
        }

        private void UpdateSubtractChildrenCount(int publishmentSystemID, string parentsPath, int subtractNum)
        {
            if (!string.IsNullOrEmpty(parentsPath))
            {
                string sqlString = string.Format("UPDATE b2c_Location SET ChildrenCount = ChildrenCount - {0} WHERE PublishmentSystemID = {1} AND ID IN ({2})", subtractNum, publishmentSystemID, parentsPath);
                this.ExecuteNonQuery(sqlString);

                LocationManager.ClearCache(publishmentSystemID);
            }
        }

        private void TaxisSubtract(int publishmentSystemID, int selectedID)
        {
            LocationInfo locationInfo = this.GetLocationInfo(selectedID);
            if (locationInfo == null) return;
            //Get Lower Taxis and ID
            int lowerID = 0;
            int lowerChildrenCount = 0;
            string lowerParentsPath = "";
            string sqlString = @"SELECT TOP 1 ID, ChildrenCount, ParentsPath
FROM b2c_Location
WHERE PublishmentSystemID = @PublishmentSystemID AND ParentID = @ParentID AND ID <> @ID AND Taxis < @Taxis
ORDER BY Taxis DESC";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, locationInfo.ParentID),
				this.GetParameter(PARM_ID, EDataType.Integer, locationInfo.ID),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, locationInfo.Taxis),
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    lowerID = Convert.ToInt32(rdr[0]);
                    lowerChildrenCount = Convert.ToInt32(rdr[1]);
                    lowerParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            string lowerNodePath = String.Concat(lowerParentsPath, ",", lowerID);
            string selectedNodePath = String.Concat(locationInfo.ParentsPath, ",", locationInfo.ID);

            this.SetTaxisSubtract(publishmentSystemID, selectedID, selectedNodePath, lowerChildrenCount + 1);
            this.SetTaxisAdd(publishmentSystemID, lowerID, lowerNodePath, locationInfo.ChildrenCount + 1);

            this.UpdateIsLastNode(publishmentSystemID, locationInfo.ParentID);
        }

        private void TaxisAdd(int publishmentSystemID, int selectedID)
        {
            LocationInfo locationInfo = this.GetLocationInfo(selectedID);
            if (locationInfo == null) return;
            //Get Higher Taxis and ID
            int higherID = 0;
            int higherChildrenCount = 0;
            string higherParentsPath = "";
            string sqlString = @"SELECT TOP 1 ID, ChildrenCount, ParentsPath
FROM b2c_Location
WHERE PublishmentSystemID = @PublishmentSystemID AND ParentID = @ParentID AND ID <> @ID AND Taxis > @Taxis
ORDER BY Taxis";

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				this.GetParameter(PARM_PARENT_ID, EDataType.Integer, locationInfo.ParentID),
				this.GetParameter(PARM_ID, EDataType.Integer, locationInfo.ID),
				this.GetParameter(PARM_TAXIS, EDataType.Integer, locationInfo.Taxis)
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    higherID = Convert.ToInt32(rdr[0]);
                    higherChildrenCount = Convert.ToInt32(rdr[1]);
                    higherParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            string higherNodePath = String.Concat(higherParentsPath, ",", higherID);
            string selectedNodePath = String.Concat(locationInfo.ParentsPath, ",", locationInfo.ID);

            this.SetTaxisAdd(publishmentSystemID, selectedID, selectedNodePath, higherChildrenCount + 1);
            this.SetTaxisSubtract(publishmentSystemID, higherID, higherNodePath, locationInfo.ChildrenCount + 1);

            this.UpdateIsLastNode(publishmentSystemID, locationInfo.ParentID);
        }

        private void SetTaxisAdd(int publishmentSystemID, int locationID, string parentsPath, int addNum)
        {
            string sqlString = string.Format("UPDATE b2c_Location SET Taxis = Taxis + {0} WHERE ID = {1} OR ParentsPath = '{2}' OR ParentsPath like '{2},%'", addNum, locationID, parentsPath);

            this.ExecuteNonQuery(sqlString);

            LocationManager.ClearCache(publishmentSystemID);
        }

        private void SetTaxisSubtract(int publishmentSystemID, int locationID, string parentsPath, int subtractNum)
        {
            string sqlString = string.Format("UPDATE b2c_Location SET Taxis = Taxis - {0} WHERE ID = {1} OR ParentsPath = '{2}' OR ParentsPath like '{2},%'", subtractNum, locationID, parentsPath);

            this.ExecuteNonQuery(sqlString);

            LocationManager.ClearCache(publishmentSystemID);
        }

        private void UpdateIsLastNode(int publishmentSystemID, int parentID)
        {
            if (parentID > 0)
            {
                string sqlString = "UPDATE b2c_Location SET IsLastNode = @IsLastNode WHERE PublishmentSystemID = @PublishmentSystemID AND ParentID = @ParentID";

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
				    this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, false.ToString()),
                    this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
				    this.GetParameter(PARM_PARENT_ID, EDataType.Integer, parentID)
			    };

                this.ExecuteNonQuery(sqlString, parms);

                sqlString = string.Format("UPDATE b2c_Location SET IsLastNode = '{0}' WHERE (ID IN (SELECT TOP 1 ID FROM b2c_Location WHERE PublishmentSystemID = {1} AND ParentID = {2} ORDER BY Taxis DESC))", true.ToString(), publishmentSystemID, parentID);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    sqlString = string.Format(@"UPDATE b2c_Location SET IsLastNode = '{0}' WHERE (ID IN (
SELECT ID FROM (
    SELECT ID FROM b2c_Location WHERE PublishmentSystemID = {1} AND ParentID = {2} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", true.ToString(), publishmentSystemID, parentID);
                }

                this.ExecuteNonQuery(sqlString);
            }
        }

        private int GetMaxTaxisByParentPath(int publishmentSystemID, string parentPath)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) AS MaxTaxis FROM b2c_Location WHERE PublishmentSystemID = {0} AND (ParentsPath = '{1}' OR ParentsPath LIKE '{1},%')", publishmentSystemID, parentPath);
            int maxTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        maxTaxis = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return maxTaxis;
        }

        private int GetParentID(int locationID)
        {
            int parentID = 0;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, locationID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PARENT_ID, nodeParms))
            {
                if (rdr.Read())
                {
                    parentID = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }
            return parentID;
        }

        private int GetIDByParentIDAndOrder(int parentID, int order)
        {
            int ID = parentID;

            string CMD = string.Format("SELECT ID FROM b2c_Location WHERE (ParentID = {0}) ORDER BY Taxis", parentID);

            using (IDataReader rdr = this.ExecuteReader(CMD))
            {
                int index = 1;
                while (rdr.Read())
                {
                    ID = Convert.ToInt32(rdr[0]);
                    if (index == order)
                        break;
                    index++;
                }
                rdr.Close();
            }
            return ID;
        }

        public int Insert(int publishmentSystemID, LocationInfo locationInfo)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        LocationInfo parentLocationInfo = this.GetLocationInfo(locationInfo.ParentID);

                        this.InsertWithTrans(publishmentSystemID, parentLocationInfo, locationInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            LocationManager.ClearCache(publishmentSystemID);

            return locationInfo.ID;
        }

        public void Update(int publishmentSystemID, LocationInfo locationInfo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_NAME, EDataType.NVarChar, 255, locationInfo.LocationName),
				this.GetParameter(PARM_PARENTS_PATH, EDataType.NVarChar, 255, locationInfo.ParentsPath),
				this.GetParameter(PARM_PARENTS_COUNT, EDataType.Integer, locationInfo.ParentsCount),
				this.GetParameter(PARM_CHILDREN_COUNT, EDataType.Integer, locationInfo.ChildrenCount),
				this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, locationInfo.IsLastNode.ToString()),
				this.GetParameter(PARM_ID, EDataType.Integer, locationInfo.ID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, updateParms);

            LocationManager.ClearCache(publishmentSystemID);
        }

        public void UpdateTaxis(int publishmentSystemID, int selectedID, bool isSubtract)
        {
            if (isSubtract)
            {
                this.TaxisSubtract(publishmentSystemID, selectedID);
            }
            else
            {
                this.TaxisAdd(publishmentSystemID, selectedID);
            }
        }

        public void Delete(int publishmentSystemID, int locationID)
        {
            LocationInfo locationInfo = this.GetLocationInfo(locationID);
            if (locationInfo != null)
            {
                ArrayList locationIDArrayList = new ArrayList();
                if (locationInfo.ChildrenCount > 0)
                {
                    locationIDArrayList = this.GetIDArrayListForDescendant(publishmentSystemID, locationID);
                }
                locationIDArrayList.Add(locationID);

                string sqlString = string.Format("DELETE FROM b2c_Location WHERE ID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(locationIDArrayList));

                int deletedNum = 0;

                using (IDbConnection conn = this.GetConnection())
                {
                    conn.Open();
                    using (IDbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            deletedNum = this.ExecuteNonQuery(trans, sqlString);

                            if (deletedNum > 0)
                            {
                                string sqlStringTaxis = string.Format("UPDATE b2c_Location SET Taxis = Taxis - {0} WHERE PublishmentSystemID = {1} AND Taxis > {2}", deletedNum, publishmentSystemID, locationInfo.Taxis);
                                this.ExecuteNonQuery(trans, sqlStringTaxis);
                            }

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
                this.UpdateIsLastNode(publishmentSystemID, locationInfo.ParentID);
                this.UpdateSubtractChildrenCount(publishmentSystemID, locationInfo.ParentsPath, deletedNum);
            }

            LocationManager.ClearCache(publishmentSystemID);
        }

        private LocationInfo GetLocationInfo(int locationID)
        {
            LocationInfo locationInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, locationID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    locationInfo = new LocationInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetInt32(6), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetInt32(8));
                }
                rdr.Close();
            }
            return locationInfo;
        }

        private ArrayList GetLocationInfoArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    LocationInfo locationInfo = new LocationInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetInt32(6), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetInt32(8));
                    arraylist.Add(locationInfo);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public int GetNodeCount(int publishmentSystemID, int locationID)
        {
            int nodeCount = 0;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, locationID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_COUNT, nodeParms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        nodeCount = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return nodeCount;
        }

        public ArrayList GetIDArrayListByParentID(int publishmentSystemID, int parentID)
        {
            string sqlString = string.Format(@"SELECT ID FROM b2c_Location WHERE PublishmentSystemID = {0} AND ParentID = {1} ORDER BY Taxis", publishmentSystemID, parentID);
            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int theID = Convert.ToInt32(rdr[0]);
                    list.Add(theID);
                }
                rdr.Close();
            }

            return list;
        }

        public ArrayList GetIDArrayListForDescendant(int publishmentSystemID, int locationID)
        {
            string sqlString = string.Format(@"SELECT ID
FROM b2c_Location
WHERE PublishmentSystemID = {0} AND (
    (ParentsPath LIKE '{1},%') OR
    (ParentsPath LIKE '%,{1},%') OR
    (ParentsPath LIKE '%,{1}') OR
    (ParentID = {1})
)
", publishmentSystemID, locationID);
            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int theID = Convert.ToInt32(rdr[0]);
                    list.Add(theID);
                }
                rdr.Close();
            }

            return list;
        }

        public ArrayList GetIDArrayListByIDCollection(int publishmentSystemID, string locationIDCollection)
        {
            ArrayList arrayList = new ArrayList();

            if (!string.IsNullOrEmpty(locationIDCollection))
            {
                string sqlString = string.Format(@"SELECT ID FROM b2c_Location WHERE PublishmentSystemID = {0} AND ID IN ({1})", publishmentSystemID, locationIDCollection);

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        int theID = Convert.ToInt32(rdr[0]);
                        arrayList.Add(theID);
                    }
                    rdr.Close();
                }
            }

            return arrayList;
        }

        public ArrayList GetIDArrayListByFirstIDArrayList(int publishmentSystemID, ArrayList firstIDArrayList)
        {
            ArrayList arraylist = new ArrayList();

            if (firstIDArrayList.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                foreach (int locationID in firstIDArrayList)
                {
                    builder.AppendFormat("ID = {0} OR ParentID = {0} OR ParentsPath LIKE '{0},%' OR ", locationID);
                }
                builder.Length -= 3;

                string sqlString = string.Format("SELECT ID FROM b2c_Location WHERE PublishmentSystemID = {0} AND {1} ORDER BY Taxis", publishmentSystemID, builder.ToString());

                using (IDataReader rdr = this.ExecuteReader(sqlString))
                {
                    while (rdr.Read())
                    {
                        int locationID = Convert.ToInt32(rdr[0]);
                        arraylist.Add(locationID);
                    }
                    rdr.Close();
                }
            }

            return arraylist;
        }

        public DictionaryEntryArrayList GetLocationInfoDictionaryEntryArrayList(int publishmentSystemID)
        {
            DictionaryEntryArrayList dictionary = new DictionaryEntryArrayList();

            ArrayList locationInfoArrayList = this.GetLocationInfoArrayList(publishmentSystemID);
            foreach (LocationInfo locationInfo in locationInfoArrayList)
            {
                DictionaryEntry entry = new DictionaryEntry(locationInfo.ID, locationInfo);
                dictionary.Add(entry);
            }

            return dictionary;
        }
    }
}
