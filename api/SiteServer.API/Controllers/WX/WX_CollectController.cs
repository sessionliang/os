using BaiRong.Core;
using BaiRong.Core.Drawing;
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
    public class WX_CollectController : ApiController
    {
        [HttpGet]
        [ActionName("GetCollectParameter")]
        public IHttpActionResult GetCollectParameter(int id)
        {
            try
            {
                string cookieSN = WeiXinManager.GetCookieSN();
                string wxOpenID = RequestUtils.GetQueryString("wxOpenID");

                DataProviderWX.CollectDAO.AddPVCount(id);

                CollectInfo collectInfo = DataProviderWX.CollectDAO.GetCollectInfo(id);

                bool isCollectd = DataProviderWX.CollectLogDAO.IsCollectd(id, cookieSN, wxOpenID);
                
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(collectInfo.PublishmentSystemID);
                string poweredBy = string.Empty;
                bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

                collectInfo.ContentImageUrl = CollectManager.GetContentImageUrl(publishmentSystemInfo, collectInfo.ContentImageUrl);
                collectInfo.EndImageUrl = CollectManager.GetEndImageUrl(publishmentSystemInfo, collectInfo.EndImageUrl);

                List<CollectItemInfo> itemInfoList = DataProviderWX.CollectItemDAO.GetCollectItemInfoList(id);
                foreach (CollectItemInfo itemInfo in itemInfoList)
                {
                    itemInfo.SmallUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, itemInfo.SmallUrl));
                    itemInfo.LargeUrl = PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(publishmentSystemInfo, itemInfo.LargeUrl));
                }

                bool isEnd = false;
                if (collectInfo.EndDate < DateTime.Now)
                {
                    isEnd = true;
                }

                string votedItemIDCollection = TranslateUtils.ObjectCollectionToString(DataProviderWX.CollectLogDAO.GetVotedItemIDList(id, cookieSN));
                Dictionary<string, int> itemIDCollectionWithRank = DataProviderWX.CollectItemDAO.GetItemIDCollectionWithRank(id);
                List<object> list = new List<object>();
                foreach (var item in itemIDCollectionWithRank)
                {
                    list.Add(new { ItemIDCollection = item.Key, Rank = item.Value });
                }

                var parameter = new { IsSuccess = true, VotedItemIDCollection = votedItemIDCollection, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, IsCollectd = isCollectd, CollectInfo = collectInfo, ItemList = itemInfoList, ItemIDCollectionWithRank = list, IsEnd = isEnd };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var parameter = new { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = true, PoweredBy = string.Empty };

                return Ok(parameter);
            }
        }

        [HttpPost]
        [ActionName("SubmitUpload")]
        public IHttpActionResult SubmitUpload(int id)
        {
            try
            {
                string cookieSN = WeiXinManager.GetCookieSN();
                string wxOpenID = RequestUtils.GetPostString("wxOpenID");

                string title = RequestUtils.GetPostStringNoSqlAndXss("title");
                string description = RequestUtils.GetPostStringNoSqlAndXss("description");
                string mobile = RequestUtils.GetPostStringNoSqlAndXss("mobile");
                string smallUrl = RequestUtils.GetPostString("smallUrl");
                string largeUrl = RequestUtils.GetPostString("largeUrl");

                CollectInfo collectInfo = DataProviderWX.CollectDAO.GetCollectInfo(id);

                CollectItemInfo itemInfo = new CollectItemInfo { ID = 0, CollectID = collectInfo.ID, PublishmentSystemID = RequestUtils.PublishmentSystemID, Title = title, SmallUrl = smallUrl, LargeUrl = largeUrl, Description = description, Mobile = mobile, IsChecked = !collectInfo.ContentIsCheck, VoteNum = 0 };

                int itemID = DataProviderWX.CollectItemDAO.Insert(itemInfo);

                var parameter = new { IsSuccess = true, IsCheck = collectInfo.ContentIsCheck, ItemID = itemID };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var parameter = new { IsSuccess = false, ErrorMessage = ex.Message };

                return Ok(parameter);
            }
        }

        [HttpPost]
        [ActionName("Upload")]
        public IHttpActionResult Upload()
        {
            try
            {
                if (HttpContext.Current.Request.Files != null && HttpContext.Current.Request.Files["Upload"] != null)
                {
                    HttpPostedFile postedFile = HttpContext.Current.Request.Files["Upload"];

                    PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;

                    string fileName = APIPathUtils.GetUploadFileName(postedFile.FileName);
                    string fileExtName = PathUtils.GetExtension(fileName).ToLower();
                    string directoryPath = APIPathUtils.GetUploadDirectoryPath();
                    string largeFilePath = PathUtils.Combine(directoryPath, fileName);
                    string smallFilePath = PathUtils.Combine(directoryPath, "s_" + fileName);

                    if (EFileSystemTypeUtils.IsImageOrFlashOrPlayer(fileExtName))
                    {
                        postedFile.SaveAs(largeFilePath);

                        ImageUtils.MakeThumbnail(largeFilePath, smallFilePath, 85, 85, true);

                        string smallUrl = APIPageUtils.GetAPIUrlByPhysicalPath(smallFilePath);
                        string largeUrl = APIPageUtils.GetAPIUrlByPhysicalPath(largeFilePath);

                        var parameter = new { IsSuccess = true, SmallUrl = smallUrl, LargeUrl = largeUrl };

                        return Ok(parameter);
                    }
                }

                return Ok(new { IsSuccess = false, ErrorMessage = "上传失败，文件格式不正确" });
            }
            catch (Exception ex)
            {
                var parameter = new { IsSuccess = false, ErrorMessage = ex.Message };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("SubmitVote")]
        public IHttpActionResult SubmitVote(int id)
        {
            try
            {
                string cookieSN = WeiXinManager.GetCookieSN();
                string wxOpenID = RequestUtils.GetQueryString("wxOpenID");
                int itemID = RequestUtils.GetIntQueryString("itemID");

                CollectInfo collectInfo = DataProviderWX.CollectDAO.GetCollectInfo(id);

                CollectManager.Vote(collectInfo.PublishmentSystemID, id, itemID, PageUtils.GetIPAddress(), cookieSN, wxOpenID, RequestUtils.CurrentUserName);

                var parameter = new { IsSuccess = true };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var parameter = new { IsSuccess = false, ErrorMessage = ex.Message };

                return Ok(parameter);
            }
        }
    }
}
