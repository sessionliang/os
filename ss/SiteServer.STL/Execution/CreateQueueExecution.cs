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
using System.Net;
using BaiRong.Core.Net;

namespace SiteServer.STL.Execution
{
    public class CreateQueueExecution : ExecutionBase, IExecution
    {
        public bool Execute(TaskInfo taskInfo)
        {
            base.Init();
            for (int i = 0; i < 100; i++)
            {
                //查询一条，执行，然后删除
                AjaxUrlInfo ajaxUrlInfo = AjaxUrlManager.FetchQueueCreateUrlInfo();
                if (ajaxUrlInfo == null)
                    return true;
                if (string.IsNullOrEmpty(ajaxUrlInfo.AjaxUrl))
                {
                    try
                    {
                        BaiRongDataProvider.CreateTaskDAO.UpdateStartTime(ajaxUrlInfo.CreateTaskID);
                    }
                    catch (Exception ex)
                    {

                    }

                    bool createIndex = false;
                    bool createChannel = false;
                    bool createContent = false;
                    bool createFile = false;
                    if (ajaxUrlInfo.TemplateID > 0)
                    {
                        createFile = true;
                    }
                    else if (ajaxUrlInfo.ContentID > 0)
                    {
                        createContent = true;
                    }
                    else if (ajaxUrlInfo.NodeID > 0)
                    {
                        createChannel = true;
                    }
                    else if (ajaxUrlInfo.PublishmentSystemID > 0)
                    {
                        createIndex = true;
                    }

                    Create(createIndex, createChannel, createContent, createFile, ajaxUrlInfo.PublishmentSystemID, ajaxUrlInfo.NodeID, ajaxUrlInfo.ContentID, ajaxUrlInfo.TemplateID);

                    BaiRongDataProvider.CreateTaskDAO.UpdateState(ajaxUrlInfo.CreateTaskID);
                }
                else
                {
                    //string result = string.Empty;
                    //WebClientUtils.Post(ajaxUrlInfo.AjaxUrl, ajaxUrlInfo.Parameters, out result);
                }
            }
            return true;
        }

        private static void Create(bool createIndex, bool createChannel, bool createContent, bool createFile, int publishmentSystemID, int nodeID, int contentID, int templateID)
        {
            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            if (publishmentSystemInfo != null)
            {
                FileSystemObject FSO = new FileSystemObject(publishmentSystemID);
                SortedList errorChannelNodeIDSortedList = new SortedList();
                SortedList errorContentNodeIDSortedList = new SortedList();
                SortedList errorFileTemplateIDSortedList = new SortedList();

                if (createIndex)
                {
                    try
                    {
                        FSO.CreateIndex();
                    }
                    catch
                    {

                    }
                }
                else if (createChannel)
                {
                    try
                    {
                        FSO.CreateChannel(nodeID);
                    }
                    catch (Exception ex)
                    {
                        errorChannelNodeIDSortedList.Add(nodeID, ex.Message);
                    }
                    if (errorContentNodeIDSortedList.Count > 0)
                    {
                        foreach (int nodeErrorID in errorContentNodeIDSortedList.Keys)
                        {
                            try
                            {
                                FSO.CreateChannel(nodeErrorID);
                            }
                            catch
                            {

                            }
                        }
                    }
                }
                else if (createContent)
                {
                    try
                    {
                        ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeID);
                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeID);
                        FSO.CreateContent(tableStyle, tableName, nodeID, contentID);
                    }
                    catch (Exception ex)
                    {
                        errorContentNodeIDSortedList.Add(nodeID, ex.Message);
                    }
                    if (errorContentNodeIDSortedList.Count > 0)
                    {
                        foreach (int nodeErrorID in errorContentNodeIDSortedList.Keys)
                        {
                            try
                            {
                                FSO.CreateContents(nodeErrorID);
                            }
                            catch { }
                        }
                    }

                }
                else if (createFile)
                {

                    try
                    {
                        FSO.CreateFile(templateID);
                    }
                    catch (Exception ex)
                    {
                        errorFileTemplateIDSortedList.Add(templateID, ex.Message);
                    }

                    if (errorFileTemplateIDSortedList.Count > 0)
                    {
                        foreach (int templateErrorID in errorFileTemplateIDSortedList.Keys)
                        {
                            try
                            {
                                FSO.CreateFile(templateErrorID);
                            }
                            catch { }
                        }
                    }
                }
            }
        }
    }
}
