using System;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class InputDAO : DataProviderBase, IInputDAO
    {
        private const string SQL_UPDATE_INPUT = "UPDATE siteserver_Input SET InputName = @InputName, IsChecked = @IsChecked, IsReply = @IsReply, IsTemplate = @IsTemplate, StyleTemplate = @StyleTemplate, ScriptTemplate = @ScriptTemplate, ContentTemplate = @ContentTemplate, SettingsXML = @SettingsXML WHERE InputID = @InputID";

        private const string SQL_DELETE_INPUT = "DELETE FROM siteserver_Input WHERE InputID = @InputID";

        private const string SQL_SELECT_INPUT_BY_WHERE = "SELECT InputID, InputName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SettingsXML,ClassifyID FROM siteserver_Input WHERE InputName = @InputName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_INPUT_ID_BY_WHERE = "SELECT InputID FROM siteserver_Input WHERE InputName = @InputName AND PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_INPUT_BY_INPUT_ID = "SELECT InputID, InputName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SettingsXML,ClassifyID FROM siteserver_Input WHERE InputID = @InputID";

        private const string SQL_SELECT_INPUT_BY_ADDDATE = "SELECT TOP 1 InputID, InputName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SettingsXML,ClassifyID FROM siteserver_Input WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis DESC, AddDate DESC";

        private const string SQL_SELECT_ALL_INPUT = "SELECT InputID, InputName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SettingsXML,ClassifyID FROM siteserver_Input WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis DESC, AddDate DESC";

        private const string SQL_SELECT_INPUT_ID = "SELECT InputID FROM siteserver_Input WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis DESC, AddDate DESC";

        private const string SQL_SELECT_INPUT_NAME = "SELECT InputName FROM siteserver_Input WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis DESC, AddDate DESC";

        private const string SQL_SELECT_CLASSIFY_ID = "SELECT  InputID, InputName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SettingsXML,ClassifyID  FROM siteserver_Input WHERE ClassifyID=@ClassifyID";

        private const string SQL_DELETE_CLASSIFY_ID = "DELETE FROM siteserver_Input WHERE ClassifyID=@ClassifyID";

        private const string SQL_SELECT_ALL = "SELECT  InputID, InputName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SettingsXML,ClassifyID  FROM siteserver_Input";

        private const string SQL_SELECT_CLASSIFY_INPUT = "SELECT InputID, InputName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SettingsXML,ClassifyID FROM siteserver_Input WHERE PublishmentSystemID = @PublishmentSystemID and ClassifyID=@ClassifyID ORDER BY Taxis DESC, AddDate DESC";

        private const string SQL_SELECT_INPUT_BY_WHERE_CLASSIFY = "SELECT InputID, InputName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SettingsXML,ClassifyID FROM siteserver_Input WHERE InputName = @InputName AND PublishmentSystemID = @PublishmentSystemID and ClassifyID=@ClassifyID";

        private const string PARM_INPUT_ID = "@InputID";
        private const string PARM_INPUT_NAME = "@InputName";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_IS_CHECKED = "@IsChecked";
        private const string PARM_IS_REPLY = "@IsReply";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_IS_TEMPLATE = "@IsTemplate";
        private const string PARM_STYLE_TEMPLATE = "@StyleTemplate";
        private const string PARM_SCRIPT_TEMPLATE = "@ScriptTemplate";
        private const string PARM_CONTENT_TEMPLATE = "@ContentTemplate";
        private const string PARM_SETTINGS_XML = "@SettingsXML";
        private const string PARM_CLASSIFY_ID = "@ClassifyID";

        public int Insert(InputInfo inputInfo)
        {
            int inputID = 0;

            string sqlString = "INSERT INTO siteserver_Input (InputName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SettingsXML,ClassifyID) VALUES (@InputName, @PublishmentSystemID, @AddDate, @IsChecked, @IsReply, @Taxis, @IsTemplate, @StyleTemplate, @ScriptTemplate, @ContentTemplate, @SettingsXML,@ClassifyID)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                sqlString = "INSERT INTO siteserver_Input (InputID, InputName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SettingsXML,ClassifyID) VALUES (siteserver_Input_SEQ.NEXTVAL, @InputName, @PublishmentSystemID, @AddDate, @IsChecked, @IsReply, @Taxis, @IsTemplate, @StyleTemplate, @ScriptTemplate, @ContentTemplate, @SettingsXML,@ClassifyID)";
            }

            int taxis = this.GetMaxTaxis(inputInfo.PublishmentSystemID) + 1;
            IDbDataParameter[] insertParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_INPUT_NAME, EDataType.NVarChar, 50, inputInfo.InputName),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, inputInfo.PublishmentSystemID),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, inputInfo.AddDate),
                this.GetParameter(PARM_IS_CHECKED, EDataType.VarChar, 18, inputInfo.IsChecked.ToString()),
                this.GetParameter(PARM_IS_REPLY, EDataType.VarChar, 18, inputInfo.IsReply.ToString()),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis),
                this.GetParameter(PARM_IS_TEMPLATE, EDataType.VarChar, 18, inputInfo.IsTemplate.ToString()),
                this.GetParameter(PARM_STYLE_TEMPLATE, EDataType.NText, inputInfo.StyleTemplate),
                this.GetParameter(PARM_SCRIPT_TEMPLATE, EDataType.NText, inputInfo.ScriptTemplate),
                this.GetParameter(PARM_CONTENT_TEMPLATE, EDataType.NText, inputInfo.ContentTemplate),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, inputInfo.Additional.ToString()),
                this.GetParameter(PARM_CLASSIFY_ID, EDataType.Integer, inputInfo.ClassifyID)
            };

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, sqlString, insertParms);
                        inputID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "siteserver_Input");
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return inputID;
        }

        public void Update(InputInfo inputInfo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_INPUT_NAME, EDataType.NVarChar, 50, inputInfo.InputName),
                this.GetParameter(PARM_IS_CHECKED, EDataType.VarChar, 18, inputInfo.IsChecked.ToString()),
                this.GetParameter(PARM_IS_REPLY, EDataType.VarChar, 18, inputInfo.IsReply.ToString()),
                this.GetParameter(PARM_IS_TEMPLATE, EDataType.VarChar, 18, inputInfo.IsTemplate.ToString()),
                this.GetParameter(PARM_STYLE_TEMPLATE, EDataType.NText, inputInfo.StyleTemplate),
                this.GetParameter(PARM_SCRIPT_TEMPLATE, EDataType.NText, inputInfo.ScriptTemplate),
                this.GetParameter(PARM_CONTENT_TEMPLATE, EDataType.NText, inputInfo.ContentTemplate),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, inputInfo.Additional.ToString()),
                this.GetParameter(PARM_INPUT_ID, EDataType.Integer, inputInfo.InputID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_INPUT, updateParms);
        }

        public void Delete(int inputID)
        {
            IDbDataParameter[] deleteParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_INPUT_ID, EDataType.Integer, inputID)
            };

            this.ExecuteNonQuery(SQL_DELETE_INPUT, deleteParms);
        }

        public InputInfo GetInputInfo(string inputName, int publishmentSystemID)
        {
            InputInfo inputInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_INPUT_NAME, EDataType.NVarChar, 50, inputName),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INPUT_BY_WHERE, parms))
            {
                if (rdr.Read())
                {
                    inputInfo = new InputInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetDateTime(3), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12));
                }
                rdr.Close();
            }

            return inputInfo;
        }

        public InputInfo GetInputInfo(string inputName, int publishmentSystemID, int itemID)
        {
            InputInfo inputInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_INPUT_NAME, EDataType.NVarChar, 50, inputName),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CLASSIFY_ID, EDataType.Integer, itemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INPUT_BY_WHERE_CLASSIFY, parms))
            {
                if (rdr.Read())
                {
                    inputInfo = new InputInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetDateTime(3), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12));
                }
                rdr.Close();
            }

            return inputInfo;
        }


        public InputInfo GetInputInfoAsPossible(int inputID, int publishmentSystemID)
        {
            InputInfo inputInfo = null;

            inputInfo = this.GetInputInfo(inputID);
            if (inputInfo == null)
            {
                inputInfo = this.GetLastAddInputInfo(publishmentSystemID);
            }

            return inputInfo;
        }

        public int GetInputIDAsPossible(string inputName, int publishmentSystemID)
        {
            int inputID = 0;

            if (!string.IsNullOrEmpty(inputName))
            {
                IDbDataParameter[] selectParms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_INPUT_NAME, EDataType.NVarChar, 50, inputName),
                    this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
                };

                using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INPUT_ID_BY_WHERE, selectParms))
                {
                    if (rdr.Read() && !rdr.IsDBNull(0))
                    {
                        inputID = rdr.GetInt32(0);
                    }
                }
            }

            if (inputID == 0)
            {
                InputInfo inputInfo = this.GetLastAddInputInfo(publishmentSystemID);
                if (inputInfo != null)
                {
                    inputID = inputInfo.InputID;
                }
            }

            return inputID;
        }

        public InputInfo GetInputInfo(int inputID)
        {
            InputInfo inputInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_INPUT_ID, EDataType.Integer, inputID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INPUT_BY_INPUT_ID, parms))
            {
                if (rdr.Read())
                {
                    inputInfo = new InputInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetDateTime(3), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12));
                }
                rdr.Close();
            }

            return inputInfo;
        }

        public InputInfo GetLastAddInputInfo(int publishmentSystemID)
        {
            InputInfo inputInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INPUT_BY_ADDDATE, parms))
            {
                if (rdr.Read())
                {
                    inputInfo = new InputInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetDateTime(3), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12));
                }
                rdr.Close();
            }

            return inputInfo;
        }

        public bool IsExists(string inputName, int publishmentSystemID)
        {
            bool exists = false;

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_INPUT_NAME, EDataType.NVarChar, 50, inputName),
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INPUT_BY_WHERE, selectParms))
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
            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL_INPUT, selectParms);
            return enumerable;
        }


        public IEnumerable GetDataSource(int publishmentSystemID, int classifyID)
        {
            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(PARM_CLASSIFY_ID, EDataType.Integer, classifyID)
            };
            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_CLASSIFY_INPUT, selectParms);
            return enumerable;
        }

        /// <summary>
        /// 有条件的表单查询
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="classifyID"></param>
        /// <param name="inputName"></param>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <returns></returns>
        public IEnumerable GetDataSource(int publishmentSystemID, int classifyID, string inputName, string dateFrom, string dateTo)
        {

            string dateString = string.Empty;
            if (!string.IsNullOrEmpty(dateFrom))
            {
                dateString = string.Format(" AND AddDate >= '{0}' ", dateFrom);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString = string.Format(" AND to_char(AddDate,'YYYY-MM-DD') >= '{0}' ", dateFrom);
                }
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                dateTo = DateUtils.GetDateString(TranslateUtils.ToDateTime(dateTo).AddDays(1));
                dateString += string.Format(" AND AddDate <= '{0}' ", dateTo);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString += string.Format(" AND to_char(AddDate,'YYYY-MM-DD') <= '{0}' ", dateTo);
                }
            }

            StringBuilder whereStr = new StringBuilder();
            whereStr.AppendFormat(" where PublishmentSystemID={0}", publishmentSystemID);

            InputClissifyInfo pinfo = DataProvider.InputClassifyDAO.GetDefaultInfo(publishmentSystemID);
            if (classifyID != pinfo.ItemID)
                whereStr.AppendFormat(" and ClassifyID={0}", classifyID);
            whereStr.Append(dateString);

            if (!string.IsNullOrEmpty(inputName))
            {
                whereStr.AppendFormat(" AND (InputName LIKE '%{0}%')  ", inputName);
            }

            whereStr.Append(" ORDER BY Taxis DESC");

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL + whereStr.ToString());
            return enumerable;
        }

        public ArrayList GetInputIDArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INPUT_ID, selectParms))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetInputNameArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] selectParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_INPUT_NAME, selectParms))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }

            return arraylist;
        }

        public string GetImportInputName(string inputName, int publishmentSystemID)
        {
            string importInputName = "";
            if (inputName.IndexOf("_") != -1)
            {
                int inputName_Count = 0;
                string lastInputName = inputName.Substring(inputName.LastIndexOf("_") + 1);
                string firstInputName = inputName.Substring(0, inputName.Length - lastInputName.Length);
                try
                {
                    inputName_Count = int.Parse(lastInputName);
                }
                catch { }
                inputName_Count++;
                importInputName = firstInputName + inputName_Count;
            }
            else
            {
                importInputName = inputName + "_1";
            }

            InputInfo inputInfo = this.GetInputInfo(inputName, publishmentSystemID);
            if (inputInfo != null)
            {
                importInputName = GetImportInputName(importInputName, publishmentSystemID);
            }

            return importInputName;
        }

        public bool UpdateTaxisToUp(int publishmentSystemID, int inputID)
        {
            string sqlString = string.Format("SELECT TOP 1 InputID, Taxis FROM siteserver_Input WHERE ((Taxis > (SELECT Taxis FROM siteserver_Input WHERE InputID = {0})) AND PublishmentSystemID ={1}) ORDER BY Taxis", inputID, publishmentSystemID);
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = Convert.ToInt32(rdr[0]);
                    higherTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(inputID);

            if (higherID != 0)
            {
                SetTaxis(inputID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int publishmentSystemID, int inputID)
        {
            string sqlString = string.Format("SELECT TOP 1 InputID, Taxis FROM siteserver_Input WHERE ((Taxis < (SELECT Taxis FROM siteserver_Input WHERE (InputID = {0}))) AND PublishmentSystemID = {1}) ORDER BY Taxis DESC", inputID, publishmentSystemID);
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = Convert.ToInt32(rdr[0]);
                    lowerTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(inputID);

            if (lowerID != 0)
            {
                SetTaxis(inputID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }
        public bool UpdateTaxisToUp(int publishmentSystemID, int inputID, int classifyID)
        {
            string sqlString = string.Format("SELECT TOP 1 InputID, Taxis FROM siteserver_Input WHERE ((Taxis > (SELECT Taxis FROM siteserver_Input WHERE InputID = {0})) AND PublishmentSystemID ={1} AND ClassifyID={2}) ORDER BY Taxis", inputID, publishmentSystemID, classifyID);
            int higherID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherID = Convert.ToInt32(rdr[0]);
                    higherTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(inputID);

            if (higherID != 0)
            {
                SetTaxis(inputID, higherTaxis);
                SetTaxis(higherID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int publishmentSystemID, int inputID, int classifyID)
        {
            string sqlString = string.Format("SELECT TOP 1 InputID, Taxis FROM siteserver_Input WHERE ((Taxis < (SELECT Taxis FROM siteserver_Input WHERE (InputID = {0}))) AND PublishmentSystemID = {1} AND ClassifyID={2}) ORDER BY Taxis DESC", inputID, publishmentSystemID, classifyID);
            int lowerID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerID = Convert.ToInt32(rdr[0]);
                    lowerTaxis = Convert.ToInt32(rdr[1]);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(inputID);

            if (lowerID != 0)
            {
                SetTaxis(inputID, lowerTaxis);
                SetTaxis(lowerID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM siteserver_Input WHERE PublishmentSystemID = {0}", publishmentSystemID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int inputID)
        {
            string sqlString = string.Format("SELECT Taxis FROM siteserver_Input WHERE (InputID = {0})", inputID);
            int taxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    taxis = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return taxis;
        }

        private void SetTaxis(int inputID, int taxis)
        {
            string sqlString = string.Format("UPDATE siteserver_Input SET Taxis = {0} WHERE InputID = {1}", taxis, inputID);
            this.ExecuteNonQuery(sqlString);
        }


        public int GetCount(int classifyID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM siteserver_Input WHERE (ClassifyID = {0})", classifyID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public void DeleteByClassifyID(int classifyID)
        {
            InputInfo inputInfo = GetInputInfo(classifyID);

            IDataParameter[] parms = new IDataParameter[]
            {
                this.GetParameter(PARM_CLASSIFY_ID, EDataType.Integer, classifyID)
            };
            this.ExecuteNonQuery(SQL_DELETE_CLASSIFY_ID, parms);

            int contentNum = (inputInfo == null ? 0 : this.GetCount(inputInfo.ClassifyID));

            DataProvider.InputClassifyDAO.UpdateContentNum(0, classifyID, contentNum);
        }

        public List<InputInfo> GetKeywordInfoList(int classifyID)
        {
            List<InputInfo> list = new List<InputInfo>();
            string sql = string.Empty;
            if (classifyID > 0)
                sql = string.Format("SELECT InputID, InputName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SettingsXML,ClassifyID FROM siteserver_Input WHERE ClassifyID = {0}", classifyID);
            else
                sql = SQL_SELECT_ALL;
            using (IDataReader rdr = this.ExecuteReader(sql))
            {
                while (rdr.Read())
                {
                    InputInfo inputInfo = new InputInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), rdr.GetDateTime(3), TranslateUtils.ToBool(rdr.GetValue(4).ToString()), TranslateUtils.ToBool(rdr.GetValue(5).ToString()), rdr.GetInt32(6), TranslateUtils.ToBool(rdr.GetValue(7).ToString()), rdr.GetValue(8).ToString(), rdr.GetValue(9).ToString(), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12));
                    list.Add(inputInfo);
                }
                rdr.Close();
            }
            return list;
        }


        public string GetSelectCommand(int classifyID)
        {
            if (classifyID > 0)
                return string.Format("SELECT InputID, InputName, PublishmentSystemID, AddDate, IsChecked, IsReply, Taxis, IsTemplate, StyleTemplate, ScriptTemplate, ContentTemplate, SettingsXML,ClassifyID FROM siteserver_Input WHERE ClassifyID = {0}", classifyID);
            else
                return SQL_SELECT_ALL;
        }

        /// <summary>
        /// 加载有权限的分类下的表单
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="classifyID"></param>
        /// <param name="inputName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="classifyIDs"></param>
        /// <returns></returns>
        public IEnumerable GetDataSource(int publishmentSystemID, int classifyID, string inputName, string dateFrom, string dateTo, ArrayList classifyIDs)
        {

            string dateString = string.Empty;
            if (!string.IsNullOrEmpty(dateFrom))
            {
                dateString = string.Format(" AND AddDate >= '{0}' ", dateFrom);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString = string.Format(" AND to_char(AddDate,'YYYY-MM-DD') >= '{0}' ", dateFrom);
                }
            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                dateTo = DateUtils.GetDateString(TranslateUtils.ToDateTime(dateTo).AddDays(1));
                dateString += string.Format(" AND AddDate <= '{0}' ", dateTo);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    dateString += string.Format(" AND to_char(AddDate,'YYYY-MM-DD') <= '{0}' ", dateTo);
                }
            }

            StringBuilder whereStr = new StringBuilder();
            whereStr.AppendFormat(" where PublishmentSystemID={0}", publishmentSystemID);

            InputClissifyInfo pinfo = DataProvider.InputClassifyDAO.GetDefaultInfo(publishmentSystemID);
            if (classifyID != pinfo.ItemID)
                whereStr.AppendFormat(" and ClassifyID={0}", classifyID);
            else
            {
                if (classifyIDs.Count > 0)
                    whereStr.AppendFormat(" and ClassifyID in ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(classifyIDs));
            }
            whereStr.Append(dateString);

            if (!string.IsNullOrEmpty(inputName))
            {
                whereStr.AppendFormat(" AND (InputName LIKE '%{0}%')  ", inputName);
            }

            whereStr.Append(" ORDER BY Taxis DESC");

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(SQL_SELECT_ALL + whereStr.ToString());
            return enumerable;
        }

    }
}