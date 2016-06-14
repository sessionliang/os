using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;

using BaiRong.Core.Data.Provider;
using System;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.STL.Parser.StlEntity
{
	public class StlPhotoEntities
	{
        private StlPhotoEntities()
		{
		}

        public const string EntityName = "Photo";                  //��ƷͼƬʵ��

        public static string PhotoID = "PhotoID";
        public static string SmallUrl = "SmallUrl";
        public static string MiddleUrl = "MiddleUrl";
        public static string LargeUrl = "LargeUrl";
        public static string Description = "Description";
        public static string ItemIndex = "ItemIndex";

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                attributes.Add(PhotoID, "ͼƬID");
                attributes.Add(SmallUrl, "Сͼ��ַ");
                attributes.Add(MiddleUrl, "��ͼ��ַ");
                attributes.Add(LargeUrl, "��ͼ��ַ");
                attributes.Add(Description, "ͼƬ˵��");
                attributes.Add(ItemIndex, "ͼƬ����");
                return attributes;
            }
        }

        internal static string Parse(string stlEntity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;

            if (contextInfo.ItemContainer == null || contextInfo.ItemContainer.PhotoItem == null) return string.Empty;

            try
            {
                string entityName = StlParserUtility.GetNameFromEntity(stlEntity);

                string type = entityName.Substring(7, entityName.Length - 8).ToLower();

                PhotoInfo photoInfo = new PhotoInfo(contextInfo.ItemContainer.PhotoItem.DataItem);

                if (StringUtils.EqualsIgnoreCase(type, StlPhotoEntities.PhotoID))
                {
                    parsedContent = photoInfo.ID.ToString();
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlPhotoEntities.SmallUrl))
                {
                    parsedContent = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, photoInfo.SmallUrl);
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlPhotoEntities.MiddleUrl))
                {
                    parsedContent = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, photoInfo.MiddleUrl);
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlPhotoEntities.LargeUrl))
                {
                    parsedContent = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, photoInfo.LargeUrl);
                }
                else if (StringUtils.EqualsIgnoreCase(type, StlPhotoEntities.Description))
                {
                    parsedContent = photoInfo.Description;
                }
                else if (StringUtils.StartsWithIgnoreCase(type, StlParserUtility.ItemIndex))
                {
                    parsedContent = StlParserUtility.ParseItemIndex(contextInfo.ItemContainer.PhotoItem.ItemIndex, type, contextInfo).ToString();
                }
            }
            catch { }

            return parsedContent;
        }
	}
}
