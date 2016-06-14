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
using SiteServer.CRM.Model;
using SiteServer.CRM.Core;

using System.Collections.Generic;

namespace SiteServer.CRM.Provider.Data.SqlServer
{
    public class FormElementDAO : DataProviderBase, IFormElementDAO
    {

        private const string SQL_SELECT_TABLE_STYLE = "SELECT ID, PageID, GroupID, AttributeName, Taxis, DisplayName, HelpText, ImageUrl, IsVisible, IsVisibleInList, IsSingleLine, InputType, DefaultValue, IsHorizontal, ExtendValues FROM crm_FormElement WHERE PageID = @PageID AND GroupID = @GroupID AND AttributeName = @AttributeName";

        private const string SQL_SELECT_TABLE_STYLE_BY_TABLE_STYLE_ID = "SELECT ID, PageID, GroupID, AttributeName, Taxis, DisplayName, HelpText, ImageUrl, IsVisible, IsVisibleInList, IsSingleLine, InputType, DefaultValue, IsHorizontal, ExtendValues FROM crm_FormElement WHERE ID = @ID";

        private const string SQL_SELECT_TABLE_STYLES = "SELECT ID, PageID, GroupID, AttributeName, Taxis, DisplayName, HelpText, ImageUrl, IsVisible, IsVisibleInList, IsSingleLine, InputType, DefaultValue, IsHorizontal, ExtendValues FROM crm_FormElement WHERE GroupID = @GroupID AND AttributeName = @AttributeName ORDER BY PageID";

        private const string SQL_SELECT_ALL_TABLE_STYLE = "SELECT ID, PageID, GroupID, AttributeName, Taxis, DisplayName, HelpText, ImageUrl, IsVisible, IsVisibleInList, IsSingleLine, InputType, DefaultValue, IsHorizontal, ExtendValues FROM crm_FormElement WHERE GroupID <> '' AND AttributeName <> '' ORDER BY Taxis DESC, ID DESC";

        private const string SQL_UPDATE_TABLE_STYLE = "UPDATE crm_FormElement SET PageID = @PageID, GroupID = @GroupID, AttributeName = @AttributeName, Taxis = @Taxis, DisplayName = @DisplayName, HelpText = @HelpText, ImageUrl = @ImageUrl, IsVisible = @IsVisible, IsVisibleInList = @IsVisibleInList, IsSingleLine = @IsSingleLine, InputType = @InputType, DefaultValue = @DefaultValue, IsHorizontal = @IsHorizontal, ExtendValues = @ExtendValues WHERE ID = @ID";

        private const string SQL_DELETE_TABLE_STYLE = "DELETE FROM crm_FormElement WHERE PageID = @PageID AND GroupID = @GroupID AND AttributeName = @AttributeName";

        private const string SQL_SELECT_TABLE_STYLE_TAXIS = "SELECT Taxis FROM crm_FormElement WHERE ID = @ID";

        private const string SQL_UPDATE_TABLE_STYLE_TAXIS = "UPDATE crm_FormElement SET Taxis = @Taxis WHERE ID = @ID";

        //AuxiliaryFormElementItemInfo
        private const string SQL_SELECT_ALL_ELEMENT_ITEM = "SELECT ID, FormElementID, ItemTitle, ItemValue, IsSelected FROM crm_FormElementItem WHERE (FormElementID = @FormElementID)";

        private const string SQL_DELETE_STYLE_ITEMS = "DELETE FROM crm_FormElementItem WHERE ID = @ID";

        private const string PARM_ID = "@ID";
        private const string PARM_PAGE_ID = "@PageID";
        private const string PARM_GROUP_ID = "@GroupID";
        private const string PARM_ATTRIBUTE_NAME = "@AttributeName";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_DISPLAY_NAME = "@DisplayName";
        private const string PARM_HELP_TEXT = "@HelpText";
        private const string PARM_IMAGE_URL = "@ImageUrl";
        private const string PARM_IS_VISIBLE = "@IsVisible";
        private const string PARM_IS_VISIBLE_IN_LIST = "@IsVisibleInList";
        private const string PARM_IS_SINGLE_LINE = "@IsSingleLine";
        private const string PARM_INPUT_TYPE = "@InputType";
        private const string PARM_DEFAULT_VALUE = "@DefaultValue";
        private const string PARM_IS_HORIZONTAL = "@IsHorizontal";
        private const string PARM_EXTEND_VALUES = "@ExtendValues";

