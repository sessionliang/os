using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model.Service;
using BaiRong.Core.Service;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class TaskAdd : BackgroundBasePage
    {
        public TextBox TaskName;
        public DropDownList FrequencyType;

        public PlaceHolder PlaceHolder_PeriodIntervalMinute;
        public TextBox PeriodInterval;
        public DropDownList PeriodIntervalType;

        public PlaceHolder PlaceHolder_NotPeriod;
        public DropDownList StartDay;
        public DropDownList StartWeekday;
        public DropDownList StartHour;

        public TextBox Description;

        public PlaceHolder PlaceHolder_Publish;
        public CheckBoxList PublishTypes;
        public PlaceHolder PlaceHolder_Publish_JustInTime;
        public TextBox PublishFilter;

        public PlaceHolder PlaceHolder_Create;
        public ListBox CreateChannelIDCollection;
        public CheckBox CreateIsCreateAll;
        public CheckBoxList CreateCreateTypes;
        public CheckBox CreateIsCreateSiteMap;

        public PlaceHolder PlaceHolder_Gather;
        public Help GatherHelp;
        public ListBox GatherListBox;

        public PlaceHolder PlaceHolder_Backup;
        public DropDownList BackupType;
        public PlaceHolder PlaceHolder_Backup_PublishmentSystem;
        public ListBox BackupPublishmentSystemIDCollection;
        public CheckBox BackupIsBackupAll;


        public PlaceHolder PlaceHolder_Check;
        public ListBox CheckChannelIDCollection;
        public CheckBox CheckIsCheckAll;

        public PlaceHolder PlaceHolder_UnCheck;
        public ListBox UnCheckChannelIDCollection;
        public CheckBox UnCheckIsCheckAll;

        private EServiceType serviceType;

        public static string GetOpenWindowStringToAdd(EServiceType serviceType, int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("ServiceType", EServiceTypeUtils.GetValue(serviceType));
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("添加任务", "modal_taskAdd.aspx", arguments);
        }

        public static string GetOpenWindowStringToEdit(int taskID, EServiceType serviceType, int publishmentSystemID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("TaskID", taskID.ToString());
            arguments.Add("ServiceType", EServiceTypeUtils.GetValue(serviceType));
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            return PageUtility.GetOpenWindowString("修改任务", "modal_taskAdd.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.serviceType = EServiceTypeUtils.GetEnumType(base.GetQueryString("ServiceType"));
            if (!IsPostBack)
            {
                if (this.serviceType == EServiceType.Publish || this.serviceType == EServiceType.Check || this.serviceType == EServiceType.UnCheck)
                {
                    EFrequencyTypeUtils.AddListItems(this.FrequencyType, true);
                }
                else
                {
                    EFrequencyTypeUtils.AddListItems(this.FrequencyType, false);
                }
                for (int i = 1; i < 32; i++)
                {
                    this.StartDay.Items.Add(new ListItem(i + "日", i.ToString()));
                }
                for (int i = 1; i < 8; i++)
                {
                    string weekName = string.Empty;
                    if (i == 1)
                    {
                        weekName = "星期一";
                    }
                    else if (i == 2)
                    {
                        weekName = "星期二";
                    }
                    else if (i == 3)
                    {
                        weekName = "星期三";
                    }
                    else if (i == 4)
                    {
                        weekName = "星期四";
                    }
                    else if (i == 5)
                    {
                        weekName = "星期五";
                    }
                    else if (i == 6)
                    {
                        weekName = "星期六";
                    }
                    else if (i == 7)
                    {
                        weekName = "星期日";
                    }
                    this.StartWeekday.Items.Add(new ListItem(weekName, i.ToString()));
                }
                for (int i = 0; i < 24; i++)
                {
                    this.StartHour.Items.Add(new ListItem(i + "点", i.ToString()));
                }

                ListItem listItem = new ListItem("周", "5040");
                this.PeriodIntervalType.Items.Add(listItem);
                listItem = new ListItem("天", "720");
                this.PeriodIntervalType.Items.Add(listItem);
                listItem = new ListItem("小时", "12");
                this.PeriodIntervalType.Items.Add(listItem);
                listItem = new ListItem("分钟", "1");
                listItem.Selected = true;
                this.PeriodIntervalType.Items.Add(listItem);

                if (this.serviceType == EServiceType.Publish)
                {
                    this.PlaceHolder_Publish.Visible = true;

                    if (base.PublishmentSystemID != 0)
                    {
                        if (base.PublishmentSystemInfo.Additional.IsSiteStorage)
                        {
                            listItem = EPublishTypeUtils.GetListItem(EPublishType.Site, false);
                            this.PublishTypes.Items.Add(listItem);
                        }
                        if (base.PublishmentSystemInfo.Additional.IsImageStorage)
                        {
                            listItem = EPublishTypeUtils.GetListItem(EPublishType.Image, false);
                            this.PublishTypes.Items.Add(listItem);
                        }
                        if (base.PublishmentSystemInfo.Additional.IsVideoStorage)
                        {
                            listItem = EPublishTypeUtils.GetListItem(EPublishType.Video, false);
                            this.PublishTypes.Items.Add(listItem);
                        }
                        if (base.PublishmentSystemInfo.Additional.IsFileStorage)
                        {
                            listItem = EPublishTypeUtils.GetListItem(EPublishType.File, false);
                            this.PublishTypes.Items.Add(listItem);
                        }
                    }
                    else
                    {
                        EPublishTypeUtils.AddListItems(this.PublishTypes);
                    }
                }
                else if (this.serviceType == EServiceType.Create)
                {
                    this.PlaceHolder_Create.Visible = true;

                    if (base.PublishmentSystemID != 0)
                    {
                        NodeManager.AddListItems(this.CreateChannelIDCollection.Items, base.PublishmentSystemInfo, false, true);
                    }
                    else
                    {
                        ArrayList arraylist = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
                        foreach (int publishmentSystemID in arraylist)
                        {
                            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                            ListItem item = new ListItem(publishmentSystemInfo.PublishmentSystemName, publishmentSystemInfo.PublishmentSystemID.ToString());
                            this.CreateChannelIDCollection.Items.Add(item);
                        }
                    }

                    ECreateTypeUtils.AddListItems(this.CreateCreateTypes);
                }
                else if (this.serviceType == EServiceType.Gather)
                {
                    this.PlaceHolder_Gather.Visible = true;

                    if (base.PublishmentSystemID != 0)
                    {
                        ArrayList gatherRuleNameArrayList = DataProvider.GatherRuleDAO.GetGatherRuleNameArrayList(base.PublishmentSystemID);
                        string gatherTypeValue = EGatherTypeUtils.GetValue(EGatherType.Web);
                        string gatherTypeText = EGatherTypeUtils.GetText(EGatherType.Web);
                        foreach (string gatherName in gatherRuleNameArrayList)
                        {
                            this.GatherListBox.Items.Add(new ListItem(gatherName + "(" + gatherTypeText + ")", gatherTypeValue + "_" + gatherName));
                        }
                        gatherRuleNameArrayList = DataProvider.GatherDatabaseRuleDAO.GetGatherRuleNameArrayList(base.PublishmentSystemID);
                        gatherTypeValue = EGatherTypeUtils.GetValue(EGatherType.Database);
                        gatherTypeText = EGatherTypeUtils.GetText(EGatherType.Database);
                        foreach (string gatherName in gatherRuleNameArrayList)
                        {
                            this.GatherListBox.Items.Add(new ListItem(gatherName + "(" + gatherTypeText + ")", gatherTypeValue + "_" + gatherName));
                        }
                        gatherRuleNameArrayList = DataProvider.GatherFileRuleDAO.GetGatherRuleNameArrayList(base.PublishmentSystemID);
                        gatherTypeValue = EGatherTypeUtils.GetValue(EGatherType.File);
                        gatherTypeText = EGatherTypeUtils.GetText(EGatherType.File);
                        foreach (string gatherName in gatherRuleNameArrayList)
                        {
                            this.GatherListBox.Items.Add(new ListItem(gatherName + "(" + gatherTypeText + ")", gatherTypeValue + "_" + gatherName));
                        }

                        this.GatherHelp.Text = "选择需要定时执行的采集名称";
                    }
                    else
                    {
                        ArrayList arraylist = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
                        foreach (int publishmentSystemID in arraylist)
                        {
                            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                            ListItem item = new ListItem(publishmentSystemInfo.PublishmentSystemName, publishmentSystemInfo.PublishmentSystemID.ToString());
                            this.GatherListBox.Items.Add(item);
                        }

                        this.GatherHelp.Text = "选择需要定时采集的应用";
                    }
                }
                else if (this.serviceType == EServiceType.Backup)
                {
                    this.PlaceHolder_Backup.Visible = true;

                    if (base.PublishmentSystemID != 0)
                    {
                        this.PlaceHolder_Backup_PublishmentSystem.Visible = false;
                    }
                    else
                    {
                        this.PlaceHolder_Backup_PublishmentSystem.Visible = true;
                        ArrayList arraylist = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
                        foreach (int publishmentSystemID in arraylist)
                        {
                            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                            ListItem item = new ListItem(publishmentSystemInfo.PublishmentSystemName, publishmentSystemInfo.PublishmentSystemID.ToString());
                            this.BackupPublishmentSystemIDCollection.Items.Add(item);
                        }
                    }

                    EBackupTypeUtils.AddListItems(this.BackupType);
                }
                else if (this.serviceType == EServiceType.Check)
                {
                    this.PlaceHolder_Check.Visible = true;

                    if (base.PublishmentSystemID != 0)
                    {
                        NodeManager.AddListItems(this.CheckChannelIDCollection.Items, base.PublishmentSystemInfo, false, true);
                    }
                    else
                    {
                        ArrayList arraylist = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
                        foreach (int publishmentSystemID in arraylist)
                        {
                            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                            ListItem item = new ListItem(publishmentSystemInfo.PublishmentSystemName, publishmentSystemInfo.PublishmentSystemID.ToString());
                            this.CheckChannelIDCollection.Items.Add(item);
                        }
                    }
                }
                else if (this.serviceType == EServiceType.UnCheck)
                {
                    this.PlaceHolder_UnCheck.Visible = true;

                    if (base.PublishmentSystemID != 0)
                    {
                        NodeManager.AddListItems(this.UnCheckChannelIDCollection.Items, base.PublishmentSystemInfo, false, true);
                    }
                    else
                    {
                        ArrayList arraylist = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
                        foreach (int publishmentSystemID in arraylist)
                        {
                            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                            ListItem item = new ListItem(publishmentSystemInfo.PublishmentSystemName, publishmentSystemInfo.PublishmentSystemID.ToString());
                            this.UnCheckChannelIDCollection.Items.Add(item);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(base.GetQueryString("TaskID")))
                {
                    int taskID = TranslateUtils.ToInt(base.GetQueryString("TaskID"));
                    TaskInfo taskInfo = BaiRongDataProvider.TaskDAO.GetTaskInfo(taskID);
                    if (taskInfo != null)
                    {
                        this.TaskName.Text = taskInfo.TaskName;
                        this.TaskName.Enabled = false;

                        ControlUtils.SelectListItems(this.FrequencyType, EFrequencyTypeUtils.GetValue(taskInfo.FrequencyType));
                        ControlUtils.SelectListItems(this.StartDay, taskInfo.StartDay.ToString());
                        ControlUtils.SelectListItems(this.StartWeekday, taskInfo.StartWeekday.ToString());
                        ControlUtils.SelectListItems(this.StartHour, taskInfo.StartHour.ToString());

                        if (taskInfo.PeriodIntervalMinute % 5040 == 0)
                        {
                            this.PeriodInterval.Text = Convert.ToInt32(taskInfo.PeriodIntervalMinute / 5040).ToString();
                            ControlUtils.SelectListItems(this.PeriodIntervalType, "5040");
                        }
                        else if (taskInfo.PeriodIntervalMinute % 720 == 0)
                        {
                            this.PeriodInterval.Text = Convert.ToInt32(taskInfo.PeriodIntervalMinute / 720).ToString();
                            ControlUtils.SelectListItems(this.PeriodIntervalType, "720");
                        }
                        else if (taskInfo.PeriodIntervalMinute % 12 == 0)
                        {
                            this.PeriodInterval.Text = Convert.ToInt32(taskInfo.PeriodIntervalMinute / 12).ToString();
                            ControlUtils.SelectListItems(this.PeriodIntervalType, "12");
                        }
                        else
                        {
                            this.PeriodInterval.Text = taskInfo.PeriodIntervalMinute.ToString();
                            ControlUtils.SelectListItems(this.PeriodIntervalType, "1");
                        }

                        this.Description.Text = taskInfo.Description;

                        if (this.serviceType == EServiceType.Publish)
                        {
                            TaskPublishInfo taskPublishInfo = new TaskPublishInfo(taskInfo.ServiceParameters);

                            ControlUtils.SelectListItems(this.PublishTypes, taskPublishInfo.PublishTypes.Split(','));
                            this.PublishFilter.Text = taskPublishInfo.Filter;
                        }
                        else if (this.serviceType == EServiceType.Create)
                        {
                            TaskCreateInfo taskCreateInfo = new TaskCreateInfo(taskInfo.ServiceParameters);
                            if (taskCreateInfo.IsCreateAll)
                            {
                                foreach (ListItem item in this.CreateChannelIDCollection.Items)
                                {
                                    item.Selected = true;
                                }
                                this.CreateIsCreateAll.Checked = true;
                            }
                            else
                            {
                                ArrayList channelIDArrayList = TranslateUtils.StringCollectionToArrayList(taskCreateInfo.ChannelIDCollection);
                                ControlUtils.SelectListItems(this.CreateChannelIDCollection, channelIDArrayList);
                                this.CreateIsCreateAll.Checked = false;
                            }
                            ArrayList createTypeArrayList = TranslateUtils.StringCollectionToArrayList(taskCreateInfo.CreateTypes);
                            foreach (ListItem item in this.CreateCreateTypes.Items)
                            {
                                if (createTypeArrayList.Contains(item.Value))
                                {
                                    item.Selected = true;
                                }
                                else
                                {
                                    item.Selected = false;
                                }
                            }

                            if (taskCreateInfo.IsCreateSiteMap)
                            {
                                this.CreateIsCreateSiteMap.Checked = true;
                            }
                        }
                        else if (this.serviceType == EServiceType.Gather)
                        {
                            TaskGatherInfo taskGatherInfo = new TaskGatherInfo(taskInfo.ServiceParameters);
                            if (base.PublishmentSystemID != 0)
                            {
                                ArrayList webGatherNames = TranslateUtils.StringCollectionToArrayList(taskGatherInfo.WebGatherNames);
                                ArrayList databaseGatherNames = TranslateUtils.StringCollectionToArrayList(taskGatherInfo.DatabaseGatherNames);
                                ArrayList fileGatherNames = TranslateUtils.StringCollectionToArrayList(taskGatherInfo.FileGatherNames);
                                foreach (ListItem item in this.GatherListBox.Items)
                                {
                                    EGatherType gatherType = EGatherTypeUtils.GetEnumType(item.Value.Split('_')[0]);
                                    string gatherName = item.Value.Substring(item.Value.Split('_')[0].Length + 1);
                                    if (gatherType == EGatherType.Web && webGatherNames.Contains(gatherName))
                                    {
                                        item.Selected = true;
                                    }
                                    else if (gatherType == EGatherType.Database && databaseGatherNames.Contains(gatherName))
                                    {
                                        item.Selected = true;
                                    }
                                    else if (gatherType == EGatherType.File && fileGatherNames.Contains(gatherName))
                                    {
                                        item.Selected = true;
                                    }
                                }
                            }
                            else
                            {
                                ArrayList publishmentSystemIDArrayList = TranslateUtils.StringCollectionToArrayList(taskGatherInfo.PublishmentSystemIDCollection);
                                ControlUtils.SelectListItems(this.GatherListBox, publishmentSystemIDArrayList);
                            }
                        }
                        else if (this.serviceType == EServiceType.Backup)
                        {
                            TaskBackupInfo taskBackupInfo = new TaskBackupInfo(taskInfo.ServiceParameters);

                            if (taskInfo.PublishmentSystemID == 0)
                            {
                                if (taskBackupInfo.IsBackupAll)
                                {
                                    foreach (ListItem item in this.BackupPublishmentSystemIDCollection.Items)
                                    {
                                        item.Selected = true;
                                    }
                                    this.BackupIsBackupAll.Checked = true;
                                }
                                else
                                {
                                    ArrayList publishmentSystemIDArrayList = TranslateUtils.StringCollectionToArrayList(taskBackupInfo.PublishmentSystemIDCollection);
                                    ControlUtils.SelectListItems(this.BackupPublishmentSystemIDCollection, publishmentSystemIDArrayList);
                                    this.BackupIsBackupAll.Checked = false;
                                }
                            }
                            else
                            {
                                ControlUtils.SelectListItems(this.BackupPublishmentSystemIDCollection, taskInfo.PublishmentSystemID.ToString());
                            }

                            ControlUtils.SelectListItems(this.BackupType, EBackupTypeUtils.GetValue(taskBackupInfo.BackupType));
                        }
                        else if (this.serviceType == EServiceType.Check)
                        {
                            TaskCheckInfo taskCheckInfo = new TaskCheckInfo(taskInfo.ServiceParameters);
                            if (taskCheckInfo.IsAll)
                            {
                                foreach (ListItem item in this.CheckChannelIDCollection.Items)
                                {
                                    item.Selected = true;
                                }
                                this.CheckIsCheckAll.Checked = true;
                            }
                            else
                            {
                                ArrayList channelIDArrayList = TranslateUtils.StringCollectionToArrayList(taskCheckInfo.ChannelIDCollection);
                                ControlUtils.SelectListItems(this.CheckChannelIDCollection, channelIDArrayList);
                                this.CheckIsCheckAll.Checked = false;
                            }
                        }
                        else if (this.serviceType == EServiceType.UnCheck)
                        {
                            TaskCheckInfo taskUnCheckInfo = new TaskCheckInfo(taskInfo.ServiceParameters);
                            if (taskUnCheckInfo.IsAll)
                            {
                                foreach (ListItem item in this.UnCheckChannelIDCollection.Items)
                                {
                                    item.Selected = true;
                                }
                                this.UnCheckIsCheckAll.Checked = true;
                            }
                            else
                            {
                                ArrayList channelIDArrayList = TranslateUtils.StringCollectionToArrayList(taskUnCheckInfo.ChannelIDCollection);
                                ControlUtils.SelectListItems(this.UnCheckChannelIDCollection, channelIDArrayList);
                                this.UnCheckIsCheckAll.Checked = false;
                            }
                        }
                    }
                }

                this.FrequencyType_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        public void FrequencyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            EFrequencyType frequencyType = EFrequencyTypeUtils.GetEnumType(this.FrequencyType.SelectedValue);
            this.PlaceHolder_NotPeriod.Visible = true;
            this.PlaceHolder_PeriodIntervalMinute.Visible = false;
            if (frequencyType == EFrequencyType.Month)
            {
                this.StartWeekday.Enabled = false;
                this.StartDay.Enabled = this.StartHour.Enabled = true;
            }
            else if (frequencyType == EFrequencyType.Week)
            {
                this.StartDay.Enabled = false;
                this.StartWeekday.Enabled = this.StartHour.Enabled = true;
            }
            else if (frequencyType == EFrequencyType.Day)
            {
                this.StartDay.Enabled = this.StartWeekday.Enabled = false;
                this.StartHour.Enabled = true;
            }
            else if (frequencyType == EFrequencyType.Hour)
            {
                this.StartDay.Enabled = this.StartWeekday.Enabled = this.StartHour.Enabled = false;
            }
            else
            {
                this.PlaceHolder_NotPeriod.Visible = false;
                this.PlaceHolder_PeriodIntervalMinute.Visible = true;

                if (frequencyType == EFrequencyType.JustInTime)
                {
                    this.PlaceHolder_PeriodIntervalMinute.Visible = false;
                }
            }

            if (this.serviceType == EServiceType.Publish)
            {
                if (frequencyType == EFrequencyType.JustInTime)
                {
                    this.PlaceHolder_Publish_JustInTime.Visible = true;
                }
                else
                {
                    this.PlaceHolder_Publish_JustInTime.Visible = false;
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            ExtendedAttributes serviceParamters = new ExtendedAttributes();
            if (this.serviceType == EServiceType.Publish)
            {
                TaskPublishInfo taskPublishInfo = new TaskPublishInfo(string.Empty);
                taskPublishInfo.PublishTypes = ControlUtils.GetSelectedListControlValueCollection(this.PublishTypes);
                if (string.IsNullOrEmpty(taskPublishInfo.PublishTypes))
                {
                    base.FailMessage("请选择发布类型！");
                    return;
                }
                taskPublishInfo.Filter = this.PublishFilter.Text;
                serviceParamters = taskPublishInfo;
            }
            else if (this.serviceType == EServiceType.Create)
            {
                TaskCreateInfo taskCreateInfo = new TaskCreateInfo(string.Empty);
                taskCreateInfo.IsCreateAll = this.CreateIsCreateAll.Checked;
                taskCreateInfo.ChannelIDCollection = TranslateUtils.ObjectCollectionToString(ControlUtils.GetSelectedListControlValueArrayList(this.CreateChannelIDCollection));
                taskCreateInfo.CreateTypes = TranslateUtils.ObjectCollectionToString(ControlUtils.GetSelectedListControlValueArrayList(this.CreateCreateTypes));
                taskCreateInfo.IsCreateSiteMap = this.CreateIsCreateSiteMap.Checked;
                serviceParamters = taskCreateInfo;
            }
            else if (this.serviceType == EServiceType.Gather)
            {
                TaskGatherInfo taskGatherInfo = new TaskGatherInfo(string.Empty);
                if (base.PublishmentSystemID != 0)
                {
                    ArrayList webGatherNames = new ArrayList();
                    ArrayList databaseGatherNames = new ArrayList();
                    ArrayList fileGatherNames = new ArrayList();
                    foreach (ListItem item in this.GatherListBox.Items)
                    {
                        if (item.Selected)
                        {
                            EGatherType gatherType = EGatherTypeUtils.GetEnumType(item.Value.Split('_')[0]);
                            string gatherName = item.Value.Substring(item.Value.Split('_')[0].Length + 1);
                            if (gatherType == EGatherType.Web && !webGatherNames.Contains(gatherName))
                            {
                                webGatherNames.Add(gatherName);
                            }
                            else if (gatherType == EGatherType.Database && !databaseGatherNames.Contains(gatherName))
                            {
                                databaseGatherNames.Add(gatherName);
                            }
                            else if (gatherType == EGatherType.File && !fileGatherNames.Contains(gatherName))
                            {
                                fileGatherNames.Add(gatherName);
                            }
                        }
                    }
                    taskGatherInfo.WebGatherNames = TranslateUtils.ObjectCollectionToString(webGatherNames);
                    taskGatherInfo.DatabaseGatherNames = TranslateUtils.ObjectCollectionToString(databaseGatherNames);
                    taskGatherInfo.FileGatherNames = TranslateUtils.ObjectCollectionToString(fileGatherNames);
                }
                else
                {
                    taskGatherInfo.PublishmentSystemIDCollection = TranslateUtils.ObjectCollectionToString(ControlUtils.GetSelectedListControlValueArrayList(this.GatherListBox));
                }
                serviceParamters = taskGatherInfo;
            }
            else if (this.serviceType == EServiceType.Backup)
            {
                TaskBackupInfo taskBackupInfo = new TaskBackupInfo(string.Empty);
                taskBackupInfo.BackupType = EBackupTypeUtils.GetEnumType(this.BackupType.SelectedValue);
                taskBackupInfo.IsBackupAll = this.BackupIsBackupAll.Checked;
                taskBackupInfo.PublishmentSystemIDCollection = TranslateUtils.ObjectCollectionToString(ControlUtils.GetSelectedListControlValueArrayList(this.BackupPublishmentSystemIDCollection));
                serviceParamters = taskBackupInfo;
            }
            else if (this.serviceType == EServiceType.Check)
            {
                TaskCheckInfo taskCheckInfo = new TaskCheckInfo(string.Empty);
                taskCheckInfo.UserName = AdminManager.Current.UserName;
                taskCheckInfo.IsAll = this.CheckIsCheckAll.Checked;
                taskCheckInfo.ChannelIDCollection = TranslateUtils.ObjectCollectionToString(ControlUtils.GetSelectedListControlValueArrayList(this.CheckChannelIDCollection));
                serviceParamters = taskCheckInfo;
            }
            else if (this.serviceType == EServiceType.UnCheck)
            {
                TaskCheckInfo taskUnCheckInfo = new TaskCheckInfo(string.Empty);
                taskUnCheckInfo.UserName = AdminManager.Current.UserName;
                taskUnCheckInfo.IsAll = this.UnCheckIsCheckAll.Checked;
                taskUnCheckInfo.ChannelIDCollection = TranslateUtils.ObjectCollectionToString(ControlUtils.GetSelectedListControlValueArrayList(this.UnCheckChannelIDCollection));
                serviceParamters = taskUnCheckInfo;
            }

            if (!string.IsNullOrEmpty(base.GetQueryString("TaskID")))
            {
                try
                {
                    int taskID = TranslateUtils.ToInt(base.GetQueryString("TaskID"));
                    TaskInfo taskInfo = BaiRongDataProvider.TaskDAO.GetTaskInfo(taskID);
                    taskInfo.FrequencyType = EFrequencyTypeUtils.GetEnumType(this.FrequencyType.SelectedValue);
                    if (taskInfo.FrequencyType == EFrequencyType.Period)
                    {
                        taskInfo.PeriodIntervalMinute = TranslateUtils.ToInt(this.PeriodInterval.Text) * TranslateUtils.ToInt(this.PeriodIntervalType.SelectedValue);
                    }
                    else if (taskInfo.FrequencyType != EFrequencyType.JustInTime)
                    {
                        taskInfo.StartDay = TranslateUtils.ToInt(this.StartDay.SelectedValue);
                        taskInfo.StartWeekday = TranslateUtils.ToInt(this.StartWeekday.SelectedValue);
                        taskInfo.StartHour = TranslateUtils.ToInt(this.StartHour.SelectedValue);
                    }
                    taskInfo.Description = this.Description.Text;
                    taskInfo.ServiceParameters = serviceParamters.ToString();

                    BaiRongDataProvider.TaskDAO.Update(taskInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, string.Format("修改{0}任务", EServiceTypeUtils.GetText(taskInfo.ServiceType)), string.Format("任务名称:{0}", taskInfo.TaskName));

                    base.SuccessMessage("任务修改成功！");
                    isChanged = true;
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "任务修改失败！");
                }
            }
            else
            {
                if (BaiRongDataProvider.TaskDAO.IsExists(this.TaskName.Text))
                {
                    base.FailMessage("任务添加失败，任务名称已存在！");
                }
                else
                {
                    try
                    {
                        TaskInfo taskInfo = new TaskInfo(AppManager.CMS.AppID);
                        taskInfo.TaskName = this.TaskName.Text;
                        taskInfo.PublishmentSystemID = base.PublishmentSystemID;
                        taskInfo.ServiceType = this.serviceType;
                        taskInfo.FrequencyType = EFrequencyTypeUtils.GetEnumType(this.FrequencyType.SelectedValue);
                        if (taskInfo.FrequencyType == EFrequencyType.Period)
                        {
                            taskInfo.PeriodIntervalMinute = TranslateUtils.ToInt(this.PeriodInterval.Text) * TranslateUtils.ToInt(this.PeriodIntervalType.SelectedValue);
                        }
                        else if (taskInfo.FrequencyType != EFrequencyType.JustInTime)
                        {
                            taskInfo.StartDay = TranslateUtils.ToInt(this.StartDay.SelectedValue);
                            taskInfo.StartWeekday = TranslateUtils.ToInt(this.StartWeekday.SelectedValue);
                            taskInfo.StartHour = TranslateUtils.ToInt(this.StartHour.SelectedValue);
                        }
                        taskInfo.Description = this.Description.Text;

                        taskInfo.ServiceParameters = serviceParamters.ToString();

                        taskInfo.IsEnabled = true;
                        taskInfo.AddDate = DateTime.Now;
                        taskInfo.OnlyOnceDate = DateUtils.SqlMinValue;
                        taskInfo.LastExecuteDate = DateUtils.SqlMinValue;

                        BaiRongDataProvider.TaskDAO.Insert(taskInfo);

                        StringUtility.AddLog(base.PublishmentSystemID, string.Format("添加{0}任务", EServiceTypeUtils.GetText(taskInfo.ServiceType)), string.Format("任务名称:{0}", taskInfo.TaskName));

                        base.SuccessMessage("任务添加成功！");
                        isChanged = true;
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "任务添加失败！");
                    }
                }
            }

            if (isChanged)
            {
                string redirectUrl = PageUtils.GetCMSUrl(string.Format("background_task.aspx?ServiceType={0}&PublishmentSystemID={1}", EServiceTypeUtils.GetValue(this.serviceType), base.PublishmentSystemID));
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, redirectUrl);
            }
        }
    }
}
