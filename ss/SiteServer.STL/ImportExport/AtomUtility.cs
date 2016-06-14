using System;
using System.Collections.Specialized;
using Atom.AdditionalElements;
using Atom.AdditionalElements.DublinCore;
using Atom.Core;
using BaiRong.Core;
using BaiRong.Core.Cryptography;

namespace SiteServer.STL.ImportExport
{
	/// <summary>
	/// Atom 0.3
	/// </summary>
	internal class AtomUtility
	{
		public const string PREFIX = "SiteServer_";

		public static void AddDcElement(ScopedElementCollection collection, string name, string content)
		{
			if (content != null && content.Length > 0)
			{
				collection.Add(new DcElement(PREFIX + name, StringUtils.ToXmlContent(content)));
			}
		}

		public static string GetDcElementContent(ScopedElementCollection additionalElements, string name)
		{
			return GetDcElementContent(additionalElements, name, "");
		}

		public static string GetDcElementContent(ScopedElementCollection additionalElements, string name, string defaultContent)
		{
			string content = defaultContent;
			string localName = AtomUtility.PREFIX + name;
			ScopedElement element = additionalElements.FindScopedElementByLocalName(localName);
			if (element != null)
			{
				content = element.Content;
			}
			return content;
		}

        public static NameValueCollection GetDcElementNameValueCollection(ScopedElementCollection additionalElements)
        {
            return additionalElements.GetNameValueCollection(AtomUtility.PREFIX);
        }

		public static AtomFeed GetEmptyFeed()
		{
			AtomFeed feed = new AtomFeed();
			feed.Title = new AtomContentConstruct("title", "siteserver channel");
			feed.Author = new AtomPersonConstruct("author",
				"siteserver", new Uri("http://www.siteserver.cn"));
			feed.Modified = new AtomDateConstruct("modified", DateTime.Now,
				TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now));

			return feed;
		}

		public static AtomEntry GetEmptyEntry()
		{
			AtomEntry entry = new AtomEntry();


			entry.Id = new Uri("http://www.siteserver.cn/");
			entry.Title = new AtomContentConstruct("title", "title");
			entry.Modified = new AtomDateConstruct("modified", DateTime.Now,
				TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now));
			entry.Issued = new AtomDateConstruct("issued", DateTime.Now,
				TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now));

			return entry;
		}

		public static string Encrypt(string inputString)
		{
			if (inputString == null || inputString.Length == 0)
				return "";
			DESEncryptor encryptor = new DESEncryptor();
			encryptor.InputString = inputString;
			encryptor.EncryptKey = "TgQQk42O";
			encryptor.DesEncrypt();
			return encryptor.OutString;
		}


		public static string Decrypt(string inputString)
		{
			if (inputString == null || inputString.Length == 0)
				return "";
			DESEncryptor encryptor = new DESEncryptor();
			encryptor.InputString = inputString;
			encryptor.DecryptKey = "TgQQk42O";
			encryptor.DesDecrypt();
			return encryptor.OutString;
		}

	}
}
