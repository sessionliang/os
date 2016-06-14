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
    public class SubscribePush : BackgroundBasePage
    {
        public Literal ltlEmail;

        private int publishmentSystemID;
        private string subscribeUserID;
        private int nodeID;

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.publishmentSystemID = base.GetIntQueryString("PublishmentSystemID");
            this.subscribeUserID = base.GetQueryString("IDsCollection");
            this.nodeID = base.GetIntQueryString("ItemID");

            // if (base.GetQueryString("IDsCollection") != null)
            //   push();

            if (!IsPostBack)
            {
                ArrayList alist = DataProvider.SubscribeUserDAO.GetSubscribeUserList(base.PublishmentSystemID, TranslateUtils.StringCollectionToArrayList(this.subscribeUserID));
                if (alist.Count > 0)
                {
                    foreach (SubscribeUserInfo info in alist)
                    {
                        ltlEmail.Text = ltlEmail.Text + " " + info.Email + " ";
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
            PageUtils.Redirect(string.Format("background_subscribeUser.aspx?PublishmentSystemID={0}&ItemID={1}", this.publishmentSystemID, this.nodeID));
        }

        public void push()
        {
            bool result = true;
            string ft = "";
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

                    SubscribeInfo subinfo = DataProvider.SubscribeDAO.GetContentInfo(base.PublishmentSystemID, this.nodeID);
                    SubscribeInfo pinfo = DataProvider.SubscribeDAO.GetDefaultInfo(base.PublishmentSystemID);

                    ArrayList infoList = DataProvider.SubscribeUserDAO.GetSubscribeUserList(base.PublishmentSystemID, TranslateUtils.StringCollectionToArrayList(this.subscribeUserID), string.Format(" and SubscribeStatu='{0}'", EBoolean.True));

                    if (infoList.Count == 0)
                    {
                        result = false;
                        base.FailMessage("订阅信息邮件发送失败：没有内容需要推送！");
                        return;
                    }
                    foreach (SubscribeUserInfo info in infoList)
                    {
                        if (!string.IsNullOrEmpty(info.SubscribeName))
                        {
                            #region 发送邮件
                            ISmtpMail smtpMail = MailUtils.GetInstance();
                            smtpMail.AddRecipient(info.Email);

                            smtpMail.MailDomainPort = ConfigManager.Additional.MailDomainPort;
                            smtpMail.IsHtml = true;
                            smtpMail.Subject = "【" + base.PublishmentSystemInfo.PublishmentSystemName + "】" + "信息订阅";

                            string strBody = "";

                            string cvalue = "";
                            string lvalue = "";

                            //如果是所有内容下进行手动推送，则推送所订阅的内容，如果某一个内容下手动推送，则只推送当前内容
                            if (subinfo.ItemID == pinfo.ItemID)
                                DataProvider.SubscribeDAO.GetValueByUserID(base.PublishmentSystemID, info.SubscribeName, out cvalue, out lvalue);
                            else
                                DataProvider.SubscribeDAO.GetValueByUserID(base.PublishmentSystemID, this.nodeID.ToString(), out cvalue, out lvalue);

                            //获取邮件模板的静态代码 
                            try
                            {
                                //TemplateInfo templateInfo = DataProvider.TemplateDAO.GetTemplateByURLType(base.PublishmentSystemID, ETemplateType.FileTemplate, sinfo.EmailContentAddress);
                                TemplateInfo templateInfo = DataProvider.TemplateDAO.GetTemplateByTemplateID(base.PublishmentSystemID, ETemplateType.FileTemplate, sinfo.EmailContentAddress);

                                if (templateInfo == null || templateInfo.TemplateType != ETemplateType.FileTemplate)
                                {
                                    base.FailMessage("订阅信息邮件发送失败：邮件模板为空，请在【其他设置】中选择邮件模板");
                                    return;
                                }
                                StringBuilder contentBuilder = new StringBuilder(CreateCacheManager.FileContent.GetTemplateContent(base.PublishmentSystemInfo, templateInfo));

                                NameValueCollection queryString = new NameValueCollection();
                                queryString.Remove("publishmentSystemID");
                                queryString.Add("channelIndex", TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(TranslateUtils.StringCollectionToArrayList(cvalue)));
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
                            bool isSuccess = smtpMail.Send(out errorMessage);
                            if (isSuccess)
                            {
                                //修改会员信息推送次数 
                                DataProvider.SubscribeUserDAO.UpdatePushNum(base.PublishmentSystemID, info.SubscribeUserID);

                                StringUtility.AddLog(base.PublishmentSystemID, "订阅信息手动推送邮件:", string.Format("接收邮件:{0},邮件内容：{1}", info.Email, strBody));
                                base.SuccessMessage("订阅信息邮件发送成功！");
                            }
                            else
                            {
                                result = false;
                                ft = ft + ("订阅信息邮件发送失败：接收邮件: " + info.Email + "," + errorMessage);
                            }

                            //记录邮件发送状态
                            SubscribePushRecordInfo srinfo = new SubscribePushRecordInfo()
                            {
                                Email = info.Email,
                                PushType = ESubscribePushType.ManualPush,
                                SubscribeName = DataProvider.SubscribeDAO.GetName(base.PublishmentSystemID, info.SubscribeName),
                                SubscriptionTemplate = "订阅信息邮件发送手动推送",
                                PublishmentSystemID = base.PublishmentSystemID,
                                PushStatu = isSuccess ? EBoolean.True : EBoolean.False,
                                UserName = BaiRongDataProvider.AdministratorDAO.UserName
                            };
                            DataProvider.SubscribePushRecordDAO.Insert(srinfo);

                            PageUtils.Redirect(string.Format("background_subscribeUser.aspx?PublishmentSystemID={0}&ItemID={1}", this.publishmentSystemID, this.nodeID));

                            #endregion
                            result = true;
                        }
                        else
                        {
                            result = false;
                            ft = "会员没有订阅内容";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    base.FailMessage(ex, "订阅信息邮件发送失败：" + ex.Message);
                }
            }
            else
            {
                result = false;
                base.FailMessage("订阅信息邮件发送失败:没有会员需要推送！");
            }

            if (result)
                PageUtils.Redirect(string.Format("background_subscribeUser.aspx?PublishmentSystemID={0}&ItemID={1}", this.publishmentSystemID, this.nodeID));
            else
                base.FailMessage("订阅信息邮件发送失败:" + ft);
        }
    }
}
