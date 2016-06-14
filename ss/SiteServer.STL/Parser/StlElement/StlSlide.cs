using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using SiteServer.CMS.Core;
using System.Collections.Generic;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlSlide
	{
        private StlSlide() { }
		public const string ElementName = "stl:slide";                      //显示图片幻灯片
		
        public const string Attribute_IsDynamic = "isdynamic";              //是否动态显示

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
				return attributes;
			}
		}

        //对“图片幻灯片”（stl:slide）元素进行解析
        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
            try
            {
                IEnumerator ie = node.Attributes.GetEnumerator();

                bool isDynamic = false;

                while (ie.MoveNext())
                {
                    XmlAttribute attr = (XmlAttribute)ie.Current;
                    string attributeName = attr.Name.ToLower();
                    if (attributeName.Equals(Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                }

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(pageInfo, contextInfo);
                }
            }
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(PageInfo pageInfo, ContextInfo contextInfo)
        {
            pageInfo.AddPageScriptsIfNotExists(PageInfo.JQuery.A_JQuery);
            pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Ac_SWFObject);

            ContentInfo contentInfo = contextInfo.ContentInfo;
            if (contentInfo == null)
            {
                contentInfo = DataProvider.ContentDAO.GetContentInfo(ETableStyle.BackgroundContent, pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contextInfo.ContentID);
            }

            List<PhotoInfo> photoInfoList = DataProvider.PhotoDAO.GetPhotoInfoList(pageInfo.PublishmentSystemID, contextInfo.ContentID);

            StringBuilder builder = new StringBuilder();

            builder.AppendFormat(@"
<script type=""text/javascript"">
var slideFullScreenUrl = ""{0}"";
", PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.Slide.FullScreenSwf));

            builder.Append(@"
var slide_data = {
");

            builder.AppendFormat(@"
    ""slide"":{{""title"":""{0}""}},
    ""images"":[
", StringUtils.ToJsString(contentInfo.Title));


            foreach (PhotoInfo photoInfo in photoInfoList)
            {
                builder.AppendFormat(@"
            {{""title"":""{0}"",""intro"":""{1}"",""previewUrl"":""{2}"",""imageUrl"":""{3}"",""id"":""{4}""}},", StringUtils.ToJsString(contentInfo.Title), StringUtils.ToJsString(photoInfo.Description), StringUtils.ToJsString(PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, photoInfo.SmallUrl)), StringUtils.ToJsString(PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, photoInfo.LargeUrl)), photoInfo.ID);
            }

            if (photoInfoList.Count > 0)
            {
                builder.Length -= 1;
            }

            builder.Append(@"
    ],
");

            int siblingContentID = BaiRongDataProvider.ContentDAO.GetContentID(pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contentInfo.NodeID, contentInfo.Taxis, true);

            if (siblingContentID > 0)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contentInfo.NodeID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(pageInfo.PublishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, nodeInfo);
                ContentInfo siblingContentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, siblingContentID);
                string title = siblingContentInfo.Title;
                string url = PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, siblingContentInfo, pageInfo.VisualType);
                PhotoInfo photoInfo = DataProvider.PhotoDAO.GetFirstPhotoInfo(pageInfo.PublishmentSystemID, siblingContentID);
                string previewUrl = string.Empty;
                if (photoInfo != null)
                {
                    previewUrl = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, photoInfo.SmallUrl);
                }
                else
                {
                    previewUrl = PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.GetIconUrl("s.gif"));
                }
                builder.AppendFormat(@"
    ""next_album"":{{""title"":""{0}"",""url"":""{1}"",""previewUrl"":""{2}""}},
", StringUtils.ToJsString(title), StringUtils.ToJsString(url), StringUtils.ToJsString(previewUrl));
            }
            else
            {
                builder.AppendFormat(@"
    ""next_album"":{{""title"":"""",""url"":""javascript:void(0);"",""previewUrl"":""{0}""}},
", StringUtils.ToJsString(PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.GetIconUrl("s.gif"))));
            }

            siblingContentID = BaiRongDataProvider.ContentDAO.GetContentID(pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contentInfo.NodeID, contentInfo.Taxis, false);

            if (siblingContentID > 0)
            {
                NodeInfo nodeInfo = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, contentInfo.NodeID);
                ETableStyle tableStyle = NodeManager.GetTableStyle(pageInfo.PublishmentSystemInfo, nodeInfo);
                string tableName = NodeManager.GetTableName(pageInfo.PublishmentSystemInfo, nodeInfo);
                ContentInfo siblingContentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, siblingContentID);
                string title = siblingContentInfo.Title;
                string url = PageUtility.GetContentUrl(pageInfo.PublishmentSystemInfo, siblingContentInfo, pageInfo.VisualType);

                PhotoInfo photoInfo = DataProvider.PhotoDAO.GetFirstPhotoInfo(pageInfo.PublishmentSystemID, siblingContentID);
                string previewUrl = string.Empty;
                if (photoInfo != null)
                {
                    previewUrl = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, photoInfo.SmallUrl);
                }
                else
                {
                    previewUrl = PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.GetIconUrl("s.gif"));
                }
                builder.AppendFormat(@"
    ""prev_album"":{{""title"":""{0}"",""url"":""{1}"",""previewUrl"":""{2}""}}
", StringUtils.ToJsString(title), StringUtils.ToJsString(url), StringUtils.ToJsString(previewUrl));
            }
            else
            {
                builder.AppendFormat(@"
    ""prev_album"":{{""title"":"""",""url"":""javascript:void(0);"",""previewUrl"":""{0}""}}
", StringUtils.ToJsString(PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.GetIconUrl("s.gif"))));
            }

            builder.Append(@"
}
</script>
");

            builder.AppendFormat(@"
<link href=""{0}"" rel=""stylesheet"" />
<script src=""{1}"" type=""text/javascript"" charset=""gb2312""></script>
", PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.Slide.Css), PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.Slide.Js));

            builder.Append(CreateCacheManager.FileContent.GetContentByFilePath(PathUtils.GetSiteFilesPath(SiteFiles.Slide.Template), ECharset.utf_8));

            return builder.ToString();
        }
	}
}
