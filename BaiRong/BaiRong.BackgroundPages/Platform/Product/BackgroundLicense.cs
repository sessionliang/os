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
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Product, "���֤����", AppManager.Platform.Permission.Platform_Product);

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
                        builder.AppendFormat("<p>�˲�ƷΪ�ٷ���ʽ��Ȩ��Ʒ������Ϊ��Ȩ��Ϣ��</p><hr />", licenseManager.Domain);
                    }

                    builder.AppendFormat("<p>��Ʒ�汾��{0} {1}</p>", AppManager.GetAppName(moduleID, true), ProductManager.GetFullVersion());

                    string urlUpload = PageUtils.GetPlatformUrl(string.Format("background_licenseUpload.aspx?productID={0}", moduleID));

                    if (licenseManager.IsLegality)
                    {
                        if (!string.IsNullOrEmpty(licenseManager.Domain))
                        {
                            builder.AppendFormat("<p>��Ʒ��Ȩ������{0}</p>", licenseManager.Domain);
                        }
                        builder.AppendFormat("<p>�ɽ���վ������{0}</p>", LicenseManager.Instance.IsMaxSiteNumberLimited ? LicenseManager.Instance.MaxSiteNumber.ToString() : "����");

                        builder.AppendFormat("<p><a href='{0}'>�������֤</a></p>", urlUpload);
                    }
                    else
                    {
                        if (licenseManager.IsExpireDateLimited)
                        {
                            if (licenseManager.ExpireDate > DateTime.Now)
                            {
                                TimeSpan ts = licenseManager.ExpireDate - DateTime.Now;
                                builder.AppendFormat("<span style='color:red'>�˲�Ʒδ����ʽ��Ȩ����������ʹ�ã�ʣ������ʱ�䣺{0}��</span>&nbsp;&nbsp;<a href='{1}'>�������֤</a>", (ts.Days > 0) ? ts.Days : 1, urlUpload);
                            }
                            else
                            {
                                builder.AppendFormat("<span style='color:red'>�˲�Ʒδ����ʽ��Ȩ����������ʹ�ã�Ŀǰ�ѹ��������ޣ�����ϵ�ٷ������Ա��ȡ��ʽ��Ȩ</span>&nbsp;&nbsp;<a href='{0}'>�������֤</a>", urlUpload);
                            }
                        }
                        else
                        {
                            builder.AppendFormat("<span style='color:red'>�˲�Ʒδ����ʽ��Ȩ���������ڸ��˼�����ҵ��;</span>&nbsp;&nbsp;<a href='{0}'>�������֤</a>", urlUpload);
                        }
                    }

                    ltlLicense.Text = builder.ToString();
                }
            }
        }
	}
}
