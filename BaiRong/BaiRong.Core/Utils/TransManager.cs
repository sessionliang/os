using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Specialized;

using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Xml;
using System.Web.Caching;

namespace BaiRong.Core
{
	public class TransManager
	{
        private TransManager()
		{

		}

        private const string CacheKeyFormat = "BaiRong.Core.TransManager.";

        private NameValueCollection nameValueCollection = new NameValueCollection();
        private bool isTrans = false;

        public static TransManager GetTransManager(string language)
        {
            string cacheKey = CacheKeyFormat + language;
            TransManager manager = CacheUtils.Get(cacheKey) as TransManager;
            if (manager == null)
            {
                manager = new TransManager();
                try
                {
                    string filePath = PathUtils.MapPath(string.Format("~/{0}/Themes/Language/{1}.xml", FileConfigManager.Instance.AdminDirectoryName, language));
                    if (FileUtils.IsFileExists(filePath))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(filePath);
                        XmlNodeList dataNodes = doc.SelectNodes("//root/data");
                        foreach (XmlNode dataNode in dataNodes)
                        {
                            string name = dataNode.Attributes["name"].Value;
                            string value = dataNode.InnerText;
                            manager.nameValueCollection.Add(name, value);
                        }

                        if (manager.nameValueCollection.Count > 0)
                        {
                            manager.isTrans = true;
                        }
                    }

                    if (manager.isTrans)
                    {
                        CacheUtils.Insert(cacheKey, manager, new CacheDependency(filePath), CacheUtils.HourFactor);
                    }
                    else
                    {
                        CacheUtils.Insert(cacheKey, manager, CacheUtils.HourFactor);
                    }
                }
                catch { }
            }
            return manager;
        }

        public string GetTransContent(string content)
        {
            if (this.isTrans)
            {
                ArrayList textList = RegexUtils.GetContents("content", "<lan>(?<content>[^><]+?)</lan>", content);
                if (textList.Count > 0)
                {
                    foreach (string text in textList)
                    {
                        if (!string.IsNullOrEmpty(this.nameValueCollection[text]))
                        {
                            content = content.Replace(string.Format("<lan>{0}</lan>", text), this.nameValueCollection[text]);
                        }
                        else
                        {
                            content = content.Replace(string.Format("<lan>{0}</lan>", text), text);
                        }
                    }
                }
                textList = RegexUtils.GetContents("content", "{lan:(?<content>[^{}]+?)}", content);
                if (textList.Count > 0)
                {
                    foreach (string text in textList)
                    {
                        if (!string.IsNullOrEmpty(this.nameValueCollection[text]))
                        {
                            content = content.Replace(string.Format(@"{{lan:{0}}}", text), this.nameValueCollection[text]);
                        }
                        else
                        {
                            content = content.Replace(string.Format(@"{{lan:{0}}}", text), text);
                        }
                    }
                }
            }
            else
            {
                content = content.Replace("<lan>", string.Empty).Replace("</lan>", string.Empty);
                ArrayList textList = RegexUtils.GetContents("content", "{lan:(?<content>[^{}]+?)}", content);
                if (textList.Count > 0)
                {
                    foreach (string text in textList)
                    {
                        content = content.Replace(string.Format(@"{{lan:{0}}}", text), text);
                    }
                }
            }

            return content;
        }

        public static string GetLanString(string str)
        {
            return string.Format("<lan>{0}</lan>", str);
        }
	}
}
