using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using System.Data;
using BaiRong.Model;
using BaiRong.Core;
using System.Collections;

namespace BaiRong.Provider.Data.SqlServer
{
    public class AjaxUrlDAO : DataProviderBase, IAjaxUrlDAO
    {
        private const string SQL_INSERT = "INSERT INTO bairong_AjaxUrl (SN, AjaxUrl, Parameters, PublishmentSystemID, ContentID, NodeID, TemplateID, CreateTaskID, ActionType) VALUES (@SN, @AjaxUrl, @Parameters, @PublishmentSystemID, @ContentID, @NodeID, @TemplateID, @CreateTaskID, @ActionType)";

        private const string SQL_DELETE = "DELETE FROM bairong_AjaxUrl WHERE SN = @SN";

        private const string SQL_DELETE_CREATETASKID = "DELETE FROM bairong_AjaxUrl WHERE CreateTaskID = @CreateTaskID";

        private const string SQL_DELETE_SAME_QUEUE = "DELETE FROM bairong_AjaxUrl WHERE PublishmentSystemID=@PublishmentSystemID AND NodeID=@NodeID AND ContentID=@ContentID AND TemplateID=@TemplateID AND CreateTaskID != 0 AND  AjaxUrl = '' AND ActionType = 'QueueCreate' ";

        private const string SQL_SELECT_ALL = "SELECT SN, AjaxUrl, Parameters, PublishmentSystemID, ContentID, NodeID, TemplateID, CreateTaskID FROM bairong_AjaxUrl WHERE AjaxUrl != '' ORDER BY CreateTaskID,SN";

        private const string SQL_SELECT_ALL_FOR_QUEUE_CREATE = "SELECT SN, AjaxUrl, Parameters, PublishmentSystemID, ContentID, NodeID, TemplateID, CreateTaskID FROM bairong_AjaxUrl WHERE AjaxUrl = '' AND ActionType = 'QueueCreate' ORDER BY CreateTaskID,SN";

        private const string SQL_SELECT_ALL_FOR_CREATE = "SELECT SN, AjaxUrl, Parameters, PublishmentSystemID, ContentID, NodeID, TemplateID, CreateTaskID FROM bairong_AjaxUrl WHERE AjaxUrl = '' AND ActionType = 'BackgroundCreate' ORDER BY CreateTaskID,SN";

        private const string PARM_SN = "@SN";
        private const string PARM_AJAX_URL = "@AjaxUrl";
        private const string PARM_PARAMETERS = "@Parameters";

        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_CONTENTID = "@ContentID";
        private const string PARM_NODEID = "@NodeID";
        private const string PARM_TEMPLATEID = "@TemplateID";
        private const string PARM_CREATETASKID = "@CreateTaskID";

        private const string PARM_ACTION_TYPE = "@ActionType";

