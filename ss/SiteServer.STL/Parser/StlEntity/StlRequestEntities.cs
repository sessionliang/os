using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using BaiRong.Model;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;

using System.Text;

namespace SiteServer.STL.Parser.StlEntity
{
	public class StlRequestEntities
	{
        private StlRequestEntities()
		{
		}

        public const string EntityName = "Request";              //«Î«Û µÃÂ

        internal static string Parse(string stlEntity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;
            try
            {
                string entityName = StlParserUtility.GetNameFromEntity(stlEntity);
                string entityValue = StlParserUtility.GetValueFromEntity(stlEntity);
                string attributeName = entityName.Substring(9, entityName.Length - 10);

                string ajaxDivID = StlParserUtility.GetAjaxDivID(pageInfo.UniqueID);
                string functionName = string.Format("stlRequest_{0}", ajaxDivID);
                parsedContent = string.Format(@"<span id=""{0}""></span>", ajaxDivID);

                StringBuilder builder = new StringBuilder();
                builder.AppendFormat(@"
<script type=""text/javascript"" language=""javascript"">
$(function(){{
    try
    {{
        var queryString = document.location.search;
        if (queryString == null || queryString.length <= 1) return;
        var reg = new RegExp(""(^|&){1}=([^&]*)(&|$)""); 
        var r = queryString.substring(1).match(reg);
        var v = decodeURI(decodeURI(r[2]));
        if (r) $(""#{2}"").html(v);", functionName, attributeName, ajaxDivID);

                if (!string.IsNullOrEmpty(entityValue))
                {
                    builder.AppendFormat(@"
         if (r) $(""#{0}"").val(v);", entityValue);
                }

                builder.AppendFormat(@"
    }}catch(e){{}}
}});
</script>
", functionName);

                pageInfo.AddPageEndScriptsIfNotExists(functionName, builder.ToString());
            }
            catch { }

            return parsedContent;
        }

        public static string ParseRequestEntities(NameValueCollection queryString, string templateContent, ECharset charset)
        {
            if (queryString.Count > 0)
            {
                foreach (string key in queryString.Keys)
                {
                    templateContent = StringUtils.ReplaceIgnoreCase(templateContent, string.Format("{{Request.{0}}}", key), PageUtils.UrlDecode(queryString[key], charset));
                }
            }
            return RegexUtils.Replace("{Request.[^}]+}", templateContent, string.Empty);
        }
	}
}
