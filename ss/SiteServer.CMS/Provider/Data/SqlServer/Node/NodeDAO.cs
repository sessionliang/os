using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using BaiRong.Core;
using BaiRong.Core.Data;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using System.Collections.Generic;


namespace SiteServer.CMS.Provider.Data.SqlServer
{
    public class NodeDAO : DataProviderBase, INodeDAO
    {
        public string TableName
        {
            get
            {
                return "siteserver_Node";
            }
        }

        #region 私有属性及方法

        private const string SQL_SELECT_NODE = "SELECT NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues FROM siteserver_Node WHERE NodeID = @NodeID";

        private const string SQL_SELECT_NODE_BY_LAST_ADD_DATE = "SELECT NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues FROM siteserver_Node WHERE ParentID = @ParentID ORDER BY AddDate Desc";

        private const string SQL_SELECT_NODE_ID = "SELECT NodeID FROM siteserver_Node WHERE NodeID = @NodeID";

        private const string SQL_SELECT_NODE_BY_TAXIS = "SELECT NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues FROM siteserver_Node WHERE ParentID = @ParentID ORDER BY Taxis";

        private const string SQL_SELECT_NODE_BY_PARENT_ID_AND_CONTENT_MODEL_ID = "SELECT NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues FROM siteserver_Node WHERE (PublishmentSystemID = @PublishmentSystemID OR NodeID = @PublishmentSystemID) AND ContentModelID = @ContentModelID";

        private const string SQL_SELECT_NODE_GROUP_NAME_COLLECTION = "SELECT NodeGroupNameCollection FROM siteserver_Node WHERE NodeID = @NodeID";

        private const string SQL_SELECT_PARENT_ID = "SELECT ParentID FROM siteserver_Node WHERE NodeID = @NodeID";

        private const string SQL_SELECT_NODE_COUNT = "SELECT COUNT(*) FROM siteserver_Node WHERE ParentID = @ParentID";

        private const string SQL_SELECT_PUBLISHMENT_SYSTEM_ID_BY_ID = "SELECT PublishmentSystemID FROM siteserver_Node WHERE NodeID = @NodeID";

        private const string SQL_SELECT_NODE_INDEX_NAME_COLLECTION = "SELECT DISTINCT NodeIndexName FROM siteserver_Node WHERE PublishmentSystemID = @PublishmentSystemID";

        private const string SQL_SELECT_NODE_ID_BY_INDEX = "SELECT NodeID FROM siteserver_Node WHERE (PublishmentSystemID = @PublishmentSystemID OR NodeID = @PublishmentSystemID) AND NodeIndexName = @NodeIndexName";

        private const string SQL_SELECT_NODE_ID_BY_CONTENT_MODEL_ID = "SELECT NodeID FROM siteserver_Node WHERE (PublishmentSystemID = @PublishmentSystemID OR NodeID = @PublishmentSystemID) AND ContentModelID = @ContentModelID";

        private const string SQL_UPDATE_NODE = "UPDATE siteserver_Node SET NodeName = @NodeName, NodeType = @NodeType, ContentModelID = @ContentModelID, ParentsPath = @ParentsPath, ParentsCount = @ParentsCount, ChildrenCount = @ChildrenCount, IsLastNode = @IsLastNode, NodeIndexName = @NodeIndexName, NodeGroupNameCollection = @NodeGroupNameCollection, ImageUrl = @ImageUrl, Content = @Content, ContentNum = @ContentNum, CommentNum = @CommentNum, FilePath = @FilePath, ChannelFilePathRule = @ChannelFilePathRule, ContentFilePathRule = @ContentFilePathRule, LinkUrl = @LinkUrl,LinkType = @LinkType, ChannelTemplateID = @ChannelTemplateID, ContentTemplateID = @ContentTemplateID, Keywords = @Keywords, Description = @Description, ExtendValues = @ExtendValues WHERE NodeID = @NodeID";

        private const string SQL_UPDATE_NODE_GROUP_NAME_COLLECTION = "UPDATE siteserver_Node SET NodeGroupNameCollection = @NodeGroupNameCollection WHERE NodeID = @NodeID";

        private const string PARM_NODE_ID = "@NodeID";
        private const string PARM_NODE_NAME = "@NodeName";
        private const string PARM_NODE_TYPE = "@NodeType";
        private const string PARM_PUBLISHMENT_SYSTEM_ID = "@PublishmentSystemID";
        private const string PARM_CONTENT_MODEL_ID = "@ContentModelID";
        private const string PARM_PARENT_ID = "@ParentID";
        private const string PARM_PARENTS_PATH = "@ParentsPath";
        private const string PARM_PARENTS_COUNT = "@ParentsCount";
        private const string PARM_CHILDREN_COUNT = "@ChildrenCount";
        private const string PARM_IS_LAST_NODE = "@IsLastNode";
        private const string PARM_NODE_INDEX_NAME = "@NodeIndexName";
        private const string PARM_NODE_GROUP_NAME_COLLECTION = "@NodeGroupNameCollection";
        private const string PARM_TAXIS = "@Taxis";
        private const string PARM_ADD_DATE = "@AddDate";
        private const string PARM_IMAGE_URL = "@ImageUrl";
        private const string PARM_CONTENT = "@Content";
        private const string PARM_CONTENT_NUM = "@ContentNum";
        private const string PARM_COMMENT_NUM = "@CommentNum";
        private const string PARM_FILE_PATH = "@FilePath";
        private const string PARM_CHANNEL_FILE_PATH_RULE = "@ChannelFilePathRule";
        private const string PARM_CONTENT_FILE_PATH_RULE = "@ContentFilePathRule";
        private const string PARM_LINK_URL = "@LinkUrl";
        private const string PARM_LINK_TYPE = "@LinkType";
        private const string PARM_CHANNEL_TEMPLATE_ID = "@ChannelTemplateID";
        private const string PARM_CONTENT_TEMPLATE_ID = "@ContentTemplateID";
        private const string PARM_KEYWORDS = "@Keywords";
        private const string PARM_DESCRIPTION = "@Description";
        private const string PARM_EXTEND_VALUES = "@ExtendValues";

        /// <summary>
        /// 使用事务添加节点信息到Node表中
        /// </summary>
        /// <param name="parentNodeInfo">父节点</param>
        /// <param name="nodeInfo">需要添加的节点</param>
        /// <param name="trans"></param>
        private void InsertNodeInfoWithTrans(NodeInfo parentNodeInfo, NodeInfo nodeInfo, IDbTransaction trans)
        {
            if (parentNodeInfo != null)
            {
                if (parentNodeInfo.NodeType == ENodeType.BackgroundPublishNode)
                {
                    nodeInfo.PublishmentSystemID = parentNodeInfo.NodeID;
                }
                else
                {
                    nodeInfo.PublishmentSystemID = parentNodeInfo.PublishmentSystemID;
                }
                if (parentNodeInfo.ParentsPath.Length == 0)
                {
                    nodeInfo.ParentsPath = parentNodeInfo.NodeID.ToString();
                }
                else
                {
                    nodeInfo.ParentsPath = parentNodeInfo.ParentsPath + "," + parentNodeInfo.NodeID;
                }
                nodeInfo.ParentsCount = parentNodeInfo.ParentsCount + 1;

                int maxTaxis = this.GetMaxTaxisByParentPath(nodeInfo.ParentsPath);
                if (maxTaxis == 0)
                {
                    maxTaxis = parentNodeInfo.Taxis;
                }
                nodeInfo.Taxis = maxTaxis + 1;
            }
            else
            {
                nodeInfo.Taxis = 1;
            }

            string SQL_INSERT_NODE = "INSERT INTO siteserver_Node (NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues) VALUES (@NodeName, @NodeType, @PublishmentSystemID, @ContentModelID, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @IsLastNode, @NodeIndexName, @NodeGroupNameCollection, @Taxis, @AddDate, @ImageUrl, @Content, @ContentNum, @CommentNum, @FilePath, @ChannelFilePathRule, @ContentFilePathRule, @LinkUrl, @LinkType, @ChannelTemplateID, @ContentTemplateID, @Keywords, @Description, @ExtendValues)";
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_INSERT_NODE = "INSERT INTO siteserver_Node (NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues) VALUES (siteserver_Node_SEQ.NEXTVAL, @NodeName, @NodeType, @PublishmentSystemID, @ContentModelID, @ParentID, @ParentsPath, @ParentsCount, @ChildrenCount, @IsLastNode, @NodeIndexName, @NodeGroupNameCollection, @Taxis, @AddDate, @ImageUrl, @Content, @ContentNum, @CommentNum, @FilePath, @ChannelFilePathRule, @ContentFilePathRule, @LinkUrl, @LinkType, @ChannelTemplateID, @ContentTemplateID, @Keywords, @Description, @ExtendValues)";
            }

