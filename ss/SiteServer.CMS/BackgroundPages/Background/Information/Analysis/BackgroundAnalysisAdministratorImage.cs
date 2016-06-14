using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Controls;
using SiteServer.CMS.Core.Security;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;



using BaiRong.Model;
using System.Data;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundAnalysisAdministratorImage : BackgroundBasePage
    {
        #region 站点
        //总数
        private readonly Hashtable HorizentalHashtable = new Hashtable();
        private readonly Hashtable VerticalHashtable = new Hashtable();
        //sort key
        public ArrayList keyArrayList = new ArrayList();
        //x
        public readonly Hashtable XHashtable = new Hashtable();
        //y
        public readonly Hashtable YHashtableNew = new Hashtable();
        public readonly Hashtable YHashtableUpdate = new Hashtable();
        public readonly Hashtable YHashtableRemark = new Hashtable();
        #endregion
        #region 管理员
        //总数
        private readonly Hashtable HorizentalHashtableUser = new Hashtable();
        private readonly Hashtable VerticalHashtableUser = new Hashtable();
        //sort key
        public ArrayList keyArrayListUser = new ArrayList();
        //x
        public readonly Hashtable XHashtableUser = new Hashtable();
        //y
        public readonly Hashtable YHashtableUserNew = new Hashtable();
        public readonly Hashtable YHashtableUserUpdate = new Hashtable();
        public readonly Hashtable YHashtableUserRemark = new Hashtable();
        #endregion

        //y轴类型
        public const string YType_New = "YType_New";
        public const string YType_Update = "YType_Update";
        public const string YType_Remrk = "YType_Remrk";
        //其他类型
        public const int YType_Other = -1;


        public DateTimeTextBox StartDate;
        public DateTimeTextBox EndDate;

        private NameValueCollection additional;

        private DateTime begin;
        private DateTime end;

        public string returnUrl;

        #region 按照站点统计
        /// <summary>
        /// 设置x轴数据
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="publishmentSystemName"></param>
        public void SetXHashtable(int publishmentSystemID, string publishmentSystemName)
        {
            if (publishmentSystemID == YType_Other)
            {
                if (!XHashtable.ContainsKey(YType_Other))
                {
                    XHashtable.Add(YType_Other, "其他");
                }
            }
            else if (!XHashtable.ContainsKey(publishmentSystemID))
            {
                XHashtable.Add(publishmentSystemID, publishmentSystemName);
            }
            if (!keyArrayList.Contains(publishmentSystemID))
            {
                keyArrayList.Add(publishmentSystemID);
            }
            keyArrayList.Sort();
            keyArrayList.Reverse();
        }
        /// <summary>
        /// 获取x轴数据
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        public string GetXHashtable(int publishmentSystemID)
        {
            if (XHashtable.ContainsKey(publishmentSystemID))
            {
                return XHashtable[publishmentSystemID].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 设置y轴数据
        /// </summary>
        /// <param name="publishemtSystemID"></param>
        /// <param name="value"></param>
        /// <param name="yType"></param>
        public void SetYHashtable(int publishemtSystemID, int value, string yType)
        {
            switch (yType)
            {
                case YType_New:
                    if (!YHashtableNew.ContainsKey(publishemtSystemID))
                    {
                        YHashtableNew.Add(publishemtSystemID, value);
                    }
                    else
                    {
                        int num = TranslateUtils.ToInt(YHashtableNew[publishemtSystemID].ToString());
                        YHashtableNew[publishemtSystemID] = num + value;
                    }
                    SetVertical(YType_New, value);
                    break;
                case YType_Update:
                    if (!YHashtableUpdate.ContainsKey(publishemtSystemID))
                    {
                        YHashtableUpdate.Add(publishemtSystemID, value);
                    }
                    else
                    {
                        int num = TranslateUtils.ToInt(YHashtableUpdate[publishemtSystemID].ToString());
                        YHashtableUpdate[publishemtSystemID] = num + value;
                    }
                    SetVertical(YType_Update, value);
                    break;
                case YType_Remrk:
                    if (!YHashtableRemark.ContainsKey(publishemtSystemID))
                    {
                        YHashtableRemark.Add(publishemtSystemID, value);
                    }
                    else
                    {
                        int num = TranslateUtils.ToInt(YHashtableRemark[publishemtSystemID].ToString());
                        YHashtableRemark[publishemtSystemID] = num + value;
                    }
                    SetVertical(YType_Remrk, value);
                    break;
                default:
                    break;
            }
            SetHorizental(publishemtSystemID, value);
        }
        /// <summary>
        /// 获取y轴数据
        /// </summary>
        /// <param name="publishemtSystemID"></param>
        /// <param name="value"></param>
        /// <param name="yType"></param>
        public string GetYHashtable(int publishemtSystemID, string yType)
        {
            switch (yType)
            {
                case YType_New:
                    if (YHashtableNew.ContainsKey(publishemtSystemID))
                    {
                        int num = TranslateUtils.ToInt(YHashtableNew[publishemtSystemID].ToString());
                        return num.ToString();
                    }
                    else
                    {
                        return "0";
                    }
                case YType_Update:
                    if (YHashtableUpdate.ContainsKey(publishemtSystemID))
                    {
                        int num = TranslateUtils.ToInt(YHashtableUpdate[publishemtSystemID].ToString());
                        return num.ToString();
                    }
                    else
                    {
                        return "0";
                    }
                case YType_Remrk:
                    if (YHashtableRemark.ContainsKey(publishemtSystemID))
                    {
                        int num = TranslateUtils.ToInt(YHashtableRemark[publishemtSystemID].ToString());
                        return num.ToString();
                    }
                    else
                    {
                        return "0";
                    }

                default:
                    return "0";
            }
        }
        /// <summary>
        /// 设置y总数
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="num"></param>
        public void SetHorizental(int publishmentSystemID, int num)
        {
            if (HorizentalHashtable[publishmentSystemID] == null)
            {
                HorizentalHashtable[publishmentSystemID] = num;
            }
            else
            {
                int totalNum = (int)HorizentalHashtable[publishmentSystemID];
                HorizentalHashtable[publishmentSystemID] = totalNum + num;
            }
        }
        /// <summary>
        /// 获取y总数
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        public string GetHorizental(int publishmentSystemID)
        {
            if (HorizentalHashtable[publishmentSystemID] != null)
            {
                int num = TranslateUtils.ToInt(HorizentalHashtable[publishmentSystemID].ToString());
                return (num == 0) ? "0" : string.Format("<strong>{0}</strong>", num);
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 设置type总数
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="num"></param>
        public void SetVertical(string type, int num)
        {
            if (VerticalHashtable[type] == null)
            {
                VerticalHashtable[type] = num;
            }
            else
            {
                int totalNum = (int)VerticalHashtable[type];
                VerticalHashtable[type] = totalNum + num;
            }
        }
        /// <summary>
        /// 获取type总数
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <returns></returns>
        public string GetVertical(string type)
        {
            if (VerticalHashtable[type] != null)
            {
                int num = TranslateUtils.ToInt(VerticalHashtable[type].ToString());
                return (num == 0) ? "0" : string.Format("<strong>{0}</strong>", num);
            }
            else
            {
                return "0";
            }
        }
        #endregion
        #region 按照管理员统计
        /// <summary>
        /// 设置x轴数据
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="publishmentSystemName"></param>
        public void SetXHashtableUser(string userName, string publishmentSystemName)
        {
            if (!XHashtableUser.ContainsKey(userName))
            {
                XHashtableUser.Add(userName, publishmentSystemName);
            }
            if (!keyArrayListUser.Contains(userName))
            {
                keyArrayListUser.Add(userName);
            }
            keyArrayListUser.Sort();
            keyArrayListUser.Reverse();
        }
        /// <summary>
        /// 获取x轴数据
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string GetXHashtableUser(string userName)
        {
            if (XHashtableUser.ContainsKey(userName))
            {
                return XHashtableUser[userName].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 设置y轴数据
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="value"></param>
        /// <param name="yType"></param>
        public void SetYHashtableUser(string userName, int value, string yType)
        {
            switch (yType)
            {
                case YType_New:
                    if (!YHashtableUserNew.ContainsKey(userName))
                    {
                        YHashtableUserNew.Add(userName, value);
                    }
                    else
                    {
                        int num = TranslateUtils.ToInt(YHashtableUserNew[userName].ToString());
                        YHashtableUserNew[userName] = num + value;
                    }
                    SetVerticalUser(YType_New, value);
                    break;
                case YType_Update:
                    if (!YHashtableUserUpdate.ContainsKey(userName))
                    {
                        YHashtableUserUpdate.Add(userName, value);
                    }
                    else
                    {
                        int num = TranslateUtils.ToInt(YHashtableUserUpdate[userName].ToString());
                        YHashtableUserUpdate[userName] = num + value;
                    }
                    SetVerticalUser(YType_Update, value);
                    break;
                case YType_Remrk:
                    if (!YHashtableUserRemark.ContainsKey(userName))
                    {
                        YHashtableUserRemark.Add(userName, value);
                    }
                    else
                    {
                        int num = TranslateUtils.ToInt(YHashtableUserRemark[userName].ToString());
                        YHashtableUserRemark[userName] = num + value;
                    }
                    SetVerticalUser(YType_Remrk, value);
                    break;
                default:
                    break;
            }
            SetHorizentalUser(userName, value);
        }
        /// <summary>
        /// 获取y轴数据
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="value"></param>
        /// <param name="yType"></param>
        public string GetYHashtableUser(string userName, string yType)
        {
            switch (yType)
            {
                case YType_New:
                    if (YHashtableUserNew.ContainsKey(userName))
                    {
                        int num = TranslateUtils.ToInt(YHashtableUserNew[userName].ToString());
                        return num.ToString();
                    }
                    else
                    {
                        return "0";
                    }
                case YType_Update:
                    if (YHashtableUserUpdate.ContainsKey(userName))
                    {
                        int num = TranslateUtils.ToInt(YHashtableUserUpdate[userName].ToString());
                        return num.ToString();
                    }
                    else
                    {
                        return "0";
                    }
                case YType_Remrk:
                    if (YHashtableUserRemark.ContainsKey(userName))
                    {
                        int num = TranslateUtils.ToInt(YHashtableUserRemark[userName].ToString());
                        return num.ToString();
                    }
                    else
                    {
                        return "0";
                    }

                default:
                    return "0";
            }
        }
        /// <summary>
        /// 设置y总数
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="num"></param>
        public void SetHorizentalUser(string userName, int num)
        {
            if (HorizentalHashtableUser[userName] == null)
            {
                HorizentalHashtableUser[userName] = num;
            }
            else
            {
                int totalNum = (int)HorizentalHashtableUser[userName];
                HorizentalHashtableUser[userName] = totalNum + num;
            }
        }
        /// <summary>
        /// 获取y总数
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string GetHorizentalUser(string userName)
        {
            if (HorizentalHashtableUser[userName] != null)
            {
                int num = TranslateUtils.ToInt(HorizentalHashtableUser[userName].ToString());
                return (num == 0) ? "0" : string.Format("<strong>{0}</strong>", num);
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 设置type总数
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="num"></param>
        public void SetVerticalUser(string type, int num)
        {
            if (VerticalHashtableUser[type] == null)
            {
                VerticalHashtableUser[type] = num;
            }
            else
            {
                int totalNum = (int)VerticalHashtableUser[type];
                VerticalHashtableUser[type] = totalNum + num;
            }
        }
        /// <summary>
        /// 获取type总数
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string GetVerticalUser(string type)
        {
            if (VerticalHashtableUser[type] != null)
            {
                int num = TranslateUtils.ToInt(VerticalHashtableUser[type].ToString());
                return (num == 0) ? "0" : string.Format("<strong>{0}</strong>", num);
            }
            else
            {
                return "0";
            }
        }
        #endregion

        public static string GetRedirectUrlString(int publishmentSystemID, string returnUrl)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("publishmentSystemID", publishmentSystemID.ToString());
            nvc.Add("returnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtils.GetCMSUrl(PageUtils.AddQueryString("background_analysisAdministratorImage.aspx", nvc));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (string.IsNullOrEmpty(base.GetQueryString("StartDate")))
            {
                this.begin = DateTime.Now.AddMonths(-1);
                this.end = DateTime.Now;
            }
            else
            {
                this.begin = TranslateUtils.ToDateTime(base.GetQueryString("StartDate"));
                this.end = TranslateUtils.ToDateTime(base.GetQueryString("EndDate"));
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, AppManager.CMS.LeftMenu.Content.ID_SiteAnalysis, "管理员工作量统计", AppManager.CMS.Permission.WebSite.SiteAnalysis);

                this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("returnUrl"));
                this.StartDate.Text = DateUtils.GetDateAndTimeString(this.begin);
                this.EndDate.Text = DateUtils.GetDateAndTimeString(this.end);

                this.additional = new NameValueCollection();
                additional["StartDate"] = this.StartDate.Text;
                additional["EndDate"] = this.EndDate.Text;

                JsManager.RegisterClientScriptBlock(Page, "NodeTreeScript", ChannelLoading.GetScript(base.PublishmentSystemInfo, ELoadingType.SiteAnalysis, this.additional));

                BindGrid();
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string userName = TranslateUtils.EvalString(e.Item.DataItem, "userName");
                int addCount = TranslateUtils.EvalInt(e.Item.DataItem, "addCount");
                int updateCount = TranslateUtils.EvalInt(e.Item.DataItem, "updateCount");
                int commentCount = TranslateUtils.EvalInt(e.Item.DataItem, "commentCount");

                Literal ltlUserName = (Literal)e.Item.FindControl("ltlUserName");
                Literal ltlDisplayName = (Literal)e.Item.FindControl("ltlDisplayName");
                Literal ltlContentAdd = (Literal)e.Item.FindControl("ltlContentAdd");
                Literal ltlContentUpdate = (Literal)e.Item.FindControl("ltlContentUpdate");
                Literal ltlContentComment = (Literal)e.Item.FindControl("ltlContentComment");

                ltlUserName.Text = userName;
                ltlDisplayName.Text = BaiRongDataProvider.AdministratorDAO.GetDisplayName(userName);

                ltlContentAdd.Text = (addCount == 0) ? "0" : string.Format("<strong>{0}</strong>", addCount);
                ltlContentUpdate.Text = (updateCount == 0) ? "0" : string.Format("<strong>{0}</strong>", updateCount);
                ltlContentComment.Text = (commentCount == 0) ? "0" : string.Format("<strong>{0}</strong>", commentCount);
            }
        }

        public void BindGrid()
        {
            ArrayList nodeIDArray = DataProvider.NodeDAO.GetNodeIDArrayListByParentID(base.PublishmentSystemID, 0);
            foreach (int nodeID in nodeIDArray)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeID);
                string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);


                //x轴信息
                SetXHashtable(nodeID, nodeInfo.NodeName);
                //y轴信息
                SetYHashtable(nodeID
                                           , DataProvider.ContentDAO.GetCountOfContentAdd(tableName, base.PublishmentSystemID, nodeInfo.NodeID, TranslateUtils.ToDateTime(additional["StartDate"]), TranslateUtils.ToDateTime(additional["EndDate"]), string.Empty)
                                           , YType_New);
                SetYHashtable(nodeID
                                            , DataProvider.ContentDAO.GetCountOfContentUpdate(tableName, base.PublishmentSystemID, nodeInfo.NodeID, TranslateUtils.ToDateTime(additional["StartDate"]), TranslateUtils.ToDateTime(additional["EndDate"]), string.Empty)
                                            , YType_Update);
                SetYHashtable(nodeID
                                            , DataProvider.CommentDAO.GetCountChecked(base.PublishmentSystemID, nodeInfo.NodeID, TranslateUtils.ToDateTime(additional["StartDate"]), TranslateUtils.ToDateTime(additional["EndDate"]))
                                            , YType_Remrk);
            }
            DataSet ds = BaiRongDataProvider.ContentDAO.GetDataSetOfAdminExcludeRecycle(base.PublishmentSystemInfo.AuxiliaryTableForContent, base.PublishmentSystemID, this.begin, this.end);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        //x轴信息
                        SetXHashtableUser(dr["userName"].ToString(), dr["userName"].ToString());
                        //y轴信息
                        SetYHashtableUser(dr["userName"].ToString()
                                           , TranslateUtils.ToInt(dr["addCount"].ToString()), YType_New);
                        SetYHashtableUser(dr["userName"].ToString()
                                           , TranslateUtils.ToInt(dr["updateCount"].ToString()), YType_Update);
                        SetYHashtableUser(dr["userName"].ToString()
                                           , TranslateUtils.ToInt(dr["commentCount"].ToString()), YType_Remrk);
                    }
                }
            }

        }

        public void Analysis_OnClick(object sender, EventArgs E)
        {
            string pageUrl = PageUtils.GetCMSUrl(string.Format("background_analysisAdministratorImage.aspx?PublishmentSystemID={0}&StartDate={1}&EndDate={2}", base.PublishmentSystemID, this.StartDate.Text, this.EndDate.Text));
            PageUtils.Redirect(pageUrl);
        }
    }
}
