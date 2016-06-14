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
    public class UnCheckExecution : ExecutionBase, IExecution
    {
        public bool Execute(TaskInfo taskInfo)
        {
            base.Init();

            TaskCheckInfo taskUnCheckInfo = new TaskCheckInfo(taskInfo.ServiceParameters);
            if (taskInfo.PublishmentSystemID != 0)
            {
                if (taskUnCheckInfo != null)
                {
                    int nodeID = taskUnCheckInfo.NodeID;
                    int contentID = taskUnCheckInfo.ContentID;
                    int checkedLevel = taskUnCheckInfo.CheckedLevel;
                    int translateNodeID = taskUnCheckInfo.TranslateNodeID;
                    string userName = taskUnCheckInfo.UserName;
                    string checkType = taskUnCheckInfo.CheckType;
                    string reasons = taskUnCheckInfo.CheckReasons;
                    string afterUnCheckType = taskUnCheckInfo.AfterCheckType;
                    string publishServiceParameters = taskUnCheckInfo.PublishServiceParameters;
                    string channelIDCollection = taskUnCheckInfo.ChannelIDCollection;
                    bool isAll = taskUnCheckInfo.IsAll;
                    UnCheckExecution.UnCheck(taskInfo, taskInfo.PublishmentSystemID, nodeID, contentID, checkedLevel, checkType, translateNodeID, reasons, userName, afterUnCheckType, publishServiceParameters, channelIDCollection, isAll);
                }
            }

            return true;

        }

        private static void UnCheck(TaskInfo taskInfo, int publishmentSystemID, int nodeID, int contentID, int checkedLevel, string checkType, int translateNodeID, string reasons, string userName, string afterUnCheckType, string publishServiceParameters, string channelIDCollection, bool isAll)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            ArrayList contentIDArrayList = new ArrayList();
            FileSystemObject FSO = new FileSystemObject(publishmentSystemID);

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
                        checkedLevel = 1;//´ýÉóºË
                        contentIDArrayList.Add(contentInfo.ID);
                        try
                        {

                            //ÉèÖÃ´ýÉóºË
                            BaiRongDataProvider.ContentDAO.UpdateIsChecked(tableName, publishmentSystemID, nodeID, contentIDArrayList, translateNodeID, true, userName, isChecked, checkedLevel, reasons);
                            contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                            if (translateNodeID > 0)
                            {
                                DataProvider.NodeDAO.UpdateContentNum(publishmentSystemInfo, translateNodeID, true);
                            }

                            DataProvider.NodeDAO.UpdateContentNum(publishmentSystemInfo, nodeID, true);

                            //É¾³ýÒ³Ãæ
                            string contentNavigateUrl = PageUtility.GetContentUrl(publishmentSystemInfo, contentInfo, EVisualType.Static);
                            FileUtils.DeleteFileIfExists(PathUtils.MapPath(contentNavigateUrl));

                            //todo
                            ArrayList arrayList = new ArrayList();
                            ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByChildNodeID(publishmentSystemID, nodeID, arrayList);
                            FSO.CreateChannelsTaskByChildNodeID(publishmentSystemID, TranslateUtils.ObjectCollectionToString(nodeIDArrayList));

                            StringUtility.AddLog(publishmentSystemID, nodeID, contentID, "ÉèÖÃÄÚÈÝ×´Ì¬Îª" + checkType, reasons);

                            if (taskInfo.ServiceType == EServiceType.UnCheck && taskInfo.FrequencyType == EFrequencyType.OnlyOnce)
                            {
                                BaiRongDataProvider.TaskDAO.Delete(taskInfo.TaskID);
                                contentInfo.SetExtendedAttribute(ContentAttribute.UnCheck_IsTask, "false");
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
                    foreach (int channelID in channelIDArray)
                    {
                        ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, channelID);
                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, channelID);
                        ArrayList contentIDArray = BaiRongDataProvider.ContentDAO.GetContentIDArrayListUnCheck(publishmentSystemID, channelID, tableName);
                        if (contentIDArray.Count == 0)
                            continue;
                        BaiRongDataProvider.ContentDAO.UpdateIsChecked(tableName, publishmentSystemID, channelID, contentIDArray, translateNodeID, true, userName, false, 1, reasons, false);
                        FSO.CreateContents(channelID);
                        //todo
                        ArrayList arrayList = new ArrayList();
                        ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByChildNodeID(publishmentSystemID, channelID, arrayList);
                        FSO.CreateChannelsTaskByChildNodeID(publishmentSystemID, TranslateUtils.ObjectCollectionToString(nodeIDArrayList));

                        StringUtility.AddLog(publishmentSystemID, nodeID, 0, "ÉèÖÃÄÚÈÝ×´Ì¬", "ÅúÁ¿ÉèÖÃÄÚÈÝ¡¾ID:" + TranslateUtils.ObjectCollectionToString(contentIDArray) + "¡¿×´Ì¬Îª´ýÉóºË");
                    }
                }
            }
        }
    }
}
