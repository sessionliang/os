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
    public class GovPublicIdentifierRuleDAO : DataProviderBase, IGovPublicIdentifierRuleDAO
	{
        private const string SQL_UPDATE = "UPDATE siteserver_GovPublicIdentifierRule SET RuleName = @RuleName, IdentifierType = @IdentifierType, MinLength = @MinLength, Suffix = @Suffix, FormatString = @FormatString, AttributeName = @AttributeName, Sequence = @Sequence, SettingsXML = @SettingsXML WHERE RuleID = @RuleID";

        private const string SQL_DELETE = "DELETE FROM siteserver_GovPublicIdentifierRule WHERE RuleID = @RuleID";

        private const string SQL_SELECT = "SELECT RuleID, RuleName, PublishmentSystemID, IdentifierType, MinLength, Suffix, FormatString, AttributeName, Sequence, Taxis, SettingsXML FROM siteserver_GovPublicIdentifierRule WHERE RuleID = @RuleID";

        private const string SQL_SELECT_ALL = "SELECT RuleID, RuleName, PublishmentSystemID, IdentifierType, MinLength, Suffix, FormatString, AttributeName, Sequence, Taxis, SettingsXML FROM siteserver_GovPublicIdentifierRule WHERE PublishmentSystemID = @PublishmentSystemID ORDER BY Taxis";

        private const string SQL_SELECT_IDENTIFIER_TYPE = "SELECT IdentifierType FROM siteserver_GovPublicIdentifierRule WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string PARM_RULE_ID = "@RuleID";
        private const string PARM_RULE_NAME = "@RuleName";
        private const string PARM_PUBLISHMENTSYSTEMID = "@PublishmentSystemID";
        private const string PARM_IDENTIFIER_TYPE = "@IdentifierType";
        private const string PARM_MIN_LENGTH = "@MinLength";
        private const string PARM_SUFFIX = "@Suffix";
        private const string PARM_FORMAT_STRING = "@FormatString";
        private const string PARM_ATTRIBUTE_NAME = "@AttributeName";
        private const string PARM_SEQUENCE = "@Sequence";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_SETTINGS_XML = "@SettingsXML";

        public void Insert(GovPublicIdentifierRuleInfo ruleInfo)
        {
            int taxis = this.GetMaxTaxis(ruleInfo.PublishmentSystemID) + 1;

            string SQL_INSERT = "INSERT INTO siteserver_GovPublicIdentifierRule (RuleName, PublishmentSystemID, IdentifierType, MinLength, Suffix, FormatString, AttributeName, Sequence, Taxis, SettingsXML) VALUES (@RuleName, @PublishmentSystemID, @IdentifierType, @MinLength, @Suffix, @FormatString, @AttributeName, @Sequence, @Taxis, @SettingsXML)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT = "INSERT INTO siteserver_GovPublicIdentifierRule (RuleID, RuleName, PublishmentSystemID, IdentifierType, MinLength, Suffix, FormatString, AttributeName, Sequence, Taxis, SettingsXML) VALUES (siteserver_GovPublicIR_SEQ.NEXTVAL, @RuleName, @PublishmentSystemID, @IdentifierType, @MinLength, @Suffix, @FormatString, @AttributeName, @Sequence, @Taxis, @SettingsXML)";
            }

			IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_RULE_NAME, EDataType.NVarChar, 255, ruleInfo.RuleName),
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, ruleInfo.PublishmentSystemID),
                this.GetParameter(PARM_IDENTIFIER_TYPE, EDataType.VarChar, 50, EGovPublicIdentifierTypeUtils.GetValue(ruleInfo.IdentifierType)),
                this.GetParameter(PARM_MIN_LENGTH, EDataType.Integer, ruleInfo.MinLength),
                this.GetParameter(PARM_SUFFIX, EDataType.VarChar, 50, ruleInfo.Suffix),
                this.GetParameter(PARM_FORMAT_STRING, EDataType.VarChar, 50, ruleInfo.FormatString),
                this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, ruleInfo.AttributeName),
                this.GetParameter(PARM_SEQUENCE, EDataType.Integer, ruleInfo.Sequence),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, ruleInfo.Additional.ToString())
			};

            this.ExecuteNonQuery(SQL_INSERT, parms);
		}

        public void Update(GovPublicIdentifierRuleInfo ruleInfo) 
		{
			IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_RULE_NAME, EDataType.NVarChar, 255, ruleInfo.RuleName),
                this.GetParameter(PARM_IDENTIFIER_TYPE, EDataType.VarChar, 50, EGovPublicIdentifierTypeUtils.GetValue(ruleInfo.IdentifierType)),
                this.GetParameter(PARM_MIN_LENGTH, EDataType.Integer, ruleInfo.MinLength),
                this.GetParameter(PARM_SUFFIX, EDataType.VarChar, 50, ruleInfo.Suffix),
                this.GetParameter(PARM_FORMAT_STRING, EDataType.VarChar, 50, ruleInfo.FormatString),
                this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, ruleInfo.AttributeName),
                this.GetParameter(PARM_SEQUENCE, EDataType.Integer, ruleInfo.Sequence),
                this.GetParameter(PARM_SETTINGS_XML, EDataType.NText, ruleInfo.Additional.ToString()),
                this.GetParameter(PARM_RULE_ID, EDataType.Integer, ruleInfo.RuleID)
			};

            this.ExecuteNonQuery(SQL_UPDATE, parms);
		}

		public void Delete(int ruleID)
		{
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_RULE_ID, EDataType.Integer, ruleID)
			};

            this.ExecuteNonQuery(SQL_DELETE, parms);
		}

        public int GetCount(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT COUNT(*) FROM siteserver_GovPublicIdentifierRule WHERE PublishmentSystemID = {0}", publishmentSystemID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public GovPublicIdentifierRuleInfo GetIdentifierRuleInfo(int ruleID)
		{
            GovPublicIdentifierRuleInfo ruleInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_RULE_ID, EDataType.Integer, ruleID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT, parms))
            {
                if (rdr.Read())
                {
                    ruleInfo = new GovPublicIdentifierRuleInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), EGovPublicIdentifierTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetInt32(8), rdr.GetInt32(9), rdr.GetValue(10).ToString());
                }
                rdr.Close();
            }

            return ruleInfo;
		}

        public ArrayList GetRuleInfoArrayList(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] selectParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PUBLISHMENTSYSTEMID, EDataType.Integer, publishmentSystemID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL, selectParms))
            {
                while (rdr.Read())
                {
                    GovPublicIdentifierRuleInfo ruleInfo = new GovPublicIdentifierRuleInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), rdr.GetInt32(2), EGovPublicIdentifierTypeUtils.GetEnumType(rdr.GetValue(3).ToString()), rdr.GetInt32(4), rdr.GetValue(5).ToString(), rdr.GetValue(6).ToString(), rdr.GetValue(7).ToString(), rdr.GetInt32(8), rdr.GetInt32(9), rdr.GetValue(10).ToString());
                    arraylist.Add(ruleInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public bool UpdateTaxisToUp(int ruleID, int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT TOP 1 RuleID, Taxis FROM siteserver_GovPublicIdentifierRule WHERE ((Taxis > (SELECT Taxis FROM siteserver_GovPublicIdentifierRule WHERE RuleID = {0})) AND PublishmentSystemID ={1}) ORDER BY Taxis", ruleID, publishmentSystemID);
            int higherRuleID = 0;
            int higherTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    higherRuleID = rdr.GetInt32(0);
                    higherTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(ruleID, publishmentSystemID);

            if (higherRuleID > 0)
            {
                SetTaxis(ruleID, higherTaxis);
                SetTaxis(higherRuleID, selectedTaxis);
                return true;
            }
            return false;
        }

        public bool UpdateTaxisToDown(int ruleID, int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT TOP 1 RuleID, Taxis FROM siteserver_GovPublicIdentifierRule WHERE ((Taxis < (SELECT Taxis FROM siteserver_GovPublicIdentifierRule WHERE RuleID = {0})) AND PublishmentSystemID = {1}) ORDER BY Taxis DESC", ruleID, publishmentSystemID);
            int lowerRuleID = 0;
            int lowerTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    lowerRuleID = rdr.GetInt32(0);
                    lowerTaxis = rdr.GetInt32(1);
                }
                rdr.Close();
            }

            int selectedTaxis = GetTaxis(ruleID, publishmentSystemID);

            if (lowerRuleID > 0)
            {
                SetTaxis(ruleID, lowerTaxis);
                SetTaxis(lowerRuleID, selectedTaxis);
                return true;
            }
            return false;
        }

        private int GetMaxTaxis(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) FROM siteserver_GovPublicIdentifierRule WHERE PublishmentSystemID = {0}", publishmentSystemID);
            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        private int GetTaxis(int ruleID, int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT Taxis FROM siteserver_GovPublicIdentifierRule WHERE RuleID = {0}", ruleID);
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

        private void SetTaxis(int ruleID, int taxis)
        {
            string sqlString = string.Format("UPDATE siteserver_GovPublicIdentifierRule SET Taxis = {0} WHERE RuleID = {1}", taxis, ruleID);
            this.ExecuteNonQuery(sqlString);
        }
	}
}