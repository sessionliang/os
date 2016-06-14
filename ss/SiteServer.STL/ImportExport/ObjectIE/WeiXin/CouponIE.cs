using System.Collections;
using Atom.Core;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using System.Collections.Generic;
using SiteServer.WeiXin.Model;
using SiteServer.WeiXin.Core;
using System.Web.Script.Serialization;
using System.Collections.Specialized;

namespace SiteServer.STL.ImportExport
{
    internal class CouponIE
    {
        private readonly int publishmentSystemID;

        private string directoryPath;

        public CouponIE(int publishmentSystemID, string directoryPath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.directoryPath = directoryPath;
        }

        public void Export()
        {
            List<CouponActInfo> couponActInfoList = DataProviderWX.CouponActDAO.GetCouponActInfoList(this.publishmentSystemID);

            string couponActDirectoryPath = PathUtils.Combine(this.directoryPath);

            DirectoryUtils.CreateDirectoryIfNotExists(couponActDirectoryPath);

            foreach (CouponActInfo couponActInfo in couponActInfoList)
            {
                string filePath = PathUtils.Combine(directoryPath, couponActInfo.ID + ".xml");

                AtomFeed feed = ExportCouponActInfo(couponActInfo);

                string keywordName = DataProviderWX.KeywordDAO.GetKeywordInfo(couponActInfo.KeywordID).Keywords;

                AtomUtility.AddDcElement(feed.AdditionalElements, "KeywordName", AtomUtility.Encrypt(keywordName));

                List<CouponInfo> couponInfoInfoList = DataProviderWX.CouponDAO.GetCouponInfoList(this.publishmentSystemID, couponActInfo.ID);
                foreach (CouponInfo couponInfo in couponInfoInfoList)
                {
                    AtomEntry entry = ExportCouponInfo(couponInfo);
                    feed.Entries.Add(entry);
                }

                feed.Save(filePath);
            }
        }

        private AtomFeed ExportCouponActInfo(CouponActInfo couponActInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in CouponActAttribute.AllAttributes)
            {
                object valueObj = couponActInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }

        private AtomEntry ExportCouponInfo(CouponInfo couponInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in CouponAttribute.AllAttributes)
            {
                object valueObj = couponInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(entry.AdditionalElements, attributeName, value);
            }

            List<CouponSNInfo> couponSNInfoList = DataProviderWX.CouponSNDAO.GetCouponSNInfoList(this.publishmentSystemID, couponInfo.ID);

            string couponSNInfoJson = TranslateUtils.ObjectToJson<List<CouponSNInfo>>(couponSNInfoList);

            AtomUtility.AddDcElement(entry.AdditionalElements, "CouponSNJson", couponSNInfoJson);

            return entry;
        }

        public void Import()
        {
            if (!DirectoryUtils.IsDirectoryExists(this.directoryPath)) return;

            string[] filePaths = DirectoryUtils.GetFilePaths(this.directoryPath);

            foreach (string filePath in filePaths)
            {
                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                CouponActInfo couponActInfo = new CouponActInfo();
                foreach (string attributeNames in CouponActAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, attributeNames));
                    couponActInfo.SetValue(attributeNames, value);
                }

                couponActInfo.PublishmentSystemID = this.publishmentSystemID;

                string keywordName = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "KeywordName"));
                couponActInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordsIDbyName(this.publishmentSystemID,keywordName);

                int actID = DataProviderWX.CouponActDAO.Insert(couponActInfo);

                foreach (AtomEntry entry in feed.Entries)
                {
                    CouponInfo couponInfo = new CouponInfo();

                    foreach (string attributeNames in CouponAttribute.AllAttributes)
                    {
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                        couponInfo.SetValue(attributeNames, value);
                    }

                    couponInfo.ActID = actID;
                    couponInfo.PublishmentSystemID = this.publishmentSystemID;

                    int couponID = DataProviderWX.CouponDAO.Insert(couponInfo);

                    string couponSNJson = AtomUtility.GetDcElementContent(entry.AdditionalElements, "CouponSNJson");

                    List<CouponSNInfo> couponSNInfoList = TranslateUtils.JsonToObject<List<CouponSNInfo>>(couponSNJson);

                    foreach (CouponSNInfo couponSNInfo in couponSNInfoList)
                    {
                        couponSNInfo.CouponID = couponID;
                        couponSNInfo.PublishmentSystemID = this.publishmentSystemID;
                        DataProviderWX.CouponSNDAO.Insert(couponSNInfo);
                    }
                }
            }
        }

    }
}
