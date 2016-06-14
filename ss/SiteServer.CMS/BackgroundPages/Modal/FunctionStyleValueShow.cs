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


namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class FunctionStyleValueShow : BackgroundBasePage
    {
        public Repeater MyRepeater;
        public Literal ltlUserName;

        private int relatedIdentity;
        private int contentID;
        private int id;
        private ArrayList relatedIdentities;
        private ETableStyle tableStyle;
        private string tableName;
        private EvaluationContentInfo einfo;
        private TrialApplyInfo ainfo;
        private TrialReportInfo rinfo;
        private SurveyQuestionnaireInfo sqinfo;
        private CompareContentInfo ccinfo;

        /// <summary>
        /// 评价内容
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="contentID"></param> 
        /// <returns></returns>
        public static string GetOpenWindowStringToEvaluation(int publishmentSystemID, int nodeID, int contentID, int eid)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedIdentity", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("ID", eid.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(ETableStyle.EvaluationContent));
            arguments.Add("TableName", EvaluationContentInfo.TableName);
            return PageUtility.GetOpenWindowString("查看评价内容", "modal_functionTableStaleShow.aspx", arguments, 520, 550);
        }

        /// <summary>
        /// 试用申请
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="contentID"></param> 
        /// <returns></returns>
        public static string GetOpenWindowStringToTrialApply(int publishmentSystemID, int nodeID, int contentID, int taid)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedIdentity", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("ID", taid.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(ETableStyle.TrialApplyContent));
            arguments.Add("TableName", TrialApplyInfo.TableName);
            return PageUtility.GetOpenWindowString("查看试用申请内容", "modal_functionTableStaleShow.aspx", arguments, 520, 550);
        }

        /// <summary>
        /// 试用报告
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="contentID"></param> 
        /// <returns></returns>
        public static string GetOpenWindowStringToTrialReport(int publishmentSystemID, int nodeID, int contentID, int taid)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedIdentity", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("ID", taid.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(ETableStyle.TrialReportContent));
            arguments.Add("TableName", TrialReportInfo.TableName);
            return PageUtility.GetOpenWindowString("查看试用报告", "modal_functionTableStaleShow.aspx", arguments, 520, 550);
        }


        /// <summary>
        /// 调查问卷
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="contentID"></param> 
        /// <returns></returns>
        public static string GetOpenWindowStringToSurvey(int publishmentSystemID, int nodeID, int contentID, int sqid)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedIdentity", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("ID", sqid.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(ETableStyle.SurveyContent));
            arguments.Add("TableName", SurveyQuestionnaireInfo.TableName);
            return PageUtility.GetOpenWindowString("查看调查问卷", "modal_functionTableStaleShow.aspx", arguments, 520, 550);
        }

        /// <summary>
        /// 比较反馈
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="contentID"></param> 
        /// <returns></returns>
        public static string GetOpenWindowStringToCompare(int publishmentSystemID, int nodeID, int contentID, int ccid)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedIdentity", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("ID", ccid.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(ETableStyle.CompareContent));
            arguments.Add("TableName", CompareContentInfo.TableName);
            return PageUtility.GetOpenWindowString("查看比较反馈内容", "modal_functionTableStaleShow.aspx", arguments, 520, 550);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;


            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.relatedIdentity = base.GetIntQueryString("RelatedIdentity");
            this.contentID = base.GetIntQueryString("ContentID");
            this.id = base.GetIntQueryString("ID");

            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, relatedIdentity);

            this.tableStyle = ETableStyleUtils.GetEnumType(base.GetQueryString("TableStyle"));
            this.tableName = base.GetQueryString("TableName");

            string adminName = AdminManager.Current.UserName;

            if (this.tableStyle == ETableStyle.TrialApplyContent)
            {
                this.ainfo = DataProvider.TrialApplyDAO.GetInfo(this.id);

                this.ltlUserName.Text = ainfo.UserName;
                if (!this.ainfo.IsChecked)
                {
                    this.ainfo.ApplyStatus = EFunctionStatusType.AlreadyView.ToString();
                    DataProvider.TrialApplyDAO.Update(ainfo);
                }
            }
            else if (this.tableStyle == ETableStyle.EvaluationContent)
            {
                this.einfo = DataProvider.EvaluationContentDAO.GetInfo(base.PublishmentSystemID, this.relatedIdentity, this.contentID, this.id);

                this.ltlUserName.Text = einfo.UserName;
            }
            else if (this.tableStyle == ETableStyle.TrialReportContent)
            {
                this.rinfo = DataProvider.TrialReportDAO.GetInfo(base.PublishmentSystemID, this.relatedIdentity, this.contentID, this.id);

                this.ltlUserName.Text = rinfo.UserName;

                if (!rinfo.ReportStatus)
                    DataProvider.TrialReportDAO.UpdateStatus(base.PublishmentSystemID, this.relatedIdentity, this.contentID, this.id, adminName);
            }
            else if (this.tableStyle == ETableStyle.SurveyContent)
            {
                this.sqinfo = DataProvider.SurveyQuestionnaireDAO.GetInfo(base.PublishmentSystemID, this.relatedIdentity, this.contentID, this.id);

                this.ltlUserName.Text = sqinfo.UserName;

                if (!sqinfo.SurveyStatus)
                    DataProvider.SurveyQuestionnaireDAO.UpdateStatus(base.PublishmentSystemID, this.relatedIdentity, this.contentID, this.id, adminName);
            }
            else if (this.tableStyle == ETableStyle.CompareContent)
            {
                this.ccinfo = DataProvider.CompareContentDAO.GetInfo(base.PublishmentSystemID, this.relatedIdentity, this.contentID, this.id);

                this.ltlUserName.Text = ccinfo.UserName;

                if (!ccinfo.CompareStatus)
                    DataProvider.CompareContentDAO.UpdateStatus(base.PublishmentSystemID, this.relatedIdentity, this.contentID, this.id, adminName);
            }
            if (!IsPostBack)
            {
                ArrayList trianApplyTableStyle = DataProvider.FunctionTableStylesDAO.GetInfoList(base.PublishmentSystemID, this.relatedIdentity, this.contentID, this.tableStyle.ToString(), "files");

                ArrayList styleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, this.tableName, this.relatedIdentities);
                ArrayList myStyleInfoArrayList = new ArrayList();
                if (styleInfoArrayList != null)
                {
                    foreach (TableStyleInfo styleInfo in styleInfoArrayList)
                    {
                        if (trianApplyTableStyle.Count > 0)
                        {
                            if (trianApplyTableStyle.Contains(styleInfo.TableStyleID))
                            {
                                myStyleInfoArrayList.Add(styleInfo);
                            }
                        }
                        else
                            if (styleInfo.IsVisible)
                            {
                                myStyleInfoArrayList.Add(styleInfo);
                            }
                    }
                }
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

                string helpHtml = string.Empty;
                if (styleInfo.DisplayName.Length > 10)
                    helpHtml = this.GetHelpHtml(styleInfo.DisplayName, styleInfo.HelpText);
                else
                    helpHtml = StringUtility.GetHelpHtml(styleInfo.DisplayName, styleInfo.HelpText);

                string inputHtml = string.Empty;
                if (this.tableStyle == ETableStyle.TrialApplyContent)
                {
                    inputHtml = InputTypeParser.GetContentByTableStyle(this.ainfo.GetExtendedAttribute(styleInfo.AttributeName), base.PublishmentSystemInfo, this.tableStyle, styleInfo);
                }
                else if (this.tableStyle == ETableStyle.EvaluationContent)
                {
                    inputHtml = InputTypeParser.GetContentByTableStyle(this.einfo.GetExtendedAttribute(styleInfo.AttributeName), base.PublishmentSystemInfo, this.tableStyle, styleInfo);
                }
                else if (this.tableStyle == ETableStyle.TrialReportContent)
                {
                    inputHtml = InputTypeParser.GetContentByTableStyle(this.rinfo.GetExtendedAttribute(styleInfo.AttributeName), base.PublishmentSystemInfo, this.tableStyle, styleInfo);
                }
                else if (this.tableStyle == ETableStyle.SurveyContent)
                {
                    inputHtml = InputTypeParser.GetContentByTableStyle(this.sqinfo.GetExtendedAttribute(styleInfo.AttributeName), base.PublishmentSystemInfo, this.tableStyle, styleInfo);
                }
                else if (this.tableStyle == ETableStyle.CompareContent)
                {
                    inputHtml = InputTypeParser.GetContentByTableStyle(this.ccinfo.GetExtendedAttribute(styleInfo.AttributeName), base.PublishmentSystemInfo, this.tableStyle, styleInfo);
                }


                Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

                if (styleInfo.DisplayName.Length > 10)

                    ltlHtml.Text = string.Format(@"
<tr> 
  <td colspan=""3"">{0}</td>
</tr><tr> 
  <td colspan=""3"">{1}</td>
</tr>
", helpHtml, inputHtml);
                else
                    ltlHtml.Text = string.Format(@"
<tr>
  <td>{0}</td>
  <td colspan=""2"">{1}</td>
</tr>
", helpHtml, inputHtml);
            }

        }

        public string GetHelpHtml(string text, string helpText)
        {
            if (string.IsNullOrEmpty(helpText)) helpText = text;
            string html = string.Format(@"{0}", text);
            return html;
        }
    }
}