        //item
        private const string PARM_FORM_ELEMENT_ID = "@FormElementID";
        private const string PARM_ITEM_TITLE = "@ItemTitle";
        private const string PARM_ITEM_VALUE = "@ItemValue";
        private const string PARM_IS_SELECTED = "@IsSelected";

        public int Insert(FormElementInfo elementInfo)
        {
            return Insert(elementInfo, false);
        }

        public int InsertWithTaxis(FormElementInfo elementInfo)
        {
            return Insert(elementInfo, true);
        }

        private string GetInsertFormElementSqlString()
        {
            string SQL_INSERT_TABLE_STYLE = "INSERT INTO crm_FormElement (PageID, GroupID, AttributeName, Taxis, DisplayName, HelpText, ImageUrl, IsVisible, IsVisibleInList, IsSingleLine, InputType, DefaultValue, IsHorizontal, ExtendValues) VALUES (@PageID, @GroupID, @AttributeName, @Taxis, @DisplayName, @HelpText, @ImageUrl, @IsVisible, @IsVisibleInList, @IsSingleLine, @InputType, @DefaultValue, @IsHorizontal, @ExtendValues)";

            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT_TABLE_STYLE = "INSERT INTO crm_FormElement (ID, PageID, GroupID, AttributeName, Taxis, DisplayName, HelpText, ImageUrl, IsVisible, IsVisibleInList, IsSingleLine, InputType, DefaultValue, IsHorizontal, ExtendValues) VALUES (crm_FormElement_SEQ.NEXTVAL, @PageID, @GroupID, @AttributeName, @Taxis, @DisplayName, @HelpText, @ImageUrl, @IsVisible, @IsVisibleInList, @IsSingleLine, @InputType, @DefaultValue, @IsHorizontal, @ExtendValues)";
            }

            return SQL_INSERT_TABLE_STYLE;
        }

        private string GetInsertFormElementItemSqlString()
        {
            string SQL_INSERT_STYLE_ITEM = "INSERT INTO crm_FormElementItem (FormElementID, ItemTitle, ItemValue, IsSelected) VALUES (@FormElementID, @ItemTitle, @ItemValue, @IsSelected)";

            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT_STYLE_ITEM = "INSERT INTO crm_FormElementItem (ID, FormElementID, ItemTitle, ItemValue, IsSelected) VALUES (crm_FormElementItem_SEQ.NEXTVAL, @FormElementID, @ItemTitle, @ItemValue, @IsSelected)";
            }

