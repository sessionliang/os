using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Text;
using BaiRong.Model;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class GovInteractRemarkDAO : DataProviderBase, IGovInteractRemarkDAO
    {
        private const string SQL_SELECT = "SELECT RemarkID, PublishmentSystemID, NodeID, ContentID, RemarkType, Remark, DepartmentID, UserName, AddDate FROM siteserver_GovInteractRemark WHERE RemarkID = @RemarkID";

        private const string SQL_SELECT_ALL = "SELECT RemarkID, PublishmentSystemID, NodeID, ContentID, RemarkType, Remark, DepartmentID, UserName, AddDate FROM siteserver_GovInteractRemark WHERE PublishmentSystemID = @PublishmentSystemID AND ContentID = @ContentID";

        private const string PARM_REMARK_ID = "@RemarkID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_CONTENT_ID = "@ContentID";
        private const string PARM_REMARK_TYPE = "@RemarkType";
        private const string PARM_REMARK = "@Remark";
        private const string PARM_DEPARTMENT_ID = "@DepartmentID";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_ADD_DATE = "@AddDate";

        public void Insert(GovInteractRemarkInfo remarkInfo)
        {
            string sqlString = "INSERT INTO siteserver_GovInteractRemark(PublishmentSystemID, NodeID, ContentID, RemarkType, Remark, DepartmentID, UserName, AddDate) VALUES (@PublishmentSystemID, @NodeID, @ContentID, @RemarkType, @Remark, @DepartmentID, @UserName, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_GovInteractRemark(RemarkID, PublishmentSystemID, NodeID, ContentID, RemarkType, Remark, DepartmentID, UserName, AddDate) VALUES (siteserver_GovInteractRemark_SEQ.NEXTVAL, @PublishmentSystemID, @NodeID, @ContentID, @RemarkType, @Remark, @DepartmentID, @UserName, @AddDate)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(GovInteractRemarkDAO.PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, remarkInfo.PublishmentSystemID),
                this.GetParameter(GovInteractRemarkDAO.PARM_NODE_ID, EDataType.Integer, remarkInfo.NodeID),
                this.GetParameter(GovInteractRemarkDAO.PARM_CONTENT_ID, EDataType.Integer, remarkInfo.ContentID),
                this.GetParameter(GovInteractRemarkDAO.PARM_REMARK_TYPE, EDataType.VarChar, 50, EGovInteractRemarkTypeUtils.GetValue(remarkInfo.RemarkType)),
                this.GetParameter(GovInteractRemarkDAO.PARM_REMARK, EDataType.NVarChar, 255, remarkInfo.Remark),
                this.GetParameter(GovInteractRemarkDAO.PARM_DEPARTMENT_ID, EDataType.Integer, remarkInfo.DepartmentID),
				this.GetParameter(GovInteractRemarkDAO.PARM_USER_NAME, EDataType.VarChar, 50, remarkInfo.UserName),
                this.GetParameter(GovInteractRemarkDAO.PARM_ADD_DATE, EDataType.DateTime, remarkInfo.AddDate)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(int remarkID)
        {
            string sqlString = string.Format("DELETE FROM siteserver_GovInteractRemark WHERE RemarkID = {0}", remarkID);
            this.ExecuteNonQuery(sqlString);
        }

        public GovInteractRemarkInfo GetRemarkInfo(int remarkID)
        {
            GovInteractRemarkInfo remarkInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_REMARK_ID, EDataType.Integer, remarkID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    remarkInfo = new GovInteractRemarkInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), EGovInteractRemarkTypeUtils.GetEnumType(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString(), rdr.GetInt32(6), rdr.GetValue(7).ToString(), rdr.GetDateTime(8));
                }
                rdr.Close();
            }

            return remarkInfo;
        }

        public ArrayList GetRemarkInfoArrayList(int publishmentSystemID, int contentID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CONTENT_ID, EDataType.Integer, contentID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    GovInteractRemarkInfo remarkInfo = new GovInteractRemarkInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetInt32(3), EGovInteractRemarkTypeUtils.GetEnumType(rdr.GetValue(4).ToString()), rdr.GetValue(5).ToString(), rdr.GetInt32(6), rdr.GetValue(7).ToString(), rdr.GetDateTime(8));

                    arraylist.Add(remarkInfo);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public IEnumerable GetDataSourceByContentID(int publishmentSystemID, int contentID)
        {
            string sqlString = string.Format("SELECT RemarkID, PublishmentSystemID, NodeID, ContentID, RemarkType, Remark, DepartmentID, UserName, AddDate FROM siteserver_GovInteractRemark WHERE PublishmentSystemID = {0} AND ContentID = {1}", publishmentSystemID, contentID);

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }
    }
}
