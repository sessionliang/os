using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Web.UI;
using BaiRong.Core;
using System.Collections;
using SiteServer.BBS.Core;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundAdSelect : BackgroundBasePage
    {
        public DataList MyDataList;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!Page.IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Template, "广告管理", AppManager.BBS.Permission.BBS_Template);

                MyDataList.DataSource = EAdLocationUtils.GetArrayList();
                MyDataList.ItemDataBound += new DataListItemEventHandler(MyDataList_ItemDataBound);
                MyDataList.DataBind();
            }
        }

        private void MyDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                EAdLocation adLocation = (EAdLocation)e.Item.DataItem;

                Literal ltlImage = e.Item.FindControl("ltlImage") as Literal;
                Literal ltlTitle = e.Item.FindControl("ltlTitle") as Literal;

                int count = DataProvider.AdDAO.GetCount(base.PublishmentSystemID, adLocation);

                ltlImage.Text = string.Format(@"<a href=""{0}""><img src=""images/adLocation/{1}.gif"" alt=""{2}"" border=""0"" /></a><br />", BackgroundAd.GetRedirectUrl(base.PublishmentSystemID, adLocation), EAdLocationUtils.GetValue(adLocation), EAdLocationUtils.GetDescription(adLocation));
                ltlTitle.Text = string.Format(@"<a href=""{0}"">{1}{2}</a>", BackgroundAd.GetRedirectUrl(base.PublishmentSystemID, adLocation), EAdLocationUtils.GetText(adLocation), count == 0 ? string.Empty : string.Format("({0})", count));
            }
        }
    }
}
