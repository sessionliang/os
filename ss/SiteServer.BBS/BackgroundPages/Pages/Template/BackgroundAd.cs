using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.BBS.Model;
using System.Web.UI;
using BaiRong.Core;
using System.Collections;
using SiteServer.BBS.Core;
using BaiRong.Model;

namespace SiteServer.BBS.BackgroundPages
{
    public class BackgroundAd : BackgroundBasePage
    {
        public DataGrid MyDataGrid;

        public Button AddAd;

        private EAdLocation adLocation;

        public static string GetRedirectUrl(int publishmentSystemID, EAdLocation adLocation)
        {
            return PageUtils.GetBBSUrl(string.Format("background_ad.aspx?publishmentSystemID={0}&adLocation={1}", publishmentSystemID, EAdLocationUtils.GetValue(adLocation)));
        }

        public string GetAdType(string adTypeStr)
        {
            EAdType adType = EAdTypeUtils.GetEnumType(adTypeStr);
            return EAdTypeUtils.GetText(adType);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.adLocation = EAdLocationUtils.GetEnumType(base.GetQueryString("adLocation"));

            if (base.GetQueryString("Delete") != null)
            {
                int adID = base.GetIntQueryString("adID");
                try
                {
                    DataProvider.AdDAO.Delete(base.PublishmentSystemID, adID);

                    base.SuccessMessage("成功删除广告");
                }
                catch (Exception ex)
                {
                    base.SuccessMessage(string.Format("删除广告失败，{0}", ex.Message));
                }
            }

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.BBS.LeftMenu.ID_Template, "广告管理", AppManager.BBS.Permission.BBS_Template);

                MyDataGrid.DataSource = DataProvider.AdDAO.GetDataSource(base.PublishmentSystemID, this.adLocation);
                MyDataGrid.ItemDataBound += MyDataGrid_ItemDataBound;
                MyDataGrid.DataBind();
            }
        }

        void MyDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int adID = TranslateUtils.EvalInt(e.Item.DataItem, "ID");
                string adName = TranslateUtils.EvalString(e.Item.DataItem, "AdName");

                Literal ltlEditUrl = e.Item.FindControl("ltlEditUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlEditUrl.Text = string.Format(@"<a href=""{0}"">编辑</a>", BackgroundAdAdd.GetRedirectUrl(base.PublishmentSystemID, this.adLocation, adID));
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}&Delete=True&adID={1}"" onClick=""javascript:return confirm('此操作将删除广告“{2}”，确认吗？');"">删除</a>", BackgroundAd.GetRedirectUrl(base.PublishmentSystemID, this.adLocation), adID, adName);
            }
        }

        public string GetIsEnabled(string isEnabledStr)
        {
            return StringUtilityBBS.GetTrueOrFalseImageHtml(isEnabledStr);
        }

        public void AddAd_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(BackgroundAdAdd.GetRedirectUrl(base.PublishmentSystemID, this.adLocation, 0));
        }
    }
}