            IDbDataParameter[] insertParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_NODE_NAME, EDataType.NVarChar, 255, nodeInfo.NodeName),
                this.GetParameter(PARM_NODE_TYPE, EDataType.VarChar, 50, ENodeTypeUtils.GetValue(nodeInfo.NodeType)),
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, nodeInfo.PublishmentSystemID),
                this.GetParameter(PARM_CONTENT_MODEL_ID, EDataType.VarChar, 50, nodeInfo.ContentModelID),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, nodeInfo.ParentID),
                this.GetParameter(PARM_PARENTS_PATH, EDataType.NVarChar, 255, nodeInfo.ParentsPath),
                this.GetParameter(PARM_PARENTS_COUNT, EDataType.Integer, nodeInfo.ParentsCount),
                this.GetParameter(PARM_CHILDREN_COUNT, EDataType.Integer, 0),
                this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, true.ToString()),
                this.GetParameter(PARM_NODE_INDEX_NAME, EDataType.NVarChar, 255, nodeInfo.NodeIndexName),
                this.GetParameter(PARM_NODE_GROUP_NAME_COLLECTION, EDataType.NVarChar, 255, nodeInfo.NodeGroupNameCollection),
                this.GetParameter(PARM_TAXIS, EDataType.Integer, nodeInfo.Taxis),
                this.GetParameter(PARM_ADD_DATE, EDataType.DateTime, nodeInfo.AddDate),
                this.GetParameter(PARM_IMAGE_URL, EDataType.VarChar, 200, nodeInfo.ImageUrl),
                this.GetParameter(PARM_CONTENT, EDataType.NText, nodeInfo.Content),
                this.GetParameter(PARM_CONTENT_NUM, EDataType.Integer, nodeInfo.ContentNum),
                this.GetParameter(PARM_COMMENT_NUM, EDataType.Integer, nodeInfo.CommentNum),
                this.GetParameter(PARM_FILE_PATH, EDataType.VarChar, 200, nodeInfo.FilePath),
                this.GetParameter(PARM_CHANNEL_FILE_PATH_RULE, EDataType.VarChar, 200, nodeInfo.ChannelFilePathRule),
                this.GetParameter(PARM_CONTENT_FILE_PATH_RULE, EDataType.VarChar, 200, nodeInfo.ContentFilePathRule),
                this.GetParameter(PARM_LINK_URL, EDataType.VarChar, 200, nodeInfo.LinkUrl),
                this.GetParameter(PARM_LINK_TYPE, EDataType.VarChar, 200, ELinkTypeUtils.GetValue(nodeInfo.LinkType)),
                this.GetParameter(PARM_CHANNEL_TEMPLATE_ID, EDataType.Integer, nodeInfo.ChannelTemplateID),
                this.GetParameter(PARM_CONTENT_TEMPLATE_ID, EDataType.Integer, nodeInfo.ContentTemplateID),
                this.GetParameter(PARM_KEYWORDS, EDataType.NVarChar, 255, nodeInfo.Keywords),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, nodeInfo.Description),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, nodeInfo.Additional.ToString())
            };

            if (nodeInfo.PublishmentSystemID != 0)
            {
                string sqlString = string.Format("UPDATE siteserver_Node SET Taxis = Taxis + 1 WHERE (Taxis >= {0}) AND (PublishmentSystemID = {1})", nodeInfo.Taxis, nodeInfo.PublishmentSystemID);
                this.ExecuteNonQuery(trans, sqlString);
            }
            this.ExecuteNonQuery(trans, SQL_INSERT_NODE, insertParms);

            nodeInfo.NodeID = BaiRongDataProvider.DatabaseDAO.GetSequence(trans, "siteserver_Node");

            if (nodeInfo.ParentsPath != null && nodeInfo.ParentsPath.Length > 0)
            {
                string sqlString = string.Concat("UPDATE siteserver_Node SET ChildrenCount = ChildrenCount + 1 WHERE NodeID in (", nodeInfo.ParentsPath, ")");

                this.ExecuteNonQuery(trans, sqlString);
            }

            string SQL_UPDATE_IS_LAST_NODE = "UPDATE siteserver_Node SET IsLastNode = @IsLastNode WHERE ParentID = @ParentID";

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, false.ToString()),
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, nodeInfo.ParentID)
            };

            this.ExecuteNonQuery(trans, SQL_UPDATE_IS_LAST_NODE, parms);

            SQL_UPDATE_IS_LAST_NODE = string.Format("UPDATE siteserver_Node SET IsLastNode = '{0}' WHERE (NodeID IN (SELECT TOP 1 NodeID FROM siteserver_Node WHERE ParentID = {1} ORDER BY Taxis DESC))", true.ToString(), nodeInfo.ParentID);
            if (this.DataBaseType == EDatabaseType.Oracle)
            {
                SQL_UPDATE_IS_LAST_NODE = string.Format(@"UPDATE siteserver_Node SET IsLastNode = '{0}' WHERE (NodeID IN (
SELECT NodeID FROM (
    SELECT NodeID FROM siteserver_Node WHERE ParentID = {1} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", true.ToString(), nodeInfo.ParentID);
            }
            this.ExecuteNonQuery(trans, SQL_UPDATE_IS_LAST_NODE);

            //OwningNodeIDCache.IsChanged = true;
            NodeManager.RemoveCache(nodeInfo.PublishmentSystemID);
            if (!PermissionsManager.Current.IsSystemAdministrator)
            {
                ProductPermissionsManager.Current.ClearCache();
            }
        }


        /// <summary>
        /// 将节点数减1
        /// </summary>
        /// <param name="parentsPath"></param>
        /// <param name="subtractNum"></param>
        private void UpdateSubtractChildrenCount(string parentsPath, int subtractNum)
        {
            if (!string.IsNullOrEmpty(parentsPath))
            {
                string sqlString = string.Concat("UPDATE siteserver_Node SET ChildrenCount = ChildrenCount - ", subtractNum, " WHERE NodeID in (", parentsPath, ")");
                this.ExecuteNonQuery(sqlString);
            }
        }


        /// <summary>
        /// 更新发布系统下的所有节点的排序号
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        private void UpdateWholeTaxisByPublishmentSystemID(int publishmentSystemID)
        {
            if (publishmentSystemID <= 0) return;
            ArrayList nodeIDArrayList = new ArrayList();
            nodeIDArrayList.Add(publishmentSystemID);
            int level = 0;
            string SELECT_LEVEL_CMD = string.Format("SELECT MAX(ParentsCount) FROM siteserver_Node WHERE (NodeID = {0}) OR (PublishmentSystemID = {0})", publishmentSystemID);
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
                ArrayList arraylist = new ArrayList(nodeIDArrayList);
                foreach (int savedNodeID in arraylist)
                {
                    int lastChildNodeIDOfSavedNodeID = savedNodeID;
                    string SELECT_NODE_CMD = string.Format("SELECT NodeID, NodeName FROM siteserver_Node WHERE ParentID = {0} ORDER BY Taxis, IsLastNode", savedNodeID);
                    using (IDataReader rdr = this.ExecuteReader(SELECT_NODE_CMD))
                    {
                        while (rdr.Read())
                        {
                            int nodeID = Convert.ToInt32(rdr[0]);
                            if (!nodeIDArrayList.Contains(nodeID))
                            {
                                int index = nodeIDArrayList.IndexOf(lastChildNodeIDOfSavedNodeID);
                                nodeIDArrayList.Insert(index + 1, nodeID);
                                lastChildNodeIDOfSavedNodeID = nodeID;
                            }
                        }
                        rdr.Close();
                    }
                }
            }


            for (int i = 1; i <= nodeIDArrayList.Count; i++)
            {
                int nodeID = (int)nodeIDArrayList[i - 1];
                string UPDATE_CMD = string.Format("UPDATE siteserver_Node SET Taxis = {0} WHERE NodeID = {1}", i, nodeID);
                this.ExecuteNonQuery(UPDATE_CMD);
            }
        }


        /// <summary>
        /// Change The Texis To Lowerer Level
        /// </summary>
        private void TaxisSubtract(int publishmentSystemID, int selectedNodeID)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, selectedNodeID);
            if (nodeInfo == null || nodeInfo.NodeType == ENodeType.BackgroundPublishNode || nodeInfo.PublishmentSystemID == 0) return;
            this.UpdateWholeTaxisByPublishmentSystemID(nodeInfo.PublishmentSystemID);
            //Get Lower Taxis and NodeID
            int lowerNodeID = 0;
            int lowerChildrenCount = 0;
            string lowerParentsPath = "";
            string sqlString = @"SELECT TOP 1 NodeID, ChildrenCount, ParentsPath
FROM siteserver_Node
WHERE (ParentID = @ParentID) AND (NodeID <> @NodeID) AND (Taxis < @Taxis) AND (PublishmentSystemID = @PublishmentSystemID)
ORDER BY Taxis DESC";

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(NodeDAO.PARM_PARENT_ID, EDataType.Integer, nodeInfo.ParentID),
                this.GetParameter(NodeDAO.PARM_NODE_ID, EDataType.Integer, nodeInfo.NodeID),
                this.GetParameter(NodeDAO.PARM_TAXIS, EDataType.Integer, nodeInfo.Taxis),
                this.GetParameter(NodeDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, nodeInfo.PublishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    lowerNodeID = Convert.ToInt32(rdr[0]);
                    lowerChildrenCount = Convert.ToInt32(rdr[1]);
                    lowerParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            string lowerNodePath = "";
            if (lowerParentsPath == "")
            {
                lowerNodePath = lowerNodeID.ToString();
            }
            else
            {
                lowerNodePath = String.Concat(lowerParentsPath, ",", lowerNodeID);
            }
            string selectedNodePath = "";
            if (nodeInfo.ParentsPath == "")
            {
                selectedNodePath = nodeInfo.NodeID.ToString();
            }
            else
            {
                selectedNodePath = String.Concat(nodeInfo.ParentsPath, ",", nodeInfo.NodeID);
            }

            this.SetTaxisSubtract(selectedNodeID, selectedNodePath, lowerChildrenCount + 1);
            this.SetTaxisAdd(lowerNodeID, lowerNodePath, nodeInfo.ChildrenCount + 1);

            this.UpdateIsLastNode(nodeInfo.ParentID);

        }

        /// <summary>
        /// Change The Texis To Higher Level
        /// </summary>
        private void TaxisAdd(int publishmentSystemID, int selectedNodeID)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, selectedNodeID);
            if (nodeInfo == null || nodeInfo.NodeType == ENodeType.BackgroundPublishNode || nodeInfo.PublishmentSystemID == 0) return;
            this.UpdateWholeTaxisByPublishmentSystemID(nodeInfo.PublishmentSystemID);
            //Get Higher Taxis and NodeID
            int higherNodeID = 0;
            int higherChildrenCount = 0;
            string higherParentsPath = "";
            string sqlString = @"SELECT TOP 1 NodeID, ChildrenCount, ParentsPath
