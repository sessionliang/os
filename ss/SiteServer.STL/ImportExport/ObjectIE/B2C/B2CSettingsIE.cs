using System;
using System.Collections;
using Atom.Core;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using System.Collections.Specialized;
using SiteServer.CMS.Core;
using SiteServer.B2C.Core;
using System.Collections.Generic;
using SiteServer.B2C.Model;

namespace SiteServer.STL.ImportExport.B2C
{
	internal class B2CSettingsIE
	{
		private readonly int publishmentSystemID;
		private readonly string directoryPath;

        private const string TYPE_PAYMENT = "Payment";
        private const string TYPE_SHIPMENT = "Shipment";
        private const string TYPE_PROMOTION = "Promotion";

        public B2CSettingsIE(int publishmentSystemID, string directoryPath)
		{
			this.publishmentSystemID = publishmentSystemID;
			this.directoryPath = directoryPath;
		}

        public void ExportSettings()
		{
            List<string> typeList = new List<string>();
            typeList.Add(B2CSettingsIE.TYPE_PAYMENT);
            typeList.Add(B2CSettingsIE.TYPE_SHIPMENT);
            typeList.Add(B2CSettingsIE.TYPE_PROMOTION);

            foreach (string type in typeList)
            {
                string filePath = this.directoryPath + PathUtils.SeparatorChar + type + ".xml";

                AtomFeed feed = AtomUtility.GetEmptyFeed();

                AtomUtility.AddDcElement(feed.AdditionalElements, "type", type);

                if (type == B2CSettingsIE.TYPE_PAYMENT)
                {
                    List<PaymentInfo> paymentInfoList = DataProviderB2C.PaymentDAO.GetPaymentInfoList(this.publishmentSystemID);
                    foreach (PaymentInfo paymentInfo in paymentInfoList)
                    {
                        AtomEntry entry = AtomUtility.GetEmptyEntry();

                        AtomUtility.AddDcElement(entry.AdditionalElements, "ID", paymentInfo.ID.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, "PublishmentSystemID", paymentInfo.PublishmentSystemID.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, "PaymentType", EPaymentTypeUtils.GetValue(paymentInfo.PaymentType));
                        AtomUtility.AddDcElement(entry.AdditionalElements, "PaymentName", paymentInfo.PaymentName);
                        AtomUtility.AddDcElement(entry.AdditionalElements, "IsEnabled", paymentInfo.IsEnabled.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, "IsOnline", paymentInfo.IsOnline.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, "Taxis", paymentInfo.Taxis.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, "Description", AtomUtility.Encrypt(paymentInfo.Description));

                        feed.Entries.Add(entry);
                    }
                }
                else if (type == B2CSettingsIE.TYPE_SHIPMENT)
                {
                    List<ShipmentInfo> shipmentInfoList = DataProviderB2C.ShipmentDAO.GetShipmentInfoList(this.publishmentSystemID);
                    foreach (ShipmentInfo shipmentInfo in shipmentInfoList)
                    {
                        AtomEntry entry = AtomUtility.GetEmptyEntry();

                        AtomUtility.AddDcElement(entry.AdditionalElements, "ID", shipmentInfo.ID.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, "PublishmentSystemID", shipmentInfo.PublishmentSystemID.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, "ShipmentName", shipmentInfo.ShipmentName);
                        AtomUtility.AddDcElement(entry.AdditionalElements, "ShipmentPeriod", EShipmentPeriodUtils.GetValue(shipmentInfo.ShipmentPeriod));
                        AtomUtility.AddDcElement(entry.AdditionalElements, "IsEnabled", shipmentInfo.IsEnabled.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, "Taxis", shipmentInfo.Taxis.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, "Description", AtomUtility.Encrypt(shipmentInfo.Description));

                        feed.Entries.Add(entry);
                    }
                }
                else if (type == B2CSettingsIE.TYPE_PROMOTION)
                {
                    List<PromotionInfo> promotionInfoList = DataProviderB2C.PromotionDAO.GetPromotionInfoList(this.publishmentSystemID);
                    foreach (PromotionInfo promotionInfo in promotionInfoList)
                    {
                        AtomEntry entry = AtomUtility.GetEmptyEntry();                        

                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.ID, promotionInfo.ID.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.PublishmentSystemID, promotionInfo.PublishmentSystemID.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.PromotionName, promotionInfo.PromotionName);
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.StartDate, promotionInfo.StartDate.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.EndDate, promotionInfo.EndDate.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.Tags, promotionInfo.Tags);
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.Target, promotionInfo.Target);
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.ChannelIDCollection, promotionInfo.ChannelIDCollection);
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.IDsCollection, promotionInfo.IDsCollection);
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.ExcludeChannelIDCollection, promotionInfo.ExcludeChannelIDCollection);
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.ExcludeIDsCollection, promotionInfo.ExcludeIDsCollection);
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.IfAmount, promotionInfo.IfAmount.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.IfCount, promotionInfo.IfCount.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.Discount, promotionInfo.Discount.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.ReturnAmount, promotionInfo.ReturnAmount.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.IsReturnMultiply, promotionInfo.IsReturnMultiply.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.IsShipmentFree, promotionInfo.IsShipmentFree.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.IsGift, promotionInfo.IsGift.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.GiftName, promotionInfo.GiftName);
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.GiftUrl, promotionInfo.GiftUrl);
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.IsEnabled, promotionInfo.IsEnabled.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.Taxis, promotionInfo.Taxis.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.AddDate, promotionInfo.AddDate.ToString());
                        AtomUtility.AddDcElement(entry.AdditionalElements, PromotionAttribute.Description, AtomUtility.Encrypt(promotionInfo.Description));

                        feed.Entries.Add(entry);
                    }
                }
                
                feed.Save(filePath);
            }
		}

		public void ImportSettings()
		{
			if (!DirectoryUtils.IsDirectoryExists(this.directoryPath)) return;
			string[] filePaths = DirectoryUtils.GetFilePaths(this.directoryPath);

			foreach (string filePath in filePaths)
			{
                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                string type = AtomUtility.GetDcElementContent(feed.AdditionalElements, "type");

                if (type == B2CSettingsIE.TYPE_PAYMENT)
                {
                    foreach (AtomEntry entry in feed.Entries)
                    {
                        PaymentInfo paymentInfo = new PaymentInfo(0, this.publishmentSystemID, EPaymentTypeUtils.GetEnumType(AtomUtility.GetDcElementContent(entry.AdditionalElements, "PaymentType")), AtomUtility.GetDcElementContent(entry.AdditionalElements, "PaymentName"), TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "IsEnabled")), TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "IsOnline")), TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Taxis")), AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Description")), string.Empty);

                        DataProviderB2C.PaymentDAO.Insert(paymentInfo);
                    }
                }
                else if (type == B2CSettingsIE.TYPE_SHIPMENT)
                {
                    foreach (AtomEntry entry in feed.Entries)
                    {
                        ShipmentInfo shipmentInfo = new ShipmentInfo(0, this.publishmentSystemID, AtomUtility.GetDcElementContent(entry.AdditionalElements, "ShipmentName"), EShipmentPeriodUtils.GetEnumType(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ShipmentPeriod")), TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "IsEnabled")), TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Taxis")), AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Description")));

                        DataProviderB2C.ShipmentDAO.Insert(shipmentInfo);
                    }
                }
                else if (type == B2CSettingsIE.TYPE_PROMOTION)
                {
                    foreach (AtomEntry entry in feed.Entries)
                    {
                        PromotionInfo promotionInfo = new PromotionInfo();

                        promotionInfo.PublishmentSystemID = this.publishmentSystemID;
                        promotionInfo.PromotionName = AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.PromotionName);
                        promotionInfo.StartDate = TranslateUtils.ToDateTime(AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.StartDate));
                        promotionInfo.EndDate = TranslateUtils.ToDateTime(AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.EndDate));
                        promotionInfo.Tags = AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.Tags);
                        promotionInfo.Target = AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.Target);

                        promotionInfo.IfAmount = TranslateUtils.ToDecimal(AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.IfAmount));
                        promotionInfo.IfCount = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.IfCount));
                        promotionInfo.Discount = TranslateUtils.ToDecimal(AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.Discount));
                        promotionInfo.ReturnAmount = TranslateUtils.ToDecimal(AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.ReturnAmount));
                        promotionInfo.IsReturnMultiply = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.IsReturnMultiply));
                        promotionInfo.IsShipmentFree = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.IsShipmentFree));
                        promotionInfo.IsGift = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.IsGift));
                        promotionInfo.GiftName = AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.GiftName);
                        promotionInfo.GiftUrl = AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.GiftUrl);
                        promotionInfo.IsEnabled = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.IsEnabled));
                        promotionInfo.AddDate = TranslateUtils.ToDateTime(AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.AddDate));
                        promotionInfo.Description = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, PromotionAttribute.Description));

                        DataProviderB2C.PromotionDAO.Insert(promotionInfo);
                    }
                }
			}
		}

	}
}
