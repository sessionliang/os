using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Controls;
using System.Text;
using System.Collections.Specialized;
using System.Data;



namespace SiteServer.CMS.BackgroundPages
{
    public abstract class BackgroundAnalysisFilesBase : BackgroundBasePage
    {
        public Repeater rptContents;
        public SqlPager spContents;
        public Literal ltlColumnHeadRows;
        public Literal ltlColumnHeadTitleRows;
        public Literal ltlTarget;
        public Literal ltlAvgCompositeScore;
        public DateTimeTextBox StartDate;
        public DateTimeTextBox EndDate;
        public Button btnReturn;

        public Alerts alertsID;

        protected NodeInfo nodeInfo;
        protected ArrayList relatedIdentities;
        protected string redirectUrl;
        private DateTime begin;
        private DateTime end;
        private NameValueCollection additional;
        private ArrayList styleInfoArrayList = new ArrayList();
        private int tdms = 0;

        private readonly Hashtable styleItemsHashtable = new Hashtable();
        private readonly Hashtable tableStyleHashtable = new Hashtable();

        //current page aspx name
        protected abstract string parentAspxName { get; }
        protected abstract string currentAspxName { get; }
        protected abstract string tableName { get; }
        protected abstract ETableStyle tableStyle { get; }

        //bread crumb
        protected abstract string pageTitle { get; }
        protected abstract string leftMenu { get; }
        protected abstract string leftSubMenu { get; }
        protected abstract string permission { get; }
        protected abstract bool returnBtn { get; }

        public string GetRedirectUrl(int publishmentSystemID, int nodeID, ETableStyle tableStyle)
        {
            return PageUtils.GetCMSUrl(string.Format("{3}?PublishmentSystemID={0}&NodeID={1}&TableStyle={2}", publishmentSystemID, nodeID, ETableStyleUtils.GetValue(tableStyle), currentAspxName));
        }

        public string GetReturnRedirectUrl(int publishmentSystemID, int nodeID, ETableStyle tableStyle)
        {
            return PageUtils.GetCMSUrl(string.Format("{3}?PublishmentSystemID={0}&NodeID={1}&TableStyle={2}", publishmentSystemID, nodeID, ETableStyleUtils.GetValue(tableStyle), parentAspxName));
        }

        public virtual void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            int nodeID = TranslateUtils.ToInt(base.GetQueryString("NodeID"), base.PublishmentSystemID);
            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.redirectUrl = this.GetRedirectUrl(base.PublishmentSystemID, nodeID, this.tableStyle);

            string contentTableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeInfo.NodeID);


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

