using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Net;
using BaiRong.Model;
using System.Collections.Generic;
using BaiRong.BackgroundPages.Modal;



namespace BaiRong.BackgroundPages
{
    public class BackgroundUserNoticeSetting : BackgroundBasePage
    {

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserMessage, "��Ϣģ��ƥ��", AppManager.User.Permission.Usercenter_Setting);
            }
        }

        public string GetNoticeSetting(EUserNoticeType type)
        {
            string retval = string.Empty;
            UserNoticeSettingInfo info = UserNoticeSettingManager.GetUserNoticeSettingInfo(EUserNoticeTypeUtils.GetValue(type));

            retval += string.Format("<td id='td_{2}'>{1}{0}({3})</td>", EUserNoticeTypeUtils.GetText(EUserNoticeTypeUtils.GetEnumType(info.UserNoticeType)), info.IsRequired ? "<span style='color:red;padding:3px;'>*</span>" : "", info.UserNoticeType, info.IsSignal ? "��ѡ" : "��ѡ");

            if (Array.IndexOf(info.Allow, "phone") >= 0)
                retval += string.Format("<td class='center'><input type='checkbox' value='{0}' name='{0}_phone' {1}/>&nbsp;&nbsp;<a href='#' onclick=\"{2}\">����ģ��</a></td>", EUserNoticeTypeUtils.GetValue(EUserNoticeTypeUtils.GetEnumType(info.UserNoticeType)), info.ByPhone ? "checked='checked'" : "", NoticeTemplateEdit.GetOpenWindowStringToEdit(EUserNoticeTypeUtils.GetValue(type), NoticeTemplateEdit.PhoneTemplate, ""));
            else
                retval += "<td class='center'><a style='color:gray;'>����ģ��</a></td>";

            if (Array.IndexOf(info.Allow, "email") >= 0)
                retval += string.Format("<td class='center'><input type='checkbox' value='{0}' name='{0}_email' {1}/>&nbsp;&nbsp;<a href='#' onclick=\"{2}\">����ģ��</a></td>", EUserNoticeTypeUtils.GetValue(EUserNoticeTypeUtils.GetEnumType(info.UserNoticeType)), info.ByEmail ? "checked='checked'" : "", NoticeTemplateEdit.GetOpenWindowStringToEdit(EUserNoticeTypeUtils.GetValue(type), NoticeTemplateEdit.EmailTemplate, ""));
            else
                retval += "<td class='center'><a style='color:gray;'>����ģ��</a></td>";

            if (Array.IndexOf(info.Allow, "message") >= 0)
                retval += string.Format("<td class='center'><input type='checkbox' value='{0}' name='{0}_message' {1}/>&nbsp;&nbsp;<a href='#' onclick=\"{2}\">����ģ��</a></td>", EUserNoticeTypeUtils.GetValue(EUserNoticeTypeUtils.GetEnumType(info.UserNoticeType)), info.ByMessage ? "checked='checked'" : "", NoticeTemplateEdit.GetOpenWindowStringToEdit(EUserNoticeTypeUtils.GetValue(type), NoticeTemplateEdit.MessageTemplate, ""));
            else
                retval += "<td class='center'><a style='color:gray;'>����ģ��</a></td>";

            return retval;
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    List<string> noticeTypeList = EUserNoticeTypeUtils.GetAllVaules();
                    Dictionary<string, string> dicError = new Dictionary<string, string>();
                    foreach (string noticeType in noticeTypeList)
                    {
                        UserNoticeSettingInfo info = UserNoticeSettingManager.GetUserNoticeSettingInfo(noticeType);
                        if (info != null)
                        {
                            bool byEmail = base.Request.Form[noticeType + "_email"] == noticeType;
                            bool byPhone = base.Request.Form[noticeType + "_phone"] == noticeType;
                            bool byMessage = base.Request.Form[noticeType + "_message"] == noticeType;
                            if (info.IsRequired && !(byEmail || byPhone || byMessage))
                            {
                                base.FailMessage("�����޸�ʧ�ܣ�ע���*������ѡ��һ��");
                                base.AddScript(string.Format("IsRequiredNotice('{0}');", noticeType));
                                if (!dicError.ContainsKey(EUserNoticeTypeUtils.GetText(EUserNoticeTypeUtils.GetEnumType(noticeType))))
                                {
                                    dicError.Add(EUserNoticeTypeUtils.GetText(EUserNoticeTypeUtils.GetEnumType(noticeType)), "ע���*������ѡ��һ��");
                                }
                                continue;
                            }

                            if (info.IsSignal)
                            {
                                int count = 0;
                                if (byEmail) count++;
                                if (byPhone) count++;
                                if (byMessage) count++;
                                if (count > 1)
                                {
                                    base.FailMessage("�����޸�ʧ�ܣ�ע�ⵥѡ��ֻ��ѡ��һ��");
                                    base.AddScript(string.Format("IsRequiredNotice('{0}');", noticeType));
                                    if (!dicError.ContainsKey(EUserNoticeTypeUtils.GetText(EUserNoticeTypeUtils.GetEnumType(noticeType))))
                                    {
                                        dicError.Add(EUserNoticeTypeUtils.GetText(EUserNoticeTypeUtils.GetEnumType(noticeType)), "ע�ⵥѡ��ֻ��ѡ��һ��");
                                    }
                                    continue;
                                }
                            }

                            if (info.ByEmail != byEmail || info.ByPhone != byPhone || info.ByMessage != byMessage)
                            {
                                info.ByEmail = byEmail;
                                info.ByPhone = byPhone;
                                info.ByMessage = byMessage;
                                UserNoticeSettingManager.SetUserNoticeSettingInfo(info);
                            }
                        }
                    }
                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "�޸���Ϣ��������");
                    if (dicError.Keys.Count == 0)
                    {
                        base.SuccessMessage("�����޸ĳɹ���");
                    }
                    else
                    {
                        string errorMsg = string.Empty;
                        foreach (string key in dicError.Keys)
                        {
                            errorMsg += "[" + key + "] " + dicError[key];
                        }
                        base.FailMessage(string.Format("�����޸�ʧ�ܣ�<br />ʧ����Ϣ��{0}", errorMsg));
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "�����޸�ʧ�ܣ�");
                }
            }
        }
    }
}
