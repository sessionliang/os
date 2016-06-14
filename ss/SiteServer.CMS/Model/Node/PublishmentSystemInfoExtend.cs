using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.CMS.Model
{
    public class PublishmentSystemInfoExtend : ExtendedAttributes
    {
        public PublishmentSystemInfoExtend(string settingsXML)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(settingsXML);
            base.SetExtendedAttribute(nameValueCollection);
        }

        /****************应用设置********************/

        public string Charset
        {
            get { return base.GetString("Charset", ECharsetUtils.GetValue(ECharset.utf_8)); }
            set { base.SetExtendedAttribute("Charset", value); }
        }

        public int PageSize
        {
            get { return base.GetInt("PageSize", 30); }
            set { base.SetExtendedAttribute("PageSize", value.ToString()); }
        }

        public EVisualType VisualType
        {
            get
            {
                EVisualType visualType = EVisualTypeUtils.GetEnumType(base.GetString("VisualType", EVisualTypeUtils.GetValue(EVisualType.Static)));
                if (visualType != EVisualType.Static && visualType != EVisualType.Dynamic)
                {
                    visualType = EVisualType.Static;
                }
                return visualType;
            }
            set { base.SetExtendedAttribute("VisualType", EVisualTypeUtils.GetValue(value)); }
        }

        public EDesignMode DesignMode
        {
            get { return EDesignModeUtils.GetEnumType(base.GetString("DesignMode", EDesignModeUtils.GetValue(EDesignMode.Edit))); }
            set { base.SetExtendedAttribute("DesignMode", EDesignModeUtils.GetValue(value)); }
        }

        public bool IsDesignGuideLine
        {
            get { return base.GetBool("IsDesignGuideLine", false); }
            set { base.SetExtendedAttribute("IsDesignGuideLine", value.ToString()); }
        }

        public bool IsCountDownload
        {
            get { return base.GetBool("IsCountDownload", true); }
            set { base.SetExtendedAttribute("IsCountDownload", value.ToString()); }
        }

        public bool IsCountHits
        {
            get { return base.GetBool("IsCountHits", false); }
            set { base.SetExtendedAttribute("IsCountHits", value.ToString()); }
        }

        public bool IsCountHitsByDay
        {
            get { return base.GetBool("IsCountHitsByDay", false); }
            set { base.SetExtendedAttribute("IsCountHitsByDay", value.ToString()); }
        }

        /// <summary>
        /// 培生智能推送
        /// by 20151124 sofuny
        /// 是否启用智能推送统计
        /// </summary>
        public bool IsIntelligentPushCount
        {
            get { return base.GetBool("IsIntelligentPushCount", false); }
            set { base.SetExtendedAttribute("IsIntelligentPushCount", value.ToString()); }
        }

        /// <summary>
        /// 培生智能推送
        /// by 20151124 sofuny
        /// 选择推送时间段统计区间
        /// </summary>
        public EIntelligentPushType IntelligentPushType
        {
            get { return EIntelligentPushTypeUtils.GetEnumType(base.GetString("IntelligentPushType", EIntelligentPushTypeUtils.GetValue(EIntelligentPushType.ALL))); }
            set { base.SetExtendedAttribute("IntelligentPushType", EIntelligentPushTypeUtils.GetValue(value)); }
        }

        public bool IsGroupContent
        {
            get { return base.GetBool("IsGroupContent", true); }
            set { base.SetExtendedAttribute("IsGroupContent", value.ToString()); }
        }

        public bool IsRelatedByTags
        {
            get { return base.GetBool("IsRelatedByTags", true); }
            set { base.SetExtendedAttribute("IsRelatedByTags", value.ToString()); }
        }

        public bool IsTranslate
        {
            get { return base.GetBool("IsTranslate", true); }
            set { base.SetExtendedAttribute("IsTranslate", value.ToString()); }
        }

        public bool IsAutoSaveContent
        {
            get { return base.GetBool("IsAutoSaveContent", true); }
            set { base.SetExtendedAttribute("IsAutoSaveContent", value.ToString()); }
        }

        public int AutoSaveContentInterval
        {
            get { return base.GetInt("AutoSaveContentInterval", 180); }
            set { base.SetExtendedAttribute("AutoSaveContentInterval", value.ToString()); }
        }

        public bool IsSaveImageInTextEditor
        {
            get { return base.GetBool("IsSaveImageInTextEditor", true); }
            set { base.SetExtendedAttribute("IsSaveImageInTextEditor", value.ToString()); }
        }

        public bool IsAutoPageInTextEditor
        {
            get { return base.GetBool("IsAutoPageInTextEditor", false); }
            set { base.SetExtendedAttribute("IsAutoPageInTextEditor", value.ToString()); }
        }

        public int AutoPageWordNum
        {
            get { return base.GetInt("AutoPageWordNum", 1500); }
            set { base.SetExtendedAttribute("AutoPageWordNum", value.ToString()); }
        }

        public bool IsContentTitleBreakLine
        {
            get { return base.GetBool("IsContentTitleBreakLine", false); }
            set { base.SetExtendedAttribute("IsContentTitleBreakLine", value.ToString()); }
        }

        /// <summary>
        /// 敏感词自动检测
        /// </summary>
        public bool IsAutoCheckKeywords
        {
            get { return base.GetBool("lIsAutoCheckKeywords", false); }
            set { base.SetExtendedAttribute("lIsAutoCheckKeywords", value.ToString()); }
        }

        public ETextEditorType TextEditorType
        {
            get { return ETextEditorTypeUtils.GetEnumType(base.GetString("TextEditorType", ETextEditorTypeUtils.GetValue(ETextEditorType.UEditor))); }
            set { base.SetExtendedAttribute("TextEditorType", ETextEditorTypeUtils.GetValue(value)); }
        }

        /// <summary>
        /// 编辑器上传文件URL前缀
        /// </summary>
        public string EditorUploadFilePre
        {
            get { return base.GetString("EditorUploadFilePre", string.Empty); }
            set { base.SetExtendedAttribute("EditorUploadFilePre", value); }
        }

        public int PhotoContentPreviewWidth
        {
            get { return base.GetInt("PhotoContentPreviewWidth", 160); }
            set { base.SetExtendedAttribute("PhotoContentPreviewWidth", value.ToString()); }
        }

        public int PhotoContentPreviewHeight
        {
            get { return base.GetInt("PhotoContentPreviewHeight", 110); }
            set { base.SetExtendedAttribute("PhotoContentPreviewHeight", value.ToString()); }
        }

        public int PhotoSmallWidth
        {
            get { return base.GetInt("PhotoSmallWidth", 70); }
            set { base.SetExtendedAttribute("PhotoSmallWidth", value.ToString()); }
        }

        public int PhotoSmallHeight
        {
            get { return base.GetInt("PhotoSmallHeight", 70); }
            set { base.SetExtendedAttribute("PhotoSmallHeight", value.ToString()); }
        }

        public int PhotoMiddleWidth
        {
            get { return base.GetInt("PhotoMiddleWidth", 400); }
            set { base.SetExtendedAttribute("PhotoMiddleWidth", value.ToString()); }
        }

        public int PhotoMiddleHeight
        {
            get { return base.GetInt("PhotoMiddleHeight", 400); }
            set { base.SetExtendedAttribute("PhotoMiddleHeight", value.ToString()); }
        }

        /****************图片水印设置********************/

        public bool IsWaterMark
        {
            get { return base.GetBool("IsWaterMark", false); }
            set { base.SetExtendedAttribute("IsWaterMark", value.ToString()); }
        }

        public bool IsImageWaterMark
        {
            get { return base.GetBool("IsImageWaterMark", false); }
            set { base.SetExtendedAttribute("IsImageWaterMark", value.ToString()); }
        }

        public int WaterMarkPosition
        {
            get { return base.GetInt("WaterMarkPosition", 9); }
            set { base.SetExtendedAttribute("WaterMarkPosition", value.ToString()); }
        }

        public int WaterMarkTransparency
        {
            get { return base.GetInt("WaterMarkTransparency", 5); }
            set { base.SetExtendedAttribute("WaterMarkTransparency", value.ToString()); }
        }

        public int WaterMarkMinWidth
        {
            get { return base.GetInt("WaterMarkMinWidth", 200); }
            set { base.SetExtendedAttribute("WaterMarkMinWidth", value.ToString()); }
        }

        public int WaterMarkMinHeight
        {
            get { return base.GetInt("WaterMarkMinHeight", 200); }
            set { base.SetExtendedAttribute("WaterMarkMinHeight", value.ToString()); }
        }

        public string WaterMarkFormatString
        {
            get { return base.GetString("WaterMarkFormatString", string.Empty); }
            set { base.SetExtendedAttribute("WaterMarkFormatString", value); }
        }

        public string WaterMarkFontName
        {
            get { return base.GetString("WaterMarkFontName", string.Empty); }
            set { base.SetExtendedAttribute("WaterMarkFontName", value); }
        }

        public int WaterMarkFontSize
        {
            get { return base.GetInt("WaterMarkFontSize", 12); }
            set { base.SetExtendedAttribute("WaterMarkFontSize", value.ToString()); }
        }

        public string WaterMarkImagePath
        {
            get { return base.GetString("WaterMarkImagePath", string.Empty); }
            set { base.SetExtendedAttribute("WaterMarkImagePath", value); }
        }

        public int Qty
        {
            get { return base.GetInt("Qty", 10); }
            set { base.SetExtendedAttribute("Qty", value.ToString()); }
        }

        /****************邮件设置********************/

        public string MailDomain
        {
            get { return base.GetString("MailDomain", "smtp.exmail.qq.com"); }
            set { base.SetExtendedAttribute("MailDomain", value); }
        }

        public int MailDomainPort
        {
            get { return base.GetInt("MailDomainPort", 25); }
            set { base.SetExtendedAttribute("MailDomainPort", value.ToString()); }
        }

        public string MailFromName
        {
            get { return base.GetString("MailFromName", string.Empty); }
            set { base.SetExtendedAttribute("MailFromName", value); }
        }

        public string MailServerUserName
        {
            get { return base.GetString("MailServerUserName", "admin@siteserver.cn"); }
            set { base.SetExtendedAttribute("MailServerUserName", value); }
        }

        public string MailServerPassword
        {
            get { return base.GetString("MailServerPassword", "brtech88"); }
            set { base.SetExtendedAttribute("MailServerPassword", value); }
        }

        public string MailSendTitle
        {
            get { return base.GetString("MailSendTitle", "尊敬的{receiver}，您的朋友{sender}推荐您阅读这篇文章"); }
            set { base.SetExtendedAttribute("MailSendTitle", value); }
        }

        public string MailSendContent
        {
            get
            {
                return base.GetString("MailSendContent", @"您好，您的朋友{sender}为您推荐了一篇文章，请点击查看：<br />
文章标题：{title}<br />
链接地址：{pageurl}<br />
<hr style=""height: 1px;"" />
{sitename}
");
            }
            set { base.SetExtendedAttribute("MailSendContent", value); }
        }

        public string MailSubscribeTitle
        {
            get { return base.GetString("MailSubscribeTitle", "尊敬的{receiver}，{sitename}邮件订阅成功"); }
            set { base.SetExtendedAttribute("MailSubscribeTitle", value); }
        }

        public string MailSubscribeContent
        {
            get
            {
                return base.GetString("MailSubscribeContent", @"恭喜您！{sitename}邮件订阅成功！<br />
您将会定期收到我们为您精心准备的订阅邮件。<br />
如您不希望再收到这些资讯，可以<a href='{unsubscribeurl}' target='_blank'>点击这里</a>退订邮件。<br />
<hr style=""height: 1px;"" />
{sitename}
");
            }
            set { base.SetExtendedAttribute("MailSubscribeContent", value); }
        }

        public bool IsLogMailSend
        {
            get { return base.GetBool("IsLogMailSend", true); }
            set { base.SetExtendedAttribute("IsLogMailSend", value.ToString()); }
        }

        /****************生成页面设置********************/

        public bool IsMultiDeployment
        {
            get { return base.GetBool("IsMultiDeployment", false); }
            set { base.SetExtendedAttribute("IsMultiDeployment", value.ToString()); }
        }

        public string HomeUrl
        {
            get
            {
                string homeUrl = base.GetString("HomeUrl", string.Empty);
                if (string.IsNullOrEmpty(homeUrl))
                {
                    homeUrl = "@/center";
                }
                return homeUrl;
            }
            set { base.SetExtendedAttribute("HomeUrl", value); }
        }

        public string OuterUrl
        {
            get { return base.GetString("OuterUrl", string.Empty); }
            set { base.SetExtendedAttribute("OuterUrl", value); }
        }

        public string InnerUrl
        {
            get { return base.GetString("InnerUrl", string.Empty); }
            set { base.SetExtendedAttribute("InnerUrl", value); }
        }

        public EFuncFilesType FuncFilesType
        {
            get { return EFuncFilesTypeUtils.GetEnumType(base.GetString("FuncFilesType", string.Empty)); }
            set { base.SetExtendedAttribute("FuncFilesType", EFuncFilesTypeUtils.GetValue(value)); }
        }

        public string IISDefaultPage
        {
            get { return base.GetString("IISDefaultPage", ""); }
            set { base.SetExtendedAttribute("IISDefaultPage", value); }
        }

        public bool IsCreateRedirectPage
        {
            get { return base.GetBool("IsCreateRedirectPage", false); }
            set { base.SetExtendedAttribute("IsCreateRedirectPage", value.ToString()); }
        }

        public string ChannelFilePathRule
        {
            get { return base.GetString("ChannelFilePathRule", "/channels/{@ChannelID}.html"); }
            set { base.SetExtendedAttribute("ChannelFilePathRule", value); }
        }

        public string ContentFilePathRule
        {
            get { return base.GetString("ContentFilePathRule", "/contents/{@ChannelID}/{@ContentID}.html"); }
            set { base.SetExtendedAttribute("ContentFilePathRule", value); }
        }

        public bool IsCreateContentIfContentChanged
        {
            get { return base.GetBool("IsCreateContentIfContentChanged", true); }
            set { base.SetExtendedAttribute("IsCreateContentIfContentChanged", value.ToString()); }
        }

        public bool IsCreateChannelIfChannelChanged
        {
            get { return base.GetBool("IsCreateChannelIfChannelChanged", true); }
            set { base.SetExtendedAttribute("IsCreateChannelIfChannelChanged", value.ToString()); }
        }

        public bool IsCreateShowPageInfo
        {
            get { return base.GetBool("IsCreateShowPageInfo", false); }
            set { base.SetExtendedAttribute("IsCreateShowPageInfo", value.ToString()); }
        }

        public bool IsCreateIE8Compatible
        {
            get { return base.GetBool("IsCreateIE8Compatible", false); }
            set { base.SetExtendedAttribute("IsCreateIE8Compatible", value.ToString()); }
        }

        public bool IsCreateBrowserNoCache
        {
            get { return base.GetBool("IsCreateBrowserNoCache", false); }
            set { base.SetExtendedAttribute("IsCreateBrowserNoCache", value.ToString()); }
        }

        public bool IsCreateJsIgnoreError
        {
            get { return base.GetBool("IsCreateJsIgnoreError", false); }
            set { base.SetExtendedAttribute("IsCreateJsIgnoreError", value.ToString()); }
        }

        public bool IsCreateSearchDuplicate
        {
            get { return base.GetBool("IsCreateSearchDuplicate", true); }
            set { base.SetExtendedAttribute("IsCreateSearchDuplicate", value.ToString()); }
        }

        public bool IsCreateWithJQuery
        {
            get { return base.GetBool("IsCreateWithJQuery", true); }
            set { base.SetExtendedAttribute("IsCreateWithJQuery", value.ToString()); }
        }

        public bool IsCreateIncludeToSSI
        {
            get { return base.GetBool("IsCreateIncludeToSSI", false); }
            set { base.SetExtendedAttribute("IsCreateIncludeToSSI", value.ToString()); }
        }

        public bool IsCreateDoubleClick
        {
            get { return base.GetBool("IsCreateDoubleClick", false); }
            set { base.SetExtendedAttribute("IsCreateDoubleClick", value.ToString()); }
        }

        public int CreateStaticMaxPage
        {
            get { return base.GetInt("CreateStaticMaxPage", 0); }
            set { base.SetExtendedAttribute("CreateStaticMaxPage", value.ToString()); }
        }

        public bool IsCreateChannelsByChildNodeID
        {
            get { return base.GetBool("IsCreateChannelsByChildNodeID", false); }
            set { base.SetExtendedAttribute("IsCreateChannelsByChildNodeID", value.ToString()); }
        }

        public int CreateScopeByChildNodeID
        {
            get { return base.GetInt("CreateScopeByChildNodeID", 3); }
            set { base.SetExtendedAttribute("CreateScopeByChildNodeID", value.ToString()); }
        }

        //add by liangjian at 20150727, 设置API访问路径，方便API分离部署
        //public bool IsSeparateAPI
        //{
        //    get { return base.GetBool("IsSeparateAPI", false); }
        //    set { base.SetExtendedAttribute("IsSeparateAPI", value.ToString()); }
        //}

        //子站单独部署 20151116 sessionliang
        public bool IsSonSiteAlone
        {
            get { return base.GetBool("IsSonSiteAlone", false); }
            set { base.SetExtendedAttribute("IsSonSiteAlone", value.ToString()); }
        }

        public string APIUrl
        {
            get { return base.GetString("APIUrl", "/api"); }
            set { base.SetExtendedAttribute("APIUrl", value); }
        }

        /****************应用地图设置********************/

        public string SiteMapGooglePath
        {
            get { return base.GetString("SiteMapGooglePath", "@/sitemap.xml"); }
            set { base.SetExtendedAttribute("SiteMapGooglePath", value); }
        }

        public string SiteMapGoogleChangeFrequency
        {
            get { return base.GetString("SiteMapGoogleChangeFrequency", "daily"); }
            set { base.SetExtendedAttribute("SiteMapGoogleChangeFrequency", value); }
        }

        public bool SiteMapGoogleIsShowLastModified
        {
            get { return base.GetBool("SiteMapGoogleIsShowLastModified", false); }
            set { base.SetExtendedAttribute("SiteMapGoogleIsShowLastModified", value.ToString()); }
        }

        public int SiteMapGooglePageCount
        {
            get { return base.GetInt("SiteMapGooglePageCount", 10000); }
            set { base.SetExtendedAttribute("SiteMapGooglePageCount", value.ToString()); }
        }

        public string SiteMapBaiduPath
        {
            get { return base.GetString("SiteMapBaiduPath", "@/baidunews.xml"); }
            set { base.SetExtendedAttribute("SiteMapBaiduPath", value); }
        }

        public string SiteMapBaiduWebMaster
        {
            get { return base.GetString("SiteMapBaiduWebMaster", string.Empty); }
            set { base.SetExtendedAttribute("SiteMapBaiduWebMaster", value); }
        }

        public string SiteMapBaiduUpdatePeri
        {
            get { return base.GetString("SiteMapBaiduUpdatePeri", "15"); }
            set { base.SetExtendedAttribute("SiteMapBaiduUpdatePeri", value); }
        }

        /****************流量统计设置********************/

        public bool IsTracker
        {
            get { return base.GetBool("IsTracker", false); }
            set { base.SetExtendedAttribute("IsTracker", value.ToString()); }
        }

        public int TrackerDays
        {
            get { return base.GetInt("TrackerDays", 0); }
            set { base.SetExtendedAttribute("TrackerDays", value.ToString()); }
        }

        public int TrackerPageView
        {
            get { return base.GetInt("TrackerPageView", 0); }
            set { base.SetExtendedAttribute("TrackerPageView", value.ToString()); }
        }

        public int TrackerUniqueVisitor
        {
            get { return base.GetInt("TrackerUniqueVisitor", 0); }
            set { base.SetExtendedAttribute("TrackerUniqueVisitor", value.ToString()); }
        }

        public int TrackerCurrentMinute
        {
            get { return base.GetInt("TrackerCurrentMinute", 30); }
            set { base.SetExtendedAttribute("TrackerCurrentMinute", value.ToString()); }
        }

        public ETrackerStyle TrackerStyle
        {
            get { return ETrackerStyleUtils.GetEnumType(base.GetString("TrackerStyle", ETrackerStyleUtils.GetValue(ETrackerStyle.Style1))); }
            set { base.SetExtendedAttribute("TrackerStyle", ETrackerStyleUtils.GetValue(value)); }
        }

        /****************显示项设置********************/

        public string ChannelDisplayAttributes
        {
            get { return base.GetString("ChannelDisplayAttributes", string.Empty); }
            set { base.SetExtendedAttribute("ChannelDisplayAttributes", value); }
        }

        public string ChannelEditAttributes
        {
            get { return base.GetString("ChannelEditAttributes", string.Empty); }
            set { base.SetExtendedAttribute("ChannelEditAttributes", value); }
        }

        /****************跨站转发设置********************/

        public bool IsCrossSiteTransChecked
        {
            get { return base.GetBool("IsCrossSiteTransChecked", false); }
            set { base.SetExtendedAttribute("IsCrossSiteTransChecked", value.ToString()); }
        }

        /****************站内链接设置********************/

        public bool IsInnerLink
        {
            get { return base.GetBool("IsInnerLink", false); }
            set { base.SetExtendedAttribute("IsInnerLink", value.ToString()); }
        }

        public bool IsInnerLinkByChannelName
        {
            get { return base.GetBool("IsInnerLinkByChannelName", false); }
            set { base.SetExtendedAttribute("IsInnerLinkByChannelName", value.ToString()); }
        }

        public string InnerLinkFormatString
        {
            get { return base.GetString("InnerLinkFormatString", @"<a href=""{0}"" target=""_blank"">{1}</a>"); }
            set { base.SetExtendedAttribute("InnerLinkFormatString", value); }
        }

        public int InnerLinkMaxNum
        {
            get { return base.GetInt("InnerLinkMaxNum", 10); }
            set { base.SetExtendedAttribute("InnerLinkMaxNum", value.ToString()); }
        }

        /****************页面访问限制********************/

        public bool IsRestriction
        {
            get { return base.GetBool("IsRestriction", false); }
            set { base.SetExtendedAttribute("IsRestriction", value.ToString()); }
        }

        public StringCollection RestrictionBlackList
        {
            get { return TranslateUtils.StringCollectionToStringCollection(base.GetString("RestrictionBlackList", string.Empty)); }
            set { base.SetExtendedAttribute("RestrictionBlackList", TranslateUtils.ObjectCollectionToString(value)); }
        }

        public StringCollection RestrictionWhiteList
        {
            get { return TranslateUtils.StringCollectionToStringCollection(base.GetString("RestrictionWhiteList", string.Empty)); }
            set { base.SetExtendedAttribute("RestrictionWhiteList", TranslateUtils.ObjectCollectionToString(value)); }
        }

        /****************记录系统操作设置********************/

        //background_templateAdd.aspx

        public string Config_TemplateIsCodeMirror
        {
            get { return base.GetString("Config_TemplateIsCodeMirror", "True"); }
            set { base.SetExtendedAttribute("Config_TemplateIsCodeMirror", value); }
        }

        //modal_textEditorInsertVideo.aspx
        public int Config_VideoContentInsertWidth
        {
            get { return base.GetInt("Config_VideoContentInsertWidth", 420); }
            set { base.SetExtendedAttribute("Config_VideoContentInsertWidth", value.ToString()); }
        }

        public int Config_VideoContentInsertHeight
        {
            get { return base.GetInt("Config_VideoContentInsertHeight", 280); }
            set { base.SetExtendedAttribute("Config_VideoContentInsertHeight", value.ToString()); }
        }

        //modal_contentExport.aspx
        public string Config_ExportType
        {
            get { return base.GetString("Config_ExportType", string.Empty); }
            set { base.SetExtendedAttribute("Config_ExportType", value); }
        }

        public string Config_ExportPeriods
        {
            get { return base.GetString("Config_ExportPeriods", string.Empty); }
            set { base.SetExtendedAttribute("Config_ExportPeriods", value); }
        }

        public string Config_ExportDisplayAttributes
        {
            get { return base.GetString("Config_ExportDisplayAttributes", string.Empty); }
            set { base.SetExtendedAttribute("Config_ExportDisplayAttributes", value); }
        }

        public string Config_ExportIsChecked
        {
            get { return base.GetString("IsChecked", string.Empty); }
            set { base.SetExtendedAttribute("IsChecked", value); }
        }

        //modal_selectImage.aspx
        public string Config_SelectImageCurrentUrl
        {
            get { return base.GetString("Config_SelectImageCurrentUrl", "@/" + this.ImageUploadDirectoryName); }
            set { base.SetExtendedAttribute("Config_SelectImageCurrentUrl", value); }
        }

        //modal_selectVideo.aspx
        public string Config_SelectVideoCurrentUrl
        {
            get { return base.GetString("Config_SelectVideoCurrentUrl", "@/" + this.VideoUploadDirectoryName); }
            set { base.SetExtendedAttribute("Config_SelectVideoCurrentUrl", value); }
        }

        //modal_selectFile.aspx
        public string Config_SelectFileCurrentUrl
        {
            get { return base.GetString("Config_SelectFileCurrentUrl", "@/" + this.FileUploadDirectoryName); }
            set { base.SetExtendedAttribute("Config_SelectFileCurrentUrl", value); }
        }

        //modal_uploadImage.aspx
        public string Config_UploadImageIsTitleImage
        {
            get { return base.GetString("Config_UploadImageIsTitleImage", "True"); }
            set { base.SetExtendedAttribute("Config_UploadImageIsTitleImage", value); }
        }

        public string Config_UploadImageTitleImageWidth
        {
            get { return base.GetString("Config_UploadImageTitleImageWidth", "300"); }
            set { base.SetExtendedAttribute("Config_UploadImageTitleImageWidth", value); }
        }

        public string Config_UploadImageTitleImageHeight
        {
            get { return base.GetString("Config_UploadImageTitleImageHeight", string.Empty); }
            set { base.SetExtendedAttribute("Config_UploadImageTitleImageHeight", value); }
        }

        public string Config_UploadImageIsTitleImageLessSizeNotThumb
        {
            get { return base.GetString("Config_UploadImageIsTitleImageLessSizeNotThumb", string.Empty); }
            set { base.SetExtendedAttribute("Config_UploadImageIsTitleImageLessSizeNotThumb", value); }
        }

        public string Config_UploadImageIsShowImageInTextEditor
        {
            get { return base.GetString("Config_UploadImageIsShowImageInTextEditor", "True"); }
            set { base.SetExtendedAttribute("Config_UploadImageIsShowImageInTextEditor", value); }
        }

        public string Config_UploadImageIsLinkToOriginal
        {
            get { return base.GetString("Config_UploadImageIsLinkToOriginal", string.Empty); }
            set { base.SetExtendedAttribute("Config_UploadImageIsLinkToOriginal", value); }
        }

        public string Config_UploadImageIsSmallImage
        {
            get { return base.GetString("Config_UploadImageIsSmallImage", "True"); }
            set { base.SetExtendedAttribute("Config_UploadImageIsSmallImage", value); }
        }

        public string Config_UploadImageSmallImageWidth
        {
            get { return base.GetString("Config_UploadImageSmallImageWidth", "500"); }
            set { base.SetExtendedAttribute("Config_UploadImageSmallImageWidth", value); }
        }

        public string Config_UploadImageSmallImageHeight
        {
            get { return base.GetString("Config_UploadImageSmallImageHeight", string.Empty); }
            set { base.SetExtendedAttribute("Config_UploadImageSmallImageHeight", value); }
        }

        public string Config_UploadImageIsSmallImageLessSizeNotThumb
        {
            get { return base.GetString("Config_UploadImageIsSmallImageLessSizeNotThumb", string.Empty); }
            set { base.SetExtendedAttribute("Config_UploadImageIsSmallImageLessSizeNotThumb", value); }
        }

        /****************评论设置********************/

        public bool IsContentCommentable
        {
            get { return base.GetBool("IsContentCommentable", true); }
            set { base.SetExtendedAttribute("IsContentCommentable", value.ToString()); }
        }

        /****************bShare设置****************/
        public string BshareUserName
        {
            get { return base.GetString("BshareUserName", string.Empty); }
            set { base.SetExtendedAttribute("BshareUserName", value); }
        }
        public string BshareUUID
        {
            get { return base.GetString("BshareUUID", string.Empty); }
            set { base.SetExtendedAttribute("BshareUUID", value); }
        }
        public string BsharePassword
        {
            get { return base.GetString("BsharePassword", string.Empty); }
            set { base.SetExtendedAttribute("BsharePassword", value); }
        }
        public string BshareSecret
        {
            get { return base.GetString("BshareSecret", string.Empty); }
            set { base.SetExtendedAttribute("BshareSecret", value); }
        }
        public string BshareJs
        {
            get { return base.GetString("BshareJs", string.Empty); }
            set { base.SetExtendedAttribute("BshareJs", value); }
        }

        /****************应用基本设置********************/
        public string SiteSettingsCollection
        {
            get { return base.GetString("SiteSettingsCollection", string.Empty); }
            set { base.SetExtendedAttribute("SiteSettingsCollection", value); }
        }

        #region 存储空间设置

        public bool IsSiteStorage
        {
            get { return base.GetBool("IsSiteStorage", false); }
            set { base.SetExtendedAttribute("IsSiteStorage", value.ToString()); }
        }

        public int SiteStorageID
        {
            get { return base.GetInt("SiteStorageID", 0); }
            set { base.SetExtendedAttribute("SiteStorageID", value.ToString()); }
        }

        public string SiteStoragePath
        {
            get { return base.GetString("SiteStoragePath", string.Empty); }
            set { base.SetExtendedAttribute("SiteStoragePath", value); }
        }

        public bool IsImageStorage
        {
            get { return base.GetBool("IsImageStorage", false); }
            set { base.SetExtendedAttribute("IsImageStorage", value.ToString()); }
        }

        public int ImageStorageID
        {
            get { return base.GetInt("ImageStorageID", 0); }
            set { base.SetExtendedAttribute("ImageStorageID", value.ToString()); }
        }

        public string ImageStoragePath
        {
            get { return base.GetString("ImageStoragePath", string.Empty); }
            set { base.SetExtendedAttribute("ImageStoragePath", value); }
        }

        public string ImageUploadDirectoryName
        {
            get { return base.GetString("ImageUploadDirectoryName", "upload/images"); }
            set { base.SetExtendedAttribute("ImageUploadDirectoryName", value); }
        }

        public string ImageUploadDateFormatString
        {
            get { return base.GetString("ImageUploadDateFormatString", EDateFormatTypeUtils.GetValue(EDateFormatType.Month)); }
            set { base.SetExtendedAttribute("ImageUploadDateFormatString", value); }
        }

        public bool IsImageUploadChangeFileName
        {
            get { return base.GetBool("IsImageUploadChangeFileName", true); }
            set { base.SetExtendedAttribute("IsImageUploadChangeFileName", value.ToString()); }
        }

        public string ImageUploadTypeCollection
        {
            get { return base.GetString("ImageUploadTypeCollection", "gif|jpg|jpeg|bmp|png|pneg|swf"); }
            set { base.SetExtendedAttribute("ImageUploadTypeCollection", value); }
        }

        public int ImageUploadTypeMaxSize
        {
            get { return base.GetInt("ImageUploadTypeMaxSize", 15360); }
            set { base.SetExtendedAttribute("ImageUploadTypeMaxSize", value.ToString()); }
        }

        public bool IsVideoStorage
        {
            get { return base.GetBool("IsVideoStorage", false); }
            set { base.SetExtendedAttribute("IsVideoStorage", value.ToString()); }
        }

        public int VideoStorageID
        {
            get { return base.GetInt("VideoStorageID", 0); }
            set { base.SetExtendedAttribute("VideoStorageID", value.ToString()); }
        }

        public string VideoStoragePath
        {
            get { return base.GetString("VideoStoragePath", string.Empty); }
            set { base.SetExtendedAttribute("VideoStoragePath", value); }
        }

        public string VideoUploadDirectoryName
        {
            get { return base.GetString("VideoUploadDirectoryName", "upload/videos"); }
            set { base.SetExtendedAttribute("VideoUploadDirectoryName", value); }
        }

        public string VideoUploadDateFormatString
        {
            get { return base.GetString("VideoUploadDateFormatString", EDateFormatTypeUtils.GetValue(EDateFormatType.Month)); }
            set { base.SetExtendedAttribute("VideoUploadDateFormatString", value); }
        }

        public bool IsVideoUploadChangeFileName
        {
            get { return base.GetBool("IsVideoUploadChangeFileName", true); }
            set { base.SetExtendedAttribute("IsVideoUploadChangeFileName", value.ToString()); }
        }

        public string VideoUploadTypeCollection
        {
            get { return base.GetString("VideoUploadTypeCollection", "asf|asx|avi|flv|mid|midi|mov|mp3|mp4|mpg|mpeg|ogg|ra|rm|rmb|rmvb|rp|rt|smi|swf|wav|webm|wma|wmv|viv"); }
            set { base.SetExtendedAttribute("VideoUploadTypeCollection", value); }
        }

        public int VideoUploadTypeMaxSize
        {
            get { return base.GetInt("VideoUploadTypeMaxSize", 307200); }
            set { base.SetExtendedAttribute("VideoUploadTypeMaxSize", value.ToString()); }
        }

        public bool IsFileStorage
        {
            get { return base.GetBool("IsFileStorage", false); }
            set { base.SetExtendedAttribute("IsFileStorage", value.ToString()); }
        }

        public int FileStorageID
        {
            get { return base.GetInt("FileStorageID", 0); }
            set { base.SetExtendedAttribute("FileStorageID", value.ToString()); }
        }

        public string FileStoragePath
        {
            get { return base.GetString("FileStoragePath", string.Empty); }
            set { base.SetExtendedAttribute("FileStoragePath", value); }
        }

        public string FileUploadDirectoryName
        {
            get { return base.GetString("FileUploadDirectoryName", "upload/files"); }
            set { base.SetExtendedAttribute("FileUploadDirectoryName", value); }
        }

        public string FileUploadDateFormatString
        {
            get { return base.GetString("FileUploadDateFormatString", EDateFormatTypeUtils.GetValue(EDateFormatType.Month)); }
            set { base.SetExtendedAttribute("FileUploadDateFormatString", value); }
        }

        public bool IsFileUploadChangeFileName
        {
            get { return base.GetBool("IsFileUploadChangeFileName", true); }
            set { base.SetExtendedAttribute("IsFileUploadChangeFileName", value.ToString()); }
        }

        public string FileUploadTypeCollection
        {
            get { return base.GetString("FileUploadTypeCollection", "zip,rar,7z,js,css,txt,doc,docx,ppt,pptx,xls,xlsx,pdf"); }
            set { base.SetExtendedAttribute("FileUploadTypeCollection", value); }
        }

        public int FileUploadTypeMaxSize
        {
            get { return base.GetInt("FileUploadTypeMaxSize", 307200); }
            set { base.SetExtendedAttribute("FileUploadTypeMaxSize", value.ToString()); }
        }

        public bool IsBackupStorage
        {
            get { return base.GetBool("IsBackupStorage", false); }
            set { base.SetExtendedAttribute("IsBackupStorage", value.ToString()); }
        }

        public int BackupStorageID
        {
            get { return base.GetInt("BackupStorageID", 0); }
            set { base.SetExtendedAttribute("BackupStorageID", value.ToString()); }
        }

        public string BackupStoragePath
        {
            get { return base.GetString("BackupStoragePath", string.Empty); }
            set { base.SetExtendedAttribute("BackupStoragePath", value); }
        }

        #endregion

        #region B2C

        /****************商品列表页设置********************/

        public string ListPriceSales
        {
            get
            {
                return base.GetString("ListPriceSales", @"1-499
500-1000
1000-1500
1500-2000
2000-3000
3000-4000
4000元以上|4000-999999");
            }
            set { base.SetExtendedAttribute("ListPriceSales", value.ToString()); }
        }

        public bool OrderIsLocationAll
        {
            get { return base.GetBool("OrderIsLocationAll", true); }
            set { base.SetExtendedAttribute("OrderIsLocationAll", value.ToString()); }
        }

        public string OrderInvoiceContentCollection
        {
            get { return base.GetString("OrderInvoiceContentCollection", "明细"); }
            set { base.SetExtendedAttribute("OrderInvoiceContentCollection", value); }
        }

        public bool OrderIsDeleteAllowed   //是否允许删除
        {
            get { return base.GetBool("OrderIsDeleteAllowed", true); }
            set { base.SetExtendedAttribute("OrderIsDeleteAllowed", value.ToString()); }
        }

        public decimal ShipmentPrice
        {
            get { return base.GetDecimal("ShipmentPrice", 0); }
            set { base.SetExtendedAttribute("ShipmentPrice", value.ToString()); }
        }

        #endregion

        #region WCM

        /****************信息公开设置********************/

        public int GovPublicNodeID
        {
            get { return base.GetInt("GovPublicNodeID", 0); }
            set { base.SetExtendedAttribute("GovPublicNodeID", value.ToString()); }
        }

        public bool GovPublicIsPublisherRelatedDepartmentID
        {
            get { return base.GetBool("GovPublicIsPublisherRelatedDepartmentID", true); }
            set { base.SetExtendedAttribute("GovPublicIsPublisherRelatedDepartmentID", value.ToString()); }
        }

        public string GovPublicDepartmentIDCollection
        {
            get { return base.GetString("GovPublicDepartmentIDCollection", string.Empty); }
            set { base.SetExtendedAttribute("GovPublicDepartmentIDCollection", value); }
        }

        /****************依申请公开设置********************/

        public int GovPublicApplyDateLimit              //办理时限
        {
            get { return base.GetInt("GovPublicApplyDateLimit", 15); }
            set { base.SetExtendedAttribute("GovPublicApplyDateLimit", value.ToString()); }
        }

        public int GovPublicApplyAlertDate              //预警
        {
            get { return base.GetInt("GovPublicApplyAlertDate", -3); }
            set { base.SetExtendedAttribute("GovPublicApplyAlertDate", value.ToString()); }
        }

        public int GovPublicApplyYellowAlertDate      //黄牌
        {
            get { return base.GetInt("GovPublicApplyYellowAlertDate", 3); }
            set { base.SetExtendedAttribute("GovPublicApplyYellowAlertDate", value.ToString()); }
        }

        public int GovPublicApplyRedAlertDate       //红牌
        {
            get { return base.GetInt("GovPublicApplyRedAlertDate", 10); }
            set { base.SetExtendedAttribute("GovPublicApplyRedAlertDate", value.ToString()); }
        }

        public bool GovPublicApplyIsDeleteAllowed   //是否允许删除
        {
            get { return base.GetBool("GovPublicApplyIsDeleteAllowed", true); }
            set { base.SetExtendedAttribute("GovPublicApplyIsDeleteAllowed", value.ToString()); }
        }

        /****************互动交流设置********************/

        public int GovInteractNodeID
        {
            get { return base.GetInt("GovInteractNodeID", 0); }
            set { base.SetExtendedAttribute("GovInteractNodeID", value.ToString()); }
        }

        public int GovInteractApplyDateLimit              //办理时限
        {
            get { return base.GetInt("GovInteractApplyDateLimit", 15); }
            set { base.SetExtendedAttribute("GovInteractApplyDateLimit", value.ToString()); }
        }

        public int GovInteractApplyAlertDate              //预警
        {
            get { return base.GetInt("GovInteractApplyAlertDate", -3); }
            set { base.SetExtendedAttribute("GovInteractApplyAlertDate", value.ToString()); }
        }

        public int GovInteractApplyYellowAlertDate      //黄牌
        {
            get { return base.GetInt("GovInteractApplyYellowAlertDate", 3); }
            set { base.SetExtendedAttribute("GovInteractApplyYellowAlertDate", value.ToString()); }
        }

        public int GovInteractApplyRedAlertDate       //红牌
        {
            get { return base.GetInt("GovInteractApplyRedAlertDate", 10); }
            set { base.SetExtendedAttribute("GovInteractApplyRedAlertDate", value.ToString()); }
        }

        public bool GovInteractApplyIsDeleteAllowed   //是否允许删除
        {
            get { return base.GetBool("GovInteractApplyIsDeleteAllowed", true); }
            set { base.SetExtendedAttribute("GovInteractApplyIsDeleteAllowed", value.ToString()); }
        }

        public bool GovInteractApplyIsOpenWindow   //是否新窗口打开
        {
            get { return base.GetBool("GovInteractApplyIsOpenWindow", false); }
            set { base.SetExtendedAttribute("GovInteractApplyIsOpenWindow", value.ToString()); }
        }

        #endregion

        #region Weixin

        public bool WX_IsWebMenu
        {
            get { return base.GetBool("WX_IsWebMenu", false); }
            set { base.SetExtendedAttribute("WX_IsWebMenu", value.ToString()); }
        }

        public string WX_WebMenuType
        {
            get { return base.GetString("WX_WebMenuType", "Type1"); }
            set { base.SetExtendedAttribute("WX_WebMenuType", value); }
        }

        public string WX_WebMenuColor
        {
            get { return base.GetString("WX_WebMenuColor", "41C281"); }
            set { base.SetExtendedAttribute("WX_WebMenuColor", value); }
        }

        public bool WX_IsWebMenuLeft
        {
            get { return base.GetBool("WX_IsWebMenuLeft", true); }
            set { base.SetExtendedAttribute("WX_IsWebMenuLeft", value.ToString()); }
        }

        public bool WX_IsPoweredBy
        {
            get { return base.GetBool("WX_IsPoweredBy", false); }
            set { base.SetExtendedAttribute("WX_IsPoweredBy", value.ToString()); }
        }

        public string WX_PoweredBy
        {
            get { return base.GetString("WX_PoweredBy", string.Empty); }
            set { base.SetExtendedAttribute("WX_PoweredBy", value.ToString()); }
        }

        public bool Card_IsClaimCardCredits
        {
            get { return base.GetBool("Card_IsClaimCardCredits", false); }
            set { base.SetExtendedAttribute("Card_IsClaimCardCredits", value.ToString()); }
        }

        public int Card_ClaimCardCredits
        {
            get { return base.GetInt("Card_ClaimCardCredits", 20); }
            set { base.SetExtendedAttribute("Card_ClaimCardCredits", value.ToString()); }
        }

        public bool Card_IsGiveConsumeCredits
        {
            get { return base.GetBool("Card_IsGiveConsumeCredits", false); }
            set { base.SetExtendedAttribute("Card_IsGiveConsumeCredits", value.ToString()); }
        }

        public decimal Card_ConsumeAmount
        {
            get { return base.GetDecimal("Card_ConsumeAmount", 100); }
            set { base.SetExtendedAttribute("Card_ConsumeAmount", value.ToString()); }
        }

        public int Card_GiveCredits
        {
            get { return base.GetInt("Card_GiveCredits", 50); }
            set { base.SetExtendedAttribute("Card_GiveCredits", value.ToString()); }
        }

        public bool Card_IsBinding
        {
            get { return base.GetBool("Card_IsBinding", true); }
            set { base.SetExtendedAttribute("Card_IsBinding", value.ToString()); }
        }

        public bool Card_IsExchange
        {
            get { return base.GetBool("WX_IsExchange", true); }
            set { base.SetExtendedAttribute("WX_IsExchange", value.ToString()); }
        }

        public decimal Card_ExchangeProportion
        {
            get { return base.GetDecimal("Card_ExchangeProportion", 10); }
            set { base.SetExtendedAttribute("Card_ExchangeProportion", value.ToString()); }
        }

        public bool Card_IsSign
        {
            get { return base.GetBool("Card_IsSign", false); }
            set { base.SetExtendedAttribute("Card_IsSign", value.ToString()); }
        }

        public string Card_SignCreditsConfigure
        {
            get { return base.GetString("Card_SignCreditsConfigure", string.Empty); }
            set { base.SetExtendedAttribute("Card_SignCreditsConfigure", value.ToString()); }
        }


        #endregion

        #region 直达号

        public bool ZDH_IsEnabled
        {
            get { return base.GetBool("ZDH_IsEnabled", true); }
            set { base.SetExtendedAttribute("ZDH_IsEnabled", value.ToString()); }
        }

        public int ZDH_DataAppID
        {
            get { return base.GetInt("ZDH_DataAppID", 3589836); }
            set { base.SetExtendedAttribute("ZDH_DataAppID", value.ToString()); }
        }

        #endregion

        #region 用户中心
        public bool IsDefaultUserCenter
        {
            get { return base.GetBool("IsDefaultUserCenter", false); }
            set { base.SetExtendedAttribute("IsDefaultUserCenter", value.ToString()); }
        }
        #endregion

        public override string ToString()
        {
            return TranslateUtils.NameValueCollectionToString(base.Attributes);
        }
    }
}
