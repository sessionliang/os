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



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundAnalysisFilesTrian : BackgroundAnalysisFilesBase
    {
        protected override string parentAspxName
        {
            get { return ""; }
        }
        protected override string currentAspxName
        {
            get { return "background_trialAnalysisFiles.aspx"; }
        }

        protected override string tableName
        {
            get { return TrialReportInfo.TableName; }
        }

        protected override ETableStyle tableStyle
        {
            get { return ETableStyle.TrialReportContent; }
        }

        protected override string pageTitle
        {
            get { return "试用报告统计分析"; }
        }

        protected override string leftMenu
        {
            get { return AppManager.CMS.LeftMenu.ID_Function; }
        }

        protected override string leftSubMenu
        {
            get { return AppManager.CMS.LeftMenu.Function.ID_Trial; }
        }

        protected override string permission
        {
            get { return AppManager.CMS.Permission.WebSite.Trial; }
        }

        protected override bool returnBtn
        {
            get { return false; }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_trialAnalysisFiles.aspx?PublishmentSystemID={0}&NodeID={1}", publishmentSystemID, nodeID));
        }

        protected override bool GetCanAnalysisFiles(string attr)
        {
            return TrialReportAttribute.IsAnalysisAttribute(attr);
        }

        protected override string GetAvgCompositeScore()
        {
            return DataProvider.TrialReportDAO.GetSelectCommendOfAnalysisByNode(base.PublishmentSystemID, this.nodeInfo.NodeID, this.StartDate.Text, this.EndDate.Text).ToString();
        }
    }
}
