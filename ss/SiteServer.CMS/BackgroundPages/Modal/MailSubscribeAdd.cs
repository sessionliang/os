using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.IO;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using System;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class MailSubscribeAdd : BackgroundBasePage
	{
        protected TextBox Receiver;
        protected TextBox Mail;

        public static string GetOpenWindowString(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("ÃÌº”” º˛∂©‘ƒ", "modal_mailSubscribeAdd.aspx", arguments, 400, 250);
        }
	
		public void Page_Load(object sender, System.EventArgs e)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            try
            {
                if (DataProvider.MailSubscribeDAO.IsExists(base.PublishmentSystemID, this.Mail.Text))
                {
                    base.FailMessage("” º˛∂©‘ƒÃÌº” ß∞‹£¨” œ‰“—¥Ê‘⁄£°");
                }
                else
                {
                    MailSubscribeInfo msInfo = new MailSubscribeInfo(0, base.PublishmentSystemID, this.Receiver.Text, this.Mail.Text, string.Empty, DateTime.Now);
                    DataProvider.MailSubscribeDAO.Insert(msInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "ÃÌº”” º˛∂©‘ƒ", string.Format("” œ‰:{0}", this.Mail.Text));

                    isChanged = true;
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
                isChanged = false;
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }
	}
}
