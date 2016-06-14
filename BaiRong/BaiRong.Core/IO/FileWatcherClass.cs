using System;
using System.Collections;
using System.Text;
using System.IO;

using BaiRong.Model;
using BaiRong.Core;

namespace BaiRong.Core.IO
{
    public class FileWatcherClass
    {
        public FileWatcherClass(string fullFileName)
        {
            FileDependency(fullFileName);
        }

        private FileSystemWatcher m_fileSystemWatcher;
        public delegate void FileChange(object sender, EventArgs e);

        //The OnFileChange event is fired when the file changes.
        public event FileChange OnFileChange;

        public void FileDependency(string fullFileName)
        {
            //Validate file.
            FileInfo fileInfo = new FileInfo(fullFileName);
            if (!fileInfo.Exists)
            {
                FileUtils.WriteText(fullFileName, ECharset.utf_8, string.Empty);
            }

            //Get path from full file name.
            string path = Path.GetDirectoryName(fullFileName);

            //Get file name from full file name.
            string fileName = Path.GetFileName(fullFileName);

            //Initialize new FileSystemWatcher.
            m_fileSystemWatcher = new FileSystemWatcher();
            m_fileSystemWatcher.Path = path;
            m_fileSystemWatcher.Filter = fileName;
            m_fileSystemWatcher.EnableRaisingEvents = true;
            this.m_fileSystemWatcher.Changed += new FileSystemEventHandler(this.fileSystemWatcher_Changed);
        }

        private void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            OnFileChange(sender, e);
        }



    }
}
