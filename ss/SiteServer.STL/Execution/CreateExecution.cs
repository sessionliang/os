using System;
using System.Collections;
using System.Text;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using BaiRong.Service;
using BaiRong.Service.Execution;
using BaiRong.Model.Service;
using SiteServer.STL.IO;

namespace SiteServer.STL.Execution
{
    public class CreateExecution : ExecutionBase, IExecution
    {
        public bool Execute(TaskInfo taskInfo)
        {
            base.Init();

            TaskCreateInfo taskCreateInfo = new TaskCreateInfo(taskInfo.ServiceParameters);
            if (!string.IsNullOrEmpty(taskCreateInfo.CreateTypes))
            {
                ArrayList createTypeArrayList = TranslateUtils.StringCollectionToArrayList(taskCreateInfo.CreateTypes);
                bool createChannel = createTypeArrayList.Contains(ECreateTypeUtils.GetValue(ECreateType.Channel));
                bool createContent = createTypeArrayList.Contains(ECreateTypeUtils.GetValue(ECreateType.Content));
                bool createFile = createTypeArrayList.Contains(ECreateTypeUtils.GetValue(ECreateType.File));
                if (taskInfo.PublishmentSystemID != 0)
                {
                    ArrayList nodeIDArrayList = null;
                    if (taskCreateInfo.IsCreateAll)
                    {
                        nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(taskInfo.PublishmentSystemID);
                    }
                    else
                    {
                        nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(taskCreateInfo.ChannelIDCollection);
                    }

                    CreateExecution.Create(createChannel, createContent, createFile, taskInfo, taskInfo.PublishmentSystemID, nodeIDArrayList);
                }
                else
                {
                    ArrayList publishmentSystemIDArrayList = null;
                    if (taskCreateInfo.IsCreateAll)
                    {
                        publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
                    }
                    else
                    {
                        publishmentSystemIDArrayList = TranslateUtils.StringCollectionToIntArrayList(taskCreateInfo.ChannelIDCollection);
                    }
                    foreach (int publishmentSystemID in publishmentSystemIDArrayList)
                    {
                        ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(publishmentSystemID);
                        CreateExecution.Create(createChannel, createContent, createFile, taskInfo, publishmentSystemID, nodeIDArrayList);
                    }
                }
            }
            if (taskCreateInfo.IsCreateSiteMap)
            {
                if (taskInfo.PublishmentSystemID != 0)
                {
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(taskInfo.PublishmentSystemID);
                    SeoManager.CreateSiteMapGoogle(publishmentSystemInfo, publishmentSystemInfo.Additional.VisualType);
                }
                else
                {
                    ArrayList publishmentSystemIDArrayList = null;
                    if (taskCreateInfo.IsCreateAll)
                    {
                        publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDArrayList();
                    }
                    else
                    {
                        publishmentSystemIDArrayList = TranslateUtils.StringCollectionToIntArrayList(taskCreateInfo.ChannelIDCollection);
                    }
                    foreach (int publishmentSystemID in publishmentSystemIDArrayList)
                    {
                        PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(taskInfo.PublishmentSystemID);
                        SeoManager.CreateSiteMapGoogle(publishmentSystemInfo, publishmentSystemInfo.Additional.VisualType);
                    }
                }
            }

            return true;
        }

        private static void Create(bool createChannel, bool createContent, bool createFile, TaskInfo taskInfo, int publishmentSystemID, ArrayList nodeIDArrayList)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (publishmentSystemInfo != null)
            {
                FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
                SortedList errorChannelNodeIDSortedList = new SortedList();
                SortedList errorContentNodeIDSortedList = new SortedList();
                SortedList errorFileTemplateIDSortedList = new SortedList();

                if (nodeIDArrayList != null && nodeIDArrayList.Count > 0)
                {
                    if (createChannel)
                    {
                        foreach (int nodeID in nodeIDArrayList)
                        {
                            try
                            {
                                FSO.CreateChannel(nodeID);
                            }
                            catch (Exception ex)
                            {
                                errorChannelNodeIDSortedList.Add(nodeID, ex.Message);
                            }
                        }
                        if (errorChannelNodeIDSortedList.Count > 0)
                        {
                            foreach (int nodeID in errorChannelNodeIDSortedList.Keys)
                            {
                                try
                                {
                                    FSO.CreateChannel(nodeID);
                                }
                                catch { }
                            }
                        }
                    }
                    if (createContent)
                    {
                        foreach (int nodeID in nodeIDArrayList)
                        {
                            try
                            {
                                FSO.CreateContents(nodeID);
                            }
                            catch (Exception ex)
                            {
                                errorContentNodeIDSortedList.Add(nodeID, ex.Message);
                            }
                        }
                        if (errorContentNodeIDSortedList.Count > 0)
                        {
                            foreach (int nodeID in errorContentNodeIDSortedList.Keys)
                            {
                                try
                                {
                                    FSO.CreateContents(nodeID);
                                }
                                catch { }
                            }
                        }
                    }
                }
                if (createFile)
                {
                    ArrayList templateIDArrayList = DataProvider.TemplateDAO.GetTemplateIDArrayListByType(publishmentSystemID, ETemplateType.FileTemplate);
                    foreach (int templateID in templateIDArrayList)
                    {
                        try
                        {
                            FSO.CreateFile(templateID);
                        }
                        catch (Exception ex)
                        {
                            errorFileTemplateIDSortedList.Add(templateID, ex.Message);
                        }
                    }
                    if (errorFileTemplateIDSortedList.Count > 0)
                    {
                        foreach (int templateID in errorFileTemplateIDSortedList.Keys)
                        {
                            try
                            {
                                FSO.CreateFile(templateID);
                            }
                            catch { }
                        }
                    }
                }
                if (errorChannelNodeIDSortedList.Count > 0 || errorContentNodeIDSortedList.Count > 0 || errorFileTemplateIDSortedList.Count > 0)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    foreach (int nodeID in errorChannelNodeIDSortedList.Keys)
                    {
                        errorMessage.AppendFormat("Create channel {0} error:{1}", nodeID, errorChannelNodeIDSortedList[nodeID]).Append(StringUtils.Constants.ReturnAndNewline);
                    }
                    foreach (int nodeID in errorContentNodeIDSortedList.Keys)
                    {
                        errorMessage.AppendFormat("Create content {0} error:{1}", nodeID, errorContentNodeIDSortedList[nodeID]).Append(StringUtils.Constants.ReturnAndNewline);
                    }
                    foreach (int templateID in errorFileTemplateIDSortedList.Keys)
                    {
                        errorMessage.AppendFormat("Create file {0} error:{1}", templateID, errorFileTemplateIDSortedList[templateID]).Append(StringUtils.Constants.ReturnAndNewline);
                    }
                }

                if (taskInfo.ServiceType == EServiceType.Create && taskInfo.FrequencyType == EFrequencyType.OnlyOnce)
                {
                    BaiRongDataProvider.TaskDAO.Delete(taskInfo.TaskID);
                }
            }
        }
    }
}
