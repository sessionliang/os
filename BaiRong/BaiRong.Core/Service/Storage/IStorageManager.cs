using System;
using System.Collections;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Core.IO.FileManagement;
using BaiRong.Core.Net;
using System.IO;
using BaiRong.Model;

namespace BaiRong.Core.Service
{
    public interface IStorageManager
	{
        bool IsEnabled { get; }

        void ChangeDirDownByRelatedPath(string relatedPath);

        void ChangeDirUpByRelatedPath(string relatedPath);

        void ChangeDir(string directoryName);

        void ChangeDirToParent();

        void CreateDirectory(string directoryName);

        void UploadDirectory(string directoryPath, string directoryName);

        void UploadDirectory(string currentDirectoryPath);

        void UploadFiles(string[] fileNames, string currentDirectoryPath);

        bool UploadFile(string filePath);

        void DownloadFiles(string[] fileNames, string currentDirectoryPath);

        void DownloadDirectory(string directoryPath, string directoryName);

        void RemoveDirs(string dir);

        void RemoveFile(string filename);

        string GetWorkingDirectory();

        ArrayList ListDirectories();

        ArrayList ListFiles();

        bool CopyFrom(string sourcePath);

        bool WriteText(string fileName, ECharset charset, string content);
	}
}
