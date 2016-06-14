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
    public class InputClassifyDAO : TreeDAO, IInputClassifyDAO
    {
        public InputClassifyDAO()
            : base(TABLE_NAME)
        {

        }

        public const string TABLE_NAME = "siteserver_InputClassify";

        public const string PARM_ADD_DATE = "@AddDate";

        /// <summary>
        /// 使用事务添加节点信息到InputClassify表中
        /// </summary>
        /// <param name="parentInputClassifyInfo">父节点</param>
        /// <param name="inputClassifyInfo">需要添加的节点</param>
        /// <param name="trans"></param>
        private void InsertInputClassifyInfoWithTrans(InputClissifyInfo parentInputClassifyInfo, InputClissifyInfo inputClassifyInfo, IDbTransaction trans)
        {
            if (parentInputClassifyInfo != null)
            {
                if (parentInputClassifyInfo.ParentsPath.Length == 0)
                {
                    inputClassifyInfo.ParentsPath = parentInputClassifyInfo.ItemID.ToString();
                }
                else
                {
                    inputClassifyInfo.ParentsPath = parentInputClassifyInfo.ParentsPath + "," + parentInputClassifyInfo.ItemID;
                }
                inputClassifyInfo.ParentsCount = parentInputClassifyInfo.ParentsCount + 1;

                int maxTaxis = base.GetMaxTaxisByParentPath(inputClassifyInfo.ParentsPath);
                if (maxTaxis == 0)
                {
                    maxTaxis = parentInputClassifyInfo.Taxis;
                }
                inputClassifyInfo.Taxis = maxTaxis + 1;
            }
            else
            {
                inputClassifyInfo.ParentsPath = "0";
                inputClassifyInfo.Taxis = 1;
            }

            string SQL_INSERT_ITEM = "INSERT INTO " + TABLE_NAME + " (ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis, AddDate) VALUES (@ItemName, @ItemIndexName, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @ContentNum, @PublishmentSystemID, @Enabled, @IsLastItem, @Taxis, @AddDate)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT_ITEM = "INSERT INTO " + TABLE_NAME + " (ItemID, ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis, AddDate) VALUES (siteserver_KeywordClassify_SEQ.NEXTVAL, @ItemName, @ItemIndexName, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @ContentNum, @PublishmentSystemID, @Enabled, @IsLastItem, @Taxis, @AddDate)";
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ITEM_NAME,EDataType.NVarChar,50,inputClassifyInfo.ItemName),
                this.GetParameter(PARM_ITEM_INDEX_NAME,EDataType.NVarChar,50,inputClassifyInfo.ItemIndexName),
                this.GetParameter(PARM_PARENT_ID,EDataType.Integer,inputClassifyInfo.ParentID),
                this.GetParameter(PARM_PARENTS_PATH,EDataType.NVarChar,255,inputClassifyInfo.ParentsPath),
                this.GetParameter(PARM_PARENTS_COUNT,EDataType.Integer,inputClassifyInfo.ParentsCount),
                this.GetParameter(PARM_CHILDREN_COUNT,EDataType.Integer,inputClassifyInfo.ChildrenCount),
                this.GetParameter(PARM_CONTENT_NUM,EDataType.Integer,inputClassifyInfo.ContentNum),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID,EDataType.Integer,inputClassifyInfo.PublishmentSystemID),
                this.GetParameter(PARM_ENABLED,EDataType.VarChar,18,inputClassifyInfo.Enabled.ToString()),
                this.GetParameter(PARM_IS_LAST_ITEM,EDataType.VarChar,18,inputClassifyInfo.IsLastItem.ToString()),
                this.GetParameter(PARM_TAXIS,EDataType.Integer,inputClassifyInfo.Taxis),
                this.GetParameter(PARM_ADD_DATE,EDataType.DateTime,inputClassifyInfo.AddDate)
            };

            if (inputClassifyInfo.ParentID > 0)
            {
                string sqlString = string.Format("UPDATE " + TABLE_NAME + " SET Taxis = Taxis + 1 WHERE (Taxis >= {0}) AND (PublishmentSystemID = {1})", inputClassifyInfo.Taxis, inputClassifyInfo.PublishmentSystemID);
                this.ExecuteNonQuery(trans, sqlString);
            }
            this.ExecuteNonQuery(trans, SQL_INSERT_ITEM, insertParms);

            inputClassifyInfo.ItemID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, TABLE_NAME);

            if (inputClassifyInfo.ParentsPath != null && inputClassifyInfo.ParentsPath.Length > 0)
            {
                string sqlString = string.Concat("UPDATE " + TABLE_NAME + " SET ChildrenCount = ChildrenCount + 1 WHERE ItemID in (", inputClassifyInfo.ParentsPath, ")");

                this.ExecuteNonQuery(trans, sqlString);
            }

            string SQL_UPDATE_IS_LAST_ITEM = "UPDATE " + TABLE_NAME + " SET IsLastItem = @IsLastItem WHERE ParentID = @ParentID";

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_IS_LAST_ITEM, EDataType.VarChar, 18, false.ToString()),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, inputClassifyInfo.ParentID)
            };

            this.ExecuteNonQuery(trans, SQL_UPDATE_IS_LAST_ITEM, parms);

            SQL_UPDATE_IS_LAST_ITEM = string.Format("UPDATE " + TABLE_NAME + " SET IsLastItem = '{0}' WHERE (ItemID IN (SELECT TOP 1 ItemID FROM " + TABLE_NAME + " WHERE ParentID = {1} ORDER BY Taxis DESC))", true.ToString(), inputClassifyInfo.ParentID);
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_UPDATE_IS_LAST_ITEM = string.Format(@"UPDATE {3} SET IsLastItem = '{0}' WHERE (ItemID IN (
SELECT ItemID FROM (
    SELECT ItemID FROM {3} WHERE ParentID = {1} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", true.ToString(), inputClassifyInfo.ParentID, TABLE_NAME);
            }
            this.ExecuteNonQuery(trans, SQL_UPDATE_IS_LAST_ITEM);

        }

        public int InsertInputClassifyInfo(int publishmentSystemID, int parentID, string itemName, string itemIndexName)
        {
            //if (publishmentSystemID > 0 && parentID == 0) return 0;
            InputClissifyInfo inputClassifyInfo = new InputClissifyInfo();
            inputClassifyInfo.ParentID = parentID;
            inputClassifyInfo.PublishmentSystemID = publishmentSystemID;
            inputClassifyInfo.ItemName = itemName;
            inputClassifyInfo.ItemIndexName = itemIndexName;
            inputClassifyInfo.AddDate = DateTime.Now;
            inputClassifyInfo.Enabled = true;
            inputClassifyInfo.ParentsPath = "0";

            InputClissifyInfo parentInputClassifyInfo = this.GetInputClassifyInfo(parentID);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.InsertInputClassifyInfoWithTrans(parentInputClassifyInfo, inputClassifyInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return inputClassifyInfo.ItemID;
        }

        public int InsertInputClassifyInfo(InputClissifyInfo inputClassifyInfo)
        {
            //if (inputClassifyInfo.PublishmentSystemID > 0 && inputClassifyInfo.ParentID == 0) return 0;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        InputClissifyInfo parentInputClassifyInfo = this.GetInputClassifyInfo(inputClassifyInfo.ParentID);

                        this.InsertInputClassifyInfoWithTrans(parentInputClassifyInfo, inputClassifyInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return inputClassifyInfo.ItemID;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="inputClassifyInfo"></param>
        public void UpdateInputClassifyInfo(InputClissifyInfo inputClassifyInfo)
        {

            string SQL_UPDATE_ITEM = "UPDATE " + TABLE_NAME + " SET ItemName=@ItemName, ItemIndexName=@ItemIndexName, ParentID=@ParentID, ParentsPath=@ParentsPath, ParentsCount=@ParentsCount, ChildrenCount=@ChildrenCount, ContentNum=@ContentNum, PublishmentSystemID=@PublishmentSystemID, Enabled=@Enabled, IsLastItem=@IsLastItem, Taxis=@Taxis, AddDate=@AddDate WHERE ItemID=@ItemID";

            IDbDataParameter[] updateParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ITEM_NAME,EDataType.NVarChar,50,inputClassifyInfo.ItemName),
                this.GetParameter(PARM_ITEM_INDEX_NAME,EDataType.NVarChar,50,inputClassifyInfo.ItemIndexName),
                this.GetParameter(PARM_PARENT_ID,EDataType.Integer,inputClassifyInfo.ParentID),
                this.GetParameter(PARM_PARENTS_PATH,EDataType.NVarChar,255,inputClassifyInfo.ParentsPath),
                this.GetParameter(PARM_PARENTS_COUNT,EDataType.Integer,inputClassifyInfo.ParentsCount),
                this.GetParameter(PARM_CHILDREN_COUNT,EDataType.Integer,inputClassifyInfo.ChildrenCount),
                this.GetParameter(PARM_CONTENT_NUM,EDataType.Integer,inputClassifyInfo.ContentNum),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID,EDataType.Integer,inputClassifyInfo.PublishmentSystemID),
                this.GetParameter(PARM_ENABLED,EDataType.VarChar,18,inputClassifyInfo.Enabled.ToString()),
                this.GetParameter(PARM_IS_LAST_ITEM,EDataType.VarChar,18,inputClassifyInfo.IsLastItem.ToString()),
                this.GetParameter(PARM_TAXIS,EDataType.Integer,inputClassifyInfo.Taxis),
                this.GetParameter(PARM_ADD_DATE,EDataType.DateTime,inputClassifyInfo.AddDate),
                this.GetParameter(PARM_ITEM_ID,EDataType.Integer,inputClassifyInfo.ItemID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_ITEM, updateParms);

        }

        public InputClissifyInfo GetInputClassifyInfo(int itemID)
        {
            string SQL_SELECT_ITEM = string.Format("SELECT ItemID, ItemName, ItemIndexName, ParentID, ParentsPath, ParentsCount, ChildrenCount, ContentNum, PublishmentSystemID, Enabled, IsLastItem, Taxis, AddDate FROM {0} WHERE ItemID = @ItemID", TABLE_NAME);

            InputClissifyInfo item = null;

            IDbDataParameter[] itemParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_ITEM_ID, EDataType.Integer, itemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_ITEM, itemParms))
            {
                if (rdr.Read())
                {
                    item = new InputClissifyInfo(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), TranslateUtils.ToBool(rdr.GetString(10)), rdr.GetInt32(11), TranslateUtils.ToDateTime(rdr.GetValue(12).ToString()));
                }
                rdr.Close();
            }
            return item;
        }

        public void Delete(int itemID)
        {
            InputClissifyInfo itemInfo = this.GetInputClassifyInfo(itemID);

            ArrayList itemIDArrayList = new ArrayList();
            if (itemInfo.ChildrenCount > 0)
            {
                itemIDArrayList = this.GetItemIDArrayListForDescendant(itemID);
            }
            itemIDArrayList.Add(itemID);

            string DELETE_CMD = string.Format("DELETE FROM {1} WHERE ItemID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(itemIDArrayList), TABLE_NAME);

            int deletedNum = 0;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        deletedNum = this.ExecuteNonQuery(trans, DELETE_CMD);

                        string TAXIS_CMD = string.Format("UPDATE {3} SET Taxis = Taxis - {0} WHERE (Taxis > {1}) AND (PublishmentSystemID = {2})", deletedNum, itemInfo.Taxis, itemInfo.PublishmentSystemID, TABLE_NAME);
                        this.ExecuteNonQuery(trans, TAXIS_CMD);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            //删除内容
            foreach (int id in itemIDArrayList)
                DataProvider.InputDAO.DeleteByClassifyID(id);

            //将下级的父级修改成自己的父级
            // string sql = string.Format("update dbo.siteserver_InputClassify set ParentID={0},ParentsPath={1},ParentsCount={2} where ItemID in ( select ItemID from siteserver_InputClassify where ParentID={3} and PublishmentSystemID={4}) and PublishmentSystemID={4} ;",);

            this.UpdateIsLastItem(itemInfo.ParentID);
            this.UpdateSubtractChildrenCount(itemInfo.ParentsPath, deletedNum);
        }


        /// <summary>
        /// 修改分类下表单数量
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="classifyID"></param>
        /// <param name="type"></param>
        public void UpdateInputCount(int publishmentSystemID, int classifyID, int type)
        {
            string i = "ContentNum";
            if (type == 1)
                i = "ContentNum+1";
            else
                i = "ContentNum -1";

            string ContentNum_SQL = string.Format("update {0} set ContentNum={1} where PublishmentSystemID={2} and ItemID={3} ", TABLE_NAME, i, publishmentSystemID, classifyID);
            this.ExecuteNonQuery(ContentNum_SQL);

        }
        public int SetDefaultInfo(int publishmentSystemID)
        {
            int contentID = 0;
            if (this.GetItemIDArrayListByParentID(publishmentSystemID, 0).Count == 0)
            {
                contentID = this.InsertInputClassifyInfo(publishmentSystemID, 0, "全部分类", "全部分类");

                int cid = this.InsertInputClassifyInfo(publishmentSystemID, contentID, "默认分类", "默认分类");

                #region 将默认分类的权限添加给角色

                ArrayList roles = DataProvider.SystemPermissionsDAO.GetSystemPermissionsInfoArrayListByPublishmentSystemID(publishmentSystemID, string.Format(" and WebsitePermissions  like '%{0}%' ", AppManager.CMS.Permission.WebSite.Input));

                foreach (SystemPermissionsInfo role in roles)
                {
                    if (!EPredefinedRoleUtils.IsPredefinedRole(role.RoleName))
                    {
                        //获取角色原有权限  
                        ArrayList websitePermissionArrayList = TranslateUtils.StringCollectionToArrayList(role.WebsitePermissions);
                        websitePermissionArrayList.Add(AppManager.CMS.Permission.WebSite.InputClassifyEdit + "_" + cid);
                        websitePermissionArrayList.Add(AppManager.CMS.Permission.WebSite.InputClassifyView + "_" + cid);

                        role.WebsitePermissions = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(websitePermissionArrayList);

                        DataProvider.SystemPermissionsDAO.Update(role);
                    }

                }
                #endregion
            }
            return contentID;
        }

        public InputClissifyInfo GetDefaultInfo(int publishmentSystemID)
        {
            InputClissifyInfo info = null;
            string SQL_WHERE = string.Format("WHERE PublishmentSystemID={0} and  ParentID = 0", publishmentSystemID);
            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(TABLE_NAME, SqlUtils.Asterisk, SQL_WHERE);

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
            {
                if (rdr.Read())
                {
                    info = new InputClissifyInfo(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetInt32(6), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), TranslateUtils.ToBool(rdr.GetString(10)), rdr.GetInt32(11), TranslateUtils.ToDateTime(rdr.GetValue(12).ToString()));
                    // info = new InputClissifyInfo();
                    //BaiRongDataProvider.DatabaseDAO.ReadResultsToExtendedAttributes(rdr, info);
                }
                rdr.Close();
            }

            return info;
        }

        /// <summary>
        /// 修改某个分类的内容数量为所有表单的总数
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="id"></param>
        public void UpdateCountByAll(int publishmentSystemID, int id)
        {
            string updateSQL = string.Format(" update siteserver_InputClassify set ContentNum=( select COUNT(*) from siteserver_Input where  PublishmentSystemID={0} ) where PublishmentSystemID={0} and ItemID ={1} ", publishmentSystemID, id);

            this.ExecuteNonQuery(updateSQL);
        }


        /// <summary>
        /// 调整某个分类下表单数量及更新全部分类下表单的数量
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="classifyID"></param> 
        /// <param name="pid">全部分类ID</param> 
        public void UpdateInputCountByClassifyID(int publishmentSystemID, int classifyID, int pid)
        {
            string updateSQL = string.Format(" update siteserver_InputClassify set ContentNum=( select COUNT(*) from siteserver_Input where  PublishmentSystemID={0} and ClassifyID={1} ) where PublishmentSystemID={0} and ItemID ={1} ", publishmentSystemID, classifyID);
            updateSQL = updateSQL + string.Format(" update siteserver_InputClassify set ContentNum=( select COUNT(*) from siteserver_Input where  PublishmentSystemID={0} ) where PublishmentSystemID={0} and ItemID ={1} ", publishmentSystemID, pid);
            this.ExecuteNonQuery(updateSQL);

        }
    }
}
