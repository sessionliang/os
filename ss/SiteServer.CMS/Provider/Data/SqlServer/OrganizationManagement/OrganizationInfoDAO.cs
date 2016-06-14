using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class OrganizationInfoDAO : DataProviderBase, IOrganizationInfoDAO
    {
        public string TableName
        {
            get
            {
                return OrganizationInfo.TableName;
            }
        }

        public int Insert(OrganizationInfo info)
        {
            int ID = 0;

            info.Taxis = this.GetMaxTaxis(info.ID, info.ClassifyID) + 1;
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        ID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TableName);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
                UpdateCount(info);
            }

            return ID;
        }

        public int GetCount(int classifyID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {1} WHERE (ClassifyID = {0})", classifyID, TableName);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }
        public int GetCountByArea(int areaID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {1} WHERE (AreaID = {0})", areaID, TableName);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public void UpdateCount(OrganizationInfo info)
        {
            //修改分类下机构的数量 
            int contentNum = this.GetCount(info.ClassifyID);
            OrganizationClassifyInfo classifyInfo = DataProvider.OrganizationClassifyDAO.GetInfo(info.ClassifyID);
            DataProvider.OrganizationClassifyDAO.UpdateContentNum(classifyInfo.PublishmentSystemID, info.ClassifyID, contentNum);

            //修改区域下机构的数量
            contentNum = this.GetCountByArea(info.AreaID);
            OrganizationAreaInfo areaInfo = DataProvider.OrganizationAreaDAO.GetInfo(info.AreaID);
            DataProvider.OrganizationAreaDAO.UpdateContentNum(classifyInfo.PublishmentSystemID, info.AreaID, contentNum);
        }

        public void Update(OrganizationInfo info)
        {
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);

            UpdateCount(info);
        }

        public bool UpdateTaxisToUp(int classifyID, int ID)
        {
            string sqlString = string.Empty;
            if (classifyID > 0)
                sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM {0} WHERE ((Taxis > (SELECT Taxis FROM {0} WHERE ID = {1} AND ClassifyID = {2})) AND ClassifyID = {2}) ORDER BY Taxis", TableName, ID, classifyID);
            else
                sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM {0} WHERE ((Taxis > (SELECT Taxis FROM {0} WHERE ID = {1})) ) ORDER BY Taxis", TableName, ID);
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = Convert.ToInt32(rdr[0]);
                    higherTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(ID);

            if (higherID != 0)
            {
                SetTaxis(ID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int classifyID, int ID)
        {
            string sqlString = string.Empty;
            if (classifyID > 0)
                sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM {0} WHERE ((Taxis < (SELECT Taxis FROM {0} WHERE (ID = {1} AND ClassifyID = {2})))  AND  ClassifyID = {2}) ORDER BY Taxis DESC", TableName, ID, classifyID);
            else
                sqlString = string.Format("SELECT TOP 1 ID, Taxis FROM {0} WHERE ((Taxis < (SELECT Taxis FROM {0} WHERE ( ID = {1})))  ) ORDER BY Taxis DESC", TableName, ID);
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = Convert.ToInt32(rdr[0]);
                    lowerTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(ID);

            if (lowerID != 0)
            {
                SetTaxis(ID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        public void Delete(ArrayList deleteIDArrayList)
        {
            OrganizationInfo info = new OrganizationInfo();
            foreach (string id in deleteIDArrayList)
            {
                info = this.GetContentInfo(TranslateUtils.ToInt(id));

                UpdateCount(info);
            }
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
        }


        public void Delete(int ID)
        {
            OrganizationInfo info = this.GetContentInfo(ID);
            string sqlString = string.Format("DELETE FROM {0} WHERE ID ={1} ", TableName, ID);
            this.ExecuteNonQuery(sqlString);

            UpdateCount(info);
        }

        public void DeleteByClassifyID(int classifyID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE ClassifyID ={1} ", TableName, classifyID);
            this.ExecuteNonQuery(sqlString);
        }

        public OrganizationInfo GetContentInfo(int ID)
        {
            OrganizationInfo info = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", ID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new OrganizationInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }


        private DataSet GetDataSetByWhereString(string whereString)
        {
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString);
            return this.ExecuteDataset(SQL_SELECT);
        }

        private IEnumerable GetDataSourceByContentNumAndWhereString(int totalNum, string whereString, string orderByString)
        {
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, totalNum, SqlUtils.Asterisk, whereString, orderByString);
            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }



        public ArrayList GetContentIDArrayListByUserName(string userName)
        {
            ArrayList IDArrayList = new ArrayList();

            string sqlString = string.Format("SELECT ID FROM {0} WHERE UserName = @UserName ORDER BY AddDate DESC, Taxis DESC", TableName);

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter("@UserName", EDataType.NVarChar, 255,userName)
            };
            using (IDataReader rdr = this.ExecuteReader(sqlString, selectParms))
            {
                while (rdr.Read())
                {
                    int ID = Convert.ToInt32(rdr[0]);
                    IDArrayList.Add(ID);
                }
                rdr.Close();
            }
            return IDArrayList;
        }

        private int GetTaxis(int ID)
        {
            string sqlString = string.Format("SELECT Taxis FROM {0} WHERE ( ID = {1})", TableName, ID);
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

        private void SetTaxis(int ID, int taxis)
        {
            string sqlString = string.Format("UPDATE {0} SET Taxis = {1} WHERE ID = {2}", TableName, taxis, ID);
            this.ExecuteNonQuery(sqlString);
        }

        private int GetMaxTaxis(int ID, int classifyID)
        {
            string sqlString = string.Empty;
            if (classifyID > 0)
                sqlString = string.Format("SELECT MAX(Taxis) FROM {0} WHERE ClassifyID = {2}", TableName, ID, classifyID);
            else
                sqlString = string.Format("SELECT MAX(Taxis) FROM {0}  ", TableName, ID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSortFieldName()
        {
            return "Taxis";
        }

        /// <summary>
        /// 转移内容
        /// </summary>
        /// <param name=" IDArrayList"></param>
        /// <param name="classifyID"></param>
        public void TranslateContent(ArrayList IDArrayList, int classifyID)
        {
            OrganizationInfo info = new OrganizationInfo();
            if (IDArrayList.Count > 0)
            {
                info = this.GetContentInfo(TranslateUtils.ToInt(IDArrayList[0].ToString()));
            }
            string sqlString = string.Format("UPDATE {0} SET ClassifyID = '{1}' WHERE  ID IN ({2})", TableName, classifyID.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(IDArrayList));

            this.ExecuteNonQuery(sqlString);

            int contentNum = this.GetCount(classifyID);
            OrganizationClassifyInfo classifyInfo = DataProvider.OrganizationClassifyDAO.GetInfo(classifyID);
            DataProvider.OrganizationClassifyDAO.UpdateContentNum(classifyInfo.PublishmentSystemID, classifyID, contentNum);

            if (IDArrayList.Count > 0)
            {
                classifyInfo = DataProvider.OrganizationClassifyDAO.GetInfo(info.ClassifyID);
                if (info != null && classifyInfo != null)
                {
                    contentNum = this.GetCount(info.ClassifyID);
                    DataProvider.OrganizationClassifyDAO.UpdateContentNum(classifyInfo.PublishmentSystemID, info.ClassifyID, contentNum);
                }
            }

        }


        /// <summary>
        /// 转移内容
        /// </summary>
        /// <param name="IDArrayList"></param>
        /// <param name="classifyID"></param>
        /// <param name="areaID"></param>
        public void TranslateContent(ArrayList IDArrayList, int classifyID, int areaID)
        {
            string sqlString = string.Format(" UPDATE {0} SET ClassifyID = '{1}', AreaID ='{3}' WHERE ID IN ({2}) ; ", TableName, classifyID.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(IDArrayList), areaID);


            ArrayList list = this.GetInfoList(string.Format(" and ID in ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(IDArrayList)));

            StringBuilder sbUpdate = new StringBuilder();
            sbUpdate.Append(sqlString);

            //修改原分类和区域数量 的SQL
            //修改原分类下的数量 
            ArrayList oldClassifyList = new ArrayList();
            foreach (OrganizationInfo cinfo in list)
            {
                if (oldClassifyList.IndexOf(cinfo.ClassifyID) == -1)
                    oldClassifyList.Add(cinfo.ClassifyID);
            }
            foreach (int oldClassifyID in oldClassifyList)
            {
                sbUpdate.AppendFormat(" UPDATE {0} SET ContentNum = (SELECT COUNT(*) AS TotalNum FROM {1} WHERE ( ClassifyID = {2})) WHERE (itemID = {2}) ;", OrganizationClassifyInfo.TableName, TableName, oldClassifyID);
            }
            //修改原分类下不同区域的数量
            ArrayList oldAreaList = new ArrayList();
            foreach (OrganizationInfo cinfo in list)
            {
                if (oldAreaList.IndexOf(cinfo.AreaID) == -1)
                    oldAreaList.Add(cinfo.AreaID);
            }
            foreach (int area in oldAreaList)
            {
                sbUpdate.AppendFormat(" UPDATE {0} SET ContentNum = (SELECT COUNT(*) AS TotalNum FROM {1} WHERE ( AreaID = ({2}))) WHERE (itemID = {2}) ;", OrganizationAreaInfo.TableName, TableName, area);
            }

            //修改现在分类下的数量
            sbUpdate.AppendFormat(" UPDATE {0} SET ContentNum = (SELECT COUNT(*) AS TotalNum FROM {1} WHERE (ClassifyID = {2})) WHERE (itemID = {2}) ;", OrganizationClassifyInfo.TableName, TableName, classifyID);
            //修改现在区域下的数量
            sbUpdate.AppendFormat(" UPDATE {0} SET ContentNum = (SELECT COUNT(*) AS TotalNum FROM {1} WHERE ( AreaID = ({2}))) WHERE (itemID = {2}) ;", OrganizationAreaInfo.TableName, TableName, areaID);

            this.ExecuteNonQuery(sbUpdate.ToString());
        }

        public string GetSelectCommend(int publishmentSystemID, int classifyID, string areaID, string keyword)
        {

            string orderByString = " order by AddDate desc";

            StringBuilder whereString = new StringBuilder("WHERE 1=1");


            whereString.AppendFormat(" and PublishmentSystemID = {0} ", publishmentSystemID);

            //判断是不是所有内容
            OrganizationClassifyInfo info = DataProvider.OrganizationClassifyDAO.GetInfo(classifyID);
            if (info.ParentID != 0)
                whereString.AppendFormat(" and ClassifyID = {0} ", classifyID);

            if (!string.IsNullOrEmpty(areaID))
            {
                whereString.AppendFormat(" AND ( AreaID IN( SELECT ItemID FROM {2} WHERE ParentID= {0} AND ClassifyID = {1})", areaID, classifyID, OrganizationAreaInfo.TableName);
                whereString.AppendFormat(" or AreaID in (SELECT ItemID FROM {2} WHERE ParentID in( SELECT ItemID FROM {2} WHERE ParentID= {0} AND ClassifyID = {1} ))", areaID, classifyID, OrganizationAreaInfo.TableName);
                whereString.AppendFormat("  or AreaID=  {0}", areaID);

                whereString.AppendFormat(" OR AreaID = {0} ", areaID);

                whereString.Append(" )");
            }


            if (!string.IsNullOrEmpty(keyword))
            {
                whereString.AppendFormat(" and ( {1} like '%{0}%' ", keyword, OrganizationInfoAttribute.OrganizationName);
                whereString.AppendFormat(" or {1} like '%{0}%' ", keyword, OrganizationInfoAttribute.OrganizationAddress);
                whereString.AppendFormat(" or Phone like '%{0}%' )", keyword);
            }

            whereString.Append(" ").Append(orderByString);

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, whereString.ToString());
        }


        public bool IsExists(int classifyID, int ID, string name)
        {
            bool isExists = false;

            string SQL_WHERE = string.Format("WHERE {2} = '{0}' and ClassifyID={1}", name, classifyID, OrganizationInfoAttribute.OrganizationName);
            if (ID != 0)
                SQL_WHERE = SQL_WHERE + " and ID <>" + ID;

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);
            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    isExists = true;
                }
                rdr.Close();
            }
            return isExists;
        }

        #region 前台方法

        /// <summary>
        /// 获取所有的机构
        /// </summary>
        /// <returns></returns>
        public ArrayList GetInfoList(string whereStr)
        {
            ArrayList infoList = new ArrayList();

            string orderStr = " ORDER BY Taxis";

            string sqlString = string.Format("SELECT * FROM {0} WHERE  1=1 ", TableName);


            sqlString = sqlString + whereStr + orderStr;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    OrganizationInfo info = new OrganizationInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    infoList.Add(info);
                }
                rdr.Close();
            }
            return infoList;
        }


        /// <summary>
        /// 获取所有的机构
        /// </summary>
        /// <returns></returns>
        public ArrayList GetInfoList(int classifyID, int areaPrentID, int areaID)
        {
            StringBuilder whereSb = new StringBuilder();

            //判断是不是所有内容
            OrganizationClassifyInfo info = DataProvider.OrganizationClassifyDAO.GetInfo(classifyID);

            if (classifyID > 0)
            {
                if (info.ParentID != 0)
                {
                    whereSb.AppendFormat(" AND ClassifyID = {0}", classifyID);
                }
            }

            if (areaID == 0)
            {
                whereSb.AppendFormat(" AND ( AreaID IN( SELECT ItemID FROM {1} WHERE ParentID= {0}  ", areaPrentID, OrganizationAreaInfo.TableName);
                if (classifyID > 0)
                {
                    //判断是不是所有内容  
                    if (info.ParentID != 0)
                    {
                        whereSb.AppendFormat(" AND ClassifyID = {0}", classifyID);
                    }
                }
                whereSb.Append(" )");

                whereSb.AppendFormat(" or AreaID in (SELECT ItemID FROM {1} WHERE ParentID in( SELECT ItemID FROM {1} WHERE ParentID= {0} ", areaPrentID, OrganizationAreaInfo.TableName);
                if (classifyID > 0)
                {
                    whereSb.AppendFormat(" AND ClassifyID = {0}", classifyID);
                }
                whereSb.Append(" ))");
                whereSb.AppendFormat("  or AreaID=  {0}", areaPrentID);

                if (areaID > 0)
                    whereSb.AppendFormat(" and AreaID = {0} ", areaID);
                whereSb.Append(" )");
            }
            else
                whereSb.AppendFormat(" and AreaID = {0} ", areaID);


            return this.GetInfoList(whereSb.ToString());
        }


        /// <summary>
        /// 获取一定范围内的机构
        /// </summary>
        /// <returns></returns>
        public ArrayList GetInfoList(int classifyID, double minLat, double maxLat, double minLng, double maxLng)
        {
            StringBuilder whereSb = new StringBuilder();

            if (classifyID > 0)
            {
                whereSb.AppendFormat(" AND ClassifyID = {0}", classifyID);
            }

            whereSb.AppendFormat(" AND (Longitude between '{2}' and '{3}' and Latitude between '{0}' and '{1}') ", minLat, maxLat, minLng, maxLng);

            return this.GetInfoList(whereSb.ToString());
        }
        public ArrayList GetInfoListByPage(int classifyID, int areaPrentID, int areaID, int pageNum, int pageIndex)
        {
            StringBuilder whereSb = new StringBuilder();

            if (classifyID > 0)
            {
                whereSb.AppendFormat(" AND  ClassifyID = {0}", classifyID);
            }

            if (areaID == 0)
            {
                whereSb.AppendFormat(" AND ( AreaID IN( SELECT ItemID FROM {1} WHERE ParentID= {0}  ", areaPrentID, OrganizationAreaInfo.TableName);
                if (classifyID > 0)
                {
                    whereSb.AppendFormat(" AND ClassifyID = {0}", classifyID);
                }
                whereSb.Append(" )");

                whereSb.AppendFormat(" or AreaID in (SELECT ItemID FROM {2} WHERE ParentID in( SELECT ItemID FROM {2} WHERE ParentID= {0}   ", areaPrentID, classifyID, OrganizationAreaInfo.TableName);
                if (classifyID > 0)
                {
                    whereSb.AppendFormat(" AND ClassifyID = {0}", classifyID);
                }
                whereSb.Append(" )  ");
                whereSb.Append(" )  ");

                whereSb.AppendFormat("  or AreaID=  {0}", areaPrentID);

                if (areaID > 0)
                    whereSb.AppendFormat(" and AreaID = {0} ", areaID);

                whereSb.Append(" )");
            }
            else
                whereSb.AppendFormat(" and AreaID = {0} ", areaID);


            ArrayList infoList = new ArrayList();

            string orderStr = " ORDER BY Taxis";

            string sqlString = string.Format("SELECT TOP {1} * FROM {0} WHERE ID >(  SELECT ISNULL(MAX(ID),0)  FROM  (  SELECT TOP ({1}*({2}-1)) ID FROM {0} where 1=1 {3} ORDER BY  ID ) A  ) {3}    ", TableName, pageNum, pageIndex, whereSb.ToString());


            sqlString = sqlString + orderStr;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    OrganizationInfo info = new OrganizationInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                    infoList.Add(info);
                }
                rdr.Close();
            }
            return infoList;
        }


        public int GetCount()
        {
            string str = string.Format(" select COUNT(*) as num from {0} where Enabled='True' ", TableName);

            int num = 0;

            using (IDataReader rdr = this.ExecuteReader(str))
            {
                if (rdr.Read())
                {
                    num = int.Parse(rdr[0].ToString());
                }
                rdr.Close();
            }

            return num;
        }

        #endregion
    }
}
