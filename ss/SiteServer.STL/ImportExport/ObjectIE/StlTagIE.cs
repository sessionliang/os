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
   public class StlTagIE
    {
        private readonly int publishmentSystemID;
		private readonly string filePath;

        public StlTagIE(int publishmentSystemID, string filePath)
		{
			this.publishmentSystemID = publishmentSystemID;
			this.filePath = filePath;
		}

        public void ExportStlTag()
		{
			AtomFeed feed = AtomUtility.GetEmptyFeed();

            ArrayList stlTagArrayList = DataProvider.StlTagDAO.GetStlTagNameArrayList(this.publishmentSystemID);

            foreach (string stlTagName in stlTagArrayList)
            {
                StlTagInfo stlTagInfo = DataProvider.StlTagDAO.GetStlTagInfo(this.publishmentSystemID, stlTagName);
                AtomEntry entry = ExportStlTagInfo(stlTagInfo);
                feed.Entries.Add(entry);
            }

			feed.Save(filePath);
		}

        private static AtomEntry ExportStlTagInfo(StlTagInfo stlTagInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            AtomUtility.AddDcElement(entry.AdditionalElements, "TagName", stlTagInfo.TagName);
            AtomUtility.AddDcElement(entry.AdditionalElements, "PublishmentSystemID", stlTagInfo.PublishmentSystemID.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "TagDescription", stlTagInfo.TagDescription);
            AtomUtility.AddDcElement(entry.AdditionalElements, "TagContent", stlTagInfo.TagContent);
            
            return entry;
        }

        public void ImportStlTag(bool overwrite)
        {
            if (!FileUtils.IsFileExists(this.filePath)) return;
            AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

            foreach (AtomEntry entry in feed.Entries)
            {
                string TagName = ConvertHelper.GetString(AtomUtility.GetDcElementContent(entry.AdditionalElements, "TagName"));

                if (!string.IsNullOrEmpty(TagName))
                {
                    StlTagInfo stlTagInfo = new StlTagInfo();
                    stlTagInfo.TagName = TagName;
                    stlTagInfo.PublishmentSystemID = this.publishmentSystemID;
                    stlTagInfo.TagDescription = AtomUtility.GetDcElementContent(entry.AdditionalElements, "TagDescription");
                    stlTagInfo.TagContent = AtomUtility.GetDcElementContent(entry.AdditionalElements, "TagContent");


                    StlTagInfo stlTag = DataProvider.StlTagDAO.GetStlTagInfo(this.publishmentSystemID, stlTagInfo.TagName);
                    if (stlTag != null)
                    {
                        if (overwrite)
                        {
                            DataProvider.StlTagDAO.Update(stlTag);
                        }
                    }
                    else
                    {
                        DataProvider.StlTagDAO.Insert(stlTagInfo);
                    }
                }
            }
        }
    }
}
