using System;
using System.Text;
using System.Data;
using System.Collections;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;

namespace BaiRong.Provider.Data.SqlServer
{
    public class TableStyleDAO : DataProviderBase, ITableStyleDAO
    {
        // Static constants
        private const string SQL_SELECT_TABLE_STYLE = "SELECT TableStyleID, RelatedIdentity, TableName, AttributeName, Taxis, DisplayName, HelpText, IsVisible, IsVisibleInList, IsSingleLine, InputType, DefaultValue, IsHorizontal, ExtendValues FROM bairong_TableStyle WHERE RelatedIdentity = @RelatedIdentity AND TableName = @TableName AND AttributeName = @AttributeName";

        private const string SQL_SELECT_TABLE_STYLE_BY_TABLE_STYLE_ID = "SELECT TableStyleID, RelatedIdentity, TableName, AttributeName, Taxis, DisplayName, HelpText, IsVisible, IsVisibleInList, IsSingleLine, InputType, DefaultValue, IsHorizontal, ExtendValues FROM bairong_TableStyle WHERE TableStyleID = @TableStyleID";

        private const string SQL_SELECT_TABLE_STYLES = "SELECT TableStyleID, RelatedIdentity, TableName, AttributeName, Taxis, DisplayName, HelpText, IsVisible, IsVisibleInList, IsSingleLine, InputType, DefaultValue, IsHorizontal, ExtendValues FROM bairong_TableStyle WHERE TableName = @TableName AND AttributeName = @AttributeName ORDER BY RelatedIdentity";

        private const string SQL_SELECT_ALL_TABLE_STYLE = "SELECT TableStyleID, RelatedIdentity, TableName, AttributeName, Taxis, DisplayName, HelpText, IsVisible, IsVisibleInList, IsSingleLine, InputType, DefaultValue, IsHorizontal, ExtendValues FROM bairong_TableStyle WHERE TableName <> '' AND AttributeName <> '' ORDER BY Taxis DESC, TableStyleID DESC";

        private const string SQL_UPDATE_TABLE_STYLE = "UPDATE bairong_TableStyle SET AttributeName = @AttributeName, Taxis = @Taxis, DisplayName = @DisplayName, HelpText = @HelpText, IsVisible = @IsVisible, IsVisibleInList = @IsVisibleInList, IsSingleLine = @IsSingleLine, InputType = @InputType, DefaultValue = @DefaultValue, IsHorizontal = @IsHorizontal, ExtendValues = @ExtendValues WHERE TableStyleID = @TableStyleID";

        private const string SQL_DELETE_TABLE_STYLE = "DELETE FROM bairong_TableStyle WHERE RelatedIdentity = @RelatedIdentity AND TableName = @TableName AND AttributeName = @AttributeName";

        private const string SQL_SELECT_TABLE_STYLE_TAXIS = "SELECT Taxis FROM bairong_TableStyle WHERE TableStyleID = @TableStyleID";

        private const string SQL_UPDATE_TABLE_STYLE_TAXIS = "UPDATE bairong_TableStyle SET Taxis = @Taxis WHERE TableStyleID = @TableStyleID";

        //AuxiliaryTableStyleItemInfo
        private const string SQL_SELECT_ALL_STYLE_ITEM = "SELECT TableStyleItemID, TableStyleID, ItemTitle, ItemValue, IsSelected FROM bairong_TableStyleItem WHERE (TableStyleID = @TableStyleID)";

        private const string SQL_DELETE_STYLE_ITEMS = "DELETE FROM bairong_TableStyleItem WHERE TableStyleID = @TableStyleID";

        private const string PARM_TABLE_STYLE_ID = "@TableStyleID";
        private const string PARM_RELATED_IDENTITY = "@RelatedIdentity";
        private const string PARM_TABLE_NAME = "@TableName";
        private const string PARM_ATTRIBUTE_NAME = "@AttributeName";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_DISPLAY_NAME = "@DisplayName";
        private const string PARM_HELP_TEXT = "@HelpText";
        private const string PARM_IS_VISIBLE = "@IsVisible";
        private const string PARM_IS_VISIBLE_IN_LIST = "@IsVisibleInList";
        private const string PARM_IS_SINGLE_LINE = "@IsSingleLine";
        private const string PARM_INPUT_TYPE = "@InputType";
        private const string PARM_DEFAULT_VALUE = "@DefaultValue";
        private const string PARM_IS_HORIZONTAL = "@IsHorizontal";
        private const string PARM_EXTEND_VALUES = "@ExtendValues";

