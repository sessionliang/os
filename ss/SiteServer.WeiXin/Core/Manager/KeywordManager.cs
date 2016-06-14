using System.Web.UI;
using BaiRong.Core;
using System.Collections.Specialized;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Text;
using SiteServer.WeiXin.Model;
using SiteServer.CMS.Core;

namespace SiteServer.WeiXin.Core
{
	public class KeywordManager
    {
        public static bool IsExists(int publishmentSystemID, string keyword)
        {
            return DataProviderWX.KeywordMatchDAO.IsExists(publishmentSystemID, keyword);
        }

        public static bool IsKeywordInsertConflict(int publishmentSystemID, string keywords, out string conflictKeywords)
        {
            bool isConflict = false;
            conflictKeywords = string.Empty;

            foreach (string str in TranslateUtils.StringCollectionToStringList(keywords, ' '))
            {
                string keyword = str.Trim();
                if (!string.IsNullOrEmpty(keyword))
                {
                    if (DataProviderWX.KeywordMatchDAO.IsExists(publishmentSystemID, keyword))
                    {
                        isConflict = true;
                        conflictKeywords += keyword + " ";
                    }
                }
            }
            conflictKeywords = conflictKeywords.Trim();

            return isConflict;
        }

        public static bool IsKeywordUpdateConflict(int publishmentSystemID, string keywordsNow, string keywordsUpdate, out string conflictKeywords)
        {
            bool isConflict = false;
            conflictKeywords = string.Empty;

            if (keywordsNow != keywordsUpdate)
            {
                List<string> keywordListNow = TranslateUtils.StringCollectionToStringList(keywordsNow, ' ');
                foreach (string str in TranslateUtils.StringCollectionToStringList(keywordsUpdate, ' '))
                {
                    string keyword = str.Trim();
                    if (!string.IsNullOrEmpty(keyword) && !keywordListNow.Contains(keyword))
                    {
                        if (DataProviderWX.KeywordMatchDAO.IsExists(publishmentSystemID, keyword))
                        {
                            isConflict = true;
                            conflictKeywords += keyword + " ";
                        }
                    }
                }

                conflictKeywords = conflictKeywords.Trim();
            }

            return isConflict;
        }

        public static bool IsKeywordUpdateConflict(int publishmentSystemID, int keywordID, string keywordsUpdate, out string conflictKeywords)
        {
            string keywordsNow = DataProviderWX.KeywordDAO.GetKeywords(keywordID);

            return IsKeywordUpdateConflict(publishmentSystemID, keywordsNow, keywordsUpdate, out conflictKeywords);
        }