        public void Insert(AjaxUrlInfo ajaxUrlInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SN, EDataType.VarChar, 50, ajaxUrlInfo.SN),
				this.GetParameter(PARM_AJAX_URL, EDataType.VarChar, 500, ajaxUrlInfo.AjaxUrl),
				this.GetParameter(PARM_PARAMETERS, EDataType.NText, ajaxUrlInfo.Parameters),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, ajaxUrlInfo.PublishmentSystemID),
				this.GetParameter(PARM_CONTENTID, EDataType.Integer, ajaxUrlInfo.ContentID),
				this.GetParameter(PARM_NODEID, EDataType.Integer, ajaxUrlInfo.NodeID),
				this.GetParameter(PARM_TEMPLATEID, EDataType.Integer, ajaxUrlInfo.TemplateID),
				this.GetParameter(PARM_CREATETASKID, EDataType.Integer, ajaxUrlInfo.CreateTaskID),
				this.GetParameter(PARM_ACTION_TYPE, EDataType.VarChar,50, ajaxUrlInfo.ActionType)
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);
        }

        public bool InsertForCreate(AjaxUrlInfo ajaxUrlInfo, out int existsID)
        {
            string isExistsSQL = string.Format(@"select CreateTaskID from bairong_AjaxUrl");
            string where = " where 1=1 ";
            if (ajaxUrlInfo.PublishmentSystemID > 0)
            {
                where += string.Format(" and PublishmentSystemID = {0} ", ajaxUrlInfo.PublishmentSystemID);
            }
            if (ajaxUrlInfo.NodeID > 0)
            {
                where += string.Format(" and NodeID = {0} ", ajaxUrlInfo.NodeID);
            }
            if (ajaxUrlInfo.ContentID > 0)
            {
                where += string.Format(" and ContentID = {0} ", ajaxUrlInfo.ContentID);
            }
            if (ajaxUrlInfo.TemplateID > 0)
            {
                where += string.Format(" and TemplateID = {0} ", ajaxUrlInfo.TemplateID);
            }

            object obj = this.ExecuteScalar(isExistsSQL + where);

            existsID = TranslateUtils.ToInt(obj != null ? obj.ToString() : string.Empty);
            if (existsID > 0)
            {
                return false;
            }
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SN, EDataType.VarChar, 50, ajaxUrlInfo.SN),
				this.GetParameter(PARM_AJAX_URL, EDataType.VarChar, 500, ajaxUrlInfo.AjaxUrl),
				this.GetParameter(PARM_PARAMETERS, EDataType.NText, ajaxUrlInfo.Parameters),
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, ajaxUrlInfo.PublishmentSystemID),
				this.GetParameter(PARM_CONTENTID, EDataType.Integer, ajaxUrlInfo.ContentID),
				this.GetParameter(PARM_NODEID, EDataType.Integer, ajaxUrlInfo.NodeID),
				this.GetParameter(PARM_TEMPLATEID, EDataType.Integer, ajaxUrlInfo.TemplateID),
				this.GetParameter(PARM_CREATETASKID, EDataType.Integer, ajaxUrlInfo.CreateTaskID),
				this.GetParameter(PARM_ACTION_TYPE, EDataType.VarChar,50, ajaxUrlInfo.ActionType)
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);
            return true;
        }

        public void Delete(string sn)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_SN, EDataType.VarChar, 50, sn)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
        }

        private List<AjaxUrlInfo> GetAjaxUrlInfoList()
        {
            List<AjaxUrlInfo> list = new List<AjaxUrlInfo>();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL))
            {
                while (rdr.Read())
                {
                    AjaxUrlInfo ajaxUrlInfo = new AjaxUrlInfo(rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), TranslateUtils.ToInt(rdr.GetValue(3).ToString()), TranslateUtils.ToInt(rdr.GetValue(4).ToString()), TranslateUtils.ToInt(rdr.GetValue(5).ToString()), TranslateUtils.ToInt(rdr.GetValue(6).ToString()), TranslateUtils.ToInt(rdr.GetValue(7).ToString()));
                    list.Add(ajaxUrlInfo);
                }
                rdr.Close();
            }
            return list;
        }

        private List<AjaxUrlInfo> GetAjaxUrlInfoListForQueueCreate()
        {
            List<AjaxUrlInfo> list = new List<AjaxUrlInfo>();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_FOR_QUEUE_CREATE))
            {
                while (rdr.Read())
                {
                    AjaxUrlInfo ajaxUrlInfo = new AjaxUrlInfo(rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), TranslateUtils.ToInt(rdr.GetValue(3).ToString()), TranslateUtils.ToInt(rdr.GetValue(4).ToString()), TranslateUtils.ToInt(rdr.GetValue(5).ToString()), TranslateUtils.ToInt(rdr.GetValue(6).ToString()), TranslateUtils.ToInt(rdr.GetValue(7).ToString()));
                    list.Add(ajaxUrlInfo);
                }
                rdr.Close();
            }
            return list;
        }
        private List<AjaxUrlInfo> GetAjaxUrlInfoListForCreate()
        {
            List<AjaxUrlInfo> list = new List<AjaxUrlInfo>();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_FOR_CREATE))
            {
                while (rdr.Read())
                {
                    AjaxUrlInfo ajaxUrlInfo = new AjaxUrlInfo(rdr.GetValue(0).ToString(), rdr.GetValue(1).ToString(), rdr.GetValue(2).ToString(), TranslateUtils.ToInt(rdr.GetValue(3).ToString()), TranslateUtils.ToInt(rdr.GetValue(4).ToString()), TranslateUtils.ToInt(rdr.GetValue(5).ToString()), TranslateUtils.ToInt(rdr.GetValue(6).ToString()), TranslateUtils.ToInt(rdr.GetValue(7).ToString()));
                    list.Add(ajaxUrlInfo);
                }
                rdr.Close();
            }
            return list;
        }

        public DictionaryEntryArrayList GetAjaxUrlInfoDictionaryEntryArrayList()
        {
            DictionaryEntryArrayList dictionary = new DictionaryEntryArrayList();

            List<AjaxUrlInfo> ajaxUrlInfoArrayList = this.GetAjaxUrlInfoList();
            foreach (AjaxUrlInfo ajaxUrlInfo in ajaxUrlInfoArrayList)
            {
                DictionaryEntry entry = new DictionaryEntry(ajaxUrlInfo.SN, ajaxUrlInfo);
                dictionary.Add(entry);
            }

            return dictionary;
        }
        public DictionaryEntryArrayList GetAjaxUrlInfoDictionaryEntryArrayListForQueueCreate()
        {
            DictionaryEntryArrayList dictionary = new DictionaryEntryArrayList();

            List<AjaxUrlInfo> ajaxUrlInfoArrayList = this.GetAjaxUrlInfoListForQueueCreate();
            foreach (AjaxUrlInfo ajaxUrlInfo in ajaxUrlInfoArrayList)
            {
                DictionaryEntry entry = new DictionaryEntry(ajaxUrlInfo.SN, ajaxUrlInfo);
                dictionary.Add(entry);
            }

            return dictionary;
        }

        public DictionaryEntryArrayList GetAjaxUrlInfoDictionaryEntryArrayListForCreate()
        {
            DictionaryEntryArrayList dictionary = new DictionaryEntryArrayList();

            List<AjaxUrlInfo> ajaxUrlInfoArrayList = this.GetAjaxUrlInfoListForCreate();
            foreach (AjaxUrlInfo ajaxUrlInfo in ajaxUrlInfoArrayList)
            {
                DictionaryEntry entry = new DictionaryEntry(ajaxUrlInfo.SN, ajaxUrlInfo);
                dictionary.Add(entry);
            }

            return dictionary;
        }

        public void DeleteByTaskID(int createTaskID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_CREATETASKID, EDataType.Integer, createTaskID)
			};

            this.ExecuteNonQuery(SQL_DELETE_CREATETASKID, parms);
        }

        public void DeleteSameQueue(AjaxUrlInfo ajaxUrlInfo)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, ajaxUrlInfo.PublishmentSystemID),
                this.GetParameter(PARM_NODEID, EDataType.Integer, ajaxUrlInfo.NodeID),
                this.GetParameter(PARM_CONTENTID, EDataType.Integer, ajaxUrlInfo.ContentID),
                this.GetParameter(PARM_TEMPLATEID, EDataType.Integer, ajaxUrlInfo.TemplateID)
			};

            this.ExecuteNonQuery(SQL_DELETE_SAME_QUEUE, parms);
        }
    }
}
