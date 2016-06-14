using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;


using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using SiteServer.CMS.BackgroundPages;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.BackgroundPages
{
    public class BackgroundLocation : BackgroundBasePage
    {
        public Repeater rptContents;

        public Button btnAdd;
        public Button btnDelete;

        private int currentLocationID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!string.IsNullOrEmpty(base.GetQueryString("Delete")) && !string.IsNullOrEmpty(base.GetQueryString("LocationIDCollection")))
            {
                ArrayList locationIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.GetQueryString("LocationIDCollection"));
                foreach (int locationID in locationIDArrayList)
                {
                    DataProviderB2C.LocationDAO.Delete(base.PublishmentSystemID, locationID);
                }
                base.SuccessMessage("成功删除所选地区");
            }
            else if (!string.IsNullOrEmpty(base.GetQueryString("LocationID")) && (!string.IsNullOrEmpty(base.GetQueryString("Subtract")) || !string.IsNullOrEmpty(base.GetQueryString("Add"))))
            {
                int locationID = int.Parse(base.GetQueryString("LocationID"));
                bool isSubtract = (!string.IsNullOrEmpty(base.GetQueryString("Subtract"))) ? true : false;
                DataProviderB2C.LocationDAO.UpdateTaxis(base.PublishmentSystemID, locationID, isSubtract);

                PageUtils.Redirect(BackgroundLocation.GetRedirectUrl(base.PublishmentSystemID, locationID));
                return;
            }

            if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.B2C.LeftMenu.ID_PaymentShipment, "地区管理", string.Empty);

                base.RegisterClientScriptBlock("NodeTreeScript", LocationTreeItem.GetScript(ELocationLoadingType.Management, null));

                if (!string.IsNullOrEmpty(base.GetQueryString("CurrentLocationID")))
                {
                    this.currentLocationID = base.GetIntQueryString("CurrentLocationID");
                    string onLoadScript = this.GetScriptOnLoad(this.currentLocationID);
                    if (!string.IsNullOrEmpty(onLoadScript))
                    {
                        Page.RegisterClientScriptBlock("NodeTreeScriptOnLoad", onLoadScript);
                    }
                }

                NameValueCollection arguments = new NameValueCollection();
                string showPopWinString = string.Empty;

                this.btnAdd.Attributes.Add("onclick", Modal.LocationAdd.GetOpenWindowStringToAdd(base.PublishmentSystemID, BackgroundLocation.GetRedirectUrl(base.PublishmentSystemID, 0)));

                this.btnDelete.Attributes.Add("onclick", JsUtils.GetRedirectStringWithCheckBoxValueAndAlert(BackgroundLocation.GetRedirectUrl(base.PublishmentSystemID, 0) + "&Delete=True", "LocationIDCollection", "LocationIDCollection", "请选择需要删除的地区！", "此操作将删除对应地区以及所有下级地区，确认删除吗？"));

                BindGrid();
            }
        }

        public static string GetRedirectUrl(int publishmentSystemID, int currentLocationID)
        {
            if (currentLocationID != 0)
            {
                return PageUtils.GetB2CUrl(string.Format("background_location.aspx?publishmentSystemID={0}&CurrentLocationID={1}", publishmentSystemID, currentLocationID));
            }
            return PageUtils.GetB2CUrl(string.Format("background_location.aspx?publishmentSystemID={0}", publishmentSystemID));
        }

        public string GetScriptOnLoad(int currentLocationID)
        {
            if (currentLocationID != 0)
            {
                LocationInfo locationInfo = LocationManager.GetLocationInfo(base.PublishmentSystemID, currentLocationID);
                if (locationInfo != null)
                {
                    string path = string.Empty;
                    if (locationInfo.ParentsCount <= 1)
                    {
                        path = currentLocationID.ToString();
                    }
                    else
                    {
                        path = locationInfo.ParentsPath.Substring(locationInfo.ParentsPath.IndexOf(",") + 1) + "," + currentLocationID.ToString();
                    }
                    return LocationTreeItem.GetScriptOnLoad(path);
                }
            }
            return string.Empty;
        }

        public void BindGrid()
        {
            try
            {
                this.rptContents.DataSource = DataProviderB2C.LocationDAO.GetIDArrayListByParentID(base.PublishmentSystemID, 0);
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int locationID = (int)e.Item.DataItem;

            LocationInfo locationInfo = LocationManager.GetLocationInfo(base.PublishmentSystemID, locationID);

            Literal ltlHtml = e.Item.FindControl("ltlHtml") as Literal;

            ltlHtml.Text = BackgroundLocation.GetLocationRowHtml(base.PublishmentSystemID, locationInfo, ELocationLoadingType.Management, null);
        }

        public static string GetLocationRowHtml(int publishmentSystemID, LocationInfo locationInfo, ELocationLoadingType loadingType, NameValueCollection additional)
        {
            LocationTreeItem treeItem = LocationTreeItem.CreateInstance(locationInfo);
            string title = treeItem.GetItemHtml(loadingType, additional, false);

            string rowHtml = string.Empty;

            if (loadingType == ELocationLoadingType.Management)
            {
                string editUrl = string.Empty;
                string upLink = string.Empty;
                string downLink = string.Empty;
                string checkBoxHtml = string.Empty;

                editUrl = string.Format(@"<a href=""javascript:;"" onclick=""{0}"">编辑</a>", Modal.LocationAdd.GetOpenWindowStringToEdit(publishmentSystemID, locationInfo.ID, BackgroundLocation.GetRedirectUrl(publishmentSystemID, locationInfo.ID)));
                string urlUp = BackgroundLocation.GetRedirectUrl(publishmentSystemID, 0) + string.Format("&Subtract=True&LocationID={0}", locationInfo.ID);
                upLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/up.gif"" border=""0"" alt=""上升"" /></a>", urlUp);

                string urlDown = BackgroundLocation.GetRedirectUrl(publishmentSystemID, 0) + string.Format("&Add=True&LocationID={0}", locationInfo.ID);
                downLink = string.Format(@"<a href=""{0}""><img src=""../Pic/icon/down.gif"" border=""0"" alt=""下降"" /></a>", urlDown);

                checkBoxHtml = string.Format("<input type='checkbox' name='LocationIDCollection' value='{0}' />", locationInfo.ID);

                rowHtml = string.Format(@"
<tr treeItemLevel=""{0}"">
    <td>{1}</td>
    <td class=""center"">{2}</td>
    <td class=""center"">{3}</td>
    <td class=""center"">{4}</td>
    <td class=""center"">{5}</td>
</tr>
", locationInfo.ParentsCount + 1, title, upLink, downLink, editUrl, checkBoxHtml);
            }
            return rowHtml;
        }
    }
}
