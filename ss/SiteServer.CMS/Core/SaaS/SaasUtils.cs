using BaiRong.Core;
using BaiRong.Core.Integration;
using BaiRong.Model;
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.CMS.Core
{
    public class SaasUtils
    {
        public static bool Sync(IntegrationCloudInfo cloudInfo, out string url)
        {
            url = string.Empty;
            bool isToSync = false;

            string token = IntegrationManager.GetIntegrationToken(AdminManager.Current.UserName);
            List<IntegrationClientInfo> clientInfoList = new List<IntegrationClientInfo>();

            List<int> clientPublishmentSystemIDList = ProductPermissionsManager.Current.PublishmentSystemIDList.ToList<int>();
            foreach (int clientPublishmentSystemID in clientPublishmentSystemIDList)
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(clientPublishmentSystemID);
                IntegrationClientInfo clientInfo = new IntegrationClientInfo(publishmentSystemInfo.PublishmentSystemID, EPublishmentSystemTypeUtils.GetValue(publishmentSystemInfo.PublishmentSystemType), publishmentSystemInfo.PublishmentSystemName);
                clientInfoList.Add(clientInfo);
            }
            clientPublishmentSystemIDList = clientPublishmentSystemIDList.OrderByDescending(s=>s).ToList();

            List<int> cloudPublishmentSystemIDList = new List<int>();
            foreach (AuthPublishmentSystemInfo authPublishmentSystemInfo in cloudInfo.PublishmentSystemInfoList)
            {
                if (authPublishmentSystemInfo.PublishmentSystemID > 0 && PublishmentSystemManager.IsExists(authPublishmentSystemInfo.PublishmentSystemID))
                {
                    cloudPublishmentSystemIDList.Add(authPublishmentSystemInfo.PublishmentSystemID);
                }
            }
            cloudPublishmentSystemIDList = cloudPublishmentSystemIDList.OrderByDescending(s=>s).ToList();

            if (TranslateUtils.ObjectCollectionToString(clientPublishmentSystemIDList) != TranslateUtils.ObjectCollectionToString(cloudPublishmentSystemIDList))
            {
                isToSync = true;
            }
            else
            {
                foreach (AuthPublishmentSystemInfo authPublishmentSystemInfo in cloudInfo.PublishmentSystemInfoList)
                {
                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(authPublishmentSystemInfo.PublishmentSystemID);

                    if (publishmentSystemInfo == null || publishmentSystemInfo.PublishmentSystemName != authPublishmentSystemInfo.PublishmentSystemName || publishmentSystemInfo.PublishmentSystemType != EPublishmentSystemTypeUtils.GetEnumType(authPublishmentSystemInfo.PublishmentSystemType))
                    {
                        isToSync = true;
                        break;
                    }
                }
            }

            if (isToSync)
            {
                IntegrationManager.API_GEXIA_COM.IntegrationSync(token, clientInfoList, out url);
            }

            //if (cloudInfo.PublishmentSystemInfoList != null && cloudInfo.PublishmentSystemInfoList.Count > 0)
            //{
            //    int existsAuthPublishmentSystemCount = 0;
            //    foreach (AuthPublishmentSystemInfo authPublishmentSystemInfo in cloudInfo.PublishmentSystemInfoList)
            //    {
            //        if (!publishmentSystemIDList.Contains(authPublishmentSystemInfo.PublishmentSystemID))
            //        {
            //            authPublishmentSystemInfo.PublishmentSystemID = 0;
            //        }
            //        if (authPublishmentSystemInfo.PublishmentSystemID > 0)
            //        {
            //            existsAuthPublishmentSystemCount++;
            //        }
            //    }

            //    if (publishmentSystemIDList.Count != existsAuthPublishmentSystemCount)
            //    {
            //        List<IntegrationClientInfo> clientInfoList = new List<IntegrationClientInfo>();

            //        if (publishmentSystemIDList.Count > 0)
            //        {
            //            foreach (int publishmentSystemID in publishmentSystemIDList)
            //            {
            //                bool isExists = false;
            //                foreach (AuthPublishmentSystemInfo authPublishmentSystemInfo in cloudInfo.PublishmentSystemInfoList)
            //                {
            //                    if (authPublishmentSystemInfo.PublishmentSystemID == publishmentSystemID)
            //                    {
            //                        isExists = true;
            //                        break;
            //                    }
            //                }

            //                if (!isExists)
            //                {
            //                    int authPublishmentSystemID = 0;
            //                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
            //                    if (publishmentSystemInfo != null)
            //                    {
            //                        foreach (AuthPublishmentSystemInfo authPublishmentSystemInfo in cloudInfo.PublishmentSystemInfoList)
            //                        {
            //                            foreach (IntegrationClientInfo clientInfo in clientInfoList)
            //                            {
            //                                if (clientInfo.ID != authPublishmentSystemInfo.ID)
            //                                {
            //                                    if (!authPublishmentSystemIDWithPublishmentSystemID.ContainsKey(authPublishmentSystemInfo.ID) && authPublishmentSystemInfo.PublishmentSystemID == 0 && EPublishmentSystemTypeUtils.GetEnumType(authPublishmentSystemInfo.PublishmentSystemType) == publishmentSystemInfo.PublishmentSystemType)
            //                                    {
            //                                        authPublishmentSystemID = authPublishmentSystemInfo.ID;
            //                                        break;
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }

            //                    if (authPublishmentSystemID > 0)
            //                    {
            //                        authPublishmentSystemIDWithPublishmentSystemID.Add(authPublishmentSystemID, publishmentSystemID);
            //                    }
            //                }
            //            }
            //        }

            //        if (authPublishmentSystemIDWithPublishmentSystemID.Count > 0)
            //        {
            //            IntegrationManager.API_GEXIA_COM.IntegrationSync(token, authPublishmentSystemIDWithPublishmentSystemID);
            //        }
            //    }
            //}

            return isToSync;
        }
    }
}