        public static string GetFunctionName(EKeywordType keywordType, int functionID)
        {
            string functionName = string.Empty;

            if (functionID > 0)
            {
                if (keywordType == EKeywordType.Album)
                {
                    AlbumInfo albumInfo = DataProviderWX.AlbumDAO.GetAlbumInfo(functionID);
                    if (albumInfo != null)
                    {
                        functionName = albumInfo.Title;
                    }
                }
                else if (keywordType == EKeywordType.Appointment)
                {
                    AppointmentInfo appointmentInfo = DataProviderWX.AppointmentDAO.GetAppointmentInfo(functionID);
                    if (appointmentInfo != null)
                    {
                        functionName = appointmentInfo.Title;
                    }
                }
                else if (keywordType == EKeywordType.Conference)
                {
                    ConferenceInfo conferenceInfo = DataProviderWX.ConferenceDAO.GetConferenceInfo(functionID);
                    if (conferenceInfo != null)
                    {
                        functionName = conferenceInfo.Title;
                    }
                }
                else if (keywordType == EKeywordType.Coupon)
                {
                    CouponActInfo couponActInfo = DataProviderWX.CouponActDAO.GetActInfo(functionID);
                    if (couponActInfo != null)
                    {
                        functionName = couponActInfo.Title;
                    }
                }

                else if (keywordType == EKeywordType.Scratch || keywordType == EKeywordType.BigWheel || keywordType == EKeywordType.GoldEgg || keywordType == EKeywordType.Flap || keywordType == EKeywordType.YaoYao)
                {
                    LotteryInfo lotteryInfo = DataProviderWX.LotteryDAO.GetLotteryInfo(functionID);
                    if (lotteryInfo != null)
                    {
                        functionName = lotteryInfo.Title;
                    }
                }

                else if (keywordType == EKeywordType.Map)
                {
                    MapInfo mapInfo = DataProviderWX.MapDAO.GetMapInfo(functionID);
                    if (mapInfo != null)
                    {
                        functionName = mapInfo.Title;
                    }
                }
                else if (keywordType == EKeywordType.Message)
                {
                    MessageInfo messageInfo = DataProviderWX.MessageDAO.GetMessageInfo(functionID);
                    if (messageInfo != null)
                    {
                        functionName = messageInfo.Title;
                    }
                }
                else if (keywordType == EKeywordType.Search)
                {
                    SearchInfo searchInfo = DataProviderWX.SearchDAO.GetSearchInfo(functionID);
                    if (searchInfo != null)
                    {
                        functionName = searchInfo.Title;
                    }
                }
                else if (keywordType == EKeywordType.Store)
                {
                    StoreInfo storeInfo = DataProviderWX.StoreDAO.GetStoreInfo(functionID);
                    if (storeInfo != null)
                    {
                        functionName = storeInfo.Title;
                    }
                }
                else if (keywordType == EKeywordType.View360)
                {
                    View360Info view360Info = DataProviderWX.View360DAO.GetView360Info(functionID);
                    if (view360Info != null)
                    {
                        functionName = view360Info.Title;
                    }
                }
                else if (keywordType == EKeywordType.Vote)
                {
                    VoteInfo voteInfo = DataProviderWX.VoteDAO.GetVoteInfo(functionID);
                    if (voteInfo != null)
                    {
                        functionName = voteInfo.Title;
                    }
                }
                else if (keywordType == EKeywordType.Wifi)
                {
                    WifiInfo wifiInfo = DataProviderWX.WifiDAO.GetWifiInfo(functionID);
                    if (wifiInfo != null)
                    {
                        functionName = wifiInfo.Title;
                    }
                }
                else if (keywordType == EKeywordType.Card)
                {
                    CardInfo cardInfo = DataProviderWX.CardDAO.GetCardInfo(functionID);
                    if (cardInfo != null)
                    {
                        functionName = cardInfo.Title;
                    }
                }

                if (!string.IsNullOrEmpty(functionName))
                {
                    functionName = string.Format(@"{0}({1})", functionName, EKeywordTypeUtils.GetText(keywordType));
                }
            }

            return functionName;
        }

