using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Xml;
using BaiRong.Core;
using SiteServer.STL.Parser.Model;
using SiteServer.CMS.Model;
using System;

namespace SiteServer.STL.Parser.StlElement
{
    public class StlQueryString
	{
        public const string ElementName = "stl:querystring";

        public static ListDictionary AttributeList
        {
            get
            {
                ListDictionary attributes = new ListDictionary();
                return attributes;
            }
        }
	}
}
