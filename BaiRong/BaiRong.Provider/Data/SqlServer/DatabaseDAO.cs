using System;
using System.Data;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;
using System.Xml;
using System.Collections.Generic;

namespace BaiRong.Provider.Data.SqlServer
{
    public class DatabaseDAO : DataProviderBase, IDatabaseDAO
    {
        public virtual void DeleteDBLog()
        {
            string databaseName = SqlUtils.GetDatabaseNameFormConnectionString(this.ADOType, BaiRongDataProvider.ConnectionString);
            //检测数据库版本
            string sqlCheck = "SELECT SERVERPROPERTY('productversion')";
            string versions = this.ExecuteScalar(sqlCheck).ToString();
            //MM.nn.bbbb.rr
            //8 -- 2000
            //9 -- 2005
            //10 -- 2008
            int version = 8;
            string[] arr = versions.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length > 0)
            {
                version = TranslateUtils.ToInt(arr[0], 8);
            }
            if (version < 10)
            {
                //2000,2005
                string sql = string.Format("BACKUP LOG [{0}] WITH NO_LOG", databaseName);
                this.ExecuteNonQuery(sql);
            }
            else
            {
                //2008+
                string sql = string.Format(@"ALTER DATABASE [{0}] SET RECOVERY SIMPLE;DBCC shrinkfile ([{0}_log], 1); ALTER DATABASE [{0}] SET RECOVERY FULL; ", databaseName);
                this.ExecuteNonQuery(sql);
            }
        }

        /*
         * PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(35);

            string sqlString = string.Format(@"select INFO_TITLE, INFORMATIONCODING, PUBLISHEDAGENCIES, INFORMATIONCATEGORY, RELEASEDATE, ISSUED, NEWKEYWORDS, OVERVIEW, CONTENT_PLAIN, CHANNEL_NAME from au_temp where CHANNEL_NAME = '规范性文件' and INFO_TITLE not in (select title from wcm_govpublic where nodeid = 86)");
            ArrayList contentInfoArrayList = new ArrayList();
            using (System.Data.IDataReader rdr = BaiRongDataProvider.DatabaseDAO.GetDataReader(BaiRongDataProvider.ConnectionString, sqlString))
            {
                while (rdr.Read())
                {
                    string INFO_TITLE = rdr.GetValue(0).ToString();
                    string INFORMATIONCODING = rdr.GetValue(1).ToString();
                    string PUBLISHEDAGENCIES = rdr.GetValue(2).ToString();
                    string INFORMATIONCATEGORY = rdr.GetValue(3).ToString();
                    DateTime RELEASEDATE = rdr.GetDateTime(4);
                    string ISSUED = rdr.GetValue(5).ToString();
                    string NEWKEYWORDS = rdr.GetValue(6).ToString();
                    string OVERVIEW = rdr.GetValue(7).ToString();
                    string CONTENT_PLAIN = rdr.GetValue(8).ToString();
                    string CHANNEL_NAME = rdr.GetValue(9).ToString();
                    GovPublicContentInfo contentInfo = new GovPublicContentInfo();

                    contentInfo.NodeID = 86;
                    contentInfo.DepartmentID = 15;
                    contentInfo.PublishmentSystemID = 35;
                    contentInfo.AddDate = contentInfo.PublishDate;
                    contentInfo.IsChecked = false;
                    contentInfo.EffectDate = contentInfo.PublishDate;
                    contentInfo.IsAbolition = false;
                    contentInfo.AbolitionDate = DateTime.Now;

                    contentInfo.Title = INFO_TITLE;
                    contentInfo.PublishDate = RELEASEDATE;
                    contentInfo.Keywords = NEWKEYWORDS;
                    contentInfo.Content = CONTENT_PLAIN;
                    contentInfo.Publisher = PUBLISHEDAGENCIES;

                    contentInfoArrayList.Add(contentInfo);
                }
                rdr.Close();
            }

            foreach (GovPublicContentInfo contentInfo in contentInfoArrayList)
            {
                DataProvider.ContentDAO.Insert(publishmentSystemInfo.AuxiliaryTableForGovPublic, publishmentSystemInfo, contentInfo);
            }
         * */


        //public virtual void DeleteDBLog()
        //{
        //    string filePath = PathUtils.MapPath("~/25173058523.xml");
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(filePath);

