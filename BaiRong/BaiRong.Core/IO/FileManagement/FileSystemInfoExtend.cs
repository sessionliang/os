using System;
using System.IO;

namespace BaiRong.Core.IO.FileManagement
{
	/// <summary>
	/// FileSystemInfoExtend 的摘要说明。
	/// </summary>
	public class FileSystemInfoExtend : SimpleFileInfoBase
	{
		private FileSystemInfo _file ;

		public FileSystemInfoExtend(FileSystemInfo file)
		{
			_file = file ;
		}

		override public string Name
		{
			get { return _file.Name ; }
		}

		override public string FullName
		{
			get { return _file.FullName ; }
		}

		public bool IsDirectory
		{
			get
			{
				return (_file.Attributes & FileAttributes.Directory)
					==FileAttributes.Directory ;
			}
		}

		public string Type
		{
			get { return this.IsDirectory ? "" : _file.Extension ; }
		}

		override public long Size
		{
			get
			{
				if ( this.IsDirectory )
					return 0L ;
				else
					return ((FileInfo)_file).Length  ;
			}
		}

		override public DateTime LastWriteTime
		{
			get { return _file.LastWriteTime ; }
		}

		public override DateTime CreationTime
		{
			get
			{
				return _file.CreationTime;
			}
		}

	}
}
