using System;
using System.IO;
using System.Web.Caching;
using BaiRong.Core;

namespace BaiRong.Core.IO.FileManagement
{
	public class FileManager
	{
		public FileManager()
		{
		}

		public static FileSystemInfoExtendCollection GetFileSystemInfoExtendCollection(string rootPath, bool reflesh)
		{
			FileSystemInfoExtendCollection retval = null;
			if (Directory.Exists(rootPath))
			{
				string cacheKey = string.Format("BaiRong.Core.IO.FileManagement.FileManager:{0}",  rootPath) ;
				if (CacheUtils.Get(cacheKey) == null || reflesh) 
				{
					DirectoryInfo currentRoot = new DirectoryInfo(rootPath);
					FileSystemInfo[] files = currentRoot.GetFileSystemInfos();
					FileSystemInfoExtendCollection fsies = new FileSystemInfoExtendCollection(files);

					CacheUtils.Max(cacheKey, fsies, new CacheDependency(rootPath));
				}

				retval = CacheUtils.Get(cacheKey) as FileSystemInfoExtendCollection ;
			}

			return retval;
		}
	}
}
