using System;
using System.Collections;
using System.Web.UI.WebControls;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Specialized;
using BaiRong.Controls;
using System.Web.UI.HtmlControls;

namespace BaiRong.BackgroundPages
{
    public class BackgroundUserAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;
        public TextBox tbUserName;
        public TextBox tbDisplayName;
        public PlaceHolder phPassword;
        public TextBox tbPassword;
        public TextBox tbEmail;
        public TextBox tbMobile;

        //public TextBox tbAddress;
        public HtmlInputHidden provinceValue;
        public Button btnReturn;

        public UserAuxiliaryControl uacAttributes;
        private int userID;
        private string returnUrl;

        public static string GetRedirectUrlToAdd(string returnUrl)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_userAdd.aspx?returnUrl={0}", StringUtils.ValueToUrl(returnUrl)));
        }

        public static string GetRedirectUrlToEdit(int userID, string returnUrl)
        {
            return PageUtils.GetPlatformUrl(string.Format("background_userAdd.aspx?userID={0}&returnUrl={1}", userID, StringUtils.ValueToUrl(returnUrl)));
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (base.IsForbidden) return;

            this.userID = TranslateUtils.ToInt(base.GetQueryString("userID"));
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("returnUrl"));

            if (!IsPostBack)
            {
                string pageTitle = this.userID == 0 ? "添加用户" : "编辑用户";
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_User, pageTitle, AppManager.User.Permission.Usercenter_User);

                this.ltlPageTitle.Text = pageTitle;
                UserInfo userInfo;
                if (this.userID > 0)
                {
                    userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(this.userID);
                    //UserContactInfo userContactInfo = BaiRongDataProvider.UserContactDAO.GetContactInfo(userInfo.UserName);
                    if (userInfo != null)
                    {
                        this.tbUserName.Text = userInfo.UserName;
                        this.tbUserName.Enabled = false;
                        this.tbDisplayName.Text = userInfo.DisplayName;
                        this.phPassword.Visible = false;
                        this.tbEmail.Text = userInfo.Email;
                        this.tbMobile.Text = userInfo.Mobile;
                        this.provinceValue.Value = userInfo.Location;

                    }
                    //if (userContactInfo != null)
                    //{
                    //    ControlUtils.SelectListItems(this.ddlGender, userContactInfo.Gender);

                    //    this.tbAddress.Text = userContactInfo.Address;
                    //    this.provinceValue.Value = userContactInfo.Position;
                    //}
                }
                else
                {
                    userInfo = new UserInfo();
                }
                this.uacAttributes.SetParameters(userInfo.Attributes, BaiRongDataProvider.UserDAO.TABLE_NAME, true, base.IsPostBack);

                if (!string.IsNullOrEmpty(this.returnUrl))
                {
                    this.btnReturn.Attributes.Add("onclick", string.Format("window.location.href='{0}';return false;", this.returnUrl));
                }
                else
                {
                    this.btnReturn.Visible = false;
                }
            }
            else
            {
                this.uacAttributes.SetParameters(base.Request.Form, BaiRongDataProvider.UserDAO.TABLE_NAME, true, base.IsPostBack);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                string errorMessage = string.Empty;

                if (userID == 0)
                {
                    if (string.IsNullOrEmpty(provinceValue.Value))
                    {
                        FailMessage("请选择所在的区域！");
                        return;
                    }

                    UserInfo userInfo = new UserInfo();
                    //UserContactInfo userContactInfo = new UserContactInfo();

                    userInfo.GroupSN = string.Empty;
                    userInfo.UserName = this.tbUserName.Text;
                    userInfo.Password = this.tbPassword.Text;
                    userInfo.CreateDate = DateTime.Now;
                    userInfo.CreateIPAddress = PageUtils.GetIPAddress();
                    userInfo.LastActivityDate = DateUtils.SqlMinValue;
                    userInfo.IsChecked = true;
                    userInfo.IsLockedOut = false;
                    userInfo.DisplayName = this.tbDisplayName.Text;
                    userInfo.Email = this.tbEmail.Text;
                    userInfo.Mobile = this.tbMobile.Text;
                    userInfo.Location = provinceValue.Value;//所在地

                    //userContactInfo.RelatedUserName = this.tbUserName.Text;
                    //userContactInfo.Gender = this.ddlGender.SelectedValue;
                    //userContactInfo.Position = provinceValue.Value;
                    //userContactInfo.Address = this.tbAddress.Text;

                    TableInputParser.AddValuesToAttributes(ETableStyle.User, BaiRongDataProvider.UserDAO.TABLE_NAME, null, base.Request.Form, userInfo.Attributes);

                    bool isCreated = BaiRongDataProvider.UserDAO.Insert(userInfo, out errorMessage);

                    if (isCreated)
                    {
                        //BaiRongDataProvider.UserContactDAO.Insert(userContactInfo);
                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加用户", string.Format("用户:{0}", this.tbUserName.Text));

                        base.SuccessMessage("用户添加成功，可以继续添加！");
                        base.AddWaitAndRedirectScript(BackgroundUserAdd.GetRedirectUrlToAdd(this.returnUrl));
                    }
                    else
                    {
                        base.FailMessage(string.Format("用户添加失败：<br>{0}", errorMessage));
                    }
                }
                else
                {
                    UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(this.userID);

                    //UserContactInfo userContactInfo = BaiRongDataProvider.UserContactDAO.GetContactInfo(userInfo.UserName);

                    userInfo.DisplayName = this.tbDisplayName.Text;
                    userInfo.Email = this.tbEmail.Text;
                    userInfo.Mobile = this.tbMobile.Text;
                    userInfo.Location = provinceValue.Value;//所在地

                    //if (userContactInfo == null)
                    //{
                    //    userContactInfo = new UserContactInfo();
                    //}

                    //userContactInfo.Gender = this.ddlGender.SelectedValue;
                    //if (!string.IsNullOrEmpty(provinceValue.Value))
                    //{
                    //    userContactInfo.Position = provinceValue.Value;
                    //}
                    //userContactInfo.Address = this.tbAddress.Text;

                    TableInputParser.AddValuesToAttributes(ETableStyle.User, BaiRongDataProvider.UserDAO.TABLE_NAME, null, base.Request.Form, userInfo.Attributes);

                    try
                    {
                        BaiRongDataProvider.UserDAO.Update(userInfo);
                        //if (userContactInfo.ID > 0)
                        //{
                        //    BaiRongDataProvider.UserContactDAO.Update(userContactInfo);
                        //}
                        //else
                        //{
                        //    BaiRongDataProvider.UserContactDAO.Insert(userContactInfo);
                        //}

                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改用户", string.Format("用户:{0}", this.tbUserName.Text));

                        base.SuccessMessage("用户修改成功！");
                        base.AddWaitAndRedirectScript(this.returnUrl);
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, string.Format("用户修改失败：{0}", ex.Message));
                    }
                }
            }
        }
    }
}