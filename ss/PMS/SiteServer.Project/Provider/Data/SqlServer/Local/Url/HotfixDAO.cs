using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core.Data.Provider;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using BaiRong.Core.Data;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.Project.Provider.Data.SqlServer
{
    public class HotfixDAO : DataProviderBase, IHotfixDAO
	{
        protected override string ConnectionString
        {
            get
            {
                return ConfigurationManager.OuterConnectionString;
            }
        }

        private const string SQL_INSERT = "INSERT INTO brs_Hotfix (Version, IsBeta, FileUrl, PageUrl, PubDate, Message, IsEnabled, IsRestrict, RestrictDomain, RestrictProductIDCollection, RestrictDatabase, RestrictVersion, RestrictIsBeta, DownloadCount) VALUES (@Version, @IsBeta, @FileUrl, @PageUrl, @PubDate, @Message, @IsEnabled, @IsRestrict, @RestrictDomain, @RestrictProductIDCollection, @RestrictDatabase, @RestrictVersion, @RestrictIsBeta, @DownloadCount)";

        private const string SQL_UPDATE = "UPDATE brs_Hotfix SET Version = @Version, IsBeta = @IsBeta, FileUrl = @FileUrl, PageUrl = @PageUrl, PubDate = @PubDate, Message = @Message, IsEnabled = @IsEnabled, IsRestrict = @IsRestrict, RestrictDomain = @RestrictDomain, RestrictProductIDCollection = @RestrictProductIDCollection, RestrictDatabase = @RestrictDatabase, RestrictVersion = @RestrictVersion, RestrictIsBeta = @RestrictIsBeta, DownloadCount = @DownloadCount WHERE ID = @ID";

        private const string SQL_DELETE = "DELETE FROM brs_Hotfix WHERE ID = @ID";

        private const string SQL_SELECT_ALL = "SELECT ID, Version, IsBeta, FileUrl, PageUrl, PubDate, Message, IsEnabled, IsRestrict, RestrictDomain, RestrictProductIDCollection, RestrictDatabase, RestrictVersion, RestrictIsBeta, DownloadCount FROM brs_Hotfix";

        public void Insert(HotfixInfo hotfixInfo)
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@Version", EDataType.VarChar, 50, hotfixInfo.Version),
                this.GetParameter("@IsBeta", EDataType.VarChar, 18, hotfixInfo.IsBeta.ToString()),
                this.GetParameter("@FileUrl", EDataType.VarChar, 200, hotfixInfo.FileUrl),
				this.GetParameter("@PageUrl", EDataType.VarChar, 200, hotfixInfo.PageUrl),
                this.GetParameter("@PubDate", EDataType.DateTime, hotfixInfo.PubDate),
                this.GetParameter("@Message", EDataType.NVarChar, 255, hotfixInfo.Message),
                this.GetParameter("@IsEnabled", EDataType.VarChar, 18, hotfixInfo.IsEnabled.ToString()),
                this.GetParameter("@IsRestrict", EDataType.VarChar, 18, hotfixInfo.IsRestrict.ToString()),
                this.GetParameter("@RestrictDomain", EDataType.VarChar, 200, hotfixInfo.RestrictDomain.ToLower()),
                this.GetParameter("@RestrictProductIDCollection", EDataType.VarChar, 255, hotfixInfo.RestrictProductIDCollection.ToLower()),
                this.GetParameter("@RestrictDatabase", EDataType.VarChar, 50, hotfixInfo.RestrictDatabase.ToLower()),
                this.GetParameter("@RestrictVersion", EDataType.VarChar, 50, hotfixInfo.RestrictVersion),
                this.GetParameter("@RestrictIsBeta", EDataType.VarChar, 18, hotfixInfo.RestrictIsBeta.ToString()),
                this.GetParameter("@DownloadCount", EDataType.Integer, hotfixInfo.DownloadCount)
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);
		}

        public void Update(HotfixInfo hotfixInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@Version", EDataType.VarChar, 50, hotfixInfo.Version),
                this.GetParameter("@IsBeta", EDataType.VarChar, 18, hotfixInfo.IsBeta.ToString()),
                this.GetParameter("@FileUrl", EDataType.VarChar, 200, hotfixInfo.FileUrl),
				this.GetParameter("@PageUrl", EDataType.VarChar, 200, hotfixInfo.PageUrl),
                this.GetParameter("@PubDate", EDataType.DateTime, hotfixInfo.PubDate),
                this.GetParameter("@Message", EDataType.NVarChar, 255, hotfixInfo.Message),
                this.GetParameter("@IsEnabled", EDataType.VarChar, 18, hotfixInfo.IsEnabled.ToString()),
                this.GetParameter("@IsRestrict", EDataType.VarChar, 18, hotfixInfo.IsRestrict.ToString()),
                this.GetParameter("@RestrictDomain", EDataType.VarChar, 200, hotfixInfo.RestrictDomain.ToLower()),
                this.GetParameter("@RestrictProductIDCollection", EDataType.VarChar, 255, hotfixInfo.RestrictProductIDCollection.ToLower()),
                this.GetParameter("@RestrictDatabase", EDataType.VarChar, 50, hotfixInfo.RestrictDatabase.ToLower()),
                this.GetParameter("@RestrictVersion", EDataType.VarChar, 50, hotfixInfo.RestrictVersion),
                this.GetParameter("@RestrictIsBeta", EDataType.VarChar, 18, hotfixInfo.RestrictIsBeta.ToString()),
                this.GetParameter("@DownloadCount", EDataType.Integer, hotfixInfo.DownloadCount),
                this.GetParameter("@ID", EDataType.Integer, hotfixInfo.ID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
        }

        public void Delete(int id)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter("@ID", EDataType.Integer, id),
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
        }

        public HotfixInfo GetHotfixInfo(int id)
		{
            HotfixInfo hotfixInfo = null;

            string sqlString = "SELECT ID, Version, IsBeta, FileUrl, PageUrl, PubDate, Message, IsEnabled, IsRestrict, RestrictDomain, RestrictProductIDCollection, RestrictDatabase, RestrictVersion, RestrictIsBeta, DownloadCount FROM brs_Hotfix WHERE ID = " + id;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    hotfixInfo = new HotfixInfo(rdr.GetInt32(0), rdr.GetString(1), TranslateUtils.ToBool(rdr.GetString(2)), rdr.GetString(3), rdr.GetString(4), rdr.GetDateTime(5), rdr.GetString(6), TranslateUtils.ToBool(rdr.GetString(7)), TranslateUtils.ToBool(rdr.GetString(8)), rdr.GetString(9), rdr.GetString(10), rdr.GetString(11), rdr.GetString(12), TranslateUtils.ToBool(rdr.GetString(13)), rdr.GetInt32(14));
                }
                rdr.Close();
            }

            return hotfixInfo;
		}

        public string GetFileUrl(int id)
        {
            string fileUrl = string.Empty;

            string sqlString = "SELECT FileUrl FROM brs_Hotfix WHERE ID = " + id;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    fileUrl = rdr.GetString(0);
                }
                rdr.Close();
            }

            return fileUrl;
        }

        public void AddDownloadCount(int id)
        {
            string sqlString = "UPDATE brs_Hotfix SET DownloadCount = DownloadCount + 1 WHERE ID = " + id;
            this.ExecuteNonQuery(sqlString);
        }

        public ArrayList GetHotfixInfoArrayListEnabled()
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = "SELECT ID, Version, IsBeta, FileUrl, PageUrl, PubDate, Message, IsEnabled, IsRestrict, RestrictDomain, RestrictProductIDCollection, RestrictDatabase, RestrictVersion, RestrictIsBeta, DownloadCount FROM brs_Hotfix WHERE IsEnabled = 'True' ORDER BY ID DESC";

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    HotfixInfo hotfixInfo = new HotfixInfo(rdr.GetInt32(0), rdr.GetString(1), TranslateUtils.ToBool(rdr.GetString(2)), rdr.GetString(3), rdr.GetString(4), rdr.GetDateTime(5), rdr.GetString(6), TranslateUtils.ToBool(rdr.GetString(7)), TranslateUtils.ToBool(rdr.GetString(8)), rdr.GetString(9), rdr.GetString(10), rdr.GetString(11), rdr.GetString(12), TranslateUtils.ToBool(rdr.GetString(13)), rdr.GetInt32(14));
                    arraylist.Add(hotfixInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public DictionaryEntryArrayList GetHotfixInfoDictionaryEntryArrayListEnabled()
        {
            DictionaryEntryArrayList dictionary = new DictionaryEntryArrayList();

            ArrayList hotfixInfoArrayList = this.GetHotfixInfoArrayListEnabled();
            foreach (HotfixInfo hotfixInfo in hotfixInfoArrayList)
            {
                DictionaryEntry entry = new DictionaryEntry(hotfixInfo.ID, hotfixInfo);
                dictionary.Add(entry);
            }

            return dictionary;
        }

        public string GetSelectSqlString()
        {
            return SQL_SELECT_ALL;
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
