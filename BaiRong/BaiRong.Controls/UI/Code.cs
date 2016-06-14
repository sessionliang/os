using System;
using System.Web;
using System.Web.UI;

using BaiRong.Core;

namespace BaiRong.Controls
{
	public class Code : LiteralControl 
	{
		public virtual string Type 
		{
			get 
			{
				string type = ViewState["Type"] as string;
				if (!string.IsNullOrEmpty(type))
				{
					return type;
				}
				else
				{
					return string.Empty;
				}
			}
			set 
			{
				ViewState["Type"] = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string code = string.Empty;
			if (!string.IsNullOrEmpty(Type))
			{
                //if (this.Type.Trim().ToLower() == "prototype")
                //{
                //    code = string.Format(@"<script language=""javascript"" src=""{0}""></script>", PageUtils.GetSiteFilesUrl(SiteFiles.Prototype.Js));
                //}
                if (this.Type.Trim().ToLower() == "jquery")
                {
                    code = string.Format(@"<script language=""javascript"" src=""{0}""></script>", PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.Js));
                }
                else if (this.Type.Trim().ToLower() == "ajaxupload")
                {
                    code = string.Format(@"<script language=""javascript"" src=""{0}""></script>", PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.AjaxUpload.Js));
                }
                else if (this.Type.Trim().ToLower() == "fancybox")
                {
                    code = string.Format(@"<script type=""text/javascript"" src=""{0}""></script><link rel=""stylesheet"" type=""text/css"" href=""{1}"" media=""screen"" />", PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.FancyBox.Js), PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.FancyBox.Css));
                }
                //else if (this.Type.Trim().ToLower() == "scriptaculous")
                //{
                //    code = string.Format(@"<script language=""javascript"" src=""{0}""></script>", PageUtils.GetSiteFilesUrl(SiteFiles.Scriptaculous.Js));
                //}
                else if (this.Type.Trim().ToLower() == "globalscript")
                {
                    code = string.Format(@"<script language=""javascript"" charset=""{0}"" src=""{1}""></script>", SiteFiles.GlobalScript.Charset, PageUtils.GetSiteFilesUrl(SiteFiles.GlobalScript.Js));
                }
                else if (this.Type.Trim().ToLower() == "lightbox")
                {
                    code = string.Format(@"
<script language=""javascript"" src=""{0}""></script>
<link media=screen href=""{1}"" type=text/css rel=stylesheet />
<script language=""javascript"">
var BaiRong.ScriptsUrl = 
</script>
", PageUtils.GetSiteFilesUrl(SiteFiles.Lightbox.Js), PageUtils.GetSiteFilesUrl(SiteFiles.Lightbox.Css));
                }
				else if (this.Type.Trim().ToLower() == "tabcontrol")
				{
                    code = string.Format(@"<link media=screen href=""{0}"" type=text/css rel=stylesheet />", PageUtils.GetSiteFilesUrl(SiteFiles.Tabstrip.Css));
				}
				else if (this.Type.Trim().ToLower() == "style")
				{
                    code = string.Format(@"<link rel=""stylesheet"" type=""text/css"" href=""{0}"">", PageUtils.GetSiteFilesUrl(SiteFiles.Global.Css));
                }
                else if (this.Type.Trim().ToLower() == "bootstrap")
                {
                    code = string.Format(@"
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
", PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.Bootstrap.Css), PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.Bootstrap.Js), PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.Bootstrap.Directory_IE));
                }
                else if (this.Type.Trim().ToLower() == "html5shiv")
                {
                    code = string.Format(@"<!--[if lt IE 9]><script src=""{0}""></script><![endif]-->", PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.Html5shiv.Js));
                }
                else if (this.Type.Trim().ToLower() == "calendar")
                {
                    code = string.Format(@"<script language=""javascript"" src=""{0}""></script>", PageUtils.GetSiteFilesUrl(SiteFiles.DatePicker.Js));
                }
                else if (this.Type.Trim().ToLower() == "angularjs")
                {
                    code = string.Format(@"<script language=""javascript"" src=""{0}""></script>", PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.AngularJS.Js));
                }
                else if (this.Type.Trim().ToLower() == "highcharts")
                {
                    code = string.Format(@"<script language=""javascript"" src=""{0}""></script><script language=""javascript"" src=""{1}""></script>", PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.Highcharts.HighchartsJs), PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.Highcharts.ExportingJs));
                }
                else if (this.Type.Trim().ToLower() == "toastr")
                {
                    code = string.Format(@"<link rel=""stylesheet"" type=""text/css"" href=""{0}""><script language=""javascript"" src=""{1}""></script>", PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.Toastr.Css), PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.Toastr.Js));
                }
                else if (this.Type.Trim().ToLower() == "arttemplate")
                {
                    code = string.Format(@"<script language=""javascript"" src=""{0}""></script><script language=""javascript"" src=""{1}""></script><script language=""javascript"" src=""{2}""></script><script language=""javascript"" src=""{3}""></script>", PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.ArtTemplate.JsJson2), PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.ArtTemplate.JsTemplate), PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.ArtTemplate.JsTemplateSimple), PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.ArtTemplate.JsUtilService));
                }
                else if (this.Type.Trim().ToLower() == "layer")
                {
                    code = string.Format(@"<script language=""javascript"" src=""{0}""></script>", PageUtils.GetSiteFilesUrl(SiteFiles.JQuery.Layer.Js));
                }
			}

			writer.Write(code);
		}


	}
}
