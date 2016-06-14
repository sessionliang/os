using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;
using BaiRong.Core.Data.Provider;
using System.Collections.Generic;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.StlElement
{
	public class StlPlayer
	{
        private StlPlayer() { }
		public const string ElementName = "stl:player";//显示播放器

		public const string Attribute_ChannelIndex = "channelindex";			//栏目索引
		public const string Attribute_ChannelName = "channelname";				//栏目名称
		public const string Attribute_Parent = "parent";						//显示父栏目
		public const string Attribute_UpLevel = "uplevel";						//上级栏目的级别
        public const string Attribute_TopLevel = "toplevel";					//从首页向下的栏目级别
        public const string Attribute_Type = "type";                            //指定存储媒体的字段

		public const string Attribute_PlayUrl = "playurl";								//显示的视频地址
        public const string Attribute_ImageUrl = "imageurl";						    //显示的图片地址

        public const string Attribute_PlayBy = "playby";                            //指定播放器
        public const string Attribute_Stretching = "stretching";                    //拉伸
		public const string Attribute_Width = "width";							//宽度
		public const string Attribute_Height = "height";						//高度
        public const string Attribute_IsAutoPlay = "isautoplay";                    //是否自动播放
        public const string Attribute_IsDynamic = "isdynamic";                  //是否动态显示

        public const string PLAYBY_BRPlayer = "BRPlayer";
        public const string PLAYBY_FlowPlayer = "FlowPlayer";
        public const string PLAYBY_JWPlayer = "JWPlayer";

        public static List<string> AttributeValuesPlayBy
        {
            get
            {
                List<string> list = new List<string>();

                list.Add(PLAYBY_BRPlayer);
                list.Add(PLAYBY_FlowPlayer);
                list.Add(PLAYBY_JWPlayer);

                return list;
            }
        }

		public static ListDictionary AttributeList
		{
			get
			{
				ListDictionary attributes = new ListDictionary();
				attributes.Add(Attribute_ChannelIndex, "栏目索引");
				attributes.Add(Attribute_ChannelName, "栏目名称");
				attributes.Add(Attribute_Parent, "显示父栏目");
				attributes.Add(Attribute_UpLevel, "上级栏目的级别");
                attributes.Add(Attribute_TopLevel, "从首页向下的栏目级别");
                attributes.Add(Attribute_Type, "指定存储媒体的字段");
                attributes.Add(Attribute_PlayUrl, "视频地址");
                attributes.Add(Attribute_ImageUrl, "图片地址");
                attributes.Add(Attribute_PlayBy, "指定播放器");
                attributes.Add(Attribute_Stretching, "拉伸");
                attributes.Add(Attribute_Width, "宽度");
				attributes.Add(Attribute_Height, "高度");
                attributes.Add(Attribute_IsAutoPlay, "是否自动播放");
                attributes.Add(Attribute_IsDynamic, "是否动态显示");
				return attributes;
			}
		}

        public static string Parse(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo)
		{
			string parsedContent = string.Empty;
			try
			{
				IEnumerator ie = node.Attributes.GetEnumerator();
				bool isGetPicUrlFromAttribute = false;
				string channelIndex = string.Empty;
				string channelName = string.Empty;
				int upLevel = 0;
                int topLevel = -1;
                string type = BackgroundContentAttribute.VideoUrl;
				string playUrl = string.Empty;
                string stretching = string.Empty;
                string imageUrl = string.Empty;
                string playBy = string.Empty;
				int width = 450;
				int height = 350;
                bool isAutoPlay = true;
                bool isDynamic = false;
                NameValueCollection parameters = new NameValueCollection();

				while (ie.MoveNext())
				{
					XmlAttribute attr = (XmlAttribute)ie.Current;
					string attributeName = attr.Name.ToLower();
					if (attributeName.Equals(StlPlayer.Attribute_ChannelIndex))
					{
                        channelIndex = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
						if (!string.IsNullOrEmpty(channelIndex))
						{
							isGetPicUrlFromAttribute = true;
						}
					}
                    else if (attributeName.Equals(StlPlayer.Attribute_ChannelName))
                    {
                        channelName = StlEntityParser.ReplaceStlEntitiesForAttributeValue(attr.Value, pageInfo, contextInfo);
                        if (!string.IsNullOrEmpty(channelName))
                        {
                            isGetPicUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlPlayer.Attribute_Parent))
                    {
                        if (TranslateUtils.ToBool(attr.Value))
                        {
                            upLevel = 1;
                            isGetPicUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlPlayer.Attribute_UpLevel))
                    {
                        upLevel = TranslateUtils.ToInt(attr.Value);
                        if (upLevel > 0)
                        {
                            isGetPicUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlPlayer.Attribute_TopLevel))
                    {
                        topLevel = TranslateUtils.ToInt(attr.Value);
                        if (topLevel >= 0)
                        {
                            isGetPicUrlFromAttribute = true;
                        }
                    }
                    else if (attributeName.Equals(StlPlayer.Attribute_Type))
                    {
                        type = attr.Value;
                    }
                    else if (attributeName.Equals(StlPlayer.Attribute_PlayUrl))
                    {
                        playUrl = attr.Value;
                    }
                    else if (attributeName.Equals(StlPlayer.Attribute_ImageUrl))
                    {
                        imageUrl = attr.Value;
                    }
                    else if (attributeName.Equals(StlPlayer.Attribute_PlayBy))
                    {
                        playBy = attr.Value;
                    }
                    else if (attributeName.Equals(StlPlayer.Attribute_Stretching))
                    {
                        stretching = attr.Value;
                    }
                    else if (attributeName.Equals(StlPlayer.Attribute_Width))
                    {
                        try
                        {
                            width = int.Parse(attr.Value);
                        }
                        catch { }
                    }
                    else if (attributeName.Equals(StlPlayer.Attribute_Height))
                    {
                        try
                        {
                            height = int.Parse(attr.Value);
                        }
                        catch { }
                    }
                    else if (attributeName.Equals(StlPlayer.Attribute_IsAutoPlay))
                    {
                        isAutoPlay = TranslateUtils.ToBool(attr.Value, true);
                    }
                    else if (attributeName.Equals(StlPlayer.Attribute_IsDynamic))
                    {
                        isDynamic = TranslateUtils.ToBool(attr.Value);
                    }
                    else
                    {
                        parameters.Add(attr.Name, attr.Value);
                    }
				}

                if (isDynamic)
                {
                    parsedContent = StlDynamic.ParseDynamicElement(ElementName, stlElement, pageInfo, contextInfo);
                }
                else
                {
                    parsedContent = ParseImpl(stlElement, node, pageInfo, contextInfo, isGetPicUrlFromAttribute, channelIndex, channelName, upLevel, topLevel, playUrl, imageUrl, playBy, stretching, width, height, type, isAutoPlay, parameters);
                }
			}
            catch (Exception ex)
            {
                parsedContent = StlParserUtility.GetStlErrorMessage(ElementName, ex);
            }

			return parsedContent;
		}

        private static string ParseImpl(string stlElement, XmlNode node, PageInfo pageInfo, ContextInfo contextInfo, bool isGetPicUrlFromAttribute, string channelIndex, string channelName, int upLevel, int topLevel, string playUrl, string imageUrl, string playBy, string stretching, int width, int height, string type, bool isAutoPlay, NameValueCollection parameters)
        {
            string parsedContent = string.Empty;

            int contentID = 0;
            //判断是否图片地址由标签属性获得
            if (!isGetPicUrlFromAttribute)
            {
                contentID = contextInfo.ContentID;
            }

            if (string.IsNullOrEmpty(playUrl))
            {
                if (contentID != 0)//获取内容视频
                {
                    if (contextInfo.ContentInfo == null)
                    {
                        playUrl = BaiRongDataProvider.ContentDAO.GetValue(pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contentID, type);
                        if (string.IsNullOrEmpty(playUrl))
                        {
                            if (!StringUtils.EqualsIgnoreCase(type, BackgroundContentAttribute.VideoUrl))
                            {
                                playUrl = BaiRongDataProvider.ContentDAO.GetValue(pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contentID, BackgroundContentAttribute.VideoUrl);
                            }
                        }
                        if (string.IsNullOrEmpty(playUrl))
                        {
                            if (!StringUtils.EqualsIgnoreCase(type, BackgroundContentAttribute.FileUrl))
                            {
                                playUrl = BaiRongDataProvider.ContentDAO.GetValue(pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contentID, BackgroundContentAttribute.FileUrl);
                            }
                        }
                    }
                    else
                    {
                        playUrl = contextInfo.ContentInfo.GetExtendedAttribute(type);
                        if (string.IsNullOrEmpty(playUrl))
                        {
                            playUrl = contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.VideoUrl);
                        }
                        if (string.IsNullOrEmpty(playUrl))
                        {
                            playUrl = contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.FileUrl);
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(imageUrl))
            {
                if (contentID != 0)
                {
                    if (contextInfo.ContentInfo == null)
                    {
                        imageUrl = BaiRongDataProvider.ContentDAO.GetValue(pageInfo.PublishmentSystemInfo.AuxiliaryTableForContent, contentID, BackgroundContentAttribute.ImageUrl);
                    }
                    else
                    {
                        imageUrl = contextInfo.ContentInfo.GetExtendedAttribute(BackgroundContentAttribute.ImageUrl);
                    }
                }
            }
            if (string.IsNullOrEmpty(imageUrl))
            {
                int channelID = StlDataUtility.GetNodeIDByLevel(pageInfo.PublishmentSystemID, contextInfo.ChannelID, upLevel, topLevel);
                channelID = CreateCacheManager.NodeID.GetNodeIDByChannelIDOrChannelIndexOrChannelName(pageInfo.PublishmentSystemID, channelID, channelIndex, channelName);
                NodeInfo channel = NodeManager.GetNodeInfo(pageInfo.PublishmentSystemID, channelID);
                imageUrl = channel.ImageUrl;
            }

            if (!string.IsNullOrEmpty(playUrl))
            {
                string extension = PathUtils.GetExtension(playUrl);
                if (EFileSystemTypeUtils.IsFlash(extension))
                {
                    parsedContent = StlFlash.Parse(stlElement, node, pageInfo, contextInfo);
                }
                else if (EFileSystemTypeUtils.IsImage(extension))
                {
                    parsedContent = StlImage.Parse(stlElement, node, pageInfo, contextInfo);
                }
                else
                {
                    int uniqueID = pageInfo.UniqueID;
                    playUrl = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, playUrl);
                    imageUrl = PageUtility.ParseNavigationUrl(pageInfo.PublishmentSystemInfo, imageUrl);

                    EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(extension);
                    if (fileType == EFileSystemType.Avi)
                    {
                        parsedContent = string.Format(@"
<object id=""palyer_{0}"" width=""{1}"" height=""{2}"" border=""0"" classid=""clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA"">
<param name=""ShowDisplay"" value=""0"">
<param name=""ShowControls"" value=""1"">
<param name=""AutoStart"" value=""{3}"">
<param name=""AutoRewind"" value=""0"">
<param name=""PlayCount"" value=""0"">
<param name=""Appearance"" value=""0"">
<param name=""BorderStyle"" value=""0"">
<param name=""MovieWindowHeight"" value=""240"">
<param name=""MovieWindowWidth"" value=""320"">
<param name=""FileName"" value=""{4}"">
<embed width=""{1}"" height=""{2}"" border=""0"" showdisplay=""0"" showcontrols=""1"" autostart=""{3}"" autorewind=""0"" playcount=""0"" moviewindowheight=""240"" moviewindowwidth=""320"" filename=""{4}"" src=""{4}"">
</embed>
</object>
", uniqueID, width, height, (isAutoPlay ? "1" : "0"), playUrl);
                    }
                    else if (fileType == EFileSystemType.Mpg)
                    {
                        parsedContent = string.Format(@"
<object classid=""clsid:05589FA1-C356-11CE-BF01-00AA0055595A"" id=""palyer_{0}"" width=""{1}"" height=""{2}"">
<param name=""Appearance"" value=""0"">
<param name=""AutoStart"" value=""{3}"">
<param name=""AllowChangeDisplayMode"" value=""-1"">
<param name=""AllowHideDisplay"" value=""0"">
<param name=""AllowHideControls"" value=""-1"">
<param name=""AutoRewind"" value=""-1"">
<param name=""Balance"" value=""0"">
<param name=""CurrentPosition"" value=""0"">
<param name=""DisplayBackColor"" value=""0"">
<param name=""DisplayForeColor"" value=""16777215"">
<param name=""DisplayMode"" value=""0"">
<param name=""Enabled"" value=""-1"">
<param name=""EnableContextMenu"" value=""-1"">
<param name=""EnablePositionControls"" value=""-1"">
<param name=""EnableSelectionControls"" value=""0"">
<param name=""EnableTracker"" value=""-1"">
<param name=""Filename"" value=""{4}"" valuetype=""ref"">
<param name=""FullScreenMode"" value=""0"">
<param name=""MovieWindowSize"" value=""0"">
<param name=""PlayCount"" value=""1"">
<param name=""Rate"" value=""1"">
<param name=""SelectionStart"" value=""-1"">
<param name=""SelectionEnd"" value=""-1"">
<param name=""ShowControls"" value=""-1"">
<param name=""ShowDisplay"" value=""-1"">
<param name=""ShowPositionControls"" value=""0"">
<param name=""ShowTracker"" value=""-1"">
<param name=""Volume"" value=""-480"">
</object>
", uniqueID, width, height, (isAutoPlay ? "true" : "false"), playUrl);
                    }
                    else if (fileType == EFileSystemType.Mpg)
                    {
                        parsedContent = string.Format(@"
<OBJECT id=""palyer_{0}"" classid=""clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA"" width=""{1}"" height=""{2}"">
<param name=""_ExtentX"" value=""6350"">
<param name=""_ExtentY"" value=""4763"">
<param name=""AUTOSTART"" value=""{3}"">
<param name=""SHUFFLE"" value=""0"">
<param name=""PREFETCH"" value=""0"">
<param name=""NOLABELS"" value=""-1"">
<param name=""SRC"" value=""{4}"">
<param name=""CONTROLS"" value=""ImageWindow"">
<param name=""CONSOLE"" value=""console1"">
<param name=""LOOP"" value=""0"">
<param name=""NUMLOOP"" value=""0"">
<param name=""CENTER"" value=""0"">
<param name=""MAINTAINASPECT"" value=""0"">
<param name=""BACKGROUNDCOLOR"" value=""#000000"">
<embed src=""{4}"" type=""audio/x-pn-realaudio-plugin"" console=""Console1"" controls=""ImageWindow"" width=""{1}"" height=""{2}"" autostart=""{3}""></OBJECT>
", uniqueID, width, height, (isAutoPlay ? "true" : "false"), playUrl);
                    }
                    else if (fileType == EFileSystemType.Rm)
                    {
                        parsedContent = string.Format(@"
<OBJECT id=""palyer_{0}"" CLASSID=""clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA"" WIDTH=""{1}"" HEIGHT=""{2}"">
<param name=""_ExtentX"" value=""9313"">
<param name=""_ExtentY"" value=""7620"">
<param name=""AUTOSTART"" value=""{3}"">
<param name=""SHUFFLE"" value=""0"">
<param name=""PREFETCH"" value=""0"">
<param name=""NOLABELS"" value=""0"">
<param name=""SRC"" value=""{4}"">
<param name=""CONTROLS"" value=""ImageWindow"">
<param name=""CONSOLE"" value=""Clip1"">
<param name=""LOOP"" value=""0"">
<param name=""NUMLOOP"" value=""0"">
<param name=""CENTER"" value=""0"">
<param name=""MAINTAINASPECT"" value=""0"">
<param name=""BACKGROUNDCOLOR"" value=""#000000"">
<embed SRC type=""audio/x-pn-realaudio-plugin"" CONSOLE=""Clip1"" CONTROLS=""ImageWindow"" WIDTH=""{1}"" HEIGHT=""{2}"" AUTOSTART=""{3}"">
</OBJECT>
", uniqueID, width, height, (isAutoPlay ? "true" : "false"), playUrl);
                    }
                    else if (fileType == EFileSystemType.Wmv)
                    {
                        parsedContent = string.Format(@"
<object id=""palyer_{0}"" WIDTH=""{1}"" HEIGHT=""{2}"" classid=""CLSID:22d6f312-b0f6-11d0-94ab-0080c74c7e95"" codebase=""http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=6,4,5,715"" standby=""Loading Microsoft Windows Media Player components..."" type=""application/x-oleobject"" align=""right"" hspace=""5"">
<param name=""AutoRewind"" value=""1"">
<param name=""ShowControls"" value=""1"">
<param name=""ShowPositionControls"" value=""0"">
<param name=""ShowAudioControls"" value=""1"">
<param name=""ShowTracker"" value=""0"">
<param name=""ShowDisplay"" value=""0"">
<param name=""ShowStatusBar"" value=""0"">
<param name=""ShowGotoBar"" value=""0"">
<param name=""ShowCaptioning"" value=""0"">
<param name=""AutoStart"" value=""{3}"">
<param name=""FileName"" value=""{4}"">
<param name=""Volume"" value=""-2500"">
<param name=""AnimationAtStart"" value=""0"">
<param name=""TransparentAtStart"" value=""0"">
<param name=""AllowChangeDisplaySize"" value=""0"">
<param name=""AllowScan"" value=""0"">
<param name=""EnableContextMenu"" value=""0"">
<param name=""ClickToPlay"" value=""0"">
</object>
", uniqueID, width, height, (isAutoPlay ? "1" : "0"), playUrl);
                    }
                    else if (fileType == EFileSystemType.Wma)
                    {
                        parsedContent = string.Format(@"
<object classid=""clsid:22D6F312-B0F6-11D0-94AB-0080C74C7E95"" id=""palyer_{0}"">
<param name=""Filename"" value=""{1}"">
<param name=""PlayCount"" value=""1"">
<param name=""AutoStart"" value=""{2}"">
<param name=""ClickToPlay"" value=""1"">
<param name=""DisplaySize"" value=""0"">
<param name=""EnableFullScreen Controls"" value=""1"">
<param name=""ShowAudio Controls"" value=""1"">
<param name=""EnableContext Menu"" value=""1"">
<param name=""ShowDisplay"" value=""1"">
</object>
", uniqueID, playUrl, (isAutoPlay ? "1" : "0"));
                    }
                    else if (fileType == EFileSystemType.Rm || fileType == EFileSystemType.Rmb || fileType == EFileSystemType.Rmvb)
                    {
                        if (string.IsNullOrEmpty(parameters["ShowDisplay"]))
                        {
                            parameters["ShowDisplay"] = "0";
                        }
                        if (string.IsNullOrEmpty(parameters["ShowControls"]))
                        {
                            parameters["ShowControls"] = "1";
                        }
                        parameters["AutoStart"] = isAutoPlay ? "1" : "0";
                        if (string.IsNullOrEmpty(parameters["AutoRewind"]))
                        {
                            parameters["AutoRewind"] = "0";
                        }
                        if (string.IsNullOrEmpty(parameters["PlayCount"]))
                        {
                            parameters["PlayCount"] = "0";
                        }
                        if (string.IsNullOrEmpty(parameters["Appearance"]))
                        {
                            parameters["Appearance"] = "0";
                        }
                        if (string.IsNullOrEmpty(parameters["BorderStyle"]))
                        {
                            parameters["BorderStyle"] = "0";
                        }
                        if (string.IsNullOrEmpty(parameters["Controls"]))
                        {
                            parameters["ImageWindow"] = "0";
                        }
                        parameters["moviewindowheight"] = height.ToString();
                        parameters["moviewindowwidth"] = width.ToString();
                        parameters["filename"] = playUrl;
                        parameters["src"] = playUrl;

                        StringBuilder paramBuilder = new StringBuilder();
                        StringBuilder embedBuilder = new StringBuilder();
                        foreach (string key in parameters.Keys)
                        {
                            paramBuilder.AppendFormat(@"<param name=""{0}"" value=""{1}"">", key, parameters[key]).Append(StringUtils.Constants.ReturnAndNewline);
                            embedBuilder.AppendFormat(@" {0}=""{1}""", key, parameters[key]);
                        }

                        parsedContent = string.Format(@"
<object id=""video_{0}"" width=""{1}"" height=""{2}"" border=""0"" classid=""clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA"">
{3}
<embed{4}>
</embed>
</object>
", uniqueID, width, height, paramBuilder, embedBuilder);
                    }
                    else
                    {
                        if (StringUtils.EqualsIgnoreCase(playBy, PLAYBY_FlowPlayer))
                        {
                            string ajaxElementID = StlParserUtility.GetAjaxDivID(pageInfo.UniqueID);
                            pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Ac_FlowPlayer);

                            parsedContent = string.Format(@"
<a href=""{0}"" style=""display:block;width:{1}px;height:{2}px;"" id=""player_{3}""></a>
<script language=""javascript"">
    flowplayer(""player_{3}"", ""{4}"", {{
        clip:  {{
            autoPlay: {5}
        }}
    }});
</script>
", playUrl, width, height, ajaxElementID, PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.FlowPlayer.Swf), isAutoPlay.ToString().ToLower());
                        }
                        else if (StringUtils.EqualsIgnoreCase(playBy, PLAYBY_JWPlayer))
                        {
                            pageInfo.AddPageScriptsIfNotExists(PageInfo.Js_Ac_JWPlayer6);
                            string ajaxElementID = StlParserUtility.GetAjaxDivID(pageInfo.UniqueID);
                            parsedContent = string.Format(@"
<div id='{0}'></div>
<script type='text/javascript'>
	jwplayer('{0}').setup({{
        autostart: {1},
		file: ""{2}"",
		width: ""{3}"",
		height: ""{4}"",
		image: ""{5}""
	}});
</script>
", ajaxElementID, isAutoPlay.ToString().ToLower(), playUrl, width, height, imageUrl);
                        }
                        else
                        {
                            string additional = string.Empty;
                            if (!string.IsNullOrEmpty(stretching))
                            {
                                additional = "&stretching=" + stretching;
                            }
                            parsedContent = string.Format(@"
<embed src=""{0}"" allowfullscreen=""true"" flashvars=""controlbar=over{1}&autostart={2}&image={3}&file={4}"" width=""{5}"" height=""{6}""/>
", PageUtility.GetSiteFilesUrl(pageInfo.PublishmentSystemInfo, SiteFiles.BRPlayer.Swf), additional, isAutoPlay.ToString().ToLower(), imageUrl, playUrl, width, height);
                        }
                    }
                }
            }

            return parsedContent;
        }
	}
}
