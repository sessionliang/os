using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.IO;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class StlTagDAO : DataProviderBase, IStlTagDAO
    {

        // Static constants
        private const string SQL_SELECT_STL_TAG = "SELECT TagName, PublishmentSystemID, TagDescription, TagContent FROM siteserver_StlTag WHERE TagName = @TagName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_ALL_STL_TAG = "SELECT TagName, PublishmentSystemID, TagDescription, TagContent FROM siteserver_StlTag WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_STL_TAG_NAMES = "SELECT TagName FROM siteserver_StlTag WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_INSERT_STL_TAG = "INSERT INTO siteserver_StlTag (TagName, PublishmentSystemID, TagDescription, TagContent) VALUES (@TagName, @PublishmentSystemID, @TagDescription, @TagContent)";

        private const string SQL_UPDATE_STL_TAG = "UPDATE siteserver_StlTag SET TagDescription = @TagDescription, TagContent = @TagContent WHERE TagName = @TagName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_DELETE_STL_TAG = "DELETE FROM siteserver_StlTag WHERE TagName = @TagName AND PublishmentSystemID = @PublishmentSystemID";

        private const string PARM_TAG_NAME = "@TagName";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_TAG_DESCRIPTION = "@TagDescription";
        private const string PARM_TAG_CONTENT = "@TagContent";

        public void Insert(StlTagInfo info)
        {
            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_TAG_NAME, EDataType.NVarChar, 50, info.TagName),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, info.PublishmentSystemID),
				this.GetParameter(PARM_TAG_DESCRIPTION, EDataType.NVarChar, 255, info.TagDescription),
				this.GetParameter(PARM_TAG_CONTENT, EDataType.NText, info.TagContent)
			};

            this.ExecuteNonQuery(SQL_INSERT_STL_TAG, insertParms);
        }

        public void Update(StlTagInfo info)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_TAG_DESCRIPTION, EDataType.NVarChar, 255, info.TagDescription),
				this.GetParameter(PARM_TAG_CONTENT, EDataType.NText, info.TagContent),
				this.GetParameter(PARM_TAG_NAME, EDataType.NVarChar, 50, info.TagName),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, info.PublishmentSystemID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_STL_TAG, updateParms);
        }

        public void Delete(int publishmentSystemID, string tagName)
        {

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TAG_NAME, EDataType.NVarChar, 50, tagName),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            this.ExecuteNonQuery(SQL_DELETE_STL_TAG, parms);
        }

        public StlTagInfo GetStlTagInfo(int publishmentSystemID, string tagName)
        {
            StlTagInfo info = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TAG_NAME, EDataType.NVarChar, 50, tagName),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_STL_TAG, parms))
            {
                if (rdr.Read())
                {
                    info = new StlTagInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString());
                }
                rdr.Close();
            }

            return info;
        }

        public ArrayList GetStlTagInfoArrayListByPublishmentSystemID(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_STL_TAG, parms))
            {
                while (rdr.Read())
                {
                    StlTagInfo info = new StlTagInfo(rdr.GetValue(0).ToString(), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString());
                    arraylist.Add(info);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetStlTagNameArrayList(int publishmentSystemID)
        {
            ArrayList list = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_STL_TAG_NAMES, parms))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return list;
        }


        public IEnumerable GetDataSource(int publishmentSystemID)
        {
            string sqlString = "SELECT TagName, PublishmentSystemID, TagDescription, TagContent FROM siteserver_StlTag WHERE PublishmentSystemID = 0 ORDER BY TagName";
            if (publishmentSystemID != 0)
            {
                sqlString = string.Format("SELECT TagName, PublishmentSystemID, TagDescription, TagContent FROM siteserver_StlTag WHERE PublishmentSystemID = 0 OR PublishmentSystemID = {0} ORDER BY PublishmentSystemID, TagName", publishmentSystemID);
            }
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }


        public bool IsExactExists(int publishmentSystemID, string tagName)
        {
            bool exists = false;

            string sqlString = string.Format("SELECT TagName FROM siteserver_StlTag WHERE TagName = @TagName AND PublishmentSystemID = @PublishmentSystemID");

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TAG_NAME, EDataType.NVarChar, 50, tagName),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
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

        public bool IsExists(int publishmentSystemID, string tagName)
        {
            bool exists = false;

            string sqlString = string.Format("SELECT TagName FROM siteserver_StlTag WHERE TagName = @TagName AND PublishmentSystemID = 0");
            if (publishmentSystemID != 0)
            {
                sqlString = string.Format("SELECT TagName FROM siteserver_StlTag WHERE TagName = @TagName AND (PublishmentSystemID = 0 OR PublishmentSystemID = @PublishmentSystemID)");
            }
            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TAG_NAME, EDataType.NVarChar, 50, tagName),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
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
    }
}
