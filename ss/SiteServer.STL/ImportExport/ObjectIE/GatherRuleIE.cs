using System.Collections;
using Atom.Core;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;

namespace SiteServer.STL.ImportExport
{
	internal class GatherRuleIE
	{
		private readonly int publishmentSystemID;
		private readonly string filePath;

		public GatherRuleIE(int publishmentSystemID, string filePath)
		{
			this.publishmentSystemID = publishmentSystemID;
			this.filePath = filePath;
		}


		public void ExportGatherRule(ArrayList gatherRuleInfoArrayList)
		{
			AtomFeed feed = AtomUtility.GetEmptyFeed();

			foreach (GatherRuleInfo gatherRuleInfo in gatherRuleInfoArrayList)
			{
				AtomEntry entry = ExportGatherRuleInfo(gatherRuleInfo);
				feed.Entries.Add(entry);
			}

			feed.Save(filePath);
		}

		private static AtomEntry ExportGatherRuleInfo(GatherRuleInfo gatherRuleInfo)
		{
			AtomEntry entry = AtomUtility.GetEmptyEntry();

			AtomUtility.AddDcElement(entry.AdditionalElements, "GatherRuleName", gatherRuleInfo.GatherRuleName);
			AtomUtility.AddDcElement(entry.AdditionalElements, "PublishmentSystemID", gatherRuleInfo.PublishmentSystemID.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "CookieString", AtomUtility.Encrypt(gatherRuleInfo.CookieString));//加密
            AtomUtility.AddDcElement(entry.AdditionalElements, "GatherUrlIsCollection", gatherRuleInfo.GatherUrlIsCollection.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "GatherUrlCollection", gatherRuleInfo.GatherUrlCollection);
            AtomUtility.AddDcElement(entry.AdditionalElements, "GatherUrlIsSerialize", gatherRuleInfo.GatherUrlIsSerialize.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "GatherUrlSerialize", gatherRuleInfo.GatherUrlSerialize);
			AtomUtility.AddDcElement(entry.AdditionalElements, "SerializeFrom", gatherRuleInfo.SerializeFrom.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "SerializeTo", gatherRuleInfo.SerializeTo.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "SerializeInterval", gatherRuleInfo.SerializeInterval.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "SerializeIsOrderByDesc", gatherRuleInfo.SerializeIsOrderByDesc.ToString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "SerializeIsAddZero", gatherRuleInfo.SerializeIsAddZero.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "NodeID", gatherRuleInfo.NodeID.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "Charset", ECharsetUtils.GetValue(gatherRuleInfo.Charset));
			AtomUtility.AddDcElement(entry.AdditionalElements, "UrlInclude", gatherRuleInfo.UrlInclude);
			AtomUtility.AddDcElement(entry.AdditionalElements, "TitleInclude", gatherRuleInfo.TitleInclude);
			AtomUtility.AddDcElement(entry.AdditionalElements, "ContentExclude", AtomUtility.Encrypt(gatherRuleInfo.ContentExclude));//加密
			AtomUtility.AddDcElement(entry.AdditionalElements, "ContentHtmlClearCollection", gatherRuleInfo.ContentHtmlClearCollection);
            AtomUtility.AddDcElement(entry.AdditionalElements, "ContentHtmlClearTagCollection", gatherRuleInfo.ContentHtmlClearTagCollection);
			AtomUtility.AddDcElement(entry.AdditionalElements, "LastGatherDate", gatherRuleInfo.LastGatherDate.ToLongDateString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "ListAreaStart", AtomUtility.Encrypt(gatherRuleInfo.ListAreaStart));//加密
			AtomUtility.AddDcElement(entry.AdditionalElements, "ListAreaEnd", AtomUtility.Encrypt(gatherRuleInfo.ListAreaEnd));//加密
			AtomUtility.AddDcElement(entry.AdditionalElements, "ContentChannelStart", AtomUtility.Encrypt(gatherRuleInfo.ContentChannelStart));//加密
			AtomUtility.AddDcElement(entry.AdditionalElements, "ContentChannelEnd", AtomUtility.Encrypt(gatherRuleInfo.ContentChannelEnd));//加密
			AtomUtility.AddDcElement(entry.AdditionalElements, "ContentTitleStart", AtomUtility.Encrypt(gatherRuleInfo.ContentTitleStart));//加密
			AtomUtility.AddDcElement(entry.AdditionalElements, "ContentTitleEnd", AtomUtility.Encrypt(gatherRuleInfo.ContentTitleEnd));//加密
			AtomUtility.AddDcElement(entry.AdditionalElements, "ContentContentStart", AtomUtility.Encrypt(gatherRuleInfo.ContentContentStart));//加密
			AtomUtility.AddDcElement(entry.AdditionalElements, "ContentContentEnd", AtomUtility.Encrypt(gatherRuleInfo.ContentContentEnd));//加密
			AtomUtility.AddDcElement(entry.AdditionalElements, "ContentNextPageStart", AtomUtility.Encrypt(gatherRuleInfo.ContentNextPageStart));//加密
			AtomUtility.AddDcElement(entry.AdditionalElements, "ContentNextPageEnd", AtomUtility.Encrypt(gatherRuleInfo.ContentNextPageEnd));//加密
            AtomUtility.AddDcElement(entry.AdditionalElements, "ContentAttributes", AtomUtility.Encrypt(gatherRuleInfo.ContentAttributes));//加密
            AtomUtility.AddDcElement(entry.AdditionalElements, "ContentAttributesXML", AtomUtility.Encrypt(gatherRuleInfo.ContentAttributesXML));//加密
            AtomUtility.AddDcElement(entry.AdditionalElements, "ExtendValues", AtomUtility.Encrypt(gatherRuleInfo.ExtendValues));//加密

			return entry;
		}


