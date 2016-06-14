using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;
using BaiRong.Controls;
using BaiRong.BackgroundPages;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace BaiRong.BackgroundPages.Modal
{
    public class UserNoticeTemplateAdd : BackgroundBasePage
    {
        public TextBox tbName;
        public DropDownList ddlUseNoticeType;
        public DropDownList ddlUserNoticeTemplateType;
        public TextBox tbTitle;
        public TextBox tbContent;
        public RadioButtonList rbIsEnable;

        public string returnUrl;
        public int userNoticeTemplateID;

        public Literal ltRules;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            userNoticeTemplateID = base.GetIntQueryString("UserNoticeTemplateID");

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserMessage, "消息模板管理", AppManager.User.Permission.Usercenter_Template);

                EBooleanUtils.AddListItems(this.rbIsEnable, "可用", "不可用");
                ControlUtils.SelectListItemsIgnoreCase(this.rbIsEnable, EBoolean.True.ToString());
                EUserNoticeTypeUtils.AddListItemsToInstall(this.ddlUseNoticeType);
                EUserNoticeTemplateTypeUtils.AddListItemsToInstall(this.ddlUserNoticeTemplateType);

                UserNoticeTemplateInfo noticeTemplateInfo = BaiRongDataProvider.UserNoticeTemplateDAO.GetNoticeTemplateInfo(userNoticeTemplateID);
                if (noticeTemplateInfo != null)
                {
                    this.tbName.Text = noticeTemplateInfo.Name;
                    this.tbTitle.Text = noticeTemplateInfo.Title;
                    this.tbContent.Text = noticeTemplateInfo.Content;
                    ControlUtils.SelectListItems(this.ddlUseNoticeType, EUserNoticeTypeUtils.GetValue(noticeTemplateInfo.RelatedIdentity));
                    ControlUtils.SelectListItems(this.ddlUserNoticeTemplateType, EUserNoticeTemplateTypeUtils.GetValue(noticeTemplateInfo.Type));
                    ControlUtils.SelectListItems(this.rbIsEnable, noticeTemplateInfo.IsEnable.ToString());
                }

                //显示可以替换的字符串选项
                this.ltRules.Text = this.GetRulesString();
            }
        }

        private string GetRulesString()
        {
            string retval = string.Empty;

            StringBuilder builder = new StringBuilder();
            int mod = 0;
            int count = 0;
            IDictionary entitiesDictionary = null;
            entitiesDictionary = UserNoticeSettingManager.TemplateFormatStringArray;

            foreach (string label in entitiesDictionary.Keys)
            {
                count++;
                string labelName = (string)entitiesDictionary[label];
                string td = string.Format(@"<td><a href=""javascript:;"" onclick=""AddOnPos('{0}');return false;"">{0}</a></td><td>{1}</td>", label, labelName);
                if (count == entitiesDictionary.Count)
                {
                    td = string.Format(@"<td><a href=""javascript:;"" onclick=""AddOnPos('{0}');return false;"">{0}</a></td><td colspan=""5"">{1}</td></tr>", label, labelName);
                }
                if (mod++ % 3 == 0)
                {
                    builder.Append("<tr>" + td);
                }
                else
                {
                    builder.Append(td);
                }
            }
            retval = builder.ToString();

            return retval;
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                string errorMessage = string.Empty;

                UserNoticeTemplateInfo noticeTemplateInfo = BaiRongDataProvider.UserNoticeTemplateDAO.GetNoticeTemplateInfo(userNoticeTemplateID);
                if (noticeTemplateInfo == null)
                    noticeTemplateInfo = new UserNoticeTemplateInfo();
                noticeTemplateInfo.Title = this.tbTitle.Text;
                noticeTemplateInfo.Name = this.tbName.Text;
                noticeTemplateInfo.Content = this.tbContent.Text;
                noticeTemplateInfo.IsEnable = TranslateUtils.ToBool(this.rbIsEnable.SelectedValue);
                noticeTemplateInfo.Type = EUserNoticeTemplateTypeUtils.GetEnumType(this.ddlUserNoticeTemplateType.SelectedValue);
                noticeTemplateInfo.RelatedIdentity = EUserNoticeTypeUtils.GetEnumType(this.ddlUseNoticeType.SelectedValue);
                noticeTemplateInfo.RemoteType = ESMSServerTypeUtils.GetEnumType(SMSServerManager.SMSServerInstance.SMSServerEName);
                try
                {
                    if (noticeTemplateInfo.ID > 0)
                    {
                        BaiRongDataProvider.UserNoticeTemplateDAO.Update(noticeTemplateInfo);

                        if (noticeTemplateInfo.Type == EUserNoticeTemplateType.Phone)
                        {
                            //调用模板修改接口
                            string templateID = noticeTemplateInfo.RemoteTemplateID;
                            if (!string.IsNullOrEmpty(templateID))
                                SMSServerManager.UpdateTemplate(templateID, SMSServerManager.GetTemplate(noticeTemplateInfo.Content), out errorMessage);
                            else
                            {
                                SMSServerManager.InsertTemplate(SMSServerManager.GetTemplate(noticeTemplateInfo.Content), out errorMessage, out templateID);
                                noticeTemplateInfo.RemoteTemplateID = templateID;
                                BaiRongDataProvider.UserNoticeTemplateDAO.Update(noticeTemplateInfo);
                            }
                        }

                        base.SuccessMessage("消息模板编辑成功！");
                    }
                    else
                    {
                        noticeTemplateInfo.ID = BaiRongDataProvider.UserNoticeTemplateDAO.Insert(noticeTemplateInfo);

                        if (noticeTemplateInfo.Type == EUserNoticeTemplateType.Phone)
                        {
                            //调用模板添加接口
                            string templateID = string.Empty;
                            SMSServerManager.InsertTemplate(SMSServerManager.GetTemplate(noticeTemplateInfo.Content), out errorMessage, out templateID);
                            noticeTemplateInfo.RemoteTemplateID = templateID;
                            BaiRongDataProvider.UserNoticeTemplateDAO.Update(noticeTemplateInfo);
                        }

                        base.SuccessMessage("消息模板添加成功！");

                    }

                    JsUtils.OpenWindow.CloseModalPage(this);
                    //if (!string.IsNullOrEmpty(returnUrl))
                    //    base.AddWaitAndRedirectScript(returnUrl);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "消息模板操作失败！");
                }
            }
        }

        public static string GetRedirectUrlToAdd(string returnUrl)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("ReturnUrl", returnUrl);
            return JsUtils.OpenWindow.GetOpenWindowString("消息模板添加", PageUtils.GetPlatformUrl(string.Format("modal_UserNoticeTemplateAdd.aspx")), nvc);
        }

        public static string GetRedirectUrlToEdit(int userNoticeTemplateID, string returnUrl)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("ReturnUrl", returnUrl);
            nvc.Add("UserNoticeTemplateID", userNoticeTemplateID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("消息模板编辑", PageUtils.GetPlatformUrl(string.Format("modal_UserNoticeTemplateAdd.aspx")), nvc);
        }

        public string pageUrl;
        public string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(pageUrl))
                {
                    pageUrl = string.Format("modal_UserNoticeTemplateAdd.aspx");
                }
                return pageUrl;
            }
        }
    }
}
