﻿using System;
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


namespace SiteServer.WCM.BackgroundPages
{
	public class BackgroundGovInteractListAccept : BackgroundGovInteractListBasePage
	{
        public void Page_Load(object sender, EventArgs E)
        {
            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_GovInteract, "待受理办件", AppManager.CMS.Permission.WebSite.GovInteract);
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int nodeID)
        {
            return PageUtils.GetWCMUrl(string.Format("background_govInteractListAccept.aspx?PublishmentSystemID={0}&NodeID={1}", publishmentSystemID, nodeID));
        }

        protected override string GetSelectString()
        {
            return DataProvider.GovInteractContentDAO.GetSelectStringByState(base.PublishmentSystemInfo, base.nodeID, EGovInteractState.New);
        }

        private string _pageUrl;
        protected override string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(this._pageUrl))
                {
                    _pageUrl = PageUtils.GetWCMUrl(string.Format("background_govInteractListAccept.aspx?PublishmentSystemID={0}&NodeID={1}", base.PublishmentSystemID, base.nodeID));
                }
                return _pageUrl;
            }
        }
	}
}
