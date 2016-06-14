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
using System.Text;
using SiteServer.STL.Parser;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class SubscribePushAgain : BackgroundBasePage
    {
        public Literal ltlEmail;

        private int publishmentSystemID;
        private string subscribeUserID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.publishmentSystemID = base.GetIntQueryString("PublishmentSystemID");
            this.subscribeUserID = base.GetQueryString("IDsCollection");

            // if (base.GetQueryString("IDsCollection") != null)
            //   push();

            if (!IsPostBack)
            {
                ArrayList alist = DataProvider.SubscribePushRecordDAO.GetSubscribeUserList(base.PublishmentSystemID, TranslateUtils.StringCollectionToArrayList(this.subscribeUserID));
                if (alist.Count > 0)
                {
                    foreach (SubscribePushRecordInfo info in alist)
                    {
                        ltlEmail.Text = ltlEmail.Text + " " + info.Email == "" ? info.Mobile : info.Email + " ";
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            push();
        }

        public void btReturn_OnClick(object sender, EventArgs e)
        {
            PageUtils.Redirect(string.Format("background_subscribePushRecord.aspx?PublishmentSystemID={0} ", this.publishmentSystemID));
        }

        public void push()
        {
            if (this.subscribeUserID != "")
            {
                try
                {
                    //获取订阅设置
                    SubscribeSetInfo sinfo = DataProvider.SubscribeSetDAO.GetSubscribeSetInfo(base.PublishmentSystemID);

                    if (string.IsNullOrEmpty(sinfo.EmailContentAddress))
                    {
                        base.FailMessage("订阅信息邮件发送失败：邮件模板为空，请在【其他设置】中选择邮件模板");
                        return;
                    }

                    ArrayList infoList = DataProvider.SubscribePushRecordDAO.GetSubscribeUserList(base.PublishmentSystemID, TranslateUtils.StringCollectionToArrayList(this.subscribeUserID));

                    string strBody = "";
                    bool isSuccess = false;
                    foreach (SubscribePushRecordInfo info in infoList)
                    {
                        //推送邮件
                        if (!string.IsNullOrEmpty(info.Email) && EBooleanUtils.Equals(EBoolean.False, info.PushStatu))
                        {
                            ISmtpMail smtpMail = MailUtils.GetInstance();
                            smtpMail.AddRecipient(info.Email);

                            smtpMail.MailDomainPort = ConfigManager.Additional.MailDomainPort;
                            smtpMail.IsHtml = true;
                            smtpMail.Subject = "【" + base.PublishmentSystemInfo.PublishmentSystemName + "】" + "信息订阅";

                            string cvalue = "";
                            string lvalue = "";
                            //获取订阅内容
                            SubscribeUserInfo uinfo = DataProvider.SubscribeUserDAO.GetContentInfo(base.PublishmentSystemID, info.Email);
                            if (uinfo == null)
                            {
                                base.FailMessage("订阅信息邮件发送失败：【" + info.Email + "】该订阅的邮件记录已经不存在");
                                return;
                            }
                            if(string.IsNullOrEmpty( info.SubscribeName))
                            {
                                base.FailMessage("订阅信息邮件发送失败：【" + info.Email + "】该订阅的邮件记录订阅内容为空已经不存在");
                                return;
                            }
                            DataProvider.SubscribeDAO.GetValueByUserID(base.PublishmentSystemID, uinfo.SubscribeName, out cvalue, out lvalue);

                            //获取邮件模板的静态代码 
                            try
                            {
                                //TemplateInfo templateInfo = DataProvider.TemplateDAO.GetTemplateByURLType(base.PublishmentSystemID, ETemplateType.FileTemplate, sinfo.EmailContentAddress);
                                TemplateInfo templateInfo = DataProvider.TemplateDAO.GetTemplateByTemplateID(base.PublishmentSystemID, ETemplateType.FileTemplate, sinfo.EmailContentAddress);

                                if (templateInfo == null || templateInfo.TemplateType != ETemplateType.FileTemplate)
                                {
                                    base.FailMessage("订阅信息邮件发送失败：模板不存在");
                                    return;
                                }
                                StringBuilder contentBuilder = new StringBuilder(CreateCacheManager.FileContent.GetTemplateContent(base.PublishmentSystemInfo, templateInfo));

                                NameValueCollection queryString = new NameValueCollection();
                                queryString.Remove("publishmentSystemID");
                                queryString.Add("channelIndex", TranslateUtils.ObjectCollectionToSqlInStringWithQuote(TranslateUtils.StringCollectionToArrayList(cvalue)));
                                queryString.Add("tags", lvalue);

                                strBody = StlUtility.ParseDynamicContent(publishmentSystemID, 0, 0, templateInfo.TemplateID, false, contentBuilder.ToString(), sinfo.EmailContentAddress, 1, "", queryString);
                            }
                            catch (Exception ex)
                            {
                                base.FailMessage("订阅信息邮件发送失败：" + ex);
                                return;
                            }
                            smtpMail.Body = strBody;
                            smtpMail.MailDomain = ConfigManager.Additional.MailDomain;
                            smtpMail.MailServerUserName = ConfigManager.Additional.MailServerUserName;
                            smtpMail.MailServerPassword = ConfigManager.Additional.MailServerPassword;

                            //开始发送
                            string errorMessage = string.Empty;
                            isSuccess = smtpMail.Send(out errorMessage);
                            if (isSuccess)
                            {
                                //修改会员信息推送次数 
                                DataProvider.SubscribeUserDAO.UpdatePushNumByEmail(base.PublishmentSystemID, info.Email);

                                //修改当前记录的推送状态 
                                info.PushStatu = EBoolean.True;
                                DataProvider.SubscribePushRecordDAO.Update(info);

                                StringUtility.AddLog(base.PublishmentSystemID, "订阅信息手动推送邮件:", string.Format("接收邮件:{0},邮件内容：{1}", info.Email, strBody));
                                base.SuccessMessage("订阅信息邮件发送成功！");
                            }
                            else
                            {
                                base.FailMessage("订阅信息邮件发送失败：" + errorMessage);
                                return;
                            }

                            //记录邮件发送状态
                            SubscribePushRecordInfo srinfo = new SubscribePushRecordInfo()
                            {
                                Email = info.Email,
                                PushType = ESubscribePushType.ManualPush,
                                SubscribeName = DataProvider.SubscribeDAO.GetName(base.PublishmentSystemID, uinfo.SubscribeName),
                                SubscriptionTemplate = "订阅信息邮件发送手动推送",
                                PublishmentSystemID = base.PublishmentSystemID,
                                PushStatu = isSuccess ? EBoolean.True : EBoolean.False,
                                UserName = BaiRongDataProvider.AdministratorDAO.UserName
                            };
                            DataProvider.SubscribePushRecordDAO.Insert(srinfo);

                            PageUtils.Redirect(string.Format("background_subscribePushRecord.aspx?PublishmentSystemID={0}", this.publishmentSystemID));
                        }

                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "订阅信息再次推送失败：" + ex.Message);
                }
            }
            else
                base.FailMessage("订阅信息再次推送失败:没有邮件或手机需要推送！");


        }
    }
}
