using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.Project.Core;
using SiteServer.Project.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.Project.Controls;
using System.Collections.Specialized;



namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundContactAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public Literal ltlAddUserName;
        public Literal ltlAccountID;
        public BREditor ChatOrNote;

        private ContactInfo contactInfo;
        private string returnUrl;

        public static string GetAddUrl(string returnUrl)
        {
            return string.Format("background_contactAdd.aspx?ReturnUrl={0}", StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetEditUrl(int id, string returnUrl)
        {
            return string.Format("background_contactAdd.aspx?ID={0}&ReturnUrl={1}", id, StringUtils.ValueToUrl(returnUrl));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int id = TranslateUtils.ToInt(Request.QueryString["ID"]);
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundContact.GetRedirectUrl(true, string.Empty, 1);
            }

            if (id > 0)
            {
                this.contactInfo = DataProvider.ContactDAO.GetContactInfo(id);
                base.AddAttributes(this.contactInfo);

                this.ltlPageTitle.Text = "修改联系人";
            }
            else
            {
                this.ltlPageTitle.Text = "新增联系人";
            }

            string uploadImageUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "Image");
            string uploadScrawlUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "Scrawl");
            string uploadFileUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "File");
            string imageManagerUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "ImageManager");
            string getMovieUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.ProjectID, ETextEditorType.UEditor, "GetMovie");
            //this.breContent.SetUrl(uploadImageUrl, uploadScrawlUrl, uploadFileUrl, imageManagerUrl, getMovieUrl);

            if (!IsPostBack)
            {
                this.ChatOrNote.Text = this.GetValue(ContactAttribute.ChatOrNote);

                if (this.contactInfo != null)
                {
                    this.ltlAddUserName.Text = AdminManager.GetDisplayName(this.contactInfo.AddUserName, true);
                }
                else
                {
                    this.ltlAddUserName.Text = AdminManager.GetDisplayName(AdminManager.Current.UserName, true);
                }

                AccountInfo accountInfo = null;
                if (this.contactInfo != null && this.contactInfo.AccountID > 0)
                {
                    accountInfo = DataProvider.AccountDAO.GetAccountInfo(this.contactInfo.AccountID);
                }

                this.ltlAccountID.Text = string.Format(@"<div class=""btn_pencil"" onclick=""{0}""><span class=""pencil""></span>　选择</div>", Modal.AccountSelect.GetShowPopWinString("selectAccountID", false));
                if (accountInfo != null)
                {
                    this.ltlAccountID.Text += string.Format(@"<script language=""javascript"">selectAccountID('{0}', '{1}');</script>", accountInfo.AccountName, accountInfo.ID);
                }
            }
        }

        public void Return_OnClick(object sender, System.EventArgs e)
        {
            PageUtils.Redirect(this.returnUrl);
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                int accountID = TranslateUtils.ToInt(base.Request.Form["accountID"]);
                int leadID = TranslateUtils.ToInt(base.Request.Form["leadID"]);
                if (this.contactInfo != null)
                {
                    ContactInfo contactInfoToEdit = DataProvider.ContactDAO.GetContactInfo(string.Empty, AdminManager.Current.UserName, accountID, leadID, base.Request.Form);
                    foreach (string attributeName in ContactAttribute.BasicAttributes)
                    {
                        if (base.Request.Form[attributeName] != null)
                        {
                            this.contactInfo.SetExtendedAttribute(attributeName, contactInfoToEdit.GetExtendedAttribute(attributeName));
                        }
                    }
                    this.contactInfo.AccountID = accountID;
                    this.contactInfo.LeadID = leadID;

                    DataProvider.ContactDAO.Update(this.contactInfo);
                }
                else
                {
                    ContactInfo contactInfoToAdd = DataProvider.ContactDAO.GetContactInfo(string.Empty, AdminManager.Current.UserName, accountID, leadID, base.Request.Form);

                    DataProvider.ContactDAO.Insert(contactInfoToAdd);

                    base.SuccessMessage("联系人添加成功");
                }
                
                base.AddWaitAndRedirectScript(this.returnUrl);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
