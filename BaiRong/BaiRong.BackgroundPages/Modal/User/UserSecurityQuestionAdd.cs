using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;
using BaiRong.Controls;
using BaiRong.BackgroundPages;
using System.Collections.Specialized;

namespace BaiRong.BackgroundPages.Modal
{
    public class UserSecurityQuestionAdd : BackgroundBasePage
    {
        public TextBox tbQuestion;
        public RadioButtonList rbIsEnable;

        public string returnUrl;
        public int userSecurityQuestionID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            userSecurityQuestionID = base.GetIntQueryString("UserSecurityQuestionID");

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserMessage, "密保问题管理", AppManager.User.Permission.Usercenter_Setting);

                EBooleanUtils.AddListItems(this.rbIsEnable, "可用", "不可用");
                ControlUtils.SelectListItemsIgnoreCase(this.rbIsEnable, EBoolean.True.ToString());

                UserSecurityQuestionInfo securityQuestionInfo = BaiRongDataProvider.UserSecurityQuestionDAO.GetSecurityQuestionInfo(userSecurityQuestionID);
                if (securityQuestionInfo != null)
                {
                    this.tbQuestion.Text = securityQuestionInfo.Question;
                    ControlUtils.SelectListItems(this.rbIsEnable, securityQuestionInfo.IsEnable.ToString());
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                UserSecurityQuestionInfo securityQuestionInfo = BaiRongDataProvider.UserSecurityQuestionDAO.GetSecurityQuestionInfo(userSecurityQuestionID);
                if (securityQuestionInfo == null)
                    securityQuestionInfo = new UserSecurityQuestionInfo();
                securityQuestionInfo.Question = this.tbQuestion.Text;
                securityQuestionInfo.IsEnable = TranslateUtils.ToBool(this.rbIsEnable.SelectedValue);
                

                try
                {
                    if (securityQuestionInfo.ID > 0)
                    {
                        BaiRongDataProvider.UserSecurityQuestionDAO.Update(securityQuestionInfo);
                        base.SuccessMessage("密保问题编辑成功！");
                    }
                    else
                    {
                        BaiRongDataProvider.UserSecurityQuestionDAO.Insert(securityQuestionInfo);
                        base.SuccessMessage("密保问题添加成功！");
                    }

                    JsUtils.OpenWindow.CloseModalPage(this);
                    //if (!string.IsNullOrEmpty(returnUrl))
                    //    base.AddWaitAndRedirectScript(returnUrl);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "密保问题操作失败！");
                }
            }
        }

        public static string GetRedirectUrlToAdd(string returnUrl)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("ReturnUrl", returnUrl);
            return JsUtils.OpenWindow.GetOpenWindowString("密保问题添加", PageUtils.GetPlatformUrl(string.Format("modal_UserSecurityQuestionAdd.aspx")), nvc);
        }

        public static string GetRedirectUrlToEdit(int userSecurityQuestionID, string returnUrl)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("ReturnUrl", returnUrl);
            nvc.Add("UserSecurityQuestionID", userSecurityQuestionID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("密保问题编辑", PageUtils.GetPlatformUrl(string.Format("modal_UserSecurityQuestionAdd.aspx")), nvc);
        }

        public string pageUrl;
        public string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(pageUrl))
                {
                    pageUrl = string.Format("modal_UserSecurityQuestionAdd.aspx");
                }
                return pageUrl;
            }
        }
    }
}
