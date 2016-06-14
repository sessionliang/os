using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using System.Collections;

using SiteServer.B2C.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using System.Text;
using System;
using System.Collections.Generic;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.Core
{
    public class PriceManager
    {
        public static decimal GetPrice(PublishmentSystemInfo publishmentSystemInfo, CartInfo cartInfo, bool isPurchaseNum)
        {
            decimal price = 0;
            if (cartInfo.GoodsID > 0)
            {
                price = DataProviderB2C.GoodsDAO.GetPriceSale(cartInfo.GoodsID);
            }
            if (price <= 0)
            {
                price = DataProviderB2C.GoodsContentDAO.GetPriceSale(publishmentSystemInfo, cartInfo.ChannelID, cartInfo.ContentID);
            }
            if (isPurchaseNum)
            {
                price = price * cartInfo.PurchaseNum;
            }
            return price;
        }

        public static decimal GetPrice(GoodsContentInfo goodsContentInfo, GoodsInfo goodsInfo)
        {
            decimal price = 0;
            if (goodsInfo.PriceSale > 0)
            {
                price = goodsInfo.PriceSale;
            }
            else
            {
                price = goodsContentInfo.PriceSale;
            }
            return price;
        }

        public static string GetDisplayMoney(decimal price)
        {
            if (price > 0)
            {
                return price.ToString("C");
            }
            return "免费";
        }

        public static AmountInfo GetAmountInfoByCarts(PublishmentSystemInfo publishmentSystemInfo, List<CartInfo> cartInfoList)
        {
            decimal priceActual = 0;
            decimal priceReturn = 0;
            decimal priceShipment = 0;
            decimal priceTotal = 0;
            int purchaseCount = 0;
            decimal priceTotalCanPromotion = 0;
            int purchaseCountCanPromotion = 0;

            List<PromotionInfo> promotionInfoList = DataProviderB2C.PromotionDAO.GetEnabledPromotionInfoList(publishmentSystemInfo.PublishmentSystemID);

            Dictionary<int, PromotionInfo> ifCountDictionary = new Dictionary<int, PromotionInfo>();
            Dictionary<decimal, PromotionInfo> ifAmountDictionary = new Dictionary<decimal, PromotionInfo>();

            List<int> excludeChannelIDList = new List<int>();
            List<string> excludeIDsList = new List<string>();

            foreach (PromotionInfo promotionInfo in promotionInfoList)
            {
                EPromotionTarget target = EPromotionTargetUtils.GetEnumType(promotionInfo.Target);
                if (target != EPromotionTarget.Site)
                {
                    continue;
                }

                if (promotionInfo.IfAmount > 0)
                {
                    ifAmountDictionary[promotionInfo.IfAmount] = promotionInfo;
                }

                if (promotionInfo.IfCount > 0)
                {
                    ifCountDictionary[promotionInfo.IfCount] = promotionInfo;
                }

                if (!string.IsNullOrEmpty(promotionInfo.ExcludeChannelIDCollection))
                {
                    foreach (int channelID in TranslateUtils.StringCollectionToIntList(promotionInfo.ExcludeChannelIDCollection))
                    {
                        if (channelID > 0)
                        {
                            if (!excludeChannelIDList.Contains(channelID))
                            {
                                excludeChannelIDList.Add(channelID);
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(promotionInfo.ExcludeIDsCollection))
                {
                    foreach (var pair in TranslateUtils.StringCollectionToIntDictionary(promotionInfo.ExcludeIDsCollection))
                    {
                        int channelID = pair.Key;
                        int contentID = pair.Value;

                        if (channelID > 0 && contentID > 0)
                        {
                            if (!excludeIDsList.Contains(string.Format("{0}_{1}", channelID, contentID)))
                            {
                                excludeIDsList.Add(string.Format("{0}_{1}", channelID, contentID));
                            }
                        }
                    }
                }
            }

            foreach (CartInfo cartInfo in cartInfoList)
            {

                GoodsContentInfo contentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(publishmentSystemInfo, cartInfo.ChannelID, cartInfo.ContentID);
                GoodsInfo goodsInfo = DataProviderB2C.GoodsDAO.GetGoodsInfoForDefault(cartInfo.GoodsID, contentInfo);
                if (contentInfo == null)
                    continue;
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, contentInfo.NodeID);
                if (nodeInfo == null)
                    continue;
                int purchaseNum = cartInfo.PurchaseNum;
                decimal price = PriceManager.GetPrice(publishmentSystemInfo, cartInfo, true);

                purchaseCount += purchaseNum;
                priceTotal += price;
                string pair = cartInfo.ChannelID + "_" + cartInfo.ContentID;
                if (!excludeChannelIDList.Contains(cartInfo.ChannelID) && !excludeIDsList.Contains(pair))
                {
                    purchaseCountCanPromotion += purchaseNum;
                    priceTotalCanPromotion += price;
                }
            }
            if (priceTotal > 0)
            {
                priceShipment = publishmentSystemInfo.Additional.ShipmentPrice;
            }

            if (ifAmountDictionary.Count > 0 || ifCountDictionary.Count > 0)
            {
                List<PromotionInfo> promotionInfoAvaliableList = new List<PromotionInfo>();

                foreach (decimal ifAmount in ifAmountDictionary.Keys)
                {
                    if (priceTotalCanPromotion >= ifAmount)
                    {
                        promotionInfoAvaliableList.Add(ifAmountDictionary[ifAmount]);
                    }
                }

                foreach (int ifCount in ifCountDictionary.Keys)
                {
                    if (purchaseCountCanPromotion >= ifCount)
                    {
                        promotionInfoAvaliableList.Add(ifCountDictionary[ifCount]);
                    }
                }

                bool isShipmentFree = false;

                foreach (PromotionInfo promotionInfoAvaliable in promotionInfoAvaliableList)
                {
                    priceReturn += promotionInfoAvaliable.ReturnAmount;
                    if (promotionInfoAvaliable.IsShipmentFree && !isShipmentFree)
                    {
                        isShipmentFree = true;
                        priceReturn += priceShipment;
                    }

                    if (promotionInfoAvaliable.Discount > 0 && promotionInfoAvaliable.Discount < 1)
                    {
                        priceReturn += (priceTotal - priceReturn) * (1 - promotionInfoAvaliable.Discount);
                    }
                }
            }

            priceReturn = -priceReturn;

            priceActual = priceTotal + priceReturn + priceShipment;

            return new AmountInfo { PriceTotal = priceTotal, PriceReturn = priceReturn, PriceShipment = priceShipment, PriceActual = priceActual };
        }
    }
}
