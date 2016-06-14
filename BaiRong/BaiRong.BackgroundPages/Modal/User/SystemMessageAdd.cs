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
    public class SystemMessageAdd : BackgroundBasePage
    {
        public TextBox tbMessageTitle;
        public BREditor breMessageContent;
        public DropDownList ddlIsViewed;

        public string returnUrl;
        public int systemMessageID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            systemMessageID = base.GetIntQueryString("SystemMessageID");

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_UserMessage, "ϵͳ����", AppManager.User.Permission.Usercenter_Msg);

                EBooleanUtils.AddListItems(this.ddlIsViewed, "ȡ������", "����");

                UserMessageInfo messageInfo = BaiRongDataProvider.UserMessageDAO.GetMessageInfo(systemMessageID);
                if (messageInfo != null)
                {
                    this.tbMessageTitle.Text = messageInfo.Title;
                    this.breMessageContent.Text = messageInfo.Content;
                    ControlUtils.SelectListItems(this.ddlIsViewed, messageInfo.IsViewed.ToString());
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                UserMessageInfo messageInfo = BaiRongDataProvider.UserMessageDAO.GetMessageInfo(systemMessageID);
                if (messageInfo == null)
                    messageInfo = new UserMessageInfo(0, "", "", EUserMessageType.SystemAnnouncement, 0, false, DateTime.Now, "", DateTime.Now, "", "");
                messageInfo.Title = this.tbMessageTitle.Text;
                messageInfo.Content = this.breMessageContent.Text;
                messageInfo.IsViewed = TranslateUtils.ToBool(this.ddlIsViewed.SelectedValue);
                try
                {
                    if (messageInfo.ID > 0)
                    {
                        BaiRongDataProvider.UserMessageDAO.Update(messageInfo);
                        base.SuccessMessage("ϵͳ����༭�ɹ���");
                    }
                    else
                    {
                        BaiRongDataProvider.UserMessageDAO.Insert(messageInfo);
                        base.SuccessMessage("ϵͳ���淢���ɹ���");
                    }

                    JsUtils.OpenWindow.CloseModalPage(this);
                    //if (!string.IsNullOrEmpty(returnUrl))
                    //    base.AddWaitAndRedirectScript(returnUrl);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "ϵͳ���淢��ʧ�ܣ�");
                }
            }
        }

        public static string GetRedirectUrlToAdd(string returnUrl)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("ReturnUrl", returnUrl);
            return JsUtils.OpenWindow.GetOpenWindowString("����ϵͳ����", PageUtils.GetPlatformUrl(string.Format("modal_SystemMessageAdd.aspx")), nvc);
        }

        public static string GetRedirectUrlToEdit(int systemMessageID, string returnUrl)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("ReturnUrl", returnUrl);
            nvc.Add("SystemMessageID", systemMessageID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("�༭ϵͳ����", PageUtils.GetPlatformUrl(string.Format("modal_SystemMessageAdd.aspx")), nvc);
        }

        public static string GetRedirectUrlToView(int systemMessageID, string returnUrl)
        {
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("ReturnUrl", returnUrl);
            nvc.Add("SystemMessageID", systemMessageID.ToString());
            return JsUtils.OpenWindow.GetOpenWindowString("�鿴ϵͳ����", PageUtils.GetPlatformUrl(string.Format("modal_SystemMessageAdd.aspx")), nvc, true);
        }

        public string pageUrl;
        public string PageUrl
        {
            get
            {
                if (string.IsNullOrEmpty(pageUrl))
                {
                    pageUrl = string.Format("modal_SystemMessageAdd.aspx");
                }
                return pageUrl;
            }
        }
    }
}
