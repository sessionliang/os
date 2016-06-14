using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model.Service;
using System.Collections;
using System.Collections.Generic;



namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundSubscribeSet : BackgroundBasePage
    {
        public DropDownList ddlEmailContentAddress;
        public DropDownList ddlMobileContentAddress;
        public RadioButton PushTypeWeek;
        public RadioButton PushTypeMonth;
        public TextBox PushWeekDate;
        public TextBox PushMonthDate;

        private SubscribeSetInfo info;
        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            info = DataProvider.SubscribeSetDAO.GetSubscribeSetInfo(this.PublishmentSystemID);


            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Subscribe, "其他设置", AppManager.CMS.Permission.WebSite.Subscribe);



                if (info != null)
                {
                    ListItem itemd = new ListItem("请选择", "0");
                    this.ddlEmailContentAddress.Items.Add(itemd);
                    this.ddlMobileContentAddress.Items.Add(itemd);
                    //加载单页模板
                    ArrayList tInfos = DataProvider.TemplateDAO.GetTemplateInfoArrayListByType(base.PublishmentSystemID, ETemplateType.FileTemplate);
                    foreach (TemplateInfo tinfo in tInfos)
                    {
                        ListItem item = new ListItem(tinfo.TemplateName, tinfo.TemplateID.ToString());
                        this.ddlEmailContentAddress.Items.Add(item);
                        this.ddlMobileContentAddress.Items.Add(item);
                    }

                    this.ddlMobileContentAddress.SelectedValue = info.MobileContentAddress;
                    this.ddlEmailContentAddress.SelectedValue = info.EmailContentAddress;
                    if (ESubscribePushDateTypeUtils.Equals(ESubscribePushDateType.Week, info.PushType))
                    {
                        this.PushTypeWeek.Checked = true;
                        this.PushWeekDate.Text = info.PushDate;
                        this.PushMonthDate.Text = "";
                    }
                    if (ESubscribePushDateTypeUtils.Equals(ESubscribePushDateType.Month, info.PushType))
                    {
                        this.PushTypeMonth.Checked = true;
                        this.PushMonthDate.Text = info.PushDate;
                        this.PushWeekDate.Text = "";
                    }
                }
                else
                {
                    //创建默认模板
                    int eid = 0, mid = 0;
                    createTemplateInfo(out eid, out mid);
                    ListItem itemd = new ListItem("请选择", "0");
                    this.ddlEmailContentAddress.Items.Add(itemd);
                    this.ddlMobileContentAddress.Items.Add(itemd);
                    //加载单页模板
                    ArrayList tInfos = DataProvider.TemplateDAO.GetTemplateInfoArrayListByType(base.PublishmentSystemID, ETemplateType.FileTemplate);
                    foreach (TemplateInfo tinfo in tInfos)
                    {
                        ListItem item = new ListItem(tinfo.TemplateName, tinfo.TemplateID.ToString());
                        this.ddlEmailContentAddress.Items.Add(item);
                        this.ddlMobileContentAddress.Items.Add(item);
                    }

                    //创建默认数据
                    info = new SubscribeSetInfo();
                    info.PublishmentSystemID = base.PublishmentSystemID;
                    info.AddDate = DateTime.Now;
                    info.PushDate = "1";
                    info.EmailContentAddress = eid.ToString();
                    info.MobileContentAddress = mid.ToString();
                    DataProvider.SubscribeSetDAO.Insert(info);


                    this.ddlMobileContentAddress.SelectedValue = info.MobileContentAddress;
                    this.ddlEmailContentAddress.SelectedValue = info.EmailContentAddress;
                    if (ESubscribePushDateTypeUtils.Equals(ESubscribePushDateType.Week, info.PushType))
                    {
                        this.PushTypeWeek.Checked = true;
                        this.PushWeekDate.Text = info.PushDate;
                        this.PushMonthDate.Text = "";
                    }
                    if (ESubscribePushDateTypeUtils.Equals(ESubscribePushDateType.Month, info.PushType))
                    {
                        this.PushTypeMonth.Checked = true;
                        this.PushMonthDate.Text = info.PushDate;
                        this.PushWeekDate.Text = "";
                    }

                }
            }
        }

        public void Add_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(PageUtils.GetSTLUrl(string.Format("background_templateMain.aspx?PublishmentSystemID={0}", base.PublishmentSystemID)));
        }
        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (this.ddlEmailContentAddress.SelectedValue == "0")
                {
                    base.FailMessage(null, "其他设置修改失败:请选择邮件内容模板！");
                    return;
                }
                if (this.ddlMobileContentAddress.SelectedValue == "0")
                {
                    base.FailMessage(null, "其他设置修改失败:请选择手机内容模板！");
                    return;
                }

                if (info != null)
                {
                    info.EmailContentAddress = this.ddlEmailContentAddress.SelectedValue;
                    info.MobileContentAddress = this.ddlMobileContentAddress.SelectedValue;
                    info.PublishmentSystemID = base.PublishmentSystemID;
                    info.AddDate = DateTime.Now;
                    if (this.PushTypeWeek.Checked)
                    {
                        if (string.IsNullOrEmpty(this.PushWeekDate.Text.Trim()) || int.Parse(this.PushWeekDate.Text) > 7 || int.Parse(this.PushWeekDate.Text) == 0)
                        {
                            base.FailMessage(null, "其他设置修改失败:每周推送时间必须为1-7！");
                            return;
                        }
                        info.PushType = ESubscribePushDateTypeUtils.GetValue(ESubscribePushDateType.Week);
                        info.PushDate = this.PushWeekDate.Text;
                    }
                    if (this.PushTypeMonth.Checked)
                    {
                        if (string.IsNullOrEmpty(this.PushMonthDate.Text) || int.Parse(this.PushMonthDate.Text) > 31 || int.Parse(this.PushMonthDate.Text)==0)
                        {
                            base.FailMessage(null, "其他设置修改失败:每月推送时间必须为1-31！");
                            return;
                        }
                        info.PushType = ESubscribePushDateTypeUtils.GetValue(ESubscribePushDateType.Month);
                        info.PushDate = this.PushMonthDate.Text;
                    }
                }
                else
                {
                    info = new SubscribeSetInfo();

                    info.EmailContentAddress = this.ddlEmailContentAddress.SelectedValue;
                    info.MobileContentAddress = this.ddlMobileContentAddress.SelectedValue;
                    info.PublishmentSystemID = base.PublishmentSystemID;
                    if (this.PushTypeWeek.Checked)
                    {
                        if (string.IsNullOrEmpty(this.PushWeekDate.Text.Trim()))
                        {
                            base.FailMessage(null, "其他设置修改失败:每周推送时间必须为1-7！");
                            return;
                        }
                        info.PushType = ESubscribePushDateTypeUtils.GetValue(ESubscribePushDateType.Week);
                        info.PushDate = this.PushWeekDate.Text;
                    }
                    if (this.PushTypeMonth.Checked)
                    {
                        if (string.IsNullOrEmpty(this.PushMonthDate.Text) || int.Parse(this.PushMonthDate.Text) > 31)
                        {
                            base.FailMessage(null, "其他设置修改失败:每月推送时间必须为1-31！");
                            return;
                        }
                        info.PushType = ESubscribePushDateTypeUtils.GetValue(ESubscribePushDateType.Month);
                        info.PushDate = this.PushMonthDate.Text;
                    }
                }

                try
                {
                    info.UserName = BaiRongDataProvider.AdministratorDAO.UserName;
                    if (info.SubscribeSetID > 0)
                    {
                        DataProvider.SubscribeSetDAO.Update(info);
                        addTask(info, "edti");
                    }
                    else
                    {
                        DataProvider.SubscribeSetDAO.Insert(info);
                        addTask(info, "add");
                    }
                    StringUtility.AddLog(base.PublishmentSystemID, "修改其他设置");
                    base.SuccessMessage("其他设置修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "其他设置修改失败！");
                }
            }
        }

        public void createTemplateInfo(out int eid, out int mid)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(base.PublishmentSystemID); 
            ECharset charset = ECharsetUtils.GetEnumType(publishmentSystemInfo.Additional.Charset);

            TemplateInfo templateInfo = new TemplateInfo(0, publishmentSystemInfo.PublishmentSystemID, "信息订阅邮件内容模板", ETemplateType.FileTemplate, "T_信息订阅邮件内容模板.html", "RssEmailTemplate.html", ".html", charset, false);
            eid = DataProvider.TemplateDAO.Insert(templateInfo, templateInfo.Content);

            templateInfo = new TemplateInfo(0, publishmentSystemInfo.PublishmentSystemID, "信息订阅手机内容模板", ETemplateType.FileTemplate, "T_信息订阅手机内容模板.html", "RssMobileTemplate.html", ".html", charset, false);
            mid = DataProvider.TemplateDAO.Insert(templateInfo, templateInfo.Content); 
        }
        public void addTask(SubscribeSetInfo info, string type)
        {
            ExtendedAttributes serviceParamters = new ExtendedAttributes();

            string taskName = "定时推送会员订阅内容";
            if (type == "add")
            {
                if (BaiRongDataProvider.TaskDAO.IsExists(taskName))
                {
                    base.FailMessage("任务添加失败，任务名称已存在！");
                }
                else
                {
                    try
                    {
                        TaskInfo taskInfo = new TaskInfo(AppManager.CMS.AppID);
                        taskInfo.TaskName = taskName;
                        taskInfo.PublishmentSystemID = base.PublishmentSystemID;
                        taskInfo.ServiceType = EServiceType.Subscribe;

                        taskInfo.FrequencyType = EFrequencyTypeUtils.GetEnumType(info.PushType.ToString());
                        if (EFrequencyTypeUtils.Equals(EFrequencyType.Month, taskInfo.FrequencyType))
                        {
                            taskInfo.StartDay = TranslateUtils.ToInt(info.PushDate);
                        }
                        if (EFrequencyTypeUtils.Equals(EFrequencyType.Week, taskInfo.FrequencyType))
                        {
                            taskInfo.StartWeekday = TranslateUtils.ToInt(info.PushDate);
                        }
                        taskInfo.Description = "按订阅信息内设置的推送周期定时推送会员的订阅内容";

                        taskInfo.ServiceParameters = serviceParamters.ToString();

                        taskInfo.IsEnabled = true;
                        taskInfo.AddDate = DateTime.Now;
                        taskInfo.OnlyOnceDate = DateUtils.SqlMinValue;
                        taskInfo.LastExecuteDate = DateUtils.SqlMinValue;

                        BaiRongDataProvider.TaskDAO.Insert(taskInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, string.Format("添加{0}任务", EServiceTypeUtils.GetText(taskInfo.ServiceType)), string.Format("任务名称:{0}", taskInfo.TaskName));

                        base.SuccessMessage("任务添加成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "任务添加失败！");
                    }
                }
            }
            else
            {
                try
                {
                    ArrayList arrlist = BaiRongDataProvider.TaskDAO.GetTaskInfoArrayList(AppManager.CMS.AppID, EServiceType.Subscribe, base.PublishmentSystemID);
                    TaskInfo taskInfo = arrlist.Count > 0 ? arrlist[0] as TaskInfo : null;

                    if (taskInfo == null)
                        addTask(info, "add");

                    taskInfo.FrequencyType = EFrequencyTypeUtils.GetEnumType(info.PushType.ToString());
                    if (EFrequencyTypeUtils.Equals(EFrequencyType.Month, taskInfo.FrequencyType))
                    {
                        taskInfo.StartDay = TranslateUtils.ToInt(info.PushDate);
                    }
                    if (EFrequencyTypeUtils.Equals(EFrequencyType.Week, taskInfo.FrequencyType))
                    {
                        taskInfo.StartWeekday = TranslateUtils.ToInt(info.PushDate);
                    }

                    taskInfo.IsEnabled = true;
                    taskInfo.AddDate = DateTime.Now;

                    BaiRongDataProvider.TaskDAO.Update(taskInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, string.Format("添加{0}任务", EServiceTypeUtils.GetText(taskInfo.ServiceType)), string.Format("任务名称:{0}", taskInfo.TaskName));

                    base.SuccessMessage("任务修改成功！");
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "任务修改失败！");
                    return;
                }
            }
        }
    }
}
