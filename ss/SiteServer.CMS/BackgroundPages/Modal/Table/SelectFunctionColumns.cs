using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class SelectFunctionColumns : BackgroundBasePage
    {
        public CheckBoxList DisplayAttributeCheckBoxList;

        private int relatedIdentity;
        private int contentID;
        private ArrayList relatedIdentities;
        private ETableStyle tableStyle;
        private bool isList;
        private string tableName;

        /// <summary>
        /// 评价字段
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="contentID"></param>
        /// <param name="isList"></param>
        /// <returns></returns>
        public static string GetOpenWindowStringToEvaluation(int publishmentSystemID, int nodeID, int contentID, bool isList)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedIdentity", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("IsList", isList.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(ETableStyle.EvaluationContent));
            arguments.Add("TableName",  EvaluationContentInfo.TableName);
            return PageUtility.GetOpenWindowString("选择需要显示的项", "modal_selectFunctionColumns.aspx", arguments, 520, 550);
        }

        /// <summary>
        /// 试用申请字段
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="contentID"></param>
        /// <param name="isList"></param>
        /// <returns></returns>
        public static string GetOpenWindowStringToTrialApply(int publishmentSystemID, int nodeID, int contentID, bool isList)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedIdentity", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("IsList", isList.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(ETableStyle.TrialApplyContent));
            arguments.Add("TableName", TrialApplyInfo.TableName);
            return PageUtility.GetOpenWindowString("选择需要显示的项", "modal_selectFunctionColumns.aspx", arguments, 520, 550);
        }

        /// <summary>
        /// 试用报告字段
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="contentID"></param>
        /// <param name="isList"></param>
        /// <returns></returns>
        public static string GetOpenWindowStringToTrialReport(int publishmentSystemID, int nodeID, int contentID, bool isList)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedIdentity", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("IsList", isList.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(ETableStyle.TrialReportContent));
            arguments.Add("TableName", TrialReportInfo.TableName);
            return PageUtility.GetOpenWindowString("选择需要显示的项", "modal_selectFunctionColumns.aspx", arguments, 520, 550);
        } 

        /// <summary>
        /// 调查问卷字段
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="contentID"></param>
        /// <param name="isList"></param>
        /// <returns></returns>
        public static string GetOpenWindowStringToSurvey(int publishmentSystemID, int nodeID, int contentID, bool isList)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedIdentity", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("IsList", isList.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(ETableStyle.SurveyContent));
            arguments.Add("TableName", SurveyQuestionnaireInfo.TableName);
            return PageUtility.GetOpenWindowString("选择需要显示的项", "modal_selectFunctionColumns.aspx", arguments, 520, 550);
        }

        /// <summary>
        /// 比较反馈字段
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="nodeID"></param>
        /// <param name="contentID"></param>
        /// <param name="isList"></param>
        /// <returns></returns>
        public static string GetOpenWindowStringToCompare(int publishmentSystemID, int nodeID, int contentID, bool isList)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("RelatedIdentity", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("IsList", isList.ToString());
            arguments.Add("TableStyle", ETableStyleUtils.GetValue(ETableStyle.CompareContent));
            arguments.Add("TableName", CompareContentInfo.TableName);
            return PageUtility.GetOpenWindowString("选择需要显示的项", "modal_selectFunctionColumns.aspx", arguments, 520, 550);
        }


        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

            this.relatedIdentity = base.GetIntQueryString("RelatedIdentity");
            this.contentID = base.GetIntQueryString("ContentID");
            this.isList = base.GetBoolQueryString("IsList");

            this.relatedIdentities = RelatedIdentities.GetChannelRelatedIdentities(base.PublishmentSystemID, relatedIdentity);

            this.tableStyle = ETableStyleUtils.GetEnumType(base.GetQueryString("TableStyle"));
            this.tableName = base.GetQueryString("TableName");


            if (this.tableStyle == ETableStyle.EvaluationContent || this.tableStyle == ETableStyle.TrialApplyContent || this.tableStyle == ETableStyle.TrialReportContent || this.tableStyle == ETableStyle.SurveyContent || this.tableStyle == ETableStyle.CompareContent)
            { 
                if (!IsPostBack)
                {
                    ArrayList evaluationContentTableStyle = DataProvider.FunctionTableStylesDAO.GetInfoList(base.PublishmentSystemID, this.relatedIdentity, this.contentID, this.tableStyle.ToString(), "files");

                    //栏目设置的字段 
                    ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, this.tableName, this.relatedIdentities);

                    foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                    {
                        if (styleInfo.IsVisible)
                        {
                            ListItem listitem = new ListItem(styleInfo.DisplayName, styleInfo.AttributeName);

                            if (evaluationContentTableStyle.Count > 0)
                            {
                                if (evaluationContentTableStyle.Contains(styleInfo.TableStyleID))
                                {
                                    listitem.Selected = true;
                                }
                            }
                            else
                                listitem.Selected = styleInfo.IsVisible;

                            this.DisplayAttributeCheckBoxList.Items.Add(listitem);
                        }
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            string displayAttributes = ControlUtils.SelectedItemsValueToStringCollection(this.DisplayAttributeCheckBoxList.Items);
            ArrayList selectedValues = ControlUtils.GetSelectedListControlValueArrayList(this.DisplayAttributeCheckBoxList);
            if (this.tableStyle == ETableStyle.EvaluationContent || this.tableStyle == ETableStyle.TrialApplyContent || this.tableStyle == ETableStyle.TrialReportContent || this.tableStyle == ETableStyle.SurveyContent || this.tableStyle == ETableStyle.CompareContent)
            {
                if (selectedValues.Count == 0)
                {
                    this.FailMessage("字段设置失败：请选择至少一个字段");
                    return;
                }
                ArrayList tableStyleInfoArrayList = TableStyleManager.GetTableStyleInfoArrayList(this.tableStyle, this.tableName, this.relatedIdentities);
                ArrayList styleInfoList = new ArrayList();
                foreach (TableStyleInfo styleInfo in tableStyleInfoArrayList)
                {
                    if (selectedValues.Contains(styleInfo.AttributeName))
                    {
                        FunctionTableStyles fs = new FunctionTableStyles();
                        fs.PublishmentSystemID = base.PublishmentSystemID;
                        fs.NodeID = this.relatedIdentity;
                        fs.ContentID = this.contentID;
                        fs.TableStyle = this.tableStyle.ToString();
                        if (styleInfo.TableStyleID == 0)
                        {
                            styleInfo.RelatedIdentity = this.relatedIdentity;
                            int id = TableStyleManager.Insert(styleInfo, tableStyle);
                            fs.TableStyleID = id;
                        }
                        else
                        {
                            fs.TableStyleID = styleInfo.TableStyleID;
                        }
                        fs.UserName = AdminManager.Current.UserName;
                        styleInfoList.Add(fs);
                    }
                }

                DataProvider.FunctionTableStylesDAO.Insert(styleInfoList, true);

                StringUtility.AddLog(base.PublishmentSystemID, "设置" + ETableStyleUtils.GetText(this.tableStyle) + "内容显示项", string.Format("显示项:{0}", TranslateUtils.ObjectCollectionToString(selectedValues)));

            }

            if (!this.isList)
            {
                JsUtils.OpenWindow.CloseModalPageWithoutRefresh(Page);
            }
            else
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }

    }
}
