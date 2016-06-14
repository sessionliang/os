using System;
using System.IO;
using System.Collections ;
using System.Threading ;

namespace BaiRong.Core.IO.FileManagement
{
	public class FileSystemInfoExtendCollection : ICollection
	{
		private FileSystemInfoExtend[] _files ;
		
		public FileSystemInfoExtendCollection(FileSystemInfo[] files)
		{
			lock(SyncRoot)
			{
				_files = new FileSystemInfoExtend[files.Length] ;
				for (int i = 0 ; i < files.Length ; i++)
					_files[i] = new FileSystemInfoExtend(files[i]) ;
			}
		}

		private FileSystemInfoExtendCollection(FileSystemInfoExtend[] files)
		{
			lock(SyncRoot)
			{
				_files = new FileSystemInfoExtend[files.Length] ;
				files.CopyTo(this._files, 0) ;
			}
		}

		public FileSystemInfoExtendCollection Folders
		{
			get 
			{
				ArrayList folderAL = new ArrayList() ;
				foreach(FileSystemInfoExtend fileItem in this._files)
				{
					if (fileItem.IsDirectory)
						folderAL.Add(fileItem) ;
				}

				FileSystemInfoExtend[] folderArray = (FileSystemInfoExtend[])folderAL.ToArray(typeof(FileSystemInfoExtend)) ;

				FileSystemInfoExtendCollection folders =  new FileSystemInfoExtendCollection(folderArray) ;

				return folders ;
			}
		}

		public FileSystemInfoExtendCollection Files
		{
			get 
			{
				ArrayList fileAL = new ArrayList();
				foreach(FileSystemInfoExtend fileItem in this._files)
				{
					if (!fileItem.IsDirectory)
						fileAL.Add(fileItem);
				}

				FileSystemInfoExtend[] fileArray = (FileSystemInfoExtend[])fileAL.ToArray(typeof(FileSystemInfoExtend));

				FileSystemInfoExtendCollection files = new FileSystemInfoExtendCollection(fileArray);

				return files;
			}
		}


		#region IEnumerable Members
		public IEnumerator GetEnumerator()
		{
			lock(SyncRoot)
			{
				return _files.GetEnumerator() ;
			}
		}
		#endregion

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return true ;
			}
		}

		public int Count
		{
			get
			{
				lock (SyncRoot)
				{
					return _files.Length ;
				}
			}
		}

		public void CopyTo(Array array, int index)
		{
			lock (SyncRoot)
			{
				_files.CopyTo(array, index) ;
			}
		}

		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		#endregion
	}
}