using BaiRong.Core;
using BaiRong.Model;
using Senparc.Weixin.MP;
using SiteServer.API.Model.WX;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;



namespace SiteServer.API.Controllers.WX
{
    public class WX_StoreController : ApiController
    {
        [HttpGet]
        [ActionName("GetStoreCategory")]
        public IHttpActionResult GetStoreCategory(int id)
        {
            try
            {
                string wxOpenID = RequestUtils.GetQueryString("wxOpenID");
                int parentID = RequestUtils.GetIntQueryString("parentID");

                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                string poweredBy = string.Empty;
                bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

                DataProviderWX.StoreDAO.AddPVCount(id);

                string title = DataProviderWX.StoreDAO.GetTitle(id);

                List<StoreCategoryInfo> categoryInfoList = DataProviderWX.StoreCategoryDAO.GetStoreCategoryInfoList(publishmentSystemInfo.PublishmentSystemID, parentID);
                List<StoreCategoryInfo> contentInfoList = new List<StoreCategoryInfo>();

                foreach (StoreCategoryInfo categoryInfo in categoryInfoList)
                {
                    categoryInfo.StoreCount = DataProviderWX.StoreItemDAO.GetAllCount(publishmentSystemInfo.PublishmentSystemID, categoryInfo.ID);
                    if (categoryInfo.StoreCount > 0)
                    {
                        contentInfoList.Add(categoryInfo);
                    }
                }

                StoreCategoryParameter parameter = new StoreCategoryParameter { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, Title = title, ContentInfoList = contentInfoList };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                StoreCategoryParameter parameter = new StoreCategoryParameter { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = true, PoweredBy = string.Empty };
                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("GetStoreItemList")]
        public IHttpActionResult GetStoreItemList(int id)
        {
            try
            {
                int categoryID = RequestUtils.GetIntQueryString("categoryID");
                string wxOpenID = RequestUtils.GetQueryString("wxOpenID");

                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                string poweredBy = string.Empty;
                bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

                string title = DataProviderWX.StoreDAO.GetTitle(id);

                List<StoreItemInfo> contentInfoList = DataProviderWX.StoreItemDAO.GetStoreItemInfoListByCategoryID(publishmentSystemInfo.PublishmentSystemID, categoryID);

                foreach (StoreItemInfo contentInfo in contentInfoList)
                {
                    contentInfo.ImageUrl = StoreManager.GetImageUrl(publishmentSystemInfo, contentInfo.ImageUrl);
                }

                StoreItemParameter parameter = new StoreItemParameter { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, Title = title, ContentInfoList = contentInfoList };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                StoreItemParameter parameter = new StoreItemParameter { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = true, PoweredBy = string.Empty };
                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("GetStoreItemInfo")]
        public IHttpActionResult GetStoreItemInfo(int id)
        {
            try
            {
                int storeItemID = RequestUtils.GetIntQueryString("storeItemID");
                string wxOpenID = RequestUtils.GetQueryString("wxOpenID");

                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                string poweredBy = string.Empty;
                bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

                string title = DataProviderWX.StoreDAO.GetTitle(id);

                StoreItemInfo contentInfo = DataProviderWX.StoreItemDAO.GetStoreItemInfo(storeItemID);

                contentInfo.ImageUrl = StoreManager.GetImageUrl(publishmentSystemInfo, contentInfo.ImageUrl);

                StoreItemParameter parameter = new StoreItemParameter { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, Title = title, ContentInfo = contentInfo };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                StoreItemParameter parameter = new StoreItemParameter { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = true, PoweredBy = string.Empty };
                return Ok(parameter);
            }
        }
    }
}