using System;
using System.Data;
using System.Collections.Generic;
using SiteServer.BBS.Model;
using BaiRong.Core.Data.Provider;
using System.Collections;
using SiteServer.BBS.Core;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.BBS.Provider.SqlServer
{
    public class AttachmentTypeDAO : DataProviderBase, IAttachmentTypeDAO
    {
        public void Insert(AttachmentTypeInfo info)
        {
            string sqlString = "INSERT INTO bbs_AttachmentType(PublishmentSystemID, FileExtName, MaxSize) VALUES(@PublishmentSystemID, @FileExtName, @MaxSize)";

            IDbDataParameter[] param = new IDbDataParameter[]
            {
                this.GetParameter("@PublishmentSystemID", EDataType.Integer, info.PublishmentSystemID),
                this.GetParameter("@FileExtName", EDataType.VarChar, 10,  info.FileExtName),
                this.GetParameter("@MaxSize", EDataType.Integer,  info.MaxSize)
            };

            this.ExecuteNonQuery(sqlString, param);
        }

        public void Update(AttachmentTypeInfo info)
        {
            string sqlString = "UPDATE bbs_AttachmentType SET FileExtName = @FileExtName, MaxSize = @MaxSize WHERE ID= @ID";
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter("@FileExtName", EDataType.VarChar, 10,  info.FileExtName),
                this.GetParameter("@MaxSize", EDataType.Integer,  info.MaxSize),
                this.GetParameter("@ID", EDataType.Integer,  info.ID)
			};

            this.ExecuteNonQuery(sqlString, parms);
        }

        public void Delete(int id)
        {
            string sqlString = "DELETE FROM bbs_AttachmentType WHERE ID= @ID";

            IDbDataParameter[] param = new IDbDataParameter[]
			{
                this.GetParameter("@ID", EDataType.Integer, id)
            };

            this.ExecuteNonQuery(sqlString, param);
        }

        public AttachmentTypeInfo GetAttachmentTypeInfo(int id)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, FileExtName, MaxSize FROM bbs_AttachmentType WHERE ID = {0}", id);

            AttachmentTypeInfo info = null;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    info = new AttachmentTypeInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetInt32(3));
                }
                rdr.Close();
            }
            return info;
        }

        public List<AttachmentTypeInfo> GetList(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT ID, PublishmentSystemID, FileExtName, MaxSize FROM bbs_AttachmentType WHERE PublishmentSystemID = {0}", publishmentSystemID);

            List<AttachmentTypeInfo> list = new List<AttachmentTypeInfo>();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    AttachmentTypeInfo info = new AttachmentTypeInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetInt32(3));
                    list.Add(info);
                }
                rdr.Close();
            }
            return list;
        }
    }
}