using System;
using System.Collections.Generic;
using System.Text;
using Atom.Core;
using System.Collections;
using SiteServer.CMS.Model;
using BaiRong.Model;
using BaiRong.Core;
using SiteServer.CMS.Core;

namespace SiteServer.STL.ImportExport
{
   public class SeoIE
    {
        private readonly int publishmentSystemID;
		private readonly string filePath;

        public SeoIE(int publishmentSystemID, string filePath)
		{
			this.publishmentSystemID = publishmentSystemID;
			this.filePath = filePath;
		}

		public void ExportSeo()
		{
			AtomFeed feed = AtomUtility.GetEmptyFeed();

			ArrayList seoNameArrayList = DataProvider.SeoMetaDAO.GetSeoMetaNameArrayList(this.publishmentSystemID);

            foreach (string seoName in seoNameArrayList)
            {
                SeoMetaInfo seoInfo = DataProvider.SeoMetaDAO.GetSeoMetaInfoBySeoMetaName(this.publishmentSystemID,seoName );
                AtomEntry entry = ExportSeoInfo(seoInfo);
                feed.Entries.Add(entry);
            }

			feed.Save(filePath);
		}
        
        private static AtomEntry ExportSeoInfo(SeoMetaInfo seoInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            AtomUtility.AddDcElement(entry.AdditionalElements, "SeoMetaID", seoInfo.SeoMetaID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "PublishmentSystemID", seoInfo.PublishmentSystemID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "SeoMetaName", seoInfo.SeoMetaName);
            AtomUtility.AddDcElement(entry.AdditionalElements, "IsDefault", seoInfo.IsDefault.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "PageTitle", seoInfo.PageTitle);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Keywords", seoInfo.Keywords);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Description", seoInfo.Description);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Copyright", seoInfo.Copyright);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Author", seoInfo.Author);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Email", seoInfo.Email);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Language", seoInfo.Language);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Charset", seoInfo.Charset);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Distribution", seoInfo.Distribution);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Rating", seoInfo.Rating);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Robots", seoInfo.Robots);
            AtomUtility.AddDcElement(entry.AdditionalElements, "RevisitAfter", seoInfo.RevisitAfter);
            AtomUtility.AddDcElement(entry.AdditionalElements, "Expires", seoInfo.Expires);

            return entry;
        }

        public void ImportSeo(bool overwrite)
        {
            if (!FileUtils.IsFileExists(this.filePath)) return;
            AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

            foreach (AtomEntry entry in feed.Entries)
            {
                int SeoMetaID =ConvertHelper.GetInteger( AtomUtility.GetDcElementContent(entry.AdditionalElements, "SeoMetaID"));

                if (!string.IsNullOrEmpty(SeoMetaID.ToString()))
                {
                    SeoMetaInfo seoInfo = new SeoMetaInfo();
                    seoInfo.SeoMetaID = SeoMetaID;
                    seoInfo.PublishmentSystemID = this.publishmentSystemID;
                    seoInfo.SeoMetaName = AtomUtility.GetDcElementContent(entry.AdditionalElements, "SeoMetaName");
                    seoInfo.IsDefault = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "IsDefault"));
                    seoInfo.PageTitle = AtomUtility.GetDcElementContent(entry.AdditionalElements, "PageTitle");
                    seoInfo.Keywords = AtomUtility.GetDcElementContent(entry.AdditionalElements, "Keywords");
                    seoInfo.Description = AtomUtility.GetDcElementContent(entry.AdditionalElements, "Description");
                    seoInfo.Copyright = AtomUtility.GetDcElementContent(entry.AdditionalElements, "Copyright");
                    seoInfo.Author = AtomUtility.GetDcElementContent(entry.AdditionalElements, "Author");
                    seoInfo.Email = AtomUtility.GetDcElementContent(entry.AdditionalElements, "Email");
                    seoInfo.Language =AtomUtility.GetDcElementContent(entry.AdditionalElements, "Language");
                    seoInfo.Charset = AtomUtility.GetDcElementContent(entry.AdditionalElements, "Charset");
                    seoInfo.Distribution = AtomUtility.GetDcElementContent(entry.AdditionalElements, "Distribution");
                    seoInfo.Rating = AtomUtility.GetDcElementContent(entry.AdditionalElements, "Rating");
                    seoInfo.Robots =AtomUtility.GetDcElementContent(entry.AdditionalElements, "Robots");
                    seoInfo.RevisitAfter = AtomUtility.GetDcElementContent(entry.AdditionalElements, "RevisitAfter");
                    seoInfo.Expires = AtomUtility.GetDcElementContent(entry.AdditionalElements, "Expires");

                    SeoMetaInfo seoMetaInfo = DataProvider.SeoMetaDAO.GetSeoMetaInfoBySeoMetaName(this.publishmentSystemID, seoInfo.SeoMetaName);
                    if (seoMetaInfo != null)
                    {
                        if (overwrite)
                        {
                            DataProvider.SeoMetaDAO.Update(seoInfo);
                        }
                    }
                    else
                    {
                        DataProvider.SeoMetaDAO.Insert(seoInfo);
                    }
                }
            }
        }
    }
}
