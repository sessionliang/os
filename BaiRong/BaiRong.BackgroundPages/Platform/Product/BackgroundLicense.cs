using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Text;

using System.Collections.Generic;

namespace BaiRong.BackgroundPages
{
	public class BackgroundLicense : BackgroundBasePage
	{
        public Repeater rptContents;
		
		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (!IsPostBack)
			{
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Product, "许可证管理", AppManager.Platform.Permission.Platform_Product);

                List<ModuleFileInfo> moduleFileInfoList = ProductFileUtils.GetModuleFileInfoListOfApps();

                this.rptContents.DataSource = moduleFileInfoList;
                this.rptContents.ItemDataBound += new RepeaterItemEventHandler(rptContents_ItemDataBound);
                this.rptContents.DataBind();
			}
		}

        void rptContents_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ModuleFileInfo moduleFileInfo = (ModuleFileInfo)e.Item.DataItem;
                string moduleID = moduleFileInfo.ModuleID.ToLower();

                if (moduleID == AppManager.Platform.AppID)
                {
                    e.Item.Visible = false;
                    return;
                }

                Literal ltlProductName = e.Item.FindControl("ltlProductName") as Literal;
                Literal ltlLicense = e.Item.FindControl("ltlLicense") as Literal;

                LicenseManager licenseManager = LicenseManager.Instance;

                if (licenseManager != null)
                {
                    ltlProductName.Text = moduleFileInfo.ModuleName;

                    StringBuilder builder = new StringBuilder();

                    if (licenseManager.IsLegality)
                    {
                        builder.AppendFormat("<p>此产品为官方正式授权产品，以下为授权信息：</p><hr />", licenseManager.Domain);
                    }

                    builder.AppendFormat("<p>产品版本：{0} {1}</p>", AppManager.GetAppName(moduleID, true), ProductManager.GetFullVersion());

                    string urlUpload = PageUtils.GetPlatformUrl(string.Format("background_licenseUpload.aspx?productID={0}", moduleID));

                    if (licenseManager.IsLegality)
                    {
                        if (!string.IsNullOrEmpty(licenseManager.Domain))
                        {
                            builder.AppendFormat("<p>产品授权域名：{0}</p>", licenseManager.Domain);
                        }
                        builder.AppendFormat("<p>可建网站数量：{0}</p>", LicenseManager.Instance.IsMaxSiteNumberLimited ? LicenseManager.Instance.MaxSiteNumber.ToString() : "不限");

                        builder.AppendFormat("<p><a href='{0}'>更换许可证</a></p>", urlUpload);
                    }
                    else
                    {
                        if (licenseManager.IsExpireDateLimited)
                        {
                            if (licenseManager.ExpireDate > DateTime.Now)
                            {
                                TimeSpan ts = licenseManager.ExpireDate - DateTime.Now;
                                builder.AppendFormat("<span style='color:red'>此产品未经正式授权，仅供体验使用，剩余体验时间：{0}天</span>&nbsp;&nbsp;<a href='{1}'>更换许可证</a>", (ts.Days > 0) ? ts.Days : 1, urlUpload);
                            }
                            else
                            {
                                builder.AppendFormat("<span style='color:red'>此产品未经正式授权，仅供体验使用，目前已过体验期限，请联系官方渠道以便获取正式授权</span>&nbsp;&nbsp;<a href='{0}'>更换许可证</a>", urlUpload);
                            }
                        }
                        else
                        {
                            builder.AppendFormat("<span style='color:red'>此产品未经正式授权，仅可用于个人及非商业用途</span>&nbsp;&nbsp;<a href='{0}'>更换许可证</a>", urlUpload);
                        }
                    }

                    ltlLicense.Text = builder.ToString();
                }
            }
        }
	}
}
