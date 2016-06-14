using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.Data;
using BaiRong.Core.AuxiliaryTable;

namespace BaiRong.Provider.Data.SqlServer
{
    public class AuxiliaryTableDataDAO : DataProviderBase, IAuxiliaryTableDataDAO
    {
        public virtual string GetCreateAuxiliaryTableSqlString(string tableName)
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
[ID] [int] IDENTITY (1, 1) NOT NULL ,
[NodeID] [int] NOT NULL DEFAULT (0) ,
[PublishmentSystemID] [int] NOT NULL DEFAULT (0) ,
[AddUserName] [nvarchar] (255) NOT NULL DEFAULT ('') ,
[LastEditUserName] [nvarchar] (255) NOT NULL DEFAULT ('') ,
[LastEditDate] [datetime] NOT NULL DEFAULT (getdate()) ,
[Taxis] [int] NOT NULL DEFAULT (0) ,
[ContentGroupNameCollection] [nvarchar] (255) NOT NULL DEFAULT ('') ,
[Tags] [nvarchar] (255) NOT NULL DEFAULT ('') ,
[SourceID] [int] NOT NULL DEFAULT (0) ,
[ReferenceID] [int] NOT NULL DEFAULT (0) ,
[IsChecked] [varchar] (18) NOT NULL DEFAULT ('') ,
[CheckedLevel] [int] NOT NULL DEFAULT (0) ,
[Comments] [int] NOT NULL DEFAULT (0) ,
[Photos] [int] NOT NULL DEFAULT (0) ,
[Teleplays] [int] NOT NULL DEFAULT (0) ,
[Hits] [int] NOT NULL DEFAULT (0) ,
[HitsByDay] [int] NOT NULL DEFAULT (0) ,
[HitsByWeek] [int] NOT NULL DEFAULT (0) ,
[HitsByMonth] [int] NOT NULL DEFAULT (0) ,
[LastHitsDate] [datetime] NOT NULL DEFAULT (getdate()) ,
[SettingsXML] [ntext] NOT NULL DEFAULT '',
");
            if (tableType == EAuxiliaryTableType.BackgroundContent)
            {
                sqlBuilder.AppendFormat(@"[{0}] [nvarchar] (50) NOT NULL DEFAULT ('') ,", ContentAttribute.MemberName).AppendLine();
            }
            else if (tableType == EAuxiliaryTableType.VoteContent)
            {
                sqlBuilder.AppendFormat(@"[{0}] [varchar] (18) NOT NULL DEFAULT ('') ,", VoteContentAttribute.IsImageVote).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [varchar] (18) NOT NULL DEFAULT ('') ,", VoteContentAttribute.IsSummary).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [int] NOT NULL DEFAULT (0) ,", VoteContentAttribute.Participants).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [varchar] (18) NOT NULL DEFAULT ('') ,", VoteContentAttribute.IsClosed).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [varchar] (18) NOT NULL DEFAULT ('') ,", VoteContentAttribute.IsTop).AppendLine();
            }
            else if (tableType == EAuxiliaryTableType.GovPublicContent)
            {
                sqlBuilder.AppendFormat(@"[{0}] [int] NOT NULL DEFAULT (0) ,", GovPublicContentAttribute.DepartmentID).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [int] NOT NULL DEFAULT (0) ,", GovPublicContentAttribute.Category1ID).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [int] NOT NULL DEFAULT (0) ,", GovPublicContentAttribute.Category2ID).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [int] NOT NULL DEFAULT (0) ,", GovPublicContentAttribute.Category3ID).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [int] NOT NULL DEFAULT (0) ,", GovPublicContentAttribute.Category4ID).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [int] NOT NULL DEFAULT (0) ,", GovPublicContentAttribute.Category5ID).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [int] NOT NULL DEFAULT (0) ,", GovPublicContentAttribute.Category6ID).AppendLine();
            }
            else if (tableType == EAuxiliaryTableType.GovInteractContent)
            {
                sqlBuilder.AppendFormat(@"[{0}] [nvarchar] (255) NOT NULL DEFAULT ('') ,", GovInteractContentAttribute.DepartmentName).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [nvarchar] (255) NOT NULL DEFAULT ('') ,", GovInteractContentAttribute.QueryCode).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [varchar] (50) NOT NULL DEFAULT ('') ,", GovInteractContentAttribute.State).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [varchar] (50) NOT NULL DEFAULT ('') ,", GovInteractContentAttribute.IPAddress).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [nvarchar] (50) NOT NULL DEFAULT ('') ,", GovInteractContentAttribute.Location).AppendLine();

                sqlBuilder.AppendFormat(@"[{0}] [varchar] (18) NOT NULL DEFAULT ('') ,", GovInteractContentAttribute.IsRecommend).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [varchar] (18) NOT NULL DEFAULT ('') ,", ContentAttribute.IsTop).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [datetime] NOT NULL DEFAULT (getdate()) ,", ContentAttribute.AddDate).AppendLine();
            }
            else if (tableType == EAuxiliaryTableType.GoodsContent)
            {
                sqlBuilder.AppendFormat(@"[{0}] [int] NOT NULL DEFAULT (0) ,", GoodsContentAttribute.BrandID).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [nvarchar] (200) NOT NULL DEFAULT ('') ,", GoodsContentAttribute.BrandValue).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [int] NOT NULL DEFAULT (0) ,", GoodsContentAttribute.Sales).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [varchar] (200) NOT NULL DEFAULT ('') ,", GoodsContentAttribute.SpecIDCollection).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [varchar] (200) NOT NULL DEFAULT ('') ,", GoodsContentAttribute.SpecItemIDCollection).AppendLine();
                sqlBuilder.AppendFormat(@"[{0}] [int] NOT NULL DEFAULT (0) ,", GoodsContentAttribute.SKUCount).AppendLine();
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
            sqlBuilder.AppendFormat(@"CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED ([ID])
)
go
CREATE INDEX [IX_{0}] ON [{0}](IsTop DESC, Taxis DESC, ID DESC)
go
CREATE INDEX [IX_Taxis] ON [{0}](Taxis DESC)
go", tableName);

            return sqlBuilder.ToString();
        }

        public ArrayList GetDefaultTableMetadataInfoArrayList(string tableName, EAuxiliaryTableType tableType)
        {
            ArrayList arraylist = new ArrayList();
            Hashtable hashtable = new Hashtable();
            if (tableType == EAuxiliaryTableType.BackgroundContent)
            {
                TableMetadataInfo metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.Title, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.SubTitle, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.ImageUrl, EDataType.VarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.VideoUrl, EDataType.VarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.FileUrl, EDataType.VarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.LinkUrl, EDataType.NVarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.Content, EDataType.NText, 16, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NText), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.Summary, EDataType.NText, 16, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NText), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.Author, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.Source, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.IsRecommend, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.IsHot, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.IsColor, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.IsTop, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.AddDate, EDataType.DateTime, 8, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.CheckTaskDate, EDataType.DateTime, 8, true, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.UnCheckTaskDate, EDataType.DateTime, 8, true, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
            }
            else if (tableType == EAuxiliaryTableType.GovPublicContent)
            {
                TableMetadataInfo metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.Title, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovPublicContentAttribute.Identifier, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovPublicContentAttribute.Description, EDataType.NText, 16, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NText), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovPublicContentAttribute.PublishDate, EDataType.DateTime, 8, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovPublicContentAttribute.EffectDate, EDataType.DateTime, 8, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovPublicContentAttribute.IsAbolition, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovPublicContentAttribute.AbolitionDate, EDataType.DateTime, 8, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovPublicContentAttribute.DocumentNo, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovPublicContentAttribute.Publisher, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovPublicContentAttribute.Keywords, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovPublicContentAttribute.ImageUrl, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovPublicContentAttribute.FileUrl, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovPublicContentAttribute.IsRecommend, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovPublicContentAttribute.IsHot, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovPublicContentAttribute.IsColor, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.IsTop, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovPublicContentAttribute.Content, EDataType.NText, 16, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NText), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.AddDate, EDataType.DateTime, 8, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
            }
            else if (tableType == EAuxiliaryTableType.GovInteractContent)
            {
                TableMetadataInfo metadataInfo = new TableMetadataInfo(0, tableName, GovInteractContentAttribute.RealName, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovInteractContentAttribute.Organization, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovInteractContentAttribute.CardType, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovInteractContentAttribute.CardNo, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovInteractContentAttribute.Phone, EDataType.VarChar, 50, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovInteractContentAttribute.PostCode, EDataType.VarChar, 50, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovInteractContentAttribute.Address, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovInteractContentAttribute.Email, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovInteractContentAttribute.Fax, EDataType.VarChar, 50, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovInteractContentAttribute.TypeID, EDataType.Integer, 38, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.Integer), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovInteractContentAttribute.IsPublic, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovInteractContentAttribute.Title, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovInteractContentAttribute.Content, EDataType.NText, 16, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NText), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovInteractContentAttribute.FileUrl, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GovInteractContentAttribute.DepartmentID, EDataType.Integer, 38, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.Integer), 0, true);
                arraylist.Add(metadataInfo);
            }
            else if (tableType == EAuxiliaryTableType.VoteContent)
            {
                TableMetadataInfo metadataInfo = new TableMetadataInfo(0, tableName, VoteContentAttribute.Title, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, VoteContentAttribute.SubTitle, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, VoteContentAttribute.MaxSelectNum, EDataType.Integer, 38, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.Integer), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, VoteContentAttribute.ImageUrl, EDataType.VarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, VoteContentAttribute.Content, EDataType.NText, 16, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NText), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, VoteContentAttribute.Summary, EDataType.NText, 16, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NText), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, VoteContentAttribute.AddDate, EDataType.DateTime, 8, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, VoteContentAttribute.EndDate, EDataType.DateTime, 8, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, VoteContentAttribute.IsVotedView, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, VoteContentAttribute.HiddenContent, EDataType.NText, 16, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NText), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.CheckTaskDate, EDataType.DateTime, 8, true, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.UnCheckTaskDate, EDataType.DateTime, 8, true, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
            }
            else if (tableType == EAuxiliaryTableType.JobContent)
            {
                TableMetadataInfo metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.Title, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, JobContentAttribute.Department, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, JobContentAttribute.Location, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, JobContentAttribute.NumberOfPeople, EDataType.NVarChar, 50, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, JobContentAttribute.Responsibility, EDataType.NText, 16, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NText), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, JobContentAttribute.Requirement, EDataType.NText, 16, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NText), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, JobContentAttribute.IsUrgent, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.IsTop, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.AddDate, EDataType.DateTime, 8, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.CheckTaskDate, EDataType.DateTime, 8, true, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.UnCheckTaskDate, EDataType.DateTime, 8, true, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
            }
            else if (tableType == EAuxiliaryTableType.GoodsContent)
            {
                TableMetadataInfo metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.Title, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.SN, EDataType.NVarChar, 50, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.Keywords, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.Summary, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.ImageUrl, EDataType.VarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.ThumbUrl, EDataType.VarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.LinkUrl, EDataType.NVarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.Content, EDataType.NText, 16, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NText), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.PriceCost, EDataType.Decimal, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.Decimal), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.PriceMarket, EDataType.Decimal, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.Decimal), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.PriceSale, EDataType.Decimal, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.Decimal), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.Stock, EDataType.Integer, 38, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.Integer), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.FileUrl, EDataType.VarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.IsRecommend, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.IsNew, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.IsHot, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.IsTop, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.IsOnSale, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.AddDate, EDataType.DateTime, 8, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);

                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.F1, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.F2, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.F3, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.F4, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.F5, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.F6, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.F7, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.F8, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.F9, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                //metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.SpecIDCollection, EDataType.VarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                //arraylist.Add(metadataInfo);
                //metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.SpecItemIDCollection, EDataType.VarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                //arraylist.Add(metadataInfo);
                //metadataInfo = new TableMetadataInfo(0, tableName, GoodsContentAttribute.SKUCount, EDataType.Integer, 38, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.Integer), 0, true);
                //arraylist.Add(metadataInfo);

                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.CheckTaskDate, EDataType.DateTime, 8, true, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.UnCheckTaskDate, EDataType.DateTime, 8, true, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
            }
            else if (tableType == EAuxiliaryTableType.BrandContent)
            {
                TableMetadataInfo metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.Title, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BrandContentAttribute.BrandUrl, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BrandContentAttribute.ImageUrl, EDataType.VarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BrandContentAttribute.LinkUrl, EDataType.NVarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BrandContentAttribute.Content, EDataType.NText, 16, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NText), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BrandContentAttribute.IsRecommend, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.IsTop, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.AddDate, EDataType.DateTime, 8, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);

                metadataInfo = new TableMetadataInfo(0, tableName, BrandContentAttribute.F1, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BrandContentAttribute.F2, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BrandContentAttribute.F3, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
            }
            else if (tableType == EAuxiliaryTableType.UserDefined)
            {
                TableMetadataInfo metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.Title, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.IsTop, EDataType.VarChar, 18, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.AddDate, EDataType.DateTime, 8, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.DateTime), 0, true);
                arraylist.Add(metadataInfo);
            }
            #region 投稿默认字段
            else if (tableType == EAuxiliaryTableType.ManuscriptContent)
            {
                TableMetadataInfo metadataInfo = new TableMetadataInfo(0, tableName, ContentAttribute.Title, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.SubTitle, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.ImageUrl, EDataType.VarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.VideoUrl, EDataType.VarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.FileUrl, EDataType.VarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.LinkUrl, EDataType.NVarChar, 200, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.VarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.Content, EDataType.NText, 16, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NText), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.Summary, EDataType.NText, 16, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NText), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.Author, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo);
                metadataInfo = new TableMetadataInfo(0, tableName, BackgroundContentAttribute.Source, EDataType.NVarChar, 255, false, EDataTypeUtils.GetDefaultString(this.DataBaseType, EDataType.NVarChar), 0, true);
                arraylist.Add(metadataInfo); 
            }
            #endregion
            return arraylist;
        }
    }
}
