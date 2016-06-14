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
    public class BackgroundAnalysisFilesSurvey : BackgroundAnalysisFilesBase
    {
        protected override string parentAspxName
        {
            get { return ""; }
        }
        protected override string currentAspxName
        {
            get { return "background_surveyAnalysisFiles.aspx"; }
        }

        protected override string tableName
        {
            get { return SurveyQuestionnaireInfo.TableName; }
        }

        protected override ETableStyle tableStyle
        {
            get { return ETableStyle.SurveyContent; }
        }

        protected override string pageTitle
        {
            get { return "调查问卷统计分析"; }
        }

        protected override string leftMenu
        {
            get { return AppManager.CMS.LeftMenu.ID_Function; }
        }

        protected override string leftSubMenu
        {
            get { return AppManager.CMS.LeftMenu.Function.ID_Survey; }
        }

        protected override string permission
        {
            get { return AppManager.CMS.Permission.WebSite.Survey; }
        }

        protected override bool returnBtn
        {
            get { return false; }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID)
        {
            return PageUtils.GetCMSUrl(string.Format("background_surveyAnalysisFiles.aspx?PublishmentSystemID={0}&NodeID={1}", publishmentSystemID, nodeID));
        }

        protected override bool GetCanAnalysisFiles(string attr)
        {
            return SurveyQuestionnaireAttribute.IsAnalysisAttribute(attr);
        }



        protected override string GetAvgCompositeScore()
        {
            return DataProvider.SurveyQuestionnaireDAO.GetSelectCommendOfAnalysisByNode(base.PublishmentSystemID, this.nodeInfo.NodeID, this.StartDate.Text, this.EndDate.Text).ToString();
        }

    }
}
