using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;

using BaiRong.Core.Data.Provider;

using System.Text;
using BaiRong.Model.Service;
using BaiRong.Controls;
using System.Collections.Generic;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class ContentCheck : BackgroundBasePage
    {
        public Literal ltlTitles;
        public RadioButtonList rblCheckType;
        public DropDownList ddlTranslateNodeID;
        public TextBox tbCheckReasons;

        //定时审核
        public DropDownList IsCheckTask;
        public PlaceHolder PlaceHolder_CheckTask;
        public DateTimeTextBox DateOnlyOnce;

        //定时下架
        public DropDownList IsUnCheckTask;
        public PlaceHolder PlaceHolder_UnCheckTask;
        public DateTimeTextBox DateUnCheck;

        private Hashtable idsHashtable = new Hashtable();
        private List<TaskInfo> taskInfoListToUpdate = new List<TaskInfo>();
        private List<TaskInfo> taskInfoListToDelete = new List<TaskInfo>();
        private string returnUrl;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtility.GetOpenWindowStringWithCheckBoxValue("审核内容", "modal_contentCheck.aspx", arguments, "ContentIDCollection", "请选择需要审核的内容！", 560, 550);
        }

        public static string GetOpenWindowStringForMultiChannels(int publishmentSystemID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtility.GetOpenWindowStringWithCheckBoxValue("审核内容", "modal_contentCheck.aspx", arguments, "IDsCollection", "请选择需要审核的内容！", 560, 550);
        }

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, int contentID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ContentIDCollection", contentID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtility.GetOpenWindowString("审核内容", "modal_contentCheck.aspx", arguments, 560, 550);
        }

        public static string GetLinkUrl(int publishmentSystemID, int nodeID, int contentID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));

            return PageUtils.GetCMSUrl(string.Format(@"modal_contentCheck.aspx?PublishmentSystemID={0}&NodeID={1}&ReturnUrl={2}&ContentIDCollection={3}", publishmentSystemID, nodeID, StringUtils.ValueToUrl(returnUrl), contentID));
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ReturnUrl");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));

            this.idsHashtable = ContentUtility.GetIDsHashtable(base.Request.QueryString);

            if (!IsPostBack)
            {
                int checkTaskTotal = 0;
                int checkContentTotal = 0;
                int unCheckTaskTotal = 0;
                ContentInfo contentInfo = null;
                StringBuilder titles = new StringBuilder();
                foreach (int nodeID in this.idsHashtable.Keys)
                {
                    ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeID);
                    string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
                    ArrayList contentIDArrayList = this.idsHashtable[nodeID] as ArrayList;
                    foreach (int contentID in contentIDArrayList)
                    {
                        checkContentTotal++;
                        contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                        int taskID = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(ContentAttribute.Check_TaskID));
                        int unCheckTaskID = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(ContentAttribute.UnCheck_TaskID));
                        StringBuilder title = new StringBuilder();
                        if (taskID == 0 && unCheckTaskID == 0 && contentInfo.CheckTaskDate == DateUtils.SqlMinValue && contentInfo.UnCheckTaskDate == DateUtils.SqlMinValue)
                        {
                            titles.Append(contentInfo.Title + "<br />");
                        }
                        else
                        {
                            if (taskID != 0 || unCheckTaskID != 0)
                            {
                                TaskInfo taskInfo = BaiRongDataProvider.TaskDAO.GetTaskInfo(taskID);
                                if (taskInfo != null)
                                {
                                    checkTaskTotal++;
                                    if (contentIDArrayList.Count == 1)
                                    {
                                        this.DateOnlyOnce.Text = taskInfo.OnlyOnceDate.ToString();
                                    }
                                }
                                TaskInfo unCheckTaskInfo = BaiRongDataProvider.TaskDAO.GetTaskInfo(unCheckTaskID);
                                if (unCheckTaskInfo != null)
                                {
                                    unCheckTaskTotal++;
                                    if (contentIDArrayList.Count == 1)
                                    {
                                        this.DateUnCheck.Text = unCheckTaskInfo.OnlyOnceDate.ToString();
                                    }
                                }
                                if (!contentInfo.IsChecked)
                                {

                                    if (taskInfo != null)
                                    {
                                        title.Append(contentInfo.Title + "（定时审核时间：" + DateUtils.GetDateAndTimeString(taskInfo.OnlyOnceDate) + "）<br />");
                                    }
                                    else
                                    {
                                        titles.Append(contentInfo.Title + "<br />");
                                    }
                                }
                                else
                                {
                                    if (unCheckTaskInfo != null)
                                    {
                                        title.Append(contentInfo.Title + "（定时下架时间：" + DateUtils.GetDateAndTimeString(unCheckTaskInfo.OnlyOnceDate) + "）<br />");
                                    }
                                    else
                                    {
                                        titles.Append(contentInfo.Title + "<br />");
                                    }
                                }
                            }
                            else if (contentInfo.CheckTaskDate != DateUtils.SqlMinValue && !contentInfo.IsChecked)
                            {
                                title.Append(contentInfo.Title + "（定时审核时间：" + DateUtils.GetDateAndTimeString(contentInfo.CheckTaskDate) + "）<br />");
                            }
                            else if (contentInfo.UnCheckTaskDate != DateUtils.SqlMinValue && contentInfo.IsChecked)
                            {
                                title.Append(contentInfo.Title + "（定时下架时间：" + DateUtils.GetDateAndTimeString(contentInfo.UnCheckTaskDate) + "）<br />");
                            }
                            if (title.Length > 0)
                                titles.Append(title);
                            //titles.Append(BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, ContentAttribute.Title) + "<br />");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(this.ltlTitles.Text))
                {
                    titles.Length -= 6;
                }
                this.ltlTitles.Text = titles.ToString();

                int checkedLevel = 5;
                bool isChecked = true;

                foreach (int nodeID in this.idsHashtable.Keys)
                {
                    int checkedLevelByNodeID = 0;
                    bool isCheckedByNodeID = CheckManager.GetUserCheckLevel(base.PublishmentSystemInfo, nodeID, out checkedLevelByNodeID);
                    if (checkedLevel > checkedLevelByNodeID)
                    {
                        checkedLevel = checkedLevelByNodeID;
                    }
                    if (!isCheckedByNodeID)
                    {
                        isChecked = isCheckedByNodeID;
                    }
                }

                LevelManager.LoadContentLevelToCheck(this.rblCheckType, base.PublishmentSystemInfo, isChecked, checkedLevel);

                ListItem listItem = new ListItem("<保持原栏目不变>", "0");
                this.ddlTranslateNodeID.Items.Add(listItem);

                NodeManager.AddListItemsForAddContent(this.ddlTranslateNodeID.Items, base.PublishmentSystemInfo, true);

                this.IsCheckTask.Items[0].Value = true.ToString();
                this.IsCheckTask.Items[1].Value = false.ToString();
                ControlUtils.SelectListItems(this.IsCheckTask, (checkTaskTotal == 1 && checkContentTotal == 1) ? "True" : "False");//todo

                this.IsUnCheckTask.Items[0].Value = true.ToString();
                this.IsUnCheckTask.Items[1].Value = false.ToString();
                ControlUtils.SelectListItems(this.IsUnCheckTask, (unCheckTaskTotal == 1 && checkContentTotal == 1) ? "True" : "False");

                this.IsCheckTask_SelectedIndexChanged(null, EventArgs.Empty);
                this.IsUnCheckTask_SelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        public void IsCheckTask_SelectedIndexChanged(object sender, EventArgs E)
        {
            if (EBooleanUtils.Equals(EBoolean.False, this.IsCheckTask.SelectedValue))
            {
                this.PlaceHolder_CheckTask.Visible = false;
            }
            else
            {
                this.PlaceHolder_CheckTask.Visible = true;
            }
        }

        public void IsUnCheckTask_SelectedIndexChanged(object sender, EventArgs E)
        {
            if (EBooleanUtils.Equals(EBoolean.False, this.IsUnCheckTask.SelectedValue))
            {
                this.PlaceHolder_UnCheckTask.Visible = false;
            }
            else
            {
                this.PlaceHolder_UnCheckTask.Visible = true;
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            int taskID = 0;
            int checkedLevel = TranslateUtils.ToIntWithNagetive(this.rblCheckType.SelectedValue);
            bool isChecked = false;
            bool isTask = false;

            if (checkedLevel >= base.PublishmentSystemInfo.CheckContentLevel)
            {
                isChecked = true;
            }
            else
            {
                isChecked = false;
            }

            ArrayList contentInfoArrayListToCheck = new ArrayList();
            Hashtable idsHashtableToCheck = new Hashtable();
            foreach (int nodeID in this.idsHashtable.Keys)
            {
                ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeID);
                string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
                ArrayList contentIDArrayList = this.idsHashtable[nodeID] as ArrayList;
                ArrayList contentIDArrayListToCheck = new ArrayList();

                int checkedLevelOfUser = 0;
                bool isCheckedOfUser = CheckManager.GetUserCheckLevel(base.PublishmentSystemInfo, nodeID, out checkedLevelOfUser);

                foreach (int contentID in contentIDArrayList)
                {
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                    if (contentInfo != null)
                    {

                        if (LevelManager.IsCheckable(base.PublishmentSystemInfo, contentInfo.NodeID, contentInfo.IsChecked, contentInfo.CheckedLevel, isCheckedOfUser, checkedLevelOfUser))
                        {
                            contentInfoArrayListToCheck.Add(contentInfo);
                            contentIDArrayListToCheck.Add(contentID);
                        }
                        taskID = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(ContentAttribute.Check_TaskID));
                        if (taskID != 0)
                        {
                            TaskInfo taskInfo = BaiRongDataProvider.TaskDAO.GetTaskInfo(taskID);
                            if (taskInfo != null)
                            {
                                taskInfoListToUpdate.Add(taskInfo);
                                taskInfoListToDelete.Add(taskInfo);
                            }
                        }
                        if (EBooleanUtils.Equals(EBoolean.False, this.IsCheckTask.SelectedValue))
                        {
                            contentInfo.SetExtendedAttribute(ContentAttribute.Check_TaskID, "0");
                            contentInfo.SetExtendedAttribute(ContentAttribute.Check_IsTask, "False");
                        }
                        if (EBooleanUtils.Equals(EBoolean.False, this.IsUnCheckTask.SelectedValue))
                        {
                            contentInfo.SetExtendedAttribute(ContentAttribute.UnCheck_TaskID, "0");
                            contentInfo.SetExtendedAttribute(ContentAttribute.UnCheck_IsTask, "False");
                        }
                        DataProvider.ContentDAO.Update(tableName, PublishmentSystemInfo, contentInfo);

                    }
                }
                if (contentIDArrayListToCheck.Count > 0)
                {
                    idsHashtableToCheck[nodeID] = contentIDArrayListToCheck;
                }
            }

            if (contentInfoArrayListToCheck.Count == 0)
            {
                JsUtils.OpenWindow.CloseModalPageWithoutRefresh(Page, "alert('您的审核权限不足，无法审核所选内容！');");
            }
            else
            {
                try
                {
                    int translateNodeID = TranslateUtils.ToInt(this.ddlTranslateNodeID.SelectedValue);

                    if (EBooleanUtils.Equals(EBoolean.False, this.IsCheckTask.SelectedValue) && EBooleanUtils.Equals(EBoolean.False, this.IsUnCheckTask.SelectedValue))
                    {
                        foreach (int nodeID in idsHashtableToCheck.Keys)
                        {
                            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
                            ArrayList contentIDArrayList = idsHashtableToCheck[nodeID] as ArrayList;
                            BaiRongDataProvider.ContentDAO.UpdateIsChecked(tableName, base.PublishmentSystemID, nodeID, contentIDArrayList, translateNodeID, true, AdminManager.Current.UserName, isChecked, checkedLevel, this.tbCheckReasons.Text);

                            DataProvider.NodeDAO.UpdateContentNum(base.PublishmentSystemInfo, nodeID, true);
                        }

                        if (translateNodeID > 0)
                        {
                            DataProvider.NodeDAO.UpdateContentNum(base.PublishmentSystemInfo, translateNodeID, true);
                        }

                        StringUtility.AddLog(base.PublishmentSystemID, base.PublishmentSystemID, 0, "设置内容状态为" + this.rblCheckType.SelectedItem.Text, this.tbCheckReasons.Text);

                        this.Redirect(isChecked, idsHashtableToCheck);
                    }
                    else
                    {
                        //定时任务
                        TaskInfo taskInfo = new TaskInfo(AppManager.CMS.AppID);
                        TaskCheckInfo taskCheckInfo = new TaskCheckInfo(string.Empty);
                        foreach (int nodeID in idsHashtableToCheck.Keys)
                        {
                            ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeID);
                            string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
                            ArrayList contentIDArrayList = idsHashtableToCheck[nodeID] as ArrayList;
                            ExtendedAttributes serviceParamters = new ExtendedAttributes();
                            if (contentIDArrayList != null && contentIDArrayList.Count > 0)
                            {
                                for (int i = 0; i < contentIDArrayList.Count; i++)
                                {
                                    int contentID = TranslateUtils.ToInt(contentIDArrayList[i].ToString());
                                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                                    if (contentInfo != null)
                                    {
                                        //与内容的当前状态不一致 && 内容已经状态监测为false
                                        //if ((contentInfo.IsChecked != isChecked || (contentInfo.IsChecked && contentInfo.CheckedLevel == checkedLevel) || (!contentInfo.IsChecked && contentInfo.CheckedLevel != checkedLevel)))
                                        //{
                                        try
                                        {
                                            taskCheckInfo.NodeID = nodeID;
                                            taskCheckInfo.ContentID = contentID;
                                            taskCheckInfo.CheckedLevel = checkedLevel;
                                            taskCheckInfo.CheckType = this.rblCheckType.SelectedItem.Text;
                                            taskCheckInfo.TranslateNodeID = translateNodeID;
                                            taskCheckInfo.CheckReasons = this.tbCheckReasons.Text;
                                            taskCheckInfo.UserName = AdminManager.Current.UserName;
                                            serviceParamters = taskCheckInfo;

                                            taskInfo.TaskName = contentInfo.Title + "定时审核任务";
                                            taskInfo.PublishmentSystemID = base.PublishmentSystemID;
                                            taskInfo.ServiceType = EServiceType.Check;
                                            taskInfo.FrequencyType = EFrequencyType.OnlyOnce;

                                            if (TranslateUtils.ToBool(this.IsCheckTask.SelectedValue) && !TranslateUtils.ToBool(contentInfo.GetExtendedAttribute(ContentAttribute.Check_IsTask)))
                                            {
                                                taskInfo.OnlyOnceDate = TranslateUtils.ToDateTime(this.DateOnlyOnce.Text);
                                                taskInfo.ServiceParameters = serviceParamters.ToString();

                                                taskInfo.IsEnabled = true;
                                                taskInfo.AddDate = DateTime.Now;
                                                taskInfo.LastExecuteDate = DateUtils.SqlMinValue;

                                                taskID = BaiRongDataProvider.TaskDAO.Insert(taskInfo);
                                                StringUtility.AddLog(base.PublishmentSystemID, string.Format("添加{0}任务", EServiceTypeUtils.GetText(taskInfo.ServiceType)), string.Format("任务名称:{0}", taskInfo.TaskName));

                                                isTask = true;

                                                contentInfo.SetExtendedAttribute(ContentAttribute.Check_IsTask, isTask.ToString());
                                                contentInfo.SetExtendedAttribute(ContentAttribute.Check_TaskID, taskID.ToString());

                                                DataProvider.ContentDAO.Update(tableName, base.PublishmentSystemInfo, contentInfo);
                                            }
                                            else
                                            {
                                                //修改
                                                int checkTaskId = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(ContentAttribute.Check_TaskID));
                                                taskInfo = BaiRongDataProvider.TaskDAO.GetTaskInfo(checkTaskId);
                                                if (taskInfo != null)
                                                {
                                                    taskInfo.OnlyOnceDate = TranslateUtils.ToDateTime(this.DateOnlyOnce.Text);
                                                    taskInfo.ServiceParameters = serviceParamters.ToString();

                                                    BaiRongDataProvider.TaskDAO.Update(taskInfo);
                                                    StringUtility.AddLog(base.PublishmentSystemID, string.Format("修改{0}任务", EServiceTypeUtils.GetText(taskInfo.ServiceType)), string.Format("任务名称:{0}", taskInfo.TaskName));
                                                }
                                            }

                                            //下架任务
                                            taskCheckInfo.NodeID = nodeID;
                                            taskCheckInfo.ContentID = contentID;
                                            taskCheckInfo.CheckedLevel = checkedLevel;
                                            taskCheckInfo.CheckType = this.rblCheckType.SelectedItem.Text;
                                            taskCheckInfo.TranslateNodeID = translateNodeID;
                                            taskCheckInfo.CheckReasons = this.tbCheckReasons.Text;
                                            taskCheckInfo.UserName = AdminManager.Current.UserName;
                                            serviceParamters = taskCheckInfo;

                                            taskInfo.TaskName = contentInfo.Title + "定时下架任务";
                                            taskInfo.PublishmentSystemID = base.PublishmentSystemID;
                                            taskInfo.ServiceType = EServiceType.UnCheck;
                                            taskInfo.FrequencyType = EFrequencyType.OnlyOnce;

                                            if (TranslateUtils.ToBool(this.IsUnCheckTask.SelectedValue) && !TranslateUtils.ToBool(contentInfo.GetExtendedAttribute(ContentAttribute.UnCheck_IsTask)))
                                            {

                                                taskInfo = new TaskInfo(AppManager.CMS.AppID);
                                                taskCheckInfo = new TaskCheckInfo(string.Empty);

                                                taskInfo.OnlyOnceDate = TranslateUtils.ToDateTime(this.DateUnCheck.Text);
                                                taskInfo.ServiceParameters = serviceParamters.ToString();

                                                taskInfo.IsEnabled = true;
                                                taskInfo.AddDate = DateTime.Now;
                                                taskInfo.LastExecuteDate = DateUtils.SqlMinValue;
                                                taskID = BaiRongDataProvider.TaskDAO.Insert(taskInfo);

                                                StringUtility.AddLog(base.PublishmentSystemID, string.Format("添加{0}任务", EServiceTypeUtils.GetText(taskInfo.ServiceType)), string.Format("任务名称:{0}", taskInfo.TaskName));

                                                isTask = true;

                                                contentInfo.SetExtendedAttribute(ContentAttribute.UnCheck_IsTask, isTask.ToString());
                                                contentInfo.SetExtendedAttribute(ContentAttribute.UnCheck_TaskID, taskID.ToString());

                                                DataProvider.ContentDAO.Update(tableName, base.PublishmentSystemInfo, contentInfo);
                                            }
                                            else
                                            {
                                                //修改
                                                int unCheckTaskId = TranslateUtils.ToInt(contentInfo.GetExtendedAttribute(ContentAttribute.UnCheck_TaskID));
                                                taskInfo = BaiRongDataProvider.TaskDAO.GetTaskInfo(unCheckTaskId);
                                                if (taskInfo != null)
                                                {
                                                    taskInfo.OnlyOnceDate = TranslateUtils.ToDateTime(this.DateUnCheck.Text);
                                                    taskInfo.ServiceParameters = serviceParamters.ToString();

                                                    BaiRongDataProvider.TaskDAO.Update(taskInfo);
                                                    StringUtility.AddLog(base.PublishmentSystemID, string.Format("修改{0}任务", EServiceTypeUtils.GetText(taskInfo.ServiceType)), string.Format("任务名称:{0}", taskInfo.TaskName));
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                        //}
                                    }
                                }
                            }
                        }
                    }
                    this.UpdateTaskInfos(taskInfoListToUpdate);
                    JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "操作失败！");
                }
            }
        }

        private void UpdateTaskInfos(List<TaskInfo> taskInfoListToUpdate)
        {
            if (taskInfoListToUpdate.Count > 0)
            {
                foreach (TaskInfo taskInfo in taskInfoListToUpdate)
                {
                    try
                    {
                        taskInfo.OnlyOnceDate = TranslateUtils.ToDateTime(this.DateOnlyOnce.Text);
                        BaiRongDataProvider.TaskDAO.Update(taskInfo);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        private void RemeveTaskInfos(List<TaskInfo> taskInfoListToDelete)
        {
            if (taskInfoListToDelete.Count > 0)
            {
                foreach (TaskInfo taskInfo in taskInfoListToDelete)
                {
                    try
                    {
                        BaiRongDataProvider.TaskDAO.Delete(taskInfo.TaskID);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        private void Redirect(bool isChecked, Hashtable idsHashtableToCheck)
        {
            if (isChecked)
            {
                ArrayList idsArrayList = new ArrayList();
                foreach (int nodeID in idsHashtableToCheck.Keys)
                {
                    ArrayList contentIDArrayList = idsHashtable[nodeID] as ArrayList;
                    if (contentIDArrayList != null)
                    {
                        foreach (int contentID in contentIDArrayList)
                        {
                            idsArrayList.Add(string.Format("{0}_{1}", nodeID, contentID));
                        }
                    }
                }
                string redirectUrl = Modal.ProgressBar.GetRedirectUrlStringWithCreateByIDsCollection(base.PublishmentSystemID, TranslateUtils.ObjectCollectionToString(idsArrayList));

                PageUtils.Redirect(redirectUrl);
            }
            else
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
            }
        }

    }
}
