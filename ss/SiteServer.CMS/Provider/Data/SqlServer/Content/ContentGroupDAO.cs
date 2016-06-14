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
    public class ContentGroupDAO : DataProviderBase, IContentGroupDAO
    {
        private const string SQL_INSERT_CONTENTGROUP = "INSERT INTO siteserver_ContentGroup (ContentGroupName, PublishmentSystemID, Taxis, Description) VALUES (@ContentGroupName, @PublishmentSystemID, @Taxis, @Description)";
        private const string SQL_UPDATE_CONTENTGROUP = "UPDATE siteserver_ContentGroup SET Description = @Description WHERE ContentGroupName = @ContentGroupName AND PublishmentSystemID = @PublishmentSystemID";
        private const string SQL_DELETE_CONTENTGROUP = "DELETE FROM siteserver_ContentGroup WHERE ContentGroupName = @ContentGroupName AND PublishmentSystemID = @PublishmentSystemID";

        private const string PARM_GROUP_NAME = "@ContentGroupName";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_DESCRIPTION = "@Description";


        public void Insert(ContentGroupInfo contentGroup)
        {
            int maxTaxis = this.GetMaxTaxis(contentGroup.PublishmentSystemID);
            contentGroup.Taxis = maxTaxis + 1;

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, contentGroup.ContentGroupName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, contentGroup.PublishmentSystemID),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, contentGroup.Taxis),
				this.GetParameter(PARM_DESCRIPTION, EDataType.NText, contentGroup.Description)
			};

            this.ExecuteNonQuery(SQL_INSERT_CONTENTGROUP, insertParms);
        }

        public void Update(ContentGroupInfo contentGroup)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_DESCRIPTION, EDataType.NText, contentGroup.Description),
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, contentGroup.ContentGroupName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, contentGroup.PublishmentSystemID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_CONTENTGROUP, updateParms);
        }

        public void Delete(string groupName, int publishmentSystemID)
        {
            IDbDataParameter[] contentGroupParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            this.ExecuteNonQuery(SQL_DELETE_CONTENTGROUP, contentGroupParms);
        }

        public ContentGroupInfo GetContentGroupInfo(string groupName, int publishmentSystemID)
        {
            ContentGroupInfo contentGroup = null;

            string sqlString = string.Format("SELECT ContentGroupName, PublishmentSystemID, Taxis, Description FROM siteserver_ContentGroup WHERE ContentGroupName = @ContentGroupName AND PublishmentSystemID = {0}", publishmentSystemID);

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupName)			 
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, selectParms))
            {
                if (rdr.Read())
                {
                    contentGroup = new ContentGroupInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString());
                }
                rdr.Close();
            }

            return contentGroup;
        }

        public bool IsExists(string groupName, int publishmentSystemID)
        {
            bool exists = false;

            string sqlString = string.Format("SELECT ContentGroupName FROM siteserver_ContentGroup WHERE ContentGroupName = @ContentGroupName AND PublishmentSystemID = {0}", publishmentSystemID);

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupName)			 
			};

            using (IDataReader rdr = this.ExecuteReader(sqlString, selectParms))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

        public IEnumerable GetDataSource(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT ContentGroupName, PublishmentSystemID, Taxis, Description FROM siteserver_ContentGroup WHERE PublishmentSystemID = {0} ORDER BY Taxis DESC, ContentGroupName", publishmentSystemID);
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }

        public ArrayList GetContentGroupInfoArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT ContentGroupName, PublishmentSystemID, Taxis, Description FROM siteserver_ContentGroup WHERE PublishmentSystemID = {0} ORDER BY Taxis DESC, ContentGroupName", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    arraylist.Add(new ContentGroupInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetInt32(2), rdr.GetValue(3).ToString()));
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetContentGroupNameArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = string.Format("SELECT ContentGroupName FROM siteserver_ContentGroup WHERE PublishmentSystemID = {0} ORDER BY Taxis DESC, ContentGroupName", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return arraylist;
        }

        private int GetTaxis(int publishmentSystemID, string groupName)
        {
            string sqlString = string.Format("SELECT Taxis FROM siteserver_ContentGroup WHERE (ContentGroupName = @ContentGroupName AND PublishmentSystemID = {0})", publishmentSystemID);

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupName)			 
			};

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString, selectParms);
        }

        private void SetTaxis(int publishmentSystemID, string groupName, int taxis)
        {
            string sqlString = string.Format("UPDATE siteserver_ContentGroup SET Taxis = {0} WHERE (ContentGroupName = @ContentGroupName AND PublishmentSystemID = {1})", taxis, publishmentSystemID);
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupName)			 
			};
            this.ExecuteNonQuery(sqlString, updateParms);
        }

        private int GetMaxTaxis(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM siteserver_ContentGroup WHERE (PublishmentSystemID = {0})", publishmentSystemID);
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

        public bool UpdateTaxisToUp(int publishmentSystemID, string groupName)
        {
            //Get Higher Taxis and ID
            string sqlString = string.Format("SELECT TOP 1 ContentGroupName, Taxis FROM siteserver_ContentGroup WHERE (Taxis > (SELECT Taxis FROM siteserver_ContentGroup WHERE ContentGroupName = @ContentGroupName AND PublishmentSystemID = {0}) AND PublishmentSystemID = {0}) ORDER BY Taxis", publishmentSystemID);
            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupName)			 
			};
            string higherGroupName = string.Empty;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString, selectParms))
            {
                if (rdr.Read())
                {
                    higherGroupName = rdr.GetValue(0).ToString();
                    higherTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            if (!string.IsNullOrEmpty(higherGroupName))
            {
                //Get Taxis Of Selected ID
                int selectedTaxis = GetTaxis(publishmentSystemID, groupName);

                //Set The Selected Class Taxis To Higher Level
                SetTaxis(publishmentSystemID, groupName, higherTaxis);
                //Set The Higher Class Taxis To Lower Level
                SetTaxis(publishmentSystemID, higherGroupName, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int publishmentSystemID, string groupName)
        {
            //Get Lower Taxis and ID
            string sqlString = string.Format("SELECT TOP 1 ContentGroupName, Taxis FROM siteserver_ContentGroup WHERE (Taxis < (SELECT Taxis FROM siteserver_ContentGroup WHERE ContentGroupName = @ContentGroupName AND PublishmentSystemID = {0}) AND PublishmentSystemID = {0}) ORDER BY Taxis DESC", publishmentSystemID);
            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_NAME, EDataType.NVarChar, 255, groupName)			 
			};
            string lowerGroupName = string.Empty;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString, selectParms))
            {
                if (rdr.Read())
                {
                    lowerGroupName = rdr.GetValue(0).ToString();
                    lowerTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            if (!string.IsNullOrEmpty(lowerGroupName))
            {
                //Get Taxis Of Selected Class
                int selectedTaxis = GetTaxis(publishmentSystemID, groupName);

                //Set The Selected Class Taxis To Lower Level
                SetTaxis(publishmentSystemID, groupName, lowerTaxis);
                //Set The Lower Class Taxis To Higher Level
                SetTaxis(publishmentSystemID, lowerGroupName, selectedTaxis);
                return true;
            }
            return false;
        }

    }
}