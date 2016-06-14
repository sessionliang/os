using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;


using BaiRong.Core.Data.Provider;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.B2C.BackgroundPages.Modal
{
	public class LocationAdd : BackgroundBasePage
	{
        public TextBox LocationName;
        public PlaceHolder phParentID;
        public DropDownList ParentID;

        private int locationID = 0;
        private string returnUrl = string.Empty;
        private bool[] isLastNodeArray;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtilityB2C.GetOpenWindowString("添加地区", "modal_locationAdd.aspx", arguments, 460, 360);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int locationID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("LocationID", locationID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtilityB2C.GetOpenWindowString("修改地区", "modal_locationAdd.aspx", arguments, 460, 360);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.locationID = base.GetIntQueryString("LocationID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundLocation.GetRedirectUrl(base.PublishmentSystemID, 0);
            }

			if (!IsPostBack)
			{
                if (this.locationID == 0)
                {
                    this.ParentID.Items.Add(new ListItem("<无上级地区>", "0"));

                    ArrayList locationIDArrayList = LocationManager.GetLocationIDArrayList(base.PublishmentSystemID);
                    int count = locationIDArrayList.Count;
                    this.isLastNodeArray = new bool[count];
                    foreach (int theLocationID in locationIDArrayList)
                    {
                        LocationInfo locationInfo = LocationManager.GetLocationInfo(base.PublishmentSystemID, theLocationID);
                        if (locationInfo.ParentsCount < 2)
                        {
                            ListItem listitem = new ListItem(this.GetTitle(locationInfo.ID, locationInfo.LocationName, locationInfo.ParentsCount, locationInfo.IsLastNode), theLocationID.ToString());
                            this.ParentID.Items.Add(listitem);
                        }
                    }
                }
                else
                {
                    this.phParentID.Visible = false;
                }

                if (this.locationID != 0)
                {
                    LocationInfo locationInfo = LocationManager.GetLocationInfo(base.PublishmentSystemID, this.locationID);

                    this.LocationName.Text = locationInfo.LocationName;
                    this.ParentID.SelectedValue = locationInfo.ParentID.ToString();
                }
			}
		}

        public string GetTitle(int locationID, string locationName, int parentsCount, bool isLastNode)
        {
            string str = "";
            if (isLastNode == false)
            {
                this.isLastNodeArray[parentsCount] = false;
            }
            else
            {
                this.isLastNodeArray[parentsCount] = true;
            }
            for (int i = 0; i < parentsCount; i++)
            {
                if (this.isLastNodeArray[i])
                {
                    str = String.Concat(str, "　");
                }
                else
                {
                    str = String.Concat(str, "│");
                }
            }
            if (isLastNode)
            {
                str = String.Concat(str, "└");
            }
            else
            {
                str = String.Concat(str, "├");
            }
            str = String.Concat(str, locationName);
            return str;
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            try
            {
                if (this.locationID == 0)
                {
                    LocationInfo locationInfo = new LocationInfo();
                    locationInfo.LocationName = this.LocationName.Text;
                    locationInfo.ParentID = TranslateUtils.ToInt(this.ParentID.SelectedValue);

                    DataProviderB2C.LocationDAO.Insert(base.PublishmentSystemID, locationInfo);
                }
                else
                {
                    LocationInfo locationInfo = LocationManager.GetLocationInfo(base.PublishmentSystemID, this.locationID);

                    locationInfo.LocationName = this.LocationName.Text;
                    locationInfo.ParentID = TranslateUtils.ToInt(this.ParentID.SelectedValue);

                    DataProviderB2C.LocationDAO.Update(base.PublishmentSystemID, locationInfo);
                }

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "维护地区信息");

                base.SuccessMessage("地区设置成功！");
                isChanged = true;
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "地区设置失败！");
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
            }
        }
	}
}
