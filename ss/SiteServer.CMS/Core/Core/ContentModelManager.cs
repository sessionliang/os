using System;
using System.Text;
using System.Xml;
using System.Collections;
using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.IO;
using BaiRong.Core.Net;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;

namespace SiteServer.CMS.Core
{
    public class ContentModelManager
    {
        private ContentModelManager()
        {
        }

        public static ContentModelInfo GetContentModelInfo(PublishmentSystemInfo publishmentSystemInfo, string contentModelID)
        {
            ContentModelInfo retval = null;
            ArrayList arraylist = GetContentModelArrayList(publishmentSystemInfo);
            foreach (ContentModelInfo modelInfo in arraylist)
            {
                if (modelInfo.ModelID == contentModelID)
                {
                    retval = modelInfo;
                    break;
                }
            }
            if (retval == null)
            {
                retval = EContentModelTypeUtils.GetContentModelInfo(publishmentSystemInfo.PublishmentSystemType, publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.AuxiliaryTableForContent, EContentModelType.Content);
            }
            return retval;
        }

        public static ArrayList GetContentModelArrayList(PublishmentSystemInfo publishmentSystemInfo)
        {
            ArrayList arraylist = new ArrayList();

            arraylist.Add(EContentModelTypeUtils.GetContentModelInfo(publishmentSystemInfo.PublishmentSystemType, publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.AuxiliaryTableForContent, EContentModelType.Content));

            arraylist.Add(EContentModelTypeUtils.GetContentModelInfo(publishmentSystemInfo.PublishmentSystemType, publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.AuxiliaryTableForContent, EContentModelType.Photo));

            if (publishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.CMS)
            {
                arraylist.Add(EContentModelTypeUtils.GetContentModelInfo(publishmentSystemInfo.PublishmentSystemType, publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.AuxiliaryTableForContent, EContentModelType.Teleplay));
            }

            if (EPublishmentSystemTypeUtils.IsB2C(publishmentSystemInfo.PublishmentSystemType))
            {
                arraylist.Add(EContentModelTypeUtils.GetContentModelInfo(publishmentSystemInfo.PublishmentSystemType, publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.AuxiliaryTableForGoods, EContentModelType.Goods));

                arraylist.Add(EContentModelTypeUtils.GetContentModelInfo(publishmentSystemInfo.PublishmentSystemType, publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.AuxiliaryTableForBrand, EContentModelType.Brand));
            }
            else if (publishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.WCM)
            {
                arraylist.Add(EContentModelTypeUtils.GetContentModelInfo(publishmentSystemInfo.PublishmentSystemType, publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.AuxiliaryTableForGovPublic, EContentModelType.GovPublic));

                arraylist.Add(EContentModelTypeUtils.GetContentModelInfo(publishmentSystemInfo.PublishmentSystemType, publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.AuxiliaryTableForGovInteract, EContentModelType.GovInteract));
            }

            arraylist.Add(EContentModelTypeUtils.GetContentModelInfo(publishmentSystemInfo.PublishmentSystemType, publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.AuxiliaryTableForVote, EContentModelType.Vote));

            arraylist.Add(EContentModelTypeUtils.GetContentModelInfo(publishmentSystemInfo.PublishmentSystemType, publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.AuxiliaryTableForJob, EContentModelType.Job));

            arraylist.AddRange(ContentModelUtils.GetContentModelArrayList(AppManager.CMS.AppID, publishmentSystemInfo.PublishmentSystemID));

            return arraylist;
        }
    }
}
