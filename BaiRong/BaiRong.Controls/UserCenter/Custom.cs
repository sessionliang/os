using System;
using System.Web;
using System.Web.UI;

using BaiRong.Core;
using BaiRong.Model;

namespace BaiRong.Controls
{
	public class Custom : LiteralControl
	{
        public virtual string Type
        {
            get
            {
                string type = Custom.Tyle_style;
                Object state = ViewState["Type"];
                if (state != null)
                {
                    type = ((string)state).Trim().ToLower();
                }
                return type;
            }
            set
            {
                ViewState["Type"] = value;
            }
        }

        private const string Tyle_style = "style";
        private const string Type_showhint = "showhint";

		protected override void Render(HtmlTextWriter writer)
		{
            if (StringUtils.EqualsIgnoreCase(this.Type, Custom.Tyle_style))
            {
                if (!string.IsNullOrEmpty(AdminManager.Theme) && !EThemeUtils.Equals(ETheme.Default, AdminManager.Theme))
                {
                    string href = PageUtils.GetSiteFilesUrl(string.Format("bairong/jquery/bootstrap/theme/{0}/bootstrap.min.css", AdminManager.Theme));
                    writer.Write(@"<link rel=""stylesheet"" type=""text/css"" href=""{0}"">", href);
                }
            }
            else if (StringUtils.EqualsIgnoreCase(this.Type, Custom.Type_showhint))
            {
                writer.Write(@"
<script language=""JavaScript"" type=""text/JavaScript"">
function hideBoxAndShowHint(obj, message, top){
	var selects = document.getElementsByTagName(""SELECT"");
	var objects = document.getElementsByTagName(""OBJECT"");
	for(var i = 0; i < selects.length; i++) {
		selects[i].style.visibility=""hidden"";
	}
	for(var i = 0; i < objects.length; i++) {
		objects[i].style.visibility=""hidden"";
	}
	document.all.hint.style.top = top;
	document.all.hint.style.display = 'block';
	document.all.hintMessage.innerHTML = message;
	obj.enabled = false;
}
</script>
<div id=""hint"" style=""position:absolute;z-index:300;height:120px;width:320px;left:50%;top:120px;margin-left:-150px;margin-top:-80px; display:none"">
<div class=""column"" style=""position:absolute;z-index:300;width:100%;left:4px;top:5px;height:90px;font-size:14px;background-color:#FFFFFF"">
  <div class=""columntitle"">操作提示</div>
  <div class=""content"" style=""height:64px;line-height:150%;"" align=""center""><BR />
	<img border=""0"" src=""../pic/animated_loading.gif"" align=""absmiddle"" />&nbsp;<span id=""hintMessage"">操作进行中, 请稍候...</span>
	<BR /><BR />
  </div>
</div>
</div>
");
            }
		}
	}
}
