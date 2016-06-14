using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class GovPublicIdentifierSeqDAO : DataProviderBase, IGovPublicIdentifierSeqDAO
	{
        public int GetSequence(int publishmentSystemID, int nodeID, int departmentID, int addYear, int ruleSequence)
        {
            int sequence = 0;

            string sqlString = string.Format("SELECT Sequence FROM siteserver_GovPublicIdentifierSeq WHERE PublishmentSystemID = {0} AND NodeID = {1} AND DepartmentID = {2} AND AddYear = {3}", publishmentSystemID, nodeID, departmentID, addYear);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read() && !rdr.IsDBNull(0))
                {
                    sequence = rdr.GetInt32(0) + 1;
                }
                rdr.Close();
            }

            if (sequence > 0)
            {
                string sqlUpdate = string.Format("UPDATE siteserver_GovPublicIdentifierSeq SET Sequence = {0} WHERE PublishmentSystemID = {1} AND NodeID = {2} AND DepartmentID = {3} AND AddYear = {4}", sequence, publishmentSystemID, nodeID, departmentID, addYear);
                base.ExecuteNonQuery(sqlUpdate);
            }
            else
            {
                sequence = ruleSequence;

                string sqlInsert = string.Format("INSERT INTO siteserver_GovPublicIdentifierSeq (PublishmentSystemID, NodeID, DepartmentID, AddYear, Sequence) VALUES ({0}, {1}, {2}, {3}, {4})", publishmentSystemID, nodeID, departmentID, addYear, sequence);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    sqlInsert = string.Format("INSERT INTO siteserver_GovPublicIdentifierSeq (SeqID, PublishmentSystemID, NodeID, DepartmentID, AddYear, Sequence) VALUES (siteserver_GovPublicIS_SEQ.NEXTVAL, {0}, {1}, {2}, {3}, {4})", publishmentSystemID, nodeID, departmentID, addYear, sequence);
                }

                base.ExecuteNonQuery(sqlInsert);

                sequence += 1;
            }

            return sequence;
        }
	}
}