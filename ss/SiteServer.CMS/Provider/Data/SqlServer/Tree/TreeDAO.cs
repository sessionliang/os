using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;
using System.Data;
using System.Collections;
using BaiRong.Core;
using BaiRong.Core.Data;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class TreeDAO : DataProviderBase, ITreeDAO
    {
        public TreeDAO(string tableName)
        {
            TABLE_NAME = tableName;
        }

        private string TABLE_NAME;

        #region ITreeItem
        public const string PARM_ITEM_ID = "@ItemID";
        public const string PARM_ITEM_NAME = "@ItemName";
        public const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        public const string PARM_PARENT_ID = "@ParentID";
        public const string PARM_PARENTS_PATH = "@ParentsPath";
        public const string PARM_PARENTS_COUNT = "@ParentsCount";
        public const string PARM_CHILDREN_COUNT = "@ChildrenCount";
        public const string PARM_IS_LAST_ITEM = "@IsLastItem";
        public const string PARM_TAXIS = "@Taxis";
        public const string PARM_CONTENT_NUM = "@ContentNum";
        public const string PARM_ENABLED = "@Enabled";
        public const string PARM_ITEM_INDEX_NAME = "@ItemIndexName";
        #endregion

        #region ITreeItem
        public ArrayList GetItemIDArrayListByItemID(int publishmentSystemID)
        {
            string sqlString = string.Format(@"SELECT ItemID
FROM {1}
WHERE PublishmentSystemID = {0} AND (ParentID >= 0)
ORDER BY Taxis", publishmentSystemID, TABLE_NAME);
            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int id = Convert.ToInt32(rdr[0]);
                    arraylist.Add(id);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetItemIDArrayListByParentID(int publishmentSystemID, int parentID)
        {
            ArrayList list = new ArrayList();
            string sqlString = string.Empty;

            sqlString = string.Format(@"SELECT ItemID
FROM {2}
WHERE (ParentID = {1}) AND PublishmentSystemID = {0}
ORDER BY Taxis", publishmentSystemID, parentID, TABLE_NAME);

            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int itemID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(itemID);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetItemIDArrayListByParentID(int publishmentSystemID, int parentID, string where)
        {
            if (string.IsNullOrEmpty(where))
                return this.GetItemIDArrayListByParentID(publishmentSystemID, parentID);

            ArrayList list = new ArrayList();
            string sqlString = string.Empty;

            sqlString = string.Format(@"SELECT ItemID
FROM {2}
WHERE (ParentID = {1}) AND PublishmentSystemID = {0} AND {3}
ORDER BY Taxis", publishmentSystemID, parentID, TABLE_NAME, where);

            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int itemID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(itemID);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public List<TreeBaseItem> GetItemInfoArrayListByParentID(int publishmentSystemID, int parentID)
        {
            string sqlString = string.Empty;

            sqlString = string.Format(@"SELECT ItemID, ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis FROM {2} 
WHERE (PublishmentSystemID={0} AND ParentID = {1})
ORDER BY Taxis", publishmentSystemID, parentID, TABLE_NAME);

            List<TreeBaseItem> arraylist = new List<TreeBaseItem>();
            TreeBaseItem item;
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    item = new TreeBaseItem(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), TranslateUtils.ToBool(rdr.GetString(10)), rdr.GetInt32(11));
                    arraylist.Add(item);
                }
                rdr.Close();
            }

            return arraylist;
        }




        public List<TreeBaseItem> GetItemInfoArrayListByParentID(int publishmentSystemID, int parentID, string where)
        {
            if (string.IsNullOrEmpty(where))
                return this.GetItemInfoArrayListByParentID(publishmentSystemID, parentID);
            string sqlString = string.Empty;
            sqlString = string.Format(@"SELECT ItemID, ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis FROM {2} 
WHERE (PublishmentSystemID={0} AND ParentID = {1} AND {3})
ORDER BY Taxis", publishmentSystemID, parentID, TABLE_NAME, where);
            List<TreeBaseItem> arraylist = new List<TreeBaseItem>();
            TreeBaseItem item;
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    item = new TreeBaseItem(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), TranslateUtils.ToBool(rdr.GetString(10)), rdr.GetInt32(11));
                    arraylist.Add(item);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public List<TreeBaseItem> GetItemInfoArrayListByPublishmentSystemID(int publishmentSystemID, string where)
        {
            string sqlString = string.Empty;
            if (!string.IsNullOrEmpty(where))
                sqlString = string.Format(@"SELECT ItemID, ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis FROM {1} 
WHERE (PublishmentSystemID={0} AND {2})
ORDER BY Taxis", publishmentSystemID, TABLE_NAME, where);
            else
                sqlString = string.Format(@"SELECT ItemID, ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis FROM {1} 
WHERE (PublishmentSystemID={0})
ORDER BY Taxis", publishmentSystemID, TABLE_NAME);
            List<TreeBaseItem> arraylist = new List<TreeBaseItem>();
            TreeBaseItem item;
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    item = new TreeBaseItem(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), TranslateUtils.ToBool(rdr.GetString(10)), rdr.GetInt32(11));
                    arraylist.Add(item);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public int GetTopID(int publishmentSystemID)
        {
            string sqlString = string.Empty;
            sqlString = string.Format(@"SELECT ItemID FROM {1} 
WHERE (PublishmentSystemID={0} AND ParentID = 0)
ORDER BY Taxis", publishmentSystemID, TABLE_NAME);
            int topID = 0;
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    topID = TranslateUtils.ToInt(rdr[0].ToString());
                }
                rdr.Close();
            }

            return topID;
        }

        public TreeBaseItem GetItemInfo(int publishmentSystemID, int itemID)
        {
            string SQL_SELECT_ITEM = string.Format("SELECT ItemID, ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis FROM {0} WHERE ItemID = @ItemID", this.TABLE_NAME);

            TreeBaseItem item = null;

            IDbDataParameter[] itemParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ITEM_ID, EDataType.Integer, itemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ITEM, itemParms))
            {
                if (rdr.Read())
                {
                    item = item = new TreeBaseItem(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), TranslateUtils.ToBool(rdr.GetString(10)), rdr.GetInt32(11));
                }
                rdr.Close();
            }
            return item;
        }

        public string GetItemName(int publishmentSystemID, int itemID)
        {
            TreeBaseItem itemInfo = GetItemInfo(publishmentSystemID, itemID) as TreeBaseItem;
            if (itemInfo != null)
                return itemInfo.ItemName;
            else
                return string.Empty;
        }

        public void UpdateTaxis(int publishmentSystemID, int itemID, bool isSubtract)
        {
            if (isSubtract)
            {
                this.TaxisSubtract(publishmentSystemID, itemID);
            }
            else
            {
                this.TaxisAdd(publishmentSystemID, itemID);
            }
        }

        public int GetMaxTaxisByParentPath(string parentPath)
        {
            string CMD = string.Concat("SELECT MAX(Taxis) AS MaxTaxis FROM ", TABLE_NAME, " WHERE (ParentsPath = '", parentPath, "') OR (ParentsPath like '", parentPath, ",%')");
            int maxTaxis = 0;

            using (IDataReader rdr = this.ExecuteReader(CMD))
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

        public ArrayList GetItemIDArrayListForDescendant(int itemID)
        {
            string sqlString = string.Format(@"SELECT ItemID
FROM {1}
WHERE (ParentsPath LIKE '{0},%') OR
      (ParentsPath LIKE '%,{0},%') OR
      (ParentsPath LIKE '%,{0}') OR
      (ParentID = {0})
", itemID, TABLE_NAME);
            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    list.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }

            return list;
        }

        /// <summary>
        /// 将节点数减1
        /// </summary>
        /// <param name="parentsPath"></param>
        /// <param name="subtractNum"></param>
        public void UpdateSubtractChildrenCount(string parentsPath, int subtractNum)
        {
            if (!string.IsNullOrEmpty(parentsPath))
            {
                string sqlString = string.Concat("UPDATE ", TABLE_NAME, " SET ChildrenCount = ChildrenCount - ", subtractNum, " WHERE ItemID in (", parentsPath, ")");
                this.ExecuteNonQuery(sqlString);
            }
        }

        /// <summary>
        /// 将节点数加1
        /// </summary>
        /// <param name="parentsPath"></param>
        /// <param name="addNum"></param>
        public void UpdateAddChildrenCount(string parentsPath, int addNum)
        {
            if (!string.IsNullOrEmpty(parentsPath))
            {
                string sqlString = string.Concat("UPDATE ", TABLE_NAME, " SET ChildrenCount = ChildrenCount + ", addNum, " WHERE ItemID in (", parentsPath, ")");
                this.ExecuteNonQuery(sqlString);
            }
        }

        /// <summary>
        /// 修改分类内容数量
        /// </summary>
        /// <param name="publishmentSystemInfo"></param>
        /// <param name="itemID"></param>
        /// <param name="isRemoveCache"></param>
        public virtual void UpdateContentNum(int publishmentSystemID, int itemID, int contentNum)
        {
            TreeBaseItem itemInfo = this.GetItemInfo(publishmentSystemID, itemID);
            string sqlString = string.Empty;

            sqlString = string.Format("UPDATE {2} SET ContentNum = {0} WHERE (itemID = {1})", contentNum, itemID, TABLE_NAME);

            if (!string.IsNullOrEmpty(sqlString))
            {
                this.ExecuteNonQuery(sqlString);
            }
        }

        #region 排序
        /// <summary>
        /// Change The Texis To Lowerer Level
        /// </summary>
        public void TaxisSubtract(int publishmentSystemID, int selectedItemID)
        {
            TreeBaseItem itemInfo = this.GetItemInfo(publishmentSystemID, selectedItemID);
            if (itemInfo == null) return;
            this.UpdateWholeTaxisByPublishmentSystemID(itemInfo.PublishmentSystemID);
            //Get Lower Taxis and ItemID
            int lowerItemID = 0;
            int lowerChildrenCount = 0;
            string lowerParentsPath = "";
            string sqlString = string.Format(@"SELECT TOP 1 ItemID, ChildrenCount, ParentsPath
FROM {0}
WHERE (ParentID = @ParentID) AND (ItemID <> @ItemID) AND (Taxis < @Taxis) AND (PublishmentSystemID = @PublishmentSystemID)
ORDER BY Taxis DESC", TABLE_NAME);

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(TreeDAO.PARM_PARENT_ID, EDataType.Integer, itemInfo.ParentID),
                this.GetParameter(TreeDAO.PARM_ITEM_ID, EDataType.Integer, itemInfo.ItemID),
                this.GetParameter(TreeDAO.PARM_TAXIS, EDataType.Integer, itemInfo.Taxis),
                this.GetParameter(TreeDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, itemInfo.PublishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    lowerItemID = Convert.ToInt32(rdr[0]);
                    lowerChildrenCount = Convert.ToInt32(rdr[1]);
                    lowerParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            string lowerItemPath = "";
            if (lowerParentsPath == "")
            {
                lowerItemPath = lowerItemID.ToString();
            }
            else
            {
                lowerItemPath = String.Concat(lowerParentsPath, ",", lowerItemID);
            }
            string selectedItemPath = "";
            if (itemInfo.ParentsPath == "")
            {
                selectedItemPath = itemInfo.ItemID.ToString();
            }
            else
            {
                selectedItemPath = String.Concat(itemInfo.ParentsPath, ",", itemInfo.ItemID);
            }

            this.SetTaxisSubtract(selectedItemID, selectedItemPath, lowerChildrenCount + 1);
            this.SetTaxisAdd(lowerItemID, lowerItemPath, itemInfo.ChildrenCount + 1);

            this.UpdateIsLastItem(itemInfo.ParentID);

        }

        /// <summary>
        /// Change The Texis To Higher Level
        /// </summary>
        public void TaxisAdd(int publishmentSystemID, int selectedItemID)
        {
            TreeBaseItem itemInfo = this.GetItemInfo(publishmentSystemID, selectedItemID);
            if (itemInfo == null) return;
            this.UpdateWholeTaxisByPublishmentSystemID(itemInfo.PublishmentSystemID);
            //Get Higher Taxis and ItemID
            int higherItemID = 0;
            int higherChildrenCount = 0;
            string higherParentsPath = "";
            string sqlString = string.Format(@"SELECT TOP 1 ItemID, ChildrenCount, ParentsPath
FROM {0}
WHERE (ParentID = @ParentID) AND (ItemID <> @ItemID) AND (Taxis > @Taxis) AND (PublishmentSystemID = @PublishmentSystemID)
ORDER BY Taxis", TABLE_NAME);

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(TreeDAO.PARM_PARENT_ID, EDataType.Integer, itemInfo.ParentID),
                this.GetParameter(TreeDAO.PARM_ITEM_ID, EDataType.Integer, itemInfo.ItemID),
                this.GetParameter(TreeDAO.PARM_TAXIS, EDataType.Integer, itemInfo.Taxis),
                this.GetParameter(TreeDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, itemInfo.PublishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    higherItemID = Convert.ToInt32(rdr[0]);
                    higherChildrenCount = Convert.ToInt32(rdr[1]);
                    higherParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            string higherItemPath = string.Empty;
            if (higherParentsPath == string.Empty)
            {
                higherItemPath = higherItemID.ToString();
            }
            else
            {
                higherItemPath = String.Concat(higherParentsPath, ",", higherItemID);
            }
            string selectedItemPath = string.Empty;
            if (itemInfo.ParentsPath == string.Empty)
            {
                selectedItemPath = itemInfo.ItemID.ToString();
            }
            else
            {
                selectedItemPath = String.Concat(itemInfo.ParentsPath, ",", itemInfo.ItemID);
            }

            this.SetTaxisAdd(selectedItemID, selectedItemPath, higherChildrenCount + 1);
            this.SetTaxisSubtract(higherItemID, higherItemPath, itemInfo.ChildrenCount + 1);

            this.UpdateIsLastItem(itemInfo.ParentID);
        }

        public void SetTaxisAdd(int itemID, string parentsPath, int AddNum)
        {
            string sqlString = string.Format("UPDATE {3} SET Taxis = Taxis + {0} WHERE ItemID = {1} OR ParentsPath = '{2}' OR ParentsPath like '{2},%'", AddNum, itemID, parentsPath, TABLE_NAME);

            this.ExecuteNonQuery(sqlString);
        }

        public void SetTaxisSubtract(int itemID, string parentsPath, int SubtractNum)
        {
            string sqlString = string.Format("UPDATE {3} SET Taxis = Taxis - {0} WHERE  ItemID = {1} OR ParentsPath = '{2}' OR ParentsPath like '{2},%'", SubtractNum, itemID, parentsPath, TABLE_NAME);

            this.ExecuteNonQuery(sqlString);
        }

        public void UpdateIsLastItem(int parentID)
        {
            if (parentID > 0)
            {
                string sqlString = string.Format("UPDATE {0} SET IsLastItem = @IsLastItem WHERE  ParentID = @ParentID", TABLE_NAME);

                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_IS_LAST_ITEM, EDataType.VarChar, 18, false.ToString()),
                    this.GetParameter(PARM_PARENT_ID, EDataType.Integer, parentID)
                };

                this.ExecuteNonQuery(sqlString, parms);

                sqlString = string.Format("UPDATE {2} SET IsLastItem = '{0}' WHERE (ItemID IN (SELECT TOP 1 ItemID FROM {2} WHERE ParentID = {1} ORDER BY Taxis DESC))", true.ToString(), parentID, TABLE_NAME);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    sqlString = string.Format(@"UPDATE {2} SET IsLastItem = '{0}' WHERE (ItemID IN (
SELECT ItemID FROM (
    SELECT ItemID FROM {2} WHERE ParentID = {1} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", true.ToString(), parentID, TABLE_NAME);
                }

                this.ExecuteNonQuery(sqlString);
            }
        }

        /// <summary>
        /// 更新所有节点的排序号
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        public void UpdateWholeTaxisByPublishmentSystemID(int publishmentSystemID)
        {
            if (publishmentSystemID <= 0) return;
            ArrayList ItemIDArrayList = new ArrayList();
            ItemIDArrayList.Add(publishmentSystemID);
            int level = 0;
            string SELECT_LEVEL_CMD = string.Format("SELECT MAX(ParentsCount) FROM {1} WHERE (PublishmentSystemID = {0})", publishmentSystemID, TABLE_NAME);
            using (IDataReader rdr = this.ExecuteReader(SELECT_LEVEL_CMD))
            {
                while (rdr.Read())
                {
                    int parentsCount = Convert.ToInt32(rdr[0]);
                    level = parentsCount;
                }
                rdr.Close();
            }

            for (int i = 0; i < level; i++)
            {
                ArrayList arraylist = new ArrayList(ItemIDArrayList);
                foreach (int savedItemID in arraylist)
                {
                    int lastChildItemIDOfSavedItemID = savedItemID;
                    string SELECT_Item_CMD = string.Format("SELECT ItemID, ItemName FROM {1} WHERE ParentID = {0} ORDER BY Taxis, IsLastItem", savedItemID, TABLE_NAME);
                    using (IDataReader rdr = this.ExecuteReader(SELECT_Item_CMD))
                    {
                        while (rdr.Read())
                        {
                            int ItemID = Convert.ToInt32(rdr[0]);
                            if (!ItemIDArrayList.Contains(ItemID))
                            {
                                int index = ItemIDArrayList.IndexOf(lastChildItemIDOfSavedItemID);
                                ItemIDArrayList.Insert(index + 1, ItemID);
                                lastChildItemIDOfSavedItemID = ItemID;
                            }
                        }
                        rdr.Close();
                    }
                }
            }


            for (int i = 1; i <= ItemIDArrayList.Count; i++)
            {
                int ItemID = (int)ItemIDArrayList[i - 1];
                string UPDATE_CMD = string.Format("UPDATE {2} SET Taxis = {0} WHERE ItemID = {1}", i, ItemID, TABLE_NAME);
                this.ExecuteNonQuery(UPDATE_CMD);
            }
        }

        /// <summary>
        /// 获取所有的IndexNames
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        public ArrayList GetItemIndexNameArrayList(int publishmentSystemID)
        {
            ArrayList list = new ArrayList();
            string SQL_SELECT_item_INDEX_NAME_COLLECTION = string.Format("SELECT DISTINCT ItemIndexName FROM {0} WHERE PublishmentSystemID = @PublishmentSystemID", TABLE_NAME);
            IDbDataParameter[] itemParms = new IDbDataParameter[]
                {
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
                };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_item_INDEX_NAME_COLLECTION, itemParms))
            {
                while (rdr.Read())
                {
                    string itemIndexName = rdr.GetValue(0).ToString();
                    list.Add(itemIndexName);
                }
                rdr.Close();
            }

            return list;
        }
        #endregion

        #region 修改父级之后，修改子集栏目数量
        public void UpdateItemNum(TreeBaseItem oldParentInfo, TreeBaseItem newParentInfo)
        {
            //以前的父级，子节点数-1
            if (oldParentInfo != null)
            {
                this.UpdateIsLastItem(oldParentInfo.ItemID);
                this.UpdateSubtractChildrenCount(oldParentInfo.ParentsPath + "," + oldParentInfo.ItemID, 1);
            }
            //现在的父级，子节点数+1
            this.UpdateIsLastItem(newParentInfo.ItemID);
            this.UpdateAddChildrenCount(newParentInfo.ParentsPath + "," + newParentInfo.ItemID, 1);
        }
        #endregion
        #endregion

        #region 特殊的加载


        /// <summary>
        /// by 20151229 sofuny
        /// 
        /// 加载有分类的树
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="classifyID"></param>
        /// <returns></returns>
        public ArrayList GetItemInfoByClassifyID(int publishmentSystemID, int classifyID)
        {
            string SQL_SELECT_ITEM = string.Format("SELECT ItemID, ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis FROM {0} WHERE {1} = @{1}", this.TABLE_NAME, OrganizationInfoAttribute.ClassifyID);
            ArrayList list = new ArrayList();

            IDbDataParameter[] itemParms = new IDbDataParameter[]
            {
                this.GetParameter("@"+OrganizationInfoAttribute.ClassifyID, EDataType.Integer, classifyID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ITEM, itemParms))
            {
                while (rdr.Read())
                {
                    int id = rdr.GetInt32(0);
                    list.Add(id);
                }
                rdr.Close();
            }
            return list;
        }

        public List<TreeBaseItem> GetItemInfoArrayListByParentID(int publishmentSystemID, int parentID, ArrayList itemList)
        {
            StringBuilder sqlString = new StringBuilder();

            sqlString.AppendFormat(@"SELECT ItemID, ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis FROM {1} 
WHERE (PublishmentSystemID={0})
", publishmentSystemID, TABLE_NAME);

            //if (itemList.Count > 0)
            sqlString.AppendFormat(" AND ParentID = {0} and ItemID in ({1})", parentID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(itemList));
            //else
            // sqlString.AppendFormat(" AND ParentID = {0}", parentID);

            sqlString.Append(" ORDER BY Taxis");

            List<TreeBaseItem> arraylist = new List<TreeBaseItem>();
            TreeBaseItem item;
            using (IDataReader rdr = this.ExecuteReader(sqlString.ToString()))
            {
                while (rdr.Read())
                {
                    item = new TreeBaseItem(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), TranslateUtils.ToBool(rdr.GetString(10)), rdr.GetInt32(11));
                    arraylist.Add(item);
                }
                rdr.Close();
            }

            return arraylist;
        }


        public ArrayList GetItemIDArrayListByParentID(int publishmentSystemID, int parentID, ArrayList itemList)
        {
            ArrayList list = new ArrayList();
            StringBuilder sqlString = new StringBuilder();

            sqlString.AppendFormat(@"SELECT ItemID
FROM {2}
WHERE (ParentID = {1}) AND PublishmentSystemID = {0}
  ", publishmentSystemID, parentID, TABLE_NAME);

            //if (itemList.Count > 0)
            sqlString.AppendFormat("   and ItemID in ({1})", parentID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(itemList));
            //else
            // sqlString.AppendFormat(" AND ParentID = {0}", parentID);

            sqlString.Append(" ORDER BY Taxis");
            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString.ToString()))
            {
                while (rdr.Read())
                {
                    int itemID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(itemID);
                }
                rdr.Close();
            }

            return arraylist;
        }
        #endregion
    }
}
