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
    public class GovPublicApplyRemarkDAO : DataProviderBase, IGovPublicApplyRemarkDAO
    {
        private const string SQL_SELECT = "SELECT RemarkID, PublishmentSystemID, ApplyID, RemarkType, Remark, DepartmentID, UserName, AddDate FROM siteserver_GovPublicApplyRemark WHERE RemarkID = @RemarkID";

        private const string SQL_SELECT_ALL = "SELECT RemarkID, PublishmentSystemID, ApplyID, RemarkType, Remark, DepartmentID, UserName, AddDate FROM siteserver_GovPublicApplyRemark WHERE ApplyID = @ApplyID";

        private const string PARM_REMARK_ID = "@RemarkID";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_APPLY_ID = "@ApplyID";
        private const string PARM_REMARK_TYPE = "@RemarkType";
        private const string PARM_REMARK = "@Remark";
        private const string PARM_DEPARTMENT_ID = "@DepartmentID";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_ADD_DATE = "@AddDate";

        public void Insert(GovPublicApplyRemarkInfo remarkInfo)
        {
            string sqlString = "INSERT INTO siteserver_GovPublicApplyRemark(PublishmentSystemID, ApplyID, RemarkType, Remark, DepartmentID, UserName, AddDate) VALUES (@PublishmentSystemID, @ApplyID, @RemarkType, @Remark, @DepartmentID, @UserName, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_GovPublicApplyRemark(RemarkID, PublishmentSystemID, ApplyID, RemarkType, Remark, DepartmentID, UserName, AddDate) VALUES (siteserver_GovPublicApplyRemark_SEQ.NEXTVAL, @PublishmentSystemID, @ApplyID, @RemarkType, @Remark, @DepartmentID, @UserName, @AddDate)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(GovPublicApplyRemarkDAO.PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, remarkInfo.PublishmentSystemID),
                this.GetParameter(GovPublicApplyRemarkDAO.PARM_APPLY_ID, EDataType.Integer, remarkInfo.ApplyID),
                this.GetParameter(GovPublicApplyRemarkDAO.PARM_REMARK_TYPE, EDataType.VarChar, 50, EGovPublicApplyRemarkTypeUtils.GetValue(remarkInfo.RemarkType)),
                this.GetParameter(GovPublicApplyRemarkDAO.PARM_REMARK, EDataType.NVarChar, 255, remarkInfo.Remark),
                this.GetParameter(GovPublicApplyRemarkDAO.PARM_DEPARTMENT_ID, EDataType.Integer, remarkInfo.DepartmentID),
				this.GetParameter(GovPublicApplyRemarkDAO.PARM_USER_NAME, EDataType.VarChar, 50, remarkInfo.UserName),
                this.GetParameter(GovPublicApplyRemarkDAO.PARM_ADD_DATE, EDataType.DateTime, remarkInfo.AddDate)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(int remarkID)
        {
            string sqlString = string.Format("DELETE FROM siteserver_GovPublicApplyRemark WHERE RemarkID = {0}", remarkID);
            this.ExecuteNonQuery(sqlString);
        }

        public GovPublicApplyRemarkInfo GetRemarkInfo(int remarkID)
        {
            GovPublicApplyRemarkInfo remarkInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_REMARK_ID, EDataType.Integer, remarkID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    remarkInfo = new GovPublicApplyRemarkInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), EGovPublicApplyRemarkTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetDateTime(7));
                }
                rdr.Close();
            }

            return remarkInfo;
        }

        public ArrayList GetRemarkInfoArrayList(int applyID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_APPLY_ID, EDataType.Integer, applyID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, parms))
            {
                while (rdr.Read())
                {
                    GovPublicApplyRemarkInfo remarkInfo = new GovPublicApplyRemarkInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetInt32(2), EGovPublicApplyRemarkTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetDateTime(7));

                    arraylist.Add(remarkInfo);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public IEnumerable GetDataSourceByApplyID(int applyID)
        {
            string sqlString = string.Format("SELECT RemarkID, PublishmentSystemID, ApplyID, RemarkType, Remark, DepartmentID, UserName, AddDate FROM siteserver_GovPublicApplyRemark WHERE ApplyID = {0}", applyID);

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }
    }
}
