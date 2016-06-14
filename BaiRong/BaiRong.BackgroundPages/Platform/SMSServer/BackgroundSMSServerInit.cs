using BaiRong.Core;
using BaiRong.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace BaiRong.BackgroundPages
{
    public class BackgroundSMSServerInit : BackgroundBasePage
    {
        public Repeater rptInstalled;
        public Repeater rptUnInstalled;

        private ArrayList smsServerInfoList;

        public static string GetRedirectUrl()
        {
            return PageUtils.GetPlatformUrl(string.Format("background_SMSServerInit.aspx"));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (base.GetQueryString("isInstall") != null && base.GetQueryString("smsServerType") != null)
            {
                ESMSServerType smsServerType = ESMSServerTypeUtils.GetEnumType(base.GetQueryString("smsServerType"));
                SMSServerInfo smsServerInfo = new SMSServerInfo();
                smsServerInfo.SMSServerName = ESMSServerTypeUtils.GetText(smsServerType);
                smsServerInfo.SMSServerEName = ESMSServerTypeUtils.GetValue(smsServerType);
                smsServerInfo.IsEnable = true;
                smsServerInfo.Additional = ESMSServerTypeUtils.GetAdditional(smsServerType);
                BaiRongDataProvider.SMSServerDAO.Insert(smsServerInfo);
                base.SuccessMessage("短信服务商添加成功");
            }
            else if (base.GetQueryString("isDelete") != null && base.GetQueryString("SMSServerID") != null)
            {
                int SMSServerID = base.GetIntQueryString("SMSServerID");
                if (SMSServerID > 0)
                {
                    BaiRongDataProvider.SMSServerDAO.Delete(SMSServerID);
                    base.SuccessMessage("短信服务商删除成功");
                }
            }
            else if (base.GetQueryString("isEnable") != null && base.GetQueryString("SMSServerID") != null)
            {
                int SMSServerID = base.GetIntQueryString("SMSServerID");
                if (SMSServerID > 0)
                {
                    SMSServerInfo smsServerInfo = BaiRongDataProvider.SMSServerDAO.GetSMSServerInfo(SMSServerID);
                    if (smsServerInfo != null)
                    {
                        string action = smsServerInfo.SMSServerEName == ConfigManager.Additional.SMSServerType ? "正在使用" : "启用";
                        ConfigManager.Additional.SMSServerType = smsServerInfo.SMSServerEName;
                        BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);
                        ConfigManager.IsChanged = true;
                        base.SuccessMessage(string.Format("成功{0}短信服务商", action));
                    }
                }
            }


            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.Platform.LeftMenu.ID_SMS, "短信服务商管理", AppManager.Platform.Permission.platform_SMS);

                this.smsServerInfoList = BaiRongDataProvider.SMSServerDAO.GetSMSServerInfoArrayList();

                this.rptInstalled.DataSource = this.smsServerInfoList;
                this.rptInstalled.ItemDataBound += new RepeaterItemEventHandler(rptInstalled_ItemDataBound);
                this.rptInstalled.DataBind();

                this.rptUnInstalled.DataSource = ESMSServerTypeUtils.GetESMSServerTypeList();
                this.rptUnInstalled.ItemDataBound += new RepeaterItemEventHandler(rptUnInstalled_ItemDataBound);
                this.rptUnInstalled.DataBind();
            }
        }

        private void rptInstalled_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SMSServerInfo smsServerInfo = e.Item.DataItem as SMSServerInfo;

                Literal ltlSMSServerEName = e.Item.FindControl("ltlSMSServerEName") as Literal;
                Literal ltlSMSServerName = e.Item.FindControl("ltlSMSServerName") as Literal;
                Literal ltlIsEnabled = e.Item.FindControl("ltlIsEnabled") as Literal;
                Literal ltlConfigUrl = e.Item.FindControl("ltlConfigUrl") as Literal;
                Literal ltlInitTemplate = e.Item.FindControl("ltlInitTemplate") as Literal;
                Literal ltlIsEnabledUrl = e.Item.FindControl("ltlIsEnabledUrl") as Literal;
                Literal ltlDeleteUrl = e.Item.FindControl("ltlDeleteUrl") as Literal;

                ltlSMSServerName.Text = smsServerInfo.SMSServerName;
                ltlSMSServerEName.Text = smsServerInfo.SMSServerEName;
                ltlIsEnabled.Text = StringUtils.GetTrueOrFalseImageHtml(smsServerInfo.SMSServerEName == ConfigManager.Additional.SMSServerType);

                string urlConfig = BackgroundSMSServerConfiguration.GetRedirectUrl(smsServerInfo.SMSServerID);

                ltlConfigUrl.Text = string.Format(@"<a href=""{0}"">设置</a>", urlConfig);

                string initTemplateUrl = Modal.ProgressBar.GetInitSMSTemplate(smsServerInfo.SMSServerEName);
                ltlInitTemplate.Text = string.Format(@"<a href=""javascript:void(0)"" onclick=""{0}"">初始化默认短信模板</a>", initTemplateUrl);

                string action = smsServerInfo.SMSServerEName == ConfigManager.Additional.SMSServerType ? "正在使用" : "启用";
                string urlIsEnabled = BackgroundSMSServerInit.GetRedirectUrl() + string.Format("?isEnable=True&SMSServerID={0}", smsServerInfo.SMSServerID);
                ltlIsEnabledUrl.Text = string.Format(@"<a href=""{0}"">{1}</a>", urlIsEnabled, action);

                string urlDelete = BackgroundSMSServerInit.GetRedirectUrl() + string.Format("?isDelete=True&SMSServerID={0}", smsServerInfo.SMSServerID);
                ltlDeleteUrl.Text = string.Format(@"<a href=""{0}"" onclick=""javascript:return confirm('此操作将删除选定的短信服务商，确认吗？');"">删除</a>", urlDelete);
            }
        }

        private void rptUnInstalled_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ESMSServerType smsServerType = (ESMSServerType)e.Item.DataItem;

                foreach (SMSServerInfo smsServerInfo in this.smsServerInfoList)
                {
                    if (smsServerInfo.SMSServerEName == ESMSServerTypeUtils.GetValue(smsServerType))
                    {
                        e.Item.Visible = false;
                        return;
                    }
                }

                Literal ltlSMSServerEName = e.Item.FindControl("ltlSMSServerEName") as Literal;
                Literal ltlSMSServerName = e.Item.FindControl("ltlSMSServerName") as Literal;
                Literal ltlInstallUrl = e.Item.FindControl("ltlInstallUrl") as Literal;

                ltlSMSServerName.Text = ESMSServerTypeUtils.GetText(smsServerType);
                ltlSMSServerEName.Text = ESMSServerTypeUtils.GetValue(smsServerType);

                string urlInstall = BackgroundSMSServerInit.GetRedirectUrl() + string.Format("?isInstall=True&smsServerType={0}", ESMSServerTypeUtils.GetValue(smsServerType));
                ltlInstallUrl.Text = string.Format(@"<a href=""{0}"">安装</a>", urlInstall);
            }
        }
    }
}
