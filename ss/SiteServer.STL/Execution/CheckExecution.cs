using System;
using System.Collections;
using System.Text;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Model.Service;
using BaiRong.Core.Service;
using BaiRong.Core.Data.Provider;
using BaiRong.Service;
using BaiRong.Service.Execution;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.STL.IO;

namespace SiteServer.STL.Execution
{
    public class CheckExecution : ExecutionBase, IExecution
    {
        public bool Execute(TaskInfo taskInfo)
        {
            base.Init();

            TaskCheckInfo taskCheckInfo = new TaskCheckInfo(taskInfo.ServiceParameters);
            if (taskInfo.PublishmentSystemID != 0)
            {
                if (taskCheckInfo != null)
                {
                    int nodeID = taskCheckInfo.NodeID;
                    int contentID = taskCheckInfo.ContentID;
                    int checkedLevel = taskCheckInfo.CheckedLevel;
                    int translateNodeID = taskCheckInfo.TranslateNodeID;
                    string userName = taskCheckInfo.UserName;
                    string checkType = taskCheckInfo.CheckType;
                    string reasons = taskCheckInfo.CheckReasons;
                    string afterCheckType = taskCheckInfo.AfterCheckType;
                    string publishServiceParameters = taskCheckInfo.PublishServiceParameters;
                    string channelIDCollection = taskCheckInfo.ChannelIDCollection;
                    bool isAll = taskCheckInfo.IsAll;
                    CheckExecution.Check(taskInfo, taskInfo.PublishmentSystemID, nodeID, contentID, checkedLevel, checkType, translateNodeID, reasons, userName, afterCheckType, publishServiceParameters, channelIDCollection, isAll);
                }
            }

            return true;

        }

        private static void Check(TaskInfo taskInfo, int publishmentSystemID, int nodeID, int contentID, int checkedLevel, string checkType, int translateNodeID, string reasons, string userName, string afterCheckType, string publishServiceParameters, string channelIDCollection, bool isAll)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
            ArrayList contentIDArrayList = new ArrayList();

            if (publishmentSystemInfo != null)
            {

                if (nodeID > 0 && contentID > 0)
                {
                    ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeID);
                    string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                    if (contentInfo != null)
                    {
                        bool isChecked = false;
                        if (checkedLevel >= publishmentSystemInfo.CheckContentLevel)
                        {
                            isChecked = true;
                        }
                        else
                        {
                            isChecked = false;
                        }

                        contentIDArrayList.Add(contentInfo.ID);
                        try
                        {
                            BaiRongDataProvider.ContentDAO.UpdateIsChecked(tableName, publishmentSystemID, nodeID, contentIDArrayList, translateNodeID, true, userName, isChecked, checkedLevel, reasons);
                            contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                            if (translateNodeID > 0)
                            {
                                DataProvider.NodeDAO.UpdateContentNum(publishmentSystemInfo, translateNodeID, true);
                            }

                            DataProvider.NodeDAO.UpdateContentNum(publishmentSystemInfo, nodeID, true);

                            FSO.CreateImmediately(EChangedType.Check, ETemplateType.ContentTemplate, nodeID, contentID, 0);

                            //todo
                            ArrayList arrayList = new ArrayList();
                            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByChildNodeID(publishmentSystemID, nodeID, arrayList);
                            FSO.CreateChannelsTaskByChildNodeID(publishmentSystemID, TranslateUtils.ObjectCollectionToString(nodeIDArrayList));

                            StringUtility.AddLog(publishmentSystemID, nodeID, contentID, "设置内容状态为" + checkType, reasons);
                            if (taskInfo.ServiceType == EServiceType.Check && taskInfo.FrequencyType == EFrequencyType.OnlyOnce)
                            {
                                BaiRongDataProvider.TaskDAO.Delete(taskInfo.TaskID);
                                contentInfo.SetExtendedAttribute(ContentAttribute.Check_IsTask, "false");
                                DataProvider.ContentDAO.Update(tableName, publishmentSystemInfo, contentInfo);
                            }
                        }
                        catch { }
                    }
                }
                else if (!string.IsNullOrEmpty(channelIDCollection))
                {
                    ArrayList channelIDArray = new ArrayList();
                    if (isAll)
                        channelIDArray = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(taskInfo.PublishmentSystemID);
                    else
                        channelIDArray = TranslateUtils.StringCollectionToArrayList(channelIDCollection);
                    foreach (var channelID in channelIDArray)
                    {
                        int channelIDInt = TranslateUtils.ToInt(channelID.ToString());

                        ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, channelIDInt);
                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, channelIDInt);
                        ArrayList contentIDArray = BaiRongDataProvider.ContentDAO.GetContentIDArrayListCheck(publishmentSystemID, channelIDInt, tableName);
                        if (contentIDArray.Count == 0)
                            continue;
                        BaiRongDataProvider.ContentDAO.UpdateIsChecked(tableName, publishmentSystemID, channelIDInt, contentIDArray, translateNodeID, true, userName, true, checkedLevel, reasons, true);
                        FSO.CreateContents(channelIDInt);
                        //todo
                        ArrayList arrayList = new ArrayList();
                        ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByChildNodeID(publishmentSystemID, channelIDInt, arrayList);
                        FSO.CreateChannelsTaskByChildNodeID(publishmentSystemID, TranslateUtils.ObjectCollectionToString(nodeIDArrayList));

                        StringUtility.AddLog(publishmentSystemID, nodeID, 0, "设置内容状态", "批量设置内容【ID:" + TranslateUtils.ObjectCollectionToString(contentIDArray) + "】状态为已审核");
                    }
                }

                //是否发布
                if (EAfterCheckTypeUtils.Equals(EAfterCheckType.Publish, afterCheckType))
                {
                    PublishExecution pe = new PublishExecution();
                    TaskInfo ti = new TaskInfo(AppManager.CMS.AppID);
                    ti.ServiceParameters = publishServiceParameters;
                    pe.Execute(ti);
                }

            }
        }

    }
}
