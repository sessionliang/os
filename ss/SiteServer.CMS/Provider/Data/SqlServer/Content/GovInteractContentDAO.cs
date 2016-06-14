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
    public class GovInteractContentDAO : DataProviderBase, IGovInteractContentDAO
    {
        public void UpdateState(PublishmentSystemInfo publishmentSystemInfo, int contentID, EGovInteractState state)
        {
            string sqlString = string.Empty;
            if (state == EGovInteractState.Checked)
            {
                sqlString = string.Format("UPDATE {0} SET State = '{1}', IsChecked='{2}', CheckedLevel = 0 WHERE ID = {3}", publishmentSystemInfo.AuxiliaryTableForGovInteract, EGovInteractStateUtils.GetValue(state), true, contentID);
            }
            else
            {
                sqlString = string.Format("UPDATE {0} SET State = '{1}', IsChecked='{2}', CheckedLevel = 0 WHERE ID = {3}", publishmentSystemInfo.AuxiliaryTableForGovInteract, EGovInteractStateUtils.GetValue(state), false, contentID);
            }
            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateDepartmentID(PublishmentSystemInfo publishmentSystemInfo, int contentID, int departmentID)
        {
            string sqlString = string.Format("UPDATE {0} SET DepartmentID = {1} WHERE ID = {2}", publishmentSystemInfo.AuxiliaryTableForGovInteract, departmentID, contentID);
            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateDepartmentID(PublishmentSystemInfo publishmentSystemInfo, ArrayList idCollection, int departmentID)
        {
            string sqlString = string.Format("UPDATE {0} SET DepartmentID = {1} WHERE ID IN ({2})", publishmentSystemInfo.AuxiliaryTableForGovInteract, departmentID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idCollection));

            this.ExecuteNonQuery(sqlString);
        }

        public GovInteractContentInfo GetContentInfo(PublishmentSystemInfo publishmentSystemInfo, int nodeID, string queryCode)
        {
            GovInteractContentInfo info = null;
            string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0} AND NodeID = {1} AND QueryCode = '{2}'", publishmentSystemInfo.PublishmentSystemID, nodeID, queryCode);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(publishmentSystemInfo.AuxiliaryTableForGovInteract, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new GovInteractContentInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public GovInteractContentInfo GetContentInfo(PublishmentSystemInfo publishmentSystemInfo, int nodeID, NameValueCollection form)
        {
            string queryCode = GovInteractApplyManager.GetQueryCode();
            int departmentID = TranslateUtils.ToInt(form[GovInteractContentAttribute.DepartmentID]);
            string departmentName = string.Empty;
            if (departmentID > 0)
            {
                departmentName = DepartmentManager.GetDepartmentName(departmentID);
            }

            string ipAddress = PageUtils.GetIPAddress();
            string location = BaiRongDataProvider.IP2CityDAO.GetCity(ipAddress);

            GovInteractContentInfo contentInfo = new GovInteractContentInfo();
            contentInfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
            contentInfo.NodeID = nodeID;
            contentInfo.DepartmentName = departmentName;
            contentInfo.QueryCode = queryCode;
            contentInfo.State = EGovInteractState.New;
            contentInfo.AddUserName = BaiRongDataProvider.UserDAO.CurrentUserName;
            contentInfo.IPAddress = ipAddress;
            contentInfo.Location = location;
            contentInfo.AddDate = DateTime.Now;

            ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, nodeID);
            InputTypeParser.AddValuesToAttributes(ETableStyle.GovInteractContent, publishmentSystemInfo.AuxiliaryTableForGovInteract, publishmentSystemInfo, relatedIdentities, form, contentInfo.Attributes);

            //foreach (string name in form.AllKeys)
            //{
            //    if (!GovInteractContentAttribute.HiddenAttributes.Contains(name.ToLower()))
            //    {
            //        string value = form[name];
            //        if (!string.IsNullOrEmpty(value))
            //        {
            //            applyInfo.SetExtendedAttribute(name, value);
            //        }
            //    }
            //}

            return contentInfo;
        }

        public GovInteractContentInfo GetContentInfo(PublishmentSystemInfo publishmentSystemInfo, int contentID)
        {
            GovInteractContentInfo info = null;
            if (contentID > 0)
            {
                if (!string.IsNullOrEmpty(publishmentSystemInfo.AuxiliaryTableForGovInteract))
                {
                    string SQL_WHERE = string.Format("WHERE ID = {0}", contentID);
                    string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(publishmentSystemInfo.AuxiliaryTableForGovInteract, SqlUtils.Asterisk, SQL_WHERE);

                    using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                    {
                        if (rdr.Read())
                        {
                            info = new GovInteractContentInfo();
                            BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                        }
                        rdr.Close();
                    }
                }
            }

            if (info != null) info.AfterExecuteReader();
            return info;
        }

        public int GetNodeID(PublishmentSystemInfo publishmentSystemInfo, int contentID)
        {
            string sqlString = string.Format("SELECT NodeID FROM {0} WHERE ID = {1}", publishmentSystemInfo.AuxiliaryTableForGovInteract, contentID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetContentNum(PublishmentSystemInfo publishmentSystemInfo)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1}", publishmentSystemInfo.AuxiliaryTableForGovInteract, publishmentSystemInfo.PublishmentSystemID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentID(PublishmentSystemInfo publishmentSystemInfo, int departmentID, DateTime begin, DateTime end)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1} AND DepartmentID = {2} AND (AddDate BETWEEN '{3}' AND '{4}')", publishmentSystemInfo.AuxiliaryTableForGovInteract, publishmentSystemInfo.PublishmentSystemID, departmentID, begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectCommendByNodeID(PublishmentSystemInfo publishmentSystemInfo, int nodeID)
        {
            return BaiRongDataProvider.ContentDAO.GetSelectCommend(publishmentSystemInfo.AuxiliaryTableForGovInteract, nodeID, ETriState.All);
        }

        public string GetSelectCommendByDepartmentID(PublishmentSystemInfo publishmentSystemInfo, int departmentID)
        {
            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(publishmentSystemInfo.Additional.GovInteractNodeID);
            nodeIDArrayList.Add(publishmentSystemInfo.Additional.GovInteractNodeID);

            string whereString = string.Format("DepartmentID = {0}", departmentID);

            return BaiRongDataProvider.ContentDAO.GetSelectCommendByWhere(publishmentSystemInfo.AuxiliaryTableForGovInteract, publishmentSystemInfo.PublishmentSystemID, nodeIDArrayList, whereString, ETriState.All);
        }

        public string GetSelectStringByState(PublishmentSystemInfo publishmentSystemInfo, int nodeID, params EGovInteractState[] states)
        {
            StringBuilder whereBuilder = new StringBuilder();
            whereBuilder.AppendFormat("WHERE PublishmentSystemID = {0} AND NodeID = {1} AND (", publishmentSystemInfo.PublishmentSystemID, nodeID);
            foreach (EGovInteractState state in states)
            {
                whereBuilder.AppendFormat(" State = '{0}' OR", EGovInteractStateUtils.GetValue(state));
            }
            whereBuilder.Length -= 2;
            whereBuilder.Append(")");
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(publishmentSystemInfo.AuxiliaryTableForGovInteract, SqlUtils.Asterisk, whereBuilder.ToString());
        }

        public string GetSelectSqlStringWithChecked(PublishmentSystemInfo publishmentSystemInfo, int nodeID, bool isReplyExists, bool isReply, int startNum, int totalNum, string whereString, string orderByString, NameValueCollection otherAttributes)
        {
            if (!string.IsNullOrEmpty(whereString) && !StringUtils.StartsWithIgnoreCase(whereString.Trim(), "AND "))
            {
                whereString = "AND " + whereString.Trim();
            }
            string sqlWhereString = string.Format("WHERE NodeID = {0} AND IsPublic = '{1}' {2}", nodeID, true.ToString(), whereString);
            if (isReplyExists)
            {
                if (isReply)
                {
                    sqlWhereString += string.Format(" AND State = '{0}'", EGovInteractStateUtils.GetValue(EGovInteractState.Checked));
                }
                else
                {
                    sqlWhereString += string.Format(" AND State <> '{0}' AND State <> '{1}'", EGovInteractStateUtils.GetValue(EGovInteractState.Checked), EGovInteractStateUtils.GetValue(EGovInteractState.Denied));
                }
            }
            if (otherAttributes != null && otherAttributes.Count > 0)
            {
                ArrayList relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(publishmentSystemInfo.PublishmentSystemID, nodeID);
                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(ETableStyle.GovInteractContent, publishmentSystemInfo.AuxiliaryTableForGovInteract, relatedIdentities);
                foreach (TableStyleInfo tableStyleInfo in styleInfoArrayList)
                {
                    if (!string.IsNullOrEmpty(otherAttributes[tableStyleInfo.AttributeName.ToLower()]))
                    {
                        sqlWhereString += string.Format(" AND ({0} like '%{1}={2}%')", ContentAttribute.SettingsXML, tableStyleInfo.AttributeName, otherAttributes[tableStyleInfo.AttributeName.ToLower()]);
                    }
                }
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(publishmentSystemInfo.AuxiliaryTableForGovInteract, startNum, totalNum, SqlUtils.Asterisk, sqlWhereString, orderByString);
        }

        public string GetSelectString(PublishmentSystemInfo publishmentSystemInfo, int nodeID)
        {
            string whereString = string.Format("WHERE PublishmentSystemID = {0} AND NodeID = {1}", publishmentSystemInfo.PublishmentSystemID, nodeID);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(publishmentSystemInfo.AuxiliaryTableForGovInteract, SqlUtils.Asterisk, whereString);
        }

        public string GetSelectString(PublishmentSystemInfo publishmentSystemInfo, int nodeID, string state, string dateFrom, string dateTo, string keyword)
        {
            StringBuilder whereBuilder = new StringBuilder();
            whereBuilder.AppendFormat("WHERE PublishmentSystemID = {0} AND NodeID = {1}", publishmentSystemInfo.PublishmentSystemID, nodeID);

            if (!string.IsNullOrEmpty(state))
            {
                whereBuilder.AppendFormat(" AND (State = '{0}')", state);
            }
            if (!string.IsNullOrEmpty(dateFrom))
            {
                whereBuilder.AppendFormat(" AND ({0} >= '{1}')", ContentAttribute.AddDate, dateFrom);
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                whereBuilder.AppendFormat(" AND ({0} <= '{1}')", ContentAttribute.AddDate, dateTo);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                whereBuilder.AppendFormat(" AND (Title LIKE '{0}' OR Content LIKE '{0}')", PageUtils.FilterSql(keyword));
            }

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(publishmentSystemInfo.AuxiliaryTableForGovInteract, SqlUtils.Asterisk, whereBuilder.ToString());
        }

        public int GetCountByNodeID(PublishmentSystemInfo publishmentSystemInfo, int nodeID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (NodeID = {1})", publishmentSystemInfo.AuxiliaryTableForGovInteract, nodeID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByPublishmentSystemID(PublishmentSystemInfo publishmentSystemInfo)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1}", publishmentSystemInfo.AuxiliaryTableForGovInteract, publishmentSystemInfo.PublishmentSystemID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentID(PublishmentSystemInfo publishmentSystemInfo, int departmentID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1} AND DepartmentID = {2}", publishmentSystemInfo.AuxiliaryTableForGovInteract, publishmentSystemInfo.PublishmentSystemID, departmentID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentID(PublishmentSystemInfo publishmentSystemInfo, int departmentID, int nodeID, DateTime begin, DateTime end)
        {
            string sqlString = string.Empty;
            if (nodeID == 0)
            {
                sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1} AND DepartmentID = {2} AND (AddDate BETWEEN '{3}' AND '{4}')", publishmentSystemInfo.AuxiliaryTableForGovInteract, publishmentSystemInfo.PublishmentSystemID, departmentID, begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1} AND NodeID = {2} AND DepartmentID = {3} AND (AddDate BETWEEN '{4}' AND '{5}')", publishmentSystemInfo.AuxiliaryTableForGovInteract, publishmentSystemInfo.PublishmentSystemID, nodeID, departmentID, begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            }
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentIDAndState(PublishmentSystemInfo publishmentSystemInfo, int departmentID, EGovInteractState state)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1} AND DepartmentID = {2} AND State = '{3}'", publishmentSystemInfo.AuxiliaryTableForGovInteract, publishmentSystemInfo.PublishmentSystemID, departmentID, EGovInteractStateUtils.GetValue(state));
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentIDAndState(PublishmentSystemInfo publishmentSystemInfo, int departmentID, EGovInteractState state, DateTime begin, DateTime end)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1} AND DepartmentID = {2} AND State = '{3}' AND (AddDate BETWEEN '{4}' AND '{5}')", publishmentSystemInfo.AuxiliaryTableForGovInteract, publishmentSystemInfo.PublishmentSystemID, departmentID, EGovInteractStateUtils.GetValue(state), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public int GetCountByDepartmentIDAndState(PublishmentSystemInfo publishmentSystemInfo, int departmentID, int nodeID, EGovInteractState state, DateTime begin, DateTime end)
        {
            string sqlString = string.Empty;
            if (nodeID == 0)
            {
                sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1} AND DepartmentID = {2} AND State = '{3}' AND (AddDate BETWEEN '{4}' AND '{5}')", publishmentSystemInfo.AuxiliaryTableForGovInteract, publishmentSystemInfo.PublishmentSystemID, departmentID, EGovInteractStateUtils.GetValue(state), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            }
            else
            {
                sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE PublishmentSystemID = {1} AND DepartmentID = {2} AND NodeID = {3} AND State = '{4}' AND (AddDate BETWEEN '{5}' AND '{6}')", publishmentSystemInfo.AuxiliaryTableForGovInteract, publishmentSystemInfo.PublishmentSystemID, departmentID, nodeID, EGovInteractStateUtils.GetValue(state), begin.ToShortDateString(), end.AddDays(1).ToShortDateString());
            }
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }
    }
}
