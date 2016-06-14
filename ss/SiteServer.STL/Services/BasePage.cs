using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SiteServer.CMS.Model;
using BaiRong.Core;
using System.Drawing;
using SiteServer.CMS.Core;

namespace SiteServer.CMS.Services
{
    public class BasePage : Page
    {
        private int publishmentSystemID = -1;
        public int PublishmentSystemID
        {
            get
            {
                if (publishmentSystemID == -1)
                {
                    string idStr = Request.QueryString["publishmentSystemID"];
                    if (!string.IsNullOrEmpty(idStr) && idStr.IndexOf(",") != -1)
                    {
                        idStr = idStr.Substring(0, idStr.Length - idStr.IndexOf(",") - 1);
                    }
                    publishmentSystemID = TranslateUtils.ToInt(idStr);
                }
                return publishmentSystemID;
            }
        }

        private PublishmentSystemInfo publishmentSystemInfo;
        public PublishmentSystemInfo PublishmentSystemInfo
        {
            get
            {
                if (publishmentSystemInfo == null)
                {
                    publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(this.PublishmentSystemID);
                }
                return publishmentSystemInfo;
            }
        }

        public string IconUrl
        {
            get
            {
                return PageUtility.Services.GetImageUrl(string.Empty);
            }
        }

        public void MyDataGrid_ItemBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes["onmouseover"] = "this.className=\'tdbg-dark\';";
                e.Item.Attributes["onmouseout"] = "this.className=\'tdbg\';";
            }
        }
    }
}
