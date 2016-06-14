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
    public class WX_AppointmentController : ApiController
    {
        [HttpGet]
        [ActionName("GetAppointmentParameter")]
        public IHttpActionResult GetAppointmentParameter(int id)
        {
            try
            {
                string cookieSN = WeiXinManager.GetCookieSN();
                string wxOpenID = RequestUtils.GetQueryString("wxOpenID");

                DataProviderWX.AppointmentDAO.AddPVCount(id);

                AppointmentInfo appointmentInfo = DataProviderWX.AppointmentDAO.GetAppointmentInfo(id);

                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(appointmentInfo.PublishmentSystemID);
                string poweredBy = string.Empty;
                bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

                appointmentInfo.ImageUrl =  AppointmentManager.GetImageUrl(publishmentSystemInfo, appointmentInfo.ImageUrl);
                appointmentInfo.ContentImageUrl = AppointmentManager.GetContentImageUrl(publishmentSystemInfo, appointmentInfo.ContentImageUrl);
                appointmentInfo.EndImageUrl = AppointmentManager.GetEndImageUrl(publishmentSystemInfo, appointmentInfo.EndImageUrl);

                List<AppointmentItemInfo> appointmentItemInfoList = DataProviderWX.AppointmentItemDAO.GetItemInfoList(appointmentInfo.PublishmentSystemID, appointmentInfo.ID);
                foreach (AppointmentItemInfo appointmentItemInfo in appointmentItemInfoList)
                {
                    appointmentItemInfo.TopImageUrl = AppointmentManager.GetContentImageUrl(publishmentSystemInfo, appointmentItemInfo.TopImageUrl);
                    appointmentItemInfo.ImageUrl = AppointmentManager.GetContentImageUrl(publishmentSystemInfo, appointmentItemInfo.ImageUrl);
                    ArrayList imageUrlCollectionArrayList = TranslateUtils.StringCollectionToArrayList(appointmentItemInfo.ImageUrlCollection);
                    ArrayList largeImageUrlCollectionArrayList = TranslateUtils.StringCollectionToArrayList(appointmentItemInfo.LargeImageUrlCollection);
                    if (imageUrlCollectionArrayList.Count > 0)
                    {
                        for (int i = 0; i < imageUrlCollectionArrayList.Count; i++)
                        {
                            imageUrlCollectionArrayList[i] = PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrlCollectionArrayList[i].ToString());
                            largeImageUrlCollectionArrayList[i] = PageUtility.ParseNavigationUrl(publishmentSystemInfo, largeImageUrlCollectionArrayList[i].ToString());
                        }
                    }
                    appointmentItemInfo.ImageUrlCollection = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(imageUrlCollectionArrayList);
                    appointmentItemInfo.LargeImageUrlCollection = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(largeImageUrlCollectionArrayList);
                }

                bool isEnd = false;
                if (appointmentInfo.EndDate < DateTime.Now)
                {
                    isEnd = true;
                }

                List<ConfigExtendInfo> configExtendInfoList =DataProviderWX.ConfigExtendDAO.GetConfigExtendInfoList(publishmentSystemInfo.PublishmentSystemID,appointmentInfo.ID, EKeywordTypeUtils.GetValue(EKeywordType.Appointment));

                AppointmentParameter parameter = new AppointmentParameter { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, AppointmentInfo = appointmentInfo, AppointmentItemInfoList = appointmentItemInfoList, ConfigExtendInfoList = configExtendInfoList, IsEnd = isEnd };

                return Ok(parameter);

            }
            catch (Exception ex)
            {
                AppointmentParameter parameter = new AppointmentParameter { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = true, PoweredBy = string.Empty };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("GetAppointmentItemParameter")]
        public IHttpActionResult GetAppointmentItemParameter(int id)
        {
            try
            {
                string cookieSN = WeiXinManager.GetCookieSN();
                string wxOpenID = RequestUtils.GetQueryString("wxOpenID");
                int itemID = RequestUtils.GetIntQueryString("itemID");
                int contentID = RequestUtils.GetIntQueryString("contentID");

                DataProviderWX.AppointmentDAO.AddPVCount(id);

                AppointmentInfo appointmentInfo = DataProviderWX.AppointmentDAO.GetAppointmentInfo(id);
                 
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(appointmentInfo.PublishmentSystemID);
                string poweredBy = string.Empty;
                bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

                appointmentInfo.ImageUrl = AppointmentManager.GetImageUrl(publishmentSystemInfo, appointmentInfo.ImageUrl);
                appointmentInfo.ContentImageUrl = AppointmentManager.GetContentImageUrl(publishmentSystemInfo, appointmentInfo.ContentImageUrl);
                appointmentInfo.EndImageUrl = AppointmentManager.GetEndImageUrl(publishmentSystemInfo, appointmentInfo.EndImageUrl);

                AppointmentItemInfo itemInfo = DataProviderWX.AppointmentItemDAO.GetItemInfo(itemID);

                if (itemInfo != null)
                {
                    if (string.IsNullOrEmpty(itemInfo.Description))
                    {
                        itemInfo.IsDescription = false;
                    }
                    if (string.IsNullOrEmpty(itemInfo.ImageUrl))
                    {
                        itemInfo.IsImageUrl = false;
                    }
                    if (string.IsNullOrEmpty(itemInfo.VideoUrl))
                    {
                        itemInfo.IsVideoUrl = false;
                    }
                    if (string.IsNullOrEmpty(itemInfo.ImageUrlCollection))
                    {
                        itemInfo.IsImageUrlCollection = false;
                    }
                    if (string.IsNullOrEmpty(itemInfo.MapAddress))
                    {
                        itemInfo.IsMap = false;
                    }
                    if (string.IsNullOrEmpty(itemInfo.Tel))
                    {
                        itemInfo.IsTel = false;
                    }

                    itemInfo.TopImageUrl = AppointmentManager.GetItemTopImageUrl(publishmentSystemInfo, itemInfo.TopImageUrl);
                    itemInfo.ImageUrl = AppointmentManager.GetContentImageUrl(publishmentSystemInfo, itemInfo.ImageUrl);
                    itemInfo.VideoUrl = PageUtility.ParseNavigationUrl(publishmentSystemInfo, itemInfo.VideoUrl);

                    ArrayList imageUrlCollectionArrayList = TranslateUtils.StringCollectionToArrayList(itemInfo.ImageUrlCollection);
                    ArrayList largeImageUrlCollectionArrayList = TranslateUtils.StringCollectionToArrayList(itemInfo.LargeImageUrlCollection);
                    if (imageUrlCollectionArrayList.Count > 0)
                    {
                        for (int i = 0; i < imageUrlCollectionArrayList.Count; i++)
                        {
                            imageUrlCollectionArrayList[i] = PageUtility.ParseNavigationUrl(publishmentSystemInfo, imageUrlCollectionArrayList[i].ToString());
                            largeImageUrlCollectionArrayList[i] = PageUtility.ParseNavigationUrl(publishmentSystemInfo, largeImageUrlCollectionArrayList[i].ToString());
                        }
                    }
                    itemInfo.ImageUrlCollection = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(imageUrlCollectionArrayList);
                    itemInfo.LargeImageUrlCollection = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(largeImageUrlCollectionArrayList);
                }

                AppointmentContentInfo contentInfo = null;
                if (contentID > 0)
                {
                    contentInfo = DataProviderWX.AppointmentContentDAO.GetContentInfo(contentID);
                }
                else
                {
                    contentInfo = DataProviderWX.AppointmentContentDAO.GetLatestContentInfo(itemInfo.ID, cookieSN, wxOpenID, UserManager.Current.UserName);
                }

                bool isEnd = false;
                if (appointmentInfo.EndDate < DateTime.Now)
                {
                    isEnd = true;
                }

                AppointmentParameter parameter = new AppointmentParameter { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, AppointmentInfo = appointmentInfo, AppointmentItemInfo = itemInfo, AppointmentContentInfo = contentInfo, IsEnd = isEnd };

                return Ok(parameter);

            }
            catch (Exception ex)
            {
                AppointmentParameter parameter = new AppointmentParameter { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = true, PoweredBy = string.Empty };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("GetAppointmentContentParameter")]
        public IHttpActionResult GetAppointmentContentParameter(int id)
        {
            try
            {
                string cookieSN = WeiXinManager.GetCookieSN();
                string wxOpenID = RequestUtils.GetQueryString("wxOpenID");

                AppointmentInfo appointmentInfo = DataProviderWX.AppointmentDAO.GetAppointmentInfo(id);
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(appointmentInfo.PublishmentSystemID);
                string poweredBy = string.Empty;
                bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

                appointmentInfo.ContentResultTopImageUrl = AppointmentManager.GetContentResultTopImageUrl(publishmentSystemInfo, appointmentInfo.ContentResultTopImageUrl);

                List<AppointmentContentInfo> contentInfoList = DataProviderWX.AppointmentContentDAO.GetLatestContentInfoList(id, cookieSN, wxOpenID, UserManager.Current.UserName);

                List<AppointmentItemInfo> itemInfoList = new List<AppointmentItemInfo>();

                foreach (AppointmentContentInfo contentInfo in contentInfoList)
                {
                    bool isExists = false;
                    foreach (AppointmentItemInfo theItemInfo in itemInfoList)
                    {
                        if (theItemInfo.ID == contentInfo.AppointmentItemID)
                        {
                            isExists = true;
                            break;
                        }
                    }

                    if (!isExists)
                    {
                        AppointmentItemInfo itemInfo = DataProviderWX.AppointmentItemDAO.GetItemInfo(contentInfo.AppointmentItemID);

                        itemInfo.TopImageUrl = AppointmentManager.GetContentImageUrl(publishmentSystemInfo, itemInfo.TopImageUrl);
                        itemInfo.ImageUrl = AppointmentManager.GetContentImageUrl(publishmentSystemInfo, itemInfo.ImageUrl);

                        itemInfoList.Add(itemInfo);
                    }
                 }

                AppointmentParameter parameter = new AppointmentParameter { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, AppointmentInfo = appointmentInfo, AppointmentItemInfoList = itemInfoList, AppointmentContentInfoList = contentInfoList };

                return Ok(parameter);

            }
            catch (Exception ex)
            {
                AppointmentParameter parameter = new AppointmentParameter { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = true, PoweredBy = string.Empty };

                return Ok(parameter);
            }
        }
          
        [HttpGet]
        [ActionName("SubmitApplication")]
        public IHttpActionResult SubmitApplication(int id)
        {
            try
            {
                int publishmentSystemID = RequestUtils.PublishmentSystemID;
                int itemID = RequestUtils.GetIntQueryString("itemID");
                string cookieSN = WeiXinManager.GetCookieSN();
                string wxOpenID = RequestUtils.GetQueryString("wxOpenID");
                string realName = RequestUtils.GetQueryString("realName");
                string email = RequestUtils.GetQueryString("email");
                string mobile = RequestUtils.GetQueryString("mobile");
                string settingsXml = RequestUtils.GetQueryString("settingsXml");


                AppointmentContentInfo contentInfo = new AppointmentContentInfo();
                contentInfo.PublishmentSystemID = publishmentSystemID;
                contentInfo.AppointmentID = id;
                contentInfo.AppointmentItemID = itemID;
                contentInfo.CookieSN = cookieSN;
                contentInfo.WXOpenID = wxOpenID;
                contentInfo.UserName = UserManager.Current.UserName;
                contentInfo.RealName = realName;
                contentInfo.Mobile = mobile;
                contentInfo.Email = email;
                contentInfo.SettingsXML = settingsXml;
                contentInfo.Status = EAppointmentStatusUtils.GetValue(EAppointmentStatus.Handling);
                contentInfo.Message = "";
                contentInfo.AddDate = DateTime.Now;

                DataProviderWX.AppointmentContentDAO.Insert(contentInfo);

                AppointmentParameter parameter = new AppointmentParameter { IsSuccess = true, ErrorMessage = string.Empty };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                AppointmentParameter parameter = new AppointmentParameter { IsSuccess = false, ErrorMessage = ex.Message };

                return Ok(parameter);
            }
        }
    }
 }
