using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using BaiRong.Core;

namespace BaiRong.Controls
{
	public class AjaxTagsList : Control
	{
		public AjaxTagsList()
		{
		}

		private bool _canUserEdit = false;
		public virtual bool CanUserEdit
		{
			get{ return _canUserEdit;}
			set{_canUserEdit = value;}
		}

		private string _deleteFunction = null;
		public string AjaxDeleteTagFunction
		{
			get{ return _deleteFunction;}
			set{_deleteFunction = value;}
		}

		private string _addFunction = null;
		public string AjaxAddFunction
		{
			get{ return _addFunction;}
			set{_addFunction = value;}
		}

		private StringCollection _tags;
		public StringCollection Tags
		{
			get{ return _tags;}
			set{_tags = value;}
		}

		protected override void OnLoad(EventArgs e)
		{
			bool enableEdit = CanUserEdit && !string.IsNullOrEmpty(this.AjaxAddFunction) && !string.IsNullOrEmpty(this.AjaxDeleteTagFunction);
			if(enableEdit)
			{
				AjaxTagsListHelper.RegisterTagsScript(this);
			}

			base.OnLoad (e);
		}


		protected override void Render(HtmlTextWriter writer)
		{

			bool enableEdit = CanUserEdit && !string.IsNullOrEmpty(this.AjaxAddFunction) && !string.IsNullOrEmpty(this.AjaxDeleteTagFunction);

			if(enableEdit)
			{
				AjaxTagsListHelper.RenderEditable(writer, this);
			}
			else
			{
				AjaxTagsListHelper.RenderNotEditable(writer, this);
			}
		}
	}

	public class AjaxTagsListHelper
	{
        private static string globeIconUrl = PageUtils.GetIconUrl("globe.gif");
        private static string globeOverIconUrl = PageUtils.GetIconUrl("globeOver.gif");

		public static string GetClientCallBack(AjaxTagsList control)
		{
			return string.Format("rebuildTagsUI_{0}", control.ClientID);
		}

