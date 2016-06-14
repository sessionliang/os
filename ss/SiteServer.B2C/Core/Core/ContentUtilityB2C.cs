using BaiRong.Model;
using SiteServer.B2C.Model;
using System.Text;
using BaiRong.Core;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core.Data.Provider;
using SiteServer.B2C.BackgroundPages;
using System.Collections.Generic;

using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.B2C.Core
{
    public class ContentUtilityB2C
    {
        private ContentUtilityB2C()
        {
        }

        public static void Translate(PublishmentSystemInfo publishmentSystemInfo, string idsCollection, string translateCollection, ETranslateContentType translateType)
        {
            if (!string.IsNullOrEmpty(idsCollection) && !string.IsNullOrEmpty(translateCollection))
            {
                ArrayList translateArrayList = TranslateUtils.StringCollectionToArrayList(translateCollection);
                foreach (string translate in translateArrayList)
                {
                    if (!string.IsNullOrEmpty(translate))
                    {
                        string[] translates = translate.Split('_');
                        if (translates.Length == 2)
                        {
                            int targetPublishmentSystemID = TranslateUtils.ToInt(translates[0]);
                            int targetNodeID = TranslateUtils.ToInt(translates[1]);

                            if (targetPublishmentSystemID > 0 && targetNodeID > 0)
                            {
                                PublishmentSystemInfo targetPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID);

                                string targetTableName = NodeManager.GetTableName(targetPublishmentSystemInfo, targetNodeID);

                                ArrayList idsArrayList = TranslateUtils.StringCollectionToArrayList(idsCollection);
                                foreach (string ids in idsArrayList)
                                {
                                    string[] nodeIDWithContentID = ids.Split('_');
                                    if (nodeIDWithContentID.Length == 2)
                                    {
                                        int nodeID = TranslateUtils.ToInt(nodeIDWithContentID[0]);
                                        int contentID = TranslateUtils.ToInt(nodeIDWithContentID[1]);

                                        NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, nodeID);
                                        ETableStyle tableStyle = NodeManager.GetTableStyle(publishmentSystemInfo, nodeInfo);
                                        string tableName = NodeManager.GetTableName(publishmentSystemInfo, nodeInfo);

                                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                                        if (contentInfo != null)
                                        {
                                            if (translateType == ETranslateContentType.Copy)
                                            {
                                                FileUtility.MoveFileByContentInfo(publishmentSystemInfo, targetPublishmentSystemInfo, contentInfo);

                                                contentInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                contentInfo.SourceID = contentInfo.NodeID;
                                                contentInfo.NodeID = targetNodeID;

                                                Dictionary<int, int> comboIDDictionary = new Dictionary<int, int>();
                                                int theContentID = DataProvider.ContentDAO.Insert(targetTableName, targetPublishmentSystemInfo, contentInfo);
                                                if (EContentModelTypeUtils.Equals(EContentModelType.Goods, nodeInfo.ContentModelID))
                                                {
                                                    if (publishmentSystemInfo.PublishmentSystemID == targetPublishmentSystemID)
                                                    {
                                                        List<SpecInfo> specInfoList = DataProviderB2C.SpecDAO.GetSpecInfoList(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID);
                                                        foreach (SpecInfo specInfo in specInfoList)
                                                        {
                                                            specInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                            specInfo.ChannelID = targetNodeID;
                                                            int theSpecID = 0;
                                                            int resourceSpecID = specInfo.SpecID;
                                                            if (!DataProviderB2C.SpecDAO.IsExists(specInfo.PublishmentSystemID, specInfo.ChannelID, specInfo.SpecName))
                                                                theSpecID = DataProviderB2C.SpecDAO.Insert(specInfo);
                                                            else
                                                            {
                                                                SpecInfo targetSpecInfo = DataProviderB2C.SpecDAO.GetSpecInfo(specInfo.PublishmentSystemID, specInfo.ChannelID, specInfo.SpecName);
                                                                theSpecID = specInfo.SpecID = targetSpecInfo.SpecID;
                                                                DataProviderB2C.SpecDAO.Update(specInfo);
                                                            }

                                                            List<SpecComboInfo> specComboInfoList = DataProviderB2C.SpecComboDAO.GetSpecComboInfoList(publishmentSystemInfo.PublishmentSystemID, contentID, resourceSpecID);

                                                            foreach (SpecComboInfo specComboInfo in specComboInfoList)
                                                            {
                                                                specComboInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                                specComboInfo.ContentID = theContentID;

                                                                int comboID = DataProviderB2C.SpecComboDAO.Insert(specComboInfo);

                                                                comboIDDictionary[specComboInfo.ComboID] = comboID;
                                                            }
                                                        }

                                                        List<GoodsInfo> goodsInfoList = DataProviderB2C.GoodsDAO.GetGoodsInfoList(publishmentSystemInfo.PublishmentSystemID, contentID);
                                                        foreach (GoodsInfo goodsInfo in goodsInfoList)
                                                        {
                                                            goodsInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                            goodsInfo.ContentID = theContentID;
                                                            List<int> comboIDList = TranslateUtils.StringCollectionToIntList(goodsInfo.ComboIDCollection);
                                                            List<int> newComboIDList = new List<int>();
                                                            foreach (int comboID in comboIDList)
                                                            {
                                                                if (comboIDDictionary.ContainsKey(comboID))
                                                                    newComboIDList.Add(comboIDDictionary[comboID]);
                                                            }
                                                            goodsInfo.ComboIDCollection = TranslateUtils.ObjectCollectionToString(newComboIDList);

                                                            DataProviderB2C.GoodsDAO.Insert(goodsInfo);
                                                        }
                                                    }

                                                    List<PhotoInfo> photoInfoList = DataProvider.PhotoDAO.GetPhotoInfoList(publishmentSystemInfo.PublishmentSystemID, contentID);
                                                    if (photoInfoList.Count > 0)
                                                    {
                                                        foreach (PhotoInfo photoInfo in photoInfoList)
                                                        {
                                                            photoInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                            photoInfo.ContentID = theContentID;

                                                            FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.SmallUrl);
                                                            FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.MiddleUrl);
                                                            FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.LargeUrl);
                                                            DataProvider.PhotoDAO.Insert(photoInfo);
                                                        }
                                                    }
                                                }

                                                if (contentInfo.IsChecked)
                                                {
                                                    string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateContent(targetPublishmentSystemID, contentInfo.NodeID, theContentID);
                                                    AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                                                }
                                            }
                                            else if (translateType == ETranslateContentType.Cut)
                                            {
                                                FileUtility.MoveFileByContentInfo(publishmentSystemInfo, targetPublishmentSystemInfo, contentInfo);
                                                contentInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                contentInfo.SourceID = contentInfo.NodeID;
                                                contentInfo.NodeID = targetNodeID;

                                                if (StringUtils.EqualsIgnoreCase(tableName, targetTableName))
                                                {
                                                    contentInfo.Taxis = DataProvider.ContentDAO.GetTaxisToInsert(targetTableName, targetNodeID, contentInfo.IsTop);
                                                    DataProvider.ContentDAO.Update(targetTableName, targetPublishmentSystemInfo, contentInfo);
                                                }
                                                else
                                                {
                                                    DataProvider.ContentDAO.Insert(targetTableName, targetPublishmentSystemInfo, contentInfo);
                                                    DataProvider.ContentDAO.DeleteContents(publishmentSystemInfo.PublishmentSystemID, tableName, TranslateUtils.ToArrayList(contentID), nodeID);
                                                }

                                                DataProvider.NodeDAO.UpdateContentNum(publishmentSystemInfo, nodeID, true);
                                                DataProvider.NodeDAO.UpdateContentNum(targetPublishmentSystemInfo, targetNodeID, true);

                                                if (EContentModelTypeUtils.Equals(EContentModelType.Goods, nodeInfo.ContentModelID))
                                                {
                                                    List<PhotoInfo> photoInfoList = DataProvider.PhotoDAO.GetPhotoInfoList(publishmentSystemInfo.PublishmentSystemID, contentID);
                                                    if (photoInfoList.Count > 0)
                                                    {
                                                        foreach (PhotoInfo photoInfo in photoInfoList)
                                                        {
                                                            photoInfo.PublishmentSystemID = targetPublishmentSystemID;

                                                            FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.SmallUrl);
                                                            FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.MiddleUrl);
                                                            FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.LargeUrl);

                                                            DataProvider.PhotoDAO.Update(photoInfo);
                                                        }
                                                    }
                                                }
                                                if (contentInfo.IsChecked)
                                                {
                                                    string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateContent(targetPublishmentSystemID, contentInfo.NodeID, contentInfo.ID);
                                                    AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                                                }
                                            }
                                            else if (translateType == ETranslateContentType.Reference)
                                            {
                                                if (contentInfo.ReferenceID == 0)
                                                {
                                                    contentInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                    contentInfo.SourceID = contentInfo.NodeID;
                                                    contentInfo.NodeID = targetNodeID;
                                                    contentInfo.ReferenceID = contentID;
                                                    DataProvider.ContentDAO.Insert(targetTableName, targetPublishmentSystemInfo, contentInfo);
                                                }
                                            }
                                            else if (translateType == ETranslateContentType.ReferenceContent)
                                            {
                                                if (contentInfo.ReferenceID == 0)
                                                {
                                                    contentInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                    contentInfo.SourceID = contentInfo.NodeID;
                                                    contentInfo.NodeID = targetNodeID;
                                                    contentInfo.ReferenceID = contentID;
                                                    contentInfo.Attributes.Add(ContentAttribute.TranslateContentType, ETranslateContentType.ReferenceContent.ToString());

                                                    Dictionary<int, int> comboIDDictionary = new Dictionary<int, int>();
                                                    int theContentID = DataProvider.ContentDAO.Insert(targetTableName, targetPublishmentSystemInfo, contentInfo);

                                                    #region 复制资源
                                                    PublishmentSystemInfo targetPulishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(targetPublishmentSystemID);
                                                    ETableStyle targetTableStyle = NodeManager.GetTableStyle(targetPulishmentSystemInfo, targetNodeID);
                                                    ContentInfo targetContentInfo = DataProvider.ContentDAO.GetContentInfo(targetTableStyle, targetTableName, theContentID);
                                                    //资源：图片，文件，视频
                                                    if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(GoodsContentAttribute.ImageUrl)))
                                                    {
                                                        //修改图片
                                                        string sourceImageUrl = PathUtility.MapPath(publishmentSystemInfo, contentInfo.GetExtendedAttribute(GoodsContentAttribute.ImageUrl));
                                                        CopyReferenceFiles(targetPublishmentSystemInfo, sourceImageUrl, publishmentSystemInfo);

                                                    }
                                                    else if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(GoodsContentAttribute.ImageUrl))))
                                                    {
                                                        ArrayList sourceImageUrls = TranslateUtils.StringCollectionToArrayList(contentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(GoodsContentAttribute.ImageUrl)));

                                                        foreach (string imageUrl in sourceImageUrls)
                                                        {
                                                            string sourceImageUrl = PathUtility.MapPath(publishmentSystemInfo, imageUrl);
                                                            CopyReferenceFiles(targetPublishmentSystemInfo, sourceImageUrl, publishmentSystemInfo);
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(GoodsContentAttribute.FileUrl)))
                                                    {
                                                        //修改附件
                                                        string sourceFileUrl = PathUtility.MapPath(publishmentSystemInfo, contentInfo.GetExtendedAttribute(GoodsContentAttribute.FileUrl));
                                                        CopyReferenceFiles(targetPublishmentSystemInfo, sourceFileUrl, publishmentSystemInfo);

                                                    }
                                                    else if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(GoodsContentAttribute.FileUrl))))
                                                    {
                                                        ArrayList sourceFileUrls = TranslateUtils.StringCollectionToArrayList(contentInfo.GetExtendedAttribute(ContentAttribute.GetExtendAttributeName(GoodsContentAttribute.FileUrl)));

                                                        foreach (string FileUrl in sourceFileUrls)
                                                        {
                                                            string sourceFileUrl = PathUtility.MapPath(publishmentSystemInfo, FileUrl);
                                                            CopyReferenceFiles(targetPublishmentSystemInfo, sourceFileUrl, publishmentSystemInfo);
                                                        }
                                                    }
                                                    if (!string.IsNullOrEmpty(contentInfo.GetExtendedAttribute(GoodsContentAttribute.ThumbUrl)))
                                                    {
                                                        //修改附件
                                                        string sourceThumbUrl = PathUtility.MapPath(publishmentSystemInfo, contentInfo.GetExtendedAttribute(GoodsContentAttribute.ThumbUrl));
                                                        CopyReferenceFiles(targetPublishmentSystemInfo, sourceThumbUrl, publishmentSystemInfo);

                                                    }
                                                    #endregion


                                                    if (EContentModelTypeUtils.Equals(EContentModelType.Goods, nodeInfo.ContentModelID))
                                                    {
                                                        //if (publishmentSystemInfo.PublishmentSystemID == targetPublishmentSystemID)
                                                        //{
                                                        List<SpecInfo> specInfoList = DataProviderB2C.SpecDAO.GetSpecInfoList(publishmentSystemInfo.PublishmentSystemID, nodeInfo.NodeID);
                                                        foreach (SpecInfo specInfo in specInfoList)
                                                        {
                                                            specInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                            specInfo.ChannelID = targetNodeID;
                                                            int theSpecID = 0;
                                                            int resourceSpecID = specInfo.SpecID;
                                                            if (!DataProviderB2C.SpecDAO.IsExists(specInfo.PublishmentSystemID, specInfo.ChannelID, specInfo.SpecName))
                                                                theSpecID = DataProviderB2C.SpecDAO.Insert(specInfo);
                                                            else
                                                            {
                                                                SpecInfo targetSpecInfo = DataProviderB2C.SpecDAO.GetSpecInfo(specInfo.PublishmentSystemID, specInfo.ChannelID, specInfo.SpecName);
                                                                theSpecID = specInfo.SpecID = targetSpecInfo.SpecID;
                                                                DataProviderB2C.SpecDAO.Update(specInfo);
                                                            }

                                                            List<SpecComboInfo> specComboInfoList = DataProviderB2C.SpecComboDAO.GetSpecComboInfoList(publishmentSystemInfo.PublishmentSystemID, contentID, resourceSpecID);

                                                            foreach (SpecComboInfo specComboInfo in specComboInfoList)
                                                            {
                                                                specComboInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                                specComboInfo.ContentID = theContentID;
                                                                specComboInfo.SpecID = theSpecID;

                                                                int comboID = DataProviderB2C.SpecComboDAO.Insert(specComboInfo);
                                                                comboIDDictionary[specComboInfo.ComboID] = comboID;
                                                            }
                                                        }

                                                        List<GoodsInfo> goodsInfoList = DataProviderB2C.GoodsDAO.GetGoodsInfoList(publishmentSystemInfo.PublishmentSystemID, contentID);
                                                        List<int> specIDList = new List<int>();
                                                        List<int> specItemIDList = new List<int>();
                                                        List<int> theUsedSpecIDList = new List<int>();
                                                        List<int> theUsedSpecItemIDList = new List<int>();
                                                        foreach (GoodsInfo goodsInfo in goodsInfoList)
                                                        {
                                                            goodsInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                            goodsInfo.ContentID = theContentID;
                                                            List<int> comboIDList = TranslateUtils.StringCollectionToIntList(goodsInfo.ComboIDCollection);
                                                            List<int> newComboIDList = new List<int>();
                                                            foreach (int comboID in comboIDList)
                                                            {
                                                                if (comboIDDictionary.ContainsKey(comboID))
                                                                    newComboIDList.Add(comboIDDictionary[comboID]);
                                                            }
                                                            goodsInfo.ComboIDCollection = TranslateUtils.ObjectCollectionToString(newComboIDList);

                                                            DataProviderB2C.GoodsDAO.Insert(goodsInfo);

                                                            //修改商品内容的Goods
                                                            DataProviderB2C.SpecComboDAO.GetSpec(goodsInfo.ComboIDCollection, out specIDList, out specItemIDList);
                                                            if (goodsInfo.IsOnSale)
                                                            {
                                                                foreach (int specID in specIDList)
                                                                {
                                                                    if (!theUsedSpecIDList.Contains(specID))
                                                                    {
                                                                        theUsedSpecIDList.Add(specID);
                                                                    }
                                                                }
                                                                foreach (int specItemID in specItemIDList)
                                                                {
                                                                    if (!theUsedSpecItemIDList.Contains(specItemID))
                                                                    {
                                                                        theUsedSpecItemIDList.Add(specItemID);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        //}

                                                        DataProviderB2C.GoodsContentDAO.UpdateSpec(targetTableName, targetPublishmentSystemID, theContentID, theUsedSpecIDList, theUsedSpecItemIDList);

                                                        List<PhotoInfo> photoInfoList = DataProvider.PhotoDAO.GetPhotoInfoList(publishmentSystemInfo.PublishmentSystemID, contentID);
                                                        if (photoInfoList.Count > 0)
                                                        {
                                                            foreach (PhotoInfo photoInfo in photoInfoList)
                                                            {
                                                                photoInfo.PublishmentSystemID = targetPublishmentSystemID;
                                                                photoInfo.ContentID = theContentID;

                                                                FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.SmallUrl);
                                                                FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.MiddleUrl);
                                                                FileUtility.MoveFileByVirtaulUrl(publishmentSystemInfo, targetPublishmentSystemInfo, photoInfo.LargeUrl);
                                                                DataProvider.PhotoDAO.Insert(photoInfo);
                                                            }
                                                        }
                                                    }

                                                    if (contentInfo.IsChecked)
                                                    {
                                                        string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateContent(targetPublishmentSystemID, contentInfo.NodeID, theContentID);
                                                        AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                StringUtility.AddLog(publishmentSystemInfo.PublishmentSystemID, "转移内容");
            }
        }

        private static void CopyReferenceFiles(PublishmentSystemInfo targetPublishmentSystemInfo, string sourceUrl, PublishmentSystemInfo sourcePublishmentSystemInfo)
        {
            string targetUrl = StringUtils.ReplaceFirst(sourcePublishmentSystemInfo.PublishmentSystemDir, sourceUrl, targetPublishmentSystemInfo.PublishmentSystemDir);
            if (!FileUtils.IsFileExists(targetUrl))
            {
                FileUtils.CopyFile(sourceUrl, targetUrl, true);
            }
        }
    }
}
