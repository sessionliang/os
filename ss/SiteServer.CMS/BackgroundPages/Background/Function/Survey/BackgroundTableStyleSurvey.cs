﻿using System;
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
    public class BackgroundTableStyleSurvey : BackgroundTableStyleBase
    {
        protected override string currentAspxName
        {
            get { return "background_surveyTableStyle.aspx"; }
        }

        protected override string tableName
        {
            get { return SurveyQuestionnaireInfo.TableName; }
        }

        protected override string pageTitle
        {
            get { return "调查问卷字段管理"; }
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
    }
}