		public static void RegisterTagsScript(AjaxTagsList control)
		{
			string userTagUrl = string.Format("tags/{0}.aspx", string.Empty);
            //string globeTagUrl = string.Format("../../photo/tags/{0}.aspx", string.Empty);
            string globeTagUrl = userTagUrl;

			string tagsScript = string.Format(@"
				<script language=""javascript"" type=""text/javascript"">

				function rebuildTagsUI_{0}(result)
				{{
					var sb = new StringBuffer();
					var tags = result.value ? result.value : [];
					
					for (var i = 0; i < tags.length; i++)
					{{
						var tag = tags[i];
						//var globeTagUrl = '../../photo/tags/' + tag + '.aspx';
						var userTagUrl = 'tags/' + tag + '.aspx';
                        var globeTagUrl = userTagUrl;

						sb.append(""<div> <a href=\"""" + globeTagUrl + ""\"" title=\""点击这个图标查看所有标上"" + tag + ""的内容\"" class=\""globe\"" onMouseOver=\""this.childNodes[0].src='{1}';\"" onMouseOut=\""this.childNodes[0].src='{2}';\""><img src=\""{2}\"" width=\""16\"" height=\""16\"" class=\""icon\"" alt=\""点击这个图标查看所有标上"" + tag + ""的内容\"" align=\""texttop\"" /></a> <a href=\"""" + userTagUrl + ""\"" >"" + tag + ""</a> &nbsp;<a href=\""javascript:;\"" title=\""删除此标签\"" onClick=\""if (window.confirm('确认删除?')){{ var tagName='"" + tag + ""';{3};}}return false;\"">[x]</a> </div>"");
					}}
					$(""thetags_{0}"").innerHTML = sb.toString();
				}}
				</script>
				", control.ClientID, AjaxTagsListHelper.globeOverIconUrl, AjaxTagsListHelper.globeIconUrl, control.AjaxDeleteTagFunction);

			control.Page.RegisterClientScriptBlock("AjaxTagsList" + control.ClientID, tagsScript);
		}

		public static string GetAjaxDeleteTagFunction(AjaxTagsList control, string methodName, string relatedIdentity)
		{
			string retval = string.Format("{0}({1},tagName,{2});", methodName, relatedIdentity, AjaxTagsListHelper.GetClientCallBack(control));
			return retval;
		}

		public static string GetAjaxAddTagFunction(AjaxTagsList control, string methodName, string relatedIdentity)
		{
            string retval = string.Format("{0}({1},$('{2}').value,{3});", methodName, relatedIdentity, control.ClientID, AjaxTagsListHelper.GetClientCallBack(control));
			return retval;
		}

		public static void RenderEditable(HtmlTextWriter writer, AjaxTagsList control)
		{
			StringBuilder builder = new StringBuilder();

            string textDivID = control.ClientID + "_textDiv";
            string editorDivID = control.ClientID + "_editorDiv";

			builder.Append("<div style=\"font-size: 14px; font-weight: bold;\">标签");
            builder.AppendFormat("&nbsp;<span id=\"{0}\" style=\"text-align:left;\"><a href=\"javascript:;\" onclick=\"$('{0}').style.display='none';$('{1}').style.display='';$('{2}').value='';$('{2}').focus();return false;\">添加标签</a></span>", textDivID, editorDivID, control.ClientID);
            builder.Append("</div>");

			builder.AppendFormat("<div id=\"thetags_{0}\">", control.ClientID);
			if (control.Tags != null && control.Tags.Count > 0)
			{
				foreach (string tag in control.Tags)
				{
					//string globeTagUrl = string.Format("../../photo/tags/{0}.aspx", tag);
					string userTagUrl = string.Format("tags/{0}.aspx", tag);
                    string globeTagUrl = userTagUrl;

                    builder.AppendFormat("<div> <a href=\"{0}\" title=\"点击查看{1}的内容\" class=\"globe\" onMouseOver=\"this.childNodes[0].src='{2}';\" onMouseOut=\"this.childNodes[0].src='{3}';\"><img src=\"{3}\" width=\"16\" height=\"16\" class=\"icon\" alt=\"点击查看{1}的内容\" align=\"texttop\" /></a> <a href=\"{4}\" >{1}</a> &nbsp;<a href=\"javascript:;\" title=\"删除此标签\" onClick=\"if (window.confirm('确认删除吗?')){{ var tagName='{1}';{5}; }}return false;\">[x]</a> </div>", globeTagUrl, tag, AjaxTagsListHelper.globeOverIconUrl, AjaxTagsListHelper.globeIconUrl, userTagUrl, control.AjaxDeleteTagFunction);
				}
			}
			builder.Append("</div>");

            builder.AppendFormat("<div style=\"width:230px;float:left;display: none;\" id=\"{0}\">", editorDivID);
			builder.AppendFormat("<input type=\"text\" id=\"{0}\" style=\"font-size:14px; font-weight:bold; font-family:arial; padding:3px; width:95%; border:1px inset #e9e9ae; background-color:#ffffd3; margin-bottom:5px;\" /><br />", control.ClientID);
            builder.AppendFormat("<input type=\"button\" onclick=\"$('{0}').style.display='';$('{1}').style.display='none';{2}\" class=\"button\" value=\"添 加\" />&nbsp;&nbsp;<input type=\"button\" onclick=\"$('{0}').style.display='';$('{1}').style.display='none';\" class=\"button\" value=\"取 消\" />", textDivID, editorDivID, control.AjaxAddFunction);

			builder.Append("<br><br>多个标签以空格分开：杭州 西湖 苏堤。如果你的标签中包含空格，比如由多个英文单词组成，请用双引号括起来，如<i>\"liveserver help\"</i>。 </div><div style=\"clear:both\"></div>");

			writer.Write(builder.ToString());
		}


		public static void RenderNotEditable(HtmlTextWriter writer, AjaxTagsList control)
		{
			StringBuilder builder = new StringBuilder();

			if (control.Tags != null && control.Tags.Count > 0)
			{
                builder.Append("<div style=\"font-size: 14px; font-weight: bold; margin-bottom: 10px;\">标签</div>");
                builder.AppendFormat("<div id=\"thetags_{0}\">", control.ClientID);
				foreach (string tag in control.Tags)
				{
					//string globeTagUrl = string.Format("../../photo/tags/{0}.aspx", tag);
					string userTagUrl = string.Format("tags/{0}.aspx", tag);
                    string globeTagUrl = userTagUrl;

					builder.AppendFormat("<div> <a href=\"{0}\" title=\"点击这个图标查看所有标上{1}的内容\" class=\"globe\" onMouseOver=\"this.childNodes[0].src='{2}';\" onMouseOut=\"this.childNodes[0].src='{3}';\"><img src=\"{3}\" width=\"16\" height=\"16\" class=\"icon\" alt=\"点击这个图标查看所有标上{1}的内容\" align=\"texttop\" /></a> <a href=\"{4}\" >{1}</a></div>", globeTagUrl, tag, AjaxTagsListHelper.globeOverIconUrl, AjaxTagsListHelper.globeIconUrl, userTagUrl);
				}
                builder.Append("</div>");
			}

			writer.Write(builder.ToString());
		}
	}
}
