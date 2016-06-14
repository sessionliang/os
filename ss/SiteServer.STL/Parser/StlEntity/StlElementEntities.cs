using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;

namespace SiteServer.STL.Parser.StlEntity
{
    public class StlElementEntities
    {
        private StlElementEntities()
        {
        }

        public const string EntityName = "stl:";                  //通用实体

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                return attributes;
            }
        }

        internal static string Parse(string stlEntity, PageInfo pageInfo, ContextInfo contextInfo)
        {
            string parsedContent = string.Empty;

            contextInfo.IsCurlyBrace = true;
            try
            {
                string stlElement = StlParserUtility.HtmlToXml(string.Format("<{0} />", stlEntity.Trim(' ', '{', '}')));

                StringBuilder innerBuilder = new StringBuilder(stlElement);
                StlParserManager.ParseInnerContent(innerBuilder, pageInfo, contextInfo);
                parsedContent = innerBuilder.ToString();
            }
            catch { }
            contextInfo.IsCurlyBrace = false;

            return parsedContent;
        }
    }
}
