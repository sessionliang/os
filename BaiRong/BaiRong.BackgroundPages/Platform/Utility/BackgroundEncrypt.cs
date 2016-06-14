using System;
using System.Web.UI.WebControls;
using BaiRong.Core.Cryptography;

using BaiRong.Core;

namespace BaiRong.BackgroundPages
{
	public class BackgroundEncrypt : BackgroundBasePage
	{
		public TextBox RawString;
        public Literal ltlString;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.Platform.LeftMenu.ID_Utility, "¼ÓÃÜ×Ö·û´®", AppManager.Platform.Permission.Platform_Utility);
            }
        }

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                this.ltlString.Text = EncryptUtils.Instance.EncryptString(RawString.Text);
			}
		}

	}
}
