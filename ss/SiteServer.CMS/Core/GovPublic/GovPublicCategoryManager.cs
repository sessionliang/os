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
	public class GovPublicCategoryManager
	{
        private GovPublicCategoryManager()
		{
            
		}

        public static void Initialize(PublishmentSystemInfo publishmentSystemInfo)
        {
            if (DataProvider.GovPublicCategoryClassDAO.GetCount(publishmentSystemInfo.PublishmentSystemID) == 0)
            {
                ArrayList categoryClassInfoArrayList = new ArrayList();
                categoryClassInfoArrayList.Add(GetCategoryClassInfo(EGovPublicCategoryClassType.Channel, publishmentSystemInfo.PublishmentSystemID));
                categoryClassInfoArrayList.Add(GetCategoryClassInfo(EGovPublicCategoryClassType.Department, publishmentSystemInfo.PublishmentSystemID));
                categoryClassInfoArrayList.Add(GetCategoryClassInfo(EGovPublicCategoryClassType.Form, publishmentSystemInfo.PublishmentSystemID));
                categoryClassInfoArrayList.Add(GetCategoryClassInfo(EGovPublicCategoryClassType.Service, publishmentSystemInfo.PublishmentSystemID));

                foreach (GovPublicCategoryClassInfo categoryClassInfo in categoryClassInfoArrayList)
                {
                    DataProvider.GovPublicCategoryClassDAO.Insert(categoryClassInfo);
                }
            }
        }

        private static GovPublicCategoryClassInfo GetCategoryClassInfo(EGovPublicCategoryClassType categoryType, int publishmentSystemID)
        {
            bool isSystem = false;
            if (categoryType == EGovPublicCategoryClassType.Channel || categoryType == EGovPublicCategoryClassType.Department)
            {
                isSystem = true;
            }
            return new GovPublicCategoryClassInfo(EGovPublicCategoryClassTypeUtils.GetValue(categoryType), publishmentSystemID, EGovPublicCategoryClassTypeUtils.GetText(categoryType), isSystem, true, string.Empty, 0, string.Empty);
        }
	}
}
