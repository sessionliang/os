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
    public class BackgroundTableStyleCompare : BackgroundTableStyleBase
    {
        protected override string currentAspxName
        {
            get { return "background_compareTableStyle.aspx"; }
        }

        protected override string tableName
        {
            get { return CompareContentInfo.TableName; }
        }

        protected override string pageTitle
        {
            get { return "比较反馈字段管理"; }
        }

        protected override string leftMenu
        {
            get { return AppManager.CMS.LeftMenu.ID_Function; }
        }

        protected override string leftSubMenu
        {
            get { return AppManager.CMS.LeftMenu.Function.ID_Compare; }
        }

        protected override string permission
        {
            get { return AppManager.CMS.Permission.WebSite.Compare; }
        }
    }
}