FROM siteserver_Node
WHERE (ParentID = @ParentID) AND (NodeID <> @NodeID) AND (Taxis > @Taxis) AND (PublishmentSystemID = @PublishmentSystemID)
ORDER BY Taxis";

            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(NodeDAO.PARM_PARENT_ID, EDataType.Integer, nodeInfo.ParentID),
                this.GetParameter(NodeDAO.PARM_NODE_ID, EDataType.Integer, nodeInfo.NodeID),
                this.GetParameter(NodeDAO.PARM_TAXIS, EDataType.Integer, nodeInfo.Taxis),
                this.GetParameter(NodeDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, nodeInfo.PublishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(sqlString, parms))
            {
                if (rdr.Read())
                {
                    higherNodeID = Convert.ToInt32(rdr[0]);
                    higherChildrenCount = Convert.ToInt32(rdr[1]);
                    higherParentsPath = rdr.GetValue(2).ToString();
                }
                else
                {
                    return;
                }
                rdr.Close();
            }


            string higherNodePath = string.Empty;
            if (higherParentsPath == string.Empty)
            {
                higherNodePath = higherNodeID.ToString();
            }
            else
            {
                higherNodePath = String.Concat(higherParentsPath, ",", higherNodeID);
            }
            string selectedNodePath = string.Empty;
            if (nodeInfo.ParentsPath == string.Empty)
            {
                selectedNodePath = nodeInfo.NodeID.ToString();
            }
            else
            {
                selectedNodePath = String.Concat(nodeInfo.ParentsPath, ",", nodeInfo.NodeID);
            }

            this.SetTaxisAdd(selectedNodeID, selectedNodePath, higherChildrenCount + 1);
            this.SetTaxisSubtract(higherNodeID, higherNodePath, nodeInfo.ChildrenCount + 1);

            this.UpdateIsLastNode(nodeInfo.ParentID);
        }

        private void SetTaxisAdd(int nodeID, string parentsPath, int AddNum)
        {
            string sqlString = string.Format("UPDATE siteserver_Node SET Taxis = Taxis + {0} WHERE NodeID = {1} OR ParentsPath = '{2}' OR ParentsPath like '{2},%'", AddNum, nodeID, parentsPath);

            this.ExecuteNonQuery(sqlString);
        }

        private void SetTaxisSubtract(int nodeID, string parentsPath, int SubtractNum)
        {
            string sqlString = string.Format("UPDATE siteserver_Node SET Taxis = Taxis - {0} WHERE  NodeID = {1} OR ParentsPath = '{2}' OR ParentsPath like '{2},%'", SubtractNum, nodeID, parentsPath);

            this.ExecuteNonQuery(sqlString);
        }


        private void UpdateIsLastNode(int parentID)
        {
            if (parentID > 0)
            {
                string sqlString = "UPDATE siteserver_Node SET IsLastNode = @IsLastNode WHERE  ParentID = @ParentID";

                IDbDataParameter[] parms = new IDbDataParameter[]
                {
                    this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, false.ToString()),
                    this.GetParameter(PARM_PARENT_ID, EDataType.Integer, parentID)
                };

                this.ExecuteNonQuery(sqlString, parms);

                sqlString = string.Format("UPDATE siteserver_Node SET IsLastNode = '{0}' WHERE (NodeID IN (SELECT TOP 1 NodeID FROM siteserver_Node WHERE ParentID = {1} ORDER BY Taxis DESC))", true.ToString(), parentID);
                if (this.DataBaseType == EDatabaseType.Oracle)
                {
                    sqlString = string.Format(@"UPDATE siteserver_Node SET IsLastNode = '{0}' WHERE (NodeID IN (
SELECT NodeID FROM (
    SELECT NodeID FROM siteserver_Node WHERE ParentID = {1} ORDER BY Taxis DESC
) WHERE ROWNUM <= 1
))", true.ToString(), parentID);
                }

                this.ExecuteNonQuery(sqlString);
            }
        }


        private int GetMaxTaxisByParentPath(string parentPath)
        {
            string CMD = string.Concat("SELECT MAX(Taxis) AS MaxTaxis FROM siteserver_Node WHERE (ParentsPath = '", parentPath, "') OR (ParentsPath like '", parentPath, ",%')");
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

        private string GetNodeGroupNameCollection(int nodeID)
        {
            string groupNameCollection = string.Empty;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_NODE_GROUP_NAME_COLLECTION, nodeParms))
            {
                if (rdr.Read())
                {
                    groupNameCollection = rdr.GetValue(0).ToString();
                }
                rdr.Close();
            }
            return groupNameCollection;
        }

        private int GetParentID(int nodeID)
        {
            int parentID = 0;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PARENT_ID, nodeParms))
            {
                if (rdr.Read())
                {
                    parentID = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }
            return parentID;
        }

        private int GetNodeIDByParentIDAndOrder(int parentID, int order)
        {
            int nodeID = parentID;

            string CMD = string.Format("SELECT NodeID FROM siteserver_Node WHERE (ParentID = {0}) ORDER BY Taxis", parentID);

            using (IDataReader rdr = this.ExecuteReader(CMD))
            {
                int index = 1;
                while (rdr.Read())
                {
                    nodeID = Convert.ToInt32(rdr[0]);
                    if (index == order)
                        break;
                    index++;
                }
                rdr.Close();
            }
            return nodeID;
        }

        #endregion

        # region 修改数据库

        public int InsertNodeInfo(int publishmentSystemID, int parentID, string nodeName, string nodeIndex, string contentModelID)
        {
            if (publishmentSystemID > 0 && parentID == 0) return 0;
            NodeInfo nodeInfo = new NodeInfo();
            nodeInfo.ParentID = parentID;
            nodeInfo.PublishmentSystemID = publishmentSystemID;
            nodeInfo.NodeName = nodeName;
            nodeInfo.NodeIndexName = nodeIndex;
            nodeInfo.ContentModelID = contentModelID;
            nodeInfo.AddDate = DateTime.Now;

            TemplateInfo defaultChannelTemplateInfo = TemplateManager.GetDefaultTemplateInfo(publishmentSystemID, ETemplateType.ChannelTemplate);
            TemplateInfo defaultContentTemplateInfo = TemplateManager.GetDefaultTemplateInfo(publishmentSystemID, ETemplateType.ContentTemplate);

            nodeInfo.ChannelTemplateID = defaultChannelTemplateInfo.TemplateID;
            nodeInfo.ContentTemplateID = defaultContentTemplateInfo.TemplateID;

            NodeInfo parentNodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, parentID);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.InsertNodeInfoWithTrans(parentNodeInfo, nodeInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return nodeInfo.NodeID;
        }

        public int InsertNodeInfo(int publishmentSystemID, int parentID, string nodeName, string nodeIndex, string contentModelID, int channelTemplateID, int contentTemplateID)
        {
            if (publishmentSystemID > 0 && parentID == 0) return 0;
            NodeInfo nodeInfo = new NodeInfo();
            nodeInfo.ParentID = parentID;
            nodeInfo.PublishmentSystemID = publishmentSystemID;
            nodeInfo.NodeName = nodeName;
            nodeInfo.NodeIndexName = nodeIndex;
            nodeInfo.ContentModelID = contentModelID;
            nodeInfo.AddDate = DateTime.Now;

            TemplateInfo defaultChannelTemplateInfo = TemplateManager.GetDefaultTemplateInfo(publishmentSystemID, ETemplateType.ChannelTemplate);
            TemplateInfo defaultContentTemplateInfo = TemplateManager.GetDefaultTemplateInfo(publishmentSystemID, ETemplateType.ContentTemplate);

            if (channelTemplateID > 0)
                nodeInfo.ChannelTemplateID = channelTemplateID;
            else
                nodeInfo.ChannelTemplateID = defaultChannelTemplateInfo.TemplateID;

            if (contentTemplateID > 0)
                nodeInfo.ContentTemplateID = contentTemplateID;
            else
                nodeInfo.ContentTemplateID = defaultContentTemplateInfo.TemplateID;

            NodeInfo parentNodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, parentID);

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.InsertNodeInfoWithTrans(parentNodeInfo, nodeInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return nodeInfo.NodeID;

        }

        public int InsertNodeInfo(NodeInfo nodeInfo)
        {
            if (nodeInfo.PublishmentSystemID > 0 && nodeInfo.ParentID == 0) return 0;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        NodeInfo parentNodeInfo = this.GetNodeInfo(nodeInfo.ParentID);

                        this.InsertNodeInfoWithTrans(parentNodeInfo, nodeInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            return nodeInfo.NodeID;
        }

        /// <summary>
        /// 添加后台发布节点
        /// </summary>
        public int InsertPublishmentSystemInfo(NodeInfo nodeInfo, PublishmentSystemInfo psInfo)
        {
            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.InsertNodeInfoWithTrans(null, nodeInfo, trans);

                        psInfo.PublishmentSystemID = nodeInfo.NodeID;

                        DataProvider.PublishmentSystemDAO.InsertWithTrans(psInfo, trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }

            PublishmentSystemManager.UpdateUrlRewriteFile();

            BaiRongDataProvider.AdministratorDAO.UpdateLastActivityDateAndPublishmentSystemID(AdminManager.Current.UserName, nodeInfo.NodeID);

            string updateNodeSqlString = string.Format("UPDATE siteserver_Node SET PublishmentSystemID = {0} WHERE NodeID = {0}", nodeInfo.NodeID);
            this.ExecuteNonQuery(updateNodeSqlString);

            DataProvider.TemplateDAO.CreateDefaultTemplateInfo(nodeInfo.NodeID);
            DataProvider.MenuDisplayDAO.CreateDefaultMenuDisplayInfo(nodeInfo.NodeID);
            return nodeInfo.NodeID;
        }


        /// <summary>
        /// 修改节点信息到Node表中
        /// </summary>
        /// <param name="nodeInfo"></param>
        public void UpdateNodeInfo(NodeInfo nodeInfo)
        {
            IDbDataParameter[] updateParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_NODE_NAME, EDataType.NVarChar, 255, nodeInfo.NodeName),
                this.GetParameter(PARM_NODE_TYPE, EDataType.VarChar, 50, ENodeTypeUtils.GetValue(nodeInfo.NodeType)),
                this.GetParameter(PARM_CONTENT_MODEL_ID, EDataType.VarChar, 50, nodeInfo.ContentModelID),
                this.GetParameter(PARM_PARENTS_PATH, EDataType.NVarChar, 255, nodeInfo.ParentsPath),
                this.GetParameter(PARM_PARENTS_COUNT, EDataType.Integer, nodeInfo.ParentsCount),
                this.GetParameter(PARM_CHILDREN_COUNT, EDataType.Integer, nodeInfo.ChildrenCount),
                this.GetParameter(PARM_IS_LAST_NODE, EDataType.VarChar, 18, nodeInfo.IsLastNode.ToString()),
                this.GetParameter(PARM_NODE_INDEX_NAME, EDataType.NVarChar, 255, nodeInfo.NodeIndexName),
                this.GetParameter(PARM_NODE_GROUP_NAME_COLLECTION, EDataType.NVarChar, 255, nodeInfo.NodeGroupNameCollection),
                this.GetParameter(PARM_IMAGE_URL, EDataType.VarChar, 200, nodeInfo.ImageUrl),
                this.GetParameter(PARM_CONTENT, EDataType.NText, nodeInfo.Content),
                this.GetParameter(PARM_CONTENT_NUM, EDataType.Integer, nodeInfo.ContentNum),
                this.GetParameter(PARM_COMMENT_NUM, EDataType.Integer, nodeInfo.CommentNum),
                this.GetParameter(PARM_FILE_PATH, EDataType.VarChar, 200, nodeInfo.FilePath),
                this.GetParameter(PARM_CHANNEL_FILE_PATH_RULE, EDataType.VarChar, 200, nodeInfo.ChannelFilePathRule),
                this.GetParameter(PARM_CONTENT_FILE_PATH_RULE, EDataType.VarChar, 200, nodeInfo.ContentFilePathRule),
                this.GetParameter(PARM_LINK_URL, EDataType.VarChar, 200, nodeInfo.LinkUrl),
                this.GetParameter(PARM_LINK_TYPE, EDataType.VarChar, 200, ELinkTypeUtils.GetValue(nodeInfo.LinkType)),
                this.GetParameter(PARM_CHANNEL_TEMPLATE_ID, EDataType.Integer, nodeInfo.ChannelTemplateID),
                this.GetParameter(PARM_CONTENT_TEMPLATE_ID, EDataType.Integer, nodeInfo.ContentTemplateID),
                this.GetParameter(PARM_KEYWORDS, EDataType.NVarChar, 255, nodeInfo.Keywords),
                this.GetParameter(PARM_DESCRIPTION, EDataType.NVarChar, 255, nodeInfo.Description),
                this.GetParameter(PARM_EXTEND_VALUES, EDataType.NText, nodeInfo.Additional.ToString()),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeInfo.NodeID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_NODE, updateParms);

            if (nodeInfo.NodeType == ENodeType.BackgroundPublishNode)
            {
                NodeManager.RemoveCache(nodeInfo.NodeID);
            }
            else
            {
                NodeManager.RemoveCache(nodeInfo.PublishmentSystemID);
            }
        }

        /// <summary>
        /// 修改排序
        /// </summary>
        public void UpdateTaxis(int publishmentSystemID, int selectedNodeID, bool isSubtract)
        {
            if (isSubtract)
            {
                this.TaxisSubtract(publishmentSystemID, selectedNodeID);
            }
            else
            {
                this.TaxisAdd(publishmentSystemID, selectedNodeID);
            }
            NodeManager.RemoveCache(publishmentSystemID);
        }

        private void UpdateNodeGroupNameCollection(int publishmentSystemID, int nodeID, string nodeGroupNameCollection)
        {
            IDbDataParameter[] parms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_NODE_GROUP_NAME_COLLECTION, EDataType.NVarChar, 255, nodeGroupNameCollection),
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
            };

            this.ExecuteNonQuery(SQL_UPDATE_NODE_GROUP_NAME_COLLECTION, parms);
            NodeManager.RemoveCache(publishmentSystemID);
        }

        public void AddNodeGroupArrayList(int publishmentSystemID, int nodeID, ArrayList nodeGroupArrayList)
        {
            ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(this.GetNodeGroupNameCollection(nodeID));
            foreach (string groupName in nodeGroupArrayList)
            {
                if (!arraylist.Contains(groupName)) arraylist.Add(groupName);
            }
            this.UpdateNodeGroupNameCollection(publishmentSystemID, nodeID, TranslateUtils.ObjectCollectionToString(arraylist));
        }

        public void UpdateContentNum(PublishmentSystemInfo publishmentSystemInfo)
        {
            ArrayList nodeIDArrayList = this.GetNodeIDArrayListByPublishmentSystemID(publishmentSystemInfo.PublishmentSystemID);
            foreach (int nodeID in nodeIDArrayList)
            {
                UpdateContentNum(publishmentSystemInfo, nodeID, false);
            }

            NodeManager.RemoveCache(publishmentSystemInfo.PublishmentSystemID);
        }

        public virtual void UpdateContentNum(PublishmentSystemInfo publishmentSystemInfo, int nodeID, bool isRemoveCache)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
            string sqlString = string.Empty;
            string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);
            if (!string.IsNullOrEmpty(tableName))
            {
                sqlString = string.Format("UPDATE siteserver_Node SET ContentNum = (SELECT COUNT(*) AS ContentNum FROM {0} WHERE (NodeID = {1})) WHERE (NodeID = {1})", tableName, nodeID);
            }
            if (!string.IsNullOrEmpty(sqlString))
            {
                this.ExecuteNonQuery(sqlString);
            }

            if (isRemoveCache)
            {
                NodeManager.RemoveCache(publishmentSystemInfo.PublishmentSystemID);
            }
        }

        public void UpdateContentNumToZero(string tableName, EAuxiliaryTableType tableType)
        {
            if (tableType == EAuxiliaryTableType.BackgroundContent)
            {
                ArrayList publishmentSystemIDArrayList = new ArrayList();
                ArrayList psIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();

                foreach (int publishmentSystemID in psIDArrayList)
                {
                    PublishmentSystemInfo psInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    if (tableType == EAuxiliaryTableType.BackgroundContent && psInfo.AuxiliaryTableForContent == tableName)
                    {
                        publishmentSystemIDArrayList.Add(publishmentSystemID);
                        NodeManager.RemoveCache(publishmentSystemID);
                    }
                    else if (tableType == EAuxiliaryTableType.GovPublicContent && psInfo.AuxiliaryTableForGovPublic == tableName)
                    {
                        publishmentSystemIDArrayList.Add(publishmentSystemID);
                        NodeManager.RemoveCache(publishmentSystemID);
                    }
                    else if (tableType == EAuxiliaryTableType.GovInteractContent && psInfo.AuxiliaryTableForGovInteract == tableName)
                    {
                        publishmentSystemIDArrayList.Add(publishmentSystemID);
                        NodeManager.RemoveCache(publishmentSystemID);
                    }
                    else if (tableType == EAuxiliaryTableType.VoteContent && psInfo.AuxiliaryTableForVote == tableName)
                    {
                        publishmentSystemIDArrayList.Add(publishmentSystemID);
                        NodeManager.RemoveCache(publishmentSystemID);
                    }
                    else if (tableType == EAuxiliaryTableType.JobContent && psInfo.AuxiliaryTableForJob == tableName)
                    {
                        publishmentSystemIDArrayList.Add(publishmentSystemID);
                        NodeManager.RemoveCache(publishmentSystemID);
                    }
                }
                if (publishmentSystemIDArrayList.Count == 0) return;

                string sqlString = string.Format("UPDATE siteserver_Node SET ContentNum = 0 WHERE PublishmentSystemID IN ({0}) OR NodeID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(publishmentSystemIDArrayList));
                this.ExecuteNonQuery(sqlString);
            }
        }

        public void UpdateCommentNum(int publishmentSystemID, int nodeID, int comments)
        {
            string sqlString = string.Format("UPDATE siteserver_Node SET CommentNum = {0} WHERE NodeID = {1}", comments, nodeID);
            this.ExecuteNonQuery(sqlString);

            NodeManager.RemoveCache(publishmentSystemID);
        }

        public void Delete(int nodeID)
        {
            NodeInfo nodeInfo = this.GetNodeInfo(nodeID);

            ArrayList nodeIDArrayList = new ArrayList();
            if (nodeInfo.ChildrenCount > 0)
            {
                nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListForDescendant(nodeID);
            }
            nodeIDArrayList.Add(nodeID);

            string DELETE_CMD = string.Format("DELETE FROM siteserver_Node WHERE NodeID IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList));

            string tableName = NodeManager.GetTableName(PublishmentSystemManager.GetPublishmentSystemInfo(nodeInfo.PublishmentSystemID), nodeInfo);
            string DELETE_CONTENT_CMD = string.Empty;
            if (!string.IsNullOrEmpty(tableName))
            {
                DELETE_CONTENT_CMD = string.Format("DELETE FROM {0} WHERE NodeID IN ({1})", tableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList));
            }

            int deletedNum = 0;

            using (IDbConnection conn = this.GetConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(DELETE_CONTENT_CMD))
                        {
                            this.ExecuteNonQuery(trans, DELETE_CONTENT_CMD);
                        }
                        deletedNum = this.ExecuteNonQuery(trans, DELETE_CMD);

                        if (nodeInfo.NodeType != ENodeType.BackgroundPublishNode)
                        {
                            string TAXIS_CMD = string.Format("UPDATE siteserver_Node SET Taxis = Taxis - {0} WHERE (Taxis > {1}) AND (PublishmentSystemID = {2})", deletedNum, nodeInfo.Taxis, nodeInfo.PublishmentSystemID);
                            this.ExecuteNonQuery(trans, TAXIS_CMD);
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
            this.UpdateIsLastNode(nodeInfo.ParentID);
            this.UpdateSubtractChildrenCount(nodeInfo.ParentsPath, deletedNum);

            if (nodeInfo.NodeType == ENodeType.BackgroundPublishNode)
            {
                DataProvider.PublishmentSystemDAO.Delete(nodeInfo.NodeID);
            }
            else
            {
                NodeManager.RemoveCache(nodeInfo.PublishmentSystemID);
            }
        }

        #endregion

        //# region 辅助属性

        //public void InsertNodeAttributes(int publishmentSystemID, int nodeID, string tableName, NameValueCollection attributes)
        //{
        //    if (attributes != null && !string.IsNullOrEmpty(tableName))
        //    {
        //        attributes[NodeAttribute.PublishmentSystemID] = publishmentSystemID.ToString();
        //        attributes[NodeAttribute.NodeID] = nodeID.ToString();
        //        IDbDataParameter[] parms = null;
        //        string SQL_INSERT = BaiRongDataProvider.TableStructureDAO.GetInsertSqlString(attributes, tableName, out parms);

        //        this.ExecuteNonQuery(SQL_INSERT, parms);
        //    }
        //}

        //public void UpdateNodeAttributes(int publishmentSystemID, int nodeID, string tableName, NameValueCollection attributes)
        //{
        //    if (!string.IsNullOrEmpty(tableName))
        //    {
        //        int id = this.GetNodeAttributesID(publishmentSystemID, nodeID, tableName);
        //        if (id == 0)
        //        {
        //            this.InsertNodeAttributes(publishmentSystemID, nodeID, tableName, attributes);
        //        }
        //        else
        //        {
        //            attributes["ID"] = id.ToString();
        //            IDbDataParameter[] parms = null;
        //            string SQL_UPDATE = BaiRongDataProvider.TableStructureDAO.GetUpdateSqlString(attributes, tableName, out parms);

        //            this.ExecuteNonQuery(SQL_UPDATE, parms);
        //        }
        //    }
        //}

        //public void DeleteNodeAttributes(int publishmentSystemID, ArrayList nodeIDArrayList, string tableName)
        //{
        //    if (!string.IsNullOrEmpty(tableName) && nodeIDArrayList != null && nodeIDArrayList.Count > 0)
        //    {
        //        string SQL_DELETE = string.Format("DELETE FROM {0} WHERE PublishmentSystemID = {1} AND NodeID IN ({2})", tableName, publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList));
        //        BaiRongDataProvider.DatabaseDAO.ExecuteSql(SQL_DELETE);
        //    }
        //}

        //public int GetNodeAttributesID(int publishmentSystemID, int nodeID, string tableName)
        //{
        //    int id = 0;
        //    if (!string.IsNullOrEmpty(tableName))
        //    {
        //        string SQL_SELECT = string.Format("SELECT ID FROM {0} WHERE PublishmentSystemID = {1} AND NodeID = {2}", tableName, publishmentSystemID, nodeID);

        //        using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
        //        {
        //            if (rdr.Read())
        //            {
        //                id = BaiRongDataProvider.DatabaseDAO.GetIntResult(SQL_SELECT);
        //            }
        //            rdr.Close();
        //        }
        //    }
        //    return id;
        //}

        //public NameValueCollection GetNodeAttributes(int publishmentSystemID, int nodeID)
        //{
        //    NameValueCollection attributes = new NameValueCollection();
        //    string tableName = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID).AuxiliaryTableForChannel;
        //    if (!string.IsNullOrEmpty(tableName))
        //    {
        //        string SQL_WHERE = string.Format("WHERE PublishmentSystemID = {0} AND NodeID = {1}", publishmentSystemID, nodeID);
        //        string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, SqlUtils.Asterisk, SQL_WHERE);

        //        using (IDataReader rdr = this.ExecuteReader(SQL_SELECT))
        //        {
        //            if (rdr.Read())
        //            {
        //                BaiRongDataProvider.DatabaseDAO.ReadResultsToNameValueCollection(rdr, attributes);
        //            }
        //            rdr.Close();
        //        }
        //    }
        //    return attributes;
        //}

        //public bool IsNodeAttributesExist(int publishmentSystemID, int nodeID)
        //{
        //    bool exist = false;
        //    string tableName = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID).AuxiliaryTableForChannel;

        //    if (!string.IsNullOrEmpty(tableName))
        //    {
        //        string sqlString = string.Format("SELECT * FROM {0} WHERE PublishmentSystemID = {1} AND NodeID = {2}", tableName, publishmentSystemID, nodeID);
        //        try
        //        {
        //            using (IDataReader rdr = this.ExecuteReader(sqlString))
        //            {
        //                if (rdr.Read())
        //                {
        //                    exist = true;
        //                }
        //                rdr.Close();
        //            }
        //        }
        //        catch
        //        {
        //        }
        //    }

        //    return exist;
        //}

        //#endregion

        public NodeInfo GetNodeInfo(int nodeID)
        {
            NodeInfo node = null;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_NODE, nodeParms))
            {
                if (rdr.Read())
                {
                    node = new NodeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), ENodeTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), rdr.GetDateTime(13), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetInt32(16), TranslateUtils.ToInt(rdr.GetValue(17).ToString()), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString(), rdr.GetValue(20).ToString(), rdr.GetValue(21).ToString(), ELinkTypeUtils.GetEnumType(rdr.GetValue(22).ToString()), rdr.GetInt32(23), rdr.GetInt32(24), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString());
                }
                rdr.Close();
            }
            return node;
        }

        /// <summary>
        /// 得到最后一个添加的子节点
        /// </summary>
        public NodeInfo GetNodeInfoByLastAddDate(int nodeID)
        {
            NodeInfo node = null;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, nodeID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_NODE_BY_LAST_ADD_DATE, nodeParms))
            {
                if (rdr.Read())
                {
                    node = new NodeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), ENodeTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), rdr.GetDateTime(13), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetInt32(16), TranslateUtils.ToInt(rdr.GetValue(17).ToString()), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString(), rdr.GetValue(20).ToString(), rdr.GetValue(21).ToString(), ELinkTypeUtils.GetEnumType(rdr.GetValue(22).ToString()), rdr.GetInt32(23), rdr.GetInt32(24), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString());
                }
                rdr.Close();
            }
            return node;
        }

        /// <summary>
        /// 得到第一个子节点
        /// </summary>
        public NodeInfo GetNodeInfoByTaxis(int nodeID)
        {
            NodeInfo node = null;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, nodeID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_NODE_BY_TAXIS, nodeParms))
            {
                if (rdr.Read())
                {
                    node = new NodeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), ENodeTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), rdr.GetDateTime(13), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetInt32(16), TranslateUtils.ToInt(rdr.GetValue(17).ToString()), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString(), rdr.GetValue(20).ToString(), rdr.GetValue(21).ToString(), ELinkTypeUtils.GetEnumType(rdr.GetValue(22).ToString()), rdr.GetInt32(23), rdr.GetInt32(24), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString());
                }
                rdr.Close();
            }
            return node;
        }

        public NodeInfo GetNodeInfoByParentID(int publishmentSystemID, int parentID, EContentModelType contentModelType)
        {
            NodeInfo nodeInfo = null;
            IDbDataParameter[] nodeParms = new IDbDataParameter[] {
                this.GetParameter(PARM_PUBLISHMENT_SYSTEM_ID,EDataType.Integer,publishmentSystemID),
                this.GetParameter(PARM_PARENT_ID,EDataType.Integer,parentID),
                this.GetParameter(PARM_CONTENT_MODEL_ID,EDataType.VarChar,50,EContentModelTypeUtils.GetValue(contentModelType))
            };
            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_NODE_BY_PARENT_ID_AND_CONTENT_MODEL_ID, nodeParms))
            {
                if (rdr.Read())
                {
                    nodeInfo = new NodeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), ENodeTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), rdr.GetDateTime(13), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetInt32(16), TranslateUtils.ToInt(rdr.GetValue(17).ToString()), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString(), rdr.GetValue(20).ToString(), rdr.GetValue(21).ToString(), ELinkTypeUtils.GetEnumType(rdr.GetValue(22).ToString()), rdr.GetInt32(23), rdr.GetInt32(24), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString());
                }
                rdr.Close();
            }
            return nodeInfo;
        }

        /// <summary>
        /// 通过栏目名获取栏目ID
        /// </summary>
        /// <param name="parentID">父节点ID</param>
        /// <param name="nodeName">栏目名称</param>
        /// <param name="recursive">是否搜索所有子节点</param>
        /// <returns>栏目ID，如果数据库中没有此栏目名称则返回0</returns>
        public int GetNodeIDByParentIDAndNodeName(int publishmentSystemID, int parentID, string nodeName, bool recursive)
        {
            int nodeID = 0;
            string sqlString = string.Empty;

            if (recursive)
            {
                if (publishmentSystemID == parentID)
                {
                    sqlString = string.Format("SELECT NodeID FROM siteserver_Node WHERE (PublishmentSystemID = {0} AND NodeName = '{1}') ORDER BY Taxis", publishmentSystemID, nodeName);
                }
                else
                {
                    sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node 
WHERE ((ParentID = {0}) OR
      (ParentsPath = '{0}') OR
      (ParentsPath LIKE '{0},%') OR
      (ParentsPath LIKE '%,{0},%') OR
      (ParentsPath LIKE '%,{0}')) AND NodeName = '{1}'
ORDER BY Taxis", parentID, PageUtils.FilterSql(nodeName));
                }
            }
            else
            {
                sqlString = string.Format("SELECT NodeID FROM siteserver_Node WHERE (ParentID = {0} AND NodeName = '{1}') ORDER BY Taxis", parentID, nodeName);
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    nodeID = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }
            return nodeID;
        }

        public int GetNodeIDByParentIDAndTaxis(int parentID, int taxis, bool isNextChannel)
        {
            int nodeID = 0;

            string sqlString = string.Empty;
            if (isNextChannel)
            {
                sqlString = string.Format("SELECT TOP 1 NodeID FROM siteserver_Node WHERE (ParentID = {0} AND Taxis > {1}) ORDER BY Taxis", parentID, taxis);
            }
            else
            {
                sqlString = string.Format("SELECT TOP 1 NodeID FROM siteserver_Node WHERE (ParentID = {0} AND Taxis < {1}) ORDER BY Taxis DESC", parentID, taxis);
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                if (rdr.Read())
                {
                    nodeID = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }
            return nodeID;
        }

        public int GetNodeID(int publishmentSystemID, string orderString)
        {
            if (orderString == "1")
                return publishmentSystemID;

            int nodeID = publishmentSystemID;

            string[] orderArr = orderString.Split(new char[] { '_' });
            for (int index = 1; index < orderArr.Length; index++)
            {
                int order = int.Parse(orderArr[index]);
                nodeID = GetNodeIDByParentIDAndOrder(nodeID, order);
            }
            return nodeID;
        }

        public int GetPublishmentSystemID(int nodeID)
        {
            int publishmentSystemID = 0;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_PUBLISHMENT_SYSTEM_ID_BY_ID, nodeParms))
            {
                if (rdr.Read())
                {
                    publishmentSystemID = Convert.ToInt32(rdr[0]);
                    if (publishmentSystemID == 0)
                    {
                        publishmentSystemID = nodeID;
                    }
                }
                rdr.Close();
            }
            return publishmentSystemID;
        }

        /// <summary>
        /// 在节点树中得到此节点的排序号，以“1_2_5_2”的形式表示
        /// </summary>
        public string GetOrderStringInPublishmentSystem(int nodeID)
        {
            string retval = "";
            if (nodeID != 0)
            {
                int parentID = GetParentID(nodeID);
                if (parentID != 0)
                {
                    string orderString = GetOrderStringInPublishmentSystem(parentID);
                    retval = orderString + "_" + GetOrderInSiblingNode(nodeID, parentID);
                }
                else
                {
                    retval = "1";
                }
            }
            return retval;
        }

        public string GetSelectCommendByNodeGroupName(int publishmentSystemID, string nodeGroupName)
        {
            string sqlString = string.Format("SELECT * FROM siteserver_Node WHERE PublishmentSystemID={0} AND (NodeGroupNameCollection LIKE '{1},%' OR NodeGroupNameCollection LIKE '%,{1}' OR NodeGroupNameCollection LIKE '%,{1},%' OR NodeGroupNameCollection='{1}')", publishmentSystemID, PageUtils.FilterSql(nodeGroupName));
            return sqlString;
        }

        public int GetOrderInSiblingNode(int nodeID, int parentID)
        {
            string CMD = string.Format("SELECT NodeID FROM siteserver_Node WHERE (ParentID = {0}) ORDER BY Taxis", parentID);
            ArrayList nodeIDArrayList = new ArrayList();
            using (IDataReader rdr = this.ExecuteReader(CMD))
            {
                while (rdr.Read())
                {
                    nodeIDArrayList.Add(rdr.GetInt32(0));
                }
                rdr.Close();
            }
            return nodeIDArrayList.IndexOf(nodeID) + 1;
        }

        public ArrayList GetNodeIndexNameArrayList(int publishmentSystemID)
        {
            ArrayList list = new ArrayList();

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
            {
                this.GetParameter(NodeDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_NODE_INDEX_NAME_COLLECTION, nodeParms))
            {
                while (rdr.Read())
                {
                    string nodeIndexName = rdr.GetValue(0).ToString();
                    list.Add(nodeIndexName);
                }
                rdr.Close();
            }

            return list;
        }

        private string GetGroupWhereString(string group, string groupNot)
        {
            StringBuilder whereStringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(group))
            {
                group = group.Trim().Trim(',');
                string[] groupArr = group.Split(',');
                if (groupArr != null && groupArr.Length > 0)
                {
                    whereStringBuilder.Append(" AND (");
                    foreach (string theGroup in groupArr)
                    {
                        if (this.DataBaseType == EDatabaseType.SqlServer)
                        {
                            whereStringBuilder.AppendFormat(" (siteserver_Node.NodeGroupNameCollection = '{0}' OR CHARINDEX('{0},',siteserver_Node.NodeGroupNameCollection) > 0 OR CHARINDEX(',{0},', siteserver_Node.NodeGroupNameCollection) > 0 OR CHARINDEX(',{0}', siteserver_Node.NodeGroupNameCollection) > 0) OR ", theGroup.Trim());
                        }
                        else if (this.DataBaseType == EDatabaseType.Oracle)
                        {
                            whereStringBuilder.AppendFormat(" (siteserver_Node.NodeGroupNameCollection = '{0}' OR instr(siteserver_Node.NodeGroupNameCollection, '{0},') > 0 OR instr(siteserver_Node.NodeGroupNameCollection, ',{0},') > 0 OR instr(siteserver_Node.NodeGroupNameCollection, ',{0}') > 0) OR ", theGroup.Trim());
                        }
                        else
                        {
                            whereStringBuilder.AppendFormat(" (siteserver_Node.NodeGroupNameCollection = '{0}' OR siteserver_Node.NodeGroupNameCollection LIKE '{0},%' OR siteserver_Node.NodeGroupNameCollection LIKE '%,{0},%' OR siteserver_Node.NodeGroupNameCollection LIKE '%,{0}') OR ", theGroup.Trim());
                        }
                    }
                    if (groupArr.Length > 0)
                    {
                        whereStringBuilder.Length = whereStringBuilder.Length - 3;
                    }
                    whereStringBuilder.Append(") ");
                }
            }

            if (!string.IsNullOrEmpty(groupNot))
            {
                groupNot = groupNot.Trim().Trim(',');
                string[] groupNotArr = groupNot.Split(',');
                if (groupNotArr != null && groupNotArr.Length > 0)
                {
                    whereStringBuilder.Append(" AND (");
                    foreach (string theGroupNot in groupNotArr)
                    {
                        if (this.DataBaseType == EDatabaseType.SqlServer)
                        {
                            whereStringBuilder.AppendFormat(" (siteserver_Node.NodeGroupNameCollection <> '{0}' AND CHARINDEX('{0},',siteserver_Node.NodeGroupNameCollection) = 0 AND CHARINDEX(',{0},',siteserver_Node.NodeGroupNameCollection) = 0 AND CHARINDEX(',{0}',siteserver_Node.NodeGroupNameCollection) = 0) AND ", theGroupNot.Trim());
                        }
                        else if (this.DataBaseType == EDatabaseType.Oracle)
                        {
                            whereStringBuilder.AppendFormat(" (siteserver_Node.NodeGroupNameCollection <> '{0}' AND instr(siteserver_Node.NodeGroupNameCollection, '{0},') = 0 AND instr(siteserver_Node.NodeGroupNameCollection, ',{0},') = 0 AND instr(siteserver_Node.NodeGroupNameCollection, ',{0}') = 0) AND ", theGroupNot.Trim());
                        }
                        else
                        {
                            whereStringBuilder.AppendFormat(" (siteserver_Node.NodeGroupNameCollection <> '{0}' AND siteserver_Node.NodeGroupNameCollection NOT LIKE '{0},%' AND siteserver_Node.NodeGroupNameCollection NOT LIKE '%,{0},%' AND siteserver_Node.NodeGroupNameCollection NOT LIKE '%,{0}') AND ", theGroupNot.Trim());
                        }
                    }
                    if (groupNotArr.Length > 0)
                    {
                        whereStringBuilder.Length = whereStringBuilder.Length - 4;
                    }
                    whereStringBuilder.Append(") ");
                }
            }
            return whereStringBuilder.ToString();
        }

        public string GetWhereString(int publishmentSystemID, string group, string groupNot, bool isImageExists, bool isImage, string where)
        {
            StringBuilder whereStringBuilder = new StringBuilder();
            if (isImageExists)
            {
                if (isImage)
                {
                    whereStringBuilder.Append(" AND siteserver_Node.ImageUrl <> '' ");
                }
                else
                {
                    whereStringBuilder.Append(" AND siteserver_Node.ImageUrl = '' ");
                }
            }

            whereStringBuilder.Append(GetGroupWhereString(group, groupNot));

            if (!string.IsNullOrEmpty(where))
            {
                whereStringBuilder.AppendFormat(" AND {0} ", where);
            }

            return whereStringBuilder.ToString();
        }

        public int GetNodeCountByPublishmentSystemID(int publishmentSystemID)
        {
            int nodeCount = 0;
            string CMD = string.Format("SELECT COUNT(*) AS TotalNum FROM siteserver_Node WHERE (PublishmentSystemID = {0} AND (NodeID = {0} OR ParentID > 0))", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(CMD))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        nodeCount = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }

            return nodeCount;
        }

        public int GetNodeCount(int nodeID)
        {
            int nodeCount = 0;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_PARENT_ID, EDataType.Integer, nodeID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_NODE_COUNT, nodeParms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        nodeCount = Convert.ToInt32(rdr[0]);
                    }
                }
                rdr.Close();
            }
            return nodeCount;
        }

        public int GetSequence(int publishmentSystemID, int nodeID)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            string sqlString = string.Format("SELECT COUNT(*) AS TotalNum FROM siteserver_Node WHERE PublishmentSystemID = {0} AND ParentID = {1} AND Taxis > (SELECT Taxis FROM siteserver_Node WHERE NodeID = {2})", publishmentSystemID, nodeInfo.ParentID, nodeID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString) + 1;
        }

        public int GetNodeIDByNodeIndexName(int publishmentSystemID, string nodeIndexName)
        {
            int nodeID = 0;
            if (nodeIndexName == null || nodeIndexName.Length == 0) return nodeID;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
            {
                this.GetParameter(NodeDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(NodeDAO.PARM_NODE_INDEX_NAME, EDataType.NVarChar, 255, nodeIndexName)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_NODE_ID_BY_INDEX, nodeParms))
            {
                if (rdr.Read())
                {
                    nodeID = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return nodeID;
        }

        public int GetNodeIDByContentModelType(int publishmentSystemID, EContentModelType contentModelType)
        {
            int nodeID = 0;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
            {
                this.GetParameter(NodeDAO.PARM_PUBLISHMENT_SYSTEM_ID, EDataType.Integer, publishmentSystemID),
                this.GetParameter(NodeDAO.PARM_CONTENT_MODEL_ID, EDataType.VarChar, 50, EContentModelTypeUtils.GetValue(contentModelType))
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_NODE_ID_BY_CONTENT_MODEL_ID, nodeParms))
            {
                if (rdr.Read())
                {
                    nodeID = Convert.ToInt32(rdr[0]);
                }
                rdr.Close();
            }

            return nodeID;
        }

        public bool IsExists(int nodeID)
        {
            bool exists = false;

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
            };

            using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_NODE_ID, nodeParms))
            {
                if (rdr.Read())
                {
                    if (!rdr.IsDBNull(0))
                    {
                        exists = true;
                    }
                }
                rdr.Close();
            }
            return exists;
        }

        public void UpdateChannelTemplateID(int nodeID, int channelTemplateID)
        {
            string sqlString = string.Format("UPDATE siteserver_Node SET ChannelTemplateID = {0} WHERE NodeID = {1}", channelTemplateID, nodeID);
            this.ExecuteNonQuery(sqlString);

            NodeManager.RemoveCache(nodeID);
        }

        public void UpdateContentTemplateID(int nodeID, int contentTemplateID)
        {
            string sqlString = string.Format("UPDATE siteserver_Node SET ContentTemplateID = {0} WHERE NodeID = {1}", contentTemplateID, nodeID);
            this.ExecuteNonQuery(sqlString);

            NodeManager.RemoveCache(nodeID);
        }

        //public int GetChannelTemplateID(int nodeID)
        //{
        //    int channelTemplateID = 0;

        //    IDbDataParameter[] nodeParms = new IDbDataParameter[]
        //    {
        //        this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
        //    };

        //    using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_CHANNEL_TEMPLATE_ID, nodeParms))
        //    {
        //        if (rdr.Read())
        //        {
        //            if (!rdr.IsDBNull(0))
        //            {
        //                channelTemplateID = Convert.ToInt32(rdr[0]);
        //            }
        //        }
        //        rdr.Close();
        //    }
        //    return channelTemplateID;
        //}

        //public int GetContentTemplateID(int nodeID)
        //{
        //    int contentTemplateID = 0;

        //    IDbDataParameter[] nodeParms = new IDbDataParameter[]
        //    {
        //        this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeID)
        //    };

        //    using (IDataReader rdr = this.ExecuteReader(SQL_SELECT_CONTENT_TEMPLATE_ID, nodeParms))
        //    {
        //        if (rdr.Read())
        //        {
        //            if (!rdr.IsDBNull(0))
        //            {
        //                contentTemplateID = Convert.ToInt32(rdr[0]);
        //            }
        //        }
        //        rdr.Close();
        //    }
        //    return contentTemplateID;
        //}

        public ArrayList GetNodeIDArrayList()
        {
            ArrayList arraylist = new ArrayList();
            string CMD = "SELECT NodeID FROM siteserver_Node";

            using (IDataReader rdr = this.ExecuteReader(CMD))
            {
                while (rdr.Read())
                {
                    int nodeIDToArrayList = Convert.ToInt32(rdr[0]);
                    arraylist.Add(nodeIDToArrayList);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetNodeIDArrayList(int nodeID, int totalNum, string orderByString, string whereString, EScopeType scopeType, string group, string groupNot)
        {
            ArrayList nodeIDArrayList = this.GetNodeIDArrayListByScopeType(nodeID, scopeType, group, groupNot);
            if (nodeIDArrayList == null || nodeIDArrayList.Count == 0)
            {
                return nodeIDArrayList;
            }

            string sqlString = string.Empty;
            if (totalNum > 0)
            {
                if (this.DataBaseType != EDatabaseType.Oracle)
                {
                    sqlString = string.Format(@"SELECT TOP {0} NodeID
FROM siteserver_Node 
WHERE (NodeID IN ({1}) {2}) {3}
", totalNum, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), whereString, orderByString);
                }
                else
                {
                    sqlString = string.Format(@"SELECT * FROM (
    SELECT NodeID
    FROM siteserver_Node 
    WHERE (NodeID IN ({0}) {1}) {2}
) WHERE ROWNUM <= {3}
", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), whereString, orderByString, totalNum);
                }
            }
            else
            {
                sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node 
WHERE (NodeID IN ({0}) {1}) {2}
", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), whereString, orderByString);
            }

            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    arraylist.Add(Convert.ToInt32(rdr[0]));
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetNodeIDArrayListByNodeType(params ENodeType[] eNodeTypeArray)
        {
            ArrayList arraylist = new ArrayList();
            if (eNodeTypeArray != null && eNodeTypeArray.Length > 0)
            {
                ArrayList nodeTypeStringArrayList = new ArrayList();
                foreach (ENodeType nodeType in eNodeTypeArray)
                {
                    nodeTypeStringArrayList.Add(ENodeTypeUtils.GetValue(nodeType));
                }
                string SqlString = string.Format("SELECT NodeID FROM siteserver_Node WHERE NodeType IN ({0})", TranslateUtils.ObjectCollectionToSqlInStringWithQuote(nodeTypeStringArrayList));

                using (IDataReader rdr = this.ExecuteReader(SqlString))
                {
                    while (rdr.Read())
                    {
                        int nodeIDToArrayList = Convert.ToInt32(rdr[0]);
                        arraylist.Add(nodeIDToArrayList);
                    }
                    rdr.Close();
                }

            }
            return arraylist;
        }

        private ArrayList GetNodeIDArrayListByScopeType(int nodeID, EScopeType scopeType)
        {
            return GetNodeIDArrayListByScopeType(nodeID, scopeType, string.Empty, string.Empty);
        }

        public ArrayList GetNodeIDArrayListByScopeType(int nodeID, EScopeType scopeType, string group, string groupNot)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = null;
            string groupWhereString = GetGroupWhereString(group, groupNot);
            if (scopeType == EScopeType.All)
            {
                sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node 
WHERE ((NodeID = {0}) OR
      (ParentID = {0}) OR
      (ParentsPath = '{0}') OR
      (ParentsPath LIKE '{0},%') OR
      (ParentsPath LIKE '%,{0},%') OR
      (ParentsPath LIKE '%,{0}')) {1}
ORDER BY Taxis", nodeID, groupWhereString);
            }
            else if (scopeType == EScopeType.Self)
            {
                arraylist.Add(nodeID);
                return arraylist;
            }
            else if (scopeType == EScopeType.Children)
            {
                sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node 
WHERE (ParentID = {0}) {1}
ORDER BY Taxis", nodeID, groupWhereString);
            }
            else if (scopeType == EScopeType.Descendant)
            {
                sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node 
WHERE ((ParentID = {0}) OR
      (ParentsPath = '{0}') OR
      (ParentsPath LIKE '{0},%') OR
      (ParentsPath LIKE '%,{0},%') OR
      (ParentsPath LIKE '%,{0}')) {1}
ORDER BY Taxis", nodeID, groupWhereString);
            }
            else if (scopeType == EScopeType.SelfAndChildren)
            {
                sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node 
WHERE ((NodeID = {0}) OR
      (ParentID = {0})) {1}
ORDER BY Taxis", nodeID, groupWhereString);
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int nodeIDToArrayList = Convert.ToInt32(rdr[0]);
                    arraylist.Add(nodeIDToArrayList);
                }
                rdr.Close();
            }

            return arraylist;
        }

        private ArrayList GetNodeIDArrayListByScopeType(NodeInfo nodeInfo, EScopeType scopeType)
        {
            return GetNodeIDArrayListByScopeType(nodeInfo, scopeType, string.Empty, string.Empty, string.Empty);
        }

        public ArrayList GetNodeIDArrayListByScopeType(NodeInfo nodeInfo, EScopeType scopeType, string group, string groupNot)
        {
            return GetNodeIDArrayListByScopeType(nodeInfo, scopeType, group, groupNot, string.Empty);
        }

        public ArrayList GetNodeIDArrayListByScopeType(NodeInfo nodeInfo, EScopeType scopeType, string group, string groupNot, string contentModelID)
        {
            ArrayList arraylist = new ArrayList();

            if (nodeInfo == null) return arraylist;

            if (nodeInfo.ChildrenCount == 0)
            {
                if (scopeType != EScopeType.Children && scopeType != EScopeType.Descendant)
                {
                    arraylist.Add(nodeInfo.NodeID);
                }

                return arraylist;
            }

            string sqlString = null;
            string whereString = GetGroupWhereString(group, groupNot);
            string contentModelWhereString = string.Empty;
            if (!string.IsNullOrEmpty(contentModelID))
            {
                whereString += string.Format(" AND ContentModelID = '{0}'", contentModelID);
            }
            if (scopeType == EScopeType.All)
            {
                sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node 
WHERE ((NodeID = {0}) OR
      (ParentID = {0}) OR
      (ParentsPath = '{0}') OR
      (ParentsPath LIKE '{0},%') OR
      (ParentsPath LIKE '%,{0},%') OR
      (ParentsPath LIKE '%,{0}')) {1}
ORDER BY Taxis", nodeInfo.NodeID, whereString);
            }
            else if (scopeType == EScopeType.Self)
            {
                arraylist.Add(nodeInfo.NodeID);
                return arraylist;
            }
            else if (scopeType == EScopeType.Children)
            {
                sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node 
WHERE (ParentID = {0}) {1}
ORDER BY Taxis", nodeInfo.NodeID, whereString);
            }
            else if (scopeType == EScopeType.Descendant)
            {
                sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node 
WHERE ((ParentID = {0}) OR
      (ParentsPath = '{0}') OR
      (ParentsPath LIKE '{0},%') OR
      (ParentsPath LIKE '%,{0},%') OR
      (ParentsPath LIKE '%,{0}')) {1}
ORDER BY Taxis", nodeInfo.NodeID, whereString);
            }
            else if (scopeType == EScopeType.SelfAndChildren)
            {
                sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node 
WHERE ((NodeID = {0}) OR
      (ParentID = {0})) {1}
ORDER BY Taxis", nodeInfo.NodeID, whereString);
            }

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int nodeIDToArrayList = Convert.ToInt32(rdr[0]);
                    arraylist.Add(nodeIDToArrayList);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetNodeIDArrayListForDescendant(int nodeID)
        {
            string sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node
WHERE (ParentsPath LIKE '{0},%') OR
      (ParentsPath LIKE '%,{0},%') OR
      (ParentsPath LIKE '%,{0}') OR
      (ParentID = {0})
", nodeID);
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

        public List<int> GetNodeIDListForDescendant(int nodeID)
        {
            string sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node
WHERE (ParentsPath LIKE '{0},%') OR
      (ParentsPath LIKE '%,{0},%') OR
      (ParentsPath LIKE '%,{0}') OR
      (ParentID = {0})
", nodeID);
            List<int> list = new List<int>();

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

        public ArrayList GetNodeIDArrayListByParentID(int publishmentSystemID, int parentID)
        {
            string sqlString = string.Empty;
            if (parentID == 0)
            {
                sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node
WHERE (NodeID = {0} OR ParentID = {0})
ORDER BY Taxis", publishmentSystemID);
            }
            else
            {
                sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node
WHERE (ParentID = {0})
ORDER BY Taxis", parentID);
            }

            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int nodeID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(nodeID);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetNodeInfoArrayListByParentID(int publishmentSystemID, int parentID)
        {
            string sqlString = string.Empty;

            sqlString = string.Format(@"SELECT NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues FROM siteserver_Node 
WHERE (PublishmentSystemID={0} AND ParentID = {1})
ORDER BY Taxis", publishmentSystemID, parentID);

            ArrayList arraylist = new ArrayList();
            NodeInfo node;
            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    node = new NodeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), ENodeTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), rdr.GetDateTime(13), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetInt32(16), TranslateUtils.ToInt(rdr.GetValue(17).ToString()), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString(), rdr.GetValue(20).ToString(), rdr.GetValue(21).ToString(), ELinkTypeUtils.GetEnumType(rdr.GetValue(22).ToString()), rdr.GetInt32(23), rdr.GetInt32(24), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString());
                    arraylist.Add(node);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetNodeIDArrayListByPublishmentSystemID(int publishmentSystemID)
        {
            string sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node
WHERE PublishmentSystemID = {0} AND (NodeID = {0} OR ParentID > 0)
ORDER BY Taxis", publishmentSystemID);
            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int nodeID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(nodeID);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public ArrayList GetNodeIDArrayListByPublishmentSystemID(int publishmentSystemID, ArrayList nodeIDArrayList)
        {
            //ArrayList theNodeIDArrayList = new ArrayList();
            //foreach (int nodeID in nodeIDArrayList)
            //{
            //    NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeID);
            //    if (nodeInfo.ParentID != publishmentSystemID)
            //    {
            //        if (nodeIDArrayList.Contains(nodeInfo.ParentID))
            //        {
            //            continue;
            //        }
            //    }
            //    theNodeIDArrayList.Add(nodeID);
            //}

            string sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node
WHERE (PublishmentSystemID = {0} AND (NodeID = {0} OR ParentID > 0))
ORDER BY Taxis", publishmentSystemID);
            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int nodeID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(nodeID);
                }
                rdr.Close();
            }

            return arraylist;
        }

        public Hashtable GetNodeInfoHashtableByPublishmentSystemID(int publishmentSystemID)
        {
            Hashtable hashtable = new Hashtable();
            string sqlString = string.Format(@"SELECT NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues
FROM siteserver_Node 
WHERE (PublishmentSystemID = {0} AND (NodeID = {0} OR ParentID > 0))
ORDER BY Taxis", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    NodeInfo nodeInfo = new NodeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), ENodeTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), rdr.GetDateTime(13), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetInt32(16), TranslateUtils.ToInt(rdr.GetValue(17).ToString()), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString(), rdr.GetValue(20).ToString(), rdr.GetValue(21).ToString(), ELinkTypeUtils.GetEnumType(rdr.GetValue(22).ToString()), rdr.GetInt32(23), rdr.GetInt32(24), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString());
                    hashtable.Add(nodeInfo.NodeID, nodeInfo);
                }
                rdr.Close();
            }

            return hashtable;
        }

        public IEnumerable GetDataSource(ArrayList nodeIDArrayList, int totalNum, string whereString, string orderByString)
        {
            if (nodeIDArrayList.Count == 0)
            {
                return null;
            }
            string sqlString = string.Empty;

            if (totalNum > 0)
            {
                if (this.DataBaseType != EDatabaseType.Oracle)
                {
                    sqlString = string.Format(@"SELECT TOP {0} NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues
FROM siteserver_Node
WHERE (NodeID IN ({1}) {2}) {3}
", totalNum, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), whereString, orderByString);
                }
                else
                {
                    sqlString = string.Format(@"SELECT * FROM (
    SELECT NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues
    FROM siteserver_Node
    WHERE (NodeID IN ({0}) {1}) {2}
) WHERE ROWNUM <= {3}
", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), whereString, orderByString, totalNum);
                }
            }
            else
            {
                sqlString = string.Format(@"SELECT NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues
FROM siteserver_Node
WHERE (NodeID IN ({0}) {1}) {2}
", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), whereString, orderByString);
            }

            IEnumerable enumerable = (IEnumerable)this.ExecuteReader(sqlString);
            return enumerable;
        }

        public DataSet GetDataSet(ArrayList nodeIDArrayList, int totalNum, string whereString, string orderByString)
        {
            if (nodeIDArrayList.Count == 0)
            {
                return null;
            }
            string sqlString = string.Empty;

            if (totalNum > 0)
            {
                if (this.DataBaseType != EDatabaseType.Oracle)
                {
                    sqlString = string.Format(@"SELECT TOP {0} NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues
FROM siteserver_Node
WHERE (NodeID IN ({1}) {2}) {3}
", totalNum, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), whereString, orderByString);
                }
                else
                {
                    sqlString = string.Format(@"SELECT * FROM (
    SELECT NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues
    FROM siteserver_Node
    WHERE (NodeID IN ({0}) {1}) {2}
) WHERE ROWNUM <= {3}
", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), whereString, orderByString, totalNum);
                }
            }
            else
            {
                sqlString = string.Format(@"SELECT NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues
FROM siteserver_Node
WHERE (NodeID IN ({0}) {1}) {2}
", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), whereString, orderByString);
            }

            DataSet dataSet = this.ExecuteDataset(sqlString);
            return dataSet;
        }

        public IEnumerable GetStlDataSource(NodeInfo nodeInfo, int startNum, int totalNum, string whereString, EScopeType scopeType, string orderByString)
        {
            string tableName = "siteserver_Node";

            ArrayList nodeIDArrayList = CreateCacheManager.NodeIDArrayList.GetNodeIDArrayListByScopeType(nodeInfo, scopeType);

            if (nodeIDArrayList == null || nodeIDArrayList.Count == 0)
            {
                return null;
            }

            string sqlWhereString = string.Format("WHERE (NodeID IN ({0}) {1})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), whereString);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, startNum, totalNum, "NodeID, AddDate, Taxis", sqlWhereString, orderByString);

            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }

        public IEnumerable GetStlDataSourceByPublishmentSystemID(int publishmentSystemID, int startNum, int totalNum, string whereString, string orderByString)
        {
            string tableName = "siteserver_Node";

            string sqlWhereString = string.Format("WHERE (PublishmentSystemID = {0} {1})", publishmentSystemID, whereString);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, startNum, totalNum, "NodeID, AddDate, Taxis", sqlWhereString, orderByString);

            return (IEnumerable)this.ExecuteReader(SQL_SELECT);
        }

        public DataSet GetStlDataSet(NodeInfo nodeInfo, int startNum, int totalNum, string whereString, EScopeType scopeType, string orderByString)
        {
            string tableName = "siteserver_Node";

            ArrayList nodeIDArrayList = CreateCacheManager.NodeIDArrayList.GetNodeIDArrayListByScopeType(nodeInfo, scopeType);
            if (nodeIDArrayList == null || nodeIDArrayList.Count == 0)
            {
                return null;
            }

            string sqlWhereString = string.Format("WHERE (NodeID IN ({0}) {1})", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDArrayList), whereString);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, startNum, totalNum, "NodeID, AddDate, Taxis", sqlWhereString, orderByString);

            return this.ExecuteDataset(SQL_SELECT);
        }

        public DataSet GetStlDataSetByPublishmentSystemID(int publishmentSystemID, int startNum, int totalNum, string whereString, string orderByString)
        {
            string tableName = "siteserver_Node";

            string sqlWhereString = string.Format("WHERE (PublishmentSystemID = {0} {1})", publishmentSystemID, whereString);

            string SQL_SELECT = BaiRongDataProvider.TableStructureDAO.GetSelectSqlString(tableName, startNum, totalNum, "NodeID, AddDate, Taxis", sqlWhereString, orderByString);

            return this.ExecuteDataset(SQL_SELECT);
        }

        public ArrayList GetNodeInfoArrayListByPublishmentSystemID(int publishmentSystemID, string whereString)
        {
            ArrayList arraylist = new ArrayList();
            string CMD = string.Format(@"SELECT NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues
FROM siteserver_Node 
WHERE (PublishmentSystemID = {0} AND (NodeID = {0} OR ParentID > 0))
{1}
ORDER BY Taxis", publishmentSystemID, whereString);

            using (IDataReader rdr = this.ExecuteReader(CMD))
            {
                while (rdr.Read())
                {
                    NodeInfo node = new NodeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), ENodeTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), rdr.GetDateTime(13), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetInt32(16), TranslateUtils.ToInt(rdr.GetValue(17).ToString()), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString(), rdr.GetValue(20).ToString(), rdr.GetValue(21).ToString(), ELinkTypeUtils.GetEnumType(rdr.GetValue(22).ToString()), rdr.GetInt32(23), rdr.GetInt32(24), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString());
                    arraylist.Add(node);
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetNodeInfoArrayList(NodeInfo nodeInfo, int totalNum, string whereString, EScopeType scopeType, string orderByString)
        {
            ArrayList NodeIDArrayList = this.GetNodeIDArrayListByScopeType(nodeInfo, scopeType);
            if (NodeIDArrayList == null || NodeIDArrayList.Count == 0)
            {
                return null;
            }

            string sqlString = string.Empty;
            if (totalNum > 0)
            {
                if (this.DataBaseType != EDatabaseType.Oracle)
                {
                    sqlString = string.Format(@"SELECT TOP {0} NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues
FROM siteserver_Node 
WHERE (NodeID IN ({1}) {2}) {3}
", totalNum, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(NodeIDArrayList), whereString, orderByString);
                }
                else
                {
                    sqlString = string.Format(@"SELECT * FROM (
    SELECT NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues
    FROM siteserver_Node 
    WHERE (NodeID IN ({0}) {1}) {2}
) WHERE ROWNUM <= {3}
", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(NodeIDArrayList), whereString, orderByString, totalNum);
                }
            }
            else
            {
                sqlString = string.Format(@"SELECT NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues
FROM siteserver_Node 
WHERE (NodeID IN ({0}) {1}) {2}
", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(NodeIDArrayList), whereString, orderByString);
            }

            ArrayList arraylist = new ArrayList();

            IDbDataParameter[] nodeParms = new IDbDataParameter[]
            {
                this.GetParameter(PARM_NODE_ID, EDataType.Integer, nodeInfo.NodeID)
            };

            using (IDataReader rdr = this.ExecuteReader(sqlString, nodeParms))
            {
                while (rdr.Read())
                {
                    NodeInfo node = new NodeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), ENodeTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), rdr.GetDateTime(13), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetInt32(16), TranslateUtils.ToInt(rdr.GetValue(17).ToString()), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString(), rdr.GetValue(20).ToString(), rdr.GetValue(21).ToString(), ELinkTypeUtils.GetEnumType(rdr.GetValue(22).ToString()), rdr.GetInt32(23), rdr.GetInt32(24), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString());
                    arraylist.Add(node);
                }
                rdr.Close();
            }
            return arraylist;
        }

        /// <summary>
        /// 得到所有系统文件夹的列表，以小写表示。
        /// </summary>
        public ArrayList GetLowerSystemDirArrayList(int parentPublishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();
            string sqlString = "SELECT PublishmentSystemDir FROM siteserver_PublishmentSystem WHERE ParentPublishmentSystemID = " + parentPublishmentSystemID;

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString().ToLower());
                }
                rdr.Close();
            }

            return arraylist;
        }

        public int GetContentNumByPublishmentSystemID(int publishmentSystemID)
        {
            string sqlString = string.Format("SELECT SUM(ContentNum) FROM siteserver_Node WHERE (PublishmentSystemID = {0})", publishmentSystemID);

            return BaiRongDataProvider.DatabaseDAO.GetIntResult(sqlString);
        }

        public ArrayList GetAllFilePathByPublishmentSystemID(int publishmentSystemID)
        {
            ArrayList arraylist = new ArrayList();

            string sqlString = string.Format("SELECT FilePath FROM siteserver_Node WHERE FilePath <> '' AND PublishmentSystemID = {0}", publishmentSystemID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    arraylist.Add(rdr.GetValue(0).ToString());
                }
                rdr.Close();
            }
            return arraylist;
        }

        public ArrayList GetNodeIDArrayListByChildNodeID(int publishmentSystemID, int childNodeID, ArrayList nodeIDArrayList)
        {
            string sqlString = string.Empty;

            sqlString = string.Format(@"SELECT NodeID,ParentID FROM siteserver_Node 
WHERE (PublishmentSystemID={0} AND NodeID={1})
ORDER BY Taxis", publishmentSystemID, childNodeID);

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int nodeID = rdr.GetInt32(0);
                    int parentNodeID = rdr.GetInt32(1);
                    nodeIDArrayList.Add(nodeID);
                    GetNodeIDArrayListByChildNodeID(publishmentSystemID, parentNodeID, nodeIDArrayList);

                }
                rdr.Close();
            }
            return nodeIDArrayList;
        }

        public ArrayList GetNodeInfoByNodeIndexOrNodeName(int publishmentSystemID, string filterSearchWords, int count)
        {
            ArrayList retval = new ArrayList();
            NodeInfo nodeInfo = null;
            string CMD = string.Format("SELECT TOP {2} NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName, NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID, ContentTemplateID, Keywords, Description, ExtendValues  FROM siteserver_Node WHERE ContentModelID = 'Goods' AND (PublishmentSystemID = {0} AND (NodeName like '%{1}%' OR NodeIndexName like '%{1}%')) ORDER BY Taxis", publishmentSystemID, PageUtils.FilterSql(filterSearchWords), count);

            using (IDataReader rdr = this.ExecuteReader(CMD))
            {

                while (rdr.Read())
                {
                    nodeInfo = new NodeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), ENodeTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), rdr.GetDateTime(13), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetInt32(16), TranslateUtils.ToInt(rdr.GetValue(17).ToString()), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString(), rdr.GetValue(20).ToString(), rdr.GetValue(21).ToString(), ELinkTypeUtils.GetEnumType(rdr.GetValue(22).ToString()), rdr.GetInt32(23), rdr.GetInt32(24), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString());
                    retval.Add(nodeInfo);
                }
                rdr.Close();
            }
            return retval;
        }

        public ArrayList GetNodeInfoByBrandName(int publishmentSystemID, string filterSearchWords, int count)
        {
            ArrayList retval = new ArrayList();
            NodeInfo nodeInfo = null;
            string CMD = string.Format(@"SELECT TOP {2} NodeID, NodeName, NodeType, PublishmentSystemID, ContentModelID, ParentID, ParentsPath, ParentsCount, ChildrenCount, IsLastNode, NodeIndexName,                                         NodeGroupNameCollection, Taxis, AddDate, ImageUrl, Content, ContentNum, CommentNum, FilePath, ChannelFilePathRule, ContentFilePathRule, LinkUrl, LinkType, ChannelTemplateID,        
                                ContentTemplateID, Keywords, Description, ExtendValues FROM siteserver_node where ContentModelID = 'Goods' AND nodeid in(SELECT NodeID FROM b2c_Filter
                                WHERE FilterID in(
                                SELECT FilterID FROM b2c_filteritem WHERE AttributeName = 'BrandID' AND PublishmentSystemID = {0} AND (Title like '%{1}%' or Title like '%{1}%')
                                ))", publishmentSystemID, PageUtils.FilterSql(filterSearchWords), count);
            using (IDataReader rdr = this.ExecuteReader(CMD))
            {

                while (rdr.Read())
                {
                    nodeInfo = new NodeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), ENodeTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), rdr.GetDateTime(13), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetInt32(16), TranslateUtils.ToInt(rdr.GetValue(17).ToString()), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString(), rdr.GetValue(20).ToString(), rdr.GetValue(21).ToString(), ELinkTypeUtils.GetEnumType(rdr.GetValue(22).ToString()), rdr.GetInt32(23), rdr.GetInt32(24), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString());
                    retval.Add(nodeInfo);
                }
                rdr.Close();
            }
            return retval;
        }

        public ArrayList GetNodeInfoByConentName(int publishmentSystemID, string filterSearchWords, int count)
        {
            ArrayList retval = new ArrayList();
            NodeInfo nodeInfo = null;
            ArrayList tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.GoodsContent);
            foreach (AuxiliaryTableInfo table in tableArrayList)
            {
                string CMD = string.Format(@"SELECT TOP {3} * FROM siteserver_node WHERE nodeid in(
                                            SELECT NodeID FROM {2} WHERE ContentModelID = 'Goods' AND Title like '%{1}%' AND NodeID > 0 AND PublishmentSystemID = {0} GROUP BY NodeID
                                            ) ", publishmentSystemID, PageUtils.FilterSql(filterSearchWords), table.TableENName, count);
                using (IDataReader rdr = this.ExecuteReader(CMD))
                {

                    while (rdr.Read())
                    {
                        nodeInfo = new NodeInfo(rdr.GetInt32(0), rdr.GetValue(1).ToString(), ENodeTypeUtils.GetEnumType(rdr.GetValue(2).ToString()), rdr.GetInt32(3), rdr.GetValue(4).ToString(), rdr.GetInt32(5), rdr.GetValue(6).ToString(), rdr.GetInt32(7), rdr.GetInt32(8), TranslateUtils.ToBool(rdr.GetValue(9).ToString()), rdr.GetValue(10).ToString(), rdr.GetValue(11).ToString(), rdr.GetInt32(12), rdr.GetDateTime(13), rdr.GetValue(14).ToString(), rdr.GetValue(15).ToString(), rdr.GetInt32(16), TranslateUtils.ToInt(rdr.GetValue(17).ToString()), rdr.GetValue(18).ToString(), rdr.GetValue(19).ToString(), rdr.GetValue(20).ToString(), rdr.GetValue(21).ToString(), ELinkTypeUtils.GetEnumType(rdr.GetValue(22).ToString()), rdr.GetInt32(23), rdr.GetInt32(24), rdr.GetValue(25).ToString(), rdr.GetValue(26).ToString(), rdr.GetValue(27).ToString());

                        retval.Add(nodeInfo);
                    }
                    rdr.Close();
                }
            }
            return retval;
        }

        #region by 20160229 sofuny 评价管理

        /// <summary>
        /// 获取某个栏目下启用评价管理的栏目 ID
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        public List<int> GetNodeIDListForDescendantUseEvaluation(int publishmentSystemID, int nodeID)
        {
            string andStr = string.Format(" and ExtendValues like '%&UseEvaluation={0}%'", EBoolean.True.ToString());
            string sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node
WHERE PublishmentSystemID = {1} and ((ParentsPath LIKE '{0},%') OR
      (ParentsPath LIKE '%,{0},%') OR
      (ParentsPath LIKE '%,{0}') OR
      (ParentID = {0})) {2} 
", nodeID, publishmentSystemID, andStr);
            List<int> list = new List<int>();

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
        /// 根据功能类型获取栏目
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        public ArrayList GetNodeIDByFunction(int publishmentSystemID, string functionStr)
        {
            string sqlString = string.Format(@" select (ParentsPath+','+CONVERT(nvarchar(20) ,NodeID)) as NodeID from dbo.siteserver_Node 
  where PublishmentSystemID={0} and ExtendValues like '%&{1}=True%'", publishmentSystemID, functionStr);
            ArrayList list = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    ArrayList ids = TranslateUtils.StringCollectionToArrayList(rdr.GetString(0));
                    foreach (string id in ids)
                    {
                        if (!string.IsNullOrEmpty(id) && !list.Contains(id))
                        {
                            list.Add(id);
                        }
                    }
                }
                rdr.Close();
            }

            return list;
        }

        /// <summary>
        /// 获取指定范围内容的栏目
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="parentID"></param>
        /// <param name="nodeIDList"></param>
        /// <returns></returns>
        public ArrayList GetNodeIDArrayListByParentID(int publishmentSystemID, int parentID, ArrayList nodeIDList)
        {
            string sqlString = string.Empty;
            if (parentID == 0)
            {
                sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node
WHERE (NodeID = {0} OR ParentID = {0} ) and NodeID in ({1})
ORDER BY Taxis", publishmentSystemID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDList));
            }
            else
            {
                sqlString = string.Format(@"SELECT NodeID
FROM siteserver_Node
WHERE (ParentID = {0} and NodeID in ({1}))
ORDER BY Taxis", parentID, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(nodeIDList));
            }

            ArrayList arraylist = new ArrayList();

            using (IDataReader rdr = this.ExecuteReader(sqlString))
            {
                while (rdr.Read())
                {
                    int nodeID = Convert.ToInt32(rdr[0]);
                    arraylist.Add(nodeID);
                }
                rdr.Close();
            }

            return arraylist;
        }

        #endregion
    }
}
