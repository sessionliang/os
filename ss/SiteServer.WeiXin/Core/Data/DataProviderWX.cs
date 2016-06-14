using System.Reflection;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using BaiRong.Core;

namespace SiteServer.WeiXin.Core
{
	public class DataProviderWX
	{
        private static string assemblyString;
        private static string namespaceString;

        static DataProviderWX()
		{
            assemblyString = "SiteServer.WeiXin";
            if (BaiRongDataProvider.DatabaseType == EDatabaseType.SqlServer)
            {
                namespaceString = "SiteServer.WeiXin.Provider.Data.SqlServer";
            }
            else if (BaiRongDataProvider.DatabaseType == EDatabaseType.Oracle)
            {
                namespaceString = "SiteServer.WeiXin.Provider.Data.Oracle";
            }
		}

        private static IMenuDAO menuDAO;
        public static IMenuDAO MenuDAO
        {
            get
            {
                if (menuDAO == null)
                {
                    string className = namespaceString + ".MenuDAO";
                    menuDAO = (IMenuDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return menuDAO;
            }
        }

        private static IWebMenuDAO webMenuDAO;
        public static IWebMenuDAO WebMenuDAO
        {
            get
            {
                if (webMenuDAO == null)
                {
                    string className = namespaceString + ".WebMenuDAO";
                    webMenuDAO = (IWebMenuDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return webMenuDAO;
            }
        }

        private static IKeywordDAO keywordDAO;
        public static IKeywordDAO KeywordDAO
        {
            get
            {
                if (keywordDAO == null)
                {
                    string className = namespaceString + ".KeywordDAO";
                    keywordDAO = (IKeywordDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return keywordDAO;
            }
        }

        private static IKeywordGroupDAO keywordGroupDAO;
        public static IKeywordGroupDAO KeywordGroupDAO
        {
            get
            {
                if (keywordGroupDAO == null)
                {
                    string className = namespaceString + ".KeywordGroupDAO";
                    keywordGroupDAO = (IKeywordGroupDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return keywordGroupDAO;
            }
        }

        private static IKeywordResourceDAO keywordResourceDAO;
        public static IKeywordResourceDAO KeywordResourceDAO
        {
            get
            {
                if (keywordResourceDAO == null)
                {
                    string className = namespaceString + ".KeywordResourceDAO";
                    keywordResourceDAO = (IKeywordResourceDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return keywordResourceDAO;
            }
        }

        private static IKeywordMatchDAO keywordMatchDAO;
        public static IKeywordMatchDAO KeywordMatchDAO
        {
            get
            {
                if (keywordMatchDAO == null)
                {
                    string className = namespaceString + ".KeywordMatchDAO";
                    keywordMatchDAO = (IKeywordMatchDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return keywordMatchDAO;
            }
        }

        private static ICountDAO countDAO;
        public static ICountDAO CountDAO
        {
            get
            {
                if (countDAO == null)
                {
                    string className = namespaceString + ".CountDAO";
                    countDAO = (ICountDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return countDAO;
            }
        }

        private static ICouponDAO couponDAO;
        public static ICouponDAO CouponDAO
        {
            get
            {
                if (couponDAO == null)
                {
                    string className = namespaceString + ".CouponDAO";
                    couponDAO = (ICouponDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return couponDAO;
            }
        }

        private static ICouponSNDAO couponSNDAO;
        public static ICouponSNDAO CouponSNDAO
        {
            get
            {
                if (couponSNDAO == null)
                {
                    string className = namespaceString + ".CouponSNDAO";
                    couponSNDAO = (ICouponSNDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return couponSNDAO;
            }
        }

        private static ICouponActDAO couponActDAO;
        public static ICouponActDAO CouponActDAO
        {
            get
            {
                if (couponActDAO == null)
                {
                    string className = namespaceString + ".CouponActDAO";
                    couponActDAO = (ICouponActDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return couponActDAO;
            }
        }

        private static IAccountDAO accountDAO;
        public static IAccountDAO AccountDAO
        {
            get
            {
                if (accountDAO == null)
                {
                    string className = namespaceString + ".AccountDAO";
                    accountDAO = (IAccountDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return accountDAO;
            }
        }

        private static IVoteDAO voteDAO;
        public static IVoteDAO VoteDAO
        {
            get
            {
                if (voteDAO == null)
                {
                    string className = namespaceString + ".VoteDAO";
                    voteDAO = (IVoteDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return voteDAO;
            }
        }

        private static IVoteItemDAO voteItemDAO;
        public static IVoteItemDAO VoteItemDAO
        {
            get
            {
                if (voteItemDAO == null)
                {
                    string className = namespaceString + ".VoteItemDAO";
                    voteItemDAO = (IVoteItemDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return voteItemDAO;
            }
        }

        private static IVoteLogDAO voteLogDAO;
        public static IVoteLogDAO VoteLogDAO
        {
            get
            {
                if (voteLogDAO == null)
                {
                    string className = namespaceString + ".VoteLogDAO";
                    voteLogDAO = (IVoteLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return voteLogDAO;
            }
        }

        private static IMessageDAO messageDAO;
        public static IMessageDAO MessageDAO
        {
            get
            {
                if (messageDAO == null)
                {
                    string className = namespaceString + ".MessageDAO";
                    messageDAO = (IMessageDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return messageDAO;
            }
        }

        private static IMessageContentDAO messageContentDAO;
        public static IMessageContentDAO MessageContentDAO
        {
            get
            {
                if (messageContentDAO == null)
                {
                    string className = namespaceString + ".MessageContentDAO";
                    messageContentDAO = (IMessageContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return messageContentDAO;
            }
        }

        private static IView360DAO view360DAO;
        public static IView360DAO View360DAO
        {
            get
            {
                if (view360DAO == null)
                {
                    string className = namespaceString + ".View360DAO";
                    view360DAO = (IView360DAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return view360DAO;
            }
        }

        private static IConferenceDAO conferenceDAO;
        public static IConferenceDAO ConferenceDAO
        {
            get
            {
                if (conferenceDAO == null)
                {
                    string className = namespaceString + ".ConferenceDAO";
                    conferenceDAO = (IConferenceDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return conferenceDAO;
            }
        }

        private static IConferenceContentDAO conferenceContentDAO;
        public static IConferenceContentDAO ConferenceContentDAO
        {
            get
            {
                if (conferenceContentDAO == null)
                {
                    string className = namespaceString + ".ConferenceContentDAO";
                    conferenceContentDAO = (IConferenceContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return conferenceContentDAO;
            }
        }

        private static IMapDAO mapDAO;
        public static IMapDAO MapDAO
        {
            get
            {
                if (mapDAO == null)
                {
                    string className = namespaceString + ".MapDAO";
                    mapDAO = (IMapDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return mapDAO;
            }
        }

        private static ILotteryDAO lotteryDAO;
        public static ILotteryDAO LotteryDAO
        {
            get
            {
                if (lotteryDAO == null)
                {
                    string className = namespaceString + ".LotteryDAO";
                    lotteryDAO = (ILotteryDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return lotteryDAO;
            }
        }

        private static ILotteryAwardDAO lotteryAwardDAO;
        public static ILotteryAwardDAO LotteryAwardDAO
        {
            get
            {
                if (lotteryAwardDAO == null)
                {
                    string className = namespaceString + ".LotteryAwardDAO";
                    lotteryAwardDAO = (ILotteryAwardDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return lotteryAwardDAO;
            }
        }

        private static ILotteryWinnerDAO lotteryWinnerDAO;
        public static ILotteryWinnerDAO LotteryWinnerDAO
        {
            get
            {
                if (lotteryWinnerDAO == null)
                {
                    string className = namespaceString + ".LotteryWinnerDAO";
                    lotteryWinnerDAO = (ILotteryWinnerDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return lotteryWinnerDAO;
            }
        }

        private static ILotteryLogDAO lotteryLogDAO;
        public static ILotteryLogDAO LotteryLogDAO
        {
            get
            {
                if (lotteryLogDAO == null)
                {
                    string className = namespaceString + ".LotteryLogDAO";
                    lotteryLogDAO = (ILotteryLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return lotteryLogDAO;
            }
        }

        private static IAlbumDAO albumDAO;
        public static IAlbumDAO AlbumDAO
        {
            get
            {
                if (albumDAO == null)
                {
                    string className = namespaceString + ".AlbumDAO";
                    albumDAO = (IAlbumDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return albumDAO;
            }
        }

        private static IAlbumContentDAO albumContentDAO;
        public static IAlbumContentDAO AlbumContentDAO
        {
            get
            {
                if (albumContentDAO == null)
                {
                    string className = namespaceString + ".AlbumContentDAO";
                    albumContentDAO = (IAlbumContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return albumContentDAO;
            }
        }

        private static IAppointmentDAO appointmentDAO;
        public static IAppointmentDAO AppointmentDAO
        {
            get
            {
                if (appointmentDAO == null)
                {
                    string className = namespaceString + ".AppointmentDAO";
                    appointmentDAO = (IAppointmentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return appointmentDAO;
            }
        }

        private static IAppointmentItemDAO appointmentItemDAO;
        public static IAppointmentItemDAO AppointmentItemDAO
        {
            get
            {
                if (appointmentItemDAO == null)
                {
                    string className = namespaceString + ".AppointmentItemDAO";
                    appointmentItemDAO = (IAppointmentItemDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return appointmentItemDAO;
            }
        }

        private static IAppointmentContentDAO appointmentContentDAO;
        public static IAppointmentContentDAO AppointmentContentDAO
        {
            get
            {
                if (appointmentContentDAO == null)
                {
                    string className = namespaceString + ".AppointmentContentDAO";
                    appointmentContentDAO = (IAppointmentContentDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return appointmentContentDAO;
            }
        }

        private static ISearchDAO searchDAO;
        public static ISearchDAO SearchDAO
        {
            get
            {
                if (searchDAO == null)
                {
                    string className = namespaceString + ".SearchDAO";
                    searchDAO = (ISearchDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return searchDAO;
            }
        }

        private static ISearchNavigationDAO searchNavigationDAO;
        public static ISearchNavigationDAO SearchNavigationDAO
        {
            get
            {
                if (searchNavigationDAO == null)
                {
                    string className = namespaceString + ".SearchNavigationDAO";
                    searchNavigationDAO = (ISearchNavigationDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return searchNavigationDAO;
            }
        }

        private static IStoreDAO storeDAO;
        public static IStoreDAO StoreDAO
        {
            get
            {
                if (storeDAO == null)
                {
                    string className = namespaceString + ".StoreDAO";
                    storeDAO = (IStoreDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return storeDAO;
            }
        }

        private static IStoreCategoryDAO storeCategoryDAO;
        public static IStoreCategoryDAO StoreCategoryDAO
        {
            get
            {
                if (storeCategoryDAO == null)
                {
                    string className = namespaceString + ".StoreCategoryDAO";
                    storeCategoryDAO = (IStoreCategoryDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return storeCategoryDAO;
            }
        }

        private static IStoreItemDAO storeItemDAO;
        public static IStoreItemDAO StoreItemDAO
        {
            get
            {
                if (storeItemDAO == null)
                {
                    string className = namespaceString + ".StoreItemDAO";
                    storeItemDAO = (IStoreItemDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return storeItemDAO;
            }
        }

        private static IWifiDAO wifiDAO;
        public static IWifiDAO WifiDAO
        {
            get
            {
                if (wifiDAO == null)
                {
                    string className = namespaceString + ".WifiDAO";
                    wifiDAO = (IWifiDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return wifiDAO;
            }
        }

        private static IWifiNodeDAO wifiNodeDAO;
        public static IWifiNodeDAO WifiNodeDAO
        {
            get
            {
                if (wifiNodeDAO == null)
                {
                    string className = namespaceString + ".WifiNodeDAO";
                    wifiNodeDAO = (IWifiNodeDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return wifiNodeDAO;
            }
        }

        private static IScenceDAO scenceDAO;
        public static IScenceDAO ScenceDAO
        {
            get
            {
                if (scenceDAO == null)
                {
                    string className = namespaceString + ".ScenceDAO";
                    scenceDAO = (IScenceDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return scenceDAO;
            }
        }

        private static IConfigExtendDAO configExtendDAO;
        public static IConfigExtendDAO ConfigExtendDAO
        {
            get
            {
                if (configExtendDAO == null)
                {
                    string className = namespaceString + ".ConfigExtendDAO";
                    configExtendDAO = (IConfigExtendDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return configExtendDAO;
            }
        }

        private static ICardDAO cardDAO;
        public static ICardDAO CardDAO
        {
            get
            {
                if (cardDAO == null)
                {
                    string className = namespaceString + ".CardDAO";
                    cardDAO = (ICardDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return cardDAO;
            }
        }

        private static ICardSNDAO cardSNDAO;
        public static ICardSNDAO CardSNDAO
        {
            get
            {
                if (cardSNDAO == null)
                {
                    string className = namespaceString + ".CardSNDAO";
                    cardSNDAO = (ICardSNDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return cardSNDAO;
            }
        }

        private static ICardEntitySNDAO cardEntitySNDAO;
        public static ICardEntitySNDAO CardEntitySNDAO
        {
            get
            {
                if (cardEntitySNDAO == null)
                {
                    string className = namespaceString + ".CardEntitySNDAO";
                    cardEntitySNDAO = (ICardEntitySNDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return cardEntitySNDAO;
            }
        }
         
        private static ICardSignLogDAO cardSignLogDAO;
        public static ICardSignLogDAO CardSignLogDAO
        {
            get
            {
                if (cardSignLogDAO == null)
                {
                    string className = namespaceString + ".CardSignLogDAO";
                    cardSignLogDAO = (ICardSignLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return cardSignLogDAO;
            }
        }

        private static ICardCashLogDAO cardCashLogDAO;
        public static ICardCashLogDAO CardCashLogDAO
        {
            get
            {
                if (cardCashLogDAO == null)
                {
                    string className = namespaceString + ".CardCashLogDAO";
                    cardCashLogDAO = (ICardCashLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return cardCashLogDAO;
            }
        }

        private static ICollectDAO collectDAO;
        public static ICollectDAO CollectDAO
        {
            get
            {
                if (collectDAO == null)
                {
                    string className = namespaceString + ".CollectDAO";
                    collectDAO = (ICollectDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return collectDAO;
            }
        }

        private static ICollectItemDAO collectItemDAO;
        public static ICollectItemDAO CollectItemDAO
        {
            get
            {
                if (collectItemDAO == null)
                {
                    string className = namespaceString + ".CollectItemDAO";
                    collectItemDAO = (ICollectItemDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return collectItemDAO;
            }
        }

        private static ICollectLogDAO collectLogDAO;
        public static ICollectLogDAO CollectLogDAO
        {
            get
            {
                if (collectLogDAO == null)
                {
                    string className = namespaceString + ".CollectLogDAO";
                    collectLogDAO = (ICollectLogDAO)Assembly.Load(assemblyString).CreateInstance(className);
                }
                return collectLogDAO;
            }
        }
	}
}
