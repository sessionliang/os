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


using System.Collections.Generic;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.CRM.BackgroundPages
{
    public class BackgroundLeadAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public Literal ltlChargeUserName;
        public Literal ltlContactScript;
        public BREditor ChatOrNote;

        private LeadInfo leadInfo;
        private string returnUrl;

        public static string GetAddUrl(string returnUrl)
        {
            return string.Format("background_leadAdd.aspx?ReturnUrl={0}", StringUtils.ValueToUrl(returnUrl));
        }

        public static string GetEditUrl(int id, string returnUrl)
        {
            return string.Format("background_leadAdd.aspx?ID={0}&ReturnUrl={1}", id, StringUtils.ValueToUrl(returnUrl));
        }

        public override string GetValue(string attributeName)
        {
            string retval = base.GetValue(attributeName);
            if (attributeName == LeadAttribute.Possibility)
            {
                if (retval == "0")
                {
                    retval = string.Empty;
                }
            }
            return retval;
        }

        public string GetOptions(string attributeName)
        {
            StringBuilder builder = new StringBuilder();
            if (attributeName == "Status")
            {
                ELeadStatus status = ELeadStatus.New;
                if (this.leadInfo != null)
                {
                    status = ELeadStatusUtils.GetEnumType(this.leadInfo.Status);
                }
                if (this.leadInfo == null || status == ELeadStatus.New || status == ELeadStatus.Contacting)
                {
                    builder.AppendFormat(@"<option {0} value=""{1}"">{2}</option>", base.GetSelected(attributeName, ELeadStatusUtils.GetValue(ELeadStatus.New)), ELeadStatusUtils.GetValue(ELeadStatus.New), ELeadStatusUtils.GetText(ELeadStatus.New));
                    builder.AppendFormat(@"<option {0} value=""{1}"">{2}</option>", base.GetSelected(attributeName, ELeadStatusUtils.GetValue(ELeadStatus.Contacting)), ELeadStatusUtils.GetValue(ELeadStatus.Contacting), ELeadStatusUtils.GetText(ELeadStatus.Contacting));
                }
                else
                {
                    builder.AppendFormat(@"<option {0} value=""{1}"">{2}</option>", base.GetSelected(attributeName, ELeadStatusUtils.GetValue(status)), ELeadStatusUtils.GetValue(status), ELeadStatusUtils.GetText(status));
                }
            }
            return builder.ToString();
        }

        public void Page_Load(object sender, EventArgs E)
        {
            int id = TranslateUtils.ToInt(Request.QueryString["ID"]);
            this.returnUrl = StringUtils.ValueFromUrl(Request.QueryString["ReturnUrl"]);
            if (string.IsNullOrEmpty(this.returnUrl))
            {
                this.returnUrl = BackgroundLead.GetRedirectUrl(true, string.Empty, string.Empty, 1);
            }

            if (id > 0)
            {
                this.leadInfo = DataProvider.LeadDAO.GetLeadInfo(id);
                base.AddAttributes(this.leadInfo.ToNameValueCollection());

                this.ltlPageTitle.Text = "修改线索";
            }
            else
            {
                this.ltlPageTitle.Text = "新增线索";
            }

            string uploadImageUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.PublishmentSystemID, ETextEditorType.UEditor, "Image");
            string uploadScrawlUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.PublishmentSystemID, ETextEditorType.UEditor, "Scrawl");
            string uploadFileUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.PublishmentSystemID, ETextEditorType.UEditor, "File");
            string imageManagerUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.PublishmentSystemID, ETextEditorType.UEditor, "ImageManager");
            string getMovieUrl = BackgroundTextEditorUpload.GetReidrectUrl(base.PublishmentSystemID, ETextEditorType.UEditor, "GetMovie");
            //this.breContent.SetUrl(uploadImageUrl, uploadScrawlUrl, uploadFileUrl, imageManagerUrl, getMovieUrl);

            if (!IsPostBack)
            {
                if (this.leadInfo != null)
                {
                    if (this.leadInfo.IsAccount)
                    {
                        AccountInfo accountInfo = DataProvider.AccountDAO.GetAccountInfo(this.leadInfo.AccountID);
                        if (accountInfo != null)
                        {
                            this.leadInfo.AccountName = accountInfo.AccountName;
                            this.leadInfo.BusinessType = accountInfo.BusinessType;
                            this.leadInfo.Website = accountInfo.Website;
                            this.leadInfo.Province = accountInfo.Province;
                            this.leadInfo.City = accountInfo.City;
                            this.leadInfo.Area = accountInfo.Area;
                            this.leadInfo.Address = accountInfo.Address;
                        }
                        else
                        {
                            this.leadInfo.IsAccount = false;
                        }
                    }

                    if (this.leadInfo.IsContact)
                    {
                        List<ContactInfo> contactInfoList = DataProvider.ContactDAO.GetContactInfoList(this.leadInfo.ID);
                        foreach (ContactInfo contactInfo in contactInfoList)
                        {
                            this.ltlContactScript.Text += string.Format("addContact(true, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');", contactInfo.ID, contactInfo.ContactName, contactInfo.JobTitle, contactInfo.AccountRole, contactInfo.Mobile, contactInfo.Telephone, contactInfo.Email, contactInfo.QQ);
                        }
                    }
                    else
                    {
                        this.ltlContactScript.Text = string.Format("addContact(false, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}');", 0, this.leadInfo.ContactName, this.leadInfo.JobTitle, this.leadInfo.AccountRole, this.leadInfo.Mobile, this.leadInfo.Telephone, this.leadInfo.Email, this.leadInfo.QQ);
                    }

                    this.ltlChargeUserName.Text = string.Format(@"<div class=""btn_pencil"" onclick=""{0}""><span class=""pencil""></span>　选择</div><script language=""javascript"">chargeUserName('{1}', '{2}');</script>", Modal.UserNameSelect.GetShowPopWinString(AdminManager.Current.DepartmentID, "chargeUserName"), AdminManager.GetDisplayName(leadInfo.ChargeUserName, true), leadInfo.ChargeUserName);
                }
                else
                {
                    this.ltlContactScript.Text = "addContact(false);";

                    this.ltlChargeUserName.Text = string.Format(@"<div class=""btn_pencil"" onclick=""{0}""><span class=""pencil""></span>　选择</div><script language=""javascript"">chargeUserName('{1}', '{2}');</script>", Modal.UserNameSelect.GetShowPopWinString(AdminManager.Current.DepartmentID, "chargeUserName"), AdminManager.GetDisplayName(AdminManager.Current.UserName, true), AdminManager.Current.UserName);
                }

                this.ChatOrNote.Text = this.GetValue(LeadAttribute.ChatOrNote);
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

                if (this.leadInfo != null)
                {
                    LeadInfo leadInfoToEdit = new LeadInfo(base.Request.Form);

                    foreach (string attributeName in LeadAttribute.AllAttributes)
                    {
                        if (base.Request.Form[attributeName] != null)
                        {
                            this.leadInfo.SetValue(attributeName, leadInfoToEdit.GetValue(attributeName));
                        }
                    }
                    if (!string.IsNullOrEmpty(location) && location.Split(',').Length == 3)
                    {
                        this.leadInfo.Province = location.Split(',')[0];
                        this.leadInfo.City = location.Split(',')[1];
                        this.leadInfo.Area = location.Split(',')[2];
                    }

                    this.AccountUpdate(location);
                    this.ContactUpdate();

                    DataProvider.LeadDAO.Update(this.leadInfo);
                }
                else
                {
                    LeadInfo leadInfoToAdd = new LeadInfo(base.Request.Form);
                    leadInfoToAdd.AddUserName = AdminManager.Current.UserName;
                    leadInfoToAdd.AddDate = DateTime.Now;

                    if (!string.IsNullOrEmpty(location) && location.Split(',').Length == 3)
                    {
                        leadInfoToAdd.Province = location.Split(',')[0];
                        leadInfoToAdd.City = location.Split(',')[1];
                        leadInfoToAdd.Area = location.Split(',')[2];
                    }

                    if (leadInfoToAdd.IsAccount)
                    {
                        int accountID = this.AccountInsert(location);
                        leadInfoToAdd.AccountID = accountID;
                    }

                    int leadID = DataProvider.LeadDAO.Insert(leadInfoToAdd);

                    if (leadInfoToAdd.IsContact)
                    {
                        this.ContactInsert(leadInfoToAdd.AccountID, leadID);
                    }

                    base.SuccessMessage("线索添加成功");
                }
                
                base.AddWaitAndRedirectScript(this.returnUrl);
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }

        private void ContactInsert(int accountID, int leadID)
        {
            ContactInfo contactInfo = DataProvider.ContactDAO.GetContactInfo(string.Empty, AdminManager.Current.UserName, accountID, leadID, base.Request.Form);

            if (StringUtils.Contains(contactInfo.ContactName, ","))
            {
                List<string> contactNameList = TranslateUtils.StringCollectionToStringList(contactInfo.ContactName);
                List<string> jobTitleList = TranslateUtils.StringCollectionToStringList(contactInfo.JobTitle);
                List<string> accountRoleList = TranslateUtils.StringCollectionToStringList(contactInfo.AccountRole);
                List<string> mobileList = TranslateUtils.StringCollectionToStringList(contactInfo.Mobile);
                List<string> telephoneList = TranslateUtils.StringCollectionToStringList(contactInfo.Telephone);
                List<string> emailList = TranslateUtils.StringCollectionToStringList(contactInfo.Email);
                List<string> qqList = TranslateUtils.StringCollectionToStringList(contactInfo.QQ);

                for (int i = 0; i < contactNameList.Count; i++)
                {
                    ContactInfo contactInfoNew = contactInfo;
                    contactInfoNew.ContactName = contactNameList[i];
                    contactInfoNew.JobTitle = jobTitleList[i];
                    contactInfoNew.AccountRole = accountRoleList[i];
                    contactInfoNew.Mobile = mobileList[i];
                    contactInfoNew.Telephone = telephoneList[i];
                    contactInfoNew.Email = emailList[i];
                    contactInfoNew.QQ = qqList[i];

                    DataProvider.ContactDAO.Insert(contactInfoNew);
                }
            }
            else
            {
                DataProvider.ContactDAO.Insert(contactInfo);
            }
        }

        private void ContactUpdate()
        {
            if (this.leadInfo.IsContact)
            {
                List<ContactInfo> contactInfoList = DataProvider.ContactDAO.GetContactInfoList(this.leadInfo.ID);

                if (contactInfoList.Count > 0)
                {
                    ContactInfo contactInfo = DataProvider.ContactDAO.GetContactInfo(string.Empty, AdminManager.Current.UserName, this.leadInfo.AccountID, this.leadInfo.ID, base.Request.Form);

                    List<int> contactIDList = TranslateUtils.StringCollectionToIntList(base.Request.Form["contactID"]);
                    List<string> contactNameList = TranslateUtils.StringCollectionToStringList(contactInfo.ContactName);
                    List<string> jobTitleList = TranslateUtils.StringCollectionToStringList(contactInfo.JobTitle);
                    List<string> accountRoleList = TranslateUtils.StringCollectionToStringList(contactInfo.AccountRole);
                    List<string> mobileList = TranslateUtils.StringCollectionToStringList(contactInfo.Mobile);
                    List<string> telephoneList = TranslateUtils.StringCollectionToStringList(contactInfo.Telephone);
                    List<string> emailList = TranslateUtils.StringCollectionToStringList(contactInfo.Email);
                    List<string> qqList = TranslateUtils.StringCollectionToStringList(contactInfo.QQ);

                    for (int i = 0; i < contactIDList.Count; i++)
                    {
                        ContactInfo contactInfoEdit = contactInfo;
                        contactInfoEdit.ID = contactIDList[i];
                        contactInfoEdit.ContactName = contactNameList[i];
                        contactInfoEdit.JobTitle = jobTitleList[i];
                        contactInfoEdit.AccountRole = accountRoleList[i];
                        contactInfoEdit.Mobile = mobileList[i];
                        contactInfoEdit.Telephone = telephoneList[i];
                        contactInfoEdit.Email = emailList[i];
                        contactInfoEdit.QQ = qqList[i];
                        if (contactInfoEdit.ID > 0)
                        {
                            DataProvider.ContactDAO.Update(contactInfoEdit);
                        }
                        else
                        {
                            DataProvider.ContactDAO.Insert(contactInfoEdit);
                        }
                    }
                }
                else
                {
                    ContactInfo contactInfo = DataProvider.ContactDAO.GetContactInfo(string.Empty, AdminManager.Current.UserName, this.leadInfo.AccountID, this.leadInfo.ID, base.Request.Form);

                    if (StringUtils.Contains(contactInfo.ContactName, ","))
                    {
                        List<string> contactNameList = TranslateUtils.StringCollectionToStringList(contactInfo.ContactName);
                        List<string> jobTitleList = TranslateUtils.StringCollectionToStringList(contactInfo.JobTitle);
                        List<string> accountRoleList = TranslateUtils.StringCollectionToStringList(contactInfo.AccountRole);
                        List<string> mobileList = TranslateUtils.StringCollectionToStringList(contactInfo.Mobile);
                        List<string> telephoneList = TranslateUtils.StringCollectionToStringList(contactInfo.Telephone);
                        List<string> emailList = TranslateUtils.StringCollectionToStringList(contactInfo.Email);
                        List<string> qqList = TranslateUtils.StringCollectionToStringList(contactInfo.QQ);

                        for (int i = 0; i < contactNameList.Count; i++)
                        {
                            ContactInfo contactInfoNew = contactInfo;
                            contactInfoNew.ContactName = contactNameList[i];
                            contactInfoNew.JobTitle = jobTitleList[i];
                            contactInfoNew.AccountRole = accountRoleList[i];
                            contactInfoNew.Mobile = mobileList[i];
                            contactInfoNew.Telephone = telephoneList[i];
                            contactInfoNew.Email = emailList[i];
                            contactInfoNew.QQ = qqList[i];

                            DataProvider.ContactDAO.Insert(contactInfoNew);
                        }
                    }
                    else
                    {
                        DataProvider.ContactDAO.Insert(contactInfo);
                    }
                }
            }
        }

        private int AccountInsert(string location)
        {
            AccountInfo accountInfo = new AccountInfo(base.Request.Form);
            accountInfo.AddUserName = AdminManager.Current.UserName;
            accountInfo.AddDate = DateTime.Now;

            if (!string.IsNullOrEmpty(location) && location.Split(',').Length == 3)
            {
                accountInfo.Province = location.Split(',')[0];
                accountInfo.City = location.Split(',')[1];
                accountInfo.Area = location.Split(',')[2];
            }
            if (accountInfo.Priority > 1) accountInfo.Priority -= 1;

            return DataProvider.AccountDAO.Insert(accountInfo);
        }

        private void AccountUpdate(string location)
        {
            if (this.leadInfo.IsAccount)
            {
                AccountInfo accountInfo = DataProvider.AccountDAO.GetAccountInfo(this.leadInfo.AccountID);
                if (accountInfo != null)
                {
                    AccountInfo accountInfoToEdit = new AccountInfo(base.Request.Form);
                    accountInfo.AddUserName = AdminManager.Current.UserName;
                    accountInfo.AddDate = DateTime.Now;

                    foreach (string attributeName in AccountAttribute.AllAttributes)
                    {
                        accountInfo.SetValue(attributeName, accountInfoToEdit.GetValue(attributeName));
                    }
                    if (!string.IsNullOrEmpty(location) && location.Split(',').Length == 3)
                    {
                        accountInfo.Province = location.Split(',')[0];
                        accountInfo.City = location.Split(',')[1];
                        accountInfo.Area = location.Split(',')[2];
                    }
                    if (accountInfo.Priority > 1) accountInfo.Priority -= 1;

                    DataProvider.AccountDAO.Update(accountInfo);
                }
                else
                {
                    AccountInfo accountInfoToAdd = new AccountInfo(base.Request.Form);
                    accountInfo.AddUserName = AdminManager.Current.UserName;
                    accountInfo.AddDate = DateTime.Now;

                    if (!string.IsNullOrEmpty(location) && location.Split(',').Length == 3)
                    {
                        accountInfoToAdd.Province = location.Split(',')[0];
                        accountInfoToAdd.City = location.Split(',')[1];
                        accountInfoToAdd.Area = location.Split(',')[2];
                    }
                    if (accountInfoToAdd.Priority > 1) accountInfoToAdd.Priority -= 1;

                    int accountID = DataProvider.AccountDAO.Insert(accountInfoToAdd);
                    this.leadInfo.AccountID = accountID;
                }
            }
            else
            {
                this.leadInfo.AccountID = 0;
            }
        }
    }
}
