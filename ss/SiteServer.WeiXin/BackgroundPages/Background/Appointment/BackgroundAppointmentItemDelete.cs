using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;

using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundAppointmentItemDelete : BackgroundBasePage
    {
        public void Page_Load(object sender, EventArgs E)
        { 
            List<int> list = TranslateUtils.StringCollectionToIntList(base.Request["IDCollection"]);
            if (list.Count > 0)
            {
                try
                {
                    DataProviderWX.AppointmentItemDAO.Delete(base.PublishmentSystemID, list);
                    Response.Write("success");
                    Response.End();
                }
                catch (Exception ex)
                {
                    Response.Write("failure");
                    Response.End();
                }
            }
        }
    }
}