        public static int GetFunctionID(KeywordInfo keywordInfo)
        {
            int functionID = 0;
            if (keywordInfo != null)
            {
                if (keywordInfo.KeywordType == EKeywordType.Album)
                {
                    functionID = DataProviderWX.AlbumDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);
                }
                else if (keywordInfo.KeywordType == EKeywordType.Appointment)
                {
                    functionID = DataProviderWX.AppointmentDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);
                }
                else if (keywordInfo.KeywordType == EKeywordType.Conference)
                {
                    functionID = DataProviderWX.ConferenceDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);
                }
                else if (keywordInfo.KeywordType == EKeywordType.Coupon)
                {
                    functionID = DataProviderWX.CouponActDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);
                }

                else if (keywordInfo.KeywordType == EKeywordType.Scratch)
                {
                    functionID = DataProviderWX.LotteryDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, ELotteryType.Scratch, keywordInfo.KeywordID);
                }
                else if (keywordInfo.KeywordType == EKeywordType.BigWheel)
                {
                    functionID = DataProviderWX.LotteryDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, ELotteryType.BigWheel, keywordInfo.KeywordID);
                }
                else if (keywordInfo.KeywordType == EKeywordType.GoldEgg)
                {
                    functionID = DataProviderWX.LotteryDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, ELotteryType.GoldEgg, keywordInfo.KeywordID);
                }
                else if (keywordInfo.KeywordType == EKeywordType.Flap)
                {
                    functionID = DataProviderWX.LotteryDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, ELotteryType.Flap, keywordInfo.KeywordID);
                }
                else if (keywordInfo.KeywordType == EKeywordType.YaoYao)
                {
                    functionID = DataProviderWX.LotteryDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, ELotteryType.YaoYao, keywordInfo.KeywordID);
                }
                
                else if (keywordInfo.KeywordType == EKeywordType.Map)
                {
                    functionID = DataProviderWX.MapDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);
                }
                else if (keywordInfo.KeywordType == EKeywordType.Message)
                {
                    functionID = DataProviderWX.MessageDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);
                }
                else if (keywordInfo.KeywordType == EKeywordType.Search)
                {
                    functionID = DataProviderWX.SearchDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);
                }
                else if (keywordInfo.KeywordType == EKeywordType.Store)
                {
                    functionID = DataProviderWX.StoreDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);
                }
                else if (keywordInfo.KeywordType == EKeywordType.View360)
                {
                    functionID = DataProviderWX.View360DAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);
                }
                else if (keywordInfo.KeywordType == EKeywordType.Vote)
                {
                    functionID = DataProviderWX.VoteDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);
                }
                else if (keywordInfo.KeywordType == EKeywordType.Wifi)
                {
                    functionID = DataProviderWX.WifiDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);
                }
                else if (keywordInfo.KeywordType == EKeywordType.Card)
                {
                    functionID = DataProviderWX.CardDAO.GetFirstIDByKeywordID(keywordInfo.PublishmentSystemID, keywordInfo.KeywordID);
                }
            }
            return functionID;
        }

        public static string GetFunctionSqlString(int publishmentSystemID, EKeywordType keywordType)
        {
            string sqlString = string.Empty;

            if (keywordType == EKeywordType.Album)
            {
                sqlString = DataProviderWX.AlbumDAO.GetSelectString(publishmentSystemID);
            }
            else if (keywordType == EKeywordType.Appointment)
            {
                sqlString = DataProviderWX.AppointmentDAO.GetSelectString(publishmentSystemID);
            }
            else if (keywordType == EKeywordType.Conference)
            {
                sqlString = DataProviderWX.ConferenceDAO.GetSelectString(publishmentSystemID);
            }
            else if (keywordType == EKeywordType.Coupon)
            {
                sqlString = DataProviderWX.CouponActDAO.GetSelectString(publishmentSystemID);
            }

            else if (keywordType == EKeywordType.Scratch)
            {
                sqlString = DataProviderWX.LotteryDAO.GetSelectString(publishmentSystemID, ELotteryType.Scratch);
            }
            else if (keywordType == EKeywordType.BigWheel)
            {
                sqlString = DataProviderWX.LotteryDAO.GetSelectString(publishmentSystemID, ELotteryType.BigWheel);
            }
            else if (keywordType == EKeywordType.GoldEgg)
            {
                sqlString = DataProviderWX.LotteryDAO.GetSelectString(publishmentSystemID, ELotteryType.GoldEgg);
            }
            else if (keywordType == EKeywordType.Flap)
            {
                sqlString = DataProviderWX.LotteryDAO.GetSelectString(publishmentSystemID, ELotteryType.Flap);
            }
            else if (keywordType == EKeywordType.YaoYao)
            {
                sqlString = DataProviderWX.LotteryDAO.GetSelectString(publishmentSystemID, ELotteryType.YaoYao);
            }
            
            else if (keywordType == EKeywordType.Map)
            {
                sqlString = DataProviderWX.MapDAO.GetSelectString(publishmentSystemID);
            }
            else if (keywordType == EKeywordType.Message)
            {
                sqlString = DataProviderWX.MessageDAO.GetSelectString(publishmentSystemID);
            }
            else if (keywordType == EKeywordType.Search)
            {
                sqlString = DataProviderWX.SearchDAO.GetSelectString(publishmentSystemID);
            }
            else if (keywordType == EKeywordType.Store)
            {
                sqlString = DataProviderWX.StoreDAO.GetSelectString(publishmentSystemID);
            }
            else if (keywordType == EKeywordType.View360)
            {
                sqlString = DataProviderWX.View360DAO.GetSelectString(publishmentSystemID);
            }
            else if (keywordType == EKeywordType.Vote)
            {
                sqlString = DataProviderWX.VoteDAO.GetSelectString(publishmentSystemID);
            }
            else if (keywordType == EKeywordType.Wifi)
            {
                sqlString = DataProviderWX.WifiDAO.GetSelectString(publishmentSystemID);
            }
            else if (keywordType == EKeywordType.Card)
            {
                sqlString = DataProviderWX.CardDAO.GetSelectString(publishmentSystemID);
            }

            return sqlString;
        }

        public static string GetFunctionUrl(int publishmentSystemID, EKeywordType keywordType, int functionID)
        {
            string functionUrl = string.Empty;

            if (functionID > 0)
            {
                if (keywordType == EKeywordType.Album)
                {
                    AlbumInfo albumInfo = DataProviderWX.AlbumDAO.GetAlbumInfo(functionID);
                    if (albumInfo != null)
                    {
                        functionUrl = AlbumManager.GetAlbumUrl(albumInfo, string.Empty);
                    }
                }
                else if (keywordType == EKeywordType.Appointment)
                {
                    functionUrl = AppointmentManager.GetIndexUrl(publishmentSystemID, functionID, string.Empty);
                }
                else if (keywordType == EKeywordType.Conference)
                {
                    ConferenceInfo conferenceInfo = DataProviderWX.ConferenceDAO.GetConferenceInfo(functionID);
                    if (conferenceInfo != null)
                    {
                        functionUrl = ConferenceManager.GetConferenceUrl(conferenceInfo, string.Empty);
                    }
                }
                else if (keywordType == EKeywordType.Scratch || keywordType == EKeywordType.BigWheel || keywordType == EKeywordType.GoldEgg || keywordType == EKeywordType.Flap || keywordType == EKeywordType.YaoYao)
                {
                    LotteryInfo lotteryInfo = DataProviderWX.LotteryDAO.GetLotteryInfo(functionID);
                    if (lotteryInfo != null)
                    {
                        functionUrl = LotteryManager.GetLotteryUrl(lotteryInfo, string.Empty);
                    }
                }
                else if (keywordType == EKeywordType.Map)
                {
                    MapInfo mapInfo = DataProviderWX.MapDAO.GetMapInfo(functionID);
                    if (mapInfo != null)
                    {
                        functionUrl = MapManager.GetMapUrl(mapInfo.MapWD);
                    }
                }
                else if (keywordType == EKeywordType.Message)
                {
                    MessageInfo messageInfo = DataProviderWX.MessageDAO.GetMessageInfo(functionID);
                    if (messageInfo != null)
                    {
                        functionUrl = MessageManager.GetMessageUrl(messageInfo, string.Empty);
                    }
                }
                else if (keywordType == EKeywordType.Search)
                {
                    SearchInfo searchInfo = DataProviderWX.SearchDAO.GetSearchInfo(functionID);
                    if (searchInfo != null)
                    {
                        functionUrl = SearchManager.GetSearchUrl(searchInfo);
                    }
                }
                else if (keywordType == EKeywordType.Store)
                {
                    StoreInfo storeInfo = DataProviderWX.StoreDAO.GetStoreInfo(functionID);
                    if (storeInfo != null)
                    {
                        functionUrl = StoreManager.GetStoreUrl(storeInfo, string.Empty);
                    }
                }
                else if (keywordType == EKeywordType.View360)
                {
                    View360Info view360Info = DataProviderWX.View360DAO.GetView360Info(functionID);
                    if (view360Info != null)
                    {
                        functionUrl = View360Manager.GetView360Url(view360Info, string.Empty);
                    }
                }
                else if (keywordType == EKeywordType.Vote)
                {
                    VoteInfo voteInfo = DataProviderWX.VoteDAO.GetVoteInfo(functionID);
                    if (voteInfo != null)
                    {
                        functionUrl = VoteManager.GetVoteUrl(voteInfo, string.Empty);
                    }
                }
                else if (keywordType == EKeywordType.Wifi)
                {
                    WifiInfo wifiInfo = DataProviderWX.WifiDAO.GetWifiInfo(functionID);
                    if (wifiInfo != null)
                    {
                        functionUrl = WifiManager.GetWifiUrl(wifiInfo, string.Empty);
                    }
                }
                else if (keywordType == EKeywordType.Card)
                {
                    CardInfo cardInfo = DataProviderWX.CardDAO.GetCardInfo(functionID);
                    if (cardInfo != null)
                    {
                        functionUrl = CardManager.GetCardUrl(cardInfo, string.Empty);
                    }
                }
            }
            
            return functionUrl;
        }
	}
}
