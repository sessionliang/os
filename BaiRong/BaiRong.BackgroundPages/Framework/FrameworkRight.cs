using System.Web.UI.WebControls;
using BaiRong.Core;
using System;


using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections;
using System.Text;
using BaiRong.Core.WebService;
using BaiRong.Core.Net;

namespace BaiRong.BackgroundPages
{
	public class FrameworkRight : BackgroundBasePage
	{
        public Literal ltlProductName;
        public Literal ltlVersionInfo;
        public Literal ltlUpdateDate;
        public Literal ltlLastLoginDate;

        public DataList dlContents;

        private string productID;

		public void Page_Load(object sender, System.EventArgs e)
		{
            if (base.IsForbidden) return;

            this.productID = base.GetQueryString("productID");

            ModuleFileInfo moduleFileInfo = ProductFileUtils.GetModuleFileInfo(this.productID);

            this.ltlProductName.Text = moduleFileInfo.FullName;
            if (!string.IsNullOrEmpty(moduleFileInfo.Description))
            {
                this.ltlProductName.Text += " ¡ª " + moduleFileInfo.Description;
            }

            this.ltlVersionInfo.Text = ProductManager.GetFullVersion();

            DateTime dateTime = BaiRongDataProvider.LogDAO.GetLastLoginDate(AdminManager.Current.UserName);

            if (dateTime != DateTime.MinValue)
            {
                this.ltlLastLoginDate.Text = DateUtils.GetDateAndTimeString(dateTime);
            }

            this.ltlUpdateDate.Text = DateUtils.GetDateAndTimeString(ConfigManager.Instance.UpdateDate);
		}
	}
}
