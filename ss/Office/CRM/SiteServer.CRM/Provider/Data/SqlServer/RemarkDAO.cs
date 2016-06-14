using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CRM.Model;
using SiteServer.CRM.Core;
using System.Text;
using BaiRong.Model;

namespace SiteServer.CRM.Provider.Data.SqlServer
{
    public class RemarkDAO : DataProviderBase, IRemarkDAO
    {
        private const string SQL_SELECT = "SELECT RemarkID, ApplyID, RemarkType, Remark, DepartmentID, UserName, AddDate FROM pms_Remark WHERE RemarkID = @RemarkID";

        private const string SQL_SELECT_ALL = "SELECT RemarkID, ApplyID, RemarkType, Remark, DepartmentID, UserName, AddDate FROM pms_Remark WHERE ApplyID = @ApplyID";

        private const string PARM_REMARK_ID = "@RemarkID";
        private const string PARM_APPLY_ID = "@ApplyID";
        private const string PARM_REMARK_TYPE = "@RemarkType";
        private const string PARM_REMARK = "@Remark";
        private const string PARM_DEPARTMENT_ID = "@DepartmentID";
        private const string PARM_USER_NAME = "@UserName";
        private const string PARM_ADD_DATE = "@AddDate";

        public void Insert(RemarkInfo remarkInfo)
        {
            string sqlString = "INSERT INTO pms_Remark(ApplyID, RemarkType, Remark, DepartmentID, UserName, AddDate) VALUES (@ApplyID, @RemarkType, @Remark, @DepartmentID, @UserName, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO pms_Remark(RemarkID, ApplyID, RemarkType, Remark, DepartmentID, UserName, AddDate) VALUES (pms_Remark_SEQ.NEXTVAL, @ApplyID, @RemarkType, @Remark, @DepartmentID, @UserName, @AddDate)";
            }

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(RemarkDAO.PARM_APPLY_ID, EDataType.Integer, remarkInfo.ApplyID),
                this.GetParameter(RemarkDAO.PARM_REMARK_TYPE, EDataType.VarChar, 50, ERemarkTypeUtils.GetValue(remarkInfo.RemarkType)),
                this.GetParameter(RemarkDAO.PARM_REMARK, EDataType.NVarChar, 255, remarkInfo.Remark),
                this.GetParameter(RemarkDAO.PARM_DEPARTMENT_ID, EDataType.Integer, remarkInfo.DepartmentID),
				this.GetParameter(RemarkDAO.PARM_USER_NAME, EDataType.VarChar, 50, remarkInfo.UserName),
                this.GetParameter(RemarkDAO.PARM_ADD_DATE, EDataType.DateTime, remarkInfo.AddDate)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(int remarkID)
        {
            string sqlString = string.Format("DELETE FROM pms_Remark WHERE RemarkID = {0}", remarkID);
            this.ExecuteNonQuery(sqlString);
        }

        public RemarkInfo GetRemarkInfo(int remarkID)
        {
            RemarkInfo remarkInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_REMARK_ID, EDataType.Integer, remarkID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    remarkInfo = new RemarkInfo(rdr.GetInt32(0), rdr.GetInt32(1), ERemarkTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetDateTime(6));
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
                    RemarkInfo remarkInfo = new RemarkInfo(rdr.GetInt32(0), rdr.GetInt32(1), ERemarkTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetValue(3).ToString(), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetDateTime(6));

                    arraylist.Add(remarkInfo);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public IEnumerable GetDataSourceByApplyID(int applyID)
        {
            string sqlString = string.Format("SELECT RemarkID, ApplyID, RemarkType, Remark, DepartmentID, UserName, AddDate FROM pms_Remark WHERE ApplyID = {0}", applyID);

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }
    }
}
