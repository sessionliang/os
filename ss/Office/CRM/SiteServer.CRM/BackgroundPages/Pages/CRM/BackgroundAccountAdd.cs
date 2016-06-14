using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CRM.Core;
using SiteServer.CRM.Model;
using System.Text;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CRM.Controls;
using System.Collections.Specialized;


using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages
{
    public class BackgroundAccountAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public Literal ltlAddUserName;
        public Literal ltlChargeUserName;
        public BREditor ChatOrNote;

        private EAccountType accountType;
        private AccountInfo accountInfo;
        private string returnUrl;

        public static string GetAddUrl(EAccountType accountType, string returnUrl)
        {
            return string.Format("background_accountAdd.aspx?AccountType={0}&ReturnUrl={1}", EAccountTypeUtils.GetValue(accountType), StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetEditUrl(int id, string returnUrl)
        {
            return string.Format("background_accountAdd.aspx?ID={0}&ReturnUrl={1}", id, StringUtils.ValueToUrl(returnUrl));
        }

        public override string GetValue(string attributeName)
        {
            if (attributeName == AccountAttribute.SN)
            {
                if (this.accountInfo == null)
                {
                    return ProjectManager.GetAccountSN(this.accountType);
                }
                else
                {
                    if (string.IsNullOrEmpty(base.GetValue(attributeName)))
                    {
                        return ProjectManager.GetAccountSN(this.accountType);
                    }
                }
            }
            
            return base.GetValue(attributeName);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int id = TranslateUtils.ToInt(Request.QueryString["ID"]);
            this.accountType = EAccountTypeUtils.GetEnumType(Request.QueryString["AccountType"]);
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundAccount.GetRedirectUrl(true, string.Empty, string.Empty, 1);
            }

            if (id > 0)
            {
                this.accountInfo = DataProvider.AccountDAO.GetAccountInfo(id);
                this.accountType = EAccountTypeUtils.GetEnumType(this.accountInfo.AccountType);
                base.AddAttributes(this.accountInfo.ToNameValueCollection());

                this.ltlPageTitle.Text = "修改客户";
            }
            else
            {
                this.ltlPageTitle.Text = "新增客户";
            }

            string uploadImageUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.PublishmentSystemID, ETextEditorType.UEditor, "Image");
            string uploadScrawlUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.PublishmentSystemID, ETextEditorType.UEditor, "Scrawl");
            string uploadFileUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.PublishmentSystemID, ETextEditorType.UEditor, "File");
            string imageManagerUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.PublishmentSystemID, ETextEditorType.UEditor, "ImageManager");
            string getMovieUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.PublishmentSystemID, ETextEditorType.UEditor, "GetMovie");
            //this.breContent.SetUrl(uploadImageUrl, uploadScrawlUrl, uploadFileUrl, imageManagerUrl, getMovieUrl);

            if (!IsPostBack)
            {
                this.ChatOrNote.Text = this.GetValue(AccountAttribute.ChatOrNote);

                if (this.accountInfo != null)
                {
                    this.ltlAddUserName.Text = AdminManager.GetDisplayName(this.accountInfo.AddUserName, true);
                    this.ltlChargeUserName.Text = string.Format(@"<div class=""btn_pencil"" onclick=""{0}""><span class=""pencil""></span>　选择</div><script language=""javascript"">chargeUserName('{1}', '{2}');</script>", Modal.UserNameSelect.GetShowPopWinString(AdminManager.Current.DepartmentID, "chargeUserName"), AdminManager.GetDisplayName(this.accountInfo.ChargeUserName, true), this.accountInfo.ChargeUserName);
                }
                else
                {
                    this.ltlAddUserName.Text = AdminManager.GetDisplayName(AdminManager.Current.UserName, true);
                    this.ltlChargeUserName.Text = string.Format(@"<div class=""btn_pencil"" onclick=""{0}""><span class=""pencil""></span>　选择</div><script language=""javascript"">chargeUserName('{1}', '{2}');</script>", Modal.UserNameSelect.GetShowPopWinString(AdminManager.Current.DepartmentID, "chargeUserName"), AdminManager.GetDisplayName(AdminManager.Current.UserName, true), AdminManager.Current.UserName);
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
                string location = base.Request.Form["location"];

                if (this.accountInfo != null)
                {
                    AccountInfo accountInfoToEdit = new AccountInfo(base.Request.Form);
                    accountInfoToEdit.AddUserName = AdminManager.Current.UserName;
                    accountInfoToEdit.AddDate = DateTime.Now;

                    foreach (string attributeName in AccountAttribute.AllAttributes)
                    {
                        if (base.Request.Form[attributeName] != null)
                        {
                            this.accountInfo.SetValue(attributeName, accountInfoToEdit.GetValue(attributeName));
                        }
                    }
                    if (!string.IsNullOrEmpty(location) && location.Split(',').Length == 3)
                    {
                        accountInfo.Province = location.Split(',')[0];
                        accountInfo.City = location.Split(',')[1];
                        accountInfo.Area = location.Split(',')[2];
                    }

                    DataProvider.AccountDAO.Update(this.accountInfo);

                    base.SuccessMessage("客户修改成功");
                }
                else
                {
                    AccountInfo accountInfoToAdd = new AccountInfo(base.Request.Form);
                    accountInfoToAdd.AddUserName = AdminManager.Current.UserName;
                    accountInfoToAdd.AddDate = DateTime.Now;

                    if (!string.IsNullOrEmpty(location) && location.Split(',').Length == 3)
                    {
                        accountInfoToAdd.Province = location.Split(',')[0];
                        accountInfoToAdd.City = location.Split(',')[1];
                        accountInfoToAdd.Area = location.Split(',')[2];
                    }

                    DataProvider.AccountDAO.Insert(accountInfoToAdd);

                    base.SuccessMessage("客户添加成功");
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
