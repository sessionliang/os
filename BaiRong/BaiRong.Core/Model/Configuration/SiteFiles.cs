using System;
using BaiRong.Core;

namespace BaiRong.Core
{
	public class SiteFiles
	{
		#region Directory
		public class Directory
		{
            public const string Users = "usersfiles";
            public const string BaiRong = "bairong";
            public const string Configuration = "configuration";
            public const string Module = "module";
            public const string Services = "services";
			public const string Skins = "bairong/skins";
            public const string Icons = "bairong/icons";
            public const string Images = "bairong/images";
			public const string FileSystem = "bairong/icons/filesystem";
            public const string Scripts = "bairong/scripts";
            public const string TemporaryFiles = "temporaryFiles";
		}
		#endregion

        public static string GetIconUrl(string iconName)
        {
            return PageUtils.Combine(SiteFiles.Directory.Icons, iconName);
        }

        #region Default
        public class Default
        {
            public const string UrlRedirectPage = "bairong/Default/UrlRedirect/default.aspx";
            public const string WebConfig = "bairong/Default/web.config";
            public const string IconFile = "bairong/Icons/usericon.gif";
            public const string AjaxProxy = "bairong/Default/ajaxProxy.aspx";
            public const string Proxy = "bairong/Default/proxy.aspx";
            public const string AccessMDB = "bairong/Default/access.mdb";
        }
        #endregion

        #region Arrow
        public class Arrow
        {
            public const string First = "bairong/icons/arrow/arrow_first.gif";
            public const string FirstDisabled = "bairong/icons/arrow/arrow_first_d.gif";
            public const string Last = "bairong/icons/arrow/arrow_last.gif";
            public const string LastDisabled = "bairong/icons/arrow/arrow_last_d.gif";
            public const string Next = "bairong/icons/arrow/arrow_next.gif";
            public const string NextDisabled = "bairong/icons/arrow/arrow_next_d.gif";
            public const string Previous = "bairong/icons/arrow/arrow_prev.gif";
            public const string PreviousDisabled = "bairong/icons/arrow/arrow_prev_d.gif";
        }
        #endregion

		#region WzToolTip
		public class WzToolTip
		{
			public const string Js = "bairong/Scripts/wz_tooltip.v3.33.js";
		}
		#endregion

		#region BaiRongFlash
		public class BaiRongFlash
		{
			public const string Js = "bairong/scripts/bairongflash.js";
		}
		#endregion

        #region SWFObject
        public class SWFObject
        {
            public const string Js = "bairong/scripts/swfobject.js";
        }
        #endregion

        #region BRPlayer
        public class BRPlayer
        {
            public const string Swf = "bairong/flashes/brplayer/player.swf";
        }
        #endregion

        #region JWPlayer6
        public class JWPlayer6
        {
            public const string Js = "bairong/flashes/jwplayer6/jwplayer.js";
        }
        #endregion

        #region FlowPlayer
        public class FlowPlayer
        {
            public const string Js = "bairong/flashes/flowplayer/flowplayer-3.2.12.min.js";
            public const string Swf = "bairong/flashes/flowplayer/flowplayer-3.2.16.swf";
        }
        #endregion

        #region MediaElement
        public class MediaElement
        {
            public const string Js = "bairong/flashes/mediaelement/mediaelement-and-player.min.js";
            public const string Css = "bairong/flashes/mediaelement/mediaelementplayer.min.css";
            public const string Swf = "bairong/flashes/mediaelement/flashmediaelement.swf";
        }
        #endregion

        #region AudioJs
        public class AudioJs
        {
            public const string Js = "bairong/flashes/audiojs/audio.min.js";
        }
        #endregion

        #region VideoJs
        public class VideoJs
        {
            public const string Css = "bairong/flashes/videojs/video-js.min.css";
            public const string Js = "bairong/flashes/videojs/video.min.js";
        }
        #endregion

        public class STL
        {
            public const string Js_PageScript = "bairong/scripts/stl/pagescript.js";
            public const string Js_PageScript_Charset = "utf-8";
            public const string Js_UserScript = "bairong/scripts/stl/userscript.js";
            public const string Js_UserScript_Charset = "utf-8";
        }

        public class Static
        {
            public const string Js_Static_AdFloating = "bairong/scripts/static/adFloating.js";
            public const string Js_Static_AdFloating_Charset = "utf-8";
        }

        #region DaD

        public class DaD
        {
            public const string Directory = "bairong/dad";

            public class Components
            {
                public const string Directory = "bairong/dad/components";
            }

            public class Templates
            {
                public const string Directory = "bairong/dad/templates";
            }
        }

        #endregion

        #region JQuery
        public class JQuery
        {
            public const string Js = "bairong/jquery/jquery-1.9.1.min.js";
            public const string Js_1_11_0 = "bairong/jquery/jquery-1.11.0.min.js";

            public class FancyBox
            {
                public const string Js = "bairong/jquery/fancybox/jquery.fancybox-1.3.4.pack.js";
                public const string Css = "bairong/jquery/fancybox/jquery.fancybox-1.3.4.css";
            }

            public class AjaxUpload
            {
                public const string Js = "bairong/jquery/ajaxUpload.js";
            }

            public class QueryString
            {
                public const string Js = "bairong/jquery/queryString.js";
            }

            public class JQueryForm
            {
                public const string Js = "bairong/jquery/jquery.form.js";
            }