        //AuxiliaryTableStyleItemInfo
        private const string PARM_TABLE_STYLE_ITEM_ID = "@TableStyleItemID";
        private const string PARM_ITEM_TITLE = "@ItemTitle";
        private const string PARM_ITEM_VALUE = "@ItemValue";
        private const string PARM_IS_SELECTED = "@IsSelected";

        public int Insert(TableStyleInfo styleInfo, ETableStyle tableStyle)
        {
            return Insert(styleInfo, tableStyle, false);
        }

        public int InsertWithTaxis(TableStyleInfo styleInfo, ETableStyle tableStyle)
        {
            return Insert(styleInfo, tableStyle, true);
        }

        private string GetInsertTableStyleSqlString()
        {
            string SQL_INSERT_TABLE_STYLE = "INSERT INTO bairong_TableStyle (RelatedIdentity, TableName, AttributeName, Taxis, DisplayName, HelpText, IsVisible, IsVisibleInList, IsSingleLine, InputType, DefaultValue, IsHorizontal, ExtendValues) VALUES (@RelatedIdentity, @TableName, @AttributeName, @Taxis, @DisplayName, @HelpText, @IsVisible, @IsVisibleInList, @IsSingleLine, @InputType, @DefaultValue, @IsHorizontal, @ExtendValues)";

            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT_TABLE_STYLE = "INSERT INTO bairong_TableStyle (TableStyleID, RelatedIdentity, TableName, AttributeName, Taxis, DisplayName, HelpText, IsVisible, IsVisibleInList, IsSingleLine, InputType, DefaultValue, IsHorizontal, ExtendValues) VALUES (bairong_TableStyle_SEQ.NEXTVAL, @RelatedIdentity, @TableName, @AttributeName, @Taxis, @DisplayName, @HelpText, @IsVisible, @IsVisibleInList, @IsSingleLine, @InputType, @DefaultValue, @IsHorizontal, @ExtendValues)";
            }

            return SQL_INSERT_TABLE_STYLE;
        }

        private string GetInsertTableStyleItemSqlString()
        {
            string SQL_INSERT_STYLE_ITEM = "INSERT INTO bairong_TableStyleItem (TableStyleID, ItemTitle, ItemValue, IsSelected) VALUES (@TableStyleID, @ItemTitle, @ItemValue, @IsSelected)";

            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT_STYLE_ITEM = "INSERT INTO bairong_TableStyleItem (TableStyleItemID, TableStyleID, ItemTitle, ItemValue, IsSelected) VALUES (bairong_TableStyleItem_SEQ.NEXTVAL, @TableStyleID, @ItemTitle, @ItemValue, @IsSelected)";
            }

