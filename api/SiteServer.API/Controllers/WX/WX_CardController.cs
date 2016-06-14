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
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;



namespace SiteServer.API.Controllers.WX
{
    public class WX_CardController : ApiController
    {
        [HttpGet]
        [ActionName("GetCardParameter")]
        public IHttpActionResult GetCardParameter(int id)
        {
            string poweredBy = string.Empty;
            PublishmentSystemManager.ClearCache(false);
            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

            try
            {
                DataProviderWX.CardDAO.AddPVCount(id);

                UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(publishmentSystemInfo.GroupSN, RequestUtils.CurrentUserName);// RequestUtils.Current;

                CardInfo cardInfo = DataProviderWX.CardDAO.GetCardInfo(id);
                List<CardOperatorInfo> operatorInfoList = new List<CardOperatorInfo>();
                operatorInfoList = TranslateUtils.JsonToObject(cardInfo.ShopOperatorList, operatorInfoList) as List<CardOperatorInfo>;
                cardInfo.ShopOperatorList = TranslateUtils.ObjectToJson(operatorInfoList);
                cardInfo.ImageUrl = CardManager.GetImageUrl(publishmentSystemInfo, cardInfo.ImageUrl);
                cardInfo.ContentFrontImageUrl = CardManager.GetContentFrontImageUrl(publishmentSystemInfo, cardInfo.ContentFrontImageUrl);
                cardInfo.ContentBackImageUrl = CardManager.GetContentBackImageUrl(publishmentSystemInfo, cardInfo.ContentBackImageUrl);
                cardInfo.ShopAddress = StringUtils.MaxLengthText(cardInfo.ShopAddress, 14);

                bool isAnonymous = false;
                bool isBinding = publishmentSystemInfo.Additional.Card_IsBinding;
                bool isExchange = publishmentSystemInfo.Additional.Card_IsExchange;
                bool isSign = publishmentSystemInfo.Additional.Card_IsSign;
                decimal exchangeProportion = publishmentSystemInfo.Additional.Card_ExchangeProportion;

                CardSNInfo cardSNInfo = DataProviderWX.CardSNDAO.GetCardSNInfo(publishmentSystemInfo.PublishmentSystemID, cardInfo.ID, string.Empty, RequestUtils.CurrentUserName);
                if (cardSNInfo == null || string.IsNullOrEmpty(RequestUtils.CurrentUserName))
                {
                    isAnonymous = true;
                    cardSNInfo = new CardSNInfo();
                    cardSNInfo.SN = DataProviderWX.CardSNDAO.GetNextCardSN(publishmentSystemInfo.PublishmentSystemID, id);
                }

                CardEntitySNInfo cardEntitySNInfo = DataProviderWX.CardEntitySNDAO.GetCardEntitySNInfo(id, string.Empty, userInfo != null ? userInfo.Mobile : string.Empty);
                if (cardEntitySNInfo == null)
                {
                    cardEntitySNInfo = new CardEntitySNInfo();
                    cardEntitySNInfo.IsBinding = false;
                }

                UserContactInfo userContactInfo = BaiRongDataProvider.UserContactDAO.GetContactInfo(RequestUtils.CurrentUserName);

                var parameter = new { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, IsAnonymous = isAnonymous, IsBinding = isBinding, IsExchange = isExchange, IsSign = isSign, ExchangeProportion = exchangeProportion, CardInfo = cardInfo, CardSNInfo = cardSNInfo, CardEntitySNInfo = cardEntitySNInfo, UserInfo = userInfo, UserContactInfo = userContactInfo };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var parameter = new { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

                return Ok(parameter);
            }
        }


        [HttpGet]
        [ActionName("Register")]
        public IHttpActionResult Register()
        {
            string userName = RequestUtils.GetQueryString("userName");
            string password = RequestUtils.GetQueryString("password");
            string email = RequestUtils.GetQueryString("email");
            string mobile = RequestUtils.GetQueryString("mobile");

            string poweredBy = string.Empty;
            string successMessage = string.Empty;
            string errorMessage = string.Empty;
            bool isRedirectToLogin = false;
            string groupSN = RequestUtils.PublishmentSystemInfo.GroupSN;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(RequestUtils.PublishmentSystemInfo, out poweredBy);
            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            string homeUrl = publishmentSystemInfo != null ? publishmentSystemInfo.PublishmentSystemUrl : string.Empty;
            bool isSuccess = UserManager.RegisterByUserController(groupSN, userName, password, email, mobile, homeUrl, out isRedirectToLogin, out successMessage, out errorMessage);

            var parameter = new { IsSuccess = isSuccess, ErrorMessage = errorMessage, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

            return Ok(parameter);

        }

        [HttpGet]
        [ActionName("Login")]
        public IHttpActionResult Login()
        {
            string loginName = RequestUtils.GetQueryString("userName");
            string password = RequestUtils.GetQueryString("password");
            bool isPersistent = RequestUtils.GetBoolQueryString("isPersistent");

            string userName = string.Empty;
            string poweredBy = string.Empty;
            string errorMessage = string.Empty;

            string groupSN = RequestUtils.PublishmentSystemInfo.GroupSN;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(RequestUtils.PublishmentSystemInfo, out poweredBy);
            bool isSuccess = BaiRongDataProvider.UserDAO.ValidateByLoginName(groupSN, loginName, password, out userName, out errorMessage);
            if (isSuccess)
            {
                CardSNInfo cardSNInfo = new CardSNInfo();

                PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
                cardSNInfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                cardSNInfo.CardID = RequestUtils.GetIntQueryString("cardID");
                cardSNInfo.SN = DataProviderWX.CardSNDAO.GetNextCardSN(publishmentSystemInfo.PublishmentSystemID, RequestUtils.GetIntQueryString("cardID"));
                cardSNInfo.UserName = userName;
                cardSNInfo.IsDisabled = true;
                cardSNInfo.AddDate = DateTime.Now;

                bool isExist = DataProviderWX.CardSNDAO.isExists(RequestUtils.PublishmentSystemInfo.PublishmentSystemID, RequestUtils.GetIntQueryString("cardID"), userName);
                if (!isExist)
                {
                    DataProviderWX.CardSNDAO.Insert(cardSNInfo);
                    if (publishmentSystemInfo.Additional.Card_IsGiveConsumeCredits)
                    {
                        UserCreditsLogInfo userCreditsLogInfo = new UserCreditsLogInfo();
                        userCreditsLogInfo.UserName = userName;
                        userCreditsLogInfo.ProductID = AppManager.WeiXin.AppID;
                        userCreditsLogInfo.Num = publishmentSystemInfo.Additional.Card_ClaimCardCredits;
                        userCreditsLogInfo.AddDate = DateTime.Now;
                        userCreditsLogInfo.IsIncreased = true;
                        userCreditsLogInfo.Action = "领卡送积分";

                        BaiRongDataProvider.UserCreditsLogDAO.Insert(userCreditsLogInfo);
                        BaiRongDataProvider.UserDAO.AddCredits(publishmentSystemInfo.GroupSN, userName, publishmentSystemInfo.Additional.Card_ClaimCardCredits);
                    }
                }

                BaiRongDataProvider.UserDAO.Login(groupSN, userName, isPersistent);
            }
            else
            {
                errorMessage = "登录失败，" + errorMessage;
            }

            var parameter = new { IsSuccess = isSuccess, ErrorMessage = errorMessage, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

            return Ok(parameter);

        }

        [HttpGet]
        [ActionName("GetUser")]
        public IHttpActionResult GetUser()
        {
            string poweredBy = string.Empty;
            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

            try
            {
                UserInfo userInfo = BaiRongDataProvider.UserDAO.GetUserInfo(publishmentSystemInfo.GroupSN, RequestUtils.CurrentUserName);// RequestUtils.Current;
                UserContactInfo userContactInfo = BaiRongDataProvider.UserContactDAO.GetContactInfo(RequestUtils.CurrentUserName);

                var parameter = new { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, UserInfo = userInfo, UserContactInfo = userContactInfo };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var parameter = new { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("EidtUser")]
        public IHttpActionResult EidtUser()
        {
            string poweredBy = string.Empty;
            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

            string displayName = RequestUtils.GetQueryString("displayName");
            string mobile = RequestUtils.GetQueryString("mobile");
            string gender = RequestUtils.GetQueryString("gender");
            string birthday = RequestUtils.GetQueryString("birthday");
            string position = RequestUtils.GetQueryString("position");
            string address = RequestUtils.GetQueryString("address");

            try
            {
                UserInfo userInfo = RequestUtils.Current;
                int userID = BaiRongDataProvider.UserDAO.GetUserID(publishmentSystemInfo.GroupSN, RequestUtils.CurrentUserName);
                UserContactInfo userContactInfo = BaiRongDataProvider.UserContactDAO.GetContactInfo(RequestUtils.CurrentUserName);

                if (userContactInfo == null)
                {
                    userContactInfo = new UserContactInfo();
                }

                userInfo.UserID = userID;
                userInfo.DisplayName = displayName;
                userInfo.Mobile = mobile;

                userContactInfo.RelatedUserName = userInfo.UserName;
                userContactInfo.Gender = gender;
                userContactInfo.Birthday = birthday;
                userContactInfo.Position = position;
                userContactInfo.Address = address;

                BaiRongDataProvider.UserDAO.Update(userInfo);
                if (userContactInfo.ID <= 0)
                {
                    BaiRongDataProvider.UserContactDAO.Insert(userContactInfo);
                }
                else
                {
                    BaiRongDataProvider.UserContactDAO.Update(userContactInfo);
                }

                var parameter = new { IsSuccess = true, ErrorMessage = string.Empty, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, UserInfo = userInfo, UserContactInfo = userContactInfo };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var parameter = new { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

                return Ok(parameter);
            }
        }


        [HttpGet]
        [ActionName("BindCard")]
        public IHttpActionResult BindCard(int id)
        {
            string poweredBy = string.Empty;
            string errorMessage = string.Empty;
            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

            string cardSN = RequestUtils.GetQueryString("cardSN");
            string mobile = RequestUtils.GetQueryString("mobile");

            try
            {
                UserInfo userInfo = RequestUtils.Current;
                CardEntitySNInfo cardEntitySNInfo = DataProviderWX.CardEntitySNDAO.GetCardEntitySNInfo(id, cardSN, mobile);
                if (cardEntitySNInfo == null)
                {
                    errorMessage = "实体卡不存在";
                }
                CardSNInfo cardSNInfo = DataProviderWX.CardSNDAO.GetCardSNInfo(publishmentSystemInfo.PublishmentSystemID, id, string.Empty, RequestUtils.CurrentUserName);
                if (cardSNInfo == null)
                {
                    errorMessage = "会员卡不存在";
                }
                if (cardEntitySNInfo != null && cardSNInfo != null)
                {
                    bool isBinding = cardEntitySNInfo.IsBinding;
                    cardEntitySNInfo.IsBinding = true;

                    CardCashLogInfo cardCashLogInfo = new CardCashLogInfo();
                    cardCashLogInfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                    cardCashLogInfo.UserName = RequestUtils.CurrentUserName;
                    cardCashLogInfo.CardID = cardSNInfo.CardID;
                    cardCashLogInfo.CardSNID = cardSNInfo.ID;
                    cardCashLogInfo.Amount = cardEntitySNInfo.Amount;
                    cardCashLogInfo.CurAmount += cardEntitySNInfo.Amount; ;
                    cardCashLogInfo.CashType = ECashTypeUtils.GetValue(ECashType.Recharge);
                    cardCashLogInfo.Operator = RequestUtils.CurrentUserName;
                    cardCashLogInfo.Description = "绑定实体卡充值";
                    cardCashLogInfo.AddDate = DateTime.Now;

                    UserCreditsLogInfo userCreditsLogInfo = new UserCreditsLogInfo();
                    userCreditsLogInfo.UserName = RequestUtils.CurrentUserName;
                    userCreditsLogInfo.ProductID = AppManager.WeiXin.AppID;
                    userCreditsLogInfo.Num = cardEntitySNInfo.Credits;
                    userCreditsLogInfo.IsIncreased = true;
                    userCreditsLogInfo.Action = "绑定实体卡添加积分";
                    userCreditsLogInfo.AddDate = DateTime.Now;

                    if (isBinding)
                    {
                        errorMessage = "实体卡已绑定";
                    }
                    else
                    {
                        if (userInfo.Mobile != cardEntitySNInfo.Mobile)
                        {
                            errorMessage = "实体卡不匹配！";
                        }
                        else
                        {
                            DataProviderWX.CardEntitySNDAO.Update(cardEntitySNInfo);

                            DataProviderWX.CardCashLogDAO.Insert(cardCashLogInfo);
                            DataProviderWX.CardSNDAO.Recharge(cardSNInfo.ID, RequestUtils.CurrentUserName, cardEntitySNInfo.Amount);

                            BaiRongDataProvider.UserCreditsLogDAO.Insert(userCreditsLogInfo);
                            BaiRongDataProvider.UserDAO.AddCredits(publishmentSystemInfo.GroupSN, RequestUtils.CurrentUserName, cardEntitySNInfo.Credits);
                        }
                    }
                }

                var parameter = new { IsSuccess = true, ErrorMessage = errorMessage, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var parameter = new { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("Consume")]
        public IHttpActionResult Consume(int id)
        {
            string poweredBy = string.Empty;
            string errorMessage = string.Empty;
            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

            decimal amount = TranslateUtils.ToDecimal(RequestUtils.GetQueryString("amount"));
            string theOperator = RequestUtils.GetQueryString("operator");
            string password = RequestUtils.GetQueryString("password");

            try
            {
                CardInfo cardInfo = DataProviderWX.CardDAO.GetCardInfo(id);
                if (cardInfo != null)
                {
                    List<CardOperatorInfo> operatorInfoList = new List<CardOperatorInfo>();
                    operatorInfoList = TranslateUtils.JsonToObject(cardInfo.ShopOperatorList, operatorInfoList) as List<CardOperatorInfo>;
                    foreach (CardOperatorInfo operatorInfo in operatorInfoList)
                    {
                        if (operatorInfo.UserName == theOperator)
                        {
                            if (!StringUtils.EqualsIgnoreCase(operatorInfo.Password, password))
                            {
                                errorMessage = "消费密码不正确";
                            }
                            break;
                        }
                    }
                }

                CardSNInfo cardSNInfo = DataProviderWX.CardSNDAO.GetCardSNInfo(publishmentSystemInfo.PublishmentSystemID, id, string.Empty, RequestUtils.CurrentUserName);
                if (cardSNInfo == null)
                {
                    errorMessage = "会员卡不存在或被删除";
                }
                else
                {
                    CardCashLogInfo cardCashLogInfo = new CardCashLogInfo();
                    cardCashLogInfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                    cardCashLogInfo.UserName = RequestUtils.CurrentUserName;
                    cardCashLogInfo.CardID = cardSNInfo.CardID;
                    cardCashLogInfo.CardSNID = cardSNInfo.ID;
                    cardCashLogInfo.Amount = amount;
                    cardCashLogInfo.CurAmount = cardSNInfo.Amount - amount;
                    cardCashLogInfo.CashType = ECashTypeUtils.GetValue(ECashType.Consume);
                    cardCashLogInfo.ConsumeType = EConsumeTypeUtils.GetValue(EConsumeType.CardAmount);
                    cardCashLogInfo.Operator = RequestUtils.CurrentUserName;
                    cardCashLogInfo.Description = "会员卡余额消费";
                    cardCashLogInfo.AddDate = DateTime.Now;

                    if (cardSNInfo.Amount < amount)
                    {
                        errorMessage = "会员卡余额不足";
                    }

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        DataProviderWX.CardCashLogDAO.Insert(cardCashLogInfo);
                        DataProviderWX.CardSNDAO.Consume(cardSNInfo.ID, RequestUtils.CurrentUserName, amount);

                        if (publishmentSystemInfo.Additional.Card_IsGiveConsumeCredits)
                        {
                            decimal consumeAmount = publishmentSystemInfo.Additional.Card_ConsumeAmount;
                            int giveCredits = publishmentSystemInfo.Additional.Card_GiveCredits;

                            UserCreditsLogInfo userCreditsLogInfo = new UserCreditsLogInfo();
                            userCreditsLogInfo.UserName = cardSNInfo.UserName;
                            userCreditsLogInfo.ProductID = AppManager.WeiXin.AppID;
                            userCreditsLogInfo.Num = (int)Math.Round(amount * (giveCredits / consumeAmount), 0);
                            userCreditsLogInfo.AddDate = DateTime.Now;
                            userCreditsLogInfo.IsIncreased = true;
                            userCreditsLogInfo.Action = "消费送积分";
                            BaiRongDataProvider.UserCreditsLogDAO.Insert(userCreditsLogInfo);
                            BaiRongDataProvider.UserDAO.AddCredits(publishmentSystemInfo.GroupSN, cardSNInfo.UserName, (int)Math.Round(amount * (giveCredits / consumeAmount), 0));

                        }
                    }
                }

                var parameter = new { IsSuccess = true, ErrorMessage = errorMessage, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var parameter = new { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("Exchange")]
        public IHttpActionResult Exchange(int id)
        {
            string poweredBy = string.Empty;
            string errorMessage = string.Empty;
            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

            int credits = RequestUtils.GetIntQueryString("credits");

            try
            {
                UserInfo userInfo = RequestUtils.Current;

                CardSNInfo cardSNInfo = DataProviderWX.CardSNDAO.GetCardSNInfo(publishmentSystemInfo.PublishmentSystemID, id, string.Empty, RequestUtils.CurrentUserName);
                if (cardSNInfo == null)
                {
                    errorMessage = "会员卡不存在或被删除";
                }
                else
                {
                    if (userInfo.Credits < credits)
                    {
                        errorMessage = "兑换积分大于可用积分";
                    }

                    decimal exchangeProportion = publishmentSystemInfo.Additional.Card_ExchangeProportion;

                    decimal exchangeAmount = credits / exchangeProportion;

                    UserCreditsLogInfo userCreditsLogInfo = new UserCreditsLogInfo();
                    userCreditsLogInfo.UserName = userInfo.UserName;
                    userCreditsLogInfo.ProductID = AppManager.WeiXin.AppID;
                    userCreditsLogInfo.Num = credits;
                    userCreditsLogInfo.AddDate = DateTime.Now;
                    userCreditsLogInfo.IsIncreased = false;
                    userCreditsLogInfo.Action = "积分兑换现金";

                    CardCashLogInfo cardCashLogInfo = new CardCashLogInfo();
                    cardCashLogInfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                    cardCashLogInfo.UserName = RequestUtils.CurrentUserName;
                    cardCashLogInfo.CardID = cardSNInfo.CardID;
                    cardCashLogInfo.CardSNID = cardSNInfo.ID;
                    cardCashLogInfo.Amount = exchangeAmount;
                    cardCashLogInfo.CurAmount = cardSNInfo.Amount + exchangeAmount;
                    cardCashLogInfo.CashType = ECashTypeUtils.GetValue(ECashType.Exchange);
                    cardCashLogInfo.ConsumeType = EConsumeTypeUtils.GetValue(EConsumeType.CardAmount);
                    cardCashLogInfo.Operator = RequestUtils.CurrentUserName;
                    cardCashLogInfo.Description = string.Format("{0}积分兑换{1}元现金", credits, exchangeAmount);
                    cardCashLogInfo.AddDate = DateTime.Now;

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        BaiRongDataProvider.UserCreditsLogDAO.Insert(userCreditsLogInfo);
                        BaiRongDataProvider.UserDAO.AddCredits(userInfo.GroupSN, userInfo.UserName, -credits);

                        DataProviderWX.CardCashLogDAO.Insert(cardCashLogInfo);
                        DataProviderWX.CardSNDAO.Exchange(cardSNInfo.ID, RequestUtils.CurrentUserName, exchangeAmount);
                    }
                }

                var parameter = new { IsSuccess = true, ErrorMessage = errorMessage, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var parameter = new { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("GetCardCashLog")]
        public IHttpActionResult GetCardCashLog(int id)
        {
            string poweredBy = string.Empty;
            string errorMessage = string.Empty;
            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

            try
            {
                UserInfo userInfo = RequestUtils.Current;

                CardInfo cardInfo = DataProviderWX.CardDAO.GetCardInfo(id);
                cardInfo.ImageUrl = CardManager.GetImageUrl(publishmentSystemInfo, cardInfo.ImageUrl);
                cardInfo.ContentFrontImageUrl = CardManager.GetContentFrontImageUrl(publishmentSystemInfo, cardInfo.ContentFrontImageUrl);
                cardInfo.ContentBackImageUrl = CardManager.GetContentBackImageUrl(publishmentSystemInfo, cardInfo.ContentBackImageUrl);

                List<CardCashYearCountInfo> cardCashYearCountInfoList = new List<CardCashYearCountInfo>();
                CardSNInfo cardSNInfo = DataProviderWX.CardSNDAO.GetCardSNInfo(publishmentSystemInfo.PublishmentSystemID, cardInfo.ID, string.Empty, RequestUtils.CurrentUserName);
                if (cardSNInfo == null)
                {
                    errorMessage = "会员卡不存在或被删除";
                }
                else
                {
                    cardCashYearCountInfoList = DataProviderWX.CardCashLogDAO.GetCardCashYearCountInfoList(id, cardSNInfo.ID, RequestUtils.CurrentUserName);
                    foreach (CardCashYearCountInfo cardCashYearCountInfo in cardCashYearCountInfoList)
                    {
                        List<CardCashMonthCountInfo> cardCashMonthCountInfoList = DataProviderWX.CardCashLogDAO.GetCardCashMonthCountInfoList(id, cardSNInfo.ID, RequestUtils.CurrentUserName, cardCashYearCountInfo.Year);
                        foreach (CardCashMonthCountInfo cardCashMonthCountInfo in cardCashMonthCountInfoList)
                        {
                            List<CardCashLogInfo> cardCashLogInfoList = DataProviderWX.CardCashLogDAO.GetCardCashLogInfoList(id, cardSNInfo.ID, RequestUtils.CurrentUserName, string.Format("{0}-{1}-{2}", cardCashMonthCountInfo.Year, cardCashMonthCountInfo.Month, 1), string.Format("{0}-{1}-{2}", cardCashMonthCountInfo.Year, TranslateUtils.ToInt(cardCashMonthCountInfo.Month) + 1, 1));
                            cardCashMonthCountInfo.CardCashLogInfoList = cardCashLogInfoList;
                        }

                        cardCashYearCountInfo.CardCashMonthCountInfoList = cardCashMonthCountInfoList;
                    }
                }

                var parameter = new { IsSuccess = true, ErrorMessage = errorMessage, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, CardInfo = cardInfo, CardSNInfo = cardSNInfo, UserInfo = userInfo, CardCashYearCountInfoList = cardCashYearCountInfoList };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var parameter = new { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("Sign")]
        public IHttpActionResult Sign()
        {
            string poweredBy = string.Empty;
            string errorMessage = string.Empty;
            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

            try
            {
                CardSignLogInfo cardSignLogInfo = new CardSignLogInfo();
                cardSignLogInfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                cardSignLogInfo.UserName = RequestUtils.CurrentUserName;
                cardSignLogInfo.SignDate = DateTime.Now;
                bool isSign = DataProviderWX.CardSignLogDAO.IsSign(publishmentSystemInfo.PublishmentSystemID, RequestUtils.CurrentUserName);
                if (isSign)
                {
                    errorMessage = "今日已签到";
                }
                else
                {
                    bool theIsSign = publishmentSystemInfo.Additional.Card_IsSign;
                    if (!theIsSign)
                    {
                        DataProviderWX.CardSignLogDAO.Insert(cardSignLogInfo);
                    }
                    else
                    {
                        int credits = CardManager.GetSignCredits(publishmentSystemInfo, RequestUtils.CurrentUserName);

                        UserCreditsLogInfo userCreditsLogInfo = new UserCreditsLogInfo();
                        userCreditsLogInfo.UserName = RequestUtils.CurrentUserName;
                        userCreditsLogInfo.ProductID = AppManager.WeiXin.AppID;
                        userCreditsLogInfo.Num = credits;
                        userCreditsLogInfo.IsIncreased = true;
                        userCreditsLogInfo.Action = DataProviderWX.CardSignLogDAO.GetSignAction();
                        userCreditsLogInfo.Description = "签到领取积分";
                        userCreditsLogInfo.AddDate = DateTime.Now;

                        DataProviderWX.CardSignLogDAO.Insert(cardSignLogInfo);
                        BaiRongDataProvider.UserCreditsLogDAO.Insert(userCreditsLogInfo);
                        BaiRongDataProvider.UserDAO.AddCredits(publishmentSystemInfo.GroupSN, RequestUtils.CurrentUserName, credits);
                    }
                }

                var parameter = new { IsSuccess = true, ErrorMessage = errorMessage, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var parameter = new { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

                return Ok(parameter);
            }
        }


        [HttpGet]
        [ActionName("GetSignRecord")]
        public IHttpActionResult GetSignRecord()
        {
            string poweredBy = string.Empty;
            string errorMessage = string.Empty;
            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

            try
            {
                UserInfo userInfo = RequestUtils.Current;

                List<CardSignLogInfo> cardSignLogInfoList = DataProviderWX.CardSignLogDAO.GetCardSignLogInfoList(publishmentSystemInfo.PublishmentSystemID, RequestUtils.CurrentUserName);

                List<UserCreditsLogInfo> userCreditsLogInfoList = BaiRongDataProvider.UserCreditsLogDAO.GetUserCreditsLogInfoList(RequestUtils.CurrentUserName, AppManager.WeiXin.AppID, DataProviderWX.CardSignLogDAO.GetSignAction());

                var parameter = new { IsSuccess = true, ErrorMessage = errorMessage, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, UserInfo = userInfo, CardSignLogInfoList = cardSignLogInfoList, UserCreditsLogInfoList = userCreditsLogInfoList };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var parameter = new { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

                return Ok(parameter);
            }
        }

        [HttpGet]
        [ActionName("GetSignRule")]
        public IHttpActionResult GetSignRule()
        {
            string poweredBy = string.Empty;
            string errorMessage = string.Empty;
            PublishmentSystemInfo publishmentSystemInfo = RequestUtils.PublishmentSystemInfo;
            bool isPoweredBy = WeiXinManager.IsPoweredBy(publishmentSystemInfo, out poweredBy);

            try
            {
                List<string> signRuleList = new List<string>();
                string signCreditsConfigure = publishmentSystemInfo.Additional.Card_SignCreditsConfigure;
                if (!string.IsNullOrEmpty(signCreditsConfigure))
                {
                    signRuleList = TranslateUtils.StringCollectionToStringList(signCreditsConfigure);
                }

                var parameter = new { IsSuccess = true, ErrorMessage = errorMessage, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy, SignRuleList = signRuleList };

                return Ok(parameter);
            }
            catch (Exception ex)
            {
                var parameter = new { IsSuccess = false, ErrorMessage = ex.Message, IsPoweredBy = isPoweredBy, PoweredBy = poweredBy };

                return Ok(parameter);
            }
        }

    }
}
