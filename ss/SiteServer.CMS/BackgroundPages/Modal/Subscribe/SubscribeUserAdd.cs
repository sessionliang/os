using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Collections.Specialized;
using System.Collections.Generic;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using BaiRong.Core.Net;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class SubscribeUserAdd : BackgroundBasePage
    {
        protected TextBox tbEmail;
        protected TextBox tbMobile;
        protected PlaceHolder phMobile;
        public RadioButtonList reMobile;
        public CheckBoxList cbSubscribe;

        private int publishmentSystemID;
        private int subscribeUserID;
        private int subscribeID;

        public static string GetOpenWindowStringToAdd(int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("添加会员订阅信息", "modal_subscribeUserAdd.aspx", arguments, 580, 500);
        }
        public static string GetOpenWindowStringToAdd(int publishmentSystemID, int subscribeID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("SubscribeID", subscribeID.ToString());
            return PageUtility.GetOpenWindowString("添加会员订阅信息", "modal_subscribeUserAdd.aspx", arguments, 580, 500);
        }

        public static string GetOpenWindowStringToEdit(int publishmentSystemID, int subscribeUserID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("SubscribeUserID", subscribeUserID.ToString());
            return PageUtility.GetOpenWindowString("修改会员订阅信息", "modal_subscribeUserAdd.aspx", arguments, 580, 500);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.publishmentSystemID = base.GetIntQueryString("PublishmentSystemID");
            this.subscribeUserID = base.GetIntQueryString("SubscribeUserID");
            this.subscribeID = base.GetIntQueryString("SubscribeID");
            SubscribeInfo subinfo = DataProvider.SubscribeDAO.GetContentInfo(base.PublishmentSystemID, this.subscribeID);
            SubscribeInfo pinfo = DataProvider.SubscribeDAO.GetDefaultInfo(base.PublishmentSystemID);

            if (!IsPostBack)
            {
                ArrayList alist = DataProvider.SubscribeDAO.GetInfoList(base.PublishmentSystemID, " and ParentID != 0 ");
                if (alist.Count > 0)
                {
                    foreach (SubscribeInfo info in alist)
                    {
                        ListItem listItem = new ListItem(info.ItemName, info.ItemID.ToString());
                        cbSubscribe.Items.Add(listItem);
                    }
                }
                if (this.subscribeUserID > 0)
                {
                    SubscribeUserInfo info = DataProvider.SubscribeUserDAO.GetContentInfo(subscribeUserID);
                    this.tbEmail.Text = info.Email;
                    this.tbEmail.Enabled = true;
                    if (!string.IsNullOrEmpty(info.Mobile))
                    {
                        this.phMobile.Visible = true;
                        this.tbMobile.Text = info.Mobile;
                    }
                    else
                    {
                        this.phMobile.Visible = false;
                        this.tbMobile.Text = "";
                    }

                    string[] subscribeName = info.SubscribeName.Split(',');
                    if (subscribeName.Length > 0)
                    {
                        foreach (string str in subscribeName)
                        {
                            foreach (ListItem item in cbSubscribe.Items)
                            {
                                if (str == item.Value)
                                    item.Selected = true;
                            }
                        }
                    }
                }
                else
                {
                    if (subinfo.ItemID == pinfo.ItemID)
                    {
                        foreach (ListItem item in cbSubscribe.Items)
                        {
                            item.Selected = true;
                        }
                    }
                    else
                        this.cbSubscribe.SelectedValue = this.subscribeID.ToString();
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            if (this.subscribeUserID > 0)
            {
                try
                {
                    string selSubscribe = "";
                    string selSubscribeID = "";
                    foreach (ListItem item in cbSubscribe.Items)
                    {
                        if (item.Selected)
                        {
                            selSubscribe = selSubscribe + item.Text + ",";
                            selSubscribeID = selSubscribeID + item.Value + ",";
                        }
                    }
                    string subscribeID = "";
                    if (selSubscribeID.Length > 0)
                    {
                        selSubscribe = selSubscribe.Substring(0, selSubscribe.Length - 1);
                        selSubscribeID = selSubscribeID.Substring(0, selSubscribeID.Length - 1);
                    }
                    else
                    {
                        base.FailMessage("修改会员订阅信息失败，请选择订阅内容！");
                        return;
                    }

                    if (this.reMobile.SelectedValue == "1" && string.IsNullOrEmpty(this.tbMobile.Text))
                    {
                        base.FailMessage("修改会员订阅信息失败，请输入手机号！");
                        return;
                    }

                    SubscribeUserInfo subscribeUserInfo = DataProvider.SubscribeUserDAO.GetContentInfo(this.subscribeUserID);
                    subscribeID = subscribeUserInfo.SubscribeName;
                    subscribeUserInfo.Email = PageUtils.FilterXSS(this.tbEmail.Text.Trim()); subscribeUserInfo.Mobile = this.reMobile.SelectedValue == "1" ? PageUtils.FilterXSS(this.tbMobile.Text.Trim()) : "";
                    subscribeUserInfo.SubscribeStatu = EBoolean.True;
                    subscribeUserInfo.SubscribeName = selSubscribeID;
                    subscribeUserInfo.PublishmentSystemID = this.publishmentSystemID;
                    DataProvider.SubscribeUserDAO.Update(subscribeUserInfo, subscribeID);
                    
                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "修改会员订阅信息失败:" + ex);
                }
            }
            else
            {
                if (DataProvider.SubscribeUserDAO.IsExists(this.tbEmail.Text))
                {
                    base.FailMessage("添加会员订阅信息失败，会员订阅信息已存在！");
                }
                else
                {
                    try
                    {
                        string selSubscribe = "";
                        string selSubscribeID = "";
                        foreach (ListItem item in cbSubscribe.Items)
                        {
                            if (item.Selected)
                            {
                                selSubscribe = selSubscribe + item.Text + ",";
                                selSubscribeID = selSubscribeID + item.Value + ",";
                            }
                        }

                        if (selSubscribeID.Length > 0)
                        {
                            selSubscribe = selSubscribe.Substring(0, selSubscribe.Length - 1);
                            selSubscribeID = selSubscribeID.Substring(0, selSubscribeID.Length - 1);
                        }
                        else
                        {
                            base.FailMessage("添加会员订阅信息失败，请选择订阅内容！");
                            return;
                        }

                        if (this.reMobile.SelectedValue == "1" && string.IsNullOrEmpty(this.tbMobile.Text))
                        {
                            base.FailMessage("添加会员订阅信息失败，请输入手机号！");
                            return;
                        }

                        SubscribeUserInfo subscribeUserInfo = new SubscribeUserInfo();
                        subscribeUserInfo.Email = PageUtils.FilterXSS(this.tbEmail.Text.Trim());
                        subscribeUserInfo.Mobile = this.reMobile.SelectedValue == "1" ? PageUtils.FilterXSS(this.tbMobile.Text.Trim()) : "";
                        subscribeUserInfo.SubscribeStatu = EBoolean.True;
                        subscribeUserInfo.SubscribeName = selSubscribeID;
                        subscribeUserInfo.PublishmentSystemID = this.publishmentSystemID;
                        DataProvider.SubscribeUserDAO.Insert(subscribeUserInfo);
                        //统计内容下的数量
                        DataProvider.SubscribeDAO.UpdateSubscribeNum(this.publishmentSystemID, selSubscribeID, 1);

                        isChanged = true;
                        sendText(subscribeUserInfo, "add");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "添加会员订阅信息失败！");
                    }
                }
            }
            //统计顶级的数量
            SubscribeInfo sinfo = DataProvider.SubscribeDAO.GetDefaultInfo(this.publishmentSystemID);
            DataProvider.SubscribeDAO.UpdateContentNum(this.publishmentSystemID, sinfo.ItemID, DataProvider.SubscribeUserDAO.GetCount(this.publishmentSystemID, string.Empty));

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }


        public void reMobile_OnSelectedIndexChanged(object sender, EventArgs E)
        {
            if (this.reMobile.SelectedValue == "1")
            {
                this.phMobile.Visible = true;
            }
            else
            {
                this.phMobile.Visible = false;
            }
        }


        public void sendText(SubscribeUserInfo info, string type)
        {

            string strBody = "【" + base.PublishmentSystemInfo.PublishmentSystemName + "】" + "您的信息订阅成功";
            bool isSuccess = true;
            if (type == "add")
            {
                //发送订阅成功邮件
                try
                {
                    ISmtpMail smtpMail = MailUtils.GetInstance();
                    string[] usernames = info.Email.Split(new char[] { ',' });
                    smtpMail.AddRecipient(usernames);

                    smtpMail.MailDomainPort = ConfigManager.Additional.MailDomainPort;
                    smtpMail.IsHtml = true;
                    smtpMail.Subject = "【" + base.PublishmentSystemInfo.PublishmentSystemName + "】" + "信息订阅";


                    smtpMail.Body = "<pre style=\"width:100%;word-wrap:break-word\">" + strBody + "</pre>";
                    smtpMail.MailDomain = ConfigManager.Additional.MailDomain;
                    smtpMail.MailServerUserName = ConfigManager.Additional.MailServerUserName;
                    smtpMail.MailServerPassword = ConfigManager.Additional.MailServerPassword;

                    //开始发送
                    string errorMessage = string.Empty;
                    isSuccess = smtpMail.Send(out errorMessage);
                    if (isSuccess)
                    {
                        StringUtility.AddLog(base.PublishmentSystemID, "订阅信息成功发送邮件:", string.Format("接收邮件:{0},邮件内容：{1}", info.Email, strBody));
                        base.SuccessMessage("订阅信息邮件发送成功！");
                    }
                    else
                    {
                        base.FailMessage("订阅信息邮件发送失败：" + errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "订阅信息邮件发送失败：" + ex.Message);
                }

                //记录邮件发送状态
                SubscribePushRecordInfo srinfo = new SubscribePushRecordInfo()
                {
                    Email = info.Email,
                    PushType = ESubscribePushType.ManualPush,
                    SubscribeName = DataProvider.SubscribeDAO.GetName(base.PublishmentSystemID, info.SubscribeName),
                    SubscriptionTemplate = "订阅信息成功发送邮件",
                    PublishmentSystemID = base.PublishmentSystemID,
                    PushStatu = isSuccess ? EBoolean.True : EBoolean.False,
                    UserName = BaiRongDataProvider.AdministratorDAO.UserName
                };
                DataProvider.SubscribePushRecordDAO.Insert(srinfo);


                //发送手机提醒
                if (!string.IsNullOrEmpty(info.Mobile))
                {
                    ArrayList mobileArrayList = new ArrayList();
                    mobileArrayList.Add(info.Mobile);

                    if (mobileArrayList.Count > 0)
                    {
                        try
                        {
                            string errorMessage = string.Empty;
                            isSuccess = SMSServerManager.Send(mobileArrayList, strBody, out errorMessage);

                            if (isSuccess)
                            {
                                StringUtility.AddLog(base.PublishmentSystemID, "订阅信息发送短信", string.Format("接收号码:{0},短信内容：{1}", TranslateUtils.ObjectCollectionToString(mobileArrayList), strBody));

                                base.SuccessMessage("订阅信息短信发送成功！");
                            }
                            else
                            {
                                base.FailMessage("订阅信息短信发送失败：" + errorMessage + "! 请检查短信服务商设置！");
                            }
                        }
                        catch (Exception ex)
                        {
                            base.FailMessage(ex, "订阅信息短信发送失败：" + ex.Message + " 请与管理员联系！");
                        }
                    }

                    //记录邮件发送状态
                    srinfo = new SubscribePushRecordInfo()
                    {
                        Mobile = info.Mobile,
                        PushType = ESubscribePushType.ManualPush,
                        SubscribeName = DataProvider.SubscribeDAO.GetName(base.PublishmentSystemID, info.SubscribeName),
                        SubscriptionTemplate = "订阅信息成功发送手机提醒",
                        PublishmentSystemID = base.PublishmentSystemID,
                        PushStatu = isSuccess ? EBoolean.True : EBoolean.False,
                        UserName = BaiRongDataProvider.AdministratorDAO.UserName
                    };
                    DataProvider.SubscribePushRecordDAO.Insert(srinfo);
                }


            }
        }
    }
}