            return SQL_INSERT_STYLE_ITEM;
        }

        private int Insert(TableStyleInfo styleInfo, ETableStyle tableStyle, bool isWithTaxis)
        {
            int tableStyleID = 0;

            if (!isWithTaxis)
            {
                styleInfo.Taxis = GetNewStyleInfoTaxis(tableStyle, styleInfo.AttributeName, styleInfo.RelatedIdentity, styleInfo.TableName);
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_RELATED_IDENTITY, EDataType.Integer, styleInfo.RelatedIdentity),
                this.GetParameter(PARM_TABLE_NAME, EDataType.VarChar, 50, styleInfo.TableName),
				this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, styleInfo.AttributeName),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, styleInfo.Taxis),
                this.GetParameter(PARM_DISPLAY_NAME, EDataType.NVarChar, 255, styleInfo.DisplayName),
                this.GetParameter(PARM_HELP_TEXT, EDataType.VarChar, 255, styleInfo.HelpText),
                this.GetParameter(PARM_IS_VISIBLE, EDataType.VarChar, 18, styleInfo.IsVisible.ToString()),
                this.GetParameter(PARM_IS_VISIBLE_IN_LIST, EDataType.VarChar, 18, styleInfo.IsVisibleInList.ToString()),
                this.GetParameter(PARM_IS_SINGLE_LINE, EDataType.VarChar, 18, styleInfo.IsSingleLine.ToString()),
				this.GetParameter(PARM_INPUT_TYPE, EDataType.VarChar, 50, EInputTypeUtils.GetValue(styleInfo.InputType)),
                this.GetParameter(PARM_DEFAULT_VALUE, EDataType.VarChar, 255, styleInfo.DefaultValue),
                this.GetParameter(PARM_IS_HORIZONTAL, EDataType.VarChar, 18, styleInfo.IsHorizontal.ToString()),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, styleInfo.Additional.ToString())
			};

            string SQL_INSERT_TABLE_STYLE = this.GetInsertTableStyleSqlString();
            string SQL_INSERT_STYLE_ITEM = this.GetInsertTableStyleItemSqlString();

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT_TABLE_STYLE, insertParms);

                        tableStyleID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bairong_TableStyle");

                        //string SELECT_CMD = "SELECT @@IDENTITY AS 'TableStyleID'";
                        //using (IDataReader rdr = this.ExecuteReader(trans, SELECT_CMD))
                        //{
                        //    if (rdr.Read())
                        //    {
                        //        tableStyleID = Convert.ToInt32(rdr[0].ToString());
                        //    }
                        //    else
                        //    {
                        //        throw new DataException();
                        //    }
                        //    rdr.Close();
                        //}

                        if (styleInfo.StyleItems != null && styleInfo.StyleItems.Count > 0)
                        {
                            foreach (TableStyleItemInfo itemInfo in styleInfo.StyleItems)
                            {
                                IDbDataParameter[] insertItemParms = new IDbDataParameter[]
							    {
								    this.GetParameter(PARM_TABLE_STYLE_ID, EDataType.Integer, tableStyleID),
								    this.GetParameter(PARM_ITEM_TITLE, EDataType.NVarChar, 255, itemInfo.ItemTitle),
								    this.GetParameter(PARM_ITEM_VALUE, EDataType.VarChar, 255, itemInfo.ItemValue),
								    this.GetParameter(PARM_IS_SELECTED, EDataType.VarChar, 18, itemInfo.IsSelected.ToString())
							    };

                                this.ExecuteNonQuery(trans, SQL_INSERT_STYLE_ITEM, insertItemParms);

                            }
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

            return tableStyleID;
        }

        private int GetNewStyleInfoTaxis(ETableStyle tableStyle, string attributeName, int relatedIdentity, string tableName)
        {
            int taxis = 0;
            if (!TableStyleManager.IsMetadata(tableStyle, attributeName))
            {
                int maxTaxis = this.GetMaxTaxisByKeyStart(relatedIdentity, tableName);
                taxis = maxTaxis + 1;
            }
            return taxis;
        }

        public void InsertWithTransaction(TableStyleInfo styleInfo, ETableStyle tableStyle, IDbTransaction trans)
        {
            styleInfo.Taxis = GetNewStyleInfoTaxis(tableStyle, styleInfo.AttributeName, styleInfo.RelatedIdentity, styleInfo.TableName);

            IDbDataParameter[] insertParms = new IDbDataParameter[]
		    {
                this.GetParameter(PARM_RELATED_IDENTITY, EDataType.Integer, styleInfo.RelatedIdentity),
                this.GetParameter(PARM_TABLE_NAME, EDataType.VarChar, 50, styleInfo.TableName),
			    this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, styleInfo.AttributeName),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, styleInfo.Taxis),
                this.GetParameter(PARM_DISPLAY_NAME, EDataType.NVarChar, 255, styleInfo.DisplayName),
                this.GetParameter(PARM_HELP_TEXT, EDataType.VarChar, 255, styleInfo.HelpText),
                this.GetParameter(PARM_IS_VISIBLE, EDataType.VarChar, 18, styleInfo.IsVisible.ToString()),
                this.GetParameter(PARM_IS_VISIBLE_IN_LIST, EDataType.VarChar, 18, styleInfo.IsVisibleInList.ToString()),
                this.GetParameter(PARM_IS_SINGLE_LINE, EDataType.VarChar, 18, styleInfo.IsSingleLine.ToString()),
			    this.GetParameter(PARM_INPUT_TYPE, EDataType.VarChar, 50, EInputTypeUtils.GetValue(styleInfo.InputType)),
                this.GetParameter(PARM_DEFAULT_VALUE, EDataType.VarChar, 255, styleInfo.DefaultValue),
                this.GetParameter(PARM_IS_HORIZONTAL, EDataType.VarChar, 18, styleInfo.IsHorizontal.ToString()),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, styleInfo.Additional.ToString())
		    };

            string SQL_INSERT_TABLE_STYLE = this.GetInsertTableStyleSqlString();
            string SQL_INSERT_STYLE_ITEM = this.GetInsertTableStyleItemSqlString();

            if (styleInfo.StyleItems == null || styleInfo.StyleItems.Count == 0)
            {
                this.ExecuteNonQuery(trans, SQL_INSERT_TABLE_STYLE, insertParms);
            }
            else
            {
                int tableStyleID = 0;

                this.ExecuteNonQuery(trans, SQL_INSERT_TABLE_STYLE, insertParms);

                tableStyleID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "bairong_TableStyle");

                //string SELECT_CMD = "SELECT @@IDENTITY AS 'TableStyleID'";
                //using (IDataReader rdr = this.ExecuteReader(trans, SELECT_CMD))
                //{
                //    if (rdr.Read())
                //    {
                //        tableStyleID = Convert.ToInt32(rdr[0].ToString());
                //    }
                //    else
                //    {
                //        throw new DataException();
                //    }
                //    rdr.Close();
                //}

                foreach (TableStyleItemInfo itemInfo in styleInfo.StyleItems)
                {
                    IDbDataParameter[] insertItemParms = new IDbDataParameter[]
				    {
					    this.GetParameter(PARM_TABLE_STYLE_ID, EDataType.Integer, tableStyleID),
					    this.GetParameter(PARM_ITEM_TITLE, EDataType.NVarChar, 255, itemInfo.ItemTitle),
					    this.GetParameter(PARM_ITEM_VALUE, EDataType.VarChar, 255, itemInfo.ItemValue),
					    this.GetParameter(PARM_IS_SELECTED, EDataType.VarChar, 18, itemInfo.IsSelected.ToString())
				    };

                    this.ExecuteNonQuery(trans, SQL_INSERT_STYLE_ITEM, insertItemParms);

                }
            }
        }

        public void InsertStyleItems(ArrayList styleItems)
        {
            string SQL_INSERT_STYLE_ITEM = this.GetInsertTableStyleItemSqlString();

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {

                        foreach (TableStyleItemInfo itemInfo in styleItems)
                        {
                            IDbDataParameter[] insertItemParms = new IDbDataParameter[]
							{
								this.GetParameter(PARM_TABLE_STYLE_ID, EDataType.Integer, itemInfo.TableStyleID),
								this.GetParameter(PARM_ITEM_TITLE, EDataType.NVarChar, 255, itemInfo.ItemTitle),
								this.GetParameter(PARM_ITEM_VALUE, EDataType.VarChar, 255, itemInfo.ItemValue),
								this.GetParameter(PARM_IS_SELECTED, EDataType.VarChar, 18, itemInfo.IsSelected.ToString())
							};

                            this.ExecuteNonQuery(trans, SQL_INSERT_STYLE_ITEM, insertItemParms);

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

        public void DeleteStyleItems(int tableStyleID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_STYLE_ID, EDataType.Integer, tableStyleID)
			};

            this.ExecuteNonQuery(SQL_DELETE_STYLE_ITEMS, parms);
        }

        public ArrayList GetStyleItemArrayList(int tableStyleID)
        {
            ArrayList styleItems = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_STYLE_ID, EDataType.Integer, tableStyleID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_STYLE_ITEM, parms))
            {
                while (rdr.Read())
                {
                    TableStyleItemInfo info = new TableStyleItemInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), TranslateUtils.ToBool(rdr.GetValue(4).ToString()));
                    styleItems.Add(info);
                }
                rdr.Close();
            }
            return styleItems;
        }

        public void Update(TableStyleInfo info)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, info.AttributeName),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, info.Taxis),
                this.GetParameter(PARM_DISPLAY_NAME, EDataType.NVarChar, 255, info.DisplayName),
                this.GetParameter(PARM_HELP_TEXT, EDataType.VarChar, 255, info.HelpText),
                this.GetParameter(PARM_IS_VISIBLE, EDataType.VarChar, 18, info.IsVisible.ToString()),
	            this.GetParameter(PARM_IS_VISIBLE_IN_LIST, EDataType.VarChar, 18, info.IsVisibleInList.ToString()),
                this.GetParameter(PARM_IS_SINGLE_LINE, EDataType.VarChar, 18, info.IsSingleLine.ToString()),
				this.GetParameter(PARM_INPUT_TYPE, EDataType.VarChar, 50, EInputTypeUtils.GetValue(info.InputType)),
                this.GetParameter(PARM_DEFAULT_VALUE, EDataType.VarChar, 255, info.DefaultValue),
                this.GetParameter(PARM_IS_HORIZONTAL, EDataType.VarChar, 18, info.IsHorizontal.ToString()),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, info.Additional.ToString()),
                this.GetParameter(PARM_TABLE_STYLE_ID, EDataType.Integer, info.TableStyleID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_TABLE_STYLE, updateParms);
        }

        public void Delete(int relatedIdentity, string tableName, string attributeName)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_RELATED_IDENTITY, EDataType.Integer, relatedIdentity),
                this.GetParameter(PARM_TABLE_NAME, EDataType.VarChar, 50, tableName),
                this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, attributeName)
            };

            this.ExecuteNonQuery(SQL_DELETE_TABLE_STYLE, parms);
            TableStyleManager.IsChanged = true;
        }

        public void Delete(ArrayList relatedIdentities, string tableName)
        {
            if (relatedIdentities != null && relatedIdentities.Count > 0)
            {
                string sqlString = string.Format("DELETE FROM bairong_TableStyle WHERE RelatedIdentity IN ({0}) AND TableName = '{1}'", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(relatedIdentities), PageUtils.FilterSql(tableName));
                this.ExecuteNonQuery(sqlString);
                TableStyleManager.IsChanged = true;
            }
        }

        public ArrayList GetTableStyleInfoArrayList(ArrayList relatedIdentities, string tableName)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT TableStyleID, RelatedIdentity, TableName, AttributeName, Taxis, DisplayName, HelpText, IsVisible, IsVisibleInList, IsSingleLine, InputType, DefaultValue, IsHorizontal, ExtendValues FROM bairong_TableStyle WHERE RelatedIdentity IN ({0}) AND TableName = '{1}' ORDER BY TableStyleID DESC", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(relatedIdentities), PageUtils.FilterSql(tableName));

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    TableStyleInfo styleInfo = GetTableStyleInfoByReader(rdr);
                    arraylist.Add(styleInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public bool IsExists(int relatedIdentity, string tableName, string attributeName)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_RELATED_IDENTITY, EDataType.Integer, relatedIdentity),
                this.GetParameter(PARM_TABLE_NAME, EDataType.VarChar, 50, tableName),
				this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, attributeName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_STYLE, parms))
            {
                if (rdr.Read())
                {
                    exists = true;
                }
                rdr.Close();
            }

            return exists;
        }

        public TableStyleInfo GetTableStyleInfo(int tableStyleID)
        {
            TableStyleInfo styleInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_TABLE_STYLE_ID, EDataType.Integer, tableStyleID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_STYLE_BY_TABLE_STYLE_ID, parms))
            {
                if (rdr.Read())
                {
                    styleInfo = GetTableStyleInfoByReader(rdr);
                }
                rdr.Close();
            }

            return styleInfo;
        }

        public TableStyleInfo GetTableStyleInfo(int relatedIdentity, string tableName, string attributeName)
        {
            TableStyleInfo styleInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_RELATED_IDENTITY, EDataType.Integer, relatedIdentity),
                this.GetParameter(PARM_TABLE_NAME, EDataType.VarChar, 50, tableName),
				this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, attributeName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_STYLE, parms))
            {
                if (rdr.Read())
                {
                    styleInfo = GetTableStyleInfoByReader(rdr);
                }
                rdr.Close();
            }

            return styleInfo;
        }

        private TableStyleInfo GetTableStyleInfoByReader(IDataReader rdr)
        {
            int tableStyleID = Convert.ToInt32(rdr["TableStyleID"]);
            int relatedIdentity = Convert.ToInt32(rdr["RelatedIdentity"]);
            string tableName = rdr["TableName"].ToString();
            string attributeName = rdr["AttributeName"].ToString();
            int taxis = Convert.ToInt32(rdr["Taxis"]);
            string displayName = rdr["DisplayName"].ToString();
            string helpText = rdr["HelpText"].ToString();
            bool isVisible = TranslateUtils.ToBool(rdr["IsVisible"].ToString());
            bool isVisibleInList = TranslateUtils.ToBool(rdr["IsVisibleInList"].ToString());
            bool isSingleLine = TranslateUtils.ToBool(rdr["IsSingleLine"].ToString());
            EInputType inputType = EInputTypeUtils.GetEnumType(rdr["InputType"].ToString());
            string defaultValue = rdr["DefaultValue"].ToString();
            bool isHorizontal = TranslateUtils.ToBool(rdr["IsHorizontal"].ToString());
            string extendValues = rdr["ExtendValues"].ToString();

            TableStyleInfo styleInfo = new TableStyleInfo(tableStyleID, relatedIdentity, tableName, attributeName, taxis, displayName, helpText, isVisible, isVisibleInList, isSingleLine, inputType, defaultValue, isHorizontal, extendValues);

            return styleInfo;
        }

        public PairArrayList GetAllTableStyleInfoPairs()
        {
            PairArrayList pairs = new PairArrayList();

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_TABLE_STYLE))
            {
                while (rdr.Read())
                {
                    TableStyleInfo styleInfo = GetTableStyleInfoByReader(rdr);
                    string key = TableStyleManager.GetCacheKey(styleInfo.RelatedIdentity, styleInfo.TableName, styleInfo.AttributeName);
                    if (!pairs.Keys.Contains(key))
                    {
                        Pair pair = new Pair(key, styleInfo);
                        pairs.Add(pair);
                    }
                }
                rdr.Close();
            }

            return pairs;
        }

        public ArrayList GetTableStyleInfoWithItemsArrayList(string tableName, string attributeName)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TABLE_NAME, EDataType.VarChar, 50, tableName),
                this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, attributeName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_STYLES, parms))
            {
                while (rdr.Read())
                {
                    TableStyleInfo styleInfo = GetTableStyleInfoByReader(rdr);
                    if (styleInfo.InputType == EInputType.CheckBox || styleInfo.InputType == EInputType.Radio || styleInfo.InputType == EInputType.SelectMultiple || styleInfo.InputType == EInputType.SelectOne)
                    {
                        ArrayList styleItems = GetStyleItemArrayList(styleInfo.TableStyleID);
                        if (styleItems != null && styleItems.Count > 0)
                        {
                            styleInfo.StyleItems = styleItems;
                        }
                    }
                    arraylist.Add(styleInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        private int GetMaxTaxisByKeyStart(int relatedIdentity, string tableName)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) AS MaxTaxis FROM bairong_TableStyle WHERE RelatedIdentity = {0} AND TableName = '{1}'", relatedIdentity, PageUtils.FilterSql(tableName));
            int maxTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        maxTaxis = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return maxTaxis;
        }

        public void TaxisUp(int tableStyleID)
        {
            TableStyleInfo styleInfo = this.GetTableStyleInfo(tableStyleID);
            if (styleInfo != null)
            {
                string sqlString = "SELECT TOP 1 TableStyleID, Taxis FROM bairong_TableStyle WHERE RelatedIdentity = @RelatedIdentity AND TableName = @TableName AND Taxis > (SELECT Taxis FROM bairong_TableStyle WHERE TableStyleID = @TableStyleID) ORDER BY Taxis";
                int higherID = 0;
                int higherTaxis = 0;

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
                    this.GetParameter(PARM_RELATED_IDENTITY, EDataType.Integer, styleInfo.RelatedIdentity),
                    this.GetParameter(PARM_TABLE_NAME, EDataType.VarChar, 50, styleInfo.TableName),
				    this.GetParameter(PARM_TABLE_STYLE_ID, EDataType.Integer, tableStyleID)
			    };

                using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
                {
                    if (rdr.Read())
                    {
                        higherID = Convert.ToInt32(rdr[0]);
                        higherTaxis = Convert.ToInt32(rdr[1]);
                    }
                    rdr.Close();
                }

                if (higherID != 0)
                {
                    this.SetTaxis(tableStyleID, higherTaxis);
                    this.SetTaxis(higherID, styleInfo.Taxis);
                }
            }
        }

        public void TaxisDown(int tableStyleID)
        {
            TableStyleInfo styleInfo = this.GetTableStyleInfo(tableStyleID);
            if (styleInfo != null)
            {
                string sqlString = "SELECT TOP 1 TableStyleID, Taxis FROM bairong_TableStyle WHERE RelatedIdentity = @RelatedIdentity AND TableName = @TableName AND Taxis < (SELECT Taxis FROM bairong_TableStyle WHERE TableStyleID = @TableStyleID) ORDER BY Taxis DESC";
                int lowerID = 0;
                int lowerTaxis = 0;

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
                    this.GetParameter(PARM_RELATED_IDENTITY, EDataType.Integer, styleInfo.RelatedIdentity),
                    this.GetParameter(PARM_TABLE_NAME, EDataType.VarChar, 50, styleInfo.TableName),
				    this.GetParameter(PARM_TABLE_STYLE_ID, EDataType.Integer, tableStyleID)
			    };

                using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
                {
                    if (rdr.Read())
                    {
                        lowerID = Convert.ToInt32(rdr[0]);
                        lowerTaxis = Convert.ToInt32(rdr[1]);
                    }
                    rdr.Close();
                }

                if (lowerID != 0)
                {
                    this.SetTaxis(tableStyleID, lowerTaxis);
                    this.SetTaxis(lowerID, styleInfo.Taxis);
                }
            }
        }

        private void SetTaxis(int tableStyleID, int taxis)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis),
				this.GetParameter(PARM_TABLE_STYLE_ID, EDataType.Integer, tableStyleID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_TABLE_STYLE_TAXIS, parms);
            TableStyleManager.IsChanged = true;
            TableStyleManager.ClearAPI();
        }


        #region by 20160228  sofuny 功能管理增加的功能类型的字段（评价管理）

        /// <summary>
        /// 获取功能表的字段
        /// </summary>
        /// <param name="tableStyle"></param>
        /// <param name="relatedIdentity"></param>
        /// <returns></returns>
        public ArrayList GetFunctionTableStyle(string tableName, int relatedIdentity, int publishmentSystemID, int contentID, string tableStyle)
        {
            ArrayList list = new ArrayList();

            string sql = string.Format(" select * from bairong_TableStyle where TableName='{0}' and RelatedIdentity={1} and TableStyleID in (select TableStyleID from dbo.siteserver_FunctionTableStyles where PublishmentSystemID={2} and NodeID={1} and ContentID={3} and TableStyle='{4}')", tableName, relatedIdentity, publishmentSystemID, contentID, tableStyle);

            using (IDataReader rdr = this.ExecuteReader(sql))
            {
                while (rdr.Read())
                {
                    TableStyleInfo styleInfo = GetTableStyleInfoByReader(rdr);
                    if (styleInfo.InputType == EInputType.CheckBox || styleInfo.InputType == EInputType.Radio || styleInfo.InputType == EInputType.SelectMultiple || styleInfo.InputType == EInputType.SelectOne)
                    {
                        ArrayList styleItems = GetStyleItemArrayList(styleInfo.TableStyleID);
                        if (styleItems != null && styleItems.Count > 0)
                        {
                            styleInfo.StyleItems = styleItems;
                        }
                    }
                    list.Add(styleInfo);
                }
                rdr.Close();
            }

            return list;
        }
        #endregion
    }
}
