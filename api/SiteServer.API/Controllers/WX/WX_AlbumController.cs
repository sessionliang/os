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
    public class WX_AlbumController : ApiController
    {
        [HttpGet]
        [ActionName("GetAlbumParameter")]
        public IHttpActionResult GetAlbumParameter(int id)
        {
            string wxOpenID = RequestUtils.GetQueryString("wxOpenID");

            try
            {
                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                string poweredBy = string.Empty;
                bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

                DataProviderWX.AlbumDAO.AddPVCount(id);

                string title = DataProviderWX.AlbumDAO.GetTitle(id);

                List<AlbumContentInfo> contentInfoList = DataProviderWX.AlbumContentDAO.GetAlbumContentInfoList(publishmentSystemInfo.PublishmentSystemID, id, 0);
                foreach (AlbumContentInfo contentInfo in contentInfoList)
                {
                    contentInfo.ImageUrl = AlbumManager.GetContentImageUrl(publishmentSystemInfo, contentInfo.ImageUrl);
                    contentInfo.LargeImageUrl = AlbumManager.GetContentImageUrl(publishmentSystemInfo, contentInfo.LargeImageUrl);
                }

                AlbumParameter parameter = new AlbumParameter { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, Title = title, ContentInfoList = contentInfoList };
                
                return Ok(parameter);
            }
            catch (Exception ex)
            {
                AlbumParameter parameter = new AlbumParameter { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = true, PoweredBy = string.Empty };
                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("GetPhotoParameter")]
        public IHttpActionResult GetPhotoParameter(int id)
        {
            string wxOpenID = RequestUtils.GetQueryString("wxOpenID");
            int parentID = RequestUtils.GetIntQueryString("parentID");

            try
            {
                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                string poweredBy = string.Empty;
                bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

                DataProviderWX.AlbumDAO.AddPVCount(id);

                string title = DataProviderWX.AlbumContentDAO.GetTitle(parentID);

                AlbumParameter parameter = new AlbumParameter { IsSuccess = true, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

                if (parentID > 0)
                {
                    List<AlbumContentInfo> contentInfoList = DataProviderWX.AlbumContentDAO.GetAlbumContentInfoList(publishmentSystemInfo.PublishmentSystemID, id, parentID);
                    foreach (AlbumContentInfo contentInfo in contentInfoList)
                    {
                        contentInfo.ImageUrl = AlbumManager.GetContentImageUrl(publishmentSystemInfo, contentInfo.ImageUrl);
                        contentInfo.LargeImageUrl = AlbumManager.GetContentImageUrl(publishmentSystemInfo, contentInfo.LargeImageUrl);
                    }

                    parameter = new AlbumParameter { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, Title = title, ContentInfoList = contentInfoList };
                }

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                AlbumParameter parameter = new AlbumParameter { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = true, PoweredBy = string.Empty };

                return Ok(parameter);
            }
        }
    }
}