            return SQL_INSERT_STYLE_ITEM;
        }

        private int Insert(FormElementInfo elementInfo, bool isWithTaxis)
        {
            int formElementID = 0;

            if (!isWithTaxis)
            {
                elementInfo.Taxis = this.GetNewFormElementInfoTaxis(elementInfo.PageID, elementInfo.GroupID);
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_PAGE_ID, EDataType.Integer, elementInfo.PageID),
                this.GetParameter(PARM_GROUP_ID, EDataType.Integer, elementInfo.GroupID),
				this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, elementInfo.AttributeName),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, elementInfo.Taxis),
                this.GetParameter(PARM_DISPLAY_NAME, EDataType.NVarChar, 255, elementInfo.DisplayName),
                this.GetParameter(PARM_HELP_TEXT, EDataType.VarChar, 255, elementInfo.HelpText),
                this.GetParameter(PARM_IMAGE_URL, EDataType.VarChar, 200, elementInfo.ImageUrl),
                this.GetParameter(PARM_IS_VISIBLE, EDataType.VarChar, 18, elementInfo.IsVisible.ToString()),
                this.GetParameter(PARM_IS_VISIBLE_IN_LIST, EDataType.VarChar, 18, elementInfo.IsVisibleInList.ToString()),
                this.GetParameter(PARM_IS_SINGLE_LINE, EDataType.VarChar, 18, elementInfo.IsSingleLine.ToString()),
				this.GetParameter(PARM_INPUT_TYPE, EDataType.VarChar, 50, EInputTypeUtils.GetValue(elementInfo.InputType)),
                this.GetParameter(PARM_DEFAULT_VALUE, EDataType.VarChar, 255, elementInfo.DefaultValue),
                this.GetParameter(PARM_IS_HORIZONTAL, EDataType.VarChar, 18, elementInfo.IsHorizontal.ToString()),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, elementInfo.Additional.ToString())
			};

            string SQL_INSERT_TABLE_STYLE = this.GetInsertFormElementSqlString();
            string SQL_INSERT_STYLE_ITEM = this.GetInsertFormElementItemSqlString();

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.ExecuteNonQuery(trans, SQL_INSERT_TABLE_STYLE, insertParms);

                        formElementID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "crm_FormElement");

                        if (elementInfo.StyleItems != null && elementInfo.StyleItems.Count > 0)
                        {
                            foreach (FormElementItemInfo itemInfo in elementInfo.StyleItems)
                            {
                                IDbDataParameter[] insertItemParms = new IDbDataParameter[]
							    {
								    this.GetParameter(PARM_FORM_ELEMENT_ID, EDataType.Integer, formElementID),
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

            return formElementID;
        }

        public int GetNewFormElementInfoTaxis(int pageID, int groupID)
        {
            int maxTaxis = this.GetMaxTaxisByKeyStart(pageID, groupID);
            return maxTaxis + 1;
        }

        public void InsertWithTransaction(FormElementInfo elementInfo, IDbTransaction trans)
        {
            elementInfo.Taxis = GetNewFormElementInfoTaxis(elementInfo.PageID, elementInfo.GroupID);

            IDbDataParameter[] insertParms = new IDbDataParameter[]
		    {
                this.GetParameter(PARM_PAGE_ID, EDataType.Integer, elementInfo.PageID),
                this.GetParameter(PARM_GROUP_ID, EDataType.Integer, elementInfo.GroupID),
			    this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, elementInfo.AttributeName),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, elementInfo.Taxis),
                this.GetParameter(PARM_DISPLAY_NAME, EDataType.NVarChar, 255, elementInfo.DisplayName),
                this.GetParameter(PARM_HELP_TEXT, EDataType.VarChar, 255, elementInfo.HelpText),
                this.GetParameter(PARM_IMAGE_URL, EDataType.VarChar, 200, elementInfo.ImageUrl),
                this.GetParameter(PARM_IS_VISIBLE, EDataType.VarChar, 18, elementInfo.IsVisible.ToString()),
                this.GetParameter(PARM_IS_VISIBLE_IN_LIST, EDataType.VarChar, 18, elementInfo.IsVisibleInList.ToString()),
                this.GetParameter(PARM_IS_SINGLE_LINE, EDataType.VarChar, 18, elementInfo.IsSingleLine.ToString()),
			    this.GetParameter(PARM_INPUT_TYPE, EDataType.VarChar, 50, EInputTypeUtils.GetValue(elementInfo.InputType)),
                this.GetParameter(PARM_DEFAULT_VALUE, EDataType.VarChar, 255, elementInfo.DefaultValue),
                this.GetParameter(PARM_IS_HORIZONTAL, EDataType.VarChar, 18, elementInfo.IsHorizontal.ToString()),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, elementInfo.Additional.ToString())
		    };

            string SQL_INSERT_TABLE_STYLE = this.GetInsertFormElementSqlString();
            string SQL_INSERT_STYLE_ITEM = this.GetInsertFormElementItemSqlString();

            if (elementInfo.StyleItems == null || elementInfo.StyleItems.Count == 0)
            {
                this.ExecuteNonQuery(trans, SQL_INSERT_TABLE_STYLE, insertParms);
            }
            else
            {
                int formElementID = 0;

                this.ExecuteNonQuery(trans, SQL_INSERT_TABLE_STYLE, insertParms);

                formElementID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "crm_FormElement");

                //string SELECT_CMD = "SELECT @@IDENTITY AS 'ID'";
                //using (IDataReader rdr = this.ExecuteReader(trans, SELECT_CMD))
                //{
                //    if (rdr.Read())
                //    {
                //        formElementID = Convert.ToInt32(rdr[0].ToString());
                //    }
                //    else
                //    {
                //        throw new DataException();
                //    }
                //    rdr.Close();
                //}

                foreach (FormElementItemInfo itemInfo in elementInfo.StyleItems)
                {
                    IDbDataParameter[] insertItemParms = new IDbDataParameter[]
				    {
					    this.GetParameter(PARM_ID, EDataType.Integer, formElementID),
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
            string SQL_INSERT_STYLE_ITEM = this.GetInsertFormElementItemSqlString();

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {

                        foreach (FormElementItemInfo itemInfo in styleItems)
                        {
                            IDbDataParameter[] insertItemParms = new IDbDataParameter[]
							{
								this.GetParameter(PARM_ID, EDataType.Integer, itemInfo.ID),
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

        public void DeleteStyleItems(int formElementID)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_ID, EDataType.Integer, formElementID)
			};

            this.ExecuteNonQuery(SQL_DELETE_STYLE_ITEMS, parms);
        }

        public ArrayList GetElementItemArrayList(int formElementID)
        {
            ArrayList styleItems = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_FORM_ELEMENT_ID, EDataType.Integer, formElementID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ALL_ELEMENT_ITEM, parms))
            {
                while (rdr.Read())
                {
                    FormElementItemInfo info = new FormElementItemInfo(rdr.GetInt32(0), rdr.GetInt32(1), rdr.GetValue(2).ToString(), rdr.GetValue(3).ToString(), TranslateUtils.ToBool(rdr.GetValue(4).ToString()));
                    styleItems.Add(info);
                }
                rdr.Close();
            }
            return styleItems;
        }

        public void Update(FormElementInfo elementInfo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PAGE_ID, EDataType.Integer, elementInfo.PageID),
                this.GetParameter(PARM_GROUP_ID, EDataType.Integer, elementInfo.GroupID),
				this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, elementInfo.AttributeName),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, elementInfo.Taxis),
                this.GetParameter(PARM_DISPLAY_NAME, EDataType.NVarChar, 255, elementInfo.DisplayName),
                this.GetParameter(PARM_HELP_TEXT, EDataType.VarChar, 255, elementInfo.HelpText),
                this.GetParameter(PARM_IMAGE_URL, EDataType.VarChar, 200, elementInfo.ImageUrl),
                this.GetParameter(PARM_IS_VISIBLE, EDataType.VarChar, 18, elementInfo.IsVisible.ToString()),
	            this.GetParameter(PARM_IS_VISIBLE_IN_LIST, EDataType.VarChar, 18, elementInfo.IsVisibleInList.ToString()),
                this.GetParameter(PARM_IS_SINGLE_LINE, EDataType.VarChar, 18, elementInfo.IsSingleLine.ToString()),
				this.GetParameter(PARM_INPUT_TYPE, EDataType.VarChar, 50, EInputTypeUtils.GetValue(elementInfo.InputType)),
                this.GetParameter(PARM_DEFAULT_VALUE, EDataType.VarChar, 255, elementInfo.DefaultValue),
                this.GetParameter(PARM_IS_HORIZONTAL, EDataType.VarChar, 18, elementInfo.IsHorizontal.ToString()),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, elementInfo.Additional.ToString()),
                this.GetParameter(PARM_ID, EDataType.Integer, elementInfo.ID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_TABLE_STYLE, updateParms);
        }

        public void Delete(int pageID, int groupID, string attributeName)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PAGE_ID, EDataType.Integer, pageID),
                this.GetParameter(PARM_GROUP_ID, EDataType.Integer, groupID),
                this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, attributeName)
            };

            this.ExecuteNonQuery(SQL_DELETE_TABLE_STYLE, parms);
        }

        public void Delete(int pageID, int groupID)
        {
            string sqlString = string.Format("DELETE FROM crm_FormElement WHERE PageID = {0} AND GroupID = {1}", pageID, groupID);
            this.ExecuteNonQuery(sqlString);
        }

        public void Delete(int formElementID)
        {
            string sqlString = string.Format("DELETE FROM crm_FormElement WHERE ID = {0}", formElementID);
            this.ExecuteNonQuery(sqlString);
        }

        public List<FormElementInfo> GetFormElementInfoList(int pageID, int groupID)
        {
            List<FormElementInfo> list = new List<FormElementInfo>();

            string sqlString = string.Format("SELECT ID, PageID, GroupID, AttributeName, Taxis, DisplayName, HelpText, ImageUrl, IsVisible, IsVisibleInList, IsSingleLine, InputType, DefaultValue, IsHorizontal, ExtendValues FROM crm_FormElement WHERE PageID = {0} AND GroupID = {1} ORDER BY Taxis", pageID, groupID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    FormElementInfo elementInfo = GetFormElementInfoByReader(rdr);
                    list.Add(elementInfo);
                }
                rdr.Close();
            }

            return list;
        }

        public bool IsExists(int pageID, int groupID, string attributeName)
        {
            bool exists = false;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PAGE_ID, EDataType.Integer, pageID),
                this.GetParameter(PARM_GROUP_ID, EDataType.Integer, groupID),
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

        public FormElementInfo GetFormElementInfo(int formElementID)
        {
            FormElementInfo elementInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_ID, EDataType.Integer, formElementID)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_STYLE_BY_TABLE_STYLE_ID, parms))
            {
                if (rdr.Read())
                {
                    elementInfo = GetFormElementInfoByReader(rdr);
                }
                rdr.Close();
            }

            return elementInfo;
        }

        public FormElementInfo GetFormElementInfo(int pageID, int groupID, string attributeName)
        {
            FormElementInfo elementInfo = null;

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
                this.GetParameter(PARM_PAGE_ID, EDataType.Integer, pageID),
                this.GetParameter(PARM_GROUP_ID, EDataType.Integer, groupID),
				this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, attributeName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_STYLE, parms))
            {
                if (rdr.Read())
                {
                    elementInfo = GetFormElementInfoByReader(rdr);
                }
                rdr.Close();
            }

            return elementInfo;
        }

        private FormElementInfo GetFormElementInfoByReader(IDataReader rdr)
        {
            int id = Convert.ToInt32(rdr["ID"]);
            int pageID = Convert.ToInt32(rdr["PageID"]);
            int groupID = Convert.ToInt32(rdr["GroupID"]);
            string attributeName = rdr["AttributeName"].ToString();
            int taxis = Convert.ToInt32(rdr["Taxis"]);
            string displayName = rdr["DisplayName"].ToString();
            string helpText = rdr["HelpText"].ToString();
            string imageUrl = rdr["ImageUrl"].ToString();
            bool isVisible = TranslateUtils.ToBool(rdr["IsVisible"].ToString());
            bool isVisibleInList = TranslateUtils.ToBool(rdr["IsVisibleInList"].ToString());
            bool isSingleLine = TranslateUtils.ToBool(rdr["IsSingleLine"].ToString());
            EInputType inputType = EInputTypeUtils.GetEnumType(rdr["InputType"].ToString());
            string defaultValue = rdr["DefaultValue"].ToString();
            bool isHorizontal = TranslateUtils.ToBool(rdr["IsHorizontal"].ToString());
            string extendValues = rdr["ExtendValues"].ToString();

            FormElementInfo elementInfo = new FormElementInfo(id, pageID, groupID, attributeName, taxis, displayName, helpText, imageUrl, isVisible, isVisibleInList, isSingleLine, inputType, defaultValue, isHorizontal, extendValues);

            return elementInfo;
        }

        public ArrayList GetFormElementInfoWithItemsArrayList(int groupID, string attributeName)
        {
            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_GROUP_ID, EDataType.Integer, groupID),
                this.GetParameter(PARM_ATTRIBUTE_NAME, EDataType.VarChar, 50, attributeName)
			};

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_TABLE_STYLES, parms))
            {
                while (rdr.Read())
                {
                    FormElementInfo elementInfo = GetFormElementInfoByReader(rdr);
                    if (elementInfo.InputType == EInputType.CheckBox || elementInfo.InputType == EInputType.Radio || elementInfo.InputType == EInputType.SelectMultiple || elementInfo.InputType == EInputType.SelectOne)
                    {
                        ArrayList styleItems = this.GetElementItemArrayList(elementInfo.ID);
                        if (styleItems != null && styleItems.Count > 0)
                        {
                            elementInfo.StyleItems = styleItems;
                        }
                    }
                    arraylist.Add(elementInfo);
                }
                rdr.Close();
            }

            return arraylist;
        }

        private int GetMaxTaxisByKeyStart(int pageID, int groupID)
        {
            string sqlString = string.Format("SELECT MAX(Taxis) AS MaxTaxis FROM crm_FormElement WHERE PageID = {0} AND GroupID = {1}", pageID, groupID);
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

        public void TaxisUp(int formElementID)
        {
            FormElementInfo elementInfo = this.GetFormElementInfo(formElementID);
            if (elementInfo != null)
            {
                string sqlString = "SELECT TOP 1 ID, Taxis FROM crm_FormElement WHERE PageID = @PageID AND GroupID = @GroupID AND Taxis > (SELECT Taxis FROM crm_FormElement WHERE ID = @ID) ORDER BY Taxis";
                int higherID = 0;
                int higherTaxis = 0;

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
                    this.GetParameter(PARM_PAGE_ID, EDataType.Integer, elementInfo.PageID),
                    this.GetParameter(PARM_GROUP_ID, EDataType.Integer, elementInfo.GroupID),
				    this.GetParameter(PARM_ID, EDataType.Integer, formElementID)
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
                    this.SetTaxis(formElementID, higherTaxis);
                    this.SetTaxis(higherID, elementInfo.Taxis);
                }
            }
        }

        public void TaxisDown(int formElementID)
        {
            FormElementInfo elementInfo = this.GetFormElementInfo(formElementID);
            if (elementInfo != null)
            {
                string sqlString = "SELECT TOP 1 ID, Taxis FROM crm_FormElement WHERE PageID = @PageID AND GroupID = @GroupID AND Taxis < (SELECT Taxis FROM crm_FormElement WHERE ID = @ID) ORDER BY Taxis DESC";
                int lowerID = 0;
                int lowerTaxis = 0;

                IDbDataParameter[] parms = new IDbDataParameter[]
			    {
                    this.GetParameter(PARM_PAGE_ID, EDataType.Integer, elementInfo.PageID),
                    this.GetParameter(PARM_GROUP_ID, EDataType.Integer, elementInfo.GroupID),
				    this.GetParameter(PARM_ID, EDataType.Integer, formElementID)
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
                    this.SetTaxis(formElementID, lowerTaxis);
                    this.SetTaxis(lowerID, elementInfo.Taxis);
                }
            }
        }

        private void SetTaxis(int formElementID, int taxis)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
			{
				this.GetParameter(PARM_TAXIS, EDataType.Integer, taxis),
				this.GetParameter(PARM_ID, EDataType.Integer, formElementID)
			};

            this.ExecuteNonQuery(SQL_UPDATE_TABLE_STYLE_TAXIS, parms);
        }
    }
}
