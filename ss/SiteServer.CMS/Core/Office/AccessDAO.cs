using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Configuration;
using System.Collections;
using SiteServer.CMS.Model;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using System.Text;
using BaiRong.Core.Data;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using System;

namespace SiteServer.CMS.Core.Office
{
    public class AccessDAO
    {
        private string connectionString;
        private OleDbConnection connection;

        public AccessDAO(string filePath)
        {
            this.connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};User Id=;Password=;", filePath);
            this.connection = new OleDbConnection(connectionString);
        }

        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }

        public string GetCreateTableSqlString(string nodeName, ArrayList tableStyleInfoArrayList, ArrayList displayAttributes)
        {
            StringBuilder createBuilder = new StringBuilder();
            createBuilder.AppendFormat("CREATE TABLE {0} ( ", nodeName);

            foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
            {
                if (displayAttributes.Contains(tableStyleInfo.AttributeName))
                {
                    createBuilder.AppendFormat(" [{0}] memo, ", tableStyleInfo.DisplayName);
                }
            }

            createBuilder.Length = createBuilder.Length - 2;
            createBuilder.Append(" )");

            return createBuilder.ToString();
        }

        public ArrayList GetInsertSqlStringArrayList(string nodeName, int publishmentSystemID, int nodeID, ETableStyle tableStyle, string tableName, ArrayList tableStyleInfoArrayList, ArrayList displayAttributes, ArrayList contentIDArrayList, bool isPeriods, string dateFrom, string dateTo, ETriState checkedState, out bool isExport)
        {
            ArrayList insertSqlArrayList = new ArrayList();

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.AppendFormat("INSERT INTO {0} (", nodeName);

            foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
            {
                if (displayAttributes.Contains(tableStyleInfo.AttributeName))
                {
                    preInsertBuilder.AppendFormat("[{0}], ", tableStyleInfo.DisplayName);
                }
            }

            preInsertBuilder.Length = preInsertBuilder.Length - 2;
            preInsertBuilder.Append(") VALUES (");

            if (contentIDArrayList == null || contentIDArrayList.Count == 0)
            {
                contentIDArrayList = BaiRongDataProvider.ContentDAO.GetContentIDArrayList(tableName, nodeID, isPeriods, dateFrom, dateTo, checkedState);
            }

            isExport = contentIDArrayList.Count > 0;

            foreach (int contentID in contentIDArrayList)
            {
                ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                if (contentInfo != null)
                {
                    StringBuilder insertBuilder = new StringBuilder();
                    insertBuilder.Append(preInsertBuilder.ToString());

                    foreach (TableStyleInfo tableStyleInfo in tableStyleInfoArrayList)
                    {
                        if (displayAttributes.Contains(tableStyleInfo.AttributeName))
                        {
                            string value = contentInfo.GetExtendedAttribute(tableStyleInfo.AttributeName);
                            insertBuilder.AppendFormat("'{0}', ", SqlUtils.ToSqlString(StringUtils.StripTags(value)));
                        }
                    }

                    insertBuilder.Length = insertBuilder.Length - 2;
                    insertBuilder.Append(") ");

                    insertSqlArrayList.Add(insertBuilder.ToString());
                }
            }
            return insertSqlArrayList;
        }

        public bool ExecuteSqlString(string sqlString)
        {
            bool resultState = false;
            OleDbTransaction myTrans = null;

            try
            {
                this.connection.Open();
                myTrans = this.connection.BeginTransaction();
                OleDbCommand command = new OleDbCommand(sqlString, this.connection, myTrans);
                command.ExecuteNonQuery();
                myTrans.Commit();
                resultState = true;
            }
            catch
            {
                if (myTrans != null)
                {
                    myTrans.Rollback();
                }
                resultState = false;
            }
            finally
            {
                this.connection.Close();
            }

            return resultState;
        }

        private OleDbDataReader ReturnDataReader(string strSQL)
        {
            OleDbDataReader dataReader = null;
            try
            {
                this.connection.Open();
                OleDbCommand command = new OleDbCommand(strSQL, this.connection);
                dataReader = command.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.connection.Close();
            }

            return dataReader;

        }

        public DataSet ReturnDataSet(string strSQL)
        {
            DataSet dataSet = new DataSet();
            try
            {
                this.connection.Open();
                OleDbDataAdapter OleDbDA = new OleDbDataAdapter(strSQL, this.connection);
                OleDbDA.Fill(dataSet, "objDataSet");                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.connection.Close();
            }
            return dataSet;
        }

        public string[] GetTableNames()
        {
            try
            {
                this.connection.Open();
                DataTable shemaTable = this.connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                int n = shemaTable.Rows.Count;
                string[] strTable = new string[n];
                int m = shemaTable.Columns.IndexOf("TABLE_NAME");
                for (int i = 0; i < n; i++)
                {
                    DataRow m_DataRow = shemaTable.Rows[i];
                    strTable[i] = m_DataRow.ItemArray.GetValue(m).ToString();
                }
                return strTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.connection.Close();
            }
        }
    }

}
