using System;
using System.Collections;
using System.Text;
using BaiRong.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using BaiRong.Model.Service;
using BaiRong.Core.Service;
using BaiRong.Core.Data.Provider;
using BaiRong.Service;
using BaiRong.Service.Execution;

namespace SiteServer.STL.Execution
{
    public class GatherExecution : ExecutionBase, IExecution
    {
        public bool Execute(TaskInfo taskInfo)
        {
            base.Init();

            TaskGatherInfo taskGatherInfo = new TaskGatherInfo(taskInfo.ServiceParameters);
            if (taskInfo.PublishmentSystemID != 0)
            {
                ArrayList webGatherNames = TranslateUtils.StringCollectionToArrayList(taskGatherInfo.WebGatherNames);
                ArrayList databaseGatherNames = TranslateUtils.StringCollectionToArrayList(taskGatherInfo.DatabaseGatherNames);
                ArrayList fileGatherNames = TranslateUtils.StringCollectionToArrayList(taskGatherInfo.FileGatherNames);
                Gather(taskInfo, taskInfo.PublishmentSystemID, webGatherNames, databaseGatherNames, fileGatherNames);
            }
            else
            {
                ArrayList publishmentSystemIDArrayList = TranslateUtils.StringCollectionToIntArrayList(taskGatherInfo.PublishmentSystemIDCollection);
                foreach (int publishmentSystemID in publishmentSystemIDArrayList)
                {
                    ArrayList webGatherNames = DataProvider.GatherRuleDAO.GetGatherRuleNameArrayList(publishmentSystemID);
                    ArrayList databaseGatherNames = DataProvider.GatherDatabaseRuleDAO.GetGatherRuleNameArrayList(publishmentSystemID);
                    ArrayList fileGatherNames = DataProvider.GatherFileRuleDAO.GetGatherRuleNameArrayList(publishmentSystemID);
                    Gather(taskInfo, publishmentSystemID, webGatherNames, databaseGatherNames, fileGatherNames);
                }
            }

            return true;
        }

        private static void Gather(TaskInfo taskInfo, int publishmentSystemID, ArrayList webGatherNames, ArrayList databaseGatherNames, ArrayList fileGatherNames)
        {
            foreach (string webGatherName in webGatherNames)
            {
                StringBuilder resultBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();
                GatherUtility.GatherWeb(publishmentSystemID, webGatherName, resultBuilder, errorBuilder, false, string.Empty);
            }
            foreach (string databaseGatherName in databaseGatherNames)
            {
                StringBuilder resultBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();
                GatherUtility.GatherDatabase(publishmentSystemID, databaseGatherName, resultBuilder, errorBuilder, false, string.Empty);
            }
            foreach (string fileGatherName in fileGatherNames)
            {
                StringBuilder resultBuilder = new StringBuilder();
                StringBuilder errorBuilder = new StringBuilder();
                GatherUtility.GatherFile(publishmentSystemID, fileGatherName, resultBuilder, errorBuilder, false, string.Empty);
            }
        }
    }
}
