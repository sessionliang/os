using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Web.UI;

using System.Text;

namespace SiteServer.WCM.BackgroundPages
{
	public class BackgroundGovPublicApplyToReply : BackgroundGovPublicApplyToBasePage
    {
        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovPublic, AppManager.CMS.LeftMenu.GovPublic.ID_GovPublicApply, "待办理申请", AppManager.CMS.Permission.WebSite.GovPublicApply);
            }
        }

        protected override string GetSelectString()
        {
            return DataProvider.GovPublicApplyDAO.GetSelectStringByState(base.PublishmentSystemID, EGovPublicApplyState.Accepted, EGovPublicApplyState.Redo);
        }

        private string _pageUrl;
        protected override string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetWCMUrl(string.Format("background_govPublicApplyToReply.aspx?PublishmentSystemID={0}", base.PublishmentSystemID));
                }
                return _pageUrl;
            }
        }
	}
}
