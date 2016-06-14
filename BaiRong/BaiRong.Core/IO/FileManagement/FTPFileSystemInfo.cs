using System;
using System.Collections;
using System.Text;
using System.IO;
using BaiRong.Core;

namespace BaiRong.Core.IO.FileManagement
{
    public class FTPFileSystemInfo : SimpleFileInfoBase
    {
        private string ftpString;
        ArrayList arraylist = new ArrayList();

        public FTPFileSystemInfo(string ftpString)
        {
            this.ftpString = ftpString;
            string[] array = ftpString.Split(' ');
            foreach (string arr in array)
            {
                if (!string.IsNullOrEmpty(arr))
                {
                    this.arraylist.Add(arr);
                }
            }
        }

        override public string Name
        {
            get
            {
                string name = string.Empty;
                if (arraylist.Count >= 9)
                {
                    for (int i = 8; i < arraylist.Count; i++)
                    {
                        name = name + arraylist[i] + " ";
                    }
                }
                return name.Trim();
            }
        }

        override public string FullName
        {
            get { return this.Name; }
        }

        public bool IsDirectory
        {
            get
            {
                return ((ftpString[0] == 'd') || (ftpString.ToUpper().IndexOf("<DIR>") >= 0));
            }
        }

        public string Type
        {
            get { return this.IsDirectory ? "" : PathUtils.GetExtension(this.Name); }
        }

        override public long Size
        {
            get
            {
                if (this.IsDirectory)
                    return 0L;
                else
                {
                    if (this.arraylist.Count >= 5)
                    {
                        return TranslateUtils.ToLong((string)this.arraylist[4]);
                    }
                    return 0L;
                }
            }
        }

        override public DateTime LastWriteTime
        {
            get
            {
                if (this.arraylist.Count >= 8)
                {
                    string str = (string)this.arraylist[5] + " " + (string)this.arraylist[6] + " " + (string)this.arraylist[7];
                    if (((string)this.arraylist[7]).IndexOf(":") != -1)
                    {
                        str = DateTime.Now.Year.ToString() + " " + (string)this.arraylist[5] + " " + (string)this.arraylist[6] + " " + (string)this.arraylist[7];
                    }

                    return TranslateUtils.ToDateTime(str);
                }
                return DateTime.Now;
            }
        }

        public override DateTime CreationTime
        {
            get
            {
                return this.LastWriteTime;
            }
        }

    }
}
