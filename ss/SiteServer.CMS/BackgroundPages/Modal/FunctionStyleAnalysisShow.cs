using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using System.Data;


namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class FunctionStyleAnalysisShow : BackgroundBasePage
    {
        public Repeater MyRepeater;

        private int relatedIdentity;
        private int contentID;
        private int id;
        private ArrayList relatedIdentities;
        private ETableStyle tableStyle;
        private string tableName;
        private ArrayList myStyleInfoArrayList = new ArrayList();
        private string dateFrom;
        private string dateTo;
        private DataTable dt;

        /// <summary>
        ///  字段统计
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="contentID"></param>
        /// <param name="style"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, int contentID, ETableStyle style, string tableName, string dateFrom, string dateEnd)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedIdentity", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(style));
            arguments.Add("TableName", tableName);
            arguments.Add("DateFrom", dateFrom);
            arguments.Add("DateTo", dateEnd);
            return PageUtility.GetOpenWindowString("查看" + ETableStyleUtils.GetText(style) + "字段统计详情", "modal_functionTableStaleAnalysisShow.aspx", arguments, 520, 550);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;


            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.relatedIdentity = base.GetIntQueryString("RelatedIdentity");
            this.contentID = base.GetIntQueryString("ContentID");

            this.dateFrom = base.GetQueryString("DateFrom");
            this.dateTo = base.GetQueryString("DateTo");

            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, relatedIdentity);

            this.tableStyle = ETableStyleUtils.GetEnumType(base.GetQueryString("TableStyle"));
            this.tableName = base.GetQueryString("TableName");

            string adminName = AdminManager.Current.UserName;

            if (!IsPostBack)
            {
                ArrayList trianApplyTableStyle = DataProvider.FunctionTableStylesDAO.GetInfoList(base.PublishmentSystemID, this.relatedIdentity, this.contentID, this.tableStyle.ToString(), "files");

                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, this.tableName, this.relatedIdentities);
                if (styleInfoArrayList != null)
                {
                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        if (trianApplyTableStyle.Count > 0)
                        {
                            if (trianApplyTableStyle.Contains(styleInfo.TableStyleID) && GetCanAnalysisFiles(styleInfo.AttributeName))
                            {
                                if ((styleInfo.InputType == EInputType.SelectOne || styleInfo.InputType == EInputType.Radio || styleInfo.InputType == EInputType.CheckBox) && styleInfo.StyleItems == null)
                                {
                                    ArrayList styleItems = BaiRongDataProvider.TableStyleDAO.GetStyleItemArrayList(styleInfo.TableStyleID);
                                    if (styleItems != null && styleItems.Count > 0)
                                    {
                                        styleInfo.StyleItems = styleItems;
                                    }
                                }
                                myStyleInfoArrayList.Add(styleInfo);
                            }
                        }
                        else
                            if (styleInfo.IsVisible && GetCanAnalysisFiles(styleInfo.AttributeName))
                            {
                                if ((styleInfo.InputType == EInputType.SelectOne || styleInfo.InputType == EInputType.Radio || styleInfo.InputType == EInputType.CheckBox) && styleInfo.StyleItems == null)
                                {
                                    ArrayList styleItems = BaiRongDataProvider.TableStyleDAO.GetStyleItemArrayList(styleInfo.TableStyleID);
                                    if (styleItems != null && styleItems.Count > 0)
                                    {
                                        styleInfo.StyleItems = styleItems;
                                    }
                                }
                                myStyleInfoArrayList.Add(styleInfo);
                            }
                    }
                }

                this.dt = GetAnalysis(this.contentID);

                this.MyRepeater.DataSource = myStyleInfoArrayList;
                this.MyRepeater.ItemDataBound += new RepeaterItemEventHandler(MyRepeater_ItemDataBound);
                this.MyRepeater.DataBind();
            }
        }

        void MyRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TableStyleInfo styleInfo = (TableStyleInfo)e.Item.DataItem;

                string helpHtml = StringUtility.GetHelpHtml(styleInfo.DisplayName, styleInfo.HelpText);

                Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

                string inputHtml = string.Empty;

                if (this.dt.Rows.Count > 0)
                {

                    if (styleInfo.InputType == EInputType.Text)
                    {
                        inputHtml = this.dt.Rows[0][styleInfo.AttributeName].ToString();
                        ltlHtml.Text = string.Format(@"
<tr>
  <td colspan=""2"">{0}</td>
  <td >{1}</td>
</tr>
", helpHtml, inputHtml);
                    }
                    else if (styleInfo.InputType == EInputType.SelectOne || styleInfo.InputType == EInputType.Radio || styleInfo.InputType == EInputType.CheckBox)
                    {
                        int row = styleInfo.StyleItems == null ? 1 : styleInfo.StyleItems.Count + 1;
                        ltlHtml.Text = string.Format(@"
<tr>
  <td rowspan=""{1}"">{0}</td> 
</tr>
", helpHtml, row);
                        if (styleInfo.StyleItems != null)
                        {
                            foreach (TableStyleItemInfo item in styleInfo.StyleItems)
                            {
                                ltlHtml.Text += string.Format(@"
<tr>
  <td >{0}</td> 
  <td >{1}</td> 
</tr>
", item.ItemTitle, this.dt.Rows[0][item.ItemValue].ToString());
                            }
                        }
                    }
                }
                else
                {
                    ltlHtml.Text = string.Format(@"
<tr>
  <td>{0}</td>
  <td colspan=""2"">{1}</td>
</tr>
", helpHtml, "-");
                }
            }
        }

        protected bool GetCanAnalysisFiles(string attr)
        {
            if (this.tableStyle == ETableStyle.EvaluationContent)
                return EvaluationContentAttribute.IsAnalysisAttribute(attr);
            else if (this.tableStyle == ETableStyle.TrialReportContent)
                return TrialReportAttribute.IsAnalysisAttribute(attr);
            else if (this.tableStyle == ETableStyle.SurveyContent)
                return SurveyQuestionnaireAttribute.IsAnalysisAttribute(attr);
            else
                return false;
        }

        protected DataTable GetAnalysis(int contentID)
        {
            ArrayList listHas = DataProvider.FunctionTableStylesDAO.GetInfoList(base.PublishmentSystemID, this.relatedIdentity, contentID, this.tableStyle.ToString(), "files");

            string tds = string.Empty;
            ArrayList sqlItems = new ArrayList();
            foreach (TableStyleInfo info in this.myStyleInfoArrayList)
            {
                if (listHas.Count > 0 && listHas.Contains(info.TableStyleID))
                {
                    if (info.InputType == EInputType.Text)
                    {
                        sqlItems.Add(string.Format("(SUM({0})/ COUNT(1)) as [{0}]", info.AttributeName));
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
                else
                    sqlItems.Add(string.Format(" '-' as {0} ", info.AttributeName));
            }

            string sql = string.Format("select {1} from {0} where PublishmentSystemID={2} and NodeID={3} and ContentID={4} {5} ", this.tableName, TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(sqlItems), base.PublishmentSystemID, this.relatedIdentity, contentID, GetDateStr());


            return DataProvider.FunctionTableStylesDAO.getContentAnalysis(sql);
        }



        public string GetDateStr()
        {
            string dateString = string.Empty;
            if (!string.IsNullOrEmpty(this.dateFrom))
            {
                dateString = string.Format(" AND AddDate >= '{0}' ", this.dateFrom);

            }
            if (!string.IsNullOrEmpty(this.dateTo))
            {
                this.dateTo = DateUtils.GetDateString(TranslateUtils.ToDateTime(this.dateTo).AddDays(1));
                dateString += string.Format(" AND AddDate <= '{0}' ", this.dateTo);
            }

            return dateString;
        }


    }
}
 