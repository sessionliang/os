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
using System.Collections.Generic;
using System.Text;
using SiteServer.CMS.Core.Security;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class PublishmentSystemSelect : BackgroundBasePage
    {
        public Literal ltlHtml;

        public static string GetOpenLayerString()
        {
            NameValueCollection arguments = new NameValueCollection();
            return JsUtils.Layer.GetOpenLayerString("—°‘Ò”¶”√", PageUtils.GetCMSUrl("modal_publishmentSystemSelect.aspx"), arguments);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                StringBuilder builder = new StringBuilder();

                List<int> publishmentSystemIDList = ProductPermissionsManager.Current.PublishmentSystemIDList;
                foreach (int publishmentSystemID in publishmentSystemIDList)
                {
                    string loadingUrl = PageUtils.GetLoadingUrl(string.Format("main.aspx?PublishmentSystemID={0}", publishmentSystemID));
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                    builder.AppendFormat(@"
<span class=""icon-span"">
    <a href=""{0}"" target=""_top"">
      {1}
      <h5>
        {2}
        <br>
        <small>{3}</small>
      </h5>
    </a>
  </span>", loadingUrl, EPublishmentSystemTypeUtils.GetIconHtml(publishmentSystemInfo.PublishmentSystemType, "icon-5"), publishmentSystemInfo.PublishmentSystemName, publishmentSystemInfo.PublishmentSystemDir);
                }

                this.ltlHtml.Text = builder.ToString();
            }
        }
    }
}
