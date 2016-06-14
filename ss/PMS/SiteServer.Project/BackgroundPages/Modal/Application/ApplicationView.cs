using System;
using System.Collections;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SiteServer.Project.Model;
using SiteServer.Project.Core;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Cryptography;

namespace SiteServer.Project.BackgroundPages.Modal
{
    public class ApplicationView : BackgroundBasePage
    {
        public Literal ltlID;
        public Literal ltlApplicationType;
        public Literal ltlApplyResource;
        public Literal ltlContactPerson;
        public Literal ltlEmail;
        public Literal ltlMobile;
        public Literal ltlQQ;
        public Literal ltlTelephone;
        public Literal ltlLocation;
        public Literal ltlAddress;
        public Literal ltlOrgType;
        public Literal ltlOrgName;
        public Literal ltlIsITDepartment;
        public Literal ltlComment;
        public Literal ltlIPAddress;
        public Literal ltlAddDate;

        private int id;

        public static string GetShowPopWinString(int id)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ID", id.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("资源申请查看", "modal_product_applicationView.aspx", arguments, true);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            this.id = int.Parse(base.Request.QueryString["ID"]);

            if (!IsPostBack)
            {
                ApplicationInfo applicationInfo = DataProvider.ApplicationDAO.GetApplicationInfo(this.id);

                this.ltlID.Text = applicationInfo.ID.ToString();
                this.ltlApplicationType.Text = EApplicationTypeUtils.GetText(applicationInfo.ApplicationType);
                this.ltlApplyResource.Text = applicationInfo.ApplyResource;
                this.ltlContactPerson.Text = applicationInfo.ContactPerson;
                this.ltlEmail.Text = applicationInfo.Email;
                this.ltlMobile.Text = applicationInfo.Mobile;
                this.ltlQQ.Text = applicationInfo.QQ;
                this.ltlTelephone.Text = applicationInfo.Telephone;
                this.ltlLocation.Text = applicationInfo.Location;
                this.ltlAddress.Text = applicationInfo.Address;
                this.ltlOrgType.Text = applicationInfo.OrgType;
                this.ltlOrgName.Text = applicationInfo.OrgName;
                this.ltlIsITDepartment.Text = StringUtils.GetTrueOrFalseImageHtml(applicationInfo.IsITDepartment);
                this.ltlComment.Text = applicationInfo.Comment;
                this.ltlIPAddress.Text = applicationInfo.IPAddress;
                this.ltlAddDate.Text = DateUtils.GetDateAndTimeString(applicationInfo.AddDate, BaiRong.Model.EDateFormatType.Chinese, BaiRong.Model.ETimeFormatType.ShortTime);
            }
        }
    }
}
