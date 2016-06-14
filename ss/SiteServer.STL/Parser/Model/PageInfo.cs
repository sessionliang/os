using System.Collections;
using System.Text;
using SiteServer.CMS.Model;
using BaiRong.Core;

using BaiRong.Model;
using System.Collections.Generic;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.Model
{
    public class PageInfo
    {
        readonly TemplateInfo templateInfo;
        private int publishmentSystemID;
        private int pageNodeID;
        private int pageContentID;
        private readonly SortedDictionary<string, string> pageHeadScripts;
        private readonly SortedDictionary<string, string> pageAfterBodyScripts;
        private readonly SortedDictionary<string, string> pageBeforeBodyScripts;
        private readonly SortedDictionary<string, string> pageEndScripts;
        private PublishmentSystemInfo publishmentSystemInfo;
        private EVisualType visualType;
        private int uniqueID;
        private bool isLegality;

        private readonly Stack channelItems;
        private readonly Stack contentItems;
        private readonly Stack commentItems;
        private readonly Stack inputItems;
        private readonly Stack websiteMessageItems;
        private readonly Stack sqlItems;
        private readonly Stack siteItems;
        private readonly Stack photoItems;
        private readonly Stack teleplayItems;
        private readonly Stack eachItems;
        private readonly Stack specItems;
        private readonly Stack filterItems;

        public class JQuery
        {
            public const string A_JQuery = "A_JQuery";                      //Jquery
            public const string A_JQuery_1_11_0 = "A_JQuery_1_11_0";        //Jquery
            public const string B_AjaxUpload = "B_AjaxUpload";              //AjaxUpload
            public const string B_QueryString = "B_QueryString";            //QueryString
            public const string B_JQueryForm = "B_JQueryForm";              //JQueryForm
            public const string B_ShowLoading = "B_ShowLoading";              //ShowLoading
            public const string B_JTemplates = "B_JTemplates";              //JTemplates
            public const string B_Validate = "B_Validate";                  //Validate
            public const string B_Bootstrap = "B_Bootstrap";                  //Bootstrap
        }

        public const string Js_Ac_SWFObject = "Js_Ac_SWFObject";                //SWFObject

        public const string Js_Ac_JWPlayer6 = "Js_Ac_JWPlayer6";                //JWPlayer6
        public const string Js_Ac_FlowPlayer = "Js_Ac_FlowPlayer";              //flowPlayer
        public const string Js_Ac_MediaElement = "Js_Ac_MediaElement";          //mediaelement
        public const string Js_Ac_AudioJs = "Js_Ac_AudioJs";                    //audio.js
        public const string Js_Ac_VideoJs = "Js_Ac_VideoJs";                    //video.js

        public const string Js_Ac_MenuScripts = "Js_Ac_MenuScripts";            //下拉菜单
        public const string Js_Ad_StlCountHits = "Js_Ad_StlCountHits";          //统计访问量
        public const string Js_Ad_AddTracker = "Js_Ad_AddTracker";              //添加流量统计代码
        public const string Js_Aj_JSON = "Js_Aj_JSON";                //JSON
        public const string Js_Ae_StlZoom = "Js_Ae_StlZoom";                    //文字缩放
        public const string Js_Af_StlPrinter = "Js_Af_StlPrinter";              //打印
        public const string Js_Ag_StlTreeNotAjax = "Js_Ag_StlTreeNotAjax";                    //树状导航
        public const string Js_Ag_StlTreeAjax = "Js_Ag_StlTreeAjax";                    //树状导航
        public const string Js_Ah_Translate = "Js_Ah_Translate";                //繁体/简体转换

        public const string Js_Page_OpenWindow = "Js_Page_OpenWindow";
        public const string Js_User_Script = "Js_User_Script";
        public const string Js_Inner_Calendar = "Js_Inner_Calendar";

        public const string Js_Static_AdFloating = "Js_Static_AdFloating";      //漂浮广告

        public const string Js_Ad_IntelligentPushCount = "Js_Ad_IntelligentPushCount";          //会员浏览内容统计栏目

        public class JsServiceComponents
        {
            public const string A_Platform_BASIC = "A_Platform_BASIC";
            public const string A_Platform_BASIC_BUT_JQUERY = "A_Platform_BASIC_BUT_JQUERY";
            public const string A_Platform_USER = "A_Platform_USER";
            public const string A_Platform_LOGIN = "A_Platform_LOGIN";
            public const string A_Platform_LOGOUT = "A_Platform_LOGOUT";
            public const string A_Platform_REGISTER = "A_Platform_REGISTER";
            public const string A_Platform_FORGET = "A_Platform_FORGET";

            public const string B_CMS_COMMENT = "B_CMS_COMMENT";

            public const string C_B2C_BASIC = "C_B2C_BASIC";
            public const string C_B2C_FILTER = "C_B2C_FILTER";
            public const string C_B2C_SPEC = "C_B2C_SPEC";
            public const string C_B2C_ORDER = "C_B2C_ORDER";
            public const string C_B2C_ORDER_CSS = "C_B2C_ORDER_CSS";
            public const string C_B2C_ORDER_SUCCESS = "C_B2C_ORDER_SUCCESS";
            public const string C_B2C_ORDER_RETURN = "C_B2C_ORDER_RETURN";
            public const string C_B2C_ORDER_LIST = "C_B2C_ORDER_LIST";

            public const string D_Home_CHANGEPWD = "D_Home_CHANGEPWD";
            public const string D_Ext_GUESSES = "D_Ext_GUESSES";
            public const string D_CONSULTATION = "D_CONSULTATION";//购物咨询
            public const string D_ORDER_COMMENT = "D_ORDER_COMMENT";//订单评价
            public const string D_PAGE_DATA_UTILS = "D_PAGE_DATA_UTILS";//分页辅助js

            #region by 20160301 sofuny 评价管理

            public const string B_CMS_EVALUATION = "B_CMS_EVALUATION";
            public const string B_CMS_TTIALAPPLY = "B_CMS_TTIALAPPLY";
            public const string B_CMS_SURVEY = "B_CMS_SURVEY";
            public const string B_CMS_COMPARE = "B_CMS_COMPARE";
            #endregion
        }

        public PageInfo(TemplateInfo templateInfo, int publishmentSystemID, int pageNodeID, int pageContentID, PublishmentSystemInfo publishmentSystemInfo, EVisualType visualType)
        {
            this.templateInfo = templateInfo;
            this.publishmentSystemID = publishmentSystemID;
            this.pageNodeID = pageNodeID;
            this.pageContentID = pageContentID;
            this.pageAfterBodyScripts = new SortedDictionary<string, string>();
            this.pageBeforeBodyScripts = new SortedDictionary<string, string>();
            this.pageEndScripts = new SortedDictionary<string, string>();
            this.pageHeadScripts = new SortedDictionary<string, string>();
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.visualType = visualType;
            this.uniqueID = 1;
            this.isLegality = LicenseManager.Instance.IsLegality;

            this.channelItems = new Stack(5);
            this.contentItems = new Stack(5);
            this.commentItems = new Stack(5);
            this.inputItems = new Stack(5);
            this.websiteMessageItems = new Stack(5);
            this.sqlItems = new Stack(5);
            this.siteItems = new Stack(5);
            this.photoItems = new Stack(5);
            this.teleplayItems = new Stack(5);
            this.eachItems = new Stack(5);
            this.specItems = new Stack(5);
            this.filterItems = new Stack(5);
        }

        public TemplateInfo TemplateInfo
        {
            get { return templateInfo; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
        }

        public int PageNodeID
        {
            get { return pageNodeID; }
        }

        public int PageContentID
        {
            get { return pageContentID; }
        }

        public void ChangeSite(PublishmentSystemInfo publishmentSystemInfo, int pageNodeID, int pageContentID, ContextInfo contextInfo, EVisualType visualType)
        {
            this.publishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.pageNodeID = pageNodeID;
            this.pageContentID = pageContentID;
            this.visualType = visualType;

            contextInfo.PublishmentSystemInfo = publishmentSystemInfo;
            contextInfo.ChannelID = pageNodeID;
            contextInfo.ContentID = pageContentID;
        }

        #region 页面脚本

        public static string GetJsCode(string pageJsName, PublishmentSystemInfo publishmentSystemInfo)
        {
            string retval = string.Empty;

            if (pageJsName == PageInfo.JQuery.A_JQuery)
            {
                if (publishmentSystemInfo.Additional.IsCreateWithJQuery)
                {
                    retval = string.Format("<script type=\"text/javascript\">!window.jQuery&&document.write('<script src=\"{0}\" language=\"javascript\"></'+'script>');</script>", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.JQuery.Js));
                }
            }
            if (pageJsName == PageInfo.JQuery.A_JQuery_1_11_0)
            {
                if (publishmentSystemInfo.Additional.IsCreateWithJQuery)
                {
                    retval = string.Format("<script type=\"text/javascript\">!window.jQuery&&document.write('<script src=\"{0}\" language=\"javascript\"></'+'script>');</script>", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.JQuery.Js_1_11_0));
                }
            }
            else if (pageJsName == PageInfo.JQuery.B_AjaxUpload)
            {
                retval = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.JQuery.AjaxUpload.Js));
            }
            else if (pageJsName == PageInfo.JQuery.B_QueryString)
            {
                retval = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.JQuery.QueryString.Js));
            }
            else if (pageJsName == PageInfo.JQuery.B_JQueryForm)
            {
                retval = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.JQuery.JQueryForm.Js));
            }
            else if (pageJsName == PageInfo.JQuery.B_ShowLoading)
            {
                retval = string.Format(@"<link href=""{0}"" rel=""stylesheet"" media=""screen"" /><script type=""text/javascript"" charset=""{1}"" src=""{2}""></script>", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.JQuery.ShowLoading.Css), SiteFiles.JQuery.ShowLoading.Charset, PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.JQuery.ShowLoading.Js));
            }
            else if (pageJsName == PageInfo.JQuery.B_JTemplates)
            {
                retval = string.Format(@"<script type=""text/javascript"" charset=""{0}"" src=""{1}""></script>", SiteFiles.JQuery.JTemplates.Charset, PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.JQuery.JTemplates.Js));
            }
            else if (pageJsName == PageInfo.JQuery.B_Validate)
            {
                retval = string.Format(@"<script type=""text/javascript"" charset=""{0}"" src=""{1}""></script>", SiteFiles.JQuery.Validate.Charset, PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.JQuery.Validate.Js));
            }
            else if (pageJsName == PageInfo.JQuery.B_Bootstrap)
            {
                retval = string.Format(@"
<link rel=""stylesheet"" type=""text/css"" href=""{0}"">
<script language=""javascript"" src=""{1}""></script>
<!--[if lte IE 6]>
<link rel=""stylesheet"" type=""text/css"" href=""{2}/bootstrap-ie6.min.css"">
<script type=""text/javascript"" src=""{2}/bootstrap-ie.js""></script>
<![endif]-->
<!--[if lte IE 7]>
<link rel=""stylesheet"" type=""text/css"" href=""{2}/ie.css"">
<![endif]-->
<script type=""text/javascript"">
(function ($) {{
    $(document).ready(function() {{
        if ($.isFunction($.bootstrapIE6)) $.bootstrapIE6($(document));
    }});
}})(jQuery);
</script>
", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.JQuery.Bootstrap.Css), PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.JQuery.Bootstrap.Js), PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.JQuery.Bootstrap.Directory_IE));
            }
            else if (pageJsName == PageInfo.Js_Ac_SWFObject)
            {
                retval = string.Format(@"<script type=""text/javascript"" src=""{0}""></script>", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.SWFObject.Js));
            }
            else if (pageJsName == PageInfo.Js_Ac_JWPlayer6)
            {
                retval = string.Format(@"<script type=""text/javascript"" src=""{0}""></script><script type=""text/javascript"">jwplayer.key=""ABCDEFGHIJKLMOPQ"";</script>", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.JWPlayer6.Js));
            }
            else if (pageJsName == PageInfo.Js_Ac_FlowPlayer)
            {
                retval = string.Format(@"<script type=""text/javascript"" src=""{0}""></script>", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.FlowPlayer.Js));
            }
            else if (pageJsName == PageInfo.Js_Ac_MediaElement)
            {
                retval = string.Format(@"<script type=""text/javascript"" src=""{0}""></script><link rel=""stylesheet"" href=""{1}"" />", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.MediaElement.Js), PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.MediaElement.Css));
            }
            else if (pageJsName == PageInfo.Js_Ac_AudioJs)
            {
                retval = string.Format(@"<script type=""text/javascript"" src=""{0}""></script>
<script type='text/javascript'>
audiojs.events.ready(function() {{
    audiojs.createAll();
}});
</script>
", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.AudioJs.Js));
            }
            else if (pageJsName == PageInfo.Js_Ac_VideoJs)
            {
                retval = string.Format(@"
<link href=""{0}"" rel=""stylesheet"">
<script type=""text/javascript"" src=""{1}""></script>
", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.VideoJs.Css), PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.VideoJs.Js));
            }
            else if (pageJsName == PageInfo.Js_Aj_JSON)
            {
                retval = string.Format(@"<script type=""text/javascript"" src=""{0}""></script>", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.JSON.Js));
            }
            else if (pageJsName == PageInfo.Js_Page_OpenWindow)
            {
                retval = @"
<div id=""stl_wnd_board"" style=""position:absolute;top:100px;left:0px;width:100%;z-index:65531;height:100%;display:none"" align=""center"">
    <div id=""stl_wnd_div"" style=""display:none; width:400px; height:330px; padding:0; margin:0px;"" align=""center"">
    <iframe id=""stl_wnd_frame"" frameborder=""0"" scrolling=""auto"" width=""100%"" height=""100%"" src=""""></iframe>
    </div>
</div>
<script>
function stlCloseWindow()
{document.getElementById('stl_wnd_div').style.display='none';document.getElementById('stl_wnd_board').style.display='none';}
function stlOpenWindow(pageUrl,width,height)
{var stl_wnd=document.getElementById('stl_wnd_div');var stl_board=document.getElementById('stl_wnd_board');var wnd_frame=document.getElementById('stl_wnd_frame');if(stl_wnd){stl_wnd.style.width=width+'px';stl_wnd.style.height=height+'px';stl_board.style.display='block';stl_board.style.top=(100+document.documentElement.scrollTop)+'px';stl_wnd.style.visible='hidden';stl_wnd.style.display='block';var url;if(pageUrl.indexOf('?')==-1){url=pageUrl+'?_r='+Math.random();}else{url=pageUrl+'&_r='+Math.random();}
wnd_frame.src=url;}}
</script>
";
            }
            else if (pageJsName == PageInfo.Js_User_Script)
            {
                retval = string.Format(@"
<script type=""text/javascript"" charset=""{0}"" src=""{1}""></script>
<script type=""text/javascript"">stlInit('{2}', '{3}', '{4}', '{5}');</script>
", SiteFiles.STL.Js_PageScript_Charset, PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.STL.Js_PageScript), PageUtility.Services.GetUrl(string.Empty).TrimEnd('/'), PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.Directory.BaiRong), publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.PublishmentSystemUrl.TrimEnd('/'));

                retval += string.Format(@"
<script type=""text/javascript"" charset=""{0}"" src=""{1}""></script>
", SiteFiles.STL.Js_UserScript_Charset, PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.STL.Js_UserScript));
            }
            else if (pageJsName == PageInfo.Js_Inner_Calendar)
            {
                retval = string.Format(@"<script type=""text/javascript"" src=""{0}""></script>", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.DatePicker.Js));
            }
            else if (pageJsName == PageInfo.Js_Static_AdFloating)
            {
                retval = string.Format(@"<script type=""text/javascript"" src=""{0}"" charset=""{1}""></script>", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.Static.Js_Static_AdFloating), SiteFiles.Static.Js_Static_AdFloating_Charset);
            }
            else if (pageJsName == PageInfo.Js_Ah_Translate)
            {
                retval = string.Format(@"<script src=""{0}"" charset=""{1}"" type=""text/javascript""></script>", PageUtility.GetSiteFilesUrl(publishmentSystemInfo, SiteFiles.tw_cn.Js), SiteFiles.tw_cn.Charset);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.A_Platform_BASIC)
            {
                string libUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/platform/components/lib");
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/platform/components/js");

                retval = string.Format(@"
<link href=""{0}/toastr/toastr.min.css"" rel=""stylesheet"" type=""text/css"" />
<script>!window.jQuery&&document.write('<script src=""{0}/jquery-1.11.0.min.js"" language=""javascript""></'+'script>');</script>
<script src=""{0}/json2.js"" language=""javascript""></script>
<script src=""{0}/toastr/toastr.min.js"" language=""javascript""></script>
<script src=""{0}/artTemplate/template.js"" language=""javascript""></script>
<script src=""{0}/layer/layer.min.js"" language=""javascript""></script>
<link href=""{0}/iCheck/flat/red.css"" rel=""stylesheet"" type=""text/css"" />
<script src=""{0}/iCheck/icheck.min.js"" language=""javascript""></script>

<script src=""{1}/services/notifyService.js"" language=""javascript""></script>
<script src=""{1}/services/utilService.js"" language=""javascript""></script>
<script src=""{1}/services/userService.js"" language=""javascript""></script>
", libUrl, jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.A_Platform_BASIC_BUT_JQUERY)
            {
                string libUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/platform/components/lib");
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/platform/components/js");

                retval = string.Format(@"
<link href=""{0}/toastr/toastr.min.css"" rel=""stylesheet"" type=""text/css"" />
<script src=""{0}/json2.js"" language=""javascript""></script>
<script src=""{0}/toastr/toastr.min.js"" language=""javascript""></script>
<script src=""{0}/artTemplate/template.js"" language=""javascript""></script>
<script src=""{0}/layer/layer.min.js"" language=""javascript""></script>
<link href=""{0}/iCheck/flat/red.css"" rel=""stylesheet"" type=""text/css"" />
<script src=""{0}/iCheck/icheck.min.js"" language=""javascript""></script>

<script src=""{1}/services/notifyService.js"" language=""javascript""></script>
<script src=""{1}/services/utilService.js"" language=""javascript""></script>
<script src=""{1}/services/userService.js"" language=""javascript""></script>
", libUrl, jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.A_Platform_USER)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/platform/components/js");

                retval = string.Format(@"
<script src=""{0}/controllers/userController.js"" language=""javascript""></script>
", jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.A_Platform_LOGIN)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/platform/components/js");

                retval = string.Format(@"
<script src=""{0}/controllers/loginController.js"" language=""javascript""></script>
", jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.A_Platform_LOGOUT)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/platform/components/js");

                retval = string.Format(@"
<script src=""{0}/controllers/logoutController.js"" language=""javascript""></script>
", jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.A_Platform_REGISTER)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/platform/components/js");

                retval = string.Format(@"
<script src=""{0}/controllers/registerController.js"" language=""javascript""></script>
", jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.A_Platform_FORGET)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/platform/components/js");

                retval = string.Format(@"
<script src=""{0}/controllers/forgetController.js"" language=""javascript""></script>
", jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.B_CMS_COMMENT)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/cms/components/js");
                string commentUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/cms/components/comment");

                retval = string.Format(@"
<script src=""{0}/services/commentService.js"" language=""javascript""></script>
<script src=""{0}/controllers/commentController.js"" language=""javascript""></script>
<link href=""{1}/style.css"" rel=""stylesheet"" type=""text/css"" />
", jsUrl, commentUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.C_B2C_BASIC)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/b2c/components/js");

                retval = string.Format(@"
<script src=""{0}/services/b2cService.js"" language=""javascript""></script>
<script src=""{0}/controllers/b2cController.js"" language=""javascript""></script>
", jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.C_B2C_FILTER)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/b2c/components/js");

                retval = string.Format(@"
<script src=""{0}/services/filterService.js"" language=""javascript""></script>
<script src=""{0}/controllers/filterController.js"" language=""javascript""></script>
", jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.C_B2C_SPEC)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/b2c/components/js");

                retval = string.Format(@"
<script src=""{0}/controllers/specController.js"" language=""javascript""></script>
", jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.C_B2C_ORDER)
            {
                string libUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/b2c/components/lib");
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/b2c/components/js");

                retval = string.Format(@"
<script src=""{0}/location/provincesdata.js"" language=""javascript""></script>
<script src=""{0}/location/jquery.provincesCity.js"" language=""javascript""></script>

<script src=""{1}/services/orderService.js"" language=""javascript""></script>
<script src=""{1}/controllers/orderController.js"" language=""javascript""></script>
", libUrl, jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.C_B2C_ORDER_CSS)
            {
                string cssUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/b2c/components/order");
                return string.Format(@"<link href=""{0}/style.css"" rel=""stylesheet"" type=""text/css"" />", cssUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.C_B2C_ORDER_SUCCESS)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/b2c/components/js");

                retval = string.Format(@"
<script src=""{0}/services/orderService.js"" language=""javascript""></script>
<script src=""{0}/controllers/orderSuccessController.js"" language=""javascript""></script>
", jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.C_B2C_ORDER_RETURN)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/b2c/components/js");

                retval = string.Format(@"
<script src=""{0}/services/orderService.js"" language=""javascript""></script>
<script src=""{0}/controllers/orderReturnController.js"" language=""javascript""></script>
", jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.C_B2C_ORDER_LIST)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/b2c/components/js");

                retval = string.Format(@"
<script src=""{0}/services/orderService.js"" language=""javascript""></script>
<script src=""{0}/controllers/orderListController.js"" language=""javascript""></script>
", jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.D_Home_CHANGEPWD)
            {
                string homeUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/cms/home");
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/cms/home/js");

                retval = string.Format(@"
<script src=""{0}/services/changePwdService.js"" language=""javascript""></script>
<script src=""{0}/controllers/changePwdController.js"" language=""javascript""></script>
<script src=""{1}/app.js"" language=""javascript""></script>
<link href=""{1}/style.css"" rel=""stylesheet"" type=""text/css"" />
", jsUrl, homeUrl);
            }

            else if (pageJsName == PageInfo.JsServiceComponents.D_Ext_GUESSES)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/b2c/components/js");

                retval = string.Format(@"
<script src=""{0}/services/extService.js"" language=""javascript""></script>
<script src=""{0}/controllers/extController.js"" language=""javascript""></script>
", jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.D_CONSULTATION)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/b2c/components/js");
                retval = string.Format(@"
<script src=""{0}/services/consultationService.js"" language=""javascript""></script>
<script src=""{0}/controllers/consultationController.js"" language=""javascript""></script>", jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.D_ORDER_COMMENT)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/b2c/components/js");
                retval = string.Format(@"
<script src=""{0}/services/orderCommentService.js"" language=""javascript""></script>
<script src=""{0}/controllers/orderCommentController.js"" language=""javascript""></script>", jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.D_PAGE_DATA_UTILS)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/b2c/components/js");
                retval = string.Format(@"
<script src=""{0}/services/pageDataUtils.js"" language=""javascript""></script>", jsUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.D_ORDER_COMMENT)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/b2c/components/js");
                retval = string.Format(@"
<script src=""{0}/services/orderCommentService.js"" language=""javascript""></script>
<script src=""{0}/controllers/orderCommentController.js"" language=""javascript""></script>", jsUrl);
            }
            #region by 20160301 sofuny 评价管理
            else if (pageJsName == PageInfo.JsServiceComponents.B_CMS_EVALUATION)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/cms/components/js");
                string evaluationUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/cms/components/evaluation");

                retval = string.Format(@"
<script src=""{0}/services/evaluationService.js"" language=""javascript""></script>
<script src=""{0}/controllers/evaluationController.js"" language=""javascript""></script>
<link href=""{1}/style.css"" rel=""stylesheet"" type=""text/css"" />
", jsUrl, evaluationUrl);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.B_CMS_TTIALAPPLY)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/cms/components/js");
                string url = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/cms/components/trial");

                retval = string.Format(@"
<script src=""{0}/services/trialApplyService.js"" language=""javascript""></script>
<script src=""{0}/controllers/trialApplyController.js"" language=""javascript""></script>
<link href=""{1}/style.css"" rel=""stylesheet"" type=""text/css"" />
", jsUrl, url);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.B_CMS_SURVEY)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/cms/components/js");
                string url = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/cms/components/survey");

                retval = string.Format(@"
<script src=""{0}/services/surveyService.js"" language=""javascript""></script>
<script src=""{0}/controllers/surveyController.js"" language=""javascript""></script>
<link href=""{1}/style.css"" rel=""stylesheet"" type=""text/css"" />
", jsUrl, url);
            }
            else if (pageJsName == PageInfo.JsServiceComponents.B_CMS_COMPARE)
            {
                string jsUrl = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/cms/components/js");
                string url = PageUtility.GetSiteFilesUrl(publishmentSystemInfo, "services/cms/components/compare");

                retval = string.Format(@"
<script src=""{0}/services/compareService.js"" language=""javascript""></script>
<script src=""{0}/controllers/compareController.js"" language=""javascript""></script>
<link href=""{1}/style.css"" rel=""stylesheet"" type=""text/css""  />
", jsUrl, url);
            }
            #endregion
            return retval;
        }

        public ICollection PageAfterBodyScriptKeys
        {
            get { return pageAfterBodyScripts.Keys; }
        }

        public ICollection PageBeforeBodyScriptKeys
        {
            get { return pageBeforeBodyScripts.Keys; }
        }

        public bool IsPageScriptsExists(string pageJsName)
        {
            return this.IsPageScriptsExists(pageJsName, true);
        }

        public bool IsPageScriptsExists(string pageJsName, bool isAfterBody)
        {
            if (isAfterBody)
            {
                return this.pageAfterBodyScripts.ContainsKey(pageJsName);
            }
            else
            {
                return this.pageBeforeBodyScripts.ContainsKey(pageJsName);
            }

        }

        public void AddPageScriptsIfNotExists(string pageJsName)
        {
            this.AddPageScriptsIfNotExists(pageJsName, true);
        }

        public void AddPageScriptsIfNotExists(string pageJsName, bool isAfterBody)
        {
            if (isAfterBody)
            {
                if (!this.pageAfterBodyScripts.ContainsKey(pageJsName))
                {
                    this.pageAfterBodyScripts.Add(pageJsName, PageInfo.GetJsCode(pageJsName, this.publishmentSystemInfo));
                }
            }
            else
            {
                if (!this.pageBeforeBodyScripts.ContainsKey(pageJsName))
                {
                    this.pageBeforeBodyScripts.Add(pageJsName, PageInfo.GetJsCode(pageJsName, this.publishmentSystemInfo));
                }
            }
        }

        public void AddPageScriptsIfNotExists(string pageJsName, string value)
        {
            this.AddPageScriptsIfNotExists(pageJsName, value, true);
        }

        public void AddPageScriptsIfNotExists(string pageJsName, string value, bool isAfterBody)
        {
            if (isAfterBody)
            {
                if (!this.pageAfterBodyScripts.ContainsKey(pageJsName))
                {
                    this.pageAfterBodyScripts.Add(pageJsName, value);
                }
            }
            else
            {
                if (!this.pageBeforeBodyScripts.ContainsKey(pageJsName))
                {
                    this.pageBeforeBodyScripts.Add(pageJsName, value);
                }
            }
        }

        public void SetPageScripts(string pageJsName, string value, bool isAfterBody)
        {
            if (isAfterBody)
            {
                this.pageAfterBodyScripts[pageJsName] = value;
            }
            else
            {
                this.pageBeforeBodyScripts[pageJsName] = value;
            }
        }

        public string GetPageScripts(string pageJsName, bool isAfterBody)
        {
            if (isAfterBody)
            {
                return this.pageAfterBodyScripts[pageJsName];
            }
            else
            {
                return this.pageBeforeBodyScripts[pageJsName];
            }
        }

        public ICollection PageEndScriptKeys
        {
            get { return pageEndScripts.Keys; }
        }

        public bool IsPageEndScriptsExists(string pageJsName)
        {
            return this.pageEndScripts.ContainsKey(pageJsName);
        }

        public void AddPageEndScriptsIfNotExists(string pageJsName)
        {
            if (!this.pageEndScripts.ContainsKey(pageJsName))
            {
                this.pageEndScripts.Add(pageJsName, PageInfo.GetJsCode(pageJsName, this.publishmentSystemInfo));
            }
        }

        public void AddPageEndScriptsIfNotExists(string pageJsName, string value)
        {
            if (!this.pageEndScripts.ContainsKey(pageJsName))
            {
                this.pageEndScripts.Add(pageJsName, value);
            }
        }

        public void SetPageEndScripts(string pageJsName, string value)
        {
            this.pageEndScripts[pageJsName] = value;
        }

        public string GetPageEndScripts(string pageJsName)
        {
            return this.pageEndScripts[pageJsName];
        }

        public ICollection PageHeadScriptKeys
        {
            get { return pageHeadScripts.Keys; }
        }

        public bool IsPageHeadScriptsExists(string pageJsName)
        {
            return this.pageHeadScripts.ContainsKey(pageJsName);
        }

        public void AddPageHeadScriptsIfNotExists(string pageJsName)
        {
            if (!this.pageHeadScripts.ContainsKey(pageJsName))
            {
                this.pageHeadScripts.Add(pageJsName, PageInfo.GetJsCode(pageJsName, this.publishmentSystemInfo));
            }
        }

        public void AddPageHeadScriptsIfNotExists(string pageJsName, string value)
        {
            if (!this.pageHeadScripts.ContainsKey(pageJsName))
            {
                this.pageHeadScripts.Add(pageJsName, value);
            }
        }

        public void SetPageHeadScripts(string pageJsName, string value)
        {
            this.pageHeadScripts[pageJsName] = value;
        }

        public string GetPageHeadScripts(string pageJsName)
        {
            return this.pageHeadScripts[pageJsName];
        }

        /// <summary>
        /// 将一个页面的js复制给本页面，提供给分页时使用
        /// add by sessionliang at 20151209
        /// </summary>
        /// <param name="lastPageInfo"></param>
        public void AddLastPageScript(PageInfo lastPageInfo)
        {
            foreach (string key in lastPageInfo.PageAfterBodyScriptKeys)
            {
                this.AddPageScriptsIfNotExists(key, lastPageInfo.GetPageScripts(key, true), true);
            }
            foreach (string key in lastPageInfo.PageBeforeBodyScriptKeys)
            {
                this.AddPageScriptsIfNotExists(key, lastPageInfo.GetPageScripts(key, false), false);
            }
            foreach (string key in lastPageInfo.PageEndScriptKeys)
            {
                this.AddPageEndScriptsIfNotExists(key, lastPageInfo.GetPageEndScripts(key));
            }
            foreach (string key in lastPageInfo.PageHeadScriptKeys)
            {
                this.AddPageHeadScriptsIfNotExists(key, lastPageInfo.GetPageHeadScripts(key));
            }
        }

        /// <summary>
        /// 将一个页面的js从本页面去除，提供给分页时使用
        ///  add by sessionliang at 20151209
        /// </summary>
        /// <param name="lastPageInfo"></param>
        public void ClearLastPageScript(PageInfo lastPageInfo)
        {
            foreach (string key in lastPageInfo.PageAfterBodyScriptKeys)
            {
                this.pageAfterBodyScripts.Remove(key);
            }
            foreach (string key in lastPageInfo.PageBeforeBodyScriptKeys)
            {
                this.pageBeforeBodyScripts.Remove(key);
            }
            foreach (string key in lastPageInfo.PageEndScriptKeys)
            {
                this.pageEndScripts.Remove(key);
            }
            foreach (string key in lastPageInfo.PageHeadScriptKeys)
            {
                this.pageHeadScripts.Remove(key);
            }
        }

        /// <summary>
        /// 清理本页面的js
        /// </summary>
        public void ClearLastPageScript()
        {

            this.pageAfterBodyScripts.Clear();

            this.pageBeforeBodyScripts.Clear();

            this.pageEndScripts.Clear();

            this.pageHeadScripts.Clear();

        }

        #endregion

        #region 缓存

        public ArrayList CacheOfInnerLinkInfoArrayList
        {
            get
            {
                ArrayList arraylist = CacheUtils.Get("PageInfo_InnerLinkInfoArrayList_" + this.publishmentSystemID) as ArrayList;
                if (arraylist == null)
                {
                    arraylist = DataProvider.InnerLinkDAO.GetInnerLinkInfoArrayList(this.publishmentSystemID);
                    ArrayList innerLinkNameArrayList = new ArrayList();
                    foreach (InnerLinkInfo innerLinkInfo in innerLinkNameArrayList)
                    {
                        innerLinkNameArrayList.Add(innerLinkInfo.InnerLinkName);
                    }
                    if (this.publishmentSystemInfo.Additional.IsInnerLinkByChannelName)
                    {
                        Hashtable nodeInfoHashtable = NodeManager.GetNodeInfoHashtableByPublishmentSystemID(this.publishmentSystemID);
                        foreach (NodeInfo nodeInfo in nodeInfoHashtable.Values)
                        {
                            if (!innerLinkNameArrayList.Contains(nodeInfo.NodeName))
                            {
                                InnerLinkInfo innerLinkInfo = new InnerLinkInfo(nodeInfo.NodeName, this.publishmentSystemID, PageUtility.GetChannelUrl(this.publishmentSystemInfo, nodeInfo, this.publishmentSystemInfo.Additional.VisualType));
                                arraylist.Add(innerLinkInfo);
                                innerLinkNameArrayList.Add(nodeInfo.NodeName);
                            }
                        }
                    }
                    foreach (InnerLinkInfo innerLinkInfo in arraylist)
                    {
                        innerLinkInfo.InnerString = string.Format(this.publishmentSystemInfo.Additional.InnerLinkFormatString, PageUtils.AddProtocolToUrl(PageUtility.ParseNavigationUrl(this.publishmentSystemInfo, innerLinkInfo.LinkUrl)), innerLinkInfo.InnerLinkName);
                    }
                    CacheUtils.Insert("PageInfo_InnerLinkInfoArrayList_" + this.publishmentSystemID, arraylist, 10);
                }
                return arraylist;
            }
        }

        #endregion

        public PublishmentSystemInfo PublishmentSystemInfo
        {
            get { return publishmentSystemInfo; }
        }

        public EVisualType VisualType
        {
            get { return visualType; }
        }

        public int UniqueID
        {
            get
            {
                return uniqueID++;
            }
        }

        public void SetUniqueID(int uniqueID)
        {
            this.uniqueID = uniqueID;
        }

        public Stack ChannelItems
        {
            get { return channelItems; }
        }

        public Stack ContentItems
        {
            get { return contentItems; }
        }

        public Stack CommentItems
        {
            get { return commentItems; }
        }

        public Stack InputItems
        {
            get { return inputItems; }
        }

        public Stack WebsiteMessageItems
        {
            get { return websiteMessageItems; }
        }

        public Stack SqlItems
        {
            get { return sqlItems; }
        }

        public Stack SiteItems
        {
            get { return siteItems; }
        }

        public Stack PhotoItems
        {
            get { return photoItems; }
        }

        public Stack TeleplayItems
        {
            get { return teleplayItems; }
        }

        public Stack EachItems
        {
            get { return eachItems; }
        }

        public Stack SpecItems
        {
            get { return specItems; }
        }

        public Stack FilterItems
        {
            get { return filterItems; }
        }
    }
}