            this.ltlTarget.Text = NodeManager.GetNodeNameNavigation(base.PublishmentSystemID, nodeID);
            this.ltlAvgCompositeScore.Text = this.GetAvgCompositeScore();

            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, nodeID);

            this.spContents.ControlToPaginate = this.rptContents;
            this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
            this.spContents.ItemsPerPage = base.PublishmentSystemInfo.Additional.PageSize;
            this.spContents.ConnectionString = BaiRongDataProvider.ConnectionString;
            if (!IsPostBack)
            {
                base.BreadCrumb(leftMenu, leftSubMenu, pageTitle, permission);

                this.StartDate.Text = DateUtils.GetDateAndTimeString(this.begin);
                this.EndDate.Text = DateUtils.GetDateAndTimeString(this.end);

                this.additional = new NameValueCollection();
                additional["StartDate"] = this.StartDate.Text;
                additional["EndDate"] = this.EndDate.Text;

                ArrayList files = new ArrayList();

                if ((ETableStyleUtils.Equals(ETableStyle.EvaluationContent, this.tableStyle) && nodeInfo.Additional.IsUseEvaluation) || (ETableStyleUtils.Equals(ETableStyle.TrialReportContent, this.tableStyle) && nodeInfo.Additional.IsUseTrial) || (ETableStyleUtils.Equals(ETableStyle.SurveyContent, this.tableStyle) && nodeInfo.Additional.IsUseSurvey))
                {
                    ArrayList list = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, tableName, this.relatedIdentities);

                    StringBuilder row = new StringBuilder();
                    row.Append("<tr>");
                    int tdm = 0;
                    foreach (TableStyleInfo styleInfo in list)
                    {

                        if (styleInfo.IsVisible && this.GetCanAnalysisFiles(styleInfo.AttributeName))
                        {
                            ArrayList styleItems = BaiRongDataProvider.TableStyleDAO.GetStyleItemArrayList(styleInfo.TableStyleID);
                            if (styleItems != null && styleItems.Count > 0)
                            {
                                styleInfo.StyleItems = styleItems;
                            }

                            this.styleInfoArrayList.Add(styleInfo);
                            tableStyleHashtable[styleInfo.AttributeName] = styleInfo;
                            styleItemsHashtable[styleInfo.AttributeName] = styleItems;
                            files.Add(" '' AS [" + styleInfo.AttributeName + "]");
                            tdm = (styleInfo.StyleItems == null ? 1 : styleInfo.StyleItems.Count);
                            tdms = tdms + tdm;
                            row.AppendFormat(@"<td colspan='{1}' class='center'>{0}</td>", styleInfo.DisplayName, tdm);
                        }
                    }
                    row.Append("</tr>");
                    this.ltlColumnHeadRows.Text = string.Format(@"<td colspan='{0}'>可统计分析字段</td>", tdms);
                    this.ltlColumnHeadTitleRows.Text = row.ToString();

                    this.spContents.SelectCommand = string.Format("select ID,Title,{0} from {1} where PublishmentSystemID={2} and NodeID={3} ", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(files), contentTableName, base.PublishmentSystemID, this.nodeInfo.NodeID);
                    this.spContents.DataBind();
                }
                else
                {
                    this.alertsID.Text = "当前栏目未启用" + AppManager.CMS.LeftMenu.GetSubText(this.leftSubMenu) + "功能，" + ETableStyleUtils.GetText(this.tableStyle) + "字段统计分析不可用！";
                }

                this.btnReturn.Visible = returnBtn;
            }
        }

        #region base

        public virtual void Redirect(object sender, EventArgs e)
        {
            PageUtils.Redirect(this.GetRedirectUrl(base.PublishmentSystemID, this.nodeInfo.NodeID, this.tableStyle));
        }
        public virtual void Return_OnClick(object sender, EventArgs e)
        {
            PageUtils.Redirect(this.GetReturnRedirectUrl(base.PublishmentSystemID, this.nodeInfo.NodeID, this.tableStyle));
        }

        protected virtual void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int contentID = TranslateUtils.EvalInt(e.Item.DataItem, "ID");
                string title = TranslateUtils.EvalString(e.Item.DataItem, "Title");

                Literal ltlContentTitle = e.Item.FindControl("ltlContentTitle") as Literal;
                Literal ltlColumnItemRows = e.Item.FindControl("ltlColumnItemRows") as Literal;

                ltlColumnItemRows.Text = GetAnalysis(contentID);

                string openWindow = Modal.FunctionStyleAnalysisShow.GetOpenWindowString(base.PublishmentSystemID, this.nodeInfo.NodeID, contentID, this.tableStyle, this.tableName, this.begin.ToString("yyyy-MM-dd"), this.end.ToString("yyyy-MM-dd"));

                ltlContentTitle.Text = string.Format("<a href=\"javascript:;\" onclick=\"{1}\">{0}</a>", title, openWindow);

            }
        }



        public void Analysis_OnClick(object sender, EventArgs E)
        {
            string pageUrl = PageUtils.GetCMSUrl(string.Format("{3}?PublishmentSystemID={0}&StartDate={1}&EndDate={2}", base.PublishmentSystemID, this.StartDate.Text, this.EndDate.Text, currentAspxName));
            PageUtils.Redirect(pageUrl);
        }


        public string GetDateStr()
        {
            string dateFrom = this.begin.ToString("yyyy-MM-dd");
            string dateTo = this.end.ToString("yyyy-MM-dd");

            string dateString = string.Empty;
            if (!string.IsNullOrEmpty(dateFrom))
            {
                dateString = string.Format(" AND AddDate >= '{0}' ", dateFrom);

            }
            if (!string.IsNullOrEmpty(dateTo))
            {
                dateTo = DateUtils.GetDateString(TranslateUtils.ToDateTime(dateTo).AddDays(1));
                dateString += string.Format(" AND AddDate <= '{0}' ", dateTo);
            }

            return dateString;
        }

        #endregion

        #region virtual

        protected virtual string GetAvgCompositeScore()
        {
            return " - ";
        }

        protected virtual bool GetCanAnalysisFiles(string attr)
        {
            if (this.tableStyle == ETableStyle.EvaluationContent)
                return EvaluationContentAttribute.IsAnalysisAttribute(attr);
            else if (this.tableStyle == ETableStyle.TrialReportContent)
                return TrialReportAttribute.IsAnalysisAttribute(attr);
            else if (this.tableStyle == ETableStyle.EvaluationContent)
                return SurveyQuestionnaireAttribute.IsAnalysisAttribute(attr);
            else
                return false;
        }


        protected virtual string GetAnalysis(int contentID)
        {
            ArrayList listHas = DataProvider.FunctionTableStylesDAO.GetInfoList(base.PublishmentSystemID, this.nodeInfo.NodeID, contentID, this.tableStyle.ToString(), "files");

            string tds = string.Empty;
            ArrayList sqlItems = new ArrayList();
            foreach (TableStyleInfo info in this.styleInfoArrayList)
            {
                if (listHas.Count > 0)
                {
                    if (listHas.Contains(info.TableStyleID))
                    {
                        if (info.InputType == EInputType.Text)
                        {
                            sqlItems.Add(string.Format("(SUM({0})/ COUNT(1))", info.AttributeName));
                        }
                        if (info.InputType == EInputType.SelectOne || info.InputType == EInputType.Radio || info.InputType == EInputType.CheckBox)
                        {
                            if (info.StyleItems != null)
                                foreach (TableStyleItemInfo item in info.StyleItems)
                                {
                                    sqlItems.Add(string.Format("sum(case when {0}='{1}' then 1 else 0 end) as [{1}]", info.AttributeName, item.ItemValue));
                                }
                            else
                                sqlItems.Add(string.Format(" '-' as {0} ", info.AttributeName));
                        }
                    }
                }
                else
                {
                    if (info.InputType == EInputType.Text)
                    {
                        sqlItems.Add(string.Format("(SUM({0})/ COUNT(1))", info.AttributeName));
                    }
                    if (info.InputType == EInputType.SelectOne || info.InputType == EInputType.Radio || info.InputType == EInputType.CheckBox)
                    {
                        if (info.StyleItems != null)
                            foreach (TableStyleItemInfo item in info.StyleItems)
                            {
                                sqlItems.Add(string.Format("sum(case when {0}='{1}' then 1 else 0 end) as [{1}]", info.AttributeName, item.ItemValue));
                            }
                        else
                            sqlItems.Add(string.Format(" '-' as {0} ", info.AttributeName));
                    }
                } 
            }

            string sql = string.Format("select {1} from {0} where PublishmentSystemID={2} and NodeID={3} and ContentID={4} {5} ", this.tableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(sqlItems), base.PublishmentSystemID, this.nodeInfo.NodeID, contentID, GetDateStr());


            DataTable dt = DataProvider.FunctionTableStylesDAO.getContentAnalysis(sql);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                for (int i = 0; i < tdms; i++)
                {
                    tds += string.Format("<td>{0}</td>", row[i]);
                }
            }
            else
            {
                for (int i = 0; i < tdms; i++)
                {
                    tds += string.Format("<td>0</td>");
                }
            }
            return tds;
        }



        #endregion

    }
}
