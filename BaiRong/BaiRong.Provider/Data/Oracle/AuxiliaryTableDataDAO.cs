using System;
using System.Collections;
using System.Text;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;
using BaiRong.Core.AuxiliaryTable;

namespace BaiRong.Provider.Data.Oracle
{
	public class AuxiliaryTableDataDAO : BaiRong.Provider.Data.SqlServer.AuxiliaryTableDataDAO
	{

		protected override string ADOType
		{
			get
			{
				return SqlUtils.ORACLE;
			}
		}

		protected override EDatabaseType DataBaseType
		{
			get
			{
                return EDatabaseType.Oracle;
			}
		}

        public override string GetCreateAuxiliaryTableSqlString(string tableName)
        {
            //获取tableType
            EAuxiliaryTableType tableType = BaiRongDataProvider.TableCollectionDAO.GetTableType(tableName);

            //获取columnSqlStringArrayList
            ArrayList columnSqlStringArrayList = new ArrayList();

            ArrayList tableMetadataInfoArrayList = TableManager.GetTableMetadataInfoArrayList(tableName);
            if (tableMetadataInfoArrayList.Count > 0)
            {
                foreach (TableMetadataInfo metadataInfo in tableMetadataInfoArrayList)
                {
                    string columnSql = SqlUtils.GetColumnSqlString(this.ADOType, metadataInfo.DataType, metadataInfo.AttributeName, metadataInfo.DataLength, metadataInfo.CanBeNull, metadataInfo.DBDefaultValue);
                    if (!string.IsNullOrEmpty(columnSql))
                    {
                        columnSqlStringArrayList.Add(columnSql);
                    }
                }
            }

            //开始生成sql语句
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat(@"CREATE TABLE [{0}] (", tableName);
            //添加默认字段
            sqlBuilder.Append(@"
[ID] NUMBER(38) NOT NULL,
[NodeID] NUMBER(38) DEFAULT 0 NOT NULL,
[PublishmentSystemID] NUMBER(38) DEFAULT 0 NOT NULL,
[AddUserName] NVARCHAR2(255) DEFAULT '' ,
[LastEditUserName] NVARCHAR2(255) DEFAULT '' ,
[LastEditDate] TIMESTAMP(6) DEFAULT sysdate NOT NULL,
[Taxis] NUMBER(38) DEFAULT 0 NOT NULL,
[ContentGroupNameCollection] NVARCHAR2(255) DEFAULT '' ,
[Tags] NVARCHAR2(255) DEFAULT '' ,
[SourceID] NUMBER(38) DEFAULT 0 NOT NULL,
[ReferenceID] NUMBER(38) DEFAULT 0 NOT NULL,
[IsChecked] VARCHAR2(18) DEFAULT '' ,
[CheckedLevel] NUMBER(38) DEFAULT 0 NOT NULL,
[Comments] NUMBER(38) DEFAULT 0 NOT NULL,
[Photos] NUMBER(38) DEFAULT 0 NOT NULL,
[Teleplays] NUMBER(38) DEFAULT 0 NOT NULL,
[Hits] NUMBER(38) DEFAULT 0 NOT NULL,
[HitsByDay] NUMBER(38) DEFAULT 0 NOT NULL,
[HitsByWeek] NUMBER(38) DEFAULT 0 NOT NULL,
[HitsByMonth] NUMBER(38) DEFAULT 0 NOT NULL,
[LastHitsDate] TIMESTAMP(6) DEFAULT sysdate NOT NULL,
[SettingsXML] NCLOB DEFAULT '',
");

            if (tableType == EAuxiliaryTableType.VoteContent)
            {
                sqlBuilder.AppendFormat(@"[{0}] VARCHAR2(18) DEFAULT '' ,", VoteContentAttribute.IsImageVote).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] VARCHAR2(18) DEFAULT '' ,", VoteContentAttribute.IsSummary).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] NUMBER(38) DEFAULT 0 NOT NULL ,", VoteContentAttribute.Participants).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] VARCHAR2(18) DEFAULT '' ,", VoteContentAttribute.IsClosed).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] VARCHAR2(18) DEFAULT '' ,", VoteContentAttribute.IsTop).AppendLine();
            }
            else if (tableType == EAuxiliaryTableType.GovPublicContent)
            {
                sqlBuilder.AppendFormat(@"[{0}] NUMBER(38) DEFAULT 0 NOT NULL ,", GovPublicContentAttribute.DepartmentID).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] NUMBER(38) DEFAULT 0 NOT NULL ,", GovPublicContentAttribute.Category1ID).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] NUMBER(38) DEFAULT 0 NOT NULL ,", GovPublicContentAttribute.Category2ID).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] NUMBER(38) DEFAULT 0 NOT NULL ,", GovPublicContentAttribute.Category3ID).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] NUMBER(38) DEFAULT 0 NOT NULL ,", GovPublicContentAttribute.Category4ID).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] NUMBER(38) DEFAULT 0 NOT NULL ,", GovPublicContentAttribute.Category5ID).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] NUMBER(38) DEFAULT 0 NOT NULL ,", GovPublicContentAttribute.Category6ID).AppendLine();
            }
            else if (tableType == EAuxiliaryTableType.GovInteractContent)
            {
                sqlBuilder.AppendFormat(@"[{0}] NVARCHAR2(255) DEFAULT '' ,", GovInteractContentAttribute.DepartmentName).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] NVARCHAR2(255) DEFAULT '' ,", GovInteractContentAttribute.QueryCode).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] VARCHAR2(50) DEFAULT '' ,", GovInteractContentAttribute.State).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] VARCHAR2(50) DEFAULT '' ,", GovInteractContentAttribute.IPAddress).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] NVARCHAR2(50) DEFAULT '' ,", GovInteractContentAttribute.Location).AppendLine();

                sqlBuilder.AppendFormat(@"[{0}] VARCHAR2(18) DEFAULT '' ,", GovInteractContentAttribute.IsRecommend).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] VARCHAR2(18) DEFAULT '' ,", ContentAttribute.IsTop).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] TIMESTAMP(6) DEFAULT sysdate NOT NULL ,", ContentAttribute.AddDate).AppendLine();
            }
            else if (tableType == EAuxiliaryTableType.GoodsContent)
            {
                sqlBuilder.AppendFormat(@"[{0}] NUMBER(38) DEFAULT 0 NOT NULL ,", GoodsContentAttribute.BrandID).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] NUMBER(38) DEFAULT 0 NOT NULL ,", GoodsContentAttribute.Sales).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] NVARCHAR2(200) DEFAULT '' ,", GoodsContentAttribute.SpecIDCollection).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] NVARCHAR2(200) DEFAULT '' ,", GoodsContentAttribute.SpecItemIDCollection).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] NUMBER(38) DEFAULT 0 NOT NULL ,", GoodsContentAttribute.SKUCount).AppendLine();
            }

            //添加后台定义字段
            if (columnSqlStringArrayList != null)
            {
                foreach (string sqlString in columnSqlStringArrayList)
                {
                    sqlBuilder.Append(sqlString).Append(@" ,
");
                }
            }
            //添加主键及索引
            sqlBuilder.AppendFormat(@"CONSTRAINT [PK_{0}] PRIMARY KEY ([ID])
)
go
CREATE INDEX [IX_{0}] ON [{0}](IsTop DESC, Taxis DESC, ID DESC)
go
CREATE INDEX [IX{0}_Taxis] ON [{0}](Taxis DESC)
go", tableName);

            sqlBuilder.AppendFormat(@"
CREATE SEQUENCE {0}_SEQ
    START WITH 1
    INCREMENT BY 1
    NOMINVALUE
    NOMAXVALUE
    NOCACHE
    ORDER
go
", tableName);

            return SqlUtils.ParseSqlString(sqlBuilder.ToString());
        }
	}
}
