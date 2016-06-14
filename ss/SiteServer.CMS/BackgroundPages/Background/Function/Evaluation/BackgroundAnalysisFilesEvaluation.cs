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
    public class BackgroundAnalysisFilesEvaluation : BackgroundAnalysisFilesBase
    {
        protected override string parentAspxName
        {
            get { return "background_analysisEvaluation.aspx"; }
        }
        protected override string currentAspxName
        {
            get { return "background_evaluationAnalysisFiles.aspx"; }
        }

        protected override string tableName
        {
            get { return EvaluationContentInfo.TableName; }
        }

        protected override ETableStyle tableStyle
        {
            get { return ETableStyle.EvaluationContent; }
        }

        protected override string pageTitle
        {
            get { return "评价字段统计分析"; }
        }

        protected override string leftMenu
        {
            get { return AppManager.CMS.LeftMenu.ID_Function; }
        }

        protected override string leftSubMenu
        {
            get { return AppManager.CMS.LeftMenu.Function.ID_Evaluation; }
        }

        protected override string permission
        {
            get { return AppManager.CMS.Permission.WebSite.Evaluation; }
        }

        protected override bool returnBtn
        {
            get { return true; }
        }

        protected override bool GetCanAnalysisFiles(string attr)
        {
            return EvaluationContentAttribute.IsAnalysisAttribute(attr);
        }

        protected string GetAvgCompositeScore()
        {
            return DataProvider.EvaluationContentDAO.GetSelectCommendOfAnalysisByNode(base.PublishmentSystemID, this.nodeInfo.NodeID, this.StartDate.Text, this.EndDate.Text).ToString();
        }
    }
}
