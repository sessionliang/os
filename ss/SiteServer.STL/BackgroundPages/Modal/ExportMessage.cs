using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Office;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using SiteServer.STL.ImportExport;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages.Modal
{
    public class ExportMessage : BackgroundBasePage
    {
        private string exportType;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.exportType = base.GetQueryString("ExportType");

            if (!IsPostBack)
            {
                bool isExport = true;
                string fileName = string.Empty;
                try
                {
                    if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_TrackerHour)
                    {
                        fileName = ExportTrackingHours();
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_TrackerDay)
                    {
                        fileName = ExportTrackingDays();
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_TrackerMonth)
                    {
                        fileName = ExportTrackingMonths();
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_TrackerYear)
                    {
                        fileName = ExportTrackingYears();
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_TrackerContent)
                    {
                        string startDateString = base.GetQueryString("StartDateString");
                        string endDateString = base.GetQueryString("EndDateString");
                        int nodeID = base.GetIntQueryString("NodeID");
                        int contentID = base.GetIntQueryString("ContentID");
                        int totalNum = base.GetIntQueryString("TotalNum");
                        bool isDelete = TranslateUtils.ToBool(base.GetQueryString("IsDelete"), false);
                        fileName = ExportTrackingContents(startDateString, endDateString, nodeID, contentID, totalNum, isDelete);
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_InputContent)
                    {
                        int inputID = base.GetIntQueryString("InputID");
                        fileName = ExportInputContent(inputID);
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_WebsiteMessageContent)
                    {
                        int websiteMessageID = base.GetIntQueryString("WebsiteMessageID");
                        int classifyID = base.GetIntQueryString("ClassifyID");
                        fileName = ExportWebsiteMessageContent(websiteMessageID, classifyID);
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_MailSubscribe)
                    {
                        fileName = ExportMailSubscribe();
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_Comment)
                    {
                        int nodeID = base.GetIntQueryString("NodeID");
                        int contentID = base.GetIntQueryString("ContentID");
                        fileName = ExportComment(nodeID, contentID);
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_GatherRule)
                    {
                        ArrayList gatherRuleNameArrayList = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("GatherRuleNameCollection"));
                        fileName = ExportGatherRule(gatherRuleNameArrayList);
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_Input)
                    {
                        int inputID = TranslateUtils.ToInt(base.GetQueryString("InputID"));
                        fileName = ExportInput(inputID);
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_RelatedField)
                    {
                        int relatedFieldID = TranslateUtils.ToInt(base.GetQueryString("RelatedFieldID"));
                        fileName = ExportRelatedField(relatedFieldID);
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_TagStyle)
                    {
                        int styleID = TranslateUtils.ToInt(base.GetQueryString("StyleID"));
                        fileName = ExportTagStyle(styleID);
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_ContentZip)
                    {
                        int nodeID = base.GetIntQueryString("NodeID");
                        ArrayList contentIDCollection = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ContentIDCollection"));
                        bool isPeriods = TranslateUtils.ToBool(base.GetQueryString("isPeriods"));
                        string startDate = base.GetQueryString("startDate");
                        string endDate = base.GetQueryString("endDate");
                        ETriState checkedState = ETriStateUtils.GetEnumType(base.GetQueryString("checkedState"));
                        isExport = ExportContentZip(nodeID, contentIDCollection, isPeriods, startDate, endDate, checkedState, out fileName);
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_ContentAccess)
                    {
                        int nodeID = base.GetIntQueryString("NodeID");
                        ArrayList contentIDCollection = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ContentIDCollection"));
                        ArrayList displayAttributes = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("DisplayAttributes"));
                        bool isPeriods = TranslateUtils.ToBool(base.GetQueryString("isPeriods"));
                        string startDate = base.GetQueryString("startDate");
                        string endDate = base.GetQueryString("endDate");
                        ETriState checkedState = ETriStateUtils.GetEnumType(base.GetQueryString("checkedState"));
                        isExport = ExportContentAccess(nodeID, contentIDCollection, displayAttributes, isPeriods, startDate, endDate, checkedState, out fileName);
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_ContentExcel)
                    {
                        int nodeID = base.GetIntQueryString("NodeID");
                        ArrayList contentIDCollection = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ContentIDCollection"));
                        ArrayList displayAttributes = TranslateUtils.StringCollectionToArrayList(base.GetQueryString("DisplayAttributes"));
                        bool isPeriods = TranslateUtils.ToBool(base.GetQueryString("isPeriods"));
                        string startDate = base.GetQueryString("startDate");
                        string endDate = base.GetQueryString("endDate");
                        ETriState checkedState = ETriStateUtils.GetEnumType(base.GetQueryString("checkedState"));
                        isExport = ExportContentExcel(nodeID, contentIDCollection, displayAttributes, isPeriods, startDate, endDate, checkedState, out fileName);
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_Channel)
                    {
                        ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("ChannelIDCollection"));
                        fileName = ExportChannel(nodeIDArrayList);
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_SingleTableStyle)
                    {
                        ETableStyle tableStyle = ETableStyleUtils.GetEnumType(base.GetQueryString("TableStyle"));
                        string tableName = base.GetQueryString("TableName");
                        int relatedIdentity = base.GetIntQueryString("RelatedIdentity");
                        fileName = ExportSingleTableStyle(tableStyle, tableName, relatedIdentity);
                    }
                    else if (this.exportType == PageUtility.ModalSTL.ExportMessage.EXPORT_TYPE_Survey)
                    { 
                        int nodeID = base.GetIntQueryString("NodeID");
                        int contentID = base.GetIntQueryString("ContentID");
                        fileName = ExportSurveyTableStyle(  nodeID, contentID);
                    }

                    if (isExport)
                    {
                        HyperLink link = new HyperLink();
                        string filePath = PathUtils.GetTemporaryFilesPath(fileName);
                        link.NavigateUrl = PageUtility.ServiceSTL.Utils.GetDownloadUrlByFilePath(filePath);
                        link.Text = "下载";
                        string successMessage = "成功导出文件！&nbsp;&nbsp;" + ControlUtils.GetControlRenderHtml(link);
                        base.SuccessMessage(successMessage);
                    }
                    else
                    {
                        base.FailMessage("导出失败，所选条件没有匹配内容，请重新选择条件导出内容");
                    }
                }
                catch (Exception ex)
                {
                    string failedMessage = "文件导出失败！<br/><br/>原因为：" + ex.Message;
                    base.FailMessage(ex, failedMessage);
                }
            }
        }

        private string ExportTrackingHours()
        {
            string docFileName = "24小时统计.xls";
            string filePath = PathUtils.GetTemporaryFilesPath(docFileName);
            ExcelObject.CreateExcelFileForTrackingHours(filePath, base.PublishmentSystemID);

            return docFileName;
        }

        private string ExportTrackingDays()
        {
            string docFileName = "天统计.xls";
            string filePath = PathUtils.GetTemporaryFilesPath(docFileName);
            ExcelObject.CreateExcelFileForTrackingDays(filePath, base.PublishmentSystemID);

            return docFileName;
        }

        private string ExportTrackingMonths()
        {
            string docFileName = "月统计.xls";
            string filePath = PathUtils.GetTemporaryFilesPath(docFileName);
            ExcelObject.CreateExcelFileForTrackingMonths(filePath, base.PublishmentSystemID);

            return docFileName;
        }

        private string ExportTrackingYears()
        {
            string docFileName = "年统计.xls";
            string filePath = PathUtils.GetTemporaryFilesPath(docFileName);
            ExcelObject.CreateExcelFileForTrackingYears(filePath, base.PublishmentSystemID);

            return docFileName;
        }

        private string ExportTrackingContents(string startDateString, string endDateString, int nodeID, int contentID, int totalNum, bool isDelete)
        {
            string docFileName = "内容统计.xls";
            string filePath = PathUtils.GetTemporaryFilesPath(docFileName);
            ExcelObject.CreateExcelFileForTrackingContents(filePath, startDateString, endDateString, base.PublishmentSystemInfo, nodeID, contentID, totalNum, isDelete);

            return docFileName;
        }

        private string ExportInputContent(int inputID)
        {
            InputInfo inputInfo = DataProvider.InputDAO.GetInputInfo(inputID);

            string docFileName = string.Format("{0}.xls", inputInfo.InputName);
            string filePath = PathUtils.GetTemporaryFilesPath(docFileName);
            ExcelObject.CreateExcelFileForInputContents(filePath, base.PublishmentSystemID, inputInfo);

            return docFileName;
        }

        private string ExportWebsiteMessageContent(int websiteMessageID, int classifyID)
        {
            WebsiteMessageInfo websiteMessageInfo = DataProvider.WebsiteMessageDAO.GetWebsiteMessageInfo(websiteMessageID);

            string docFileName = string.Format("{0}.xls", websiteMessageInfo.WebsiteMessageName);
            string filePath = PathUtils.GetTemporaryFilesPath(docFileName);
            ExcelObject.CreateExcelFileForWebsiteMessageContents(filePath, base.PublishmentSystemID, websiteMessageInfo, classifyID);

            return docFileName;
        }

        private string ExportMailSubscribe()
        {
            string docFileName = "邮件订阅.xls";
            string filePath = PathUtils.GetTemporaryFilesPath(docFileName);
            ExcelObject.CreateExcelFileForMailSubscribe(filePath, base.PublishmentSystemID);

            return docFileName;
        }

        private string ExportComment(int nodeID, int contentID)
        {
            string docFileName = "评论.xls";
            string filePath = PathUtils.GetTemporaryFilesPath(docFileName);
            ExcelObject.CreateExcelFileForComments(filePath, base.PublishmentSystemInfo, nodeID, contentID);

            return docFileName;
        }

        private string ExportGatherRule(ArrayList gatherRuleNameArrayList)
        {
            string docFileName = "GatherRule.xml";
            string filePath = PathUtils.GetTemporaryFilesPath(docFileName);

            ExportObject exportObject = new ExportObject(base.PublishmentSystemID);
            ArrayList gatherRuleInfoArrayList = new ArrayList();
            foreach (string gatherRuleName in gatherRuleNameArrayList)
            {
                gatherRuleInfoArrayList.Add(DataProvider.GatherRuleDAO.GetGatherRuleInfo(gatherRuleName, base.PublishmentSystemID));
            }

            exportObject.ExportGatherRule(filePath, gatherRuleInfoArrayList);

            return docFileName;
        }

        private string ExportInput(int inputID)
        {
            ExportObject exportObject = new ExportObject(base.PublishmentSystemID);
            return exportObject.ExportInput(inputID);
        }

        private string ExportRelatedField(int relatedFieldID)
        {
            ExportObject exportObject = new ExportObject(base.PublishmentSystemID);
            return exportObject.ExportRelatedField(relatedFieldID);
        }

        private string ExportTagStyle(int styleID)
        {
            TagStyleInfo styleInfo = DataProvider.TagStyleDAO.GetTagStyleInfo(styleID);

            ExportObject exportObject = new ExportObject(base.PublishmentSystemID);
            return exportObject.ExportTagStyle(styleInfo);
        }

        private bool ExportContentZip(int nodeID, ArrayList contentIDArrayList, bool isPeriods, string dateFrom, string dateTo, ETriState checkedState, out string fileName)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            fileName = string.Format("{0}.zip", nodeInfo.NodeName);
            string filePath = PathUtils.GetTemporaryFilesPath(fileName);
            ExportObject exportObject = new ExportObject(base.PublishmentSystemID);
            return exportObject.ExportContents(filePath, nodeID, contentIDArrayList, isPeriods, dateFrom, dateTo, checkedState);
        }

        private bool ExportContentAccess(int nodeID, ArrayList contentIDArrayList, ArrayList displayAttributes, bool isPeriods, string dateFrom, string dateTo, ETriState checkedState, out string fileName)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            fileName = string.Format("{0}.mdb", nodeInfo.NodeName);
            string filePath = PathUtils.GetTemporaryFilesPath(fileName);
            return AccessObject.CreateAccessFileForContents(filePath, base.PublishmentSystemInfo, nodeInfo, contentIDArrayList, displayAttributes, isPeriods, dateFrom, dateTo, checkedState);
        }

        private bool ExportContentExcel(int nodeID, ArrayList contentIDArrayList, ArrayList displayAttributes, bool isPeriods, string dateFrom, string dateTo, ETriState checkedState, out string fileName)
        {
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);

            fileName = string.Format("{0}.xls", nodeInfo.NodeName);
            string filePath = PathUtils.GetTemporaryFilesPath(fileName);
            return ExcelObject.CreateExcelFileForContents(filePath, base.PublishmentSystemInfo, nodeInfo, contentIDArrayList, displayAttributes, isPeriods, dateFrom, dateTo, checkedState);
        }

        private string ExportChannel(ArrayList nodeIDArrayList)
        {
            ExportObject exportObject = new ExportObject(base.PublishmentSystemID);
            return exportObject.ExportChannels(nodeIDArrayList);
        }

        private string ExportSingleTableStyle(ETableStyle tableStyle, string tableName, int relatedIdentity)
        {
            ExportObject exportObject = new ExportObject(base.PublishmentSystemID);
            return exportObject.ExportSingleTableStyle(tableStyle, tableName, relatedIdentity);
        }

        private string ExportSurveyTableStyle(   int nodeID, int contentID)
        {
            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeID);
            string tableName = base.PublishmentSystemInfo.AuxiliaryTableForContent;
            ContentInfo info = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

            string docFileName = string.Format("{0}的调查问卷明细.xls", info.Title);
            string filePath = PathUtils.GetTemporaryFilesPath(docFileName);
            ExcelObject.CreateExcelFileForSurveyContents(filePath, base.PublishmentSystemID, nodeID, contentID);

            return docFileName;
        }
    }
}
