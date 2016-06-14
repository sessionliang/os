using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;


namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class ResumeContentDAO : DataProviderBase, IResumeContentDAO
	{
        public string TableName
        {
            get
            {
                return "siteserver_ResumeContent";
            }
        }

		public int Insert(ResumeContentInfo info)
		{
			int contentID = 0;

            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(info.Attributes, TableName, out parms);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT, parms);

                        contentID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TableName);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

			return contentID;
		}

        public void Update(ResumeContentInfo info)
		{
            info.BeforeExecuteNonQuery();
            IDbDataParameter[] parms = null;
            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(info.Attributes, TableName, out parms);

            this.ExecuteNonQuery(SQL_UPDATE, parms);
		}

        public void SetIsView(ArrayList idCollection, bool isView)
        {
            string sqlString = string.Format("UPDATE {0} SET IsView = '{1}' WHERE ID IN ({2})", TableName, isView.ToString(), TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(idCollection));

            this.ExecuteNonQuery(sqlString);
        }

		public void Delete(ArrayList deleteIDArrayList)
		{
            string sqlString = string.Format("DELETE FROM {0} WHERE ID IN ({1})", TableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(deleteIDArrayList));
            this.ExecuteNonQuery(sqlString);
		}

        public void Delete(int styleID)
        {
            string sqlString = string.Format("DELETE FROM {0} WHERE StyleID ={1}", TableName, styleID);
            this.ExecuteNonQuery(sqlString);
        }

        public ResumeContentInfo GetContentInfo(int publishmentSystemID, int styleID, NameValueCollection form)
        {
            ResumeContentInfo contentInfo = new ResumeContentInfo(0, styleID, publishmentSystemID, TranslateUtils.ToInt(form[ResumeContentAttribute.JobContentID]), BaiRongDataProvider.UserDAO.CurrentUserName, DateTime.Now);

            foreach (string name in form.AllKeys)
            {
                if (ResumeContentAttribute.BasicAttributes.Contains(name.ToLower()))
                {
                    string value = form[name];
                    if (!string.IsNullOrEmpty(value))
                    {
                        contentInfo.SetExtendedAttribute(name, value);
                    }
                }
            }

            int count = TranslateUtils.ToInt(form[ResumeContentAttribute.Exp_Count]);
            contentInfo.SetExtendedAttribute(ResumeContentAttribute.Exp_Count, count.ToString());
            for (int index = 1; index <= count; index++)
            {
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Exp_FromYear, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Exp_FromMonth, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Exp_ToYear, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Exp_ToMonth, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Exp_EmployerName, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Exp_Department, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Exp_EmployerPhone, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Exp_WorkPlace, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Exp_PositionTitle, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Exp_Industry, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Exp_Summary, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Exp_Score, index);
            }

            count = TranslateUtils.ToInt(form[ResumeContentAttribute.Pro_Count]);
            contentInfo.SetExtendedAttribute(ResumeContentAttribute.Pro_Count, count.ToString());
            for (int index = 1; index <= count; index++)
            {
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Pro_FromYear, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Pro_FromMonth, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Pro_ToYear, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Pro_ToMonth, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Pro_ProjectName, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Pro_Summary, index);
            }

            count = TranslateUtils.ToInt(form[ResumeContentAttribute.Edu_Count]);
            contentInfo.SetExtendedAttribute(ResumeContentAttribute.Edu_Count, count.ToString());
            for (int index = 1; index <= count; index++)
            {
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Edu_FromYear, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Edu_FromMonth, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Edu_ToYear, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Edu_ToMonth, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Edu_SchoolName, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Edu_Education, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Edu_Profession, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Edu_Summary, index);
            }

            count = TranslateUtils.ToInt(form[ResumeContentAttribute.Tra_Count]);
            contentInfo.SetExtendedAttribute(ResumeContentAttribute.Tra_Count, count.ToString());
            for (int index = 1; index <= count; index++)
            {
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Tra_FromYear, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Tra_FromMonth, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Tra_ToYear, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Tra_ToMonth, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Tra_TrainerName, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Tra_TrainerAddress, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Tra_Lesson, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Tra_Centification, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Tra_Summary, index);
            }

            count = TranslateUtils.ToInt(form[ResumeContentAttribute.Lan_Count]);
            contentInfo.SetExtendedAttribute(ResumeContentAttribute.Lan_Count, count.ToString());
            for (int index = 1; index <= count; index++)
            {
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Lan_Language, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Lan_Level, index);
            }

            count = TranslateUtils.ToInt(form[ResumeContentAttribute.Ski_Count]);
            contentInfo.SetExtendedAttribute(ResumeContentAttribute.Ski_Count, count.ToString());
            for (int index = 1; index <= count; index++)
            {
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Ski_SkillName, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Ski_UsedTimes, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Ski_Ability, index);
            }

            count = TranslateUtils.ToInt(form[ResumeContentAttribute.Cer_Count]);
            contentInfo.SetExtendedAttribute(ResumeContentAttribute.Cer_Count, count.ToString());
            for (int index = 1; index <= count; index++)
            {
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Cer_CertificationName, index);
                this.SetValueByIndex(contentInfo, form, ResumeContentAttribute.Cer_EffectiveDate, index);
            }

            return contentInfo;
        }

        private void SetValueByIndex(ResumeContentInfo contentInfo, NameValueCollection form, string attributeName, int index)
        {
            string value = form[ResumeContentAttribute.GetAttributeName(attributeName, index)];
            if (value == null)
            {
                value = string.Empty;
            }
            value = value.Replace("&", string.Empty);
            if (index == 1)
            {
                contentInfo.SetExtendedAttribute(attributeName, value);
            }
            else
            {
                contentInfo.SetExtendedAttribute(attributeName, contentInfo.GetExtendedAttribute(attributeName) + "&" + value);
            }
        }

        public ResumeContentInfo GetContentInfo(int contentID)
		{
			ResumeContentInfo info = null;
            string SQL_WHERE = string.Format("WHERE ID = {0}", contentID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new ResumeContentInfo();
                    BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            if (info != null) info.AfterExecuteReader();
			return info;
		}

        public int GetCount(int styleID)
		{
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (StyleID = {1})", TableName, styleID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
		}

        public int GetCount(int publishmentSystemID, int jobContentID)
        {
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM {0} WHERE (PublishmentSystemID = {1} AND JobContentID = {2})", TableName, publishmentSystemID, jobContentID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public string GetSelectStringOfID(int styleID, string whereString)
        {
            string where = string.Format("WHERE (StyleID = {0} {1}) ORDER BY ID DESC", styleID, whereString);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, "ID", where);
        }

        public string GetSelectStringOfID(int publishmentSystemID, int jobContentID, string whereString)
        {
            string where = string.Format("WHERE (PublishmentSystemID = {0} AND JobContentID = {1} {2}) ORDER BY ID DESC", publishmentSystemID, jobContentID, whereString);
            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, "ID", where);
        }

        public string GetSelectSqlString(int styleID, int startNum, int totalNum, string whereString, string orderByString)
        {
            if (!string.IsNullOrEmpty(whereString) && !StringUtils.StartsWithIgnoreCase(whereString.Trim(), "AND "))
            {
                whereString = "AND " + whereString.Trim();
            }
            string sqlWhereString = string.Format("WHERE StyleID = {0} {1}", styleID, whereString);

            return BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TableName, startNum, totalNum, SqlUtils.Asterisk, sqlWhereString, orderByString);
        }

        public string GetSortFieldName()
        {
            return "ID";
        }
	}
}