            public class ShowLoading
            {
                public const string Js = "bairong/jquery/showLoading/js/jquery.showLoading.min.js";
                public const string Css = "bairong/jquery/showLoading/css/showLoading.css";
                public const string Charset = "utf-8";
            }

            public class JTemplates
            {
                public const string Js = "bairong/jquery/jquery-jtemplates.js";
                public const string Charset = "utf-8";
            }

            public class Validate
            {
                public const string Js = "bairong/jquery/validate.js";
                public const string Charset = "utf-8";
            }

            #region Bootstrap
            public class Bootstrap
            {
                public const string Css = "bairong/jquery/bootstrap/css/bootstrap.min.css";
                public const string Js = "bairong/jquery/bootstrap/js/bootstrap.min.js";
                public const string Directory_IE = "bairong/jquery/bootstrap/ie";
            }
            #endregion

            #region AngularJS
            public class AngularJS
            {
                public const string Js = "bairong/jquery/angular.min.js";
            }
            #endregion

            #region Highcharts
            public class Highcharts
            {
                public const string HighchartsJs = "bairong/scripts/highcharts/js/highcharts.js";
                public const string ExportingJs = "bairong/scripts/highcharts/js/modules/exporting.js";
            }
            #endregion

            #region Toastr
            public class Toastr
            {
                public const string Js = "bairong/jquery/toastr/toastr.min.js";
                public const string Css = "bairong/jquery/toastr/toastr.min.css";
            }
            #endregion

            #region ArtTemplate
            public class ArtTemplate
            {
                public const string JsJson2 = "bairong/jquery/artTemplate/json2.js";
                public const string JsTemplate = "bairong/jquery/artTemplate/template.js";
                public const string JsTemplateSimple = "bairong/jquery/artTemplate/template-simple.js";
                public const string JsUtilService = "bairong/jquery/artTemplate/utilService.js";
            }
            #endregion

            #region Layer
            public class Layer
            {
                public const string Js = "bairong/jquery/layer/layer.min.js";
            }
            #endregion

            #region Html5shiv
            public class Html5shiv
            {
                public const string Js = "bairong/jquery/html5shiv/html5shiv.js";
            }
            #endregion
        }
        #endregion

        #region GlobalScript
        public class GlobalScript
        {
            public const string Js = "bairong/scripts/global.v1.3.5.js";
            public const string Charset = "gb2312";
        }
        #endregion

        #region JSON
        public class JSON
		{
            public const string Js = "bairong/scripts/json.v1.1.js";
		}
		#endregion

        #region Validate
        public class Validate
        {
            public const string Js = "bairong/scripts/independent/validate.js";
            public const string Charset = "utf-8";
        }
        #endregion

        #region DateString
        public class DateString
        {
            public const string Js = "bairong/scripts/datestring.js";
            public const string Charset = "utf-8";
        }
        #endregion

        #region Lightbox
        public class Lightbox
        {
            public const string Js = "bairong/scripts/lightbox/lightbox.js";
            public const string Css = "bairong/scripts/lightbox/lightbox.css";
        }
        #endregion

		#region Tabstrip
		public class Tabstrip
		{
			public const string Js = "bairong/scripts/tabstrip.js";
			public const string Css = "bairong/styles/tabstrip.css";
		}
		#endregion

        #region Tracker
        public class Tracker
        {
            public const string Js = "bairong/scripts/independent/tracker.js";
        }
        #endregion

		#region Global
		public class Global
		{
			public const string Css = "bairong/styles/global.v1.0.css";
		}
		#endregion

		#region Flashes
		public class Flashes
		{
            public const string Vcastr = "bairong/flashes/vcastr3.swf";
            public const string FocusViewer = "bairong/flashes/focusviewer.swf";
            public const string Bcastr = "bairong/flashes/bcastr31.swf";
            public const string Ali = "bairong/flashes/focusali.swf";
		}
		#endregion

		#region MM_Menu
		public class MM_Menu
		{
			public const string Js = "bairong/scripts/mm_menu.js";
		}
		#endregion

        #region DatePicker
        public class DatePicker
        {
            public const string Js = "bairong/scripts/datepicker/wdatepicker.js";

            public const string OnFocus = "WdatePicker({isShowClear:false,readOnly:true,dateFmt:'yyyy-MM-dd HH:mm:ss'});";
            public const string FormatString = "yyyy-MM-dd HH:mm:ss";

            public const string OnFocusDateOnly = "WdatePicker({isShowClear:false,readOnly:true,dateFmt:'yyyy-MM-dd'});";
            public const string FormatStringDateOnly = "yyyy-MM-dd";
        }
        #endregion

        #region Slide
        public class Slide
        {
            public const string FullScreenSwf = "bairong/scripts/slide/fullscreen.swf";
            public const string Js = "bairong/scripts/slide/script.js";
            public const string Css = "bairong/scripts/slide/style.css";
            public const string Template = "bairong/scripts/slide/template.html";
        }
        #endregion

        #region Print
        public class Print
        {
            public const string Js_Utf8 = "bairong/scripts/print_uft8.js";
            public const string Js_Gb2312 = "bairong/scripts/print.js";
            public const string IconUrl = "bairong/Icons/print";
        }
        #endregion

        #region tw_cn
        public class tw_cn
        {
            public const string Js = "bairong/scripts/independent/tw_cn.js";
            public const string Charset = "utf-8";
        }
        #endregion
	}
}
