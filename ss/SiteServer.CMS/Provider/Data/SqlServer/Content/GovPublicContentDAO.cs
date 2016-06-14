using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using BaiRong.Core.AuxiliaryTable;

using SiteServer.CMS.Core.Security;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class GovPublicContentDAO : DataProviderBase, IGovPublicContentDAO
	{
        public GovPublicContentInfo GetContentInfo(PublishmentSystemInfo publishmentSystemInfo, int contentID)
        {
            GovPublicContentInfo info = null;
            if (contentID > 0)
            {
                if (!string.IsNullOrEmpty(publishmentSystemInfo.AuxiliaryTableForGovPublic))
                {
                    string SQL_WHERE = string.Format("WHERE ID = {0}", contentID);
                    string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(publishmentSystemInfo.AuxiliaryTableForGovPublic, SqlUtils.Asterisk, SQL_WHERE);

                    using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                    {
                        if (rdr.Read())
                        {
                            info = new GovPublicContentInfo();
                            BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                        }
                        rdr.Close();
                    }
                }
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public int GetContentNum(PublishmentSystemInfo publishmentSystemInfo)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1}", publishmentSystemInfo.AuxiliaryTableForGovPublic, publishmentSystemInfo.PublishmentSystemID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentID(PublishmentSystemInfo publishmentSystemInfo, int departmentID, DateTime begin, DateTime end)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1} AND DepartmentID = {2} AND (AddDate BETWEEN '{3}' AND '{4}')", publishmentSystemInfo.AuxiliaryTableForGovPublic, publishmentSystemInfo.PublishmentSystemID, departmentID, begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectCommendByNodeID(PublishmentSystemInfo publishmentSystemInfo, int nodeID)
        {
            return BaiRongDataProvider.ContentDAO.GetSelectCommend(publishmentSystemInfo.AuxiliaryTableForGovPublic, nodeID, ETriState.All);
        }

        public string GetSelectCommendByDepartmentID(PublishmentSystemInfo publishmentSystemInfo, int departmentID)
        {
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(publishmentSystemInfo.Additional.GovPublicNodeID);
            nodeIDArrayList.Add(publishmentSystemInfo.Additional.GovPublicNodeID);

            string whereString = string.Format("DepartmentID = {0}", departmentID);

            return BaiRongDataProvider.ContentDAO.GetSelectCommendByWhere(publishmentSystemInfo.AuxiliaryTableForGovPublic, publishmentSystemInfo.PublishmentSystemID, nodeIDArrayList, whereString, ETriState.All);
        }

        public string GetSelectCommendByCategoryID(PublishmentSystemInfo publishmentSystemInfo, string classCode, int categoryID)
        {
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(publishmentSystemInfo.Additional.GovPublicNodeID);
            nodeIDArrayList.Add(publishmentSystemInfo.Additional.GovPublicNodeID);

            string attributeName = DataProvider.GovPublicCategoryClassDAO.GetContentAttributeName(classCode, publishmentSystemInfo.PublishmentSystemID);

            string whereString = string.Format("{0} = {1}", attributeName, categoryID);

            return BaiRongDataProvider.ContentDAO.GetSelectCommendByWhere(publishmentSystemInfo.AuxiliaryTableForGovPublic, publishmentSystemInfo.PublishmentSystemID, nodeIDArrayList, whereString, ETriState.All);
        }

        public void CreateIdentifier(PublishmentSystemInfo publishmentSystemInfo, int parentNodeID, bool isAll)
        {
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(parentNodeID);
            nodeIDArrayList.Add(parentNodeID);
            foreach (int nodeID in nodeIDArrayList)
            {
                ArrayList contentIDArrayList = BaiRongDataProvider.ContentDAO.GetContentIDArrayList(publishmentSystemInfo.AuxiliaryTableForGovPublic, nodeID);
                foreach (int contentID in contentIDArrayList)
                {
                    GovPublicContentInfo contentInfo = this.GetContentInfo(publishmentSystemInfo, contentID);
                    bool isCreate = false;
                    if (isAll || string.IsNullOrEmpty(contentInfo.Identifier))
                    {
                        isCreate = true;
                    }
                    if (isCreate)
                    {
                        string identifier = GovPublicManager.GetIdentifier(publishmentSystemInfo, contentInfo.NodeID, contentInfo.DepartmentID, contentInfo);
                        contentInfo.Identifier = identifier;
                        DataProvider.ContentDAO.Update(publishmentSystemInfo.AuxiliaryTableForGovPublic, publishmentSystemInfo, contentInfo);
                    }
                }
            }
        }
	}
}
