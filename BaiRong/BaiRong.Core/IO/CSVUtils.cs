using System.Collections;
using System.Data.OleDb;
using System.Text;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Core.Data;
using BaiRong.Core.IO;
using BaiRong.Model;
using System;
using BaiRong.Core.Data.Provider;
using System.Data;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace BaiRong.Core.IO
{
	public class CSVUtils
	{
        public static void ExportCSVFile(string filePath, List<string> nameList, List<List<string>> valueListOfList)
        {
            DirectoryUtils.CreateDirectoryIfNotExists(DirectoryUtils.GetDirectoryPath(filePath));
            FileUtils.DeleteFileIfExists(filePath);

            StringBuilder builder = new StringBuilder();
            foreach (string name in nameList)
            {
                builder.Append(name).Append(",");
            }
            builder.Length -= 1;
            builder.Append("\n");

            foreach (List<string> valueList in valueListOfList)
            {
                foreach (string value in valueList)
                {
                    builder.Append(value).Append(",");
                }
                builder.Length -= 1;
                builder.Append("\n");
            }

            FileUtils.WriteText(filePath, BaiRong.Model.ECharset.gb2312, builder.ToString());
        }

        public static List<string> ReadCSVFile(string filePath)
        {
            List<string> valueList = new List<string>();

            //string content = FileUtils.ReadText(filePath, ECharset.utf_8);
            string content = FileUtils.ReadText(filePath, ECharset.gb2312);
            if (!string.IsNullOrEmpty(content))
            {
                valueList = TranslateUtils.StringCollectionToStringList(content, '\n');
            }

            return valueList;
        }
	}
}
