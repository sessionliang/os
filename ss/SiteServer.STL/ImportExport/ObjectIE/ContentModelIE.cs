using System.Collections;
using Atom.Core;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.STL.ImportExport
{
    internal class ContentModelIE
    {
        private readonly int publishmentSystemID;
        private readonly string filePath;
        private readonly string productID;

        public ContentModelIE(int publishmentSystemID, string filePath, string productID)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.filePath = filePath;
            this.productID = productID;
        }


        public void ExportContentModel(ArrayList conetentModelInfoArrayList)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (ContentModelInfo contentModelInfo in conetentModelInfoArrayList)
            {
                AtomEntry entry = ExportContentModelInfo(contentModelInfo);
                feed.Entries.Add(entry);
            }

            feed.Save(filePath);
        }

        private static AtomEntry ExportContentModelInfo(ContentModelInfo contentModelInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            AtomUtility.AddDcElement(entry.AdditionalElements, "ModelID", contentModelInfo.ModelID);
            AtomUtility.AddDcElement(entry.AdditionalElements, "ProductID", contentModelInfo.ProductID);
            AtomUtility.AddDcElement(entry.AdditionalElements, "SiteID", contentModelInfo.SiteID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "ModelName", contentModelInfo.ModelName);
            AtomUtility.AddDcElement(entry.AdditionalElements, "IsSystem", contentModelInfo.IsSystem.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "TableName", contentModelInfo.TableName);
            AtomUtility.AddDcElement(entry.AdditionalElements, "TableType", contentModelInfo.TableType.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "IconUrl", contentModelInfo.IconUrl);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Description", contentModelInfo.Description);

            return entry;
        }


        public void ImportContentModelInfo(bool overwrite)
        {
            if (!FileUtils.IsFileExists(this.filePath)) return;
            AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

            foreach (AtomEntry entry in feed.Entries)
            {
                string modelID = AtomUtility.GetDcElementContent(entry.AdditionalElements, "ModelID");

                if (!string.IsNullOrEmpty(modelID))
                {
                    ContentModelInfo contentModelInfo = new ContentModelInfo();
                    contentModelInfo.ModelID = modelID;
                    contentModelInfo.ProductID = AtomUtility.GetDcElementContent(entry.AdditionalElements, "ProductID");
                    contentModelInfo.SiteID = this.publishmentSystemID;
                    contentModelInfo.ModelName = AtomUtility.GetDcElementContent(entry.AdditionalElements, "ModelName");
                    contentModelInfo.IsSystem = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "IsSystem"));
                    contentModelInfo.TableName = AtomUtility.GetDcElementContent(entry.AdditionalElements, "TableName");
                    contentModelInfo.TableType = EAuxiliaryTableTypeUtils.GetEnumType(AtomUtility.GetDcElementContent(entry.AdditionalElements, "TableType"));
                    contentModelInfo.IconUrl = AtomUtility.GetDcElementContent(entry.AdditionalElements, "IconUrl");
                    contentModelInfo.Description = AtomUtility.GetDcElementContent(entry.AdditionalElements, "Description");


                    ContentModelInfo srcContentModelInfo = ContentModelManager.GetContentModelInfo(PublishmentSystemManager.GetPublishmentSystemInfo(this.publishmentSystemID), modelID);
                    AuxiliaryTableInfo auxiliaryTableInfo = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableInfo(modelID);
                    if (auxiliaryTableInfo != null && auxiliaryTableInfo.TableENName == modelID)
                    {
                        if (overwrite)
                        {
                            BaiRongDataProvider.ContentModelDAO.Update(contentModelInfo);
                        }
                        else
                        {
                            string importContentModelID = BaiRongDataProvider.ContentModelDAO.GetImportContentModelID(this.publishmentSystemID, contentModelInfo.ModelID, this.productID);
                            contentModelInfo.ModelID = importContentModelID;
                            BaiRongDataProvider.ContentModelDAO.Insert(contentModelInfo);
                        }
                    }
                    else
                    {
                        BaiRongDataProvider.ContentModelDAO.Insert(contentModelInfo);
                    }
                }
            }
        }

    }
}
