using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.Pages.MLibManage
{

    public class Contents : SystemBasePage
    {
        public TextBox Keyword;
        public HtmlInputText start;
        public HtmlInputText end;
        public PlaceHolder phContents;
        public Repeater dlContents;
        public SqlPager spContents;
        public Literal ltlCount;
        public PlaceHolder phNoData;
        public Literal ltlContentType;
        public string homeUrl;
        public DropDownList ddlPublishmentSystem;
        public DropDownList NodeIDDropDownList;
        public Literal ltlCountUser;

        public ArrayList nodeList = new ArrayList();

        #region 下载
        public PlaceHolder phShow;
        public PlaceHolder phExport;
        public HyperLink lkDown;
        #endregion


        public void Page_Load(object sender, EventArgs e)
        {
            if (ConfigManager.Additional.IsUseMLib == false)
            {
                Response.Write("<script>alert('投稿中心尚未开启.');history.go(-1);</script>");
                Response.End();
            }
            //if (string.IsNullOrEmpty(ConfigManager.Additional.MLibPublishmentSystemIDs))
            //{
            //    Response.Write("<script>alert('投稿中心尚未开启.');history.go(-1);</script>");
            //    Response.End();
            //    return;
            //}

            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetUniqueUserCenter();
            if (publishmentSystemInfo != null)
                homeUrl = publishmentSystemInfo.PublishmentSystemDir;

            #region 投稿范围
            //this.ddlPublishmentSystem.Items.Clear();
            //string MLibPDs = string.Empty;
            //if (UserManager.Current.NewGroupID != 0 && UserManager.CurrentNewGroup.ItemID != 0)
            //{
            //    if (!string.IsNullOrEmpty(UserManager.CurrentNewGroup.SetXML))
            //        if (UserManager.CurrentNewGroup.Additional.IsUseMLibScope)
            //            MLibPDs = ConfigManager.Additional.MLibPublishmentSystemIDs;
            //        else
            //            MLibPDs = UserManager.CurrentNewGroup.Additional.MLibPublishmentSystemIDs;
            //    else
            //        MLibPDs = ConfigManager.Additional.MLibPublishmentSystemIDs;
            //}
            //else
            //    MLibPDs = ConfigManager.Additional.MLibPublishmentSystemIDs;

            //if (this.ddlPublishmentSystem.Items.Count == 0)
            //{
            //    ArrayList MLibPublishmentSystemIDs = TranslateUtils.StringCollectionToArrayList(MLibPDs);
            //    foreach (string pid in MLibPublishmentSystemIDs)
            //    {
            //        PublishmentSystemInfo info = PublishmentSystemManager.GetPublishmentSystemInfo(TranslateUtils.ToInt(pid));
            //        if (info == null)
            //            continue;
            //        ListItem item = new ListItem(info.PublishmentSystemName, info.PublishmentSystemID.ToString());
            //        this.ddlPublishmentSystem.Items.Add(item);
            //    }
            //}
            #endregion
            #region 获取稿件发布都有权限的站点和栏目
            if (this.ddlPublishmentSystem.Items.Count == 0)
            {
                this.ddlPublishmentSystem.Items.Clear();
                foreach (PublishmentSystemInfo info in PublishmentSystemManager.GetPublishmentSystem(UserManager.CurrentNewGroupMLibAddUser))
                {
                    ListItem item = new ListItem(info.PublishmentSystemName, info.PublishmentSystemID.ToString());
                    this.ddlPublishmentSystem.Items.Add(item);
                }
            }
            #endregion
            if (!IsPostBack)
            {
                //if (!string.IsNullOrEmpty(base.Request.QueryString["Delete"]))
                //{
                //    ArrayList idsArrayList = TranslateUtils.StringCollectionToArrayList(Request.QueryString["IDsCollection"]);
                //    Hashtable hashtable = new Hashtable();
                //    foreach (string ids in idsArrayList)
                //    {
                //        int nodeID = TranslateUtils.ToInt(ids.Split('_')[0]);
                //        int contentID = TranslateUtils.ToInt(ids.Split('_')[1]);
                //        if (hashtable.Contains(nodeID))
                //        {
                //            hashtable[nodeID] = hashtable[nodeID] as string + "," + contentID.ToString();
                //        }
                //        else
                //        {
                //            hashtable[nodeID] = contentID.ToString();
                //        }
                //    }
                //    foreach (int nodeID in hashtable.Keys)
                //    {
                //        ArrayList contentIDArrayList = TranslateUtils.StringCollectionToIntArrayList(hashtable[nodeID] as string);
                //    }
                //}

                this.ltlCountUser.Text = UserManager.Current.MLibNum.ToString();

                ddlPublishmentSystem_SelectedIndexChanged(sender, e);
            }

            if (!string.IsNullOrEmpty(base.Request.QueryString["Export"]))
            {
                this.phShow.Visible = false;
                this.phExport.Visible = true;
                string docFileName = UserManager.Current.UserName + "的稿件.xls";
                create(docFileName);
                this.lkDown.NavigateUrl = PageUtils.GetTemporaryFilesUrl(docFileName);
                this.lkDown.Text = "&nbsp;&nbsp; 下载 &nbsp;&nbsp; ";

                //Response.Write("<script>window.location.href='" + PageUtils.GetTemporaryFilesUrl(docFileName) + "';</script>");
                //Response.End();            
            }
        }

        public void ContentBind()
        {
            string keyWord = Keyword.Text.Trim();
            string startInfo = start.Value;
            string endInfo = end.Value;

            int publishmentSystemID = TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue);
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            int nodeId = TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue);

            this.spContents.ControlToPaginate = this.dlContents;
            this.dlContents.ItemDataBound += new RepeaterItemEventHandler(dlContents_ItemDataBound);

            this.spContents.ItemsPerPage = 20;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;

            StringBuilder whereString = new StringBuilder();

            whereString.AppendFormat(" WHERE MemberName = '{0}'", UserManager.Current.UserName);
            if (!string.IsNullOrEmpty(keyWord))
            {
                whereString.AppendFormat(" AND Title like '%{0}%'", keyWord);
            }
            if (!string.IsNullOrEmpty(startInfo) && !string.IsNullOrEmpty(endInfo))
            {
                whereString.Append(" AND (AddDate>='" + startInfo + "' AND AddDate<='" + endInfo + "') ");
            }
            whereString.AppendFormat(" AND PublishmentSystemID = {0}", publishmentSystemID.ToString());
            if (nodeId == 0)
            {
                nodeId = publishmentSystemID;

                string nodeStr = string.Empty;
                if (nodeList.Count == 0)
                {
                    ArrayList mLibNodeInfoArrayList = PublishmentSystemManager.GetNode(UserManager.CurrentNewGroupMLibAddUser, publishmentSystemInfo.PublishmentSystemID);
                    foreach (NodeInfo info in mLibNodeInfoArrayList)
                    { 
                        nodeStr += " NodeID = " + info.NodeID + " OR";
                    }
                }
                else
                {
                    foreach (int id in nodeList)
                    {
                        nodeStr += " NodeID = " + id + " OR";
                    }
                }
                if (!string.IsNullOrEmpty(nodeStr))
                    nodeStr = nodeStr.Substring(0, nodeStr.Length - 3);
                if (!string.IsNullOrEmpty(nodeStr))
                    whereString.AppendFormat(" AND ({0})", nodeStr);
            }
            else
            {
                whereString.AppendFormat(" AND NodeID = {0}", nodeId.ToString());
            }



            //try
            //{

            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemID, nodeId);
            ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
            string nodeTableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

            this.spContents.SelectCommand = "SELECT * FROM " + nodeTableName + whereString.ToString();
            this.spContents.SortField = ContentAttribute.AddDate;
            this.spContents.SortMode = SortMode.DESC;


            this.spContents.DataBind();
            //}
            //catch
            //{
            //    Response.Write("<script>alert('请选择栏目.');history.go(-1);</script>");
            //    Response.End();
            //}

            this.ltlCount.Text = this.spContents.TotalCount.ToString();

            if (this.spContents.TotalCount > 0)
            {
                this.phContents.Visible = true;
                this.phNoData.Visible = false;
            }
            else
            {
                this.phContents.Visible = false;
                this.phNoData.Visible = true;
            }
        }

        private void dlContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Literal ltlID = e.Item.FindControl("ltlID") as Literal;
                Literal ltlContent = e.Item.FindControl("ltlContent") as Literal;
                Literal ltlChannel = e.Item.FindControl("ltlChannel") as Literal;
                Literal ltlState = e.Item.FindControl("ltlState") as Literal;
                Literal ltlDateTime = e.Item.FindControl("ltlDateTime") as Literal;
                Literal ltlOperate = e.Item.FindControl("ltlOperate") as Literal;

                ContentInfo contentInfo = new ContentInfo(e.Item.DataItem);

                if (contentInfo != null)
                {
                    ltlContent.Text = contentInfo.Title;

                    string nodeName = NodeManager.GetNodeNameNavigation(contentInfo.PublishmentSystemID, contentInfo.NodeID);

                    ltlChannel.Text = nodeName;

                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(contentInfo.PublishmentSystemID);
                    ltlState.Text = LevelManager.GetCheckState(publishmentSystemInfo, contentInfo.IsChecked, contentInfo.CheckedLevel);

                    ltlDateTime.Text = contentInfo.AddDate.ToString();
                    ltlOperate.Text = GetOperateText(contentInfo, contentInfo.IsChecked, contentInfo.CheckedLevel, contentInfo.ID);
                }
                else
                {
                    e.Item.Visible = false;
                }
            }
        }


        public void ddlPublishmentSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            int publishmentSystemId = TranslateUtils.ToInt(this.ddlPublishmentSystem.SelectedValue);
            PublishmentSystemInfo pinfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemId);
            this.NodeIDDropDownList.Items.Clear();
            this.nodeList.Clear();
            ListItem itemd = new ListItem("全部", "0");
            this.NodeIDDropDownList.Items.Add(itemd);
            ArrayList mLibNodeInfoArrayList = PublishmentSystemManager.GetNode(UserManager.CurrentNewGroupMLibAddUser, pinfo.PublishmentSystemID);
            foreach (NodeInfo nodeInfo in mLibNodeInfoArrayList)
            {
                ListItem item = new ListItem(nodeInfo.NodeName, nodeInfo.NodeID.ToString());
                this.NodeIDDropDownList.Items.Add(item);
                this.nodeList.Add(nodeInfo.NodeID);
            }
            ContentBind();
        }

        public void NodeIDDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContentBind();
        }


        public string GetOperateText(ContentInfo cinfo, bool isChecked, int CheckedLevel, int id)
        {
            string returnVal = "";

            if (isChecked)
            {
                PublishmentSystemInfo info = PublishmentSystemManager.GetPublishmentSystemInfo(cinfo.PublishmentSystemID);
                string url = PageUtility.GetContentUrl(info, cinfo, true, info.Additional.VisualType);
                //string publishmentSystemPath = PathUtility.GetPublishmentSystemPath(info);
                //string requestPath = publishmentSystemPath.Replace(PathUtils.SeparatorChar, PageUtils.SeparatorChar);
                //string fhurl = PageUtils.Combine(requestPath, url);
                ////查看网页
                //if (FileUtils.IsFileExists(fhurl))
                returnVal = string.Format("<a href='{0}' target='blank'>查看</a>", url);
                //else
                //    returnVal = "<a href='submissionShow.aspx?PublishmentSystemID=" + cinfo.PublishmentSystemID + "&NodeID=" + cinfo.NodeID + "&ID=" + id + "'>查看</a>";
            }
            else
            {
                //未审核 则显示信息
                returnVal = "<a href='submissionShow.aspx?PublishmentSystemID=" + cinfo.PublishmentSystemID + "&NodeID=" + cinfo.NodeID + "&ID=" + id + "'>查看</a>";
            }

            return returnVal;
        }

        public string GetDeleteUrl(string idsCollection)
        {
            return string.Format(@"contents.aspx?type={1}&Delete=True&IDsCollection={2}", "", idsCollection);
        }

        public void Search_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                ContentBind();
            }
        }


        public void create(string docFileName)
        {

            ArrayList mLibScopeInfoArrayList = new ArrayList();
            //if (UserManager.Current.NewGroupID != 0 && UserManager.CurrentNewGroup.ItemID != 0)
            //{
            //    if (UserManager.CurrentNewGroup.Additional.IsUseMLibScope)
            //    {
            //        mLibScopeInfoArrayList = TranslateUtils.StringCollectionToArrayList(ConfigManager.Additional.MLibPublishmentSystemIDs);
            //    }
            //    else
            //    {
            //        mLibScopeInfoArrayList = TranslateUtils.StringCollectionToArrayList(UserManager.CurrentNewGroup.Additional.MLibPublishmentSystemIDs);
            //    }
            //}
            //else
            //{
            //    mLibScopeInfoArrayList = TranslateUtils.StringCollectionToArrayList(ConfigManager.Additional.MLibPublishmentSystemIDs);
            //}
            #region 获取稿件发布都有权限的站点和栏目

            mLibScopeInfoArrayList = PublishmentSystemManager.GetPublishmentSystem(UserManager.CurrentNewGroupMLibAddUser);

            #endregion
            string httpHost = Request.Url.Host;

            string filePath = PathUtils.GetTemporaryFilesPath(docFileName);
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            string OLEDBConnStr = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", filePath);
            OLEDBConnStr += " Extended Properties=Excel 8.0;";
            StringBuilder createBuilder = new StringBuilder();

            createBuilder.Append("CREATE TABLE tb_mlib ( ");

            createBuilder.AppendFormat(" [{0}] NTEXT, ", "站点栏目");
            createBuilder.AppendFormat(" [{0}] NTEXT, ", "标题");
            createBuilder.AppendFormat(" [{0}] NTEXT, ", "链接");
            createBuilder.Length = createBuilder.Length - 2;
            createBuilder.Append(" )");

            StringBuilder preInsertBuilder = new StringBuilder();
            preInsertBuilder.Append("INSERT INTO tb_mlib (");

            preInsertBuilder.AppendFormat("[{0}], ", "站点栏目");
            preInsertBuilder.AppendFormat("[{0}], ", "标题");
            preInsertBuilder.AppendFormat("[{0}], ", "链接");
            preInsertBuilder.Length = preInsertBuilder.Length - 2;
            preInsertBuilder.Append(" ) VALUES (");


            ArrayList insertSqlArrayList = new ArrayList();

            foreach (PublishmentSystemInfo pinfo in mLibScopeInfoArrayList)
            {
                if (pinfo == null)
                    break;
                string tableName = pinfo.AuxiliaryTableForContent;
                DataSet ds = new DataSet();
                try
                {
                    ds = DataProvider.ContentDAO.GetDateSet(tableName, pinfo.PublishmentSystemID, string.Empty, string.Empty, string.Empty, true, true, UserManager.Current.UserName);


                    foreach (DataTable dt in ds.Tables)
                    {
                        //定义表对象与行对象，同时用DataSet对其值进行初始化  
                        DataRow[] myRow = dt.Select();//可以类似dt.Select("id>10")之形式达到数据筛选目的 
                        int cl = dt.Columns.Count;

                        //向HTTP输出流中写入取得的数据信息 

                        //逐行处理数据   
                        foreach (DataRow row in myRow)
                        {
                            if (pinfo == null)
                                break;
                            string nodeName = NodeManager.GetNodeNameNavigation(TranslateUtils.ToInt(row[0].ToString()), TranslateUtils.ToInt(row[1].ToString()));

                            ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(ETableStyle.BackgroundContent,
                           tableName, TranslateUtils.ToInt(row[3].ToString()));


                            StringBuilder insertBuilder = new StringBuilder();
                            insertBuilder.Append(preInsertBuilder.ToString());

                            insertBuilder.AppendFormat("'{0}', ", pinfo.PublishmentSystemName + " > " + nodeName);
                            insertBuilder.AppendFormat("'{0}', ", Server.HtmlDecode(row[2].ToString()));
                            insertBuilder.AppendFormat("'{0}', ", PageUtility.GetContentUrl(pinfo, contentInfo, true, pinfo.Additional.VisualType));

                            insertBuilder.Length = insertBuilder.Length - 2;
                            insertBuilder.Append(") ");

                            insertSqlArrayList.Add(insertBuilder.ToString());

                        }
                    }
                }
                catch
                {

                }
            }
            OleDbConnection oConn = new OleDbConnection();

            oConn.ConnectionString = OLEDBConnStr;
            OleDbCommand oCreateComm = new OleDbCommand();
            oCreateComm.Connection = oConn;
            oCreateComm.CommandText = createBuilder.ToString();

            oConn.Open();
            oCreateComm.ExecuteNonQuery();
            foreach (string insertSql in insertSqlArrayList)
            {
                OleDbCommand oInsertComm = new OleDbCommand();
                oInsertComm.Connection = oConn;
                oInsertComm.CommandText = insertSql;
                oInsertComm.ExecuteNonQuery();
            }
            oConn.Close();


        }
    }
}