		public void ImportGatherRule(bool overwrite)
		{
			if (!FileUtils.IsFileExists(this.filePath)) return;
            AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

			foreach (AtomEntry entry in feed.Entries)
			{
				string gatherRuleName = AtomUtility.GetDcElementContent(entry.AdditionalElements, "GatherRuleName");

				if (!string.IsNullOrEmpty(gatherRuleName))
				{
					GatherRuleInfo gatherRuleInfo = new GatherRuleInfo();
					gatherRuleInfo.GatherRuleName = gatherRuleName;
					gatherRuleInfo.PublishmentSystemID = this.publishmentSystemID;
					gatherRuleInfo.CookieString = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "CookieString"));//解密
                    gatherRuleInfo.GatherUrlIsCollection = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "GatherUrlIsCollection"));
					gatherRuleInfo.GatherUrlCollection = AtomUtility.GetDcElementContent(entry.AdditionalElements, "GatherUrlCollection");
                    gatherRuleInfo.GatherUrlIsSerialize = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "GatherUrlIsSerialize"));
					gatherRuleInfo.GatherUrlSerialize = AtomUtility.GetDcElementContent(entry.AdditionalElements, "GatherUrlSerialize");
					gatherRuleInfo.SerializeFrom = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "SerializeFrom"));
					gatherRuleInfo.SerializeTo = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "SerializeTo"));
					gatherRuleInfo.SerializeInterval = TranslateUtils.ToInt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "SerializeInterval"));
                    gatherRuleInfo.SerializeIsOrderByDesc = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "SerializeIsOrderByDesc"));
                    gatherRuleInfo.SerializeIsAddZero = TranslateUtils.ToBool(AtomUtility.GetDcElementContent(entry.AdditionalElements, "SerializeIsAddZero"));
					gatherRuleInfo.NodeID = this.publishmentSystemID;
					gatherRuleInfo.Charset = ECharsetUtils.GetEnumType(AtomUtility.GetDcElementContent(entry.AdditionalElements, "Charset"));
					gatherRuleInfo.UrlInclude = AtomUtility.GetDcElementContent(entry.AdditionalElements, "UrlInclude");
					gatherRuleInfo.TitleInclude = AtomUtility.GetDcElementContent(entry.AdditionalElements, "TitleInclude");
					gatherRuleInfo.ContentExclude = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentExclude"));//解密
					gatherRuleInfo.ContentHtmlClearCollection = AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentHtmlClearCollection");
                    gatherRuleInfo.ContentHtmlClearTagCollection = AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentHtmlClearTagCollection");
					gatherRuleInfo.LastGatherDate = DateUtils.SqlMinValue;
					gatherRuleInfo.ListAreaStart = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ListAreaStart"));//解密
					gatherRuleInfo.ListAreaEnd = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ListAreaEnd"));//解密
					gatherRuleInfo.ContentChannelStart = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentChannelStart"));//解密
					gatherRuleInfo.ContentChannelEnd = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentChannelEnd"));//解密
					gatherRuleInfo.ContentTitleStart = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentTitleStart"));//解密
					gatherRuleInfo.ContentTitleEnd = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentTitleEnd"));//解密
					gatherRuleInfo.ContentContentStart = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentContentStart"));//解密
					gatherRuleInfo.ContentContentEnd = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentContentEnd"));//解密
					gatherRuleInfo.ContentNextPageStart = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentNextPageStart"));//解密
					gatherRuleInfo.ContentNextPageEnd = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentNextPageEnd"));//解密
                    gatherRuleInfo.ContentAttributes = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentAttributes"));//解密
                    gatherRuleInfo.ContentAttributesXML = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ContentAttributesXML"));//解密
                    gatherRuleInfo.ExtendValues = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, "ExtendValues"));//解密

					GatherRuleInfo srcGatherRuleInfo = DataProvider.GatherRuleDAO.GetGatherRuleInfo(gatherRuleInfo.GatherRuleName, this.publishmentSystemID);
					if (srcGatherRuleInfo != null)
					{
						if (overwrite)
						{
							DataProvider.GatherRuleDAO.Update(gatherRuleInfo);
						}
						else
						{
							string importGatherRuleName = DataProvider.GatherRuleDAO.GetImportGatherRuleName(this.publishmentSystemID, gatherRuleInfo.GatherRuleName);
							gatherRuleInfo.GatherRuleName = importGatherRuleName;
							DataProvider.GatherRuleDAO.Insert(gatherRuleInfo);
						}
					}
					else
					{
						DataProvider.GatherRuleDAO.Insert(gatherRuleInfo);
					}
				}
			}
		}

	}
}
