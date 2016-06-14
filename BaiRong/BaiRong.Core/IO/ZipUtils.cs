﻿using System;
using System.IO;

using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

using BaiRong.Core;

namespace BaiRong.Core.IO
{
	public class ZipUtils
	{
		private ZipUtils()
		{
		}

        public static void PackFiles(string zipFilePath, string directoryPath)
        {
            FastZip fz = new FastZip();
            fz.CreateEmptyDirectories = true;
            fz.CreateZip(zipFilePath, directoryPath, true, string.Empty);
        }

        public static void UnpackFiles(string zipFilePath, string directoryPath)
        {
            FastZip fz = new FastZip();
            fz.ExtractZip(zipFilePath, directoryPath, null);
            //fz.ExtractZip(zipFilePath, directoryPath, String.Empty);
        }

        //public static bool UnpackFiles(string zipFilePath, string directoryPath)
        //{
        //    if (!Directory.Exists(directoryPath))
        //        Directory.CreateDirectory(directoryPath);

        //    ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath));

        //    ZipEntry theEntry;
        //    while ((theEntry = s.GetNextEntry()) != null)
        //    {
        //        string directoryName = Path.GetDirectoryName(theEntry.Name);
        //        string fileName = Path.GetFileName(theEntry.Name);

        //        if (directoryName != String.Empty)
        //            Directory.CreateDirectory(directoryPath + directoryName);

        //        if (fileName != String.Empty)
        //        {
        //            FileStream streamWriter = File.Create(Path.Combine(directoryPath, theEntry.Name));

        //            int size = 2048;
        //            byte[] data = new byte[2048];
        //            while (true)
        //            {
        //                size = s.Read(data, 0, data.Length);
        //                if (size > 0)
        //                {
        //                    streamWriter.Write(data, 0, size);
        //                }
        //                else
        //                {
        //                    break;
        //                }
        //            }

        //            streamWriter.Close();
        //        }
        //    }
        //    s.Close();
        //    return true;
        //}


		public static void Zip(string directoryPath, string zipFilePath)
		{
			string[] filenames = Directory.GetFiles(directoryPath);

			Crc32 crc = new Crc32();
			ZipOutputStream s = new ZipOutputStream(File.Create(zipFilePath));

			s.SetLevel(6); // 0 - store only to 9 - means best compression

			foreach (string file in filenames)
			{
				FileStream fs = File.OpenRead(file);

				byte[] buffer = new byte[fs.Length];
				fs.Read(buffer, 0, buffer.Length);
				ZipEntry entry = new ZipEntry(file);

				entry.DateTime = DateTime.Now;

				// set Size and the crc, because the information
				// about the size and crc should be stored in the header
				// if it is not set it is automatically written in the footer.
				// (in this case size == crc == -1 in the header)
				// Some ZIP programs have problems with zip files that don't store
				// the size and crc in the header.
				entry.Size = fs.Length;
				fs.Close();

				crc.Reset();
				crc.Update(buffer);

				entry.Crc = crc.Value;

				s.PutNextEntry(entry);

				s.Write(buffer, 0, buffer.Length);

			}

			s.Finish();
			s.Close();
		}


		public static void UnZip(string directoryPath, string zipFilePath)
		{
			ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath));

			ZipEntry theEntry;
			while ((theEntry = s.GetNextEntry()) != null)
			{
				FileInfo fileInfo = new FileInfo(theEntry.Name);
				string fileName = fileInfo.Name;

				// create directory
				DirectoryUtils.CreateDirectoryIfNotExists(directoryPath);

				if (fileName != String.Empty)
				{
					string filePath = directoryPath + Path.DirectorySeparatorChar + fileName;
					FileStream streamWriter = File.Create(filePath);

					int size = 2048;
					byte[] data = new byte[2048];
					while (true)
					{
						size = s.Read(data, 0, data.Length);
						if (size > 0)
						{
							streamWriter.Write(data, 0, size);
						}
						else
						{
							break;
						}
					}

					streamWriter.Close();
				}
			}
			s.Close();
		}



//		public static void UnZip(string zipFilePath)
//		{
//			ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath));
//
//			ZipEntry theEntry;
//			while ((theEntry = s.GetNextEntry()) != null)
//			{
//
//				Console.WriteLine(theEntry.Name);
//
//				string directoryName = Path.GetDirectoryName(theEntry.Name);
//				string fileName = Path.GetFileName(theEntry.Name);
//
//				// create directory
//				Directory.CreateDirectory(directoryName);
//
//				if (fileName != String.Empty)
//				{
//					FileStream streamWriter = File.Create(theEntry.Name);
//
//					int size = 2048;
//					byte[] data = new byte[2048];
//					while (true)
//					{
//						size = s.Read(data, 0, data.Length);
//						if (size > 0)
//						{
//							streamWriter.Write(data, 0, size);
//						}
//						else
//						{
//							break;
//						}
//					}
//
//					streamWriter.Close();
//				}
//			}
//			s.Close();
//		}
	}
}
