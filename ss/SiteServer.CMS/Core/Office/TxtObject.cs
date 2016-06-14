using System.Collections;
using System.Data.OleDb;
using System.Text;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using System;
using BaiRong.Core.Data.Provider;
using System.Data;
using System.Collections.Specialized;

namespace SiteServer.CMS.Core.Office
{
	public class TxtObject
	{
        public static ArrayList GetContentsByTxtFile(string directoryPath, PublishmentSystemInfo publishmentSystemInfo, NodeInfo nodeInfo)
        {
            ArrayList contentInfoArrayList = new ArrayList();

            string[] filePaths = DirectoryUtils.GetFilePaths(directoryPath);
            foreach (string filePath in filePaths)
            {
                if (EFileSystemTypeUtils.Equals(EFileSystemType.Txt, PathUtils.GetExtension(filePath)))
                {
                    try
                    {
                        string content = FileUtils.ReadText(filePath, ECharset.gb2312);
                        if (!string.IsNullOrEmpty(content))
                        {
                            content = content.Trim();
                            string title = StringUtils.GetFirstOfStringCollection(content, '\r');
                            if (!string.IsNullOrEmpty(title))
                            {
                                BackgroundContentInfo contentInfo = new BackgroundContentInfo();
                                contentInfo.Title = title.Trim();
                                contentInfo.PublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                                contentInfo.NodeID = nodeInfo.NodeID;
                                contentInfo.LastEditDate = DateTime.Now;
                                contentInfo.Content = StringUtils.ReplaceNewlineToBR(content.Replace(title, string.Empty).Trim());

                                contentInfoArrayList.Add(contentInfo);
                            }
                        }
                    }
                    catch { }
                }
            }

            return contentInfoArrayList;
        }
	}
}