        //    XmlNode xn = doc.SelectSingleNode("datas");
        //    foreach (XmlNode data in xn.ChildNodes)
        //    {
        //        XmlElement eData = (XmlElement)data;
        //        string INFO_TITLE = string.Empty;
        //        string INFORMATIONCODING = string.Empty;
        //        string PUBLISHEDAGENCIES = string.Empty;
        //        string INFORMATIONCATEGORY = string.Empty;
        //        string RELEASEDATE = string.Empty;
        //        string ISSUED = string.Empty;
        //        string NEWKEYWORDS = string.Empty;
        //        string OVERVIEW = string.Empty;
        //        string CONTENT_PLAIN = string.Empty;
        //        string CHANNEL_NAME = string.Empty;
        //        foreach (XmlNode node in eData.ChildNodes)
        //        {
        //            if (StringUtils.EqualsIgnoreCase(node.Name, "INFO_TITLE"))
        //            {
        //                INFO_TITLE = node.InnerText;
        //            }
        //            else if (StringUtils.EqualsIgnoreCase(node.Name, "INFORMATIONCODING"))
        //            {
        //                INFORMATIONCODING = node.InnerText;
        //            }
        //            else if (StringUtils.EqualsIgnoreCase(node.Name, "PUBLISHEDAGENCIES"))
        //            {
        //                PUBLISHEDAGENCIES = node.InnerText;
        //            }
        //            else if (StringUtils.EqualsIgnoreCase(node.Name, "INFORMATIONCATEGORY"))
        //            {
        //                INFORMATIONCATEGORY = node.InnerText;
        //            }
        //            else if (StringUtils.EqualsIgnoreCase(node.Name, "RELEASEDATE"))
        //            {
        //                RELEASEDATE = node.InnerText;
        //            }
        //            else if (StringUtils.EqualsIgnoreCase(node.Name, "ISSUED"))
        //            {
        //                ISSUED = node.InnerText;
        //            }
        //            else if (StringUtils.EqualsIgnoreCase(node.Name, "NEWKEYWORDS"))
        //            {
        //                NEWKEYWORDS = node.InnerText;
        //            }
        //            else if (StringUtils.EqualsIgnoreCase(node.Name, "OVERVIEW"))
        //            {
        //                OVERVIEW = node.InnerText;
        //            }
        //            else if (StringUtils.EqualsIgnoreCase(node.Name, "CONTENT_PLAIN"))
        //            {
        //                CONTENT_PLAIN = node.InnerText;
        //            }
        //            else if (StringUtils.EqualsIgnoreCase(node.Name, "CHANNEL_NAME"))
        //            {
        //                CHANNEL_NAME = node.InnerText;
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(INFO_TITLE) && !string.IsNullOrEmpty(RELEASEDATE))
        //        {
        //            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        //            long lTime = long.Parse(TranslateUtils.ToLong(RELEASEDATE) + "0000");
        //            TimeSpan toNow = new TimeSpan(lTime);
        //            DateTime releaseDate = dtStart.Add(toNow);

        //            string sql = string.Format("INSERT INTO au_temp (INFO_TITLE, INFORMATIONCODING, PUBLISHEDAGENCIES, INFORMATIONCATEGORY, RELEASEDATE, ISSUED, NEWKEYWORDS, OVERVIEW, CONTENT_PLAIN, CHANNEL_NAME) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')", INFO_TITLE, INFORMATIONCODING, PUBLISHEDAGENCIES, INFORMATIONCATEGORY, releaseDate.ToString(), ISSUED, NEWKEYWORDS, OVERVIEW, CONTENT_PLAIN, CHANNEL_NAME);
        //            this.ExecuteNonQuery(sql);
        //        }
        //    }
        //}


        //public virtual void DeleteDBLog()
        //{
        //    string filePath = PathUtils.MapPath("~/2514524577.xml");
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(filePath);

        //    XmlNode xn=doc.SelectSingleNode("datas");
        //    foreach (XmlNode data in xn.ChildNodes)
        //    {
        //        XmlElement eData = (XmlElement)data;
        //        string title = string.Empty;
        //        long releaseDate = 0;
        //        foreach (XmlNode node in eData.ChildNodes)
        //        {
        //            if (StringUtils.EqualsIgnoreCase(node.Name, "INFO_TITLE"))
        //            {
        //                title = node.InnerText;
        //            }
        //            else if (StringUtils.EqualsIgnoreCase(node.Name, "RELEASEDATE"))
        //            {
        //                releaseDate = TranslateUtils.ToLong(node.InnerText);
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(title) && releaseDate > 0)
        //        {
        //            //DateTime publishDate = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddSeconds(releaseDate);

        //            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        //            long lTime = long.Parse(releaseDate + "0000");
        //            TimeSpan toNow = new TimeSpan(lTime);
        //            DateTime publishDate = dtStart.Add(toNow);

        //            string sql = string.Format("INSERT INTO au_temp (title, publishdate) VALUES('{0}', '{1}')", title, publishDate.ToString());
        //            this.ExecuteNonQuery(sql);
        //        }
        //    }
        //}

        //public virtual void DeleteDBLog()
        //{
        //    string sqlString = "select id, content_bak from au_wcm_content";
        //    Hashtable updateHashtable = new Hashtable();

        //    using (IDataReader rdr = this.ExecuteReader(sqlString))
        //    {
        //        while (rdr.Read())
        //        {
        //            int id = rdr.GetInt32(0);
        //            string content_bak = rdr.GetValue(1).ToString();
        //            if (!string.IsNullOrEmpty(content_bak) && content_bak.Length >= 4000)
        //            {
        //                updateHashtable[id] = content_bak.Replace("[SITESERVER_PAGE]", string.Empty);
        //            }
        //        }
        //        rdr.Close();
        //    }

        //    string updateString = "UPDATE au_wcm_content set [Content]=@Content WHERE [ID]=@ID";

        //    foreach (int contentID in updateHashtable.Keys)
        //    {
        //        string content = (string)updateHashtable[contentID];

        //        IDbDataParameter[] parms = new IDbDataParameter[]
        //        {
        //            this.GetParameter("@Content", EDataType.NText, content),
        //            this.GetParameter("@ID", EDataType.Integer, contentID)
        //        };
        //        this.ExecuteNonQuery(updateString, parms);
        //    }
        //}


        public void ExecuteSql(string sqlString)
        {
            this.ExecuteSql(null, sqlString);
        }

        public void ExecuteSql(string connectionString, string sqlString)
        {
            if (!string.IsNullOrEmpty(sqlString))
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = base.ConnectionString;
                }
                using (IDbConnection conn = this.GetConnection(connectionString))
                {
                    conn.Open();
                    try
                    {
                        this.ExecuteNonQuery(conn, sqlString);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public void ExecuteSql(ArrayList sqlArrayList)
        {
            this.ExecuteSql(null, sqlArrayList);
        }

        public void ExecuteSql(string connectionString, ArrayList sqlArrayList)
        {
            if (sqlArrayList != null && sqlArrayList.Count > 0)
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = base.ConnectionString;
                }

                using (IDbConnection conn = this.GetConnection(connectionString))
                {
                    conn.Open();
                    using (IDbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (string sql in sqlArrayList)
                            {
                                this.ExecuteNonQuery(trans, sql);
                            }

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        public void ExecuteSqlInFile(string pathToScriptFile, StringBuilder errorBuilder)
        {
            IDbConnection connection;
            StreamReader _reader = null;

            string sqlString = "";

            if (false == System.IO.File.Exists(pathToScriptFile))
            {
                throw new Exception("File " + pathToScriptFile + " does not exists");
            }
            using (Stream stream = System.IO.File.OpenRead(pathToScriptFile))
            {
                _reader = new StreamReader(stream, System.Text.Encoding.Default);

                connection = base.GetConnection();

                IDbCommand command = SqlUtils.GetIDbCommand(this.ADOType);

                connection.Open();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;

                while (null != (sqlString = SqlUtils.ReadNextSqlString(_reader)))
                {
                    command.CommandText = sqlString;
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        errorBuilder.AppendFormat(@"
                    sql:{0}
                    message:{1}
                    ", sqlString, ex.Message);
                    }
                }

                _reader.Close();
            }
            connection.Close();
        }

        public void ExecuteSqlInFile(string pathToScriptFile, string tableName, StringBuilder errorBuilder)
        {
            IDbConnection connection;
            StreamReader _reader = null;

            string sqlString = "";

            if (false == System.IO.File.Exists(pathToScriptFile))
            {
                throw new Exception("File " + pathToScriptFile + " does not exists");
            }
            using (Stream stream = System.IO.File.OpenRead(pathToScriptFile))
            {
                _reader = new StreamReader(stream, System.Text.Encoding.Default);

                connection = base.GetConnection();

                IDbCommand command = SqlUtils.GetIDbCommand(this.ADOType);

                connection.Open();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;

                while (null != (sqlString = SqlUtils.ReadNextSqlString(_reader)))
                {
                    sqlString = string.Format(sqlString, tableName);
                    command.CommandText = sqlString;
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        errorBuilder.AppendFormat(@"
                    sql:{0}
                    message:{1}
                    ", sqlString, ex.Message);
                    }
                }

                _reader.Close();
            }
            connection.Close();
        }

        public int GetIntResult(string connectionString, string sqlString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            int count = 0;

            using (IDbConnection conn = this.GetConnection(connectionString))
            {
                conn.Open();
                using (IDataReader rdr = this.ExecuteReader(conn, sqlString))
                {
                    if (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            count = Convert.ToInt32(rdr[0]);
                        }
                    }
                    rdr.Close();
                }
            }
            return count;
        }

        public int GetIntResult(string sqlString)
        {
            int count = 0;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDataReader rdr = this.ExecuteReader(conn, sqlString))
                {
                    if (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            count = Convert.ToInt32(rdr[0]);
                        }
                    }
                    rdr.Close();
                }
            }
            return count;
        }

        public int GetIntResult(string sqlString, IDataParameter[] parms)
        {
            int count = 0;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDataReader rdr = this.ExecuteReader(conn, sqlString, parms))
                {
                    if (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            count = Convert.ToInt32(rdr[0]);
                        }
                    }
                    rdr.Close();
                }
            }
            return count;
        }

        public ArrayList GetIntArrayList(string sqlString)
        {
            ArrayList arraylist = new ArrayList();

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDataReader rdr = this.ExecuteReader(conn, sqlString))
                {
                    while (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            arraylist.Add(Convert.ToInt32(rdr[0]));
                        }
                    }
                    rdr.Close();
                }
            }
            return arraylist;
        }

        public ArrayList GetIntArrayList(string connectionString, string sqlString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            ArrayList arraylist = new ArrayList();

            using (IDbConnection conn = this.GetConnection(connectionString))
            {
                conn.Open();
                using (IDataReader rdr = this.ExecuteReader(conn, sqlString))
                {
                    while (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            arraylist.Add(Convert.ToInt32(rdr[0]));
                        }
                    }
                    rdr.Close();
                }
            }
            return arraylist;
        }

        public List<int> GetIntList(string sqlString)
        {
            List<int> list = new List<int>();

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDataReader rdr = this.ExecuteReader(conn, sqlString))
                {
                    while (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            list.Add(Convert.ToInt32(rdr[0]));
                        }
                    }
                    rdr.Close();
                }
            }
            return list;
        }

        public List<int> GetIntList(string connectionString, string sqlString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            List<int> list = new List<int>();

            using (IDbConnection conn = this.GetConnection(connectionString))
            {
                conn.Open();
                using (IDataReader rdr = this.ExecuteReader(conn, sqlString))
                {
                    while (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            list.Add(Convert.ToInt32(rdr[0]));
                        }
                    }
                    rdr.Close();
                }
            }
            return list;
        }

        public string GetString(string connectionString, string sqlString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            string retval = string.Empty;

            using (IDbConnection conn = this.GetConnection(connectionString))
            {
                conn.Open();
                using (IDataReader rdr = this.ExecuteReader(conn, sqlString))
                {
                    if (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            retval = Convert.ToString(rdr[0]);
                        }
                    }
                    rdr.Close();
                }
            }
            return retval;
        }

        public int GetSequence(IDbTransaction trans, string tableName)
        {
            int id = 0;

            string sqlString = string.Empty;

            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = string.Format("select {0}_SEQ.CURRVAL from dual", tableName);
            }
            else
            {
                sqlString = "SELECT @@IDENTITY AS 'ID'";
            }

            using (IDataReader rdr = this.ExecuteReader(trans, sqlString))
            {
                if (rdr.Read())
                {
                    id = Convert.ToInt32(rdr[0].ToString());
                }
                else
                {
                    trans.Rollback();
                }
                rdr.Close();
            }

            return id;
        }

        public string GetString(string sqlString)
        {
            string value = string.Empty;
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        value = Convert.ToString(rdr.GetValue(0));
                    }
                }
                rdr.Close();
            }

            return value;
        }

        public ArrayList GetStringArrayList(string sqlString)
        {
            ArrayList arraylist = new ArrayList();

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDataReader rdr = this.ExecuteReader(conn, sqlString))
                {
                    while (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            arraylist.Add(Convert.ToString(rdr[0]));
                        }
                    }
                    rdr.Close();
                }
            }
            return arraylist;
        }

        public List<string> GetStringList(string sqlString)
        {
            List<string> list = new List<string>();

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDataReader rdr = this.ExecuteReader(conn, sqlString))
                {
                    while (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            list.Add(Convert.ToString(rdr[0]));
                        }
                    }
                    rdr.Close();
                }
            }
            return list;
        }

        public ArrayList GetStringArrayList(string connectionString, string sqlString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            ArrayList arraylist = new ArrayList();

            using (IDbConnection conn = this.GetConnection(connectionString))
            {
                conn.Open();
                using (IDataReader rdr = this.ExecuteReader(conn, sqlString))
                {
                    while (rdr.Read())
                    {
                        if (!rdr.IsDBNull(0))
                        {
                            arraylist.Add(Convert.ToString(rdr[0]));
                        }
                    }
                    rdr.Close();
                }
            }
            return arraylist;
        }

        public DateTime GetDateTime(string sqlString)
        {
            DateTime datetime = DateTime.MinValue;
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        datetime = rdr.GetDateTime(0);
                    }
                }
                rdr.Close();
            }

            return datetime;
        }

        public DateTime GetDateTime(string sqlString, IDataParameter[] parms)
        {
            DateTime datetime = DateTime.MinValue;
            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        datetime = rdr.GetDateTime(0);
                    }
                }
                rdr.Close();
            }

            return datetime;
        }

        public DataSet GetDataSetByWhereString(string tableENName, string whereString)
        {
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableENName, SqlUtils.Asterisk, whereString);

            DataSet dataset = this.ExecuteDataset(SQL_SELECT);
            return dataset;
        }

        public IDataReader GetDataReader(string connectionString, string sqlString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            if (string.IsNullOrEmpty(sqlString)) return null;
            return this.ExecuteReader(connectionString, sqlString);
        }

        public IEnumerable GetDataSource(string sqlString)
        {
            if (string.IsNullOrEmpty(sqlString)) return null;
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }

        public IEnumerable GetDataSource(string connectionString, string sqlString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            if (string.IsNullOrEmpty(sqlString)) return null;
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(connectionString, sqlString);
            return enumerable;
        }

        public DataSet GetDataSet(string connectionString, string sqlString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = base.ConnectionString;
            }

            if (string.IsNullOrEmpty(sqlString)) return null;
            DataSet dataset = this.ExecuteDataset(connectionString, sqlString);
            return dataset;
        }

        public DataSet GetDataSet(string sqlString)
        {
            if (string.IsNullOrEmpty(sqlString)) return null;
            DataSet dataset = this.ExecuteDataset(sqlString);
            return dataset;
        }

        public void ReadResultsToExtendedAttributes(IDataRecord rdr, ExtendedAttributes attributes)
        {
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                string columnName = rdr.GetName(i);
                attributes.SetExtendedAttribute(columnName, Convert.ToString(rdr.GetValue(i)));
            }
        }

        public void ReadResultsToNameValueCollection(IDataRecord rdr, NameValueCollection attributes)
        {
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                string columnName = rdr.GetName(i);
                attributes.Set(columnName, Convert.ToString(rdr.GetValue(i)));
            }
        }

        //public void OldVersionTrans(ArrayList tableNameArrayList)
        //{
        //    try
        //    {
        //        this.ExecuteNonQuery("update siteserver_node set content = REPLACE(REPLACE(REPLACE(CAST(content AS NVARCHAR(4000)), '&quot;', '\"'), '&lt;', '<'), '&gt;', '>')");
        //    }
        //    catch (Exception ex)
        //    {
        //        TraceUtils.Warn(ex.Message);
        //    }

        //    foreach (string tableName in tableNameArrayList)
        //    {
        //        try
        //        {
        //            string sqlString = string.Format("select [id],[content] from [{0}]", tableName);
        //            SortedList sortedlist = new SortedList();
        //            using (IDataReader rdr = this.ExecuteReader(sqlString))
        //            {
        //                while (rdr.Read())
        //                {
        //                    int id = rdr.GetInt32(0);
        //                    string content = rdr.GetValue(1).ToString();
        //                    if (!string.IsNullOrEmpty(content))
        //                    {
        //                        sortedlist.Add(id, content.Replace("&quot;", "\"").Replace("&lt;", "<").Replace("&gt;", ">").Replace("''", "'"));
        //                    }
        //                }
        //                rdr.Close();
        //            }

        //            foreach (int id in sortedlist.Keys)
        //            {
        //                string updateString = string.Empty;
        //                try
        //                {
        //                    string content = (string)sortedlist[id];
        //                    content = content.Replace("'", "''");
        //                    updateString = string.Format("update [{0}] set [content] = '{1}' where [id] = {2}", tableName, content, id);
        //                    this.ExecuteNonQuery(updateString);
        //                }
        //                catch (Exception ex)
        //                {
        //                    TraceUtils.Warn(ex.Message);
        //                    TraceUtils.Write(updateString);
        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            TraceUtils.Warn(ex.Message);
        //        }
        //    }
        //}

        public int GetPageTotalCount(string sqlString)
        {
            string cmdText;

            string temp = sqlString.ToLower();
            int pos = temp.LastIndexOf("order by");
            if (pos > -1)
                sqlString = sqlString.Substring(0, pos);




            // Add new ORDER BY info if SortKeyField is specified
            //if (!string.IsNullOrEmpty(sortField) && addCustomSortInfo)
            //    SelectCommand += " ORDER BY " + SortField;

            if (BaiRongDataProvider.DatabaseType != EDatabaseType.Oracle)
            {
                cmdText = string.Format("SELECT COUNT(*) FROM ({0}) AS t0", sqlString);
            }
            else
            {
                cmdText = string.Format("SELECT COUNT(1) FROM ({0})", sqlString);
            }
            return GetIntResult(cmdText);
        }

        public string GetPageSqlString(string sqlString, string orderByString, int recordCount, int itemsPerPage, int currentPageIndex)
        {
            string temp = sqlString.ToLower();
            int pos = temp.LastIndexOf("order by");
            if (pos > -1)
                sqlString = sqlString.Substring(0, pos);

            int recordsInLastPage = itemsPerPage;

            // Calculate the correspondent number of pages
            int lastPage = recordCount / itemsPerPage;
            int remainder = recordCount % itemsPerPage;
            if (remainder > 0)
                lastPage++;
            int pageCount = lastPage;

            if (remainder > 0)
                recordsInLastPage = remainder;

            int recsToRetrieve = itemsPerPage;
            if (currentPageIndex == pageCount - 1)
                recsToRetrieve = recordsInLastPage;

            string orderByString2 = orderByString.Replace(" DESC", " DESC2");
            orderByString2 = orderByString2.Replace(" ASC", " DESC");
            orderByString2 = orderByString2.Replace(" DESC2", " ASC");

            if (BaiRongDataProvider.DatabaseType != EDatabaseType.Oracle)
            {
                return string.Format(@"
SELECT * FROM 
(SELECT TOP {0} * FROM 
(SELECT TOP {1} * FROM ({2}) AS t0 {3}) AS t1 
{4}) AS t2 
{3}",
                recsToRetrieve,                        // {0} --> page size
                itemsPerPage * (currentPageIndex + 1),    // {1} --> size * index
                sqlString,                        // {2} --> base query
                orderByString,                            // {3} --> key field in the query
                orderByString2);
            }
            else
            {
                return string.Format(@"
SELECT * FROM 
(
    SELECT * FROM (
        {2} {3}
    ) WHERE ROWNUM <= {1} {4}
) WHERE ROWNUM <= {0} {3}",
                recsToRetrieve,                        // {0} --> page size
                itemsPerPage * (currentPageIndex + 1),    // {1} --> size * index
                sqlString,                        // {2} --> base query
                orderByString,                            // {3} --> key field in the query
                orderByString2);
            }
        }

        #region 辅助表数据操作

        public int DataInsert(NameValueCollection attributes, string tableName)
        {
            int id = 0;

            if (!string.IsNullOrEmpty(tableName))
            {
                IDbDataParameter[] parms = null;
                string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(attributes, tableName, out parms);

                using (IDbConnection conn = this.GetConnection())
                {
                    conn.Open();
                    using (IDbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                            id = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, tableName);

                            //using (IDataReader rdr = this.ExecuteReader(trans, "SELECT @@IDENTITY AS 'ID'"))
                            //{
                            //    rdr.Read();
                            //    id = Convert.ToInt32(rdr[0].ToString());
                            //    rdr.Close();
                            //}

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
            return id;
        }

        public void DataUpdate(NameValueCollection attributes, string tableName)
        {
            if (!string.IsNullOrEmpty(tableName))
            {
                IDbDataParameter[] parms = null;
                string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(attributes, tableName, out parms);

                this.ExecuteNonQuery(SQL_UPDATE, parms);
            }
        }

        public bool DataIsExists(string tableName, NameValueCollection whereMap)
        {
            bool exists = false;

            if (whereMap != null && whereMap.Count > 0)
            {
                StringBuilder whereBuilder = new StringBuilder("WHERE ");
                foreach (string whereKey in whereMap.Keys)
                {
                    whereBuilder.AppendFormat("[{0}] = {1} ", whereKey, whereMap[whereKey]);
                }

                string SQL_SELECT = string.Format("SELECT id FROM [{0}] {1}", tableName, whereBuilder.ToString());

                try
                {
                    using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                    {
                        if (rdr.Read())
                        {
                            if (!rdr.IsDBNull(0))
                            {
                                exists = true;
                            }
                        }
                        rdr.Close();
                    }
                }
                catch { }
            }
            return exists;
        }

        public NameValueCollection DataGetAttributes(string tableName, NameValueCollection whereMap)
        {
            NameValueCollection attributes = null;
            if (whereMap != null && whereMap.Count > 0)
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    StringBuilder whereBuilder = new StringBuilder("WHERE ");
                    foreach (string whereKey in whereMap.Keys)
                    {
                        whereBuilder.AppendFormat("[{0}] = {1} ", whereKey, whereMap[whereKey]);
                    }
                    string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, SqlUtils.Asterisk, whereBuilder.ToString());

                    using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
                    {
                        if (rdr.Read())
                        {
                            attributes = new NameValueCollection();
                            ReadResultsToNameValueCollection(rdr, attributes);
                        }
                        rdr.Close();
                    }
                }
            }
            return attributes;
        }

        public void DataDelete(string tableName, NameValueCollection whereMap)
        {
            if (whereMap != null && whereMap.Count > 0)
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    StringBuilder whereBuilder = new StringBuilder("WHERE ");
                    foreach (string whereKey in whereMap.Keys)
                    {
                        whereBuilder.AppendFormat("[{0}] = {1} ", whereKey, whereMap[whereKey]);
                    }
                    string SQL_DELETE = string.Format("DELETE FROM [{0}] {1}", tableName, whereBuilder.ToString());

                    this.ExecuteNonQuery(SQL_DELETE);
                }
            }
        }

        #endregion

        public void ClearDatabase(string connectionString)
        {
            this.ExecuteSql(connectionString, @"
DECLARE c1 cursor for  
select 'alter table ['+ object_name(parent_obj) + '] drop constraint ['+name+']; '  
from sysobjects  
where xtype = 'F'  
open c1  
declare @c1 varchar(8000)  
fetch next from c1 into @c1  
while(@@fetch_status=0)  
begin  
exec(@c1)  
fetch next from c1 into @c1  
end
close c1  
deallocate c1  
");
            this.ExecuteSql(connectionString, @"
declare @sql varchar(8000)  
while (select count(*) from sysobjects where type='U')>0  
begin  
SELECT @sql='drop table ' + name  
FROM sysobjects  
WHERE (type = 'U')  
ORDER BY 'drop table ' + name  
exec(@sql)   
end
");
        }
    }
}
